using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComBase;
using ComEmrBase;
using ComLibB;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using Oracle.DataAccess.Client;

namespace ComNurLibB
{
    public partial class frmActing : Form, MainFormMessage
    {
        #region MainFormMessage InterFace

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

        #endregion MainFormMessage InterFace

        private bool bolLoad = false;

        frmDCActing DCActing = null;
        private string GstrHospCode = "";
        
        // 파라메타로 받아 올것 - 영록
        private string FstrRoom = "";

        private string FstrPtno = "";
        private string FstrSname = "";
        private string FstrInDate = "";
        private string FstrDEPT = "";
        private string FstrDrCode = "";
        private string FstrROWID = ""; //NUR_ER_PATIENT
        private int mintIPDNO = 0;
        private bool mbolACT_OK = false;  //NUR_ER_PATIENT
        private string FstrEndDate = ""; // 추가로 필요함. - 영록 // OPD_MASTER -> BDATE
        ////////////////////////////////////////////////////

        private bool FbBtnClick = false;

        private string mstrACTTable = "";
        private string mstrDate = "";
        private string mstrTime = "";
        private string mstrWard = ""; //ComboWard
        private string mstrEXEName = "";
        private string mstrBDate = "";

        #region EMR 저장에 필요

        private string mstrChartX1 = "<chart>";
        private string mstrChartX2 = "</chart>";

        private string mstrXml = "";
        private string[] mstrTagHead = null;
        private string[] mstrTagTail = null;

        private string mstrUSEID = "";
        private string mstrChartDate = "";
        private string mstrChartTime = "";
        private int mintAcpNo = 0;
        private string mstrPTNO = "";
        private string mstrInOutCls = "";
        private string mstrMedFrDate = "";
        private string mstrMedEndDate = "";
        private string mstrMedDeptCd = "";
        private string mstrMedDrCd = "";
        private string mstrMedFrTime = "";
        private string mstrMedEndTime = "";
        private string mstrNewChartCd = "";

        /// <summary>
        /// EMR 환자정보
        /// </summary>
        EmrPatient pAcp = null;

        /// <summary>
        /// 삽입 번들용
        /// </summary>
        frmEmrChartNew frmEmrChartNewX = null;
        #endregion EMR 저장에 필요

        private int FnBRow = 0;

        private string FstrLineGubun = "";

        private CheckBoxCellType ctCheck = new CheckBoxCellType();
        private TextCellType ctText = new TextCellType();

        // 부득이 하게 두개 사용
        private TextCellType CellText = new TextCellType();

        private CheckBoxCellType CellChk = new CheckBoxCellType();
        private DateTimeCellType CellDate = new DateTimeCellType();
        private DateTimeCellType CellTime = new DateTimeCellType();
        private ComboBoxCellType CellCbo = new ComboBoxCellType();
        private ButtonCellType CellBtn1 = new ButtonCellType();//유지
        private ButtonCellType CellBtn2 = new ButtonCellType();//제거
        private ButtonCellType CellBtn3 = new ButtonCellType();//삭제
        private ButtonCellType CellBtn4 = new ButtonCellType();//수정


        private enum enumColumn_LineAct
        {
            /* NO,확인일시,상태,삽입위치(카테터종류),조작전손위생,
             * 허브소독,무균술적용,발적,부종,삼출액, 
             * 압통, 이상없음, 드레싱일, 드레싱시,종류, 
             * 소독제, 건조여부, 부착상태, 필요성사정,삽입일시, 
             * 유지일, 액팅유지, 액팅제거, ROWID, SEQNO, 
             * 내용삭제, 내용수정, EMRNO, 종류
             */

            NUMBER = 0,            CHECK_DATETIME = 1,          STATE = 2,                INSERT_LOCATE = 3,            HAND_HYGN = 4,
            HERB_STRL = 5,         ASEPTIC_TECH = 6,            OBSERVE_1 = 7,            OBSERVE_2 = 8,                OBSERVE_3 = 9,
            OBSERVE_4 = 10,        OBSERVE_5 = 11,              DRS_DATE = 12,            DRS_TIME = 13,                DRS_KIND = 14,
            DRS_NAME = 15,         DRS_STATE1 =16 ,             DRS_STATE2 = 17,          NEED_ASSES = 18,              INSERT_DATETIME = 19,
            KEEP_DAY = 20,         ACT_KEEP = 21,               ACT_REMOVE = 22,          ROWID = 23,                   SEQNO = 24,
            ACT_DEL = 25,          ACT_UPDATE = 26,             EMRNO = 27,               CATH_TYPE = 28
        }

        private struct LINE1_ACT
        {

            
            
            public string IPDNO;
            public string Pano;
            public string WardCode;
            public string RoomCode;
            public string DeptCode;
            public string DrCode;
            public string InDate;
            public string STARTDATE;
            public string STARTTIME;
            public string ACTDATE;
            public string ACTTIME;
            public string status;
            public string ACTSABUN;
            public string PART1;
            public string PART2;
            public string LOCATE1;
            public string LOCATE2;
            public string MON1;
            public string MON2;
            public string MON3;
            public string MON4;
            public string MON5;
            public string MON6;
            public string D_DATE;
            public string D_TIME;
            public string D_PART;
            public string D_MON1;
            public string D_MON2;
            public string SEQNO;
            public string USEDATE;
            public string INSERT_BUSE;
            public string DUTY;
            public string HAND_HYGN;        //조작 전 손위생
            public string HERB_STRL;        //허브소독
            public string ASEPTIC_TECH;     //메인 허브 교체 시 무균실 적용
            public string DRS_NAME;         //종류    
            public string NEED_ASSES;       //필요성 사정
            public string BIGO;             //비고
        }



        private LINE1_ACT Line1Act = new LINE1_ACT();

        private void ClearLine1Act()
        {
            Line1Act.IPDNO = "";
            Line1Act.Pano = "";
            Line1Act.WardCode = "";
            Line1Act.RoomCode = "";
            Line1Act.DeptCode = "";
            Line1Act.DrCode = "";
            Line1Act.InDate = "";
            Line1Act.STARTDATE = "";
            Line1Act.STARTTIME = "";
            Line1Act.ACTDATE = "";
            Line1Act.ACTTIME = "";
            Line1Act.status = "";
            Line1Act.ACTSABUN = "";
            Line1Act.PART1 = "";
            Line1Act.PART2 = "";
            Line1Act.LOCATE1 = "";
            Line1Act.LOCATE2 = "";
            Line1Act.MON1 = "";
            Line1Act.MON2 = "";
            Line1Act.MON3 = "";
            Line1Act.MON4 = "";
            Line1Act.MON5 = "";
            Line1Act.MON6 = "";
            Line1Act.D_DATE = "";
            Line1Act.D_TIME = "";
            Line1Act.D_PART = "";
            Line1Act.D_MON1 = "";
            Line1Act.D_MON2 = "";
            Line1Act.SEQNO = "";
            Line1Act.USEDATE = "";
            Line1Act.INSERT_BUSE = "";
            Line1Act.DUTY = "";
            Line1Act.HAND_HYGN = "";
            Line1Act.HERB_STRL = "";
            Line1Act.ASEPTIC_TECH = "";
            Line1Act.DRS_NAME = "";
            Line1Act.NEED_ASSES = "";
            Line1Act.BIGO = "";

        }

        public frmActing(string strPtno, string strInDate, string strRoom, string strWard, string strDEPT, string strDrCode, string strEndDate, bool bolACT_OK)
        {
            InitializeComponent();
            FstrPtno = "";
            FstrInDate = "";
            FstrRoom = "";
            mstrWard = "";
            FstrDEPT = "";
            FstrDrCode = "";
            FstrEndDate = "";
            mbolACT_OK = false;
        }

        public frmActing(string strPtno, string strInDate, string strDEPT, string strWard)
        {
            InitializeComponent();
            FstrPtno = strPtno;
            FstrInDate = strInDate;
            FstrDEPT = strDEPT;
            mstrWard = strWard;
        }

        public frmActing(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            mstrWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
        }

        private void frmActing_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            //임시
            if (clsType.User.JobGroup != "JOB013005" && clsType.User.JobGroup != "JOB013006")
            {
                button1.Visible = false;
            }

            bolLoad = true;

            int i = 0;

            // 임시 사용
            pSubFormToControl(frmPatientInfoX, panPwInfo);

            ctText.Multiline = true;

            conPatInfo1.Height = 81;

            //lock 로직 글로벌 사용
            clsPublic.GnJobSabun = Convert.ToInt64(clsType.User.Sabun);
            clsPublic.GstrIpAddress = clsCompuInfo.gstrCOMIP;
            clsLockCheck.GstrLockRemark = clsType.User.JobName + " 님이 간호창에서 작업중 !! ";

            mstrDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            mstrTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");

            lblLineAct.Location = new Point(lblLineAct.Location.X, tabControl1.Location.Y);

            if (mstrWard == "" && VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "") != "")
            {
                mstrWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

            cboWard.Text = mstrWard;           

            lblMSG.BackColor = Color.FromArgb(0, 0, 192);
            lblMSG.Text = "";

            panActModify.Visible = false;

            tabControl1.SelectedIndex = 0;

            FstrLineGubun = "";

            panActModify.Visible = false;
            panActModify2.Visible = false;
            panBeginBuse.Visible = false;
            panBeginBuseP.Visible = false;

            mstrACTTable = "KOSMOS_OCS.OCS_IORDER_ACT";

            ssM_Sheet1.Columns.Get(38).Visible = false; //'DC
            ssM_Sheet1.Columns.Get(39).Visible = false; //'DC횟수
            ssM_Sheet1.Columns.Get(47).Visible = false; //'ROWID
            ssM_Sheet1.Columns.Get(48).Visible = false; //'ORDERNO
            ssM_Sheet1.Columns.Get(50).Visible = false; //'itemcd
            ssM_Sheet1.Columns.Get(52).Visible = false; //'참고사항OLD

            dtpDate1.Value = Convert.ToDateTime(mstrDate);

            ComboWard_SET();
            ComFunc.ComboFind(cboWard, "T", 0, mstrWard);
            ssJellco_Sheet1.ColumnCount = 0;

            cboTeam.Items.Clear();
            cboTeam.Items.Add("전체");
            cboTeam.Items.Add("A");
            cboTeam.Items.Add("B");
            cboTeam.Items.Add("지정");
            cboTeam.SelectedIndex = 0;

            cboHH.Items.Clear();
            cboHH.Items.Add("08");
            cboHH.Items.Add("13");
            cboHH.Items.Add("18");
            cboHH.Items.Add("21");

            for (i = 0; i < 24; i++)
            {
                cboHH.Items.Add(i.ToString("00"));
            }
            cboHH.SelectedIndex = 0;

            cboMM.Items.Clear();
            for (i = 0; i < 60; i++)
            {
                cboMM.Items.Add(i.ToString("00"));
            }
            cboMM.SelectedIndex = 0;

            cboOptTime.Items.Clear();
            cboOptTime.Items.Add("08");
            cboOptTime.Items.Add("13");
            cboOptTime.Items.Add("18");
            cboOptTime.Items.Add("21");

            for (i = 0; i < 24; i++)
            {
                cboOptTime.Items.Add(i.ToString("00"));
            }
            cboOptTime.SelectedIndex = 0;

            cboOptSec.Items.Clear();
            for (i = 0; i < 60; i++)
            {
                cboOptSec.Items.Add(i.ToString("00"));
            }
            cboOptSec.SelectedIndex = 0;

            cboJob.Items.Clear();

            if (cboWard.Text.Trim() == "ER")
            {
                cboJob.Items.Add("1.액팅대상리스트");
                cboJob.Items.Add("2.액팅완료리스트");
                cboJob.Items.Add("3.당일내원리스트");
                cboJob.Items.Add("4.당일입원리스트");
                cboJob.Items.Add("5.당일퇴원리스트");
                cboJob.Items.Add("6.당일내시경환자리스트");
            }
            else
            {
                cboJob.Items.Add("1.재원자명단");
                cboJob.Items.Add("2.당일입원자");
                cboJob.Items.Add("3.퇴원예고자");
                cboJob.Items.Add("4.당일퇴원자");
                cboJob.Items.Add("G.응급실경유환자(재원중)");
                cboJob.Items.Add("L.C-Line 유지 환자");
                cboJob.Items.Add("M.항암제투여자");
                cboJob.Items.Add("5.중증도미분류");
                cboJob.Items.Add("6.수술예정자");
                cboJob.Items.Add("7.진단명 누락자");
                cboJob.Items.Add("A.응급실경유입원(1-3일전)");
                cboJob.Items.Add("B.재원기간 7-14일 환자");
                cboJob.Items.Add("C.재원기간 3-7일 환자");
                cboJob.Items.Add("D.어제퇴원자");
                cboJob.Items.Add("E.자가약");
                cboJob.Items.Add("F.기타투약기록");
            }

            cboJob.SelectedIndex = 0;

            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept, "1", 1);

            cboDrCode.Items.Clear();
            cboDrCode.Items.Add("****.전체");
            cboDrCode.SelectedIndex = 0;

            CellBtn1.Text = "유지";
            CellBtn2.Text = "제거";
            CellBtn3.Text = "삭제";
            CellBtn4.Text = "수정";

            if (mstrWard == "ER" || mstrWard == "OP" || mstrWard == "AG" || clsType.User.JobGroup == "JOB018005" ||
            clsType.User.JobGroup == "JOB013007" || clsType.User.JobGroup == "JOB013047")
            //clsType.User.JobGroup == "JOB004003")     // 영상의학과 간호사 제외 처리 COMBOBOX 에 별도 구분 추가함. 2019-02-19
            {
                cboWard.Enabled = true;
            }
            else
            {
                cboWard.Enabled = false;
            }

            //2018-11-23 안정수, 내시경실에서 사용시 병동세팅 되도록 추가함
            if (clsType.User.JobGroup == "JOB006001" || clsType.User.JobGroup == "JOB006002" || clsType.User.JobGroup == "JOB013002" || clsType.User.JobGroup == "JOB013055")
            {
                //내시경실 ENDO, ER 조회가능하도록 2019-08-08
                cboWard.Items.Clear();
                cboWard.Items.Add("ENDO");
                cboWard.Items.Add("ER");
                cboWard.Enabled = true;
                cboWard.SelectedIndex = 0;
                // cboWard.Text = "ENDO";
                // mstrWard = "ENDO";
            }
            else if (clsType.User.JobGroup == "JOB004003") //영상의학과 간호사
            {
                cboWard.Text = "CT/MRI";
                mstrWard = "CT/MRI";
            }


            if (ComQuery.NURSE_System_Manager_Check(VB.Val(clsType.User.Sabun)) == true || clsType.User.JobGroup == "JOB017001")
            {
                btnDeleteDCSession.Visible = true;

                optGB1.Visible = true;

                cboWard.Enabled = true;
            }

            if (mstrWard == "ER")
            {
                label6.Text = "내원일자";
                chkDate.Checked = true;
            }
            else
            {
                label6.Text = "처방일자";
                chkDate.Checked = false;
            }

            optBun0.Checked = true;
            optGB0.Checked = true; //GetData()
           
            bolLoad = false;
            btnView_Click(null, null);
        }

        private string readEMIHOTDT(string strPano, string strIndate, string strWARDCODE)
        {

            if (strWARDCODE != "ER")
            {
                return "";
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strOTDT = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT TO_CHAR(TO_DATE(PTMIOTDT || ' ' || PTMIOTTM,'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI') OTDT ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.VIEW_ER_EMIHPTMI A ";
                SQL += ComNum.VBLF + "  WHERE PTMIINDT = '" + VB.Replace(strIndate, "-", "") + "' ";
                SQL += ComNum.VBLF + "    AND PTMIIDNO = '" + strPano + "' ";
                SQL += ComNum.VBLF + "    AND PTMIDGKD NOT IN ('4')  ";
                SQL += ComNum.VBLF + "    AND TRIM(PTMIOTDT) IS NOT NULL ";
                SQL += ComNum.VBLF + "  ORDER BY PTMIIDNO ASC, PTMIINDT ASC, PTMIINTM ASC ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strOTDT;
                }

                if (dt.Rows.Count > 0)
                {
                    strOTDT = dt.Rows[i]["OTDT"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

            return strOTDT;

        }

        private bool readEMIHOTDT2(string strPano, string strIndate, string strWARDCODE, string strACTTIME)
        {

            //액팅 시간과 퇴실 시간을 체크하는 로직
            // return 값이 true일 때 액팅 가능함.
            // return 값이 false이면 퇴실시간 이후 액팅 건으로 액팅 불가
            // 참고 : 에러 또는 응급실내원내역을 읽지 못할 경우에는 기존조건 체크하도록 .

            if (strWARDCODE != "ER")
            {
                return true;
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            bool bRtn = false;

            string strCount = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT COUNT(*) CNT FROM KOSMOS_PMPA.VIEW_ER_EMIHPTMI ";
                SQL += ComNum.VBLF + "  WHERE PTMIINDT = '" + VB.Replace(strIndate, "-", "") + "' ";
                SQL += ComNum.VBLF + "    AND PTMIIDNO = '" + strPano + "' ";
                SQL += ComNum.VBLF + "    AND PTMIDGKD NOT IN ('4')  ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return bRtn;
                }

                if (dt.Rows.Count > 0)
                {
                    strCount = dt.Rows[i]["CNT"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strCount == "")
                {
                    return bRtn;
                }

                if (VB.Val(strCount) == 1)
                {
                    //기존 조건 타도록(readEMIHOTDT)
                    return bRtn;
                }

                SQL = " SELECT PTMIIDNO ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.VIEW_ER_EMIHPTMI A ";
                SQL += ComNum.VBLF + "  WHERE PTMIINDT = '" + VB.Replace(strIndate, "-", "") + "' ";
                SQL += ComNum.VBLF + "    AND PTMIIDNO = '" + strPano + "' ";
                SQL += ComNum.VBLF + "    AND PTMIDGKD NOT IN ('4')  ";
                SQL += ComNum.VBLF + "    AND (";
                SQL += ComNum.VBLF + "            (PTMIOTDT = ' ' OR PTMIOTDT IS NULL) "; //퇴실내역이 없거나
                //SQL += ComNum.VBLF + "             OR";
                //SQL += ComNum.VBLF + "            (PTMIOTDT IS NOT NULL AND TO_DATE(PTMIOTDT || ' ' || PTMIOTTM,'YYYY-MM-DD HH24:MI') > TO_DATE('" + strACTTIME + "','YYY-MM-DD HH24:MI')) "; // 퇴실시간보다 액팅시간이 빠를 경우
                SQL += ComNum.VBLF + "         )";
                //내원 당일 2회 이상일 경우 최종 내원 시간의 내원내역을 비교
                SQL += ComNum.VBLF + "    AND PTMIINTM = ( ";
                SQL += ComNum.VBLF + "                        SELECT MAX(PTMIINTM) PTMIINTM";
                SQL += ComNum.VBLF + "                          FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI ";
                SQL += ComNum.VBLF + "                         WHERE PTMIINDT = '" + VB.Replace(strIndate, "-", "") + "' ";
                SQL += ComNum.VBLF + "                           AND PTMIIDNO = '" + strPano + "' ";
                SQL += ComNum.VBLF + "                           AND PTMIDGKD NOT IN ('4')  ";
                SQL += ComNum.VBLF + "                   )";
                //SQL += ComNum.VBLF + "  ORDER BY PTMIOTDT ASC, PTMIOTTM ASC ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return bRtn;
                }

                if (dt.Rows.Count > 0)
                {
                    bRtn = true;        //값이 있어야 액팅 가능
                }
                else
                {
                    bRtn = false;       //값이 없으면 액팅 불가능
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                return bRtn;
                
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return bRtn;
            }

            

        }

        private void ComboWard_SET()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT WARDCODE, WARDNAME  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_WARD ";
                SQL = SQL + ComNum.VBLF + "WHERE WARDCODE NOT IN ('IU','NP','2W','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY WARDCODE ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboWard.Items.Clear();

                if (dt.Rows.Count > 0)
                {
                    cboWard.Items.Add("전체");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                    }
                    cboWard.Items.Add("SICU");
                    cboWard.Items.Add("MICU");
                    cboWard.Items.Add("HD");
                    cboWard.Items.Add("OP");
                    cboWard.Items.Add("AG");
                    cboWard.Items.Add("ENDO");
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        public void rGetData(string argPtno, string argIndate, string argDept, string strWard)
        {
            FstrPtno = argPtno;
            FstrInDate = argIndate;
            FstrDEPT = argDept;
            mstrWard = strWard;

            SCREEN_CLEAR();

            GetData();
        }

        public void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (clsLockCheck.GstrLockPtno != "")
            {
                clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
            }

            clsLockCheck.GstrLockPtno = FstrPtno;

            if (optGB1.Checked == true)
            {
                if (clsLockCheck.GstrLockPtno == "")
                {
                    optGB0.Checked = true;
                    return;
                }

                if (clsLockCheck.IpdOcs_Lock_Insert_NEW() != "OK")
                {
                    optGB0.Checked = true;
                    return;
                }

                if (CheckSending() == false)
                {
                    optGB0.Checked = true;
                    return;
                }
            }

            

            mstrBDate = dtpDate1.Value.ToString("yyyy-MM-dd");

            if (FstrDEPT == "HD" && FstrRoom == "200")
            {
                conPatInfo1.SetDisPlay(clsType.User.Sabun, "O", mstrBDate, FstrPtno, FstrDEPT);
            }
            else
            {
                conPatInfo1.SetDisPlay(clsType.User.Sabun, "I", mstrBDate, FstrPtno, FstrDEPT);
            }

            frmPatientInfoX.rGetDate(FstrPtno, Convert.ToString(mintIPDNO));

            //string strName = "";
            string strBun = "";
            //string strInDate = "";

            //TODO : 예외처리
            //If Row = 0 Or Col = 0 Then Exit Sub
            //If Col = 14 Then Exit Sub

            //자가약
            panTitleSub1.Visible = false;
            panSelf.Visible = false;
            expandableSplitter3.Visible = false;
            panel2.Dock = DockStyle.Fill;
            ssS.Visible = false;

            lblMSG.BackColor = Color.FromArgb(0, 0, 192);
            lblMSG.Text = "";

            lblMSG.Text = READ_ALLERGY(FstrPtno);

            if (lblMSG.Text.Trim() != "")
            {
                // TODO : TimerMSG.Enabled = True
            }

            if (optBun0.Checked == true)
            {
                strBun = "'11','12'";
            }
            else if (optBun1.Checked == true)
            {
                strBun = "'20','23' ";
            }
            else if (optBun2.Checked == true)
            {
                strBun = "'22','23','24','25','26','27','28','29',"
                       + "'31','32','33','34','35','36','37','38','39',"
                       + "'41','42','43','44','45','46','47','48','49',"
                       + "'51','52','53','54','55','56','57','58','59',"
                       + "'61','62','63','64','65','66','67','68','69',"
                       + "'70','71','72','73'";
            }

            if (mstrWard == "ER")
            {
                strBun = "'11','12','20','23'";
            }

            //LAB
            //If optBun(3).Value = True Then strBun = strBun & IIf(strBun <> "", ",", "") & "'52','53','54','55','56','57','58','59','60','61','62','63'"

            if (optBun4.Checked == true)
            {
                strBun = "중증응급가산";
            }

            btnActOK.Visible = false;
            btnActOK.Text = "액팅완료";

            if (mstrWard == "ER" || strBun == "중증응급가산")
            {
                btnActOK.Visible = true;

                if (mbolACT_OK == false)
                {
                    btnActOK.Text = "완료취소";
                }
            }

            btnExamDrug.Visible = READ_EXAMDRUG();

            chkInOit.Checked = true;
            lblINOUT.Text = READ_INOUT(FstrPtno);
            FbBtnClick = true;
            READ_OCS_IORDER_MUTI(FstrPtno, strBun);
            FbBtnClick = false;

            //'=======================================
            if (optBun0.Checked == true || optBun1.Checked == true)
            {
                READ_SELFMED(FstrPtno, FstrInDate);
            }
            //'=======================================

            READ_JELLCO(FstrPtno, FstrInDate, mstrDate);

            if (mstrWard == "ER")
            {
                cboHH.Text = VB.Left(mstrTime, 2);
                cboMM.Text = VB.Right(mstrTime, 2);
            }

            READ_CENTRAL_KEEP(FstrPtno, FstrInDate);

            CENTRAL_LINE_YN();

            if (FstrLineGubun == "중심정맥관")
            {
                SSLINE1_CLEAR();
                btnSreachKeepL1_Click(null, null);
                READ_LINE1_HISTORY("");
            }
            else if (FstrLineGubun == "말초정맥관")
            {
                SSLINEP1_CLEAR();
                btnSreachKeepLP1_Click(null, null);
                READ_LINE1P_HISTORY("");
            }

            ssM.Visible = true;

            this.Enabled = true;
        }

        private bool CheckSending()
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                //SQL = "";
                //SQL = " SELECT A.*, TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE1, SLIPNO, A.ROWID , ";
                //SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE1  ";
                //SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_IORDER  A";
                //SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO     = '" + FstrPtno + "' ";
                //SQL = SQL + ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + mstrBDate + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "  AND A.GBSEND ='*' ";
                //SQL = SQL + ComNum.VBLF + "  AND A.GBPICKUP = '*'       ";

                ////    '2014-02-27
                //if (cboWard.Text.Trim() == "ER")
                //{
                //    SQL = SQL + ComNum.VBLF + "  AND GBIOE IN ('EI') ";
                //}

                SQL = "";
                SQL += " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE1                                                                          \r";
                SQL += "   FROM KOSMOS_OCS.OCS_IORDER                                                                                       \r";
                SQL += "  WHERE Ptno       = '" + FstrPtno + "'                                                                             \r";
                SQL += "    AND BDATE      = TO_DATE('" + mstrBDate + "', 'YYYY-MM-DD')                                                     \r";
                SQL += "    AND DEPTCODE  != 'ER'                                                                                           \r";
                //2019-02-26
                //SQL += "    AND ((GBSEND = ' ' AND (ACCSEND IS NULL or ACCSEND = 'Z')) OR GBSEND = '*')                                     \r";
                SQL += "    AND ( ";
                //2020-06-01 금액샌드 체크 부분은 제외
                //SQL += "         (SUCODE NOT IN ('C-OT','C-PC','C-EN') AND ((GBSEND = ' ' AND (ACCSEND IS NULL or ACCSEND = 'Z') ) OR GBSEND = '*'))";
                //SQL += "      OR (SUCODE IN ('C-OT','C-PC','C-EN') AND DRCODE2 IS NOT NULL AND ((GBSEND = ' ' AND (ACCSEND IS NULL or ACCSEND = 'Z')) OR GBSEND = '*')) ";
                SQL += "         (SUCODE NOT IN ('C-OT','C-PC','C-EN') AND GBSEND = '*')";
                SQL += "      OR (SUCODE IN ('C-OT','C-PC','C-EN') AND DRCODE2 IS NOT NULL AND GBSEND = '*') ";
                SQL += "        )";
                SQL += "    AND ORDERSITE not in ('NDC', 'ERO')                                                                             \r";
                SQL += "    AND GBPRN != 'P'                                                                                                \r";
                SQL += "    AND GBSTATUS != 'D'                                                                                             \r";
                SQL += "    AND SUCODE IS NOT NULL                                                                                          \r";
                SQL += "    AND SLIPNO != '0106'                                                                                            \r";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox(dt.Rows[0]["BDATE1"].ToString().Trim() + "일자에 미 전송된 오더가 있습니다.잠시후에 작업 해주세요..", "확인");
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                dt.Dispose();
                dt = null;
                return true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private void READ_LINE1P_HISTORY(string ArgSeqno)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssLineP3_Sheet1.RowCount = 0;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT STATUS, INSERT_BUSE, DUTY, TO_CHAR(STARTDATE, 'YYYY-MM-DD HH24:MI') STARTDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(ACTDATE, 'YYYY-MM-DD HH24:MI') ACTDATE, STATUS, PART1 || ' ' || PART2 PART,";
                SQL = SQL + ComNum.VBLF + " LOCATE1 || ' ' || LOCATE2 LOCATE,";
                SQL = SQL + ComNum.VBLF + " DECODE(MONITOR1, '1', '발적', '') || ' ' ||";
                SQL = SQL + ComNum.VBLF + " DECODE(MONITOR2, '1', '부종', '') || ' ' ||";
                SQL = SQL + ComNum.VBLF + " DECODE(MONITOR3, '1', '삼출물', '') || ' ' ||";
                SQL = SQL + ComNum.VBLF + " DECODE(MONITOR4, '1', '압통', '') || ' ' ||";
                SQL = SQL + ComNum.VBLF + " DECODE(MONITOR6, '1', '열감', '') || ' ' ||";
                SQL = SQL + ComNum.VBLF + " DECODE(MONITOR5, '1', '이상없음', '') MONITOR, D_PART,";
                SQL = SQL + ComNum.VBLF + " DECODE(D_STATUS1, 'Y', '건조됨', '건조되지 않음') || ' ' || DECODE(D_STATUS2, '양호', '부착됨', '떨어짐') D_STATUS, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(D_DATE, 'YYYY-MM-DD HH24:MI') D_DATE, ACTSABUN, ROWID, SEQNO, USEDATE, BIGO";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPtno + "'";
                SQL = SQL + ComNum.VBLF + "       AND ( IPDNO = " + mintIPDNO + " OR IPDNO = 0 ) ";

                if (ArgSeqno != "")
                {
                    SQL = SQL + ComNum.VBLF + "      AND SEQNO = " + ArgSeqno;
                }

                SQL = SQL + ComNum.VBLF + "      AND ACTDATE >= TRUNC(SYSDATE-6)";
                SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssLineP3_Sheet1.RowCount = dt.Rows.Count;
                    ssLineP3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssLineP3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        ssLineP3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DUTY"].ToString().Trim();
                        ssLineP3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["INSERT_BUSE"].ToString().Trim();
                        ssLineP3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["STATUS"].ToString().Trim();
                        ssLineP3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PART"].ToString().Trim();
                        ssLineP3_Sheet1.Cells[i, 5].Text = dt.Rows[i]["LOCATE"].ToString().Trim();
                        ssLineP3_Sheet1.Cells[i, 6].Text = dt.Rows[i]["STARTDATE"].ToString().Trim();
                        ssLineP3_Sheet1.Cells[i, 7].Text = dt.Rows[i]["USEDATE"].ToString().Trim();
                        ssLineP3_Sheet1.Cells[i, 8].Text = dt.Rows[i]["MONITOR"].ToString().Trim();
                        ssLineP3_Sheet1.Cells[i, 9].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["ACTSABUN"].ToString().Trim());
                        ssLineP3_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssLineP3_Sheet1.Cells[i, 11].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssLineP3_Sheet1.Cells[i, 12].Text = dt.Rows[i]["BIGO"].ToString().Trim();

                        if (KEEP_DATA_BOLD(dt.Rows[i]["SEQNO"].ToString().Trim()) == true)
                        {
                            ssLineP3_Sheet1.Rows.Get(i).Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;   
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void SSLINEP1_CLEAR()
        {
            ssLineP1_Sheet1.Cells[2, 0].Text = "";
            ssLineP1_Sheet1.Cells[2, 1].Text = "";
            ssLineP1_Sheet1.Cells[2, 2].Text = "";
            ssLineP1_Sheet1.Cells[2, 3].Text = "";
            ssLineP1_Sheet1.Cells[2, 4].Text = "";
            ssLineP1_Sheet1.Cells[2, 5].Value = false;
            ssLineP1_Sheet1.Cells[2, 6].Value = false;
            ssLineP1_Sheet1.Cells[2, 7].Value = false;
            ssLineP1_Sheet1.Cells[2, 8].Value = false;
            ssLineP1_Sheet1.Cells[2, 9].Value = false;
            ssLineP1_Sheet1.Cells[2, 10].Text = "";
        }

        private void READ_LINE1_HISTORY(string ArgSeqno)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssLine3_Sheet1.RowCount = 0;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT STATUS, INSERT_BUSE, DUTY, TO_CHAR(STARTDATE, 'YYYY-MM-DD HH24:MI') STARTDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(ACTDATE, 'YYYY-MM-DD HH24:MI') ACTDATE, STATUS, PART1 || ' ' || PART2 PART,";
                SQL = SQL + ComNum.VBLF + " LOCATE1 || ' ' || LOCATE2 LOCATE,";
                SQL = SQL + ComNum.VBLF + " DECODE(MONITOR1, '1', '발적', '') || ' ' ||";
                SQL = SQL + ComNum.VBLF + " DECODE(MONITOR2, '1', '부종', '') || ' ' ||";
                SQL = SQL + ComNum.VBLF + " DECODE(MONITOR3, '1', '삼출물', '') || ' ' ||";
                SQL = SQL + ComNum.VBLF + " DECODE(MONITOR4, '1', '압통', '') || ' ' ||";
                SQL = SQL + ComNum.VBLF + " DECODE(MONITOR5, '1', '이상없음', '') MONITOR, D_PART,";
                SQL = SQL + ComNum.VBLF + " DECODE(D_STATUS1, 'Y', '건조됨', '건조되지 않음') || ' ' || DECODE(D_STATUS2, '양호', '부착됨', '떨어짐') D_STATUS, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(D_DATE, 'YYYY-MM-DD HH24:MI') D_DATE, ACTSABUN, ROWID, SEQNO, USEDATE,";
                SQL = SQL + ComNum.VBLF + " HAND_HYGN, HERB_STRL, ASEPTIC_TECH, DRS_NAME, NEED_ASSES ";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPtno + "'";
                if (cboWard.Text.Trim() == "33")
                {
                    SQL = SQL + ComNum.VBLF + "       AND (IPDNO = " + mintIPDNO + "OR IPDNO = 0)";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + mintIPDNO;
                }

                if (ArgSeqno != "")
                {
                    SQL = SQL + ComNum.VBLF + "      AND SEQNO = " + ArgSeqno;
                }

                SQL = SQL + ComNum.VBLF + "      AND ACTDATE >= TRUNC(SYSDATE-6)";
                SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssLine3_Sheet1.RowCount = dt.Rows.Count;
                    ssLine3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssLine3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DUTY"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["INSERT_BUSE"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["STATUS"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PART"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 5].Text = dt.Rows[i]["LOCATE"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 6].Text = dt.Rows[i]["STARTDATE"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 7].Text = dt.Rows[i]["USEDATE"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 8].Text = dt.Rows[i]["HAND_HYGN"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 9].Text = dt.Rows[i]["HERB_STRL"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ASEPTIC_TECH"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 8+3].Text = dt.Rows[i]["MONITOR"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 9+3].Text = dt.Rows[i]["D_DATE"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 10+3].Text = dt.Rows[i]["D_PART"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 14].Text = dt.Rows[i]["DRS_NAME"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 11+4].Text = dt.Rows[i]["D_STATUS"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 16].Text = dt.Rows[i]["NEED_ASSES"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 12+5].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["ACTSABUN"].ToString().Trim());
                        ssLine3_Sheet1.Cells[i, 13+5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssLine3_Sheet1.Cells[i, 14+5].Text = dt.Rows[i]["SEQNO"].ToString().Trim();

                        if (KEEP_DATA_BOLD(dt.Rows[i]["SEQNO"].ToString().Trim()) == true)
                        {
                            ssLine3_Sheet1.Rows.Get(i).Font = new Font("맑은 고딕", 8, FontStyle.Bold);
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private bool KEEP_DATA_BOLD(string ArgSeqno)
        {
            bool rtmVal = false;
            int i = 0;

            for (i = 2; i < ssLine2_Sheet1.RowCount; i++)
            {
                //if (ssLine2_Sheet1.Cells[i, 20].Text.Trim() == ArgSeqno)
                if (ssLine2_Sheet1.Cells[i, (int)(enumColumn_LineAct.SEQNO)].Text.Trim() == ArgSeqno)
                {
                    rtmVal = true;
                    break;
                }
            }

            return rtmVal;
        }

        private void SSLINE1_CLEAR()
        {
            ssLine1_Sheet1.Cells[2, 0].Text = "비터널식";
            ssLine1_Sheet1.Cells[2, 1].Text = "";
            clsSpread.gSpreadComboDataSetEx(ssLine1, 2, 1, 2, 1, VB.Split("CVC(non HD)//PICC//Temp, HD cath//Swan-Ganz", "//"));
            ssLine1_Sheet1.Cells[2, 2].Text = "";
            ssLine1_Sheet1.Cells[2, 3].Text = "";
            ssLine1_Sheet1.Cells[2, 4].Value = false;
            ssLine1_Sheet1.Cells[2, 5].Value = false;
            ssLine1_Sheet1.Cells[2, 6].Value = false;
            ssLine1_Sheet1.Cells[2, 7].Value = false;
            ssLine1_Sheet1.Cells[2, 8].Value = false;
            ssLine1_Sheet1.Cells[2, 9].Text = "";
            ssLine1_Sheet1.Cells[2, 10].Text = "";
            ssLine1_Sheet1.Cells[2, 11].Text = "";
            ssLine1_Sheet1.Cells[2, 12].Text = "";
            ssLine1_Sheet1.Cells[2, 13].Text = "";
        }

        private void CENTRAL_LINE_YN()
        {
            lblLineAct.Visible = false;

            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL MST";
                SQL = SQL + ComNum.VBLF + "  WHERE  EXISTS (";
                SQL = SQL + ComNum.VBLF + "  SELECT ACTDATE, SEQNO FROM (";
                SQL = SQL + ComNum.VBLF + "  SELECT MAX(ACTDATE) ACTDATE, SEQNO";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL";
                SQL = SQL + ComNum.VBLF + "  GROUP BY SEQNO ) SUB";
                SQL = SQL + ComNum.VBLF + "  WHERE MST.ACTDATE = SUB.ACTDATE";
                SQL = SQL + ComNum.VBLF + "       AND MST.SEQNO = SUB.SEQNO)";
                SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + mintIPDNO;
                SQL = SQL + ComNum.VBLF + "       AND PANO = '" + FstrPtno + "'";
                SQL = SQL + ComNum.VBLF + "       AND STATUS IN ('삽입','유지')";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblLineAct.Visible = true;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void READ_CENTRAL_KEEP(string argPTNO, string ArgInDate)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT MAX(ACTDATE) ACTDATE, A.SEQNO, B.CHECK2, PART1 || ' ' || PART2 PART, LOCATE1 || ' ' || LOCATE2 LOCATE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL A, KOSMOS_PMPA.NUR_LINE_ALERT_CENTRAL B";
                SQL = SQL + ComNum.VBLF + " WHERE A.SEQNO = B.SEQNO(+)";
                SQL = SQL + ComNum.VBLF + "      AND A.PANO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + "    AND ACTDATE >= TO_DATE('" + ArgInDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "      AND NOT EXISTS ( SELECT *";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL SUB";
                SQL = SQL + ComNum.VBLF + " WHERE A.SEQNO = SUB.SEQNO";
                SQL = SQL + ComNum.VBLF + "      AND SUB.STATUS = '제거')";
                SQL = SQL + ComNum.VBLF + "      AND B.CHECK2 = '1'";
                SQL = SQL + ComNum.VBLF + "      AND TRUNC(B.WRITEDATE) = TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.SEQNO, B.CHECK2, PART1 || ' ' || PART2, LOCATE1 || ' ' || LOCATE2";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("★☆ 선택한 환자의 해당 중심정맥관을 제거하십시요.☆★" + ComNum.VBLF + ComNum.VBLF +
                        "★ 종류 : " + dt.Rows[0]["PART"].ToString().Trim() + ComNum.VBLF +
                        "★ 위치 : " + dt.Rows[0]["LOCATE"].ToString().Trim(), "확인");
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void READ_JELLCO(string argPTNO, string ArgInDate, string argChartDate)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            string strTEMP1 = "";
            string strTEMP2 = "";

            optArm.Checked = false;
            optLeg.Checked = false;

            optRight.Checked = false;
            optLeft.Checked = false;
            optBoth.Checked = false;

            btnJellcoDel.Visible = false;
            btnJellcoUpdate.Visible = false;
            ChkOptionTime.Checked = false;
            txtJellcoRowid.Text = "";
            dtpOptDate.CalendarForeColor = Color.FromArgb(0, 0, 0);
            cboOptTime.ForeColor = Color.FromArgb(0, 0, 0);
            cboOptSec.ForeColor = Color.FromArgb(0, 0, 0);

            ssJellco_Sheet1.ColumnCount = 0;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(CHARTDATE, 'YYYY-MM-DD') CHARTDATE, TO_CHAR(CHARTDATE, 'HH24:MI') CHARTTIME, GUBUN1, GUBUN2, ROWID, KEEP ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_JELLCO ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + ArgInDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + ArgInDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= TO_DATE('" + Convert.ToDateTime(argChartDate).AddDays(-30).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY CHARTDATE DESC, CHARTTIME DESC ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssJellco_Sheet1.ColumnCount = dt.Rows.Count;
                    ssJellco_Sheet1.SetRowHeight(-1, 18);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssJellco_Sheet1.Columns.Get(i).Label = dt.Rows[i]["CHARTDATE"].ToString().Trim();

                        ssJellco_Sheet1.Cells[0, i].Text = dt.Rows[i]["CHARTTIME"].ToString().Trim() + (dt.Rows[i]["CHARTTIME"].ToString().Trim() == "1" ? "(Keep)" : "");

                        switch (dt.Rows[i]["GUBUN1"].ToString().Trim())
                        {
                            case "0":
                                strTEMP1 = "Arm";
                                break;

                            case "1":
                                strTEMP1 = "Leg";
                                break;

                            case "X":
                                strTEMP1 = "Remove";
                                break;
                        }

                        switch (dt.Rows[i]["GUBUN2"].ToString().Trim())
                        {
                            case "0":
                                strTEMP2 = "Right";
                                break;

                            case "1":
                                strTEMP2 = "Left";
                                break;

                            case "2":
                                strTEMP2 = "Both";
                                break;

                            case "X":
                                strTEMP2 = "";
                                break;
                        }

                        ssJellco_Sheet1.Cells[1, i].Text = strTEMP1 + " " + strTEMP2;
                        ssJellco_Sheet1.Cells[2, i].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        if (dt.Rows[i]["GUBUN2"].ToString().Trim() == "1")
                        {
                            ssJellco_Sheet1.Columns.Get(i).BackColor = Color.FromArgb(255, 200, 200);
                        }
                        else
                        {
                            ssJellco_Sheet1.Columns.Get(i).BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void READ_SELFMED(string argPTNO, string ArgInDate)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            //int nREADM = 0;
            //string strQty = "";
            //string strDOSNAME = "";

            double nActCnt = 0;

            Cursor.Current = Cursors.WaitCursor;

            ssS_Sheet1.RowCount = 0;

            ArgInDate = ArgInDate.Replace("-", "");

            try
            {
                SQL = " SELECT GBN, DRUGCODE, DRUGNAME, DOSNAME, ";
                SQL = SQL + ComNum.VBLF + " QTY, TIMES, QTYTIMES, DECODE(CONTENTS, NULL, QTY, CONTENTS) CONTENTS,  ";
                SQL = SQL + ComNum.VBLF + " SEQNO, ROWID";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_CADEX_SELFMED";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE = '" + ArgInDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY DRUGCODE, DRUGNAME ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssS_Sheet1.RowCount = dt.Rows.Count;

                    //자가약
                    panTitleSub1.Visible = true;
                    panSelf.Visible = true;
                    expandableSplitter3.Visible = true;
                    panel2.Dock = DockStyle.Top;

                    ssS.Visible = true;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["GBN"].ToString().Trim())
                        {
                            case "O":
                                ssS_Sheet1.Cells[i, 0].Text = "외래처방약";
                                break;

                            case "H":
                                ssS_Sheet1.Cells[i, 0].Text = "약제과식별";
                                break;

                            case "T":
                                ssS_Sheet1.Cells[i, 0].Text = "병원퇴원약";
                                break;

                            default:
                                ssS_Sheet1.Cells[i, 0].Text = "";
                                break;
                        }

                        ssS_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DRUGCODE"].ToString().Trim();
                        ssS_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DRUGNAME"].ToString().Trim();
                        ssS_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();
                        ssS_Sheet1.Cells[i, 4].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ssS_Sheet1.Cells[i, 5].Text = dt.Rows[i]["TIMES"].ToString().Trim();
                        ssS_Sheet1.Cells[i, 6].Text = dt.Rows[i]["QTYTIMES"].ToString().Trim();
                        nActCnt = READ_ACT_CNT(VB.Val(dt.Rows[i]["SEQNO"].ToString().Trim()), dtpDate1.Value.ToString("yyyy-MM-dd"));
                        ssS_Sheet1.Cells[i, 7].Text = Convert.ToString(nActCnt);

                        ssS_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssS_Sheet1.Cells[i, 13].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ssS_Sheet1.Cells[i, 14].Text = dt.Rows[i]["SEQNO"].ToString().Trim();

                        SET_ACT_TABLE(Convert.ToInt32(VB.Val(dt.Rows[i]["TIMES"].ToString().Trim())), i, nActCnt, Convert.ToInt32(VB.Val(dt.Rows[i]["SEQNO"].ToString().Trim())), dtpDate1.Value.ToString("yyyy-MM-dd"));

                        ssS_Sheet1.SetRowHeight(i, Convert.ToInt32(ssS_Sheet1.GetPreferredRowHeight(i)));
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void SET_ACT_TABLE(int argTimes, int argROW, double argActCnt, int ArgSeqno, string ArgDate)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader Rs1 = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ctText.WordWrap = true;
                for (i = 8; i < 12; i++)
                {
                    //TODO : 확인하기 - 영록
                    if (ssS_Sheet1.Cells[argROW, i].CellType == null || ssS_Sheet1.Cells[argROW, i].CellType.ToString() != "TextCellType")
                    {
                        ssS_Sheet1.Cells[argROW, i].CellType = ctText;
                    }
                    ssS_Sheet1.Cells[argROW, i].Text = "";
                    ssS_Sheet1.Cells[argROW, i].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssS_Sheet1.Cells[argROW, i].VerticalAlignment = CellVerticalAlignment.Center;
                    ssS_Sheet1.Cells[argROW, i].BackColor = Color.FromArgb(255, 255, 200);
                    ssS_Sheet1.Cells[argROW, i].Font = new Font("맑은 고딕", 9, FontStyle.Regular);
                }

                if (VB.IsNumeric(argTimes) == true || argTimes == 0)
                {
                    SQL = "";
                    SQL = " SELECT A.ROWID, TO_CHAR(A.ACTDATE, 'DD') ACTDATE, TO_CHAR(A.ACTDATE, 'HH24:MI') ACTTIME, A.ACTSABUN";
                    SQL = SQL + ComNum.VBLF + "    , (SELECT  MAX(KORNAME) FROM KOSMOS_ADM.INSA_MST BB1";
                    SQL = SQL + ComNum.VBLF + "                WHERE BB1.SABUN = A.ACTSABUN";
                    SQL = SQL + ComNum.VBLF + "                AND ( BB1.TOIDAY IS NULL OR BB1.TOIDAY < TRUNC(SYSDATE)) ) AS ACTNAME";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_CADEX_SELFMED_ACT A";
                    SQL = SQL + ComNum.VBLF + " WHERE A.SEQNO = " + ArgSeqno;
                    SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE >= TO_DATE('" + ArgDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + ArgDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                    SQL = SQL + ComNum.VBLF + "   ORDER BY A.ACTDATE ASC ";

                    SqlErr = clsDB.GetAdoRs(ref Rs1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (Rs1.HasRows == true)
                    {
                        //for (i = 0; i < dt.Rows.Count; i++)
                        //{
                        //    ssS_Sheet1.Cells[argROW, i + 8].BackColor = Color.FromArgb(255, 200, 100);
                        //    ssS_Sheet1.Cells[argROW, i + 8].Font = new Font(ssS_Sheet1.Cells[argROW, i + 8].Font, FontStyle.Bold);
                        //    //ssS_Sheet1.Cells[argROW, i + 8].Text = clsOpdNr.READ_INSA_NAME(clsDB.DbCon, VB.Val(dt.Rows[i]["ACTSABUN"].ToString().Trim()).ToString("00000"))
                        //    //+ ComNum.VBLF + dt.Rows[i]["ACTDATE"].ToString().Trim() + " " + dt.Rows[i]["ACTTIME"].ToString().Trim();
                        //    ssS_Sheet1.Cells[argROW, i + 8 + 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        //}
                        i = 0;
                        while (Rs1.Read())
                        {
                            ssS_Sheet1.Cells[argROW, i + 8].BackColor = Color.FromArgb(255, 200, 100);
                            ssS_Sheet1.Cells[argROW, i + 8].Font = new Font(ssS_Sheet1.Cells[argROW, i + 8].Font, FontStyle.Bold);

                            ssS_Sheet1.Cells[argROW, i + 8].Text = Rs1.GetValue(4).ToString().Trim() + ComNum.VBLF + Rs1.GetValue(1).ToString().Trim() + " " + Rs1.GetValue(2).ToString().Trim();
                            ssS_Sheet1.Cells[argROW, i + 8 + 7].Text = Rs1.GetValue(0).ToString().Trim();
                            i = i + 1;
                        }
                    }

                    Rs1.Dispose();
                    Rs1 = null;

                    if (argActCnt < argTimes)
                    {
                        for (i = Convert.ToInt32(8 + argActCnt); i < 8 + argTimes; i++)
                        {
                            ssS_Sheet1.Cells[argROW, i].CellType = ctCheck;
                            ssS_Sheet1.Cells[argROW, i].HorizontalAlignment = CellHorizontalAlignment.Center;
                            ssS_Sheet1.Cells[argROW, i].VerticalAlignment = CellVerticalAlignment.Center;
                            ssS_Sheet1.Cells[argROW, i].ForeColor = Color.FromArgb(0, 0, 0);
                            ssS_Sheet1.Cells[argROW, i].BackColor = Color.FromArgb(255, 200, 200);
                            ssS_Sheet1.Cells[argROW, i].Value = false;
                        }
                    }
                }

                ctText.WordWrap = false;

                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        private double READ_ACT_CNT(double ArgSeqno, string ArgDate)
        {
            double RtnVal = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_CADEX_SELFMED_ACT ";
                SQL = SQL + ComNum.VBLF + " WHERE SEQNO = " + ArgSeqno;
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + ArgDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + ArgDate + " 23:59','YYYY-MM-DD HH24:MI') ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = VB.Val(dt.Rows[0]["CNT"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return RtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return RtnVal;
            }
        }

        private void READ_OCS_IORDER_MUTI_1111(string argPTNO, string argBun)
        {
            int nMaxDiv = 0;

            string strUnit = "";
            double nBContents = 0;
            double nContents = 0;
            int nDiv = 0;
            double nDivQty = 0;

            double nUnitNew1 = 0;
            string strUnitNew2 = "";
            string strUnitNew3 = "";
            double nUnitNew4 = 0;

            string strBDATE = "";

            int i = 0;
            int h = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            OracleDataReader Rs1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssM_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(M.INDATE,'YYYY-MM-DD') INDATE, M.WARDCODE,   M.ROOMCODE,   M.PANO,       M.SNAME,      M.SEX,       ";
                SQL = SQL + ComNum.VBLF + "        M.AGE,        M.DEPTCODE,   O.SLIPNO,     O.ORDERCODE, O.POWDER, O.GBER, O.GBPRN,  ";
                SQL = SQL + ComNum.VBLF + "        C.ORDERNAME,  C.ORDERNAMES, C.DISPHEADER, O.CONTENTS,  O.DCDIV, O.GBSELF, O.GBNGT, ";
                SQL = SQL + ComNum.VBLF + "        O.BCONTENTS,  O.REALQTY,    O.DOSCODE,    D.DOSNAME,   D.GBDIV, O.NURREMARK, ";
                SQL = SQL + ComNum.VBLF + "        O.GBGROUP,    O.REMARK,     O.GBINFO,     O.GBTFLAG,   O.BUN,   O.ACTDIV, ";
                SQL = SQL + ComNum.VBLF + "        O.GBPRN,      O.GBORDER,    O.ORDERSITE,  O.ORDERNO,   TO_CHAR(O.BDATE,'YYYY-MM-DD') BDATE , O.GBSTATUS,  ";
                SQL = SQL + ComNum.VBLF + "        O.GBPORT,     N.DRNAME,     O.SUCODE,     O.QTY,  O.BUN ,O.NAL, O.ROWID, ";
                SQL = SQL + ComNum.VBLF + "        S.UNITNEW1,   S.UNITNEW2,   S.UNITNEW3,   S.UNITNEW4, C.DISPRGB, C.ITEMCD, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(O.PICKUPDATE,'YYYY-MM-DD HH24:MI') PICKUPDATE, S.SUNAMEK ,O.PICKUPSABUN, TO_CHAR(O.ENTDATE, 'YYYY-MM-DD') AS ENTDATE, ";
                SQL = SQL + ComNum.VBLF + " O.GBIOE ";      //2019-02-14 응급실 NDC 용
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_IORDER O, ";

                if (VB.Left(cboJob.Text, 1) == "D")
                {
                    SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.IPD_NEW_MASTER  M,                           ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " (SELECT ACTDATE, INDATE, OUTDATE, GBSTS, WARDCODE, ROOMCODE, PANO, SNAME, SEX, AGE, DEPTCODE, IPDNO ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + "  WHERE IPDNO = (SELECT ";
                    SQL = SQL + ComNum.VBLF + "                     MAX(IPDNO) ";
                    SQL = SQL + ComNum.VBLF + "                  FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + "                  WHERE(ACTDATE IS NULL OR OUTDATE = TRUNC(SYSDATE))   ";
                    SQL = SQL + ComNum.VBLF + " 		         AND PANO = '" + argPTNO + "')) M,                 ";
                }

                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P,                           ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_ORDERCODE           C,                           ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_ODOSAGE             D,                           ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_DOCTOR              N,                            ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_SUN     S                          ";
                SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + argPTNO + "'";

                if (argBun == "중증응급가산")
                {
                    SQL = SQL + ComNum.VBLF + "   AND O.SUCODE IN (SELECT SUNEXT FROM KOSMOS_PMPA.BAS_SUN WHERE SUGBAA IN ('1','2','3'))";
                    SQL = SQL + ComNum.VBLF + "   AND O.BUN IN ( '28','30','34','35','38','40')";
                    SQL = SQL + ComNum.VBLF + "   AND O.BDATE >= TRUNC(M.INDATE) AND O.BDATE <= TRUNC(M.INDATE + 1)";
                    SQL = SQL + ComNum.VBLF + "   AND EXISTS ( SELECT SUB1.PANO";     //'NUR_MASTER  ACT_OK ='1' 이면 중증응급가상 미비된 엑팅 오더 보이지 않음;
                    SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_MASTER SUB1";
                    SQL = SQL + ComNum.VBLF + "   WHERE SUB1.PANO = M.PANO";
                    SQL = SQL + ComNum.VBLF + "   AND SUB1.IPDNO = M.IPDNO";
                    SQL = SQL + ComNum.VBLF + "   AND SUB1.ACT_OK IS NULL)";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND O.BUN IN ( " + argBun + " ) ";
                    SQL = SQL + ComNum.VBLF + "  AND BDATE >= TO_DATE('" + dtpDate1.Value.AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND BDATE <= TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                }

                SQL = SQL + ComNum.VBLF + "   AND  (O.GBPRN IN  NULL OR O.GBPRN <> 'P') ";

                if (mstrWard == "ER")
                {
                    SQL = SQL + ComNum.VBLF + " AND  (  O.ORDERSITE  IN ('ERO')   OR  O.GBIOE IN ('E','EI') )";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND  (O.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') OR O.ORDERSITE IS NULL )";
                }

                SQL = SQL + ComNum.VBLF + " AND    O.GBPRN <>'S' "; //'JJY 추가(2000/05/22 'S는 선수납(선불);
                SQL = SQL + ComNum.VBLF + " AND   ( O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND   ACTDIV >'0' ) )  ";
                SQL = SQL + ComNum.VBLF + " AND    O.PTNO       =  M.PANO           ";
                SQL = SQL + ComNum.VBLF + "  AND  O.GBPICKUP = '*' ";
                SQL = SQL + ComNum.VBLF + "  AND  ( O.VERBC IS NULL OR O.VERBC <>'Y' )";

                if (mstrWard == "HD")
                {
                    SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
                }
                else if (mstrWard == "ENDO")
                {
                    SQL = SQL + ComNum.VBLF + "AND O.DOSCODE IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE WARDCODE IN ( 'EN') ) ";
                    SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
                }
                else if (mstrWard == "CT/MRI")
                {
                    SQL = SQL + ComNum.VBLF + "AND O.DOSCODE IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE WARDCODE IN ( 'RD') ) ";
                    SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
                }
                else if (mstrWard == "OP" || mstrWard  == "AG")
                {
                    //    '수술방은 모든 오더 보이도록 처리 추후 보완 예정;
                }
                else if (mstrWard == "RA")
                {
                    SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
                }
                else if (mstrWard == "MICU")
                {
                    SQL = SQL + ComNum.VBLF + " AND M.WARDCODE ='IU'";
                    SQL = SQL + ComNum.VBLF + " AND M.ROOMCODE ='234'";
                    SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
                    if (mstrWard == "SICU")
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.WARDCODE ='IU'   ";
                        SQL = SQL + ComNum.VBLF + " AND M.ROOMCODE ='233'";
                    }
                    else if (mstrWard != "ER")
                    {
                        if (mstrWard == "IQ" || mstrWard == "ND" || mstrWard == "NR")
                        {
                            SQL = SQL + ComNum.VBLF + " AND  M.WARDCODE IN ('IQ','ND','NR')";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " AND  M.WARDCODE = '" + mstrWard.Trim() + "' ";
                        }
                    }
                }

                SQL = SQL + ComNum.VBLF + "  AND   O.QTY  <>  '0'    ";
                SQL = SQL + ComNum.VBLF + "  AND    M.GBSTS NOT IN  ('9') "; //" '입원취소 제외;

                if (VB.Left(cboJob.Text.Trim(), 1) == "D") //'어제퇴원자;
                {
                    SQL = SQL + ComNum.VBLF + " AND  M.OUTDATE = TRUNC(SYSDATE -1) ";  //'계산 완료 환자도 ACTING 은 가능;
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND  (M.ACTDATE IS NULL OR M.OUTDATE =TRUNC(SYSDATE))  ";   //'계산 완료 환자도 ACTING 은 가능;
                }

                SQL = SQL + ComNum.VBLF + " AND    O.PTNO       =  P.PANO(+)        ";
                SQL = SQL + ComNum.VBLF + " AND    O.SLIPNO     =  C.SLIPNO(+)      ";
                SQL = SQL + ComNum.VBLF + " AND    O.ORDERCODE  =  C.ORDERCODE(+)   ";
                SQL = SQL + ComNum.VBLF + " AND    (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";
                SQL = SQL + ComNum.VBLF + " AND    O.DOSCODE    =  D.DOSCODE(+)     ";
                SQL = SQL + ComNum.VBLF + " AND    O.DRCODE      =  N.SABUN(+)      ";

                SQL = SQL + ComNum.VBLF + " AND    O.SUCODE = S.SUNEXT(+) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY O.BDATE,   O.GBGROUP, O.BUN, O.GBDIV,  O.SUCODE,  M.ROOMCODE, M.PANO,  O.DOSCODE,O.SLIPNO, O.ORDERCODE,  O.SEQNO     ";

                if (mstrWard == "ER")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(M.BDATE,'YYYY-MM-DD') INDATE, 'ER' WARDCODE,   '100' ROOMCODE,   M.PANO,       M.SNAME,      M.SEX,       ";
                    SQL = SQL + ComNum.VBLF + "        M.AGE,        M.DEPTCODE,   O.SLIPNO,     O.ORDERCODE, O.POWDER, O.GBER, O.GBPRN,  ";

                    if (optBun4.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        C.ORDERNAME || DECODE(O.GBINFO, NULL, '', '(' || O.GBINFO || ')') ORDERNAME,  C.ORDERNAMES, C.DISPHEADER, O.CONTENTS,  O.DCDIV, O.GBSELF, O.GBNGT, ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        C.ORDERNAME,  C.ORDERNAMES, C.DISPHEADER, O.CONTENTS,  O.DCDIV, O.GBSELF, O.GBNGT, ";
                    }

                    SQL = SQL + ComNum.VBLF + "        O.BCONTENTS,  O.REALQTY,    O.DOSCODE,    D.DOSNAME,   D.GBDIV, O.NURREMARK, ";
                    SQL = SQL + ComNum.VBLF + "        O.GBGROUP,    O.REMARK,     O.GBINFO,     O.GBTFLAG,   O.BUN,   O.ACTDIV, ";
                    SQL = SQL + ComNum.VBLF + "        O.GBPRN,      O.GBORDER,    O.ORDERSITE,  O.ORDERNO,   TO_CHAR(O.BDATE,'YYYY-MM-DD') BDATE , O.GBSTATUS,  ";
                    SQL = SQL + ComNum.VBLF + "        O.GBPORT,     N.DRNAME,     O.SUCODE,     O.QTY,  O.BUN ,O.NAL, O.ROWID, ";
                    SQL = SQL + ComNum.VBLF + "        S.UNITNEW1,   S.UNITNEW2,   S.UNITNEW3,   S.UNITNEW4, C.DISPRGB, C.ITEMCD, S.SUNAMEK ,O.PICKUPSABUN,";
                    SQL = SQL + ComNum.VBLF + "        O.GBIOE, O.VER ";
                    SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_IORDER O, ";
                    SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.OPD_MASTER  M,                           ";
                    SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P,                           ";
                    SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_ORDERCODE           C,                           ";
                    SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_ODOSAGE             D,                           ";
                    SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_DOCTOR              N,                            ";
                    SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_SUN     S                          ";
                    SQL = SQL + ComNum.VBLF + " WHERE  O.PTNO = '" + argPTNO + "' ";

                    if (argBun == "중증응급가산")
                    {
                        SQL = SQL + ComNum.VBLF + " AND  O.SUCODE IN (SELECT SUNEXT FROM KOSMOS_PMPA.BAS_SUN WHERE SUGBAA IN ('1','2','3') OR SUNEXT IN ('US119','US11ER','US22ER','US24','US24A') )";
                    }
                    else if (optBun2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND  O.BUN IN ('22','28','30','34','35','38','40','41','42','43','44','45','46','47','48','49','50','51') ";
                        SQL = SQL + ComNum.VBLF + " AND  O.SUCODE NOT IN (SELECT SUNEXT FROM KOSMOS_PMPA.BAS_SUN WHERE SUGBAA IN ('1','2','3'))";
                        SQL = SQL + ComNum.VBLF + " AND  NOT (S.BCODE IN ('999999') OR S.BCODE IS NULL)";
                        SQL = SQL + ComNum.VBLF + " AND  S.GBWON1 = '0'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND  O.BUN IN ( " + argBun + " ) ";
                    }

                    SQL = SQL + ComNum.VBLF + " AND  (O.GBPRN IN  NULL OR O.GBPRN <> 'P') ";
                    SQL = SQL + ComNum.VBLF + " AND  O.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') ";       //2019-02-25 ER 입원처방 부분 DC되도록 보완
                    SQL = SQL + ComNum.VBLF + " AND  (  O.ORDERSITE  IN ('ERO')   OR  O.GBIOE IN ('E','EI') )";
                    SQL = SQL + ComNum.VBLF + " AND   O.BDATE >= TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                    if (argPTNO == "08294668" && dtpDate1.Value.ToString("yyyy-MM-dd") == "2019-03-15")
                    {
                        SQL = SQL + ComNum.VBLF + " AND O.BDATE >= TO_DATE('2019-03-15','YYYY-MM-DD') ";
                    }

                    else
                    {
                        if (mstrDate == dtpDate1.Value.AddDays(2).ToString("yyyy-MM-dd"))
                        {
                            SQL = SQL + ComNum.VBLF + " AND   O.BDATE <= TO_DATE('" + dtpDate1.Value.AddDays(2).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " AND   O.BDATE <= TO_DATE('" + dtpDate1.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        }
                    }

                    SQL = SQL + ComNum.VBLF + " AND    O.GBPRN <>'S' ";  //'JJY 추가(2000/05/22 'S는 선수납(선불);
                    SQL = SQL + ComNum.VBLF + " AND   ( O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND   ACTDIV >'0' ) )  ";
                    SQL = SQL + ComNum.VBLF + " AND    O.PTNO       =  M.PANO           ";
                    SQL = SQL + ComNum.VBLF + "  AND   O.QTY  <>  '0'    ";
                    SQL = SQL + ComNum.VBLF + " AND  M.ACTDATE = TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND  M.DEPTCODE = 'ER'";
                    SQL = SQL + ComNum.VBLF + " AND   O.GBTFLAG <> 'T'";        //'2010-04-27     양수령수간호사 퇴원약 제외해달라고 함;
                    SQL = SQL + ComNum.VBLF + " AND    O.PTNO       =  P.PANO(+)        ";
                    SQL = SQL + ComNum.VBLF + " AND    O.SLIPNO     =  C.SLIPNO(+)      ";
                    SQL = SQL + ComNum.VBLF + " AND    O.ORDERCODE  =  C.ORDERCODE(+)   ";
                    SQL = SQL + ComNum.VBLF + " AND    (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";
                    SQL = SQL + ComNum.VBLF + " AND    O.DOSCODE    =  D.DOSCODE(+)     ";
                    SQL = SQL + ComNum.VBLF + " AND    O.DRCODE      =  N.SABUN(+)      ";
                    SQL = SQL + ComNum.VBLF + " AND    O.SUCODE = S.SUNEXT(+) ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY O.BDATE,   O.GBGROUP, O.BUN, O.GBDIV,  O.SUCODE,  '100', M.PANO,  O.DOSCODE,O.SLIPNO, O.ORDERCODE,  O.SEQNO, O.ENTDATE     ";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssM_Sheet1.Columns.Get(14, 17).Label = " ";

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        //    With ssM
                        ssM_Sheet1.RowCount = ssM_Sheet1.RowCount + 1;

                        nUnitNew1 = VB.Val(dt.Rows[i]["UNITNEW1"].ToString().Trim());
                        nBContents = VB.Val(dt.Rows[i]["BCONTENTS"].ToString().Trim());
                        strUnitNew2 = dt.Rows[i]["UNITNEW2"].ToString().Trim();
                        strUnitNew3 = dt.Rows[i]["UNITNEW3"].ToString().Trim();
                        nUnitNew4 = VB.Val(dt.Rows[i]["UNITNEW4"].ToString().Trim());

                        if (nUnitNew4 == 0)
                        {
                            nUnitNew4 = nUnitNew1;
                        }

                        nContents = VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim());

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 0].Text = (dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D" ? "DC" : "");

                        //if (ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 0].Text.Trim() == "DC")
                        //{
                        //    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 0].Font = new Font("MS Sans Serif", 10, FontStyle.Bold);
                        //    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 0].BackColor = Color.FromArgb(255, 255, 0);

                        //    if (optGB1.Checked == true)
                        //    {
                        //        ssM_Sheet1.Rows.Get(ssM_Sheet1.RowCount - 1).Locked = true;
                        //    }
                        //}

                        if (optBun2.Checked == true)
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 1].Text = READ_SuGaBunRu(dt.Rows[i]["BUN"].ToString().Trim());
                        }
                        //else if (optBun3.Checked == true)
                        //{
                        //    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 1].Text = "lab";
                        //}
                        else
                        {
                            switch (dt.Rows[i]["BUN"].ToString().Trim())
                            {
                                case "11":
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 1].Text = "내복";
                                    break;

                                case "12":
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 1].Text = "외용";
                                    break;

                                case "20":
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 1].Text = "주사";
                                    break;
                            }
                        }
                        //                        .ColWidth(.Col) = .MaxTextColWidth(.Col) + 2
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 53].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();


                        if (dt.Rows[i]["ORDERCODE"].ToString().Trim() == "W-MOX")
                        {
                            //string DD ="";
                            //DD = "W-MOX";
                        }

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Text = clsIpdNr.READ_AST_REACTION(clsDB.DbCon, dt.Rows[i]["ORDERCODE"].ToString().Trim(), argPTNO, dt.Rows[i]["INDATE"].ToString().Trim());

                        switch (dt.Rows[i]["BUN"].ToString().Trim())
                        {
                            case "11":
                            case "12":
                            case "20":
                                if (dt.Rows[i]["SUNAMEK"].ToString().Trim().IndexOf("자가") > -1)
                                {
                                    strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim() + strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                                }
                                else
                                {
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = clsIpdNr.READ_ORDERSITE(dt.Rows[i]["ORDERSITE"].ToString().Trim())
                                    + clsIpdNr.READ_ATTENTION(clsDB.DbCon, dt.Rows[i]["ORDERCODE"].ToString().Trim()) + dt.Rows[i]["SUNAMEK"].ToString().Trim()
                                    + clsIpdNr.GetDrugInfoSnaem(clsDB.DbCon, dt.Rows[i]["ORDERCODE"].ToString().Trim());
                                }
                                break;

                            default:
                                if (dt.Rows[i]["ORDERNAMES"].ToString().Trim() != "")
                                {
                                    strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim() + strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                                }
                                else if (dt.Rows[i]["DISPHEADER"].ToString().Trim() != "")
                                {
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim() + dt.Rows[i]["DISPHEADER"].ToString().Trim() + " " + dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                }
                                else
                                {
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim() + dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                }
                                break;
                        }

                        if(dt.Rows[i]["VER"].ToString().Trim() == "CPORDER" && ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.IndexOf("[CP]") == -1)
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "[CP]" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                        }

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();

                        SQL = "";
                        SQL = " SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE ";
                        SQL = SQL + ComNum.VBLF + " WHERE DOSFULLCODE LIKE '%HS%' ";
                        SQL = SQL + ComNum.VBLF + "    AND GBDIV ='1' ";
                        SQL = SQL + ComNum.VBLF + "    AND DOSCODE = '" + dt.Rows[i]["DOSCODE"].ToString().Trim() + "'";

                        SqlErr = clsDB.GetAdoRs(ref Rs1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (Rs1.HasRows == true)
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 8].Font = new Font("맑은 고딕", 15, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 8].BackColor = Color.FromArgb(128, 255, 255);
                        }

                        Rs1.Dispose();
                        Rs1 = null;

                        //GoSub Read_Specman

                        SQL = " SELECT SPECNAME FROM KOSMOS_OCS.OCS_OSPECIMAN ";
                        SQL = SQL + "WHERE SPECCODE = '" + dt.Rows[i]["DOSCODE"].ToString().Trim() + "' ";
                        SQL = SQL + "  AND SLIPNO   = '" + dt.Rows[i]["SLIPNO"].ToString().Trim() + "'   ";

                        SqlErr = clsDB.GetAdoRs(ref Rs1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (Rs1.HasRows == true)
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 8].Text = dt1.Rows[0]["SPECNAME"].ToString().Trim();
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 8].Text = Rs1.GetValue(0).ToString().Trim();
                        }
                        Rs1.Dispose();
                        Rs1 = null;

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                        nDiv = Convert.ToInt32(VB.Val(dt.Rows[i]["GBDIV"].ToString().Trim()));
                        nDiv = (nDiv == 0 ? 1 : nDiv);

                        if (dt.Rows[i]["ORDERCODE"].ToString().Trim() == "TR016")
                        {
                            nDiv = 1;
                        }
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 10].Text = Convert.ToString(nDiv);

                        if (nMaxDiv < nDiv)
                        {
                            nMaxDiv = nDiv;
                        }

                        //            .Col = 12:
                        if (dt.Rows[i]["BUN"].ToString().Trim() == "11"
                         || dt.Rows[i]["BUN"].ToString().Trim() == "12"
                         || dt.Rows[i]["BUN"].ToString().Trim() == "20"
                         || dt.Rows[i]["BUN"].ToString().Trim() == "23")
                        {
                            strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();

                            if (strUnit.IndexOf("/") > -1)
                            {
                                strUnit = VB.Split(strUnit, "/")[VB.Split(strUnit, "/").Length - 1];

                                switch (strUnit)
                                {
                                    case "A":
                                        strUnit = "ⓐ";
                                        break;

                                    case "T":
                                        strUnit = "ⓣ";
                                        break;

                                    case "V":
                                        strUnit = "ⓥ";
                                        break;

                                    case "BT":
                                        strUnit = "ⓑ";
                                        break;
                                }
                            }

                            if (nBContents == nContents)
                            {
                                nDivQty = VB.Val(dt.Rows[i]["REALQTY"].ToString().Trim()) / nDiv * nUnitNew1;
                            }
                            else
                            {
                                nDivQty = VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim()) / nDiv;
                            }

                            nDivQty = VB.FixDbl((nDivQty + 0.05) * 10) / 10;

                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 11].Text = " " + nDivQty + " " + dt.Rows[i]["UNITNEW2"].ToString().Trim() + " /" + VB.Left(strUnit.Trim() + VB.Space(3), 3);

                            if (dt.Rows[i]["REALQTY"].ToString().Trim() != "1" && dt.Rows[i]["BCONTENTS"].ToString().Trim() != dt.Rows[i]["CONTENTS"].ToString().Trim())
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 11].Text = "오류";
                            }
                        }

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["ACTDIV"].ToString().Trim();

                        SQL = "";
                        SQL = " SELECT COUNT(*) CNT FROM KOSMOS_OCS.OCS_IORDER";
                        SQL = SQL + ComNum.VBLF + " WHERE BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND PTNO = '" + argPTNO + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND ORDERSITE   IN ('DC0','DC1','DC2') ";
                        SQL = SQL + ComNum.VBLF + "   AND ORDERNO = '" + dt.Rows[i]["ORDERNO"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "   AND DIVQTY IS NULL ";

                        SqlErr = clsDB.GetAdoRs(ref Rs1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (Rs1.HasRows == true)
                        {
                            if (VB.Val(Rs1.GetValue(0).ToString().Trim()) > 0)
                            {
                                ssM_Sheet1.Rows.Get(ssM_Sheet1.RowCount - 1).BackColor = Color.FromArgb(200, 200, 200);
                            }
                        }

                        Rs1.Dispose();
                        Rs1 = null;

                        //'멀티 CHECKBOX

                        for (h = 1; h <= nDiv; h++)
                        {
                            //ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].Tag = dt.Rows[i]["ENTDATE"].ToString().Trim();

                            //TODO : 맞는지 확인 요망 - 영록
                            if (ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].CellType == null || ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].CellType.ToString() != "CheckBoxCellType")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].CellType = ctCheck;
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].HorizontalAlignment = CellHorizontalAlignment.Center;
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].VerticalAlignment = CellVerticalAlignment.Center;
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].BackColor = Color.FromArgb(255, 200, 200);
                            }

                            //'ACT완료된 내역에 표시 하기
                            SQL = "";
                            SQL = " SELECT EMRNO, ACTSABUN, TO_CHAR(ACTTIME, 'DD HH24:MI') ACTTIME ";
                            SQL = SQL + ComNum.VBLF + " FROM " + mstrACTTable;
                            SQL = SQL + ComNum.VBLF + " WHERE PTNO  = '" + argPTNO + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND ORDERNO = '" + dt.Rows[i]["ORDERNO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND GBSTATUS NOT IN ('D+') ";
                            SQL = SQL + ComNum.VBLF + "   AND ACTDIV = '" + Convert.ToString(h) + "' ";

                            SqlErr = clsDB.GetAdoRs(ref Rs1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (Rs1.HasRows == true)
                            {
                                //'TYPE 변경
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].CellType = ctText;
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].BackColor = Color.FromArgb(255, 200, 100);
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].Text = clsOpdNr.READ_INSA_NAME(clsDB.DbCon, Rs1.GetValue(1).ToString().Trim()) + ComNum.VBLF + Rs1.GetValue(2).ToString().Trim();

                                if (VERIFY_CHART(Rs1.GetValue(0).ToString().Trim()) == false)
                                {
                                    ssM_Sheet1.Rows.Get(ssM_Sheet1.RowCount - 1).BackColor = Color.FromArgb(255, 0, 0);
                                }
                            }

                            Rs1.Dispose();
                            Rs1 = null;

                            //'DC 자료 READ
                            SQL = "";
                            SQL = " SELECT NURSEID, TO_CHAR(ENTDATE, 'DD HH24:MI') ENTDATE, TO_CHAR(ENTDATE, 'YYYY-MM-DD') ENTDATE1 FROM KOSMOS_OCS.OCS_IORDER ";
                            SQL = SQL + ComNum.VBLF + " WHERE BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND PTNO  = '" + argPTNO + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND ORDERNO = '" + dt.Rows[i]["ORDERNO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND GBSTATUS  IN ('D-') ";
                            SQL = SQL + ComNum.VBLF + "   AND ORDERSITE IN ('DC1', 'NDC') ";
                            SQL = SQL + ComNum.VBLF + "   AND DCDIV = '" + Convert.ToString(h) + "' ";

                            SqlErr = clsDB.GetAdoRs(ref Rs1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (Rs1.HasRows == true)
                            {
                                //'TYPE 변경
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].CellType = ctText;
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].BackColor = Color.FromArgb(255, 50, 50);
                                if (cboWard.Text == "ER")
                                {
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].Text = "NDC" + ComNum.VBLF
                                        + clsOpdNr.READ_INSA_NAME(clsDB.DbCon, Rs1.GetValue(0).ToString().Trim())
                                        + ComNum.VBLF + Rs1.GetValue(1).ToString().Trim();
                                }
                                else
                                {
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].Text = "NDC" + ComNum.VBLF
                                        + clsOpdNr.READ_INSA_NAME(clsDB.DbCon, Rs1.GetValue(0).ToString().Trim());
                                }

                                //ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].Tag = dt1.Rows[0]["ENTDATE1"].ToString().Trim();
                            }

                            Rs1.Dispose();
                            Rs1 = null;
                        }

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 40].Text = (dt.Rows[i]["POWDER"].ToString().Trim() == "1" ? "◎" : "");

                        //            .Col = 42

                        if (VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 16 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 21)
                        {
                            if (dt.Rows[i]["GBNGT"].ToString().Trim() != "")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 41].Text = dt.Rows[i]["GBNGT"].ToString().Trim();
                            }

                            if (dt.Rows[i]["GBGROUP"].ToString().Trim() != "")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 41].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                            }
                        }
                        else if (VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 28 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 39)
                        {
                            //'손동현 위를 아래로 한다.
                            //'처치/재료는 GbNgt    나머지는 Group
                            if (VB.IsNumeric(dt.Rows[i]["GBGROUP"].ToString().Trim()) == true || dt.Rows[i]["GBGROUP"].ToString().Trim() == "")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 41].Text = dt.Rows[i]["GBNGT"].ToString().Trim();
                            }
                            else
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 41].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                            }
                        }
                        else
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 41].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                        }

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 42].Text = dt.Rows[i]["GBER"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 43].Text = dt.Rows[i]["GBSELF"].ToString().Trim();

                        //            .Col = 45
                        if (string.Compare(dt.Rows[i]["SLIPNO"].ToString().Trim(), "A1") >= 0 && string.Compare(dt.Rows[i]["SLIPNO"].ToString().Trim(), "A4") <= 0)
                        {
                        }
                        else
                        {
                            if (dt.Rows[i]["Remark"].ToString().Trim() != "")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 44].Text = "#";
                            }
                            if (dt.Rows[i]["GbPrn"].ToString().Trim() != "")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 44].Text = dt.Rows[i]["GbPrn"].ToString().Trim();
                            }
                            if (dt.Rows[i]["GbTFlag"].ToString().Trim() == "T")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 44].Text = "T";
                            }
                            if (dt.Rows[i]["GbTFlag"].ToString().Trim() == "O")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 44].Text = "O";
                            }
                            if (dt.Rows[i]["GbTFlag"].ToString().Trim() == "A")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 44].Text = "A";
                            }

                            if (dt.Rows[i]["GBPRN"].ToString().Trim() == "S" || ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 44].Text == "S")
                            {
                                //ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "(선)" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "(선수납)" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                            }
                            else if (dt.Rows[i]["GBPRN"].ToString().Trim() == "B")
                            {
                                //ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "(수)" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "(수납완료)" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                            }

                            if (dt.Rows[i]["GBPRN"].ToString().Trim() == "A")
                            {
                                //ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "(선)" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "(선수납)" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                            }
                        }

                        //            .Col = 46
                        if (dt.Rows[i]["GBPORT"].ToString().Trim() == "M")
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 45].Text = "M";
                        }

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 46].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 47].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 48].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 49].Text = dt.Rows[i]["NURREMARK"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 50].Text = dt.Rows[i]["ITEMCD"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 51].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 52].Text = dt.Rows[i]["NURREMARK"].ToString().Trim();

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 54].Text = clsOpdNr.READ_INSA_NAME(clsDB.DbCon, dt.Rows[i]["PICKUPSABUN"].ToString().Trim()) + clsVbfunc.READ_INSA_BUSE(clsDB.DbCon, dt.Rows[i]["PICKUPSABUN"].ToString().Trim());

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 55].Text = dt.Rows[i]["GBIOE"].ToString().Trim();     //2019-02-14

                        if (dt.Rows[i]["DISPRGB"].ToString().Trim() != "")
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 0, ssM_Sheet1.RowCount - 1, 13].ForeColor =
                            ColorTranslator.FromWin32(int.Parse(dt.Rows[i]["DISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                        }

                        if (dt.Rows[i]["BDATE"].ToString().Trim() != strBDATE)
                        {
                            ssM_Sheet1.Rows.Get(ssM_Sheet1.RowCount - 1).Border = new FarPoint.Win.LineBorder(Color.Red, 1, false, true, false, false);

                            strBDATE = dt.Rows[i]["BDATE"].ToString().Trim();

                            FnBRow = ssM_Sheet1.RowCount - 1;
                        }

                        SQL = "";
                        SQL = " SELECT  ROWID FROM KOSMOS_OCS.OCS_IORDER ";
                        SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GBSTATUS  IN  (' ','D+','D','D-') ";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE      = TO_DATE('" + dtpDate1.Value.AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "    AND ORDERCODE = '" + dt.Rows[i]["ORDERCODE"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND QTY = '" + dt.Rows[i]["QTY"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND REALQTY = '" + dt.Rows[i]["REALQTY"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND CONTENTS = '" + dt.Rows[i]["CONTENTS"].ToString().Trim() + "'  ";

                        if (dt.Rows[i]["REMARK"].ToString().Trim() == "")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND ( REMARK = '" + dt.Rows[i]["REMARK"].ToString().Trim() + "'  OR REMARK IS NULL ) ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "    AND REMARK = '" + dt.Rows[i]["REMARK"].ToString().Trim() + "' ";
                        }

                        if (mstrWard == "ER")
                        {
                            SQL = SQL + ComNum.VBLF + "  AND GBIOE IN ('E','EI') ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  AND ( GBIOE NOT IN ('E','EI') OR GBIOE IS NULL) ";
                        }

                        SqlErr = clsDB.GetAdoRs(ref Rs1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (Rs1.HasRows == false)
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 5].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 5].ForeColor = Color.FromArgb(0, 0, 0);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 6].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 6].ForeColor = Color.FromArgb(0, 0, 0);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].ForeColor = Color.FromArgb(0, 0, 0);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 9].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 9].ForeColor = Color.FromArgb(0, 0, 0);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 10].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 10].ForeColor = Color.FromArgb(0, 0, 0);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 11].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 11].ForeColor = Color.FromArgb(0, 0, 0);
                        }

                        Rs1.Dispose();
                        Rs1 = null;

                        if (ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Text == "Negative")
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Text = "-";
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Font = new Font("맑은 고딕", 20, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].ForeColor = Color.FromArgb(0, 0, 255);
                        }
                        else if (ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Text == "Positive")
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Text = "+";
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Font = new Font("맑은 고딕", 20, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].ForeColor = Color.FromArgb(255, 0, 0);
                        }

                        ssM_Sheet1.SetRowHeight(ssM_Sheet1.RowCount - 1, Convert.ToInt32(ssM_Sheet1.GetPreferredRowHeight(ssM_Sheet1.RowCount - 1)) + 10);
                    }

                    for (i = 14; i < 38; i++)
                    {
                        ssM_Sheet1.Columns.Get(i).Visible = true;

                        if (i > nMaxDiv + 14)
                        {
                            ssM_Sheet1.Columns.Get(i).Visible = false;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }
        
        private void READ_OCS_IORDER_MUTI(string argPTNO, string argBun)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Log.Debug("===================== READ_OCS_IORDER_MUTI Start =====================argPTNO: {}, argBun: {}", argPTNO, argBun);

            int nMaxDiv = 0;

            string strUnit = string.Empty;
            double nBContents = 0;
            double nContents = 0;
            int nDiv = 0;
            double nDivQty = 0;

            double nUnitNew1 = 0;
            string strUnitNew2 = string.Empty;
            string strUnitNew3 = string.Empty;
            double nUnitNew4 = 0;

            string strBDATE = string.Empty;

            int i = 0;
            int h = 0;
            DataTable dt = null;
            //DataTable dt1 = null;
            OracleDataReader Rs1 = null;
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수

            ssM_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL.AppendLine(" SELECT TO_CHAR(M.INDATE,'YYYY-MM-DD') INDATE, M.WARDCODE,   M.ROOMCODE,   M.PANO,       M.SNAME,      M.SEX,      O.VER, ");
                SQL.AppendLine("        M.AGE,        M.DEPTCODE,   O.SLIPNO,     O.ORDERCODE, O.POWDER, O.GBER, O.GBPRN,  ");
                SQL.AppendLine("        C.ORDERNAME,  C.ORDERNAMES, C.DISPHEADER, O.CONTENTS,  O.DCDIV, O.GBSELF, O.GBNGT, ");
                SQL.AppendLine("        O.BCONTENTS,  O.REALQTY,    O.DOSCODE,    D.DOSNAME,   D.GBDIV, O.NURREMARK, ");
                SQL.AppendLine("        O.GBGROUP,    O.REMARK,     O.GBINFO,     O.GBTFLAG,   O.BUN,   O.ACTDIV, ");
                SQL.AppendLine("        O.GBPRN,      O.GBORDER,    O.ORDERSITE,  O.ORDERNO,   TO_CHAR(O.BDATE,'YYYY-MM-DD') BDATE , O.GBSTATUS,  ");
                SQL.AppendLine("        O.GBPORT,     N.DRNAME,     O.SUCODE,     O.QTY,  O.BUN ,O.NAL, O.ROWID, ");
                SQL.AppendLine("        S.UNITNEW1,   S.UNITNEW2,   S.UNITNEW3,   S.UNITNEW4, C.DISPRGB, C.ITEMCD, ");
                SQL.AppendLine("        TO_CHAR(O.PICKUPDATE,'YYYY-MM-DD HH24:MI') PICKUPDATE, KOSMOS_PMPA.FC_NUR_READ_MAYAK_GUBUN(O.SUCODE) || S.SUNAMEK SUNAMEK, ");
                SQL.AppendLine("        O.PICKUPSABUN, TO_CHAR(O.ENTDATE, 'YYYY-MM-DD') AS ENTDATE, ");
                SQL.AppendLine("        O.GBIOE ");      //2019-02-14 응급실 NDC 용
                SQL.AppendLine("		, KOSMOS_OCS.FC_INSA_MST_KORNAME(LPAD(TRIM(O.PICKUPSABUN), 5, '0')) AS PICKUPNAME ");
                SQL.AppendLine("        , KOSMOS_OCS.FC_INSA_BUSE(LPAD(TRIM(O.PICKUPSABUN), 5, '0')) AS PICKUPBUSE ");
                SQL.AppendLine("        , KOSMOS_OCS.FC_READ_ATTENTION(O.SUCODE) AS ATTENTION ");
                SQL.AppendLine("        , KOSMOS_OCS.FC_READ_AST_REACTION(O.SUCODE, O.PTNO, TO_CHAR(M.INDATE,'YYYY-MM-DD'), KOSMOS_OCS.FC_GET_AGE2(O.PTNO, SYSDATE)) AS AST_ATTENTION ");
                SQL.AppendLine("        , KOSMOS_OCS.FC_OCS_DRUGINFO_SNAME(O.ORDERCODE) AS DRUGNAME ");
                SQL.AppendLine("        , KOSMOS_OCS.FC_OCS_OSPECIMAN_SPECNAME(O.SLIPNO, O.DOSCODE) AS SPECNAME ");
                SQL.AppendLine("        , KOSMOS_OCS.FC_OCS_ODOSAGE_NAME1(O.DOSCODE) AS DOSCODEYN ");
                //SQL.AppendLine("        , KOSMOS_OCS.FC_OCS_IORDER_CNT1(O.PTNO, O.BDATE, O.ORDERNO) AS IORDER_CNT1 ");
                SQL.AppendLine("		, (SELECT COUNT(*) CNT ");
                SQL.AppendLine("		             FROM KOSMOS_OCS.OCS_IORDER ");
                SQL.AppendLine("		             WHERE ORDERNO = O.ORDERNO ");
                SQL.AppendLine("		                  AND PTNO = O.PTNO ");
                SQL.AppendLine("		                  AND BDATE = O.BDATE ");
                SQL.AppendLine("		                  AND ORDERSITE   IN ('DC0','DC1','DC2') ");
                SQL.AppendLine("		                  AND DIVQTY IS NULL) AS IORDER_CNT1 ");
                SQL.AppendLine("        , ( SELECT  ");
                SQL.AppendLine("                MAX(AA1.ORDERNO) ");
                SQL.AppendLine("            FROM KOSMOS_OCS.OCS_IORDER AA1  ");
                SQL.AppendLine("            WHERE AA1.PTNO  = O.PTNO ");
                SQL.AppendLine("               AND AA1.BDATE  = (O.BDATE - 1) ");
                SQL.AppendLine("               AND AA1.GBSTATUS  IN  (' ','D+','D','D-')  ");
                SQL.AppendLine("               AND AA1.ORDERCODE = O.ORDERCODE ");
                SQL.AppendLine("               AND AA1.QTY = O.QTY ");
                SQL.AppendLine("               AND AA1.REALQTY = O.REALQTY ");
                SQL.AppendLine("               AND AA1.CONTENTS = O.CONTENTS ");
                SQL.AppendLine("               AND ( AA1.GBIOE NOT IN ('E','EI') OR AA1.GBIOE IS NULL) ) AS BEFORDAY ");
                SQL.AppendLine(" FROM   KOSMOS_OCS.OCS_IORDER O, ");

                if (VB.Left(cboJob.Text, 1) == "D")
                {
                    SQL.AppendLine("        KOSMOS_PMPA.IPD_NEW_MASTER  M,                           ");
                }
                else
                {
                    SQL.AppendLine(" (SELECT ACTDATE, INDATE, OUTDATE, GBSTS, WARDCODE, ROOMCODE, PANO, SNAME, SEX, AGE, DEPTCODE, IPDNO ");
                    SQL.AppendLine("  FROM KOSMOS_PMPA.IPD_NEW_MASTER ");
                    SQL.AppendLine("  WHERE IPDNO = (SELECT ");
                    SQL.AppendLine("                     MAX(IPDNO) ");
                    SQL.AppendLine("                  FROM KOSMOS_PMPA.IPD_NEW_MASTER ");
                    SQL.AppendLine("                  WHERE(ACTDATE IS NULL OR OUTDATE = TRUNC(SYSDATE))   ");
                    SQL.AppendLine(" 		         AND PANO = '" + argPTNO + "' AND GBSTS NOT IN ('9')) ) M,                 ");
                }

                SQL.AppendLine("        KOSMOS_PMPA.BAS_PATIENT P,                           ");
                SQL.AppendLine("        KOSMOS_OCS.OCS_ORDERCODE           C,                           ");
                SQL.AppendLine("        KOSMOS_OCS.OCS_ODOSAGE             D,                           ");
                SQL.AppendLine("        KOSMOS_OCS.OCS_DOCTOR              N,                            ");
                SQL.AppendLine("        KOSMOS_PMPA.BAS_SUN     S                          ");
                SQL.AppendLine("  WHERE PTNO = '" + argPTNO + "'");

                if (argBun == "중증응급가산")
                {
                    SQL.AppendLine("   AND O.SUCODE IN (SELECT SUNEXT FROM KOSMOS_PMPA.BAS_SUN WHERE SUGBAA IN ('1','2','3'))");
                    SQL.AppendLine("   AND O.BUN IN ( '28','30','34','35','38','40')");
                    SQL.AppendLine("   AND O.BDATE >= TRUNC(M.INDATE) AND O.BDATE <= TRUNC(M.INDATE + 1)");
                    SQL.AppendLine("   AND EXISTS ( SELECT SUB1.PANO");     //'NUR_MASTER  ACT_OK ='1' 이면 중증응급가상 미비된 엑팅 오더 보이지 않음;
                    SQL.AppendLine("   FROM KOSMOS_PMPA.NUR_MASTER SUB1");
                    SQL.AppendLine("   WHERE SUB1.PANO = M.PANO");
                    SQL.AppendLine("   AND SUB1.IPDNO = M.IPDNO");
                    SQL.AppendLine("   AND SUB1.ACT_OK IS NULL)");
                }
                else
                {
                    SQL.AppendLine("  AND O.BUN IN ( " + argBun + " ) ");
                    SQL.AppendLine("  AND BDATE >= TO_DATE('" + dtpDate1.Value.AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')");
                    if (mstrWard == "ER" && chkERActingList.Checked == true)
                    {
                        SQL.AppendLine("  AND BDATE <= TO_DATE('" + dtpDate1.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')");
                    }
                    else
                    {
                        SQL.AppendLine("  AND BDATE <= TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')");
                    }
                }

                SQL.AppendLine("   AND  (O.GBPRN IN  NULL OR O.GBPRN <> 'P') ");

                if (mstrWard == "ER")
                {
                    SQL.AppendLine(" AND  (  O.ORDERSITE  IN ('ERO')   OR  O.GBIOE IN ('E','EI') )");
                }
                else
                {
                    SQL.AppendLine(" AND  (O.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') OR O.ORDERSITE IS NULL )");
                }

                SQL.AppendLine(" AND    O.GBPRN <>'S' "); //'JJY 추가(2000/05/22 'S는 선수납(선불);
                SQL.AppendLine(" AND   ( O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND   ACTDIV >'0' ) )  ");
                SQL.AppendLine(" AND    O.PTNO       =  M.PANO           ");
                SQL.AppendLine("  AND  O.GBPICKUP = '*' ");
                SQL.AppendLine("  AND  ( O.VERBC IS NULL OR O.VERBC <>'Y' )");

                if (mstrWard == "HD")
                {
                    SQL.AppendLine(" AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)");
                }
                else if (mstrWard == "ENDO")
                {
                    SQL.AppendLine("AND O.DOSCODE IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE WARDCODE IN ( 'EN') ) ");
                    SQL.AppendLine(" AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)");
                }
                else if (mstrWard == "CT/MRI")
                {
                    SQL.AppendLine("AND O.DOSCODE IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE WARDCODE IN ( 'RD') ) ");
                    SQL.AppendLine(" AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)");
                }
                else if (mstrWard == "OP" || mstrWard == "AG")
                {
                    //    '수술방은 모든 오더 보이도록 처리 추후 보완 예정;
                }
                else if (mstrWard == "RA")
                {
                    SQL.AppendLine(" AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)");
                }
                else if (mstrWard == "MICU")
                {
                    SQL.AppendLine(" AND M.WARDCODE ='IU'");
                    SQL.AppendLine(" AND M.ROOMCODE ='234'");
                    SQL.AppendLine(" AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)");
                }
                else
                {
                    SQL.AppendLine(" AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)");
                    if (mstrWard == "SICU")
                    {
                        SQL.AppendLine(" AND M.WARDCODE ='IU'   ");
                        SQL.AppendLine(" AND M.ROOMCODE ='233'");
                    }
                    else if (mstrWard != "ER")
                    {
                        if (mstrWard == "IQ" || mstrWard == "ND" || mstrWard == "NR")
                        {
                            SQL.AppendLine(" AND  M.WARDCODE IN ('IQ','ND','NR')");
                        }
                        else
                        {
                            SQL.AppendLine(" AND  M.WARDCODE = '" + mstrWard.Trim() + "' ");
                        }
                    }
                }

                SQL.AppendLine("  AND   O.QTY  <>  '0'    ");
                SQL.AppendLine("  AND    M.GBSTS NOT IN  ('9') "); //" '입원취소 제외;

                if (VB.Left(cboJob.Text.Trim(), 1) == "D") //'어제퇴원자;
                {
                    SQL.AppendLine(" AND  M.OUTDATE = TRUNC(SYSDATE -1) ");  //'계산 완료 환자도 ACTING 은 가능;
                }
                else
                {
                    SQL.AppendLine(" AND  (M.ACTDATE IS NULL OR M.OUTDATE =TRUNC(SYSDATE))  ");   //'계산 완료 환자도 ACTING 은 가능;
                }

                SQL.AppendLine(" AND    O.PTNO       =  P.PANO(+)        ");
                SQL.AppendLine(" AND    O.SLIPNO     =  C.SLIPNO(+)      ");
                SQL.AppendLine(" AND    O.ORDERCODE  =  C.ORDERCODE(+)   ");
                SQL.AppendLine(" AND    (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ");
                SQL.AppendLine(" AND    O.DOSCODE    =  D.DOSCODE(+)     ");
                SQL.AppendLine(" AND    O.DRCODE      =  N.SABUN(+)      ");

                SQL.AppendLine(" AND    O.SUCODE = S.SUNEXT(+) ");
                SQL.AppendLine(" ORDER BY O.BDATE,   O.GBGROUP, O.BUN, O.GBDIV,  O.SUCODE,  M.ROOMCODE, M.PANO,  O.DOSCODE,O.SLIPNO, O.ORDERCODE,  O.SEQNO     ");

                if (mstrWard == "ER")
                {
                    SQL.Clear();
                    SQL.AppendLine(" SELECT TO_CHAR(M.BDATE,'YYYY-MM-DD') INDATE, 'ER' WARDCODE,   '100' ROOMCODE,   M.PANO,       M.SNAME,      M.SEX,       ");
                    SQL.AppendLine("        M.AGE,        M.DEPTCODE,   O.SLIPNO,     O.ORDERCODE, O.POWDER, O.GBER, O.GBPRN, '' VER,  ");

                    if (optBun4.Checked == true)
                    {
                        SQL.AppendLine("        C.ORDERNAME || DECODE(O.GBINFO, NULL, '', '(' || O.GBINFO || ')') ORDERNAME,  C.ORDERNAMES, C.DISPHEADER, O.CONTENTS,  O.DCDIV, O.GBSELF, O.GBNGT, ");
                    }
                    else
                    {
                        SQL.AppendLine("        C.ORDERNAME,  C.ORDERNAMES, C.DISPHEADER, O.CONTENTS,  O.DCDIV, O.GBSELF, O.GBNGT, ");
                    }

                    SQL.AppendLine("        O.BCONTENTS,  O.REALQTY,    O.DOSCODE,    D.DOSNAME,   D.GBDIV, O.NURREMARK, ");
                    SQL.AppendLine("        O.GBGROUP,    O.REMARK,     O.GBINFO,     O.GBTFLAG,   O.BUN,   O.ACTDIV, ");
                    SQL.AppendLine("        O.GBPRN,      O.GBORDER,    O.ORDERSITE,  O.ORDERNO,   TO_CHAR(O.BDATE,'YYYY-MM-DD') BDATE , O.GBSTATUS,  ");
                    SQL.AppendLine("        O.GBPORT,     N.DRNAME,     O.SUCODE,     O.QTY,  O.BUN ,O.NAL, O.ROWID, ");
                    SQL.AppendLine("        S.UNITNEW1,   S.UNITNEW2,   S.UNITNEW3,   S.UNITNEW4, C.DISPRGB, C.ITEMCD, KOSMOS_PMPA.FC_NUR_READ_MAYAK_GUBUN(O.SUCODE) || S.SUNAMEK SUNAMEK, O.PICKUPSABUN,");
                    SQL.AppendLine("        O.GBIOE ");
                    SQL.AppendLine("		, KOSMOS_OCS.FC_INSA_MST_KORNAME(LPAD(TRIM(O.PICKUPSABUN), 5, '0')) AS PICKUPNAME ");
                    SQL.AppendLine("        , KOSMOS_OCS.FC_INSA_BUSE(LPAD(TRIM(O.PICKUPSABUN), 5, '0')) AS PICKUPBUSE ");
                    SQL.AppendLine("        , KOSMOS_OCS.FC_READ_ATTENTION(O.SUCODE) AS ATTENTION ");
                    SQL.AppendLine("        , KOSMOS_OCS.FC_READ_AST_REACTION(O.SUCODE, O.PTNO, TO_CHAR(M.BDATE,'YYYY-MM-DD'), KOSMOS_OCS.FC_GET_AGE2(O.PTNO, SYSDATE)) AS AST_ATTENTION ");
                    SQL.AppendLine("        , KOSMOS_OCS.FC_OCS_DRUGINFO_SNAME(O.ORDERCODE) AS DRUGNAME ");
                    SQL.AppendLine("        , KOSMOS_OCS.FC_OCS_OSPECIMAN_SPECNAME(O.SLIPNO, O.DOSCODE) AS SPECNAME ");
                    SQL.AppendLine("        , KOSMOS_OCS.FC_OCS_ODOSAGE_NAME1(O.DOSCODE) AS DOSCODEYN ");
                    //SQL.AppendLine("        , KOSMOS_OCS.FC_OCS_IORDER_CNT1(O.PTNO, O.BDATE, O.ORDERNO) AS IORDER_CNT1 ");
                    SQL.AppendLine("		, (SELECT COUNT(*) CNT ");
                    SQL.AppendLine("		             FROM KOSMOS_OCS.OCS_IORDER ");
                    SQL.AppendLine("		             WHERE ORDERNO = O.ORDERNO ");
                    SQL.AppendLine("		                  AND PTNO = O.PTNO ");
                    SQL.AppendLine("		                  AND BDATE = O.BDATE ");
                    SQL.AppendLine("		                  AND ORDERSITE   IN ('DC0','DC1','DC2') ");
                    SQL.AppendLine("		                  AND DIVQTY IS NULL) AS IORDER_CNT1 ");
                    SQL.AppendLine("        , ( SELECT  ");
                    SQL.AppendLine("                MAX(AA1.ORDERNO) ");
                    SQL.AppendLine("            FROM KOSMOS_OCS.OCS_IORDER AA1  ");
                    SQL.AppendLine("            WHERE AA1.PTNO  = O.PTNO ");
                    SQL.AppendLine("               AND AA1.BDATE  = (O.BDATE - 1) ");
                    SQL.AppendLine("               AND AA1.GBSTATUS  IN  (' ','D+','D','D-')  ");
                    SQL.AppendLine("               AND AA1.ORDERCODE = O.ORDERCODE ");
                    SQL.AppendLine("               AND AA1.QTY = O.QTY ");
                    SQL.AppendLine("               AND AA1.REALQTY = O.REALQTY ");
                    SQL.AppendLine("               AND AA1.CONTENTS = O.CONTENTS ");
                    SQL.AppendLine("               AND AA1.GBIOE IN ('E','EI') ) AS BEFORDAY ");
                    SQL.AppendLine(" FROM   KOSMOS_OCS.OCS_IORDER O, ");
                    SQL.AppendLine("        KOSMOS_PMPA.OPD_MASTER  M,                           ");
                    SQL.AppendLine("        KOSMOS_PMPA.BAS_PATIENT P,                           ");
                    SQL.AppendLine("        KOSMOS_OCS.OCS_ORDERCODE           C,                           ");
                    SQL.AppendLine("        KOSMOS_OCS.OCS_ODOSAGE             D,                           ");
                    SQL.AppendLine("        KOSMOS_OCS.OCS_DOCTOR              N,                            ");
                    SQL.AppendLine("        KOSMOS_PMPA.BAS_SUN     S                          ");
                    SQL.AppendLine(" WHERE  O.PTNO = '" + argPTNO + "' ");

                    if (argBun == "중증응급가산")
                    {
                        SQL.AppendLine(" AND  O.SUCODE IN (SELECT SUNEXT FROM KOSMOS_PMPA.BAS_SUN WHERE SUGBAA IN ('1','2','3') OR SUNEXT IN ('US119','US11ER','US22ER','US24','US24A') )");
                    }
                    else if (optBun2.Checked == true)
                    {
                        SQL.AppendLine(" AND  O.BUN IN ('22','28','30','34','35','38','40','41','42','43','44','45','46','47','48','49','50','51') ");
                        SQL.AppendLine(" AND  O.SUCODE NOT IN (SELECT SUNEXT FROM KOSMOS_PMPA.BAS_SUN WHERE SUGBAA IN ('1','2','3'))");
                        SQL.AppendLine(" AND  NOT (S.BCODE IN ('999999') OR S.BCODE IS NULL)");
                        SQL.AppendLine(" AND  S.GBWON1 = '0'");
                    }
                    else
                    {
                        SQL.AppendLine(" AND  O.BUN IN ( " + argBun + " ) ");
                    }

                    SQL.AppendLine(" AND  (O.GBPRN IN  NULL OR O.GBPRN <> 'P') ");
                    SQL.AppendLine(" AND  O.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') ");       //2019-02-25 ER 입원처방 부분 DC되도록 보완
                    SQL.AppendLine(" AND  (  O.ORDERSITE  IN ('ERO')   OR  O.GBIOE IN ('E','EI') )");
                    SQL.AppendLine(" AND   O.BDATE >= TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");

                    if (chkERgbn.Checked == true)       //장기 재실환자를 위한 옵션
                    { }
                    else
                    {
                        if (argPTNO == "08294668" && dtpDate1.Value.ToString("yyyy-MM-dd") == "2019-03-15")
                        {
                            SQL.AppendLine(" AND O.BDATE >= TO_DATE('2019-03-15','YYYY-MM-DD') ");
                        }

                        else
                        {
                            if (mstrDate == dtpDate1.Value.AddDays(2).ToString("yyyy-MM-dd"))
                            {
                                SQL.AppendLine(" AND   O.BDATE <= TO_DATE('" + dtpDate1.Value.AddDays(2).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");
                            }
                            else
                            {
                                SQL.AppendLine(" AND   O.BDATE <= TO_DATE('" + dtpDate1.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");
                            }
                        }
                    }
                    SQL.AppendLine(" AND    O.GBPRN <>'S' ");  //'JJY 추가(2000/05/22 'S는 선수납(선불);
                    SQL.AppendLine(" AND   ( O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND   ACTDIV >'0' ) )  ");
                    SQL.AppendLine(" AND    O.PTNO       =  M.PANO           ");
                    SQL.AppendLine("  AND   O.QTY  <>  '0'    ");
                    if (chkERgbn.Checked == true)
                    { }
                    else
                    {
                        SQL.AppendLine(" AND  M.ACTDATE = TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");
                    }
                    SQL.AppendLine(" AND  M.DEPTCODE = 'ER'");
                    SQL.AppendLine(" AND   O.GBTFLAG <> 'T'");        //'2010-04-27     양수령수간호사 퇴원약 제외해달라고 함;
                    SQL.AppendLine(" AND    O.PTNO       =  P.PANO(+)        ");
                    SQL.AppendLine(" AND    O.SLIPNO     =  C.SLIPNO(+)      ");
                    SQL.AppendLine(" AND    O.ORDERCODE  =  C.ORDERCODE(+)   ");
                    SQL.AppendLine(" AND    (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ");
                    SQL.AppendLine(" AND    O.DOSCODE    =  D.DOSCODE(+)     ");
                    SQL.AppendLine(" AND    O.DRCODE      =  N.SABUN(+)      ");
                    SQL.AppendLine(" AND    O.SUCODE = S.SUNEXT(+) ");
                    SQL.AppendLine(" ORDER BY O.BDATE,   O.GBGROUP, O.BUN, O.GBDIV,  O.SUCODE,  '100', M.PANO,  O.DOSCODE,O.SLIPNO, O.ORDERCODE,  O.SEQNO, O.ENTDATE     ");
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                ssM_Sheet1.Columns.Get(14, 17).Label = " ";

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        //    With ssM
                        ssM_Sheet1.RowCount = ssM_Sheet1.RowCount + 1;

                        nUnitNew1 = VB.Val(dt.Rows[i]["UNITNEW1"].ToString().Trim());
                        nBContents = VB.Val(dt.Rows[i]["BCONTENTS"].ToString().Trim());
                        strUnitNew2 = dt.Rows[i]["UNITNEW2"].ToString().Trim();
                        strUnitNew3 = dt.Rows[i]["UNITNEW3"].ToString().Trim();
                        nUnitNew4 = VB.Val(dt.Rows[i]["UNITNEW4"].ToString().Trim());

                        if (nUnitNew4 == 0)
                        {
                            nUnitNew4 = nUnitNew1;
                        }

                        nContents = VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim());

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 0].Text = (dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D" ? "DC" : "");

                        if (optBun2.Checked == true)
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 1].Text = READ_SuGaBunRu(dt.Rows[i]["BUN"].ToString().Trim());
                        }
                        else
                        {
                            switch (dt.Rows[i]["BUN"].ToString().Trim())
                            {
                                case "11":
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 1].Text = "내복";
                                    break;

                                case "12":
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 1].Text = "외용";
                                    break;

                                case "20":
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 1].Text = "주사";
                                    break;
                            }
                        }
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 53].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["AST_ATTENTION"].ToString().Trim();
                        //ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Text = clsIpdNr.READ_AST_REACTION(clsDB.DbCon, dt.Rows[i]["ORDERCODE"].ToString().Trim(), argPTNO, dt.Rows[i]["INDATE"].ToString().Trim());

                        switch (dt.Rows[i]["BUN"].ToString().Trim())
                        {
                            case "11":
                            case "12":
                            case "20":
                                if (dt.Rows[i]["SUNAMEK"].ToString().Trim().IndexOf("자가") > -1)
                                {
                                    strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim() + strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                                }
                                else
                                {
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = clsIpdNr.READ_ORDERSITE(dt.Rows[i]["ORDERSITE"].ToString().Trim())
                                    + dt.Rows[i]["ATTENTION"].ToString().Trim() + dt.Rows[i]["SUNAMEK"].ToString().Trim()
                                    + dt.Rows[i]["DRUGNAME"].ToString().Trim();
                                }
                                break;

                            default:
                                if (dt.Rows[i]["ORDERNAMES"].ToString().Trim() != "")
                                {
                                    strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim() + strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                                }
                                else if (dt.Rows[i]["DISPHEADER"].ToString().Trim() != "")
                                {
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim() + dt.Rows[i]["DISPHEADER"].ToString().Trim() + " " + dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                }
                                else
                                {
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim() + dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                }
                                break;
                        }

                        if (dt.Rows[i]["VER"].ToString().Trim() == "CPORDER" && ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.IndexOf("[CP]") == -1)
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "[CP]" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                        }

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();
                        
                        if (dt.Rows[i]["DOSCODEYN"].ToString().Trim() != "")
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 8].Font = new Font("맑은 고딕", 15, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 8].BackColor = Color.FromArgb(128, 255, 255);
                        }
                        
                        if (dt.Rows[i]["SPECNAME"].ToString().Trim() != "")
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 8].Text = dt.Rows[0]["SPECNAME"].ToString().Trim();
                        }

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                        nDiv = Convert.ToInt32(VB.Val(dt.Rows[i]["GBDIV"].ToString().Trim()));
                        nDiv = (nDiv == 0 ? 1 : nDiv);

                        if (dt.Rows[i]["ORDERCODE"].ToString().Trim() == "TR016")
                        {
                            nDiv = 1;
                        }
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 10].Text = Convert.ToString(nDiv);

                        if (nMaxDiv < nDiv)
                        {
                            nMaxDiv = nDiv;
                        }

                        //            .Col = 12:
                        if (dt.Rows[i]["BUN"].ToString().Trim() == "11"
                         || dt.Rows[i]["BUN"].ToString().Trim() == "12"
                         || dt.Rows[i]["BUN"].ToString().Trim() == "20"
                         || dt.Rows[i]["BUN"].ToString().Trim() == "23")
                        {
                            strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();

                            if (strUnit.IndexOf("/") > -1)
                            {
                                strUnit = VB.Split(strUnit, "/")[VB.Split(strUnit, "/").Length - 1];

                                switch (strUnit)
                                {
                                    case "A":
                                        strUnit = "ⓐ";
                                        break;

                                    case "T":
                                        strUnit = "ⓣ";
                                        break;

                                    case "V":
                                        strUnit = "ⓥ";
                                        break;

                                    case "BT":
                                        strUnit = "ⓑ";
                                        break;
                                }
                            }

                            if (nBContents == nContents)
                            {
                                nDivQty = VB.Val(dt.Rows[i]["REALQTY"].ToString().Trim()) / nDiv * nUnitNew1;
                            }
                            else
                            {
                                nDivQty = VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim()) / nDiv;
                            }

                            nDivQty = VB.FixDbl((nDivQty + 0.05) * 10) / 10;

                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 11].Text = " " + nDivQty + " " + dt.Rows[i]["UNITNEW2"].ToString().Trim() + " /" + VB.Left(strUnit.Trim() + VB.Space(3), 3);

                            if (dt.Rows[i]["REALQTY"].ToString().Trim() != "1" && dt.Rows[i]["BCONTENTS"].ToString().Trim() != dt.Rows[i]["CONTENTS"].ToString().Trim())
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 11].Text = "오류";
                            }
                        }

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["ACTDIV"].ToString().Trim();
                        
                        if (VB.Val(dt.Rows[i]["IORDER_CNT1"].ToString().Trim()) > 0)
                        {
                            ssM_Sheet1.Rows.Get(ssM_Sheet1.RowCount - 1).BackColor = Color.FromArgb(200, 200, 200);
                        }

                        //'멀티 CHECKBOX
                        for (h = 1; h <= nDiv; h++)
                        {
                            if (ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].CellType == null || ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].CellType.ToString() != "CheckBoxCellType")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].CellType = ctCheck;
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].HorizontalAlignment = CellHorizontalAlignment.Center;
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].VerticalAlignment = CellVerticalAlignment.Center;
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].BackColor = Color.FromArgb(255, 200, 200);
                            }

                            //'ACT완료된 내역에 표시 하기
                            SQL.Clear();
                            SQL.AppendLine(" SELECT ");
                            SQL.AppendLine("    A.EMRNO, A.ACTSABUN, TO_CHAR(A.ACTTIME, 'DD HH24:MI') ACTTIME, B.KORNAME AS ACTUSER ");
                            SQL.AppendLine("    , ( SELECT ");
                            SQL.AppendLine("            MAX(BB1.EMRNO)");
                            SQL.AppendLine("        FROM KOSMOS_EMR.AVIEWCHARTINFO BB1");
                            SQL.AppendLine("        WHERE BB1.EMRNO = A.EMRNO");
                            SQL.AppendLine("          AND BB1.FORMNO = 1796 ");
                            SQL.AppendLine("      )AS ACTYN ");

                            //SQL.AppendLine("        FROM KOSMOS_EMR.EMRXML_TUYAK BB1");
                            //SQL.AppendLine("        WHERE BB1.EMRNO = A.EMRNO )AS ACTYN ");

                            SQL.AppendLine(" FROM " + mstrACTTable + " A");
                            SQL.AppendLine(" LEFT OUTER JOIN " + ComNum.DB_ERP + "INSA_MST  B");
                            SQL.AppendLine("    ON TRIM(B.SABUN) = LPAD(TRIM(A.ACTSABUN), 5 , '0') ");
                            SQL.AppendLine("    AND ( B.TOIDAY IS NULL OR B.TOIDAY < TRUNC(SYSDATE) ) ");
                            SQL.AppendLine(" WHERE A.ORDERNO = :orderNo         ");
                            SQL.AppendLine("   AND A.PTNO  = :argPTNO           ");
                            SQL.AppendLine("   AND A.GBSTATUS NOT IN ('D+')     ");
                            SQL.AppendLine("   AND A.ACTDIV = :actDiv           ");

                            ClsParameter parameter = new ClsParameter();
                            parameter.Add(":orderNo", dt.Rows[i]["ORDERNO"].ToString().Trim());
                            parameter.Add(":argPTNO", argPTNO);
                            parameter.Add(":actDiv", Convert.ToString(h));

                            SqlErr = clsDB.GetAdoRsP(ref Rs1, SQL.ToString().Trim(), clsDB.DbCon, parameter);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (Rs1.HasRows == true)
                            {
                                //'TYPE 변경
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].CellType = ctText;
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].BackColor = Color.FromArgb(255, 200, 100);
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].Text = Rs1.GetValue(3).ToString().Trim() + ComNum.VBLF + Rs1.GetValue(2).ToString().Trim();

                                if (VB.Val(Rs1.GetValue(4).ToString()) <= 0)
                                {
                                    ssM_Sheet1.Rows.Get(ssM_Sheet1.RowCount - 1).BackColor = Color.FromArgb(255, 0, 0);
                                }
                            }

                            Rs1.Dispose();
                            Rs1 = null;
                            
                            //'DC 자료 READ
                            SQL.Clear();
                            SQL.AppendLine(" SELECT ");
                            SQL.AppendLine("    A.NURSEID, B.KORNAME AS ACTUSER, TO_CHAR(A.ENTDATE, 'DD HH24:MI') ENTDATE ");
                            SQL.AppendLine(" FROM KOSMOS_OCS.OCS_IORDER A");
                            SQL.AppendLine(" LEFT OUTER JOIN " + ComNum.DB_ERP + "INSA_MST  B");
                            SQL.AppendLine("    ON TRIM(B.SABUN) = LPAD(TRIM(A.NURSEID), 5 , '0') ");
                            SQL.AppendLine("    AND ( B.TOIDAY IS NULL OR B.TOIDAY < TRUNC(SYSDATE) ) ");
                            SQL.AppendLine(" WHERE A.PTNO  = :argPTNO ");
                            SQL.AppendLine("   AND A.BDATE = TO_DATE( :bDate ,'YYYY-MM-DD') ");
                            SQL.AppendLine("   AND A.ORDERNO = :oderNo ");
                            SQL.AppendLine("   AND A.GBSTATUS  IN ('D-') ");
                            SQL.AppendLine("   AND A.ORDERSITE IN ('DC1', 'NDC') ");
                            SQL.AppendLine("   AND A.DCDIV = :dcDiv ");

                            parameter = new ClsParameter();
                            parameter.Add(":argPTNO", argPTNO);
                            parameter.Add(":bDate", dt.Rows[i]["BDATE"].ToString().Trim());
                            parameter.Add(":orderNo", dt.Rows[i]["ORDERNO"].ToString().Trim());
                            parameter.Add(":dcDiv", Convert.ToString(h));

                            SqlErr = clsDB.GetAdoRsP(ref Rs1, SQL.ToString().Trim(), clsDB.DbCon, parameter);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (Rs1.HasRows == true)
                            {
                                //'TYPE 변경
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].CellType = ctText;
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].BackColor = Color.FromArgb(255, 50, 50);
                                if (cboWard.Text == "ER")
                                {
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].Text = "NDC" + ComNum.VBLF
                                                                                                   + Rs1.GetValue(1).ToString().Trim()
                                                                                                   + ComNum.VBLF + Rs1.GetValue(2).ToString().Trim();
                                }
                                else
                                {
                                    ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 13 + h].Text = "NDC" + Rs1.GetValue(1).ToString().Trim();
                                }
                            }

                            Rs1.Dispose();
                            Rs1 = null;
                            
                        }

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 40].Text = (dt.Rows[i]["POWDER"].ToString().Trim() == "1" ? "◎" : "");

                        //            .Col = 42

                        if (VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 16 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 21)
                        {
                            if (dt.Rows[i]["GBNGT"].ToString().Trim() != "")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 41].Text = dt.Rows[i]["GBNGT"].ToString().Trim();
                            }

                            if (dt.Rows[i]["GBGROUP"].ToString().Trim() != "")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 41].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                            }
                        }
                        else if (VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 28 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 39)
                        {
                            //'손동현 위를 아래로 한다.
                            //'처치/재료는 GbNgt    나머지는 Group
                            if (VB.IsNumeric(dt.Rows[i]["GBGROUP"].ToString().Trim()) == true || dt.Rows[i]["GBGROUP"].ToString().Trim() == "")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 41].Text = dt.Rows[i]["GBNGT"].ToString().Trim();
                            }
                            else
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 41].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                            }
                        }
                        else
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 41].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                        }

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 42].Text = dt.Rows[i]["GBER"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 43].Text = dt.Rows[i]["GBSELF"].ToString().Trim();

                        //            .Col = 45
                        if (string.Compare(dt.Rows[i]["SLIPNO"].ToString().Trim(), "A1") >= 0 && string.Compare(dt.Rows[i]["SLIPNO"].ToString().Trim(), "A4") <= 0)
                        {
                        }
                        else
                        {
                            if (dt.Rows[i]["Remark"].ToString().Trim() != "")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 44].Text = "#";
                            }
                            if (dt.Rows[i]["GbPrn"].ToString().Trim() != "")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 44].Text = dt.Rows[i]["GbPrn"].ToString().Trim();
                            }
                            if (dt.Rows[i]["GbTFlag"].ToString().Trim() == "T")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 44].Text = "T";
                            }
                            if (dt.Rows[i]["GbTFlag"].ToString().Trim() == "O")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 44].Text = "O";
                            }
                            if (dt.Rows[i]["GbTFlag"].ToString().Trim() == "A")
                            {
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 44].Text = "A";
                            }

                            if (dt.Rows[i]["GBPRN"].ToString().Trim() == "S" || ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 44].Text == "S")
                            {
                                //ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "(선)" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "(선수납)" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                            }
                            else if (dt.Rows[i]["GBPRN"].ToString().Trim() == "B")
                            {
                                //ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "(수)" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "(수납완료)" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                            }

                            if (dt.Rows[i]["GBPRN"].ToString().Trim() == "A")
                            {
                                //ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "(A)" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                                ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text = "(선수납)" + ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Text.Trim();
                            }
                        }

                        //            .Col = 46
                        if (dt.Rows[i]["GBPORT"].ToString().Trim() == "M")
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 45].Text = "M";
                        }

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 46].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 47].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 48].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 49].Text = dt.Rows[i]["NURREMARK"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 50].Text = dt.Rows[i]["ITEMCD"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 51].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 52].Text = dt.Rows[i]["NURREMARK"].ToString().Trim();

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 54].Text = dt.Rows[i]["PICKUPNAME"].ToString().Trim() + dt.Rows[i]["PICKUPBUSE"].ToString().Trim();

                        ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 55].Text = dt.Rows[i]["GBIOE"].ToString().Trim();     //2019-02-14

                        if (dt.Rows[i]["DISPRGB"].ToString().Trim() != "")
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 0, ssM_Sheet1.RowCount - 1, 13].ForeColor =
                            ColorTranslator.FromWin32(int.Parse(dt.Rows[i]["DISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                        }

                        if (dt.Rows[i]["BDATE"].ToString().Trim() != strBDATE)
                        {
                            ssM_Sheet1.Rows.Get(ssM_Sheet1.RowCount - 1).Border = new FarPoint.Win.LineBorder(Color.Red, 1, false, true, false, false);

                            strBDATE = dt.Rows[i]["BDATE"].ToString().Trim();

                            FnBRow = ssM_Sheet1.RowCount - 1;
                        }

                        if (dt.Rows[i]["BEFORDAY"].ToString().Trim() == "")
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 5].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 5].ForeColor = Color.FromArgb(0, 0, 0);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 6].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 6].ForeColor = Color.FromArgb(0, 0, 0);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 7].ForeColor = Color.FromArgb(0, 0, 0);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 9].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 9].ForeColor = Color.FromArgb(0, 0, 0);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 10].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 10].ForeColor = Color.FromArgb(0, 0, 0);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 11].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 11].ForeColor = Color.FromArgb(0, 0, 0);
                        }

                        if (ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Text == "Negative")
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Text = "-";
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Font = new Font("맑은 고딕", 20, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].ForeColor = Color.FromArgb(0, 0, 255);
                        }
                        else if (ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Text == "Positive")
                        {
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Text = "+";
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].Font = new Font("맑은 고딕", 20, FontStyle.Bold);
                            ssM_Sheet1.Cells[ssM_Sheet1.RowCount - 1, 3].ForeColor = Color.FromArgb(255, 0, 0);
                        }

                        ssM_Sheet1.SetRowHeight(ssM_Sheet1.RowCount - 1, Convert.ToInt32(ssM_Sheet1.GetPreferredRowHeight(ssM_Sheet1.RowCount - 1)) + 10);


                    }

                    for (i = 14; i < 38; i++)
                    {
                        ssM_Sheet1.Columns.Get(i).Visible = true;

                        if (i > nMaxDiv + 14)
                        {
                            ssM_Sheet1.Columns.Get(i).Visible = false;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;


                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Log.Debug("===================== READ_OCS_IORDER_MUTI loop END ===================== {}sec, {}ms", ts.Seconds, ts.Milliseconds);


            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }




        }

        private bool VERIFY_CHART(string argEMRNO)
        {
            bool RtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT EMRNO FROM KOSMOS_EMR.EMRXML_TUYAK ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + argEMRNO;

                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT EMRNO";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + argEMRNO;
                SQL = SQL + ComNum.VBLF + "   AND FORMNO = 1796";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = true;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return RtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return RtnVal;
            }
        }

        /// <summary>
        /// READ_수가분류
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private string READ_SuGaBunRu(string arg)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'BAS_수가분류코드'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + arg + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return RtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return RtnVal;
            }
        }

        private string READ_INOUT(string argPTNO)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strDateTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "DM");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT GBN ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_CADEX_INOUT ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SDATETIME <= TO_DATE('" + strDateTime + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND EDATETIME >= TO_DATE('" + strDateTime + "','YYYY-MM-DD HH24:MI') ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[0]["GBN"].ToString().Trim();

                    switch (RtnVal)
                    {
                        case "0":
                            RtnVal = "현재 외출 중입니다.";
                            break;

                        case "1":
                            RtnVal = "현재 외박 중입니다.";
                            break;
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return RtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return RtnVal;
            }
        }

        private bool READ_EXAMDRUG()
        {
            bool RtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT  PTNO ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.ENDO_JUPMST   ";
                SQL = SQL + ComNum.VBLF + "WHERE PTNO ='" + FstrPtno + "' ";
                SQL = SQL + ComNum.VBLF + "     AND ( BDATE =TO_DATE('" + mstrDate + "','YYYY-MM-DD')  OR RDATE =TO_DATE('" + mstrDate + "','YYYY-MM-DD') )";
                SQL = SQL + ComNum.VBLF + "     AND ORDERCODE IN ('00440913','00440160')  ";  //'대장내시경 코리트산;
                SQL = SQL + ComNum.VBLF + "     AND GBSUNAP ='1' ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT PANO";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.XRAY_DETAIL ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO ='" + FstrPtno + "' ";
                SQL = SQL + ComNum.VBLF + "     AND ( TRUNC(SEEKDATE)  =TO_DATE('" + mstrDate + "','YYYY-MM-DD')  OR BDATE  =TO_DATE('" + mstrDate + "','YYYY-MM-DD') )  ";
                SQL = SQL + ComNum.VBLF + "     AND XCODE IN ( 'HA474D') ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT PANO";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.XRAY_DETAIL ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO ='" + FstrPtno + "' ";
                SQL = SQL + ComNum.VBLF + "     AND ( TRUNC(SEEKDATE)  =TO_DATE('" + mstrDate + "','YYYY-MM-DD')  OR BDATE  =TO_DATE('" + mstrDate + "','YYYY-MM-DD') ) ";
                SQL = SQL + ComNum.VBLF + "     AND XCODE IN ( 'HA032') ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT PANO";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.XRAY_DETAIL ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO ='" + FstrPtno + "' ";
                SQL = SQL + ComNum.VBLF + "     AND BDATE  =TO_DATE('" + mstrDate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "     AND XCODE IN ( 'HA032','HA031') ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = true;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return RtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return RtnVal;
            }
        }

        private string READ_ALLERGY(string argPTNO)
        {
            string RtnVal = "";
            DataTable dt = null;
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT REMARK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_ALLERGY_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        RtnVal = RtnVal + dt.Rows[i]["REMARK"].ToString().Trim() + ",";
                    }

                    RtnVal = VB.Mid(RtnVal, 1, RtnVal.Length - 1);
                    RtnVal = "해당환자는 " + RtnVal + "에 알러지 반응이 있습니다. *** Order 엑팅시 꼭 확인하십시오***";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked == true)
            {
                //TODO 임시로 날짜 조정
                dtpDate2.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                dtpDate2.Visible = true;
                cboHH.Visible = true;
                cboMM.Visible = true;
            }
            else
            {
                dtpDate2.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                dtpDate2.Visible = false;
                cboHH.Visible = false;
                cboMM.Visible = false;
            }
        }

        private void btnSaveActSession_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return; //권한 확인

            string strNowDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            string strNowTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");

            if (chkDate.Checked == true)
            {
                if (Verificate_Date(dtpDate2.Value.ToString("yyyy-MM-dd"), cboHH.Text.Trim() + ":" + cboMM.Text.Trim()) == false)
                {
                    return;
                }
            }

            ////2020-05-21
            #region 퇴실시간 이후 액팅건 제한 작업
            string strOTDT = "";

            string strACTTIME = "";

            if (chkDate.Checked == true)
            {
                strACTTIME = dtpDate2.Value.ToString("yyyy-MM-dd") + " " + cboHH.Text.Trim() + ":" + cboMM.Text.Trim();
            }
            else
            {
                strACTTIME = strNowDate + " " + strNowTime;
            }


            strOTDT = readEMIHOTDT(FstrPtno, FstrInDate, cboWard.Text.Trim());

            if (optBun4.Checked == true && cboWard.Text == "ER" && strOTDT != "")
            {
                //2021 - 04 - 09 테스트 후 업로드 예정
                if (readEMIHOTDT2(FstrPtno, FstrInDate, cboWard.Text.Trim(), strACTTIME) == true)
                {

                }
                else
                {
                    if (chkDate.Checked == true)
                    {
                        if (Convert.ToDateTime(dtpDate2.Value.ToString("yyyy-MM-dd") + " " + cboHH.Text.Trim() + ":" + cboMM.Text.Trim()) > Convert.ToDateTime(strOTDT))
                        {
                            ComFunc.MsgBox("액팅시간이 퇴실시간 이후입니다. 퇴실시간 이전으로 해주세요.");
                            return;
                        }
                    }
                    else
                    {
                        if (Convert.ToDateTime(strNowDate + " " + strNowTime) > Convert.ToDateTime(strOTDT))
                        {
                            ComFunc.MsgBox("액팅시간이 퇴실시간 이후입니다. 퇴실시간 이전으로 해주세요.");
                            return;
                        }
                    }
                }
            } 
            #endregion

            if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == false)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("정말로 Acting을 실시 하시겠습니까?", "Acting 확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            if (clsType.User.Sabun == "")
            {
                ComFunc.MsgBox("실시자 정보가 없습니다." + ComNum.VBLF + "Re Login해 주십시오");
                return;
            }

            int i = 0;
            int h = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            string strROWID = "";
            int nOrderNo = 0;

            int nActDivOLD = 0;
            int nActDiv = 0;
            int nActCol = 0;
            string strActTime = "";
            string strORDERCODE = "";
            string strOrderName = "";

            string strInDate = "";
            string strINOUT = "";

            string strSEQNO = "";  //'자가약 식별 시퀀스
            int nTimes = 0;

            string strFormNo = "";

            string strRndNo = "9" + VB.Val(FstrPtno).ToString("999999") + ComQuery.CurrentDateTime(clsDB.DbCon, "T"); //965 기록지 임시 strEMRNO

            string strData = "";
            string strEMRNO = "";

            string strDcOrderMsg = "";
            string strEMRNOMsg = "";

            string strData965 = "";

            mstrXml = "";
            mstrTagHead = null;
            mstrTagTail = null;

            mstrUSEID = "";
            mstrChartDate = "";
            mstrChartTime = "";
            mintAcpNo = 0;
            mstrPTNO = "";
            mstrInOutCls = "";
            mstrMedFrDate = "";
            mstrMedEndDate = "";
            mstrMedDeptCd = "";
            mstrMedDrCd = "";
            mstrMedFrTime = "";
            mstrMedEndTime = "";
            mstrNewChartCd = "";

            if (optBun2.Checked == true) //|| optBun3.Checked = true)
            {
                strFormNo = "965";
            }
            else if (optBun0.Checked == true || optBun1.Checked == true)
            {
                strFormNo = "1796";
            }
            else if (optBun4.Checked == true)
            {
                if (mstrWard == "ER")
                {
                    strFormNo = "2049";
                }
                else
                {
                    strFormNo = "2504";
                }
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                mstrUSEID = clsType.User.Sabun;

                if (chkDate.Checked == true)
                {
                    strActTime = dtpDate2.Value.ToString("yyyy-MM-dd") + " " + cboHH.Text.Trim() + ":" + cboMM.Text.Trim();
                    mstrChartDate = dtpDate2.Value.ToString("yyyyMMdd");
                    mstrChartTime = cboHH.Text.Trim() + cboMM.Text.Trim() + "00";
                }
                else
                {
                    mstrChartDate = strNowDate.Replace("-", "");
                    mstrChartTime = strNowTime.Replace(":", "");   //'Format(strSysTime, "HH24MISS")
                    strActTime = strNowDate + " " + strNowTime;
                }

                mintAcpNo = 0;
                mstrPTNO = FstrPtno;
                // 응급실은 InOutCls를 외래로 변경
                if (cboWard.Text.Trim() == "ER")
                {
                    mstrInOutCls = "O";
                }
                else
                {
                    mstrInOutCls = "I";
                }

                mstrMedFrDate = FstrInDate.Replace("-", "");
                strInDate = FstrInDate;
                mstrMedEndDate = FstrEndDate.Replace("-", "");
                mstrMedDeptCd = FstrDEPT;
                mstrMedDrCd = FstrDrCode;

                mstrMedFrTime = "120000";
                mstrMedEndTime = "";
                mstrNewChartCd = strFormNo;

                strData = "";

                EmrForm pForm = clsEmrChart.ClearEmrForm();
                Dictionary<string, string> strDataNew = new Dictionary<string, string>();
                string strSAVEGB = "1";
                string strSAVECERT = "1";

                //'<<<<<
                for (i = 0; i < ssM_Sheet1.RowCount; i++)
                {
                    strROWID = ssM_Sheet1.Cells[i, 47].Text.Trim();
                    nActCol = 0;

                    for (h = 14; h < 38; h++)
                    {
                        if (ssM_Sheet1.Columns.Get(h).Visible == false)
                        {
                            break;
                        }

                        nActCol = nActCol + 1;

                        if (ssM_Sheet1.Cells[i, h].CellType != null && ssM_Sheet1.Cells[i, h].CellType.ToString() == "CheckBoxCellType")
                        {
                            if (Convert.ToBoolean(ssM_Sheet1.Cells[i, h].Value) == true)
                            {
                                nOrderNo = Convert.ToInt32(VB.Val(ssM_Sheet1.Cells[i, 48].Text.Trim()));
                                nActDivOLD = Convert.ToInt32(VB.Val(ssM_Sheet1.Cells[i, 13].Text.Trim()));

                                //중복 acting 문제로 다시 한번 읽어서 처리 비교함

                                SQL = "";
                                SQL = "SELECT GBDIV , ACTDIV, GBSTATUS      ";
                                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_IORDER       ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt.Rows.Count > 0)
                                {
                                    if (VB.Val(dt.Rows[0]["GBDIV"].ToString().Trim()) == 1)
                                    {
                                        if (ssM_Sheet1.Cells[i, 0].Text.Trim() == "DC")
                                        {
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            dt.Dispose();
                                            dt = null;
                                            ComFunc.MsgBox("QD 처방은 의사가 DC 한경우 액팅 할 수 없습니다.  8332로 연락 주세요.");
                                            return;
                                        }
                                    }

                                    nActDiv = Convert.ToInt32(VB.Val(dt.Rows[0]["ACTDIV"].ToString().Trim()));

                                    if (VB.Val(dt.Rows[0]["GBDIV"].ToString().Trim()) <= VB.Val(dt.Rows[0]["ACTDIV"].ToString().Trim()))
                                    {
                                        dt.Dispose();
                                        dt = null;
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(" ACTING 작업중 오류 발생 하였습니다. " + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 ACTGING 하세요 ", "오류");
                                        return;
                                    }

                                    if (nActDivOLD != nActDiv)
                                    {
                                        dt.Dispose();
                                        dt = null;
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(" ACTING 작업중 오류 발생 하였습니다. " + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 ACTGING 하세요 ", "오류");
                                        return;
                                    }

                                    if (dt.Rows[0]["GBSTATUS"].ToString().Trim() == "D")
                                    {
                                        strDcOrderMsg = strDcOrderMsg + strORDERCODE + " " + strOrderName + ComNum.VBLF;
                                    }
                                }
                                else
                                {
                                    dt.Dispose();
                                    dt = null;
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(" ACTING 작업중 오류 발생 하였습니다. " + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 ACTGING 하세요 ", "오류");
                                    return;
                                }

                                dt.Dispose();
                                dt = null;

                                #region 수액기록지 없으면 생성
                                if (clsEmrQuery.SapExists(clsDB.DbCon, strROWID) &&
                                    clsEmrQuery.SapOrderExists(clsDB.DbCon, strROWID) == false &&
                                    clsEmrQuery.SaveActOrder(clsDB.DbCon, pAcp, strROWID, mstrChartDate, mstrChartTime, chkDate.Checked))
                                {
                                    //clsDB.setRollbackTran(clsDB.DbCon);
                                    //ComFunc.MsgBoxEx(this, "수액 기록지 저장 도중 오류가 발생했습니다.", "오류");
                                    //return;
                                }
                                #endregion

                                //'>>mtsEmr
                                strEMRNO = "";

                                if (strFormNo == "965")
                                {
                                    if (ssM_Sheet1.Cells[i, 1].Text.Trim() != "")
                                    {
                                        strData965 = strData965 + "(" + ssM_Sheet1.Cells[i, 1].Text.Trim() + ")";    //'구분
                                    }
                                    //strData965 = strData965 + ssM_Sheet1.Cells[i, 7].Text.Trim().Replace("[CP]", "");    //'OrderName 필요시 주석 풀어서 처리
                                    strData965 = strData965 + ssM_Sheet1.Cells[i, 7].Text.Trim();    //'OrderName
                                    strData965 = strData965 + ComNum.VBLF;
                                    strEMRNO = strRndNo;
                                }
                                else if (strFormNo == "2049")
                                {
                                    strData = "";

                                    //'간호기록
                                    SetTagHeadTagTail(strFormNo, ref mstrTagHead, ref mstrTagTail);

                                    if (ssM_Sheet1.Cells[i, 1].Text.Trim() != "")
                                    {
                                        strData = mstrTagHead[0] + "(" + ssM_Sheet1.Cells[i, 1].Text.Trim() + ")" + ssM_Sheet1.Cells[i, 7].Text.Trim() + mstrTagTail[0]; //'구분'OrderName
                                        //strData = mstrTagHead[0] + "(" + ssM_Sheet1.Cells[i, 1].Text.Trim() + ")" + ssM_Sheet1.Cells[i, 7].Text.Trim().Replace("[CP]", "") + mstrTagTail[0]; //'구분'OrderName      //필요시 주석 풀어서 처리
                                    }
                                    else
                                    {
                                        strData = mstrTagHead[0] + ssM_Sheet1.Cells[i, 7].Text.Trim() + mstrTagTail[0]; //'구분'OrderName
                                        //strData = mstrTagHead[0] + ssM_Sheet1.Cells[i, 7].Text.Trim().Replace("[CP]","") + mstrTagTail[0]; //'구분'OrderName 필요시 주석 제외 처리
                                    }

                                    mstrXml = "";
                                    mstrXml = lblXmlHead.Text.Trim() + mstrChartX1 + strData + mstrChartX2;

                                    strEMRNO = Convert.ToString(ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GETEMRXMLNO"));

                                    pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFormNo, clsEmrQuery.GetNewFormMaxUpdateNo(clsDB.DbCon, VB.Val(strFormNo)).ToString());

                                    if (pForm.FmOLDGB == 1)
                                    {
                                        //emr 저장
                                        if (clsNurse.CREATE_EMR_XMLINSRT3(VB.Val(strEMRNO), strFormNo, clsType.User.IdNumber, mstrChartDate, mstrChartTime, mintAcpNo,
                                            mstrPTNO, mstrInOutCls, mstrMedFrDate, mstrMedFrTime, mstrMedEndDate, mstrMedEndTime, mstrMedDeptCd, mstrMedDrCd, "0", 0, mstrXml) == false)
                                        {
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            ComFunc.MsgBox(" ACTING 작업중 오류 발생 하였습니다. " + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 ACTGING 하세요 ", "오류");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if (ssM_Sheet1.Cells[i, 1].Text.Trim() != "")
                                        {
                                            strData = "(" + ssM_Sheet1.Cells[i, 1].Text.Trim() + ")" + ssM_Sheet1.Cells[i, 7].Text.Trim(); //'구분'OrderName
                                            //strData = "(" + ssM_Sheet1.Cells[i, 1].Text.Trim() + ")" + ssM_Sheet1.Cells[i, 7].Text.Trim().Replace("[CP]",""); //'구분'OrderName
                                        }
                                        else
                                        {
                                            strData = ssM_Sheet1.Cells[i, 7].Text.Trim(); //'구분'OrderName
                                            //strData = ssM_Sheet1.Cells[i, 7].Text.Trim().Replace("[CP]",""); //'구분'OrderName
                                        }

                                        #region 신규 저장
                                        strEMRNO = clsEmrQuery.SaveNurseRecord(clsDB.DbCon, pAcp, strFormNo, pForm.FmUPDATENO.ToString(), "0", mstrChartDate, mstrChartTime,
                                                               clsType.User.IdNumber, clsType.User.IdNumber, strSAVEGB, strSAVECERT, "0", "",
                                                               "", "", strData.Replace("'", "`"), "", "ER", "", false).ToString();
                                        #endregion
                                    }
                                }
                                else if (strFormNo == "2504")
                                {
                                    strData = "";

                                    pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFormNo, clsEmrQuery.GetNewFormMaxUpdateNo(clsDB.DbCon, VB.Val(strFormNo)).ToString());

                                    if (pForm.FmOLDGB == 1)
                                    {
                                        //'간호기록
                                        SetTagHeadTagTail(strFormNo, ref mstrTagHead, ref mstrTagTail);

                                        strData = mstrTagHead[0] + Convert.ToDateTime(ssM_Sheet1.Cells[i, 51].Text.Trim()).ToString("yyyyMMdd") + mstrTagTail[0]; //'처방일자
                                        strData = strData + mstrTagHead[1] + ssM_Sheet1.Cells[i, 2].Text.Trim() + mstrTagTail[1]; //'처방코드
                                        strData = strData + mstrTagHead[2] + ssM_Sheet1.Cells[i, 7].Text.Trim() + mstrTagTail[2]; //'처방명
                                        //strData = strData + mstrTagHead[2] + ssM_Sheet1.Cells[i, 7].Text.Trim().Replace("[CP]","") + mstrTagTail[2]; //'처방명
                                        strData = strData + mstrTagHead[3] + ssM_Sheet1.Cells[i, 8].Text.Trim() + mstrTagTail[3]; //'용법검체
                                        strData = strData + mstrTagHead[4] + ssM_Sheet1.Cells[i, 10].Text.Trim() + mstrTagTail[4]; //'일투량
                                        strData = strData + mstrTagHead[5] + ssM_Sheet1.Cells[i, 9].Text.Trim() + mstrTagTail[5]; //'횟수

                                        //'=============================================================
                                        //'2010-04-26 김현욱 추가
                                        //'항생제 반응 및 외출외박 내역 투약기록지 REMARK에 자동 뿌려주기
                                        if (chkInOit.Checked == false)
                                        {
                                            if (lblINOUT.Text.Trim() == "현재 외출 중입니다.")
                                            {
                                                strINOUT = "(외출)";
                                            }
                                            else if (lblINOUT.Text.Trim() == "현재 외박 중입니다.")
                                            {
                                                strINOUT = "(외박)";
                                            }
                                            else
                                            {
                                                strINOUT = "";
                                            }
                                        }
                                        //'=============================================================

                                        strData = strData + mstrTagHead[6] + strINOUT + ssM_Sheet1.Cells[i, 46].Text.Trim() + " "
                                            + clsIpdNr.READ_AST_REACTION(clsDB.DbCon, strORDERCODE, FstrPtno, strInDate) + mstrTagTail[6]; //'비고
                                        strData = strData + mstrTagHead[7] + strROWID + mstrTagTail[7]; //'RowId
                                        strData = strData + mstrTagHead[8] + ssM_Sheet1.Cells[i, 16].Text.Trim() + mstrTagTail[8]; //'ACTING 컬럼
                                        strData = strData + mstrTagHead[9] + ssM_Sheet1.Cells[i, 49].Text.Trim() + mstrTagTail[9]; //참고사항

                                        mstrXml = "";
                                        mstrXml = lblXmlHead.Text.Trim() + mstrChartX1 + strData + mstrChartX2;

                                        strEMRNO = Convert.ToString(ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GETEMRXMLNO"));

                                        //emr 저장
                                        if (clsNurse.CREATE_EMR_XMLINSRT3(VB.Val(strEMRNO), strFormNo, clsType.User.IdNumber, mstrChartDate, mstrChartTime, mintAcpNo,
                                            mstrPTNO, mstrInOutCls, mstrMedFrDate, mstrMedFrTime, mstrMedEndDate, mstrMedEndTime, mstrMedDeptCd, mstrMedDrCd, "0", 1, mstrXml) == false)
                                        {
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            ComFunc.MsgBox(" ACTING 작업중 오류 발생 하였습니다. " + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 ACTGING 하세요 ", "오류");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        #region 신규 기록지 저장
                                        strDataNew.Clear();
                                        strDataNew.Add("I0000016396", Convert.ToDateTime(ssM_Sheet1.Cells[i, 51].Text.Trim()).ToString("yyyyMMdd")); //'처방일자
                                        strDataNew.Add("I0000037685", ssM_Sheet1.Cells[i, 2].Text.Trim()); //'처방코드
                                        strDataNew.Add("I0000006552", ssM_Sheet1.Cells[i, 7].Text.Trim()); //'처방명
                                        //strDataNew.Add("I0000006552", ssM_Sheet1.Cells[i, 7].Text.Trim().Replace("[CP]","")); //'처방명
                                        strDataNew.Add("I0000037687", ssM_Sheet1.Cells[i, 8].Text.Trim()); //'용법검체
                                        strDataNew.Add("I0000037686", ssM_Sheet1.Cells[i, 10].Text.Trim()); //'일투량
                                        strDataNew.Add("I0000013529", ssM_Sheet1.Cells[i, 9].Text.Trim()); //'횟수

                                        //'=============================================================
                                        //'2010-04-26 김현욱 추가
                                        //'항생제 반응 및 외출외박 내역 투약기록지 REMARK에 자동 뿌려주기
                                        if (chkInOit.Checked == false)
                                        {
                                            if (lblINOUT.Text.Trim() == "현재 외출 중입니다.")
                                            {
                                                strINOUT = "(외출)";
                                            }
                                            else if (lblINOUT.Text.Trim() == "현재 외박 중입니다.")
                                            {
                                                strINOUT = "(외박)";
                                            }
                                            else
                                            {
                                                strINOUT = "";
                                            }
                                        }
                                        //'=============================================================

                                        strDataNew.Add("I0000001311", strINOUT + ssM_Sheet1.Cells[i, 46].Text.Trim() + " "
                                            + clsIpdNr.READ_AST_REACTION(clsDB.DbCon, strORDERCODE, FstrPtno, strInDate)); //'비고
                                        strDataNew.Add("I0000031495", strROWID); //'RowId
                                        strDataNew.Add("I0000037688", ssM_Sheet1.Cells[i, 16].Text.Trim()); //'ACTING 컬럼
                                        strDataNew.Add("I0000010053", ssM_Sheet1.Cells[i, 49].Text.Trim()); //참고사항

                                        strEMRNO = clsEmrQuery.SaveNurChartFlow(clsDB.DbCon, this, pAcp, pForm, mstrChartDate, mstrChartTime, strDataNew).ToString();
                                        #endregion
                                    }

                                }
                                else
                                {
                                    pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFormNo, clsEmrQuery.GetNewFormMaxUpdateNo(clsDB.DbCon, VB.Val(strFormNo)).ToString());


                                    if (pForm.FmOLDGB == 1)
                                    {
                                        #region XML
                                        //'투약기록지
                                        strEMRNO = "";
                                        strEMRNO = Convert.ToString(ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GETEMRXMLNO"));

                                        SQL = "";
                                        SQL = "INSERT INTO KOSMOS_EMR.EMRXML_TUYAK  ";
                                        SQL = SQL + ComNum.VBLF + "(  ";
                                        SQL = SQL + ComNum.VBLF + "    EMRNO, FORMNO, USEID, CHARTDATE, CHARTTIME,  ";
                                        SQL = SQL + ComNum.VBLF + "    ACPNO, PTNO, INOUTCLS, MEDFRDATE, MEDFRTIME,  ";
                                        SQL = SQL + ComNum.VBLF + "    MEDENDDATE, MEDENDTIME, MEDDEPTCD, MEDDRCD, MIBICHECK,  ";
                                        SQL = SQL + ComNum.VBLF + "    WRITEDATE, WRITETIME, IT1, IT2,  ";
                                        SQL = SQL + ComNum.VBLF + "    IT3, IT4, IT5, IT6, IT7,  ";
                                        SQL = SQL + ComNum.VBLF + "    IT8, IT9, IT10  ";
                                        SQL = SQL + ComNum.VBLF + ")  ";
                                        SQL = SQL + ComNum.VBLF + "    VALUES  ";
                                        SQL = SQL + ComNum.VBLF + "(  ";
                                        SQL = SQL + ComNum.VBLF + "    " + strEMRNO + ",  ";
                                        SQL = SQL + ComNum.VBLF + "    " + strFormNo + ",  ";
                                        SQL = SQL + ComNum.VBLF + "    '" + mstrUSEID + "',  ";
                                        SQL = SQL + ComNum.VBLF + "    '" + mstrChartDate + "',  ";
                                        SQL = SQL + ComNum.VBLF + "    '" + mstrChartTime + "',  ";
                                        SQL = SQL + ComNum.VBLF + "    " + mintAcpNo + ",  ";
                                        SQL = SQL + ComNum.VBLF + "    '" + mstrPTNO + "',  ";
                                        SQL = SQL + ComNum.VBLF + "    '" + (VB.GetSetting("TWIN", "NURVEIW", "WARDCODE") == "ER" ? "O" : mstrInOutCls) + "',  ";
                                        SQL = SQL + ComNum.VBLF + "    '" + mstrMedFrDate + "',  ";
                                        SQL = SQL + ComNum.VBLF + "    '" + mstrMedFrTime + "',  ";
                                        SQL = SQL + ComNum.VBLF + "    '" + mstrMedEndDate + "',  ";
                                        SQL = SQL + ComNum.VBLF + "    '" + mstrMedEndTime + "',  ";
                                        SQL = SQL + ComNum.VBLF + "    '" + mstrMedDeptCd + "',  ";
                                        SQL = SQL + ComNum.VBLF + "    '" + mstrMedDrCd + "',  ";
                                        SQL = SQL + ComNum.VBLF + "    '0',  ";
                                        SQL = SQL + ComNum.VBLF + "    '" + strNowDate.Replace("-", "") + "',  ";
                                        SQL = SQL + ComNum.VBLF + "    '120000',  ";
                                        SQL = SQL + ComNum.VBLF + "    '" + Convert.ToDateTime(ssM_Sheet1.Cells[i, 51].Text.Trim()).ToString("yyyyMMdd") + "',  "; //'처방일자
                                        SQL = SQL + ComNum.VBLF + "    '" + ssM_Sheet1.Cells[i, 2].Text.Trim() + "',  "; //'처방코드
                                        SQL = SQL + ComNum.VBLF + "    '" + ssM_Sheet1.Cells[i, 7].Text.Trim() + "',  "; //'처방명
                                        //SQL = SQL + ComNum.VBLF + "    '" + ssM_Sheet1.Cells[i, 7].Text.Trim().Replace("[CP]","") + "',  "; //'처방명
                                        SQL = SQL + ComNum.VBLF + "    '" + ssM_Sheet1.Cells[i, 8].Text.Trim() + "',  "; //'용법검체
                                        SQL = SQL + ComNum.VBLF + "    '" + ssM_Sheet1.Cells[i, 10].Text.Trim() + "',  "; //'일투량
                                        SQL = SQL + ComNum.VBLF + "    '" + ssM_Sheet1.Cells[i, 9].Text.Trim() + "',  "; //'횟수

                                        //'=============================================================
                                        //'2010-04-26 김현욱 추가
                                        //'항생제 반응 및 외출외박 내역 투약기록지 REMARK에 자동 뿌려주기
                                        if (chkInOit.Checked == false)
                                        {
                                            if (lblINOUT.Text.Trim() == "현재 외출 중입니다.")
                                            {
                                                strINOUT = "(외출)";
                                            }
                                            else if (lblINOUT.Text.Trim() == "현재 외박 중입니다.")
                                            {
                                                strINOUT = "(외박)";
                                            }
                                            else
                                            {
                                                strINOUT = "";
                                            }
                                        }
                                        //'=============================================================

                                        SQL = SQL + ComNum.VBLF + "    '" + strINOUT + ssM_Sheet1.Cells[i, 46].Text.Trim() + " "
                                            + clsIpdNr.READ_AST_REACTION(clsDB.DbCon, strORDERCODE, FstrPtno, strInDate) + "',  "; //'비고
                                        SQL = SQL + ComNum.VBLF + "    '" + strROWID + "',  "; //'RowId
                                        SQL = SQL + ComNum.VBLF + "    '" + ssM_Sheet1.Cells[i, 16].Text.Trim() + "',  "; //'ACTING 컬럼
                                        SQL = SQL + ComNum.VBLF + "    '" + ssM_Sheet1.Cells[i, 49].Text.Trim() + "'  "; //참고사항
                                        SQL = SQL + ComNum.VBLF + ")  ";

                                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                        if (SqlErr != "")
                                        {
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            ComFunc.MsgBox("투약기록지 생성중 에러 발생");
                                            Cursor.Current = Cursors.Default;
                                            return;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        strDataNew.Clear();
                                        strDataNew.Add("I0000016396", Convert.ToDateTime(ssM_Sheet1.Cells[i, 51].Text.Trim()).ToString("yyyyMMdd")); //처방일자
                                        strDataNew.Add("I0000037685", ssM_Sheet1.Cells[i, 2].Text.Trim()); //처방코드
                                        strDataNew.Add("I0000006552", ssM_Sheet1.Cells[i, 7].Text.Trim()); //처방명
                                        //strDataNew.Add("I0000006552", ssM_Sheet1.Cells[i, 7].Text.Trim().Replace("[CP]","")); //처방명
                                        strDataNew.Add("I0000037687", ssM_Sheet1.Cells[i, 8].Text.Trim()); //용법검체
                                        strDataNew.Add("I0000037847", ssM_Sheet1.Cells[i, 9].Text.Trim()); //총수량
                                        strDataNew.Add("I0000011700", ssM_Sheet1.Cells[i, 10].Text.Trim()); //횟수
                                        strDataNew.Add("I0000031009", ssM_Sheet1.Cells[i, 11].Text.Trim()); //1회 투여량
                                        strDataNew.Add("I0000001311", strINOUT + ssM_Sheet1.Cells[i, 46].Text.Trim() + " "
                                            + clsIpdNr.READ_AST_REACTION(clsDB.DbCon, strORDERCODE, FstrPtno, strInDate)); //비고
                                        strDataNew.Add("I0000031495", strROWID); //RowId
                                        strDataNew.Add("I0000037688", ssM_Sheet1.Cells[i, 16].Text.Trim()); //ACTING 컬럼
                                        strDataNew.Add("I0000010053", ssM_Sheet1.Cells[i, 49].Text.Trim()); //참고사항

                                        strEMRNO = clsEmrQuery.SaveNurChartFlow(clsDB.DbCon, this, pAcp, pForm, mstrChartDate, mstrChartTime, strDataNew).ToString();
                                    }

                                  
                                }
                                //    '<<<<

                                SQL = "";
                                SQL = "INSERT INTO " + mstrACTTable;
                                SQL = SQL + ComNum.VBLF + " ( PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, ";
                                SQL = SQL + ComNum.VBLF + "   BUN, GBORDER, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, ";
                                SQL = SQL + ComNum.VBLF + "   GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, ";
                                SQL = SQL + ComNum.VBLF + "   GBSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ";
                                SQL = SQL + ComNum.VBLF + "   ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, ";
                                SQL = SQL + ComNum.VBLF + "   DUR, LABELPRINT, ACTDIV, ACTTIME, ACTSABUN, DIVQTY, GBIOE, EMRNO  )  ";
                                SQL = SQL + ComNum.VBLF + " SELECT PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE,  ";
                                SQL = SQL + ComNum.VBLF + " SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, ";
                                SQL = SQL + ComNum.VBLF + " DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, ";
                                SQL = SQL + ComNum.VBLF + " GBTFLAG, GBSEND, GBPOSITION, ";
                                SQL = SQL + ComNum.VBLF + " ' ' , ";  //' GBSTATUS NULL 처리 (의사가 DC 되더라도) NULL 처리;
                                SQL = SQL + ComNum.VBLF + " NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, ";
                                SQL = SQL + ComNum.VBLF + "GBPORT, ORDERSITE, MULTI, MULTIREMARK, DUR, LABELPRINT, " + Convert.ToString(nActCol);

                                if (chkDate.Checked == true)
                                {
                                    SQL = SQL + ComNum.VBLF + "  , TO_DATE('" + strActTime + "','YYYY-MM-DD HH24:MI:SS') ,";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + " , SYSDATE,";
                                }

                                SQL = SQL + Convert.ToString(Convert.ToInt32(clsType.User.Sabun)) + ", REALQTY / GBDIV , GBIOE, " + (strEMRNO == "" ? "0" : strEMRNO);
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER ";
                                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                nActDiv = nActDiv + 1;

                                SQL = "";
                                SQL = " SELECT COUNT(*) CNT ";
                                SQL = SQL + ComNum.VBLF + "  FROM " + mstrACTTable;
                                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + FstrPtno + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + Convert.ToString(nOrderNo) + " ";
                                SQL = SQL + ComNum.VBLF + "   AND GBSTATUS = ' ' ";

                                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt.Rows.Count > 0)
                                {
                                    nActDiv = Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim());
                                }

                                dt.Dispose();
                                dt = null;

                                SQL = "";
                                SQL = " UPDATE KOSMOS_OCS.OCS_IORDER  ";
                                SQL = SQL + ComNum.VBLF + "SET ACTDIV = " + Convert.ToString(nActDiv) + "";
                                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }
                        }
                    }
                }

                if (strFormNo == "965")
                {
                    #region XML
                    pForm.FmOLDGB = 0;

                    if (pForm.FmOLDGB == 1)
                    {
                        SetTagHeadTagTail(strFormNo, ref mstrTagHead, ref mstrTagTail);

                        strData = "";
                        strData = mstrTagHead[0] + mstrWard + mstrTagTail[0];  //병동
                        strData = strData + mstrTagHead[1] + FstrRoom + mstrTagTail[1]; //호실
                        strData = strData + mstrTagHead[2] + "" + mstrTagTail[2]; //간호문제
                        strData = strData + mstrTagHead[3] + "" + mstrTagTail[3]; //구분
                        strData = strData + mstrTagHead[4] + strData965 + mstrTagTail[4]; //간호기록

                        mstrXml = "";
                        mstrXml = lblXmlHead.Text.Trim() + mstrChartX1 + strData + mstrChartX2;

                        strEMRNO = Convert.ToString(ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GETEMRXMLNO"));

                        if (clsNurse.CREATE_EMR_XMLINSRT3(VB.Val(strEMRNO), strFormNo, clsType.User.IdNumber, mstrChartDate, mstrChartTime, mintAcpNo,
                            mstrPTNO, mstrInOutCls, mstrMedFrDate, mstrMedFrTime, mstrMedEndDate, mstrMedEndTime,
                            mstrMedDeptCd, mstrMedDrCd, "0", 0, mstrXml) == false)
                        {
                            strEMRNOMsg = "간호기록지가 생성되지 않았습니다. 반드시 의료정보과(8333 김현욱계장)로 연락하시기 바랍니다.";
                        }
                        else
                        {
                            SQL = "";
                            SQL = " UPDATE " + mstrACTTable;
                            SQL = SQL + ComNum.VBLF + " SET EMRNO = " + strEMRNO;
                            SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + FstrPtno + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + dtpDate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND EMRNO = " + strRndNo;

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                    else
                    {

                        double dblEmrNo = clsEmrQuery.SaveNurseRecord(clsDB.DbCon, pAcp, pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), "0", mstrChartDate, mstrChartTime,
                                                                 clsType.User.IdNumber, clsType.User.IdNumber, strSAVEGB, strSAVECERT, "0", "",
                                                                 "", "", strData965, "", mstrWard, FstrRoom, false);

                        SQL = "";
                        SQL = " UPDATE " + mstrACTTable;
                        SQL = SQL + ComNum.VBLF + " SET EMRNO = " + dblEmrNo;
                        SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + FstrPtno + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + dtpDate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND EMRNO = " + strRndNo;

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    #endregion
                }

                pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, "1796", clsEmrQuery.GetNewFormMaxUpdateNo(clsDB.DbCon, 1796).ToString());


                for (i = 0; i < ssS_Sheet1.RowCount; i++)
                {
                    strSEQNO = ssS_Sheet1.Cells[i, 14].Text.Trim();
                    nTimes = Convert.ToInt32(VB.Val(ssS_Sheet1.Cells[i, 5].Text.Trim()));
                    for (h = 8; h < 8 + nTimes; h++)
                    {
                        if (ssS_Sheet1.Cells[i, h].CellType != null && ssS_Sheet1.Cells[i, h].CellType.ToString() == "CheckBoxCellType")
                        {
                            if (Convert.ToBoolean(ssS_Sheet1.Cells[i, h].Value) == true)
                            {
                                strData = "";
                                strEMRNO = "";

                                if (pForm.FmOLDGB == 1)
                                {
                                    #region XML
                                    strEMRNO = Convert.ToString(ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GETEMRXMLNO"));

                                    SQL = "";
                                    SQL = "INSERT INTO KOSMOS_EMR.EMRXML_TUYAK  ";
                                    SQL = SQL + ComNum.VBLF + "(  ";
                                    SQL = SQL + ComNum.VBLF + "    EMRNO, FORMNO, USEID, CHARTDATE, CHARTTIME,  ";
                                    SQL = SQL + ComNum.VBLF + "    ACPNO, PTNO, INOUTCLS, MEDFRDATE, MEDFRTIME,  ";
                                    SQL = SQL + ComNum.VBLF + "    MEDENDDATE, MEDENDTIME, MEDDEPTCD, MEDDRCD, MIBICHECK,  ";
                                    SQL = SQL + ComNum.VBLF + "    WRITEDATE, WRITETIME, IT1, IT2,  ";
                                    SQL = SQL + ComNum.VBLF + "    IT3, IT4, IT5, IT6, IT7,  ";
                                    SQL = SQL + ComNum.VBLF + "    IT8, IT9, IT10  ";
                                    SQL = SQL + ComNum.VBLF + ")  ";
                                    SQL = SQL + ComNum.VBLF + "    VALUES  ";
                                    SQL = SQL + ComNum.VBLF + "(  ";
                                    SQL = SQL + ComNum.VBLF + "    " + strEMRNO + ",  ";
                                    SQL = SQL + ComNum.VBLF + "    1796,  ";
                                    SQL = SQL + ComNum.VBLF + "    '" + mstrUSEID + "',  ";
                                    SQL = SQL + ComNum.VBLF + "    '" + mstrChartDate + "',  ";
                                    SQL = SQL + ComNum.VBLF + "    '" + mstrChartTime + "',  ";
                                    SQL = SQL + ComNum.VBLF + "    0,  ";
                                    SQL = SQL + ComNum.VBLF + "    '" + mstrPTNO + "',  ";
                                    SQL = SQL + ComNum.VBLF + "    '" + (VB.GetSetting("TWIN", "NURVEIW", "WARDCODE") == "ER" ? "O" : mstrInOutCls) + "',  ";
                                    SQL = SQL + ComNum.VBLF + "    '" + mstrMedFrDate + "',  ";
                                    SQL = SQL + ComNum.VBLF + "    '" + mstrMedFrTime + "',  ";
                                    SQL = SQL + ComNum.VBLF + "    '" + mstrMedEndDate + "',  ";
                                    SQL = SQL + ComNum.VBLF + "    '" + mstrMedEndTime + "',  ";
                                    SQL = SQL + ComNum.VBLF + "    '" + mstrMedDeptCd + "',  ";
                                    SQL = SQL + ComNum.VBLF + "    '" + mstrMedDrCd + "',  ";
                                    SQL = SQL + ComNum.VBLF + "    '0',  ";
                                    SQL = SQL + ComNum.VBLF + "    '" + strNowDate.Replace("-", "") + "',  ";
                                    SQL = SQL + ComNum.VBLF + "    '120000',  ";
                                    SQL = SQL + ComNum.VBLF + "    '" + dtpDate2.Value.ToString("yyyyMMdd") + "',  "; //'처방일자
                                    SQL = SQL + ComNum.VBLF + "    '" + "" + "',  "; //'처방코드
                                    SQL = SQL + ComNum.VBLF + "    '" + ssS_Sheet1.Cells[i, 2].Text.Trim() + "',  "; //'처방명
                                    SQL = SQL + ComNum.VBLF + "    '" + ssS_Sheet1.Cells[i, 3].Text.Trim() + "',  "; //'용법검체
                                    SQL = SQL + ComNum.VBLF + "    '" + ssS_Sheet1.Cells[i, 13].Text.Trim() + "',  "; //'일투량
                                    SQL = SQL + ComNum.VBLF + "    '" + ssS_Sheet1.Cells[i, 5].Text.Trim() + "',  "; //'횟수

                                    //'=============================================================
                                    //'2010-04-26 김현욱 추가
                                    //'항생제 반응 및 외출외박 내역 투약기록지 REMARK에 자동 뿌려주기
                                    if (chkInOit.Checked == false)
                                    {
                                        if (lblINOUT.Text.Trim() == "현재 외출 중입니다.")
                                        {
                                            strINOUT = "(외출)";
                                        }
                                        else if (lblINOUT.Text.Trim() == "현재 외박 중입니다.")
                                        {
                                            strINOUT = "(외박)";
                                        }
                                        else
                                        {
                                            strINOUT = "";
                                        }
                                    }
                                    //'=============================================================

                                    SQL = SQL + ComNum.VBLF + "    '" + strINOUT + "',  "; //'비고
                                    SQL = SQL + ComNum.VBLF + "    '" + ssS_Sheet1.Cells[i, 12].Text.Trim() + "',  "; //'RowId
                                    SQL = SQL + ComNum.VBLF + "    '" + "" + "',  "; //'ACTING 컬럼
                                    SQL = SQL + ComNum.VBLF + "    '" + "[자가약]" + "'  "; //참고사항
                                    SQL = SQL + ComNum.VBLF + ")  ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        ComFunc.MsgBox(" ACTING 작업중 오류 발생 하였습니다. " + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 ACTGING 하세요 ", "오류");
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    strDataNew.Clear();
                                    strDataNew.Add("I0000016396", dtpDate2.Value.ToString("yyyyMMdd")); //처방일자
                                    //strDataNew.Add("I0000037685", ssM_Sheet1.Cells[i, 2].Text.Trim()); //처방코드
                                    strDataNew.Add("I0000006552", ssS_Sheet1.Cells[i, 2].Text.Trim()); //처방명
                                    strDataNew.Add("I0000037687", ssS_Sheet1.Cells[i, 3].Text.Trim()); //용법검체
                                    strDataNew.Add("I0000037686", ssS_Sheet1.Cells[i, 4].Text.Trim()); //총수량
                                    strDataNew.Add("I0000011700", ssS_Sheet1.Cells[i, 5].Text.Trim()); //횟수
                                    strDataNew.Add("I0000031009", ssS_Sheet1.Cells[i, 6].Text.Trim()); //1회 투여량
                                    strDataNew.Add("I0000001311", strINOUT); //비고
                                    strDataNew.Add("I0000031495", ssS_Sheet1.Cells[i, 12].Text.Trim()); //RowId
                                    //strDataNew.Add("I0000037688", ssM_Sheet1.Cells[i, 16].Text.Trim()); //ACTING 컬럼
                                    strDataNew.Add("I0000010053", "[자가약]"); //참고사항

                                    strEMRNO = clsEmrQuery.SaveNurChartFlow(clsDB.DbCon, this, pAcp, pForm, mstrChartDate, mstrChartTime, strDataNew).ToString();

                                }


                                SQL = "";
                                SQL = " INSERT INTO KOSMOS_EMR.EMR_CADEX_SELFMED_ACT(";
                                SQL = SQL + ComNum.VBLF + " SEQNO, PTNO, BDATE, DEPTCODE,";
                                SQL = SQL + ComNum.VBLF + " DRCODE, ACTSABUN, ACTDATE, EMRNO) VALUES (";
                                SQL = SQL + ComNum.VBLF + Convert.ToString(VB.Val(strSEQNO)) + ",'" + FstrPtno + "', TO_DATE('" + dtpDate1.Text.Trim() + "','YYYY-MM-DD'), '" + FstrDEPT + "', ";
                                SQL = SQL + ComNum.VBLF + "'" + FstrDrCode + "'," + Convert.ToString(Convert.ToInt32(clsType.User.Sabun));

                                if (chkDate.Checked == true)
                                {
                                    SQL = SQL + "  , TO_DATE('" + strActTime + "','YYYY-MM-DD HH24:MI:SS') ,";
                                }
                                else
                                {
                                    SQL = SQL + " , SYSDATE,";
                                }

                                SQL = SQL + ComNum.VBLF + Convert.ToString(VB.Val(strEMRNO)) + ")";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                SCREEN_CLEAR();

                GetData();

                if (strDcOrderMsg.Trim() != "")
                {
                    ComFunc.MsgBox(strDcOrderMsg + " 이미 의사가 오더 DC 되었습니다."
                                    + ComNum.VBLF + ComNum.VBLF + "오더확인하세요.", "확인");
                }

                if (strEMRNOMsg != "")
                {
                    ComFunc.MsgBox(strEMRNOMsg, "에러!");
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private bool SetTagHeadTagTail(string strFormNo, ref string[] strTagHead, ref string[] strTagTail)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT FORMNO, TAGHEAD, TAGTAIL      ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMROPTFORM   ";
                SQL = SQL + ComNum.VBLF + "WHERE FORMNO = " + strFormNo + "   ";
                SQL = SQL + ComNum.VBLF + "ORDER BY  ITEMNO   ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                strTagHead = null;
                strTagTail = null;

                if (dt.Rows.Count > 0)
                {
                    strTagHead = new string[dt.Rows.Count];
                    strTagTail = new string[dt.Rows.Count];

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strTagHead[i] = dt.Rows[i]["TAGHEAD"].ToString().Trim();
                        strTagTail[i] = dt.Rows[i]["TAGTAIL"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                return true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    ComFunc.MsgBox(ex.Message);
                    dt.Dispose();
                    dt = null;
                }
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void SCREEN_CLEAR()
        {
            ssM_Sheet1.RowCount = 0;
            //this.Enabled = false;
            //ssM.Col = 9
            //ssM.Row = 0

            if (optBun2.Checked == true) //|| optBun3.Checked == true)
            {
                ssM_Sheet1.Columns.Get(8).Label = "Pickup시간";
                optGB1.Enabled = false;
                optGB0.Checked = true;
            }
            else
            {
                ssM_Sheet1.Columns.Get(8).Label = "용법 및 검체";
                optGB1.Enabled = true;
                //sspEmrno.Visible = False
                //If FstrNotePC = "Y" Then
                //    ssM.Height = 10000
                //Else
                //    ssM.Height = 11580
                //End If
            }

            //ssLine2_Sheet1.RowCount = 2;
            //ssLineP2_Sheet1.RowCount = 2;
        }

        private bool Verificate_Date(string ArgDate, string ArgTime)
        {
            //'2013-11-04 주임 김현욱
            //'날짜 검증
            //'액팅일자를 임의로 주는 경우 조회된 데이터의 오더일과 비교하여
            //'액팅일자와 오더일자가 불일치 하는 경우 경고 메시지와 함께 액팅 불가능 하도록 함.

            bool RtnVal = true;
            int i = 0;
            string strMaxDate = "";

            //'2013-11-20 ER Keep 때문 추가
            if (mstrWard == "ER")
            {
                for (i = 0; i < ssM_Sheet1.RowCount; i++)
                {
                    if (strMaxDate == "")
                    {
                        strMaxDate = ssM_Sheet1.Cells[i, 51].Text.Trim();
                    }
                    else
                    {
                        if (Convert.ToDateTime(strMaxDate) < Convert.ToDateTime(ssM_Sheet1.Cells[i, 51].Text.Trim()))
                        {
                            strMaxDate = ssM_Sheet1.Cells[i, 51].Text.Trim();
                        }
                    }
                }

                if (Convert.ToDateTime(ArgDate) > Convert.ToDateTime(strMaxDate).AddDays(10))
                {
                    ComFunc.MsgBox("ER은 처방기준 10일까지  액팅제한!! 확인하시기 바랍니다.", "에러");
                    RtnVal = false;
                }
            }
            else
            {
                if (Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A")) < Convert.ToDateTime(ArgDate + " " + ArgTime))
                {
                    ComFunc.MsgBox("임의시간은 현재 시각보다 이전이여야 합니다. 액팅일시를 확인하시기 바랍니다.", "에러");
                    RtnVal = false;
                }
            }

            return RtnVal;
        }

        private void optBun_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
            {
                return;
            }

            if (optBun4.Checked == true)
            {
                mstrACTTable = "KOSMOS_OCS.OCS_IORDER_ACT_ER";

                if (mstrWard == "ER")
                {
                    ssM_Sheet1.Columns.Get(3).Visible = false;
                    ssM_Sheet1.Columns.Get(8).Visible = false;
                    ssM_Sheet1.Columns.Get(11).Visible = false;
                    ssM_Sheet1.Columns.Get(12).Visible = false;
                    ssM_Sheet1.Columns.Get(40).Visible = false;
                    ssM_Sheet1.Columns.Get(41).Visible = false;
                    ssM_Sheet1.Columns.Get(42).Visible = false;
                    ssM_Sheet1.Columns.Get(43).Visible = false;
                    ssM_Sheet1.Columns.Get(44).Visible = false;
                    ssM_Sheet1.Columns.Get(45).Visible = false;
                }
                else
                {
                    cboJob.SelectedIndex = 4;
                }
            }
            else
            {
                mstrACTTable = "KOSMOS_OCS.OCS_IORDER_ACT";

                if (mstrWard == "ER")
                {
                    ssM_Sheet1.Columns.Get(3).Visible = true;
                    ssM_Sheet1.Columns.Get(8).Visible = true;
                    ssM_Sheet1.Columns.Get(11).Visible = true;
                    ssM_Sheet1.Columns.Get(12).Visible = true;
                    ssM_Sheet1.Columns.Get(40).Visible = true;
                    ssM_Sheet1.Columns.Get(41).Visible = true;
                    ssM_Sheet1.Columns.Get(42).Visible = true;
                    ssM_Sheet1.Columns.Get(43).Visible = true;
                    ssM_Sheet1.Columns.Get(44).Visible = true;
                    ssM_Sheet1.Columns.Get(45).Visible = true;
                }
                else
                {
                    cboJob.SelectedIndex = 0;
                }
            }
            GetData();
        }

        private void optGB_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
            {
                return;
            }

            if (optGB0.Checked == true)
            {
                btnSaveActSession.Enabled = true;
                btnSaveActSession.BackColor = SystemColors.HotTrack;

                btnDeleteDCSession.Enabled = false;
                btnDeleteDCSession.BackColor = Color.Gray;
            }
            else
            {
                btnSaveActSession.Enabled = false;
                btnSaveActSession.BackColor = Color.Gray;

                btnDeleteDCSession.Enabled = true;
                btnDeleteDCSession.BackColor = Color.Red;
            }

            GetData();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (bolLoad == true)
            {
                return;
            }
            
            if (cboWard.Text.Trim() == "ER")
            {
                tabControl2.SelectedTab = tabPage5;
            }
            else
            {
                tabControl2.SelectedTab = tabPage4;
            }

            chkERgbn.Checked = false;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            ssList_Sheet1.RowCount = 0;

            int i = 0;

            string strJob = "";
            string strPriDate = "";
            string strToDate = "";
            string strNextDate = "";
            string strBun = "";
            string strROOM_OLD = "";

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SCREEN_CLEAR();

            if (optBun0.Checked == true)
            {
                strBun = "'11','12'";
            }

            if (optBun1.Checked == true)
            {
                strBun = strBun + (strBun != "" ? "," : "") + "'20','23'";
            }

            if (optBun2.Checked == true || optBun4.Checked == true)
            {
                strBun = "'22','23','24','25','26','27','28','29',"
                          + "'31','32','33','34','35','36','37','38','39',"
                          + "'41','42','43','44','45','46','47','48','49',"
                          + "'51','52','53','54','55','56','57','58','59',"
                          + "'61','62','63','64','65','66','67','68','69',"
                          + "'70','71','72','73'";
            }

            if (optBun4.Checked == true)
            {
                mstrACTTable = "KOSMOS_OCS.OCS_IORDER_ACT_ER";
            }
            else if (optBun2.Checked == true)
            {
                mstrACTTable = "KOSMOS_OCS.OCS_IORDER_ACT_ETC";
            }
            else
            {
                mstrACTTable = "KOSMOS_OCS.OCS_IORDER_ACT";
            }

            btnActOK.Visible = false;
            strJob = VB.Left(cboJob.Text, 1);

            strPriDate = dtpDate1.Value.AddDays(-1).ToString("yyyy-MM-dd");
            strToDate = dtpDate1.Value.ToString("yyyy-MM-dd");
            strNextDate = dtpDate1.Value.AddDays(1).ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT M.WARDCODE,M.ROOMCODE,M.PANO,M.SNAME,M.SEX,M.AGE,M.BI,M.PNAME,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.INDATE,'YYYY-MM-DD') INDATE,M.ILSU,M.IPDNO,M.GBSTS,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.OUTDATE,'YYYY-MM-DD') OUTDATE,M.GBDRG, ";
                SQL = SQL + ComNum.VBLF + " M.DEPTCODE,M.DRCODE,D.DRNAME,M.AMSET1,M.AMSET4,M.AMSET6,M.AMSET7, '' REP  ";
                SQL = SQL + ComNum.VBLF + ",KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(M.PANO, TRUNC(SYSDATE)) AS FC_INFECT   ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_PMPA.IPD_NEW_MASTER  M, ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P, ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_DOCTOR  D ";

                switch (cboWard.Text.Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + " WHERE M.WARDCODE>' ' ";
                        break;

                    case "MICU":
                        SQL = SQL + ComNum.VBLF + " WHERE M.ROOMCODE = '234' ";
                        break;

                    case "SICU":
                        SQL = SQL + ComNum.VBLF + " WHERE M.ROOMCODE = '233' ";
                        break;

                    case "ND":
                    case "NR":
                        SQL = SQL + ComNum.VBLF + " WHERE M.WARDCODE IN ( 'ND','IQ' ,'NR') ";
                        break;

                    case "HD":
                        SQL = SQL + ComNum.VBLF + " WHERE M.PANO IN ( SELECT PANO FROM  TONG_HD_DAILY WHERE TRUNC(TDATE) = TO_DATE('" + strToDate + "','YYYY-MM-DD') )";
                        break;

                    case "OP":
                    case "AG":
                        SQL = SQL + ComNum.VBLF + " WHERE ( M.PANO IN (SELECT PANO FROM KOSMOS_PMPA.ORAN_MASTER WHERE OPDATE =TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                        if ((cboWard.Text.Trim()) == "AG")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND GbAngio ='Y')) ";
                        }
                        else if ((cboWard.Text.Trim()) == "OP")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND GbAngio ='N' ";
                            SQL = SQL + ComNum.VBLF + "   )  ";
                            SQL = SQL + ComNum.VBLF + "  OR M.PANO IN (SELECT PTNO FROM KOSMOS_OCS.OCS_ITRANSFER WHERE TODEPTCODE = 'PC' AND TRUNC(BDATE) = TO_DATE('" + strToDate + "','YYYY-MM-DD')) )";

                        }
                        break;

                    case "ENDO":
                        SQL = SQL + ComNum.VBLF + " WHERE  M.PANO IN ( SELECT PTNO FROM KOSMOS_OCS.ENDO_JUPMST WHERE TRUNC(RDATE) =TO_DATE('" + strToDate + "','YYYY-MM-DD') ) ";
                        break;

                    case "ER":
                        SQL = SQL + ComNum.VBLF + " WHERE M.PANO IN ( SELECT PANO FROM KOSMOS_PMPA.OPD_MASTER  WHERE TRUNC(BDATE) =TO_DATE('" + strToDate + "','YYYY-MM-DD') AND DEPTCODE ='ER' ) ";
                        break;

                    case "RA":
                        SQL = SQL + ComNum.VBLF + " WHERE  M.PANO IN ( SELECT PTNO   FROM KOSMOS_OCS.OCS_ITRANSFER  WHERE TODRCODE ='1107' AND GBDEL <>'*'  AND TRUNC(EDATE) =TO_DATE('" + strToDate + "','YYYY-MM-DD' ))  ";
                        break;
                    case "CT/MRI":
                        SQL = SQL + ComNum.VBLF + " WHERE M.PANO IN ( SELECT PANO FROM KOSMOS_PMPA.XRAY_DETAIL ";
                        SQL = SQL + ComNum.VBLF + "                    WHERE SEEKDATE >= TO_DATE('" + strToDate + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "                      AND SEEKDATE <= TO_DATE('" + strToDate + " 23:59', 'YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "                      AND XJONG IN('4','5','6'))";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + " WHERE M.WARDCODE='" + cboWard.Text.Trim() + "' ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "  AND M.PANO=P.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND M.DRCODE=D.DRCODE(+) ";

                if (VB.Left(cboDept.Text.Trim(), 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' ";
                }

                if (VB.Left(cboDrCode.Text.Trim(), 4) != "****")
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.DRCODE = '" + VB.Left(cboDrCode.Text.Trim(), 4) + "' ";
                }

                if (txtSelname.Text.Trim().Length > 1 && txtSelname.Text.Trim() != "조회이름")
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.SNAME LIKE  '%" + txtSelname.Text + "%' ";
                }

                //'작업분류
                if (strJob == "1" || strJob == "L" || strJob == "M") //'재원자
                {
                    SQL = SQL + ComNum.VBLF + " AND ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OUTDATE IS NULL) ";
                    SQL = SQL + ComNum.VBLF + " OR M.OUTDATE>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + " AND M.IPWONTIME < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.PANO < '90000000' ";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS <> '9' ";

                    if (strJob == "L")
                    {
                        SQL = SQL + ComNum.VBLF + " AND EXISTS";
                        SQL = SQL + ComNum.VBLF + "  (SELECT IPDNO";
                        SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL MST";
                        SQL = SQL + ComNum.VBLF + "   WHERE  EXISTS (";
                        SQL = SQL + ComNum.VBLF + "   SELECT ACTDATE, SEQNO FROM (";
                        SQL = SQL + ComNum.VBLF + "   SELECT MAX(ACTDATE) ACTDATE, SEQNO";
                        SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL";
                        SQL = SQL + ComNum.VBLF + "   GROUP BY SEQNO ) SUB";
                        SQL = SQL + ComNum.VBLF + "   WHERE MST.ACTDATE = SUB.ACTDATE";
                        SQL = SQL + ComNum.VBLF + "        AND MST.SEQNO = SUB.SEQNO)";
                        SQL = SQL + ComNum.VBLF + "        AND MST.IPDNO = M.IPDNO";
                        SQL = SQL + ComNum.VBLF + "        AND STATUS IN ('삽입','유지'))";
                    }
                    else if (strJob == "M")
                    {
                        SQL = SQL + ComNum.VBLF + " AND EXISTS ( SELECT PANO FROM KOSMOS_PMPA.IPD_NEW_SLIP SUB ";
                        SQL = SQL + ComNum.VBLF + "  WHERE M.IPDNO = SUB.IPDNO";
                        SQL = SQL + ComNum.VBLF + "       AND SUB.BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "       AND SUB.SUNEXT IN (";
                        SQL = SQL + ComNum.VBLF + "              SELECT SUNEXT";
                        SQL = SQL + ComNum.VBLF + "                FROM KOSMOS_PMPA.BAS_SUN";
                        SQL = SQL + ComNum.VBLF + "              WHERE GBANTICAN = 'Y')) ";
                    }
                }
                else if (strJob == "2") // '당일입원자
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.INDATE >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.IPWONTIME >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.IPWONTIME < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.PANO < '90000000' ";
                    SQL = SQL + ComNum.VBLF + "  AND M.PANO <> '81000004' ";
                    SQL = SQL + ComNum.VBLF + "  AND M.GBSTS <> '9' ";
                }
                else if (strJob == "3") //'퇴원예고
                {
                    SQL = SQL + ComNum.VBLF + " AND EXISTS ( SELECT * FROM KOSMOS_PMPA.NUR_MASTER SUB ";
                    SQL = SQL + ComNum.VBLF + "  WHERE M.PANO = SUB.PANO";
                    SQL = SQL + ComNum.VBLF + "     AND SUB.ROUTDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND M.IPDNO = SUB.IPDNO) ";
                }
                else if (strJob == "4") //'당일퇴원
                {
                    SQL = SQL + ComNum.VBLF + " AND M.OUTDATE=TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS = '7' "; //'퇴원수납완료;
                }
                else if (strJob == "6") //'수술예정자
                {
                    SQL = SQL + ComNum.VBLF + " AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS IN ('0','2')  ";
                }
                else if (strJob == "A") //'응급실경유입원 1-3일전
                {
                    SQL = SQL + ComNum.VBLF + " AND ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OUTDATE IS NULL) ";
                    SQL = SQL + ComNum.VBLF + " OR M.OUTDATE >=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + " AND (M.ILSU >= 1 AND M.ILSU<=3) ";
                    SQL = SQL + ComNum.VBLF + " AND M.AMSET7 IN ('3','4','5') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OUTDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS <> '9' ";
                }
                else if (strJob == "G")
                {
                    SQL = SQL + ComNum.VBLF + " AND ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OUTDATE IS NULL) ";
                    SQL = SQL + ComNum.VBLF + " OR M.OUTDATE >=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + " AND M.AMSET7 IN ('3','4','5') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OUTDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS <> '9' ";
                    SQL = SQL + ComNum.VBLF + " AND INDATE > TO_DATE('2016-02-22','YYYY-MM-DD')";
                }
                else if (strJob == "B") //'재원기간 7-14일 환자
                {
                    SQL = SQL + ComNum.VBLF + " AND ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OUTDATE IS NULL) ";
                    SQL = SQL + ComNum.VBLF + " OR M.OUTDATE>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + " AND (M.ILSU>=7 AND M.ILSU<=14) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS <> '9' ";
                }
                else if (strJob == "C") //'재원기간 3-7일 환자
                {
                    SQL = SQL + ComNum.VBLF + " AND ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OUTDATE IS NULL) ";
                    SQL = SQL + ComNum.VBLF + " OR M.OUTDATE>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + " AND (M.ILSU>=3 AND M.ILSU<=7) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS <> '9' ";
                }
                else if (strJob == "D") //'어제퇴원자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.OUTDATE=TRUNC(SYSDATE-1) ";
                }
                else if (strJob == "E") //'자가약
                {
                    SQL = SQL + ComNum.VBLF + " AND ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OUTDATE IS NULL) ";
                    SQL = SQL + ComNum.VBLF + " OR M.OUTDATE>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + " AND M.IPWONTIME < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.PANO < '90000000' ";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS <> '9' ";
                    SQL = SQL + ComNum.VBLF + "   AND M.PANO IN (  ";
                    SQL = SQL + ComNum.VBLF + "         SELECT  S.PTNO   ";
                    SQL = SQL + ComNum.VBLF + "           FROM   KOSMOS_EMR.EMR_CADEX_SELFMED S , KOSMOS_PMPA.IPD_NEW_MASTER  IPD      ";
                    SQL = SQL + ComNum.VBLF + "          WHERE  IPD.ACTDATE IS NULL  ";
                    SQL = SQL + ComNum.VBLF + "            AND  IPD.PANO=S.PTNO  ";
                    SQL = SQL + ComNum.VBLF + "            AND  S.DELDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "               AND   TO_CHAR(IPD.INDATE,'YYYYMMDD') = S.MEDFRDATE  ";
                    SQL = SQL + ComNum.VBLF + "    GROUP BY S.PTNO  )";
                }
                else if (strJob == "F") //'검사약제
                {
                    SQL = SQL + ComNum.VBLF + " AND ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OUTDATE IS NULL) ";
                    SQL = SQL + ComNum.VBLF + " OR M.OUTDATE>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + " AND M.IPWONTIME < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.PANO < '90000000' ";
                    SQL = SQL + ComNum.VBLF + "   AND M.PANO IN (  ";
                    SQL = SQL + ComNum.VBLF + "   SELECT  PTNO ";
                    SQL = SQL + ComNum.VBLF + "        FROM KOSMOS_OCS.ENDO_JUPMST   ";
                    SQL = SQL + ComNum.VBLF + "      WHERE BDATE =TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";
                    SQL = SQL + ComNum.VBLF + "         AND ORDERCODE IN ('00440913','00440160')  ";  //'대장내시경 코리트산;
                    SQL = SQL + ComNum.VBLF + "         AND GBSUNAP ='1' ";
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "      SELECT PANO";
                    SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_PMPA.XRAY_DETAIL ";
                    SQL = SQL + ComNum.VBLF + "      WHERE BDATE  =TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";
                    SQL = SQL + ComNum.VBLF + "         AND XCODE IN ( 'HA474D') ";
                    SQL = SQL + ComNum.VBLF + "         AND GBEND ='1' ";  //'촬영완료;
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "      SELECT PANO";
                    SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_PMPA.XRAY_DETAIL ";
                    SQL = SQL + ComNum.VBLF + "      WHERE BDATE  =TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";
                    SQL = SQL + ComNum.VBLF + "         AND XCODE IN ( 'HA032') ";
                    SQL = SQL + ComNum.VBLF + "         AND GBEND ='1' ";  //'촬영완료;
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "     SELECT PANO";
                    SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_PMPA.XRAY_DETAIL ";
                    SQL = SQL + ComNum.VBLF + "      WHERE BDATE  =TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";
                    SQL = SQL + ComNum.VBLF + "         AND XCODE IN ( 'HA032','HA031') ";
                    SQL = SQL + ComNum.VBLF + "         AND GBEND ='1' ";  //'촬영완료;
                    SQL = SQL + ComNum.VBLF + "  ) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS IN ('0','2')  ";
                }

                if (cboTeam.Text.Trim() == "지정")
                {
                    SQL = SQL + ComNum.VBLF + " AND M.PANO IN (SELECT PTNO FROM KOSMOS_PMPA.NUR_SABUN_PTNO WHERE SABUN = '" + clsType.User.Sabun + "')       ";
                }
                else if (cboTeam.Text.Trim() != "전체")
                {
                    SQL = SQL + ComNum.VBLF + "  AND EXISTS ";
                    SQL = SQL + ComNum.VBLF + " (SELECT * FROM KOSMOS_PMPA.NUR_TEAM_ROOMCODE T";
                    SQL = SQL + ComNum.VBLF + "          WHERE M.WARDCODE = T.WARDCODE";
                    SQL = SQL + ComNum.VBLF + "             AND M.ROOMCODE = T.ROOMCODE";
                    SQL = SQL + ComNum.VBLF + "             AND T.TEAM = '" + cboTeam.Text.Trim() + "')";
                }

                if (chkTransfor.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND EXISTS (";
                    SQL = SQL + ComNum.VBLF + "  SELECT SUB1.PANO";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_TRANSFOR SUB1";
                    SQL = SQL + ComNum.VBLF + "  WHERE SUB1.PANO = M.PANO";
                    SQL = SQL + ComNum.VBLF + "  AND SUB1.TOWARD = M.WARDCODE";
                    SQL = SQL + ComNum.VBLF + "  AND SUB1.FRWARD <> SUB1.TOWARD";
                    SQL = SQL + ComNum.VBLF + "  AND SUB1.IPDNO = M.IPDNO";
                    SQL = SQL + ComNum.VBLF + "  AND SUB1.TRSDATE >= TO_DATE('" + strToDate + " 00:00','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "  AND SUB1.TRSDATE <= TO_DATE('" + strToDate + " 23:59','YYYY-MM-DD HH24:MI'))";
                }

                // 'SORT
                if (cboWard.Text.Trim() == "32" || cboWard.Text.Trim() == "33" || cboWard.Text.Trim() == "35")
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY M.ROOMCODE, M.BEDNUM, M.SNAME, M.INDATE DESC  ";
                }
                else if (cboWard.Text.Trim() == "HD")
                {
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY M.ROOMCODE,M.SNAME, M.INDATE DESC  ";
                }

                //FstrTable = "OCS_IORDER";

                if (cboWard.Text.Trim() == "ER")
                {
                    //SQL = "  SELECT 'ER' WARDCODE, '100' ROOMCODE, M.PANO,M.SNAME,M.SEX,M.AGE, '0' GBSTS, M.REP, ";
                    SQL = "  SELECT 'ER' WARDCODE, (SELECT NAME ";
                    SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_PMPA.BAS_BCODE ";
                    SQL = SQL + ComNum.VBLF + "      WHERE GUBUN = 'ETC_응급실환자구역' ";
                    SQL = SQL + ComNum.VBLF + "        AND CODE = ER_NUM) ROOMCODE, ";
                    SQL = SQL + ComNum.VBLF + "  M.PANO,M.SNAME,M.SEX,M.AGE, '0' GBSTS, M.REP, ";
                    SQL = SQL + ComNum.VBLF + " M.DEPTCODE,M.DRCODE,D.DRNAME, TO_CHAR(M.ACTDATE, 'YYYY-MM-DD') INDATE, '' OUTDATE, E.ROWID, E.ACT_OK, '' GBDRG";
                    SQL = SQL + ComNum.VBLF + ",KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(M.PANO, TRUNC(SYSDATE)) AS FC_INFECT   ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER  M, ";
                    SQL = SQL + ComNum.VBLF + "       KOSMOS_PMPA.BAS_PATIENT P, ";
                    SQL = SQL + ComNum.VBLF + "       KOSMOS_PMPA.BAS_DOCTOR  D, ";
                    SQL = SQL + ComNum.VBLF + "       KOSMOS_PMPA.NUR_ER_PATIENT E";
                    SQL = SQL + ComNum.VBLF + " WHERE M.DEPTCODE ='ER'  ";
                    SQL = SQL + ComNum.VBLF + "   AND M.PANO=P.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND M.DRCODE=D.DRCODE(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND M.PANO = E.PANO ";
                    SQL = SQL + ComNum.VBLF + "   AND M.BDATE = E.JDATE ";
                    if (txtSelname2.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND M.SNAME LIKE '%" + txtSelname2.Text.Trim() + "%'";
                    }

                    if (chkERDate.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "   AND E.JDATE >= TO_DATE('" + dtpFDate.Value.ToShortDateString() + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND E.JDATE <= TO_DATE('" + dtpTDate.Value.ToShortDateString() + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   AND M.BDATE = TO_DATE('" + strToDate + "', 'YYYY-MM-DD') ";
                    }

                    if (rdoERPart7.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND (E.DGKD IS NOT NULL AND E.DGKD NOT IN ('3', '4')) "; //질병여부
                        SQL = SQL + ComNum.VBLF + "    AND E.IPDATE IS NULL "; //입원일자
                        SQL = SQL + ComNum.VBLF + "    AND ((E.OutTime IS NULL AND E.HOJINDATE1 IS NOT NULL) OR (E.OutTime IS NOT NULL AND E.HOJINDATE1 IS NULL))                     ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY E.InTime DESC, M.SName, E.Pano ";
                    }
                    else
                    {


                        if (rdoERPart2.Checked == true) //관찰
                        {
                            SQL = SQL + ComNum.VBLF + "     AND M.ER_NUM IN (" + clsErNr.GstrArea1 + ") ";
                        }
                        else if (rdoERPart3.Checked == true) //중증
                        {
                            SQL = SQL + ComNum.VBLF + "     AND M.ER_NUM IN (" + clsErNr.GstrArea2 + ") ";
                        }
                        else if (rdoERPart4.Checked == true) //소아
                        {
                            SQL = SQL + ComNum.VBLF + "     AND M.ER_NUM IN (" + clsErNr.GstrArea3 + ") ";
                        }
                        else if (rdoERPart5.Checked == true) //격리
                        {
                            SQL = SQL + ComNum.VBLF + "     AND M.ER_NUM IN (" + clsErNr.GstrArea4 + ") ";
                        }

                        switch (VB.Left(cboJob.Text.Trim(), 1))
                        {
                            case "1":
                                SQL = SQL + ComNum.VBLF + "   AND (E.ACT_OK IS NULL OR E.ACT_OK = '0')";//'액팅대상;
                                break;

                            case "2":
                                SQL = SQL + ComNum.VBLF + "   AND E.ACT_OK = '1'"; //'액팅완료;
                                break;

                            case "3": //확인 필요
                            case "4":
                                SQL = SQL + ComNum.VBLF + "   AND E.OUTGBN IN ('1','9')";       //'당일 입원;
                                break;

                            case "5":
                                SQL = SQL + ComNum.VBLF + "   AND E.OUTGBN NOT IN ('1','9')";   //'당일 퇴원;
                                break;

                            case "6":
                                SQL = SQL + ComNum.VBLF + "   AND M.PANO IN ( SELECT PTNO FROM KOSMOS_OCS.ENDO_JUPMST WHERE TRUNC(RDATE) =TO_DATE('" + strToDate + "','YYYY-MM-DD') )";   //'당일 내시경 환자 리스트
                                break;

                        }

                        SQL = SQL + ComNum.VBLF + "ORDER BY M.SNAME ";
                    }
                    //FstrTable = "OCS_EORDER"
                }

                if (cboWard.Text.Trim() == "HD")
                {
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "  SELECT 'HD' WARDCODE, 200 ROOMCODE, PANO, SNAME, ";
                    SQL = SQL + ComNum.VBLF + " SEX, AGE, BI, '' PNAME, TO_CHAR(BDATE,'YYYY-MM-DD') INDATE, 0 ILSU, 0 IPDNO, ";
                    SQL = SQL + ComNum.VBLF + " '0' GBSTS, TO_CHAR(BDATE,'YYYY-MM-DD') OUTDATE, ";
                    SQL = SQL + ComNum.VBLF + " '' GBDRG, DEPTCODE, A.DRCODE, B.DRNAME, ";
                    SQL = SQL + ComNum.VBLF + " '' AMSET1, '' AMSET4, ''AMSET6, ''AMSET7, '' REP ";
                    SQL = SQL + ComNum.VBLF + ",KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(PANO, TRUNC(SYSDATE)) AS FC_INFECT   ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER A, KOSMOS_PMPA.BAS_DOCTOR B";
                    SQL = SQL + ComNum.VBLF + " WHERE A.DRCODE = B.DRCODE";
                    SQL = SQL + ComNum.VBLF + "      AND DEPTCODE = 'HD'";
                    SQL = SQL + ComNum.VBLF + "      AND A.JIN IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K', 'N','I','J','Q','R','S','A','B')  ";
                    SQL = SQL + ComNum.VBLF + "      AND BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY WARDCODE, ROOMCODE, SNAME, INDATE";

                    //   FstrTable = "OCS_OORDER"
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (strROOM_OLD != dt.Rows[i]["ROOMCODE"].ToString().Trim())
                        {
                            ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                            strROOM_OLD = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        }

                        ssList_Sheet1.Cells[i, 15].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = clsIpdNr.READ_AGE_GESAN(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["AGE"].ToString().Trim()) + "/" + dt.Rows[i]["SEX"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["GBDRG"].ToString().Trim() == "D" ? "[DRG]" : "");
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                        if (cboWard.Text.Trim() == "ER")
                        {
                            ssList_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ACT_OK"].ToString().Trim();
                        }

                        if (cboWard.Text.Trim() != "ER" && cboWard.Text.Trim() != "HD" && optBun4.Checked == true)
                        {
                            ssList_Sheet1.Cells[i, 12].Text = Pat_Nur_Master_ACT_OK(dt.Rows[i]["IPDNO"].ToString().Trim());
                        }

                        if (clsVbfunc.GetPreToi(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim(), dtpDate1.Value.ToString("yyyy-MM-dd")) == "1")
                        {
                            ssList_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(100, 100, 255);
                        }

                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DRCODE"].ToString().Trim();

                        //TODO : 격리 이미지 만들기 보류

                        //SET_Infection_new(dt.Rows[i]["SNAME"].ToString().Trim(), dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["INDATE"].ToString().Trim(), 3, i, ssList, Picture1, imgInfect, "1");
                        //ssList_Sheet1.Cells[i, 2].Value = clsNurse.Resource2files(dt.Rows[i]["FC_INFECT"].ToString().Trim());

                        if (cboWard.Text.Trim() != "ER")
                        {
                            ssList_Sheet1.Cells[i, 14].Text = dt.Rows[i]["IPDNO"].ToString().Trim();

                            SQL = "";

                            if (optBun4.Checked == true)
                            {
                                SQL = "SELECT COUNT(A.PTNO) CNT ";
                                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_IORDER A, ";
                                SQL = SQL + ComNum.VBLF + "KOSMOS_PMPA.IPD_NEW_MASTER B ";
                                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "     AND A.PTNO = B.PANO";
                                SQL = SQL + ComNum.VBLF + "     AND (A.GBPRN IN NULL OR A.GBPRN <> 'P') ";
                                SQL = SQL + ComNum.VBLF + "     AND A.GBPRN <>'S' ";
                                SQL = SQL + ComNum.VBLF + "     AND A.GBSTATUS NOT IN ('D','D-')        ";
                                SQL = SQL + ComNum.VBLF + "     AND A.GBDIV > A.ACTDIV + NVL(A.DCDIV,0)  ";
                                SQL = SQL + ComNum.VBLF + "     AND (A.GBIOE NOT IN ('E','EI')  OR A.GBIOE IS NULL) ";
                                SQL = SQL + ComNum.VBLF + "     AND (B.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') OR B.OUTDATE = TRUNC(SYSDATE))";
                                SQL = SQL + ComNum.VBLF + "     AND A.SUCODE IN (SELECT SUNEXT FROM KOSMOS_PMPA.BAS_SUN WHERE SUGBAA IN ('1','2','3'))";
                                SQL = SQL + ComNum.VBLF + "     AND A.BUN IN ( '28','30','34','35','38','40')";
                                SQL = SQL + ComNum.VBLF + "     AND A.BDATE >= TRUNC(B.INDATE) AND A.BDATE <= TRUNC(B.INDATE + 1)";
                                SQL = SQL + ComNum.VBLF + "     AND EXISTS ( SELECT SUB1.PANO";
                                SQL = SQL + ComNum.VBLF + "                  FROM KOSMOS_PMPA.NUR_MASTER SUB1";
                                SQL = SQL + ComNum.VBLF + "                  WHERE SUB1.PANO = B.PANO";
                                SQL = SQL + ComNum.VBLF + "                  AND SUB1.IPDNO = B.IPDNO";
                                SQL = SQL + ComNum.VBLF + "                  AND SUB1.ACT_OK IS NULL)";
                            }
                            else
                            {
                                SQL = "";
                                SQL = " SELECT COUNT(*) CNT FROM KOSMOS_OCS.OCS_IORDER ";
                                SQL = SQL + ComNum.VBLF + " WHERE BDATE =TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + "' ,'YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "   AND PTNO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND BUN IN ( " + strBun + " ) ";
                                SQL = SQL + ComNum.VBLF + "   AND (GBPRN IN  NULL OR GBPRN <> 'P') ";
                                SQL = SQL + ComNum.VBLF + "   AND GBPRN <>'S' ";  //'JJY 추가(2000/05/22 'S는 선수납(선불);
                                SQL = SQL + ComNum.VBLF + "   AND GBSTATUS NOT IN ('D','D-')        ";
                                SQL = SQL + ComNum.VBLF + "   AND GBDIV > ACTDIV + NVL(DCDIV,0)  ";

                                if (cboWard.Text.Trim() == "ER")
                                {
                                    SQL = SQL + ComNum.VBLF + " AND GBIOE IN ('E','EI') ";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + " AND (GBIOE NOT IN ('E','EI')  OR GBIOE IS NULL) ";
                                }
                            }

                            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                if (VB.Val(dt1.Rows[0]["CNT"].ToString().Trim()) > 0)
                                {
                                    ssList_Sheet1.Cells[i, 6].Text = "◆";
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        switch (dt.Rows[i]["GBSTS"].ToString().Trim())
                        {
                            case "1":
                                ssList_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(255, 255, 100);
                                break;

                            case "2":
                                ssList_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(255, 255, 100);
                                break;

                            case "4":
                                break;

                            case "5":
                                ssList_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(128, 255, 255);
                                break;

                            case "6":
                                ssList_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(255, 255, 100);
                                break;

                            case "7":
                                ssList_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(255, 100, 100);
                                break;

                            default:
                                //strRoutDate 없는 변수라 일딴 주석
                                //if (Convert.ToDateTime(strRoutDate) > Convert.ToDateTime(mstrDate))
                                //{
                                //    ssList_Sheet1.Cells[i, 14].Text = VB.Right(strRoutDate, 5);
                                //}
                                break;
                        }

                        //당일입원
                        if (cboWard.Text.Trim() == "ER")
                        {
                            if (dt.Rows[i]["REP"].ToString().Trim() == "#")
                            {
                                ssList_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(255, 192, 255);
                            }
                        }
                        else
                        {
                            if (dt.Rows[i]["INDATE"].ToString().Trim() == strToDate)
                            {
                                ssList_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(255, 192, 255);
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                txtSelname.Text = "";

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string Pat_Nur_Master_ACT_OK(string ArgIPDNO)
        {
            string RtnVal = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ACT_OK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE IPDNO = " + ArgIPDNO;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[i]["ACT_OK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return RtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return RtnVal;
            }
        }

        private void ssList_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            string strTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");

            //2018-12-06 병동 간호사 요청. Time 초기화 안 함
            txtLine1Date.Text = strDate;
            //txtLine1Time.Text = ComFunc.FormatStrToDate(strTime, "M");
            txtLine2Date.Text = strDate;
            //txtLine2Time.Text = ComFunc.FormatStrToDate(strTime, "M");
            txtLine1PDate.Text = strDate;
            //txtLine1PTime.Text = ComFunc.FormatStrToDate(strTime, "M");
            txtLine2PDate.Text = strDate;
            //txtLine2PTime.Text = ComFunc.FormatStrToDate(strTime, "M");
            dtpDate2.Value = Convert.ToDateTime(strDate);
            dtpOptDate.Value = Convert.ToDateTime(strDate);

            //2018-10-25 응급실 요청. 임의시간설정 Default
            if (cboWard.Text.Trim() == "ER")
            {
                chkDate.Checked = true;
            }

            SearchssList(e.Row, e.Column);
        }

        private void SearchssList(int intRow, int intCol)
        {
            mstrBDate = dtpDate1.Value.ToString("yyyy-MM-dd");

            FstrRoom = ssList_Sheet1.Cells[intRow, 15].Text.Trim();

            //2019-01-30 안정수, 테이블 칼럼크기는 4인데 5인경우 에러발생,, 임시적으로 4자리까지 짜름
            if (VB.IsNumeric(FstrRoom) == false)
            {
                FstrRoom = "100";
            }
            else
            {
                FstrRoom = ssList_Sheet1.Cells[intRow, 15].Text.Trim();
            }

            mstrWard = cboWard.Text.Trim();
            FstrPtno = ssList_Sheet1.Cells[intRow, 1].Text.Trim();
            FstrSname = ssList_Sheet1.Cells[intRow, 2].Text.Trim();
            FstrInDate = ssList_Sheet1.Cells[intRow, 7].Text.Trim();
            FstrDEPT = ssList_Sheet1.Cells[intRow, 9].Text.Trim();
            FstrDrCode = ssList_Sheet1.Cells[intRow, 10].Text.Trim();
            FstrROWID = ssList_Sheet1.Cells[intRow, 11].Text.Trim();
            mintIPDNO = (ssList_Sheet1.Cells[intRow, 14].Text.Trim() == "" ? 0 : Convert.ToInt32(ssList_Sheet1.Cells[intRow, 14].Text.Trim()));
            mbolACT_OK = (ssList_Sheet1.Cells[intRow, 12].Text.Trim() == "1" ? false : true);

            pAcp = clsEmrChart.ClearPatient();
            pAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, FstrPtno, FstrDEPT.Equals("ER") || mintIPDNO == 0 ? "O" : "I", FstrInDate.Replace("-", ""), FstrDEPT);

            if (pAcp == null)
            {
                ComFunc.MsgBoxEx(this, "접수 내역을 찾을수 없습니다.");
                return;
            }

            SCREEN_CLEAR();

            GetData();
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VB.Left(cboDept.Text.Trim(), 2) == "**")
            {
                cboDrCode.Items.Clear();
                cboDrCode.Items.Add("****.전체");
            }
            else
            {
                clsVbfunc.SetDrCodeCombo(clsDB.DbCon, cboDrCode, VB.Left(cboDept.Text.Trim(), 2), "", 1, "");
            }

            cboDrCode.SelectedIndex = 0;
        }

        private void txtSelname_Enter(object sender, EventArgs e)
        {
            txtSelname.Text = "";
        }

        private void lblMSG_Click(object sender, EventArgs e)
        {
            ComFunc.MsgBox(clsBagage.readAllergyInfo(clsDB.DbCon, FstrPtno), "알러지 등록 정보");
        }

        private void btnActOK_Click(object sender, EventArgs e)
        {
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'응급실 액팅 완료 여부,병동 응급가산수가 완료여부

                if (cboWard.Text.Trim() == "ER")
                {
                    if (FstrROWID == "")
                    {
                        return;
                    }

                    if (btnActOK.Text.Trim() == "액팅완료")
                    {
                        SQL = "";
                        SQL = " UPDATE KOSMOS_PMPA.NUR_ER_PATIENT SET";
                        SQL = SQL + ComNum.VBLF + " ACT_OK = '1'";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        btnActOK.Text = "완료취소";
                    }
                    else if (btnActOK.Text.Trim() == "완료취소")
                    {
                        SQL = "";
                        SQL = " UPDATE KOSMOS_PMPA.NUR_ER_PATIENT SET";
                        SQL = SQL + ComNum.VBLF + " ACT_OK = NULL";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        btnActOK.Text = "액팅완료";
                    }
                }
                else
                {
                    if (mintIPDNO == 0)
                    {
                        return;
                    }

                    if (btnActOK.Text.Trim() == "액팅완료")
                    {
                        SQL = "";
                        SQL = " UPDATE KOSMOS_PMPA.NUR_MASTER SET";
                        SQL = SQL + ComNum.VBLF + " ACT_OK = '1'";
                        SQL = SQL + ComNum.VBLF + " WHERE IPDNO  = " + mintIPDNO;

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        btnActOK.Text = "완료취소";
                    }
                    else if (btnActOK.Text.Trim() == "완료취소")
                    {
                        SQL = "";
                        SQL = " UPDATE KOSMOS_PMPA.NUR_MASTER SET";
                        SQL = SQL + ComNum.VBLF + " ACT_OK = NULL";
                        SQL = SQL + ComNum.VBLF + " WHERE IPDNO  = " + mintIPDNO;

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        btnActOK.Text = "액팅완료";
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                btnView_Click(null, null);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExamDrug_Click(object sender, EventArgs e)
        {
            if (FstrDEPT == "")
            {
                return;
            }

            if (System.IO.File.Exists(@"C:\CMC\ocsexe\mhemrward.exe") == true)
            {
                VB.Shell(@"C:\CMC\ocsexe\mhemrward.exe " + Convert.ToInt32(clsType.User.Sabun));
            }
            else
            {
                ComFunc.MsgBox("실행파일 없습니다..전산정보팀 연락!!");
            }

            //TODO : EMR로 대체
            mstrBDate = dtpDate1.Value.ToString("yyyy-MM-dd");
            //mstrNewChartCd = "1796"

            //gptEmrPtTmp.PtMedDeptCd = FstrDEPT
            //gptEmrPtTmp.PtPtNo = FstrPtno
            //gptEmrPtTmp.PtName = FstrSname
            //gptEmrPtTmp.PtMedDrCd = FstrDrCode

            //gptEmrUs.UsUseId = GnJobSabun
            //gptEmrUs.UsPassWord = FstrPassWord

            //SQL = " SELECT SABUN FROM KOSMOS_OCS.OCS_DOCTOR "
            //SQL = SQL & " WHERE DRCODE ='" & FstrDrCode & "' "
            //SQL = SQL & "   AND GBOUT ='N'"
            //result = AdoOpenSet(rs2, SQL)

            //gptEmrUs.UsUseId = Format(Trim(rs2!Sabun & ""), "#####")

            //AdoCloseSet rs2

            //gptEmrPtTmp.PtAcpNo = ""
            //gptEmrPtTmp.PtInOutCls = "I"
            //gptEmrPtTmp.PtMedFrDate = Replace(FstrInDate, "-", "")
            //gptEmrPtTmp.PtMedFrTime = "120000"     'Replace(PAT.JTime, ":", "") & "00"
            //gptEmrPtTmp.PtMedEndDate = Format(Now, "YYYYMMDD")        'PAT.RDate
            //gptEmrPtTmp.PtMedEndTime = "120000"        'Replace(PAT.JTime, ":", "") & "00"

            //If UCase(App.EXEName) <> "NRER" Then
            //   frmTextEmrWardMain_ACT.Show 1
            //End If
        }

        private void btnJellco_Click(object sender, EventArgs e)
        {
            string strGUBUN1 = "";
            string strGUBUN2 = "";
            string strChartDate = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (optArm.Checked == false && optLeg.Checked == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("'Arm' 또는 'Leg' 을 선택하십시요.");
                    return;
                }

                strChartDate = dtpOptDate.Value.ToString("yyyy-MM-dd") + " " + cboOptTime.Text.Trim() + ":" + cboOptSec.Text.Trim();

                if (ChkOptionTime.Checked == true && strChartDate == "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (FstrPtno == "" || FstrInDate == "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (optArm.Checked == true)
                {
                    strGUBUN1 = "0";
                }
                else if (optLeg.Checked == true)
                {
                    strGUBUN1 = "1";
                }

                if (optRight.Checked == true)
                {
                    strGUBUN2 = "0";
                }
                else if (optLeft.Checked == true)
                {
                    strGUBUN2 = "1";
                }
                else if (optBoth.Checked == true)
                {
                    strGUBUN2 = "2";
                }

                SQL = "";
                SQL = " INSERT INTO KOSMOS_PMPA.NUR_JELLCO(";
                SQL = SQL + ComNum.VBLF + " PANO, INDATE, CHARTDATE, GUBUN1, ";
                SQL = SQL + ComNum.VBLF + " GUBUN2, WRITEDATE, WRITESABUN, KEEP ";
                SQL = SQL + ComNum.VBLF + " ) VALUES ( ";
                SQL = SQL + ComNum.VBLF + "'" + FstrPtno + "', TO_DATE('" + FstrInDate + "','YYYY-MM-DD'), ";

                if (ChkOptionTime.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strChartDate + "','YYYY-MM-DD HH24:MI'), ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " SYSDATE, ";
                }

                SQL = SQL + ComNum.VBLF + "'" + strGUBUN1 + "','" + strGUBUN2 + "', SYSDATE, '" + clsType.User.Sabun + "','0') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                READ_JELLCO(FstrPtno, FstrInDate, mstrDate);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnKeep_Click(object sender, EventArgs e)
        {
            string strGUBUN1 = "";
            string strGUBUN2 = "";
            string strChartDate = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (optArm.Checked == false && optLeg.Checked == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("'Arm' 또는 'Leg' 을 선택하십시요.");
                    return;
                }

                strChartDate = dtpOptDate.Value.ToString("yyyy-MM-dd") + " " + cboOptTime.Text.Trim() + ":" + cboOptSec.Text.Trim();

                if (ChkOptionTime.Checked == true && strChartDate == "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (FstrPtno == "" || FstrInDate == "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (optArm.Checked == true)
                {
                    strGUBUN1 = "0";
                }
                else if (optLeg.Checked == true)
                {
                    strGUBUN1 = "1";
                }

                if (optRight.Checked == true)
                {
                    strGUBUN2 = "0";
                }
                else if (optLeft.Checked == true)
                {
                    strGUBUN2 = "1";
                }
                else if (optBoth.Checked == true)
                {
                    strGUBUN2 = "2";
                }

                SQL = "";
                SQL = " INSERT INTO KOSMOS_PMPA.NUR_JELLCO(";
                SQL = SQL + ComNum.VBLF + " PANO, INDATE, CHARTDATE, GUBUN1, ";
                SQL = SQL + ComNum.VBLF + " GUBUN2, WRITEDATE, WRITESABUN, KEEP ";
                SQL = SQL + ComNum.VBLF + " ) VALUES ( ";
                SQL = SQL + ComNum.VBLF + "'" + FstrPtno + "', TO_DATE('" + FstrInDate + "','YYYY-MM-DD'), ";
                if (ChkOptionTime.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strChartDate + "','YYYY-MM-DD HH24:MI'), ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " SYSDATE, ";
                }
                SQL = SQL + ComNum.VBLF + "'" + strGUBUN1 + "','" + strGUBUN2 + "', SYSDATE, '" + clsType.User.Sabun + "','1') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                READ_JELLCO(FstrPtno, FstrInDate, mstrDate);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strGUBUN1 = "";
            string strGUBUN2 = "";

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FstrPtno == "" || FstrInDate == "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                strGUBUN1 = "X";
                strGUBUN2 = "X";

                SQL = "";
                SQL = " SELECT GUBUN1 ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_JELLCO ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPtno + "' ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + FstrInDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + FstrInDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY CHARTDATE DESC ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["GUBUN1"].ToString().Trim() == "X")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("이미 Remove 하였습니다.", "확인");
                        return;
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " INSERT INTO KOSMOS_PMPA.NUR_JELLCO(";
                SQL = SQL + ComNum.VBLF + " PANO, INDATE, CHARTDATE, GUBUN1, ";
                SQL = SQL + ComNum.VBLF + " GUBUN2, WRITEDATE, WRITESABUN ";
                SQL = SQL + ComNum.VBLF + " ) VALUES ( ";
                SQL = SQL + ComNum.VBLF + "'" + FstrPtno + "', TO_DATE('" + FstrInDate + "','YYYY-MM-DD'), SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "'" + strGUBUN1 + "','" + strGUBUN2 + "', SYSDATE, '" + clsType.User.Sabun + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                READ_JELLCO(FstrPtno, FstrInDate, mstrDate);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void ChkOptionTime_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkOptionTime.Checked == true)
            {
                dtpOptDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                dtpOptDate.Visible = true;
                cboOptTime.Visible = true;
                cboOptSec.Visible = true;
            }
            else
            {
                dtpOptDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                dtpOptDate.Visible = false;
                cboOptTime.Visible = false;
                cboOptSec.Visible = false;
            }
        }

        private void btnJellcoUpdate_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            //string strOK = "";
            string strROWID = "";
            string strChartDate = "";

            strROWID = txtJellcoRowid.Text.Trim();

            strChartDate = dtpOptDate.Value.ToString("yyyy-MM-dd") + " " + cboOptTime.Text.Trim() + ":" + cboOptSec.Text.Trim();

            //strOK = "OK";

            if (strROWID == "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            if (strChartDate == "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            if (ChkOptionTime.Checked == false)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " INSERT INTO KOSMOS_PMPA.NUR_JELLCO_HISTORY ( ";
                SQL = SQL + ComNum.VBLF + "  PANO, INDATE, CHARTDATE, GUBUN1, GUBUN2, WRITEDATE, WRITESABUN, DELDATE, DELSABUN, KEEP) ";
                SQL = SQL + ComNum.VBLF + "  SELECT PANO, INDATE, CHARTDATE, GUBUN1, GUBUN2, WRITEDATE, WRITESABUN, SYSDATE, '" + clsType.User.Sabun + "', KEEP ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_JELLCO ";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " UPDATE KOSMOS_PMPA.NUR_JELLCO SET ";
                SQL = SQL + ComNum.VBLF + " CHARTDATE = TO_DATE('" + strChartDate + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                READ_JELLCO(FstrPtno, FstrInDate, mstrDate);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnJellcoDel_Click(object sender, EventArgs e)
        {
            //string strOK = "";
            string strROWID = "";

            strROWID = txtJellcoRowid.Text.Trim();

            if (strROWID == "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            if (ChkOptionTime.Checked == false)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " INSERT INTO KOSMOS_PMPA.NUR_JELLCO_HISTORY ( ";
                SQL = SQL + ComNum.VBLF + "  PANO, INDATE, CHARTDATE, GUBUN1, GUBUN2, WRITEDATE, WRITESABUN, DELDATE, DELSABUN, KEEP) ";
                SQL = SQL + ComNum.VBLF + "  SELECT PANO, INDATE, CHARTDATE, GUBUN1, GUBUN2, WRITEDATE, WRITESABUN, SYSDATE, '" + clsType.User.Sabun + "', KEEP ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_JELLCO ";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " DELETE KOSMOS_PMPA.NUR_JELLCO ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                READ_JELLCO(FstrPtno, FstrInDate, mstrDate);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssJellco_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            if (ssJellco_Sheet1.RowCount == 0)
            {
                return;
            }

            txtJellcoRowid.Text = ssJellco_Sheet1.Cells[2, e.Column].Text.Trim();

            if (txtJellcoRowid.Text.Trim() == "")
            {
                return;
            }

            btnJellcoDel.Visible = true;
            btnJellcoUpdate.Visible = true;
            ChkOptionTime.Checked = true;

            dtpOptDate.Value = Convert.ToDateTime(ssJellco_Sheet1.Columns.Get(e.Column).Label.Trim());

            cboOptTime.Text = VB.Left(ssJellco_Sheet1.Cells[0, e.Column].Text.Trim(), 2);

            cboOptSec.Text = VB.Right(ssJellco_Sheet1.Cells[1, e.Column].Text.Trim(), 2);

            cboOptTime.ForeColor = Color.FromArgb(0, 0, 250);
            cboOptSec.ForeColor = Color.FromArgb(0, 0, 250);
        }

        private void ssS_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strROWID = "";
            string strEMRNO = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0;

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                return; //권한 확인

            if (optGB1.Checked == false)
            {
                return;
            }

            if (e.Column < 9 && e.Column > 12)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("Acting 취소를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            strROWID = ssS_Sheet1.Cells[e.Row, e.Column + 7].Text.Trim();

            if (strROWID == "")
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "SELECT EMRNO FROM KOSMOS_EMR.EMR_CADEX_SELFMED_ACT ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strEMRNO = dt.Rows[0]["EMRNO"].ToString().Trim();
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " INSERT INTO KOSMOS_EMR.EMR_CADEX_SELFMED_ACT_HIS(";
                SQL = SQL + ComNum.VBLF + " SEQNO, PTNO, BDATE, DEPTCODE, DRCODE, WARDCODE, ROOMCODE, ACTSABUN, ACTDATE, EMRNO, DELSABUN, DELDATE)  ";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + " SEQNO, PTNO, BDATE, DEPTCODE, DRCODE, WARDCODE, ROOMCODE, ACTSABUN, ACTDATE, EMRNO, " + clsType.User.Sabun + ", SYSDATE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_CADEX_SELFMED_ACT ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " DELETE KOSMOS_EMR.EMR_CADEX_SELFMED_ACT ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (DeleteEmrXmlData(strEMRNO) == false)
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                READ_SELFMED(FstrPtno, FstrInDate);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssM_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            string strORDERCODE = "";
            string strAST = "";
            string strROWID = "";

            string strGroup = "";
            int nSRow = 0;
            int nERow = 0;

            int i = 0;

            string SQL = "";
            string SqlErr = "";
            //int intRowAffected = 0;

            //string strOrdercode = "";
            string strInsulin = "";
            DataTable dt = null;

            if (FbBtnClick == true)
            {
                return;
            }

            if (e.Column < 14 || e.Column > 37)
            {
                return;
            }

            if (optGB1.Checked == true)
            {
                if (ssM_Sheet1.Cells[e.Row, 40].Text.Trim() == "◎" && READ_PROGRESS_POWDER_TARGET(FstrPtno, ssM_Sheet1.Cells[e.Row, 48].Text.Trim()) == true)
                {
                    ComFunc.MsgBox("Powder로 조제완료 반환불가.", "확인");
                    ssM_Sheet1.Cells[e.Row, e.Column].Value = false;
                }
            }

            if (optBun1.Checked == false)
            {
                DC_Check(e.Row, Convert.ToBoolean(ssM_Sheet1.Cells[e.Row, e.Column].Value));
                return;
            }

            //if (ssM_Sheet1.Cells[e.Row, 0].Text == "DC" && Convert.ToBoolean(ssM_Sheet1.Cells[e.Row, e.Column].Value) == true)
            //{
            //    ssM_Sheet1.Cells[e.Row, e.Column].Value = false;
            //    ComFunc.MsgBox("DC처방은 수정하실 수 없습니다.");
            //    return;
            //}

            strORDERCODE = ssM_Sheet1.Cells[e.Row, 2].Text.Trim();
            strAST = ssM_Sheet1.Cells[e.Row, 3].Text.Trim();
            strROWID = ssM_Sheet1.Cells[e.Row, 47].Text.Trim();

            //2019-08-27 인슐린일 경우 remark에 시행일자 표시
            if (e.Column >= 14 && e.Column <= 17)
            {
                if (VB.Left(strORDERCODE, 1) == "H" && ssM_Sheet1.Cells[e.Row, e.Column].Text.Trim() == "True")
                {
                    strInsulin = "";

                    SQL = " SELECT A.JEPCODE ";
                    SQL += ComNum.VBLF + "   FROM KOSMOS_ADM.DRUG_MASTER1 A, KOSMOS_ADM.DRUG_MASTER4 B ";
                    SQL += ComNum.VBLF + "  WHERE (A.SUGA_UNIT3 = 'PEN' OR A.SUGA_UNIT3 = 'A')";
                    SQL += ComNum.VBLF + "    AND B.MAXQTY_INSULIN = '1' ";
                    SQL += ComNum.VBLF + "    AND A.JEPCODE = B.JEPCODE ";
                    SQL += ComNum.VBLF + "    AND A.JEPCODE = '" + strORDERCODE + "' ";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strInsulin = "OK";
                    }
                    dt.Dispose();
                    dt = null;

                    if (strInsulin == "OK")
                    {
                        string strRemark = VB.InputBox("인슐린 펜의 시행일자를 입력해주세요" + ComNum.VBLF + "(ex) 2019년 12월 25일)", "시행일자 입력");
                        ssM_Sheet1.Cells[e.Row, 49].Text = ssM_Sheet1.Cells[e.Row, 49].Text + " ★인슐린 투약 준비용: " + strRemark + " 시행";
                        Update_NurRemark("OK");
                    }
                }
            }

            if (strAST == ""
                && READ_AST_TARGET(strORDERCODE, clsIpdNr.READ_AGE_GESAN(clsDB.DbCon, FstrPtno)) == true
                && Convert.ToBoolean(ssM_Sheet1.Cells[e.Row, e.Column].Value) == true
                && READ_IRRIGATION(strROWID) == false)
            {
                ComFunc.MsgBox("AST 결과가 등록되어 있지 않습니다. AST 결과를 등록하십시요.", "확인");

                frmAllergyAndAnti frmAllergyAndAntiX = new frmAllergyAndAnti(FstrPtno, FstrInDate);
                frmAllergyAndAntiX.ShowDialog();
                frmAllergyAndAntiX = null;

                FbBtnClick = true;
                SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                FbBtnClick = false;
            }

            if (READ_AST_TARGET(strORDERCODE, clsIpdNr.READ_AGE_GESAN(clsDB.DbCon, FstrPtno)) && strAST == "" && READ_IRRIGATION(strROWID) == false)
            {
                FbBtnClick = true;
                ssM_Sheet1.Cells[e.Row, e.Column].Value = false;
                FbBtnClick = false;
            }

            //if (optGB1.Checked == true)
            //{
            //    if (ssM_Sheet1.Cells[e.Row, 40].Text.Trim() == "◎")
            //    {
            //        ComFunc.MsgBox("Powder로 조제된 약제는 약국에 문의후에 DC해주세요.", "확인");
            //    }
            //}

            strGroup = ssM_Sheet1.Cells[e.Row, 41].Text.Trim();

            if (strGroup != "")
            {
                //'같은 group은 같이 체크
                if (e.Row > FnBRow)
                {
                    nSRow = FnBRow;
                    nERow = ssM_Sheet1.RowCount - 1;
                }
                else
                {
                    nSRow = 0;
                    nERow = FnBRow - 1;
                }

                for (i = nSRow; i <= nERow; i++)
                {
                    if (ssM_Sheet1.Cells[i, 41].Text.Trim() == strGroup && i != e.Row)
                    {
                        if (Convert.ToBoolean(ssM_Sheet1.Cells[e.Row, e.Column].Value) == false)
                        {
                            return;
                        }

                        if (ssM_Sheet1.Cells[i, e.Column].CellType != null && ssM_Sheet1.Cells[i, e.Column].CellType.ToString() == "CheckBoxCellType")
                        {
                            ssM_Sheet1.Cells[i, e.Column].Value = ssM_Sheet1.Cells[e.Row, e.Column].Value;

                            //2018-11-14 그룹일 시에도 AST결과 등록창 호출되도록 수정

                            strORDERCODE = ssM_Sheet1.Cells[i, 2].Text.Trim();
                            strAST = ssM_Sheet1.Cells[i, 3].Text.Trim();
                            strROWID = ssM_Sheet1.Cells[i, 47].Text.Trim();

                            if (strAST == ""
                                && READ_AST_TARGET(strORDERCODE, clsIpdNr.READ_AGE_GESAN(clsDB.DbCon, FstrPtno)) == true
                                && Convert.ToBoolean(ssM_Sheet1.Cells[i, e.Column].Value) == true
                                && READ_IRRIGATION(strROWID) == false)
                            {
                                ComFunc.MsgBox("AST 결과가 등록되어 있지 않습니다. AST 결과를 등록하십시요.", "확인");

                                frmAllergyAndAnti frmAllergyAndAntiX = new frmAllergyAndAnti(FstrPtno, FstrInDate);
                                frmAllergyAndAntiX.ShowDialog();
                                frmAllergyAndAntiX = null;

                                FbBtnClick = true;
                                SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                                FbBtnClick = false;
                            }

                            if (READ_AST_TARGET(strORDERCODE, clsIpdNr.READ_AGE_GESAN(clsDB.DbCon, FstrPtno)) && strAST == "" && READ_IRRIGATION(strROWID) == false)
                            {
                                FbBtnClick = true;
                                ssM_Sheet1.Cells[i, e.Column].Value = false;
                                FbBtnClick = false;
                            }

                            DC_Check(e.Row, Convert.ToBoolean(ssM_Sheet1.Cells[e.Row, e.Column].Value));
                        }
                    }
                }
            }
            else
            {
                DC_Check(e.Row, Convert.ToBoolean(ssM_Sheet1.Cells[e.Row, e.Column].Value));
            }
        }

        private void DC_Check(int intRow, bool bolChk)
        {
            return;//적용 방식 변경

            //if (optGB1.Checked == true)
            //{
            //    for (int h = 14; h < 38; h++)
            //    {
            //        if (ssM_Sheet1.Cells[intRow, h].CellType != null
            //           && ssM_Sheet1.Cells[intRow, h].CellType.ToString() == "CheckBoxCellType")
            //        {
            //            ssM_Sheet1.Cells[intRow, h].Value = bolChk;
            //        }
            //    }
            //}
        }

        private bool READ_IRRIGATION(string arg)
        {
            bool RtnVal = false;
            string strPtNo = "";
            string strBDATE = "";
            string strSucode = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (mstrEXEName == "NRINFO")
            {
                return RtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT PTNO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, SUCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + arg + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strPtNo = dt.Rows[0]["PTNO"].ToString().Trim();
                    strBDATE = dt.Rows[0]["BDATE"].ToString().Trim();
                    strSucode = dt.Rows[0]["SUCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT IRRIGATION ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_AST ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ORDERCODE = '" + strSucode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND IRRIGATION = 'Y' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = true;
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                return RtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return RtnVal;
            }
        }

        private bool READ_AST_TARGET(string arg, string ArgAge)
        {
            bool RtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Right(ArgAge, 1) == "m")
            {
                if (VB.Val(ArgAge.Replace("m", "")) <= 6)
                {
                    return RtnVal;
                }
            }

            if (VB.Right(ArgAge, 1) == "d")
            {
                return RtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT CODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE CODE = '" + arg + "' ";
                SQL = SQL + ComNum.VBLF + "    AND GUBUN = 'NUR_AST대상항생제' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["CODE"].ToString().Trim() == "W-RAMI")
                    {
                        if (VB.Val(ArgAge.Replace("m", "")) < 18)
                        {
                            RtnVal = true;
                        }
                        else
                        {
                            RtnVal = false;
                        }
                    }
                    else
                    {
                        RtnVal = true;
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return RtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return RtnVal;
            }
        }


        private bool READ_PROGRESS_POWDER_TARGET(string argPTNO, string argorderno)
        {
            bool RtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

           
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT * ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_PROGRESS_POWDER ";
                //SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE orderno = '" + argorderno + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = true;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return RtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return RtnVal;
            }
        }


        private void ssM_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strBdate = "";
            bool bolChck = false;
            int intChckChck = -1;

            if (e.ColumnHeader == true)
            {
                if (e.Column < 14 || e.Column > 37)
                {
                    return;
                }

                for (int i = 0; i < ssM_Sheet1.RowCount; i++)
                {
                    if (i == 0)
                    {
                        strBdate = ssM_Sheet1.Cells[i, 51].Text.Trim();
                    }
                    else
                    {
                        if (strBdate != ssM_Sheet1.Cells[i, 51].Text.Trim() && bolChck == true)
                        {
                            return;
                        }
                    }

                    if (ssM_Sheet1.Cells[i, e.Column].CellType != null
                            && ssM_Sheet1.Cells[i, e.Column].CellType.ToString() == "CheckBoxCellType"
                            && ssM_Sheet1.Rows.Get(i).Locked == false)
                    {
                        if (ssM_Sheet1.Cells[i, 0].Text.Trim() != "DC")
                        {
                            if (intChckChck == -1)
                            {
                                if (Convert.ToBoolean(ssM_Sheet1.Cells[i, e.Column].Value) == true)
                                {
                                    intChckChck = 1;
                                }
                                else
                                {
                                    intChckChck = 0;
                                }
                            }

                            ssM_Sheet1.Cells[i, e.Column].Value = (intChckChck == 1 ? false : true);
                            strBdate = ssM_Sheet1.Cells[i, 51].Text.Trim();
                            bolChck = true;
                        }
                    }
                }

                intChckChck = -1;
                return;
            }

            if (ssM_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.Column == 4)
            {
                if (ssM_Sheet1.Cells[e.Row, 2].Text.Trim() != "")
                {
                    if (clsBagage.USE_DIF() == true)
                    {
                        //frmSupDrstDifDown frmSupDrstDifDownX = new frmSupDrstDifDown("NRINFO", ssM_Sheet1.Cells[e.Row, 2].Text.Trim());
                        //frmSupDrstDifDownX.StartPosition = FormStartPosition.CenterParent;
                        //frmSupDrstDifDownX.ShowDialog();
                        //frmSupDrstDifDownX = null;

                        //2019-07-12 전산업무 의뢰서 2019-822
                        frmSupDrstInfoEntryNew f = new frmSupDrstInfoEntryNew(ssM_Sheet1.Cells[e.Row, 2].Text.Trim(), "NRINFO", "VIEW");
                        f.ShowDialog(this);
                        f = null;
                    }
                }
            }

            if (e.Column == 3)
            {
                if (READ_AST_TARGET(ssM_Sheet1.Cells[e.Row, 2].Text.Trim(), clsIpdNr.READ_AGE_GESAN(clsDB.DbCon, FstrPtno)) == true)
                {
                    frmAllergyAndAnti frmAllergyAndAntiX = new frmAllergyAndAnti(FstrPtno, FstrInDate);
                    frmAllergyAndAntiX.ShowDialog();
                    frmAllergyAndAntiX = null;
                    SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                }
            }

            if (e.Column < 14 || e.Column > 37)
            {
                return;
            }

            if ((ssM_Sheet1.Cells[e.Row, e.Column].BackColor == Color.FromArgb(255, 200, 100)
                || ssM_Sheet1.Cells[e.Row, e.Column].BackColor == Color.FromArgb(255, 0, 0))
                && ssM_Sheet1.Cells[e.Row, e.Column].CellType.ToString() == "TextCellType")
            {
                if (ComFunc.MsgBoxQ("Acting 취소를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
                Acting_Cancel(e.Row, e.Column);
            }
            else if (ssM_Sheet1.Cells[e.Row, e.Column].BackColor == Color.FromArgb(255, 50, 50)
            && ssM_Sheet1.Cells[e.Row, e.Column].CellType.ToString() == "TextCellType")
            {
                //if (Convert.ToDateTime(ssM_Sheet1.Cells[e.Row, e.Column].Tag)
                //    < Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-1))
                //{
                //    ComFunc.MsgBox("간호부요청으로 전날 취소한 NDC 까지 취소 가능 합니다.");
                //    return;
                //}

                if (optGB1.Checked == false)
                {
                    ComFunc.MsgBox("NDC 취소는 NDC 선택 후 이용해 주세요!!");
                    return;
                }

                //if (ssM_Sheet1.Cells[e.Row, 0].Text.Trim() == "DC")
                //{
                //    ComFunc.MsgBox("의사 DC된 처방의 NDC는 취소가 불가 합니다. 고도화 개발팀 8719 로 연락 주세요.");
                //    return;
                //}

                if (ComFunc.MsgBoxQ("NDC 취소를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }

                NDC_Cancel(e.Row, e.Column);
            }
        }

        private void NDC_Cancel(int intRow, int intCol)
        {
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            bool bolMAYAK = false;
            string strBdate = "";
            string strSuCode = "";
            string strORDERCODE = "";

            DataTable dt = null;
            string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            int nOrderNo = Convert.ToInt32(ssM_Sheet1.Cells[intRow, 48].Text.Trim());
            string strROWID = ssM_Sheet1.Cells[intRow, 47].Text.Trim();
            strORDERCODE = ssM_Sheet1.Cells[intRow, 53].Text.Trim();

            //if ()

            if (ChkIn(FstrPtno, mintIPDNO) == false)
            {
                ComFunc.MsgBox("현재 환자는 재원상태가 아닙니다. 확인 후 디시  해주십시오.");
                return;
            }

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //SQL = "";
                //SQL = " SELECT A.*, TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE1, SLIPNO, A.ROWID , A.SUCODE, ";
                //SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE1  ";
                //SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_IORDER  A";
                //SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO     = '" + FstrPtno + "' ";
                //SQL = SQL + ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + mstrBDate + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "  AND A.GBSEND ='*' ";

                ////    '2014-02-27
                //if (cboWard.Text.Trim() == "ER")
                //{
                //    SQL = SQL + ComNum.VBLF + "  AND GBIOE IN ('EI') ";
                //}

                SQL = "";
                SQL += " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE1                                                                            \r";
                SQL += "   FROM KOSMOS_OCS.OCS_IORDER                                                                                           \r";
                SQL += "  WHERE Ptno       = '" + FstrPtno + "'                                                                                 \r";
                SQL += "    AND BDATE      = TO_DATE('" + mstrBDate + "', 'YYYY-MM-DD')                                                         \r";
                SQL += "    AND DEPTCODE  != 'ER'                                                                                               \r";
                //2020-12-14 미전송 체크 조건 주석처리 되어 있어 해제함  -김민철  (소스  History 참조)
                SQL += "    AND (ACCSEND IS NULL or ACCSEND = 'Z')                                                                              \r";
                SQL += "    AND ORDERSITE not in ('NDC', 'ERO')                                                                                 \r";
                SQL += "    AND GBPRN != 'P'                                                                                                    \r";
                SQL += "    AND GBSEND = ' '                                                                                                    \r";
                SQL += "    AND GBSTATUS != 'D'                                                                                                 \r";
                SQL += "    AND SUCODE IS NOT NULL                                                                                              \r";
                SQL += "    AND SLIPNO != '0106'                                                                                                \r";    //자가약

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(dt.Rows[0]["BDATE1"].ToString().Trim() + "일자에 미 전송된 오더가 있습니다.잠시후에 작업 해주세요..", "확인");
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT ROWID , MAYAK , TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE, SUCODE FROM KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + FstrPtno + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + nOrderNo + " ";
                SQL = SQL + ComNum.VBLF + "   AND ORDERCODE = '" + strORDERCODE + "'";
                SQL = SQL + ComNum.VBLF + "   AND GBSTATUS = 'D-'";
                SQL = SQL + ComNum.VBLF + "   AND ORDERSITE ='NDC' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 오더는 이미 NDC 취소 가 되었습니다" + ComNum.VBLF + " 취소 후 환자를 다시 선택 해주세요 ", "확인");
                    return;
                }

                strBdate = dt.Rows[0]["BDATE"].ToString().Trim();

                strSuCode = dt.Rows[0]["SUCODE"].ToString().Trim();

                bolMAYAK = CheckMAYAK(strSuCode);

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "UPDATE KOSMOS_OCS.OCS_IORDER     ";
                SQL = SQL + ComNum.VBLF + "SET DCDIV = '' , NURREMARK = ''  ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("NDC 취소 작업중 오류 발생 하였습니다. " + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 NDC 취소하세요 ");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = "UPDATE KOSMOS_OCS.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + "SET      ";
                SQL = SQL + ComNum.VBLF + "    DCDIV = '',";
                SQL = SQL + ComNum.VBLF + "    ORDERSITE = 'CAN',";
                SQL = SQL + ComNum.VBLF + "    ACCSEND = 'Y'";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID IN(SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "                FROM KOSMOS_OCS.OCS_IORDER";
                SQL = SQL + ComNum.VBLF + "                WHERE PTNO = '" + FstrPtno + "'";
                SQL = SQL + ComNum.VBLF + "                    AND ORDERNO = " + nOrderNo + "";
                SQL = SQL + ComNum.VBLF + "                    AND SUCODE = '" + strSuCode + "'";
                SQL = SQL + ComNum.VBLF + "                    AND GBSTATUS = 'D-'";
                SQL = SQL + ComNum.VBLF + "                    AND ORDERSITE = 'NDC')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("NDC 취소 작업중 오류 발생 하였습니다. " + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 NDC 취소하세요 ");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "INSERT INTO KOSMOS_OCS.OCS_IORDER ( PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, ";
                SQL = SQL + ComNum.VBLF + " SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, ";
                SQL = SQL + ComNum.VBLF + " GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, GBPOSITION, GBSTATUS, ";
                SQL = SQL + ComNum.VBLF + " NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE,     ";
                SQL = SQL + ComNum.VBLF + " MULTI, MULTIREMARK, DUR, LABELPRINT, ACTDIV, GBPICKUP, PICKUPSABUN, PICKUPDATE,  DIVQTY, ";
                SQL = SQL + ComNum.VBLF + " CORDERCODE, CSUCODE, CBUN, ACCSEND  )";
                SQL = SQL + ComNum.VBLF + " SELECT PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, ";
                SQL = SQL + ComNum.VBLF + " BCONTENTS, REALQTY, QTY, REALNAL * -1, -1 *  NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV,    ";
                SQL = SQL + ComNum.VBLF + " GBBOTH, GBACT, GBTFLAG, ' ', GBPOSITION, 'D+', NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, ";
                SQL = SQL + ComNum.VBLF + " GBGROUP, GBPORT, 'CAN', MULTI, MULTIREMARK, DUR, LABELPRINT, ACTDIV , '*', " + Convert.ToInt32(clsType.User.Sabun) + ", SYSDATE,  DIVQTY  * -1, ";
                SQL = SQL + ComNum.VBLF + " ORDERCODE, SUCODE, BUN, '' ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID IN(SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "                 FROM KOSMOS_OCS.OCS_IORDER";
                SQL = SQL + ComNum.VBLF + "                 WHERE PTNO = '" + FstrPtno + "'";
                SQL = SQL + ComNum.VBLF + "                     AND ORDERNO = " + nOrderNo + "";
                SQL = SQL + ComNum.VBLF + "                     AND SUCODE = '" + strSuCode + "'";
                SQL = SQL + ComNum.VBLF + "                     AND GBSTATUS = 'D-'";
                SQL = SQL + ComNum.VBLF + "                     AND ORDERSITE = 'NDC')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("NDC 취소 작업중 오류 발생 하였습니다. " + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 NDC 취소하세요 ");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (bolMAYAK == false)
                {
                    //ndc 취소건도 전송되도록 보완. 
                    //if (ssM_Sheet1.Cells[intRow, 40].Text != "◎")
                    //{
                        if (CANCEL_RTN(ref SQL, ref SqlErr, strDate, nOrderNo, FstrPtno, strBdate, strSuCode) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("NDC 취소 작업중 오류 발생 하였습니다. " + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 NDC 취소하세요 ");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("NDC 취소 작업중 오류 발생 하였습니다. " + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 NDC 취소하세요 ");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    //}
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("수가코드 : " + strSuCode + "은(는) 마약 또는 향정경구약 이므로 OCS에서 반드시 다시 D/C 해주세요.", "확인"); //2019-02-13 향정경구약 추가

                    GetData();

                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                //for (int h = 14; h < 38; h++)
                //{
                //    if (ssM_Sheet1.Cells[intRow, h].CellType != null
                //       && ssM_Sheet1.Cells[intRow, h].CellType.ToString() == "CheckBoxCellType"
                //       && ssM_Sheet1.Cells[intRow, h].BackColor == Color.FromArgb(255, 50, 50))
                //    {
                //        ssM_Sheet1.Cells[intRow, h].CellType = ctCheck;
                //        ssM_Sheet1.Cells[intRow, h].HorizontalAlignment = CellHorizontalAlignment.Center;
                //        ssM_Sheet1.Cells[intRow, h].VerticalAlignment = CellVerticalAlignment.Center;
                //        ssM_Sheet1.Cells[intRow, h].BackColor = Color.FromArgb(255, 200, 200);

                //    }
                //}
                GetData();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("NDC 취소 작업중 오류 발생 하였습니다. " + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 NDC 취소하세요 " + ComNum.VBLF + ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 환자가 재원인지 확인한다.
        /// </summary>
        /// <param name="fstrPtno"></param>
        /// <param name="mintIPDNO"></param>
        /// <returns></returns>
        private bool ChkIn(string fstrPtno, int mintIPDNO)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ComFunc cf = new ComFunc();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT IPDNO, PANO, ACTDATE, OUTDATE, GBSTS      ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.IPD_NEW_MASTER      ";
                
                if (mstrWard == "ER")       //2019-02-25
                {
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + fstrPtno + "'";
                    SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + FstrInDate + " 00:00' ,'YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, FstrInDate, 1) + " 23:59','YYYY-MM-DD HH24:MI')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE IPDNO = " + mintIPDNO + "       ";
                    SQL = SQL + ComNum.VBLF + "    AND PANO = '" + fstrPtno + "'        ";
                }
                //SQL = SQL + ComNum.VBLF + "    AND ACTDATE IS NULL      ";
                //SQL = SQL + ComNum.VBLF + "    AND OUTDATE IS NULL      ";
                SQL = SQL + ComNum.VBLF + "    AND GBSTS IN ('0', '1', '2', '3', '4')        "; // 원무과 요청 심사 완료 후에 미전송 오더 확인한다고 해서 풀어줌
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return true;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return false;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool CheckMAYAK(string strSuCode)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TRIM(JEPCODE)       ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_ADM.DRUG_JEP  ";
                SQL = SQL + ComNum.VBLF + "WHERE(CHENGGU = '09' OR(SUB = '16' AND BUN = '2'))  ";
                SQL = SQL + ComNum.VBLF + "     AND TRIM(JEPCODE) = '" + strSuCode + "'";
                // 2019-02-13 향정 경구약 추가함
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.DRUG_JEP A, KOSMOS_PMPA.BAS_SUT B ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.JEPCODE = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "    AND A.CHENGGU = '08' ";
                SQL = SQL + ComNum.VBLF + "    AND B.BUN = '11' ";
                SQL = SQL + ComNum.VBLF + "    AND B.SUNEXT = '" + strSuCode + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return true;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 'ORDERSITE DC0:의사DC , DC1:간호사DC , DC2:내복약간호사 DC
        /// </summary>
        /// <param name="dbCon"></param>
        private bool CANCEL_RTN(ref string SQL, ref string SqlErr, string strDate, int intOrderNo, string mstrPtNo, string strBdate, string strSuCode)
        {
            DataTable dt = null;
            int intRowAffected = 0;

            string strORDERCODE = "";
            string strQty = "";
            string strOrderSite = "";
            string strBun = "";
            string strTO = "";
            string strROWID = "";

            SQL = "";
            SQL = "SELECT PTNO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, ORDERCODE, SEQNO, QTY,ORDERSITE, SLIPNO, ROWID ";
            SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_IORDER  ";
            //SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + mstrROWID + "'"; //'ROWID 로 읽어온이유는 모든내용이동일하기때문;
            SQL = SQL + ComNum.VBLF + "   WHERE PTNO = '" + mstrPtNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBdate + "','YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + intOrderNo;
            SQL = SQL + ComNum.VBLF + "   AND SUCODE = '" + strSuCode + "'";
            SQL = SQL + ComNum.VBLF + "   AND GBSTATUS = 'D+'";
            SQL = SQL + ComNum.VBLF + "   AND ORDERSITE IN ('DC0','DC1')     ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                return false;
            }

            if (dt.Rows.Count > 0)
            {
                strORDERCODE = dt.Rows[0]["ORDERCODE"].ToString().Trim();
                strQty = dt.Rows[0]["QTY"].ToString().Trim();
                strOrderSite = dt.Rows[0]["ORDERSITE"].ToString().Trim();
                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
            }
            else
            {
                return false;
            }

            dt.Dispose();
            dt = null;

            if (strOrderSite == "DC1")
            {
                SQL = "";
                SQL = "UPDATE ";
                SQL = SQL + ComNum.VBLF + "   KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + ComNum.VBLF + " SET  ORDERSITE ='CAN' ";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID ='" + strROWID + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }

                SQL = "";
                SQL = "INSERT INTO ";
                SQL = SQL + ComNum.VBLF + "  KOSMOS_OCS.OCS_IORDER ( PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ";
                SQL = SQL + ComNum.VBLF + "                          ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS, ";
                SQL = SQL + ComNum.VBLF + "                          REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, ";
                SQL = SQL + ComNum.VBLF + "                          GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, ";
                SQL = SQL + ComNum.VBLF + "                          GBSEND, ACCSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, WARDCODE, ";
                SQL = SQL + ComNum.VBLF + "                          ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ";
                SQL = SQL + ComNum.VBLF + "                          ORDERSITE, MULTI, MULTIREMARK, DUR, GBPICKUP , PICKUPSABUN, ";
                SQL = SQL + ComNum.VBLF + "                          PICKUPDATE, MAYAK, POWDER,GBIOE,IP,GBCHK,GBVERB,VERBAL,ENTDATE2 )  ";
                SQL = SQL + ComNum.VBLF + " (SELECT PTNO, BDATE, SEQNO,  DEPTCODE, DRCODE,  STAFFID,  SLIPNO,    ";
                SQL = SQL + ComNum.VBLF + "        ORDERCODE,   SUCODE, BUN,      GBORDER, CONTENTS, BCONTENTS, ";
                SQL = SQL + ComNum.VBLF + "        REALQTY,     QTY,    REALNAL * -1,      NAL * -1, DOSCODE,   ";
                SQL = SQL + ComNum.VBLF + "        GBINFO,      GBSELF, GBSPC,    GBNGT,   GBER,     GBPRN,     ";
                SQL = SQL + ComNum.VBLF + "        GBDIV,       GBBOTH, GBACT,    GBTFLAG,";

                //2018-12-19
                if (VB.Val(strQty) == 0)
                {
                    SQL = SQL + ComNum.VBLF + "' ',";   //GBSEND
                    SQL = SQL + ComNum.VBLF + "'Y',";   //ACCSEND
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " '*',";  //GBSEND
                    SQL = SQL + ComNum.VBLF + "'',";    //ACCSEND
                }

                SQL = SQL + ComNum.VBLF + "        GBPOSITION ,'D-', ";
                SQL = SQL + ComNum.VBLF + "        '" + clsType.User.Sabun + "', ";
                SQL = SQL + ComNum.VBLF + "        SYSDATE,     WARDCODE,ROOMCODE, BI,     ORDERNO,  REMARK,    ";
                SQL = SQL + ComNum.VBLF + "        TO_DATE('" + strDate + "','YYYY-MM-DD'),      GBGROUP,   ";
                SQL = SQL + ComNum.VBLF + "        GBPORT,      'CAN' , MULTI, MULTIREMARK , DUR, '*','" + clsType.User.Sabun + "' , SYSDATE, MAYAK, POWDER,GBIOE,'" + clsCompuInfo.gstrCOMIP + "','A1',GBVERB,VERBAL,SYSDATE ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + ComNum.VBLF + " WHERE  ROWID = '" + strROWID + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }

                SQL = "";
                SQL = " INSERT INTO ";
                SQL = SQL + ComNum.VBLF + "  KOSMOS_OCS.OCS_IORDER (PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, ";
                SQL = SQL + ComNum.VBLF + "                          SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, ";
                SQL = SQL + ComNum.VBLF + "                          BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, ";
                SQL = SQL + ComNum.VBLF + "                          GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, ";
                SQL = SQL + ComNum.VBLF + "                          GBTFLAG, GBSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, ";
                SQL = SQL + ComNum.VBLF + "                          WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, ";
                SQL = SQL + ComNum.VBLF + "                          GBPORT, ORDERSITE, MULTI, MULTIREMARK, DUR, GBPICKUP, ";
                SQL = SQL + ComNum.VBLF + "                          PICKUPSABUN, PICKUPDATE, MAYAK, POWDER,GBIOE,IP,GBCHK, ";
                SQL = SQL + ComNum.VBLF + "                          GBVERB,VERBAL,ENTDATE2 )  ";
                SQL = SQL + ComNum.VBLF + " (SELECT PTNO, BDATE, SEQNO,  DEPTCODE, DRCODE,  STAFFID,  SLIPNO,    ";
                SQL = SQL + ComNum.VBLF + "        ORDERCODE,   SUCODE, BUN,      GBORDER, CONTENTS, BCONTENTS, ";
                SQL = SQL + ComNum.VBLF + "        REALQTY,     QTY,    REALNAL * -1,      NAL * -1, DOSCODE,   ";
                SQL = SQL + ComNum.VBLF + "        GBINFO,      GBSELF, GBSPC,    GBNGT,   GBER,     GBPRN,     ";
                SQL = SQL + ComNum.VBLF + "        GBDIV,       GBBOTH, GBACT,    GBTFLAG, ";
                SQL = SQL + ComNum.VBLF + "        '*',";
                SQL = SQL + ComNum.VBLF + "        GBPOSITION, ' ', ";
                SQL = SQL + ComNum.VBLF + " '" + clsType.User.Sabun + "', ";
                SQL = SQL + ComNum.VBLF + "        SYSDATE,     WARDCODE,ROOMCODE, BI,     ORDERNO,  REMARK,    ";
                SQL = SQL + ComNum.VBLF + "        TO_DATE('" + strDate + "','YYYY-MM-DD'),      GBGROUP,   ";
                SQL = SQL + ComNum.VBLF + "        GBPORT,      'CAN' , MULTI , MULTIREMARK, DUR , '*','" + clsType.User.Sabun + "' , SYSDATE, MAYAK, POWDER,GBIOE,'" + clsCompuInfo.gstrCOMIP + "','A2',GBVERB,VERBAL,SYSDATE ";
                SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO='" + mstrPtNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ORDERCODE ='" + strORDERCODE + "'";
                SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "'";
                SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + intOrderNo;
                SQL = SQL + ComNum.VBLF + "   AND ORDERSITE ='DC1'";
                SQL = SQL + ComNum.VBLF + "   AND GBSTATUS ='D-'";
                SQL = SQL + ComNum.VBLF + "   AND (DCDIV = 0 OR DCDIV IS NULL) )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }

                SQL = "UPDATE ";
                SQL = SQL + ComNum.VBLF + "   KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + ComNum.VBLF + " SET ORDERSITE='CAN' ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO='" + mstrPtNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ORDERCODE ='" + strORDERCODE + "'";
                SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "'";
                SQL = SQL + ComNum.VBLF + "   AND ORDERSITE ='DC1'";
                SQL = SQL + ComNum.VBLF + "   AND GBSTATUS ='D-'";
                SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + intOrderNo;
                SQL = SQL + ComNum.VBLF + "   AND (DCDIV = 0 OR DCDIV IS NULL) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }
            }
            else //의사 DC
            {
                if (strBun == "11" && (strTO != "T" && strTO != "O")) //'내복약 DC일경우(퇴원약,외출약제외) 무조건 금액반영해줌
                {
                    SQL = "";
                    SQL = " INSERT INTO ";
                    SQL = SQL + ComNum.VBLF + "   KOSMOS_OCS.OCS_IORDER (PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ";
                    SQL = SQL + ComNum.VBLF + "                          ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS, ";
                    SQL = SQL + ComNum.VBLF + "                          REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, ";
                    SQL = SQL + ComNum.VBLF + "                          GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, ";
                    SQL = SQL + ComNum.VBLF + "                          GBPOSITION, GBSTATUS, NURSEID, ENTDATE, WARDCODE, ROOMCODE, ";
                    SQL = SQL + ComNum.VBLF + "                          BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, ";
                    SQL = SQL + ComNum.VBLF + "                          MULTIREMARK, DUR, GBPICKUP, PICKUPSABUN, PICKUPDATE, MAYAK, ";
                    SQL = SQL + ComNum.VBLF + "                          POWDER,GBIOE,IP,GBCHK,GBVERB,VERBAL,ENTDATE2 )  ";
                    SQL = SQL + ComNum.VBLF + " (SELECT PTNO, BDATE, SEQNO,  DEPTCODE, DRCODE,  STAFFID,  SLIPNO,    ";
                    SQL = SQL + ComNum.VBLF + "        ORDERCODE,   SUCODE, BUN,      GBORDER, CONTENTS, BCONTENTS, ";
                    SQL = SQL + ComNum.VBLF + "        REALQTY,     QTY,    REALNAL * -1,      NAL * -1, DOSCODE,   ";
                    SQL = SQL + ComNum.VBLF + "        GBINFO,      GBSELF, GBSPC,    GBNGT,   GBER,     GBPRN,     ";
                    SQL = SQL + ComNum.VBLF + "        GBDIV,       GBBOTH, GBACT,    GBTFLAG, ";
                    SQL = SQL + ComNum.VBLF + "        '*',";
                    SQL = SQL + ComNum.VBLF + "        GBPOSITION, ' ', ";
                    SQL = SQL + ComNum.VBLF + " '" + clsType.User.Sabun + "', ";
                    SQL = SQL + ComNum.VBLF + "        SYSDATE,     WARDCODE,ROOMCODE, BI,     ORDERNO,  REMARK,    ";
                    SQL = SQL + ComNum.VBLF + "        TO_DATE('" + strDate + "','YYYY-MM-DD'),      GBGROUP,   ";
                    SQL = SQL + ComNum.VBLF + "        GBPORT,      'DC2', MULTI, MULTIREMARK, DUR, '*', '" + clsType.User.Sabun + "', SYSDATE  , MAYAK, POWDER,GBIOE,'" + clsCompuInfo.gstrCOMIP + "','A5',GBVERB,VERBAL,SYSDATE ";
                    SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_IORDER  ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO='" + mstrPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBdate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND ORDERCODE ='" + strORDERCODE + "'";
                    SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "'";
                    SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + intOrderNo;
                    SQL = SQL + ComNum.VBLF + "   AND ORDERSITE ='DC2'";
                    SQL = SQL + ComNum.VBLF + "   AND GBSTATUS ='D-')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        return false;
                    }
                }

                SQL = "";
                SQL = "INSERT INTO ";
                SQL = SQL + ComNum.VBLF + "   KOSMOS_OCS.OCS_IORDER  (PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ";
                SQL = SQL + ComNum.VBLF + "                           ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS, ";
                SQL = SQL + ComNum.VBLF + "                           REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, ";
                SQL = SQL + ComNum.VBLF + "                           GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, ";
                SQL = SQL + ComNum.VBLF + "                           GBTFLAG, GBSEND, ACCSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, ";
                SQL = SQL + ComNum.VBLF + "                           WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, ";
                SQL = SQL + ComNum.VBLF + "                           GBPORT, ORDERSITE, MULTI, MULTIREMARK, DUR, GBPICKUP, ";
                SQL = SQL + ComNum.VBLF + "                           PICKUPSABUN, PICKUPDATE, MAYAK, POWDER,GBIOE,IP,GBCHK,GBVERB,VERBAL,ENTDATE2 ) ";
                SQL = SQL + ComNum.VBLF + " (SELECT PTNO, BDATE, SEQNO,  DEPTCODE, DRCODE,  STAFFID,  SLIPNO,    ";
                SQL = SQL + ComNum.VBLF + "        ORDERCODE,   SUCODE, BUN,      GBORDER, CONTENTS, BCONTENTS, ";
                SQL = SQL + ComNum.VBLF + "        REALQTY,     QTY,    REALNAL * -1,      NAL * -1, DOSCODE,   ";
                SQL = SQL + ComNum.VBLF + "        GBINFO,      GBSELF, GBSPC,    GBNGT,   GBER,     GBPRN,     ";
                SQL = SQL + ComNum.VBLF + "        GBDIV,       GBBOTH, GBACT,    GBTFLAG, ";

                if (VB.Val(strQty) == 0)
                {
                    SQL = SQL + ComNum.VBLF + "' ',";
                    SQL = SQL + ComNum.VBLF + "'Y',";       //2019-06-17 추가
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " '*',";
                    SQL = SQL + ComNum.VBLF + " '',";       //2019-06-17 추가
                }

                SQL = SQL + ComNum.VBLF + "        GBPOSITION , 'D-', ";
                SQL = SQL + ComNum.VBLF + " '" + clsType.User.Sabun + "', ";
                SQL = SQL + ComNum.VBLF + "        SYSDATE,     WARDCODE,ROOMCODE, BI,     ORDERNO,  REMARK,    ";
                SQL = SQL + ComNum.VBLF + "        TO_DATE('" + strDate + "','YYYY-MM-DD'),      GBGROUP,   ";
                SQL = SQL + ComNum.VBLF + "        GBPORT,      'CAN' , MULTI, MULTIREMARK, DUR, '*', '" + clsType.User.Sabun + "', SYSDATE , MAYAK, POWDER,GBIOE,'" + clsCompuInfo.gstrCOMIP + "','A6',GBVERB,VERBAL,SYSDATE ";
                SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + ComNum.VBLF + " WHERE  ROWID = '" + strROWID + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }

                SQL = "";
                SQL = "UPDATE ";
                SQL = SQL + ComNum.VBLF + "   KOSMOS_OCS.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + " SET ORDERSITE='CAN' ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO='" + mstrPtNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ORDERCODE ='" + strORDERCODE + "'";
                SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSuCode + "'";
                SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + intOrderNo;
                SQL = SQL + ComNum.VBLF + "   AND (ORDERSITE  ='DC0'  OR ORDERSITE='DC2')";
                SQL = SQL + ComNum.VBLF + "   AND (DCDIV = 0 OR DCDIV IS NULL) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }
            }

            return true;
        }

        private void Acting_Cancel(int intRow, int intCol)
        {
            //string strOK = "OK";
            string strROWID = ssM_Sheet1.Cells[intRow, 47].Text.Trim();
            string strRowid_act = "";
            string strEMRNO = "";

            int nOrderNo = Convert.ToInt32(ssM_Sheet1.Cells[intRow, 48].Text.Trim());
            int nActCol = intCol - 13;
            double nActDiv = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0;

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                return; //권한 확인

            if (clsEmrQuery.SapOrderExists(clsDB.DbCon, strROWID, nActCol))
            {
                ComFunc.MsgBoxEx(this, "이미 시작중인 수액기록지가 있습니다.\r\n삭제 한 후 취소 해주세요");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT ROWID  ";
                SQL = SQL + ComNum.VBLF + " FROM " + mstrACTTable;
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + FstrPtno + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + nOrderNo + " ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDIV = '" + nActCol + "'";
                SQL = SQL + ComNum.VBLF + "   AND GBSTATUS = ' '";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 오더는 이미 ACTING 취소 가 되었습니다" + ComNum.VBLF + " 취소 후 환자를 다시 선택 해주세요 ", "확인");
                    SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                    return;
                }
                else
                {
                    strRowid_act = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //'중복 acting 문제로 다시 한번 읽어서 처리 비교함

                SQL = "";
                SQL = " SELECT GBSTATUS, GBDIV , ACTDIV  FROM KOSMOS_OCS.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("ACTING 취소 중 문제가 발생하였습니다. 전산실로 연락 부탁드립니다.", "확인");
                    SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                    return;
                }
                else
                {
                    nActDiv = VB.Val(dt.Rows[0]["ACTDIV"].ToString().Trim());
                    nActDiv = nActDiv - 1;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " UPDATE " + mstrACTTable;
                SQL = SQL + ComNum.VBLF + "   SET GBSTATUS = 'D+'";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowid_act + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                    return;
                }

                SQL = "";
                SQL = "INSERT INTO " + mstrACTTable;
                SQL = SQL + ComNum.VBLF + " ( PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS,  ";
                SQL = SQL + ComNum.VBLF + "REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG,   ";
                SQL = SQL + ComNum.VBLF + "GBSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT,   ";
                SQL = SQL + ComNum.VBLF + "ORDERSITE, MULTI, MULTIREMARK, DUR, LABELPRINT, ACTDIV, ACTTIME, ACTSABUN, DIVQTY )";
                SQL = SQL + ComNum.VBLF + "SELECT PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS, ";
                SQL = SQL + ComNum.VBLF + "REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, ";
                SQL = SQL + ComNum.VBLF + "GBPOSITION, 'D+', NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, ";
                SQL = SQL + ComNum.VBLF + "MULTI, MULTIREMARK, DUR, LABELPRINT, ACTDIV , SYSDATE,  " + Convert.ToString(Convert.ToInt32(clsType.User.Sabun)) + ", DIVQTY  * -1  ";
                SQL = SQL + ComNum.VBLF + "FROM " + mstrACTTable;
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowid_act + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                    return;
                }

                SQL = "";
                SQL = " SELECT COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + "  FROM " + mstrACTTable;
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + FstrPtno + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + nOrderNo + " ";
                SQL = SQL + ComNum.VBLF + "   AND GBSTATUS = ' ' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("ACTING 취소 중 문제가 발생하였습니다. 전산실로 연락 부탁드립니다.", "확인");
                    SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                    return;
                }
                else
                {
                    nActDiv = VB.Val(dt.Rows[0]["CNT"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " UPDATE KOSMOS_OCS.OCS_IORDER SET ACTDIV = " + nActDiv + "  ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                    return;
                }

                //'mtsemr 20100323
                //'액팅 취소시 emr 투약기록지도 삭제 한다.
                SQL = "";
                SQL = " SELECT EMRNO ";
                SQL = SQL + ComNum.VBLF + "  FROM " + mstrACTTable;
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strRowid_act + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("ACTING 취소 중 문제가 발생하였습니다. 전산실로 연락 부탁드립니다.", "확인");
                    SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                    return;
                }
                else
                {
                    strEMRNO = dt.Rows[0]["EMRNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (DeleteEmrXmlData(strEMRNO) == false)
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("ACTING 취소 중 문제가 발생하였습니다. 전산실로 연락 부탁드립니다.", "확인");
                    SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
                    return;
                }

                //'mtsemr>>>

                //'SHEEL 체크박스로 변경

                ssM_Sheet1.Cells[intRow, intCol].CellType = ctCheck;
                ssM_Sheet1.Cells[intRow, intCol].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssM_Sheet1.Cells[intRow, intCol].VerticalAlignment = CellVerticalAlignment.Center;
                ssM_Sheet1.Cells[intRow, intCol].ForeColor = Color.FromArgb(0, 0, 0);
                ssM_Sheet1.Cells[intRow, intCol].BackColor = Color.FromArgb(255, 210, 210);
                ssM_Sheet1.Cells[intRow, intCol].Value = false;

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                SearchssList(ssList_Sheet1.ActiveRowIndex, ssList_Sheet1.ActiveColumnIndex);
            }
        }

        private bool DeleteEmrXmlData(string strEMRNO)
        {
            //'기존차트를 변경할 경우 : 백업 테이블로 백업을 하고 신규 data를 입력한다
            //'KOSMOS_EMR.EMRXMLHISTORY_HISTORYNO_SEQ
            double dblEmrHisNo = 0;

            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            dblEmrHisNo = clsIpdNr.GetSequencesNo(clsDB.DbCon, "KOSMOS_EMR.EMRXMLHISNO");

            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string strCurTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");

            EmrForm pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, "1796", clsEmrQuery.GetNewFormMaxUpdateNo(clsDB.DbCon, 1796).ToString());


            if (pForm.FmOLDGB == 1)
            {
                #region XXML
                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_EMR.EMRXMLHISTORY";
                SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS, HISTORYWRITEDATE,HISTORYWRITETIME, UPDATENO,";
                SQL = SQL + ComNum.VBLF + "      DELUSEID,CERTNO)";
                SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(SYSDATE,'YYYYMMDD') , TO_CHAR(SYSDATE,'HH24MMSS') , UPDATENO, ";
                SQL = SQL + ComNum.VBLF + "       '" + clsType.User.Sabun + "',CERTNO";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEMRNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEMRNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_EMR.EMRXMLHISTORY_TUYAK";
                SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,HISTORYWRITEDATE,HISTORYWRITETIME, ";
                SQL = SQL + ComNum.VBLF + "      DELUSEID,CERTNO, IT1, IT2, IT3, IT4, IT5, IT6, IT7, IT8, IT9, IT10)";
                SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(SYSDATE,'YYYYMMDD') , TO_CHAR(SYSDATE,'HH24MMSS') ,  ";
                SQL = SQL + ComNum.VBLF + "       '" + clsType.User.Sabun + "',CERTNO, IT1, IT2, IT3, IT4, IT5, IT6, IT7, IT8, IT9, IT10";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML_TUYAK";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEMRNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXML_TUYAK";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEMRNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXMLMST";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEMRNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                #endregion
            }
            else
            {
                if (clsEmrQuery.SaveChartMastHis(clsDB.DbCon, strEMRNO, dblEmrHisNo, strCurDate, strCurTime, "C", "", clsType.User.IdNumber) != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
            }

        

            return true;
        }

        private void Frm_rEventClosed()
        {
            DCActing.Dispose();
            DCActing = null;
        }

        private void Frm_rSendMsg(string strHospCode)
        {
            GstrHospCode = strHospCode;

        }


        private void btnDeleteDCSession_Click(object sender, EventArgs e)
        {
            GstrHospCode = "";


            //Call READ_SYSDATE
            if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == false)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("정말로 DC을 실시 하시겠습니까?", "DC확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            DCActing = new frmDCActing();
            DCActing.rSendMsg += Frm_rSendMsg;
            DCActing.rEventClosed += Frm_rEventClosed;
            DCActing.StartPosition = FormStartPosition.CenterScreen;
            DCActing.ShowDialog();
            DCActing = null;


            string strROWID = "";
            //int nOrderNo = 0;
            //string strFlag = "";
            //int nActDiv = 0;
            int nActCol = 0;
            int nDCDiv = 0;

            //string strOK = "";
            //string strActTime = "";

            //string strOrderName = "";
            //string strTable = "";

            double nDivQty = 0;
            double nQty = 0;
            int nContents = 0;
            int nBContents = 0;
            string strItemCD = "";
            string strGbIOE = "";   //2019-02-14(ER 처방 부분 DC의 경우 EI 처방만 가능하도록

            int i = 0;
            int h = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            bool bolD = false;
            string strBUN = "";
            string strORDERCODE = "";
            double dblSQty = 0;
            double dblDvQty = 0;
            double dblQty = 0;
            double dblDQty = 0;
            string strNURSEID = "";

            //수가 수량과 실수량 다른 경우 체크용
            double dblRealQty = 0;
            double dblDvRealQty = 0;
            double dblSRealQty = 0;

            //int nTemp = 0;

            string strMsg = "";

            bool bolMAYAK = false;


            DataTable dt = null;

            /* 아래 조건에 해당할 경우만 DC가능
             * TABLE  : IPD_NEW_MASTER
               COULMN : GBSTS
                        0 : 재원
                        1 : 가퇴원
                        2 : 퇴원접수
                        3 : 대조리스트인쇄
                        4 : 심사부분완료
            */
            if (ChkIn(FstrPtno, mintIPDNO) == false)
            {
                ComFunc.MsgBox("현재 환자는 재원상태가 아닙니다. 확인 후 디시  해주십시오.");
                return;
            }

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //SQL = "";
                //SQL = " SELECT A.*, TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE1, SLIPNO, A.ROWID , ";
                //SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE1 ";
                //SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_IORDER  A";
                //SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO     = '" + FstrPtno + "' ";
                //SQL = SQL + ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + mstrBDate + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "  AND (A.GBSEND ='*' OR A.ACCSEND IS NULL) ";

                ////    '2014-02-27
                //if (cboWard.Text.Trim() == "ER")
                //{
                //    SQL = SQL + ComNum.VBLF + "  AND GBIOE IN ('EI') ";
                //}

                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE1 ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_IORDER ";
                SQL += ComNum.VBLF + "  WHERE Ptno       = '" + FstrPtno + "' ";
                SQL += ComNum.VBLF + "    AND BDATE      = TO_DATE('" + mstrBDate + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  != 'ER' ";
                //2019-02-25 
                //이비인후과 안과 마취과 컨설트의 경우 DRCODE2 컬럼에 값이 있을 경우(회신한 경우)만 ACCSEND = 'Z' 를 체크함
                //SQL += "    AND (ACCSEND IS NULL or ACCSEND = 'Z')              
                SQL += ComNum.VBLF + "    AND  ( ";
                SQL += ComNum.VBLF + "            ( SUCODE NOT IN ('C-OT', 'C-EN', 'C-PC') AND (ACCSEND IS NULL OR ACCSEND = 'Z') ) ";
                SQL += ComNum.VBLF + "          OR ";
                SQL += ComNum.VBLF + "            ( SUCODE IN ('C-EN', 'C-OT', 'C-PC') AND DRCODE2 IS NOT NULL AND ACCSEND = 'Z') ";
                SQL += ComNum.VBLF + "          ) ";
                SQL += ComNum.VBLF + "    AND ORDERSITE not in ('NDC', 'ERO') ";
                SQL += ComNum.VBLF + "    AND GBPRN != 'P' ";
                SQL += ComNum.VBLF + "    AND GBSEND = ' '  ";
                SQL += ComNum.VBLF + "    AND GBSTATUS != 'D' ";
                SQL += ComNum.VBLF + "    AND SUCODE IS NOT NULL  ";
                SQL += ComNum.VBLF + "    AND SLIPNO != '0106' "; //지참약

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    strMsg = dt.Rows[0]["BDATE1"].ToString().Trim();
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox(strMsg + "일자에 미 전송된 오더가 있습니다.잠시후에 작업 해주세요..", "확인");
                    Cursor.Current = Cursors.Default;
                    GetData();
                    return;
                }

                dt.Dispose();
                dt = null;

                //strOK = "OK";

                for (i = 0; i < ssM_Sheet1.RowCount; i++)
                {
                    strROWID = ssM_Sheet1.Cells[i, 47].Text.Trim();
                    strItemCD = ssM_Sheet1.Cells[i, 50].Text.Trim();
                    strGbIOE = ssM_Sheet1.Cells[i, 55].Text.Trim();     //2019-02-14

                    nActCol = 0;
                    
                    if (mstrWard == "ER" && strGbIOE != "EI")       //2019-02-25
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }
                        ComFunc.MsgBox(strMsg + "응급실 처방의 간호사 (부분)DC의 경우 응급실 입원처방만 가능합니다.", "확인");
                        Cursor.Current = Cursors.Default;
                        GetData();
                        return;
                    }

                    for (h = 14; h < 38; h++)
                    {
                        //ssM.Col = j: strFlag = ssM.Text

                        if (ssM_Sheet1.Columns.Get(h).Visible == false)
                        {
                            break;
                        }

                        nActCol = nActCol + 1;

                        if (ssM_Sheet1.Cells[i, h].CellType != null
                            && ssM_Sheet1.Cells[i, h].CellType.ToString() == "CheckBoxCellType"
                            && Convert.ToBoolean(ssM_Sheet1.Cells[i, h].Value) == true)
                        {
                            //'중복 acting 문제로 다시 한번 읽어서 처리 비교함

                            dblDQty = dblDQty + 1;  // qty 나누기 div가  소수점으로 연산될때에서 오류 있음

                            SQL = "";
                            SQL = "SELECT GBDIV , ACTDIV, GBSTATUS,  DCDIV, CONTENTS, BCONTENTS, REALQTY, QTY, BUN, ORDERCODE, NURSEID,  SUCODE";
                            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_IORDER      ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt.Rows.Count == 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                dt.Dispose();
                                dt = null;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            nDCDiv = Convert.ToInt32(VB.Val(dt.Rows[0]["DCDIV"].ToString().Trim()));

                            if (nDCDiv != 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("이미 NDC 되었던 처방이 있습니다. " + ComNum.VBLF
                                    + "ORDERCODE : " + dt.Rows[0]["ORDERCODE"].ToString().Trim() + ComNum.VBLF
                                    + "추가 DC를 원하지면 기존의 DC를 취소 하시고 다시 DC 헤주세요.");
                                dt.Dispose();
                                dt = null;
                                return;
                            }

                            nContents = Convert.ToInt32(VB.Val(dt.Rows[0]["CONTENTS"].ToString().Trim()));
                            nBContents = Convert.ToInt32(VB.Val(dt.Rows[0]["BCONTENTS"].ToString().Trim()));

                            strBUN = dt.Rows[0]["BUN"].ToString().Trim();

                            dblQty = VB.Val(dt.Rows[0]["QTY"].ToString().Trim());
                            dblDvQty = VB.Val(dt.Rows[0]["QTY"].ToString().Trim()) / VB.Val(dt.Rows[0]["GBDIV"].ToString().Trim());
                            dblRealQty = VB.Val(dt.Rows[0]["REALQTY"].ToString().Trim());
                            dblDvRealQty = VB.Val(dt.Rows[0]["REALQTY"].ToString().Trim()) / VB.Val(dt.Rows[0]["GBDIV"].ToString().Trim());
                            //dblDvQty = Convert.ToDouble(dt.Rows[0]["QTY"].ToString().Trim()) / VB.Val(dt.Rows[0]["GBDIV"].ToString().Trim());


                            //2019-08-19 외용약의 경우 나눈 값이 정수(소수점이 없음)일 때만 NDC가 가능하도록 보완
                            #region                            
                            if (strBUN == "12" && (dblDvQty % 1) != 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                dt.Dispose();
                                dt = null;
                                ComFunc.MsgBox("외용약의 경우 한개의 약을 부분적으로 나누는 DC는 불가능합니다. " +ComNum.VBLF + 
                                             " 해당 약 전체를 취소하실 경우 의사처방에서 DC를 하시기 바랍니다.", "NDC 오류");
                                return;
                            }
                            #endregion

                            if (dt.Rows[0]["GBSTATUS"].ToString().Trim() == "D")
                            {
                                bolD = true;
                            }
                            else
                            {
                                bolD = false;
                            }

                            bolMAYAK = CheckMAYAK(dt.Rows[0]["SUCODE"].ToString().Trim());

                            strNURSEID = dt.Rows[0]["NURSEID"].ToString().Trim();

                            strORDERCODE = dt.Rows[0]["ORDERCODE"].ToString().Trim();

                            //'ITEMCD(약제):(A:Int(수량) * Divide, B:Int(수량 * Divide))

                            //'간호사 DC는 반올림 안함. 정수화해서 작업함.
                            if (nContents != nBContents) //'용량 콘트롤
                            {
                                nContents = Convert.ToInt32(nContents / VB.Val(dt.Rows[0]["GBDIV"].ToString().Trim()));

                                if (strItemCD == "A") //'Int(수량) * Divide
                                {
                                    nQty = Convert.ToInt32((nContents / nBContents) + 0.99);
                                }
                                else if (strItemCD == "B")//'Int(수량 * Divide)
                                {
                                    nQty = Convert.ToInt32((nContents / nBContents) + 0.99);
                                }
                                else
                                {
                                    nQty = ((nContents / nBContents));
                                }

                                nQty = VB.FixDbl(nQty * 100) / 100; //'소수 2자리 절사

                                nDivQty = 1;
                            }
                            else //'수량 콘트롤
                            {
                                nDivQty = VB.Val(dt.Rows[0]["REALQTY"].ToString().Trim()) / VB.Val(dt.Rows[0]["GBDIV"].ToString().Trim());
                                nDivQty = VB.FixDbl(nDivQty * 100) / 100; //'절사

                                //'계산방식에 따라서 올림 반올림 설청
                                nQty = VB.Val(dt.Rows[0]["QTY"].ToString().Trim()) / VB.Val(dt.Rows[0]["GBDIV"].ToString().Trim());
                            }

                            if (VB.Val(dt.Rows[0]["GBDIV"].ToString().Trim()) - VB.Val(dt.Rows[0]["ACTDIV"].ToString().Trim())
                            <= VB.Val(dt.Rows[0]["DCDIV"].ToString().Trim()))
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                dt.Dispose();
                                dt = null;
                                ComFunc.MsgBox(" NDC 작업중 오류 발생 하였습니다. "
                                + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 NDC 하세요 ", "오류");
                                return;
                            }

                            dt.Dispose();
                            dt = null;

                            nDCDiv = nDCDiv + 1;

                            // '오더 DC
                            SQL = "";
                            SQL = "INSERT INTO KOSMOS_OCS.OCS_IORDER ( PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS, ";
                            SQL = SQL + ComNum.VBLF + " REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, ";
                            SQL = SQL + ComNum.VBLF + " GBPOSITION, GBSTATUS, NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, ";
                            SQL = SQL + ComNum.VBLF + " MULTI, MULTIREMARK, DUR, LABELPRINT, ACTDIV,  DIVQTY, GBIOE, GBPICKUP, PICKUPSABUN , PICKUPDATE, DCDIV , EMRSET, MAYAK, POWDER, ";
                            SQL = SQL + ComNum.VBLF + " CORDERCODE, CSUCODE, CBUN )";
                            SQL = SQL + ComNum.VBLF + " SELECT PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, ";
                            SQL = SQL + ComNum.VBLF + " '" + nContents + "' , "; //'1회투여 용량;
                            SQL = SQL + ComNum.VBLF + " '" + nBContents + "'  , ";
                            SQL = SQL + ComNum.VBLF + " '" + nDivQty + "', ";
                            SQL = SQL + ComNum.VBLF + " '" + nQty + "' , ";
                            SQL = SQL + ComNum.VBLF + " REALNAL * -1, -1 * NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, '1', GBBOTH, GBACT, GBTFLAG, ' ', GBPOSITION, ";
                            SQL = SQL + ComNum.VBLF + " '" + "D-'  ,";
                            SQL = SQL + ComNum.VBLF + " '" + Convert.ToInt32(clsType.User.Sabun) + "' , ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, 'NDC', ";
                            SQL = SQL + ComNum.VBLF + " MULTI, MULTIREMARK, DUR, LABELPRINT, ACTDIV, ";
                            SQL = SQL + ComNum.VBLF + " '" + nDivQty + "' , GBIOE , '*', '" + Convert.ToInt32(clsType.User.Sabun) + "',  SYSDATE, '" + nActCol + "'  , EMRSET, MAYAK, POWDER, ";
                            SQL = SQL + ComNum.VBLF + "  ORDERCODE, SUCODE, BUN ";
                            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_IORDER ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(" NDC 작업중 오류 발생 하였습니다. "
                                + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 NDC 하세요 ");
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }

                    if (dblDQty != 0)
                    {
                        //부분NDC 적용시 제일 먼저 바꿔야 함.

                        SQL = "";
                        SQL = "UPDATE KOSMOS_OCS.OCS_IORDER  SET DCDIV = " + dblDQty + ",NURREMARK = '" + GstrHospCode + "' " ;
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(" NDC 작업중 오류 발생 하였습니다. "
                                            + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 NDC 하세요 ");
                            Cursor.Current = Cursors.Default;
                            return;
                        }




                        dblSQty = VB.Val(((dblQty - (dblDvQty * dblDQty))).ToString("###0.0#"));
                        

                        if (bolMAYAK == false)
                        {
                            //2020-12-09 파우더 NDC 후 간호DC 처방 발생하도록 보완 작업=> 
                            //if (ssM_Sheet1.Cells[i, 40].Text != "◎")
                            //{
                            //실수량과 수가수량 다를 경우 + 수량콘트롤인 경우(2021-04-22) realqty 읽기 아직 테스트 중.
                            if (dblQty != dblRealQty && nContents == nBContents)
                            {
                                dblSRealQty = VB.Val(((dblRealQty - (dblDvRealQty * dblDQty))).ToString("###0.0#"));

                                //DELETE_NDC(ref SQL, ref SqlErr, dtpDate1.Value.ToString("yyyy-MM-dd"), bolD, strROWID, strBUN, strNURSEID, dblSQty, dblQty.ToString("###0.0#"), dblDQty.ToString("###0.0#"), strORDERCODE, dblDvQty, dblSRealQty);
                                DELETE_NDC(ref SQL, ref SqlErr, dtpDate1.Value.ToString("yyyy-MM-dd"), bolD, strROWID, strBUN, strNURSEID, dblSQty, dblQty.ToString("###0.0#"), dblDQty.ToString("###0.0#"), strORDERCODE, dblDvQty);
                            }
                            else
                            {
                                DELETE_NDC(ref SQL, ref SqlErr, dtpDate1.Value.ToString("yyyy-MM-dd"), bolD, strROWID, strBUN, strNURSEID, dblSQty, dblQty.ToString("###0.0#"), dblDQty.ToString("###0.0#"), strORDERCODE, dblDvQty);
                            }
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(" NDC 작업중 오류 발생 하였습니다. "
                                + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 NDC 하세요 ");
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            //}
                        }

                        dblDQty = 0;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(" NDC 작업중 오류 발생 하였습니다. "
                     + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 NDC 하세요 ", "오류");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

            SCREEN_CLEAR();

            optGB0.Checked = true;
        }

        //TODO : NDC 적용 고민
        /// <summary>
        /// '수량반납
        /// </summary>
        /// <param name="sQL"></param>
        /// <param name="sqlErr"></param>
        /// <param name="strDate"></param>
        /// <returns></returns>
        private bool DELETE_NDC(ref string SQL, ref string SqlErr, string strDate
            , bool mbolD, string mstrROWID, string mstrBun, string mstrNurseID, double mdblSQty, string mstrQty, string mstrDQty, string mstrORDERCODE, double mdblCheckQty, double mdblReadQty = 0)
        {
            bool RtnVal = false;
            int intRowAffected = 0;

            double dblQty = 0;

            string decSeparator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator; //소수점 여부 확인용


            if (mbolD == false) //'의사가 처방취소 "않"한 경우
            {
                SQL = "";
                SQL = "UPDATE ";
                SQL = SQL + "  KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + " SET GBSTATUS = ' ', ORDERSITE = ' ' WHERE ROWID = '" + mstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    return RtnVal;
                }

                SQL = "INSERT INTO ";
                SQL = SQL + ComNum.VBLF + "  KOSMOS_OCS.OCS_IORDER  (PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, ";
                SQL = SQL + ComNum.VBLF + " GBORDER, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, ";
                SQL = SQL + ComNum.VBLF + " GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, ";
                SQL = SQL + ComNum.VBLF + " REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, DUR , GBPICKUP, PICKUPSABUN, PICKUPDATE , MAYAK, ";
                SQL = SQL + ComNum.VBLF + " POWDER,GBIOE,IP,GBCHK,GBVERB,VERBAL,ENTDATE2) ";
                SQL = SQL + ComNum.VBLF + "(SELECT PTNO, BDATE, SEQNO,  DEPTCODE, DRCODE,  STAFFID,  SLIPNO,    ";
                SQL = SQL + ComNum.VBLF + "        ORDERCODE,   SUCODE, BUN,      GBORDER, CONTENTS, BCONTENTS, ";
                SQL = SQL + ComNum.VBLF + "        REALQTY,     QTY,    REALNAL * -1,      NAL * -1, DOSCODE,   ";
                SQL = SQL + ComNum.VBLF + "        GBINFO,      GBSELF, GBSPC,    GBNGT,   GBER,     GBPRN,     ";
                SQL = SQL + ComNum.VBLF + "        GBDIV,       GBBOTH, GBACT,    GBTFLAG, '*',      GBPOSITION,";
                SQL = SQL + ComNum.VBLF + "       'D-', ";
                SQL = SQL + ComNum.VBLF + " '" + Convert.ToInt32(clsType.User.Sabun) + "', ";
                SQL = SQL + ComNum.VBLF + "        SYSDATE,     WARDCODE,ROOMCODE, BI,     ORDERNO,  REMARK,    ";
                SQL = SQL + ComNum.VBLF + "        TO_DATE('" + strDate + "','YYYY-MM-DD'),      GBGROUP,   ";
                SQL = SQL + ComNum.VBLF + "        GBPORT,      'DC1' ,MULTI, MULTIREMARK , DUR, '*', '" + Convert.ToInt32(clsType.User.Sabun) + "', SYSDATE, MAYAK, POWDER,GBIOE,'" + clsCompuInfo.gstrCOMIP + "','N1', ";

                //      '2015-08-10
                if (mstrBun == "11" || mstrBun == "12" || mstrBun == "20")
                {
                    if (mstrNurseID == "")
                    {
                        SQL = SQL + "        'N', ";
                    }
                    else
                    {
                        SQL = SQL + "        'Y', ";
                    }
                }
                else
                {
                    SQL = SQL + "        'N', ";
                }

                SQL = SQL + "        VERBAL,SYSDATE   ";
                SQL = SQL + " FROM  KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + " WHERE  ROWID = '" + mstrROWID + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    return RtnVal;
                }
            }

            SQL = "";
            SQL = "INSERT INTO ";
            SQL = SQL + ComNum.VBLF + "   KOSMOS_OCS.OCS_IORDER  ( PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, ";
            SQL = SQL + ComNum.VBLF + "GBORDER, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN,";
            SQL = SQL + ComNum.VBLF + " GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, ACCSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO,";
            SQL = SQL + ComNum.VBLF + " REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, DUR, GBPICKUP, PICKUPSABUN, PICKUPDATE, MAYAK,";
            SQL = SQL + ComNum.VBLF + " POWDER,GBIOE,IP,GBCHK,GBVERB,VERBAL,ENTDATE2 ) ";
            SQL = SQL + ComNum.VBLF + "(SELECT PTNO, BDATE, SEQNO,  DEPTCODE, DRCODE,  STAFFID,  SLIPNO,    ";
            if (mdblReadQty > 0)
            {
                SQL = SQL + ComNum.VBLF + "        ORDERCODE,   SUCODE, BUN,      GBORDER, BCONTENTS, BCONTENTS, '" + mdblReadQty.ToString("###0.##") + "', '" + mdblSQty.ToString("###0.##") + "',"; //'간호사 반환은 무조건 수량으로 반환 됩니다.;
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "        ORDERCODE,   SUCODE, BUN,      GBORDER, BCONTENTS, BCONTENTS, '" + mdblSQty.ToString("###0.##") + "', '" + mdblSQty.ToString("###0.##") + "',"; //'간호사 반환은 무조건 수량으로 반환 됩니다.;
            }
            SQL = SQL + ComNum.VBLF + "        REALNAL ,      NAL , DOSCODE,   ";
            SQL = SQL + ComNum.VBLF + "        GBINFO,      GBSELF, GBSPC,    GBNGT,   GBER,     GBPRN,     ";

            //'2012-03-14

            if (mstrBun == "11")
            {
                SQL = SQL + "        GBDIV,    ";
            }
            else
            {
                SQL = SQL + "        GBDIV,    ";
            }

            SQL = SQL + "        GBBOTH, GBACT,    GBTFLAG, ";

            if (mstrBun == "11" || mstrBun == "12" || mstrBun == "20" || mstrBun == "21" || mstrBun == "23") //'주사,약 SEND 해줌
            {


                //2019-04-18 소수점이 포함 될 경우 오작동함. 보완함. 테스트 중!
                if (mstrDQty.Contains(decSeparator) && VB.Val(mstrDQty) < 1)        
                {
                    dblQty = VB.Val(mstrQty) - (VB.Val(mstrDQty) * mdblCheckQty);
                }
                else
                {
                    dblQty = VB.Val(mstrQty) - VB.Val(mstrDQty);
                }
                //if (VB.Val(mstrQty) - VB.Val(mstrDQty) <= 0 )
                //{
                //    SQL = SQL + "  ' ', ";  //GBSEND
                //    SQL = SQL + "  'Y', ";  //ACCSEND
                //}
                if (dblQty <= 0)
                { 
                    SQL = SQL + "  ' ', ";  //GBSEND
                    SQL = SQL + "  'Y', ";  //ACCSEND
                }
                else
                {
                    SQL = SQL + "  '*', ";  //GBSEND
                    if (mdblSQty == 0)  //2019-06-17 QTY = 0 이면 ACCSEND 는 Y 처리
                    {
                        SQL = SQL + "  'Y',  ";  //ACCSEND
                    }
                    else
                    { 
                        SQL = SQL + "  '',  ";  //ACCSEND
                    }
                }
            }
            else
            {
                if (mstrORDERCODE == "C3710" || mstrORDERCODE == "B2522" || mstrORDERCODE == "B2521" || mstrORDERCODE == "A27" || mstrORDERCODE == "A26") //'BST 는전송함
                {
                    SQL = SQL + "  '*',";
                    SQL = SQL + "  '',  ";  //ACCSEND
                }
                else
                {
                    SQL = SQL + "  ' ',"; //'약주사가 아니면 SEND 않해줌;
                    SQL = SQL + "  'Y', ";  //ACCSEND
                }
            }

            SQL = SQL + "        GBPOSITION, 'D+', ";
            SQL = SQL + " '" + Convert.ToInt32(clsType.User.Sabun) + "', ";
            SQL = SQL + "        SYSDATE,     WARDCODE,ROOMCODE, BI,     ORDERNO,  REMARK,    ";
            SQL = SQL + "        TO_DATE('" + strDate + "','YYYY-MM-DD'),      GBGROUP,   ";

            if (mbolD == false)
            {
                SQL = SQL + "        GBPORT,     'DC1', MULTI, MULTIREMARK, DUR ";  //'의사가취소않한경우;
            }
            else
            {
                SQL = SQL + "        GBPORT,     'DC0', MULTI, MULTIREMARK, DUR ";  //'의사가 취소한경우;
            }

            SQL = SQL + "  , '*', '" + Convert.ToInt32(clsType.User.Sabun) + "', SYSDATE , MAYAK, POWDER,GBIOE,'" + clsCompuInfo.gstrCOMIP + "','N2',";

            //'2015-08-10

            if (mstrBun == "11" || mstrBun == "12" || mstrBun == "20")
            {
                if (mstrNurseID == "")
                {
                    SQL = SQL + "        'N', ";
                }
                else
                {
                    SQL = SQL + "        'Y', ";
                }
            }
            else
            {
                SQL = SQL + "        'N', ";
            }

            SQL = SQL + "  VERBAL,SYSDATE ";
            SQL = SQL + " FROM  KOSMOS_OCS.OCS_IORDER  ";
            SQL = SQL + " WHERE  ROWID = '" + mstrROWID + "') ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                return RtnVal;
            }

            return true;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime SysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            ssLine1_Sheet1.Columns[15].Visible = false;
            ssLineP1_Sheet1.Columns[12].Visible = false;

            if (tabControl1.SelectedIndex == 1)
            {
                //깔끔한 락 관리 때문에 NDC 상태 해제
                if (optGB1.Checked == true)
                {
                    optGB0.Checked = true;
                }

                FstrLineGubun = "중심정맥관";
                SSLINE1_CLEAR(); 
                btnSreachKeepL1_Click(null, null);
                btnSreachL1_Click(null, null);
                ssLine1_Sheet1.Columns[15].Visible = clsEmrQuery.ChartOrder_Exists(this, pAcp, SysDate.ToString("yyyyMMdd"), FstrLineGubun, "'삽입', '유지', '제거'");
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                //깔끔한 락 관리 때문에 NDC 상태 해제
                if (optGB1.Checked == true)
                {
                    optGB0.Checked = true;
                }

                FstrLineGubun = "말초정맥관";
                SSLINEP1_CLEAR();
                btnSreachKeepLP1_Click(null, null);
                btnSreachLP1_Click(null, null);
                ssLineP1_Sheet1.Columns[12].Visible = clsEmrQuery.ChartOrder_Exists(this, pAcp, SysDate.ToString("yyyyMMdd"), FstrLineGubun, "'삽입', '유지'");
            }
            else
            {
                FstrLineGubun = "";
            }
        }

        private void btnSreachKeepL1_Click(object sender, EventArgs e)
        {
            string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strStartDate = "";
            string strACTDATE = "";

            ssLine2_Sheet1.RowCount = 2;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            FbBtnClick = true;
            lblLineAct.Visible = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(ACTDATE, 'YYYY-MM-DD HH24:MI') ACTDATE, PART1 || ' ' || PART2 PART, ";
                SQL = SQL + ComNum.VBLF + " LOCATE1 || ' ' || LOCATE2 LOCATE, ";
                SQL = SQL + ComNum.VBLF + " MONITOR1, MONITOR2, MONITOR3, MONITOR4, MONITOR5, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(D_DATE, 'YYYY-MM-DD HH24:MI') D_DATE, ";
                SQL = SQL + ComNum.VBLF + " D_PART, D_STATUS1, D_STATUS2, ACTSABUN, SEQNO, ";
                SQL = SQL + ComNum.VBLF + " ROWID, EMRNO, STATUS, DUTY, MONITOR6," ;
                SQL = SQL + ComNum.VBLF + " HAND_HYGN, HERB_STRL, ASEPTIC_TECH, DRS_NAME, NEED_ASSES ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL MST";
                SQL = SQL + ComNum.VBLF + "  WHERE  EXISTS (";
                SQL = SQL + ComNum.VBLF + "  SELECT ACTDATE, SEQNO FROM (";
                SQL = SQL + ComNum.VBLF + "  SELECT MAX(ACTDATE) ACTDATE, SEQNO";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL";
                SQL = SQL + ComNum.VBLF + "  GROUP BY SEQNO ) SUB";
                SQL = SQL + ComNum.VBLF + "  WHERE MST.ACTDATE = SUB.ACTDATE";
                SQL = SQL + ComNum.VBLF + "       AND MST.SEQNO = SUB.SEQNO)";
                if (cboWard.Text.Trim() == "33")
                {
                    SQL = SQL + ComNum.VBLF + "       AND (IPDNO = " + mintIPDNO + "OR IPDNO = 0)";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + mintIPDNO;
                }
                SQL = SQL + ComNum.VBLF + "       AND PANO = '" + FstrPtno + "'";
                SQL = SQL + ComNum.VBLF + "       AND STATUS IN ('삽입','유지')";
                SQL = SQL + ComNum.VBLF + "  ORDER BY PART1, PART2, LOCATE1, LOCATE2, ACTDATE";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblLineAct.Visible = true;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssLine2_Sheet1.RowCount = ssLine2_Sheet1.RowCount + 1;

                        DRAW_SSLINE2(ssLine2_Sheet1.RowCount - 1);

                        strACTDATE = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.NUMBER)].Text = Convert.ToString(i + 1);
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.STATE)].Text = dt.Rows[i]["STATUS"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.CHECK_DATETIME)].Text = Convert.ToDateTime(strACTDATE).ToString("MM/dd HH:mm").Replace("-", "/");
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.INSERT_LOCATE)].Text = dt.Rows[i]["LOCATE"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.HAND_HYGN)].Text = dt.Rows[i]["HAND_HYGN"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.HERB_STRL)].Text = dt.Rows[i]["HERB_STRL"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.ASEPTIC_TECH)].Text = dt.Rows[i]["ASEPTIC_TECH"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.DRS_NAME)].Text = dt.Rows[i]["DRS_NAME"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.NEED_ASSES)].Text = dt.Rows[i]["NEED_ASSES"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.OBSERVE_1)].Text = dt.Rows[i]["MONITOR1"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.OBSERVE_2)].Text = dt.Rows[i]["MONITOR2"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.OBSERVE_3)].Text = dt.Rows[i]["MONITOR3"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.OBSERVE_4)].Text = dt.Rows[i]["MONITOR4"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.OBSERVE_5)].Text = dt.Rows[i]["MONITOR5"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.DRS_DATE)].Text = VB.Left(dt.Rows[i]["D_DATE"].ToString().Trim(), 10);
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.DRS_TIME)].Text = VB.Right(dt.Rows[i]["D_DATE"].ToString().Trim(), 5);
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.DRS_KIND)].Text = dt.Rows[i]["D_PART"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.DRS_STATE1)].Text = dt.Rows[i]["D_STATUS1"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.DRS_STATE2)].Text = dt.Rows[i]["D_STATUS2"].ToString().Trim();
                        strStartDate = READ_STARTDATE(dt.Rows[i]["SEQNO"].ToString().Trim());

                        if (strStartDate == "")
                        {
                            strStartDate = strACTDATE;
                        }

                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.INSERT_DATETIME)].Text = Convert.ToDateTime(strStartDate).ToString("MM/dd HH:mm").Replace("-", "/");
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.KEEP_DAY)].Text = Convert.ToString(VB.DateDiff("d", Convert.ToDateTime(VB.Left(strStartDate, 10)), Convert.ToDateTime(strDate)) + 1);
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.ROWID)].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.SEQNO)].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.EMRNO)].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                        ssLine2_Sheet1.Cells[ssLine2_Sheet1.RowCount - 1, (int)(enumColumn_LineAct.CATH_TYPE)].Text = dt.Rows[i]["PART"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

            FbBtnClick = false;
        }

        private string READ_STARTDATE(string ArgSeqno)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD HH24:MI') ACTDATE ";

                if (FstrLineGubun == "말초정맥관")
                {
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL ";
                }

                SQL = SQL + ComNum.VBLF + " WHERE SEQNO = " + ArgSeqno;
                SQL = SQL + ComNum.VBLF + "    AND STATUS = '삽입' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["ACTDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (rtnVal == "")
                {
                    SQL = "";
                    SQL = " SELECT MIN(TO_CHAR(ACTDATE,'YYYY-MM-DD HH24:MI')) ACTDATE ";

                    if (FstrLineGubun == "말초정맥관")
                    {
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL ";
                    }

                    SQL = SQL + ComNum.VBLF + " WHERE SEQNO = " + ArgSeqno;

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        rtnVal = dt.Rows[0]["ACTDATE"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
                Cursor.Current = Cursors.Default;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private void DRAW_SSLINE2(int intRow)
        {
            int i = 0;

            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.NUMBER)].CellType = CellText;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.NUMBER)].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.NUMBER)].VerticalAlignment = CellVerticalAlignment.Center;

            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.NUMBER)].Text = Convert.ToString(intRow - 1);

            for (i = 1; i < 4; i++)
            {
                ssLine2_Sheet1.Cells[intRow, i].CellType = CellText;
                ssLine2_Sheet1.Cells[intRow, i].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssLine2_Sheet1.Cells[intRow, i].VerticalAlignment = CellVerticalAlignment.Center;
            }

            CellCbo.Editable = true;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.HAND_HYGN)].CellType = CellCbo;
            clsSpread.gSpreadComboDataSetEx(ssLine2, intRow, (int)(enumColumn_LineAct.HAND_HYGN), intRow, (int)(enumColumn_LineAct.HAND_HYGN), VB.Split("수행//미수행", "//"));
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.HAND_HYGN)].HorizontalAlignment = CellHorizontalAlignment.Left;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.HAND_HYGN)].VerticalAlignment = CellVerticalAlignment.Center;

            CellCbo.Editable = true;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.HERB_STRL)].CellType = CellCbo;
            clsSpread.gSpreadComboDataSetEx(ssLine2, intRow, (int)(enumColumn_LineAct.HERB_STRL), intRow, (int)(enumColumn_LineAct.HERB_STRL), VB.Split("15초이상//미수행", "//"));
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.HERB_STRL)].HorizontalAlignment = CellHorizontalAlignment.Left;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.HERB_STRL)].VerticalAlignment = CellVerticalAlignment.Center;

            CellCbo.Editable = true;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.ASEPTIC_TECH)].CellType = CellCbo;
            clsSpread.gSpreadComboDataSetEx(ssLine2, intRow, (int)(enumColumn_LineAct.ASEPTIC_TECH), intRow, (int)(enumColumn_LineAct.ASEPTIC_TECH), VB.Split("적용//미적용", "//"));
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.ASEPTIC_TECH)].HorizontalAlignment = CellHorizontalAlignment.Left;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.ASEPTIC_TECH)].VerticalAlignment = CellVerticalAlignment.Center;

            for (i = 7; i < 12; i++)
            {
                CellChk.Caption = "";
                ssLine2_Sheet1.Cells[intRow, i].CellType = CellChk;
                ssLine2_Sheet1.Cells[intRow, i].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssLine2_Sheet1.Cells[intRow, i].VerticalAlignment = CellVerticalAlignment.Center;
            }

            CellDate.DateTimeFormat = DateTimeFormat.UserDefined;
            CellDate.UserDefinedFormat = "yyyy-MM-dd";
            CellDate.MaximumDate = Convert.ToDateTime("9998-12-31");
            CellDate.MinimumDate = Convert.ToDateTime("1900-01-01");
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_DATE)].CellType = CellDate;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_DATE)].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_DATE)].VerticalAlignment = CellVerticalAlignment.Center;
            
            CellTime.DateTimeFormat = DateTimeFormat.UserDefined;
            CellTime.UserDefinedFormat = "HH:mm";
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_TIME)].CellType = CellTime;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_TIME)].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_TIME)].VerticalAlignment = CellVerticalAlignment.Center;
            
            CellCbo.Editable = true;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_KIND)].CellType = CellCbo;
            clsSpread.gSpreadComboDataSetEx(ssLine2, intRow, (int)(enumColumn_LineAct.DRS_KIND), intRow, (int)(enumColumn_LineAct.DRS_KIND), VB.Split("투명필름//멸균거즈//멸균거즈+투명필름", "//"));
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_KIND)].HorizontalAlignment = CellHorizontalAlignment.Left;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_KIND)].VerticalAlignment = CellVerticalAlignment.Center;

            CellCbo.Editable = true;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_NAME)].CellType = CellCbo;
            clsSpread.gSpreadComboDataSetEx(ssLine2, intRow, (int)(enumColumn_LineAct.DRS_NAME), intRow, (int)(enumColumn_LineAct.DRS_NAME), VB.Split("2%클로르헥시딘//기타", "//"));
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_NAME)].HorizontalAlignment = CellHorizontalAlignment.Left;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_NAME)].VerticalAlignment = CellVerticalAlignment.Center;

            CellCbo.Editable = true;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_STATE1)].CellType = CellCbo;
            clsSpread.gSpreadComboDataSetEx(ssLine2, intRow, (int)(enumColumn_LineAct.DRS_STATE1), intRow, (int)(enumColumn_LineAct.DRS_STATE1), VB.Split("Y//N", "//"));
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_STATE1)].HorizontalAlignment = CellHorizontalAlignment.Left;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_STATE1)].VerticalAlignment = CellVerticalAlignment.Center;

            CellCbo.Editable = true;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_STATE2)].CellType = CellCbo;
            clsSpread.gSpreadComboDataSetEx(ssLine2, intRow, (int)(enumColumn_LineAct.DRS_STATE2), intRow, (int)(enumColumn_LineAct.DRS_STATE2), VB.Split("양호//불량", "//"));
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_STATE2)].HorizontalAlignment = CellHorizontalAlignment.Left;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_STATE2)].VerticalAlignment = CellVerticalAlignment.Center;

            CellCbo.Editable = true;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.NEED_ASSES)].CellType = CellCbo;
            clsSpread.gSpreadComboDataSetEx(ssLine2, intRow, (int)(enumColumn_LineAct.NEED_ASSES), intRow, (int)(enumColumn_LineAct.NEED_ASSES), VB.Split("혈관확보//전신부종//열역학모니터링//기타:", "//"));
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.NEED_ASSES)].HorizontalAlignment = CellHorizontalAlignment.Left;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.NEED_ASSES)].VerticalAlignment = CellVerticalAlignment.Center;

            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.INSERT_DATETIME)].CellType = CellText;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.INSERT_DATETIME)].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.INSERT_DATETIME)].VerticalAlignment = CellVerticalAlignment.Center;

            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.KEEP_DAY)].CellType = CellText;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.KEEP_DAY)].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.KEEP_DAY)].VerticalAlignment = CellVerticalAlignment.Center;

            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.ACT_KEEP)].CellType = CellBtn1;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.ACT_REMOVE)].CellType = CellBtn2;

            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.ROWID)].CellType = CellText;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.ROWID)].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.ROWID)].VerticalAlignment = CellVerticalAlignment.Center;

            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.SEQNO)].CellType = CellText;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.SEQNO)].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.SEQNO)].VerticalAlignment = CellVerticalAlignment.Center;

            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.ACT_DEL)].CellType = CellBtn3;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.ACT_UPDATE)].CellType = CellBtn4;

            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.EMRNO)].CellType = CellText;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.EMRNO)].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.EMRNO)].VerticalAlignment = CellVerticalAlignment.Center;

            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.CATH_TYPE)].CellType = CellText;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.CATH_TYPE)].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.CATH_TYPE)].VerticalAlignment = CellVerticalAlignment.Center;

            ssLine2_Sheet1.SetRowHeight(-1, 30);
            //SSLine2.FontBold = True
        }

        private void btnSreachKeepLP1_Click(object sender, EventArgs e)
        {
            string strStartDate = "";
            string strACTDATE = "";
            string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            ssLineP2_Sheet1.RowCount = 2;

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            FbBtnClick = true;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(ACTDATE, 'YYYY-MM-DD HH24:MI') ACTDATE, PART1 || ' ' || PART2 PART, ";
                SQL = SQL + ComNum.VBLF + " LOCATE1 || ' ' || LOCATE2 LOCATE, ";
                SQL = SQL + ComNum.VBLF + " MONITOR1, MONITOR2, MONITOR3, MONITOR4, MONITOR5, MONITOR6, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(D_DATE, 'YYYY-MM-DD HH24:MI') D_DATE, ";
                SQL = SQL + ComNum.VBLF + " D_PART, D_STATUS1, D_STATUS2, ACTSABUN, SEQNO, ";
                SQL = SQL + ComNum.VBLF + " ROWID, EMRNO, STATUS, DUTY, BIGO";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL MST";
                SQL = SQL + ComNum.VBLF + "  WHERE  EXISTS (";
                SQL = SQL + ComNum.VBLF + "  SELECT ACTDATE, SEQNO FROM (";
                SQL = SQL + ComNum.VBLF + "  SELECT MAX(ACTDATE) ACTDATE, SEQNO";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL";
                SQL = SQL + ComNum.VBLF + "  GROUP BY SEQNO ) SUB";
                SQL = SQL + ComNum.VBLF + "  WHERE MST.ACTDATE = SUB.ACTDATE";
                SQL = SQL + ComNum.VBLF + "       AND MST.SEQNO = SUB.SEQNO)";
                SQL = SQL + ComNum.VBLF + "       AND ( IPDNO = " + mintIPDNO + " OR IPDNO = 0 ) ";
                SQL = SQL + ComNum.VBLF + "       AND ACTDATE >= TRUNC(SYSDATE-3) ";
                SQL = SQL + ComNum.VBLF + "       AND PANO = '" + FstrPtno + "'";
                SQL = SQL + ComNum.VBLF + "       AND STATUS IN ('삽입','유지')";
                SQL = SQL + ComNum.VBLF + "  ORDER BY PART1, PART2, LOCATE1, LOCATE2, ACTDATE";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssLineP2_Sheet1.RowCount = ssLineP2_Sheet1.RowCount + 1;

                        DRAW_SSLINEP2(ssLineP2_Sheet1.RowCount - 1);

                        strACTDATE = dt.Rows[i]["ACTDATE"].ToString().Trim();

                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 0].Text = Convert.ToString(i + 1);

                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["STATUS"].ToString().Trim();
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 2].Text = Convert.ToDateTime(strACTDATE).ToString("MM-dd HH:mm").Replace("-", "/");
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["DUTY"].ToString().Trim();
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["PART"].ToString().Trim();
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["LOCATE"].ToString().Trim();
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["MONITOR1"].ToString().Trim();
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["MONITOR2"].ToString().Trim();
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["MONITOR3"].ToString().Trim();
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["MONITOR4"].ToString().Trim();
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["MONITOR6"].ToString().Trim();
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["MONITOR5"].ToString().Trim();
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["BIGO"].ToString().Trim();

                        strStartDate = READ_STARTDATE(dt.Rows[i]["SEQNO"].ToString().Trim());

                        if (strStartDate == "")
                        {
                            strStartDate = strACTDATE;
                        }

                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 12 + 1].Text = Convert.ToDateTime(strStartDate).ToString("MM/dd HH:mm").Replace("-", "/");
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 13 + 1].Text = Convert.ToString(VB.DateDiff("d", Convert.ToDateTime(VB.Left(strStartDate, 10)), Convert.ToDateTime(strDate)) + 1);
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 16 + 1].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 17 + 1].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssLineP2_Sheet1.Cells[ssLineP2_Sheet1.RowCount - 1, 20 + 1].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

            FbBtnClick = false;
        }

        private void DRAW_SSLINEP2(int intRow)
        {
            int i = 0;

            ssLineP2_Sheet1.Cells[intRow, 0].CellType = CellText;
            ssLineP2_Sheet1.Cells[intRow, 0].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssLineP2_Sheet1.Cells[intRow, 0].VerticalAlignment = CellVerticalAlignment.Center;
            ssLineP2_Sheet1.Cells[intRow, 0].Text = Convert.ToString(intRow - 1);

            for (i = 1; i < 5; i++)
            {
                ssLineP2_Sheet1.Cells[intRow, i].CellType = CellText;
                ssLineP2_Sheet1.Cells[intRow, i].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssLineP2_Sheet1.Cells[intRow, i].VerticalAlignment = CellVerticalAlignment.Center;
                ssLineP2_Sheet1.Cells[intRow, i].Text = Convert.ToString(intRow - 1);
            }

            ssLineP2_Sheet1.Cells[intRow, 5].CellType = CellText;
            ssLineP2_Sheet1.Cells[intRow, 5].HorizontalAlignment = CellHorizontalAlignment.Left;
            ssLineP2_Sheet1.Cells[intRow, 5].VerticalAlignment = CellVerticalAlignment.Center;
            ssLineP2_Sheet1.Cells[intRow, 5].Text = Convert.ToString(intRow - 1);

            for (i = 6; i < 12; i++)
            {
                CellChk.Caption = "";
                ssLineP2_Sheet1.Cells[intRow, i].CellType = CellChk;
                ssLineP2_Sheet1.Cells[intRow, i].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssLineP2_Sheet1.Cells[intRow, i].VerticalAlignment = CellVerticalAlignment.Center;
            }

            CellCbo.Editable = true;
            ssLineP2_Sheet1.Cells[intRow, 12].CellType = CellCbo;
            //2021.11.22 간호부 고경자 팀장요청 한글로 "헤파린 캡" 문구추가요청
            //clsSpread.gSpreadComboDataSetEx(ssLineP2, intRow, 12, intRow, 12, VB.Split(" //A-line", "//"));
            clsSpread.gSpreadComboDataSetEx(ssLineP2, intRow, 12, intRow, 12, VB.Split(" //A-line//헤파린 캡", "//"));
            ssLineP2_Sheet1.Cells[intRow, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
            ssLineP2_Sheet1.Cells[intRow, 12].VerticalAlignment = CellVerticalAlignment.Center;

            //ssLineP2_Sheet1.Cells[intRow, 12].CellType = CellText;
            //ssLineP2_Sheet1.Cells[intRow, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
            //ssLineP2_Sheet1.Cells[intRow, 12].VerticalAlignment = CellVerticalAlignment.Center;
            //ssLineP2_Sheet1.Cells[intRow, 12].Text = Convert.ToString(intRow - 1);



            ssLineP2_Sheet1.Cells[intRow, 12 + 1].CellType = CellText;
            ssLineP2_Sheet1.Cells[intRow, 12 + 1].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssLineP2_Sheet1.Cells[intRow, 12 + 1].VerticalAlignment = CellVerticalAlignment.Center;

            ssLineP2_Sheet1.Cells[intRow, 13 + 1].CellType = CellText;
            ssLineP2_Sheet1.Cells[intRow, 13 + 1].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssLineP2_Sheet1.Cells[intRow, 13 + 1].VerticalAlignment = CellVerticalAlignment.Center;

            ssLineP2_Sheet1.Cells[intRow, 14 + 1].CellType = CellBtn1;
            ssLineP2_Sheet1.Cells[intRow, 15 + 1].CellType = CellBtn2;
            ssLineP2_Sheet1.Cells[intRow, 18 + 1].CellType = CellBtn3;
            ssLineP2_Sheet1.Cells[intRow, 19 + 1].CellType = CellBtn4;

            ssLineP2_Sheet1.Cells[intRow, 20 + 1].CellType = CellText;
            ssLineP2_Sheet1.Cells[intRow, 20 + 1].HorizontalAlignment = CellHorizontalAlignment.Left;
            ssLineP2_Sheet1.Cells[intRow, 20 + 1].VerticalAlignment = CellVerticalAlignment.Center;

            ssLineP2_Sheet1.SetRowHeight(intRow, 30);
            //SSLineP2.FontBold = True
        }

        private void btnSreachL1_Click(object sender, EventArgs e)
        {
            READ_LINE1_HISTORY("");
        }

        private void btnSreachLP1_Click(object sender, EventArgs e)
        {
            READ_LINE1P_HISTORY("");
        }

        private void ssLine1_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if (FbBtnClick == true)
            {
                return;
            }

            if (ssLine1_Sheet1.RowCount == 0)
            {
                return;
            }

            FbBtnClick = false;
            ClearLine1Act();

            if (e.Column == 14 && e.Row == 1)
            {
            }
            else if (e.Column == 15 && e.Row == 0)
            {
                //당일 처방내역 버튼
                DateTime SysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                using (frmSugaOrderSave frmSugaOrderSaveX = new frmSugaOrderSave(SysDate.ToString("yyyy-MM-dd"), "2638", "중심정맥관", "전체", "중심정맥관", pAcp, SysDate.ToString("yyyyMMdd"), null, -1))
                {
                    frmSugaOrderSaveX.StartPosition = FormStartPosition.CenterScreen;
                    frmSugaOrderSaveX.ShowDialog(this);
                }
                ssLine1_Sheet1.Columns[15].Visible = clsEmrQuery.ChartOrder_Exists(this, pAcp, SysDate.ToString("yyyyMMdd"), "중심정맥관", "'삽입', '유지'");
            }
            else
            {
                if (e.Row == 2)
                {
                    switch (e.Column)
                    {
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                            if (e.Column == 8)
                            {
                                FbBtnClick = true;

                                ssLine1_Sheet1.Cells[e.Row, 4].Value = false;
                                ssLine1_Sheet1.Cells[e.Row, 5].Value = false;
                                ssLine1_Sheet1.Cells[e.Row, 6].Value = false;
                                ssLine1_Sheet1.Cells[e.Row, 7].Value = false;

                                FbBtnClick = false;
                            }
                            else
                            {
                                FbBtnClick = true;

                                ssLine1_Sheet1.Cells[e.Row, 8].Value = false;

                                FbBtnClick = false;
                            }
                            break;
                    }
                }
                return;
            }

            if (FstrPtno == "")
            {
                ComFunc.MsgBox("환자를 선택 후 액팅하시기 바랍니다.", "확인");
                return;
            }               

            Line1Act.IPDNO = Convert.ToString(mintIPDNO);
            Line1Act.Pano = FstrPtno;
            Line1Act.WardCode = cboWard.Text.Trim();
            Line1Act.RoomCode = FstrRoom;
            Line1Act.DeptCode = FstrDEPT;
            Line1Act.DrCode = FstrDrCode;
            Line1Act.InDate = FstrInDate;
            if (chkLineChartDate.Checked == true)
            {
                Line1Act.ACTDATE = txtLine1Date.Text.Trim();
                Line1Act.ACTTIME = txtLine1Time.Text.Trim();
            }
            else
            {
                Line1Act.ACTDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                Line1Act.ACTTIME = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
            }

            Line1Act.STARTDATE = Line1Act.ACTDATE;
            Line1Act.STARTTIME = Line1Act.ACTTIME;
            Line1Act.ACTSABUN = clsType.User.Sabun;
            Line1Act.status = "삽입";

            if (e.Column == 14 && e.Row == 1)
            {
                panBeginBuse.Visible = true;
                optBuse1.Checked = true;
                txtBuseEtc.Text = "";
            }
        }

        private void FrmEmrChartNewX_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrChartNewX != null)
            {
                frmEmrChartNewX.Dispose();
                frmEmrChartNewX = null;
            }
        }

        private void ssLine1_ComboCloseUp(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column == 0 && e.Row == 2)
            {
                switch (ssLine1_Sheet1.Cells[e.Row, e.Column].Text.Trim())
                {
                    case "비터널식":
                        ssLine1_Sheet1.Cells[e.Row, 1].Text = "";
                        clsSpread.gSpreadComboDataSetEx(ssLine1, e.Row, 1, e.Row, 1, VB.Split("CVC(non HD)//PICC//Temp, HD cath//Swan-Ganz//chemoport", "//"));
                        break;

                    case "터널식":
                        ssLine1_Sheet1.Cells[e.Row, 1].Text = "";
                        clsSpread.gSpreadComboDataSetEx(ssLine1, e.Row, 1, e.Row, 1, VB.Split("Hicman//케모포트//Perm.HD cath", "//"));
                        break;
                }
            }

            if (e.Column == 1 && e.Row == 3)
            {
                if (ssLine1_Sheet1.Cells[e.Row, e.Column].Text.Trim() == "PICC")
                {
                    ssLine1_Sheet1.Cells[e.Row, 3].Text = "cubital fossa";
                }
                else
                {
                    ssLine1_Sheet1.Cells[e.Row, 3].Text = "";
                }
            }
        }

        private void chkLineChartDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLineChartDate.Checked == true)
            {
                txtLine1Date.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                txtLine1Time.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
                txtLine1Date.Visible = true;
                txtLine1Time.Visible = true;
            }
            else
            {
                txtLine1Date.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                txtLine1Time.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
                txtLine1Date.Visible = false;
                txtLine1Time.Visible = false;
            }
        }

        private void chkLineChartDate2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLineChartDate2.Checked == true)
            {
                txtLine2Date.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                txtLine2Time.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
                txtLine2Date.Visible = true;
                txtLine2Time.Visible = true;
            }
            else
            {
                txtLine2Date.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                txtLine2Time.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
                txtLine2Date.Visible = false;
                txtLine2Time.Visible = false;
            }
        }

        private void ssLine2_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            string strROWID = "";

            ClearLine1Act();

            if (e.Row < 2)
            {
                return;
            }

            if (FbBtnClick == true)
            {
                return;
            }

            switch (e.Column)
            {
                case (int)(enumColumn_LineAct.OBSERVE_1):
                case (int)(enumColumn_LineAct.OBSERVE_2):
                case (int)(enumColumn_LineAct.OBSERVE_3):
                case (int)(enumColumn_LineAct.OBSERVE_4):
                case (int)(enumColumn_LineAct.OBSERVE_5):
                    if (e.Column == (int)(enumColumn_LineAct.OBSERVE_5))
                    {
                        FbBtnClick = true;

                        ssLine2_Sheet1.Cells[e.Row, (int)(enumColumn_LineAct.OBSERVE_1)].Value = false;
                        ssLine2_Sheet1.Cells[e.Row, (int)(enumColumn_LineAct.OBSERVE_2)].Value = false;
                        ssLine2_Sheet1.Cells[e.Row, (int)(enumColumn_LineAct.OBSERVE_3)].Value = false;
                        ssLine2_Sheet1.Cells[e.Row, (int)(enumColumn_LineAct.OBSERVE_4)].Value = false;

                        FbBtnClick = false;
                    }
                    else
                    {
                        FbBtnClick = true;

                        ssLine2_Sheet1.Cells[e.Row, (int)(enumColumn_LineAct.OBSERVE_5)].Value = false;

                        FbBtnClick = false;
                    }
                    break;
            }

            if (e.Column == (int)(enumColumn_LineAct.ACT_DEL))
            {
                if (ComFunc.MsgBoxQ("삭제 후 복구는 불가능합니다. 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }

                strROWID = ssLine2_Sheet1.Cells[e.Row, (int)(enumColumn_LineAct.ROWID)].Text.Trim();

                DelLineAct1(strROWID);

                SSLINE1_CLEAR();
                btnSreachKeepL1_Click(null, null);
                btnSreachL1_Click(null, null);

                return;
            }

            switch (e.Column)
            {
                case (int)(enumColumn_LineAct.ACT_KEEP):
                    Line1Act.status = "유지";
                    break;

                case (int)(enumColumn_LineAct.ACT_REMOVE):
                    Line1Act.status = "제거";
                    break;

                default:
                    return;
            }

            strROWID = ssLine2_Sheet1.Cells[e.Row, (int)(enumColumn_LineAct.ROWID)].Text.Trim();
            Line1Act.SEQNO = ssLine2_Sheet1.Cells[e.Row, (int)(enumColumn_LineAct.SEQNO)].Text.Trim();

            Line1Act.IPDNO = Convert.ToString(mintIPDNO);
            Line1Act.Pano = FstrPtno;
            Line1Act.WardCode = cboWard.Text.Trim();
            Line1Act.RoomCode = FstrRoom;
            Line1Act.DeptCode = FstrDEPT;
            Line1Act.DrCode = FstrDrCode;
            Line1Act.InDate = FstrInDate;

            if (chkLineChartDate2.Checked == true)
            {
                Line1Act.ACTDATE = txtLine2Date.Text.Trim();
                Line1Act.ACTTIME = txtLine2Time.Text.Trim();
            }
            else
            {
                Line1Act.ACTDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                Line1Act.ACTTIME = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
            }

            Line1Act.ACTSABUN = clsType.User.Sabun;

            SetSSLine2(strROWID, e.Row);

            if (Line1Act.D_DATE == "" || Line1Act.D_TIME == "")
            {
                ComFunc.MsgBox("드래싱 시행일시를 입력하십시요.", "확인");
                return;
            }

            #region 수가 연동 드레싱이 서버날짜랑 같을때만
            DateTime DrsDate = Convert.ToDateTime(ssLine2_Sheet1.Cells[e.Row, (int)enumColumn_LineAct.DRS_DATE].Text);
            DateTime SysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            if ((Line1Act.status == "유지" || Line1Act.status == "제거") && DrsDate.Date == SysDate.Date &&
                FormPatInfoFunc.Set_FormPatInfo_ItemSugaMaaping(clsDB.DbCon, "2638", pAcp.ward, "중심정맥관", Line1Act.status))
            {
                using (frmSugaOrderSave frm = new frmSugaOrderSave(DrsDate.Date.ToString("yyyy-MM-dd"), "2638", "중심정맥관", Line1Act.status, "중심정맥관", pAcp, SysDate.ToString("yyyyMMdd"), null, -1))
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog(this);
                }

                ssLine1_Sheet1.Columns[15].Visible = clsEmrQuery.ChartOrder_Exists(this, pAcp, SysDate.ToString("yyyyMMdd"), "중심정맥관", Line1Act.status);
            }
            #endregion

            INSERT_LINE1();

            SSLINE1_CLEAR();
            btnSreachKeepL1_Click(null, null);
            btnSreachL1_Click(null, null);
        }

        private void INSERT_LINE1()
        {
            string strMon1 = "";
            string strMon2 = "";
            string strMon3 = "";
            string strMon4 = "";
            string strMon5 = "";
            string strMon6 = "";

            string strDMon1 = "";
            string strDMon2 = "";

            string strActTime = "";

            string strEMRNO = "";

            string strData = "";

            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            //TODO : 전자인증...ㄷㄷ
            //If GnJobSabun = 4349 Or GstrJobGrade = "EDPS" Then
            //Else
            //    If "NO" = CERT_CHECK(GnJobSabun) Then
            //        MsgBox "전자인증이 없습니다. 확인하시기 바랍니다.", vbInformation, "확인"
            //        Exit Function
            //    End If
            //End If

            if (FstrLineGubun == "")
            {
                ComFunc.MsgBox("정상적인 접근이 아닙니다. 전산정보과로 문의하시기 바랍니다.", "확인");
                return;
            }

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            EmrForm pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, FstrLineGubun.Equals("중심정맥관") ? "2638" : "2240", clsEmrQuery.GetNewFormMaxUpdateNo(clsDB.DbCon, VB.Val(FstrLineGubun.Equals("중심정맥관") ? "2638" : "2240")).ToString());
            Dictionary<string, string> strDataNew = new Dictionary<string, string>();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (Line1Act.status == "삽입" && Line1Act.SEQNO == "")
                {
                    SQL = "";
                    SQL = "SELECT KOSMOS_PMPA.SEQ_NURLINEACT.NEXTVAL SEQNO FROM DUAL";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        Line1Act.SEQNO = dt.Rows[0]["SEQNO"].ToString().Trim();
                    }
                    else
                    {
                        dt.Dispose();
                        dt = null;
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("데이터 정보가 부족합니다.");
                        return;
                    }

                    dt.Dispose();
                    dt = null;
                }

                CREATE_LINE1_CHARTDATA();

                strActTime = Line1Act.ACTTIME.Replace(":", "");

                if (VB.Val(strActTime) >= VB.Val("0700") && VB.Val(strActTime) <= VB.Val("1459"))
                {
                    Line1Act.DUTY = "Day";
                }
                else if (VB.Val(strActTime) >= VB.Val("1500") && VB.Val(strActTime) <= VB.Val("2259"))
                {
                    Line1Act.DUTY = "Evening";
                }
                else if (VB.Val(strActTime) >= VB.Val("2300") || VB.Val(strActTime) <= VB.Val("0659"))
                {
                    Line1Act.DUTY = "Night";
                }

                if (Line1Act.MON1 == "1")
                {
                    strMon1 = "발적";
                }

                if (Line1Act.MON2 == "1")
                {
                    strMon1 = "부종";
                }

                if (Line1Act.MON3 == "1")
                {
                    strMon1 = "삼출물";
                }

                if (Line1Act.MON4 == "1")
                {
                    strMon1 = "압통";
                }

                if (Line1Act.MON6 == "1")
                {
                    strMon1 = "열감";
                }

                if (Line1Act.MON5 == "1")
                {
                    strMon1 = "이상없음";
                }

                if (Line1Act.D_MON1 == "Y")
                {
                    strDMon1 = "건조됨";
                }
                else
                {
                    strDMon1 = "건조되지 않음";
                }

                if (Line1Act.D_MON2 == "양호")
                {
                    strDMon2 = "부착됨";
                }
                else
                {
                    strDMon2 = "떨어짐";
                }

                if (pForm.FmOLDGB == 1)
                {
                    #region xml
                    strEMRNO = "";
                    strEMRNO = Convert.ToString(ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GETEMRXMLNO"));

                    SetTagHeadTagTail(mstrNewChartCd, ref mstrTagHead, ref mstrTagTail);

                    strData = "";
                    strData = mstrTagHead[0] + Line1Act.ACTDATE + " " + Line1Act.ACTTIME + mstrTagTail[0]; //'점검일
                    strData = strData + mstrTagHead[1] + Line1Act.DUTY + mstrTagTail[1];  //    'duty
                    strData = strData + mstrTagHead[2] + Line1Act.INSERT_BUSE + mstrTagTail[2]; //    '삽입부서
                    strData = strData + mstrTagHead[3] + Line1Act.status + mstrTagTail[3];  //       '액팅구분
                    strData = strData + mstrTagHead[4] + Line1Act.LOCATE1 + " " + Line1Act.LOCATE2 + mstrTagTail[4]; //     '종류
                    strData = strData + mstrTagHead[5] + Line1Act.PART1 + " " + Line1Act.PART2 + mstrTagTail[5];//     '삽입위치
                    strData = strData + mstrTagHead[6] + Line1Act.STARTDATE + " " + Line1Act.STARTTIME + mstrTagTail[6];//     '점검일
                    strData = strData + mstrTagHead[7] + Line1Act.USEDATE + mstrTagTail[7];//       '유지일
                    strData = strData + mstrTagHead[8] + (strMon1 + " " + strMon2 + " " + strMon3 + " " + strMon4 + " " + strMon5 + " " + strMon6).Trim() + mstrTagTail[8];//    '삽입부위관찰

                    if (FstrLineGubun != "말초정맥관")
                    {
                        strData = strData + mstrTagHead[9] + Line1Act.D_DATE + " " + Line1Act.D_TIME + mstrTagTail[9];//      '드레싱일시
                        strData = strData + mstrTagHead[10] + Line1Act.D_PART + mstrTagTail[10];//      '드레싱종류
                        strData = strData + mstrTagHead[11] + strDMon1 + " " + strDMon2 + mstrTagTail[11];//     '드레싱상태
                    }

                    mstrXml = "";
                    mstrXml = lblXmlHead.Text.Trim() + mstrChartX1 + strData + mstrChartX2;

                    if (clsNurse.CREATE_EMR_XMLINSRT3(VB.Val(strEMRNO), mstrNewChartCd, clsType.User.IdNumber, mstrChartDate, mstrChartTime, mintAcpNo,
                                            mstrPTNO, mstrInOutCls, mstrMedFrDate, mstrMedFrTime, mstrMedEndDate, mstrMedEndTime, mstrMedDeptCd, mstrMedDrCd, "0", 0, mstrXml) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("기록지 생성에 오류가 발생 하였습니다. 의료정보과에 문의하시기 바랍니다.", "확인");
                        return;
                    }
                    #endregion
                }
                else
                {
                    #region 신규
                    if (FstrLineGubun.Equals("말초정맥관"))
                    {
                        strDataNew.Clear();
                        strDataNew.Add("I0000008882", Line1Act.ACTDATE + " " + Line1Act.ACTTIME); //'점검일
                        strDataNew.Add("I0000037603", Line1Act.INSERT_BUSE); //    '삽입부서
                        strDataNew.Add("I0000037604", Line1Act.status);  //       '액팅구분
                        //if (pForm.FmFORMNO == 2227)
                        //{
                        //    strDataNew.Add("I0000037611", Line1Act.PART1 + " " + Line1Act.PART2); //     '종류
                        //    strDataNew.Add("I0000021853", Line1Act.LOCATE1 + " " + Line1Act.LOCATE2);//     '삽입위치
                        //}
                        //else if (pForm.FmFORMNO == 2240)
                        //{
                        strDataNew.Add("I0000021853", Line1Act.LOCATE1 + " " + Line1Act.LOCATE2);//     '삽입위치
                        strDataNew.Add("I0000037689", Line1Act.PART1);//     '바늘크기
                        strDataNew.Add("I0000001311", Line1Act.BIGO);//     '비고
                        //}

                        strDataNew.Add("I0000016403", Line1Act.STARTDATE + " " + Line1Act.STARTTIME);//     '점검일
                        strDataNew.Add("I0000037605", Line1Act.USEDATE);//       '유지일
                        strDataNew.Add("I0000037606", (strMon1 + " " + strMon2 + " " + strMon3 + " " + strMon4 + " " + strMon5 + " " + strMon6).Trim());//    '삽입부위관찰

                        //if (FstrLineGubun != "말초정맥관")
                        //{
                        //    strDataNew.Add("I0000037607", Line1Act.D_DATE + " " + Line1Act.D_TIME);//      '드레싱일시
                        //    strDataNew.Add("I0000031440", Line1Act.D_PART);//      '드레싱종류
                        //    strDataNew.Add("I0000037609", strDMon1 + " " + strDMon2);//     '드레싱상태
                        //}

                        strEMRNO = clsEmrQuery.SaveNurChartFlow(clsDB.DbCon, this, pAcp, pForm, mstrChartDate, mstrChartTime, strDataNew).ToString();
                    }
                    else
                    {
                        #region 유지 bundle
                        if (pForm.FmFORMNO == 2638)
                        {
                            strDataNew.Clear();
                            strDataNew.Add("I0000038112", Line1Act.status);                                     //중심정맥관 상태
                            strDataNew.Add("I0000021853", Line1Act.LOCATE1 + " " + Line1Act.LOCATE2);           //삽입위치
                            strDataNew.Add("I0000037640", Line1Act.HAND_HYGN);                                  //조작 전 손위생
                            strDataNew.Add("I0000038102", Line1Act.HERB_STRL);                                  //허브소독
                            strDataNew.Add("I0000038103", Line1Act.ASEPTIC_TECH);                               //메인 허브 교체 시 무균술 적용
                            strDataNew.Add("I0000005082", Line1Act.MON1);                                       //발적
                            strDataNew.Add("I0000021993", Line1Act.MON2);                                       //부종
                            strDataNew.Add("I0000038107", Line1Act.MON3);                                       //삼출물
                            strDataNew.Add("I0000000141", Line1Act.MON4);                                       //압통
                            strDataNew.Add("I0000033640", Line1Act.MON5);                                       //이상없음
                            strDataNew.Add("I0000038108", Line1Act.D_DATE);                                     //드레싱 일자
                            strDataNew.Add("I0000038109", Line1Act.D_TIME);                                     //드레싱 시간
                            strDataNew.Add("I0000031440", Line1Act.D_PART);                                     //드레싱 종류
                            strDataNew.Add("I0000037566", Line1Act.DRS_NAME);                                   //소독제
                            strDataNew.Add("I0000038104", Line1Act.D_MON1);                                     //건조여부
                            strDataNew.Add("I0000038110", Line1Act.D_MON2);                                     //부착상태
                            strDataNew.Add("I0000038105", Line1Act.NEED_ASSES);                                 //필요성사정
                            strDataNew.Add("I0000037613", Line1Act.STARTDATE + " " + Line1Act.STARTTIME);       //삽입일시
                            strDataNew.Add("I0000037605", Line1Act.USEDATE);                                    //유지일
                            strDataNew.Add("I0000024046", Line1Act.PART1 + " " + Line1Act.PART2);               //종류
                        }

                        strEMRNO = clsEmrQuery.SaveNurChartFlow(clsDB.DbCon, this, pAcp, pForm, mstrChartDate, mstrChartTime, strDataNew).ToString();
                        #endregion
                    }
                    #endregion
                }



                if (FstrLineGubun == "말초정맥관")
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL";
                }
                else
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL";
                }

                SQL = SQL + ComNum.VBLF + " (IPDNO, PANO, INDATE, WARDCODE,";
                SQL = SQL + ComNum.VBLF + " ROOMCODE, STARTDATE, ACTDATE, STATUS, ";
                SQL = SQL + ComNum.VBLF + " PART1, PART2, LOCATE1, LOCATE2, ";
                SQL = SQL + ComNum.VBLF + " MONITOR1, MONITOR2, MONITOR3, MONITOR4,";
                SQL = SQL + ComNum.VBLF + " MONITOR6, ";
                SQL = SQL + ComNum.VBLF + " MONITOR5, D_DATE, D_PART, ";
                SQL = SQL + ComNum.VBLF + " D_STATUS1, D_STATUS2, ACTSABUN, WRITEDATE, ";
                SQL = SQL + ComNum.VBLF + " EMRNO, SEQNO, USEDATE, INSERT_BUSE, DUTY, ";
                SQL = SQL + ComNum.VBLF + " HAND_HYGN, HERB_STRL, ASEPTIC_TECH, DRS_NAME, NEED_ASSES, BIGO ";
                SQL = SQL + ComNum.VBLF + " ) VALUES ( ";
                SQL = SQL + ComNum.VBLF + Line1Act.IPDNO + ", '" + Line1Act.Pano + "', TO_DATE('" + Line1Act.InDate + "','YYYY-MM-DD'), '" + Line1Act.WardCode + "', ";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.RoomCode + "', TO_DATE('" + (Line1Act.STARTDATE + " " + Line1Act.STARTTIME).Trim()
                    + "','YYYY-MM-DD HH24:MI'), TO_DATE('" + (Line1Act.ACTDATE + " " + Line1Act.ACTTIME).Trim() + "','YYYY-MM-DD HH24:MI'), '"
                    + Line1Act.status + "',";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.PART1 + "','" + Line1Act.PART2 + "','" + Line1Act.LOCATE1 + "','" + Line1Act.LOCATE2 + "',";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.MON1 + "','" + Line1Act.MON2 + "','" + Line1Act.MON3 + "','" + Line1Act.MON4 + "',";
                    SQL = SQL + ComNum.VBLF + "'" + Line1Act.MON6 + "', ";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.MON5 + "', TO_DATE('" + (Line1Act.D_DATE + " " + Line1Act.D_TIME).Trim()
                    + "','YYYY-MM-DD HH24:MI'),'" + Line1Act.D_PART + "','" + Line1Act.D_MON1 + "',";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.D_MON2 + "', " + Line1Act.ACTSABUN + ", SYSDATE, ";
                SQL = SQL + ComNum.VBLF + strEMRNO + "," + Line1Act.SEQNO + ",'" + Line1Act.USEDATE + "','" + Line1Act.INSERT_BUSE + "','" + Line1Act.DUTY + "', ";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.HAND_HYGN + "','" + Line1Act.HERB_STRL + "','" + Line1Act.ASEPTIC_TECH + "','" + Line1Act.DRS_NAME + "', ";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.NEED_ASSES + "','" + Line1Act.BIGO + "')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ClearLine1Act();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                ClearLine1Act();
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// INSERT_LINE1 clsDB.setBeginTran(clsDB.DbCon) 없는 버전
        /// </summary>
        private bool INSERT_LINE1X()
        {
            string strMon1 = "";
            string strMon2 = "";
            string strMon3 = "";
            string strMon4 = "";
            string strMon5 = "";
            string strMon6 = "";

            string strDMon1 = "";
            string strDMon2 = "";

            string strActTime = "";

            string strEMRNO = "";

            string strData = "";

            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            //TODO : 전자인증...ㄷㄷ
            //If GnJobSabun = 4349 Or GstrJobGrade = "EDPS" Then
            //Else
            //    If "NO" = CERT_CHECK(GnJobSabun) Then
            //        MsgBox "전자인증이 없습니다. 확인하시기 바랍니다.", vbInformation, "확인"
            //        Exit Function
            //    End If
            //End If

            EmrForm pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, FstrLineGubun.Equals("중심정맥관") ? "2638" : "2240", clsEmrQuery.GetNewFormMaxUpdateNo(clsDB.DbCon, VB.Val(FstrLineGubun.Equals("중심정맥관") ? "2638" : "2240")).ToString());

            Dictionary<string, string> strDataNew = new Dictionary<string, string>();

            try
            {
                if (Line1Act.status == "삽입" && Line1Act.SEQNO == "")
                {
                    SQL = "";
                    SQL = "SELECT KOSMOS_PMPA.SEQ_NURLINEACT.NEXTVAL SEQNO FROM DUAL";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return false;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        Line1Act.SEQNO = dt.Rows[0]["SEQNO"].ToString().Trim();
                    }
                    else
                    {
                        dt.Dispose();
                        dt = null;
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("데이터 정보가 부족합니다.");
                        return false;
                    }

                    dt.Dispose();
                    dt = null;
                }

                CREATE_LINE1_CHARTDATA();

                strActTime = Line1Act.ACTTIME.Replace(":", "");

                if (VB.Val(strActTime) >= VB.Val("0700") && VB.Val(strActTime) <= VB.Val("1459"))
                {
                    Line1Act.DUTY = "Day";
                }
                else if (VB.Val(strActTime) >= VB.Val("1500") && VB.Val(strActTime) <= VB.Val("2259"))
                {
                    Line1Act.DUTY = "Evening";
                }
                else if (VB.Val(strActTime) >= VB.Val("2300") || VB.Val(strActTime) <= VB.Val("0659"))
                {
                    Line1Act.DUTY = "Night";
                }

                if (Line1Act.MON1 == "1")
                {
                    strMon1 = "발적";
                }

                if (Line1Act.MON2 == "1")
                {
                    strMon1 = "부종";
                }

                if (Line1Act.MON3 == "1")
                {
                    strMon1 = "삼출물";
                }

                if (Line1Act.MON4 == "1")
                {
                    strMon1 = "압통";
                }

                if (Line1Act.MON6 == "1")
                {
                    strMon1 = "열감";
                }

                if (Line1Act.MON5 == "1")
                {
                    strMon1 = "이상없음";
                }

                if (Line1Act.D_MON1 == "Y")
                {
                    strDMon1 = "건조됨";
                }
                else
                {
                    strDMon1 = "건조되지 않음";
                }

                if (Line1Act.D_MON2 == "양호")
                {
                    strDMon2 = "부착됨";
                }
                else
                {
                    strDMon2 = "떨어짐";
                }

                if (pForm.FmOLDGB == 1)
                {
                    #region XML
                    strEMRNO = "";
                    strEMRNO = Convert.ToString(ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GETEMRXMLNO"));

                    SetTagHeadTagTail(mstrNewChartCd, ref mstrTagHead, ref mstrTagTail);

                    strData = "";
                    strData = mstrTagHead[0] + Line1Act.ACTDATE + " " + Line1Act.ACTTIME + mstrTagTail[0]; //'점검일
                    strData = strData + mstrTagHead[1] + Line1Act.DUTY + mstrTagTail[1];  //    'duty
                    strData = strData + mstrTagHead[2] + Line1Act.INSERT_BUSE + mstrTagTail[2]; //    '삽입부서
                    strData = strData + mstrTagHead[3] + Line1Act.status + mstrTagTail[3];  //       '액팅구분
                    strData = strData + mstrTagHead[4] + Line1Act.LOCATE1 + " " + Line1Act.LOCATE2 + mstrTagTail[4]; //     '종류
                    strData = strData + mstrTagHead[5] + Line1Act.PART1 + " " + Line1Act.PART2 + mstrTagTail[5];//     '삽입위치
                    strData = strData + mstrTagHead[6] + Line1Act.STARTDATE + " " + Line1Act.STARTTIME + mstrTagTail[6];//     '점검일
                    strData = strData + mstrTagHead[7] + Line1Act.USEDATE + mstrTagTail[7];//       '유지일
                    strData = strData + mstrTagHead[8] + (strMon1 + " " + strMon2 + " " + strMon3 + " " + strMon4 + " " + strMon5 + " " + strMon6).Trim() + mstrTagTail[8];//    '삽입부위관찰

                    if (FstrLineGubun != "말초정맥관")
                    {
                        strData = strData + mstrTagHead[9] + Line1Act.D_DATE + " " + Line1Act.D_TIME + mstrTagTail[9];//      '드레싱일시
                        strData = strData + mstrTagHead[10] + Line1Act.D_PART + mstrTagTail[10];//      '드레싱종류
                        strData = strData + mstrTagHead[11] + strDMon1 + " " + strDMon2 + mstrTagTail[11];//     '드레싱상태
                    }

                    mstrXml = "";
                    mstrXml = lblXmlHead.Text.Trim() + mstrChartX1 + strData + mstrChartX2;

                    if (clsNurse.CREATE_EMR_XMLINSRT3(VB.Val(strEMRNO), mstrNewChartCd, clsType.User.IdNumber, mstrChartDate, mstrChartTime, mintAcpNo,
                                            mstrPTNO, mstrInOutCls, mstrMedFrDate, mstrMedFrTime, mstrMedEndDate, mstrMedEndTime, mstrMedDeptCd, mstrMedDrCd, "0", 0, mstrXml) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("기록지 생성에 오류가 발생 하였습니다. 의료정보과에 문의하시기 바랍니다.", "확인");
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region 신규
                    if (FstrLineGubun.Equals("말초정맥관"))
                    {
                        strDataNew.Clear();
                        strDataNew.Add("I0000008882", Line1Act.ACTDATE + " " + Line1Act.ACTTIME); //'점검일
                        strDataNew.Add("I0000037603", Line1Act.INSERT_BUSE); //    '삽입부서
                        strDataNew.Add("I0000037604", Line1Act.status);  //       '액팅구분
                        //if (pForm.FmFORMNO == 2227)
                        //{
                        //    strDataNew.Add("I0000037611", Line1Act.PART1 + " " + Line1Act.PART2); //     '종류
                        //    strDataNew.Add("I0000021853", Line1Act.LOCATE1 + " " + Line1Act.LOCATE2);//     '삽입위치
                        //}
                        //else if (pForm.FmFORMNO == 2240)
                        //{
                        strDataNew.Add("I0000021853", Line1Act.LOCATE1 + " " + Line1Act.LOCATE2);//     '삽입위치
                        strDataNew.Add("I0000037689", Line1Act.PART1);//     '바늘크기
                        strDataNew.Add("I0000001311", Line1Act.BIGO);//     '비고
                        //}

                        strDataNew.Add("I0000016403", Line1Act.STARTDATE + " " + Line1Act.STARTTIME);//     '점검일
                        strDataNew.Add("I0000037605", Line1Act.USEDATE);//       '유지일
                        strDataNew.Add("I0000037606", (strMon1 + " " + strMon2 + " " + strMon3 + " " + strMon4 + " " + strMon5 + " " + strMon6).Trim());//    '삽입부위관찰

                        //if (FstrLineGubun != "말초정맥관")
                        //{
                        //    strDataNew.Add("I0000037607", Line1Act.D_DATE + " " + Line1Act.D_TIME);//      '드레싱일시
                        //    strDataNew.Add("I0000031440", Line1Act.D_PART);//      '드레싱종류
                        //    strDataNew.Add("I0000037609", strDMon1 + " " + strDMon2);//     '드레싱상태
                        //}

                        strEMRNO = clsEmrQuery.SaveNurChartFlow(clsDB.DbCon, this, pAcp, pForm, mstrChartDate, mstrChartTime, strDataNew).ToString();
                    }
                    else
                    {
                        #region 유지 bundle
                        if (pForm.FmFORMNO == 2638)
                        {
                            strDataNew.Clear();
                            strDataNew.Add("I0000038112", Line1Act.status);                                     //중심정맥관 상태
                            strDataNew.Add("I0000021853", Line1Act.LOCATE1 + " " + Line1Act.LOCATE2);           //삽입위치
                            strDataNew.Add("I0000037640", Line1Act.HAND_HYGN);                                  //조작 전 손위생
                            strDataNew.Add("I0000038102", Line1Act.HERB_STRL);                                  //허브소독
                            strDataNew.Add("I0000038103", Line1Act.ASEPTIC_TECH);                               //메인 허브 교체 시 무균술 적용
                            strDataNew.Add("I0000005082", Line1Act.MON1);                                       //발적
                            strDataNew.Add("I0000021993", Line1Act.MON2);                                       //부종
                            strDataNew.Add("I0000038107", Line1Act.MON3);                                       //삼출물
                            strDataNew.Add("I0000000141", Line1Act.MON4);                                       //압통
                            strDataNew.Add("I0000033640", Line1Act.MON5);                                       //이상없음
                            strDataNew.Add("I0000038108", Line1Act.D_DATE);                                     //드레싱 일자
                            strDataNew.Add("I0000038109", Line1Act.D_TIME);                                     //드레싱 시간
                            strDataNew.Add("I0000031440", Line1Act.D_PART);                                     //드레싱 종류
                            strDataNew.Add("I0000037566", Line1Act.DRS_NAME);                                   //소독제
                            strDataNew.Add("I0000038104", Line1Act.D_MON1);                                     //건조여부
                            strDataNew.Add("I0000038110", Line1Act.D_MON2);                                     //부착상태
                            strDataNew.Add("I0000038105", Line1Act.NEED_ASSES);                                 //필요성사정
                            strDataNew.Add("I0000037613", Line1Act.STARTDATE + " " + Line1Act.STARTTIME);       //삽입일시
                            strDataNew.Add("I0000037605", Line1Act.USEDATE);                                    //유지일
                            strDataNew.Add("I0000024046", Line1Act.PART1 + " " + Line1Act.PART2);               //종류
                        }

                        strEMRNO = clsEmrQuery.SaveNurChartFlow(clsDB.DbCon, this, pAcp, pForm, mstrChartDate, mstrChartTime, strDataNew).ToString();
                        #endregion
                    }
                    #endregion
                }


                if (FstrLineGubun == "말초정맥관")
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL";
                }
                else
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL";
                }

                SQL = SQL + ComNum.VBLF + " (IPDNO, PANO, INDATE, WARDCODE,";
                SQL = SQL + ComNum.VBLF + " ROOMCODE, STARTDATE, ACTDATE, STATUS, ";
                SQL = SQL + ComNum.VBLF + " PART1, PART2, LOCATE1, LOCATE2, ";
                SQL = SQL + ComNum.VBLF + " MONITOR1, MONITOR2, MONITOR3, MONITOR4,";
                SQL = SQL + ComNum.VBLF + " MONITOR6, ";
                SQL = SQL + ComNum.VBLF + " MONITOR5, D_DATE, D_PART, ";
                SQL = SQL + ComNum.VBLF + " D_STATUS1, D_STATUS2, ACTSABUN, WRITEDATE, ";
                SQL = SQL + ComNum.VBLF + " EMRNO, SEQNO, USEDATE, INSERT_BUSE, DUTY, BIGO,";
                SQL = SQL + ComNum.VBLF + " HAND_HYGN, HERB_STRL, ASEPTIC_TECH, DRS_NAME, NEED_ASSES) VALUES ( ";
                SQL = SQL + ComNum.VBLF + Line1Act.IPDNO + ", '" + Line1Act.Pano + "', TO_DATE('" + Line1Act.InDate + "','YYYY-MM-DD'), '"
                    + Line1Act.WardCode + "', ";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.RoomCode + "', TO_DATE('" + (Line1Act.STARTDATE + " " + Line1Act.STARTTIME).Trim()
                    + "','YYYY-MM-DD HH24:MI'), TO_DATE('" + (Line1Act.ACTDATE + " " + Line1Act.ACTTIME).Trim() + "','YYYY-MM-DD HH24:MI'), '"
                    + Line1Act.status + "',";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.PART1 + "','" + Line1Act.PART2 + "','" + Line1Act.LOCATE1 + "','" + Line1Act.LOCATE2 + "',";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.MON1 + "','" + Line1Act.MON2 + "','" + Line1Act.MON3 + "','" + Line1Act.MON4 + "',";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.MON6 + "', ";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.MON5 + "', TO_DATE('" + (Line1Act.D_DATE + " " + Line1Act.D_TIME).Trim()
                    + "','YYYY-MM-DD HH24:MI'),'" + Line1Act.D_PART + "','" + Line1Act.D_MON1 + "',";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.D_MON2 + "', " + Line1Act.ACTSABUN + ", SYSDATE, ";
                SQL = SQL + ComNum.VBLF + strEMRNO + "," + Line1Act.SEQNO + ",'" + Line1Act.USEDATE + "','" + Line1Act.INSERT_BUSE + "','" + Line1Act.DUTY
                    + "','" + Line1Act.BIGO + "', ";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.HAND_HYGN + "','" + Line1Act.HERB_STRL + "','" + Line1Act.ASEPTIC_TECH + "','" + Line1Act.DRS_NAME + "', ";
                SQL = SQL + ComNum.VBLF + "'" + Line1Act.NEED_ASSES + "')";


                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                ClearLine1Act();

                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                ClearLine1Act();
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void CREATE_LINE1_CHARTDATA()
        {
            mstrUSEID = Line1Act.ACTSABUN;

            mstrChartDate = Line1Act.ACTDATE.Replace("-", "");
            mstrChartTime = Line1Act.ACTTIME.Replace(":", "");
            mintAcpNo = 0;
            mstrPTNO = FstrPtno;
            if (cboWard.Text.Trim() == "ER")        //ER일 경우 외래로.
            {
                mstrInOutCls = "O";
            }
            else
            {
                mstrInOutCls = "I";
            }

            //mstrInOutCls = "I";
            mstrMedFrDate = Line1Act.InDate.Replace("-", "");
            mstrMedEndDate = "";
            mstrMedDeptCd = Line1Act.DeptCode;
            mstrMedDrCd = Line1Act.DrCode;
            mstrMedFrTime = "120000";
            mstrMedEndTime = "";
            if (FstrLineGubun == "말초정맥관")
            {
                mstrNewChartCd = "2240";
            }
            else
            {
                mstrNewChartCd = "2638";
            }
        }

        private bool SetSSLine2(string strROWID, int intRow)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strStartDate = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT PART1, PART2, LOCATE1, LOCATE2, INSERT_BUSE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    Line1Act.PART1 = dt.Rows[0]["PART1"].ToString().Trim();
                    Line1Act.PART2 = dt.Rows[0]["PART2"].ToString().Trim();
                    Line1Act.LOCATE1 = dt.Rows[0]["LOCATE1"].ToString().Trim();
                    Line1Act.LOCATE2 = dt.Rows[0]["LOCATE2"].ToString().Trim();
                    Line1Act.INSERT_BUSE = dt.Rows[0]["INSERT_BUSE"].ToString().Trim();
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("에러 발생 전산실 문의하시기 바랍니다.", "확인");
                    return false;
                }

                dt.Dispose();
                dt = null;

                Line1Act.HAND_HYGN = ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.HAND_HYGN)].Text.Trim();
                Line1Act.HERB_STRL = ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.HERB_STRL)].Text.Trim();
                Line1Act.ASEPTIC_TECH = ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.ASEPTIC_TECH)].Text.Trim();
                Line1Act.MON1 = (Convert.ToBoolean(ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.OBSERVE_1)].Value) == true ? "1" : "0");
                Line1Act.MON2 = (Convert.ToBoolean(ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.OBSERVE_2)].Value) == true ? "1" : "0");
                Line1Act.MON3 = (Convert.ToBoolean(ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.OBSERVE_3)].Value) == true ? "1" : "0");
                Line1Act.MON4 = (Convert.ToBoolean(ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.OBSERVE_4)].Value) == true ? "1" : "0");
                Line1Act.MON5 = (Convert.ToBoolean(ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.OBSERVE_5)].Value) == true ? "1" : "0");
                Line1Act.D_DATE = ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_DATE)].Text.Trim();
                Line1Act.D_TIME = ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_TIME)].Text.Trim();
                Line1Act.D_PART = ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_KIND)].Text.Trim();
                Line1Act.DRS_NAME = ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_NAME)].Text.Trim();
                Line1Act.D_MON1 = ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_STATE1)].Text.Trim();
                Line1Act.D_MON2 = ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.DRS_STATE2)].Text.Trim();
                Line1Act.NEED_ASSES = ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.NEED_ASSES)].Text.Trim();
                strStartDate = READ_STARTDATE(Line1Act.SEQNO);
                Line1Act.STARTDATE = VB.Left(strStartDate, 10);
                Line1Act.STARTTIME = VB.Right(strStartDate, 5);

                Line1Act.USEDATE = ssLine2_Sheet1.Cells[intRow, (int)(enumColumn_LineAct.KEEP_DAY)].Text.Trim();

                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private void DelLineAct1(string strROWID)
        {
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strEMRNO = "";
            DataTable dt = null;

            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT EMRNO";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strEMRNO = dt.Rows[0]["EMRNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strROWID != "")
                {
                    if (DeleteEmrXmlData(strEMRNO) == false)
                    {
                        ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    else
                    {
                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL_DEL ";
                        SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " DELETE KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                    }
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssLine3_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssLine3_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            if (FstrLineGubun == "")
            {
                ComFunc.MsgBox("정상적인 접근이 아닙니다. 전산정보과에 문의하시기 바랍니다.", "확인");
                return;
            }

            READ_ACT_LINE_MODIFY(ssLine3_Sheet1.Cells[e.Row, 13+5].Text.Trim());
        }

        private void READ_ACT_LINE_MODIFY(string argROWID)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            string strStartDate = "";
            string strACTDATE = "";

            panActModify.Visible = true;
            panActModify.Location = new Point(35, 484);
            ssLineModify_Sheet1.RowCount = 3;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(ACTDATE, 'YYYY-MM-DD') ACTDATE, TO_CHAR(ACTDATE, 'HH24:MI') ACTTIME, PART1 || ' ' || PART2 PART, ";
                SQL = SQL + ComNum.VBLF + " LOCATE1 || ' ' || LOCATE2 LOCATE, ";
                SQL = SQL + ComNum.VBLF + " MONITOR1, MONITOR2, MONITOR3, MONITOR4, MONITOR5, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(D_DATE, 'YYYY-MM-DD HH24:MI') D_DATE, ";
                SQL = SQL + ComNum.VBLF + " D_PART, D_STATUS1, D_STATUS2, ACTSABUN, SEQNO, ";
                SQL = SQL + ComNum.VBLF + " ROWID, EMRNO, STATUS, DUTY," ;
                SQL = SQL + ComNum.VBLF + " HAND_HYGN, HERB_STRL, ASEPTIC_TECH, DRS_NAME, NEED_ASSES ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + argROWID + "'";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strACTDATE = dt.Rows[0]["ACTDATE"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 0].Text = dt.Rows[0]["STATUS"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 1].Text = dt.Rows[0]["ACTDATE"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 2].Text = dt.Rows[0]["ACTTIME"].ToString().Trim();
                    //ssLineModify_Sheet1.Cells[2, 3].Text = dt.Rows[0]["DUTY"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 3].Text = dt.Rows[0]["LOCATE"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 4].Text = dt.Rows[0]["HAND_HYGN"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 5].Text = dt.Rows[0]["HERB_STRL"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 6].Text = dt.Rows[0]["ASEPTIC_TECH"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 7].Value = (dt.Rows[0]["MONITOR1"].ToString().Trim() == "1" ? true : false);
                    ssLineModify_Sheet1.Cells[2, 8].Value = (dt.Rows[0]["MONITOR2"].ToString().Trim() == "1" ? true : false);
                    ssLineModify_Sheet1.Cells[2, 9].Value = (dt.Rows[0]["MONITOR3"].ToString().Trim() == "1" ? true : false);
                    ssLineModify_Sheet1.Cells[2, 10].Value = (dt.Rows[0]["MONITOR4"].ToString().Trim() == "1" ? true : false);
                    ssLineModify_Sheet1.Cells[2, 11].Value = (dt.Rows[0]["MONITOR5"].ToString().Trim() == "1" ? true : false);

                    ssLineModify_Sheet1.Cells[2, 12].Text = VB.Left(dt.Rows[0]["D_DATE"].ToString().Trim(), 10);
                    ssLineModify_Sheet1.Cells[2, 13].Text = VB.Right(dt.Rows[0]["D_DATE"].ToString().Trim(), 5);
                    ssLineModify_Sheet1.Cells[2, 14].Text = dt.Rows[0]["D_PART"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 15].Text = dt.Rows[0]["DRS_NAME"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 16].Text = dt.Rows[0]["D_STATUS1"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 17].Text = dt.Rows[0]["D_STATUS2"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 18].Text = dt.Rows[0]["NEED_ASSES"].ToString().Trim();
                    strStartDate = READ_STARTDATE(dt.Rows[0]["SEQNO"].ToString().Trim());

                    if (strStartDate == "")
                    {
                        strStartDate = strACTDATE;
                    }

                    ssLineModify_Sheet1.Cells[2, 19].Text = Convert.ToDateTime(strStartDate).ToString("MM/dd").Replace("-", "/");

                    if (dt.Rows[0]["STATUS"].ToString().Trim() == "삽입")
                    {
                        ssLineModify_Sheet1.Cells[2, 20].Text = "";
                    }
                    else
                    {
                        ssLineModify_Sheet1.Cells[2, 20].Text = Convert.ToString(VB.DateDiff("d", VB.Left(strStartDate, 10), strACTDATE) + 1);
                    }

                    ssLineModify_Sheet1.Cells[2, 21].Text = dt.Rows[0]["ROWID"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 22].Text = dt.Rows[0]["SEQNO"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 23].Text = dt.Rows[0]["EMRNO"].ToString().Trim();
                    ssLineModify_Sheet1.Cells[2, 24].Text = dt.Rows[0]["PART"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnBuse_Click(object sender, EventArgs e)
        {
            Line1Act.PART1 = ssLine1_Sheet1.Cells[2, 0].Text.Trim();
            Line1Act.PART2 = ssLine1_Sheet1.Cells[2, 1].Text.Trim();
            Line1Act.LOCATE1 = ssLine1_Sheet1.Cells[2, 2].Text.Trim();
            Line1Act.LOCATE2 = ssLine1_Sheet1.Cells[2, 3].Text.Trim();
            Line1Act.MON1 = (Convert.ToBoolean(ssLine1_Sheet1.Cells[2, 4].Value) == true ? "1" : "0");
            Line1Act.MON2 = (Convert.ToBoolean(ssLine1_Sheet1.Cells[2, 5].Value) == true ? "1" : "0");
            Line1Act.MON3 = (Convert.ToBoolean(ssLine1_Sheet1.Cells[2, 6].Value) == true ? "1" : "0");
            Line1Act.MON4 = (Convert.ToBoolean(ssLine1_Sheet1.Cells[2, 7].Value) == true ? "1" : "0");
            Line1Act.MON5 = (Convert.ToBoolean(ssLine1_Sheet1.Cells[2, 8].Value) == true ? "1" : "0");
            Line1Act.D_DATE = ssLine1_Sheet1.Cells[2, 9].Text.Trim();
            Line1Act.D_TIME = ssLine1_Sheet1.Cells[2, 10].Text.Trim();
            Line1Act.D_PART = ssLine1_Sheet1.Cells[2, 11].Text.Trim();
            Line1Act.D_MON1 = ssLine1_Sheet1.Cells[2, 12].Text.Trim();
            Line1Act.D_MON2 = ssLine1_Sheet1.Cells[2, 13].Text.Trim();

            if (optBuse1.Checked == true)
            {
                Line1Act.INSERT_BUSE = cboWard.Text.Trim();
            }
            else if (optBuse2.Checked == true)
            {
                Line1Act.INSERT_BUSE = "ER";
            }
            else if (optBuse3.Checked == true)
            {
                Line1Act.INSERT_BUSE = "HD";
            }
            else if (optBuse4.Checked == true)
            {
                Line1Act.INSERT_BUSE = "OR";
            }
            else if (optBuse5.Checked == true)
            {
                Line1Act.INSERT_BUSE = "Angio실";
            }
            else if (optBuse6.Checked == true)
            {
                Line1Act.INSERT_BUSE = "외부";
            }
            else if (optBuse7.Checked == true)
            {
                Line1Act.INSERT_BUSE = "기타" + (txtBuseEtc.Text.Trim() != "" ? "(" + txtBuseEtc.Text.Trim() + ")" : "");
            }

            if (Line1Act.PART1 == "" || Line1Act.PART2 == "" || Line1Act.LOCATE1 == "" || Line1Act.LOCATE2 == "")
            {
                ComFunc.MsgBox("필수 입력 항목 누락입니다. 확인하시기 바랍니다.", "확인");
                return;
            }

            //중심도관
            EmrForm bundleForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "2643");

            //if (FormPatInfoFunc.Set_FormPatInfo_IsWrite(clsDB.DbCon, bundleForm, pAcp) == false)
            //{
                frmEmrChartNewX = new frmEmrChartNew(bundleForm.FmFORMNO.ToString(), bundleForm.FmUPDATENO.ToString(), pAcp, "0", "W");
                frmEmrChartNewX.StartPosition = FormStartPosition.CenterParent;
                frmEmrChartNewX.FormClosed += FrmEmrChartNewX_FormClosed;
                frmEmrChartNewX.Show(this);

                #region 삽입장소, 터널식, 비터널식, 부위, 목적 등 매핑
                Control[] controls = frmEmrChartNewX.Controls.Find("panChart", true);
                if (controls.Length > 0)
                {
                    //삽입장소
                    Control[] control2 = controls[0].Controls.Find("I0000037612", true);
                    if (control2.Length > 0)
                    {
                        control2[0].Text = Line1Act.INSERT_BUSE;
                    }

                    if (ssLine1_Sheet1.Cells[2, 0].Text.Trim() == "비터널식")
                    {
                        //비터널식
                        control2 = controls[0].Controls.Find("I0000037615", true);
                        if (control2.Length > 0)
                        {
                            control2[0].Text = Line1Act.PART2;
                        }
                    }
                    else
                    {
                        //터널식
                        control2 = controls[0].Controls.Find("I0000037616", true);
                        if (control2.Length > 0)
                        {
                            control2[0].Text = Line1Act.PART2;
                        }
                    }

                    //삽입부위
                    control2 = controls[0].Controls.Find("I0000037617", true);
                    if (control2.Length > 0)
                    {
                        control2[0].Text = Line1Act.LOCATE1 + " " + Line1Act.LOCATE2;
                    }

                    //드레싱 종류
                    control2 = controls[0].Controls.Find("I0000031440", true);
                    if (control2.Length > 0)
                    {
                        control2[0].Text = Line1Act.D_PART;
                    }

                    if (Line1Act.INSERT_BUSE == "외부")
                    {
                        //외부
                        control2 = controls[0].Controls.Find("I0000037900", true);
                        if (control2.Length > 0)
                        {
                            (control2[0] as RadioButton).Checked = true;
                        }
                    }
                    else
                    {
                        //원내
                        control2 = controls[0].Controls.Find("I0000037899", true);
                        if (control2.Length > 0)
                        {
                            (control2[0] as RadioButton).Checked = true;
                        }
                    }
                }
            #endregion

            //    return;
            //}

            #region 수가 연동
            if (FormPatInfoFunc.Set_FormPatInfo_ItemSugaMaaping(clsDB.DbCon, "2638", pAcp.ward, "중심정맥관", "삽입"))
            {
                DateTime SysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                using (frmSugaOrderSave frm = new frmSugaOrderSave(SysDate.ToString("yyyy-MM-dd"), "2638", "중심정맥관", "삽입", "중심정맥관", pAcp, SysDate.ToString("yyyyMMdd"), null, -1))
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog(this);
                }
                ssLine1_Sheet1.Columns[15].Visible = clsEmrQuery.ChartOrder_Exists(this, pAcp, SysDate.ToString("yyyyMMdd"), "중심정맥관", "삽입");
            }
            #endregion

            INSERT_LINE1();

            SSLINE1_CLEAR();
            btnSreachKeepL1_Click(null, null);
            btnSreachL1_Click(null, null);

            panBeginBuse.Visible = false;
        }

        private void btnBuseC_Click(object sender, EventArgs e)
        {
            panBeginBuse.Visible = false;
        }

        private void ssLineP1_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if (FbBtnClick == true)
            {
                return;
            }

            FbBtnClick = false;

            ClearLine1Act();

            if (e.Column == 11 && e.Row == 1)
            {
            }
            else if (e.Column == 12 && e.Row == 0)
            {
                //당일 처방내역 버튼
                //당일 처방내역 버튼
                DateTime SysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                using (frmSugaOrderSave frmSugaOrderSaveX = new frmSugaOrderSave(SysDate.ToString("yyyy-MM-dd"), "2240", "말초정맥관", "", "말초정맥관", pAcp, SysDate.ToString("yyyyMMdd"), null, -1))
                {
                    frmSugaOrderSaveX.StartPosition = FormStartPosition.CenterScreen;
                    frmSugaOrderSaveX.ShowDialog(this);
                }
            }
            else
            {
                if (e.Row == 2)
                {
                    switch (e.Column)
                    {
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            if (e.Column == 9)
                            {
                                FbBtnClick = true;

                                ssLineP1_Sheet1.Cells[e.Row, 4].Value = false;
                                ssLineP1_Sheet1.Cells[e.Row, 5].Value = false;
                                ssLineP1_Sheet1.Cells[e.Row, 6].Value = false;
                                ssLineP1_Sheet1.Cells[e.Row, 7].Value = false;
                                ssLineP1_Sheet1.Cells[e.Row, 8].Value = false;

                                FbBtnClick = false;
                            }
                            else
                            {
                                FbBtnClick = true;
                                ssLineP1_Sheet1.Cells[e.Row, 9].Value = false;
                                FbBtnClick = false;
                            }
                            break;
                    }
                }

                return;
            }

            if (FstrPtno == "")
            {
                ComFunc.MsgBox("환자를 선택 후 액팅하시기 바랍니다.", "확인");
                return;
            }

            Line1Act.IPDNO = Convert.ToString(mintIPDNO);
            Line1Act.Pano = FstrPtno;
            Line1Act.WardCode = cboWard.Text.Trim();
            Line1Act.RoomCode = FstrRoom;
            Line1Act.DeptCode = FstrDEPT;
            Line1Act.DrCode = FstrDrCode;
            Line1Act.InDate = FstrInDate;

            if (chkLinePChartDate.Checked == true)
            {
                Line1Act.ACTDATE = txtLine1PDate.Text.Trim();
                Line1Act.ACTTIME = txtLine1PTime.Text.Trim();
            }
            else
            {
                Line1Act.ACTDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                Line1Act.ACTTIME = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
            }

            Line1Act.STARTDATE = Line1Act.ACTDATE;
            Line1Act.STARTTIME = Line1Act.ACTTIME;
            Line1Act.ACTSABUN = clsType.User.Sabun;
            Line1Act.status = "삽입";

            if (e.Column == 11 && e.Row == 1)
            {
                panBeginBuseP.Visible = true;
                optBuse1P.Checked = true;
                txtBuseEtcP.Text = "";
            }
        }

        private void ssLineP2_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            string strROWID = "";
            //string strStartDate = "";

            //string strEMRNO = "";

            ClearLine1Act();

            if (e.Row < 2)
            {
                return;
            }

            if (FbBtnClick == true)
            {
                return;
            }

            switch (e.Column)
            {
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    if (e.Column == 11)
                    {
                        FbBtnClick = true;

                        ssLineP2_Sheet1.Cells[e.Row, 6].Value = false;
                        ssLineP2_Sheet1.Cells[e.Row, 7].Value = false;
                        ssLineP2_Sheet1.Cells[e.Row, 8].Value = false;
                        ssLineP2_Sheet1.Cells[e.Row, 9].Value = false;
                        ssLineP2_Sheet1.Cells[e.Row, 10].Value = false;

                        FbBtnClick = false;
                    }
                    else
                    {
                        FbBtnClick = true;

                        ssLineP2_Sheet1.Cells[e.Row, 11].Value = false;

                        FbBtnClick = false;
                    }
                    break;
            }

            if (e.Column == 18 + 1)
            {
                if (ComFunc.MsgBoxQ("삭제 후 복구는 불가능합니다. 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }

                strROWID = ssLineP2_Sheet1.Cells[e.Row, 16 + 1].Text.Trim();

                DelLineAct2(strROWID);

                SSLINEP1_CLEAR();
                btnSreachKeepLP1_Click(null, null);
                btnSreachLP1_Click(null, null);

                return;
            }

            switch (e.Column)
            {
                case 14 + 1:
                    Line1Act.status = "유지";
                    break;

                case 15 + 1:
                    Line1Act.status = "제거";
                    break;

                default:
                    return;
            }

            strROWID = ssLineP2_Sheet1.Cells[e.Row, 16 + 1].Text.Trim();
            Line1Act.SEQNO = ssLineP2_Sheet1.Cells[e.Row, 17 + 1].Text.Trim();

            Line1Act.IPDNO = Convert.ToString(mintIPDNO);
            Line1Act.Pano = FstrPtno;
            Line1Act.WardCode = cboWard.Text.Trim();
            Line1Act.RoomCode = FstrRoom;
            //2019-02-03
            if (cboWard.Text.Trim() == "ER" && FstrRoom != "100")
            {
                Line1Act.RoomCode = "100";
            }
            Line1Act.DeptCode = FstrDEPT;
            Line1Act.DrCode = FstrDrCode;
            Line1Act.InDate = FstrInDate;

            if (chkLinePChartDate2.Checked == true)
            {
                Line1Act.ACTDATE = txtLine2PDate.Text.Trim();
                Line1Act.ACTTIME = txtLine2PTime.Text.Trim();
            }
            else
            {
                Line1Act.ACTDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                Line1Act.ACTTIME = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
            }

            Line1Act.ACTSABUN = clsType.User.Sabun;

            SetLine1Act(strROWID, e.Row);

            #region 수가 연동 드레싱이 서버날짜랑 같을때만
            DateTime SysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            if (Line1Act.status == "유지" && FormPatInfoFunc.Set_FormPatInfo_ItemSugaMaaping(clsDB.DbCon, "2240", pAcp.ward, "말초정맥관", Line1Act.status))
            {
                using (frmSugaOrderSave frm = new frmSugaOrderSave(SysDate.Date.ToString("yyyy-MM-dd"), "2240", "말초정맥관", Line1Act.status, "말초정맥관", pAcp, SysDate.ToString("yyyyMMdd"), null, -1))
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog(this);
                }

                ssLineP1_Sheet1.Columns[12].Visible = clsEmrQuery.ChartOrder_Exists(this, pAcp, SysDate.ToString("yyyyMMdd"), "말초정맥관", Line1Act.status);
            }
            #endregion

            INSERT_LINE1();
            SSLINEP1_CLEAR();
            btnSreachKeepLP1_Click(null, null);
            btnSreachLP1_Click(null, null);
        }

        private bool SetLine1Act(string strROWID, int intRow)
        {
            string strStartDate = "";
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return false; //권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT PART1, PART2, LOCATE1, LOCATE2 ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    Line1Act.PART1 = dt.Rows[0]["PART1"].ToString().Trim();
                    Line1Act.PART2 = dt.Rows[0]["PART2"].ToString().Trim();
                    Line1Act.LOCATE1 = dt.Rows[0]["LOCATE1"].ToString().Trim();
                    Line1Act.LOCATE2 = dt.Rows[0]["LOCATE2"].ToString().Trim();
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("에러 발생 전산실 문의하시기 바랍니다.", "확인");
                    return false;
                }

                dt.Dispose();
                dt = null;

                Line1Act.MON1 = (Convert.ToBoolean(ssLineP2_Sheet1.Cells[intRow, 6].Value) == true ? "1" : "0");
                Line1Act.MON2 = (Convert.ToBoolean(ssLineP2_Sheet1.Cells[intRow, 7].Value) == true ? "1" : "0");
                Line1Act.MON3 = (Convert.ToBoolean(ssLineP2_Sheet1.Cells[intRow, 8].Value) == true ? "1" : "0");
                Line1Act.MON4 = (Convert.ToBoolean(ssLineP2_Sheet1.Cells[intRow, 9].Value) == true ? "1" : "0");
                Line1Act.MON6 = (Convert.ToBoolean(ssLineP2_Sheet1.Cells[intRow, 10].Value) == true ? "1" : "0");
                Line1Act.MON5 = (Convert.ToBoolean(ssLineP2_Sheet1.Cells[intRow, 11].Value) == true ? "1" : "0");
                Line1Act.BIGO = ssLineP2_Sheet1.Cells[intRow, 12].Text.Trim();

                strStartDate = READ_STARTDATE(Line1Act.SEQNO);
                Line1Act.STARTDATE = VB.Left(strStartDate, 10);
                Line1Act.STARTTIME = VB.Right(strStartDate, 5);

                Line1Act.USEDATE = ssLineP2_Sheet1.Cells[intRow, 13 + 1].Text.Trim();

                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private void DelLineAct2(string strROWID)
        {
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strEMRNO = "";
            DataTable dt = null;

            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT EMRNO";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL  ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strEMRNO = dt.Rows[0]["EMRNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strROWID != "")
                {
                    if (DeleteEmrXmlData(strEMRNO) == false)
                    {
                        ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    else
                    {
                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL_DEL ";
                        SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " DELETE KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                    }
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnBuseP_Click(object sender, EventArgs e)
        {
            Line1Act.PART1 = ssLineP1_Sheet1.Cells[2, 0].Text.Trim();
            Line1Act.PART2 = ssLineP1_Sheet1.Cells[2, 1].Text.Trim();
            Line1Act.LOCATE1 = ssLineP1_Sheet1.Cells[2, 2].Text.Trim();
            Line1Act.LOCATE2 = ssLineP1_Sheet1.Cells[2, 3].Text.Trim();
            Line1Act.MON1 = (Convert.ToBoolean(ssLineP1_Sheet1.Cells[2, 4].Value) == true ? "1" : "0");
            Line1Act.MON2 = (Convert.ToBoolean(ssLineP1_Sheet1.Cells[2, 5].Value) == true ? "1" : "0");
            Line1Act.MON3 = (Convert.ToBoolean(ssLineP1_Sheet1.Cells[2, 6].Value) == true ? "1" : "0");
            Line1Act.MON4 = (Convert.ToBoolean(ssLineP1_Sheet1.Cells[2, 7].Value) == true ? "1" : "0");
            Line1Act.MON6 = (Convert.ToBoolean(ssLineP1_Sheet1.Cells[2, 8].Value) == true ? "1" : "0");
            Line1Act.MON5 = (Convert.ToBoolean(ssLineP1_Sheet1.Cells[2, 9].Value) == true ? "1" : "0");
            Line1Act.BIGO = ssLineP1_Sheet1.Cells[2, 10].Text.Trim();

            if (optBuse1P.Checked == true)
            {
                Line1Act.INSERT_BUSE = cboWard.Text.Trim();
                //2019-02-03
                if (cboWard.Text.Trim() == "ER" && FstrRoom != "100")
                {
                    Line1Act.RoomCode = "100";
                }
            }
            else if (optBuse2P.Checked == true)
            {
                Line1Act.INSERT_BUSE = "ER";

                //2019-02-03
                if (cboWard.Text.Trim() == "ER" && FstrRoom != "100")
                {
                    Line1Act.RoomCode = "100";
                }
            }
            else if (optBuse3P.Checked == true)
            {
                Line1Act.INSERT_BUSE = "HD";
            }
            else if (optBuse4P.Checked == true)
            {
                Line1Act.INSERT_BUSE = "OR";
            }
            else if (optBuse5P.Checked == true)
            {
                Line1Act.INSERT_BUSE = "Angio실";
            }
            else if (optBuse6P.Checked == true)
            {
                Line1Act.INSERT_BUSE = "외부";
            }
            else if (optBuse7P.Checked == true)
            {
                Line1Act.INSERT_BUSE = "기타" + (txtBuseEtcP.Text.Trim() != "" ? "(" + txtBuseEtcP.Text.Trim() + ")" : "");
            }

            if (Line1Act.PART1 == "" || Line1Act.LOCATE1 == "" || Line1Act.LOCATE2 == "")
            {
                ComFunc.MsgBox("필수 입력 항목 누락입니다. 확인하시기 바랍니다.", "확인");
                return;
            }


            INSERT_LINE1();

            #region 수가 연동
            if (FormPatInfoFunc.Set_FormPatInfo_ItemSugaMaaping(clsDB.DbCon, "2240", pAcp.ward, "말초정맥관", "삽입"))
            {
                DateTime SysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                using (frmSugaOrderSave frm = new frmSugaOrderSave(SysDate.ToString("yyyy-MM-dd"), "2240", "말초정맥관", "삽입", "말초정맥관", pAcp, SysDate.ToString("yyyyMMdd"), null, -1))
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog(this);
                }
                ssLineP1_Sheet1.Columns[12].Visible = clsEmrQuery.ChartOrder_Exists(this, pAcp, SysDate.ToString("yyyyMMdd"), FstrLineGubun, "'삽입', '유지'");
            }
            
            #endregion

            SSLINEP1_CLEAR();
            btnSreachKeepLP1_Click(null, null);
            btnSreachLP1_Click(null, null);

            panBeginBuseP.Visible = false;
        }

        private void btnBuseCP_Click(object sender, EventArgs e)
        {
            panBeginBuseP.Visible = false;
        }

        private void ssLineP3_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssLineP3_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            READ_ACT_LINE_MODIFY2(ssLineP3_Sheet1.Cells[e.Row, 10].Text.Trim());
        }

        private void READ_ACT_LINE_MODIFY2(string argROWID)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strStartDate = "";
            string strACTDATE = "";

            panActModify2.Visible = true;
            panActModify2.Location = new Point(35, 484);
            ssLineModify2_Sheet1.RowCount = 3;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(ACTDATE, 'YYYY-MM-DD') ACTDATE, TO_CHAR(ACTDATE, 'HH24:MI') ACTTIME, PART1 || ' ' || PART2 PART, ";
                SQL = SQL + ComNum.VBLF + " LOCATE1 || ' ' || LOCATE2 LOCATE, ";
                SQL = SQL + ComNum.VBLF + " MONITOR1, MONITOR2, MONITOR3, MONITOR4, MONITOR5, MONITOR6, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(D_DATE, 'YYYY-MM-DD HH24:MI') D_DATE, ";
                SQL = SQL + ComNum.VBLF + " D_PART, D_STATUS1, D_STATUS2, ACTSABUN, SEQNO, ";
                SQL = SQL + ComNum.VBLF + " ROWID, EMRNO, STATUS, DUTY, BIGO";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + argROWID + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strACTDATE = dt.Rows[0]["ACTDATE"].ToString().Trim();
                    ssLineModify2_Sheet1.Cells[2, 0].Text = dt.Rows[0]["STATUS"].ToString().Trim();
                    ssLineModify2_Sheet1.Cells[2, 1].Text = dt.Rows[0]["ACTDATE"].ToString().Trim();
                    ssLineModify2_Sheet1.Cells[2, 2].Text = dt.Rows[0]["ACTTIME"].ToString().Trim();
                    ssLineModify2_Sheet1.Cells[2, 3].Text = dt.Rows[0]["DUTY"].ToString().Trim();
                    ssLineModify2_Sheet1.Cells[2, 4].Text = dt.Rows[0]["PART"].ToString().Trim();
                    ssLineModify2_Sheet1.Cells[2, 5].Text = dt.Rows[0]["LOCATE"].ToString().Trim();
                    ssLineModify2_Sheet1.Cells[2, 6].Value = (dt.Rows[0]["MONITOR1"].ToString().Trim() == "1" ? true : false);
                    ssLineModify2_Sheet1.Cells[2, 7].Value = (dt.Rows[0]["MONITOR2"].ToString().Trim() == "1" ? true : false);
                    ssLineModify2_Sheet1.Cells[2, 8].Value = (dt.Rows[0]["MONITOR3"].ToString().Trim() == "1" ? true : false);
                    ssLineModify2_Sheet1.Cells[2, 9].Value = (dt.Rows[0]["MONITOR4"].ToString().Trim() == "1" ? true : false);
                    ssLineModify2_Sheet1.Cells[2, 10].Value = (dt.Rows[0]["MONITOR6"].ToString().Trim() == "1" ? true : false);
                    ssLineModify2_Sheet1.Cells[2, 11].Value = (dt.Rows[0]["MONITOR5"].ToString().Trim() == "1" ? true : false);
                    ssLineModify2_Sheet1.Cells[2, 12].Text = dt.Rows[0]["BIGO"].ToString().Trim();
                    strStartDate = READ_STARTDATE(dt.Rows[0]["SEQNO"].ToString().Trim());

                    if (strStartDate == "")
                    {
                        strStartDate = strACTDATE;
                    }

                    ssLineModify2_Sheet1.Cells[2, 12 + 1].Text = Convert.ToDateTime(strStartDate).ToString("MM/dd").Replace("-", "/");

                    if (dt.Rows[0]["STATUS"].ToString().Trim() == "삽입")
                    {
                        ssLineModify2_Sheet1.Cells[2, 13 + 1].Text = "";
                    }
                    else
                    {
                        ssLineModify2_Sheet1.Cells[2, 13 + 1].Text = Convert.ToString(VB.DateDiff("d", VB.Left(strStartDate, 10), strACTDATE) + 1);
                    }

                    ssLineModify2_Sheet1.Cells[2, 14 + 1].Text = dt.Rows[0]["ROWID"].ToString().Trim();
                    ssLineModify2_Sheet1.Cells[2, 15 + 1].Text = dt.Rows[0]["SEQNO"].ToString().Trim();
                    ssLineModify2_Sheet1.Cells[2, 16 + 1].Text = dt.Rows[0]["EMRNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnChangeAct_Click(object sender, EventArgs e)
        {
            string strROWID = "";
            string strStartDate = "";

            string strEMRNO = "";

            string strACTSABUN = "";

            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            ClearLine1Act();

            if (FstrLineGubun == "")
            {
                ComFunc.MsgBox("정상적인 접근이 아닙니다. 전산정보과에 문의하시기 바랍니다.", "확인");
                return;
            }

            if (ComFunc.MsgBoxQ("수정 후 복구는 불가능합니다. 수정하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            strROWID = ssLineModify_Sheet1.Cells[2, 21].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT SEQNO, EMRNO, ACTSABUN ";
                if (FstrLineGubun == "말초정맥관")
                {
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL ";
                }

                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    Line1Act.SEQNO = dt.Rows[0]["SEQNO"].ToString().Trim();
                    strEMRNO = dt.Rows[0]["EMRNO"].ToString().Trim();
                    strACTSABUN = dt.Rows[0]["ACTSABUN"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Line1Act.status = ssLineModify_Sheet1.Cells[2, 0].Text.Trim();
                strROWID = ssLineModify_Sheet1.Cells[2, 21].Text.Trim();

                Line1Act.IPDNO = Convert.ToString(mintIPDNO);
                Line1Act.Pano = FstrPtno;
                Line1Act.WardCode = cboWard.Text.Trim();
                Line1Act.RoomCode = FstrRoom;
                Line1Act.DeptCode = FstrDEPT;
                Line1Act.DrCode = FstrDrCode;
                Line1Act.InDate = FstrInDate;
                Line1Act.ACTDATE = ssLineModify_Sheet1.Cells[2, 1].Text.Trim();
                Line1Act.ACTTIME = ssLineModify_Sheet1.Cells[2, 2].Text.Trim();
                Line1Act.ACTSABUN = clsType.User.Sabun;

                SQL = "";
                SQL = " SELECT PART1, PART2, LOCATE1, LOCATE2, WARDCODE, INSERT_BUSE, ROOMCODE ";
                if (FstrLineGubun == "말초정맥관")
                {
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL ";
                }

                SQL = SQL + ComNum.VBLF + " WHERE SEQNO = " + Line1Act.SEQNO;
                SQL = SQL + ComNum.VBLF + "     AND ROWNUM = 1";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    Line1Act.PART1 = dt.Rows[0]["PART1"].ToString().Trim();
                    Line1Act.PART2 = dt.Rows[0]["PART2"].ToString().Trim();
                    Line1Act.LOCATE1 = dt.Rows[0]["LOCATE1"].ToString().Trim();
                    Line1Act.LOCATE2 = dt.Rows[0]["LOCATE2"].ToString().Trim();
                    Line1Act.INSERT_BUSE = dt.Rows[0]["INSERT_BUSE"].ToString().Trim();
                    Line1Act.WardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    Line1Act.RoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("에러 발생 전산실 문의하시기 바랍니다.", "확인");
                    return;
                }

                dt.Dispose();
                dt = null;

                Line1Act.HAND_HYGN = ssLineModify_Sheet1.Cells[2, 4].Text.Trim();
                Line1Act.HERB_STRL = ssLineModify_Sheet1.Cells[2, 5].Text.Trim();
                Line1Act.ASEPTIC_TECH = ssLineModify_Sheet1.Cells[2, 6].Text.Trim();
                Line1Act.MON1 = (Convert.ToBoolean(ssLineModify_Sheet1.Cells[2, 7].Value) == true ? "1" : "0");
                Line1Act.MON2 = (Convert.ToBoolean(ssLineModify_Sheet1.Cells[2, 8].Value) == true ? "1" : "0");
                Line1Act.MON3 = (Convert.ToBoolean(ssLineModify_Sheet1.Cells[2, 9].Value) == true ? "1" : "0");
                Line1Act.MON4 = (Convert.ToBoolean(ssLineModify_Sheet1.Cells[2, 10].Value) == true ? "1" : "0");
                Line1Act.MON5 = (Convert.ToBoolean(ssLineModify_Sheet1.Cells[2, 11].Value) == true ? "1" : "0");
                Line1Act.D_DATE = ssLineModify_Sheet1.Cells[2, 12].Text.Trim();
                Line1Act.D_TIME = ssLineModify_Sheet1.Cells[2, 13].Text.Trim();
                Line1Act.D_PART = ssLineModify_Sheet1.Cells[2, 14].Text.Trim();
                Line1Act.DRS_NAME = ssLineModify_Sheet1.Cells[2, 15].Text.Trim();
                Line1Act.D_MON1 = ssLineModify_Sheet1.Cells[2, 16].Text.Trim();
                Line1Act.D_MON2 = ssLineModify_Sheet1.Cells[2, 17].Text.Trim();
                Line1Act.NEED_ASSES = ssLineModify_Sheet1.Cells[2, 18].Text.Trim();
                strStartDate = READ_STARTDATE(Line1Act.SEQNO);
                Line1Act.STARTDATE = VB.Left(strStartDate, 10);
                Line1Act.STARTTIME = VB.Right(strStartDate, 5);

                Line1Act.USEDATE = ssLineModify_Sheet1.Cells[2, 20].Text.Trim();

                if (strROWID != "")
                {
                    if (DeleteEmrXmlData(strEMRNO) == false)
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                        return;
                    }
                    else
                    {
                        if (FstrLineGubun == "말초정맥관")
                        {
                            SQL = " INSERT INTO KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL_DEL ";
                            SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                        }
                        else
                        {
                            SQL = " INSERT INTO KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL_DEL ";
                            SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL ";
                        }

                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (FstrLineGubun == "말초정맥관")
                        {
                            SQL = " DELETE KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL";
                        }
                        else
                        {
                            SQL = " DELETE KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL";
                        }

                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                if (INSERT_LINE1X() == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (FstrLineGubun == "말초정맥관")
                {
                    SSLINEP1_CLEAR();
                    btnSreachKeepLP1_Click(null, null);
                    btnSreachLP1_Click(null, null);
                }
                else
                {
                    SSLINE1_CLEAR();

                    btnSreachKeepL1_Click(null, null);
                    btnSreachL1_Click(null, null);
                }

                panActModify.Visible = false;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnDeleteAct_Click(object sender, EventArgs e)
        {
            ActDEL();
        }

        private void ActDEL()
        {
            string strROWID = "";
            //string strStartDate = "";

            string strEMRNO = "";

            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            ClearLine1Act();

            if (ComFunc.MsgBoxQ("삭제 후 복구는 불가능합니다. 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            if (FstrLineGubun == "말초정맥관")
            {
                strROWID = ssLineModify_Sheet1.Cells[2, 16].Text.Trim();
            }
            else
            {
                strROWID = ssLineModify_Sheet1.Cells[2, 21].Text.Trim();
            }

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT EMRNO";
                if (FstrLineGubun == "말초정맥관")
                {
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL ";
                }

                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strEMRNO = dt.Rows[0]["EMRNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strROWID != "")
                {
                    if (DeleteEmrXmlData(strEMRNO) == false)
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                        return;
                    }
                    else
                    {
                        if (FstrLineGubun == "말초정맥관")
                        {
                            SQL = " INSERT INTO KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL_DEL ";
                            SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                        }
                        else
                        {
                            SQL = " INSERT INTO KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL_DEL ";
                            SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL ";
                        }

                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (FstrLineGubun == "말초정맥관")
                        {
                            SQL = " DELETE KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL";
                        }
                        else
                        {
                            SQL = " DELETE KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL";
                        }

                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (FstrLineGubun == "말초정맥관")
                {
                    SSLINEP1_CLEAR();
                    btnSreachKeepLP1_Click(null, null);
                    btnSreachLP1_Click(null, null);
                }
                else
                {
                    SSLINE1_CLEAR();

                    btnSreachKeepL1_Click(null, null);
                    btnSreachL1_Click(null, null);
                }

                panActModify.Visible = false;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnCancelAct_Click(object sender, EventArgs e)
        {
            panActModify.Visible = false;
        }

        private void btnChangeAct2_Click(object sender, EventArgs e)
        {
            string strROWID = "";
            string strStartDate = "";

            string strEMRNO = "";

            string strACTSABUN = "";

            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            ClearLine1Act();

            if (FstrLineGubun == "")
            {
                ComFunc.MsgBox("정상적인 접근이 아닙니다. 전산정보과에 문의하시기 바랍니다.", "확인");
                return;
            }

            if (ComFunc.MsgBoxQ("수정 후 복구는 불가능합니다. 수정하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            strROWID = ssLineModify2_Sheet1.Cells[2, 14 + 1].Text.Trim();

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT SEQNO, EMRNO, ACTSABUN ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    Line1Act.SEQNO = dt.Rows[0]["SEQNO"].ToString().Trim();
                    strEMRNO = dt.Rows[0]["EMRNO"].ToString().Trim();
                    strACTSABUN = dt.Rows[0]["ACTSABUN"].ToString().Trim();
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("데이터가 존재 하지 않습니다.");
                    return;
                }

                dt.Dispose();
                dt = null;

                Line1Act.status = ssLineModify2_Sheet1.Cells[2, 0].Text.Trim();
                strROWID = ssLineModify2_Sheet1.Cells[2, 14 + 1].Text.Trim();

                Line1Act.IPDNO = Convert.ToString(mintIPDNO);
                Line1Act.Pano = FstrPtno;
                Line1Act.WardCode = cboWard.Text.Trim();
                Line1Act.RoomCode = FstrRoom;
                Line1Act.DeptCode = FstrDEPT;
                Line1Act.DrCode = FstrDrCode;
                Line1Act.InDate = FstrInDate;
                Line1Act.ACTDATE = ssLineModify2_Sheet1.Cells[2, 1].Text.Trim();
                Line1Act.ACTTIME = ssLineModify2_Sheet1.Cells[2, 2].Text.Trim();
                Line1Act.ACTSABUN = clsType.User.Sabun;

                SQL = "";
                SQL = " SELECT PART1, PART2, LOCATE1, LOCATE2, WARDCODE, INSERT_BUSE, ROOMCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                SQL = SQL + ComNum.VBLF + " WHERE SEQNO = " + Line1Act.SEQNO;
                SQL = SQL + ComNum.VBLF + "     AND ROWNUM = 1";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    Line1Act.PART1 = dt.Rows[0]["PART1"].ToString().Trim();
                    Line1Act.PART2 = dt.Rows[0]["PART2"].ToString().Trim();
                    Line1Act.LOCATE1 = dt.Rows[0]["LOCATE1"].ToString().Trim();
                    Line1Act.LOCATE2 = dt.Rows[0]["LOCATE2"].ToString().Trim();
                    Line1Act.INSERT_BUSE = dt.Rows[0]["INSERT_BUSE"].ToString().Trim();
                    Line1Act.WardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    Line1Act.RoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("에러 발생 전산실 문의하시기 바랍니다.", "확인");
                    return;
                }

                dt.Dispose();
                dt = null;

                Line1Act.MON1 = (Convert.ToBoolean(ssLineModify2_Sheet1.Cells[2, 6].Value) == true ? "1" : "0");
                Line1Act.MON2 = (Convert.ToBoolean(ssLineModify2_Sheet1.Cells[2, 7].Value) == true ? "1" : "0");
                Line1Act.MON3 = (Convert.ToBoolean(ssLineModify2_Sheet1.Cells[2, 8].Value) == true ? "1" : "0");
                Line1Act.MON4 = (Convert.ToBoolean(ssLineModify2_Sheet1.Cells[2, 9].Value) == true ? "1" : "0");
                Line1Act.MON6 = (Convert.ToBoolean(ssLineModify2_Sheet1.Cells[2, 10].Value) == true ? "1" : "0");
                Line1Act.MON5 = (Convert.ToBoolean(ssLineModify2_Sheet1.Cells[2, 11].Value) == true ? "1" : "0");
                Line1Act.BIGO = ssLineModify2_Sheet1.Cells[2, 12].Text.Trim();
                strStartDate = READ_STARTDATE(Line1Act.SEQNO);
                Line1Act.STARTDATE = VB.Left(strStartDate, 10);
                Line1Act.STARTTIME = VB.Right(strStartDate, 5);
                Line1Act.USEDATE = ssLineModify2_Sheet1.Cells[2, 13 + 1].Text.Trim();

                if (strROWID != "")
                {
                    if (DeleteEmrXmlData(strEMRNO) == false)
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                        return;
                    }
                    else
                    {
                        if (FstrLineGubun == "말초정맥관")
                        {
                            SQL = " INSERT INTO KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL_DEL ";
                            SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                        }
                        else
                        {
                            SQL = " INSERT INTO KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL_DEL ";
                            SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL ";
                        }

                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (FstrLineGubun == "말초정맥관")
                        {
                            SQL = " DELETE KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL";
                        }
                        else
                        {
                            SQL = " DELETE KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL";
                        }

                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                if (INSERT_LINE1X() == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                SSLINEP1_CLEAR();
                btnSreachKeepLP1_Click(null, null);
                btnSreachLP1_Click(null, null);

                panActModify2.Visible = false;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnDeleteAct2_Click(object sender, EventArgs e)
        {
            string strROWID = "";
            //string strStartDate = "";
            string strEMRNO = "";

            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            ClearLine1Act();

            if (ComFunc.MsgBoxQ("삭제 후 복구는 불가능합니다. 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            strROWID = ssLineModify2_Sheet1.Cells[2, 14 + 1].Text.Trim();

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT EMRNO";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strEMRNO = dt.Rows[0]["EMRNO"].ToString().Trim();
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("데이터가 존재하지 않습니다.");
                    return;
                }

                dt.Dispose();
                dt = null;

                if (strROWID != "")
                {
                    if (DeleteEmrXmlData(strEMRNO) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.", "확인");
                        return;
                    }
                    else
                    {
                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL_DEL ";
                        SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " DELETE KOSMOS_PMPA.NUR_LINE_ACT_PERIPHERAL";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("챠트 삭제 중 에러가 발생하였습니다. 전산실로 문의하시기 바랍니다.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                SSLINEP1_CLEAR();
                btnSreachKeepLP1_Click(null, null);
                btnSreachLP1_Click(null, null);

                panActModify2.Visible = false;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnCancelAct2_Click(object sender, EventArgs e)
        {
            panActModify2.Visible = false;
        }

        private void chkLinePChartDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLinePChartDate.Checked == true)
            {
                txtLine1PDate.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                txtLine1PTime.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
                txtLine1PDate.Visible = true;
                txtLine1PTime.Visible = true;
            }
            else
            {
                txtLine1PDate.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                txtLine1PTime.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
                txtLine1PDate.Visible = false;
                txtLine1PTime.Visible = false;
            }
        }

        private void chkLinePChartDate2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLinePChartDate2.Checked == true)
            {
                txtLine2PDate.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                txtLine2PTime.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
                txtLine2PDate.Visible = true;
                txtLine2PTime.Visible = true;
            }
            else
            {
                txtLine2PDate.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                txtLine2PTime.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
                txtLine2PDate.Visible = false;
                txtLine2PTime.Visible = false;
            }
        }

        private void Update_NurRemark(string argGBN = "")
        {
            //arggbn 에 OK라는 값이 있을 경우 강제 업데이트

            int intRow = ssM_Sheet1.ActiveRowIndex;
            int intCol = ssM_Sheet1.ActiveColumnIndex;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strROWID = "";
            string strMemo = "";
            string strMEMOOLD = "";

            string strOrdercode = "";

            strOrdercode = ssM_Sheet1.Cells[intRow, 2].Text.Trim();

            if (intCol == 49 || argGBN == "OK")
            {
                strROWID = ssM_Sheet1.Cells[intRow, 47].Text.Trim();
                strMemo = ssM_Sheet1.Cells[intRow, 49].Text.Trim().Replace("'", "`");
                strMEMOOLD = ssM_Sheet1.Cells[intRow, 52].Text.Trim().Replace("'", "`");

                if (strMemo != strMEMOOLD)
                {
                    if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                    {
                        ComFunc.MsgBox("현재 프로그램의 저장/수정 권환이 없습니다.");
                        return; //권한 확인
                    }

                    Cursor.Current = Cursors.WaitCursor;
                    clsDB.setBeginTran(clsDB.DbCon);

                    try
                    {
                        SQL = "";
                        SQL = " UPDATE KOSMOS_OCS.OCS_IORDER SET NURREMARK = '" + strMemo + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("참고사항 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(ex.Message);
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void ssM_EditModeOff(object sender, EventArgs e)
        {
            Update_NurRemark();
        }

        private void frmActing_FormClosed(object sender, FormClosedEventArgs e)
        {
            //lock 관리
            if (clsLockCheck.GstrLockPtno != "")
            {
                clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
            }

            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        private void frmActing_Activated(object sender, EventArgs e)
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

        private void cboTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnView_Click(null, null);
        }

        private void dtpDate1_ValueChanged(object sender, EventArgs e)
        {
            btnView_Click(null, null);
        }

        private void cboJob_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnView_Click(null, null);
        }

        private void cboDrCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnView_Click(null, null);
        }

        private void chkInfec_CheckedChanged(object sender, EventArgs e)
        {
            btnView_Click(null, null);
        }

        private void chkTransfor_CheckedChanged(object sender, EventArgs e)
        {
            btnView_Click(null, null);
        }

        private void cboMM_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cboHH_TextUpdate(object sender, EventArgs e)
        {
            if (cboHH.Text.Trim() != "" && VB.IsNumeric(cboHH.Text) == false)
            {
                ComFunc.MsgBox("정확한 시을 입력해 주세요");
                cboHH.Text = "";
                return;
            }

            if (VB.Val(cboHH.Text) > 24 || VB.Val(cboHH.Text) < 0)
            {
                ComFunc.MsgBox("정확한 시을 입력해 주세요");
                cboHH.Text = "";
            }

            //if (cboHH.Text.Trim().Length < 2)
            //{
            //    ComFunc.MsgBox("정확한 시을 입력해 주세요." + ComNum.VBLF + " 예) 6시의 경우 '06', 1시의 경우 '01'");
            //    cboHH.Text = "";
            //}
        }

        private void cboMM_TextUpdate(object sender, EventArgs e)
        {
            if (cboMM.Text.Trim() != "" && VB.IsNumeric(cboMM.Text) == false)
            {
                ComFunc.MsgBox("정확한 분을 입력해 주세요");
                cboHH.Text = "";
                return;
            }

            if (VB.Val(cboMM.Text) > 59 || VB.Val(cboMM.Text) < 0)
            {
                ComFunc.MsgBox("정확한 분을 입력해 주세요");
                cboHH.Text = "";
            }
        }

        //TODO 환자 정보창에 추가 되기 전까지 이용함 - 한곳에서 지우기 위해 여기서 다 선언 함.. - 영록
        //로드 부분과 환자 선택 부분에 각각 frmPatientInfoX.rGetDate() 사용
        //pSubFormToControl는 로드 부분에서 사용

        private frmPatientInfo frmPatientInfoX = new frmPatientInfo();

        private void pSubFormToControl(Form frm, Control pControl)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.Text = "";
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            frm.Dock = DockStyle.Fill;
            frm.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift == true)
            {
                if (e.KeyCode == Keys.H)
                {
                    if (ssM_Sheet1.Columns.Get(38).Visible == false)
                    {
                        ssM_Sheet1.Columns.Get(47).Visible = true; //'ROWID
                        ssM_Sheet1.Columns.Get(48).Visible = true; //'ORDERNO
                        ssM_Sheet1.Columns.Get(50).Visible = true; //'itemcd
                        ssM_Sheet1.Columns.Get(52).Visible = true; //'참고사항OLD
                    }
                    else
                    {
                        ssM_Sheet1.Columns.Get(47).Visible = false; //'ROWID
                        ssM_Sheet1.Columns.Get(48).Visible = false; //'ORDERNO
                        ssM_Sheet1.Columns.Get(50).Visible = false; //'itemcd
                        ssM_Sheet1.Columns.Get(52).Visible = false; //'참고사항OLD
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VB.SaveSetting("TWIN", "NURVIEW", "WARDCODE", "OP");
        }

        private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {

            cboJob.Items.Clear();
            chkERgbn.Visible = false;

            if (cboWard.Text.Trim() == "ER")
            {
                cboJob.Items.Add("1.액팅대상리스트");
                cboJob.Items.Add("2.액팅완료리스트");
                cboJob.Items.Add("3.당일내원리스트");
                cboJob.Items.Add("4.당일입원리스트");
                cboJob.Items.Add("5.당일퇴원리스트");
                cboJob.Items.Add("6.당일내시경환자리스트");
                chkERgbn.Visible = true;
            }
            else
            {
                cboJob.Items.Add("1.재원자명단");
                cboJob.Items.Add("2.당일입원자");
                cboJob.Items.Add("3.퇴원예고자");
                cboJob.Items.Add("4.당일퇴원자");
                cboJob.Items.Add("G.응급실경유환자(재원중)");
                cboJob.Items.Add("L.C-Line 유지 환자");
                cboJob.Items.Add("M.항암제투여자");
                cboJob.Items.Add("5.중증도미분류");
                cboJob.Items.Add("6.수술예정자");
                cboJob.Items.Add("7.진단명 누락자");
                cboJob.Items.Add("A.응급실경유입원(1-3일전)");
                cboJob.Items.Add("B.재원기간 7-14일 환자");
                cboJob.Items.Add("C.재원기간 3-7일 환자");
                cboJob.Items.Add("D.어제퇴원자");
                cboJob.Items.Add("E.자가약");
                cboJob.Items.Add("F.기타투약기록");
            }

            cboJob.SelectedIndex = 0;
        }

        private void cboHH_Leave(object sender, EventArgs e)
        {
            if (cboHH.Text.Trim().Length < 2)
            {
                cboHH.Text = "0" + cboHH.Text.Trim();
            }
        }

        private void cboMM_Leave(object sender, EventArgs e)
        {
            if (cboMM.Text.Trim().Length < 2)
            {
                cboMM.Text = "0" + cboMM.Text.Trim();
            }
        }

        private void txtSelname2_Enter(object sender, EventArgs e)
        {
            txtSelname2.Text = "";
        }
    }
}