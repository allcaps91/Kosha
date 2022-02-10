using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using ComDbB;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibe
    /// File Name       : frmDur.cs
    /// Description     : DUR 자동 동기화
    /// Author          : 김해수
    /// Create Date     : 2021-04-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "\drug\dur\dur02.frm(FrmDur) >> frmDur.cs 폼이름 재정의" /> 
    /// 
    public partial class frmDur : Form
    {
        public static HiraDur.IHIRAClient DurClient = default(HiraDur.IHIRAClient);
        SQLiteConnection conn = null;
        SQLiteCommand cmd = null;
        SQLiteCommand cnt = null;
        SQLiteDataReader Rdr = null;

        string FstrPath = "";

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();
        PsmhDb dbCon = new PsmhDb();

        string SQL = "";
        string SqlErr = "";
        int intRowAffected = 0;
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;
        DataTable dt3 = null;
        DataTable rs = null;

        #region //MainFormMessage
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
        #endregion

        public frmDur()
        {
            InitializeComponent();
            setEvent();
        }

        public frmDur(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        void setCtrlData()
        {
            setCombo();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            ////탭버튼클릭 이벤트
            //this.tab1.Click += new EventHandler(eTabClick);
            //this.tab2.Click += new EventHandler(eTabClick);
            //this.tab3.Click += new EventHandler(eTabClick);

            this.btnConnect.Click += new EventHandler(eBtnClick);
            //this.btnSearch.Click += new EventHandler(eBtnSearch);
            //this.btnSearch2.Click += new EventHandler(eBtnSearch);
            //this.btnSearch5.Click += new EventHandler(eBtnSearch);
            //this.btnSearch6.Click += new EventHandler(eBtnSearch);
            //this.btnSearch8.Click += new EventHandler(eBtnSearch);
            //this.btnSearch9.Click += new EventHandler(eBtnSearch);

            //this.btnSave1.Click += new EventHandler(eBtnSave);
            //this.btnSave2.Click += new EventHandler(eBtnSave);
            //this.btnSave3.Click += new EventHandler(eBtnSave);
            //this.btnSave5.Click += new EventHandler(eBtnSave);
            //this.btnSave_Session.Click += new EventHandler(eBtnSave);
            //this.btnMemo.Click += new EventHandler(eBtnSave);

            //this.btnPrint.Click += new EventHandler(eBtnPrint);  //스프레드 방식 출력
            //this.btnPrint2.Click += new EventHandler(eBtnPrint);
            //this.btnPrint3.Click += new EventHandler(eBtnPrint); //Print.print 출력방식

            ////명단 더블클릭 이벤트
            //this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            //this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            ////this.ssList.MouseUp += new MouseEventHandler(eMouseEvent);
            //this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            //this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);
            //this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);
            //this.ssList.EditModeOff += new EventHandler(eSpreadEditOff);

            //this.cboGubun.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            //this.cboPart.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            //this.cboAmPm.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            //this.cboIO.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            //this.cboSleep.SelectedIndexChanged += new EventHandler(eCboSelChanged);

            //this.txtSearch.KeyDown += new KeyEventHandler(eTxtKeyDown);
            //this.dtpDate.ValueChanged += new EventHandler(eDtpValueChanged);


        }

        void setCombo()
        {
          
        }

        void setTxtTip()
        {

        }

        void setCtrlInit()
        {
           
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                //툴팁
                setTxtTip();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();

                //Call FormInfo_History(Me.Name, Me.Caption)  폼로딩 사용 빈도   

                FstrPath = "C:\\HIRA\\IHIRADUR\\";

                FileInfo fileInfo = new FileInfo(FstrPath + "M.DB");
                if (fileInfo.Exists)
                {
                    System.Type DurClient_t = System.Type.GetTypeFromProgID("HiraDur.Client", true);
                    DurClient = (HiraDur.IHIRAClient)System.Activator.CreateInstance(DurClient_t, true);
                }
                else
                {
                    ComFunc.MsgBox("설정 파일( C:\\HIRA\\IHIRADUR\\M.DB 가 없습니다.","파일확인");
                }

                this.Refresh();

                lblButtom.Text = "";

                read_sysdate();



               
                timer1.Interval = 60000; //타이머 간격을 60초로 준 것이다.
                //timer1.Interval = 1000; //타이머 간격을 1초로 준 것이다.
                timer1_Tick(sender, e);
                timer1.Start(); //타이머를 발동시킨다.
            }
        }

        void eFormResize(object sender, EventArgs e)
        {
            
        }

        void eFormActivated(object sender, EventArgs e)
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

        void eFormClosed(object sender, FormClosedEventArgs e)
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

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnConnect)  
            {
                //DBUpdate_Click();
                string strOK = "";
                strOK = HIRA_UPDATE();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string strOK = "";

            if(cpublic.strSysTime.CompareTo("12:30:00") > 0 && cpublic.strSysTime.CompareTo("13:00:00") < 0)
            {
                read_sysdate();

                timer1.Stop();
                //작동
                SQL = " SELECT HIRAJOB , ROWID FROM ADMIN.BAS_JOB " + ComNum.VBLF;
                SQL += " WHERE JOBDATE = TRUNC(SYSDATE)                 " + ComNum.VBLF;
       
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);    //전체 조회자료 

                if(dt.Rows.Count > 0)
                {
                    if(dt.Rows[0]["HIRAJOB"].ToString().Trim() != "*")
                    {
                        strOK = HIRA_UPDATE();

                        if(strOK == "OK")
                        {
                            SQL = " UPDATE ADMIN.BAS_JOB SET HIRAJOB ='*' WHERE JOBDATE =TRUNC(SYSDATE) " + ComNum.VBLF;

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("HIRA 업데이트 오류", "확인");
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                timer1.Start();
            }

         
        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        private string HIRA_UPDATE()
        {
            string rtnVal = "OK";

            if(DUR_CONNECT() == "NO")
            {
                ComFunc.MsgBox("동기화 실패", "실패");
                rtnVal = "NO";
                return rtnVal;
            }
            else
            {
                lblButtom.Text = "연결완료";
                Application.DoEvents();
            }

            string strSDate = "";
            string strEDate = "";

            strSDate = cpublic.strSysDate;
            strEDate = cpublic.strSysDate;

            //Check 파일 존재 여부 
            HIRA_DUR_DBINSERT("TBJBD40");

            HIRA_DUR_DBINSERT("TBJBD43");

            HIRA_DUR_DBINSERT("TBJBD44");

            HIRA_DUR_DBINSERT("TBJBD46");

            HIRA_DUR_DBINSERT("TBJBD47");

            HIRA_DUR_DBINSERT("TBJBD48");

            HIRA_DUR_DBINSERT("TBJBD52");

            HIRA_DUR_DBINSERT("TBJBD55");

            HIRA_DUR_DBINSERT("TBJBD56");

            HIRA_DUR_DBINSERT("TBJBD63");

            HIRA_DUR_DBINSERT("TBDUD230");

            BAS_MSELF_RTN();
            BAS_MSELF_RTN2();

            strEDate = cpublic.strSysDate;

            lblButtom.Text = strSDate + " ~ " + strEDate + " 작업 완료 !";
            Application.DoEvents();

            return rtnVal;
        }

        private string DUR_CONNECT()
        {
            string rtnVal = "NO";

            int nResult = 0;

            nResult = DurClient.SyncDB("37100068");

            if(nResult == 0 || nResult == 16000)
            {

            }
            else
            {
                rtnVal = "NO";
                return rtnVal;
            }

            rtnVal = "OK";
            return rtnVal;
        }

        void HIRA_DUR_DBINSERT(string ArgTABLE)
        {
            int count = 0;
            string strOK = "";

            //TBJBD40-------------------------------------
            string strARTCNM = "";
            string strNOM = "";
            string strUNIT = "";
            string strMNF_CO_NM = "";
            string strMDC_CORS = "";
            string strPAY_TYPE = "";
            string strUN_COST = "";
            string strMAX_COST = "";

            //TBJBD43-------------------------------------
            string strDUR_CD_A = "";
            string strDUR_CD_B = "";
            string strADPT_FR_DT = "";
            string strADPT_TO_DT = "";
            string strADPT_TYPE = "";
            string strDIV_CD3 = "";
            string strINCOMP_MEDC_DT = "";
            string strUNIT_TYPE = "";
            string DUR_SD_EFT = "";
            string BEF_CMPT_CD = "";
            string SPC_AGE = "";
            string SPC_AGE_UNIT = "";
            string Sex = "";
            string EXM_RNG_TP = "";
            string EXM_TYPE = "";
            string OFFR_MSG = "";
            string ANNCE_DT = "";
            string ANNCE_NO = "";
            string EXCEP_DD_CNT = "";

            //TBJBD44-------------------------------------
            string strGNL_NM_CD = "";
            string strSPC_AGE = "";
            string strSPC_AGE_UNIT = "";
            string AGE_SD_EFT = "";
            string AGE_PRS_CND_CD = "";

            //TBJBD46-------------------------------------
            string strMEDC_CD = "";

            //TBJBD47-------------------------------------

            //TBJBD48-------------------------------------
            string strMEDC_INF_TYPE = "";

            //TBJBD52-------------------------------------
            string strLOW_IQTY_MEDC_CD = "";
            string strADPT_STAT_TYPE = "";
            string strUNIT_CNT = "";
            string strHIGH_IQTY_MEDC_CD = "";

            //TBJBD55-------------------------------------
            string strGUBUN_CD = "";
            string strDD_MAX_QTY_FREQ = "";
            string strMAX_MDC_TERM = "";
            string strANNCE_NO = "";
            string strANNCE_DT = "";

            //TBJBD63-------------------------------------
            string strELMT_CD = "";
            string strEXAM_TYPE = "";
            string strCONTRAD_GRADE = "";
            string strINCOMP_REASON = "";

            //TBJDB230-------------------------------------
            string EXM_KND_CD = "";
            string INFM_CD = "";
            string ADT_STA_DD = "";
            string ADT_END_DD = "";
            string ADT_TP_CD = "";
            string CND_CD = "";
            string RMK = "";

            string strSQLLite = "";
            //string strRowid = "";

            strOK = "OK";

            lblButtom.Text = ArgTABLE + " 동기화중 시간: "+ cpublic.strSysTime;
            Application.DoEvents();

            switch (ArgTABLE)
            {
                case "TBJBD40":
                    strSQLLite = " SELECT ALL  MEDC_CD, ADPT_FR_DT, ADPT_TO_DT, ARTCNM, NOM, UNIT, MNF_CO_NM, MDC_CORS, PAY_TYPE, UN_COST, MAX_COST, GNL_NM_CD  FROM " + ArgTABLE;
                    break;
                case "TBJBD43":
                    TRUNCATE_TALBE(ArgTABLE);
                    strSQLLite = "SELECT ALL DUR_CD_A, DUR_CD_B, ADPT_FR_DT, ADPT_TO_DT, ADPT_TYPE, DIV_CD3, INCOMP_MEDC_DT, UNIT_TYPE, DUR_SD_EFT, BEF_CMPT_CD, SPC_AGE, SPC_AGE_UNIT, SEX, EXM_RNG_TP, EXM_TYPE, OFFR_MSG, ANNCE_DT, ANNCE_NO, EXCEP_DD_CNT FROM " + ArgTABLE;
                    break;
                case "TBJBD44":
                    TRUNCATE_TALBE(ArgTABLE);
                    strSQLLite = " SELECT ALL GNL_NM_CD, ADPT_FR_DT, ADPT_TO_DT, SPC_AGE, SPC_AGE_UNIT, ADPT_TYPE, AGE_SD_EFT, EXM_TYPE,  OFFR_MSG , ANNCE_DT , ANNCE_NO , AGE_PRS_CND_CD FROM " + ArgTABLE;
                    break;
                case "TBJBD46":
                    TRUNCATE_TALBE(ArgTABLE);
                    strSQLLite = " SELECT ALL DUR_CD_A, DUR_CD_B, ADPT_FR_DT, ADPT_TO_DT, MEDC_CD, DIV_CD3 FROM " + ArgTABLE;
                    break;
                case "TBJBD47":
                    TRUNCATE_TALBE(ArgTABLE);
                    strSQLLite = " SELECT ALL GNL_NM_CD, ADPT_FR_DT, ADPT_TO_DT, SPC_AGE, SPC_AGE_UNIT, MEDC_CD FROM " + ArgTABLE;
                    break;
                case "TBJBD48":
                    TRUNCATE_TALBE(ArgTABLE);
                    strSQLLite = " SELECT ALL MEDC_INF_TYPE , MEDC_CD, ADPT_TYPE, ADPT_FR_DT, ADPT_TO_DT FROM " + ArgTABLE;
                    break;
                case "TBJBD52":
                    TRUNCATE_TALBE(ArgTABLE);
                    strSQLLite = " SELECT ALL LOW_IQTY_MEDC_CD, ADPT_STAT_TYPE, ADPT_FR_DT, UNIT_CNT, ADPT_TO_DT, HIGH_IQTY_MEDC_CD FROM " + ArgTABLE;
                    break;
                case "TBJBD55":
                    TRUNCATE_TALBE(ArgTABLE);
                    strSQLLite = " SELECT ALL TYPE_CD, GNL_NM_CD, ADPT_FR_DT, ADPT_TO_DT, MAX_QTY_CONVERT_EXPN, MAX_MDC_TERM, ADPT_TYPE,  ANNCE_DT, ANNCE_NO  FROM " + ArgTABLE;
                    break;
                case "TBJBD56":
                    TRUNCATE_TALBE(ArgTABLE);
                    strSQLLite = " SELECT ALL TYPE_CD, GNL_NM_CD, MEDC_CD, ADPT_FR_DT, ADPT_TO_DT FROM " + ArgTABLE;
                    break;
                case "TBJBD63":
                    TRUNCATE_TALBE(ArgTABLE);
                    strSQLLite = " SELECT ALL ELMT_CD, ADPT_FR_DT, ADPT_TO_DT, EXAM_TYPE, CONTRAD_GRADE, ADPT_TYPE, INCOMP_REASON FROM " + ArgTABLE;
                    break;
                case "TBDUD230":
                    TRUNCATE_TALBE(ArgTABLE);
                    strSQLLite = " SELECT ALL EXM_KND_CD,   INFM_CD,   ADT_STA_DD ,   ADT_END_DD ,   ADT_TP_CD ,   OFFR_MSG,   CND_CD,  RMK  FROM " + ArgTABLE;
                    break;
            }

            if(ArgTABLE == "TBJBD40")
            {
                conn = new SQLiteConnection("Data Source=" + FstrPath + "medi.db;Version=3;");
                conn.Open();
            }
            else
            {
                conn = new SQLiteConnection("Data Source=" + FstrPath + "m.db;Version=3;");
                conn.Open();
            }

            cmd = new SQLiteCommand(strSQLLite, conn);
            Rdr = cmd.ExecuteReader();

            cnt = new SQLiteCommand("select count(*) from " + ArgTABLE, conn);
            int RowCount = 0;
            RowCount = Convert.ToInt32(cnt.ExecuteScalar());

            pgbar.Minimum = count;
            pgbar.Maximum = RowCount;
            pgbar.Value = pgbar.Minimum;

            while (Rdr.Read())
            {
                switch (ArgTABLE)
                {
                    case "TBJBD40":
                        strMEDC_CD = Rdr["MEDC_CD"].ToString().Trim();           //약품코드
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();     //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();     //적용종료일자
                        strARTCNM = Rdr["ARTCNM"].ToString().Trim();             //약품명
                        strNOM = Rdr["NOM"].ToString().Trim();                   //약품명
                        strUNIT = Rdr["UNIT"].ToString().Trim();                 //UNIT
                        strMNF_CO_NM = VB.Replace(Rdr["MNF_CO_NM"].ToString(), "'", "").Trim();       //제약회사

                        strMDC_CORS = Rdr["MDC_CORS"].ToString().Trim();

                        strPAY_TYPE = Rdr["PAY_TYPE"].ToString().Trim();
                        strUN_COST = Rdr["UN_COST"].ToString().Trim();
                        strMAX_COST = Rdr["MAX_COST"].ToString().Trim();
                        strGNL_NM_CD = Rdr["GNL_NM_CD"].ToString().Trim();       //일반명코드

                        #region INSERT_RTN_TBJBD40

                        SQL = " SELECT MEDC_CD FROM HIRA_TBJBD40 WHERE MEDC_CD ='" + strMEDC_CD + "'" + ComNum.VBLF;
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (dt1.Rows.Count == 0)
                        {
                            SQL = " INSERT INTO ADMIN.HIRA_TBJBD40(MEDC_CD, ADPT_FR_DT, ADPT_TO_DT, ARTCNM, NOM, UNIT, MNF_CO_NM, MDC_CORS, PAY_TYPE, UN_COST, MAX_COST, GNL_NM_CD )" + ComNum.VBLF;
                            SQL += " VALUES ('" + strMEDC_CD + "' , '" + strADPT_FR_DT + "', '" + strADPT_TO_DT + "'," + ComNum.VBLF;
                            SQL += " '" + Quot_Conv(strARTCNM) + "', '" + strNOM + "', '" + strUNIT + "', '" + strMNF_CO_NM + "', " + ComNum.VBLF;
                            SQL += "  '" + strMDC_CORS + "' , '" + strPAY_TYPE + "', '" + strUN_COST + "', '" + strMAX_COST + "', '" + strGNL_NM_CD + "' ) " + ComNum.VBLF;

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                strOK = "NO";
                                MessageBox.Show("INSERT_RTN_TBJBD40 ERROR", "확인");
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        #endregion
                        break;
                    case "TBJBD43":
                        strDUR_CD_A = Rdr["DUR_CD_A"].ToString().Trim();                //배합금기코드A
                        strDUR_CD_B = Rdr["DUR_CD_B"].ToString().Trim();                //배합금기코드B
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();            //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();            //적용종료일자
                        strADPT_TYPE = Rdr["ADPT_TYPE"].ToString().Trim();              //적용구분(0:적용, 1:해지)
                        strDIV_CD3 = Rdr["DIV_CD3"].ToString().Trim();                  //분류코드
                        strINCOMP_MEDC_DT = Rdr["INCOMP_MEDC_DT"].ToString().Trim();    //증가약제일자
                        strUNIT_TYPE = Rdr["UNIT_TYPE"].ToString().Trim();              //단위구분(1일)

                        DUR_SD_EFT = Rdr["DUR_SD_EFT"].ToString().Trim();               
                        BEF_CMPT_CD = Rdr["BEF_CMPT_CD"].ToString().Trim();
                        SPC_AGE = Rdr["SPC_AGE"].ToString().Trim();
                        SPC_AGE_UNIT = Rdr["SPC_AGE_UNIT"].ToString().Trim();
                        Sex = Rdr["SEX"].ToString().Trim();

                        EXM_RNG_TP = Rdr["EXM_RNG_TP"].ToString().Trim();
                        EXM_TYPE = Rdr["EXM_TYPE"].ToString().Trim();
                        OFFR_MSG = Rdr["OFFR_MSG"].ToString().Trim();
                        ANNCE_DT = Rdr["ANNCE_DT"].ToString().Trim();
                        ANNCE_NO = Rdr["ANNCE_NO"].ToString().Trim();
                        EXCEP_DD_CNT = Rdr["EXCEP_DD_CNT"].ToString().Trim();

                        #region INSERT_RTN_TBJBD43

                        SQL = " INSERT INTO ADMIN.HIRA_TBJBD43(DUR_CD_A, DUR_CD_B, ADPT_FR_DT, ADPT_TO_DT, ADPT_TYPE, DIV_CD3, " + ComNum.VBLF;
                        SQL += " INCOMP_MEDC_DT, UNIT_TYPE, DUR_SD_EFT, BEF_CMPT_CD, SPC_AGE, SPC_AGE_UNIT, SEX, EXM_RNG_TP, EXM_TYPE, OFFR_MSG, ANNCE_DT, ANNCE_NO, EXCEP_DD_CNT )" + ComNum.VBLF;
                        SQL += " VALUES ('" + strDUR_CD_A + "' , '" + strDUR_CD_B + "', '" + strADPT_FR_DT + "'," + ComNum.VBLF;
                        SQL += " '" + strADPT_TO_DT + "', '" + strADPT_TYPE + "', '" + strDIV_CD3 + "', '" + strINCOMP_MEDC_DT + "', " + ComNum.VBLF;
                        SQL += " '" + strUNIT_TYPE + "' , '" + DUR_SD_EFT + "', '" + BEF_CMPT_CD + "', '" + SPC_AGE + "', '" + SPC_AGE_UNIT + "', '" + Sex + "', " + ComNum.VBLF;
                        SQL += " '" + EXM_RNG_TP + "', '" + EXM_TYPE + "', '" + OFFR_MSG + "', '" + ANNCE_DT + "', '" + ANNCE_NO + "', '" + EXCEP_DD_CNT + "' )" + ComNum.VBLF;

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD43 ERROR", "확인");
                        }
                        #endregion
                        break;
                    case "TBJBD44":
                        strGNL_NM_CD = Rdr["GNL_NM_CD"].ToString().Trim();       //일반명코드
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();     //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();     //적용종료일자
                        strSPC_AGE = Rdr["SPC_AGE"].ToString().Trim();           //특정연령
                        strSPC_AGE_UNIT = Rdr["SPC_AGE_UNIT"].ToString().Trim(); // 특정연령단위
                        strADPT_TYPE = Rdr["ADPT_TYPE"].ToString().Trim();       //적용구분

                        AGE_SD_EFT = Rdr["AGE_SD_EFT"].ToString().Trim();
                        EXM_TYPE = Rdr["EXM_TYPE"].ToString().Trim();
                        OFFR_MSG = Rdr["OFFR_MSG"].ToString().Trim();
                        ANNCE_DT = Rdr["ANNCE_DT"].ToString().Trim();
                        ANNCE_NO = Rdr["ANNCE_NO"].ToString().Trim();
                        AGE_PRS_CND_CD = Rdr["AGE_PRS_CND_CD"].ToString().Trim();

                        #region INSERT_RTN_TBJBD44

                        SQL = " INSERT INTO ADMIN.HIRA_TBJBD44(GNL_NM_CD, ADPT_FR_DT, ADPT_TO_DT, SPC_AGE, " + ComNum.VBLF;
                        SQL += " SPC_AGE_UNIT, ADPT_TYPE, AGE_SD_EFT, EXM_TYPE,  OFFR_MSG , ANNCE_DT , ANNCE_NO , AGE_PRS_CND_CD ) " + ComNum.VBLF;
                        SQL += " VALUES ('" + strGNL_NM_CD + "' , '" + strADPT_FR_DT + "', " + ComNum.VBLF;
                        SQL += " '" + strADPT_TO_DT + "', '" + strSPC_AGE + "', '" + strSPC_AGE_UNIT + "', '" + strADPT_TYPE + "' , " + ComNum.VBLF;
                        SQL += " '" + VB.Replace(AGE_SD_EFT, "'", "`") + "', '" + EXM_TYPE + "', '" + OFFR_MSG + "', '" + ANNCE_DT + "', '" + ANNCE_NO + "', '" + AGE_PRS_CND_CD + "')" + ComNum.VBLF;

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD44 ERROR", "확인");
                        }
                        #endregion
                        break;
                    case "TBJBD46":
                        strDUR_CD_A = Rdr["DUR_CD_A"].ToString().Trim();           //배합금기코드 A
                        strDUR_CD_B = Rdr["DUR_CD_B"].ToString().Trim();           //배합금기코드 B
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();       //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();       //적용종료일자
                        strMEDC_CD = Rdr["MEDC_CD"].ToString().Trim();             //약품코드
                        strDIV_CD3 = Rdr["DIV_CD3"].ToString().Trim();             //분류코드

                        #region INSERT_RTN_TBJBD46

                        SQL = " INSERT INTO ADMIN.HIRA_TBJBD46(DUR_CD_A, DUR_CD_B, ADPT_FR_DT, ADPT_TO_DT, MEDC_CD, DIV_CD3) " + ComNum.VBLF;
                        SQL += " VALUES ('" + strDUR_CD_A + "' , '" + strDUR_CD_B + "', '" + strADPT_FR_DT + "'," + ComNum.VBLF;
                        SQL += " '" + strADPT_TO_DT + "', '" + strMEDC_CD + "', '" + strDIV_CD3 + "' )" + ComNum.VBLF;

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD46 ERROR", "확인");
                        }
                        #endregion
                        break;
                    case "TBJBD47":
                        strGNL_NM_CD = Rdr["GNL_NM_CD"].ToString().Trim();          //일반명코드                        
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();        //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();        //적용종료일자
                        strSPC_AGE = Rdr["SPC_AGE"].ToString().Trim();              //특정연령
                        strSPC_AGE_UNIT = Rdr["SPC_AGE_UNIT"].ToString().Trim();    //특정연령단위
                        strMEDC_CD = Rdr["MEDC_CD"].ToString().Trim();              //약품코드


                        #region INSERT_RTN_TBJBD47
                        SQL = " INSERT INTO ADMIN.HIRA_TBJBD47(GNL_NM_CD, ADPT_FR_DT, ADPT_TO_DT, SPC_AGE, SPC_AGE_UNIT, MEDC_CD ) " + ComNum.VBLF;
                        SQL += " VALUES ('" + strGNL_NM_CD + "' , '" + strADPT_FR_DT + "'," + ComNum.VBLF;
                        SQL += " '" + strADPT_TO_DT + "', '" + strSPC_AGE + "', '" + strSPC_AGE_UNIT + "', '" + strMEDC_CD + "' )" + ComNum.VBLF;

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD47 ERROR", "확인");
                        }
                        #endregion
                        break;
                    case "TBJBD48":
                        strMEDC_INF_TYPE = Rdr["MEDC_INF_TYPE"].ToString().Trim();      //약품정보구분
                        strMEDC_CD = Rdr["MEDC_CD"].ToString().Trim();                  //약품코드
                        strADPT_TYPE = Rdr["ADPT_TYPE"].ToString().Trim();              //적용구분
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();            //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();            //적용종료일자


                        #region INSERT_RTN_TBJBD48
                        SQL = " INSERT INTO ADMIN.HIRA_TBJBD48(MEDC_INF_TYPE, MEDC_CD, ADPT_TYPE, ADPT_FR_DT, ADPT_TO_DT )" + ComNum.VBLF;
                        SQL += " VALUES ('" + strMEDC_INF_TYPE + "' , '" + strMEDC_CD + "', '" + strADPT_TYPE + "'," + ComNum.VBLF;
                        SQL += " '" + strADPT_FR_DT + "', '" + strADPT_TO_DT + "' ) " + ComNum.VBLF;

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD48 ERROR", "확인");
                        }
                        #endregion
                        break;
                    case "TBJBD52":
                        strLOW_IQTY_MEDC_CD = Rdr["LOW_IQTY_MEDC_CD"].ToString().Trim();     //약품정보구입

                        strADPT_STAT_TYPE = Rdr["ADPT_STAT_TYPE"].ToString().Trim();         //적용상태구분
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();                 //적용개시일자
                        strUNIT_CNT = Rdr["UNIT_CNT"].ToString().Trim();                     //배수아이템
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();                 //적용종료일자
                        strHIGH_IQTY_MEDC_CD = Rdr["HIGH_IQTY_MEDC_CD"].ToString().Trim();   //고함량약품코드


                        #region INSERT_RTN_TBJBD52
                        SQL = " INSERT INTO ADMIN.HIRA_TBJBD52(LOW_IQTY_MEDC_CD, ADPT_STAT_TYPE, ADPT_FR_DT, UNIT_CNT, ADPT_TO_DT, HIGH_IQTY_MEDC_CD )" + ComNum.VBLF;
                        SQL += " VALUES ('" + strLOW_IQTY_MEDC_CD + "' , '" + strADPT_STAT_TYPE + "', " + ComNum.VBLF;
                        SQL += " '" + strADPT_FR_DT + "', '" + strUNIT_CNT + "', '" + strADPT_TO_DT + "' ,'" + strHIGH_IQTY_MEDC_CD + "')" + ComNum.VBLF;

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD52 ERROR", "확인");
                        }
                        #endregion
                        break;
                    case "TBJBD55":
                        strGUBUN_CD = Rdr["TYPE_CD"].ToString().Trim();                       //구분코드

                        strGNL_NM_CD = Rdr["GNL_NM_CD"].ToString().Trim();                    //일반명코드
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();                // 적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();                //적용종료일자
                        strDD_MAX_QTY_FREQ = Rdr["MAX_QTY_CONVERT_EXPN"].ToString().Trim(); //1일 최대투여량 환산
                        strMAX_MDC_TERM = Rdr["MAX_MDC_TERM"].ToString().Trim();            //최대투여기간
                        strADPT_TYPE = Rdr["ADPT_TYPE"].ToString().Trim();                  //적용구분

                        strANNCE_DT = Rdr["ANNCE_DT"].ToString().Trim();                    //고시일자
                        strANNCE_NO = Rdr["ANNCE_NO"].ToString().Trim();                    //고시번호

                        #region INSERT_RTN_TBJBD55
                        SQL = " INSERT INTO ADMIN.HIRA_TBJBD55(GUBUN_CD, GNL_NM_CD, ADPT_FR_DT, ADPT_TO_DT, DD_MAX_QTY_FREQ, MAX_MDC_TERM, ADPT_TYPE, ANNCE_DT, ANNCE_NO )" + ComNum.VBLF;
                        SQL += " VALUES ('" + strGUBUN_CD + "' , '" + strGNL_NM_CD + "'," + ComNum.VBLF;
                        SQL += " '" + strADPT_FR_DT + "', '" + strADPT_TO_DT + "' ,'" + strDD_MAX_QTY_FREQ + "', " + ComNum.VBLF;
                        SQL += " '" + strMAX_MDC_TERM + "','" + strADPT_TYPE + "', '" + strANNCE_DT + "', '" + strANNCE_NO + "' ) " + ComNum.VBLF;

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);


                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD55 ERROR", "확인");
                        }
                        #endregion
                        break;
                    case "TBJBD56":
                        strGUBUN_CD = Rdr["TYPE_CD"].ToString().Trim();         //구분코드
                        strGNL_NM_CD = Rdr["GNL_NM_CD"].ToString().Trim();      //일반명코드
                        strMEDC_CD = Rdr["MEDC_CD"].ToString().Trim();          //약품코드
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();    //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();    //적용종료일자

                        #region INSERT_RTN_TBJBD56
                        SQL = " INSERT INTO ADMIN.HIRA_TBJBD56(GUBUN_CD, GNL_NM_CD, MEDC_CD, ADPT_FR_DT, ADPT_TO_DT )" + ComNum.VBLF;
                        SQL += " VALUES ('" + strGUBUN_CD + "' , '" + strGNL_NM_CD + "', '" + strMEDC_CD + "'," + ComNum.VBLF;
                        SQL += " '" + strADPT_FR_DT + "',  '" + strADPT_TO_DT + "' )" + ComNum.VBLF;

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD56 ERROR", "확인");
                        }
                        #endregion
                        break;
                    case "TBJBD63":
                        strELMT_CD = Rdr["ELMT_CD"].ToString().Trim();                  //성분코드
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();            //적용개시일자                        
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();            //적용종료일자
                        strEXAM_TYPE = Rdr["EXAM_TYPE"].ToString().Trim();              //점검구분
                        strCONTRAD_GRADE = Rdr["CONTRAD_GRADE"].ToString().Trim();      //등급구분
                        strADPT_TYPE = Rdr["ADPT_TYPE"].ToString().Trim();              //적용구분
                        strINCOMP_REASON = Rdr["INCOMP_REASON"].ToString().Trim();      //금기사유

                        #region INSERT_RTN_TBJBD63
                        SQL = " INSERT INTO ADMIN.HIRA_TBJBD63( ELMT_CD, ADPT_FR_DT, ADPT_TO_DT, EXAM_TYPE, CONTRAD_GRADE, ADPT_TYPE, INCOMP_REASON) " + ComNum.VBLF;
                        SQL += " VALUES ('" + strELMT_CD + "' , '" + strADPT_FR_DT + "', '" + strADPT_TO_DT + "'," + ComNum.VBLF;
                        SQL += " '" + strEXAM_TYPE + "',  '" + strCONTRAD_GRADE + "' , '" + strADPT_TYPE + "', '" + strINCOMP_REASON + "' )" + ComNum.VBLF;

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD63 ERROR", "확인");
                        }
                        #endregion
                        break;
                    case "TBDUD230":
                        EXM_KND_CD = Rdr["EXM_KND_CD"].ToString().Trim();               //성분코드
                        INFM_CD = Rdr["INFM_CD"].ToString().Trim();                     //적용개시일자
                        ADT_STA_DD = Rdr["ADT_STA_DD"].ToString().Trim();               //적용종료일자
                        ADT_END_DD = Rdr["ADT_END_DD"].ToString().Trim();               //점검구분
                        ADT_TP_CD = Rdr["ADT_TP_CD"].ToString().Trim();                 //등급구분
                        OFFR_MSG = Rdr["OFFR_MSG"].ToString().Trim();                   //적용구분
                        CND_CD = Rdr["CND_CD"].ToString().Trim();                       //금기사유
                        RMK = Rdr["RMK"].ToString().Trim();                             //금기사유


                        #region INSERT_RTN_TBDUD230
                        SQL = " INSERT INTO ADMIN.HIRA_TBDUD230( EXM_KND_CD,   INFM_CD,   ADT_STA_DD ,   ADT_END_DD ,   ADT_TP_CD ,   OFFR_MSG,   CND_CD,  RMK )" + ComNum.VBLF;
                        SQL += " VALUES ('" + EXM_KND_CD + "',  '" + INFM_CD + "',  '" + ADT_STA_DD + "' ,  '" + ADT_END_DD + "', '" + ADT_TP_CD + "' , '" + Quot(OFFR_MSG) + "', '" + CND_CD + "',  '" + VB.Left(RMK, 280) + "' )" + ComNum.VBLF;

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBDUD230 ERROR", "확인");
                        }
                        #endregion
                        break;
                }

                if (strOK == "NO") { break; }
                count = count + 1;
                pgbar.Value = count;
                pgbar.Text = Convert.ToInt32(Convert.ToDouble(count) / Convert.ToDouble(Convert.ToInt32(RowCount)) * 100) + "%";
                Application.DoEvents();
            }

            Rdr.Close();
            conn.Close();

        }

        void TRUNCATE_TALBE(string ArgTable)
        {
            SQL = "TRUNCATE TABLE  ADMIN.HIRA_" + ArgTable;
            SqlErr = clsDB.GetDataTable(ref rs, SQL, clsDB.DbCon);
            rs.Dispose();
            rs = null;
        }

        string Quot_Conv(string strString)
        {
            string rtnVal = "";

            if(strString == "") { return rtnVal; }


            rtnVal = VB.Replace(strString, "'", "`");

            return rtnVal; 
        }

        string Quot(string strString)
        {
            string rtnVal = "";

            rtnVal = VB.Replace(strString, "'", "''");

            return rtnVal;
        }

        void BAS_MSELF_RTN()
        {
            int i = 0;
            string strSuCode = "";
            string strFieldA = "";
            string strFieldB = "";
            string strDate = "";

            //항상 심평원과 1대일 동기화

            //기존자료 삭제 
            clsDB.setBeginTran(clsDB.DbCon);

            SQL = "";
            SQL = " DELETE ADMIN.BAS_MSELF WHERE GUBUNA = '1' AND GUBUNB = '0' ";
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            //등록할 자료 읽기
            SQL = " SELECT C.SUNEXT,  C.SUNAMEK, A.GNL_NM_CD,  A.SPC_AGE,  A.SPC_AGE_UNIT, A.ADPT_FR_DT, A.ADPT_TO_DT, TO_CHAR(D.DELDATE,'YYYYMMDD') DELDATE  , F.OFFR_MSG, F.CND_CD";
            SQL = SQL + ComNum.VBLF + " FROM ADMIN.HIRA_TBJBD44 A ,    ADMIN.EDI_SUGA B ,  ADMIN.BAS_SUN C , ADMIN.BAS_SUT D, ADMIN.HIRA_TBDUD230 F ";
            SQL = SQL + ComNum.VBLF + " WHERE A.GNL_NM_CD =  B.SCODE";
            SQL = SQL + ComNum.VBLF + " AND B.CODE = C.BCODE";
            SQL = SQL + ComNum.VBLF + " AND A.ADPT_FR_DT <= '" + VB.Format(cpublic.strSysDate, "YYYYMMDD") + "' ";
            SQL = SQL + ComNum.VBLF + " AND A.ADPT_TYPE = '0' ";
            SQL = SQL + ComNum.VBLF + " AND C.SUNEXT =D.SUNEXT ";
            SQL = SQL + ComNum.VBLF + " AND  B.CODE NOT IN";
            SQL = SQL + ComNum.VBLF + "                     (SELECT B.MEDC_CD ";
            SQL = SQL + ComNum.VBLF + "                      FROM ADMIN.HIRA_TBJBD47 B ";
            SQL = SQL + ComNum.VBLF + "                      WHERE B.GNL_NM_CD = A.GNL_NM_CD)";
            SQL = SQL + ComNum.VBLF + " AND A.SPC_AGE_UNIT || COALESCE(A.AGE_PRS_CND_CD,'') = F.INFM_CD";
            SQL = SQL + ComNum.VBLF + " AND F.EXM_KND_CD = '01'";
            SQL = SQL + ComNum.VBLF + " AND D.DELDATE IS NULL ";
            SQL = SQL + ComNum.VBLF + " AND A.ADPT_TO_DT ='99991231'";
            SQL = SQL + ComNum.VBLF + " ORDER BY C.SUNEXT, A.ADPT_FR_DT DESC";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strSuCode = dt.Rows[i]["SuNext"].ToString().Trim();
                    strFieldA = dt.Rows[i]["SPC_AGE"].ToString().Trim();
                    strFieldB = VB.Replace(dt.Rows[i]["OFFR_MSG"].ToString().Trim(), " ", "");
                    strDate = dt.Rows[i]["ADPT_FR_DT"].ToString();

                    SQL = " INSERT INTO ADMIN.BAS_MSELF (SuCode,GubunA,GubunB,FieldA,FieldB, " + ComNum.VBLF;
                    SQL += " EntDate) VALUES ('" + strSuCode + "','1','0','" + strFieldA + "','" + strFieldB + "', " + ComNum.VBLF;
                    SQL += " TO_DATE('" + strDate + "','YYYY-MM-DD')) " + ComNum.VBLF;

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("INSERT BAS_MSELF ERROR", "확인");
                    }
                }
            }

            dt.Dispose();
            dt = null;

            clsDB.setCommitTran(clsDB.DbCon);

        }

        void BAS_MSELF_RTN2()
        {
            //병용금기 
            int i = 0;
            string strSuCodeA = "";
            string strSuCodeB = "";
            long nREAD = 0;
            string strAnnDate = "";

            //항상 심평원과 1대일 동기화

            //기존자료 삭제 
            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT B.DUR_CD_A,  B.ANNCE_DT, B.DUR_SD_EFT, C.SUNEXT CSUNEXT ,C.SUNAMEK  SSUNAMEK, B.DUR_CD_B,  E.SUNEXT ESUNEXT  , E.SUNAMEK  ESUNAMEK ";
            SQL = SQL + ComNum.VBLF + " FROM  ADMIN.EDI_SUGA A , ADMIN.HIRA_TBJBD43 B, ";
            SQL = SQL + ComNum.VBLF + " ( SELECT AA.SUNEXT , AA.SUNAMEK , AA.BCODE FROM   ADMIN.BAS_SUN AA, ADMIN.BAS_SUT BB ";
            SQL = SQL + ComNum.VBLF + "   WHERE AA.SUNEXT = BB.SUNEXT ";
            SQL = SQL + ComNum.VBLF + "   AND BB.DELDATE IS NULL ";
            SQL = SQL + ComNum.VBLF + " ) C, ";
            SQL = SQL + ComNum.VBLF + "   ADMIN.EDI_SUGA D ,";
            SQL = SQL + ComNum.VBLF + " (";
            SQL = SQL + ComNum.VBLF + "   SELECT AA.SUNEXT , AA.SUNAMEK, AA.BCODE FROM   ADMIN.BAS_SUN AA, ADMIN.BAS_SUT BB ";
            SQL = SQL + ComNum.VBLF + "   WHERE AA.SUNEXT = BB.SUNEXT ";
            SQL = SQL + ComNum.VBLF + "   AND BB.DELDATE IS NULL ";
            SQL = SQL + ComNum.VBLF + " ) E ";
            SQL = SQL + ComNum.VBLF + "   WHERE RTRIM(A.SCODE) = B.DUR_CD_A ";
            SQL = SQL + ComNum.VBLF + "   AND A.CODE = C.BCODE ";
            SQL = SQL + ComNum.VBLF + "   AND  RTRIM(D.SCODE) = B.DUR_CD_B";
            SQL = SQL + ComNum.VBLF + "   AND D.CODE = E.BCODE";
            SQL = SQL + ComNum.VBLF + "   ORDER BY B.ANNCE_DT DESC ,  C.SUNEXT ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            nREAD = dt.Rows.Count;

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < nREAD; i++)
                {
                    strAnnDate = VB.Left(dt.Rows[i]["ANNCE_DT"].ToString().Trim(), 4) + "-" + VB.Mid(dt.Rows[i]["ANNCE_DT"].ToString().Trim(), 5, 2) + "-" + VB.Mid(dt.Rows[i]["ANNCE_DT"].ToString().Trim(), 7, 2);
                    strSuCodeA = dt.Rows[i]["CSUNEXT"].ToString().Trim();
                    strSuCodeB = dt.Rows[i]["ESUNEXT"].ToString().Trim();

                    //성분코드 색깔표시
                    SQL = " SELECT ROWID FROM ADMIN.HIRA_DURSCODE " + ComNum.VBLF;
                    SQL += " WHERE ( SCODEA = '" + dt.Rows[i]["DUR_CD_A"].ToString().Trim() + "' AND SCODEB = '" + dt.Rows[i]["DUR_CD_B"].ToString().Trim() + "' )" + ComNum.VBLF;
                    SQL += "    OR ( SCODEA = '" + dt.Rows[i]["DUR_CD_A"].ToString().Trim() + "' AND SCODEB = '" + dt.Rows[i]["DUR_CD_B"].ToString().Trim() + "' )" + ComNum.VBLF;

                    SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                    if(dt2.Rows.Count == 0)
                    {
                        //등록작업
                        SQL = " SELECT SUCODE, FIELDA " + ComNum.VBLF;
                        SQL += " From ADMIN.BAS_MSELF " + ComNum.VBLF;
                        SQL += " WHERE GUBUNA='0' " + ComNum.VBLF;
                        SQL += "   AND GUBUNB='9'" + ComNum.VBLF;
                        SQL += "   AND (" + ComNum.VBLF;
                        SQL += "         (SUCODE = '" + strSuCodeA + "'  AND FIELDA = '" + strSuCodeB + "' )" + ComNum.VBLF;
                        SQL += "     OR  (SUCODE = '" + strSuCodeB + "'  AND FIELDA = '" + strSuCodeA + "' )" + ComNum.VBLF;
                        SQL += "       )" + ComNum.VBLF;
                        SQL += " ORDER BY SUCODE" + ComNum.VBLF;

                        SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);

                        if(dt3.Rows.Count == 0)
                        {
                            switch (strSuCodeA)
                            {
                                case "METJ50":
                                case "MOBIC15":
                                case "MOBIC7.5":
                                case "COXI7.5":
                                    //류마티스내과제외
                                    break;
                                default:
                                    SQL = " INSERT INTO ADMIN.BAS_MSELF (SUCODE, GUBUNA, GUBUNB, FIELDA, FIELDB, ENTDATE ) " + ComNum.VBLF;
                                    SQL += " VALUES ( '" + strSuCodeA + "' , '0','9', '" + strSuCodeB + "' ,'',TO_DATE('" + strAnnDate + "','YYYY-MM-DD') )" + ComNum.VBLF;

                                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        MessageBox.Show("INSERT BAS_MSELF ERROR", "확인");
                                    }
                                    break;
                            }
                        }

                        dt3.Dispose();
                        dt3 = null;
                    }

                    dt2.Dispose();
                    dt2 = null;
                }  
            }

            dt.Dispose();
            dt = null;

            clsDB.setCommitTran(clsDB.DbCon);

        }

        void DBUpdate_Click()
        {
            int i = 0;
            string strJong = "";
            string strDir = "";
            string strCnt = "";
            int nIndex = 0;
            string strFileName = "";

            string strSDate = "";
            string strEDate = "";

            strSDate = cpublic.strSysDate + " " + cpublic.strSysTime;
            strEDate = cpublic.strSysDate + " " + cpublic.strSysTime;

            //파일 존재 여부 확인 작업
            FstrPath = "C:\\Program Files\\청구소프트웨어\\dur\\";

            FileInfo fileInfo = new FileInfo(FstrPath + "M.DB");
            if (fileInfo.Exists)
            {
                
            }
            else
            {
                ComFunc.MsgBox("설정 파일( " + FstrPath + "M.DB 가 없습니다.", "파일확인");
                return;
            }

            HIRA_DUR_DBUPDATE("TBJBD43");

            HIRA_DUR_DBUPDATE("TBJBD44");
                                        
            HIRA_DUR_DBUPDATE("TBJBD46");
                                        
            HIRA_DUR_DBUPDATE("TBJBD47");
                                        
            HIRA_DUR_DBUPDATE("TBJBD48");
                                        
            HIRA_DUR_DBUPDATE("TBJBD52");
                                        
            HIRA_DUR_DBUPDATE("TBJBD55");
                                        
            HIRA_DUR_DBUPDATE("TBJBD56");
                                        
            HIRA_DUR_DBUPDATE("TBJBD63");

            strEDate = cpublic.strSysDate + " " + cpublic.strSysTime;

            ComFunc.MsgBox(strSDate + " ~ " + strEDate + " 작업 완료하였습니다.", "확인");
        }

        void HIRA_DUR_DBUPDATE(string ArgTABLE)
        {
            long nNo = 0;
            string strOK = "";
            int count = 0;

            //TBJBD43---------------------
            string strDUR_CD_A = "";
            string strDUR_CD_B = "";
            string strADPT_FR_DT = "";
            string strADPT_TO_DT = "";
            string strADPT_TYPE = "";
            string strDIV_CD3 = "";
            string strINCOMP_MEDC_DT = "";
            string strUNIT_TYPE = "";

            //TBJBD44---------------------
            string strGNL_NM_CD = "";
            string strSPC_AGE = "";
            string strSPC_AGE_UNIT = "";

            //TBJBD46---------------------
            string strMEDC_CD = "";

            //TBJBD47---------------------
            //TBJBD48---------------------
            string strMEDC_INF_TYPE = "";

            //TBJBD52---------------------
            string strLOW_IQTY_MEDC_CD = "";
            string strADPT_STAT_TYPE = "";
            string strUNIT_CNT = "";
            string strHIGH_IQTY_MEDC_CD = "";

            //TBJBD55---------------------
            string strGUBUN_CD = "";
            string strDD_MAX_QTY_FREQ = "";
            string strMAX_MDC_TERM = "";

            //TBJBD63---------------------
            string strELMT_CD = "";
            string strEXAM_TYPE = "";
            string strCONTRAD_GRADE = "";
            string strINCOMP_REASON = "";

            string strSQLLite = "";
            string strRowid = "";

            strOK = "OK";

            switch (ArgTABLE)
            {
                case "TBJBD43":
                    strSQLLite = " SELECT ALL DUR_CD_A, DUR_CD_B, ADPT_FR_DT, ADPT_TO_DT, ADPT_TYPE, DIV_CD3, INCOMP_MEDC_DT, UNIT_TYPE FROM " + ArgTABLE ;
                    break;
                case "TBJBD44":
                    strSQLLite = " SELECT ALL GNL_NM_CD, ADPT_FR_DT, ADPT_TO_DT, SPC_AGE, SPC_AGE_UNIT, ADPT_TYPE FROM " + ArgTABLE;
                    break;
                case "TBJBD46":
                    strSQLLite = " SELECT ALL DUR_CD_A, DUR_CD_B, ADPT_FR_DT, ADPT_TO_DT, MEDC_CD, DIV_CD3 FROM" + ArgTABLE;
                    break;
                case "TBJBD47":
                    strSQLLite = " SELECT ALL GNL_NM_CD, ADPT_FR_DT, ADPT_TO_DT, SPC_AGE, SPC_AGE_UNIT, MEDC_CD FROM " + ArgTABLE;
                    break;
                case "TBJBD48":
                    strSQLLite = " SELECT ALL MEDC_INF_TYPE , MEDC_CD, ADPT_TYPE, ADPT_FR_DT, ADPT_TO_DT FROM " + ArgTABLE;
                    break;
                case "TBJBD52":
                    strSQLLite = " SELECT ALL LOW_IQTY_MEDC_CD, ADPT_STAT_TYPE, ADPT_FR_DT, UNIT_CNT, ADPT_TO_DT, HIGH_IQTY_MEDC_CD FROM " + ArgTABLE;
                    break;
                case "TBJBD55":
                    strSQLLite = " SELECT ALL GUBUN_CD, GNL_NM_CD, ADPT_FR_DT, ADPT_TO_DT, DD_MAX_QTY_FREQ, MAX_MDC_TERM, ADPT_TYPE FROM " + ArgTABLE;
                    break;
                case "TBJBD56":
                    strSQLLite = " SELECT ALL GUBUN_CD, GNL_NM_CD, MEDC_CD, ADPT_FR_DT, ADPT_TO_DT FROM " + ArgTABLE;
                    break;
                case "TBJBD63":
                    strSQLLite = " SELECT ALL ELMT_CD, ADPT_FR_DT, ADPT_TO_DT, EXAM_TYPE, CONTRAD_GRADE, ADPT_TYPE, INCOMP_REASON FROM " + ArgTABLE;
                    break;
            }

            conn = new SQLiteConnection("Data Source=" + FstrPath + "medi.db;Version=3;");
            conn.Open();
            cmd = new SQLiteCommand(strSQLLite, conn);
            Rdr = cmd.ExecuteReader();

            cnt = new SQLiteCommand("select count(*) from " + ArgTABLE, conn);
            int RowCount = 0;
            RowCount = Convert.ToInt32(cnt.ExecuteScalar());

            pgbar.Minimum = count;
            pgbar.Maximum = RowCount;
            pgbar.Value = pgbar.Minimum;

            while (Rdr.Read())
            {
                nNo = nNo + 1;
                switch (ArgTABLE)
                {
                    case "TBJBD43":
                        strDUR_CD_A = Rdr["DUR_CD_A"].ToString().Trim();             //배합금기코드A
                        strDUR_CD_B = Rdr["DUR_CD_B"].ToString().Trim();             //배합금기코드B
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();         //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();         //적용종료일자
                        strADPT_TYPE = Rdr["ADPT_TYPE"].ToString().Trim();           //적용구분(0:적용, 1:해지)
                        strDIV_CD3 = Rdr["DIV_CD3"].ToString().Trim();               //분류코드
                        strINCOMP_MEDC_DT = Rdr["INCOMP_MEDC_DT"].ToString().Trim(); //증가약제일자
                        strUNIT_TYPE = Rdr["UNIT_TYPE"].ToString().Trim();           //단위구분(1일)

                        #region INSERT_RTN_TBJBD43
                        SQL = " SELECT ROWID FROM ADMIN.HIRA_TBJBD43      " + ComNum.VBLF;
                        SQL += " WHERE DUR_CD_A = '" + strDUR_CD_A + "'         " + ComNum.VBLF;
                        SQL += " AND DUR_CD_B = '" + strDUR_CD_B + "'           " + ComNum.VBLF;
                        SQL += " AND ADPT_FR_DT = '" + strADPT_FR_DT + "'       " + ComNum.VBLF;

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if(dt.Rows.Count > 0)
                        {
                            strRowid = dt.Rows[0]["ROWID"].ToString().Trim();

                            SQL = " UPDATE ADMIN.HIRA_TBJBD43 SET             ";
                            SQL += " ADPT_FR_DT = '" + strADPT_FR_DT + "' ,         ";
                            SQL += " ADPT_TO_DT = '" + strADPT_TO_DT + "' ,         ";
                            SQL += " ADPT_TYPE= '" + strADPT_TYPE + "' ,            ";
                            SQL += " DIV_CD3 = '" + strDIV_CD3 + "' ,               ";
                            SQL += " INCOMP_MEDC_DT= '" + strINCOMP_MEDC_DT + "',   ";
                            SQL += " UNIT_TYPE = '" + strUNIT_TYPE + "'             ";
                            SQL += " WHERE ROWID = '" + strRowid + "'               ";
                        }
                        else
                        {
                            SQL = " INSERT INTO ADMIN.HIRA_TBJBD43(DUR_CD_A, DUR_CD_B, ADPT_FR_DT, ADPT_TO_DT, ADPT_TYPE, DIV_CD3, INCOMP_MEDC_DT, UNIT_TYPE )    ";
                            SQL += " VALUES ('" + strDUR_CD_A + "' , '" + strDUR_CD_B + "', '" + strADPT_FR_DT + "',                                                    ";
                            SQL += " '" + strADPT_TO_DT + "', '" + strADPT_TYPE + "', '" + strDIV_CD3 + "', '" + strINCOMP_MEDC_DT + "',                                ";
                            SQL += " '" + strUNIT_TYPE + "' )                                                                                                           ";
                        }

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD43 ERROR", "확인");
                        }

                        dt.Dispose();
                        dt = null;
                        #endregion
                        break;
                    case "TBJBD44":
                        strGNL_NM_CD = Rdr["GNL_NM_CD"].ToString().Trim();          //일반명코드
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();        //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();        //적용종료일자
                        strSPC_AGE = Rdr["SPC_AGE"].ToString().Trim();              //특정연령
                        strSPC_AGE_UNIT = Rdr["SPC_AGE_UNIT"].ToString().Trim();    //특정연령단위
                        strADPT_TYPE = Rdr["ADPT_TYPE"].ToString().Trim();          //적용구분

                        #region INSERT_RTN_TBJBD44
                        SQL = " SELECT ROWID FROM ADMIN.HIRA_TBJBD44      " + ComNum.VBLF;
                        SQL += " WHERE GNL_NM_CD = '" + strGNL_NM_CD + "'       " + ComNum.VBLF;
                        SQL += " AND ADPT_FR_DT = '" + strADPT_FR_DT + "'       " + ComNum.VBLF;
                        SQL += " AND ADPT_TYPE = '" + strADPT_FR_DT + "'        " + ComNum.VBLF;

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (dt.Rows.Count > 0)
                        {
                            strRowid = dt.Rows[0]["ROWID"].ToString().Trim();

                            SQL = " UPDATE ADMIN.HIRA_TBJBD44 SET         ";
                            SQL += " ADPT_FR_DT = '" + strADPT_FR_DT + "' ,     ";
                            SQL += " ADPT_TO_DT = '" + strADPT_TO_DT + "' ,     ";
                            SQL += " SPC_AGE= '" + strSPC_AGE + "' ,            ";
                            SQL += " SPC_AGE_UNIT= '" + strSPC_AGE_UNIT + "' ,  ";
                            SQL += " ADPT_TYPE= '" + strADPT_TYPE + "'          ";
                            SQL += " WHERE ROWID = '" + strRowid + "'           ";
                        }
                        else
                        {
                            SQL = " INSERT INTO ADMIN.HIRA_TBJBD44(GNL_NM_CD, ADPT_FR_DT, ADPT_TO_DT, SPC_AGE, SPC_AGE_UNIT, ADPT_TYPE )  ";
                            SQL += " VALUES ('" + strGNL_NM_CD + "' , '" + strADPT_FR_DT + "',                                                  ";
                            SQL += " '" + strADPT_TO_DT + "', '" + strSPC_AGE + "', '" + strSPC_AGE_UNIT + "', '" + strADPT_TYPE + "' )         ";
                        }

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD44 ERROR", "확인");
                        }

                        dt.Dispose();
                        dt = null;
                        #endregion
                        break;
                    case "TBJBD46":
                        strDUR_CD_A = Rdr["DUR_CD_A"].ToString().Trim();            //배합금기코드A
                        strDUR_CD_B = Rdr["DUR_CD_B"].ToString().Trim();            //배합금기코드B
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();        //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();        //적용종료일자
                        strMEDC_CD = Rdr["MEDC_CD"].ToString().Trim();              //약품코드
                        strDIV_CD3 = Rdr["DIV_CD3"].ToString().Trim();              //분류코드

                        #region INSERT_TRN_TBJBD46
                        SQL = " SELECT ROWID FROM ADMIN.HIRA_TBJBD46      " + ComNum.VBLF;
                        SQL += " WHERE DUR_CD_A = '" + strDUR_CD_A + "'         " + ComNum.VBLF;
                        SQL += " AND DUR_CD_B = '" + strDUR_CD_B + "'           " + ComNum.VBLF;
                        SQL += " AND ADPT_FR_DT = '" + strADPT_FR_DT + "'       " + ComNum.VBLF;

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (dt.Rows.Count > 0)
                        {
                            strRowid = dt.Rows[0]["ROWID"].ToString().Trim();

                            SQL = " UPDATE ADMIN.HIRA_TBJBD46 SET         ";
                            SQL += " ADPT_FR_DT = '" + strADPT_FR_DT + "' ,     ";
                            SQL += " ADPT_TO_DT = '" + strADPT_TO_DT + "' ,     ";
                            SQL += " MEDC_CD= '" + strMEDC_CD + "' ,            ";
                            SQL += " DIV_CD3 = '" + strDIV_CD3 + "'             ";
                            SQL += " WHERE ROWID = '" + strRowid + "'           ";
                        }
                        else
                        {
                            SQL = " INSERT INTO ADMIN.HIRA_TBJBD46(DUR_CD_A, DUR_CD_B, ADPT_FR_DT, ADPT_TO_DT, MEDC_CD, DIV_CD3)      ";
                            SQL += " VALUES ('" + strDUR_CD_A + "' , '" + strDUR_CD_B + "', '" + strADPT_FR_DT + "',                        ";
                            SQL += " '" + strADPT_TO_DT + "', '" + strMEDC_CD + "', '" + strDIV_CD3 + "' )                                  ";
                        }

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_TRN_TBJBD46 ERROR", "확인");
                        }

                        dt.Dispose();
                        dt = null;
                        #endregion
                        break;
                    case "TBJBD47":
                        strGNL_NM_CD = Rdr["GNL_NM_CD"].ToString().Trim();          //일반명코드
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();        //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();        //적용종료일자
                        strSPC_AGE = Rdr["SPC_AGE"].ToString().Trim();              //특정연령
                        strSPC_AGE_UNIT = Rdr["SPC_AGE_UNIT"].ToString().Trim();    //특정연령단위
                        strMEDC_CD = Rdr["MEDC_CD"].ToString().Trim();              //약품코드

                        #region INSERT_RTN_TBJBD47
                        SQL = " SELECT ROWID FROM ADMIN.HIRA_TBJBD47      " + ComNum.VBLF;
                        SQL += " WHERE GNL_NM_CD = '" + strGNL_NM_CD + "'       " + ComNum.VBLF;
                        SQL += " AND ADPT_FR_DT = '" + strADPT_FR_DT + "'       " + ComNum.VBLF;

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (dt.Rows.Count > 0)
                        {
                            strRowid = dt.Rows[0]["ROWID"].ToString().Trim();

                            SQL = " UPDATE ADMIN.HIRA_TBJBD47 SET         ";
                            SQL += " ADPT_FR_DT = '" + strADPT_FR_DT + "' ,     ";
                            SQL += " ADPT_TO_DT = '" + strADPT_TO_DT + "' ,     ";
                            SQL += " SPC_AGE= '" + strSPC_AGE + "' ,            ";
                            SQL += " SPC_AGE_UNIT= '" + strSPC_AGE_UNIT + "' ,  ";
                            SQL += " MEDC_CD= '" + strMEDC_CD + "'              ";
                            SQL += " WHERE ROWID = '" + strRowid + "'           ";
                        }
                        else
                        {
                            SQL = " INSERT INTO ADMIN.HIRA_TBJBD47(GNL_NM_CD, ADPT_FR_DT, ADPT_TO_DT, SPC_AGE, SPC_AGE_UNIT, MEDC_CD)     ";
                            SQL += " VALUES ('" + strGNL_NM_CD + "' , '" + strADPT_FR_DT + "',                                                  ";
                            SQL += " '" + strADPT_TO_DT + "', '" + strSPC_AGE + "', '" + strSPC_AGE_UNIT + "', '" + strMEDC_CD + "' )           ";
                        }

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD47 ERROR", "확인");
                        }

                        dt.Dispose();
                        dt = null;
                        #endregion
                        break;
                    case "TBJBD48":
                        strMEDC_INF_TYPE = Rdr["MEDC_INF_TYPE"].ToString().Trim();      //약품정보구분
                        strMEDC_CD = Rdr["MEDC_CD"].ToString().Trim();                  //악품코드
                        strADPT_TYPE = Rdr["ADPT_TYPE"].ToString().Trim();              //적용구분
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();            //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();            //적용종료일자

                        #region INSERT_RTN_TBJBD48
                        SQL = "SELECT ROWID FROM ADMIN.HIRA_TBJBD48           " + ComNum.VBLF;
                        SQL += " WHERE MEDC_INF_TYPE = '" + strMEDC_INF_TYPE + "'   " + ComNum.VBLF;
                        SQL += " AND MEDC_CD = '" + strMEDC_CD + "'                 " + ComNum.VBLF;

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (dt.Rows.Count > 0)
                        {
                            strRowid = dt.Rows[0]["ROWID"].ToString().Trim();

                            SQL = " UPDATE ADMIN.HIRA_TBJBD48 SET             ";
                            SQL += " MEDC_INF_TYPE = '" + strMEDC_INF_TYPE + "' ,   ";
                            SQL += " MEDC_CD = '" + strMEDC_CD + "' ,               ";
                            SQL += " ADPT_TYPE= '" + strADPT_TYPE + "' ,            ";
                            SQL += " ADPT_FR_DT = '" + strADPT_FR_DT + "' ,         ";
                            SQL += " ADPT_TO_DT = '" + strADPT_TO_DT + "'           ";
                            SQL += " WHERE ROWID = '" + strRowid + "'               ";
                        }
                        else
                        {
                            SQL = " INSERT INTO ADMIN.HIRA_TBJBD48(MEDC_INF_TYPE, MEDC_CD, ADPT_TYPE, ADPT_FR_DT, ADPT_TO_DT )    ";
                            SQL += " VALUES ('" + strMEDC_INF_TYPE + "' , '" + strMEDC_CD + "', '" + strADPT_TYPE + "',                 ";
                            SQL += " '" + strADPT_FR_DT + "', '" + strADPT_TO_DT + "' )                                                 ";
                        }

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD48 ERROR", "확인");
                        }

                        dt.Dispose();
                        dt = null;
                        #endregion
                        break;
                    case "TBJBD52":
                        strLOW_IQTY_MEDC_CD = Rdr["LOW_IQTY_STAT_TYPE"].ToString().Trim();      //약품정보구분
                        strADPT_STAT_TYPE = Rdr["ADPT_STAT_TYPE"].ToString().Trim();            //적용상태구분
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();                    //적용개시일자
                        strUNIT_CNT = Rdr["UNIT_CNT"].ToString().Trim();                        //배수아이템
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();                    //적용종료일자
                        strHIGH_IQTY_MEDC_CD = Rdr["HIGH_IQTY_MEDC_CD"].ToString().Trim();      //고함량약품코드

                        #region INSERT_RTN_TBJBD52
                        SQL = " SELECT ROWID FROM ADMIN.HIRA_TBJBD52              " + ComNum.VBLF;
                        SQL += " WHERE LOW_IQTY_MEDC_CD = '" + strLOW_IQTY_MEDC_CD + "' " + ComNum.VBLF;
                        SQL += " AND HIGH_IQTY_MEDC_CD = '" + strHIGH_IQTY_MEDC_CD + "' " + ComNum.VBLF;

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (dt.Rows.Count > 0)
                        {
                            strRowid = dt.Rows[0]["ROWID"].ToString().Trim();

                            SQL = " UPDATE ADMIN.HIRA_TBJBD52 SET                     ";
                            SQL += " ADPT_STAT_TYPE = '" + strADPT_STAT_TYPE + "' ,         ";
                            SQL += " ADPT_FR_DT = '" + strADPT_FR_DT + "' ,                 ";
                            SQL += " UNIT_CNT= '" + strUNIT_CNT + "' ,                      ";
                            SQL += " ADPT_TO_DT = '" + strADPT_TO_DT + "',                  ";
                            SQL += " HIGH_IQTY_MEDC_CD = '" + strHIGH_IQTY_MEDC_CD + "'     ";
                            SQL += " WHERE ROWID = '" + strRowid + "'                       ";
                        }
                        else
                        {
                            SQL = " INSERT INTO ADMIN.HIRA_TBJBD52(LOW_IQTY_MEDC_CD, ADPT_STAT_TYPE, ADPT_FR_DT, UNIT_CNT, ADPT_TO_DT, HIGH_IQTY_MEDC_CD )    ";
                            SQL += " VALUES ('" + strLOW_IQTY_MEDC_CD + "' , '" + strADPT_STAT_TYPE + "',                                                           ";
                            SQL += " '" + strADPT_FR_DT + "', '" + strUNIT_CNT + "', '" + strADPT_TO_DT + "' ,'" + strHIGH_IQTY_MEDC_CD + "')                       ";
                        }

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD52 ERROR", "확인");
                        }

                        dt.Dispose();
                        dt = null;
                        #endregion
                        break;
                    case "TBJBD55":
                        strGUBUN_CD = Rdr["GUBUN_CD"].ToString().Trim();                        //구분코드
                        strGNL_NM_CD = Rdr["GNL_NM_CD"].ToString().Trim();                      //일반명코드
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();                    //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();                    //적용종료일자
                        strDD_MAX_QTY_FREQ = Rdr["DD_MAX_QTY_FREQ"].ToString().Trim();          //1일 최대투여량 환산
                        strMAX_MDC_TERM = Rdr["MAX_MDC_TERM"].ToString().Trim();                //최대투여기간
                        strADPT_TYPE = Rdr["ADPT_TYPE"].ToString().Trim();                      //적용구분

                        #region INSERT_RTN_TBJBD55
                        SQL = " SELECT ROWID FROM ADMIN.HIRA_TBJBD55  " + ComNum.VBLF;
                        SQL += " WHERE GUBUN_CD = '" + strGUBUN_CD + "'     " + ComNum.VBLF;
                        SQL += " AND GNL_NM_CD = '" + strGNL_NM_CD + "'     " + ComNum.VBLF;
                        SQL += " AND ADPT_FR_DT = '" + strADPT_FR_DT + "'   " + ComNum.VBLF;

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (dt.Rows.Count > 0)
                        {
                            strRowid = dt.Rows[0]["ROWID"].ToString().Trim();

                            SQL = " UPDATE ADMIN.HIRA_TBJBD55 SET                 ";
                            SQL += " GUBUN_CD = '" + strGUBUN_CD + "',                  ";
                            SQL += " GNL_NM_CD = '" + strGNL_NM_CD + "',                ";
                            SQL += " ADPT_FR_DT = '" + strADPT_FR_DT + "' ,             ";
                            SQL += " ADPT_TO_DT = '" + strADPT_TO_DT + "',              ";
                            SQL += " DD_MAX_QTY_FREQ= '" + strDD_MAX_QTY_FREQ + "' ,    ";
                            SQL += " MAX_MDC_TERM= '" + strMAX_MDC_TERM + "' ,          ";
                            SQL += " ADPT_TYPE= '" + strADPT_TYPE + "'                  ";
                            SQL += " WHERE ROWID = '" + strRowid + "'                   ";
                        }
                        else
                        {
                            SQL = " INSERT INTO ADMIN.HIRA_TBJBD55(GUBUN_CD, GNL_NM_CD, ADPT_FR_DT, ADPT_TO_DT, DD_MAX_QTY_FREQ, MAX_MDC_TERM, ADPT_TYPE )    ";
                            SQL += " VALUES ('" + strGUBUN_CD + "' , '" + strGNL_NM_CD + "',                                                                        ";
                            SQL += " '" + strADPT_FR_DT + "', '" + strADPT_TO_DT + "' ,'" + strDD_MAX_QTY_FREQ + "',                                                ";
                            SQL += " '" + strMAX_MDC_TERM + "','" + strADPT_TYPE + "')                                                                              ";
                        }

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD55 ERROR", "확인");
                        }

                        dt.Dispose();
                        dt = null;
                        #endregion
                        break;
                    case "TBJBD56":
                        strGUBUN_CD = Rdr["GUBUN_CD"].ToString().Trim();                        //구분코드
                        strGNL_NM_CD = Rdr["GNL_NM_CD"].ToString().Trim();                      //일반명코드
                        strMEDC_CD = Rdr["MEDC_CD"].ToString().Trim();                          //약품코드
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();                    //적용개시일자
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();                    //적용종료일자 

                        #region INSERT_RTN_TBJBD56
                        SQL = " SELECT ROWID FROM ADMIN.HIRA_TBJBD56      " + ComNum.VBLF;
                        SQL += " WHERE GUBUN_CD = '" + strGUBUN_CD + "'         " + ComNum.VBLF;
                        SQL += " AND GNL_NM_CD = '" + strGNL_NM_CD + "'         " + ComNum.VBLF;
                        SQL += " AND MEDC_CD = '" + strMEDC_CD + "'             " + ComNum.VBLF;

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (dt.Rows.Count > 0)
                        {
                            strRowid = dt.Rows[0]["ROWID"].ToString().Trim();

                            SQL = " UPDATE ADMIN.HIRA_TBJBD56 SET     ";
                            SQL += " GUBUN_CD = '" + strGUBUN_CD + "' ,     ";
                            SQL += " GNL_NM_CD  = '" + strGNL_NM_CD + "' ,  ";
                            SQL += " MEDC_CD = '" + strMEDC_CD + "' ,       ";
                            SQL += " ADPT_FR_DT = '" + strADPT_FR_DT + "' , ";
                            SQL += " ADPT_TO_DT = '" + strADPT_TO_DT + "'   ";
                            SQL += " WHERE ROWID = '" + strRowid + "'       ";
                        }
                        else
                        {
                            SQL = " INSERT INTO ADMIN.HIRA_TBJBD56(GUBUN_CD, GNL_NM_CD, MEDC_CD, ADPT_FR_DT, ADPT_TO_DT )     ";
                            SQL += " VALUES ('" + strGUBUN_CD + "' , '" + strGNL_NM_CD + "', '" + strMEDC_CD + "',                  ";
                            SQL += " '" + strADPT_FR_DT + "',  '" + strADPT_TO_DT + "' )                                            ";
                        }

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD56 ERROR", "확인");
                        }

                        dt.Dispose();
                        dt = null;
                        #endregion
                        break;
                    case "TBJBD63":
                        strELMT_CD = Rdr["ELMT_CD"].ToString().Trim();                          //성분코드                        
                        strADPT_FR_DT = Rdr["ADPT_FR_DT"].ToString().Trim();                    //적용개시일자                        
                        strADPT_TO_DT = Rdr["ADPT_TO_DT"].ToString().Trim();                    //적용종료일자
                        strEXAM_TYPE = Rdr["EXAM_TYPE"].ToString().Trim();                      //점검구분
                        strCONTRAD_GRADE = Rdr["CONTRAD_GRADE"].ToString().Trim();              //등급구분
                        strADPT_TYPE = Rdr["ADPT_TYPE"].ToString().Trim();                      //적용구분
                        strINCOMP_REASON = Rdr["INCOMP_REASON"].ToString().Trim();              //금기사유

                        #region INSERT_RTN_TBJBD63
                        SQL = " SELECT ROWID FROM ADMIN.HIRA_TBJBD63      " + ComNum.VBLF;
                        SQL += " WHERE ELMT_CD = '" + strGUBUN_CD + "'          " + ComNum.VBLF;

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (dt.Rows.Count > 0)
                        {
                            strRowid = dt.Rows[0]["ROWID"].ToString().Trim();

                            SQL = " UPDATE ADMIN.HIRA_TBJBD63 SET                 ";
                            SQL += " ELMT_CD = '" + strELMT_CD + "' ,                   ";
                            SQL += " ADPT_FR_DT = '" + strADPT_FR_DT + "' ,             ";
                            SQL += " ADPT_TO_DT = '" + strADPT_TO_DT + "'               ";
                            SQL += " EXAM_TYPE  = '" + strEXAM_TYPE + "' ,              ";
                            SQL += " CONTRAD_GRADE = '" + strCONTRAD_GRADE + "' ,       ";
                            SQL += " ADPT_TYPE = '" + strADPT_TYPE + "' ,               ";
                            SQL += " INCOMP_REASON = '" + strINCOMP_REASON + "'         ";
                            SQL += " WHERE ROWID = '" + strRowid + "'                   ";
                        }
                        else
                        {
                            SQL = " INSERT INTO ADMIN.HIRA_TBJBD63( ELMT_CD, ADPT_FR_DT, ADPT_TO_DT, EXAM_TYPE, CONTRAD_GRADE, ADPT_TYPE, INCOMP_REASON)  ";
                            SQL += " VALUES ('" + strELMT_CD + "' , '" + strADPT_FR_DT + "', '" + strADPT_TO_DT + "',                                           ";
                            SQL += " '" + strEXAM_TYPE + "',  '" + strCONTRAD_GRADE + "' , '" + strADPT_TYPE + "', '" + strINCOMP_REASON + "' )                 ";
                        }

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            strOK = "NO";
                            MessageBox.Show("INSERT_RTN_TBJBD44 ERROR", "확인");
                        }

                        dt.Dispose();
                        dt = null;
                        #endregion
                        break;
                }

                count = count + 1;
                pgbar.Value = count;
                pgbar.Text = Convert.ToInt32(Convert.ToDouble(count) / Convert.ToDouble(Convert.ToInt32(RowCount)) * 100) + "%";
                Application.DoEvents();

            }

            Rdr.Close();
            conn.Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

        }
    }
}
