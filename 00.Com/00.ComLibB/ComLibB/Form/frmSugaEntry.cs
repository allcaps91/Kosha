using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComBase; //기본 클래스


namespace ComLibB
{
    /// <summary>
    /// /// <summary>
    /// Class Name : frmSugaEntry
    /// File Name : frmSugaEntry.cs
    /// Title or Description : 수가등록
    /// Author : 유진호
    /// Create Date : 2017-11-01
    /// Update Histroy :     
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso>
    /// VB\basic\busuga\Busuga01.frm(FrmSugaEntry)
    /// </seealso>    
    /// </summary>
    public partial class frmSugaEntry : Form, MainFormMessage
    {
        #region //MainFormMessage
        string mPara1 = "";
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


        string GstrHelpCode = "";

        struct TABLE_EDI_SUGA 
        {
            public string ROWID;
            public string Code;
            public string Jong;
            public string Pname;
            public string Bun;
            public string Danwi1;
            public string Danwi2;
            public string Spec;
            public string COMPNY;
            public string Effect;
            public string Gubun;
            public string Dangn;
            public string JDate1;
            public string Price1;
            public string JDate2;
            public string Price2;
            public string JDate3;
            public string Price3;
            public string JDate4;
            public string Price4;
            public string JDate5;
            public string Price5;
        }
        TABLE_EDI_SUGA TES;

        #region // 변수선언
        private int FnBAmt;
        private int FnTAmt;
        private int FnIAmt;

        private string FstrBCode;
        private string FstrSuHam;
        private string FstrEdiJong;
        private string FstrSuNext;
        private string FstrOldBun;      //'오더판넬에 분류기호 변경여부
        private bool FnCodeChange;
        private string FstrSutROWID;
        private int nCols;              //'표준수가 ss3.col
        private int nRows;              //'표준수가 ss3.row

        //ss2 클립보드 용
        private int intClipRow = 0;
        private int intClipRowCnt = 0;
        //

        private string GstrDrugCode = "";
        private string GstrBCodeShow = "";
        private string GstrSanCode = "";


        private bool FailFlag;

        private int nCol;

        private string strDel;
        private string strChange;


        private string strBun;
        private string strNu;
        private string strSuNext;
        private string strSunameK;
        private string strSunameE;
        private string strSunameG;
        private double nSuham;
        private string strUnit;
        private string strDaiCode;
        private string strHCode;
        private string strBCode;
        private string strEdiJong;
        private string strEdiDate;
        private string strOldJong;
        private string strOldBCode;
        private double nOldGesu;
        private string strWonCode;
        private string strGBWON1;
        private string strGBWON2;
        private string strGbYebang;             //As String *1
        private string strGbCsInfo;             //As String *1
        private string strGbSimli;              //As String *1
        private string strDtlBun;               //As String *4 '수가 상세분류
        private string strGbYeyak;              //As String *2 '예약선수금 분류
        private string strSelect;               //'선택진료구분
        private string strGbSugbF;              //'심사과 관리코드(비급여)
        private string strGbAnti;               //'항생제 관리여부
        private string strGbGoji;               //'고지혈증관리약제
        private string strGbGanJang;            //'간장용제관리약제
        private string strGbRare;               //'희귀난치성질환
        private string strGBBone;               //'골다공증
        private string strGbAntiCan;            //'항암제
        private string strGbPPI;                //'PPI제제
        private string strGBDementia;           //'치매약제
        private string strGBDiabetes;           //'당뇨약제
        private string strGBDrug;               //'저가약제관리
        private string strGBOCSF;               //'OCS 급여 가능
        private string strGBWonF;               //'원무과 급여 전환 메세지
        private string strGBGABA;               //'GABAPENTIN 계열
        private string strGBDrugNO;             //'저가약제 제외
        private string strGBNS;                 //'신경차단술
        private string strGBOCSDrug;            //'향정신성 ocs 사유
        private string strGbOpRoom;             //'수술적예방대상(항생제)
        private string strGBTax;                //'부과세대상
        private string strGbTB;                 //'항결핵(지원금)
        private string strBlood;                //'혈우약제
        private string strGbMT004;              //'MT004
        private string strGbSelfHang;           //비급여 고지항목 2020-11-27
        private string strGbTaHPSUGA;           //타병원 수가 체크 2021-04-26
        private string strHangJungJuSa;             //항정신장기주사제
        private string strBunSukSimSa;          //분석심사
        private string strDRGBunHang;           //질병군분류항

        private string strDRG100;               //'DRG 100/100
        private string strDRGF;                 //'DRG 비급여
        private string strDRGADD;               //'DRG 외과가산
        private string strDRGCode;              //'DRG 코드
        private string strDRGOpen;              //'DRG 복강개방
        private string strDRGOGADD;             //'DRG 산부인과 가산(30%)
        private string strDRGBOSANG;             //'DRG 산부인과 가산(30%)
        private string strDRGOT;

        private string strEdiDate3;
        private string strBcode3;
        private double strGesu3;
        private string strEdiJong3;
        private string strEdiDate4;
        private string strBcode4;
        private double strGesu4;
        private string strEdiJong4;
        private string strEdiDate5;
        private string strBcode5;
        private double strGesu5;
        private string strEdiJong5;

        private string strSugbA;
        private string strSugbB;
        private string strSugbC;
        private string strSugbD;
        private string strSugbE;
        private string strSugbF;
        private string strSugbG;
        private string strSugbH;
        private string strSugbI;
        private string strSugbJ;
        private string strSugbK;
        private string strSugbL;
        private string strSugbM;
        private string strSugbN;
        private string strSugbO;
        private string strSugbP;
        private string strSugbQ;
        private string strSugbR;
        private string strSugbS;
        private string strSugbT;
        private string strSugbU;
        private string strSugbV;
        private string strSugbW;
        private string strSugbX;
        private string strSugbY;
        private string strSugbZ;
        private string strSugbAA;


        private string strSugbAB;      //'2017-06-20
        private string strSugbAC;
        private string strSugbAD;
        private string strSugbAE;
        private string strSugbAF;
        private string strSugbAG;   //2019-08-26

        private string strSugbSS;
        private string strSugbBi;

        private double nSuQty;
        private int nBAmt;
        private int nTAmt;
        private int nIAmt;
        private int nSAmt;
        private int nSelAmt;
        private string strSuDate;
        private int nOldBAmt;
        private int nOldTAmt;
        private int nOldIAmt;
        private string strSuDate3;
        private int nBAmt3;
        private int nTAmt3;
        private int nIAmt3;
        private string strSuDate4;
        private int nBAmt4;
        private int nTAmt4;
        private int nIAmt4;
        private string strSuDate5;
        private int nBAmt5;
        private int nTAmt5;
        private int nIAmt5;
        private int nSORT;
        private int strBiGoAmt = 0;// 2020-01-02 수가안말리게 변수 사용

        private string strNurCode;

        private string strSuhROWID;
        private int nCNT;
        string[,] strSuga;
        #endregion

        #region // 클래스 선언
        private FrmSugaSerch FrmSugaSerchX = null;                  // 수가코드 찾기
        private frmSCode frmSCodeX = null;                          // 동일성분 조회
        private frmSearchUnpaid frmSearchUnpaidX = null;            // 비급여 항목조회
        private frmSearchUnpaid2 frmSearchUnpaidX2 = null;            // 비급여 항목조회
        private frmTACostView frmTACostViewX = null;                // 자보 비용산정 조회
        private frmSearchBCode frmSearchBCodeX = null;              // 표준코드 찾기
        private frmSearchGuip frmSearchGuipX = null;                // 구입신고 내역 조회
        private frmYGuipView frmYGuipViewX = null;                  // 의약품 실구입 신고내역 조회
        private frmGiSugaHelp frmGiSugaHelpX = null;		        // 처치재료수가코드찾기
        private frmSugaCompare frmSugaCompareX = null;		        // 표준수가와비교
        private frmBasXray frmBasXrayX = null;			            // 방사선단순촬영변환        
        private frmViewGoji frmViewGojiX;                           // 특정약제조회(고지혈약제)
        private frmBloodSuga frmBloodSugaX = null;                  // 혈액은행 수가매핑 조회
        private frmWonhangHelp frmWonhangHelpX = null;              // 원가 항목 찾기
        private frmNurCodeHelp frmNurCodeHelpX = null;              // 간호활동 조회
        private frmYAKHelp frmYAKHelpX = null;                      // 약품 분류 찾기
        private frmAntiHelp frmAntiHelpX = null;                    // 항생제 계열 찾기
        private PmpaMirBunHelp PmpaMirBunHelpX = null;              // 분류찾기
        private PmpaMirNUHelp PmpaMirNUHelpX = null;                // 누적찾기
        private frmPmpaMirJemsuEntry frmPmpaMirJemsuEntryX = null;  // 상대가치점수 등록
        private frmJunCodeEntry frmJunCodeEntryX = null;            // 준용코드 산출식 등록
        private frmDrgBaseCode frmDrgBaseCodeX = null;              // DRG 기초코드 관리
        private frmSugaList frmSugaListX = null;                    // 수가코드 목록조회
        private frmSugaDayCount frmSugaDayCountX = null;            // 약품일수관리
        private frmDrJob01 frmDrJob01X = null;                      // 약품처방일수관리(약제과용)
        private frmOcsMsgPano_Mir frmOcsMsgPano_MirX = null;        // 심사과환자메세지등록(외래환자)-청구참고사항

        private frmOcsMessage frmOcsMessageX = null;
        private FrmOcsMsg FrmOcsMsgx = null;
        private FrmOcsMsgPano_I FrmOcsMsgPano_IX = null;
        private FrmOcsMsgPano_O FrmOcsMsgPano_OX = null;
        private FrmOcsMsgPano_O2 FrmOcsMsgPano_O2X = null;

        private frmSimsaInfor frmSimsaInforX = null;
        private frmSimsaInfor_File frmSimsaInfor_FileX = null;
        private frmSimsaInfor_Ward frmSimsaInfor_WardX = null;
        private frmSimsaInfor_MIR frmSimsaInfor_MIRX = null;

        private frmHoanBul frmHoanBulX = null;
        private frmMsgSend frmMsgSendX = null;
        #endregion

        public frmSugaEntry(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmSugaEntry(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }

        public frmSugaEntry()
        {
            InitializeComponent();
        }

        public frmSugaEntry(string strHelpCode)
        {
            InitializeComponent();
            GstrHelpCode = strHelpCode;  
        }

        private void frmSugaEntry_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            WindowState = FormWindowState.Maximized;

            clsPublic.GnJobSabun = long.Parse(clsType.User.Sabun);

            strSuga = new string[5, 5];
            TES = new TABLE_EDI_SUGA();
            ComFunc CF = new ComFunc();

            ss2_Sheet1.Columns[5].Visible = false;  //'항
            ss2_Sheet1.Columns[6].Visible = false;  //'목
            ss2_Sheet1.Columns[18].Visible = false; //'한글수가

            ss2_Sheet1.Columns[75 + 3].Visible = false; //'변경여부
            ss2_Sheet1.Columns[76 + 3].Visible = false; //'최초BAmt
            ss2_Sheet1.Columns[77 + 3].Visible = false; //'최초TAmt
            ss2_Sheet1.Columns[78 + 3].Visible = false; //'최초IAmt

            ss2_Sheet1.Columns[81 + 3].Visible = false; //'순위
            ss2_Sheet1.Columns[94 + 3].Visible = false; //'ROWID

            ss2_Sheet1.RowCount = 30;

            ss4_Sheet1.Columns[4].Visible = false;

            SCREEN_CLEAR();
            ComFunc.ReadSysDate(clsDB.DbCon);
            CF.Combo_BCode_SET(clsDB.DbCon, cboDtlBun, "BAS_수가상세분류", true, 1);
            CF.Combo_BCode_SET(clsDB.DbCon, cboGbYeyak, "ETC_통합예약분류", true, 1);


            cboSelect.Items.Clear();
            cboSelect.Items.Add("*.적용안함");
            cboSelect.Items.Add("1.진찰");
            cboSelect.Items.Add("2.의학관리");
            cboSelect.Items.Add("3.검사");
            cboSelect.Items.Add("4.영상진단 및 방사선치료");
            cboSelect.Items.Add("5.마취");
            cboSelect.Items.Add("6.정신요법");
            cboSelect.Items.Add("7.처치수술");

            ComboUnit_Set();

            //2019-10-11
            cboDrg100.Items.Clear();
            cboDrg100.Items.Add("Y.100/100");
            cboDrg100.Items.Add("2.100/20(임시)");  
            cboDrg100.Items.Add("4.100/80(비급여)");
            cboDrg100.Items.Add("5.100/50");
            cboDrg100.Items.Add("6.100/80(중복)");

            //2019-12-31
            cboBosang.Items.Clear();
            cboBosang.Items.Add("      ");
            cboBosang.Items.Add("1. 1.0");
            cboBosang.Items.Add("2. 0.8");

            //2020-01-13 안과인공수정체
            cboDrgOT.Items.Clear();
            cboDrgOT.Items.Add("");
            cboDrgOT.Items.Add("1.인공수정체");

            txtSuDate.Text = clsPublic.GstrSysDate;
            lblViewOnly.Visible = false;

            if (GstrHelpCode != "")
            {
                txtCode.Text = GstrHelpCode;
                Screen_Display();
                lblMsg.Text = "";
                GstrHelpCode = "";
                txtGbA.Focus();
            }

            cboCode.Items.Clear();

            if (clsPublic.GstrPassProgramID == "BVSUGA")
            {
                this.Text = "수가코드 조회";
                this.btnSave.Visible = false;
                this.btnDelete.Visible = false;
                this.btnChangeCode.Visible = false;
                this.btnSearchSelf.Visible = false;
                this.btnCancel.Text = "다시조회";
                this.mnuExit.Text = "수가조회종료";
                this.lblViewOnly.Top = 120;
                this.lblViewOnly.Left = 6250;
                this.panel2.Enabled = false;

                this.mnuJemsu.Visible = false;
                this.mnuSanjeng.Visible = false;
                this.mnuMessage.Visible = false;
                this.mnuHoanBul.Visible = false;
                this.mnuSimSaInfor00.Visible = false;
                this.mnuView5.Visible = false;
                //this.mnuView6.Visible = false;
            }

            switch (clsType.User.Sabun)
            {
                case "16412":
                    this.mnuSimSaInfor00.Visible = true;
                    break;
                case "19684":
                    this.mnuSimSaInfor00.Visible = true;
                    break;
                case "18266":
                    this.mnuSimSaInfor00.Visible = true;
                    break;
            }

            CF = null;

            switch (clsType.User.Sabun)
            {
                case "45316":
                    NoUSER.Visible = true;
                    break;
                default:
                    NoUSER.Visible = false;
                    break;
            }

            //2021-10-20 심사팀장 요청으로 안보이게 작업
            chkAnti.Visible = false;
        }

        private void frmSugaEntry_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmSugaEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(frmSugaListX != null)
            {
                frmSugaListX.Dispose();
                frmSugaListX = null;
            }

            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void UnloadActiveForm(Form frm)
        {
            if (frm != null) 
            {
                frm.Dispose();
                frm = null;
            }
        }

        private void Read_DRUG_JEP_REQ_DETAIL(string argSUNEXT)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = " SELECT CERT, CONTENTS, BUN, LTDNAME, BCODE, JEPCODE, JEPNAME, UNIT1, UNIT2, UNIT3, UNIT4, PART_J,";
                SQL = SQL + ComNum.VBLF + "    PART_F, PART_U, PART_O, SDATE, BIGO, PRICE, JEPENAME, SUNGBUN, DOSCODE, JEHYUNG ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_JEP_REQ_DETAIL ";
                SQL = SQL + ComNum.VBLF + " WHERE JEPCODE = '" + argSUNEXT + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtSuNext.Text = dt.Rows[0]["JEPCODE"].ToString().Trim();   //'품명코드
                    txtDaiCode.Text = dt.Rows[0]["Bun"].ToString().Trim();      //'약품분류
                    lblDaiCode.Text = READ_DaicodeName(txtDaiCode.Text);
                    txtSuNameK.Text = VB.Replace(dt.Rows[0]["JEPNAME"].ToString().Trim(), ComNum.VBLF, "");      //'한글명
                    txtSuNameE.Text = VB.Replace(dt.Rows[0]["JEPENAME"].ToString().Trim(), ComNum.VBLF, "");    //'영문명
                    txtSuNameG.Text = VB.Replace(dt.Rows[0]["SUNGBUN"].ToString().Trim(), ComNum.VBLF, "");     //'성분명

                    txtUnit_New1.Text = dt.Rows[0]["UNIT1"].ToString().Trim();  //'약제용량
                    cboUnit_New2.Text = dt.Rows[0]["UNIT2"].ToString().Trim();  //'약제용량
                    cboUnit_New3.Text = dt.Rows[0]["UNIT3"].ToString().Trim();  //'제형
                    txtUnit_New4.Text = dt.Rows[0]["UNIT4"].ToString().Trim();  //부피   2019-08-16 김해수 부피 추가

                    txtWon.Text = "1601";
                    lblWon.Text = READ_WonName(txtWon.Text);


                    ss3_Sheet1.Cells[1, 0].Text = dt.Rows[0]["sDate"].ToString().Trim(); // '적용일자
                    ss3_Sheet1.Cells[2, 0].Text = dt.Rows[0]["BCode"].ToString().Trim(); // '표준코드
                    ss3_Sheet1.Cells[3, 0].Text = "1"; // '계수
                    ss3_Sheet1.Cells[4, 0].Text = "3"; // '종료


                    ss1_Sheet1.Cells[1, 0].Text = dt.Rows[0]["sDate"].ToString().Trim(); // '적용일자
                    ss1_Sheet1.Cells[2, 0].Text = dt.Rows[0]["PRICE"].ToString().Trim(); // '단가
                    ss1_Sheet1.Cells[3, 0].Text = dt.Rows[0]["PRICE"].ToString().Trim(); // '단가
                    ss1_Sheet1.Cells[4, 0].Text = dt.Rows[0]["PRICE"].ToString().Trim(); // '단가
                     

                    txtGbJ.Text = dt.Rows[0]["PART_J"].ToString().Trim();   //'J항
                    if (VB.Trim(txtGbJ.Text) == "1")     //'2018-01-25
                    {
                        txtSuNameK.Text = VB.Trim(txtSuNameK.Text) + "(원외)";
                    }
                    txtGbF.Text = dt.Rows[0]["PART_F"].ToString().Trim();   //'F항
                    txtGbO.Text = dt.Rows[0]["PART_O"].ToString().Trim();   //'O항
                    txtGbA.Text = "1";  //'A항
                    txtGbL.Text = "3";  //'L항
                    txtGbV.Text = "1";  //'V항
                    txtGbK.Text = "1";  //'K항
                    txtGbU.Text = dt.Rows[0]["PART_U"].ToString().Trim(); //U항 2019-08-16 김해수

                    switch (VB.Left(dt.Rows[0]["JEHYUNG"].ToString().Trim(), 1))
                    {
                        case "1":   //'경구
                            txtBun.Text = "11";
                            lblBun.Text = Read_BunName(txtBun.Text);
                            txtNu.Text = "04";
                            lblNu.Text = READ_NuName(txtNu.Text);
                            txtGbB.Text = "1";  //'B항
                            break;
                        case "2":
                            txtBun.Text = "20";
                            lblBun.Text = Read_BunName(txtBun.Text);
                            txtNu.Text = "05"; 
                            lblNu.Text = READ_NuName(txtNu.Text);
                            break;
                        case "3":
                            txtBun.Text = "12";
                            lblBun.Text = Read_BunName(txtBun.Text);
                            txtNu.Text = "04";
                            lblNu.Text = READ_NuName(txtNu.Text);
                            break;
                    }
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
        }


        private string GBSELECT_SET(string ArgSugbE, string argBun, string ArgWonCode)
        {
            string rtnVal = "";
            int iBun = 0;

            try
            {
                iBun = Convert.ToInt32(VB.Val(argBun));

                if (iBun == 1 || iBun == 2)
                {
                    rtnVal = "1";
                }
                else if (iBun >= 3 && iBun <= 10)
                {
                    rtnVal = "2";
                }
                else if (iBun == 22 || iBun == 23)
                {
                    rtnVal = "5";
                }
                else if (iBun == 26 || iBun == 27)
                {
                    rtnVal = "6";
                }
                else if (iBun == 65 || iBun == 70)
                {
                    if (ArgWonCode == "1447")
                    {
                        rtnVal = "4";
                    }
                }
                else if (iBun == 72 || iBun == 73)
                {
                    rtnVal = "4";
                }
                else if (iBun >= 28 || iBun <= 51)
                {
                    rtnVal = "7";
                }
                else
                {
                    rtnVal = "";
                }
            }
            catch
            {
                return rtnVal;
            }


            return rtnVal;
        }


        private bool SUGA_AMT_INSERT(string ArgSuCode, string argSUNEXT, string ArgDate, long ArgBAmt, long ArgTAmt, long ArgIAmt, long ArgSAmt = 0)
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;


            if (ArgDate == "" && (ArgBAmt != 0 || ArgTAmt != 0 || ArgIAmt != 0))
            {
                ArgDate = "1800-01-01";
            }

            if (ArgDate == "")
            {
                return true;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT BAMT, TAMT, IAMT, ROWID FROM KOSMOS_PMPA.BAS_SUGA_AMT ";
                SQL = SQL + ComNum.VBLF + " WHERE SUCODE = '" + ArgSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SUNEXT = '" + argSUNEXT + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SUDATE = TO_DATE('" + ArgDate + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count == 0)
                {
                    // '등록                      
                    SQL = " INSERT INTO  KOSMOS_PMPA.BAS_SUGA_AMT ( SUCODE, SUNEXT , SUDATE, BAMT, TAMT, IAMT) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " '" + ArgSuCode + "','" + argSUNEXT + "' , TO_DATE('" + ArgDate + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + " '" + ArgBAmt + "', '" + ArgTAmt + "',  '" + ArgIAmt + "'  )  ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                }
                else
                {
                    //'수정 금액비교  
                    string strROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                    SQL = "UPDATE KOSMOS_PMPA.BAS_SUGA_AMT SET ";
                    SQL = SQL + ComNum.VBLF + "  SUDATE  = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "  BAMT = '" + ArgBAmt + "', ";
                    SQL = SQL + ComNum.VBLF + "  TAMT = '" + ArgTAmt + "', ";
                    SQL = SQL + ComNum.VBLF + "  IAMT = '" + ArgIAmt + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
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
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }


        private void SUGA_AMT_READ(string ArgSuCode, string argSUNEXT, FarPoint.Win.Spread.FpSpread ArgSS)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            //'로직 수정 금액  테이블 변경

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "SELECT SUDATE, IAMT, BAMT , TAMT,  SAMT, SELAMT, ROWID  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUGA_AMT ";
                SQL = SQL + ComNum.VBLF + " WHERE SUCODE = '" + ArgSuCode + "'";
                SQL = SQL + ComNum.VBLF + "   AND SUNEXT = '" + argSUNEXT + "'";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SUDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgSS.ActiveSheet.Cells[0, 1, ArgSS.ActiveSheet.RowCount - 1, ArgSS.ActiveSheet.ColumnCount - 1].Text = "";

                    if (ArgSS.ActiveSheet.ColumnCount < dt.Rows.Count)
                    {
                        ArgSS.ActiveSheet.ColumnCount = dt.Rows.Count;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ArgSS.ActiveSheet.Cells[1, i].Text = dt.Rows[i]["SuDate"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[2, i].Text = dt.Rows[i]["BAmt"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[3, i].Text = dt.Rows[i]["TAmt"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[4, i].Text = dt.Rows[i]["IAmt"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[5, i].Text = dt.Rows[i]["SAMT"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[6, i].Text = dt.Rows[i]["SELAMT"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[7, i].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
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
        }


        private void SUGA_DRG_ADD_AMT_READ(string ArgSuCode, string argSUNEXT, FpSpread ArgSS)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            //'로직 수정 금액  테이블 변경

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "SELECT SUDATE, JUAMT, BUAMT , DUAMT,  ROWID  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUGA_DRGADD ";
                SQL = SQL + ComNum.VBLF + " WHERE SUCODE = '" + ArgSuCode + "'";
                SQL = SQL + ComNum.VBLF + "   AND SUNEXT = '" + argSUNEXT + "'";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SUDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgSS.ActiveSheet.Cells[0, 1, ArgSS.ActiveSheet.RowCount - 1, ArgSS.ActiveSheet.ColumnCount - 1].Text = "";

                    if (ArgSS.ActiveSheet.ColumnCount < dt.Rows.Count)
                    {
                        ArgSS.ActiveSheet.ColumnCount = dt.Rows.Count;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ArgSS.ActiveSheet.Cells[1, i].Text = dt.Rows[i]["SuDate"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[2, i].Text = dt.Rows[i]["JUAmt"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[3, i].Text = dt.Rows[i]["BUAmt"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[4, i].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[5, i].Text = dt.Rows[i]["DUAmt"].ToString().Trim();
                    }
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
        }

        private void SUGA_DRG_ADD_AMT_READ_NEW(string ArgSuCode, string argSUNEXT, FpSpread ArgSS)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            //'로직 수정 금액  테이블 변경

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "SELECT TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE, DRGF, DRG100 , ROWID  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUGA_DRGADD_NEW ";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + argSUNEXT + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY SUDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgSS.ActiveSheet.Cells[0, 1, ArgSS.ActiveSheet.RowCount - 1, ArgSS.ActiveSheet.ColumnCount - 1].Text = "";

                    if (ArgSS.ActiveSheet.ColumnCount < dt.Rows.Count)
                    {
                        ArgSS.ActiveSheet.ColumnCount = dt.Rows.Count;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ArgSS.ActiveSheet.Cells[1, i].Text = dt.Rows[i]["SuDate"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[2, i].Text = dt.Rows[i]["DRGF"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[3, i].Text = dt.Rows[i]["DRG100"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[4, i].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
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
        }


        private void ComboUnit_Set()
        {
            cboUnit_New2.Items.Clear();
            cboUnit_New2.Items.Add("mg");
            cboUnit_New2.Items.Add("g");
            cboUnit_New2.Items.Add("ml");
            cboUnit_New2.Items.Add("L");
            cboUnit_New2.Items.Add("cc");
            cboUnit_New2.Items.Add("iu");
            cboUnit_New2.Items.Add("만iu");
            cboUnit_New2.Items.Add("mEq");
            cboUnit_New2.Items.Add("mcg");
            cboUnit_New2.Items.Add("cm");
            cboUnit_New2.Items.Add("ug");
            cboUnit_New2.Items.Add("매");


            cboUnit_New3.Items.Clear();
            cboUnit_New3.Items.Add("T");
            cboUnit_New3.Items.Add("V");
            cboUnit_New3.Items.Add("A");
            cboUnit_New3.Items.Add("C");
            cboUnit_New3.Items.Add("BT");
            cboUnit_New3.Items.Add("cap");
            cboUnit_New3.Items.Add("dose");
            cboUnit_New3.Items.Add("EA");
            cboUnit_New3.Items.Add("PK");
            cboUnit_New3.Items.Add("PFS");
            cboUnit_New3.Items.Add("set");
            cboUnit_New3.Items.Add("bag");

            cboUnit_New3.Items.Add("정");
            cboUnit_New3.Items.Add("통");
            cboUnit_New3.Items.Add("포");
        }

        private void SCREEN_CLEAR()
        {
            lblMsg.Text = "";
            txtCode.ReadOnly = false;
            FnCodeChange = false;
            txtCode.Text = "";
            txtGbA.Text = "1"; txtGbB.Text = "0"; txtGbC.Text = "0";
            txtGbD.Text = "0"; txtGbE.Text = "0"; txtGbF.Text = "0";
            txtGbG.Text = "1"; txtGbH.Text = "0"; txtGbI.Text = "0";
            txtGbJ.Text = "0"; txtGbK.Text = "0"; txtGbL.Text = "1";
            txtGbM.Text = "0"; txtGbN.Text = "0"; txtGbO.Text = "0";
            txtGbP.Text = "0"; txtGbQ.Text = "0"; txtGbR.Text = "0";
            txtGbS.Text = "0"; txtGbT.Text = "0"; txtGbU.Text = "0";
            txtGbV.Text = "0"; txtGbW.Text = "0"; txtGbZ.Text = "0";
            txtGbAA.Text = "0";
            txtGbAB.Text = "0";
            txtGbAC.Text = "0";
            txtGbAD.Text = "0";
            txtGbAE.Text = "0";
            txtGbAF.Text = "0";
            txtGbAG.Text = "0";

            txtBun.Text = ""; lblBun.Text = "";
            txtNu.Text = ""; lblNu.Text = "";
            txtSuNext.Text = ""; txtUnit.Text = ""; txtHCode.Text = "";
            txtDelDate.Text = ""; txtSuNameK.Text = ""; txtSuNameG.Text = "";
            txtSuNameE.Text = ""; txtDayMax.Text = ""; txtTotMax.Text = "";
            txtDaiCode.Text = ""; txtWon.Text = ""; txtAntiClass.Text = "";

            cboSuNext.Items.Clear();

            txtGbP.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(235)))));

            txtUnit_New1.Text = "";
            cboUnit_New2.Text = "";
            cboUnit_New3.Text = "";
            cboSelect.Text = "";
            txtUnit_New4.Text = "";
            txtMemo.Text = "";
            txtBigo.Text = ""; //2019-08-19 KHS 비고란추가

            cboDtlBun.SelectedIndex = -1;
            cboGbYeyak.SelectedIndex = -1;
            txtNurCode.Text = "";
            lblNurCode.Text = "";

            lblWon.Text = "";
            lblDaiCode.Text = "";
            lblAntiClass.Text = "";
            chkWon1.Checked = false;
            chkWon2.Checked = false;
            chkYebang.Checked = false;
            chkCsInfo.Checked = false;
            chkSimli.Checked = false;
            chkSelf.Checked = false;
            chkAnti.Checked = false;
            chkGoji.Checked = false;
            chkGanJang.Checked = false;
            chkRare.Checked = false;
            chkBone.Checked = false;
            chkAntiCan.Checked = false;
            chkPPI.Checked = false;
            chkDementia.Checked = false;
            chkDiabetes.Checked = false;
            chkDrug.Checked = false;
            chkOCSF.Checked = false;
            chkWonF.Checked = false;
            chkGaBa.Checked = false;
            chkDrugNo.Checked = false;
            chkOCSDrug.Checked = false;
            chkNS.Checked = false;
            chkOCSDrug.Checked = false;
            chkOpRoom.Checked = false;
            chkGbTax.Checked = false;
            chkGbTB.Checked = false;
            chkGbSelfHang.Checked = false; //2020-11-27
            chkBSSimSa.Checked = false; //2021-03-25
            chkTAHPSUGA.Checked = false; //2021-04-26
            chkDRGBunHang.Checked = false; //2021-09-10

            chkBlood.Checked = false;
            chkMT004.Checked = false;
            chkHangJungJuSa.Checked = false;


            txtDrgCode.Text = "";
            txtDrgName.Text = "";

            cboDrg100.Text = "";
            cboDrgOT.Text = "";
            chkDrgF.Checked = false;
            chkDRGADD.Checked = false;
            chkDRGOGADD.Checked = false;
            chkDrgOpen.Checked = false;
            cboBosang.SelectedIndex = -1;




            ss1_Sheet1.ColumnCount = 26;
            ss1_Sheet1.Cells[0, 0, ss1_Sheet1.RowCount - 1, ss1_Sheet1.ColumnCount - 1].Text = "";

            ssDrg_Sheet1.RowCount = 26;
            ssDrg_Sheet1.Cells[0, 0, ssDrg_Sheet1.RowCount - 1, ssDrg_Sheet1.ColumnCount - 1].Text = "";
            ssDrg1.ActiveSheet.RowCount = 5;
            ssDrg1.ActiveSheet.Cells[0, 0, ssDrg1.ActiveSheet.RowCount - 1, ssDrg1.ActiveSheet.ColumnCount - 1].Text = "";

            ssAMT2_Sheet1.Cells[0, 0, ssAMT2_Sheet1.RowCount - 1, ssAMT2_Sheet1.ColumnCount - 1].Text = "";

            txtSCode_H.Text = "";
            txtSNext_H.Text = "";

            SS_Clear(ss2_Sheet1);

            ss3_Sheet1.Cells[0, 0, ss3_Sheet1.RowCount - 1, ss3_Sheet1.ColumnCount - 1].Text = "";
            ss4_Sheet1.RowCount = 0;

            //'공용변수를 Clear
            FstrSutROWID = ""; FstrOldBun = "";
            FnBAmt = 0; FnTAmt = 0; FnIAmt = 0;
            FstrBCode = ""; FstrSuHam = ""; FstrEdiJong = "";

            //'각종 Enable Set
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            btnChangeCode.Enabled = false;
            btnSearchHelp.Enabled = true;
            panel2.Enabled = false;
            //panSut.Enabled = false;
            //panSuh.Enabled = false;
            ss2_Sheet1.OperationMode = OperationMode.ReadOnly;

            ssAMT2_Sheet1.OperationMode = OperationMode.ReadOnly;
            mnuList.Enabled = true;


            lblSugaMsg.Text = "";
        }

        private void SS_Clear(SheetView Spd)
        {
            ss2_Sheet1.RowCount = 0;
            ss2_Sheet1.RowCount = 30;
            Spd.Cells[0, 0, Spd.RowCount - 1, Spd.ColumnCount - 1].Text = "";
        }

        private void Screen_Display()
        {

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int nREAD = 0;
            bool strTrue = false;

            txtCode.Text = txtCode.Text.Trim().ToUpper();
            if (txtCode.Text == "") return;

            txtCode.ReadOnly = true;
            //panSut.Enabled = true;
            btnSave.Enabled = true;

            btnCancel.Enabled = true;
            btnSearchHelp.Enabled = false;
            mnuList.Enabled = false;
            panel2.Enabled = true;
            FnBAmt = 0;
            FnTAmt = 0;
            FnIAmt = 0;
            FstrOldBun = "";


            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                // 'BAS_SUT를 READ
                SQL = "SELECT A.BUN,A.NU,A.SUGBA,A.SUGBB,A.SUGBC,A.SUGBD,A.SUGBE,A.SUGBC,A.SUGBF,";
                SQL = SQL + ComNum.VBLF + "       A.SUGBG,A.SUGBH,A.SUGBI,A.SUGBJ,A.SUGBK,A.SUGBL,B.SUGBM,B.SUGBN,B.SUGBO,";
                SQL = SQL + ComNum.VBLF + "       B.SUGBP,B.SUGBQ,B.SUGBR,B.SUGBS, B.SUGBT, B.SUGBU, B.SUGBV, ";
                SQL = SQL + ComNum.VBLF + "       A.BAMT,A.TAMT,A.IAMT,A.OLDBAMT,A.OLDTAMT,A.OLDIAMT,A.DAYMAX,A.TOTMAX,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.SUDATE,'YYYY-MM-DD') SUDATE,A.SUNEXT,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.SUDATE3,'YYYY-MM-DD') SUDATE3,A.BAMT3,A.TAMT3,A.IAMT3,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.SUDATE4,'YYYY-MM-DD') SUDATE4,A.BAMT4,A.TAMT4,A.IAMT4,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.SUDATE5,'YYYY-MM-DD') SUDATE5,A.BAMT5,A.TAMT5,A.IAMT5,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.DELDATE,'YYYY-MM-DD') DELDATE,B.SUNAMEK,B.SUNAMEE,";
                SQL = SQL + ComNum.VBLF + "       B.SUNAMEG,B.SUHAM,B.UNIT,B.DAICODE,B.HCODE,B.BCODE,B.WONCODE,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(B.EDIDATE,'YYYY-MM-DD') EDIDATE,B.OLDBCODE,B.OLDGESU,";
                SQL = SQL + ComNum.VBLF + "       B.EDIJONG,B.OLDJONG,B.GBWON1,GBWON2,A.ROWID SUTROWID, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(B.EDIDATE3,'YYYY-MM-DD') EDIDATE3, B.BCODE3, B.GESU3, B.EDIJONG3,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(B.EDIDATE4,'YYYY-MM-DD') EDIDATE4, B.BCODE4, B.GESU4, B.EDIJONG4,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(B.EDIDATE5,'YYYY-MM-DD') EDIDATE5, B.BCODE5, B.GESU5, B.EDIJONG5,";
                SQL = SQL + ComNum.VBLF + "       B.GBYEBANG,B.GBCSINFO,B.GBSIMLI,B.DTLBUN, B.NURCODE,B.GBYEYAK, B.GBSUGBF , ";
                SQL = SQL + ComNum.VBLF + "       B.GBANTI, B.ANTICLASS, B.GBGOJI, B.GBGANJANG, B.GBRARE, B.GBBONE,B.GBANTICAN, ";
                SQL = SQL + ComNum.VBLF + "       UNITNEW1, UNITNEW2, UNITNEW3, UNITNEW4 , B.MEMO , B.SUGBW , B.GBPPI, B.GBDEMENTIA, ";
                SQL = SQL + ComNum.VBLF + "       B.GBDIA, B.GBDRUG , B.GBOCSF, B.GBWONF, B.GBGABA, B.GBDRUGNO  , B.GBNS, B.GBSELECT, B.GBOCSDRUG ,";
                SQL = SQL + ComNum.VBLF + "       B.DRGCODE, B.DRG100, B.DRGF, B.DRGADD, B.DRGOPEN, B.DRGOGADD , B.SUGBX , B.GBOPROOM ,B.GBBLOOD, ";
                SQL = SQL + ComNum.VBLF + "       B.GBTAX, B.GBTB, B.SUGBY, B.SUGBZ, B.GBMT004, B.SUGBAA, B.SUGBAB, B.SUGBAC, B.SUGBAD, B.SUGBAE,  ";
                SQL = SQL + ComNum.VBLF + "       B.SUGBAF, B.BIGO, B.SUGBAG, B.DRGBOSANG, B.GBSELFHANG, B.GBHJJUSA, B.GBBSSimSa, B.GBTAHPSUGA, B.GBDRGBUNHANG  ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_SUT A,BAS_SUN B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.SUCODE = '" + txtCode.Text + "' "; 
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = B.SUNEXT(+) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon); 

                if (SqlErr != "") 
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    Read_DRUG_JEP_REQ_DETAIL(txtCode.Text.Trim());
                    txtGbA.Focus();
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrSuNext = dt.Rows[0]["SuNext"].ToString().Trim();
                    FstrSutROWID = dt.Rows[0]["SutROWID"].ToString().Trim();
                    FstrOldBun = dt.Rows[0]["Bun"].ToString().Trim();

                    txtGbA.Text = dt.Rows[0]["SugbA"].ToString().Trim();
                    txtGbB.Text = dt.Rows[0]["SugbB"].ToString().Trim();
                    txtGbC.Text = dt.Rows[0]["SugbC"].ToString().Trim();
                    txtGbD.Text = dt.Rows[0]["SugbD"].ToString().Trim();
                    txtGbE.Text = dt.Rows[0]["SugbE"].ToString().Trim();
                    txtGbF.Text = dt.Rows[0]["SugbF"].ToString().Trim();
                    txtGbG.Text = dt.Rows[0]["SugbG"].ToString().Trim();
                    txtGbH.Text = dt.Rows[0]["SugbH"].ToString().Trim();
                    txtGbI.Text = dt.Rows[0]["SugbI"].ToString().Trim();
                    txtGbJ.Text = dt.Rows[0]["SugbJ"].ToString().Trim();
                    txtGbK.Text = dt.Rows[0]["SugbK"].ToString().Trim();
                    txtGbL.Text = dt.Rows[0]["SugbL"].ToString().Trim();
                    txtGbM.Text = dt.Rows[0]["SugbM"].ToString().Trim();
                    txtGbN.Text = dt.Rows[0]["SugbN"].ToString().Trim();
                    txtGbO.Text = dt.Rows[0]["SugbO"].ToString().Trim();

                    txtGbP.Text = dt.Rows[0]["SugbP"].ToString().Trim();
                    txtGbP.BackColor = dt.Rows[0]["SugbP"].ToString().Trim() == "1" ? Color.FromArgb(255, 192, 192) : Color.FromArgb(255, 255, 232);

                    txtGbQ.Text = dt.Rows[0]["SugbQ"].ToString().Trim();
                    txtGbR.Text = dt.Rows[0]["SugbR"].ToString().Trim();
                    txtGbS.Text = dt.Rows[0]["SugbS"].ToString().Trim();
                    txtGbT.Text = dt.Rows[0]["SugbT"].ToString().Trim();
                    txtGbU.Text = dt.Rows[0]["SugbU"].ToString().Trim();
                    txtGbV.Text = dt.Rows[0]["SugbV"].ToString().Trim();
                    txtGbW.Text = dt.Rows[0]["SugbW"].ToString().Trim();
                    txtGbX.Text = dt.Rows[0]["Sugbx"].ToString().Trim();

                    txtGbY.Text = dt.Rows[0]["SugbY"].ToString().Trim();
                    txtGbZ.Text = dt.Rows[0]["SugbZ"].ToString().Trim();
                    txtGbAA.Text = dt.Rows[0]["SugbAA"].ToString().Trim();

                    txtGbAB.Text = dt.Rows[0]["SugbAB"].ToString().Trim();    //'2017-06-20
                    txtGbAC.Text = dt.Rows[0]["SugbAC"].ToString().Trim();    //'2017-06-20
                    txtGbAD.Text = dt.Rows[0]["SugbAD"].ToString().Trim();    //'2017-06-20
                    txtGbAE.Text = dt.Rows[0]["SugbAE"].ToString().Trim();    //'2019-04-26
                    txtGbAF.Text = dt.Rows[0]["SugbAF"].ToString().Trim();    //'2019-04-26
                    txtGbAG.Text = dt.Rows[0]["SugbAG"].ToString().Trim();    //'2019-04-26

                    if (txtGbV.Text == "") txtGbV.Text = "0";
                    if (txtGbU.Text == "") txtGbU.Text = "0";
                    if (txtGbP.Text == "") txtGbP.Text = "9";
                    if (txtGbQ.Text == "") txtGbQ.Text = "0";
                    if (txtGbW.Text == "") txtGbW.Text = "0";
                    if (txtGbX.Text == "") txtGbX.Text = "0";
                    if (txtGbY.Text == "") txtGbY.Text = "0";
                    if (txtGbZ.Text == "") txtGbZ.Text = "0";
                    if (txtGbAA.Text == "") txtGbAA.Text = "0";
                    if (txtGbAB.Text == "") txtGbAB.Text = "0";
                    if (txtGbAC.Text == "") txtGbAC.Text = "0";
                    if (txtGbAD.Text == "") txtGbAD.Text = "0";
                    if (txtGbAE.Text == "") txtGbAE.Text = "0";
                    if (txtGbAF.Text == "") txtGbAF.Text = "0";
                    if (txtGbAG.Text == "") txtGbAG.Text = "0";

                    txtBun.Text = dt.Rows[0]["Bun"].ToString().Trim();
                    txtNu.Text = dt.Rows[0]["Nu"].ToString().Trim();


                    lblBun.Text = Read_BunName(dt.Rows[0]["Bun"].ToString().Trim());
                    lblNu.Text = READ_NuName(dt.Rows[0]["Nu"].ToString().Trim());


                    txtDayMax.Text = dt.Rows[0]["DayMax"].ToString().Trim();
                    txtTotMax.Text = dt.Rows[0]["TotMax"].ToString().Trim();
                    txtSuNext.Text = dt.Rows[0]["SuNext"].ToString().Trim();
                    txtUnit.Text = dt.Rows[0]["Unit"].ToString().Trim();
                    txtHCode.Text = dt.Rows[0]["HCode"].ToString().Trim();
                    txtDelDate.Text = dt.Rows[0]["DelDate"].ToString().Trim();
                    txtDaiCode.Text = dt.Rows[0]["DaiCode"].ToString().Trim();
                    lblDaiCode.Text = READ_DaicodeName(txtDaiCode.Text);
                    txtWon.Text = dt.Rows[0]["WonCode"].ToString().Trim();
                    txtAntiClass.Text = dt.Rows[0]["AntiClass"].ToString().Trim();
                    lblAntiClass.Text = READ_AntiClassName(dt.Rows[0]["AntiClass"].ToString().Trim());

                    txtUnit_New1.Text = dt.Rows[0]["unitnew1"].ToString().Trim();
                    cboUnit_New2.Text = dt.Rows[0]["unitnew2"].ToString().Trim();
                    cboUnit_New3.Text = dt.Rows[0]["unitnew3"].ToString().Trim();
                    txtUnit_New4.Text = dt.Rows[0]["unitnew4"].ToString().Trim();
                    txtMemo.Text = dt.Rows[0]["MEMO"].ToString().Trim();
                    txtBigo.Text = dt.Rows[0]["BIGO"].ToString().Trim(); //2019-08-19 KHS 비고란추가


                    if (dt.Rows[0]["GbWon1"].ToString().Trim() == "1") chkWon1.Checked = true;           //'재료대 여부
                    if (dt.Rows[0]["GbWon2"].ToString().Trim() == "1") chkWon2.Checked = true;           //'조영제 여부
                    if (dt.Rows[0]["GbYebang"].ToString().Trim() == "Y") chkYebang.Checked = true;       //'예방접종
                    if (dt.Rows[0]["GbCsInfo"].ToString().Trim() == "Y") chkCsInfo.Checked = true;       //'고객정보(특수검사)
                    if (dt.Rows[0]["GbSimli"].ToString().Trim() == "Y") chkSimli.Checked = true;         //'임상심리실 오더전달
                    if (dt.Rows[0]["GbSugbF"].ToString().Trim() == "1") chkSelf.Checked = true;          //'심사과 관리용(f항)
                    if (dt.Rows[0]["GbAnti"].ToString().Trim() == "Y") chkAnti.Checked = true;           //'항생제 관리여부


                    if (dt.Rows[0]["GbGoji"].ToString().Trim() == "Y") chkGoji.Checked = true;         //'고지혈증 관리여부
                    if (dt.Rows[0]["GbGanJang"].ToString().Trim() == "Y") chkGanJang.Checked = true;   //'간장용제 관리여부
                    if (dt.Rows[0]["GbRare"].ToString().Trim() == "Y") chkRare.Checked = true;         //'희귀난치성 질환 @V 코드
                    if (dt.Rows[0]["GbBONE"].ToString().Trim() == "Y") chkBone.Checked = true;         //'골다공
                    if (dt.Rows[0]["GbAntiCan"].ToString().Trim() == "Y") chkAntiCan.Checked = true;   //'항암제


                    if (dt.Rows[0]["GbPPI"].ToString().Trim() == "Y") chkPPI.Checked = true;           //'PPI제제
                    if (dt.Rows[0]["GbDementia"].ToString().Trim() == "Y") chkDementia.Checked = true; //'치매약제


                    if (dt.Rows[0]["GBDia"].ToString().Trim() == "Y") chkDiabetes.Checked = true;      //'당뇨약제점검


                    if (dt.Rows[0]["GBDrug"].ToString().Trim() == "Y") chkDrug.Checked = true;         //'저가약제관리
                    if (dt.Rows[0]["GBOCSF"].ToString().Trim() == "Y") chkOCSF.Checked = true;         //'외래 OCS 급여가능
                    if (dt.Rows[0]["GBWONF"].ToString().Trim() == "Y") chkWonF.Checked = true;         //'원무과 급여전화 메제지


                    if (dt.Rows[0]["GBGABA"].ToString().Trim() == "Y") chkGaBa.Checked = true;          //'GABAPENTIN 계열
                    if (dt.Rows[0]["GBOCSDrug"].ToString().Trim() == "Y") chkOCSDrug.Checked = true;    //'향정산신성 osc 사유


                    if (dt.Rows[0]["GBDrugNo"].ToString().Trim() == "Y") chkDrugNo.Checked = true;      //'저가약제제외


                    if (dt.Rows[0]["GBNS"].ToString().Trim() == "Y") chkNS.Checked = true;              //'신경차단술
                    if (dt.Rows[0]["GBOCSDRUG"].ToString().Trim() == "Y") chkOCSDrug.Checked = true;    //'향정신성 OCS 사유

                    if (dt.Rows[0]["GBOpRoom"].ToString().Trim() == "Y") chkOpRoom.Checked = true;      //'수술예방적대상
                    if (dt.Rows[0]["GBTAX"].ToString().Trim() == "Y") chkGbTax.Checked = true;          //'부과세대상
                    if (dt.Rows[0]["GBTB"].ToString().Trim() == "Y") chkGbTB.Checked = true;            //'항결핵(지원금)


                    if (dt.Rows[0]["GbBlood"].ToString().Trim() == "Y") chkBlood.Checked = true;        //'혈우약제 2015-02-12
                    if (dt.Rows[0]["GbMT004"].ToString().Trim() == "Y") chkMT004.Checked = true;        //'MT004 구분

                    if (dt.Rows[0]["GBSELFHANG"].ToString().Trim() == "Y") chkGbSelfHang.Checked = true;        //'비급여 고지 항목 2020-11-27
                    if (dt.Rows[0]["GBTAHPSUGA"].ToString().Trim() == "Y") chkTAHPSUGA.Checked = true;     //2021-04-26 타병원수가 체크
                    if (dt.Rows[0]["GBHJJuSa"].ToString().Trim() == "Y") chkHangJungJuSa.Checked = true; //2021-03-18 항정신장기주사제
                    if (dt.Rows[0]["GBBSSimSa"].ToString().Trim() == "Y") chkBSSimSa.Checked = true;    //2021-03-25  분석심사 
                    if (dt.Rows[0]["GBDRGBunHang"].ToString().Trim() == "Y") chkDRGBunHang.Checked = true; //2021-09-10 DRG분류항


                    txtDrgCode.Text = dt.Rows[0]["drgcode"].ToString().Trim();   //'drg 코드

                    if (dt.Rows[0]["GbMT004"].ToString().Trim() != "")
                    {
                        txtDrgName.Text = READ_DRGNAME(txtDrgCode.Text);        //'drg 코드  
                    }

                    switch (dt.Rows[0]["drg100"].ToString().Trim())
                    {
                        case "Y":
                            cboDrg100.Text = "Y.100/100";
                            break;
                        case "2":
                            cboDrg100.Text = "6.100/20(임시)";
                            break;
                        case "4":
                            cboDrg100.Text = "4.100/80(비급여)";
                            break;
                        case "5":
                            cboDrg100.Text = "5.100/50";
                            break;
                        case "6":
                            cboDrg100.Text = "6.100/80(중복)";
                            break;
                    }

                    //2020-01-13 BAS_SUGA_DRGADD_NEW 안과 구해오기
                    SQL = " SELECT SUNEXT, DRGOT ";
                    SQL = SQL + ComNum.VBLF + "  FROM BAS_SUGA_DRGADD_NEW";
                    SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + txtCode.Text + "' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY SUDATE DESC";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if(dt1.Rows.Count > 0)
                    {
                        switch (dt1.Rows[0]["DRGOT"].ToString().Trim())
                        {
                            case "1":
                                cboDrgOT.Text = "1.인공수정체";
                                break;
                            //case "2":
                            //    cboDrgOT.Text = "2.연성-양안";
                            //    break;
                            default:
                                cboDrgOT.SelectedIndex = 0;
                                break;
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (dt.Rows[0]["drgf"].ToString().Trim() == "Y") chkDrgF.Checked = true;                 //'drg 비급여
                    if (dt.Rows[0]["drgadd"].ToString().Trim() == "Y") chkDRGADD.Checked = true;             //'drg 외과가산
                    if (dt.Rows[0]["drgOgadd"].ToString().Trim() == "Y") chkDRGOGADD.Checked = true;         //'drg 산부인과가산
                    if (dt.Rows[0]["drgopen"].ToString().Trim() == "Y") chkDrgOpen.Checked = true;           //'drg 복강경 개복
                    switch (dt.Rows[0]["DRGBOSANG"].ToString().Trim())
                    {
                        case "1":
                            cboBosang.SelectedIndex = 1;
                            break;
                        case "0.8":
                            cboBosang.SelectedIndex = 2;
                            break;
                        default:
                            cboBosang.SelectedIndex = 0;
                            break;
                    }
                    lblWon.Text = READ_WonName(txtWon.Text);
                    txtSuNameK.Text = dt.Rows[0]["SuNameK"].ToString().Trim();
                    txtSuNameE.Text = dt.Rows[0]["SuNameE"].ToString().Trim();
                    txtSuNameG.Text = dt.Rows[0]["SuNameG"].ToString().Trim();


                    //'수가상세분류
                    cboDtlBun.SelectedIndex = -1;
                    for (int idx = 0; idx < cboDtlBun.Items.Count; idx++)
                    {
                        if (VB.Left(cboDtlBun.Items[idx].ToString(), 4) == dt.Rows[0]["DtlBun"].ToString().Trim())
                        {
                            cboDtlBun.SelectedIndex = idx;
                            break;
                        }
                    }


                    //'예약검사 분류
                    cboGbYeyak.SelectedIndex = -1;
                    if (dt.Rows[0]["GbYeyak"].ToString().Trim() != "")
                    {
                        for (int idx = 0; idx < cboGbYeyak.Items.Count; idx++)
                        {
                            if (VB.Left(cboGbYeyak.Items[idx].ToString(), 2) == dt.Rows[0]["GbYeyak"].ToString().Trim())
                            {
                                cboGbYeyak.SelectedIndex = idx;
                                break;
                            }
                        }
                    }


                    //'선택진료구분
                    cboSelect.SelectedIndex = -1;
                    for (int idx = 0; idx < cboSelect.Items.Count; idx++)
                    {
                        if (VB.Left(cboSelect.Items[idx].ToString(), 1) == dt.Rows[0]["GBSELECT"].ToString().Trim())
                        {
                            cboSelect.SelectedIndex = idx;
                            break;
                        }
                    }


                    //'간호활동
                    txtNurCode.Text = dt.Rows[0]["NURCODE"].ToString().Trim();
                    lblNurCode.Text = READ_NurCodeName(txtNurCode.Text);


                    SUGA_AMT_READ(txtCode.Text, txtCode.Text, ss1);

                    SUGA_DRG_ADD_AMT_READ(txtCode.Text, txtCode.Text, ssDrg);
                    SUGA_DRG_ADD_AMT_READ_NEW(txtCode.Text, txtCode.Text, ssDrg1);


                    FnBAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["BAmt"].ToString().Trim()));
                    FnTAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["TAmt"].ToString().Trim()));
                    FnIAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["IAmt"].ToString().Trim()));


                    ss3_Sheet1.Cells[1, 0].Text = dt.Rows[0]["EdiDate"].ToString().Trim();
                    ss3_Sheet1.Cells[2, 0].Text = dt.Rows[0]["BCode"].ToString().Trim();

                    if (GstrBCodeShow == "OK")
                    {
                        if (frmSearchBCodeX != null)
                        {
                            frmSearchBCodeX.Dispose();
                            frmSearchBCodeX = null;
                        }
                        frmSearchBCodeX = new frmSearchBCode(dt.Rows[0]["BCode"].ToString().Trim());
                        frmSearchBCodeX.StartPosition = FormStartPosition.CenterParent;
                        frmSearchBCodeX.rEventClosed += FrmSugaSerchX_rEventClosed;
                        frmSearchBCodeX.Show();
                    }

                    ss3_Sheet1.Cells[3, 0].Text = dt.Rows[0]["SuHam"].ToString().Trim();
                    ss3_Sheet1.Cells[4, 0].Text = dt.Rows[0]["EdiJong"].ToString().Trim();

                    ss3_Sheet1.Cells[1, 1].Text = dt.Rows[0]["EdiDate3"].ToString().Trim();
                    ss3_Sheet1.Cells[2, 1].Text = dt.Rows[0]["OldBCode"].ToString().Trim();
                    ss3_Sheet1.Cells[3, 1].Text = dt.Rows[0]["OldGesu"].ToString().Trim();
                    ss3_Sheet1.Cells[4, 1].Text = dt.Rows[0]["OldJong"].ToString().Trim();

                    ss3_Sheet1.Cells[1, 2].Text = dt.Rows[0]["EdiDate4"].ToString().Trim();
                    ss3_Sheet1.Cells[2, 2].Text = dt.Rows[0]["BCode3"].ToString().Trim();
                    ss3_Sheet1.Cells[3, 2].Text = dt.Rows[0]["Gesu3"].ToString().Trim();
                    ss3_Sheet1.Cells[4, 2].Text = dt.Rows[0]["EdiJong3"].ToString().Trim();

                    ss3_Sheet1.Cells[1, 3].Text = dt.Rows[0]["EdiDate5"].ToString().Trim();
                    ss3_Sheet1.Cells[2, 3].Text = dt.Rows[0]["BCode4"].ToString().Trim();
                    ss3_Sheet1.Cells[3, 3].Text = dt.Rows[0]["Gesu4"].ToString().Trim();
                    ss3_Sheet1.Cells[4, 3].Text = dt.Rows[0]["EdiJong4"].ToString().Trim();

                    ss3_Sheet1.Cells[1, 4].Text = dt.Rows[0]["EdiDate5"].ToString().Trim();
                    ss3_Sheet1.Cells[2, 4].Text = dt.Rows[0]["BCode5"].ToString().Trim();
                    ss3_Sheet1.Cells[3, 4].Text = dt.Rows[0]["Gesu5"].ToString().Trim();
                    ss3_Sheet1.Cells[4, 4].Text = dt.Rows[0]["EdiJong5"].ToString().Trim();


                    FstrBCode = dt.Rows[0]["BCode"].ToString().Trim();
                    FstrSuHam = dt.Rows[0]["SuHam"].ToString().Trim();
                    FstrEdiJong = dt.Rows[0]["EdiJong"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //'history 등록
                if (FstrSuNext != "")
                {
                    strTrue = false;
                    for (i = 0; i < cboCode.Items.Count; i++)
                    {
                        if (cboCode.Items[i].ToString() == txtCode.Text)
                        {
                            strTrue = true;
                        }
                    }

                    if (strTrue == false)
                    {
                        cboCode.Items.Insert(0, txtCode.Text);
                        cboCode.SelectedIndex = 0;
                    }
                }



                //'외래 제한상항 display
                SQL = "SELECT SUCODE, GUBUNA, GUBUNB, FIELDA, FIELDB, TO_CHAR(ENTDATE,'YYYY-MM-DD') ENTDATE, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF ";
                SQL = SQL + ComNum.VBLF + " WHERE SUCODE = '" + txtCode.Text + "'  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ss4_Sheet1.RowCount = 0;
                ss4_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["GUBUNA"].ToString().Trim() + dt.Rows[i]["GUBUNB"].ToString().Trim())
                        {
                            case "11":
                                ss4_Sheet1.Cells[i, 0].Text = "xx세이하는 사용금지";
                                break;
                            case "12":
                                ss4_Sheet1.Cells[i, 0].Text = "xx세이상은 사용금지";
                                break;
                            case "21":
                                ss4_Sheet1.Cells[i, 0].Text = "특정과만 급여";
                                break;
                            case "22":
                                ss4_Sheet1.Cells[i, 0].Text = "특정과는 비급여";
                                break;
                            case "32":
                                ss4_Sheet1.Cells[i, 0].Text = "동일성분 n종만 급여,나머지 비급여";
                                break;
                            case "41":
                                ss4_Sheet1.Cells[i, 0].Text = "특정상병만 급여";
                                break;
                            case "42":
                                ss4_Sheet1.Cells[i, 0].Text = "특정상병은 총액";
                                break;
                            case "43":
                                ss4_Sheet1.Cells[i, 0].Text = "외래 OCS 수가별 상병체크";
                                break;
                            case "52":
                                ss4_Sheet1.Cells[i, 0].Text = "특정환자종류,특정상병에 비급여";
                                break;
                            case "53":
                                ss4_Sheet1.Cells[i, 0].Text = "보호환자,비급여";
                                break;
                            case "62":
                                ss4_Sheet1.Cells[i, 0].Text = "HD환자 필수약제";
                                break;
                            case "71":
                                ss4_Sheet1.Cells[i, 0].Text = "남자 특정과 비급여";
                                break;
                            case "72":
                                ss4_Sheet1.Cells[i, 0].Text = "여자 특정과 비급여";
                                break;
                            case "80":
                                ss4_Sheet1.Cells[i, 0].Text = "동시처방불가(검사)";
                                break;
                            case "81":
                                ss4_Sheet1.Cells[i, 0].Text = "동시처방불가(약제)";
                                break;
                            case "82":
                                ss4_Sheet1.Cells[i, 0].Text = "배수함량코드";
                                break;
                            case "83":
                                ss4_Sheet1.Cells[i, 0].Text = "연령금기(미만)";
                                break;
                            case "84":
                                ss4_Sheet1.Cells[i, 0].Text = "외래 OCS 1회처방 일수 제한";
                                break;
                            case "85":
                                ss4_Sheet1.Cells[i, 0].Text = "외래 OCS 1일 처방당 일용량(갯수)제한";
                                break;
                            case "86":
                                ss4_Sheet1.Cells[i, 0].Text = "외래/입원 OCS 분할처방 금지";
                                break;
                            case "87":
                                ss4_Sheet1.Cells[i, 0].Text = "외래 OCS 기간별 갯수제한";
                                break;
                            case "88":
                                ss4_Sheet1.Cells[i, 0].Text = "외래 OCS 기간제한";
                                break;
                            case "A2":
                                ss4_Sheet1.Cells[i, 0].Text = "약국전송 제외";
                                break;
                            case "C1":
                                ss4_Sheet1.Cells[i, 0].Text = "청구수가변환";
                                break;
                            case "D1":
                                ss4_Sheet1.Cells[i, 0].Text = "외래처방 상병제한";
                                break;
                        }

                        ss4_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FIELDA"].ToString().Trim();
                        ss4_Sheet1.Cells[i, 2].Text = dt.Rows[i]["FIELDB"].ToString().Trim();
                        ss4_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                switch (clsType.User.IdNumber)
                {
                    case "15273"://정희정계장
                    case "45316"://김해수
                        btnDelete.Enabled = true; //2019-11-09 정희정 계장님 요청으로 계장님만 오픈가능
                        break;
                    default:
                        break;
                }
                    
                btnChangeCode.Enabled = true;
                if (txtGbA.Text == "1")
                {
                    txtGbA.Focus();
                    return;
                }

                ss2_Sheet1.OperationMode = OperationMode.Normal;
                ssAMT2_Sheet1.OperationMode = OperationMode.Normal;

                cboSuNext.Items.Clear();
                cboSuNext.Items.Add(txtCode.Text);

                //'BAS_SUH(복합,Routine코드)
                SQL = "     SELECT A.BUN,A.NU,A.SUGBA,A.SUGBB,A.SUGBC,A.SUGBD,A.SUGBE,A.SUGBC,A.SUGBF,";
                SQL = SQL + ComNum.VBLF + "      A.SUGBG,A.SUGBH,A.SUGBI,A.SUGBJ,A.SUGBK,A.SUGBL,B.SUGBM,B.SUGBN,B.SUGBO, ";
                SQL = SQL + ComNum.VBLF + "      B.SUGBP,B.SUGBQ,B.SUGBR,B.SUGBS,B.SUGBT, B.SUGBU, B.SUGBV, ";
                SQL = SQL + ComNum.VBLF + "      A.BAMT,A.TAMT,A.IAMT,A.OLDBAMT,A.OLDTAMT,A.OLDIAMT,A.SUGBSS,A.SUGBBI,";
                SQL = SQL + ComNum.VBLF + "      A.SUQTY,TO_CHAR(A.SUDATE,'YYYY-MM-DD') SUDATE,A.SUNEXT,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.SUDATE3,'YYYY-MM-DD') SUDATE3,BAMT3,TAMT3,IAMT3,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.SUDATE4,'YYYY-MM-DD') SUDATE4,BAMT4,TAMT4,IAMT4,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.SUDATE5,'YYYY-MM-DD') SUDATE5,BAMT5,TAMT5,IAMT5,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.DELDATE,'YYYY-MM-DD') DELDATE,A.ROWID,B.SUNAMEK,";
                SQL = SQL + ComNum.VBLF + "      B.SUNAMEE,B.SUNAMEG,B.SUHAM,B.UNIT,B.DAICODE,B.HCODE,B.BCODE,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(B.EDIDATE,'YYYY-MM-DD') EDIDATE,B.OLDBCODE,B.OLDGESU,";
                SQL = SQL + ComNum.VBLF + "      B.EDIJONG,B.OLDJONG,B.WONCODE,B.GBWON1,B.GBWON2,A.SORT, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(B.EDIDATE3,'YYYY-MM-DD') EDIDATE3, B.BCODE3, B.GESU3, B.EDIJONG3,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(B.EDIDATE4,'YYYY-MM-DD') EDIDATE4, B.BCODE4, B.GESU4, B.EDIJONG4,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(B.EDIDATE5,'YYYY-MM-DD') EDIDATE5, B.BCODE5, B.GESU5, B.EDIJONG5,";
                SQL = SQL + ComNum.VBLF + "      B.NURCODE, B.SUGBW ,B.SUGBX , B.SUGBY, B.SUGBZ, B.SUGBAA, B.GBMT004, B.SUGBAB, ";
                SQL = SQL + ComNum.VBLF + "      B.SUGBAC, B.SUGBAD, B.SUGBAE, B.SUGBAF, B.SUGBAG";
                SQL = SQL + ComNum.VBLF + " FROM BAS_SUH A,BAS_SUN B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.SUCODE = '" + txtCode.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.SUNEXT = B.SUNEXT(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.SORT,A.SUNEXT ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    txtGbA.Focus();
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        //SS2.Row = I + 1
                        //SS2.Col = 1:  SS2.Text = ""

                        ss2_Sheet1.Cells[i, 0].Text = "";
                        ss2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SORT"].ToString().Trim();     //'우선순위
                        ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        cboSuNext.Items.Add(dt.Rows[i]["SuNext"].ToString().Trim());
                        ss2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Bun"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Nu"].ToString().Trim();


                        ss2_Sheet1.Cells[i, 5].Text = "";   //'Trim(AdoGetString(AdoRes, "Bun", i)) '항
                        ss2_Sheet1.Cells[i, 6].Text = "";   //'Trim(AdoGetString(AdoRes, "Nu", i))  '목

                        ss2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["NURCODE"].ToString().Trim();       //'간호활동

                        ss2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["BCode"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 9].Text = VB.Val(dt.Rows[i]["SuHam"].ToString().Trim()).ToString("###0.0000");
                        ss2_Sheet1.Cells[i, 10].Text = dt.Rows[i]["EdiJong"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 11].Text = dt.Rows[i]["SugbSS"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 12].Text = dt.Rows[i]["SugbBi"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 13].Text = dt.Rows[i]["SuQty"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 14].Text = dt.Rows[i]["BAmt"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 15].Text = dt.Rows[i]["TAmt"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 16].Text = dt.Rows[i]["IAmt"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 17].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 18].Text = dt.Rows[i]["HCode"].ToString().Trim();


                        //'A-L항
                        ss2_Sheet1.Cells[i, 19].Text = dt.Rows[i]["SugbA"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 20].Text = dt.Rows[i]["SugbB"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 21].Text = dt.Rows[i]["SugbC"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 22].Text = dt.Rows[i]["SugbD"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 23].Text = dt.Rows[i]["SugbE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 24].Text = dt.Rows[i]["SugbF"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 25].Text = dt.Rows[i]["SugbG"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 26].Text = dt.Rows[i]["SugbH"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 27].Text = dt.Rows[i]["SugbI"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 28].Text = dt.Rows[i]["SugbJ"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 29].Text = dt.Rows[i]["SugbK"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 30].Text = dt.Rows[i]["SugbL"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 31].Text = dt.Rows[i]["SugbM"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 32].Text = dt.Rows[i]["SugbN"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 33].Text = dt.Rows[i]["SugbO"].ToString().Trim();

                        ss2_Sheet1.Cells[i, 34].Text = dt.Rows[i]["SugbP"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 34].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 34].Text = "9";
                        }
                        ss2_Sheet1.Cells[i, 35].Text = dt.Rows[i]["SugbQ"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 35].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 35].Text = "0";
                        }


                        ss2_Sheet1.Cells[i, 36].Text = dt.Rows[i]["SugbR"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 36].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 36].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 37].Text = dt.Rows[i]["SugbS"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 37].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 37].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 38].Text = dt.Rows[i]["SugbT"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 38].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 38].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 39].Text = dt.Rows[i]["SugbU"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 39].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 39].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 40].Text = dt.Rows[i]["SugbV"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 40].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 40].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 41].Text = dt.Rows[i]["SugbW"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 41].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 41].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 42].Text = dt.Rows[i]["SugbX"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 42].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 42].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 43].Text = dt.Rows[i]["SugbY"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 43].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 43].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 44].Text = dt.Rows[i]["SugbZ"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 44].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 44].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 45].Text = dt.Rows[i]["SugbAA"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 45].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 45].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 46].Text = dt.Rows[i]["SugbAB"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 46].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 46].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 47].Text = dt.Rows[i]["SugbAC"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 47].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 47].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 48].Text = dt.Rows[i]["SugbAD"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 48].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 48].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 49].Text = dt.Rows[i]["SugbAE"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 49].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 49].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 50].Text = dt.Rows[i]["SugbAF"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 50].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 50].Text = "0";
                        }

                        ss2_Sheet1.Cells[i, 51].Text = dt.Rows[i]["SugbAG"].ToString().Trim();
                        if (ss2_Sheet1.Cells[i, 51].Text == "")
                        {
                            ss2_Sheet1.Cells[i, 51].Text = "0";
                        }

                        //'변경일자1,보험1,자보1,일반수가1
                        ss2_Sheet1.Cells[i, 49 + 3].Text = dt.Rows[i]["SuDate"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 50 + 3].Text = dt.Rows[i]["OldBAmt"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 51 + 3].Text = dt.Rows[i]["OldTAmt"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 52 + 3].Text = dt.Rows[i]["OldIAmt"].ToString().Trim();


                        //'보험코드,표준계수
                        ss2_Sheet1.Cells[i, 53 + 3].Text = dt.Rows[i]["EdiDate"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 54 + 3].Text = dt.Rows[i]["OldBCode"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 55 + 3].Text = VB.Format(VB.Val(dt.Rows[i]["OldGesu"].ToString().Trim()), "###0.0000");
                        ss2_Sheet1.Cells[i, 56 + 3].Text = dt.Rows[i]["OldJong"].ToString().Trim();


                        //'영문명,약품성분명,단위,약품대분류
                        ss2_Sheet1.Cells[i, 57 + 3].Text = dt.Rows[i]["SuNameE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 58 + 3].Text = dt.Rows[i]["SuNameG"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 59 + 3].Text = dt.Rows[i]["Unit"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 60 + 3].Text = dt.Rows[i]["DaiCode"].ToString().Trim();

                        //'원가분류
                        ss2_Sheet1.Cells[i, 61 + 3].Text = dt.Rows[i]["WonCode"].ToString().Trim();

                        //'변경일자3,보험3,자보3,일반수가3
                        ss2_Sheet1.Cells[i, 63 + 3].Text = dt.Rows[i]["SuDate3"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 64 + 3].Text = dt.Rows[i]["BAmt3"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 65 + 3].Text = dt.Rows[i]["TAmt3"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 66 + 3].Text = dt.Rows[i]["IAmt3"].ToString().Trim();

                        //'변경일자4,보험4,자보4,일반수가4
                        ss2_Sheet1.Cells[i, 67 + 3].Text = dt.Rows[i]["SuDate4"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 68 + 3].Text = dt.Rows[i]["BAmt4"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 69 + 3].Text = dt.Rows[i]["TAmt4"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 70 + 3].Text = dt.Rows[i]["IAmt4"].ToString().Trim();

                        //'변경일자5,보험5,자보5,일반수가5
                        ss2_Sheet1.Cells[i, 71 + 3].Text = dt.Rows[i]["SuDate5"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 72 + 3].Text = dt.Rows[i]["BAmt5"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 73 + 3].Text = dt.Rows[i]["TAmt5"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 74 + 3].Text = dt.Rows[i]["IAmt5"].ToString().Trim();
                        //'변경여부                        
                        ss2_Sheet1.Cells[i, 75 + 3].Text = "";

                        //'수가변경여부 Check용
                        ss2_Sheet1.Cells[i, 76 + 3].Text = dt.Rows[i]["BAmt"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 77 + 3].Text = dt.Rows[i]["TAmt"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 78 + 3].Text = dt.Rows[i]["IAmt"].ToString().Trim();

                        //'원가분석용
                        ss2_Sheet1.Cells[i, 79 + 3].Text = dt.Rows[i]["GbWon1"].ToString().Trim();    //'재료대 여부
                        ss2_Sheet1.Cells[i, 80 + 3].Text = dt.Rows[i]["GbWon2"].ToString().Trim();    //'조영제 여부
                        //'조회시의 우선순위
                        ss2_Sheet1.Cells[i, 81 + 3].Text = dt.Rows[i]["SORT"].ToString().Trim();    //'우선순위


                        //'OLD 표준코드,계수,종류
                        ss2_Sheet1.Cells[i, 82 + 3].Text = dt.Rows[i]["EdiDate3"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 83 + 3].Text = dt.Rows[i]["BCode3"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 84 + 3].Text = dt.Rows[i]["Gesu3"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 85 + 3].Text = dt.Rows[i]["EdiJong3"].ToString().Trim();

                        ss2_Sheet1.Cells[i, 86 + 3].Text = dt.Rows[i]["EdiDate4"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 87 + 3].Text = dt.Rows[i]["BCode4"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 88 + 3].Text = dt.Rows[i]["Gesu4"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 89 + 3].Text = dt.Rows[i]["EdiJong4"].ToString().Trim();

                        ss2_Sheet1.Cells[i, 90 + 3].Text = dt.Rows[i]["EdiDate5"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 91 + 3].Text = dt.Rows[i]["BCode5"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 92 + 3].Text = dt.Rows[i]["Gesu5"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 93 + 3].Text = dt.Rows[i]["EdiJong5"].ToString().Trim();


                        //'등록시 사용할 ROWID                        
                        ss2_Sheet1.Cells[i, 94 + 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();   //'ROWID


                        ss2_Sheet1.Cells[i, 95 + 3].Text = dt.Rows[i]["BCode"].ToString().Trim();                           //'최초표준코드
                        ss2_Sheet1.Cells[i, 96 + 3].Text = VB.Val(dt.Rows[i]["SuHam"].ToString().Trim()).ToString("###0.0000");   //'최초표준계수
                        ss2_Sheet1.Cells[i, 97 + 3].Text = dt.Rows[i]["EdiJong"].ToString().Trim();                         //'최초표준종류                        
                    }
                }

                dt.Dispose();
                dt = null;

                cboSuNext.Text = "";
                cboSuNext.SelectedIndex = 0;
                txtGbA.Focus();

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
        }
        
        private bool BAS_SUGA_DRGADD_NEW()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt5 = null;

            bool rtVal = false;
            string dtSUDATE = ""; //테이블 데이터 날짜
            string dtDRGF = "";   //테이블 비급여 체크
            string dtDRG100 = ""; //테이블 100/100 체크
            string dtDRGOT = ""; //테이블 안과수정체 체크
            string strSUDATE1 = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"); //설정 데이터날짜
            string strDRGF = "";   //설정 비급여 체크
            string strDRG100 = ""; //설정 100/100 체크
            string strDRGOT = "";  //설정 OT 체크
            string strROWID = "";
            
            if (chkDrgF.Checked == true) { strDRGF = "Y"; }
            strDRG100 = VB.Left(cboDrg100.Text, 1);
            strDRGOT = VB.Left(cboDrgOT.Text, 1);

            try
            {
                SQL = "SELECT SUNEXT, TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE, DRGF, DRG100, DRGOT, ROWID";
                SQL = SQL + ComNum.VBLF + " FROM BAS_SUGA_DRGADD_NEW";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + txtCode.Text + "'";
                SQL = SQL + ComNum.VBLF + "ORDER BY SUDATE DESC";

                SqlErr = clsDB.GetDataTable(ref dt5, SQL, clsDB.DbCon);

                if(dt5.Rows.Count == 0 && (chkDrgF.Checked == true || VB.Left(cboDrg100.Text,1) != "" || VB.Left(cboDrgOT.Text,1) != ""))
                {
                    SQL = "INSERT INTO BAS_SUGA_DRGADD_NEW(SUNEXT, SUDATE,DRGF,DRG100, DRGOT)  ";
                    SQL = SQL + ComNum.VBLF + "VALUES('" + txtCode.Text + "', TO_DATE('" + strSUDATE1 + "','YYYY-MM-DD'), '" + strDRGF + "', '" + strDRG100 + "', '" + strDRGOT + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("BAS_SUGA_DRGADD_NEW INSERT시 오류 발생");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                }

                if(dt5.Rows.Count > 0)
                {
                    dtSUDATE = dt5.Rows[0]["SUDATE"].ToString().Trim();
                    dtDRGF = dt5.Rows[0]["DRGF"].ToString().Trim();
                    dtDRG100 = dt5.Rows[0]["DRG100"].ToString().Trim();
                    strROWID = dt5.Rows[0]["ROWID"].ToString().Trim();
                    dtDRGOT = dt5.Rows[0]["DRGOT"].ToString().Trim();

                    if(dtSUDATE != strSUDATE1 && (dtDRGF != strDRGF || dtDRG100 != strDRG100 || dtDRGOT != strDRGOT))
                    {
                        //INSERT
                        SQL = "INSERT INTO BAS_SUGA_DRGADD_NEW(SUNEXT, SUDATE,DRGF,DRG100)  ";
                        SQL = SQL + ComNum.VBLF + "VALUES('" + txtCode.Text + "', TO_DATE('" + strSUDATE1 + "','YYYY-MM-DD'), '" + strDRGF + "', '" + strDRG100 + "')";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("BAS_SUGA_DRGADD_NEW INSERT시 오류 발생");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }
                    }
                    else if(dtSUDATE == strSUDATE1 && (dtDRGF != strDRGF || dtDRG100 != strDRG100 || dtDRGOT != strDRGOT))
                    {
                        //UPDATE
                        SQL = "UPDATE KOSMOS_PMPA.BAS_SUGA_DRGADD_NEW SET DRGF = '" + strDRGF + "', DRG100 = '" + strDRG100 + "', DRGOT = '" + strDRGOT + "'";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("BAS_SUGA_DRGADD_NEW UPDATE시 오류 발생");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }
                    }
                }

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("BAS_SUGA_DRGADD_NEW 조회시 오류");
                    return rtVal;
                }
            }
            catch (Exception ex)
            {
                if (dt5 != null)
                {
                    dt5.Dispose();
                    dt5 = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            rtVal = true;

            return rtVal;
        }

        private bool Suga_History_Insert(string ArgJob)  //'ArgJob : 1.신규 2.수정전 3.수정후 4.삭제
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            bool rtVal = false;
            string strJob;

            if (FstrSutROWID.Trim() == "" && ArgJob == "2")
            {
                rtVal = true;
                return rtVal;
            }
            strJob = ArgJob;
            if (FstrSutROWID.Trim() == "" && ArgJob == "3")
            {
                ArgJob = "1"; //'신규
            }



            //'BAS_SUT History Insert
            SQL = "INSERT INTO BAS_SUGAHIS( JOBDATE, JOBSABUN, JOBGBN, SUCODE, BUN, NU, SUGBA, SUGBB, SUGBC, SUGBD, SUGBE, SUGBF, SUGBG, SUGBH, SUGBI, SUGBJ, SUGBK, SUGBL, SUGBSS, SUGBBI, SUQTY, IAMT, TAMT, BAMT, SUDATE, OLDIAMT, OLDTAMT, OLDBAMT, DAYMAX, TOTMAX, SUNEXT, DELDATE, SUNAMEK, UNIT, DAICODE, HCODE, BCODE, TABLEGBN, SUGBM, SUGBN, SUGBO, SUGBP, SUGBQ, SUGBR, SUGBS, SUGBT, SUGBU, SUGBV, NURCODE, WONCODE, SUGBW,GBPPI , GBDEMENTIA, GBDIA,   GBDRUG, GBOCSF, GBWONF, GBGABA, GBDRUGNO, GBNS, GBSELECT, SUGBX )  ";
            SQL = SQL + ComNum.VBLF + "SELECT SYSDATE," + clsType.User.Sabun + ",'" + strJob + "',A.SUCODE,A.BUN,A.NU,";
            SQL = SQL + ComNum.VBLF + "       A.SUGBA,A.SUGBB,A.SUGBC,A.SUGBD,A.SUGBE,A.SUGBF,A.SUGBG,A.SUGBH,A.SUGBI,";
            SQL = SQL + ComNum.VBLF + "       A.SUGBJ,A.SUGBK,A.SUGBL,'','',0,A.IAMT,A.TAMT,A.BAMT,A.SUDATE,";
            SQL = SQL + ComNum.VBLF + "       A.OLDIAMT,A.OLDTAMT,A.OLDBAMT,A.DAYMAX,A.TOTMAX,A.SUNEXT,A.DELDATE,";
            SQL = SQL + ComNum.VBLF + "       B.SUNAMEK,B.UNIT,B.DAICODE,B.HCODE,B.BCODE,'T', B.SUGBM,";
            SQL = SQL + ComNum.VBLF + "       B.SUGBN, B.SUGBO, B.SUGBP, B.SUGBQ, B.SUGBR, B.SUGBS, B.SUGBT, B.SUGBU, B.SUGBV, B.NURCODE, B.WONCODE, B.SUGBW, B.GBPPI , B.GBDEMENTIA, B.GBDIA,   B.GBDRUG, B.GBOCSF, B.GBWONF, B.GBGABA, B.GBDRUGNO, B.GBNS, B.GBSELECT , B.SUGBX  ";
            SQL = SQL + ComNum.VBLF + "  FROM BAS_SUT A,BAS_SUN B ";
            SQL = SQL + ComNum.VBLF + " WHERE A.SUCODE = '" + txtCode.Text.Trim() + "' ";
            SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = B.SUNEXT ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("BAS_SUT 변경 History에 INSERT시 오류 발생");
                Cursor.Current = Cursors.Default;
                return rtVal;
            }

            //'BAS_SUH History Insert
            SQL = "INSERT INTO BAS_SUGAHIS(JOBDATE, JOBSABUN, JOBGBN, SUCODE, BUN, NU, SUGBA, SUGBB, SUGBC, SUGBD, SUGBE, SUGBF, ";
            SQL = SQL + ComNum.VBLF + " SUGBG, SUGBH, SUGBI, SUGBJ, SUGBK, SUGBL, SUGBSS, SUGBBI, SUQTY, IAMT, TAMT, BAMT, ";
            SQL = SQL + ComNum.VBLF + " SUDATE, OLDIAMT, OLDTAMT, OLDBAMT, DAYMAX, TOTMAX, SUNEXT, DELDATE, SUNAMEK, UNIT, ";
            SQL = SQL + ComNum.VBLF + " DAICODE, HCODE, BCODE, TABLEGBN, SUGBM, SUGBN, SUGBO, SUGBP, SUGBQ, SUGBR, SUGBS, SUGBT, ";
            SQL = SQL + ComNum.VBLF + " SUGBU, SUGBV, NURCODE, WONCODE, SUGBW, GBPPI, GBDEMENTIA, GBDIA,   GBDRUG, GBOCSF, ";
            SQL = SQL + ComNum.VBLF + " GBWONF, GBGABA, GBDRUGNO, GBNS, GBSELECT, SUGBX ) ";
            SQL = SQL + ComNum.VBLF + "SELECT SYSDATE," + clsType.User.Sabun + ",'" + strJob + "',A.SUCODE,A.BUN,A.NU,";
            SQL = SQL + ComNum.VBLF + "       A.SUGBA,A.SUGBB,A.SUGBC,A.SUGBD,A.SUGBE,A.SUGBF,A.SUGBG,A.SUGBH,A.SUGBI,";
            SQL = SQL + ComNum.VBLF + "       A.SUGBJ,A.SUGBK,A.SUGBL,A.SUGBSS,A.SUGBBI,A.SUQTY,A.IAMT,A.TAMT,A.BAMT,A.SUDATE,";
            SQL = SQL + ComNum.VBLF + "       A.OLDIAMT,A.OLDTAMT,A.OLDBAMT,0,0,A.SUNEXT,A.DELDATE,";
            SQL = SQL + ComNum.VBLF + "       B.SUNAMEK,B.UNIT,B.DAICODE,B.HCODE,B.BCODE,'H', B.SUGBM, ";
            SQL = SQL + ComNum.VBLF + "       B.SUGBN, B.SUGBO, B.SUGBP, B.SUGBQ, B.SUGBR, B.SUGBS, B.SUGBT, B.SUGBU, B.SUGBV, ";
            SQL = SQL + ComNum.VBLF + "       B.NURCODE, B.WONCODE, B.SUGBW, B.GBPPI , B.GBDEMENTIA, B.GBDIA,   B.GBDRUG, B.GBOCSF, ";
            SQL = SQL + ComNum.VBLF + "       B.GBWONF, B.GBGABA, B.GBDRUGNO, B.GBNS, B.GBSELECT, B.SUGBX  ";
            SQL = SQL + ComNum.VBLF + "  FROM BAS_SUH A,BAS_SUN B ";
            SQL = SQL + ComNum.VBLF + " WHERE A.SUCODE = '" + txtCode.Text.Trim() + "' ";
            SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = B.SUNEXT ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("BAS_SUH 변경 History에 INSERT시 오류 발생");
                Cursor.Current = Cursors.Default;
                return rtVal;
            }


            rtVal = true;
            return rtVal;
        }

        #region //chkWon1 Event
        private void chkWon1_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "원가분석시 0.행위료,1.재료대로 집계됨";
        }

        private void chkWon1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void chkWon1_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }
        #endregion

        #region //chkWon2 Event
        private void chkWon2_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "원가분석시 0.일반 1.방사선조영제";
        }

        private void chkWon2_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void chkWon2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            txtCode.Focus();
        }

        private void btnChangeCode_Click(object sender, EventArgs e)
        {
            int i = 0;
            txtCode.ReadOnly = false;
            FnCodeChange = true;

            for (i = 0; i < ss2_Sheet1.NonEmptyRowCount; i++)
            {
                ss2_Sheet1.Cells[i, 94 + 3].Text = "";
            }

            for (i = 0; i < ss1_Sheet1.ColumnCount; i++)
            {
                ss1_Sheet1.Cells[7, i].Text = "";
            }

            txtCode.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsPublic.GstrMsgList = "수가를 정말로 삭제를 하시겠습니까?" + ComNum.VBLF + ComNum.VBLF;
            clsPublic.GstrMsgList = clsPublic.GstrMsgList + "만일 삭제할 수가코드 처방이 발생되었으면" + ComNum.VBLF;
            clsPublic.GstrMsgList = clsPublic.GstrMsgList + "계산이 불가능하며 내역 조회도 불가능 합니다.";

            if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            //////clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);



            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

                //'삭제자료를 History에 INSERT
                if (Suga_History_Insert("4") != true) return;


                SQL = "DELETE FROM BAS_SUT ";
                SQL = SQL + ComNum.VBLF + "WHERE SUCODE = '" + txtCode.Text.Trim() + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("BAS_SUT 삭제시 오류가 발생함");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "DELETE FROM BAS_SUH WHERE SUCODE = '" + txtCode.Text.Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("BAS_SUH 삭제시 오류가 발생함");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();
                txtCode.Text = "";
                txtCode.Focus();

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnSearchDrg_Click(object sender, EventArgs e)
        {
            //닫는 이벤트 내용
            if (frmDrgBaseCodeX != null)
            {
                frmDrgBaseCodeX.Dispose();
                frmDrgBaseCodeX = null;
            }
            frmDrgBaseCodeX = new frmDrgBaseCode();
            frmDrgBaseCodeX.StartPosition = FormStartPosition.CenterParent;
            frmDrgBaseCodeX.rSetHelpCode += frmDrgBaseCodeX_rSetHelpCode;
            frmDrgBaseCodeX.rEventClosed += frmDrgBaseCodeX_rEventClosed;
            frmDrgBaseCodeX.ShowDialog();
        }

        private void btnSearchHelp_Click(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            //닫는 이벤트 내용
            if (FrmSugaSerchX != null)
            {
                FrmSugaSerchX.Dispose();
                FrmSugaSerchX = null;
            }
            FrmSugaSerchX = new FrmSugaSerch();
            FrmSugaSerchX.StartPosition = FormStartPosition.CenterParent;
            FrmSugaSerchX.rSetHelpCode += FrmSugaSerchX_rSetHelpCode;
            FrmSugaSerchX.rEventClosed += FrmSugaSerchX_rEventClosed;
            FrmSugaSerchX.ShowDialog();


            if (GstrHelpCode != "")
            {
                txtCode.Text = GstrHelpCode;
                Screen_Display();
                lblMsg.Text = "";
                GstrHelpCode = "";
                txtGbA.Focus();
                return;
            }
        }

        private void FrmSugaSerchX_rEventClosed()
        {
            //닫는 이벤트 내용
            if (FrmSugaSerchX != null)
            {
                FrmSugaSerchX.Dispose();
                FrmSugaSerchX = null;
            }
        }

        private void FrmSugaSerchX_rSetHelpCode(string strCode)
        {
            if (strCode != "")
            {
                GstrHelpCode = strCode;
            }
        }

        private bool SUGA_DRG_ADD_AMT()
        {
            string strDel = "";
            string strSuDate = "";
            int nJuAmt = 0;
            int nBuAmt = 0;
            string strROWID = "";
            int nDuAmt = 0;

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;


            for (i = 0; i < ssDrg_Sheet1.ColumnCount; i++)
            {
                strDel = ssDrg_Sheet1.Cells[0, i].Text;
                strSuDate = ssDrg_Sheet1.Cells[1, i].Text;
                nJuAmt = Convert.ToInt32(VB.Val(ssDrg_Sheet1.Cells[2, i].Text));
                nBuAmt = Convert.ToInt32(VB.Val(ssDrg_Sheet1.Cells[3, i].Text));
                strROWID = ssDrg_Sheet1.Cells[4, i].Text;
                nDuAmt = Convert.ToInt32(VB.Val(ssDrg_Sheet1.Cells[5, i].Text));


                if (strDel == "True")
                {
                    if (strROWID != "")  //'삭제
                    {
                        SQL = "UPDATE  KOSMOS_PMPA.BAS_SUGA_DRGADD SET DELDATE = SYSDATE , DELSABUN = '" + clsType.User.Sabun + "'  WHERE ROWID = '" + strROWID + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }
                else if (strROWID == "")    // '등록
                {
                    if (strSuDate != "")
                    {
                        SQL = " INSERT INTO  KOSMOS_PMPA.BAS_SUGA_DRGADD ( SUCODE, SUNEXT , SUDATE, JUAMT, BUAMT, DUAMT  ) VALUES ( ";
                        SQL = SQL + ComNum.VBLF + " '" + txtCode.Text + "','" + txtSuNext.Text + "' , TO_DATE('" + strSuDate + "','YYYY-MM-DD') , ";
                        SQL = SQL + ComNum.VBLF + " '" + nJuAmt + "', '" + nBuAmt + "' ,'" + nDuAmt + "'  )  ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }
                else    //'갱신
                {
                    SQL = "UPDATE KOSMOS_PMPA.BAS_SUGA_DRGADD SET SUDATE  = TO_DATE('" + strSuDate + "','YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + " JUAMT = '" + nJuAmt + "', ";
                    SQL = SQL + ComNum.VBLF + " BUAMT = '" + nBuAmt + "' , ";
                    SQL = SQL + ComNum.VBLF + " DUAMT = '" + nDuAmt + "'  ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //int i;
            //int j;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Array.Clear(strSuga, 0, strSuga.Length);

            if (Data_Field_Check() == false) return;

            if (FnCodeChange == true)
            {
                if (DRUG_JEP_CHECK() == false) return;
            }


            #region 비급여고지항목 팝업 알림 
            if (chkGbSelfHang.Checked == true)
            {
                ComFunc.MsgBox("비급여 고지 항목이며 별도 신고 확인 바랍니다.", "비급여 고지 항목 알림[0]"); 
            }

            SQL = "SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN = 'ETC_비급여고지수가목록' ";
            SQL = SQL + "AND CODE = '" + txtCode.Text.Trim() + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if(dt.Rows.Count == 1)
            {
                ComFunc.MsgBox("비급여 고지 항목이며 별도 신고 확인 바랍니다.", "비급여 고지 항목 알림 [수가]");
            }

            dt.Dispose();
            dt = null;

            SQL = "SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN = 'ETC_비급여고지수가목록' ";
            SQL = SQL + "AND CODE = '" + ss3_Sheet1.Cells[2, 0].Text.Trim() + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 1)
            {
                ComFunc.MsgBox("비급여 고지 항목이며 별도 신고 확인 바랍니다.", "비급여 고지 항목 알림[표준코드]");
            }

            dt.Dispose();
            dt = null;

            #endregion

            #region 타병원 수가 저장시 E항 J항 0아니면 알림    J항은 0 1 2 3 4 아니면 
            if(chkTAHPSUGA.Checked == true)
            {
                int strChkTASUGA = 0;

                if(txtGbE.Text != "0")
                {
                    strChkTASUGA = 1;
                }

                switch (txtGbJ.Text.Trim())
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                        break;
                    default:
                        strChkTASUGA = 1;
                        break;
                }

                if(strChkTASUGA == 1)
                {
                    ComFunc.MsgBox("타병원 수가는 E항과 J항 확인 요망.", "타병원수가 확인");
                }
            }
            #endregion

            Cursor.Current = Cursors.WaitCursor; 
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return;  //권한 확인

                if (Suga_History_Insert("2") == false)      //'변경전 자료를 History에 INSERT
                {
                    return;
                }
                if (SUGA_Amt_RTN() == false)
                {
                    return;
                }
                if (SUGA_DRG_ADD_AMT() == false)
                {
                    return;
                }
                if (SUT_DATA_UPDATE() == false)
                {
                    return;
                }
                if (SUH_DATA_UPDATE() == false)
                {
                    return;
                }
                if (Suga_History_Insert("3") == false)   //'변경후 자료를 History에 INSERT
                {
                    return;
                }
                
                //2020-01-13 BAS_SUGA_DRGADD_NEW DRG코드 (비급여,100/100 업데이트) KHS
                if(BAS_SUGA_DRGADD_NEW() == false)
                {
                    return;
                }

                //'분류기호가 변경되면 OCS의 오더판넬에 분류기호를 변경함
                if (FstrOldBun != "" && FstrOldBun != txtBun.Text)
                {
                    if (OCS_Bun_Change() == false) return;
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }


            if (txtDelDate.Text != "")
            {
                clsPublic.GstrMsgList = "수가에 삭제일자를 등록하셨습니다." + ComNum.VBLF + ComNum.VBLF;
                clsPublic.GstrMsgList = clsPublic.GstrMsgList + " ▶오더판넬에 삭제 Flag를 등록하지 않으면" + ComNum.VBLF;
                clsPublic.GstrMsgList = clsPublic.GstrMsgList + " ▶오더는 전달되나 수납에 누락될 수 있습니다.";
                ComFunc.MsgBox(clsPublic.GstrMsgList, "확인");
            }

            SCREEN_CLEAR();
            txtCode.Text = "";
            txtCode.Focus();
        }

        private bool OCS_Bun_Change()   //'분류기호가 변경되면 오더판넬에 분류기호 변경
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            // '오더판넬 분류기호 변경
            SQL = "UPDATE KOSMOS_OCS.OCS_ORDERCODE SET BUN='" + txtBun.Text + "' ";
            SQL = SQL + ComNum.VBLF + "WHERE SuCode='" + txtCode.Text.Trim() + "' ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("OCS_ORDERCODE에 분류기호 변경중 오류발생");
                Cursor.Current = Cursors.Default;
                return false;
            }

            // '약속처방 분류기호 변경
            SQL = "UPDATE KOSMOS_OCS.OCS_OPRM SET BUN='" + txtBun.Text + "' ";
            SQL = SQL + "WHERE SUCODE='" + txtCode.Text.Trim() + "' ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("OCS_OPRM에 분류기호 변경중 오류발생");
                Cursor.Current = Cursors.Default;
                return false;
            }

            return true;
        }

        private bool SS2_TO_String(int iRow)
        {
            strDel = ss2_Sheet1.Cells[iRow, 0].Text;
            nSORT = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 1].Text)); //'우선순위
            strSuNext = ss2_Sheet1.Cells[iRow, 2].Text;
            strBun = ss2_Sheet1.Cells[iRow, 3].Text;
            strNu = ss2_Sheet1.Cells[iRow, 4].Text;


            strNurCode = ss2_Sheet1.Cells[iRow, 7].Text;


            strBCode = ss2_Sheet1.Cells[iRow, 8].Text;
            nSuham = Convert.ToDouble(VB.Val(ss2_Sheet1.Cells[iRow, 9].Text));
            strEdiJong = ss2_Sheet1.Cells[iRow, 10].Text;


            strSugbSS = ss2_Sheet1.Cells[iRow, 11].Text;
            strSugbBi = ss2_Sheet1.Cells[iRow, 12].Text;
            nSuQty = Convert.ToDouble(VB.Val(ss2_Sheet1.Cells[iRow, 13].Text));
            nBAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 14].Text));
            nTAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 15].Text));
            nIAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 16].Text));
            strSunameK = clsVbfunc.QuotationChange(ss2_Sheet1.Cells[iRow, 17].Text);
            strHCode = ss2_Sheet1.Cells[iRow, 18].Text;
            strSugbA = ss2_Sheet1.Cells[iRow, 19].Text;
            strSugbB = ss2_Sheet1.Cells[iRow, 20].Text;
            strSugbC = ss2_Sheet1.Cells[iRow, 21].Text;
            strSugbD = ss2_Sheet1.Cells[iRow, 22].Text;
            strSugbE = ss2_Sheet1.Cells[iRow, 23].Text;
            strSugbF = ss2_Sheet1.Cells[iRow, 24].Text;
            strSugbG = ss2_Sheet1.Cells[iRow, 25].Text;
            strSugbH = ss2_Sheet1.Cells[iRow, 26].Text;
            strSugbI = ss2_Sheet1.Cells[iRow, 27].Text;
            strSugbJ = ss2_Sheet1.Cells[iRow, 28].Text;
            strSugbK = ss2_Sheet1.Cells[iRow, 29].Text;
            strSugbL = ss2_Sheet1.Cells[iRow, 30].Text;
            strSugbM = ss2_Sheet1.Cells[iRow, 31].Text;
            strSugbN = ss2_Sheet1.Cells[iRow, 32].Text;
            strSugbO = ss2_Sheet1.Cells[iRow, 33].Text;
            strSugbP = ss2_Sheet1.Cells[iRow, 34].Text;
            strSugbQ = ss2_Sheet1.Cells[iRow, 35].Text;
            strSugbR = ss2_Sheet1.Cells[iRow, 36].Text;


            strSugbS = ss2_Sheet1.Cells[iRow, 37].Text;
            strSugbT = ss2_Sheet1.Cells[iRow, 38].Text;


            strSugbU = ss2_Sheet1.Cells[iRow, 39].Text;
            strSugbV = ss2_Sheet1.Cells[iRow, 40].Text;
            strSugbW = ss2_Sheet1.Cells[iRow, 41].Text;
            strSugbX = ss2_Sheet1.Cells[iRow, 42].Text;


            strSugbY = ss2_Sheet1.Cells[iRow, 43].Text;
            strSugbZ = ss2_Sheet1.Cells[iRow, 44].Text;
            strSugbAA = ss2_Sheet1.Cells[iRow, 45].Text;
            strSugbAB = ss2_Sheet1.Cells[iRow, 46].Text;
            strSugbAC = ss2_Sheet1.Cells[iRow, 47].Text;
            strSugbAD = ss2_Sheet1.Cells[iRow, 48].Text;
            strSugbAE = ss2_Sheet1.Cells[iRow, 49].Text;
            strSugbAF = ss2_Sheet1.Cells[iRow, 50].Text;
            strSugbAG = ss2_Sheet1.Cells[iRow, 51].Text;

            strSuDate = ss2_Sheet1.Cells[iRow, 49 + 3].Text;
            nOldBAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 50 + 3].Text));
            nOldTAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 51 + 3].Text));
            nOldIAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 52 + 3].Text));



            strEdiDate = ss2_Sheet1.Cells[iRow, 53 + 3].Text;


            strOldBCode = ss2_Sheet1.Cells[iRow, 54 + 3].Text;
            nOldGesu = Convert.ToDouble(VB.Val(ss2_Sheet1.Cells[iRow, 55 + 3].Text));
            strOldJong = ss2_Sheet1.Cells[iRow, 56 + 3].Text;
            strSunameE = clsVbfunc.QuotationChange(ss2_Sheet1.Cells[iRow, 57 + 3].Text);
            strSunameG = clsVbfunc.QuotationChange(ss2_Sheet1.Cells[iRow, 58 + 3].Text);
            strUnit = ss2_Sheet1.Cells[iRow, 59 + 3].Text;
            strDaiCode = ss2_Sheet1.Cells[iRow, 60 + 3].Text;
            strWonCode = ss2_Sheet1.Cells[iRow, 61 + 3].Text;


            strSuDate3 = ss2_Sheet1.Cells[iRow, 63 + 3].Text;
            nBAmt3 = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 64 + 3].Text));
            nTAmt3 = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 65 + 3].Text));
            nIAmt3 = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 66 + 3].Text));


            strSuDate4 = ss2_Sheet1.Cells[iRow, 67 + 3].Text;
            nBAmt4 = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 68 + 3].Text));
            nTAmt4 = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 69 + 3].Text));
            nIAmt4 = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 70 + 3].Text));

            strSuDate5 = ss2_Sheet1.Cells[iRow, 71 + 3].Text;
            nBAmt5 = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 72 + 3].Text));
            nTAmt5 = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 73 + 3].Text));
            nIAmt5 = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 74 + 3].Text));

            strChange = ss2_Sheet1.Cells[iRow, 75 + 3].Text;

            strGBWON1 = VB.Val(ss2_Sheet1.Cells[iRow, 79 + 3].Text).ToString("0");
            strGBWON2 = VB.Val(ss2_Sheet1.Cells[iRow, 80 + 3].Text).ToString("0");

            strEdiDate3 = ss2_Sheet1.Cells[iRow, 82 + 3].Text;
            strBcode3 = ss2_Sheet1.Cells[iRow, 83 + 3].Text;
            strGesu3 = Convert.ToDouble(VB.Val(ss2_Sheet1.Cells[iRow, 84 + 3].Text));
            strEdiJong3 = VB.Left(ss2_Sheet1.Cells[iRow, 85 + 3].Text, 1);
            strEdiDate4 = ss2_Sheet1.Cells[iRow, 86 + 3].Text;
            strBcode4 = ss2_Sheet1.Cells[iRow, 87 + 3].Text;
            strGesu4 = Convert.ToDouble(VB.Val(ss2_Sheet1.Cells[iRow, 88 + 3].Text));
            strEdiJong4 = VB.Left(ss2_Sheet1.Cells[iRow, 89 + 3].Text, 1);
            strEdiDate5 = ss2_Sheet1.Cells[iRow, 90 + 3].Text;
            strBcode5 = ss2_Sheet1.Cells[iRow, 91 + 3].Text;
            strGesu5 = Convert.ToDouble(VB.Val(ss2_Sheet1.Cells[iRow, 92 + 3].Text));
            strEdiJong5 = VB.Left(ss2_Sheet1.Cells[iRow, 93 + 3].Text, 1);

            strSuhROWID = ss2_Sheet1.Cells[iRow, 94 + 3].Text;


            //'금액 자동 저장 설정
            if (SUGA_AMT_INSERT(txtCode.Text, strSuNext, strSuDate, nBAmt, nTAmt, nIAmt) == false)
            {
                return false;
            }
            if (SUGA_AMT_INSERT(txtCode.Text, strSuNext, strSuDate3, nOldBAmt, nOldTAmt, nOldIAmt) == false)
            {
                return false;
            }
            if (SUGA_AMT_INSERT(txtCode.Text, strSuNext, strSuDate4, nBAmt3, nTAmt3, nIAmt3) == false)
            {
                return false;
            }
            if (SUGA_AMT_INSERT(txtCode.Text, strSuNext, strSuDate5, nBAmt4, nTAmt4, nIAmt4) == false)
            {
                return false;
            }
            if (SUGA_AMT_INSERT(txtCode.Text, strSuNext, "", nBAmt5, nTAmt5, nIAmt5) == false)
            {
                return false;
            }

            return true;
        }

        private bool SUH_DATA_UPDATE()  //'SUH 변경분 UPDATE
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strROWID;

            //'BAS_SUH입력내용 점검
            for (i = 0; i < ss2_Sheet1.NonEmptyRowCount; i++)
            {
                if (SS2_TO_String(i) == false)
                {
                    continue;
                }

                strROWID = strSuhROWID;

                if (strDel == "True")
                {
                    if (strROWID != "")
                    {
                        SQL = "DELETE BAS_SUH WHERE ROWID='" + strROWID + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("BAS_SUH 삭제시 오류가 발생함");
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }
                else if (strSuNext != "" && strChange == "Y")
                {
                    if (strROWID != "")  //'UPDATE
                    {
                        SQL = "UPDATE BAS_SUH SET Bun='" + strBun + "',Nu='" + strNu + "',";
                        SQL = SQL + "SugbA='" + strSugbA + "',SugbB='" + strSugbB + "',";
                        SQL = SQL + "SugbC='" + strSugbC + "',SugbD='" + strSugbD + "',";
                        SQL = SQL + "SugbE='" + strSugbE + "',SugbF='" + strSugbF + "',";
                        SQL = SQL + "SugbG='" + strSugbG + "',SugbH='" + strSugbH + "',";
                        SQL = SQL + "SugbI='" + strSugbI + "',SugbJ='" + strSugbJ + "',";
                        SQL = SQL + "SugbK='" + strSugbK + "',SugbL='" + strSugbL + "',";
                        SQL = SQL + "SugbSS='" + strSugbSS + "',SugbBi='" + strSugbBi + "',";
                        SQL = SQL + "SuQty=" + nSuQty + ",IAmt=" + nIAmt + ",";
                        SQL = SQL + "TAmt=" + nTAmt + ",BAmt=" + nBAmt + ",";
                        SQL = SQL + "SuDate=TO_DATE('" + strSuDate + "','YYYY-MM-DD'),";
                        SQL = SQL + "OldIAmt=" + nOldIAmt + ",OldTAmt=" + nOldTAmt + ",";
                        SQL = SQL + "OldBAmt=" + nOldBAmt + ",SuNext='" + strSuNext + "',";
                        SQL = SQL + "SuDate3=TO_DATE('" + strSuDate3 + "','YYYY-MM-DD'),";
                        SQL = SQL + "IAmt3=" + nIAmt3 + ",TAmt3=" + nTAmt3 + ",BAmt3=" + nBAmt3 + ",";
                        SQL = SQL + "SuDate4=TO_DATE('" + strSuDate4 + "','YYYY-MM-DD'),";
                        SQL = SQL + "IAmt4=" + nIAmt4 + ",TAmt4=" + nTAmt4 + ",BAmt4=" + nBAmt4 + ",";
                        SQL = SQL + "SuDate5=TO_DATE('" + strSuDate5 + "','YYYY-MM-DD'),";
                        SQL = SQL + "IAmt5=" + nIAmt5 + ",TAmt5=" + nTAmt5 + ",BAmt5=" + nBAmt5 + ",";
                        SQL = SQL + "SORT=" + nSORT + " ";
                        SQL = SQL + "WHERE ROWID = '" + strROWID + "' ";
                    }
                    else
                    {
                        SQL = "INSERT INTO BAS_SUH (SUCODE,SUNEXT,BUN,NU,SUGBA,SUGBB,SUGBC,SUGBD,";
                        SQL = SQL + "SUGBE,SUGBF,SUGBG,SUGBH,SUGBI,SUGBJ,SUGBK,SUGBL,SUGBSS,SUGBBI,";
                        SQL = SQL + "SUQTY,BAMT,TAMT,IAMT,SUDATE,OLDBAMT,OLDTAMT,OLDIAMT,";
                        SQL = SQL + "SUDATE3,BAMT3,TAMT3,IAMT3,SUDATE4,BAMT4,TAMT4,IAMT4,";
                        SQL = SQL + "SUDATE5,BAMT5,TAMT5,IAMT5,SORT)";
                        SQL = SQL + " VALUES ('" + txtCode.Text + "','" + strSuNext + "','";
                        SQL = SQL + strBun + "','" + strNu + "','" + strSugbA + "','" + strSugbB + "','";
                        SQL = SQL + strSugbC + "','" + strSugbD + "','" + strSugbE + "','";
                        SQL = SQL + strSugbF + "','" + strSugbG + "','" + strSugbH + "','";
                        SQL = SQL + strSugbI + "','" + strSugbJ + "','" + strSugbK + "','";
                        SQL = SQL + strSugbL + "','" + strSugbSS + "','" + strSugbBi + "',";
                        SQL = SQL + nSuQty + "," + nBAmt + "," + nTAmt + "," + nIAmt + ",";
                        SQL = SQL + "TO_DATE('" + strSuDate + "','YYYY-MM-DD'),";
                        SQL = SQL + nOldBAmt + "," + nOldTAmt + "," + nOldIAmt + ",";
                        SQL = SQL + "TO_DATE('" + strSuDate3 + "','YYYY-MM-DD'),";
                        SQL = SQL + nBAmt3 + "," + nTAmt3 + "," + nIAmt3 + ",";
                        SQL = SQL + "TO_DATE('" + strSuDate4 + "','YYYY-MM-DD'),";
                        SQL = SQL + nBAmt4 + "," + nTAmt4 + "," + nIAmt4 + ",";
                        SQL = SQL + "TO_DATE('" + strSuDate5 + "','YYYY-MM-DD'),";
                        SQL = SQL + nBAmt5 + "," + nTAmt5 + "," + nIAmt5 + "," + nSORT + " ";
                        SQL = SQL + " ) ";
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("BAS_SUH에 " + strSuNext + "코드 UPDATE 도중 오류가 발생함");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }


                    //'수가 품명 Table에 Update
                    SQL = "SELECT ROWID FROM BAS_SUN ";
                    SQL = SQL + "WHERE Sunext = '" + strSuNext + "'";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    strROWID = "";
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return false;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;

                    strSelect = GBSELECT_SET(strSugbE, strBun, strWonCode);

                    if (strROWID != "")
                    {
                        SQL = "UPDATE BAS_SUN SET ";
                        SQL = SQL + "SunameK = '" + strSunameK + "', ";
                        SQL = SQL + "SunameE = '" + strSunameE + "', ";
                        SQL = SQL + "SunameG = '" + strSunameG + "', ";
                        SQL = SQL + "SuHam = " + nSuham + ", ";
                        SQL = SQL + "Unit = '" + strUnit + "', ";
                        SQL = SQL + "DaiCode = '" + strDaiCode + "', ";
                        SQL = SQL + "Hcode = '" + strHCode + "', ";
                        SQL = SQL + "Bcode = '" + strBCode + "', ";
                        SQL = SQL + "EdiJong='" + strEdiJong + "', ";
                        SQL = SQL + "EdiDate=TO_DATE('" + strEdiDate + "','YYYY-MM-DD'),";
                        SQL = SQL + "OldBCode='" + strOldBCode + "',";
                        SQL = SQL + "OldGesu=" + nOldGesu + ",";
                        SQL = SQL + "OldJong='" + strOldJong + "', ";
                        SQL = SQL + "WonCode='" + strWonCode + "',";
                        SQL = SQL + "SugbM='" + strSugbM + "', ";
                        SQL = SQL + "SugbN='" + strSugbN + "',";
                        SQL = SQL + "SugbO='" + strSugbO + "',";
                        SQL = SQL + "SugbP='" + strSugbP + "',";
                        SQL = SQL + "SugbQ='" + strSugbQ + "',";
                        SQL = SQL + "SugbR='" + strSugbR + "',";
                        SQL = SQL + "SugbS='" + strSugbS + "',";
                        SQL = SQL + "SugbT='" + strSugbT + "',";
                        SQL = SQL + "SugbU='" + strSugbU + "',";
                        SQL = SQL + "SugbV='" + strSugbV + "',";
                        SQL = SQL + "SugbW='" + strSugbW + "',";
                        SQL = SQL + "SugbX='" + strSugbX + "',";
                        SQL = SQL + "SugbY='" + strSugbY + "',";
                        SQL = SQL + "SugbZ='" + strSugbZ + "',";
                        SQL = SQL + "SugbAA='" + strSugbAA + "',";
                        SQL = SQL + "SugbAB='" + strSugbAB + "',";  //'2017-06-20
                        SQL = SQL + "SugbAC='" + strSugbAC + "',";
                        SQL = SQL + "SugbAD='" + strSugbAD + "',";
                        SQL = SQL + "SugbAE='" + strSugbAE + "',";
                        SQL = SQL + "SugbAF='" + strSugbAF + "',";
                        SQL = SQL + "SugbAG='" + strSugbAG + "',";  //2019-08-26
                        SQL = SQL + "GbWon1='" + strGBWON1 + "',";
                        SQL = SQL + "GbWon2='" + strGBWON2 + "', ";
                        SQL = SQL + "DtlBun='" + strDtlBun + "',";
                        SQL = SQL + "NURCODE = '" + strNurCode + "',";
                        SQL = SQL + "GBMT004 = '" + strGbMT004 + "',";
                        SQL = SQL + "GBSELECT = '" + strSelect + "', ";
                        SQL = SQL + "Edidate3 = TO_DATE('" + strEdiDate3 + "','YYYY-MM-DD') ,";
                        SQL = SQL + "EdiJong3 = '" + VB.Left(strEdiJong3, 1) + "', bcode3 ='" + strBcode3 + "', gesu3 ='" + strGesu3 + "',";
                        SQL = SQL + "Edidate4 = TO_DATE('" + strEdiDate4 + "','YYYY-MM-DD') ,";
                        SQL = SQL + "EdiJong4 = '" + strEdiJong4 + "', bcode4='" + strBcode4 + "',  gesu4 ='" + strGesu4 + "',";
                        SQL = SQL + "Edidate5 = TO_DATE('" + strEdiDate5 + "','YYYY-MM-DD') ,";
                        SQL = SQL + "EdiJong5 = '" + VB.Left(strEdiJong5, 1) + "', bcode5='" + strBcode5 + "',  gesu5 ='" + strGesu5 + "' ";
                        SQL = SQL + "WHERE ROWID = '" + strROWID + "' ";
                    }
                    else
                    {
                        SQL = "INSERT INTO BAS_SUN (SUNEXT,SUNAMEK,SUNAMEE,SUNAMEG,SUHAM,UNIT,DAICODE,";
                        SQL = SQL + "HCODE,BCODE,EDIJONG,EDIDATE,OLDJONG,OLDBCODE,OLDGESU,WONCODE,";
                        SQL = SQL + "SUGBM,SUGBN,SUGBO, SUGBP, GBWON1,GBWON2,EDIDATE3, EDIJONG3, BCODE3,";
                        SQL = SQL + "GESU3, EDIDATE4, EDIJONG4, BCODE4, GESU4, EDIDATE5, EDIJONG5,";
                        SQL = SQL + "BCODE5, GESU5, SUGBQ, SUGBR, SUGBS, SUGBT, SUGBU, DTLBUN, SUGBV, ";
                        SQL = SQL + " NURCODE, SUGBW, GBSELECT, SUGBX, SUGBY, SUGBZ, GBMT004, SUGBAA, ";
                        SQL = SQL + " SUGBAB, SUGBAC, SUGBAD, SUGBAE, SUGBAF, SUGBAG ) ";
                        SQL = SQL + " VALUES ('" + strSuNext + "','" + strSunameK + "','";
                        SQL = SQL + strSunameE + "','" + strSunameG + "'," + nSuham + ",'";
                        SQL = SQL + strUnit + "','" + strDaiCode + "','" + strHCode + "','";
                        SQL = SQL + strBCode + "','" + strEdiJong + "',TO_DATE('" + strEdiDate + "','YYYY-MM-DD'),'";
                        SQL = SQL + strOldJong + "','" + strOldBCode + "'," + nOldGesu + ",'";
                        SQL = SQL + strWonCode + "','" + strSugbM + "','" + strSugbN + "','" + strSugbO + "','" + strSugbP + "','";
                        SQL = SQL + strGBWON1 + "','" + strGBWON2 + "',";
                        SQL = SQL + "TO_DATE('" + strEdiDate3 + "','YYYY-MM-DD'), ";
                        SQL = SQL + "'" + VB.Left(strEdiJong3, 1) + "', '" + strBcode3 + "','" + strGesu3 + "',";
                        SQL = SQL + "TO_DATE('" + strEdiDate4 + "','YYYY-MM-DD'), ";
                        SQL = SQL + "'" + strEdiJong4 + "', '" + strBcode4 + "','" + strGesu4 + "',";
                        SQL = SQL + "TO_DATE('" + strEdiDate5 + "','YYYY-MM-DD'), ";
                        SQL = SQL + "'" + strEdiJong5 + "', '" + strBcode5 + "','" + strGesu5 + "',";
                        SQL = SQL + "'" + strSugbQ + "','" + strSugbR + "' , '" + strSugbS + "', ";
                        SQL = SQL + "'" + strSugbT + "', '" + strSugbU + "','" + strDtlBun + "', ";
                        SQL = SQL + "'" + strSugbV + "','" + strNurCode + "', '" + strSugbW + "' , '" + strSelect + "' , '" + strSugbX + "' , ";
                        SQL = SQL + "'" + strSugbY + "', '" + strSugbZ + "' , '" + strGbMT004 + "', '" + strSugbAA + "',";
                        SQL = SQL + "'" + strSugbAB + "','" + strSugbAC + "','" + strSugbAD + "','" + strSugbAE + "','" + strSugbAF + "', '"+ strSugbAG + "') ";
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("BAS_SUN에 자료를 UPDATE시 오류가 발생함");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
            }
            return true;
        }

        private bool SUT_DATA_UPDATE()  //'BAS_SUT의 변경내역을 UPDATE
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strROWID;

            txtSuNameK.Text = clsVbfunc.QuotationChange(txtSuNameK.Text.Trim());
            txtSuNameE.Text = clsVbfunc.QuotationChange(txtSuNameE.Text.Trim());
            txtSuNameG.Text = clsVbfunc.QuotationChange(txtSuNameG.Text.Trim());

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            SQL = "SELECT ROWID FROM BAS_SUN ";
            SQL = SQL + ComNum.VBLF + "WHERE Sunext = '" + txtSuNext.Text.Trim() + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            strROWID = "";
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return false; 
            }

            if (dt.Rows.Count > 0)
            {
                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;



            strGBWON1 = "0";
            strGBWON2 = "0";

            if (chkWon1.Checked == true) strGBWON1 = "1";
            if (chkWon2.Checked == true) strGBWON2 = "1";

            strGbYebang = "";
            strGbCsInfo = "";
            strGbSimli = "";
            strGbAnti = "";
            strGbGoji = "";
            strGbGanJang = "";
            strGbRare = "";
            strGBBone = ""; 
            strDRG100 = "";
            strDRGOT = "";
            strGbAntiCan = "";//20190723 KHS 변수 클리어 추가
            strGbPPI = "";//20190723 KHS 변수 클리어 추가
            strGBDementia = "";//20190723 KHS 변수 클리어 추가 
            strGBDiabetes = "";//20190723 KHS 변수 클리어 추가
            strGBDrug = "";//20190723 KHS 변수 클리어 추가
            strGBOCSF = "";//20190723 KHS 변수 클리어 추가
            strGBWonF = "";//20190723 KHS 변수 클리어 추가
            strGBGABA = "";//20190723 KHS 변수 클리어 추가
            strGbMT004 = "";//20190723 KHS 변수 클리어 추가
            strGBOCSDrug = "";//20190723 KHS 변수 클리어 추가
            strDRGOGADD = "";//20190723 KHS 변수 클리어 추가
            strGbOpRoom = "";//20190723 KHS 변수 클리어 추가
            strGBTax = "";//20190723 KHS 변수 클리어 추가
            strGbTB = "";//20190723 KHS 변수 클리어 추가
            strDRGBOSANG = "";//2019-12-31 KHS변수 클리어

            strGBDrugNO = "";
            strGBNS = "";
            
            strDRGCode = "";
            strDRGF = "";
            strDRGADD = "";
            strDRGOpen = "";
            strBlood = "";

            strGbSelfHang = "";
            strGbTaHPSUGA = "";
            strHangJungJuSa = "";
            strBunSukSimSa = "";
            strDRGBunHang = "";


            if (chkYebang.Checked == true) strGbYebang = "Y";
            if (chkCsInfo.Checked == true) strGbCsInfo = "Y";
            if (chkSimli.Checked == true) strGbSimli = "Y";
            if (chkAnti.Checked == true) strGbAnti = "Y";       //'항생제관리여부
            if (chkGoji.Checked == true) strGbGoji = "Y";       //'고지혈증관리여부
            if (chkGanJang.Checked == true) strGbGanJang = "Y"; //'간장용제관리여부
            if (chkRare.Checked == true) strGbRare = "Y";       //'희귀난치성 질환 @V코드 여부
            if (chkRare.Checked == true) strDRG100 = "Y";       //'희귀난치성 질환 @V코드 여부
            if (chkBone.Checked == true) strGBBone = "Y";       //'골다공증
            if (chkAntiCan.Checked == true) strGbAntiCan = "Y"; //'항암제
            if (chkPPI.Checked == true) strGbPPI = "Y";         //'PPI제제
            if (chkDementia.Checked == true) strGBDementia = "Y";   //'치매약제
            if (chkDiabetes.Checked == true) strGBDiabetes = "Y";   //'당뇨약제
            if (chkDrug.Checked == true) strGBDrug = "Y";       //'저가약제관리
            if (chkOCSF.Checked == true) strGBOCSF = "Y";       //'OCS 급여가능
            if (chkWonF.Checked == true) strGBWonF = "Y";       //'원무과 급여전환 메세지 처리
            if (chkGaBa.Checked == true) strGBGABA = "Y";       //'GABAPENTIN 계열약제
            if (chkBlood.Checked == true) strBlood = "Y";       //'혈우약 2015-02-12
            if (chkMT004.Checked == true) strGbMT004 = "Y";     //'MT004
            if (chkDrugNo.Checked == true) strGBDrugNO = "Y";   //'저가약제제외
            if (chkNS.Checked == true) strGBNS = "Y";               //'신경차단술
            if (chkOCSDrug.Checked == true) strGBOCSDrug = "Y";     //'향정신성 OCS 사유


            strDRGCode = txtDrgCode.Text;   //'DRG 코드

            if (VB.Left(cboDrg100.Text, 1) != "") strDRG100 = VB.Left(cboDrg100.Text, 1).Trim();
            if (VB.Left(cboDrgOT.Text, 1) != "") strDRGOT = VB.Left(cboDrgOT.Text, 1).Trim();


            if (chkDrgOpen.Checked == true) strDRGOpen = "Y";    //'DRG 복강경 개복
            if (chkDrgF.Checked == true) strDRGF = "Y";          //'DRG 비급여
            if (chkDRGADD.Checked == true) strDRGADD = "Y";      //'DRG 외과 가산
            if (chkDRGOGADD.Checked == true) strDRGOGADD = "Y";  //'DRG 외과 가산
            //DRG 질병군 보상률
            switch(VB.Left(cboBosang.Text.Trim(),1))
            {
                case "1":
                    strDRGBOSANG = "1.0";
                    break;
                case "2":
                    strDRGBOSANG = "0.8";
                    break;
                default:
                    strDRGBOSANG = "0.0";
                    break;
            }


            if (chkOpRoom.Checked == true) strGbOpRoom = "Y";   //'수술예방적대상
            if (chkGbTax.Checked == true) strGBTax = "Y";       //'부과세대상
            if (chkGbTB.Checked == true) strGbTB = "Y";         //'항결핵(지원금)
            if (chkGbSelfHang.Checked == true) strGbSelfHang = "Y";         //'비급여고지항목
            if (chkTAHPSUGA.Checked == true) strGbTaHPSUGA = "Y";          //타병원 수가 체크
            if (chkHangJungJuSa.Checked == true) strHangJungJuSa = "Y";  //항정신장기주사제
            if (chkBSSimSa.Checked == true) strBunSukSimSa = "Y";       //분석심사
            if (chkDRGBunHang.Checked == true) strDRGBunHang = "Y";      //질병군분류항


            strNurCode = txtNurCode.Text;
            strGbYeyak = VB.Left(cboGbYeyak.Text, 2).Trim();
            strSelect = VB.Left(cboSelect.Text, 1).Trim();

            strSelect = GBSELECT_SET(txtGbE.Text, txtBun.Text, txtWon.Text.Trim()); 



            //    '심사과 관리용(F항)
            if (chkSelf.Checked == true) strGbSugbF = "1";


            strEdiDate = "";
            strBCode = "";
            nSuham = 0;
            strEdiJong = "";
            strEdiDate3 = "";
            strOldBCode = "";
            nOldGesu = 0;
            strOldJong = "";
            strEdiDate4 = "";
            strBcode3 = "";
            strGesu3 = 0;
            strEdiJong3 = "";
            strEdiDate5 = "";
            strBcode4 = "";
            strGesu4 = 0;
            strEdiJong4 = "";
            strBcode5 = "";
            strGesu5 = 0;
            strEdiJong5 = "";

            nCol = 0;
            for (i = 0; i < ss3_Sheet1.Rows.Count; i++)
            {
                if (ss3_Sheet1.Cells[0, i].Text != "True")
                {
                    nCol = nCol + 1;

                    switch (nCol)
                    {
                        case 1:
                            strEdiDate = ss3_Sheet1.Cells[1, i].Text;
                            strBCode = ss3_Sheet1.Cells[2, i].Text;
                            if (ss3_Sheet1.Cells[3, i].Text != "0") nSuham = Convert.ToDouble(VB.Val(ss3_Sheet1.Cells[3, i].Text));
                            strEdiJong = VB.Left(ss3_Sheet1.Cells[4, i].Text, 1);
                            break;
                        case 2:
                            strEdiDate3 = ss3_Sheet1.Cells[1, i].Text;
                            strOldBCode = ss3_Sheet1.Cells[2, i].Text;
                            if (ss3_Sheet1.Cells[3, i].Text != "0") nOldGesu = Convert.ToDouble(VB.Val(ss3_Sheet1.Cells[3, i].Text));
                            strOldJong = VB.Left(ss3_Sheet1.Cells[4, i].Text, 1);
                            break;
                        case 3:
                            strEdiDate4 = ss3_Sheet1.Cells[1, i].Text;
                            strBcode3 = ss3_Sheet1.Cells[2, i].Text;
                            if (ss3_Sheet1.Cells[3, i].Text != "0") strGesu3 = Convert.ToDouble(VB.Val(ss3_Sheet1.Cells[3, i].Text));
                            strEdiJong3 = VB.Left(ss3_Sheet1.Cells[4, i].Text, 1);
                            break;
                        case 4:
                            strEdiDate5 = ss3_Sheet1.Cells[1, i].Text;
                            strBcode4 = ss3_Sheet1.Cells[2, i].Text;
                            if (ss3_Sheet1.Cells[3, i].Text != "0") strGesu4 = Convert.ToDouble(VB.Val(ss3_Sheet1.Cells[3, i].Text));
                            strEdiJong4 = VB.Left(ss3_Sheet1.Cells[4, i].Text, 1);
                            break;
                        case 5:
                            strBcode5 = ss3_Sheet1.Cells[2, i].Text;
                            if (ss3_Sheet1.Cells[3, i].Text != "0") strGesu5 = Convert.ToDouble(VB.Val(ss3_Sheet1.Cells[3, i].Text));
                            strEdiJong5 = VB.Left(ss3_Sheet1.Cells[4, i].Text, 1);
                            break;
                    }
                }
            }

            if (strROWID != "")
            {
                SQL = "UPDATE BAS_SUN SET ";
                SQL = SQL + "SunameK = '" + txtSuNameK.Text.Trim() + "', ";
                SQL = SQL + "SunameE = '" + txtSuNameE.Text.Trim() + "', ";
                SQL = SQL + "SunameG = '" + txtSuNameG.Text.Trim() + "', ";
                SQL = SQL + "SuHam   =" + nSuham + ", ";
                SQL = SQL + "Unit    = '" + txtUnit.Text.Trim() + "', ";
                SQL = SQL + "DaiCode = '" + txtDaiCode.Text.Trim() + "', ";
                SQL = SQL + "Hcode   = '" + txtHCode.Text.Trim() + "', ";
                SQL = SQL + "Bcode   = '" + strBCode.Trim() + "', ";
                SQL = SQL + "EdiJong = '" + strEdiJong.Trim() + "',";
                SQL = SQL + "EdiDate = TO_DATE('" + strEdiDate + "','YYYY-MM-DD'),";
                SQL = SQL + "OldJong = '" + strOldJong.Trim() + "',";
                SQL = SQL + "OldBCode= '" + strOldBCode.Trim() + "',";
                SQL = SQL + "OldGesu =" + nOldGesu + ",";
                SQL = SQL + "WonCode = '" + txtWon.Text.Trim() + "',";
                SQL = SQL + "SugbM   = '" + txtGbM.Text + "', ";
                SQL = SQL + "SugbN   = '" + txtGbN.Text + "',";
                SQL = SQL + "SugbO   = '" + txtGbO.Text + "',";
                SQL = SQL + "SugbP   = '" + txtGbP.Text + "',";
                SQL = SQL + "SugbR   = '" + txtGbR.Text + "',";
                SQL = SQL + "SugbS   = '" + txtGbS.Text + "',";
                SQL = SQL + "SugbT   = '" + txtGbT.Text + "',";
                SQL = SQL + "SugbU   = '" + txtGbU.Text + "',";
                SQL = SQL + "SugbV   = '" + txtGbV.Text + "',";
                SQL = SQL + "SugbW   = '" + txtGbW.Text + "',";
                SQL = SQL + "SugbX   = '" + txtGbX.Text + "',";
                SQL = SQL + "SugbY   = '" + txtGbY.Text + "',";
                SQL = SQL + "SugbZ   = '" + txtGbZ.Text + "',";
                SQL = SQL + "SugbAA   = '" + txtGbAA.Text + "',";
                SQL = SQL + "SugbAB   = '" + txtGbAB.Text + "',";    //'2017-06-20
                SQL = SQL + "SugbAC   = '" + txtGbAC.Text + "',";
                SQL = SQL + "SugbAD   = '" + txtGbAD.Text + "',";
                SQL = SQL + "SugbAE   = '" + txtGbAE.Text + "',";
                SQL = SQL + "SugbAF   = '" + txtGbAF.Text + "',";
                SQL = SQL + "SugbAG   = '" + txtGbAG.Text + "',";  //2019-08-26

                SQL = SQL + "GbWon1  = '" + strGBWON1 + "',";
                SQL = SQL + "GbWon2  = '" + strGBWON2 + "',";
                SQL = SQL + "GbYebang= '" + strGbYebang + "',";
                SQL = SQL + "GbCsInfo= '" + strGbCsInfo + "',";
                SQL = SQL + "GbSimli=  '" + strGbSimli + "',";
                SQL = SQL + "DtlBun=   '" + strDtlBun + "',";
                SQL = SQL + "Edidate3 = TO_DATE('" + strEdiDate3 + "','YYYY-MM-DD') ,";
                SQL = SQL + "EdiJong3 = '" + strEdiJong3 + "', BCODE3='" + strBcode3 + "',  GESU3='" + strGesu3 + "', ";
                SQL = SQL + "Edidate4 = TO_DATE('" + strEdiDate4 + "','YYYY-MM-DD') ,";
                SQL = SQL + "EdiJong4 = '" + strEdiJong4 + "', BCODE4='" + strBcode4 + "',  GESU4='" + strGesu4 + "', ";
                SQL = SQL + "Edidate5 = TO_DATE('" + strEdiDate5 + "','YYYY-MM-DD') ,";
                SQL = SQL + "EdiJong5 = '" + strEdiJong5 + "', BCODE5= '" + strBcode5 + "', GESU5='" + strGesu5 + "', ";
                SQL = SQL + "SugbQ    = '" + txtGbQ.Text + "', ";
                SQL = SQL + "GbYeyak  = '" + strGbYeyak + "', ";
                SQL = SQL + "GBSugbF   = '" + strGbSugbF + "', ";
                SQL = SQL + "NURCODE  = '" + strNurCode + "', ";
                SQL = SQL + "GBANTI   = '" + strGbAnti + "', ";
                SQL = SQL + "ANTICLASS = '" + txtAntiClass.Text + "' , ";


                SQL = SQL + "  GBGOjI  = '" + strGbGoji + "', ";
                SQL = SQL + "  GBGANJANG = '" + strGbGanJang + "', ";
                SQL = SQL + "  GBRARE = '" + strGbRare + "', ";
                SQL = SQL + "  GBBONE = '" + strGBBone + "', ";


                SQL = SQL + "  GBAntiCan = '" + strGbAntiCan + "', ";           //'2009-09-21 항암제  윤조연
                SQL = SQL + "  GBPPI = '" + strGbPPI + "', ";                   //'PPI
                SQL = SQL + "  GBDementia  = '" + strGBDementia + "', ";        //'치매약제
                SQL = SQL + "  GBDia  = '" + strGBDiabetes + "' , ";            //'당뇨약제
                SQL = SQL + "  GBDrug = '" + strGBDrug + "' ,";                 //'저가약제관리
                SQL = SQL + "  GBOCSF = '" + strGBOCSF + "' ,";                 //'OCS 급여가능
                SQL = SQL + "  GBWONF = '" + strGBWonF + "' ,";                 //'급여전환(원무과)
                SQL = SQL + "  GBGABA = '" + strGBGABA + "' ,";                 //'GABAPENTIN 계열 약제
                SQL = SQL + "  GBDrugNO = '" + strGBDrugNO + "' ,";             //'저가약제 제외
                SQL = SQL + "  GBNS = '" + strGBNS + "' ,";                     //'신경차단술
                SQL = SQL + "  GBOCSDRUG = '" + strGBOCSDrug + "', ";           //'향정신성 OCS 사유
                SQL = SQL + "  GBOpRoom = '" + strGbOpRoom + "', ";             //' 수술적예방대상
                SQL = SQL + "  GBTAX = '" + strGBTax + "', ";                   //'부과세
                SQL = SQL + "  GBTB = '" + strGbTB + "', ";                     //'항결핵(지원금)
                SQL = SQL + "  GbBlood = '" + strBlood + "', ";                 //'혈우약제 2015-02-12
                SQL = SQL + "  GbMT004 = '" + strGbMT004 + "', ";               //'MT004

                SQL = SQL + "  GBSELFHANG = '" + strGbSelfHang + "', ";                 //'비급여 고지항목 2020-11-27
                SQL = SQL + "  GBTAHPSUGA = '" + strGbTaHPSUGA + "', ";                 //'타병원 수가 체크 2021-04-26
                SQL = SQL + "  GBHJJuSa = '" + strHangJungJuSa + "', ";                 //'항정신장기주사제 2021-03-18
                SQL = SQL + "  GBBSSIMSA = '" + strBunSukSimSa + "', ";                 //'분석심사 2021-03-25
                SQL = SQL + "  GBDRGBunHang = '" + strDRGBunHang + "', ";               //질병군 분류 항

                if (txtUnit_New1.Text.Contains("/"))
                {
                    //2020-07-09 정희정 /슬래시 있을경우 짜른다 .
                    string strTxtUnit_New1 = "";
                    string strSp = "/";

                    strTxtUnit_New1 = txtUnit_New1.Text;
                    strTxtUnit_New1 = strTxtUnit_New1.Substring(0, strTxtUnit_New1.IndexOf(strSp));

                    SQL = SQL + "  UNITNEW1 = '" + strTxtUnit_New1 + "' ,";
                }
                else
                {
                    SQL = SQL + "  UNITNEW1 = '" + txtUnit_New1.Text + "' ,";
                }
                
                SQL = SQL + "  UNITNEW2 = '" + cboUnit_New2.Text + "' ,";
                SQL = SQL + "  UNITNEW3 = '" + cboUnit_New3.Text + "', ";
                SQL = SQL + "  UNITNEW4 = '" + txtUnit_New4.Text + "', ";
                SQL = SQL + "  GBSELECT = '" + strSelect + "', ";
                SQL = SQL + "  MEMO = '" + txtMemo.Text + "',  ";
                SQL = SQL + "  DRGCODE = '" + strDRGCode + "',";
                SQL = SQL + "  DRG100  = '" + strDRG100 + "' , ";
                SQL = SQL + "  DRGF = '" + strDRGF + "', ";
                SQL = SQL + "  DRGADD = '" + strDRGADD + "' ,";
                SQL = SQL + "  DRGOgADD = '" + strDRGOGADD + "' ,";
                SQL = SQL + "  DRGBOSANG = '" + strDRGBOSANG + "' ,"; //2019-12-31 보상률 추가
                SQL = SQL + "  DRGOPEN = '" + strDRGOpen + "' ,";
                SQL = SQL + "  BIGO = '" + txtBigo.Text + "' "; //2019-08-19 KHS 비고란추가

                SQL = SQL + "WHERE ROWID = '" + strROWID + "' ";
            }
            else
            {
                SQL = "INSERT INTO BAS_SUN (SUNEXT,SUNAMEK,SUNAMEE,SUNAMEG,SUHAM,UNIT,DAICODE,";
                SQL = SQL + "HCODE,BCODE,EDIJONG,EDIDATE,OLDJONG,OLDBCODE,OLDGESU,WONCODE,";
                SQL = SQL + "SUGBM, SUGBN, SUGBO, SUGBP, GBWON1,GBWON2,EDIDATE3, EDIJONG3, BCODE3,";
                SQL = SQL + "GESU3, EDIDATE4, EDIJONG4, BCODE4, GESU4, EDIDATE5, EDIJONG5,";
                SQL = SQL + "BCODE5, GESU5,GBYEBANG,GBCSINFO,GBSIMLI,SUGBQ,SUGBR, SUGBS, SUGBT,";
                SQL = SQL + " SUGBU, DTLBUN, SUGBV,GBYEYAK, NURCODE, GBSUGBF, GBANTI, ANTICLASS, GBGOJI, GBGANJANG, ";
                SQL = SQL + " GBRARE, GBBONE,GBANTICAN, UNITNEW1, UNITNEW2, UNITNEW3, UNITNEW4, MEMO , SUGBW, GBPPI, GBDEMENTIA , ";
                SQL = SQL + " GBDIA, GBDRUG,  GBOCSF, GBWONF, GBGABA ,GBNS, GBSELECT, GBOCSDRUG  ,  ";
                SQL = SQL + " DRGCODE, DRG100, DRGF, DRGADD, DRGOPEN , DRGOGADD , SUGBX, GBOPROOM, GBTAX , GBTB,GBBLOOD, SUGBY, SUGBZ, GBMT004,";
                SQL = SQL + " SUGBAA, SUGBAB, SUGBAC, SUGBAD, SUGBAE, SUGBAF, BIGO, SUGBAG, DRGBOSANG, GBSELFHANG, GBHJJuSa, GBBSSimSa, GBTAHPSUGA, GBDRGBunHang ) "; //2019-08-19 KHS 비고란추가,보상률 란추가
                SQL = SQL + " VALUES ('" + txtSuNext.Text.Trim() + "','" + txtSuNameK.Text.Trim() + "','";
                SQL = SQL + txtSuNameE.Text.Trim() + "','" + txtSuNameG.Text.Trim() + "'," + nSuham + ",'";
                SQL = SQL + txtUnit.Text.Trim() + "','" + txtDaiCode.Text.Trim() + "','" + txtHCode.Text.Trim() + "','";
                SQL = SQL + strBCode + "','" + strEdiJong + "',TO_DATE('" + strEdiDate + "','YYYY-MM-DD'),'";
                SQL = SQL + strOldJong + "','" + strOldBCode + "'," + nOldGesu + ",'";
                SQL = SQL + txtWon.Text.Trim() + "','" + txtGbM.Text.Trim() + "','";
                SQL = SQL + txtGbN.Text.Trim() + "','" + txtGbO.Text.Trim() + "','" + txtGbP.Text.Trim() + "','";
                SQL = SQL + strGBWON1 + "','" + strGBWON2 + "', ";
                SQL = SQL + " TO_DATE('" + strEdiDate3 + "','YYYY-MM-DD'), ";
                SQL = SQL + " '" + strEdiJong3 + "', '" + strBcode3 + "','" + strGesu3 + "',";
                SQL = SQL + " TO_DATE('" + strEdiDate4 + "','YYYY-MM-DD'), ";
                SQL = SQL + " '" + strEdiJong4 + "', '" + strBcode4 + "','" + strGesu4 + "',";
                SQL = SQL + " TO_DATE('" + strEdiDate5 + "','YYYY-MM-DD'), ";
                SQL = SQL + " '" + strEdiJong5 + "', '" + strBcode5 + "','" + strGesu5 + "',";
                SQL = SQL + " '" + strGbYebang + "','" + strGbCsInfo + "',";
                SQL = SQL + " '" + strGbSimli + "','" + txtGbQ.Text.Trim() + "', ";
                SQL = SQL + " '" + txtGbR.Text.Trim() + "', '" + txtGbS.Text.Trim() + "', ";
                SQL = SQL + " '" + txtGbT.Text.Trim() + "', '" + txtGbU.Text.Trim() + "', ";
                SQL = SQL + " '" + strDtlBun + "', '" + txtGbV.Text.Trim() + "', ";
                SQL = SQL + " '" + strGbYeyak + "','" + strNurCode + "' ,";
                SQL = SQL + " '" + strGbSugbF + "','" + strGbAnti + "'  ,'" + txtAntiClass.Text.Trim() + "' , ";
                SQL = SQL + "  '" + strGbGoji + "', '" + strGbGanJang + "', '" + strGbRare + "' , '" + strGBBone + "' , '" + strGbAntiCan + "' , "; 

               if (txtUnit_New1.Text.Contains("/"))
                {
                    //2020-07-09 정희정 /슬래시 있을경우 짜른다 .
                    string strTxtUnit_New1 = "";
                    string strSp = "/";

                    strTxtUnit_New1 = txtUnit_New1.Text;
                    strTxtUnit_New1 = strTxtUnit_New1.Substring(0, strTxtUnit_New1.IndexOf(strSp));

                    SQL = SQL + "  '" + strTxtUnit_New1 + "', '" + cboUnit_New2.Text.Trim() + "', '" + cboUnit_New3.Text.Trim() + "', ";
                }
                else
                {
                    SQL = SQL + "  '" + txtUnit_New1.Text.Trim() + "', '" + cboUnit_New2.Text.Trim() + "', '" + cboUnit_New3.Text.Trim() + "', ";
                }

                SQL = SQL + "  '" + txtUnit_New4.Text.Trim() + "' ,'" + txtMemo.Text.Trim() + "',   '" + txtGbW.Text.Trim() + "' ,'" + strGbPPI + "',";
                SQL = SQL + "  '" + strGBDementia + "' ,'" + strGBDiabetes + "' , '" + strGBDrug + "', '" + strGBOCSF + "' , ";
                SQL = SQL + "  '" + strGBWonF + "' , '" + strGBGABA + "',  '" + strGBNS + "'  ,'" + strSelect + "' , '" + strGBOCSDrug + "',   ";
                SQL = SQL + "  '" + strDRGCode + "', '" + strDRG100 + "', '" + strDRGF + "', '" + strDRGADD + "','" + strDRGOpen + "' , ";
                SQL = SQL + "  '" + strDRGOGADD + "', '" + strSugbX + "' , '" + strGbOpRoom + "' , '" + strGBTax + "' , ";
                SQL = SQL + "  '" + strGbTB + "','" + strBlood + "',  ";
                SQL = SQL + "  '" + txtGbY.Text.Trim() + "' , '" + txtGbZ.Text.Trim() + "', '" + strGbMT004 + "', ";
                SQL = SQL + "  '" + txtGbAA.Text.Trim() + "','" + txtGbAB.Text.Trim() + "','" + txtGbAC.Text.Trim() + "',";
                SQL = SQL + "  '" + txtGbAD.Text.Trim() + "','" + txtGbAE.Text.Trim() + "','" + txtGbAF.Text.Trim() + "',";
                SQL = SQL + "  '" + txtBigo.Text + "','" + txtGbAG.Text.Trim() + "', '" + strDRGBOSANG + "', '" + strGbSelfHang + "',";
                SQL = SQL + "  '" + strHangJungJuSa + "','" + strBunSukSimSa + "','" + strGbTaHPSUGA + "','" + strDRGBunHang + "' ";
                SQL = SQL + " ) ";
            }
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("BAS_SUN에 자료를 UPDATE시 오류가 발생함");
                Cursor.Current = Cursors.Default;
                return false;
            }


            SQL = " UPDATE KOSMOS_OCS.OCS_DRUGINFO_NEW SET";
            SQL = SQL + ComNum.VBLF + " BUNCODE = '" + txtDaiCode.Text.Trim() + "' ";
            SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + txtSuNext.Text.Trim() + "' ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("OCS_DRUGINFO_NEW에 자료를 UPDATE시 오류가 발생함");
                Cursor.Current = Cursors.Default;
                return false;
            }


            strSuDate = strSuga[0, 0];
            nBAmt = Convert.ToInt32(VB.Val(strSuga[0, 1]));
            nTAmt = Convert.ToInt32(VB.Val(strSuga[0, 2]));
            nIAmt = Convert.ToInt32(VB.Val(strSuga[0, 3]));


            strSuDate3 = strSuga[1, 0];
            nOldBAmt = Convert.ToInt32(VB.Val(strSuga[1, 1]));
            nOldTAmt = Convert.ToInt32(VB.Val(strSuga[1, 2]));
            nOldIAmt = Convert.ToInt32(VB.Val(strSuga[1, 3]));

            strSuDate4 = strSuga[2, 0];
            nBAmt3 = Convert.ToInt32(VB.Val(strSuga[2, 1]));
            nTAmt3 = Convert.ToInt32(VB.Val(strSuga[2, 2]));
            nIAmt3 = Convert.ToInt32(VB.Val(strSuga[2, 3]));

            strSuDate5 = strSuga[3, 0];
            nBAmt4 = Convert.ToInt32(VB.Val(strSuga[3, 1]));
            nTAmt4 = Convert.ToInt32(VB.Val(strSuga[3, 2]));
            nIAmt4 = Convert.ToInt32(VB.Val(strSuga[3, 3]));

            nBAmt5 = Convert.ToInt32(VB.Val(strSuga[4, 1]));
            nTAmt5 = Convert.ToInt32(VB.Val(strSuga[4, 2]));
            nIAmt5 = Convert.ToInt32(VB.Val(strSuga[4, 3]));


            //    'BAS_SUT 자료가 있는지 Check            
            SQL = "SELECT ROWID FROM BAS_SUT ";
            SQL = SQL + "WHERE SuCode = '" + txtCode.Text.Trim() + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            strROWID = "";
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return false;
            }

            if (dt.Rows.Count > 0)
            {
                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            if (strROWID.Trim() == "")
            {
                SQL = "INSERT INTO BAS_SUT (SuCode,SuNext,Bun,Nu,SugbA,SugbB,SugbC,SugbD,SugbE,SugbF,SugbG,";
                SQL = SQL + ComNum.VBLF + "SugbH,SugbI,SugbJ,SugbK,SugbL,DayMax,TotMax,DelDate,BAmt,TAmt,IAmt,";
                SQL = SQL + ComNum.VBLF + "SuDate,OldBAmt,OldTAmt,OldIAmt,SuDate3,BAmt3,TAmt3,IAmt3,";
                SQL = SQL + ComNum.VBLF + "SuDate4,BAmt4,TAmt4,IAmt4,SuDate5,BAmt5,TAmt5,IAmt5) ";
                SQL = SQL + ComNum.VBLF + " VALUES ('" + txtCode.Text + "','" + txtSuNext.Text.Trim() + "',";
                SQL = SQL + ComNum.VBLF + "'" + txtBun.Text + "','" + txtNu.Text + "','" + txtGbA.Text + "',";
                SQL = SQL + ComNum.VBLF + "'" + txtGbB.Text + "','" + txtGbC.Text + "','" + txtGbD.Text + "',";
                SQL = SQL + ComNum.VBLF + "'" + txtGbE.Text + "','" + txtGbF.Text + "','" + txtGbG.Text + "',";
                SQL = SQL + ComNum.VBLF + "'" + txtGbH.Text + "','" + txtGbI.Text + "','" + txtGbJ.Text + "',";
                SQL = SQL + ComNum.VBLF + "'" + txtGbK.Text + "','" + txtGbL.Text + "','" + txtDayMax.Text + "',";
                SQL = SQL + ComNum.VBLF + "'" + txtTotMax.Text + "',TO_DATE('" + txtDelDate.Text + "','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + nBAmt + "," + nTAmt + "," + nIAmt + ",";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + strSuDate + "', 'yyyy-mm-dd'),";
                SQL = SQL + ComNum.VBLF + nOldBAmt + "," + nOldTAmt + "," + nOldIAmt + ",";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + strSuDate3 + "', 'yyyy-mm-dd'),";
                SQL = SQL + ComNum.VBLF + nBAmt3 + "," + nTAmt3 + "," + nIAmt3 + ",";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + strSuDate4 + "', 'yyyy-mm-dd'),";
                SQL = SQL + ComNum.VBLF + nBAmt4 + "," + nTAmt4 + "," + nIAmt4 + ",";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + strSuDate5 + "', 'yyyy-mm-dd'),";
                SQL = SQL + ComNum.VBLF + nBAmt5 + "," + nTAmt5 + "," + nIAmt5 + ") ";
            }
            else
            {
                SQL = "UPDATE BAS_SUT SET ";
                SQL = SQL + ComNum.VBLF + "Bun = '" + txtBun.Text + "', ";
                SQL = SQL + ComNum.VBLF + "Nu = '" + txtNu.Text + "', ";
                SQL = SQL + ComNum.VBLF + "SugbA = '" + txtGbA.Text + "', ";
                SQL = SQL + ComNum.VBLF + "SugbB = '" + txtGbB.Text + "', ";
                SQL = SQL + ComNum.VBLF + "SugbC = '" + txtGbC.Text + "', ";
                SQL = SQL + ComNum.VBLF + "SugbD = '" + txtGbD.Text + "', ";
                SQL = SQL + ComNum.VBLF + "SugbE = '" + txtGbE.Text + "', ";
                SQL = SQL + ComNum.VBLF + "SugbF = '" + txtGbF.Text + "', ";
                SQL = SQL + ComNum.VBLF + "SugbG = '" + txtGbG.Text + "', ";
                SQL = SQL + ComNum.VBLF + "SugbH = '" + txtGbH.Text + "', ";
                SQL = SQL + ComNum.VBLF + "SugbI = '" + txtGbI.Text + "', ";
                SQL = SQL + ComNum.VBLF + "SugbJ = '" + txtGbJ.Text + "', ";
                SQL = SQL + ComNum.VBLF + "SugbK = '" + txtGbK.Text + "', ";
                SQL = SQL + ComNum.VBLF + "SugbL = '" + txtGbL.Text + "', ";
                SQL = SQL + ComNum.VBLF + "DayMax = '" + txtDayMax.Text + "', ";
                SQL = SQL + ComNum.VBLF + "TotMax = '" + txtTotMax.Text + "', ";
                SQL = SQL + ComNum.VBLF + "Sunext = '" + txtSuNext.Text + "', ";
                SQL = SQL + ComNum.VBLF + "DelDate = TO_DATE('" + txtDelDate.Text.Trim() + "','yyyy-mm-dd'),";
                SQL = SQL + ComNum.VBLF + "IAmt = " + nIAmt + ",TAmt=" + nTAmt + ",BAmt=" + nBAmt + ",";
                SQL = SQL + ComNum.VBLF + "Sudate = TO_DATE('" + strSuDate + "', 'yyyy-mm-dd'), ";
                SQL = SQL + ComNum.VBLF + "OldIAmt = " + nOldIAmt + ",OldTAmt=" + nOldTAmt + ",OldBAmt=" + nOldBAmt + ",";
                SQL = SQL + ComNum.VBLF + "Sudate3 = TO_DATE('" + strSuDate3 + "', 'yyyy-mm-dd'), ";
                SQL = SQL + ComNum.VBLF + "IAmt3 = " + nIAmt3 + ",TAmt3=" + nTAmt3 + ",BAmt3=" + nBAmt3 + ",";
                SQL = SQL + ComNum.VBLF + "Sudate4 = TO_DATE('" + strSuDate4 + "', 'yyyy-mm-dd'), ";
                SQL = SQL + ComNum.VBLF + "IAmt4 = " + nIAmt4 + ",TAmt4=" + nTAmt4 + ",BAmt4=" + nBAmt4 + ",";
                SQL = SQL + ComNum.VBLF + "Sudate5 = TO_DATE('" + strSuDate5 + "', 'yyyy-mm-dd'), ";
                SQL = SQL + ComNum.VBLF + "IAmt5 = " + nIAmt5 + ",TAmt5=" + nTAmt5 + ",BAmt5=" + nBAmt5 + " ";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
            }
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("BAS_SUN에 자료를 UPDATE시 오류가 발생함");
                Cursor.Current = Cursors.Default;
                return false;
            }

            return true;
        }

        private bool SUGA_Amt_RTN()
        {
            nCNT = 0;
            string strROWID;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            for (int i = 0; i < ss1_Sheet1.Columns.Count; i++)
            {
                strDel = ss1_Sheet1.Cells[0, i].Text;
                strSuDate = ComFunc.FormatStrToDateTime(ss1_Sheet1.Cells[1, i].Text, "D");
                nBAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[2, i].Text));
                nTAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[3, i].Text));
                nIAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[4, i].Text));
                nSAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[5, i].Text));
                nSelAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[6, i].Text));
                strROWID = ss1_Sheet1.Cells[7, i].Text;

                if (strDel == "" && strSuDate != "")
                {

                    if (nCNT < 5)
                    {
                        strSuga[nCNT, 0] = strSuDate;
                        strSuga[nCNT, 1] = nBAmt.ToString();
                        strSuga[nCNT, 2] = nTAmt.ToString();
                        strSuga[nCNT, 3] = nIAmt.ToString();
                    }
                    nCNT = nCNT + 1;
                }

                if (strSuDate == "" && (nBAmt != 0 || nTAmt != 0 || nIAmt != 0)) strSuDate = "1800-01-01";

                if (strDel == "True")
                {
                    if (strROWID != "")  //'삭제
                    {
                        SQL = "UPDATE  KOSMOS_PMPA.BAS_SUGA_AMT SET DELDATE = SYSDATE , DELSABUN = '" + clsType.User.Sabun + "'  WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }
                else if (strROWID == "")    //'등록
                {
                    if (strSuDate != "")
                    {
                        SQL = " INSERT INTO  KOSMOS_PMPA.BAS_SUGA_AMT ( SUCODE, SUNEXT , SUDATE, BAMT, TAMT, IAMT, SAMT, SelAmt ) VALUES ( ";
                        SQL = SQL + " '" + txtCode.Text + "','" + txtSuNext.Text + "' , TO_DATE('" + strSuDate + "','YYYY-MM-DD') , ";
                        SQL = SQL + " '" + nBAmt + "', '" + nTAmt + "',  '" + nIAmt + "' , '" + nSAmt + "', '" + nSelAmt + "' )  ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }
                else  // '갱신
                {
                    SQL = "UPDATE KOSMOS_PMPA.BAS_SUGA_AMT SET SUDATE  = TO_DATE('" + strSuDate + "','YYYY-MM-DD') ,";
                    SQL = SQL + "  BAMT = '" + nBAmt + "', ";
                    SQL = SQL + "  TAMT = '" + nTAmt + "', ";
                    SQL = SQL + "  IAMT = '" + nIAmt + "', ";
                    SQL = SQL + "  SAMT = '" + nSAmt + "', ";
                    SQL = SQL + "  SelAMT = '" + nSelAmt + "' ";
                    SQL = SQL + " WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }


            }
            return true;
        }

        /// <summary>
        /// 숫자인지 문자인지 비교 후 숫자로 리턴
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private int CompareNumberString(string strValue)
        {
            if (VB.IsNumeric(strValue) == true)
            {
                return Convert.ToInt32(VB.Val(strValue));
            }
            else
            {
                return VB.Asc(strValue);
            }
        }

        private bool Data_Field_Check() //'입력한 내용을 Check
        {
            int i;

            if (txtCode.Text.Trim() != txtSuNext.Text.Trim())
            { 
                ComFunc.MsgBox("수가코드와 품명코드 틀림", "품명코드오류");
                txtSuNext.Focus();
                return false;
            }
            if (VB.RTrim(txtSuNameK.Text) == "")
            {
                ComFunc.MsgBox("수가명이 입력되지 않았읍니다", "한글수가명 선택");
                txtSuNameK.Focus();
                return false;
            }


            if (CompareNumberString(VB.RTrim(txtGbA.Text)) < CompareNumberString("1") || CompareNumberString(VB.Trim(txtGbA.Text)) > CompareNumberString("3"))
            {
                ComFunc.MsgBox("항목이 선택되지 않았읍니다", "A항 선택");
                txtGbA.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbB.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbB.Text)) > CompareNumberString("Z"))
            {
                ComFunc.MsgBox("항목이 선택되지 않았읍니다", "B항 선택");
                txtGbB.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbC.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbC.Text)) > CompareNumberString("8"))
            {
                ComFunc.MsgBox("항목이 선택되지 않았읍니다", "C항 선택");
                txtGbC.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbD.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbD.Text)) > CompareNumberString("F"))
            {
                ComFunc.MsgBox("항목이 선택되지 않았읍니다", "D항 선택");
                txtGbD.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbE.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbE.Text)) > CompareNumberString("1"))
            {
                ComFunc.MsgBox("항목이 선택되지 않았읍니다", "E항 선택");
                txtGbE.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbF.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbF.Text)) > CompareNumberString("3"))
            {
                ComFunc.MsgBox("항목이 선택되지 않았읍니다", "F항 선택");
                txtGbF.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbG.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbG.Text)) > CompareNumberString("6"))
            {
                ComFunc.MsgBox("항목이 선택되지 않았읍니다", "G항 선택");
                txtGbG.Focus();
                return false;
            }


            if (CompareNumberString(VB.RTrim(txtGbH.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbH.Text)) > CompareNumberString("2"))
            {
                ComFunc.MsgBox("항목이 선택되지 않았읍니다", "H항 선택");
                txtGbH.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbI.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbI.Text)) > CompareNumberString("8"))
            {
                ComFunc.MsgBox("항목이 선택되지 않았읍니다", "I항 선택");
                txtGbI.Focus();
                return false;
            }


            if (txtBun.Text == "11" || txtBun.Text == "12" || txtBun.Text == "20" || txtBun.Text == "23")
            {
                if (CompareNumberString(VB.RTrim(txtGbJ.Text)) < CompareNumberString("1") && CompareNumberString(VB.Trim(txtGbJ.Text)) > CompareNumberString("3"))
                {
                    ComFunc.MsgBox("항목이 선택되지 않았읍니다(1,2,3)", "J항 선택");
                    txtGbJ.Focus();
                    return false;
                }
            }
            else
            {
                if (CompareNumberString(VB.RTrim(txtGbJ.Text)) != CompareNumberString("0") && CompareNumberString(VB.Trim(txtGbJ.Text)) != CompareNumberString("9") && CompareNumberString(VB.Trim(txtGbJ.Text)) != CompareNumberString("8") && CompareNumberString(VB.Trim(txtGbJ.Text)) != CompareNumberString("5"))
                {
                    ComFunc.MsgBox("항목이 선택되지 않았읍니다(0,5,8,9)", "J항 선택");
                    txtGbJ.Focus();
                    return false;
                }
            }


            if (CompareNumberString(VB.RTrim(txtGbK.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbK.Text)) > CompareNumberString("2"))
            {
                ComFunc.MsgBox("항목이 선택되지 않았읍니다", "K항 선택");
                txtGbK.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbL.Text)) < CompareNumberString("1") || CompareNumberString(VB.Trim(txtGbL.Text)) > CompareNumberString("8"))
            {
                ComFunc.MsgBox("항목이 선택되지 않았읍니다(1,2,3,4,7,8)", "L항 선택");
                txtGbL.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbM.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbM.Text)) > CompareNumberString("1"))
            {
                ComFunc.MsgBox("항목이 선택되지 않았읍니다", "M항 선택");
                txtGbM.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbN.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbN.Text)) > CompareNumberString("1"))
            {
                ComFunc.MsgBox("항목이 선택되지 않았읍니다", "N항 선택");
                txtGbN.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbO.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbO.Text)) > CompareNumberString("C"))
            {
                ComFunc.MsgBox("의약분업 0 - C만 가능합니다.", "O항 선택");
                txtGbO.Focus();
                return false;
            }


            if (!(CompareNumberString(VB.Trim(txtGbP.Text)) == CompareNumberString("0")
                || CompareNumberString(VB.Trim(txtGbP.Text)) == CompareNumberString("1")
                || CompareNumberString(VB.Trim(txtGbP.Text)) == CompareNumberString("2")
                || CompareNumberString(VB.Trim(txtGbP.Text)) == CompareNumberString("9")))
            {
                ComFunc.MsgBox("비급여분류(0. 1.인정 2.임의 9.제외)만 가능합니다.", "P항 선택");
                txtGbP.Focus();
                return false;
            }



            if (!(CompareNumberString(VB.RTrim(txtGbQ.Text)) == CompareNumberString("0")
                || CompareNumberString(VB.Trim(txtGbQ.Text)) == CompareNumberString("1")))
            {
                ComFunc.MsgBox("산재급여(0.적용안함 1.급여처리)만 가능합니다.", "Q항 선택");
                txtGbQ.Focus();
                return false;
            }



            if (!(CompareNumberString(VB.RTrim(txtGbR.Text)) == CompareNumberString("0")
                || CompareNumberString(VB.Trim(txtGbR.Text)) == CompareNumberString("1")))
            {
                ComFunc.MsgBox("외래비급여항목중(0.자보급여 1.자보비급여)만 가능합니다.", "R항 선택");
                txtGbR.Focus();
                return false;
            }

            //'2017-12-27
            if (!(CompareNumberString(VB.RTrim(txtGbS.Text)) == CompareNumberString("0")
                || CompareNumberString(VB.Trim(txtGbS.Text)) == CompareNumberString("1")
                || CompareNumberString(VB.Trim(txtGbS.Text)) == CompareNumberString("2")
                || CompareNumberString(VB.Trim(txtGbS.Text)) == CompareNumberString("3")
                || CompareNumberString(VB.Trim(txtGbS.Text)) == CompareNumberString("4")
                || CompareNumberString(VB.Trim(txtGbS.Text)) == CompareNumberString("5")
                || CompareNumberString(VB.Trim(txtGbS.Text)) == CompareNumberString("6")
                || CompareNumberString(VB.Trim(txtGbS.Text)) == CompareNumberString("7")
                || CompareNumberString(VB.Trim(txtGbS.Text)) == CompareNumberString("8")
                || CompareNumberString(VB.Trim(txtGbS.Text)) == CompareNumberString("9")  //2019-08-30
                ))
            {
                ComFunc.MsgBox("100/100 표기(1.100/100,2.코로나임시(20%) 3.100/30(중복), 4.100/80, 5.100/50 6.100/80(중복)) 7.100/50(중복) 8.100/90(중복), 9.100/90 만 가능합니다.", "S항 선택");
                txtGbS.Focus();
                return false;
            }
            if (!(CompareNumberString(VB.RTrim(txtGbT.Text)) == CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbT.Text)) == CompareNumberString("1")))
            {
                ComFunc.MsgBox("DRG 비급여항목(0.DRG 급여, 1.DRG비급여)만 가능합니다.", "T항 선택");
                txtGbT.Focus();
                return false;
            }


            if (!(CompareNumberString(VB.RTrim(txtGbU.Text)) == CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbU.Text)) == CompareNumberString("1") || CompareNumberString(VB.Trim(txtGbU.Text)) == CompareNumberString("2")))
            {
                ComFunc.MsgBox("고가약제 여부(1.고가, 0.기본, 2.저가)만 가능합니다.", "U항 선택");
                txtGbU.Focus();
                return false;
            }


            switch (txtGbV.Text)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "9":
                    break;
                default:
                    ComFunc.MsgBox("약제, 재료 수불 0,1,2,3,4,5,6,9까지만 가능합니다.", "V항 선택");
                    txtGbV.Focus();
                    return false;
            }



            if (CompareNumberString(VB.RTrim(txtGbW.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbW.Text)) > CompareNumberString("2"))
            {
                ComFunc.MsgBox("치매진단 0 - 2만 가능합니다.", "W항 선택");
                txtGbW.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbX.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbX.Text)) > CompareNumberString("1"))
            {
                ComFunc.MsgBox("산재자보 기술료가산은 0, 1만 가능합니다.", "X항 선택");
                txtGbX.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbY.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbY.Text)) > CompareNumberString("2"))
            {
                ComFunc.MsgBox("외과가산은  0, 1, 2만 가능합니다.", "Y항 선택");
                txtGbY.Focus();
                return false;
            }


            //'2017-05-25
            if (CompareNumberString(VB.RTrim(txtGbZ.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbZ.Text)) > CompareNumberString("4"))
            {
                ComFunc.MsgBox("흉부외과가산은  0, 1, 2, 3, 4만 가능합니다.", "Z항 선택");
                txtGbZ.Focus();
                return false;
            }


            if (CompareNumberString(VB.RTrim(txtGbAA.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbAA.Text)) > CompareNumberString("3"))
            {
                ComFunc.MsgBox("응급가산은  0, 1, 2, 3만 가능합니다.", "AA항 선택");
                txtGbAA.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbAB.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbAB.Text)) > CompareNumberString("1"))
            {
                ComFunc.MsgBox("판독가산은  0, 1 만 가능합니다.", "AB항 선택");
                txtGbAB.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbAC.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbAC.Text)) > CompareNumberString("3"))
            {
                ComFunc.MsgBox("마취가산은  0, 1, 2, 3 만 가능합니다.", "AC항 선택");
                txtGbAC.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbAD.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbAD.Text)) > CompareNumberString("1"))
            {
                ComFunc.MsgBox("화상가산은  0, 1 만 가능합니다.", "AD항 선택");
                txtGbAD.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbAE.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbAE.Text)) > CompareNumberString("3"))
            {
                ComFunc.MsgBox("신경외과가산은  0, 1, 2, 3 만 가능합니다.", "AE항 선택");
                txtGbAE.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbAF.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbAF.Text)) > CompareNumberString("1"))
            {
                ComFunc.MsgBox("호스피스 별도산정수가 대상은 0, 1 만 가능합니다.", "AF항 선택");
                txtGbAF.Focus();
                return false;
            }

            if (CompareNumberString(VB.RTrim(txtGbAG.Text)) < CompareNumberString("0") || CompareNumberString(VB.Trim(txtGbAG.Text)) > CompareNumberString("1"))
            {
                ComFunc.MsgBox("ASA가산  대상은 0, 1 만 가능합니다.", "AG항 선택");
                txtGbAG.Focus();
                return false;
            }

            if (VB.RTrim(txtSuNext.Text) == "")
            {
                ComFunc.MsgBox("품목코드가 선택되지 않았읍니다", "품목코드선택");
                txtSuNext.Focus();
                return false;
            }

            if (VB.RTrim(txtBun.Text) == "")
            {
                ComFunc.MsgBox("분류코드가 선택되지 않았읍니다", "분류코드선택");
                txtBun.Focus();
                return false;
            }

            if (VB.RTrim(txtNu.Text) == "")
            {
                ComFunc.MsgBox("누적코드가 선택되지 않았읍니다", "누적코드선택");
                txtNu.Focus();
                return false;
            }

            if (VB.RTrim(ss3_Sheet1.Cells[2, 0].Text) == "" && txtGbF.Text == "0")
            {
                ComFunc.MsgBox("보험코드가 선택되지 않았습니다", "보험코드선택");
                ss3.Focus();
                return false;
            }

            if (txtGbF.Text == "0" && (CompareNumberString(ss3_Sheet1.Cells[4, 0].Text) < CompareNumberString("1") || CompareNumberString(ss3_Sheet1.Cells[4, 0].Text) > CompareNumberString("8")))
            {
                ComFunc.MsgBox("표준코드 종류가 오류입니다(1,2,3,4,7,8만가능)", "표준코드종류");
                ss3.Focus();
                return false;
            }



            if (txtGbF.Text != "1" && CompareNumberString(ss3_Sheet1.Cells[2, 0].Text) == CompareNumberString("999999"))
            {
                ComFunc.MsgBox("F항이 '1' 인경우만 표준코드 999999 입력 가능합니다.", "표준코드종류");
            }

            if (txtGbF.Text == "0" && txtGbL.Text != ss3_Sheet1.Cells[4, 0].Text)
            {
                ComFunc.MsgBox("L항과 표준코드종류가 틀림", "L항 선택");
                txtGbL.Focus();
                return false;
            }



            if (VB.RTrim(txtWon.Text) == "")
            {
                ComFunc.MsgBox("원가분류가 공란입니다.", "원가분류선택");
                txtWon.Focus();
                return false;
            }
            strDtlBun = VB.Trim(VB.Left(cboDtlBun.Text, 4));





            //    'BAS_SUH입력내용 점검
            FailFlag = false;

            for (i = 0; i < ss2_Sheet1.Rows.Count; i++)
            {
                if (SS2_TO_String(i) == false)        //'Sheet의 값을 변수로 Move
                {
                    return false;
                }

                if (strDel != "1")
                {
                    if (strBun != "" || strNu != "" || strSuNext != "" || strSunameK != "")
                    {
                        if (strBun == "")
                        {
                            ComFunc.MsgBox("분류코드가 공란입니다.", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strNu == "")
                        {
                            ComFunc.MsgBox("누적코드가 공란입니다.", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strSuNext == "")
                        {
                            ComFunc.MsgBox("품명코드가 공란입니다.", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strSunameK == "")
                        {
                            ComFunc.MsgBox("한글명칭이 공란입니다.", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strSugbSS == "" || CompareNumberString(strSugbSS) > CompareNumberString("5"))
                        {
                            ComFunc.MsgBox("소아성인구분 0-5만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (CompareNumberString(strSugbBi) < CompareNumberString("0") || CompareNumberString(strSugbBi) > CompareNumberString("2"))
                        {
                            ComFunc.MsgBox("보험구분 0-2만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (nSuQty == 0)
                        {
                            ComFunc.MsgBox("수량이 Zero 입니다.", "GROUP코드");
                            FailFlag = true;
                        }
                        if (!(CompareNumberString(strSugbA) == CompareNumberString("2") || CompareNumberString(strSugbA) == CompareNumberString("3")))
                        {
                            ComFunc.MsgBox("수가구분 A항은 2,3만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (CompareNumberString(strSugbB) < CompareNumberString("0") || CompareNumberString(strSugbB) > CompareNumberString("Z"))
                        {
                            ComFunc.MsgBox("수가구분 B항은 0-9,A-Z만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        //if (CompareNumberString(strSugbC) < CompareNumberString("0") || CompareNumberString(strSugbC) > CompareNumberString("6"))
                        if (CompareNumberString(strSugbC) < CompareNumberString("0") || CompareNumberString(strSugbC) > CompareNumberString("7")) //2019-08-02 수가 C항 7 번추가
                        {
                            ComFunc.MsgBox("수가구분 C항은 0-7만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (CompareNumberString(strSugbD) < CompareNumberString("0") || CompareNumberString(strSugbD) > CompareNumberString("F"))
                        {
                            ComFunc.MsgBox("수가구분 D항은 0-F만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (CompareNumberString(strSugbE) < CompareNumberString("0") || CompareNumberString(strSugbE) > CompareNumberString("1"))
                        {
                            ComFunc.MsgBox("수가구분 E항은 0-1만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (CompareNumberString(strSugbF) < CompareNumberString("0") || CompareNumberString(strSugbF) > CompareNumberString("2"))
                        {
                            ComFunc.MsgBox("수가구분 F항은 0-2만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (CompareNumberString(strSugbG) < CompareNumberString("0") || CompareNumberString(strSugbG) > CompareNumberString("6"))
                        {
                            ComFunc.MsgBox("수가구분 G항은 0-6만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (CompareNumberString(strSugbH) < CompareNumberString("0") || CompareNumberString(strSugbH) > CompareNumberString("2"))
                        {
                            ComFunc.MsgBox("수가구분 H항은 0-2만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (CompareNumberString(strSugbI) < CompareNumberString("0") || CompareNumberString(strSugbI) > CompareNumberString("2"))
                        {
                            ComFunc.MsgBox("수가구분 I항은 0-8만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (CompareNumberString(strBun) == CompareNumberString("11") || CompareNumberString(strBun) == CompareNumberString("12") || CompareNumberString(strBun) == CompareNumberString("20"))
                        {
                            if (CompareNumberString(strSugbJ) < CompareNumberString("1") && CompareNumberString(strSugbJ) > CompareNumberString("3"))
                            {
                                ComFunc.MsgBox("수가구분 J항은 1,2,3만 가능함", "GROUP코드");
                                FailFlag = true;
                            }
                        }
                        else
                        { 
                            if (strSugbJ != "0" && strSugbJ != "9")
                            {
                                ComFunc.MsgBox("수가구분 J항은 0,9만 가능함", "GROUP코드");
                                FailFlag = true;
                            }
                        }
                        if (CompareNumberString(strSugbK) < CompareNumberString("0") || CompareNumberString(strSugbK) > CompareNumberString("2"))
                        {
                            ComFunc.MsgBox("수가구분 K항은 0-2만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (CompareNumberString(strSugbL) < CompareNumberString("1") || CompareNumberString(strSugbL) > CompareNumberString("8"))
                        {
                            ComFunc.MsgBox("수가구분 L항은 1-8만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strSugbM != "0" && strSugbM != "1")
                        {
                            ComFunc.MsgBox("수가구분 M항은 0,1만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strSugbN != "0" && strSugbN != "1")
                        {
                            ComFunc.MsgBox("수가구분 N항은 0,1만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (CompareNumberString(strSugbO) < CompareNumberString("0") || CompareNumberString(strSugbO) > CompareNumberString("C"))
                        {
                            ComFunc.MsgBox("수가구분 O항은 0-C만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strSugbP != "1" && strSugbP != "2" && strSugbP != "9")
                        {
                            ComFunc.MsgBox("수가구분 P항은 (1.인정 2.임의 9.제외) 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (CompareNumberString(strSugbQ) < CompareNumberString("0") || CompareNumberString(strSugbQ) > CompareNumberString("1"))
                        {
                            ComFunc.MsgBox("수가구분 Q은 산재급여 0,1 만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strSugbR != "0" && strSugbR != "1")
                        {
                            ComFunc.MsgBox("수가구분 R은 외래비급여항목 자보급여/비급여중 0,1 만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strSugbS != "0" && strSugbS != "1" && strSugbS != "4" && strSugbS != "5" && strSugbS != "6" && strSugbS != "3")
                        {
                            ComFunc.MsgBox("수가구분 S은 100/100 표시중 (1.100/100,2.코로나임시(20%) 3.100/30, 4.100/80, 5.100/50 6.100/80(비급여9/1적용), 만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strSugbT != "0" && strSugbT != "1")
                        {
                            ComFunc.MsgBox("수가구분 T은 DRG 비급여표시중 0,1 만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strSugbU != "0" && strSugbU != "1" && strSugbU != "2")
                        {
                            ComFunc.MsgBox("수가구분 U은 고가약제표시중 0,1,2 만 가능함", "GROUP코드");
                            FailFlag = true;
                        }


                        if (strSugbAA != "0" && strSugbAA != "1" && strSugbAA != "2" && strSugbAA != "3")
                        {
                            ComFunc.MsgBox("수가구분 AA 응금가산은 0,1,2,3 만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strSugbAB != "0" && strSugbAB != "1")
                        {
                            ComFunc.MsgBox("수가구분 AB 판독가산은 0,1 만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strSugbAC != "0" && strSugbAC != "1" && strSugbAC != "2" && strSugbAC != "3")
                        {
                            ComFunc.MsgBox("수가구분 AC 마취가산은 0,1,2,3 만 가능함", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strSugbAD != "0" && strSugbAD != "1")
                        {
                            ComFunc.MsgBox("수가구분 AD 화상가산은 0,1 만 가능함", "GROUP코드");
                            FailFlag = true;
                        }

                        if (strSugbAE != "0" && strSugbAE != "1" && strSugbAE != "2" && strSugbAE != "3")
                        {
                            ComFunc.MsgBox("수가구분 AE 신경외과가산은 0,1,2,3 만 가능함", "GROUP코드");
                            FailFlag = true;
                        }

                        if (strSugbAF != "0" && strSugbAF != "1")
                        {
                            ComFunc.MsgBox("수가구분 AF 호스피스 별도산정수가는 0,1 만 가능함", "GROUP코드");
                            FailFlag = true;
                        }

                        if (strSugbAG != "0" && strSugbAG != "1")
                        {
                            ComFunc.MsgBox("수가구분 AG ASA수가는 0,1 만 가능함", "GROUP코드");
                            FailFlag = true;
                        }

                        switch (strSugbV)
                        {
                            case "0":
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "9":
                                break;
                            default:
                                ComFunc.MsgBox("수가구분 V항은 약제, 재료 수불구분 0,1,2,3,4,5,6,9 만 가능함", "GROUP코드");
                                FailFlag = true;
                                break;
                        }


                        if (CompareNumberString(strSugbX) < CompareNumberString("0") || CompareNumberString(strSugbX) > CompareNumberString("1"))
                        {
                            ComFunc.MsgBox("수가구분 X항은 0-1만 가능함", "GROUP코드");
                            FailFlag = true;
                        }


                        if (strBCode == "" && strSugbF == "0")
                        {
                            ComFunc.MsgBox("보험코드가 선택되지 않았습니다", "GROUP코드");
                            FailFlag = true;
                        }
                        if (strBCode != "" && nSuham == 0)
                        {
                            nSuham = 1;
                        }
                        if (strSugbF == "0" && (CompareNumberString(strEdiJong) < CompareNumberString("1") || CompareNumberString(strEdiJong) > CompareNumberString("8")))
                        {
                            ComFunc.MsgBox("표준코드 종류가 오류입니다(1,2,3,4,7,8만가능)", "GROUP코드");
                            FailFlag = true;
                        }

                        //if (strOldBCode == "" && strEdiDate != "") 2020-02-08 정희정 계장 요청으로 제외
                        //{
                            //ComFunc.MsgBox("Old표준코드는 공란이나 적용일자가 공란이 아님", "GROUP코드");
                            //FailFlag = true;
                        //}

                        if (strOldBCode != "" && (CompareNumberString(strOldJong) < CompareNumberString("1") || CompareNumberString(strOldJong) > CompareNumberString("8")))
                        {
                            ComFunc.MsgBox("표준코드 OLD종류가 오류입니다(1,2,3,4,7,8만가능)", "GROUP코드");
                            FailFlag = true;
                        }

                        //if (strOldBCode != "" && strEdiDate == "") 2020-02-08 정희정 계장 요청으로 제외
                        //{
                        //    ComFunc.MsgBox("OLD표준코드 적용일자가 공란입니다.", "GROUP코드");
                        //    FailFlag = true;
                        //}

                        if (strWonCode == "")
                        {
                            ComFunc.MsgBox("원가분류가 공란입니다.", "GROUP코드");
                            FailFlag = true;
                        }
                    }

                    if (FailFlag == true) break;
                }
            } // For

            if (FailFlag == true)
            {
                ss2.Focus();
                ss2_Sheet1.SetActiveCell(i, 2);
                return false;
            }

            return true;
        }

        private bool DRUG_JEP_CHECK() //약제팀 코드 중복 체크
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SQL = "SELECT JEPCODE FROM KOSMOS_ADM.DRUG_JEP WHERE JEPCODE='" + txtCode.Text.Trim().ToUpper() + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return false;
            }

            if (dt.Rows.Count > 0)
            {
                ComFunc.MsgBox("변경하실 코드가 [ 약제팀 ] 코드 중복 코드입니다.", "코드변경 오류");
                txtCode.Focus();
                return false;
            }

            dt.Dispose();
            dt = null;

            return true;
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {

            int i;
            int nBAmt;
            int nTAmt;
            int nIAmt;
            int nSAmt;
            int nSelAmt;
            string strSuDate;
            string strROWID;
            string strDel;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            //////clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);



            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인


                if (txtSCode_H.Text == "" || txtSNext_H.Text == "")
                {
                    ComFunc.MsgBox("하위코드를 다시 선택 하세요", "확인");
                    return;
                }

                for (i = 0; i < ssAMT2_Sheet1.ColumnCount; i++)
                {
                    strDel = ssAMT2_Sheet1.Cells[0, i].Text;
                    strSuDate = ssAMT2_Sheet1.Cells[1, i].Text;
                    nBAmt = Convert.ToInt32(VB.Val(ssAMT2_Sheet1.Cells[2, i].Text));
                    nTAmt = Convert.ToInt32(VB.Val(ssAMT2_Sheet1.Cells[3, i].Text));
                    nIAmt = Convert.ToInt32(VB.Val(ssAMT2_Sheet1.Cells[4, i].Text));
                    nSAmt = Convert.ToInt32(VB.Val(ssAMT2_Sheet1.Cells[5, i].Text));
                    nSelAmt = Convert.ToInt32(VB.Val(ssAMT2_Sheet1.Cells[6, i].Text));
                    strROWID = ssAMT2_Sheet1.Cells[7, i].Text;

                    if (strSuDate == "" && (nBAmt != 0 || nTAmt != 0 || nIAmt != 0))
                    {
                        strSuDate = "1800-01-01";
                    }

                    if (strDel == "True")
                    {
                        if (strROWID != "")  //'삭제
                        {
                            SQL = "UPDATE  KOSMOS_PMPA.BAS_SUGA_AMT SET DELDATE = SYSDATE , DELSABUN = '" + clsType.User.Sabun + "'  WHERE ROWID = '" + strROWID + "' ";
                        }
                    }
                    else if (strROWID == "")    //'등록
                    {
                        if (strSuDate != "")
                        {
                            SQL = " INSERT INTO  KOSMOS_PMPA.BAS_SUGA_AMT ( SUCODE, SUNEXT , SUDATE, BAMT, TAMT, IAMT, SAMT, SELAMT ) VALUES ( ";
                            SQL = SQL + " '" + txtSCode_H.Text + "','" + txtSNext_H.Text + "' , TO_DATE('" + strSuDate + "','YYYY-MM-DD') , ";
                            SQL = SQL + " '" + nBAmt.ToString() + "', '" + nTAmt.ToString() + "',  '" + nIAmt.ToString() + "' , '" + nSAmt.ToString() + "' , '" + nSelAmt.ToString() + "' )  ";
                        }
                    }
                    else// '갱신                
                    {
                        SQL = "UPDATE KOSMOS_PMPA.BAS_SUGA_AMT SET SUDATE  = TO_DATE('" + strSuDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + "  BAMT = '" + nBAmt + "', ";
                        SQL = SQL + "  TAMT = '" + nTAmt + "', ";
                        SQL = SQL + "  IAMT = '" + nIAmt + "', ";
                        SQL = SQL + "  SAMT = '" + nSAmt + "',";
                        SQL = SQL + "  SelAMT = '" + nSelAmt + "'";
                        SQL = SQL + " WHERE ROWID = '" + strROWID + "' ";
                    }

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

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnSearchSameComponent_Click(object sender, EventArgs e)
        {

            GstrHelpCode = "";
            GstrHelpCode = txtCode.Text;

            //닫는 이벤트 내용
            if (frmSCodeX != null)
            {
                frmSCodeX.Dispose();
                frmSCodeX = null;
            }
            frmSCodeX = new frmSCode(GstrHelpCode);
            frmSCodeX.StartPosition = FormStartPosition.CenterParent;
            frmSCodeX.rSendHelpCode += frmSCodeX_rSendHelpCode;
            frmSCodeX.rEventClose += frmSCodeX_rEventClose;
            frmSCodeX.ShowDialog();


            if (GstrHelpCode != "")
            {
                txtCode.Text = GstrHelpCode;
                txtCode_Leave(txtCode, e);
            }
        }

        private void frmSCodeX_rEventClose()
        {
            //닫는 이벤트 내용
            if (frmSCodeX != null)
            {
                frmSCodeX.Dispose();
                frmSCodeX = null;
            }
        }

        private void frmSCodeX_rSendHelpCode(string strHelpCode)
        {
            if (strHelpCode != "")
            {
                GstrHelpCode = strHelpCode;
            }
        }

        private void btnSearchSelf_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();

            //닫는 이벤트 내용
            if (frmSearchUnpaidX != null)
            {
                frmSearchUnpaidX.Dispose();
                frmSearchUnpaidX = null;
            }
            frmSearchUnpaidX = new frmSearchUnpaid();
            frmSearchUnpaidX.StartPosition = FormStartPosition.CenterParent;
            frmSearchUnpaidX.ShowDialog();
        }

        private void TxtCode_CodeChange()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                //'BAS_SUT에 코드가 있으면 변경 않됨
                SQL = "SELECT ROWID FROM BAS_SUT WHERE SUCODE='" + txtCode.Text.Trim().ToUpper() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("변경하실 코드가 등록되어 있는 코드입니다.", "코드변경 오류");
                    txtCode.Text = "";
                    txtCode.Focus();
                    return;
                }

                FnCodeChange = false;
                txtSuNext.Text = txtCode.Text;
                FstrSuNext = txtCode.Text;
                txtCode.ReadOnly = true;
                btnDelete.Enabled = false;
                txtGbA.Focus();

                //'그룹코드 자료가 있으면 수정여부에 'Y'를 SET
                for (i = 0; i < ss2_Sheet1.RowCount; i++)
                {
                    ss2_Sheet1.Cells[i, 75 + 3].Text = "Y";
                    ss2_Sheet1.Cells[i, 94 + 3].Text = "";      //'신규로 INSERT하기 위하여 ROWID를 Clear
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

        }

        private void cboSuNext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void cboUnit_New2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void cboUnit_New3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuList_Click(object sender, EventArgs e)
        {

            //닫는 이벤트 내용
            if (frmSugaListX != null)
            {
                frmSugaListX.Show();
                return;
                //frmSugaListX.Dispose();
                //frmSugaListX = null;
            }
            GstrHelpCode = "";
            frmSugaListX = new frmSugaList();
            frmSugaListX.StartPosition = FormStartPosition.CenterParent;
            frmSugaListX.rSetHelpCode += frmSugaListX_rSetHelpCode;
            frmSugaListX.rEventClosed += frmSugaListX_rEventClosed;
            frmSugaListX.FormClosed += FrmSugaListX_FormClosed;
            frmSugaListX.Show();
        }

        private void FrmSugaListX_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmSugaListX.Dispose();
            frmSugaListX = null;
        }

        private void frmSugaListX_rEventClosed()
        {
            //닫는 이벤트 내용
            if (frmSugaListX != null)
            {
                frmSugaListX.Dispose();
                frmSugaListX = null;
            }
        }

        private void frmSugaListX_rSetHelpCode(string strHelpCode)
        {
            if (strHelpCode != "")
            {
                GstrHelpCode = strHelpCode;

                txtCode.Text = GstrHelpCode;
                Screen_Display();
                lblMsg.Text = "";

                GstrHelpCode = "";
                txtGbA.Focus();
            }
        }

        private void mnuView10_Click(object sender, EventArgs e)
        {
            GstrHelpCode = ss3_Sheet1.Cells[2, 0].Text;

            //닫는 이벤트 내용
            if (frmTACostViewX != null)
            {
                frmTACostViewX.Dispose();
                frmTACostViewX = null;
            }
            frmTACostViewX = new frmTACostView();
            frmTACostViewX.StartPosition = FormStartPosition.CenterParent;
            frmTACostViewX.Show();
        }

        private void mnuView1_Click(object sender, EventArgs e)
        {
            GstrBCodeShow = "OK";

            //닫는 이벤트 내용
            if (frmSearchBCodeX != null)
            {
                frmSearchBCodeX.Dispose();
                frmSearchBCodeX = null;
            }
            frmSearchBCodeX = new frmSearchBCode();
            frmSearchBCodeX.StartPosition = FormStartPosition.CenterParent;
            frmSearchBCodeX.Show();
        }

        private void mnuView2_Click(object sender, EventArgs e)
        {
            GstrHelpCode = ss3_Sheet1.Cells[1, 1].Text;

            //닫는 이벤트 내용
            if (frmSearchGuipX != null)
            {
                frmSearchGuipX.Dispose();
                frmSearchGuipX = null;
            }
            frmSearchGuipX = new frmSearchGuip(GstrHelpCode);
            frmSearchGuipX.StartPosition = FormStartPosition.CenterParent;
            frmSearchGuipX.Show();
        }

        private void mnuView3_Click(object sender, EventArgs e)
        {
            GstrHelpCode = ss3_Sheet1.Cells[2, 0].Text;

            //닫는 이벤트 내용
            if (frmYGuipViewX != null)
            {
                frmYGuipViewX.Dispose();
                frmYGuipViewX = null;
            }
            frmYGuipViewX = new frmYGuipView(GstrHelpCode, clsType.User.Sabun);
            frmYGuipViewX.StartPosition = FormStartPosition.CenterParent;
            frmYGuipViewX.Show();
        }

        private void mnuView4_Click(object sender, EventArgs e)
        {
            //닫는 이벤트 내용
            if (frmGiSugaHelpX != null)
            {
                frmGiSugaHelpX.Dispose();
                frmGiSugaHelpX = null;
            }
            frmGiSugaHelpX = new frmGiSugaHelp();
            frmGiSugaHelpX.StartPosition = FormStartPosition.CenterParent;
            frmGiSugaHelpX.Show();
        }

        private void mnuView5_Click(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            //닫는 이벤트 내용
            if (frmSugaCompareX != null)
            {
                frmSugaCompareX.Dispose();
                frmSugaCompareX = null;
            }
            frmSugaCompareX = new frmSugaCompare();
            frmSugaCompareX.StartPosition = FormStartPosition.CenterParent;
            frmSugaCompareX.rSetHelpCode += frmSugaCompareX_rSetHelpCode;
            frmSugaCompareX.rEventClosed += frmSugaCompareX_rEventClosed;
            frmSugaCompareX.ShowDialog();

            if (GstrHelpCode != "")
            {
                txtCode.Text = GstrHelpCode;
                Screen_Display();
                lblMsg.Text = "";
                GstrHelpCode = "";
                txtGbA.Focus();
                return;
            }

        }

        private void frmSugaCompareX_rEventClosed()
        {
            //닫는 이벤트 내용
            if (frmSugaCompareX != null)
            {
                frmSugaCompareX.Dispose();
                frmSugaCompareX = null;
            }
        }

        private void frmSugaCompareX_rSetHelpCode(string strHelpCode)
        {
            if (strHelpCode != "")
            {
                GstrHelpCode = strHelpCode;
            }
        }

        private void menuView6_Click(object sender, EventArgs e)
        {
            //닫는 이벤트 내용
            if (frmSugaDayCountX != null)
            {
                frmSugaDayCountX.Dispose();
                frmSugaDayCountX = null;
            }
            frmSugaDayCountX = new frmSugaDayCount();
            frmSugaDayCountX.StartPosition = FormStartPosition.CenterParent;
            frmSugaDayCountX.Show();
        }

        private void mnuView7_Click(object sender, EventArgs e)
        {
            GstrHelpCode = txtCode.Text;

            //약품처방일수관리(약제과용)
            frmDrJob01X = new frmDrJob01(GstrHelpCode);
            frmDrJob01X.StartPosition = FormStartPosition.CenterParent;
            frmDrJob01X.Show();
        }

        private void mnuView8_Click(object sender, EventArgs e)
        {
            //닫는 이벤트 내용
            if (frmViewGojiX != null)
            {
                frmViewGojiX.Dispose();
                frmViewGojiX = null;
            }
            frmViewGojiX = new frmViewGoji();
            frmViewGojiX.StartPosition = FormStartPosition.CenterParent;
            frmViewGojiX.Show();
        }

        private void mnuView9_Click(object sender, EventArgs e)
        {
            if (txtBun.Text != "65") return;
            GstrHelpCode = txtCode.Text;

            if (GstrHelpCode == "") return;

            //닫는 이벤트 내용
            if (frmBasXrayX != null)
            {
                frmBasXrayX.Dispose();
                frmBasXrayX = null;
            }
            frmBasXrayX = new frmBasXray(GstrHelpCode);
            frmBasXrayX.StartPosition = FormStartPosition.CenterParent;
            frmBasXrayX.Show();
        }

        private void mnuJemsu_Click(object sender, EventArgs e)
        {
            //닫는 이벤트 내용
            if (frmPmpaMirJemsuEntryX != null)
            {
                frmPmpaMirJemsuEntryX.Dispose();
                frmPmpaMirJemsuEntryX = null;
            }
            frmPmpaMirJemsuEntryX = new frmPmpaMirJemsuEntry();
            frmPmpaMirJemsuEntryX.StartPosition = FormStartPosition.CenterParent;
            frmPmpaMirJemsuEntryX.Show();
        }

        private void mnuSanjeng_Click(object sender, EventArgs e)
        {
            GstrSanCode = txtCode.Text.Trim();

            frmSanjengGijun frmSanjengGijunX = new frmSanjengGijun(GstrSanCode);
            frmSanjengGijunX.StartPosition = FormStartPosition.CenterParent;
            frmSanjengGijunX.Show();
        }

        private void mnuMessage01_Click(object sender, EventArgs e)
        {
            if (frmOcsMessageX != null)
            {
                frmOcsMessageX.Dispose();
                frmOcsMessageX = null;
            }
            frmOcsMessageX = new frmOcsMessage();
            frmOcsMessageX.StartPosition = FormStartPosition.CenterParent;
            frmOcsMessageX.Show();
        }

        private void mnuMessage02_Click(object sender, EventArgs e)
        {
            GstrSanCode = txtCode.Text;
            if (FrmOcsMsgx != null)
            {
                FrmOcsMsgx.Dispose();
                FrmOcsMsgx = null;
            }
            FrmOcsMsgx = new FrmOcsMsg(GstrSanCode);
            FrmOcsMsgx.StartPosition = FormStartPosition.CenterParent;
            FrmOcsMsgx.Show();
        }

        private void mnuMessage03_Click(object sender, EventArgs e)
        {
            if (FrmOcsMsgPano_IX != null)
            {
                FrmOcsMsgPano_IX.Dispose();
                FrmOcsMsgPano_IX = null;
            }
            FrmOcsMsgPano_IX = new FrmOcsMsgPano_I();
            FrmOcsMsgPano_IX.StartPosition = FormStartPosition.CenterParent;
            FrmOcsMsgPano_IX.Show();
        }

        private void mnuMessage05_Click(object sender, EventArgs e)
        {
            GstrHelpCode = "진료과";

            if (FrmOcsMsgPano_O2X != null)
            {
                FrmOcsMsgPano_O2X.Dispose();
                FrmOcsMsgPano_O2X = null;
            }
            FrmOcsMsgPano_O2X = new FrmOcsMsgPano_O2(GstrHelpCode);
            FrmOcsMsgPano_O2X.StartPosition = FormStartPosition.CenterParent;
            FrmOcsMsgPano_O2X.Show();
        }

        private void mnuMessage06_Click(object sender, EventArgs e)
        {
            GstrHelpCode = "고지혈";

            if (FrmOcsMsgPano_O2X != null)
            {
                FrmOcsMsgPano_O2X.Dispose();
                FrmOcsMsgPano_O2X = null;
            }
            FrmOcsMsgPano_O2X = new FrmOcsMsgPano_O2(GstrHelpCode);
            FrmOcsMsgPano_O2X.StartPosition = FormStartPosition.CenterParent;
            FrmOcsMsgPano_O2X.Show();
        }

        private void mnuMessage07_Click(object sender, EventArgs e)
        {
            GstrHelpCode = "간장용제";

            if (FrmOcsMsgPano_O2X != null)
            {
                FrmOcsMsgPano_O2X.Dispose();
                FrmOcsMsgPano_O2X = null;
            }
            FrmOcsMsgPano_O2X = new FrmOcsMsgPano_O2(GstrHelpCode);
            FrmOcsMsgPano_O2X.StartPosition = FormStartPosition.CenterParent;
            FrmOcsMsgPano_O2X.Show();
        }

        private void mnuMessage08_Click(object sender, EventArgs e)
        {
            GstrHelpCode = "항암제";

            if (FrmOcsMsgPano_O2X != null)
            {
                FrmOcsMsgPano_O2X.Dispose();
                FrmOcsMsgPano_O2X = null;
            }
            FrmOcsMsgPano_O2X = new FrmOcsMsgPano_O2(GstrHelpCode);
            FrmOcsMsgPano_O2X.StartPosition = FormStartPosition.CenterParent;
            FrmOcsMsgPano_O2X.Show();
        }

        private void mnuMessage09_Click(object sender, EventArgs e)
        {
            GstrHelpCode = "치매약제";

            if (FrmOcsMsgPano_O2X != null)
            {
                FrmOcsMsgPano_O2X.Dispose();
                FrmOcsMsgPano_O2X = null;
            }
            FrmOcsMsgPano_O2X = new FrmOcsMsgPano_O2(GstrHelpCode);
            FrmOcsMsgPano_O2X.StartPosition = FormStartPosition.CenterParent;
            FrmOcsMsgPano_O2X.Show();
        }

        private void menuMessage04_Click(object sender, EventArgs e)
        {
            if (FrmOcsMsgPano_OX != null)
            {
                FrmOcsMsgPano_OX.Dispose();
                FrmOcsMsgPano_OX = null;
            }
            FrmOcsMsgPano_OX = new FrmOcsMsgPano_O();
            FrmOcsMsgPano_OX.StartPosition = FormStartPosition.CenterParent;
            FrmOcsMsgPano_OX.Show();
        }

        private void menuMessage10_Click(object sender, EventArgs e)
        {
            if (frmMsgSendX != null)
            {
                frmMsgSendX.Dispose();
                frmMsgSendX = null;
            }
            frmMsgSendX = new frmMsgSend();
            frmMsgSendX.StartPosition = FormStartPosition.CenterParent;
            frmMsgSendX.Show();
        }

        private void mnuHoanBul_Click(object sender, EventArgs e)
        {
            if (frmHoanBulX != null)
            {
                frmHoanBulX.Dispose();
                frmHoanBulX = null;
            }
            frmHoanBulX = new frmHoanBul();
            frmHoanBulX.StartPosition = FormStartPosition.CenterParent;
            frmHoanBulX.Show();
        }

        private void mnuSimSaInfor01_Click(object sender, EventArgs e)
        {
            GstrHelpCode = txtCode.Text;

            if (frmSimsaInforX != null)
            {
                frmSimsaInforX.Dispose();
                frmSimsaInforX = null;
            }
            frmSimsaInforX = new frmSimsaInfor(GstrHelpCode);
            frmSimsaInforX.StartPosition = FormStartPosition.CenterParent;
            frmSimsaInforX.Show();
        }

        private void mnuSimSaInfor03_Click(object sender, EventArgs e)
        {
            GstrHelpCode = txtCode.Text;

            if (frmSimsaInfor_FileX != null)
            {
                frmSimsaInfor_FileX.Dispose();
                frmSimsaInfor_FileX = null;
            }
            frmSimsaInfor_FileX = new frmSimsaInfor_File(GstrHelpCode);
            frmSimsaInfor_FileX.StartPosition = FormStartPosition.CenterParent;
            frmSimsaInfor_FileX.Show();
        }

        private void mnuSimSaInfor02_Click(object sender, EventArgs e)
        {
            GstrHelpCode = txtCode.Text;

            if (frmSimsaInfor_WardX != null)
            {
                frmSimsaInfor_WardX.Dispose();
                frmSimsaInfor_WardX = null;
            }
            frmSimsaInfor_WardX = new frmSimsaInfor_Ward(GstrHelpCode);
            frmSimsaInfor_WardX.StartPosition = FormStartPosition.CenterParent;
            frmSimsaInfor_WardX.Show();
        }

        private void mnuSimSaInfor04_Click(object sender, EventArgs e)
        {
            if (frmSimsaInfor_MIRX != null)
            {
                frmSimsaInfor_MIRX.Dispose();
                frmSimsaInfor_MIRX = null;
            }
            frmSimsaInfor_MIRX = new frmSimsaInfor_MIR();
            frmSimsaInfor_MIRX.StartPosition = FormStartPosition.CenterParent;
            frmSimsaInfor_MIRX.Show();
        }

        private void mnuSimSaInfor05_Click(object sender, EventArgs e)
        {
            frmOcsMsgPano_MirX = new frmOcsMsgPano_Mir();
            frmOcsMsgPano_MirX.StartPosition = FormStartPosition.CenterParent;
            frmOcsMsgPano_MirX.Show();
        }

        private void mnuDruginfo_Click(object sender, EventArgs e)
        {
            GstrDrugCode = txtCode.Text;

            if (USE_DIF() == true)
            {
                //frmSupDrstDifDown frmSupDrstDifDownX = new frmSupDrstDifDown("BUSUGA", GstrDrugCode);
                //frmSupDrstDifDownX.ShowDialog();

                //2019-07-12 전산업무 의뢰서 2019-822
                frmSupDrstInfoEntryNew f = new frmSupDrstInfoEntryNew(GstrDrugCode, "BUSUGA", "VIEW");
                f.ShowDialog(this);                
            }
            else
            {
                frmSupDrstInfoEntryNew frmSupDrstInfoEntryNewX = new frmSupDrstInfoEntryNew(GstrDrugCode);
                frmSupDrstInfoEntryNewX.ShowDialog();
            }
        }

        private bool USE_DIF()
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return rtVal; //권한 확인

                SQL = " SELECT NAME";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'DRUG_DIF_교체'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = 'USE'";
                SQL = SQL + ComNum.VBLF + "   AND NAME = 'Y'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtVal = true;
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

            return rtVal;
        }

        private void mnuIllHCode_Click(object sender, EventArgs e)
        {
            frmILLHCode frmILLHCodeX = new frmILLHCode();
            frmILLHCodeX.StartPosition = FormStartPosition.CenterParent;
            frmILLHCodeX.Show();
        }

        private void mnuDrugNew_Click(object sender, EventArgs e)
        {
            frmSupDrstNewCode frmSupDrstNewCodeX = new frmSupDrstNewCode();
            frmSupDrstNewCodeX.StartPosition = FormStartPosition.CenterParent;
            frmSupDrstNewCodeX.Show();
        }

        private void mnuBlood_Click(object sender, EventArgs e)
        {
            //닫는 이벤트 내용
            if (frmBloodSugaX != null)
            {
                frmBloodSugaX.Dispose();
                frmBloodSugaX = null;
            }
            frmBloodSugaX = new frmBloodSuga();
            frmBloodSugaX.StartPosition = FormStartPosition.CenterParent;
            frmBloodSugaX.Show();
        }

        private void SS1_Suga_Move()    //'현재수가를 변경수가1로 Move
        {
            int i;
            int j;
            string strData;

            //int nBAmt;
            //int nTAmt;
            //int nIAmt;

            //'변경수가1이 현재일보다 크거나 같으면 Move 안함
            if (Convert.ToDateTime(ss1_Sheet1.Cells[1, 0].Text) >= Convert.ToDateTime(txtSuDate.Text)) return;

            //'보험수가가 변경되지 않았으면 이동 안함
            nBAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[1, 0].Text));
            if (nBAmt == FnBAmt) return;

            //'변경수가1~4를 1칸씩 우측으로 이동

            for (i = 5; i >= 0; i--)
            {
                for (j = 0; j <= 6; j++)
                {
                    strData = ss1_Sheet1.Cells[j, i].Text;
                    ss1_Sheet1.Cells[j, i + 1].Text = strData;
                }
            }

            //'변경전의 수가를 변경수가1에 MOVE
            txtSuDate.Text = ss1_Sheet1.Cells[0, 0].Text;
            FnBAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[2, 1].Text));
            FnTAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[3, 1].Text));
            FnIAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[4, 1].Text));

            return;
        }

        private void SS1_Suga_Gesan()    //'보험수가를 기준으로 자보,일반수가를 계산
        {
            int i;
            int j;
            string strData;

            //'변경수가1이 현재일보다 크거나 같으면 Move 안함
            if (Convert.ToDateTime(ss1_Sheet1.Cells[1, 0].Text) < Convert.ToDateTime(txtSuDate.Text))
            {
                for (i = ss1_Sheet1.ColumnCount - 2; i >= 0; i--)
                {
                    for (j = 0; j < 5; j++)
                    {
                        strData = ss1_Sheet1.Cells[j, i].Text;
                        ss1_Sheet1.Cells[j, i + 1].Text = strData;
                    }
                }

                ss1_Sheet1.Cells[1, 0].Text = txtSuDate.Text;

                ss1_Sheet1.Cells[2, 1].Text = FnBAmt.ToString();
                ss1_Sheet1.Cells[3, 1].Text = FnTAmt.ToString();
                ss1_Sheet1.Cells[4, 1].Text = FnIAmt.ToString();
            }

            nBAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[2, 0].Text.Replace(",", "")));
            ss1_Sheet1.Cells[3, 0].Text = nBAmt.ToString();

            nIAmt = Gesan_IlbanAmt(nBAmt, txtBun.Text, txtGbE.Text, txtGbF.Text);
            ss1_Sheet1.Cells[4, 0].Text = nIAmt.ToString();

            return;
        }

        private void txtWon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtWon_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈원가의 항목분류는? (더블클릭:찾기 Help)◈";
            txtWon.SelectionStart = 0;
            txtWon.SelectionLength = VB.Len(txtWon.Text);
        }

        private void txtWon_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void txtWon_DoubleClick(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            //닫는 이벤트 내용
            if (frmWonhangHelpX != null)
            {
                frmWonhangHelpX.Dispose();
                frmWonhangHelpX = null;
            }
            frmWonhangHelpX = new frmWonhangHelp();
            frmWonhangHelpX.StartPosition = FormStartPosition.CenterParent;
            frmWonhangHelpX.rSetCodeName += frmWonhangHelpX_rSetCodeName;
            frmWonhangHelpX.rEventClosed += frmWonhangHelpX_rEventClosed;
            frmWonhangHelpX.ShowDialog();

            if (GstrHelpCode != "")
            {
                txtWon.Text = GstrHelpCode;
                lblWon.Text = READ_WonName(txtWon.Text);
                GstrHelpCode = "";
                SendKeys.Send("{Tab}");
            }
        }

        private void frmWonhangHelpX_rEventClosed()
        {
            //닫는 이벤트 내용
            if (frmWonhangHelpX != null)
            {
                frmWonhangHelpX.Dispose();
                frmWonhangHelpX = null;
            }
        }

        private void frmWonhangHelpX_rSetCodeName(string strCode)
        {
            if (strCode != "")
            {
                GstrHelpCode = strCode;
            }
        }

        private string READ_WonName(string ArgCode)
        {
            string ArgReturn = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                if (VB.Trim(ArgCode) == "")
                {
                    return ArgReturn;
                }

                SQL = "SELECT HANGNAME FROM KOSMOS_ADM.WON_HANG ";
                SQL = SQL + ComNum.VBLF + "WHERE HANG='" + VB.Trim(ArgCode) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["HangName"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "** ERROR **";
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

            return ArgReturn;
        }

        private void txtUnit_New1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtUnit_New4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtTotMax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtSuDate_DoubleClick(object sender, EventArgs e)
        {
            Calendar_Date_Select(this.txtSuDate);
        }

        private void Calendar_Date_Select(Control ArgText)
        {
            clsPublic.GstrCalDate = VB.Trim(ArgText.Text);
            if (VB.Len(VB.Trim(ArgText.Text)) != 10)
            {
                clsPublic.GstrCalDate = clsPublic.GstrSysDate;
            }

            frmCalendar2 frmCalendarX = new frmCalendar2();
            frmCalendarX.StartPosition = FormStartPosition.CenterParent;
            frmCalendarX.ShowDialog();
            frmCalendarX.Dispose();
            frmCalendarX = null;

            if (VB.Len(clsPublic.GstrCalDate) == 10)
            {
                ArgText.Text = clsPublic.GstrCalDate;
            }
        }

        private void txtSuDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtCode.ReadOnly == false)
            {
                txtCode.Focus();
            }
        }

        private void txtSuNext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtSuNameK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtSuNameE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtSuNameG_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtMemo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtNu_DoubleClick(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            //닫는 이벤트 내용
            if (PmpaMirNUHelpX != null)
            {
                PmpaMirNUHelpX.Dispose();
                PmpaMirNUHelpX = null;
            }
            PmpaMirNUHelpX = new PmpaMirNUHelp();
            PmpaMirNUHelpX.StartPosition = FormStartPosition.CenterParent;
            PmpaMirNUHelpX.rSetHelpCode += PmpaMirNUHelpX_rSetHelpCode;
            PmpaMirNUHelpX.rEventClosed += PmpaMirNUHelpX_rEventClosed;
            PmpaMirNUHelpX.ShowDialog();

            if (GstrHelpCode != "")
            {
                txtNu.Text = GstrHelpCode;
                lblNu.Text = READ_NuName(txtNu.Text);
                GstrHelpCode = "";
                SendKeys.Send("{Tab}");
            }
        }

        private void PmpaMirNUHelpX_rEventClosed()
        {
            //닫는 이벤트 내용
            if (PmpaMirNUHelpX != null)
            {
                PmpaMirNUHelpX.Dispose();
                PmpaMirNUHelpX = null;
            }
        }

        private void PmpaMirNUHelpX_rSetHelpCode(string strHelpCode)
        {
            if (strHelpCode != "")
            {
                GstrHelpCode = strHelpCode.Trim();
            }
        }

        private void txtNu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtNu_Leave(object sender, EventArgs e)
        {
            lblNu.Text = READ_NuName(txtNu.Text);
        }

        private string READ_NuName(string ArgCode)
        {
            string ArgReturn = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                SQL = "SELECT Name FROM BAS_BUN ";
                SQL = SQL + ComNum.VBLF + "WHERE Jong = '2' ";
                SQL = SQL + ComNum.VBLF + "  AND Code = '" + VB.Trim(ArgCode) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["Name"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "ERROR";
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

            return ArgReturn;
        }

        private void txtNurCode_DoubleClick(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            //닫는 이벤트 내용
            if (frmNurCodeHelpX != null)
            {
                frmNurCodeHelpX.Dispose();
                frmNurCodeHelpX = null;
            }
            frmNurCodeHelpX = new frmNurCodeHelp();
            frmNurCodeHelpX.StartPosition = FormStartPosition.CenterParent;
            frmNurCodeHelpX.rSetHelpCode += frmNurCodeHelpX_rSetHelpCode;
            frmNurCodeHelpX.rEventClosed += frmNurCodeHelpX_rEventClosed;
            frmNurCodeHelpX.ShowDialog();

            if (GstrHelpCode != "")
            {
                txtNurCode.Text = VB.Trim(VB.Left(GstrHelpCode, 4));
                lblNurCode.Text = VB.Mid(GstrHelpCode, 5, GstrHelpCode.Length);
                GstrHelpCode = "";
                SendKeys.Send("{Tab}");
            }
        }

        private void frmNurCodeHelpX_rEventClosed()
        {
            //닫는 이벤트 내용
            if (FrmSugaSerchX != null)
            {
                FrmSugaSerchX.Dispose();
                FrmSugaSerchX = null;
            }
        }

        private void frmNurCodeHelpX_rSetHelpCode(string strHelpCode)
        {
            if (strHelpCode != "")
            {
                GstrHelpCode = strHelpCode;
            }
        }

        private void txtHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtHang_Leave(object sender, EventArgs e)
        {
            lblHang.Text = READ_HangName(txtBun.Text);
        }

        private string READ_HangName(string argBun)
        {
            string ArgReturn = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                SQL = "SELECT Name FROM BAS_BUN ";
                SQL = SQL + ComNum.VBLF + "WHERE Jong = '3' ";
                SQL = SQL + ComNum.VBLF + "  AND Code = '" + VB.Format(argBun, "00") + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["Name"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "ERROR";
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

            return ArgReturn;
        }

        private void txtHCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtMok_TextChanged(object sender, EventArgs e)
        {
            lblMok.Text = READ_MokName(txtBun.Text);
        }

        private string READ_MokName(string argBun)
        {
            string ArgReturn = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                SQL = "SELECT Name FROM BAS_BUN ";
                SQL = SQL + ComNum.VBLF + "WHERE Jong = '4' ";
                SQL = SQL + ComNum.VBLF + "  AND Code = '" + VB.Format(argBun, "00") + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["Name"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "ERROR";
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

            return ArgReturn;
        }

        private void txtGbA_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "1.단순, 2.복합, 3. Routine";
            txtGbA.SelectionStart = 0;
            txtGbA.SelectionLength = VB.Len(txtGbA.Text);
        }

        private void txtGbA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbB_Enter(object sender, EventArgs e)
        {
            if (CompareNumberString(txtGbA.Text) > CompareNumberString("3"))
            {
                SendKeys.Send("{Tab}");
            }
            else
            {
                lblMsg.Text = "◈약조제료(1.내복 2.내복+제재 3.외용 4.외용+제재) ◈소아가산(A.10% B.15% C.20% D.25% E.30% F.35% G.40% H.45% I.50% , J.60%(신생아), Y.70세이상노인30%, Z.35세이상산모30%)" + ComNum.VBLF;
                lblMsg.Text = lblMsg.Text + "  ◈주사수기료(1.IM, 2.IV, 3.IM/IV, 4.100cc, 5.500cc, 6.1000cc, 7.KK045, 9.관절강내) 8.자가투여 주사제";
                txtGbB.SelectionStart = 0;
                txtGbB.SelectionLength = VB.Len(txtGbB.Text);
            }
        }

        private void txtGbB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbC_Enter(object sender, EventArgs e)
        {
            if (CompareNumberString(txtGbB.Text) >= CompareNumberString("0") && CompareNumberString(txtGbB.Text) <= CompareNumberString("Z"))
            {
                lblMsg.Text = "0. 심야가산 무, 1. 심야가산가능, 1. 5cc 증류수, 2. 10cc 증류수, 3. 20cc 증류수, 4. 5ccNSA, 5. 10ccNSA, 6. 20ccNSA, 7. NS10 8. NS10 X 2 ";
                txtGbC.SelectionStart = 0;
                txtGbC.SelectionLength = VB.Len(txtGbC.Text);
            }
            else
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbD_Enter(object sender, EventArgs e)
        {
            if (CompareNumberString(txtGbC.Text) > CompareNumberString("6") || CompareNumberString(txtGbC.Text) == CompareNumberString("4") || VB.RTrim(txtGbC.Text) == "")
            {
                SendKeys.Send("{Tab}");
            }
            else
            {
                lblMsg.Text = "검사체감종목수(1-9,A:10,B:11,C:12,D=13,E:14,F=15)";
                txtGbD.SelectionStart = 0;
                txtGbD.SelectionLength = VB.Len(txtGbD.Text);
            }
        }

        private void txtGbD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbE_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "기술가산여부 (0.재료대 1.기술료가산)";
            txtGbE.SelectionStart = 0;
            txtGbE.SelectionLength = VB.Len(txtGbE.Text);
        }

        private void txtGbE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbF_Enter(object sender, EventArgs e)
        {
            if (CompareNumberString(txtGbE.Text) > CompareNumberString("1") || VB.RTrim(txtGbE.Text) == "")
            {
                SendKeys.Send("{Tab}");
            }
            else
            {
                lblMsg.Text = "0.급여, 1.비급여, 2.비급여 (입력 수정 가능)  3.100/100 ";
                txtGbF.SelectionStart = 0;
                txtGbF.SelectionLength = VB.Len(txtGbF.Text);
            }
        }

        private void txtGbF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbG_Enter(object sender, EventArgs e)
        {
            if (CompareNumberString(txtGbF.Text) > CompareNumberString("2") || VB.RTrim(txtGbF.Text) == "")
            {
                SendKeys.Send("{Tab}");
            }
            else
            {
                lblMsg.Text = "1. 수량,날수 입력, 2. 수량 입력, 3.날수 입력, 4.시간,분 입력, 6. 금액 입력";
                txtGbG.SelectionStart = 0;
                txtGbG.SelectionLength = VB.Len(txtGbG.Text);
            }
        }

        private void txtGbG_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbH_Enter(object sender, EventArgs e)
        {
            if (CompareNumberString(txtGbG.Text) > CompareNumberString("6") || CompareNumberString(txtGbG.Text) == CompareNumberString("5") || txtGbG.Text == "0" || VB.RTrim(txtGbG.Text) == "")
            {
                SendKeys.Send("{Tab}");
            }
            else
            {
                lblMsg.Text = "[특진료구분] 0. 가산무, 1. 가산가능 50%, 2. 가산 가능 100%";
                txtGbH.SelectionStart = 0;
                txtGbH.SelectionLength = VB.Len(txtGbH.Text);
            }
        }

        private void txtGbH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbI_Enter(object sender, EventArgs e)
        {
            if (CompareNumberString(txtGbH.Text) > CompareNumberString("2") || VB.RTrim(txtGbH.Text) == "")
            {
                SendKeys.Send("{Tab}");
            }
            else
            {
                lblMsg.Text = "1.재원기간수량, 2.재원기간일수, 3.평생관리(수량), 4.평생관리(일수), 5.사용제한약품, 6.일당관리, 7.주당관리, 8.월관리";
                txtGbI.SelectionStart = 0;
                txtGbI.SelectionLength = VB.Len(txtGbI.Text);
            }
        }

        private void txtGbI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbJ_Enter(object sender, EventArgs e)
        {
            if (CompareNumberString(txtGbI.Text) > CompareNumberString("8") || VB.RTrim(txtGbI.Text) == "")
            {
                SendKeys.Send("{Tab}");
            }
            else
            {
                lblMsg.Text = "▶검사(0.일반 8.외부:영대(EDI만 병원수가10%가산), 9.외부:삼광(EDI만 의원수가10%가산)),▶약주사(1.원외처방전용 2.입원처방전용 3.원내외혼용, 4.원내만전용(입원+외래)), 5.의원단가";
                txtGbJ.SelectionStart = 0;
                txtGbJ.SelectionLength = VB.Len(txtGbJ.Text);
            }
        }

        private void txtGbJ_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbK_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "0.감액대상 코드 1.감액제외 코드 2.재료대 정상할인율적용";
            txtGbK.SelectionStart = 0;
            txtGbK.SelectionLength = VB.Len(txtGbK.Text);
        }

        private void txtGbK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbL_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "1.수가 2.준용수가 3.국산보험등제약 4.수입약 7.협약재료 8.일반재료";
            txtGbL.SelectionStart = 0;
            txtGbL.SelectionLength = VB.Len(txtGbL.Text);
        }

        private void txtGbL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbM_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "0.기타 1.퇴장방지의약품 10%가산(수가에 가산하여 입력하세요)"; ;
            txtGbM.SelectionStart = 0;
            txtGbM.SelectionLength = VB.Len(txtGbM.Text);
        }

        private void txtGbM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbN_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "0.기타 1.선수납수가";
            txtGbN.SelectionStart = 0;
            txtGbN.SelectionLength = VB.Len(txtGbN.Text);
        }

        private void txtGbN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbO_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "의약분업(0.기타 1.예방접종 2.진단의약품 3.결핵치료제 4.조제실제제 5.마약 6.방사성의약품" + ComNum.VBLF;
            lblMsg.Text = lblMsg.Text + "7.기계장치이용 8.시술필요약품 9.희귀의약품 A.차광,냉장주사제 B.항암주사 C.주사약제";
            txtGbO.SelectionStart = 0;
            txtGbO.SelectionLength = VB.Len(txtGbO.Text);
        }

        private void txtGbO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }



        private void txtGbN_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void txtGbO_Leave(object sender, EventArgs e)
        {
            txtGbO.Text = VB.UCase(txtGbO.Text);
            lblMsg.Text = "";
        }

        private void txtGbP_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "비급여분류(1.인정 2.임의 9.제외)";
            txtGbP.SelectionStart = 0;
            txtGbP.SelectionLength = VB.Len(txtGbP.Text);
        }

        private void txtGbP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbP_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void txtGbQ_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "산재 약급여처리(0.적용안함 1.급여처리)";
            txtGbP.SelectionStart = 0;
            txtGbP.SelectionLength = VB.Len(txtGbP.Text);
        }

        private void txtGbQ_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbQ_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void txtGbR_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "외래비급여항목중에서 (0.자보급여 1.자보비급여)";
            txtGbR.SelectionStart = 0;
            txtGbR.SelectionLength = VB.Len(txtGbR.Text);
        }

        private void txtGbR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        } 

        private void txtGbR_Leave(object sender, EventArgs e)
        {    
            lblMsg.Text = "";
        }

        private void txtGbS_Enter(object sender, EventArgs e) 
        {
            lblMsg.Text = "1.100/100 2.코로나임시(20%) 3.100/30(중복)  4.100/80   5.100/50   6.100/80(중복) 7.100/50(중복)  8.100/90(중복)  9.100/90 "; //20190729  3번 의뢰서건 추가
            txtGbS.SelectionStart = 0;
            txtGbS.SelectionLength = VB.Len(txtGbS.Text);
        }

        private void txtGbS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbS_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void txtGbT_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "1.DRG 비급여 ";
            txtGbT.SelectionStart = 0;
            txtGbT.SelectionLength = VB.Len(txtGbT.Text);
        }

        private void txtGbT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbT_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void txtGbU_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "0 기본, 1.고가 ,2.저가(2017/08부터사용)";
            txtGbU.SelectionStart = 0;
            txtGbU.SelectionLength = VB.Len(txtGbU.Text);
        }

        private void txtGbU_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbU_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void txtGbV_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈약제, 재료 수불구분(0.행위, 1.약제, 2.재료, 3.약제(제제약), 4.방사선조영제, 5.마취(분), 6.공급실(가공), 9.기타)";
            txtGbV.SelectionStart = 0;
            txtGbV.SelectionLength = VB.Len(txtGbV.Text);
        }

        private void txtGbV_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbV_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void txtGbW_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈치매 미수 구분(0.일반 1.진단, 2.감별)";
            txtGbW.SelectionStart = 0;
            txtGbW.SelectionLength = VB.Len(txtGbW.Text);
        }

        private void txtGbW_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbW_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void txtGbX_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈산재/자보 기술가산여부( 0.재료대, 1.기술료가산)";
            txtGbX.SelectionStart = 0;
            txtGbX.SelectionLength = VB.Len(txtGbX.Text);
        }

        private void txtGbX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbX_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void txtGbY_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈외과가산여부( 0.간산무, 1. 20%, 2. 30% )";
            txtGbY.SelectionStart = 0;
            txtGbY.SelectionLength = VB.Len(txtGbY.Text);
        }

        private void txtGbY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbY_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void txtGbZ_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈흉부외과가산여부( 0.가산무,   1. 100%,   2. 70%,   3. 30%,   4. 20% )";
            txtGbZ.SelectionStart = 0;
            txtGbZ.SelectionLength = VB.Len(txtGbZ.Text);
        }

        private void txtGbZ_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbZ_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void txtDelDate_DoubleClick(object sender, EventArgs e)
        {
            Calendar_Date_Select(this.txtDelDate);
        }

        private void txtDelDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtDayMax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtDaiCode_DoubleClick(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            //닫는 이벤트 내용
            if (frmYAKHelpX != null)
            {
                frmYAKHelpX.Dispose();
                frmYAKHelpX = null;
            }
            frmYAKHelpX = new frmYAKHelp();
            frmYAKHelpX.StartPosition = FormStartPosition.CenterParent;
            frmYAKHelpX.rSetHelpCode += frmYAKHelpX_rSetHelpCode;
            frmYAKHelpX.rEventClosed += frmYAKHelpX_rEventClosed;
            frmYAKHelpX.ShowDialog();

            if (GstrHelpCode != "")
            {
                txtDaiCode.Text = GstrHelpCode;
                lblDaiCode.Text = READ_DaicodeName(txtDaiCode.Text);
                GstrHelpCode = "";
                SendKeys.Send("{Tab}");
            }
        }

        private void frmYAKHelpX_rEventClosed()
        {
            //닫는 이벤트 내용
            if (frmYAKHelpX != null)
            {
                frmYAKHelpX.Dispose();
                frmYAKHelpX = null;
            }
        }

        private void frmYAKHelpX_rSetHelpCode(string strHelpCode)
        {
            if (strHelpCode != "")
            {
                GstrHelpCode = strHelpCode.Trim();
            }
        }

        private void txtDaiCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtDaiCode_Leave(object sender, EventArgs e)
        {
            lblDaiCode.Text = READ_DaicodeName(txtDaiCode.Text);
        }

        private string READ_DaicodeName(string ArgCode)
        {
            string ArgReturn = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                if (VB.Trim(ArgCode) == "" || VB.IsNumeric(ArgCode) == false)
                {
                    return ArgReturn;
                }

                SQL = "SELECT ClassName FROM BAS_CLASS ";
                SQL = SQL + ComNum.VBLF + "WHERE ClassCode= '" + ArgCode.Trim() + "' ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["ClassName"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "** ERROR **";
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

            return ArgReturn;
        }

        private void ss1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            int i;
            int j;
            string strData;

            if (e.Column == 0 && e.Row == 1)
            {
                for (i = ss1_Sheet1.RowCount; i >= 0; i--)
                {

                    if (i == 0) i = i;

                    for (j = 0; j < ss1_Sheet1.RowCount; j++)
                    {
                        strData = ss1_Sheet1.Cells[j, i].Text;
                        ss1_Sheet1.Cells[j, i + 1].Text = strData;
                    }
                }

                ss1_Sheet1.Cells[1, 0].Text = txtSuDate.Text;
                ss1_Sheet1.Cells[7, 0].Text = "";   //'rowid
            }


            if (e.Column == 0 && e.Row == 5)
            {
                GstrHelpCode = ss3_Sheet1.Cells[2, 0].Text;

                //닫는 이벤트 내용
                if (frmYGuipViewX != null)
                {
                    frmYGuipViewX.Dispose();
                    frmYGuipViewX = null;
                }
                frmYGuipViewX = new frmYGuipView(GstrHelpCode, clsType.User.Sabun);
                frmYGuipViewX.StartPosition = FormStartPosition.CenterParent;
                //frmSearchUnpaidX.TopMost = true;
                frmYGuipViewX.ShowDialog();
            }
        }

        private void ss2_Change(object sender, ChangeEventArgs e)
        {
            string strData;
            string strMsg;
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strBCode;
            string strGesu;
            string strJong;

            if (chkJob2.Checked == true) //'그룹코드 중복 점검
            {
                try
                {
                    if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                    strData = ss2_Sheet1.Cells[e.Row, e.Column].Text.Trim().ToUpper();

                    SQL = " SELECT A.SUCODE, B.SUNAMEK , C.SUNAMEK NAME ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_SUH A ,KOSMOS_PMPA.BAS_SUN B , KOSMOS_PMPA.BAS_SUN C ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.SUNEXT ='" + strData + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.SUCODE = B.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = C.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.SUCODE, B.SUNAMEK, C.SUNAMEK ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    strMsg = "";

                    if (dt.Rows.Count > 0)
                    {
                        strMsg = " << 중복코드 알림 : " + strData + "  : " + dt.Rows[0]["NAME"].ToString().Trim() + " >>-------------------- " + ComNum.VBLF + ComNum.VBLF;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strMsg = strMsg + VB.Left(dt.Rows[i]["SUCODE"].ToString().Trim() + VB.Space(10), 10) + " : " + dt.Rows[i]["SUNAMEK"].ToString().Trim() + ComNum.VBLF;
                        }
                        strMsg = strMsg + ComNum.VBLF + " ------------------------------------------------------------------------------- " + ComNum.VBLF;

                        ComFunc.MsgBox(strMsg, "확인");
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
            }

            ss2_Sheet1.Cells[e.Row, 75 + 3].Text = "Y";
            ss2_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(182, 255, 255);

            strBCode = ss2_Sheet1.Cells[e.Row, 95 + 3].Text;
            strGesu = ss2_Sheet1.Cells[e.Row, 96 + 3].Text;
            strJong = ss2_Sheet1.Cells[e.Row, 97 + 3].Text;

            if(string.IsNullOrEmpty(ss2_Sheet1.Cells[e.Row, 14].Text))
            {
                ss2_Sheet1.Cells[e.Row, 14].Value = 0;
            }

            if (e.Column == 2)
            {
                SS2_SuNext_Change(e.Row, e.Column); 
                return;
            }

            if ((e.Column == 14 || e.Column == 15 || e.Column == 16) && chkJob1.Checked == true)
            {
                SS2_Suga_Move(e.Row, e.Column); 
            }

            if (e.Column == 14 && chkJob1.Checked == true)
            {
                SS2_Suga_Gesan(e.Row, e.Column);
            }

            if (e.Column == 8)
            {
                SS2_BCode_Change(e.Row, e.Column, strBCode, strGesu, strJong);
            }

            if ((e.Column == 53 + 3) && e.Column == 82 + 3 && e.Column == 86 + 3 && e.Column == 90 + 3)
            {
                SS2_OldBCode_Change(e.Row, e.Column);
            }

            return;
        }

        private void SS2_SuNext_Change(int iRow, int iCol)    //'품명코드 변경시
        {
            string strData;
            string strSSData = "";

            int j = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            strData = VB.Trim(ss2_Sheet1.Cells[iRow, 2].Text).ToUpper();
            ss2_Sheet1.Cells[iRow, 2].Text = strData;

            if (strData == "") return;

            strBun = ss2_Sheet1.Cells[iRow, 3].Text;
            strNu = ss2_Sheet1.Cells[iRow, 4].Text;

            nBAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 14].Text));


            //'공란줄에 기본자료를 Setting

            if (strBun == "" || strNu == "" || nBAmt == 0)
            {
                ss2_Sheet1.Cells[iRow, 11].Text = "0";
                ss2_Sheet1.Cells[iRow, 12].Text = "0";
                ss2_Sheet1.Cells[iRow, 13].Text = "1";


                try
                {
                    if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                    //'BAS_SUT에 자료가 있는지 Check
                    SQL = "SELECT BUN,NU,SUGBA,SUGBB,SUGBC,SUGBD,SUGBE,SUGBF,SUGBG,SUGBH,";
                    SQL = SQL + ComNum.VBLF + " SUGBI,SUGBJ,SUGBK,SUGBL,BAMT,TAMT,IAMT,OLDBAMT,OLDTAMT,OLDIAMT,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE3,'YYYY-MM-DD') SUDATE3,BAMT3,TAMT3,IAMT3,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE4,'YYYY-MM-DD') SUDATE4,BAMT4,TAMT4,IAMT4,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE5,'YYYY-MM-DD') SUDATE5,BAMT5,TAMT5,IAMT5 ";
                    SQL = SQL + ComNum.VBLF + " FROM BAS_SUT ";
                    SQL = SQL + ComNum.VBLF + "WHERE SUNEXT = '" + strData + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(VB.Val(dt.Rows[0]["SugbA"].ToString().Trim())) > Convert.ToInt32(VB.Val("1")))
                        {
                            ComFunc.MsgBox("묶음코드하위 하위코드로  묶코드를 사용할수 없습니다", "확인");
                            ss2_Sheet1.Cells[iRow, 0].Text = "";
                            return;
                        }

                        ss2_Sheet1.Cells[iRow, 3].Text = dt.Rows[0]["Bun"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 4].Text = dt.Rows[0]["Nu"].ToString().Trim();

                        ss2_Sheet1.Cells[iRow, 14].Text = dt.Rows[0]["BAmt"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 15].Text = dt.Rows[0]["TAmt"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 16].Text = dt.Rows[0]["IAmt"].ToString().Trim();

                        ss2_Sheet1.Cells[iRow, 19].Text = "3";
                        ss2_Sheet1.Cells[iRow, 20].Text = dt.Rows[0]["SugbB"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 21].Text = dt.Rows[0]["SugbC"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 22].Text = dt.Rows[0]["SugbD"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 23].Text = dt.Rows[0]["SugbE"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 24].Text = dt.Rows[0]["SugbF"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 25].Text = dt.Rows[0]["SugbG"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 26].Text = dt.Rows[0]["SugbH"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 27].Text = dt.Rows[0]["SugbI"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 28].Text = dt.Rows[0]["SugbJ"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 29].Text = dt.Rows[0]["SugbK"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 30].Text = dt.Rows[0]["SugbL"].ToString().Trim();

                        ss2_Sheet1.Cells[iRow, 49 + 3].Text = dt.Rows[0]["SuDate"].ToString().Trim();

                        ss2_Sheet1.Cells[iRow, 50 + 3].Text = dt.Rows[0]["OldBAmt"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 51 + 3].Text = dt.Rows[0]["OldTAmt"].ToString().Trim();
                        ss2_Sheet1.Cells[iRow, 52 + 3].Text = dt.Rows[0]["OldIAmt"].ToString().Trim();
                    }
                    else
                    {
                        if (iRow > 0)
                        {
                            strSSData = VB.Trim(ss2_Sheet1.Cells[iRow - 1, 3].Text);    //'분류
                            ss2_Sheet1.Cells[iRow, 3].Text = strSSData;
                            strSSData = VB.Trim(ss2_Sheet1.Cells[iRow - 1, 4].Text);    //'누적
                            ss2_Sheet1.Cells[iRow, 4].Text = strSSData;

                            for (j = 20; j < 41; j++)
                            {
                                strSSData = VB.Trim(ss2_Sheet1.Cells[iRow - 1, j].Text);
                                ss2_Sheet1.Cells[iRow, j].Text = strSSData;
                            }
                        }
                        else
                        {
                            ss2_Sheet1.Cells[iRow, 19].Text = txtGbA.Text;
                            for (j = 20; j < 48; j++)
                            {
                                ss2_Sheet1.Cells[iRow, j].Text = "0";
                            }
                        }
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
            }



            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";
                SQL = SQL + ComNum.VBLF + "";
                //'BAS_SUN의 내역을 Display
                SQL = "SELECT SUNAMEK,SUNAMEE,SUNAMEG,UNIT,HCODE,DAICODE,BCODE,SUHAM,EDIJONG, ";
                SQL = SQL + " TO_CHAR(EDIDATE,'YYYY-MM-DD') EDIDATE,OLDBCODE,OLDGESU,OLDJONG,SUGBM,";
                SQL = SQL + " SugbN,SugbO, SugbP, SugbQ, SugbR, SugbS, SugbT, SugbU, SugbV, NURCODE,";
                SQL = SQL + " WONCODE , SugbX, SugbY, SugbZ, SugbAA,SugbAB,SugbAC, SugbAD, SugbAE, SugbAF, SugbAG, SugbW ";
                SQL = SQL + " FROM BAS_SUN WHERE SUNEXT='" + strData + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss2_Sheet1.Cells[iRow, 7].Text = dt.Rows[0]["NURCODE"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 8].Text = dt.Rows[0]["BCode"].ToString().Trim();    //'표준코드
                    ss2_Sheet1.Cells[iRow, 9].Text = VB.Format(VB.Val(dt.Rows[0]["SuHam"].ToString().Trim()), "###0.0000");    //'환산계수
                    ss2_Sheet1.Cells[iRow, 10].Text = dt.Rows[0]["EdiJong"].ToString().Trim();  //'표준코드 종류

                    ss2_Sheet1.Cells[iRow, 17].Text = dt.Rows[0]["SuNameK"].ToString().Trim();    //'한글명칭
                    ss2_Sheet1.Cells[iRow, 18].Text = dt.Rows[0]["HCode"].ToString().Trim();    //'한글수가
                    ss2_Sheet1.Cells[iRow, 31].Text = dt.Rows[0]["SugbM"].ToString().Trim();    //'퇴장방지의약품
                    ss2_Sheet1.Cells[iRow, 32].Text = dt.Rows[0]["SugbN"].ToString().Trim();    //'선수납물품
                    ss2_Sheet1.Cells[iRow, 33].Text = dt.Rows[0]["SugbO"].ToString().Trim();    //'원내조제

                    ss2_Sheet1.Cells[iRow, 34].Text = dt.Rows[0]["SugbP"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 35].Text = dt.Rows[0]["SugbQ"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 36].Text = dt.Rows[0]["SugbR"].ToString().Trim();

                    ss2_Sheet1.Cells[iRow, 37].Text = dt.Rows[0]["SugbS"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 38].Text = dt.Rows[0]["SugbT"].ToString().Trim();

                    ss2_Sheet1.Cells[iRow, 39].Text = dt.Rows[0]["SugbU"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 40].Text = dt.Rows[0]["SugbV"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 41].Text = dt.Rows[0]["SugbW"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 42].Text = dt.Rows[0]["SugbX"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 43].Text = dt.Rows[0]["SugbY"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 44].Text = dt.Rows[0]["SugbZ"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 45].Text = dt.Rows[0]["SugbAA"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 46].Text = dt.Rows[0]["SugbAB"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 47].Text = dt.Rows[0]["SugbAC"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 48].Text = dt.Rows[0]["SugbAD"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 49].Text = dt.Rows[0]["SugbAE"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 50].Text = dt.Rows[0]["SugbAF"].ToString().Trim();
                    ss2_Sheet1.Cells[iRow, 51].Text = dt.Rows[0]["SugbAG"].ToString().Trim();

                    ss2_Sheet1.Cells[iRow, 53 + 3].Text = dt.Rows[0]["EdiDate"].ToString().Trim();  //'적용일자
                    ss2_Sheet1.Cells[iRow, 54 + 3].Text = dt.Rows[0]["OldBCode"].ToString().Trim();    //'표준코드
                    ss2_Sheet1.Cells[iRow, 55 + 3].Text = VB.Val(dt.Rows[0]["OldGesu"].ToString().Trim()).ToString("###0.0000");    //'환산계수
                    ss2_Sheet1.Cells[iRow, 56 + 3].Text = dt.Rows[0]["OldJong"].ToString().Trim();  //'표준코드 종류

                    ss2_Sheet1.Cells[iRow, 57 + 3].Text = dt.Rows[0]["SuNameE"].ToString().Trim();    //'영문명칭
                    ss2_Sheet1.Cells[iRow, 58 + 3].Text = dt.Rows[0]["SuNameG"].ToString().Trim();    //'약품성분명
                    ss2_Sheet1.Cells[iRow, 59 + 3].Text = dt.Rows[0]["Unit"].ToString().Trim();    //'단위
                    ss2_Sheet1.Cells[iRow, 60 + 3].Text = dt.Rows[0]["DaiCode"].ToString().Trim();    //'약품대분류
                    ss2_Sheet1.Cells[iRow, 61 + 3].Text = dt.Rows[0]["WONCode"].ToString().Trim();    //'원가분류
                }
                else
                {
                    ss2_Sheet1.Cells[iRow, 31].Text = "0";   //'퇴장방지의약품
                    ss2_Sheet1.Cells[iRow, 32].Text = "0";   //'선수납물품
                    ss2_Sheet1.Cells[iRow, 33].Text = "0";   //'원내조제
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
        }


        private void SS2_Suga_Move(int iRow, int iCol)  //'현재수가를 변경수가1로 Move
        {
            string strSuDate;
            int nBAmt;
            int nTAmt;
            int nIAmt;

            //'변경수가1이 현재일보다 크거나 같으면 Move 안함    
            strSuDate = ss2_Sheet1.Cells[iRow, 49 + 3].Text;

            if (strSuDate != "")
            {
                if (DateTime.Compare(Convert.ToDateTime(strSuDate), Convert.ToDateTime(txtSuDate.Text)) >= 0) return;
            }

            //'보험수가가 변경되지 않았으면 이동 안함
            nBAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 14].Text));
            nOldBAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 76 + 3].Text));    //' 2003-10-08
            if (nBAmt == nOldBAmt) return;

            //'수가4를 수가5로 Move            
            strSuDate = ss2_Sheet1.Cells[iRow, 67 + 3].Text;
            nBAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 68 + 3].Text));
            nTAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 69 + 3].Text));
            nIAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 70 + 3].Text));

            if (strSuDate != "" || nBAmt != 0 || nTAmt != 0 || nIAmt != 0)
            {
                ss2_Sheet1.Cells[iRow, 71 + 3].Text = strSuDate;
                ss2_Sheet1.Cells[iRow, 72 + 3].Text = nBAmt.ToString();
                ss2_Sheet1.Cells[iRow, 73 + 3].Text = nTAmt.ToString();
                ss2_Sheet1.Cells[iRow, 74 + 3].Text = nIAmt.ToString();
            }

            //'수가3를 수가4로 Move
            strSuDate = ss2_Sheet1.Cells[iRow, 63 + 3].Text;
            nBAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 64 + 3] .Text));
            nTAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 65 + 3].Text));
            nIAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 66 + 3].Text));

            if (strSuDate != "" || nBAmt != 0 || nTAmt != 0 || nIAmt != 0)
            {
                ss2_Sheet1.Cells[iRow, 67 + 3].Text = strSuDate;
                ss2_Sheet1.Cells[iRow, 68 + 3].Text = nBAmt.ToString();
                ss2_Sheet1.Cells[iRow, 69 + 3].Text = nTAmt.ToString();
                ss2_Sheet1.Cells[iRow, 70 + 3].Text = nIAmt.ToString();
            }

            //'수가2를 수가3로 Move            
            strSuDate = ss2_Sheet1.Cells[iRow, 49 + 3].Text;
            nBAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 50 + 3].Text));
            nTAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 51 + 3].Text));
            nIAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 52 + 3].Text));

            if (strSuDate != "" || nBAmt != 0 || nTAmt != 0 || nIAmt != 0)
            {
                ss2_Sheet1.Cells[iRow, 63 + 3].Text = strSuDate;
                ss2_Sheet1.Cells[iRow, 64 + 3].Text = nBAmt.ToString();
                ss2_Sheet1.Cells[iRow, 65 + 3].Text = nTAmt.ToString();
                ss2_Sheet1.Cells[iRow, 66 + 3].Text = nIAmt.ToString();
            }

            //'최초의 금액을 수가2로 Move         
            strSuDate = ss2_Sheet1.Cells[iRow, 49 + 3].Text;
            nBAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 76 + 3].Text));
            nTAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 77 + 3].Text));
            nIAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 78 + 3].Text));

            if (strSuDate != "" || nBAmt != 0 || nTAmt != 0 || nIAmt != 0)
            {
                ss2_Sheet1.Cells[iRow, 49 + 3].Text = txtSuDate.Text;
                ss2_Sheet1.Cells[iRow, 50 + 3].Text = nBAmt.ToString();
                ss2_Sheet1.Cells[iRow, 51 + 3].Text = nTAmt.ToString();
                ss2_Sheet1.Cells[iRow, 52 + 3].Text = nIAmt.ToString();
            }
        }

        private void SS2_Suga_Gesan(int iRow, int iCol)
        {
            string strGbE;
            string strGbF;

            strBun = ss2_Sheet1.Cells[iRow, 3].Text;
            nBAmt = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 14].Text));
            strGbE = ss2_Sheet1.Cells[iRow, 23].Text;
            strGbF = ss2_Sheet1.Cells[iRow, 24].Text;

            nIAmt = Gesan_IlbanAmt(nBAmt, strBun, strGbE, strGbF);  //'일반수가 계산

            ss2_Sheet1.Cells[iRow, 15].Text = nBAmt.ToString();
            ss2_Sheet1.Cells[iRow, 16].Text = nIAmt.ToString();
        }


        private int Gesan_IlbanAmt(int ArgBAmt, string argBun, string ArgSugbE, string ArgSugbF, string ArgSugbJ = "")
        {
            int nIAmt = 0;

            //'진찰료,입원료는 보험가,일반가 동일하게 처리함
            if (Convert.ToInt32(VB.Val(argBun)) <= Convert.ToInt32(VB.Val("10")))
            {
                return ArgBAmt;
            }


            //'비급여수가(식대(74)-종합건진(84)는 보험가,일반가 동일하게 처리함
            if (Convert.ToInt32(VB.Val(argBun)) >= Convert.ToInt32(VB.Val("74")))
            {
                return ArgBAmt;
            }


            //'내복약,외용약품의 일반가 계산
            if ((argBun == "11" || argBun == "12") && ArgSugbE == "0")
            {
                nIAmt = Gesan_IlbanAmt_YAK(ArgBAmt, ArgSugbF);
            }
            //'주사약 일반가 계산
            else if (argBun == "20" && ArgSugbE == "0")
            {
                nIAmt = Gesan_IlbanAmt_JUSA(ArgBAmt, ArgSugbF);
            }
            //'기타 일반수가를 계산
            else
            {
                nIAmt = Gesan_IlbanAmt_ETC(ArgBAmt, ArgSugbE, ArgSugbJ);
            }

            return nIAmt;
        }

        private int Gesan_IlbanAmt_YAK(int ArgBAmt, string ArgSugbF)   //'내복약,외용약품의 일반가 계산
        {
            int nIAmt = 0;

            //'비급여수가 100,000원이상은 보험가,일반가 동일하게 처리함
            if (ArgSugbF == "1" && ArgBAmt >= 100000)
            {
                nIAmt = ArgBAmt;
                return nIAmt;
            }

            if (ArgBAmt < 11)
            {
                nIAmt = ArgBAmt * 5;
            }
            else if (ArgBAmt < 51)
            {
                nIAmt = ArgBAmt * 4;
            }
            else if (ArgBAmt < 101)
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 3.5);
            }
            else if (ArgBAmt < 500)
            {
                nIAmt = ArgBAmt * 3;
            }
            else if (ArgBAmt < 1001)
            {
                nIAmt = ArgBAmt * 2;
            }
            else
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 1.5);
            }
            return nIAmt;
        }



        private int Gesan_IlbanAmt_JUSA(int ArgBAmt, string ArgSugbF)   //'주사약제 일반수가 계산
        {
            int nIAmt = 0;

            //'비급여수가 100,000원이상은 보험가,일반가 동일하게 처리함
            if (ArgSugbF == "1" && ArgBAmt >= 100000)
            {
                nIAmt = ArgBAmt;
                return nIAmt;
            }

            if (ArgBAmt < 501)
            {
                nIAmt = ArgBAmt * 5;
            }
            else if (ArgBAmt < 1001)
            {
                nIAmt = ArgBAmt * 4;
            }
            else if (ArgBAmt < 3001)
            {
                nIAmt = ArgBAmt * 3;
            }
            else if (ArgBAmt < 5001)
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 2.5);
            }
            else if (ArgBAmt < 10001)
            {
                nIAmt = ArgBAmt * 2;
            }
            else
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 1.5);
            }
            return nIAmt;
        }



        private int Gesan_IlbanAmt_ETC(int ArgBAmt, string ArgSugbE, string ArgSugbJ)    //'기타수가 일반가 계산
        {
            int nIAmt = 0;

            //'행위료이면 보험수가 * 보험병원가산율 * 2
            if (ArgSugbE == "1") nIAmt = Convert.ToInt32((ArgBAmt * 1.25) * 2);
            //'재료대이면 보험수가의 2배
            if (ArgSugbE != "1") nIAmt = ArgBAmt * 2;
            //'10원보다 크면 10원미만 절사
            //'외부의뢰검사 는 절사 않함
            if (ArgSugbJ != "9" && ArgSugbJ != "8")
            {
                if (nIAmt > 10)
                {
                    nIAmt = (int)Math.Round(Convert.ToDouble(nIAmt) / 10, 0, MidpointRounding.ToEven);
                    nIAmt = (int)Math.Round(Convert.ToDouble(nIAmt) * 10, 0, MidpointRounding.ToEven);
                }
            }
            return nIAmt;
        }

        private void SS2_BCode_Change(int iRow, int iCol, string strBCode, string strGesu, string strJong) //'보험코드 변경시
        {
            string strData = "";

            strData = ss2_Sheet1.Cells[iRow, iCol].Text.ToUpper().Trim();
            if (strData == "")
            {
                ss2_Sheet1.Cells[iRow, 9].Text = "";
                ss2_Sheet1.Cells[iRow, 10].Text = "";
                return;
            }

            READ_EDI_SUGA(strData);
            if (TES.ROWID.Trim() != "")
            {
                ss2_Sheet1.Cells[iRow, 8].Text = strData;
                if (Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 9].Text)) == 0)
                {
                    ss2_Sheet1.Cells[iRow, 9].Text = "1.0000";
                }

                ss2_Sheet1.Cells[iRow, 10].Text = TES.Jong;   //'표준코드 종류
                ss2_Sheet1.Cells[iRow, 30].Text = TES.Jong;   //'L항

                lblMsg.Text = TES.Pname;
            }
            else if (strData == "000000" || strData == "999999" || strData == "AAAAAA" || strData == "JJJJJJ")
            {
                ss2_Sheet1.Cells[iRow, 8].Text = strData;
                ss2_Sheet1.Cells[iRow, 9].Text = "1.0000";
                ss2_Sheet1.Cells[iRow, 10].Text = "1";


                switch (strData)
                {
                    case "000000":
                        lblMsg.Text = "표준코드 산정제외 코드";
                        break;
                    case "999999":
                        lblMsg.Text = "표준코드 추후등록 예정";
                        break;
                    case "AAAAAA":
                        lblMsg.Text = "산재특정 진료수가코드입니다.";
                        break;
                    case "JJJJJJ":
                        lblMsg.Text = "준용수가 코드입니다.";
                        ss2_Sheet1.Cells[iRow, 10].Text = "2";
                        ss2_Sheet1.Cells[iRow, 30].Text = "2";
                        break;
                }
            }
            else
            {
                clsPublic.GstrMsgList = strData + "표준코드에 등록 않되었습니다." + ComNum.VBLF;
                clsPublic.GstrMsgList = clsPublic.GstrMsgList + "정말로 변경을 하시겠습니까?";
                if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    ss2_Sheet1.Cells[iRow, 8].Text = strData;
                    if (Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 9].Text)) == 0)
                    {
                        ss2_Sheet1.Cells[iRow, 9].Text = "1.0000";
                    }
                    ss2_Sheet1.Cells[iRow, 10].Text = "";   //'표준코드의 종류                    
                }
                else
                {
                    ss2_Sheet1.Cells[iRow, 8].Text = "";
                    ss2_Sheet1.Cells[iRow, 10].Text = "";   //'표준코드 종류                    
                    lblMsg.Text = strData + " 표준코드 등록 않됨";
                }
            }


            string strEdiDate;
            string strEdiBcode;
            string strEdiGesu;
            string strEdiJong;


            //'한칸씩 이동    
            strEdiDate = ss2_Sheet1.Cells[iRow, 86 + 3].Text;
            strEdiBcode = ss2_Sheet1.Cells[iRow, 87 + 3].Text;
            strEdiGesu = ss2_Sheet1.Cells[iRow, 88 + 3].Text;
            strEdiJong = ss2_Sheet1.Cells[iRow, 89 + 3].Text;

            ss2_Sheet1.Cells[iRow, 90 + 3].Text = strEdiDate;
            ss2_Sheet1.Cells[iRow, 91 + 3].Text = strEdiBcode;
            ss2_Sheet1.Cells[iRow, 92 + 3].Text = strEdiGesu;
            ss2_Sheet1.Cells[iRow, 93 + 3].Text = strEdiJong;


            strEdiDate = ss2_Sheet1.Cells[iRow, 82 + 3].Text;
            strEdiBcode = ss2_Sheet1.Cells[iRow, 83 + 3].Text;
            strEdiGesu = ss2_Sheet1.Cells[iRow, 84 + 3].Text;
            strEdiJong = ss2_Sheet1.Cells[iRow, 85 + 3].Text;

            ss2_Sheet1.Cells[iRow, 86 + 3].Text = strEdiDate;
            ss2_Sheet1.Cells[iRow, 87 + 3].Text = strEdiBcode;
            ss2_Sheet1.Cells[iRow, 88 + 3].Text = strEdiGesu;
            ss2_Sheet1.Cells[iRow, 89 + 3].Text = strEdiJong;

            strEdiDate = ss2_Sheet1.Cells[iRow, 53 + 3].Text;
            strEdiBcode = ss2_Sheet1.Cells[iRow, 54 + 3].Text;
            strEdiGesu = ss2_Sheet1.Cells[iRow, 55 + 3].Text;
            strEdiJong = ss2_Sheet1.Cells[iRow, 56 + 3].Text;

            ss2_Sheet1.Cells[iRow, 82 + 3].Text = strEdiDate;
            ss2_Sheet1.Cells[iRow, 83 + 3].Text = strEdiBcode;
            ss2_Sheet1.Cells[iRow, 84 + 3].Text = strEdiGesu;
            ss2_Sheet1.Cells[iRow, 85 + 3].Text = strEdiJong;

            ss2_Sheet1.Cells[iRow, 53 + 3].Text = txtSuDate.Text;
            ss2_Sheet1.Cells[iRow, 54 + 3].Text = strBCode;
            ss2_Sheet1.Cells[iRow, 55 + 3].Text = strGesu;
            ss2_Sheet1.Cells[iRow, 56 + 3].Text = strJong;
        }


        private void SS2_OldBCode_Change(int iRow, int iCol) //'Old보험코드 변경시
        {
            string strData = "";

            strData = VB.Trim(VB.UCase(ss2_Sheet1.Cells[iRow, iCol].Text));
            if (strData == "")
            {
                ss2_Sheet1.Cells[iRow, iCol + 1].Text = "";
                ss2_Sheet1.Cells[iRow, iCol + 2].Text = "";
                return;
            }

            READ_EDI_SUGA(strData);
            if (VB.Trim(TES.ROWID) != "")
            {
                ss2_Sheet1.Cells[iRow, iCol].Text = strData;
                if (Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, iCol + 1].Text)) == 0)
                {
                    ss2_Sheet1.Cells[iRow, iCol + 1].Text = "1.0000";
                }

                ss2_Sheet1.Cells[iRow, iCol + 2].Text = TES.Jong;   //'표준코드 종류

                lblMsg.Text = TES.Pname;
            }
            else if (strData == "000000" || strData == "999999" || strData == "AAAAAA" || strData == "JJJJJJ")
            {
                ss2_Sheet1.Cells[iRow, iCol].Text = strData;
                ss2_Sheet1.Cells[iRow, iCol + 1].Text = "1.0000";
                ss2_Sheet1.Cells[iRow, iCol + 2].Text = "1";


                switch (strData)
                {
                    case "000000":
                        lblMsg.Text = "표준코드 산정제외 코드";
                        break;
                    case "999999":
                        lblMsg.Text = "표준코드 추후등록 예정";
                        break;
                    case "AAAAAA":
                        lblMsg.Text = "산재특정 진료수가코드입니다.";
                        break;
                    case "JJJJJJ":
                        lblMsg.Text = "준용수가 코드입니다.";
                        ss2_Sheet1.Cells[iRow, iCol + 2].Text = "2";
                        break;
                }
            }
            else
            {
                clsPublic.GstrMsgList = strData + "표준코드에 등록 않되었습니다." + ComNum.VBLF;
                clsPublic.GstrMsgList = clsPublic.GstrMsgList + "정말로 변경을 하시겠습니까?";
                if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    ss2_Sheet1.Cells[iRow, iCol + 1].Text = strData;
                    if (Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[iRow, 9].Text)) == 0)
                    {
                        ss2_Sheet1.Cells[iRow, iCol + 1].Text = "1.0000";
                    }
                    ss2_Sheet1.Cells[iRow, iCol + 2].Text = "";   //'표준코드의 종류                    
                }
                else
                {
                    ss2_Sheet1.Cells[iRow, iCol].Text = "";
                    ss2_Sheet1.Cells[iRow, iCol + 2].Text = "";   //'표준코드 종류                    
                    lblMsg.Text = strData + " 표준코드 등록 않됨";
                }
            }
        }


        private void READ_EDI_SUGA(string ArgCode, string argSUNEXT = "", string ArgJong = "")  //'EDI 표준수가를 READ
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";
                SQL = SQL + ComNum.VBLF + "";

                SQL = "      SELECT ROWID VROWID,CODE VCODE,JONG VJONG,";
                SQL = SQL + "    PNAME VPNAME,BUN VBUN,DANWI1 VDANWI1,";
                SQL = SQL + "    DANWI2 VDANWI2,SPEC VSPEC,COMPNY VCOMPNY,";
                SQL = SQL + "    EFFECT VEFFECT,GUBUN VGUBUN,DANGN VDANGN,";
                SQL = SQL + "    TO_CHAR(JDATE1,'YYYY-MM-DD') VJDATE1,PRICE1 VPRICE1,";
                SQL = SQL + "    TO_CHAR(JDATE2,'YYYY-MM-DD') VJDATE2,PRICE2 VPRICE2,";
                SQL = SQL + "    TO_CHAR(JDATE3,'YYYY-MM-DD') VJDATE3,PRICE3 VPRICE3,";
                SQL = SQL + "    TO_CHAR(JDATE4,'YYYY-MM-DD') VJDATE4,PRICE4 VPRICE4,";
                SQL = SQL + "    TO_CHAR(JDATE5,'YYYY-MM-DD') VJDATE5,PRICE5 VPRICE5 ";
                SQL = SQL + " FROM KOSMOS_PMPA.EDI_SUGA ";
                SQL = SQL + "WHERE CODE = '" + VB.Trim(ArgCode) + "' ";
                //'표준코드 30050010이 산소,실구입재료 2개가 존재함

                if (ArgJong != "")
                {
                    if (ArgJong == "8")
                    {
                        SQL = SQL + " AND JONG='8' ";
                    }
                    else
                    {
                        SQL = SQL + " AND JONG<>'8' ";
                    }
                }
                else
                {
                    switch (ArgCode)
                    {
                        case "N0041001":
                        case "N0041002":
                        case "N0041003":
                        case "N0021001":
                        case "30050010":
                        case "J5010001":
                        case "C2302005":
                        case "N0051010":
                            if (argSUNEXT == VB.Trim(ArgCode))
                            {
                                SQL = SQL + " AND JONG='8' ";
                            }
                            else
                            {
                                SQL = SQL + " AND JONG<>'8' ";
                            }
                            break;
                        default:
                            break;
                    }
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    TES.ROWID = dt.Rows[0]["vROWID"].ToString().Trim();
                    TES.Code = dt.Rows[0]["vCode"].ToString().Trim();
                    TES.Jong = dt.Rows[0]["vJong"].ToString().Trim();
                    TES.Pname = dt.Rows[0]["vPname"].ToString().Trim();
                    TES.Bun = dt.Rows[0]["vBun"].ToString().Trim();
                    TES.Danwi1 = dt.Rows[0]["vDanwi1"].ToString().Trim();
                    TES.Danwi2 = dt.Rows[0]["vDanwi2"].ToString().Trim();
                    TES.Spec = dt.Rows[0]["vSpec"].ToString().Trim();
                    TES.COMPNY = dt.Rows[0]["vCompny"].ToString().Trim();
                    TES.Effect = dt.Rows[0]["vEffect"].ToString().Trim();
                    TES.Gubun = dt.Rows[0]["vGubun"].ToString().Trim();
                    TES.Dangn = dt.Rows[0]["vDangn"].ToString().Trim();
                    TES.JDate1 = dt.Rows[0]["vJDate1"].ToString().Trim();
                    TES.Price1 = dt.Rows[0]["vPrice1"].ToString().Trim();
                    TES.JDate2 = dt.Rows[0]["vJDate2"].ToString().Trim();
                    TES.Price2 = dt.Rows[0]["vPrice2"].ToString().Trim();
                    TES.JDate3 = dt.Rows[0]["vJDate3"].ToString().Trim();
                    TES.Price3 = dt.Rows[0]["vPrice3"].ToString().Trim();
                    TES.JDate4 = dt.Rows[0]["vJDate4"].ToString().Trim();
                    TES.Price4 = dt.Rows[0]["vPrice4"].ToString().Trim();
                    TES.JDate5 = dt.Rows[0]["vJDate5"].ToString().Trim();
                    TES.Price5 = dt.Rows[0]["vPrice5"].ToString().Trim();
                }
                else
                {
                    TES.ROWID = ""; TES.Code = ""; TES.Jong = "";
                    TES.Pname = ""; TES.Bun = ""; TES.Danwi1 = "";
                    TES.Danwi2 = ""; TES.Spec = ""; TES.COMPNY = "";
                    TES.Effect = ""; TES.Gubun = ""; TES.Dangn = "";
                    TES.JDate1 = ""; TES.Price1 = "0";
                    TES.JDate2 = ""; TES.Price2 = "0";
                    TES.JDate3 = ""; TES.Price3 = "0";
                    TES.JDate4 = ""; TES.Price4 = "0";
                    TES.JDate5 = ""; TES.Price5 = "0";
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
        }

        private void ss2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strMsg;
            string strData;
            if (e.RowHeader == true || e.ColumnHeader == true) return;

            //' If Col = 3 Then '수가 금액 조회


            ssAMT2_Sheet1.Cells[0, ssAMT2_Sheet1.RowCount - 1, 0, ssAMT2_Sheet1.ColumnCount - 1].Text = "";

            strMsg = "";

            strData = ss2_Sheet1.Cells[e.Row, 2].Text;
            strMsg = strMsg + "  품명코드: " + VB.Trim(ss2_Sheet1.Cells[e.Row, 2].Text);
            txtSCode_H.Text = txtCode.Text;
            txtSNext_H.Text = ss2_Sheet1.Cells[e.Row, 2].Text;
            strMsg = strMsg + "  표준코드: " + VB.Trim(ss2_Sheet1.Cells[e.Row, 8].Text);
            strMsg = strMsg + "  표준계수: " + VB.Trim(ss2_Sheet1.Cells[e.Row, 9].Text);
            strMsg = strMsg + "  품명: " + VB.Trim(ss2_Sheet1.Cells[e.Row, 17].Text);

            lblSugaMsg.Text = strMsg;

            if (strData != "")
            {
                SUGA_AMT_READ(txtCode.Text, strData, ssAMT2);
            }
        }

        private void ss2_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ss2_Sheet1.NonEmptyRowCount == 0) return;

            string strData;

            if (e.Column == 7)  //'간호활동
            {
                GstrHelpCode = "";

                //닫는 이벤트 내용
                if (frmNurCodeHelpX != null)
                {
                    frmNurCodeHelpX.Dispose();
                    frmNurCodeHelpX = null;
                }
                frmNurCodeHelpX = new frmNurCodeHelp();
                frmNurCodeHelpX.StartPosition = FormStartPosition.CenterParent;
                frmNurCodeHelpX.rSetHelpCode += frmNurCodeHelpX_rSetHelpCode;
                frmNurCodeHelpX.rEventClosed += frmNurCodeHelpX_rEventClosed;
                frmNurCodeHelpX.ShowDialog();

                if (GstrHelpCode != "")
                {
                    ss2_Sheet1.Cells[e.Row, e.Column].Text = VB.Trim(VB.Left(GstrHelpCode, 4));
                    ss2_Sheet1.Cells[e.Row, 75 + 3].Text = "Y"; //'변경여부                    
                    GstrHelpCode = "";
                }
                SendKeys.Send("{Tab}");
                return;
            }


            if (e.Column != 8 && e.Column != 57 && e.Column != 86 && e.Column != 90 && e.Column != 94) return;

            strData = ss2_Sheet1.Cells[e.Row, e.Column].Text.Trim().ToUpper();

            if (strData != "JJJJJJ")
            {
                GstrHelpCode = strData;
                //GstrHelpCode = strData;

                //닫는 이벤트 내용
                if (frmSearchBCodeX != null)
                {
                    frmSearchBCodeX.Dispose();
                    frmSearchBCodeX = null;
                }
                frmSearchBCodeX = new frmSearchBCode(GstrHelpCode);
                frmSearchBCodeX.StartPosition = FormStartPosition.CenterParent;
                frmSearchBCodeX.Show();
                frmSearchBCodeX.TopMost = true;

            }
            else
            {
                GstrHelpCode = strData;
                //GstrHelpCode = strData;

                //닫는 이벤트 내용
                if (frmJunCodeEntryX != null)
                {
                    frmJunCodeEntryX.Dispose();
                    frmJunCodeEntryX = null;
                }
                frmJunCodeEntryX = new frmJunCodeEntry(GstrHelpCode);
                frmJunCodeEntryX.StartPosition = FormStartPosition.CenterParent;
                frmJunCodeEntryX.Show();
                frmJunCodeEntryX.TopMost = true;
            }

            GstrHelpCode = "";
            //GstrHelpCode = "";
        }

        private void ss2_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {

            //'-2015-12-29-------------------------( SS2 항목 구성표 )-----------------------------------------------
            //'  01.Del   02.순위 03.품명코드 04.분류 05.누적   08.간호활동
            //'  09.보험(표준)코드 10.환산계수    11.종류
            //'  12.SS    13.Bi       14.수량  15.보험가 16.자보가 17.일반가
            //'  18.품명  19.한글수가 20.SugbA 21.SugbB 22.SugbC 23.SugbD 24.SugbE 25.SugbF 26.SugbG
            //'  27.SugbH 28.SugbI 29.SugbJ 30.SugbK 31.SugbL 32.SugbM 33.SugbN 34.SugbO 35.SugbP
            //'  36.SugbQ 37.SugbR 38.SugbS 39.SugbT 40.SugbU 41.SugbV 42.SugbW  43.SugbX,
            //'  44.SugbY  45.SugbZ, 46.SugbAA,

            //'  47.변경일자1      48.보험가1     49.자보가1     50.일반수가1
            //'  51.적용일자       52.Old표준코드 53.Old환산계수 54.종류
            //'  55.영문명칭       56.약품성분명  57.단위        58.약품대분류 59.원가분류
            //'  60.구입원가(사용않함)
            //'  61.변경일자2      62.보험가2     63.자보가2   64.일반수가2
            //'  65.변경일자3      66.보험가3     67.자보가3   68.일반수가3
            //'  69.변경일자4      70.보험가4     71.자보가4   72.일반수가4
            //'  73.변경여부
            //'  74.종전보험       75.종전자보    76.종전일반,
            //'  77.원가 재료대    78.원가 조영제 79.순위
            //'  80.적용일자3      81.표준코드3   82.환산계수3 83.종류3
            //'  84.적용일자3      85.표준코드3   86.환산계수3 87.종류3
            //'  88.적용일자3      89.표준코드3   90.환산계수3 91.종류3
            //'  92.rowid
            //'--------------------------------------------------------------------------------------------

            //'--------------------------( SS2 항목 구성표 )-----------------------------------------------
            //'  01.Del   02.순위 03.품명코드 04.분류 05.누적   08.간호활동
            //'  09.보험(표준)코드 10.환산계수    11.종류
            //'  12.SS    13.Bi       14.수량  15.보험가 16.자보가 17.일반가
            //'  18.품명  19.한글수가 20.SugbA 21.SugbB 22.SugbC 23.SugbD 24.SugbE 25.SugbF 26.SugbG
            //'  27.SugbH 28.SugbI 29.SugbJ 30.SugbK 31.SugbL 32.SugbM 33.SugbN 34.SugbO 35.SugbP
            //'  36.SugbQ 37.SugbR 38.SugbS 39.SugbT 40.SugbU 41.SugbV 42.SugbW  43.SugbX,  44.SugbY  45.SugbZ,

            //'  46.변경일자1      47.보험가1     48.자보가1     49.일반수가1
            //'  50.적용일자       51.Old표준코드 52.Old환산계수 53.종류
            //'  54.영문명칭       55.약품성분명  56.단위        57.약품대분류 58.원가분류
            //'  59.구입원가(사용않함)
            //'  60.변경일자2      61.보험가2     62.자보가2   63.일반수가2
            //'  64.변경일자3      65.보험가3     66.자보가3   67.일반수가3
            //'  68.변경일자4      69.보험가4     70.자보가4   71.일반수가4
            //'  72.변경여부
            //'  73.종전보험       74.종전자보    75.종전일반,
            //'  76.원가 재료대    77.원가 조영제 78.순위
            //'  79.적용일자3      80.표준코드3   81.환산계수3 82.종류3
            //'  83.적용일자3      84.표준코드3   85.환산계수3 86.종류3
            //'  87.적용일자3      88.표준코드3   89.환산계수3 90.종류3
            //'  91.rowid
            //'--------------------------------------------------------------------------------------------


            //'--------------------------( SS2 항목 구성표 )-----------------------------------------------
            //'  01.Del   02.순위 03.품명코드 04.분류 05.누적   08.간호활동
            //'  09.보험(표준)코드 10.환산계수    11.종류
            //'  12.SS    13.Bi       14.수량  15.보험가 16.자보가 17.일반가
            //'  18.품명  19.한글수가 20.SugbA 21.SugbB 22.SugbC 23.SugbD 24.SugbE 25.SugbF 26.SugbG
            //'  27.SugbH 28.SugbI 29.SugbJ 30.SugbK 31.SugbL 32.SugbM 33.SugbN 34.SugbO 35.SugbP
            //'  36.SugbQ 37.SugbR 38.SugbS 39.SugbT 40.SugbU 41.SugbV 42.SugbW  43.SugbX

            //'  44.변경일자1      45.보험가1     46.자보가1     47.일반수가1
            //'  48.적용일자       49.Old표준코드 50.Old환산계수 51.종류
            //'  52.영문명칭       53.약품성분명  54.단위        55.약품대분류 56.원가분류
            //'  57.구입원가(사용않함)
            //'  58.변경일자2      59.보험가2     60.자보가2   61.일반수가2
            //'  62.변경일자3      63.보험가3     64.자보가3   65.일반수가3
            //'  66.변경일자4      67.보험가4     68.자보가4   69.일반수가4
            //'  70.변경여부
            //'  71.종전보험       72.종전자보    73.종전일반,
            //'  74.원가 재료대    75.원가 조영제 76.순위
            //'  77.적용일자3      78.표준코드3   79.환산계수3 79.종류3
            //'  81.적용일자3      82.표준코드3   83.환산계수3 84.종류3
            //'  85.적용일자3      86.표준코드3   87.환산계수3 88.종류3
            //'  89.rowid
            //'--------------------------------------------------------------------------------------------


            //'2013/10/04--------------------------( SS2 항목 구성표 )-----------------------------------------------
            //'  01.Del   02.순위 03.품명코드 04.분류 05.누적   08.간호활동
            //'  09.보험(표준)코드 10.환산계수    11.종류
            //'  12.SS    13.Bi       14.수량  15.보험가 16.자보가 17.일반가
            //'  18.품명  19.한글수가 20.SugbA 21.SugbB 22.SugbC 23.SugbD 24.SugbE 25.SugbF 26.SugbG
            //'  27.SugbH 28.SugbI 29.SugbJ 30.SugbK 31.SugbL 32.SugbM 33.SugbN 34.SugbO 35.SugbP
            //'  36.SugbQ 37.SugbR 38.SugbS 39.SugbT 40.SugbU 41.SugbV 42.SugbW

            //'  43.변경일자1      44.보험가1     45.자보가1     46.일반수가1
            //'  47.적용일자       48.Old표준코드 49.Old환산계수 50.종류
            //'  51.영문명칭       52.약품성분명  53.단위        54.약품대분류 55.원가분류
            //'  56.구입원가(사용않함)
            //'  57.변경일자2      58.보험가2     59.자보가2   60.일반수가2
            //'  61.변경일자3      62.보험가3     63.자보가3   64.일반수가3
            //'  65.변경일자4      66.보험가4     67.자보가4   68.일반수가4
            //'  69.변경여부
            //'  70.종전보험       71.종전자보    72.종전일반,
            //'  73.원가 재료대    74.원가 조영제 75.순위
            //'  76.적용일자3      77.표준코드3   78.환산계수3 79.종류3
            //'  80.적용일자3      81.표준코드3   82.환산계수3 83.종류3
            //'  84.적용일자3      85.표준코드3   86.환산계수3 87.종류3
            //'  88.rowid
            //'--------------------------------------------------------------------------------------------


            //'-2010/03/05-------------------------( SS2 항목 구성표 )-----------------------------------------------
            //'  01.Del   02.순위 03.품명코드 04.분류 05.누적   08.간호활동
            //'  09.보험(표준)코드 10.환산계수    11.종류
            //'  12.SS    13.Bi       14.수량  15.보험가 16.자보가 17.일반가
            //'  18.품명  19.한글수가 20.SugbA 21.SugbB 22.SugbC 23.SugbD 24.SugbE 25.SugbF 26.SugbG
            //'  27.SugbH 28.SugbI 29.SugbJ 30.SugbK 31.SugbL 32.SugbM 33.SugbN 34.SugbO 35.SugbP
            //'  36.SugbQ 37.SugbR 38.SugbS 39.SugbT 40.SugbU 41.SugbV
            //'  42.변경일자1      43.보험가1     44.자보가1     45.일반수가1
            //'  46.적용일자       47.Old표준코드 48.Old환산계수 49.종류
            //'  50.영문명칭       51.약품성분명  52.단위      53.약품대분류 54.원가분류
            //'  55.구입원가(사용않함)
            //'  56.변경일자2      57.보험가2     58.자보가2   59.일반수가2
            //'  60.변경일자3      61.보험가3     62.자보가3   63.일반수가3
            //'  64.변경일자4      65.보험가4     66.자보가4   67.일반수가4
            //'  68.변경여부
            //'  69.종전보험       70.종전자보    71.종전일반,
            //'  72.원가 재료대    73.원가 조영제 74.순위
            //'  75.적용일자3      76.표준코드3   77.환산계수3 78.종류3
            //'  79.적용일자3      80.표준코드3   81.환산계수3 82.종류3
            //'  83.적용일자3      84.표준코드3   85.환산계수3 86.종류3
            //'  87.rowid
            //'--------------------------------------------------------------------------------------------


            //'-2008/01/26-------------------------( SS2 항목 구성표 )-----------------------------------------------
            //'  01.Del   02.순위 03.품명코드 04.분류 05.누적   08.간호활동
            //'  09.SS    10.Bi       11.수량 12.보험가 13.자보가 14.일반가
            //'  15.품명  16.한글수가 17.SugbA 18.SugbB 19.SugbC 20.SugbD 21.SugbE 22.SugbF 23.SugbG
            //'  24.SugbH 25.SugbI 26.SugbJ 27.SugbK 28.SugbL 29.SugbM 30.SugbN 31.SugbO 32.SugbP
            //'  33.SugbQ 34.SugbR 35.SugbS 36.SugbT 37.SugbU 38.SugbV
            //'  39.변경일자1      40.보험가1     41.자보가1   42.일반수가1
            //'  43.보험(표준)코드 44.환산계수    45.종류      46.적용일자
            //'  47.Old표준코드    48.Old환산계수 49.종류
            //'  50.영문명칭       51.약품성분명  52.단위      53.약품대분류 54.원가분류
            //'  55.구입원가(사용않함)
            //'  56.변경일자2      57.보험가2     58.자보가2   59.일반수가2
            //'  60.변경일자3      61.보험가3     62.자보가3   63.일반수가3
            //'  64.변경일자4      65.보험가4     66.자보가4   67.일반수가4
            //'  68.변경여부
            //'  69.종전보험       70.종전자보    71.종전일반,
            //'  72.원가 재료대    73.원가 조영제 74.순위
            //'  75.적용일자3      76.표준코드3   77.환산계수3 78.종류3
            //'  79.적용일자3      80.표준코드3   81.환산계수3 82.종류3
            //'  83.적용일자3      84.표준코드3   85.환산계수3 86.종류3
            //'  87.rowid
            //'--------------------------------------------------------------------------------------------


            //'--------------------------( SS2 항목 구성표 )-----------------------------------------------
            //'  01.Del   02.품명코드 03.분류 04.누적   07.간호활동
            //'  08.SS    09.Bi       10.수량 11.보험가 12.자보가 13.일반가
            //'  14.품명  15.한글수가 16.SugbA 17.SugbB 18.SugbC 19.SugbD 20.SugbE 21.SugbF 22.SugbG
            //'  23.SugbH 24.SugbI 25.SugbJ 26.SugbK 27.SugbL 28.SugbM 29.SugbN 30.SugbO 31.SugbP
            //'  32.SugbQ 33.SugbR 34.SugbS 35.SugbT 36.SugbU 37.SugbV
            //'  38.변경일자1      39.보험가1     40.자보가1   41.일반수가1
            //'  42.보험(표준)코드 43.환산계수    44.종류      45.적용일자
            //'  46.Old표준코드    47.Old환산계수 48.종류
            //'  49.영문명칭       50.약품성분명  51.단위      52.약품대분류 53.원가분류
            //'  54.구입원가(사용않함)
            //'  55.변경일자2      56.보험가2     57.자보가2   58.일반수가2
            //'  59.변경일자3      60.보험가3     61.자보가3   62.일반수가3
            //'  63.변경일자4      64.보험가4     65.자보가4   66.일반수가4
            //'  67.변경여부
            //'  68.종전보험       69.종전자보    70.종전일반,
            //'  71.원가 재료대    72.원가 조영제 73.순위
            //'  74.적용일자3      75.표준코드3   76.환산계수3 77.종류3
            //'  78.적용일자3      79.표준코드3   80.환산계수3 81.종류3
            //'  82.적용일자3      83.표준코드3   84.환산계수3 85.종류3
            //'  86.rowid
            //'--------------------------------------------------------------------------------------------



            //''--------------------------(SS2 항목 구성표 OLD)---------------------------------------------- -
            //''  01.Del   02.품명코드 03.분류 04.누적 07.SS 08.Bi 09.수량 10.보험가 11.자보가 12.일반가
            //''  13.품명  14.한글수가 15.SugbA 16.SugbB 17.SugbC 18.SugbD 19.SugbE 20.SugbF 21.SugbG
            //''  22.SugbH 23.SugbI 24.SugbJ 25.SugbK 26.SugbL 27.SugbM 28.SugbN 29.SugbO 30.SugbP
            //''  31.SugbQ 32.SugbR 33.SugbS 34.SugbS 35.SugbU 36.SugbV
            //''  37.변경일자1      38.보험가1     39.자보가1   40.일반수가1
            //''  41.보험(표준)코드 42.환산계수    43.종류      44.적용일자
            //''  45.Old표준코드    46.Old환산계수 47.종류
            //''  48.영문명칭       49.약품성분명  50.단위      51.약품대분류 52.원가분류
            //''  53.구입원가
            //''  54.변경일자2      55.보험가2     56.자보가2   57.일반수가2
            //''  58.변경일자3      59.보험가3     60.자보가3   61.일반수가3
            //''  62.변경일자4      63.보험가4     64.자보가4   65.일반수가4
            //''  66.변경여부
            //''  67.종전보험       68.종전자보    69.종전일반,
            //''  70.원가 재료대    71.원가 조영제 72.순위
            //''  73.적용일자3      74.표준코드3   75.환산계수3   76.종류3
            //''  77.적용일자3      78.표준코드3   79.환산계수3   80.종류3
            //''  81.적용일자3      82.표준코드3   83.환산계수3   84.종류3
            //''  85.rowid
            //''--------------------------------------------------------------------------------------------




            SS2_Help_Message(e.NewRow, e.NewColumn);

            string strMsg;
            string strData;

            //' If Row = 0 Or Col = 0 Then Exit Sub


            //' If Col = 3 Then '수가 금액 조회

            ssAMT2_Sheet1.Cells[0, ssAMT2_Sheet1.RowCount - 1, 0, ssAMT2_Sheet1.ColumnCount - 1].Text = "";

            strMsg = "";

            strData = ss2_Sheet1.Cells[e.Row, 2].Text;
            strMsg = strMsg + "  품명코드: " + VB.Trim(ss2_Sheet1.Cells[e.Row, 2].Text);
            strMsg = strMsg + "  표준코드: " + VB.Trim(ss2_Sheet1.Cells[e.Row, 8].Text);
            strMsg = strMsg + "  표준계수: " + VB.Trim(ss2_Sheet1.Cells[e.Row, 9].Text);
            strMsg = strMsg + "  품명: " + VB.Trim(ss2_Sheet1.Cells[e.Row, 17].Text);

            lblSugaMsg.Text = strMsg;


            if (strData != "")
            {
                //SUGA_AMT_READ(txtCode.Text, strData, ssAMT2);
            }


            //'SQL = " SELECT SUDATE, BAMT, TAMT, IAMT, SAMT, ROWID  FROM KOSMOS_PMPA.BAS_SUGA_AMT "
            //'SQL = SQL & " WHERE SUCODE = '" & TxtCode.Text & "' "
            //'SQL = SQL & "   AND SUNEXT = '" & strData & "' "
            //'SQL = SQL & "  ORDER BY SUDATE DESC "
            //'Result = AdoOpenSet(Rs, SQL)
            //'
            //'
            //'
            //'
            //'If SSAMT2.MaxCols < RowIndicator Then
            //'  SSAMT2.MaxCols = RowIndicator + 1
            //'End If
            //'
            //'For i = 0 To RowIndicator - 1
            //'   SSAMT2.Col = i + 1
            //'   SSAMT2.Row = 1: SSAMT2.Text = Trim(Rs!SuDate & "")
            //'   SSAMT2.Row = 2: SSAMT2.Text = Trim(Rs!BAmt & "")
            //'   SSAMT2.Row = 3: SSAMT2.Text = Trim(Rs!TAmt & "")
            //'   SSAMT2.Row = 4: SSAMT2.Text = Trim(Rs!IAmt & "")
            //'   SSAMT2.Row = 5: SSAMT2.Text = Trim(Rs!SAMT & "")
            //'   SSAMT2.Row = 6: SSAMT2.Text = Trim(Rs!ROWID & "")
            //'   Rs.MoveNext
            //'Next i
            //'
            //'AdoCloseSet Rs

        }



        private void SS2_Help_Message(int NewRow, int NewCol)
        {

            switch (NewCol + 1)
            {
                case 4:
                    lblMsg.Text = "분류코드를 입력하세요";
                    break;
                case 5:
                    lblMsg.Text = "누적코드를 입력하세요.";
                    break;

                case 8:  //'간호활동
                    lblMsg.Text = READ_NurCodeName(ss2_Sheet1.Cells[NewRow, NewCol].Text);
                    break;

                //'SugbSS(성인,소아구분),SugbBi
                case 12:
                    lblMsg.Text = "방사선: 0.전체 1.성인남자 2.성인여자 3.9-12세남아 4.8세이하" + ComNum.VBLF;
                    lblMsg.Text = lblMsg.Text + "방사선제외: 0.전체환자 1.8세이상 2.8세미만";
                    break;
                case 13:
                    lblMsg.Text = "보험일반구분(0.공통 1.보험 2.일반  3.TA )"; //2020-07-06
                    break;
                case 1:
                    lblMsg.Text = "0.전체환자공통계산 1.보험환자만계산 2.일반환자만 계산";
                    break;
                //'수가구분 A-L항
                case 17:
                    lblMsg.Text = "1.단순, 2.복합, 3. Routine";
                    break;
                case 18:
                    lblMsg.Text = "◈약조제료(1.내복 2.내복+제재 3.외용 4.외용+제재) ◈소아가산(A.10% B.15% C.20% D.25% E.30% F.35% G.40% H.45% I.50%)" + ComNum.VBLF;
                    lblMsg.Text = lblMsg.Text + "  ◈주사수기료(1.IM, 2.IV, 3.IM/IV, 4.100cc, 5.500cc, 6.1000cc, 9.관절강내) 8.자가투여 주사제";
                    break;
                case 19:
                    lblMsg.Text = "0. 심야가산 무, 1. 심야가산가능, 1. 5cc 증류수, 2. 10cc 증류수, 3. 20cc 증류수 4. 5ccNSA, 5. 10ccNSA, 6. 20ccNSA )";
                    break;
                case 20:
                    lblMsg.Text = "검사체감종목수(1-9,A:10,B:11,C:12,D=13,E:14,F=15)";
                    break;
                case 21:
                    lblMsg.Text = "기술가산여부 (0.재료대 1.기술료가산)";
                    break;
                case 22:
                    lblMsg.Text = "0. 급 여  ,  1. 비 급 여  ,  2. 비 급 여 (입력 수정 가능)";
                    break;
                case 23:
                    lblMsg.Text = "1. 수량,날수 입력, 2. 수량 입력, 3.날수 입력, 4.시간,분 입력, 6. 금액 입력";
                    break;
                case 24:
                    lblMsg.Text = "[특진료구분] 0. 가산무, 1. 가산가능 50%, 2. 가산 가능 100%";
                    break;
                case 25:
                    lblMsg.Text = "1.재원기간수량, 2.재원기간일수, 3.평생관리(수량), 4.평생관리(일수), 5.사용제한약품, 6.일당관리, 7.주당관리, 8.월관리";
                    break;
                case 26:
                    lblMsg.Text = "▶검사(0.일반 9.외부의뢰),▶약주사(1.원외처방전용 2.입원처방전용 3.원내외혼용)";
                    break;
                case 27:
                    lblMsg.Text = "0.감액대상 코드 1.감액제외 코드 2.재료대 정상할인율적용";
                    break;
                case 28:
                    lblMsg.Text = "1.수가 2.준용수가 3.국산보험등제약 4.수입약 7.협약재료 8.일반재료";
                    break;
                case 29:
                    lblMsg.Text = "0.기타 1.퇴장방지의약품 10%가산(수가에 가산하여 입력하세요)";
                    break;
                case 30:
                    lblMsg.Text = "0.기타 1.선수납수가";
                    break;
                case 31:
                    lblMsg.Text = "의약분업(0.기타 1.예방접종 2.진단의약품 3.결핵치료제 4.조제실제제 5.마약 6.방사성의약품" + ComNum.VBLF;
                    lblMsg.Text = lblMsg.Text + "7.기계장치이용 8.시술필요약품 9.희귀의약품 A.차광,냉장주사제 B.항암주사";
                    break;
                case 32:
                    lblMsg.Text = "비급여분류(1.인정 2.임의 9.제외)";
                    break;
                case 33:
                    lblMsg.Text = "산재급여(0.적용안함 1.급여)";
                    break;
                case 34:
                    lblMsg.Text = "외래비급여중(0.자보급여 1.자보비급여)";
                    break;
                case 35:
                    lblMsg.Text = "100/100 표시(1.100/100)";
                    break;
                case 36:
                    lblMsg.Text = "DRG 비급여( 1.DRG 비급여)";
                    break;
                case 37:
                    lblMsg.Text = "고가약제( 1.고가, 0.저가)";
                    break;
                case 38:
                    lblMsg.Text = "약제, 재료 수불구분(0.행위, 1.약제, 2.재료, 3.약제(제제약), 4.방사선조영제, 5.마취(분), 6.공급실(가공), 9.기타)";
                    break;
                case 42:
                    lblMsg.Text = "치매 지원사업(0.일반 1.진단, 2.감별) ";
                    break;
                case 43:
                    lblMsg.Text = "산재자보 기술가산여부 (0.재료대 1.기술료가산)";
                    break;
                case 44:
                    lblMsg.Text = "외과가산여부(0.가산무 1.20%, 2.30%) ";
                    break;
                case 45:
                    lblMsg.Text = "흉부외과가산여부 (0.가산무 1.100%, 2. 70%  3.30%   4.20%)";
                    break;
                case 46:
                    lblMsg.Text = "응급가산 (0.가산무 1.응급가산 50%, 2. 중증응급가산 50%, 3. 권역응급가산 50% )";
                    break;
                case 47:
                    lblMsg.Text = "판독가산 (0.가산무 1.판독가산 10%)";
                    break;
                case 48:
                    lblMsg.Text = "마취가산 (0.가산무 1.개두마취 30%, 2. 일측폐환기 30%, 3. 개흉적 심장수술 마취 30% )";
                    break;

                //'표준코드,표준계수
                //'case 43: lblMsg.Text = "표준코드를 입력하세요(더블클릭:표준코드 찾기,999999,AAAAAA:EDI제외,000000:EDI산정제외)"
                //'case 44: lblMsg.Text = "표준계수를 입력하세요"


                case 54 + 2:
                case 83 + 2:
                case 87 + 2:
                case 91 + 2:
                    lblMsg.Text = "Old표준계수 적용일자를 입력하세요";
                    break;
                case 55 + 2:
                case 84 + 2:
                case 88 + 2:
                case 92 + 2:
                    lblMsg.Text = "Old표준코드를 입력하세요(더블클릭:표준코드 찾기,999999, AAAAAA:EDI제외,000000:EDI산정제외)";
                    break;
                case 56 + 2:
                case 85 + 2:
                case 86 + 2:
                case 93 + 2:
                    lblMsg.Text = "Old표준계수를 입력하세요";
                    break;


                //'영문명칭,약품성분명,단위,약품대분류
                case 58 + 2:
                    lblMsg.Text = "영문 수가명칭을 입력하세요";
                    break;
                case 59 + 2:
                    lblMsg.Text = "약품 성분명을 입력하세요";
                    break;
                case 60 + 2:
                    lblMsg.Text = "단위를 입력하세요";
                    break;
                case 61 + 2:
                    lblMsg.Text = "약품인경우 보사부분류코드를 입력하세요";
                    break;

                //'원가분류,구입원가
                case 62 + 2:
                    lblMsg.Text = "원가분류코드 4자리를 입력하세요";
                    break;
                case 63 + 2:
                    lblMsg.Text = "구입원가를 입력하세요.";
                    break;
                case 80 + 2:
                    lblMsg.Text = "원가분석: 0.행위료 1.재료대 입력하세요.";
                    break;
                case 81 + 2:
                    lblMsg.Text = "원가분석: 0.일반 1.방사선 조영제 입력하세요.";
                    break;
                case 82 + 2:
                    lblMsg.Text = "그룹코드별 우선순위를 입력하세요(0-99)";
                    break;
                default:
                    lblMsg.Text = "";
                    break;
            }
        }

        private string READ_NurCodeName(string ArgCode)
        {
            string ArgReturn = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                if (ArgCode == "") return ArgReturn;

                if (Convert.ToInt32(VB.Val(ArgCode)) == 0)
                {
                    return ArgReturn;
                }

                SQL = "SELECT GUBUN1 || '-' || GUBUN2 GUBUN FROM KOSMOS_ADM.ABC_NURSE_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE Code=" + ArgCode + " ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["GUBUN"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "** ERROR **";
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

            return ArgReturn;
        }

        private void ss3_ClipboardPasting(object sender, ClipboardPastingEventArgs e)
        {
            try
            {

                if (ss3_Sheet1.ActiveRowIndex == 0 || ss3_Sheet1.ActiveRowIndex == 1 || Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == false) return;

                string strClipboardText = (string)Clipboard.GetDataObject().GetData(DataFormats.Text);

                if (strClipboardText.IndexOf("\r\n") == -1)
                {
                    return;
                }

                strClipboardText = strClipboardText.Substring(0, strClipboardText.IndexOf("\r"));

                //'변수에 담아둠(ss3의 컨트록 때사용)
                nCols = ss3_Sheet1.ActiveColumnIndex;
                nRows = ss3_Sheet1.ActiveRowIndex;

                if (nCols == 0 && nRows != 0)
                {
                    //if (string.IsNullOrEmpty(strClipboardText)) return;

                    ss3_Sheet1.Cells[ss3_Sheet1.ActiveRowIndex, 0].Text = strClipboardText;
                    SS3_Bcode_Move();
                }

                e.Handled = true;
            }
            catch
            {
            }
            return;
        }

        private void SS3_Bcode_Move()   //'현재수가를 변경수가1로 Move
        {
            int i;
            int j;
            string strData;
            string strBCode;

            //'변경수가1이 현재일보다 크거나 같으면 Move 안함
            if (VB.Val(ss3_Sheet1.Cells[1, 0].Text.Trim().Replace("-", "")) >= VB.Val(txtSuDate.Text.Trim().Replace("-", ""))) return;

            //'보험수가가 변경되지 않았으면 이동 안함
            strBCode = ss3_Sheet1.Cells[2, 0].Text;

            if (VB.Val(strBCode) == FnBAmt) return;

            //'변경수가1~4를 1칸씩 우측으로 이동            
            for (i = 4; i >= 0; i--)
            {
                for (j = 1; j <= 4; j++)
                {
                    strData = ss3_Sheet1.Cells[j, i].Text;
                    ss3_Sheet1.Cells[j, i + 1].Text = strData;
                }
            }


            //'변경전의 수가를 변경코드1에 MOVE
            ss3_Sheet1.Cells[1, 0].Text = txtSuDate.Text;

            ss3_Sheet1.Cells[2, 1].Text = FstrBCode;
            ss3_Sheet1.Cells[3, 1].Text = FstrSuHam;
            ss3_Sheet1.Cells[4, 1].Text = FstrEdiJong;

            return;
        }

        private void ss3_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //'변수에 담아둠(ss3의 컨트록 때사용)
            nCols = e.Column;
            nRows = e.Row;

            if (chk1.Checked == true) return;

            if (e.Row == 2)
            {
                if (e.RowHeader == true)
                {
                    GstrHelpCode = ss3_Sheet1.Cells[2, 0].Text;
                    //닫는 이벤트 내용
                    if (frmSearchGuipX != null)
                    {
                        frmSearchGuipX.Dispose();
                        frmSearchGuipX = null;
                    }
                    frmSearchGuipX = new frmSearchGuip(GstrHelpCode);
                    frmSearchGuipX.StartPosition = FormStartPosition.CenterParent;
                    frmSearchGuipX.ShowDialog();

                    return;
                }

                if (VB.Trim(ss3_Sheet1.Cells[2, 0].Text) != "JJJJJJ")
                {
                    GstrHelpCode = ss3_Sheet1.Cells[e.Row, e.Column].Text.ToUpper().Trim();
                    GstrHelpCode = ss3_Sheet1.Cells[e.Row, e.Column].Text.ToUpper().Trim();
                    if (GstrHelpCode != "")
                    {
                        if (frmSearchBCodeX != null)
                        {
                            frmSearchBCodeX.Dispose();
                            frmSearchBCodeX = null;
                        }
                        frmSearchBCodeX = new frmSearchBCode(GstrHelpCode);
                        frmSearchBCodeX.StartPosition = FormStartPosition.CenterParent;
                        frmSearchBCodeX.rEventClosed += frmSearchBCodeX_rEventClosed;
                        frmSearchBCodeX.ShowDialog();
                    }
                    else
                    {
                        GstrBCodeShow = "";

                        if (frmSearchBCodeX != null)
                        {
                            frmSearchBCodeX.Dispose();
                            frmSearchBCodeX = null;
                        }
                        frmSearchBCodeX = new frmSearchBCode();
                        frmSearchBCodeX.StartPosition = FormStartPosition.CenterParent;
                        frmSearchBCodeX.rEventClosed += frmSearchBCodeX_rEventClosed;
                        frmSearchBCodeX.ShowDialog();

                        GstrHelpCode = "";
                        GstrHelpCode = "";
                    }
                }
                else
                {
                    GstrHelpCode = ss3_Sheet1.Cells[e.Row, e.Column].Text.ToUpper().Trim();
                    GstrHelpCode = ss3_Sheet1.Cells[e.Row, e.Column].Text.ToUpper().Trim();

                    //닫는 이벤트 내용
                    if (frmJunCodeEntryX != null)
                    {
                        frmJunCodeEntryX.Dispose();
                        frmJunCodeEntryX = null;
                    }
                    frmJunCodeEntryX = new frmJunCodeEntry();
                    frmJunCodeEntryX.StartPosition = FormStartPosition.CenterParent;
                    frmJunCodeEntryX.ShowDialog();

                    GstrHelpCode = "";
                    GstrHelpCode = "";
                }
            }
        }

        private void frmSearchBCodeX_rEventClosed()
        {
            //닫는 이벤트 내용
            if (frmSearchBCodeX != null)
            {
                frmSearchBCodeX.Dispose();
                frmSearchBCodeX = null;
            }
        }

        private void ss3_KeyDown(object sender, KeyEventArgs e)
        {

            if (nRows == 3)
            {
                if (e.KeyValue == 119)
                {
                    if (VB.Trim(ss3_Sheet1.Cells[4, nCols].Text) == "4" || VB.Trim(ss3_Sheet1.Cells[4, nCols].Text) == "8")
                    {
                        GstrHelpCode = VB.UCase(VB.Trim(ss3_Sheet1.Cells[2, nCols].Text));

                        //닫는 이벤트 내용
                        if (frmSearchGuipX != null)
                        {
                            frmSearchGuipX.Dispose();
                            frmSearchGuipX = null;
                        }
                        frmSearchGuipX = new frmSearchGuip(GstrHelpCode);
                        frmSearchGuipX.StartPosition = FormStartPosition.CenterParent;
                        frmSearchGuipX.Show();

                        GstrHelpCode = "";
                    }
                }
                else if (e.KeyValue == 120)
                {
                    if (VB.Trim(ss3_Sheet1.Cells[4, nCols].Text) == "3")
                    {
                        GstrHelpCode = VB.UCase(VB.Trim(ss3_Sheet1.Cells[2, nCols].Text));

                        //닫는 이벤트 내용
                        if (frmYGuipViewX != null)
                        {
                            frmYGuipViewX.Dispose();
                            frmYGuipViewX = null;
                        }
                        frmYGuipViewX = new frmYGuipView(GstrHelpCode, clsType.User.Sabun);
                        frmYGuipViewX.StartPosition = FormStartPosition.CenterParent;
                        frmYGuipViewX.Show();

                        GstrHelpCode = "";
                    }
                }
                else if (nCols == 3 && e.KeyCode == Keys.F10)   //'상한가는 현재표준코드일때만 실행
                {
                    if (VB.Trim(ss3_Sheet1.Cells[4, nCols].Text) == "8")
                    {
                        GstrHelpCode = VB.UCase(VB.Trim(ss3_Sheet1.Cells[2, nCols].Text));

                        //닫는 이벤트 내용
                        if (frmGiSugaHelpX != null)
                        {
                            frmGiSugaHelpX.Dispose();
                            frmGiSugaHelpX = null;
                        }
                        frmGiSugaHelpX = new frmGiSugaHelp();
                        frmGiSugaHelpX.StartPosition = FormStartPosition.CenterParent;
                        frmGiSugaHelpX.Show();

                        GstrHelpCode = "";
                    }
                }
            }
        }

        private void ss3_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string strSSBcode;
            string strSSEdiJong;
            string strSSGesu;


            lblMsg.Text = "";
            if (e.NewRow == 2 && e.NewColumn == 0)
            {
                lblMsg.Text = "보험코드는? (더블클릭:찾기Help,F8:구입신고,F9:약실구입,F10:상한가)";
            }
            else if (e.Row == 2 && e.Column >= 0)
            {
                lblMsg.Text = "Old 보험코드는? (더블클릭:찾기Help,F8:구입신고,F9:실구입조회)";
            }



            if (e.NewRow == 3)
            {
                lblMsg.Text = "◈환산계수=수가단위/표준단위 ◈Ex1)수가 ALS: 10ml/S 표준:1ml 환산계수=10" + ComNum.VBLF;
                lblMsg.Text = lblMsg.Text + "◈Ex2) 수가 A-2LD1 5ml/s 표준:20ml 환산계수=0.25";
            }


            //'If Col = 1 Then Exit Sub
            strSSBcode = VB.Trim(ss3_Sheet1.Cells[2, e.Column].Text);
            strSSGesu = VB.Trim(ss3_Sheet1.Cells[3, e.Column].Text);
            strSSEdiJong = VB.Trim(ss3_Sheet1.Cells[4, e.Column].Text);



            if (VB.Trim(strSSBcode) == "")
            {
                strSSEdiJong = "";
                return;
            }

            if (e.Row == 4)
            {
                switch (strSSEdiJong)
                {
                    case "":
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4": 
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        break;
                    default:
                        ComFunc.MsgBox("Edi 표준코드 종류가 오류입니다", "확인");
                        ss3_Sheet1.Cells[4, e.Column].Text = "";
                        ss3_Sheet1.Cells[4, e.Column].CanFocus = true;
                        break;
                }
            }

            if (e.Row == 2 && e.Column == 0)
            {
                lblMsg.Text = "";
                READ_EDI_SUGA(VB.Trim(strSSBcode), VB.Trim(txtSuNext.Text), strSSEdiJong);
                if (VB.Trim(TES.ROWID) != "")
                {
                    txtGbL.Text = TES.Jong;
                    ss3_Sheet1.Cells[4, e.Column].Text = TES.Jong;
                    lblMsg.Text = TES.Pname;
                }
                else if (strSSBcode == "000000" || strSSBcode == "999999" || strSSBcode == "AAAAAA" || strSSBcode == "JJJJJJ")
                {
                    txtGbL.Text = "1";
                    ss3_Sheet1.Cells[4, e.Column].Text = "1";
                    switch (strSSBcode)
                    {
                        case "000000":
                            lblMsg.Text = "표준코드 산정제외 코드";
                            break;
                        case "999999":
                            lblMsg.Text = "표준코드 추후등록 예정";
                            break;
                        case "AAAAAA":
                            lblMsg.Text = "산재특정 수가코드입니다.";
                            break;
                        case "JJJJJJ":
                            lblMsg.Text = "준용수가 코드입니다.";
                            txtGbL.Text = "2";
                            ss3_Sheet1.Cells[4, e.Column].Text = "2";
                            break;
                    }
                }
                else
                {
                    lblMsg.Text = strSSBcode + "표준코드에 등록 않되었습니다.";
                }



                //'SS3.Row = 3: SS3.Text = "":
                //''비급여수가 읽기
                //'SQL = " SELECT ROWID FROM EDI_SUGA_BIGUP "
                //'SQL = SQL & " WHERE CODE = '" & Trim(strSSBcode) & "'"
                //'result = AdoOpenSet(Rs, SQL)
                //'
                //'If RowIndicator = 0 Then
                //'   SS3.Row = 3: SS3.Text = "":
                //'End If
                //'AdoCloseSet Rs
                //'GstrMsgList = strSSBcode & "표준코드에 등록 않되었습니다." & vbCrLf
                //'GstrMsgList = GstrMsgList & "정말로 변경을 하시겠습니까?"
                //'If MsgBox(GstrMsgList, vbInformation + vbYesNo, "확인") = vbYes Then
                //     'SS3.Row = 2: SS3.Text = "":
                //                    'TxtEdiJong.Locked = False
                //'Else
                //'    SS3.Row = 2: SS3.Text = "0"
                //'    SS3.Row = 2: SS3.Text = "": SS3.SetFocus
                //'End If

                if (strSSBcode != "" && strSSGesu == "")
                {
                    ss3_Sheet1.Cells[4, e.Column].Text = "1";
                }
            }
        }

        private void ss4_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            //''   "11.xx세이하는 비급여"
            //''   "12.xx세이상은 비급여"
            //''   "21.특정과만 급여"
            //''   "22.특정과는 비급여"
            //''   "32.동일성분 n종만 급여,나머지 비급여"
            //''   "41.특정상병만 급여"
            //''   "42.특정상병은 총액"
            //''   "52.특정환자종류,특정상병에 비급여"
            //''   "62.HD환자 필수약제"
            //''   "71.남자 특정과 비급여"
            //''   "72.여자 특정과 비급여"
            //''   "81.동시처방불가"
            //''   "A2.약국전송 제외"
            //'
            //'    Dim strGubunA As String
            //'    Dim strGubunB As String
            //'
            //'    SS4.Row = NewRow
            //'    SS4.Col = 1: strGubunA = SS4.Text
            //'    SS4.Col = 2: strGubunB = SS4.Text
            //'
            //'    Select Case strGubunA & strGubunB
            //'      Case "11": PanelMsg.Caption = "11.xx세 이하는 비급여"
            //'      Case "12": PanelMsg.Caption = "12.xx세 이상은 비급여"
            //'      Case "21": PanelMsg.Caption = "21.특정과만 급여"
            //'      Case "22": PanelMsg.Caption = "22.특정과는 비급여"
            //'      Case "32": PanelMsg.Caption = "32.동일성분 n종만 급여,나머지 비급여"
            //'      Case "41": PanelMsg.Caption = "41.특정상병만 급여"
            //'      Case "42": PanelMsg.Caption = "42.특정상병은 총액"
            //'      Case "52": PanelMsg.Caption = "52.특정환자종류,특정상병에 비급여"
            //'      Case "62": PanelMsg.Caption = "62.HD환자 필수약제"
            //'      Case "71": PanelMsg.Caption = "71.남자 특정과 비급여"
            //'      Case "72": PanelMsg.Caption = "72.여자 특정과 비급여"
            //'      Case "81": PanelMsg.Caption = "81.동시처방불가"
            //'      Case "A2": PanelMsg.Caption = "A2.약국전송 제외"
            //'    End Select


        }

        private void ssDrg_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i;
            int j;
            string strData;


            if (e.Column == 0 && e.Row == 1)
            {
                for (i = ssDrg_Sheet1.ColumnCount - 2; i >= 0; i--)
                {
                    if (i == 1)
                    {
                        i = i;
                    }

                    for (j = 0; j < ssDrg_Sheet1.RowCount; j++)
                    {
                        strData = ssDrg_Sheet1.Cells[j, i].Text;
                        ssDrg_Sheet1.Cells[j, i + 1].Text = strData;
                    }
                }

                ssDrg_Sheet1.Cells[1, 0].Text = txtSuDate.Text;
                ssDrg_Sheet1.Cells[4, 0].Text = ""; //'rowid                
            }
        }

        private void txtGbAA_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈응급가산(1.응급가산50%, 2.중증응급가산50%, 3.권역응급가산)";

            txtGbAA.SelectionStart = 0;
            txtGbAA.SelectionLength = VB.Len(txtGbAA.Text);
        }

        private void txtGbAB_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈판독가산(1.가산10%)";

            txtGbAB.SelectionStart = 0;
            txtGbAB.SelectionLength = VB.Len(txtGbAB.Text);
        }

        private void txtGbAC_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈마취가산(1.개두마취30%, 2.일측폐환기30%, 3.개흉적 심장수술 마취30%)";

            txtGbAC.SelectionStart = 0;
            txtGbAC.SelectionLength = VB.Len(txtGbAC.Text);
        }

        private void txtGbAD_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈화상가산(1.화상가산30%)";

            txtGbAD.SelectionStart = 0;
            txtGbAD.SelectionLength = VB.Len(txtGbAD.Text);
        }

        private void txtGbAA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbAB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbAC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbAD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbAE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbAF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGbAG_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }



        private void txtCode_Enter(object sender, EventArgs e)
        {
            //txtCode.ImeMode = ImeMode.Alpha;
        }

        private void txtDrgCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtDrgName.Text = READ_DRGNAME(txtDrgCode.Text);
            }
        }

        private void txtDrgCode_DoubleClick(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            //닫는 이벤트 내용
            if (frmDrgBaseCodeX != null)
            {
                frmDrgBaseCodeX.Dispose();
                frmDrgBaseCodeX = null;
            }
            frmDrgBaseCodeX = new frmDrgBaseCode();
            frmDrgBaseCodeX.StartPosition = FormStartPosition.CenterParent;
            frmDrgBaseCodeX.rSetHelpCode += frmDrgBaseCodeX_rSetHelpCode;
            frmDrgBaseCodeX.rEventClosed += frmDrgBaseCodeX_rEventClosed;
            frmDrgBaseCodeX.ShowDialog();


            if (GstrHelpCode != "")
            {
                txtDrgCode.Text = GstrHelpCode;
                txtDrgName.Text = READ_DRGNAME(txtDrgCode.Text);
                GstrHelpCode = "";
                SendKeys.Send("{Tab}");
            }
        }

        private void frmDrgBaseCodeX_rEventClosed()
        {
            //닫는 이벤트 내용
            if (frmDrgBaseCodeX != null)
            {
                frmDrgBaseCodeX.Dispose();
                frmDrgBaseCodeX = null;
            }
        }

        private void frmDrgBaseCodeX_rSetHelpCode(string strHelpCode)
        {
            if (strHelpCode != "")
            {
                GstrHelpCode = strHelpCode.Trim();
            }
        }

        private string READ_DRGNAME(string ArgCode)
        {
            string ArgReturn = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                if (VB.Trim(ArgCode) == "" || ArgCode == "")
                {
                    return ArgReturn;
                }

                SQL = " SELECT DCODE, DNAME ";
                SQL = SQL + "  FROM KOSMOS_PMPA.DRG_CODE_NEW ";
                SQL = SQL + " WHERE DCODE = '" + ArgCode + "' ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["DNAME"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "";
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

            return ArgReturn;
        }

        private void txtAntiClass_DoubleClick(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            //닫는 이벤트 내용
            if (frmAntiHelpX != null)
            {
                frmAntiHelpX.Dispose();
                frmAntiHelpX = null;
            }
            frmAntiHelpX = new frmAntiHelp();
            frmAntiHelpX.StartPosition = FormStartPosition.CenterParent;
            frmAntiHelpX.rSetHelpCode += frmAntiHelpX_rSetHelpCode;
            frmAntiHelpX.rEventClosed += frmAntiHelpX_rEventClosed;
            frmAntiHelpX.ShowDialog();

            if (GstrHelpCode != "")
            {
                txtAntiClass.Text = GstrHelpCode;
                lblAntiClass.Text = READ_AntiClassName(txtAntiClass.Text);
                GstrHelpCode = "";
                SendKeys.Send("{Tab}");
            }
        }

        private void frmAntiHelpX_rEventClosed()
        {
            //닫는 이벤트 내용
            if (frmAntiHelpX != null)
            {
                frmAntiHelpX.Dispose();
                frmAntiHelpX = null;
            }
        }

        private void frmAntiHelpX_rSetHelpCode(string strHelpCode)
        {
            if (strHelpCode != "")
            {
                GstrHelpCode = strHelpCode.Trim();
            }
        }

        private void txtAntiClass_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈Anti 계열 분류는? (더블클릭:찾기 Help)◈";
            txtAntiClass.SelectionStart = 0;
            txtAntiClass.SelectionLength = VB.Len(txtAntiClass.Text);
        }

        private void txtBun_DoubleClick(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            //닫는 이벤트 내용
            if (PmpaMirBunHelpX != null)
            {
                PmpaMirBunHelpX.Dispose();
                PmpaMirBunHelpX = null;
            }
            PmpaMirBunHelpX = new PmpaMirBunHelp(GstrHelpCode);
            PmpaMirBunHelpX.StartPosition = FormStartPosition.CenterParent;
            PmpaMirBunHelpX.rSetHelpCode += PmpaMirBunHelpX_rSetHelpCode;
            PmpaMirBunHelpX.rEventClosed += PmpaMirBunHelpX_rEventClosed;
            PmpaMirBunHelpX.ShowDialog();

            if (GstrHelpCode != "")
            {
                txtBun.Text = GstrHelpCode;
                lblBun.Text = Read_BunName(txtBun.Text);
                GstrHelpCode = "";
                SendKeys.Send("{Tab}");
            }
        }

        private void PmpaMirBunHelpX_rEventClosed()
        {
            //닫는 이벤트 내용
            if (PmpaMirBunHelpX != null)
            {
                PmpaMirBunHelpX.Dispose();
                PmpaMirBunHelpX = null;
            }
        }

        private void PmpaMirBunHelpX_rSetHelpCode(string strHelpCode)
        {
            if (strHelpCode != "")
            {
                GstrHelpCode = strHelpCode.Trim();
            }
        }

        private void txtBun_Leave(object sender, EventArgs e)
        {
            lblBun.Text = Read_BunName(txtBun.Text);
        }

        private string Read_BunName(string argBun)     //'분류명칭
        {
            string ArgReturn = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                SQL = "SELECT Name FROM BAS_BUN ";
                SQL = SQL + "WHERE Jong = '1' ";
                SQL = SQL + "  AND Code = '" + VB.Trim(argBun) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["Name"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "";
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

            return ArgReturn;
        }

        private void txtCode_Leave(object sender, EventArgs e)
        {
            //txtCode.Text = txtCode.Text.Trim().ToUpper();
            //if (string.IsNullOrEmpty(txtCode.Text)) return;


            ////'기존의 내용을 읽어와서 코드만 변경할 경우
            //if (FnCodeChange == true)
            //{
            //    TxtCode_CodeChange();
            //    return;
            //}

            //Screen_Display();
        }


        private string READ_AntiClassName(string ArgCode) // '항생제 분류명칭
        {

            string ArgReturn = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                if (VB.Trim(ArgCode) == "")
                {
                    return ArgReturn;
                }

                SQL = " SELECT CODE, NAME FROM BAS_BCODE";
                SQL = SQL + " WHERE GUBUN ='BAS_항생제계열'";
                SQL = SQL + "   AND CODE = '" + ArgCode + "' ";
                SQL = SQL + " ORDER BY CODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["Name"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "";
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

            return ArgReturn;
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //txtCode_Leave(null, null);
                txtCode.Text = txtCode.Text.Trim().ToUpper();
                if (string.IsNullOrEmpty(txtCode.Text)) return;


                //'기존의 내용을 읽어와서 코드만 변경할 경우
                if (FnCodeChange == true)
                {
                    TxtCode_CodeChange();
                    return;
                }

                Screen_Display();
            }
        }

        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.Checked)
            {
                chk.ForeColor = Color.Red;
            }
            else
            {
                chk.ForeColor = Color.White;
            }
        }

        private void chkJob_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.Checked)
            {
                chk.ForeColor = Color.Red;
            }
            else
            {
                chk.ForeColor = Color.Black;
            }
        }

        private void cboCode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void ss1_ClipboardPasting(object sender, ClipboardPastingEventArgs e)
        {
            if (ss1_Sheet1.NonEmptyRowCount == 0 || Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == false) return;

            string strClipboardText = (string)Clipboard.GetDataObject().GetData(DataFormats.Text);

            if (strClipboardText.IndexOf("\r\n") == -1)
            {
                return;
            }

            strClipboardText = strClipboardText.Substring(0, strClipboardText.IndexOf("\r")).Replace(",", "");

            if (ss1_Sheet1.ActiveRowIndex == 2 && ss1_Sheet1.ActiveColumnIndex == 0 && chkJob1.Checked == true)
            {
                //ss1.ClipboardOptions = ClipboardOptions.
                if (VB.IsNumeric(strClipboardText) == false) return;
                ss1_Sheet1.Cells[2, 0].Text = int.Parse(strClipboardText).ToString();
                //ss1_Sheet1.Cells[2, 0].Text = ss1_Sheet1.Cells[2, 0].Text.Trim().Replace(",", "");
                SS1_Suga_Gesan();
            }

            e.Handled = true;
            return;
        }

        private void cboCode_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (VB.Trim(cboCode.Text) == "") return;
            if (VB.Trim(txtCode.Text) == VB.Trim(cboCode.Text)) return;

            btnCancel_Click(sender, e);
            txtCode.Text = cboCode.Text;

            //'기존의 내용을 읽어와서 코드만 변경할 경우
            if (FnCodeChange == true)
            {
                TxtCode_CodeChange();
                return;
            }
            Screen_Display();
        }

        private void ss3_EditModeOff(object sender, EventArgs e)
        {
            //if (ss3_Sheet1.ActiveRowIndex == 0 || ss3_Sheet1.ActiveRowIndex == 1) return;

            ////'변수에 담아둠(ss3의 컨트록 때사용)
            //nCols = ss3_Sheet1.ActiveColumnIndex;
            //nRows = ss3_Sheet1.ActiveRowIndex;

            //if (nCols == 0 && nRows != 0)
            //{
            //    SS3_Bcode_Move();
            //}
        }

        private void ss1_EditModeOff(object sender, EventArgs e)
        {
            if ((ss1_Sheet1.ActiveColumnIndex == 0 && ss1_Sheet1.ActiveRowIndex == 2) && chkJob1.Checked == true)
            {
                //if (VB.IsNumeric(ss1_Sheet1.Cells[ss1_Sheet1.ActiveRowIndex, ss1_Sheet1.ActiveColumnIndex].Text.Replace(",", "")) == false) return;
                //ss1_Sheet1.Cells[e.Row, e.Column].Text = ss1_Sheet1.Cells[e.Row, e.Column].Text.Replace(",", "");
                if(strBiGoAmt != Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[ss1_Sheet1.ActiveRowIndex, ss1_Sheet1.ActiveColumnIndex].Text)))
                {
                    ss1_Sheet1.Cells[ss1_Sheet1.ActiveRowIndex, ss1_Sheet1.ActiveColumnIndex].Text = VB.Val(ss1_Sheet1.Cells[ss1_Sheet1.ActiveRowIndex, ss1_Sheet1.ActiveColumnIndex].Text.Replace(",", "")).ToString();
                    SS1_Suga_Gesan();
                }
            }
            strBiGoAmt = 0;
            return;
        }

        private void ss3_EditChange(object sender, EditorNotifyEventArgs e)
        {
            nCols = e.Column;
            nRows = e.Row;
        }

        private void ss3_Change(object sender, ChangeEventArgs e)
        {
            if (e.Row == 0) return;

            //'변수에 담아둠(ss3의 컨트록 때사용)
            nCols = e.Column;
            nRows = e.Row;

            if (nCols == 0 && nRows != 0)
            {
                SS3_Bcode_Move();
            }
        }

        private void ss2_ClipboardPasting(object sender, ClipboardPastingEventArgs e)
        {
            string strClipboardText = (string)Clipboard.GetDataObject().GetData(DataFormats.Text);
            if (strClipboardText.IndexOf("\n") == -1)
            {
                intClipRowCnt = -1;
                return;
            }

            //e.Handled = true;
            intClipRow = ss2_Sheet1.ActiveRowIndex;
            intClipRowCnt = ss2_Sheet1.ActiveRowIndex + strClipboardText.Split(new char[] { '\n' }).Length;
            //string[] strCol = null;
            //int intRow = ss2_Sheet1.ActiveRowIndex;

            //for (int i = 0; i < strRow.Length; i++)
            //{
            //    strCol = strRow[i].Split(new char[] { (char)9 });
            //    strCol[76 + 2] = "Y";
            //    strCol[95 + 2] = string.Empty;

            //    for (int j = 1; j < strCol.Length; j++)
            //    {
            //        ss2_Sheet1.Cells[intRow + i, j - 1].Text = strCol[i];
            //    }
            //}
        }

        private void ss2_ClipboardPasted(object sender, ClipboardPastedEventArgs e)
        {
            if (ss2_Sheet1.ActiveColumnIndex == 14)
            {
                ss2_Sheet1.Cells[ss2_Sheet1.ActiveRowIndex, 15].Text = ss2_Sheet1.Cells[ss2_Sheet1.ActiveRowIndex, 14].Text;
            }

            if (intClipRowCnt == -1)
            {
                return;
            }

            for(int i = intClipRow; i < intClipRowCnt; i++)
            {
                if(string.IsNullOrEmpty(ss2_Sheet1.Cells[i, 0].Text.Trim()) == false) //있음 수정
                {
                    ss2_Sheet1.Cells[i, 75 + 3].Text = "Y";
                    ss2_Sheet1.Cells[i, 94 + 3].Text = string.Empty;
                }
                else //없음 신규등록
                {
                    ss2_Sheet1.Cells[i, 75 + 3].Text = string.Empty;
                    ss2_Sheet1.Cells[i, 94 + 3].Text = string.Empty;
                }
            }

            if(ss2_Sheet1.ActiveColumnIndex == 14)
            {
                ss2_Sheet1.Cells[ss2_Sheet1.ActiveRowIndex, 15].Text = ss2_Sheet1.Cells[ss2_Sheet1.ActiveRowIndex, 14].Text;
            }

            intClipRow = -1;
            intClipRowCnt = -1; 
        }

        private void btnSize_Click(object sender, EventArgs e)
        {
            if(btnSize.Text.Equals("늘리기"))
            {
                ss2.Height = 426;
                lblSugaMsg.Visible = false;
                panel3.Visible = false;
                btnSize.Text = "줄이기";
            }
            else
            {
                ss2.Height = 270;
                lblSugaMsg.Visible = true;
                panel3.Visible = true;
                btnSize.Text = "늘리기";
            }
        }

        private void txtCode_Click(object sender, EventArgs e)
        {
            //txtCode.ImeMode = ImeMode.Alpha;
        }

        private void mnuCancer_Click(object sender, EventArgs e)
        {
            using (frmILLHCodeCancer frm = new frmILLHCodeCancer())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }

        private void txtGbAF_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈호스피스 별도산정(0.대상아님, 1.별도산정 대상)";

            txtGbAF.SelectionStart = 0;
            txtGbAF.SelectionLength = VB.Len(txtGbAF.Text);
        }

        private void txtGbAE_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈신경외과 가산(0.가산무, 1.5%, 2.10%, 3.15%)";

            txtGbAE.SelectionStart = 0;
            txtGbAE.SelectionLength = VB.Len(txtGbAE.Text);
        }

        private void txtGbAG_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "◈ASA 가산(0.가산무, 1.ASA 가산)";

            txtGbAG.SelectionStart = 0;
            txtGbAG.SelectionLength = VB.Len(txtGbAG.Text);
        }

        private void ss1_EditModeOn(object sender, EventArgs e)
        {
            if (ss1_Sheet1.ActiveRowIndex == 2)
            {
                strBiGoAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[ss1_Sheet1.ActiveRowIndex, ss1_Sheet1.ActiveColumnIndex].Text));
            }
            
        }

        private void btnSelfHang_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();

            //닫는 이벤트 내용
            if (frmSearchUnpaidX2 != null)
            {
                frmSearchUnpaidX2.Dispose();
                frmSearchUnpaidX2 = null;
            }
            frmSearchUnpaidX2 = new frmSearchUnpaid2();
            frmSearchUnpaidX2.StartPosition = FormStartPosition.CenterParent;
            frmSearchUnpaidX2.ShowDialog();
        }
    }
}
