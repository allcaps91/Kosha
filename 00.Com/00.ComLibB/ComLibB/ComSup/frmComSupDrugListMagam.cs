using ComBase; //기본 클래스
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupDrugListMagam.cs
    /// Description     : 비상마약대장 마감
    /// Author          : 이정현
    /// Create Date     : 2017-07-12
    /// Update History  : 약제팀, 중환자실, 호스피스병동, 혈관조영실, 내시경실, 응급의료센터, 주사실, 종검 통합
    /// <history> 
    /// 각 부서별 비상마약대장 마감 통합
    /// </history>
    /// <seealso>
    /// PSMH\drug\drmayak\FrmDrugListMagam.frm
    /// PSMH\drug\drmayak\FrmDrugListMagam4H.frm
    /// PSMH\drug\drmayak\FrmDrugListMagamAG.frm
    /// PSMH\drug\drmayak\FrmDrugListMagamENDO.frm
    /// PSMH\drug\drmayak\FrmDrugListMagamER.frm
    /// PSMH\drug\drmayak\FrmDrugListMagamIU.frm
    /// PSMH\drug\drmayak\FrmDrugListMagamJusa.frm
    /// PSMH\drug\drmayak\FrmDrugListMagamTO.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drmayak\ojumst.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupDrugListMagam : Form
    {
        //마감 원래대로 돌리기 
        //액팅취소 -> ocs_drug(마감정보) 삭제
        //부서코드
        //DELETE ADMIN.OCS_DRUG
        //WHERE WARDCODE = ''
        //조건1(등록번호)
        //WHERE PTNO = '81000004'
        //조건2(마감일자)
        //where BUILDDATE = to_date('2017-08-10', 'YYYY-MM-DD')

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        private string GstrWardCode = "";

        public frmComSupDrugListMagam()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 부서/병동 코드로 생성자 입력
        /// 32/33/35(중환자실), 4H(호스피스), AG(혈관조영실), AN(약제팀), EN(내시경실), ER(응급의료센터), JS(주사실), TO(종합검진센터)
        /// </summary>
        /// <param name="strWardCode"></param>
        public frmComSupDrugListMagam(string strWardCode)
        {
            InitializeComponent();
            
            GstrWardCode = strWardCode;
        }

        private void frmComSupDrugListMagam_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (GstrWardCode.Trim() != "")
            {
                switch(GstrWardCode)
                {
                    case "32":  //중환자실(ICU) -- 과거
                    case "33":  //응급 중환자실(EICU)
                    case "35":  //중환자실(GICU)
                    case "4H":  //호스피스병동
                    case "AG":  //혈관조영실
                    case "AN":  //약제팀
                    case "EN":  //내시경실
                    case "ER":  //응급의료센터
                    case "JS":  //주사실
                    case "TO":  //종합검진
                        break;
                    default:
                        ComFunc.MsgBox("지정된 부서코드가 없습니다.");
                        rEventClosed();
                        break;
                }
            }
            else
            {
                ComFunc.MsgBox("지정된 부서코드가 없습니다.");
                rEventClosed();
            }

            SetForm();
        }

        private void SetForm()
        {
            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            
            switch(GstrWardCode)
            {
                case "AN":  //약제팀
                    lblMayakMagam.Text = "비상마약대장 마감_마취과";
                    break;
                case "32":  //중환자실(ICU) -- 과거
                case "33":  //응급 중환자실(EICU)
                case "35":  //중환자실(GICU)
                    lblMayakMagam.Text = "비상마약대장 마감_중환자실";
                    break;
                case "4H":  //호스피스병동
                    lblMayakMagam.Text = "비상마약대장 마감_호스피스";
                    break;
                case "AG":  //혈관조영실
                    lblMayakMagam.Text = "비상마약대장 마감_혈관조영실";
                    break;
                case "EN":  //내시경실
                    lblMayakMagam.Text = "비상마약대장 마감_내시경실";
                    break;
                case "ER":  //응급의료센터
                    lblMayakMagam.Text = "비상마약대장 마감_응급의료센터";
                    break;
                case "JS":  //주사실
                    lblMayakMagam.Text = "비상마약대장 마감_주사실";
                    break;
                case "TO":  //종합검진
                    lblMayakMagam.Text = "비상마약대장 마감_종합건진";
                    break;
            }

            switch (GstrWardCode)
            {
                case "AN":  //약제팀
                    lblMayakMagam.Text = "비상마약대장 마감_마취과";
                    grpBox.Text = "시간기준(자동인식)";
                    lbl0.Text = "▶ 16시 마감 (평일)";
                    lbl1.Text = "▶ 12시 마감 (토요일)";
                    lbl2.Text = "▶ 12시 마감 (일요일)";
                    lbl3.Text = "▶ 12시 마감 (공휴일)";
                    lbl4.Text = "▶ 진료과(PC) 처방 집계 포함";

                    grpBox.ForeColor = Color.Blue;
                    lbl0.ForeColor = Color.Black;
                    lbl1.ForeColor = Color.Black;
                    lbl2.ForeColor = Color.Black;
                    lbl3.ForeColor = Color.Black;
                    lbl4.ForeColor = Color.Black;

                    btnBuild.Visible = true;
                    btnBuild.Location = new Point(314, 3);
                    btnBuild.Text = "마  감";

                    btnBuild2.Visible = false;
                    break;
                case "4H":  //호스피스병동(4H)
                case "ER":  //응급의료센터(ER)
                case "32":  //중환자실(ICU) -- 과거
                case "33":  //응급 중환자실(EICU)
                case "35":  //중환자실(GICU)
                    grpBox.Text = "시간기준(자동인식)";
                    lbl0.Text = "▶ 매번 마감시점에 차수별로 마감됩니다.";
                    lbl1.Text = "▶ 간호사 ACTING 기준으로 마감됩니다.";
                    lbl2.Text = "▶ 비치수량을 초과하는 처방일 경우"
                        + ComNum.VBLF + VB.Space(4) + "관리대장에서 제외됩니다.";
                    lbl3.Text = "";
                    lbl4.Text = "";

                    grpBox.ForeColor = Color.Blue;
                    lbl0.ForeColor = Color.Black;
                    lbl1.ForeColor = Color.Red;
                    lbl2.ForeColor = Color.Red;
                    lbl3.ForeColor = Color.Black;
                    lbl4.ForeColor = Color.Black;

                    btnBuild.Visible = true;
                    btnBuild.Location = new Point(314, 3);
                    btnBuild.Text = "마  감";

                    btnBuild2.Visible = false;
                    break;
                case "AG":  //혈관조영실
                case "JS":  //주사실
                    grpBox.Text = "시간기준(자동인식)";
                    lbl0.Text = "▶ 매번 마감시점에 차수별로 마감됩니다.";
                    lbl1.Text = "▶ 비치수량을 초과하는 처방일 경우"
                        + ComNum.VBLF + VB.Space(4) + "관리대장에서 제외됩니다.";
                    lbl2.Text = "";
                    lbl3.Text = "";
                    lbl4.Text = "";

                    grpBox.ForeColor = Color.Blue;
                    lbl0.ForeColor = Color.Black;
                    lbl1.ForeColor = Color.Red;
                    lbl2.ForeColor = Color.Black;
                    lbl3.ForeColor = Color.Black;
                    lbl4.ForeColor = Color.Black;

                    btnBuild.Visible = true;
                    btnBuild.Location = new Point(314, 3);
                    btnBuild.Text = "마  감";

                    btnBuild2.Visible = false;
                    break;
                case "EN":  //내시경실
                    grpBox.Text = "마감기준";
                    lbl0.Text = "▶ 내시경실에서 실사용 기준";
                    lbl1.Text = "";
                    lbl2.Text = "";
                    lbl3.Text = "";
                    lbl4.Text = "";

                    grpBox.ForeColor = Color.Blue;
                    lbl0.ForeColor = Color.Black;
                    lbl1.ForeColor = Color.Black;
                    lbl2.ForeColor = Color.Black;
                    lbl3.ForeColor = Color.Black;
                    lbl4.ForeColor = Color.Black;

                    btnBuild.Visible = true;
                    btnBuild.Location = new Point(314, 3);
                    btnBuild.Text = "마  감";

                    btnBuild2.Visible = false;
                    break;
                case "TO":  //종합검진
                    grpBox.Text = "마감기준";
                    lbl0.Text = "▶ 종검향정관리에서 차수설정 기준";
                    lbl1.Text = "";
                    lbl2.Text = "";
                    lbl3.Text = "";
                    lbl4.Text = "";

                    grpBox.ForeColor = Color.Blue;
                    lbl0.ForeColor = Color.Black;
                    lbl1.ForeColor = Color.Black;
                    lbl2.ForeColor = Color.Black;
                    lbl3.ForeColor = Color.Black;
                    lbl4.ForeColor = Color.Black;

                    btnBuild.Visible = true;
                    btnBuild.Location = new Point(239, 3);
                    btnBuild.Text = "1차마감";

                    btnBuild2.Visible = true;
                    btnBuild2.Location = new Point(314, 3);
                    btnBuild2.Text = "2차마감";
                    break;
            }
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            this.Enabled = false;

            if (SaveData() == true)
            {
                ComFunc.MsgBox("비상마약류 자료형성 완료");
                rEventClosed();
            }

            this.Enabled = true;
        }

        private bool SaveData(string strChasu = "1")
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            int i = 0;
            int k = 0;

            string SqlErr = "";
            bool rtnVal = false;
            int intRowAffected = 0;

            string strDate = "";
            string strDateB = "";

            string strGBn = "";

            string strFTime = "";
            string strTTime = "";

            string strYoil = "";
            
            bool bolHuil = false;   //오늘
            bool bolHuilY = false;  //어제

            double dblQty = 0;
            double dblJQty = 0;
            double dblUnit = 0;

            string strSuCode = "";

            strDate = dtpDate.Value.ToString("yyyy-MM-dd");
            strDateB = dtpDate.Value.AddDays(-1).ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            
            
            try
            {
                switch (GstrWardCode)
                {
                    case "AN":  //약제팀
                        if (dtpDate.Value.ToString("yyyy-MM-dd") == "2013-12-17")
                        {
                            ComFunc.MsgBox("전산실로 연락바람");
                            return rtnVal;
                        }

                        bolHuil = clsVbfunc.ChkDateHuIl(clsDB.DbCon, strDate);
                        bolHuilY = clsVbfunc.ChkDateHuIl(clsDB.DbCon, strDateB);
                        strYoil = clsVbfunc.GetYoIl(strDate);

                        if (strDateB == "2014-09-10") { bolHuil = false; }  //당일
                        if (strDateB == "2014-09-11") { bolHuilY = false; }  //어제

                        //2015-08-12
                        if (strDateB == "2015-08-14") { bolHuil = false; }  //당일
                        if (strDateB == "2015-08-15") { bolHuilY = false; }  //어제

                        switch (strYoil)
                        {
                            case "토요일":
                                //if (bolHuilY == true) { strFTime = "12:00:01"; } else { strFTime = "16:00:01"; }
                                //2020-08-24 변경 16:00 -> 15:00
                                if (bolHuilY == true) { strFTime = "12:00:01"; } else { strFTime = "15:00:01"; }
                                strTTime = "12:00:00";  //평일 12시
                                break;
                            case "일요일":
                                strFTime = "12:00:01";  //평일 12시
                                strTTime = "12:00:00";  //평일 12시
                                break;
                            case "월요일":
                                strFTime = "12:00:01";  //평일 12시
                                //if (bolHuil == true) { strTTime = "12:00:00"; } else { strTTime = "16:00:00"; }//평일 12시
                                //2020-08-24 변경 16:00 -> 15:00
                                if (bolHuil == true) { strTTime = "12:00:00"; } else { strTTime = "15:00:00"; }//평일 12시
                                break;
                            default:
                                //if (bolHuilY == true) { strFTime = "12:00:01"; } else { strFTime = "16:00:01"; }
                                //if (bolHuil == true) { strTTime = "12:00:00"; } else { strTTime = "16:00:00"; }
                                //2020-08-24 변경 16:00 -> 15:00
                                if (bolHuilY == true) { strFTime = "12:00:01"; } else { strFTime = "15:00:01"; }
                                if (bolHuil == true) { strTTime = "12:00:00"; } else { strTTime = "15:00:00"; }
                                break;
                        }

                        if (ComFunc.MsgBoxQ(strDate + "  일 " + strTTime + " 비상마약류 관리 대장 자료 형성 하시겠습니까?"
                            , "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No) { return rtnVal; }

                        if (dtpDate.Value <= Convert.ToDateTime("2012-09-30")) { ComFunc.MsgBox("2012년 10월 1일 부터 자료형성 가능합니다."); }


                        string strLogChasu = "";
                        ComFunc.ReadSysDate(clsDB.DbCon);
                        if (bolHuil == false && (strYoil != "토요일" || strYoil != "일요일"))
                        {
                            if (strYoil == "월요일")
                            {
                                //2018-12-10(1차수)          
                                //기준시간 09시 00분 (+-)1시간
                                if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(strDate + " 08:00:01") &&
                                    Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) <= Convert.ToDateTime(strDate + " 10:00:00"))
                                {
                                    strLogChasu = "1";
                                }

                                //2018-12-10(2차수)        
                                //기준시간 11시 30분 (+-)1시간
                                if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(strDate + " 10:30:01") &&
                                    Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) <= Convert.ToDateTime(strDate + " 12:30:00"))
                                {
                                    strLogChasu = "2";
                                }

                                ////2018-12-10(3차수)        
                                ////기준시간 16시 00분 (+-)1시간
                                //if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(strDate + " 15:00:01") &&
                                //    Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) <= Convert.ToDateTime(strDate + " 17:00:00"))
                                //{
                                //    strLogChasu = "3";
                                //}

                                //2020-08-24(3차수)        
                                //기준시간 15시 00분 (+-)1시간
                                if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(strDate + " 14:00:01") &&
                                    Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) <= Convert.ToDateTime(strDate + " 16:00:00"))
                                {
                                    strLogChasu = "3";
                                }
                            }
                            else
                            {
                                if (strYoil == "토요일")
                                {
                                    //2018-12-10(1차수)   
                                    //기준시간 12시 00분 (+-)1시간
                                    if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(strDate + " 11:00:01") &&
                                    Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) <= Convert.ToDateTime(strDate + " 13:00:00"))
                                    {
                                        strLogChasu = "1";
                                    }
                                }
                                else
                                {
                                    //2018-12-10(1차수)          
                                    //기준시간 09시 00분 (+-)1시간
                                    if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(strDate + " 08:00:01") &&
                                        Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) <= Convert.ToDateTime(strDate + " 10:00:00"))
                                    {
                                        strLogChasu = "1";
                                    }

                                    //2018-12-10(2차수)        
                                    //기준시간 11시 30분 (+-)1시간
                                    if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(strDate + " 10:30:01") &&
                                        Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) <= Convert.ToDateTime(strDate + " 12:30:00"))
                                    {
                                        strLogChasu = "2";
                                    }

                                    ////2018-12-10(3차수)        
                                    ////기준시간 16시 00분 (+-)1시간
                                    //if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(strDate + " 15:00:01") &&
                                    //    Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) <= Convert.ToDateTime(strDate + " 17:00:00"))
                                    //{
                                    //    strLogChasu = "3";
                                    //}

                                    //2020-08-24(3차수)        
                                    //기준시간 15시 00분 (+-)1시간
                                    if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(strDate + " 14:00:01") &&
                                        Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) <= Convert.ToDateTime(strDate + " 16:00:00"))
                                    {
                                        strLogChasu = "3";
                                    }
                                }
                            }
                        }
                        else if (strYoil == "일요일")
                        {
                            //2018-12-10(1차수)   
                            //기준시간 12시 00분 (+-)1시간
                            if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(strDate + " 11:00:01") &&
                            Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) <= Convert.ToDateTime(strDate + " 13:00:00"))
                            {
                                strLogChasu = "1";
                            }
                        }
                        //2020-01-01 휴일인데 마감안되서 로직 추가.
                        else if (bolHuil == true)
                        {
                            //2018-12-10(1차수)          
                            //기준시간 09시 00분 (+-)1시간
                            //if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(strDate + " 08:00:01") &&
                            //    Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) <= Convert.ToDateTime(strDate + " 10:00:00"))
                            //{
                            //    strLogChasu = "1";
                            //}

                            #region 로그 없어서(토, 일 이랑 동일하게 벼ㅑㄴ경)
                            if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(strDate + " 11:00:01") &&
                              Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) <= Convert.ToDateTime(strDate + " 13:00:00"))
                            {
                                strLogChasu = "1";
                            }
                            #endregion
                        }

                        if (strLogChasu != "")
                        {
                            // 마감로그 발생
                            if (SaveLog(strDate, GstrWardCode, strLogChasu) == false)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }

                        break;
                    case "4H":  //호스피스
                    case "AG":  //혈관조영실
                    case "ER":  //응급의료센터
                    case "32":  //중환자실(ICU) -- 과거
                    case "33":  //응급 중환자실(EICU)
                    case "35":  //중환자실(GICU)
                    case "JS":  //주사실
                        switch(GstrWardCode)
                        {
                            case "4H":
                            case "32":
                            case "33":
                            case "35":
                                if (GstrWardCode.Trim() == "") { ComFunc.MsgBox("해당 병동 선택오류"); return rtnVal; }
                                break;
                        }

                        SQL = "";
                        SQL = "SELECT MAX(NO1) AS MNO FROM " + ComNum.DB_MED + "OCS_DRUG ";
                        SQL = SQL + ComNum.VBLF + "   WHERE BUILDDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE = '" + GstrWardCode + "'  "; //ICU
                        SQL = SQL + ComNum.VBLF + "     AND GBN2 IN ('2','3') ";    //소모

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strChasu = (VB.Val(dt.Rows[0]["MNO"].ToString().Trim()) + 1).ToString();
                        }

                        dt.Dispose();
                        dt = null;

                        if (ComFunc.MsgBoxQ(strDate + "일 " + strChasu + " 차수 비상마약류 관리대장 자료 형성을 하겠습니까?"
                            , "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No) { return rtnVal; }

                        //마약 대장 차수 로직 등록
                        if (GstrWardCode != "JS")
                        {
                            if (strDate == "2013-12-17") { ComFunc.MsgBox("전산실로 연락 바람"); return rtnVal; }
                        }

                        switch(GstrWardCode)
                        {
                            case "4H":
                            case "ER":
                            case "32":
                            case "33":
                            case "35":
                                if (Convert.ToDateTime(strDate) < Convert.ToDateTime("2014-02-18")) { ComFunc.MsgBox("2014년 02월 18일 부터 자료형성 가능합니다."); return rtnVal; }
                                break;
                            case "AG":
                                if (Convert.ToDateTime(strDate) < Convert.ToDateTime("2014-10-02")) { ComFunc.MsgBox("2014년 10월 02일 부터 자료형성 가능합니다."); return rtnVal; }
                                break;
                            case "JS":
                                if (Convert.ToDateTime(strDate) < Convert.ToDateTime("2014-06-01")) { ComFunc.MsgBox("2014년 06월 01일 부터 자료형성 가능합니다."); return rtnVal; }
                                break;
                        }

                        switch(GstrWardCode)
                        {
                            case "4H":
                            case "ER":
                            case "32":
                            case "33":
                            case "35":
                                //응급실 간호사 actting 시간 set
                                if (READ_OCS_ACTING(strDate) == false)
                                {
                                    return rtnVal;
                                }   
                                break;
                        }
                        break;
                    case "EN":  //내시경실
                        if (ComFunc.MsgBoxQ(strDate + "일 " + strChasu + " 차수 비상마약류 관리대장 자료 형성을 하겠습니까?"
                            , "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No) { return rtnVal; }

                        if (Convert.ToDateTime(strDate) < Convert.ToDateTime("2015-01-05")) { ComFunc.MsgBox("2015년 01월 05일 부터 자료형성 가능합니다."); return rtnVal; }

                        if (Endo_Hyang_cnt(SQL, intRowAffected, SqlErr) == false)
                        {
                            return rtnVal;
                        }
                        break;
                    case "TO":
                        if (ComFunc.MsgBoxQ(strDate + "일 비상마약류 관리대장 자료 형성을 하겠습니까?"
                            , "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No) { return rtnVal; }

                        if (Convert.ToDateTime(strDate) < Convert.ToDateTime("2014-11-17")) { ComFunc.MsgBox("2014년 11월 17일 부터 자료형성 가능합니다."); return rtnVal; }

                        //기존자료 삭제 후 자료 형성
                        SQL = "";
                        SQL = "DELETE " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "   WHERE BUILDDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND WARDCODE = '" + GstrWardCode + "' ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (TOMAGAN_RTN(strChasu) == true)
                        {
                            clsDB.setCommitTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;

                            rtnVal = true;
                            return rtnVal;
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                }

                //소모내역 집계 ----------------------------------------------------
                //마감은 항상 전일 부터 당일까지 마감 설정
                //마감 시간을 설정함  '4시 30분 고정

                //기존자료 삭제
                SQL = "";
                SQL = "DELETE " + ComNum.DB_MED + "OCS_DRUG ";
                SQL = SQL + ComNum.VBLF + "   WHERE BUILDDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND WARDCODE = '" + GstrWardCode + "' ";

                switch (GstrWardCode)
                {
                    case "AN":  //약제팀
                        SQL = SQL + ComNum.VBLF + "     AND GBN2 IN ('2','3') ";
                        break;
                    case "4H":  //호스피스
                    case "AG":  //혈관조영실
                    //case "EN":
                    case "32":
                    case "33":
                    case "35":
                    case "JS":
                    case "ER":
                        SQL = SQL + ComNum.VBLF + "     AND GBN2 IN ('3') ";
                        break;
                }

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                switch (GstrWardCode)
                {
                    case "AN":  //약제팀
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_MED + "OCS_MAYAK";
                        SQL = SQL + ComNum.VBLF + "     SET";
                        SQL = SQL + ComNum.VBLF + "         MDATE = ENTDATE ";
                        SQL = SQL + ComNum.VBLF + "WHERE BDATE_R >=TO_DATE('" + strDateB + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " AND BDATE_R <=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " AND ( WARDCODE ='" + GstrWardCode + "' OR DEPTCODE ='PC' OR WARDCODE ='OP' ) ";
                        SQL = SQL + ComNum.VBLF + " AND ORDERNO >0 ";
                        SQL = SQL + ComNum.VBLF + " AND MDATE IS NULL";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_MED + "OCS_HYANG";
                        SQL = SQL + ComNum.VBLF + "     SET";
                        SQL = SQL + ComNum.VBLF + "         MDATE = ENTDATE ";
                        SQL = SQL + ComNum.VBLF + "WHERE BDATE >=TO_DATE('" + strDateB + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " AND BDATE <=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " AND ( WARDCODE ='" + GstrWardCode + "' OR DEPTCODE ='PC' OR WARDCODE ='OP') ";
                        SQL = SQL + ComNum.VBLF + " AND ORDERNO >0 ";
                        SQL = SQL + ComNum.VBLF + " AND MDATE IS NULL";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        //자료 형성
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (BDATE ,GBN, GBN2, PTNO, SNAME,DEPTCODE, DRSABUN, IO, WARDCODE, ROOMCODE, SUCODE, QTY, REALQTY, NAL, DOSCODE, ORDERNO, ENTDATE, BUILDDATE, MDATE, SUCODER) ";
                        SQL = SQL + ComNum.VBLF + " SELECT";
                        SQL = SQL + ComNum.VBLF + "     BDATE_R, '1', '2', PTNO, SNAME, DEPTCODE, DRSABUN, IO, '" + GstrWardCode + "', ROOMCODE, DECODE(RTRIM(SUCODE),'N-FE-PC','N-FE2',SUCODE), TRUNC(QTY+ 0.9), REALQTY, NAL, DOSCODE, ORDERNO, ENTDATE, TO_DATE('" + strDate + "','YYYY-MM-DD'), MDATE, SUCODE ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_MAYAK A ";
                        SQL = SQL + ComNum.VBLF + "     WHERE BDATE_R >=TO_DATE('" + strDateB + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND BDATE_R <=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND ( WARDCODE ='" + GstrWardCode + "' OR DEPTCODE ='PC' OR WARDCODE ='OP' ) ";
                        SQL = SQL + ComNum.VBLF + "     AND ORDERNO >0 ";
                        SQL = SQL + ComNum.VBLF + "     AND MDATE >= TO_DATE('" + strDateB + " " + strFTime + "','YYYY-MM-DD HH24:MI:SS') ";
                        SQL = SQL + ComNum.VBLF + "     AND MDATE <= TO_DATE('" + strDate + " " + strTTime + "','YYYY-MM-DD HH24:MI:SS') ";
                        SQL = SQL + ComNum.VBLF + "UNION ALL ";
                        SQL = SQL + ComNum.VBLF + " SELECT";
                        SQL = SQL + ComNum.VBLF + "     BDATE, '2', '2', PTNO, SNAME, DEPTCODE, DRSABUN, IO, '" + GstrWardCode + "', ROOMCODE, SUCODE, TRUNC(QTY + 0.9), REALQTY, NAL, DOSCODE, ORDERNO, ENTDATE, TO_DATE('" + strDate + "','YYYY-MM-DD'), MDATE, SUCODE ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_HYANG A ";
                        SQL = SQL + ComNum.VBLF + "     WHERE BDATE >=TO_DATE('" + Convert.ToDateTime(strDateB).AddDays(-3).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND BDATE <=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND ( WARDCODE ='" + GstrWardCode + "' OR DEPTCODE ='PC' OR WARDCODE ='OP' ) ";
                        SQL = SQL + ComNum.VBLF + "     AND ORDERNO >0 ";
                        SQL = SQL + ComNum.VBLF + "     AND MDATE >= TO_DATE('" + strDateB + " " + strFTime + "','YYYY-MM-DD HH24:MI:SS') ";
                        SQL = SQL + ComNum.VBLF + "     AND MDATE <= TO_DATE('" + strDate + " " + strTTime + "','YYYY-MM-DD HH24:MI:SS') ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY MDATE ASC  ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        //2019-07-08
                        //마약 사용을 안했을 경우 DRUG 테이블에 맞는 차수에 쓰레기 데이터 추가 (마감여부만 확인하기 위함)
                        if (intRowAffected == 0)
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                            SQL = SQL + ComNum.VBLF + "     (BDATE ,GBN, GBN2, NO1, WARDCODE, MDATE, BUILDDATE) ";
                            SQL = SQL + ComNum.VBLF + "  VALUES(TO_DATE('" + strDate + "','YYYY-MM-DD'), '0', '0', '" + strChasu + "', '" + GstrWardCode + "', TO_DATE('" + strDate + " " + strTTime + "','YYYY-MM-DD HH24:MI:SS'), TO_DATE('" + strDate + "' ,'YYYY-MM-DD'))";                            

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        }

                        if (bolHuil == false && (strYoil != "토요일" || strYoil != "일요일"))
                        {

                            if (strYoil == "월요일")
                            {
                                //2018-12-10(1차수)
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUG";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         NO1 = 1 ";
                                SQL = SQL + ComNum.VBLF + " WHERE MDATE >= TO_DATE('" + strDateB + " " + (bolHuilY ? "12:00:00" : "15:00:00") + "','YYYY-MM-DD HH24:MI:SS') ";
                                SQL = SQL + ComNum.VBLF + "   AND MDATE <=TO_DATE('" + strDate + " 09:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                //SQL = SQL + ComNum.VBLF + "   AND MDATE <= TO_DATE('" + strDate + " " + strTTime + "','YYYY-MM-DD HH24:MI:SS') ";
                                //SQL = SQL + ComNum.VBLF + "WHERE MDATE >=TO_DATE('" + strDateB + " 12:00:01','YYYY-MM-DD HH24:MI:SS') ";
                                SQL = SQL + ComNum.VBLF + " AND BUILDDATE = TO_DATE('" + strDate + "' ,'YYYY-MM-DD')";
                                SQL = SQL + ComNum.VBLF + " AND GBN2 IN('2','0') ";


                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                                //2018-12-10(2차수)
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUG";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         NO1 = 2 ";
                                SQL = SQL + ComNum.VBLF + "WHERE MDATE >=TO_DATE('" + strDate + " 09:00:01','YYYY-MM-DD HH24:MI:SS') ";
                                SQL = SQL + ComNum.VBLF + " AND MDATE <=TO_DATE('" + strDate + " 11:30:00','YYYY-MM-DD HH24:MI:SS') ";
                                SQL = SQL + ComNum.VBLF + " AND BUILDDATE = TO_DATE('" + strDate + "' ,'YYYY-MM-DD')";
                                SQL = SQL + ComNum.VBLF + " AND GBN2 IN('2','0') ";


                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                                //2018-12-10(3차수)
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUG";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         NO1 = 3 ";
                                SQL = SQL + ComNum.VBLF + "WHERE MDATE >=TO_DATE('" + strDate + " 11:30:01','YYYY-MM-DD HH24:MI:SS') ";
                                //SQL = SQL + ComNum.VBLF + " AND MDATE <=TO_DATE('" + strDate + " 16:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                //2020-08-24, 16:00 -> 15:00 변경
                                SQL = SQL + ComNum.VBLF + " AND MDATE <=TO_DATE('" + strDate + " 15:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                SQL = SQL + ComNum.VBLF + " AND BUILDDATE = TO_DATE('" + strDate + "' ,'YYYY-MM-DD')";
                                SQL = SQL + ComNum.VBLF + " AND GBN2 IN('2','0') ";


                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }
                            else
                            {
                                if (strYoil == "토요일")
                                {
                                    //2018-12-10(1차수)
                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUG";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         NO1 = 1 ";
                                    //SQL = SQL + ComNum.VBLF + "WHERE MDATE >=TO_DATE('" + strDateB + " 16:00:01','YYYY-MM-DD HH24:MI:SS') ";
                                    //2020-08-24, 16:00 -> 15:00 변경
                                    SQL = SQL + ComNum.VBLF + "WHERE MDATE >=TO_DATE('" + strDateB + " 15:00:01','YYYY-MM-DD HH24:MI:SS') ";
                                    SQL = SQL + ComNum.VBLF + " AND MDATE <=TO_DATE('" + strDate + " 12:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                    SQL = SQL + ComNum.VBLF + " AND BUILDDATE = TO_DATE('" + strDate + "' ,'YYYY-MM-DD')";
                                    SQL = SQL + ComNum.VBLF + " AND GBN2 IN('2','0') ";


                                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }
                                else
                                {
                                    //2018-12-10(1차수)
                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUG";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         NO1 = 1 ";
                                    //SQL = SQL + ComNum.VBLF + "WHERE MDATE >=TO_DATE('" + strDateB + " 16:00:01','YYYY-MM-DD HH24:MI:SS') ";
                                    //2020-08-24, 16:00 -> 15:00 변경
                                    SQL = SQL + ComNum.VBLF + "WHERE MDATE >=TO_DATE('" + strDateB  + (bolHuilY ? "12:00:00" : "15:00:01") + "','YYYY-MM-DD HH24:MI:SS') ";
                                    SQL = SQL + ComNum.VBLF + " AND MDATE <=TO_DATE('" + strDate + " 09:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                    SQL = SQL + ComNum.VBLF + " AND BUILDDATE = TO_DATE('" + strDate + "' ,'YYYY-MM-DD')";
                                    SQL = SQL + ComNum.VBLF + " AND GBN2 IN('2','0') ";


                                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                    //2018-12-10(2차수)
                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUG";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         NO1 = 2 ";
                                    SQL = SQL + ComNum.VBLF + "WHERE MDATE >=TO_DATE('" + strDate + " 09:00:01','YYYY-MM-DD HH24:MI:SS') ";
                                    SQL = SQL + ComNum.VBLF + " AND MDATE <=TO_DATE('" + strDate + " 11:30:00','YYYY-MM-DD HH24:MI:SS') ";
                                    SQL = SQL + ComNum.VBLF + " AND BUILDDATE = TO_DATE('" + strDate + "' ,'YYYY-MM-DD')";
                                    SQL = SQL + ComNum.VBLF + " AND GBN2 IN('2','0') ";


                                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                    //2018-12-10(3차수)
                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUG";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         NO1 = 3 ";
                                    SQL = SQL + ComNum.VBLF + "WHERE MDATE >=TO_DATE('" + strDate + " 11:30:01','YYYY-MM-DD HH24:MI:SS') ";
                                    //SQL = SQL + ComNum.VBLF + " AND MDATE <=TO_DATE('" + strDate + " 16:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                    //2020-08-24, 16:00 -> 15:00 변경
                                    SQL = SQL + ComNum.VBLF + " AND MDATE <=TO_DATE('" + strDate + " 15:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                    SQL = SQL + ComNum.VBLF + " AND BUILDDATE = TO_DATE('" + strDate + "' ,'YYYY-MM-DD')";
                                    SQL = SQL + ComNum.VBLF + " AND GBN2 IN('2','0') ";


                                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }

                            }
                        }
                        else if (strYoil == "일요일")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUG";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         NO1 = 1 ";
                            SQL = SQL + ComNum.VBLF + "WHERE MDATE >=TO_DATE('" + strDateB + " 12:00:01','YYYY-MM-DD HH24:MI:SS') ";
                            SQL = SQL + ComNum.VBLF + " AND MDATE <=TO_DATE('" + strDate + " 12:00:00','YYYY-MM-DD HH24:MI:SS') ";
                            SQL = SQL + ComNum.VBLF + " AND BUILDDATE = TO_DATE('" + strDate + "' ,'YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + " AND GBN2 IN('2','0') ";


                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                        }
                        //2020-01-01 휴일인데 마감안되서 로직 추가.
                        else if (bolHuil == true )
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUG";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         NO1 = 1 ";
                            //SQL = SQL + ComNum.VBLF + "WHERE MDATE >=TO_DATE('" + strDateB + " 16:00:01','YYYY-MM-DD HH24:MI:SS') ";
                            //2020-08-24, 16:00 -> 15:00 변경
                            SQL = SQL + ComNum.VBLF + "WHERE MDATE >=TO_DATE('" + strDateB + " 15:00:01','YYYY-MM-DD HH24:MI:SS') ";
                            #region 2021-05-20 마취과 요청으로 수정 함. 12시까지(토, 일과 동일하게.)
                            //SQL = SQL + ComNum.VBLF + " AND MDATE <=TO_DATE('" + strDate + " 09:00:00','YYYY-MM-DD HH24:MI:SS') ";
                            SQL = SQL + ComNum.VBLF + " AND MDATE <=TO_DATE('" + strDate + " 12:00:00','YYYY-MM-DD HH24:MI:SS') ";
                            #endregion
                            SQL = SQL + ComNum.VBLF + " AND BUILDDATE = TO_DATE('" + strDate + "' ,'YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + " AND GBN2 IN('2','0') ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }


                        //자동 입고 처리 ----------------------------------------------------
                        //당일입고 데이타 삭제
                        SQL = "";
                        SQL = "DELETE " + ComNum.DB_MED + "OCS_DRUG    ";
                        SQL = SQL + ComNum.VBLF + " WHERE BUILDDATE =TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND GBN2 = '1' ";
                        SQL = SQL + ComNum.VBLF + "     AND REALQTY IS NULL ";
                        SQL = SQL + ComNum.VBLF + "     AND WARDCODE ='" + GstrWardCode + "' ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        break;
                    case "4H":  //호스피스
                    case "AG":  //혈관조영실
                    case "ER":  //응급의료센터
                    case "32":
                    case "33":
                    case "35":
                    case "JS":
                        //자기 차수에  비치  수량 만큼만 빌드 처리
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     TO_CHAR(MAX(JDATE) ,'YYYY-MM-DD') AS JDATE, SUCODE";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG_SET ";
                        SQL = SQL + ComNum.VBLF + "     WHERE JDATE <= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND WARDCODE ='" + GstrWardCode + "'  "; // ICU
                        SQL = SQL + ComNum.VBLF + "         AND GBN = '1'  ";    //마약
                        SQL = SQL + ComNum.VBLF + "GROUP BY SUCODE  ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                if (GstrWardCode != "JS")
                                {
                                    SQL = "";
                                    SQL = "SELECT QTY, SUCODE FROM " + ComNum.DB_MED + "OCS_DRUG_SET ";
                                    SQL = SQL + ComNum.VBLF + "     WHERE JDATE <= TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "         AND WARDCODE = '" + GstrWardCode + "'  "; //ICU
                                    SQL = SQL + ComNum.VBLF + "         AND SUCODE = '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "ORDER BY JDATE DESC ";
                                }
                                else
                                {
                                    SQL = "";
                                    SQL = "SELECT QTY , SUCODE FROM " + ComNum.DB_MED + "OCS_DRUG_SET ";
                                    SQL = SQL + ComNum.VBLF + "WHERE JDATE = TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE = '" + GstrWardCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND SUCODE = '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                                }

                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    dblQty = VB.Val(dt1.Rows[0]["QTY"].ToString().Trim());

                                    if (GstrWardCode != "JS")
                                    {
                                        dblJQty = VB.Val(dt1.Rows[0]["QTY"].ToString().Trim());
                                    }

                                    strSuCode = dt1.Rows[0]["SUCODE"].ToString().Trim();
                                }

                                dt1.Dispose();
                                dt1 = null;

                                if (GstrWardCode != "JS")
                                {
                                    if (dblJQty != 0)
                                    {
                                        SQL = "";
                                        SQL = "SELECT ROWID, (QTY * NAL) AS QTY FROM " + ComNum.DB_MED + "OCS_MAYAK";

                                        switch (GstrWardCode)
                                        {
                                            case "4H":
                                            case "32":
                                            case "33":
                                            case "35":
                                                SQL = SQL + ComNum.VBLF + " WHERE BDATE_R >=TO_DATE('2014-06-10','YYYY-MM-DD') ";
                                                break;
                                            case "AG":
                                                SQL = SQL + ComNum.VBLF + " WHERE BDATE_R >=TO_DATE('2014-10-02','YYYY-MM-DD') ";
                                                break;
                                            case "ER":
                                                SQL = SQL + ComNum.VBLF + " WHERE BDATE_R >=TO_DATE('2014-04-01','YYYY-MM-DD') ";
                                                break;
                                        }

                                        SQL = SQL + ComNum.VBLF + "     AND BDATE_R <=TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                                        if (GstrWardCode == "ER")
                                        {
                                            SQL = SQL + ComNum.VBLF + "     AND ( WARDCODE ='" + GstrWardCode + "' OR ( WARDCODE IS NULL AND DEPTCODE ='" + GstrWardCode + "'))"; //ICU
                                        }
                                        else
                                        {
                                            SQL = SQL + ComNum.VBLF + "     AND WARDCODE ='" + GstrWardCode + "'  "; //ICU
                                        }

                                        SQL = SQL + ComNum.VBLF + "     AND ORDERNO >0 ";
                                        SQL = SQL + ComNum.VBLF + "     AND MDATE IS NULL";

                                        switch (GstrWardCode)
                                        {
                                            case "4H":
                                            case "ER":
                                            case "32":
                                            case "33":
                                            case "35":
                                                SQL = SQL + ComNum.VBLF + "     AND ACTTIME IS NOT NULL ";
                                                SQL = SQL + ComNum.VBLF + "     AND QTY <= " + dblJQty + " ";
                                                break;
                                        }

                                        if (GstrWardCode == "ER")
                                        {
                                            SQL = SQL + ComNum.VBLF + "     AND SUCODE = '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                                        }
                                        else
                                        {
                                            SQL = SQL + ComNum.VBLF + "     AND SUCODE = '" + strSuCode.Trim() + "' ";
                                        }

                                        SQL = SQL + ComNum.VBLF + "     AND DOSCODE NOT IN ";                    //내시경용법제외
                                        SQL = SQL + ComNum.VBLF + "          (SELECT DOSCODE ";
                                        SQL = SQL + ComNum.VBLF + "             FROM ADMIN.OCS_ODOSAGE ";
                                        SQL = SQL + ComNum.VBLF + "           WHERE WARDCODE = 'EN')";
                                        SQL = SQL + ComNum.VBLF + "ORDER BY ENTDATE ";

                                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            Cursor.Current = Cursors.Default;
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            return rtnVal;
                                        }
                                        if (dt1.Rows.Count > 0)
                                        {
                                            for (k = 0; k < dt1.Rows.Count; k++)
                                            {
                                                if ((dblQty - VB.Val(dt1.Rows[k]["QTY"].ToString().Trim())) >= 0)
                                                {
                                                    dblQty = dblQty - VB.Val(dt1.Rows[k]["QTY"].ToString().Trim());

                                                    SQL = "";
                                                    SQL = "UPDATE " + ComNum.DB_MED + "OCS_MAYAK";
                                                    SQL = SQL + ComNum.VBLF + "     SET";
                                                    SQL = SQL + ComNum.VBLF + "         MDATE = TO_DATE('" + strDate + "','YYYY-MM-DD'), ";
                                                    SQL = SQL + ComNum.VBLF + "         CHASU = '" + strChasu + "' ";
                                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[k]["ROWID"].ToString().Trim() + "' ";

                                                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                                    if (SqlErr != "")
                                                    {
                                                        clsDB.setRollbackTran(clsDB.DbCon);
                                                        ComFunc.MsgBox(SqlErr);
                                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                        Cursor.Current = Cursors.Default;
                                                        return rtnVal;
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }

                                        dt1.Dispose();
                                        dt1 = null;

                                        switch (GstrWardCode)
                                        {
                                            case "4H":
                                            case "AG":
                                            case "32":
                                            case "33":
                                            case "35":
                                                //비치수량 변경시 자동 입고처리
                                                SQL = "";
                                                SQL = "SELECT SUM(QTY * NAL ) QTY";
                                                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_DRUG ";
                                                SQL = SQL + ComNum.VBLF + "   WHERE WARDCODE ='" + GstrWardCode + "'  ";    //ICU
                                                SQL = SQL + ComNum.VBLF + "     AND GBN2 = '3' ";   //재고
                                                SQL = SQL + ComNum.VBLF + "     AND BUILDDATE =TO_DATE('" + strDateB + "','YYYY-MM-DD') ";  //전일
                                                SQL = SQL + ComNum.VBLF + "     AND SUCODE = '" + strSuCode + "' ";

                                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                                if (SqlErr != "")
                                                {
                                                    clsDB.setRollbackTran(clsDB.DbCon);
                                                    Cursor.Current = Cursors.Default;
                                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                    return rtnVal;
                                                }
                                                if (dt1.Rows.Count > 0)
                                                {
                                                    if (dblJQty != VB.Val(dt1.Rows[0]["QTY"].ToString().Trim()))
                                                    {
                                                        SQL = "";
                                                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                                                        SQL = SQL + ComNum.VBLF + "     (NO1, BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY , NAL, ENTDATE, BUILDDATE )   ";
                                                        SQL = SQL + ComNum.VBLF + "VALUES";
                                                        SQL = SQL + ComNum.VBLF + "     (";
                                                        SQL = SQL + ComNum.VBLF + "         '" + strChasu + "' ,";
                                                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                                                        SQL = SQL + ComNum.VBLF + "         '1' ,'1', ";
                                                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                                                        SQL = SQL + ComNum.VBLF + "         '" + strSuCode + "', ";
                                                        SQL = SQL + ComNum.VBLF + "         '" + (dblJQty - VB.Val(dt1.Rows[0]["QTY"].ToString().Trim())) + "',";
                                                        SQL = SQL + ComNum.VBLF + "         1, SYSDATE,";
                                                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                                                        SQL = SQL + ComNum.VBLF + "     )";

                                                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                                        if (SqlErr != "")
                                                        {
                                                            clsDB.setRollbackTran(clsDB.DbCon);
                                                            ComFunc.MsgBox(SqlErr);
                                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                            Cursor.Current = Cursors.Default;
                                                            return rtnVal;
                                                        }
                                                    }
                                                }

                                                dt1.Dispose();
                                                dt1 = null;
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    SQL = "";
                                    SQL = "SELECT";
                                    SQL = SQL + ComNum.VBLF + "     ROWID, (QTY * NAL) AS QTY";
                                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_JUSASUB ";
                                    SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE >= TO_DATE('2014-06-01','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "         AND ACTDATE <= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "         AND Sucode IN ( SELECT JEPCODE FROM " + ComNum.DB_ERP + "DRUG_JEP WHERE CHENGGU = '09'  )";  //마약
                                    SQL = SQL + ComNum.VBLF + "         AND PART = '1' ";
                                    SQL = SQL + ComNum.VBLF + "         AND SUCODE = '" + strSuCode.Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "         AND MDATE IS NULL ";
                                    SQL = SQL + ComNum.VBLF + "ORDER BY ACTDATE2 ";

                                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        return rtnVal;
                                    }
                                    if (dt1.Rows.Count > 0)
                                    {
                                        for(k = 0; k < dt1.Rows.Count; k++)
                                        {
                                            if (dblQty - VB.Val(dt1.Rows[k]["QTY"].ToString().Trim()) >= 0)
                                            {
                                                dblQty = dblQty - VB.Val(dt1.Rows[k]["QTY"].ToString().Trim());

                                                SQL = "";
                                                SQL = "UPDATE " + ComNum.DB_MED + "ETC_JUSASUB";
                                                SQL = SQL + ComNum.VBLF + "     SET";
                                                SQL = SQL + ComNum.VBLF + "         MDATE = TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                                                SQL = SQL + ComNum.VBLF + "         CHASU = '" + strChasu + "',";
                                                SQL = SQL + ComNum.VBLF + "         GBN ='1'  ";
                                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[k]["ROWID"].ToString().Trim() + "' ";

                                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                                if (SqlErr != "")
                                                {
                                                    clsDB.setRollbackTran(clsDB.DbCon);
                                                    ComFunc.MsgBox(SqlErr);
                                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                    Cursor.Current = Cursors.Default;
                                                    return rtnVal;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }

                                    dt1.Dispose();
                                    dt1 = null;

                                    //비치수량 변경시 자동 입고처리
                                    //SQL = "";
                                    //SQL = "SELECT";
                                    //SQL = SQL + ComNum.VBLF + "     SUM(QTY * NAL) AS QTY";
                                    //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG ";
                                    //SQL = SQL + ComNum.VBLF + "     WHERE WARDCODE = '" + GstrWardCode + "' ";
                                    //SQL = SQL + ComNum.VBLF + "         AND GBN2 = '3' ";  //재고
                                    //SQL = SQL + ComNum.VBLF + "         AND BUILDDATE = TO_DATE('" + strDateB + "','YYYY-MM-DD') ";  //전일
                                    //SQL = SQL + ComNum.VBLF + "         AND SUCODE = '" + strSuCode + "' ";

                                    //SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                    //if (SqlErr != "")
                                    //{
                                    //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    //    return rtnVal;
                                    //}
                                    //if (dt1.Rows.Count > 0)
                                    //{
                                    //    if (dblQty != VB.Val(dt1.Rows[0]["QTY"].ToString().Trim()))
                                    //    {
                                    //        SQL = "";
                                    //        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                                    //        SQL = SQL + ComNum.VBLF + "     (NO1, BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY , NAL, ENTDATE, BUILDDATE)";
                                    //        SQL = SQL + ComNum.VBLF + "VALUES";
                                    //        SQL = SQL + ComNum.VBLF + "     (";
                                    //        SQL = SQL + ComNum.VBLF + "         '" + strChasu + "',";
                                    //        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                                    //        SQL = SQL + ComNum.VBLF + "         '1',";
                                    //        SQL = SQL + ComNum.VBLF + "         '1',";
                                    //        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                                    //        SQL = SQL + ComNum.VBLF + "         '" + strSuCode + "',";
                                    //        SQL = SQL + ComNum.VBLF + "         '" + (dblQty - VB.Val(dt1.Rows[0]["QTY"].ToString().Trim())) + "',";
                                    //        SQL = SQL + ComNum.VBLF + "         1,";
                                    //        SQL = SQL + ComNum.VBLF + "         SYSDATE,";
                                    //        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                                    //        SQL = SQL + ComNum.VBLF + "     )";

                                    //        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                    //        if (SqlErr != "")
                                    //        {
                                    //            clsDB.setRollbackTran(clsDB.DbCon);
                                    //            ComFunc.MsgBox(SqlErr);
                                    //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    //            Cursor.Current = Cursors.Default;
                                    //            return rtnVal;
                                    //        }
                                    //    }
                                    //}

                                    //dt1.Dispose();
                                    //dt1 = null;
                                }
                            }
                        }

                        dt.Dispose();
                        dt = null;

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     TO_CHAR(MAX(JDATE) ,'YYYY-MM-DD') JDATE , SUCODE  ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG_SET ";
                        SQL = SQL + ComNum.VBLF + "     WHERE JDATE <=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND WARDCODE ='" + GstrWardCode + "' "; //ICU
                        SQL = SQL + ComNum.VBLF + "         AND GBN = '2'  ";  //향정
                        SQL = SQL + ComNum.VBLF + "GROUP BY SUCODE  ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                if (GstrWardCode != "JS")
                                {
                                    SQL = "";
                                    SQL = "SELECT QTY, SUCODE FROM " + ComNum.DB_MED + "OCS_DRUG_SET ";
                                    SQL = SQL + ComNum.VBLF + " WHERE JDATE <= TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "    AND WARDCODE ='" + GstrWardCode + "'  "; //ICU
                                    SQL = SQL + ComNum.VBLF + "    AND SUCODE = '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "ORDER BY  JDATE DESC ";
                                }
                                else
                                {
                                    SQL = "SELECT QTY , SUCODE FROM " + ComNum.DB_MED + "OCS_DRUG_SET ";
                                    SQL = SQL + ComNum.VBLF + " WHERE JDATE = TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE = '" + GstrWardCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND SUCODE = '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                                }

                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    dblQty = VB.Val(dt1.Rows[0]["QTY"].ToString().Trim());

                                    if (GstrWardCode != "JS")
                                    {
                                        dblJQty = VB.Val(dt1.Rows[0]["QTY"].ToString().Trim());
                                    }

                                    strSuCode = dt1.Rows[0]["SUCODE"].ToString().Trim();
                                }

                                dt1.Dispose();
                                dt1 = null;

                                if (GstrWardCode != "JS")
                                {
                                    if (dblJQty != 0)
                                    {
                                        SQL = "";
                                        SQL = "SELECT";
                                        SQL = SQL + ComNum.VBLF + "     ROWID, (QTY * NAL) AS QTY";
                                        SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_MED + "OCS_HYANG ";

                                        switch (GstrWardCode)
                                        {
                                            case "4H":
                                            case "32":
                                            case "33":
                                            case "35":
                                                SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TO_DATE('2014-06-23','YYYY-MM-DD') ";
                                                break;
                                            case "AG":
                                                SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TO_DATE('2014-10-02','YYYY-MM-DD') ";
                                                break;
                                            case "ER":
                                                SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TO_DATE('2014-04-01','YYYY-MM-DD') ";
                                                break;
                                        }

                                        SQL = SQL + ComNum.VBLF + "    AND BDATE <= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE = '" + GstrWardCode + "'  "; //ICU
                                        SQL = SQL + ComNum.VBLF + "    AND ORDERNO > 0 ";
                                        SQL = SQL + ComNum.VBLF + "    AND MDATE IS NULL";

                                        switch (GstrWardCode)
                                        {
                                            case "4H":
                                            case "ER":
                                            case "32":
                                            case "33":
                                            case "35":
                                                SQL = SQL + ComNum.VBLF + "    AND ACTTIME IS NOT NULL ";
                                                break;
                                        }

                                        SQL = SQL + ComNum.VBLF + "    AND SUCODE = '" + strSuCode.Trim() + "' ";

                                        if (GstrWardCode == "ER")
                                        {
                                            SQL = SQL + ComNum.VBLF + "    AND QTY <= '" + dblQty + "' ";
                                        }

                                        SQL = SQL + ComNum.VBLF + "    AND DOSCODE NOT IN  ";                    //내시경용법제외
                                        SQL = SQL + ComNum.VBLF + "             (SELECT DOSCODE ";
                                        SQL = SQL + ComNum.VBLF + "                 FROM ADMIN.OCS_ODOSAGE ";
                                        SQL = SQL + ComNum.VBLF + "              WHERE WARDCODE = 'EN') ";  //내시경실 제외

                                        switch (GstrWardCode)
                                        {
                                            case "4H":
                                            case "AG":
                                            case "32":
                                            case "33":
                                            case "35":
                                                SQL = SQL + ComNum.VBLF + "    AND QTY * NAL <= '" + dblQty + "' ";
                                                break;
                                        }

                                        SQL = SQL + ComNum.VBLF + "ORDER BY ENTDATE ";

                                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            Cursor.Current = Cursors.Default;
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            return rtnVal;
                                        }
                                        if (dt1.Rows.Count > 0)
                                        {
                                            for (k = 0; k < dt1.Rows.Count; k++)
                                            {
                                                if ((dblQty - VB.Val(dt1.Rows[k]["QTY"].ToString().Trim())) >= 0)
                                                {
                                                    dblQty = dblQty - VB.Val(dt1.Rows[k]["QTY"].ToString().Trim());

                                                    SQL = "";
                                                    SQL = "UPDATE " + ComNum.DB_MED + "OCS_HYANG";
                                                    SQL = SQL + ComNum.VBLF + "     SET";

                                                    switch (GstrWardCode)
                                                    {
                                                        case "4H":
                                                        case "AG":
                                                        case "32":
                                                        case "33":
                                                        case "35":
                                                            SQL = SQL + ComNum.VBLF + "         MDATE = TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                                                            break;
                                                        case "ER":
                                                            SQL = SQL + ComNum.VBLF + "         MDATE = SYSDATE,";
                                                            break;
                                                    }

                                                    SQL = SQL + ComNum.VBLF + "         CHASU = '" + strChasu + "'  ";
                                                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt1.Rows[k]["ROWID"].ToString().Trim() + "' ";

                                                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                                    if (SqlErr != "")
                                                    {
                                                        clsDB.setRollbackTran(clsDB.DbCon);
                                                        ComFunc.MsgBox(SqlErr);
                                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                        Cursor.Current = Cursors.Default;
                                                        return rtnVal;
                                                    }
                                                }
                                                else
                                                {
                                                    ComFunc.MsgBox(strSuCode.Trim() + " 비치 수량보다 소모량이 큽니다.");
                                                    break;
                                                }
                                            }
                                        }

                                        dt1.Dispose();
                                        dt1 = null;

                                        switch (GstrWardCode)
                                        {
                                            case "4H":
                                            case "AG":
                                            case "32":
                                            case "33":
                                            case "35":
                                                //비치수량 변경시 자동 입고처리
                                                SQL = "";
                                                SQL = "SELECT";
                                                SQL = SQL + ComNum.VBLF + "     SUM(QTY * NAL) AS QTY";
                                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG ";
                                                SQL = SQL + ComNum.VBLF + " WHERE  WARDCODE ='" + GstrWardCode + "'  "; //ICU
                                                SQL = SQL + ComNum.VBLF + "     AND GBN2 = '3' ";  //재고
                                                SQL = SQL + ComNum.VBLF + "     AND BUILDDATE =TO_DATE('" + strDateB + "','YYYY-MM-DD') "; //전일
                                                SQL = SQL + ComNum.VBLF + "     AND SUCODE = '" + strSuCode + "' ";

                                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                                if (SqlErr != "")
                                                {
                                                    clsDB.setRollbackTran(clsDB.DbCon);
                                                    Cursor.Current = Cursors.Default;
                                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                    return rtnVal;
                                                }
                                                if (dt1.Rows.Count > 0)
                                                {
                                                    if (dblJQty != VB.Val(dt1.Rows[0]["QTY"].ToString().Trim()))
                                                    {
                                                        SQL = "";
                                                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                                                        SQL = SQL + ComNum.VBLF + "     (NO1,  BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY , NAL, ENTDATE, BUILDDATE )   ";
                                                        SQL = SQL + ComNum.VBLF + "VALUES";
                                                        SQL = SQL + ComNum.VBLF + "     (";
                                                        SQL = SQL + ComNum.VBLF + "         '" + strChasu + "',";
                                                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                                                        SQL = SQL + ComNum.VBLF + "         '2' ,'1',";
                                                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                                                        SQL = SQL + ComNum.VBLF + "         '" + strSuCode + "', ";
                                                        SQL = SQL + ComNum.VBLF + "         '" + (dblJQty - VB.Val(dt1.Rows[0]["QTY"].ToString().Trim())) + "',";
                                                        SQL = SQL + ComNum.VBLF + "         1, SYSDATE,";
                                                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                                                        SQL = SQL + ComNum.VBLF + "     )";

                                                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                                        if (SqlErr != "")
                                                        {
                                                            clsDB.setRollbackTran(clsDB.DbCon);
                                                            ComFunc.MsgBox(SqlErr);
                                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                            Cursor.Current = Cursors.Default;
                                                            return rtnVal;
                                                        }
                                                    }
                                                }

                                                dt1.Dispose();
                                                dt1 = null;
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    SQL = "";
                                    SQL = "SELECT ROWID, (QTY * NAL) AS QTY FROM " + ComNum.DB_MED + "ETC_JUSASUB ";
                                    SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE >= TO_DATE('2014-06-16','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "    AND ACTDATE <= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "    AND Sucode IN (SELECT B.SUNEXT FROM " + ComNum.DB_ERP + "DRUG_JEP A, " + ComNum.DB_ERP + "DRUG_CONVRATE B WHERE A.SUB = '16' AND A.BUN = '2' AND A.JEPCODE = B.JEPCODE)"; //향정
                                    SQL = SQL + ComNum.VBLF + "    AND PART = '1' ";
                                    SQL = SQL + ComNum.VBLF + "    AND SUCODE = '" + strSuCode.Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "    AND MDATE IS NULL ";
                                    SQL = SQL + ComNum.VBLF + "ORDER BY ACTDATE2 ";

                                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        return rtnVal;
                                    }
                                    if (dt1.Rows.Count > 0)
                                    {
                                        for (k = 0; k < dt1.Rows.Count; k++)
                                        {
                                            if (dblQty - VB.Val(dt1.Rows[k]["QTY"].ToString().Trim()) >= 0)
                                            {
                                                dblQty = dblQty - VB.Val(dt1.Rows[k]["QTY"].ToString().Trim());

                                                SQL = "";
                                                SQL = "UPDATE " + ComNum.DB_MED + "ETC_JUSASUB";
                                                SQL = SQL + ComNum.VBLF + "     SET";
                                                SQL = SQL + ComNum.VBLF + "         MDATE = TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                                                SQL = SQL + ComNum.VBLF + "         CHASU = '" + strChasu + "',";
                                                SQL = SQL + ComNum.VBLF + "         GBN ='2'";
                                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[k]["ROWID"].ToString().Trim() + "' ";

                                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                                if (SqlErr != "")
                                                {
                                                    clsDB.setRollbackTran(clsDB.DbCon);
                                                    ComFunc.MsgBox(SqlErr);
                                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                    Cursor.Current = Cursors.Default;
                                                    return rtnVal;
                                                }
                                            }
                                            else
                                            {
                                                ComFunc.MsgBox(strSuCode.Trim() + " 비치 수량보다 소모량이 큽니다.");
                                                break;
                                            }
                                        }
                                    }

                                    dt1.Dispose();
                                    dt1 = null;

                                    //비치수량 변경시 자동 입고처리
                                    //SQL = "";
                                    //SQL = "SELECT SUM(QTY * NAL) AS QTY";
                                    //SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_DRUG ";
                                    //SQL = SQL + ComNum.VBLF + "WHERE WARDCODE = '" + GstrWardCode + "' ";
                                    //SQL = SQL + ComNum.VBLF + "     AND GBN2 = '3' ";   //재고
                                    //SQL = SQL + ComNum.VBLF + "     AND BUILDDATE = TO_DATE('" + strDateB + "','YYYY-MM-DD') ";  //전일
                                    //SQL = SQL + ComNum.VBLF + "     AND SUCODE = '" + strSuCode + "' ";

                                    //SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                    //if (SqlErr != "")
                                    //{
                                    //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    //    return rtnVal;
                                    //}
                                    //if (dt1.Rows.Count > 0)
                                    //{
                                    //    if (dblQty != VB.Val(dt1.Rows[0]["QTY"].ToString().Trim()))
                                    //    {
                                    //        SQL = "";
                                    //        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                                    //        SQL = SQL + ComNum.VBLF + "     (NO1, BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, NAL, ENTDATE, BUILDDATE)";
                                    //        SQL = SQL + ComNum.VBLF + "VALUES";
                                    //        SQL = SQL + ComNum.VBLF + "     (";
                                    //        SQL = SQL + ComNum.VBLF + "         '" + strChasu + "',";
                                    //        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                                    //        SQL = SQL + ComNum.VBLF + "         '2',";
                                    //        SQL = SQL + ComNum.VBLF + "         '1',";
                                    //        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                                    //        SQL = SQL + ComNum.VBLF + "         '" + strSuCode + "', ";
                                    //        SQL = SQL + ComNum.VBLF + "         '" + (dblQty - VB.Val(dt1.Rows[0]["QTY"].ToString().Trim())) + "',";
                                    //        SQL = SQL + ComNum.VBLF + "         1,";
                                    //        SQL = SQL + ComNum.VBLF + "         SYSDATE,";
                                    //        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                                    //        SQL = SQL + ComNum.VBLF + "     )";

                                    //        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                    //        if (SqlErr != "")
                                    //        {
                                    //            clsDB.setRollbackTran(clsDB.DbCon);
                                    //            ComFunc.MsgBox(SqlErr);
                                    //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    //            Cursor.Current = Cursors.Default;
                                    //            return rtnVal;
                                    //        }
                                    //    }
                                    //}

                                    //dt1.Dispose();
                                    //dt1 = null;
                                }
                            }
                        }

                        dt.Dispose();
                        dt = null;

                        //자료 형성
                        if (GstrWardCode != "JS")
                        {
                            SQL = " INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                            SQL = SQL + ComNum.VBLF + "     ( NO1, BDATE ,GBN, GBN2, PTNO, SNAME,DEPTCODE, DRSABUN, IO, WARDCODE, ROOMCODE, SUCODE, QTY, REALQTY, NAL, DOSCODE, ORDERNO, ENTDATE, BUILDDATE, MDATE, SUCODER ) ";
                            SQL = SQL + ComNum.VBLF + " SELECT '" + strChasu + "' , BDATE_R, '1',  '2',  PTNO, SNAME,  DEPTCODE,  DRSABUN, IO, '" + GstrWardCode + "', ROOMCODE, DECODE(RTRIM(SUCODE),'N-FE-PC','N-FE2',SUCODE),  TRUNC(QTY+ 0.9), REALQTY, NAL, DOSCODE, ORDERNO, ENTDATE, TO_DATE('" + strDate + "','YYYY-MM-DD'), MDATE, SUCODE ";
                            SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_MED + "OCS_MAYAK A ";

                            switch (GstrWardCode)
                            {
                                case "4H":
                                case "AG":
                                case "32":
                                case "33":
                                case "35":
                                    SQL = SQL + ComNum.VBLF + "    WHERE WARDCODE ='" + GstrWardCode + "'  "; //ICU
                                    break;
                                case "ER":
                                    SQL = SQL + ComNum.VBLF + "    WHERE ( WARDCODE ='" + GstrWardCode + "' OR(  WARDCODE IS NULL AND DEPTCODE ='" + GstrWardCode + "'))"; //ICU
                                    break;
                            }

                            SQL = SQL + ComNum.VBLF + "      AND ORDERNO >0 ";
                            SQL = SQL + ComNum.VBLF + "      AND TRUNC(MDATE)  >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "      AND CHASU = '" + strChasu + "' ";
                            SQL = SQL + ComNum.VBLF + " UNION ALL ";
                            SQL = SQL + ComNum.VBLF + "   SELECT '" + strChasu + "' , BDATE, '2',  '2',  PTNO, SNAME,  DEPTCODE,  DRSABUN, IO, '" + GstrWardCode + "', ROOMCODE, SUCODE,  TRUNC(QTY+ 0.9), REALQTY, NAL, DOSCODE, ORDERNO, ENTDATE, TO_DATE('" + strDate + "','YYYY-MM-DD') , MDATE, SUCODE ";
                            SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_MED + "OCS_HYANG A ";

                            switch (GstrWardCode)
                            {
                                case "4H":
                                case "AG":
                                case "32":
                                case "33":
                                case "35":
                                    SQL = SQL + ComNum.VBLF + "    WHERE WARDCODE ='" + GstrWardCode + "'  "; //ICU
                                    break;
                                case "ER":
                                    SQL = SQL + ComNum.VBLF + "    WHERE ( WARDCODE ='" + GstrWardCode + "' OR(  WARDCODE IS NULL AND DEPTCODE ='" + GstrWardCode + "'))"; //ICU
                                    break;
                            }

                            SQL = SQL + ComNum.VBLF + "       AND ORDERNO >0 ";
                            SQL = SQL + ComNum.VBLF + "       AND TRUNC(MDATE) = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "       AND CHASU = '" + strChasu + "' ";
                            SQL = SQL + ComNum.VBLF + "    ORDER BY MDATE ASC  ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                        else
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                            SQL = SQL + ComNum.VBLF + "     (NO1, BDATE ,GBN, GBN2, PTNO, SNAME, DEPTCODE, DRSABUN, IO, WARDCODE, ROOMCODE, SUCODE, QTY, REALQTY, NAL, DOSCODE, ORDERNO, ENTDATE, BUILDDATE, MDATE, SUCODER)";
                            SQL = SQL + ComNum.VBLF + "SELECT";
                            SQL = SQL + ComNum.VBLF + "     '" + strChasu + "', BDATE , GBN, '2', PTNO, B.SNAME, A.DEPTCODE, C.SABUN, 'O', '" + GstrWardCode + "', '', SUCODE,  TRUNC(QTY + 0.9), QTY, NAL, DOSCODE, ORDERNO, A.ACTDATE2, TO_DATE('" + strDate + "','YYYY-MM-DD'), MDATE, SUCODE";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_JUSASUB A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_MED + "OCS_DOCTOR C ";
                            SQL = SQL + ComNum.VBLF + "   WHERE TRUNC(MDATE)  >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "      AND CHASU = '" + strChasu + "' ";
                            SQL = SQL + ComNum.VBLF + "      AND A.PTNO = B.PANO(+)";
                            SQL = SQL + ComNum.VBLF + "      AND A.DRCODE = C.DRCODE ";
                            SQL = SQL + ComNum.VBLF + "ORDER BY MDATE ASC  ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                        break;
                    case "EN":
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     MAX(NO1) AS MNO";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG ";
                        SQL = SQL + ComNum.VBLF + "     WHERE BUILDDATE =TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND WARDCODE = '" + GstrWardCode + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND GBN2 IN ('2','3') ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strChasu = (VB.Val(dt.Rows[i]["MNO"].ToString().Trim()) + 1).ToString();
                        }

                        dt.Dispose();
                        dt = null;

                        //소모내역 집계 ----------------------------------------------------

                        //기존자료 삭제
                        SQL = "";
                        SQL = "DELETE " + ComNum.DB_MED + "OCS_DRUG ";
                        SQL = SQL + ComNum.VBLF + "     WHERE BUILDDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND WARDCODE = '" + GstrWardCode + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND GBN2 IN ('3')";     //당일제고 (차수별 이라서 ) 당일제고는 항상 삭제 후 생성

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        //자기 차수에  비치  수량 만큼만 빌드 처리
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     TO_CHAR(MAX(JDATE),'YYYY-MM-DD') AS JDATE, SUCODE, GBN";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG_SET";
                        SQL = SQL + ComNum.VBLF + "   WHERE JDATE <= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND WARDCODE= '" + GstrWardCode + "' ";
                        SQL = SQL + ComNum.VBLF + "     AND GBN ='2'  ";  //향정
                        SQL = SQL + ComNum.VBLF + "GROUP BY  SUCODE, GBN  ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                strGBn = dt.Rows[i]["GBN"].ToString().Trim();

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     QTY, SUCODE";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG_SET ";
                                SQL = SQL + ComNum.VBLF + "     WHERE JDATE <= TO_DATE('" + Convert.ToDateTime(dt.Rows[i]["JDATE"].ToString().Trim()).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "         AND WARDCODE = '" + GstrWardCode + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND SUCODE = '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "ORDER BY  JDATE DESC ";

                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    dblQty = VB.Val(dt1.Rows[0]["QTY"].ToString().Trim());
                                    dblJQty = VB.Val(dt1.Rows[0]["QTY"].ToString().Trim());
                                    strSuCode = dt1.Rows[0]["SUCODE"].ToString().Trim();
                                }

                                dt1.Dispose();
                                dt1 = null;

                                SQL = "";
                                SQL = "SELECT UNITNEW1 FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                                SQL = SQL + ComNum.VBLF + "  WHERE SUNEXT = '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "'";

                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    dblUnit = VB.Val(dt1.Rows[0]["UNITNEW1"].ToString().Trim());
                                }

                                dt1.Dispose();
                                dt1 = null;

                                //당월제고
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                                SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, SUCODE, WARDCODE, QTY, REALQTY, NAL, BUILDDATE) ";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + "         '" + strGBn + "', ";
                                SQL = SQL + ComNum.VBLF + "         '3',";
                                SQL = SQL + ComNum.VBLF + "         '" + strSuCode + "',";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                                SQL = SQL + ComNum.VBLF + "         '" + dblQty + "',";
                                SQL = SQL + ComNum.VBLF + "         '" + dblQty + "',";
                                SQL = SQL + ComNum.VBLF + "         '1',";
                                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                                SQL = SQL + ComNum.VBLF + "     )";

                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                                //if (strSuCode == "A-POL12G")    //로직 삭제하지 마삼
                                //{
                                //    i = i;
                                //    //dblQty = 10;
                                //}


                                //2019-07-25 코드변경 (A-POL12 -> A-POL12A, A-POL12G -> A-PO12GA)
                                //2020-10-07 코드추가 A-POL12A -> A-ANE12
                                if (dblJQty != 0 || strSuCode == "A-POL12A" || strSuCode == "A-ANE12")
                                {
                                    SQL = "";
                                    SQL = "SELECT";
                                    SQL = SQL + ComNum.VBLF + "     ROWID, (REALQTY * NAL) AS QTY";
                                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_HYANG";
                                    SQL = SQL + ComNum.VBLF + "   WHERE BDATE >= TO_DATE('2015-01-05','YYYY-MM-DD')";
                                    SQL = SQL + ComNum.VBLF + "      AND BDATE <= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "      AND WARDCODE ='" + GstrWardCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "      AND ( MDATE IS NULL OR TRUNC(MDATE) = TO_DATE('" + strDate + "','YYYY-MM-DD') )";
                                    SQL = SQL + ComNum.VBLF + "      AND ENTQTY > 0 ";
                                    SQL = SQL + ComNum.VBLF + "      AND SUCODE = '" + strSuCode.Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "      AND DOSCODE IN";                    //내시경용법
                                    SQL = SQL + ComNum.VBLF + "                 (SELECT DOSCODE ";
                                    SQL = SQL + ComNum.VBLF + "                     FROM " + ComNum.DB_MED + "OCS_ODOSAGE ";
                                    SQL = SQL + ComNum.VBLF + "                 WHERE WARDCODE = '" + GstrWardCode + "')";  //내시경실

                                    if (strSuCode != "A-POL12A" || strSuCode != "A-ANE12")
                                    {
                                        SQL = SQL + ComNum.VBLF + "      AND (REALQTY * NAL) <= '" + dblQty + "' ";
                                    }

                                    SQL = SQL + ComNum.VBLF + "  ORDER BY ENTDATE ";

                                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        return rtnVal;
                                    }

                                    //if (dt.Rows[i]["SUCODE"].ToString().Trim() == "A-POL12G")
                                    //{
                                    //    i = i;
                                    //}

                                    //if (dt.Rows[i]["SUCODE"].ToString().Trim() == "A-BASCA")
                                    //{
                                    //    i = i;
                                    //}

                                    if (dt1.Rows.Count > 0)
                                    {
                                        //기본 비치수량 보다 많이 소모 못하게함
                                        for(k = 0; k < dt1.Rows.Count; k++)
                                        {
                                            if (dblQty - VB.Val(dt1.Rows[k]["QTY"].ToString().Trim()) >= 0)
                                            {
                                                SQL = "";
                                                SQL = "UPDATE " + ComNum.DB_MED + "OCS_HYANG";
                                                SQL = SQL + ComNum.VBLF + "     SET";
                                                SQL = SQL + ComNum.VBLF + "         MDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                                                SQL = SQL + ComNum.VBLF + "         CHASU = '" + strChasu + "' ";
                                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[k]["ROWID"].ToString().Trim() + "' ";

                                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                                if (SqlErr != "")
                                                {
                                                    clsDB.setRollbackTran(clsDB.DbCon);
                                                    ComFunc.MsgBox(SqlErr);
                                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                    Cursor.Current = Cursors.Default;
                                                    return rtnVal;
                                                }

                                                //자료 형성
                                                //19-05-16 이현종 - 내시경실 요청으로 소수 둘째자리 까지만 표기되게 수정.
                                                SQL = "";
                                                SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                                                SQL = SQL + ComNum.VBLF + "     (NO1, BDATE ,GBN, GBN2, PTNO, SNAME, DEPTCODE, DRSABUN, IO, WARDCODE, ROOMCODE, SUCODE, QTY, REALQTY, NAL, DOSCODE, ORDERNO, ENTDATE, BUILDDATE, MDATE, SUCODER, ENTQTY2) ";
                                                SQL = SQL + ComNum.VBLF + "SELECT '" + strChasu + "' , BDATE, '" + strGBn + "', '2',  PTNO, SNAME,  DEPTCODE,  DRSABUN, IO, '" + GstrWardCode + "', ROOMCODE,  DECODE(RTRIM(SUCODE),'A-POL12A','A-PO12GA',SUCODE),  TRUNC(QTY+ 0.9), TRUNC(ENTQTY / " + dblUnit + ", 2), NAL, DOSCODE, ORDERNO, ENTDATE, TO_DATE('" + strDate + "','YYYY-MM-DD') , MDATE, SUCODE, ENTQTY ";
                                                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_MED + "OCS_HYANG A ";
                                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[k]["ROWID"].ToString().Trim() + "' ";

                                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                                if (SqlErr != "")
                                                {
                                                    clsDB.setRollbackTran(clsDB.DbCon);
                                                    ComFunc.MsgBox(SqlErr);
                                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                    Cursor.Current = Cursors.Default;
                                                    return rtnVal;
                                                }
                                            }
                                            else
                                            {
                                                ComFunc.MsgBox(strSuCode + " 비치 수량보다 소모량이 큽니다.");
                                                break;
                                            }
                                        }
                                    }

                                    dt1.Dispose();
                                    dt1 = null;
                                }
                            }
                        }

                        dt.Dispose();
                        dt = null;

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (NO1, BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, NAL, ENTDATE, BUILDDATE)";
                        SQL = SQL + ComNum.VBLF + "SELECT";
                        SQL = SQL + ComNum.VBLF + "     NO1, TO_DATE('" + strDate + "','YYYY-MM-DD'), '2' ,'1', '" + GstrWardCode + "', SUCODE, SUM(QTY * NAL),  1, SYSDATE, TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "  WHERE BUILDDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "      AND GBN2 = '2' ";
                        SQL = SQL + ComNum.VBLF + "      AND NO1 = '" + strChasu + "'";
                        SQL = SQL + ComNum.VBLF + "      AND WARDCODE ='TO' ";
                        SQL = SQL + ComNum.VBLF + "GROUP BY NO1, BDATE, GBN, SUCODE ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        //자기 차수에  비치  수량 만큼만 빌드 처리
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     TO_CHAR(MAX(JDATE),'YYYY-MM-DD') AS JDATE, SUCODE, GBN ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG_SET ";
                        SQL = SQL + ComNum.VBLF + "   WHERE JDATE <=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND WARDCODE = '" + GstrWardCode + "' ";
                        SQL = SQL + ComNum.VBLF + "     AND GBN ='1'  ";  //마약
                        SQL = SQL + ComNum.VBLF + "GROUP BY SUCODE, GBN  ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for(i = 0; i < dt.Rows.Count; i++)
                            {
                                strGBn = dt.Rows[i]["GBN"].ToString().Trim();

                                SQL = "";
                                SQL = "SELECT QTY, SUCODE FROM " + ComNum.DB_MED + "OCS_DRUG_SET ";
                                SQL = SQL + ComNum.VBLF + "  WHERE JDATE <= TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "    AND WARDCODE ='" + GstrWardCode + "' ";
                                SQL = SQL + ComNum.VBLF + "    AND SUCODE = '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "ORDER BY  JDATE DESC ";

                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    dblQty = VB.Val(dt1.Rows[0]["QTY"].ToString().Trim());
                                    dblJQty = VB.Val(dt1.Rows[0]["QTY"].ToString().Trim());
                                    strSuCode = dt1.Rows[0]["SUCODE"].ToString().Trim();
                                }

                                dt1.Dispose();
                                dt1 = null;

                                SQL = "";
                                SQL = "SELECT UNITNEW1 FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                                SQL = SQL + ComNum.VBLF + "  WHERE SUNEXT = '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";

                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    dblUnit = VB.Val(dt1.Rows[0]["UNITNEW1"].ToString().Trim());
                                }

                                dt1.Dispose();
                                dt1 = null;

                                //당월제고
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                                SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, SUCODE, WARDCODE, QTY, REALQTY, NAL, BUILDDATE) ";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + "         '" + strGBn + "',";
                                SQL = SQL + ComNum.VBLF + "         '3',";
                                SQL = SQL + ComNum.VBLF + "         '" + strSuCode + "',";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                                SQL = SQL + ComNum.VBLF + "         '" + dblQty + "',";
                                SQL = SQL + ComNum.VBLF + "         '" + dblQty + "',";
                                SQL = SQL + ComNum.VBLF + "         '1',";
                                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                                SQL = SQL + ComNum.VBLF + "     )";

                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                                if (dblJQty != 0)
                                {
                                    SQL = "";
                                    SQL = "SELECT ROWID, (REALQTY * NAL) AS QTY FROM " + ComNum.DB_MED + "OCS_MAYAK ";
                                    SQL = SQL + ComNum.VBLF + "    WHERE BDATE >=TO_DATE('2015-06-17','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "      AND BDATE <= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "      AND WARDCODE ='" + GstrWardCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "      AND ( MDATE IS NULL OR TRUNC(MDATE) = TO_DATE('" + strDate + "','YYYY-MM-DD') )";
                                    SQL = SQL + ComNum.VBLF + "      AND ENTQTY > 0 ";
                                    SQL = SQL + ComNum.VBLF + "      AND SUCODE = '" + strSuCode.Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "      AND DOSCODE IN ";                    //내시경용법
                                    SQL = SQL + ComNum.VBLF + "                 (SELECT DOSCODE ";
                                    SQL = SQL + ComNum.VBLF + "                     FROM " + ComNum.DB_MED + "OCS_ODOSAGE ";
                                    SQL = SQL + ComNum.VBLF + "                 WHERE WARDCODE = '" + GstrWardCode + "')";  //내시경실;
                                    SQL = SQL + ComNum.VBLF + "      AND REALQTY * NAL <= '" + dblQty + "' ";
                                    SQL = SQL + ComNum.VBLF + "ORDER BY ENTDATE ";

                                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        return rtnVal;
                                    }
                                    //if (dt.Rows[i]["SUCODE"].ToString().Trim() = "N-PTD25")
                                    //{
                                    //    i = i;
                                    //}

                                    //if (dt.Rows[i]["SUCODE"].ToString().Trim() = "A-BASCA")
                                    //{
                                    //    i = i;
                                    //}

                                    if (dt1.Rows.Count > 0)
                                    {
                                        for(k = 0; k < dt1.Rows.Count; k++)
                                        {
                                            //기본 비치수량 보다 많이 소모 못하게함
                                            if (dblQty - VB.Val(dt1.Rows[k]["QTY"].ToString().Trim()) >= 0)
                                            {
                                                dblQty = dblQty - VB.Val(dt1.Rows[k]["QTY"].ToString().Trim());

                                                SQL = "";
                                                SQL = "UPDATE " + ComNum.DB_MED + "OCS_MAYAK";
                                                SQL = SQL + ComNum.VBLF + "     SET";
                                                SQL = SQL + ComNum.VBLF + "         MDATE = TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                                                SQL = SQL + ComNum.VBLF + "         CHASU = '" + strChasu + "'  ";
                                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[k]["ROWID"].ToString().Trim() + "' ";

                                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                                if (SqlErr != "")
                                                {
                                                    clsDB.setRollbackTran(clsDB.DbCon);
                                                    ComFunc.MsgBox(SqlErr);
                                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                    Cursor.Current = Cursors.Default;
                                                    return rtnVal;
                                                }

                                                //자료 형성
                                                SQL = "";
                                                SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                                                SQL = SQL + ComNum.VBLF + "     (NO1, BDATE ,GBN, GBN2, PTNO, SNAME, DEPTCODE, DRSABUN, IO, WARDCODE, ROOMCODE, SUCODE, QTY, REALQTY, NAL, DOSCODE, ORDERNO, ENTDATE, BUILDDATE, MDATE, SUCODER , ENTQTY2 ) ";
                                                SQL = SQL + ComNum.VBLF + "SELECT '" + strChasu + "' , BDATE, '" + strGBn + "', '2', PTNO, SNAME, DEPTCODE, DRSABUN, IO, '" + GstrWardCode + "', ROOMCODE, SUCODE, TRUNC(QTY+ 0.9), (ENTQTY / " + dblUnit + ") , NAL, DOSCODE, ORDERNO, ENTDATE, TO_DATE('" + strDate + "','YYYY-MM-DD') , MDATE, SUCODE, ENTQTY ";
                                                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_MED + "OCS_MAYAK A ";
                                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[k]["ROWID"].ToString().Trim() + "' ";

                                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                                if (SqlErr != "")
                                                {
                                                    clsDB.setRollbackTran(clsDB.DbCon);
                                                    ComFunc.MsgBox(SqlErr);
                                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                    Cursor.Current = Cursors.Default;
                                                    return rtnVal;
                                                }
                                            }
                                            else
                                            {
                                                ComFunc.MsgBox(strSuCode + " 비치 수량보다 소모량이 큽니다.");
                                                break;
                                            }
                                        }
                                    }

                                    dt1.Dispose();
                                    dt1 = null;
                                }
                            }
                        }

                        dt.Dispose();
                        dt = null;
                        break;
                }
                
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                SQL = SQL + ComNum.VBLF + "     (";

                switch (GstrWardCode)
                {
                    case "4H":  //호스피스
                    case "AG":  //혈관조영실
                    case "ER":  //응급의료센터
                    case "EN":
                    case "32":
                    case "33":
                    case "35":
                    case "JS":
                        SQL = SQL + "NO1, ";
                        break;
                }

                SQL = SQL + "BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY , NAL, ENTDATE, BUILDDATE )   ";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     ";

                switch (GstrWardCode)
                {
                    case "4H":  //호스피스
                    case "AG":  //혈관조영실
                    case "ER":  //응급의료센터
                    case "EN":
                    case "32":
                    case "33":
                    case "35":
                    case "JS":
                        SQL = SQL + "NO1, ";
                        break;
                }

                SQL = SQL + "TO_DATE('" + strDate + "','YYYY-MM-DD'), GBN ,'1',  '" + GstrWardCode + "', SUCODE, SUM(QTY *  NAL) ,  1, SYSDATE, TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG    ";
                SQL = SQL + ComNum.VBLF + " WHERE BUILDDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "      AND GBN2 = '2' ";

                switch (GstrWardCode)
                {
                    case "4H":  //호스피스
                    case "AG":  //혈관조영실
                    case "ER":  //응급의료센터
                    case "EN":
                    case "32":
                    case "33":
                    case "35":
                    case "JS":
                        SQL = SQL + ComNum.VBLF + "      AND NO1 = '" + strChasu + "'";
                        break;
                }

                if (GstrWardCode != "EN")
                {
                    SQL = SQL + ComNum.VBLF + "      AND WARDCODE ='" + GstrWardCode + "'  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "      AND WARDCODE ='TO'  ";
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY ";

                switch (GstrWardCode)
                {
                    case "4H":  //호스피스
                    case "AG":  //혈관조영실
                    case "ER":  //응급의료센터
                    case "EN":
                    case "32":
                    case "33":
                    case "35":
                    case "JS":
                        SQL = SQL + "NO1, ";
                        break;
                }

                SQL = SQL + "BDATE, GBN, SUCODE ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (GstrWardCode != "EN")
                {
                    //재고 집계 ----------------------------------------------------
                    //전일재고 + 입고 - 소모
                    //view use
                    SQL = "";
                    SQL = "CREATE OR REPLACE VIEW " + ComNum.DB_MED + "VIEW_OCS_DRUG";
                    SQL = SQL + ComNum.VBLF + "     ( ";

                    switch (GstrWardCode)
                    {
                        case "4H":  //호스피스
                        case "AG":  //혈관조영실
                        case "ER":  //응급의료센터
                        case "32":
                        case "33":
                        case "35":
                        case "JS":
                            SQL = SQL + "NO1, ";
                            break;
                    }

                    SQL = SQL + "GBN, SUCODE, QTY, WARDCODE ) AS ";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     ";

                    switch (GstrWardCode)
                    {
                        case "4H":  //호스피스
                        case "AG":  //혈관조영실
                        case "ER":  //응급의료센터
                        case "32":
                        case "33":
                        case "35":
                        case "JS":
                            SQL = SQL + "NO1, ";
                            break;
                    }

                    SQL = SQL + "GBN, SUCODE , SUM(QTY * NAL ), WARDCODE  ";
                    SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_MED + "OCS_DRUG ";
                    SQL = SQL + ComNum.VBLF + "   WHERE  GBN2 = '3' ";  //재고
                    SQL = SQL + ComNum.VBLF + "     AND BUILDDATE =TO_DATE('" + strDateB + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND WARDCODE ='" + GstrWardCode + "' ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY ";

                    switch (GstrWardCode)
                    {
                        case "4H":  //호스피스
                        case "AG":  //혈관조영실
                        case "ER":  //응급의료센터
                        case "32":
                        case "33":
                        case "35":
                        case "JS":
                            SQL = SQL + "NO1, ";
                            break;
                    }

                    SQL = SQL + "GBN,SUCODE , WARDCODE";
                    SQL = SQL + ComNum.VBLF + "UNION ALL ";
                    SQL = SQL + ComNum.VBLF + " SELECT";
                    SQL = SQL + ComNum.VBLF + "     ";

                    switch (GstrWardCode)
                    {
                        case "4H":  //호스피스
                        case "AG":  //혈관조영실
                        case "ER":  //응급의료센터
                        case "32":
                        case "33":
                        case "35":
                        case "JS":
                            SQL = SQL + "NO1, ";
                            break;
                    }

                    SQL = SQL + "GBN,SUCODE , SUM(QTY * NAL ), WARDCODE  ";
                    SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_MED + "OCS_DRUG ";
                    SQL = SQL + ComNum.VBLF + "   WHERE  GBN2 = '1' ";
                    SQL = SQL + ComNum.VBLF + "     AND TRUNC(BUILDDATE) =TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND WARDCODE ='" + GstrWardCode + "' ";

                    switch (GstrWardCode)
                    {
                        case "4H":  //호스피스
                        case "AG":  //혈관조영실
                        case "ER":  //응급의료센터
                        case "32":
                        case "33":
                        case "35":
                        case "JS":
                            SQL = SQL + ComNum.VBLF + "      AND NO1 = '" + strChasu + "'";
                            break;
                    }

                    SQL = SQL + ComNum.VBLF + "GROUP BY ";

                    switch (GstrWardCode)
                    {
                        case "4H":  //호스피스
                        case "AG":  //혈관조영실
                        case "ER":  //응급의료센터
                        case "32":
                        case "33":
                        case "35":
                        case "JS":
                            SQL = SQL + "NO1, ";
                            break;
                    }

                    SQL = SQL + "GBN, SUCODE , WARDCODE";
                    SQL = SQL + ComNum.VBLF + "UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     ";

                    switch (GstrWardCode)
                    {
                        case "4H":  //호스피스
                        case "AG":  //혈관조영실
                        case "ER":  //응급의료센터
                        case "32":
                        case "33":
                        case "35":
                        case "JS":
                            SQL = SQL + "NO1, ";
                            break;
                    }

                    SQL = SQL + "GBN, SUCODE , SUM(QTY* NAL) * -1 , WARDCODE ";
                    SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_MED + "OCS_DRUG ";
                    SQL = SQL + ComNum.VBLF + "   WHERE  GBN2 = '2' ";  //소모
                    SQL = SQL + ComNum.VBLF + "     AND BUILDDATE =TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "      AND WARDCODE ='" + GstrWardCode + "' ";

                    switch (GstrWardCode)
                    {
                        case "4H":  //호스피스
                        case "AG":  //혈관조영실
                        case "ER":  //응급의료센터
                        case "32":
                        case "33":
                        case "35":
                        case "JS":
                            SQL = SQL + ComNum.VBLF + "      AND NO1 = '" + strChasu + "'";
                            break;
                    }

                    SQL = SQL + ComNum.VBLF + "GROUP BY ";

                    switch (GstrWardCode)
                    {
                        case "4H":  //호스피스
                        case "AG":  //혈관조영실
                        case "ER":  //응급의료센터
                        case "32":
                        case "33":
                        case "35":
                        case "JS":
                            SQL = SQL + "NO1, ";
                            break;
                    }

                    SQL = SQL + "GBN, SUCODE , WARDCODE";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    //당월제고
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                    SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, SUCODE, WARDCODE, QTY, REALQTY, NAL , BUILDDATE ) ";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_DATE('" + strDate + "','YYYY-MM-DD'), GBN, '3', SUCODE, WARDCODE, SUM(QTY), SUM(QTY), '1', TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "VIEW_OCS_DRUG ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY  GBN, SUCODE, WARDCODE ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private bool SaveLog(string Bdate, string WardCode, string Chasu)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            if (VB.IsDate(Bdate) == false) return true;

            try
            {                
                //당월제고
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG_MAGAMLOG";
                SQL = SQL + ComNum.VBLF + "     (BDATE, WARDCODE, CHASU, BUILD, SABUN) ";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         '" + Convert.ToDateTime(Bdate).ToString("yyyyMMdd") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + WardCode + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + Chasu + "', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE,";
                SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "' ";
                SQL = SQL + ComNum.VBLF + "     )";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {                    
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool READ_OCS_ACTING(string strDate)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            int i = 0;

            bool rtnVal = false;

            string SqlErr = "";
            int intRowAffected = 0;

            string strOK = "";
            string strActTime = "";

            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ROWID, ORDERNO, PTNO, BDATE, SUCODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MAYAK ";
                SQL = SQL + ComNum.VBLF + "  WHERE BDATE_R >=TO_DATE('2015-12-10','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE_R <=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                
                switch(GstrWardCode)
                {
                    case "4H":
                    case "32":
                    case "33":
                    case "35":
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE ='" + GstrWardCode + "'  "; //ICU
                        break;
                    case "ER":
                        SQL = SQL + ComNum.VBLF + "    AND ( WARDCODE ='" + GstrWardCode + "' OR (WARDCODE IS NULL AND DEPTCODE ='" + GstrWardCode + "') )"; //ICU
                        break;
                }

                SQL = SQL + ComNum.VBLF + "    AND ORDERNO >0 ";
                SQL = SQL + ComNum.VBLF + "    AND MDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND ACTTIME IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND DOSCODE NOT IN  ";                    //내시경용법제외
                SQL = SQL + ComNum.VBLF + "          (SELECT DOSCODE ";
                SQL = SQL + ComNum.VBLF + "             FROM ADMIN.OCS_ODOSAGE ";
                SQL = SQL + ComNum.VBLF + "           WHERE WARDCODE = 'EN') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY ENTDATE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     ROWID, TO_CHAR(ACTTIME,'YYYY-MM-DD HH24:MI') ACTTIME   ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER_ACT ";
                        SQL = SQL + ComNum.VBLF + "  WHERE BDATE <=TO_DATE('" + Convert.ToDateTime(dt.Rows[i]["BDate"].ToString().Trim()).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "    AND PTNO = '" + dt.Rows[i]["PtNo"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND ORDERNO = '" + dt.Rows[i]["ORDERNO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND SUCODE ='" + dt.Rows[i]["Sucode"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY ACTTIME ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }

                        strOK = "N";

                        if (dt1.Rows.Count > 0)
                        {
                            strOK = "Y";
                            strActTime = dt1.Rows[0]["ACTTIME"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strOK == "Y")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_MAYAK";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         ACTTIME = TO_DATE('" + strActTime + "','YYYY-MM-DD HH24:MI')  ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ROWID,  ORDERNO, PTNO, BDATE, SUCODE   ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_HYANG ";

                switch(GstrWardCode)
                {
                    case "4H":
                    case "32":
                    case "33":
                    case "35":
                        SQL = SQL + ComNum.VBLF + "  WHERE BDATE >=TO_DATE('2015-12-10','YYYY-MM-DD') ";
                        break;
                    case "ER":
                        SQL = SQL + ComNum.VBLF + "  WHERE BDATE >=TO_DATE('2015-11-10','YYYY-MM-DD') ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "     AND BDATE <=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND WARDCODE ='" + GstrWardCode + "'  "; //ICU
                SQL = SQL + ComNum.VBLF + "     AND ORDERNO >0 ";
                SQL = SQL + ComNum.VBLF + "     AND MDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "     AND ACTTIME IS NULL ";
                SQL = SQL + ComNum.VBLF + "     AND DOSCODE NOT IN ";                    //내시경용법제외
                SQL = SQL + ComNum.VBLF + "                 (SELECT DOSCODE ";
                SQL = SQL + ComNum.VBLF + "                     FROM ADMIN.OCS_ODOSAGE ";
                SQL = SQL + ComNum.VBLF + "                 WHERE WARDCODE = 'EN') ";  //내시경실 제외
                SQL = SQL + ComNum.VBLF + "ORDER BY ENTDATE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     ROWID, TO_CHAR(ACTTIME,'YYYY-MM-DD HH24:MI') ACTTIME";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER_ACT ";
                        SQL = SQL + ComNum.VBLF + " WHERE BDATE <= TO_DATE('" + Convert.ToDateTime(dt.Rows[i]["BDate"].ToString().Trim()).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND PTNO = '" + dt.Rows[i]["PtNo"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND ORDERNO = '" + dt.Rows[i]["ORDERNO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE = '" + dt.Rows[i]["Sucode"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY ACTTIME ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }

                        strOK = "N";

                        if (dt1.Rows.Count > 0)
                        {
                            strOK = "Y";
                            strActTime = dt1.Rows[0]["ACTTIME"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strOK == "Y")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_HYANG";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         ACTTIME = TO_DATE('" + strActTime + "','YYYY-MM-DD HH24:MI')  ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private bool Endo_Hyang_cnt(string SQL, int intRowAffected, string SqlErr)
        {
            DataTable dt = null;
            DataTable dt1 = null;

            string strJDate = dtpDate.Value.ToString("yyyy-MM-dd");
            string strGbn = "";
            string strFlag = "";

            double dblContent = 0;
            int i = 0;

            //향정
            SQL = "";
            SQL = "UPDATE " + ComNum.DB_MED + "OCS_MAYAK";
            SQL = SQL + ComNum.VBLF + "     SET";
            SQL = SQL + ComNum.VBLF + "         ENTQTY = '',";
            SQL = SQL + ComNum.VBLF + "         MDATE = '' ";
            SQL = SQL + ComNum.VBLF + "WHERE MDATE = TO_DATE('" + strJDate + "' ,'YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='" + GstrWardCode + "' ";

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            //마약
            SQL = "";
            SQL = "UPDATE " + ComNum.DB_MED + "OCS_HYANG";
            SQL = SQL + ComNum.VBLF + "     SET";
            SQL = SQL + ComNum.VBLF + "         ENTQTY = '',";
            SQL = SQL + ComNum.VBLF + "         MDATE ='' ";
            SQL = SQL + ComNum.VBLF + "WHERE MDATE = TO_DATE('" + strJDate + "' ,'YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND WARDCODE = '" + GstrWardCode + "' ";

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            //OCS_DRUG_SET
            //마약 향정 구분 추가
            SQL = "";
            SQL = "UPDATE " + ComNum.DB_MED + "ENDO_HYANG_CNT";
            SQL = SQL + ComNum.VBLF + "     SET";
            SQL = SQL + ComNum.VBLF + "         GBN = '1' ";
            SQL = SQL + ComNum.VBLF + "WHERE RDATE = TO_DATE('" + strJDate + "' ,'YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND ORDERCODE IN";
            SQL = SQL + ComNum.VBLF + "             (SELECT SUCODE FROM " + ComNum.DB_MED + "OCS_DRUG_SET";
            SQL = SQL + ComNum.VBLF + "                 WHERE WARDCODE = '" + GstrWardCode + "'";
            SQL = SQL + ComNum.VBLF + "                 AND GBN = '1')";

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            SQL = "";
            SQL = "UPDATE " + ComNum.DB_MED + "ENDO_HYANG_CNT";
            SQL = SQL + ComNum.VBLF + "     SET";
            SQL = SQL + ComNum.VBLF + "         GBN = '2' ";
            SQL = SQL + ComNum.VBLF + "WHERE RDATE = TO_DATE('" + strJDate + "' ,'YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND ORDERCODE IN";
            SQL = SQL + ComNum.VBLF + "             (SELECT SUCODE FROM " + ComNum.DB_MED + "OCS_DRUG_SET";
            SQL = SQL + ComNum.VBLF + "                 WHERE WARDCODE = '" + GstrWardCode + "'";
            SQL = SQL + ComNum.VBLF + "                 AND GBN = '2')";

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            SQL = "";
            SQL = "SELECT";
            SQL = SQL + ComNum.VBLF + "     PTNO, RDATE, JDATE,  ORDERCODE, ORDERNO , GBN, CNT, NVL(ENTQTY, CONTENT) AS CONTENT ";  //단위 ml임.
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "ENDO_HYANG_CNT ";
            SQL = SQL + ComNum.VBLF + "     WHERE RDATE = TO_DATE('" + strJDate + "' ,'YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "ORDER BY GBN ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }
            if (dt.Rows.Count > 0)
            {
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    strGbn = dt.Rows[i]["GBN"].ToString().Trim();
                    
                    dblContent = VB.Val(dt.Rows[i]["CONTENT"].ToString().Trim());

                    strFlag = "N";

                    if (strGbn == "2")
                    {
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     DEPTCODE, ROWID";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_HYANG ";
                        SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + dt.Rows[i]["PTNO"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "         AND SUCODE ='" + dt.Rows[i]["ORDERCODE"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND WARDCODE IN ('TO','HR','" + GstrWardCode + "') ";
                            
                        if (VB.Val(dt.Rows[i]["ORDERNO"].ToString().Trim()) > 0)
                        {
                            SQL = SQL + ComNum.VBLF + "         AND BDATE <=TO_DATE('" + Convert.ToDateTime(dt.Rows[i]["RDate"].ToString().Trim()).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "         AND ORDERNO ='" + dt.Rows[i]["ORDERNO"].ToString().Trim() + "' ";
                        }

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return false;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_HYANG";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         ENTQTY = '" + dblContent + "',";
                            SQL = SQL + ComNum.VBLF + "         CERTNO = NULL";

                            if (dt1.Rows[0]["DeptCode"].ToString().Trim() == "TO" || dt1.Rows[0]["DeptCode"].ToString().Trim() == "HR")
                            {
                                SQL = SQL + ComNum.VBLF + "         , QTY= REALQTY  ";
                            }

                            if (dt1.Rows[0]["DeptCode"].ToString().Trim() != "EN")
                            {
                                SQL = SQL + ComNum.VBLF + "         , WARDCODE = '" + GstrWardCode + "' "; // , DOSCODE ='920103' "
                            }


                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }

                            strFlag = "Y";
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strFlag == "N")
                        {
                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     DEPTCODE, ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_HYANG ";
                            SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + dt.Rows[i]["PtNo"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "         AND BDATE = TO_DATE('" + Convert.ToDateTime(dt.Rows[i]["RDate"].ToString().Trim()).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "         AND SUCODE = '" + dt.Rows[i]["OrderCode"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND WARDCODE IN ('TO','HR','" + GstrWardCode + "') ";

                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return false;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_MED + "OCS_HYANG";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         ENTQTY = '" + dblContent + "',";
                                SQL = SQL + ComNum.VBLF + "         CERTNO = NULL ";

                                if (dt1.Rows[0]["DeptCode"].ToString().Trim() == "TO" || dt1.Rows[0]["DeptCode"].ToString().Trim() == "HR")
                                {
                                    SQL = SQL + ComNum.VBLF + "         , QTY= REALQTY  ";
                                }

                                if (dt1.Rows[0]["DeptCode"].ToString().Trim() != "EN")
                                {
                                    SQL = SQL + ComNum.VBLF + "         , WARDCODE = 'EN' "; //   , DOSCODE ='920103' "
                                }
                                
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID ='" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";

                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }

                                strFlag = "Y";
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    if (strGbn == "1")
                    {
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     DEPTCODE, ROWID";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MAYAK ";
                        SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + dt.Rows[i]["PtNo"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "         AND BDATE <= TO_DATE('" + Convert.ToDateTime(dt.Rows[i]["RDate"].ToString().Trim()).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "         AND SUCODE ='" + dt.Rows[i]["OrderCode"].ToString().Trim() + "' ";

                        if (VB.Val(dt.Rows[i]["ORDERNO"].ToString().Trim()) > 0)
                        {
                            SQL = SQL + ComNum.VBLF + "         AND ORDERNO ='" + dt.Rows[i]["ORDERNO"].ToString().Trim() + "' ";
                        }

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return false;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_MAYAK";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         ENTQTY = '" + dblContent + "',";
                            SQL = SQL + ComNum.VBLF + "         CERTNO = NULL ";

                            if (dt1.Rows[0]["DeptCode"].ToString().Trim() == "TO" || dt1.Rows[0]["DeptCode"].ToString().Trim() == "HR")
                            {
                                SQL = SQL + ComNum.VBLF + " ,  QTY= REALQTY  ";
                            }

                            if (dt1.Rows[0]["DeptCode"].ToString().Trim() != "EN")
                            {
                                SQL = SQL + ComNum.VBLF + " ,  WARDCODE = 'EN' ";     //, DOSCODE ='920103' "
                            }

                            SQL = SQL + " WHERE ROWID ='" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }

                            strFlag = "Y";
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strFlag == "N")
                        {
                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     DEPTCODE, ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MAYAK ";
                            SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + dt.Rows[i]["PtNo"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "         AND BDATE_R =TO_DATE('" + Convert.ToDateTime(dt.Rows[i]["JDATE"].ToString().Trim()).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "         AND SUCODE ='" + dt.Rows[i]["OrderCode"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND (WARDCODE IN ('TO','HR','EN') OR DEPTCODE IN ('TO','HR') )";
                            SQL = SQL + ComNum.VBLF + "         AND ORDERNO  IS NULL ";

                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return false;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_MED + "OCS_MAYAK";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         ENTQTY = '" + dblContent + "',";
                                SQL = SQL + ComNum.VBLF + "         CERTNO = NULL";

                                if (dt1.Rows[0]["DeptCode"].ToString().Trim() == "TO" || dt1.Rows[0]["DeptCode"].ToString().Trim() == "HR")
                                {
                                    SQL = SQL + ComNum.VBLF + "         , QTY= REALQTY  ";
                                }
                                if (dt1.Rows[0]["DeptCode"].ToString().Trim() != "EN")
                                {
                                    SQL = SQL + ComNum.VBLF + "         , WARDCODE = 'EN' ";  //, DOSCODE ='920103' "
                                }

                                SQL = SQL + ComNum.VBLF + "         WHERE ROWID ='" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";

                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }

                                strFlag = "Y";
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
                }
            }

            dt.Dispose();
            dt = null;

            return true;
        }

        private bool TOMAGAN_RTN(string strChasu)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            DataTable dt = null;
            DataTable dt1 = null;
            int i = 0;
            int k = 0;

            string strDate = dtpDate.Value.ToString("yyyy-MM-dd");
            string strDateB = dtpDate.Value.AddDays(-1).ToString("yyyy-MM-dd");

            double dblQty = 0;
            double dblJQty = 0;

            string strSuCode = "";

            //기존자료 삭제
            SQL = "";
            SQL = "DELETE " + ComNum.DB_MED + "OCS_DRUG ";
            SQL = SQL + ComNum.VBLF + "   WHERE BUILDDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "     AND WARDCODE = '" + GstrWardCode + "' ";
            SQL = SQL + ComNum.VBLF + "     AND GBN2 IN ('3') ";        //당일제고 (차수별 이라서 ) 당일제고는 항상 삭제 후 생성

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            //자기 차수에  비치  수량 만큼만 빌드 처리
            SQL = "";
            SQL = "SELECT TO_CHAR(MAX(JDATE) ,'YYYY-MM-DD') JDATE , SUCODE  ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_DRUG_SET ";
            SQL = SQL + ComNum.VBLF + "   WHERE JDATE <= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "     AND WARDCODE = '" + GstrWardCode + "' ";
            SQL = SQL + ComNum.VBLF + "     AND GBN ='2'  ";  //향정
            SQL = SQL + ComNum.VBLF + "   GROUP BY  SUCODE  ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SQL = "";
                    SQL = "SELECT QTY, SUCODE FROM " + ComNum.DB_MED + "OCS_DRUG_SET ";
                    SQL = SQL + ComNum.VBLF + "  WHERE JDATE <= TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND WARDCODE ='" + GstrWardCode + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND SUCODE = '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY  JDATE DESC ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        dblQty = VB.Val(dt1.Rows[0]["QTY"].ToString().Trim());
                        dblJQty = VB.Val(dt1.Rows[0]["QTY"].ToString().Trim());
                        strSuCode = dt1.Rows[0]["SUCODE"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //당월제고
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                    SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, SUCODE, WARDCODE, QTY, REALQTY, NAL, BUILDDATE) ";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + "         '2',";
                    SQL = SQL + ComNum.VBLF + "         '3',";
                    SQL = SQL + ComNum.VBLF + "         '" + strSuCode + "',";
                    SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                    SQL = SQL + ComNum.VBLF + "         '" + dblQty + "',";
                    SQL = SQL + ComNum.VBLF + "         '" + dblQty + "',";
                    SQL = SQL + ComNum.VBLF + "         '1',";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dblJQty != 0)
                    {
                        SQL = "";
                        SQL = "SELECT ROWID,  REALQTY QTY, ENTQTY2 FROM " + ComNum.DB_PMPA + "HIC_HYANG_APPROVE ";
                        SQL = SQL + ComNum.VBLF + "    WHERE BDATE =TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "      AND SUCODE = '" + strSuCode.Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "      AND CHASU ='" + strChasu + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND ENTQTY2 <> 0 ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            //기본 비치수량 보다 많이 집계(소모) 못하게함
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                if (dblQty - VB.Val(dt1.Rows[k]["QTY"].ToString().Trim()) >= 0)
                                {
                                    dblQty = dblQty - VB.Val(dt1.Rows[k]["QTY"].ToString().Trim());

                                    SQL = "";
                                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                                    SQL = SQL + ComNum.VBLF + "     (NO1, BDATE, GBN, GBN2, PTNO, SNAME, DEPTCODE, DRSABUN, IO, WARDCODE, ROOMCODE, SUCODE, QTY, REALQTY, NAL, DOSCODE, ORDERNO, ENTDATE, BUILDDATE, MDATE, SUCODER, ENTQTY2) ";
                                    SQL = SQL + ComNum.VBLF + "SELECT";
                                    SQL = SQL + ComNum.VBLF + "     '" + strChasu + "', BDATE, '2', '2', PTNO, SNAME, DEPTCODE, DRSABUN, 'O', '" + GstrWardCode + "', '', SUCODE, '1', ENTQTY2 / QTY , NAL, DOSCODE,'', ENTDATE, BDATE, BDATE, SUCODE , ENTQTY2 ";
                                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "HIC_HYANG_APPROVE ";
                                    SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + dt1.Rows[k]["ROWID"].ToString().Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "       AND CHASU = '" + strChasu + "' ";

                                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }
            }

            dt.Dispose();
            dt = null;

            SQL = "";
            SQL = "INSERT INTO ADMIN.OCS_DRUG";
            SQL = SQL + ComNum.VBLF + "     (NO1, BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY , NAL, ENTDATE, BUILDDATE )   ";
            SQL = SQL + ComNum.VBLF + "SELECT";
            SQL = SQL + ComNum.VBLF + "     NO1, TO_DATE('" + strDate + "','YYYY-MM-DD'), '2' ,'1',  'TO', SUCODE, SUM(QTY *  NAL) ,  1, SYSDATE, TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG    ";
            SQL = SQL + ComNum.VBLF + "    WHERE BUILDDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "      AND GBN2 = '2' ";
            SQL = SQL + ComNum.VBLF + "      AND NO1 = '" + strChasu + "'";
            SQL = SQL + ComNum.VBLF + "      AND WARDCODE ='TO' ";
            SQL = SQL + ComNum.VBLF + "    GROUP BY NO1, BDATE, GBN, SUCODE ";

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            rtnVal = true;
            return rtnVal;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void btnBuild2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            this.Enabled = false;

            if (SaveData("2") == true)
            {
                ComFunc.MsgBox("비상마약류 자료형성 완료");
                rEventClosed();
            }

            this.Enabled = true;
        }

    }
}
