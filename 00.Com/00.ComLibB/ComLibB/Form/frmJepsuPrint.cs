using FarPoint.Win;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : FrmJepsuPrint.cs
    /// Description     : 접수증 출력하기
    /// Author          : 이정현
    /// Create Date     : 2017-08-29
    /// <history> 
    /// 접수증 출력하기
    /// </history>
    /// <seealso>
    /// PSMH\Ocs\ekg\FrmJepsuPrint.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\Ocs\ekg\ekg.vbp
    /// </vbp>
    /// </summary>
    public partial class frmJepsuPrint : Form
    {

        private string GstrPANO = "";
        private string GstrDeptCode = "";
        private string GstrOK = "";
        private string GstrGubun = "";
        private string GstrFluPrt = "";
        private string GstrDate = "";
        private string GstrRDate = "";
        private string GstrDrCode = "";
        private string GstrJob = "";
        private string GstrAddPrt = "";       //환자확인용 추가 인쇄 2019-07-03
        private string mstrPrintName = "";

        private string GstrBI = "";
        private string GstrBi51 = "";
        private string GstrJin = "";
        private string GstrROWID = "";
        private string GstrGbSPC = "";
        private string GstrFall = "";
        private string GstrOldMan = "";
        private string GstrPatChk = "";
        private string GstrEMR2 = "";
        private string GstrAiFlu = "";

        Image img = null;
        string mstrBaCode = "";

        ComFunc CF = new ComFunc();
        clsSpread SP = new clsSpread();

        FarPoint.Win.Spread.CellType.TextCellType TextCell = new FarPoint.Win.Spread.CellType.TextCellType();

        public frmJepsuPrint()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자로 글로벌 변수 대체
        /// Show 하지 않고 사용 하세요
        /// 진단검사용
        /// </summary>
        /// <param name="strGubun">App.EXEName</param>
        /// <param name="strDate"></param>
        /// <param name="strROWID"></param>
        /// <param name="strOK"></param>
        /// <param name="strFluPrt">신종플루</param>
        /// <param name="strPrintName">프린터 이름</param>
        public frmJepsuPrint(string strGubun, string strDate, string strROWID, string strOK, string strFluPrt, string strPrintName)
        {
            InitializeComponent();

            GstrGubun = strGubun;
            GstrOK = strOK;
            GstrFluPrt = strFluPrt;
            GstrDate = strDate;
            GstrROWID = strROWID;
            mstrPrintName = strPrintName;
            //BIXOLON SRP-350plusII

            Print();
        }

        /// <summary>
        /// 생성자로 글로벌 변수 대체
        /// Show 하지 않고 사용 하세요
        /// 외래용
        /// </summary>
        /// <param name="strPANO"></param>
        /// <param name="strDeptCode"></param>
        /// <param name="strDate"></param>
        /// <param name="strFluPrt">신종플루</param>
        /// <param name="strPrintName">프린터 이름</param>
        public frmJepsuPrint(string strPANO, string strDeptCode, string strDate, string strFluPrt, string strPrintName) //외래용
        {
            InitializeComponent();

            GstrPANO = strPANO;
            GstrDeptCode = strDeptCode;
            GstrFluPrt = strFluPrt;
            GstrDate = strDate;
            mstrPrintName = strPrintName;
            //BIXOLON SRP-350plusII

            Print();
        }

        /// <summary>
        /// 생성자로 글로벌 변수 대체
        /// Show 하지 않고 사용 하세요
        /// 외래용
        /// </summary>
        /// <param name="strGubun">외래접수증:OPD, 입원증:IPD</param>
        /// <param name="strPANO">병록번호</param>
        /// <param name="strDeptCode">진료과</param>
        /// <param name="strDate">접수일자</param>
        /// <param name="strDrCode">의사코드</param>
        /// <param name="strJob"></param>
        /// <param name="strPrintName">프린터 이름</param>
        public frmJepsuPrint(string strGubun, string strPANO, string strDeptCode, string strDate, string strRDate, string strDrCode, string strJob, string strPrintName) //외래용
        {
            InitializeComponent();

            GstrGubun = strGubun;
            GstrPANO = strPANO;
            GstrDeptCode = strDeptCode;
            GstrDate = strDate;
            GstrRDate = strRDate;
            GstrDrCode = strDrCode;
            if (strGubun == "IPD") GstrJob = strJob;
            if (strGubun == "OPD") GstrFluPrt = strJob;
            mstrPrintName = strPrintName;

            Print();
        }

        private void Print() //load 대신 사용
        {
            ////폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            TextCell.Multiline = true;
            TextCell.WordWrap = true;

            clsPrint CP = new clsPrint();

            string strPrintName1 = "";
            string strPrintName2 = "";

            try
            {
                strPrintName1 = clsPrint.gGetDefaultPrinter();
                strPrintName2 = CP.getPrinter_Chk(mstrPrintName.ToUpper());

                if (strPrintName2 != "")
                {
                    if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
                    if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

                    clsPrint.gSetDefaultPrinter(strPrintName2);

                    if (GstrOK == "OK" && GstrGubun == "EKG")
                    {
                        btnEKG2_Click(null, null);
                    }
                    else if (GstrOK == "OK2" && GstrGubun == "EKG")
                    {
                        btnEcho_Click(null, null);
                    }
                    else if (GstrGubun == "EKG")
                    {
                        btnEKG_Click(null, null);
                    }
                    else
                    {
                        if (GstrPANO != "" && GstrDeptCode != "")
                        {
                            if (GstrDeptCode == "TO")
                            {
                                btnPrintTO_Click(null, null);
                            }
                            else
                            {
                                btnPrint_Click(null, null);
                            }
                        }
                    }
                }
                ComFunc.Delay(5000);
                clsPrint.gSetDefaultPrinter(strPrintName1);
                //this.Close();

            }
            catch (Exception ex)
            {
                clsPrint.gSetDefaultPrinter(strPrintName1);
                ComFunc.MsgBox("접수증 출력 오류" + ComNum.VBLF + ComNum.VBLF + ex.Message);
                this.Close();
            }
        }

        // 외래간호 출력속도 문제로 별도함수 구성
        public void OpdNr_Print(string strGubun, string strPANO, string strDeptCode, string strDate, string strRDate, string strDrCode, string strJob, string strPrintName, string strPrtAdd = "")
        {
            GstrGubun = strGubun;
            GstrPANO = strPANO;
            GstrDeptCode = strDeptCode;
            GstrDate = strDate;
            GstrRDate = strRDate;
            GstrDrCode = strDrCode;
            GstrAddPrt = strPrtAdd;
            if (strGubun == "IPD") GstrJob = strJob;
            if (strGubun == "OPD") GstrFluPrt = strJob;
            mstrPrintName = strPrintName;

            TextCell.Multiline = true;
            TextCell.WordWrap = true;

            clsPrint CP = new clsPrint();

            string strPrintName1 = "";
            string strPrintName2 = "";

            try
            {
                //if (CP.isPrinterOffLine("EMR서식") == false)
                //{
                //    ComFunc.MsgBox("'EMR서식' 프린터 연결상태를 다시 확인해 주세요.");
                //    return;
                //}
                //if (CP.isPrinterOffLine("접수증") == false)
                //{
                //    ComFunc.MsgBox("'접수증' 프린터 연결상태를 다시 확인해 주세요.");
                //    return;
                //}

                strPrintName1 = CP.getPrinter_Chk("EMR서식");
                strPrintName2 = CP.getPrinter_Chk("접수증");

                // 프린터 드라이버 세팅 에러일시 리턴
                if (strPrintName1 == "")
                {
                    strPrintName1 = clsPrint.gGetDefaultPrinter();
                }

                if (strPrintName2 != "")
                {
                    if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
                    if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

                    if (GstrGubun == "IPD")
                    {
                        // 입원장
                        print_IpdResv(strPrintName2);
                    }
                    else if (GstrGubun == "OPD")
                    {
                        // 외래접수증
                        print_OpdJupsu(strPrintName2);
                    }
                }
                ComFunc.Delay(1000);
            }
            catch (Exception ex)
            {
                clsPrint.gSetDefaultPrinter(strPrintName1);
                ComFunc.MsgBox("접수증 출력 오류" + ComNum.VBLF + ComNum.VBLF + ex.Message);
                this.Close();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;

            string[] strGel = new string[41];
            string strDeptName = "";
            string strDeptCode = "";
            string strPano = "";
            string strBDATE = "";
            string strSName = "";
            string strSex = "";
            string strPart = "";
            string strBi = "";
            string strOutDate = "";
            string strWARD = "";
            string strDeptCnt = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strTelTime = "";
            string strWrtTime = "";
            string strChk = "";
            string strDrcode = "";
            string strDrname = "";
            string strEMR2 = "";
            string StrJin = "";
            string strDeptCode2 = "";
            string strEmrSinGu = "";
            string strChojae = "";
            string strLastDay = "";
            string strSisel = "";
            string strTel = "";
            string strHPhone = "";
            string strErDate = "";
            string strSms = "";
            string strJuso = "";
            string strMailJuso = "";
            string strZipCode = "";
            string strToSName = "";  //당일당일과 동명2인 변수

            string strMCode = "";  //희귀V  = V
            string strVCode = "";  //등록암 = C
            string strAiFlu = "";
            string strgbflu_vac = "";
            string strOldMan = "";  //어르신먼저구분
            string strNameE = "";  //영문 이름

            string strOT = "";  //안과검진구분
            string strGbSPC = "";  //선택진료

            string strGubun = "";
            string strNoExe = "";  //미시행여부
            string strSC = "";  //금연클리닉체크
            string strHC = "";  //해바라기센터구분

            string strPatChk = "";
            string strBi51 = "";

            string strFall = "";  //낙상고위험군

            int intAge = 0;
            int intSEQNO = 0;

            strPatChk = "";     //원무과 보내기 대상

            strPano = GstrPANO;
            strDeptCode = GstrDeptCode;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.CHOJAE,";
                SQL = SQL + ComNum.VBLF + "     A.DEPTCODE, B.SNAME, A.BI, A.AGE, A.EMRSINGU,";
                SQL = SQL + ComNum.VBLF + "     B.JUMIN1, B.JUMIN2,B.JUMIN3, A.PART, A.TELTIME,";
                SQL = SQL + ComNum.VBLF + "     A.WRTTIME, A.SEQNO,A.DRCODE, B.SEX, A.JIN, a.gbflu_vac,a.OldMan,a.Gubun,    ";
                SQL = SQL + ComNum.VBLF + "     B.TEL, B.HPHONE,B.GBSMS, B.JUSO, B.ZIPCODE1 || B.ZIPCODE2 ZIPCODE, B.AIFLU ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_WORK A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND BDATE = TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+)";
                SQL = SQL + ComNum.VBLF + "         AND (A.DELMARK <> '*' OR A.DELMARK IS NULL)";

                if (GstrFluPrt != "Y")  //신플아닐경우
                {
                    SQL = SQL + ComNum.VBLF + "         AND (GbFlu <>'Y' OR gbFlu IS NULL  )";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         AND GbFlu ='Y'";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY SEQNO DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strBDATE = dt.Rows[0]["BDATE"].ToString().Trim();
                    strSName = dt.Rows[0]["SNAME"].ToString().Trim() + READ_BAS_SR_Name2(strPano);
                    strBi = dt.Rows[0]["BI"].ToString().Trim();
                    intAge = Convert.ToInt32(VB.Val(dt.Rows[0]["AGE"].ToString().Trim()));
                    strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();

                    if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    {
                        strJumin2 = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    }
                    else
                    {
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }

                    strPart = dt.Rows[0]["PART"].ToString().Trim();
                    strTelTime = dt.Rows[0]["TELTIME"].ToString().Trim();
                    strWrtTime = dt.Rows[0]["WRTTIME"].ToString().Trim();
                    strDrcode = dt.Rows[0]["DRCODE"].ToString().Trim();
                    strSex = dt.Rows[0]["SEX"].ToString().Trim();
                    intSEQNO = Convert.ToInt32(VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim()));
                    StrJin = dt.Rows[0]["JIN"].ToString().Trim();
                    strTel = VB.IIf(dt.Rows[0]["TEL"].ToString().Trim() == "000-0000", "", dt.Rows[0]["TEL"].ToString().Trim()).ToString();
                    strHPhone = dt.Rows[0]["HPHONE"].ToString().Trim();
                    strSms = dt.Rows[0]["GBSMS"].ToString().Trim();
                    strJuso = dt.Rows[0]["JUSO"].ToString().Trim();
                    strZipCode = dt.Rows[0]["ZIPCODE"].ToString().Trim();
                    strAiFlu = dt.Rows[0]["AIFLU"].ToString().Trim();
                    strgbflu_vac = dt.Rows[0]["GBFLU_VAC"].ToString().Trim();
                    strOldMan = dt.Rows[0]["OLDMAN"].ToString().Trim();
                    strGubun = dt.Rows[0]["GUBUN"].ToString().Trim();
                    strMailJuso = "";

                    //우편번호로 동명칭을 읽음
                    SQL = "";
                    SQL = "SELECT MailJuso FROM " + ComNum.DB_PMPA + "BAS_MAILNEW ";
                    SQL = SQL + ComNum.VBLF + "     WHERE MailCode = '" + strZipCode + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        strMailJuso = dt1.Rows[0]["MAILJUSO"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    strNameE = clsVbfunc.GetPatientEName(clsDB.DbCon, strPano, "1");

                    strChojae = "";

                    if (dt.Rows[0]["CHOJAE"].ToString().Trim() == "1")
                    {
                        strChojae = "(초진)";
                    }

                    if (strChojae == "")
                    {
                        //과초진 여부 설정
                        SQL = "";
                        SQL = "SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_LASTEXAM ";
                        SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND DEPTCODE = '" + strDeptCode + "'";

                        if (strDeptCode == "MD")
                        {
                            if (strDrcode == "1107" || strDrcode == "1125")
                            {
                                SQL = SQL + ComNum.VBLF + "         AND DRCODE in ('1107','1125') ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "         AND DRCODE not in ( '1107','1125') ";
                            }
                        }

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            strChojae = "과재진";
                        }
                        else
                        {
                            strChojae = "과초진";
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    strEmrSinGu = "";

                    if (dt.Rows[0]["EMRSINGU"].ToString().Trim() == "1")
                    {
                        strEmrSinGu = "(신)";
                    }

                    switch (strBi)
                    {
                        case "11":
                        case "12":
                        case "13":
                            strBi = "보험";
                            break;
                        case "21":
                            strBi = "1종";
                            strPatChk = "OK";
                            break;
                        case "22":
                            strBi = "2종";
                            strPatChk = "OK";
                            break;
                        case "23":
                            strBi = "3종";
                            break;
                        case "24":
                            strBi = "행려";
                            break;
                        case "31":
                            strBi = "산재";
                            break;
                        case "32":
                            strBi = "공상";
                            break;
                        case "33":
                            strBi = "산재공상";
                            break;
                        case "41":
                        case "42":
                        case "43":
                        case "44":
                        case "51":
                            if (strBi == "51") { strBi51 = "Y"; }
                            strBi = "일반";
                            break;
                        case "52":
                            strBi = "TA";
                            break;
                        case "55":
                            strBi = "TA일반";
                            break;
                    }

                    GstrBi51 = strBi51;

                    //진료과명
                    strDeptName = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, strDeptCode);

                    //진료과 마지막진료일자
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(MAX(BDATE),'YYYY-MM-DD') AS BDATE";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "       AND BDATE <> TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + "       AND PANO = '" + strPano + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        strLastDay = dt1.Rows[0]["BDATE"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //시설여부
                    SQL = "";
                    SQL = "SELECT GUBUN FROM " + ComNum.DB_PMPA + "BAS_NAHOMEGAM ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        switch (dt1.Rows[0]["GUBUN"].ToString().Trim())
                        {
                            case "J":
                                strSisel = "나자렛";
                                break;
                            case "N":
                                strSisel = "햇빛";
                                break;
                            case "L":
                                strSisel = "마리아";
                                break;
                            case "M":
                                strSisel = "요양원";
                                break;
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //두과표시 다시 2014-11-20
                    SQL = "";
                    SQL = "SELECT DEPTCODE FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND ACTDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND DEPTCODE NOT IN ('" + strDeptCode + "') ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY DEPTCODE ";

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
                            if (i == 0) { strDeptCnt = "타과 "; }

                            strDeptCnt = strDeptCnt + dt1.Rows[i]["DEPTCODE"].ToString().Trim();
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //당일동명2인 점검함 - 2009-09-28
                    SQL = "";
                    SQL = "SELECT COUNT(PANO) AS CNT FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE = TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + "      AND SNAME ='" + strSName + "' ";
                    SQL = SQL + ComNum.VBLF + "      AND DeptCode ='" + strDeptCode + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        if (VB.Val(dt1.Rows[0]["CNT"].ToString().Trim()) > 1)
                        {
                            strToSName = "[동명인]";
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //희귀V, 암체크
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     MCode,VCode,GbOT,GbSPC,JinDtl,Jin";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE =  TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND Pano ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND DeptCode ='" + strDeptCode + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        if (dt1.Rows[0]["MCODE"].ToString().Trim() == "V0000") { strMCode = "ⓥ"; }
                        if (dt1.Rows[0]["MCODE"].ToString().Trim() == "C0000") { strMCode = "차상위C"; }
                        if (dt1.Rows[0]["MCODE"].ToString().Trim() == "E0000") { strMCode = "차상위E"; strPatChk = "OK"; }
                        if (dt1.Rows[0]["MCODE"].ToString().Trim() == "F0000") { strMCode = "차상위F"; strPatChk = "OK"; }
                        if (dt1.Rows[0]["VCODE"].ToString().Trim() == "V193" || dt1.Rows[0]["VCODE"].ToString().Trim() == "V194") { strVCode = "ⓒ"; }
                        if (dt1.Rows[0]["GBOT"].ToString().Trim() == "Y") { strOT = "Y"; }
                        if (dt1.Rows[0]["GBSPC"].ToString().Trim() == "1") { strGbSPC = "1"; }
                        if (dt1.Rows[0]["JIN"].ToString().Trim() == "5") { strPatChk = "OK"; }
                        if (dt1.Rows[0]["JIN"].ToString().Trim() == "E") { strPatChk = "OK"; }

                        if (dt1.Rows[0]["JINDTL"].ToString().Trim() == "12")
                        {
                            strSC = "★ 금연클리닉대상 ★";
                        }
                        else if (dt1.Rows[0]["JINDTL"].ToString().Trim() == "15" || dt1.Rows[0]["JINDTL"].ToString().Trim() == "16")
                        {
                            strHC = "♥";
                        }

                        if (dt1.Rows[0]["JINDTL"].ToString().Trim() == "22")
                        {
                            strSC = "★ 조산및저체중대상(@F016)";
                        }

                    }

                    dt1.Dispose();
                    dt1 = null;

                    //미시행
                    strNoExe = clsVbfunc.CHECK_EXECUTE_new(clsDB.DbCon, strPano, strDeptCode);

                    strDrname = clsVbfunc.GetBASDoctorName(clsDB.DbCon, strDrcode);

                    //마지막퇴원일자
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     MAX(TO_CHAR(OUTDATE,'YYYY-MM-DD')) AS OUTDATE, WARDCODE ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND GBSTS = '7' ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY WARDCODE ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY OutDate DESC ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count == 0)
                    {
                        dt1.Dispose();
                        dt1 = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        return;
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //emr 영상 존재여부
                    SQL = "";
                    SQL = "SELECT PATID FROM " + ComNum.DB_EMR + "EMR_TREATT ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PATID = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND CLASS = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND CHECKED = '1' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        strEMR2 = "*S*";
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (strSms == "Y")
                    {
                        strErDate = strErDate + " 정보동의(SMS)";
                    }
                    else if (strSms == "N")
                    {
                        strErDate = strErDate + " 동의요청(SMS)";
                    }
                    else if (strSms == "X")
                    {
                        strErDate = strErDate + " 동의거부(SMS)";
                    }
                    else
                    {
                        strErDate = strErDate + " 동의요청(SMS)";
                    }

                    //ER환자내역확인
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     MAX(TO_CHAR(ACTDATE,'YYYY-MM-DD')) AS ACTDATE ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND ACTDATE >= TO_DATE('" + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-15).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND ACTDATE <= TO_DATE('" + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND DEPTCODE = 'ER' ";
                    SQL = SQL + ComNum.VBLF + "         AND REP <> '#' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        strErDate = strErDate + " ER내원 : " + VB.Right(dt1.Rows[0]["ACTDATE"].ToString().Trim(), 5);
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (clsVbfunc.READ_낙상고위험체크_OPD(clsDB.DbCon, strPano, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), intAge.ToString()) == "OK")
                    {
                        strFall = "OK";

                    }

                    if (strDeptCode == "RA")
                    {
                        strDeptCode2 = "MD";
                    }
                    else
                    {
                        strDeptCode2 = strDeptCode;
                    }

                    //바코드 접수증 출력
                    #region 바코드 접수증 출력

                    clsSpread.SpdPrint_Margin setMargin;
                    clsSpread.SpdPrint_Option setOption;

                    clsSpread SP = new clsSpread();

                    #region 출력내용

                    //바코드
                    //ssPrint_Sheet1.Cells[0, 1].Text = strPano + VB.Asc(VB.Left(strDeptCode2, 1)) + VB.Asc(VB.Right(strDeptCode2, 1));

                    BarcodeLib.Barcode b = new BarcodeLib.Barcode();

                    img = b.Encode(BarcodeLib.TYPE.CODE128, strPano + VB.Asc(VB.Left(strDeptCode2, 1)) + VB.Asc(VB.Right(strDeptCode2, 1)), Color.Black, Color.White, 250, 40);
                    ssPrint_Sheet1.Cells[1, 0].Value = img;

                    mstrBaCode = strPano + VB.Asc(VB.Left(strDeptCode2, 1)) + VB.Asc(VB.Right(strDeptCode2, 1));

                    //출력
                    ssPrint_Sheet1.Cells[1, 0].Text = "";
                    ssPrint_Sheet1.Cells[1, 0].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[1, 1].Text = "";
                    ssPrint_Sheet1.Cells[1, 1].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[1, 2].Text = "";
                    ssPrint_Sheet1.Cells[1, 2].Font = new Font("굴림", 9, FontStyle.Regular);

                    ssPrint_Sheet1.Cells[2, 0].Text = "";
                    ssPrint_Sheet1.Cells[2, 0].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[2, 1].Text = "";
                    ssPrint_Sheet1.Cells[2, 1].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[2, 2].Text = "";
                    ssPrint_Sheet1.Cells[2, 2].Font = new Font("굴림", 9, FontStyle.Regular);

                    ssPrint_Sheet1.Cells[3, 0].Text = "";
                    ssPrint_Sheet1.Cells[3, 0].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[3, 1].Text = "";
                    ssPrint_Sheet1.Cells[3, 1].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[3, 2].Text = "";
                    ssPrint_Sheet1.Cells[3, 2].Font = new Font("굴림", 9, FontStyle.Regular);

                    ssPrint_Sheet1.Cells[4, 0].Text = "";
                    ssPrint_Sheet1.Cells[4, 0].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[4, 1].Text = "";
                    ssPrint_Sheet1.Cells[4, 1].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[4, 2].Text = "";
                    ssPrint_Sheet1.Cells[4, 2].Font = new Font("굴림", 9, FontStyle.Regular);

                    ssPrint_Sheet1.Cells[5, 0].Text = "";
                    ssPrint_Sheet1.Cells[5, 0].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[5, 1].Text = "";
                    ssPrint_Sheet1.Cells[5, 1].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[5, 2].Text = "";
                    ssPrint_Sheet1.Cells[5, 2].Font = new Font("굴림", 9, FontStyle.Regular);

                    ssPrint_Sheet1.Cells[6, 0].Text = "";
                    ssPrint_Sheet1.Cells[6, 0].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[6, 1].Text = "";
                    ssPrint_Sheet1.Cells[6, 1].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[6, 2].Text = "";
                    ssPrint_Sheet1.Cells[6, 2].Font = new Font("굴림", 9, FontStyle.Regular);

                    ssPrint_Sheet1.Cells[7, 0].Text = "";
                    ssPrint_Sheet1.Cells[7, 0].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[7, 1].Text = "";
                    ssPrint_Sheet1.Cells[7, 1].Font = new Font("굴림", 9, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[7, 2].Text = "";
                    ssPrint_Sheet1.Cells[7, 2].Font = new Font("굴림", 9, FontStyle.Regular);

                    for (int h = 1; h < 8; i++)
                    {
                        ssPrint_Sheet1.SetRowHeight(h, Convert.ToInt32(ssPrint_Sheet1.GetPreferredRowHeight(h) + 5));
                    }

                    if (ssPrint_Sheet1.GetRowHeight(6) < 40)
                    {
                        ssPrint_Sheet1.SetRowHeight(6, 40);
                    }
                    #endregion

                    //string strTitle = "";
                    string strHeader = "";
                    string strFooter = "";
                    bool PrePrint = false;

                    strHeader = "";
                    strFooter = "";

                    setMargin = new clsSpread.SpdPrint_Margin(75, 5, 5, 10, 5, 10);
                    setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

                    SP.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);

                    #endregion
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnPrintTO_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strPano = GstrPANO;
            string strDeptCode = GstrDeptCode;

            string strDeptCode2 = "";
            string strJepDate = "";
            string strSName = "";
            string strGjJong = "";
            string strSex = "";
            string strNameE = "";
            int intAge = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, PTNO, SNAME, SEX, AGE, GJJONG, B.NAME, A.JEPDATE, A.SDATE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "HEA_JEPSU A , " + ComNum.DB_PMPA + "HEA_EXJONG B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.JEPDATE = TO_DATE('" + GstrDate + "','YYYY-MM-DD' ) ";
                SQL = SQL + ComNum.VBLF + "         AND A.GJJONG = B.CODE";
                SQL = SQL + ComNum.VBLF + "         AND A.PTNO = '" + GstrPANO + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strPano = dt.Rows[0]["PANO"].ToString().Trim();
                    strJepDate = dt.Rows[0]["JEPDATE"].ToString().Trim();
                    strSName = dt.Rows[0]["SNAME"].ToString().Trim();

                    strGjJong = dt.Rows[0]["NAME"].ToString().Trim();
                    intAge = Convert.ToInt32(VB.Val(dt.Rows[0]["AGE"].ToString().Trim()));
                    strSex = dt.Rows[0]["SEX"].ToString().Trim();

                    strNameE = clsVbfunc.GetPatientEName(clsDB.DbCon, GstrPANO, "1");

                    if (strDeptCode == "RA")
                    {
                        strDeptCode2 = "MD";
                    }
                    else
                    {
                        strDeptCode2 = strDeptCode;
                    }

                    //바코드 접수증 출력
                    #region 바코드 접수증 출력

                    clsSpread.SpdPrint_Margin setMargin;
                    clsSpread.SpdPrint_Option setOption;

                    clsSpread SP = new clsSpread();
                    clsPrint CP = new clsPrint();

                    string strPrintName = CP.getPmpaBarCodePrinter("신용카드");

                    if (CP.isPmpaBarCodePrinter(strPrintName) == false)
                    {
                        return;
                    }

                    #region 출력내용

                    //바코드
                    //ssPrint_Sheet1.Cells[1, 0].Text = strPano + strNameE.Trim();

                    BarcodeLib.Barcode b = new BarcodeLib.Barcode();

                    img = b.Encode(BarcodeLib.TYPE.CODE128, strPano + strNameE.Trim().ToUpper(), Color.Black, Color.White, 250, 40);
                    ssPrint_Sheet1.Cells[1, 0].Value = img;

                    mstrBaCode = strPano + strNameE.Trim().ToUpper();

                    //내용

                    ssPrint_Sheet1.RowCount = 10;

                    //ssPrint_Sheet1.Cells[2, 0].Text = "검 사 명:";
                    //ssPrint_Sheet1.Cells[2, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    //ssPrint_Sheet1.Cells[2, 1].Text = strOrderName;
                    //ssPrint_Sheet1.Cells[2, 1].Font = new Font("굴림", 12, FontStyle.Regular);
                    //ssPrint_Sheet1.Cells[2, 1].ColumnSpan = 2;

                    //ssPrint_Sheet1.Cells[3, 0].Text = "등록 번호:";
                    //ssPrint_Sheet1.Cells[3, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    //ssPrint_Sheet1.Cells[3, 1].Text = strPano + " " + strSName;
                    //ssPrint_Sheet1.Cells[3, 1].Font = new Font("굴림", 14, FontStyle.Bold);
                    //ssPrint_Sheet1.Cells[3, 1].ColumnSpan = 2;

                    //ssPrint_Sheet1.Cells[4, 0].Text = "나이(성별): " + Convert.ToString(intAge) + "세(" + strSex + ") 진료과: " + strDeptCode + "(" + strDrCode + ")";
                    //ssPrint_Sheet1.Cells[4, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    //ssPrint_Sheet1.Cells[4, 0].ColumnSpan = 3;

                    //if (strTeam_Tel != "")
                    //{
                    //    ssPrint_Sheet1.Cells[5, 0].Text = "처방일자: " + strBDate + " [" + strTeam_Tel + "]";
                    //}
                    //else
                    //{
                    //    ssPrint_Sheet1.Cells[5, 0].Text = "처방일자: " + strBDate;
                    //}
                    //ssPrint_Sheet1.Cells[5, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    //ssPrint_Sheet1.Cells[5, 0].ColumnSpan = 3;

                    //ssPrint_Sheet1.Cells[6, 0].Text = "입원/외래: " + strGBIO + "(" + strRoomCode + ") " + "주민번호:" + strJumin;
                    //ssPrint_Sheet1.Cells[6, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    //ssPrint_Sheet1.Cells[6, 0].ColumnSpan = 3;

                    //ssPrint_Sheet1.Cells[7, 0].CellType = TextCell;

                    //if (clsVbfunc.READ_낙상고위험체크_OPD(clsDB.DbCon, strPano, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), intAge.ToString()) == "OK")
                    //{
                    //    strRemark = "★낙상★" + ComNum.VBLF + strRemark;
                    //}

                    //if (strRemark.Trim() != "")
                    //{
                    //    ssPrint_Sheet1.Cells[7, 0].Text = "REMARK" + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF + strRemark;
                    //}
                    //else
                    //{
                    //    ssPrint_Sheet1.Cells[7, 0].Text = "REMARK";
                    //}

                    //ssPrint_Sheet1.Cells[7, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    //ssPrint_Sheet1.Cells[7, 0].ColumnSpan = 3;

                    //for (int i = 2; i < 7; i++)
                    //{
                    //    ssPrint_Sheet1.SetRowHeight(i, Convert.ToInt32(ssPrint_Sheet1.GetPreferredRowHeight(i) + 5));
                    //}

                    //ssPrint_Sheet1.SetRowHeight(7, Convert.ToInt32(ssPrint_Sheet1.GetPreferredRowHeight(7) + 20));

                    //if (ssPrint_Sheet1.GetRowHeight(7) < 80)
                    //{
                    //    ssPrint_Sheet1.SetRowHeight(7, 80);
                    //    ssPrint_Sheet1.Cells[7, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                    //}

                    #endregion

                    //string strTitle = "";
                    string strHeader = "";
                    string strFooter = "";
                    bool PrePrint = false;

                    strHeader = "";
                    strFooter = "";

                    setMargin = new clsSpread.SpdPrint_Margin(75, 5, 5, 10, 5, 10);
                    setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

                    SP.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);

                    #endregion
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnEKG_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SqlErr = "";

            string strPano = "";
            string strDeptCode = "";
            string strDeptCode2 = "";
            string strBDate = "";
            string strSName = "";
            string strSex = "";
            string strJumin = "";
            string strNameE = "";
            string strOrderName = "";
            string strDrCode = "";
            string strRoomCode = "";
            string strGBIO = "";
            string strRemark = "";
            string strTeam_Tel = "";

            int intAge = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PTNO, A.SNAME, A.SEX, A.AGE, TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, A.GBIO, A.ROOMCODE, A.DEPTCODE,A.DRCODE, A.REMARK,  B.ORDERNAME,c.Jumin1 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_JUPMST A , " + ComNum.DB_MED + "OCS_ORDERCODE B, " + ComNum.DB_PMPA + "BAS_PATIENT c ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.ROWID = '" + GstrROWID + "' ";
                SQL = SQL + ComNum.VBLF + "         AND a.Ptno=c.Pano ";
                SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE = B.ORDERCODE(+)";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strPano = dt.Rows[0]["PTNO"].ToString().Trim();
                    strBDate = dt.Rows[0]["BDATE"].ToString().Trim();
                    strSName = dt.Rows[0]["SNAME"].ToString().Trim();

                    intAge = Convert.ToInt32(VB.Val(dt.Rows[0]["AGE"].ToString().Trim()));
                    strSex = dt.Rows[0]["SEX"].ToString().Trim();
                    strJumin = dt.Rows[0]["JUMIN1"].ToString().Trim();

                    strNameE = clsVbfunc.GetPatientEName(clsDB.DbCon, strPano, "1");

                    strOrderName = dt.Rows[0]["ORDERNAME"].ToString().Trim();
                    strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    strDrCode = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                    strRoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    strGBIO = VB.IIf(dt.Rows[0]["GBIO"].ToString().Trim() == "O", "외래", "입원").ToString();
                    strRemark = dt.Rows[0]["REMARK"].ToString().Trim();

                    //2014-12-05
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     a.WardCode,a.Tel,a.Team  ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_TEAM a, " + ComNum.DB_PMPA + "NUR_TEAM_ROOMCODE b";
                    SQL = SQL + ComNum.VBLF + "     Where a.WardCode = b.WardCode";
                    SQL = SQL + ComNum.VBLF + "         AND a.TEAM=b.TEAM";
                    SQL = SQL + ComNum.VBLF + "         AND b.RoomCode =" + VB.Val(dt.Rows[0]["ROOMCODE"].ToString().Trim());

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count == 0)
                    {
                        SQL = "";
                        SQL = "SELECT NAME, CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE a, " + ComNum.DB_PMPA + "BAS_ROOM b ";
                        SQL = SQL + ComNum.VBLF + "     WHERE a.Code = b.WardCode(+)";
                        SQL = SQL + ComNum.VBLF + "         AND a.GUBUN = 'ETC_병동전화' ";
                        SQL = SQL + ComNum.VBLF + "         AND b.RoomCode = " + VB.Val(dt.Rows[0]["ROOMCODE"].ToString().Trim());

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt2.Rows.Count > 0)
                        {
                            strTeam_Tel = dt2.Rows[0]["CODE"].ToString().Trim() + " " + dt2.Rows[0]["NAME"].ToString().Trim();
                        }

                        dt2.Dispose();
                        dt2 = null;
                    }
                    else
                    {
                        strTeam_Tel = dt1.Rows[0]["WARDCODE"].ToString().Trim() + " " + dt1.Rows[0]["TEL"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (strDeptCode == "RA")
                    {
                        strDeptCode2 = "MD";
                    }
                    else
                    {
                        strDeptCode2 = strDeptCode;
                    }

                    //바코드 접수증 출력
                    #region 바코드 접수증 출력

                    clsSpread.SpdPrint_Margin setMargin;
                    clsSpread.SpdPrint_Option setOption;

                    clsSpread SP = new clsSpread();

                    #region 출력내용

                    //바코드
                    //ssPrint_Sheet1.Cells[1, 0].Text = strPano + strNameE.Trim();

                    BarcodeLib.Barcode b = new BarcodeLib.Barcode();

                    img = b.Encode(BarcodeLib.TYPE.CODE128, strPano + strNameE.Trim().ToUpper(), Color.Black, Color.White, 250, 40);
                    ssPrint_Sheet1.Cells[1, 0].Value = img;

                    mstrBaCode = strPano + strNameE.Trim().ToUpper();

                    //내용

                    ssPrint_Sheet1.RowCount = 2;
                    ssPrint_Sheet1.RowCount = 8;

                    ssPrint_Sheet1.Cells[2, 0].Text = "검 사 명:";
                    ssPrint_Sheet1.Cells[2, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[2, 1].Text = strOrderName;
                    ssPrint_Sheet1.Cells[2, 1].Font = new Font("굴림", 12, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[2, 1].ColumnSpan = 2;

                    ssPrint_Sheet1.Cells[3, 0].Text = "등록 번호:";
                    ssPrint_Sheet1.Cells[3, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[3, 1].Text = strPano + " " + strSName;
                    ssPrint_Sheet1.Cells[3, 1].Font = new Font("굴림", 14, FontStyle.Bold);
                    ssPrint_Sheet1.Cells[3, 1].ColumnSpan = 2;

                    ssPrint_Sheet1.Cells[4, 0].Text = "나이(성별): " + Convert.ToString(intAge) + "세(" + strSex + ") 진료과: " + strDeptCode + "(" + strDrCode + ")";
                    ssPrint_Sheet1.Cells[4, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[4, 0].ColumnSpan = 3;

                    if (strTeam_Tel != "")
                    {
                        ssPrint_Sheet1.Cells[5, 0].Text = "처방일자: " + strBDate + " [" + strTeam_Tel + "]";
                    }
                    else
                    {
                        ssPrint_Sheet1.Cells[5, 0].Text = "처방일자: " + strBDate;
                    }
                    ssPrint_Sheet1.Cells[5, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[5, 0].ColumnSpan = 3;

                    ssPrint_Sheet1.Cells[6, 0].Text = "입원/외래: " + strGBIO + "(" + strRoomCode + ") " + "주민번호:" + strJumin;
                    ssPrint_Sheet1.Cells[6, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[6, 0].ColumnSpan = 3;

                    ssPrint_Sheet1.Cells[7, 0].CellType = TextCell;

                    if (clsVbfunc.READ_낙상고위험체크_OPD(clsDB.DbCon, strPano, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), intAge.ToString()) == "OK")
                    {
                        strRemark = "★낙상★" + ComNum.VBLF + strRemark;
                    }

                    if (strRemark.Trim() != "")
                    {
                        ssPrint_Sheet1.Cells[7, 0].Text = "REMARK" + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF + strRemark;
                    }
                    else
                    {
                        ssPrint_Sheet1.Cells[7, 0].Text = "REMARK";
                    }

                    ssPrint_Sheet1.Cells[7, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[7, 0].ColumnSpan = 3;

                    for (int i = 2; i < 7; i++)
                    {
                        ssPrint_Sheet1.SetRowHeight(i, Convert.ToInt32(ssPrint_Sheet1.GetPreferredRowHeight(i) + 5));
                    }

                    ssPrint_Sheet1.SetRowHeight(7, Convert.ToInt32(ssPrint_Sheet1.GetPreferredRowHeight(7) + 20));

                    if (ssPrint_Sheet1.GetRowHeight(7) < 80)
                    {
                        ssPrint_Sheet1.SetRowHeight(7, 80);
                        ssPrint_Sheet1.Cells[7, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                    }

                    #endregion

                    //string strTitle = "";
                    string strHeader = "";
                    string strFooter = "";
                    bool PrePrint = false;

                    strHeader = "";
                    strFooter = "";

                    setMargin = new clsSpread.SpdPrint_Margin(75, 5, 5, 10, 5, 10);
                    setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, false, false, true, true, true, false, false);

                    SP.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);

                    #endregion
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnEKG2_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SqlErr = "";

            string strPano = "";
            string strDeptCode = "";

            string strDeptCode2 = "";
            string strBDate = "";
            string strSName = "";
            string strTemp = "";
            string strH = "";
            string strW = "";
            string strB1 = "";
            string strB2 = "";
            string strSex = "";
            string strJumin = "";
            string strNameE = "";
            string strOrderName = "";
            string strDrCode = "";
            string strRoomCode = "";
            string strGBIO = "";
            string strRemark = "";
            string strTeam_Tel = "";

            int intAge = 0;

            //try
            //{
            SQL = "";
            SQL = "SELECT";
            SQL = SQL + ComNum.VBLF + "     A.PTNO, A.SNAME, A.SEX, A.AGE, TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, A.GBIO, A.ROOMCODE, A.DEPTCODE,A.DRCODE, A.REMARK, B.ORDERNAME,c.Jumin1 ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_JUPMST A , " + ComNum.DB_MED + "OCS_ORDERCODE B, " + ComNum.DB_PMPA + "BAS_PATIENT c ";
            SQL = SQL + ComNum.VBLF + "     WHERE A.ROWID = '" + GstrROWID + "' ";
            SQL = SQL + ComNum.VBLF + "         AND a.Ptno = c.Pano ";
            SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE = B.ORDERCODE(+)";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                strPano = dt.Rows[0]["PTNO"].ToString().Trim();
                strBDate = dt.Rows[0]["BDATE"].ToString().Trim();
                strSName = dt.Rows[0]["SNAME"].ToString().Trim();

                strTemp = get_vital_Data(strPano);
                strH = VB.Pstr(strTemp, "^^", 1);
                strW = VB.Pstr(strTemp, "^^", 2);
                strB1 = VB.Pstr(strTemp, "^^", 3);
                strB2 = VB.Pstr(strTemp, "^^", 4);

                intAge = Convert.ToInt32(VB.Val(dt.Rows[0]["AGE"].ToString().Trim()));
                strSex = dt.Rows[0]["SEX"].ToString().Trim();
                strJumin = dt.Rows[0]["JUMIN1"].ToString().Trim();
                strNameE = clsVbfunc.GetPatientEName(clsDB.DbCon, strPano, "1");
                strOrderName = dt.Rows[0]["ORDERNAME"].ToString().Trim();
                strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                strDrCode = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                strRoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                strGBIO = VB.IIf(dt.Rows[0]["GBIO"].ToString().Trim() == "O", "외래", "입원").ToString();
                strRemark = dt.Rows[0]["REMARK"].ToString().Trim();

                //2014-12-05
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     a.WardCode,a.Tel,a.Team  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_TEAM a, " + ComNum.DB_PMPA + "NUR_TEAM_ROOMCODE b";
                SQL = SQL + ComNum.VBLF + "     Where a.WardCode = b.WardCode";
                SQL = SQL + ComNum.VBLF + "         AND a.TEAM = b.TEAM";
                SQL = SQL + ComNum.VBLF + "         AND b.RoomCode = " + VB.Val(dt.Rows[0]["ROOMCODE"].ToString().Trim());

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt1.Rows.Count == 0)
                {
                    SQL = "";
                    SQL = "SELECT NAME, CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE a, " + ComNum.DB_PMPA + "BAS_ROOM b ";
                    SQL = SQL + ComNum.VBLF + "     WHERE a.Code = b.WardCode(+)";
                    SQL = SQL + ComNum.VBLF + "         AND a.GUBUN = 'ETC_병동전화' ";
                    SQL = SQL + ComNum.VBLF + "         AND b.RoomCode = " + VB.Val(dt.Rows[0]["ROOMCODE"].ToString().Trim());

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        strTeam_Tel = dt2.Rows[0]["CODE"].ToString().Trim() + " " + dt2.Rows[0]["NAME"].ToString().Trim();
                    }

                    dt2.Dispose();
                    dt2 = null;
                }
                else
                {
                    strTeam_Tel = dt1.Rows[0]["WARDCODE"].ToString().Trim() + " " + dt1.Rows[0]["TEL"].ToString().Trim();
                }

                dt1.Dispose();
                dt1 = null;

                if (strDeptCode == "RA")
                {
                    strDeptCode2 = "MD";
                }
                else
                {
                    strDeptCode2 = strDeptCode;
                }

                //바코드 접수증 출력
                #region 바코드 접수증 출력

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                clsSpread SP = new clsSpread();

                #region 출력내용

                //바코드
                //ssPrint_Sheet1.Cells[1, 0].Text = strPano + strNameE.Trim();

                BarcodeLib.Barcode b = new BarcodeLib.Barcode();

                img = b.Encode(BarcodeLib.TYPE.CODE128, strPano + strNameE.Trim().ToUpper(), Color.Black, Color.White, 250, 40);

                ssPrint_Sheet1.Cells[1, 0].Value = img;

                mstrBaCode = strPano + strNameE.Trim().ToUpper();

                //내용

                ssPrint_Sheet1.RowCount = 2;
                ssPrint_Sheet1.RowCount = 13;

                ssPrint_Sheet1.Cells[2, 0].Text = "검 사 명:";
                ssPrint_Sheet1.Cells[2, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                ssPrint_Sheet1.Cells[2, 1].Text = strOrderName;
                ssPrint_Sheet1.Cells[2, 1].Font = new Font("굴림", 14, FontStyle.Bold);
                ssPrint_Sheet1.Cells[2, 1].ColumnSpan = 2;

                ssPrint_Sheet1.Cells[3, 0].Text = "등록 번호:";
                ssPrint_Sheet1.Cells[3, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                ssPrint_Sheet1.Cells[3, 1].Text = strPano + " " + strSName;
                ssPrint_Sheet1.Cells[3, 1].Font = new Font("굴림", 14, FontStyle.Bold);
                ssPrint_Sheet1.Cells[3, 1].ColumnSpan = 2;

                ssPrint_Sheet1.Cells[4, 0].Text = "나이(성별): " + Convert.ToString(intAge) + "세(" + strSex + ") 진료과: " + strDeptCode + "(" + strDrCode + ")";
                ssPrint_Sheet1.Cells[4, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                ssPrint_Sheet1.Cells[4, 0].ColumnSpan = 3;

                if (strTeam_Tel != "")
                {
                    ssPrint_Sheet1.Cells[5, 0].Text = "처방일자: " + strBDate + " [" + strTeam_Tel + "]";
                }
                else
                {
                    ssPrint_Sheet1.Cells[5, 0].Text = "처방일자: " + strBDate;
                }
                ssPrint_Sheet1.Cells[5, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                ssPrint_Sheet1.Cells[5, 0].ColumnSpan = 3;

                ssPrint_Sheet1.Cells[6, 0].Text = "입원/외래: " + strGBIO + "(" + strRoomCode + ") " + "주민번호:" + strJumin;
                ssPrint_Sheet1.Cells[6, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                ssPrint_Sheet1.Cells[6, 0].ColumnSpan = 3;


                ssPrint_Sheet1.Cells[7, 0].Border = new ComplexBorder(null, new ComplexBorderSide(ComplexBorderSideStyle.DoubleLine, Color.Black), null, null);

                if (clsVbfunc.READ_낙상고위험체크_OPD(clsDB.DbCon, strPano, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), intAge.ToString()) == "OK")
                {
                    strRemark = "★낙상★" + ComNum.VBLF + strRemark;
                }

                ssPrint_Sheet1.Cells[7, 0].CellType = TextCell;

                if (strRemark.Trim() != "")
                {
                    ssPrint_Sheet1.Cells[7, 0].Text = "REMARK" + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF + strRemark;
                }
                else
                {
                    ssPrint_Sheet1.Cells[7, 0].Text = "REMARK";
                }

                ssPrint_Sheet1.Cells[7, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                ssPrint_Sheet1.Cells[7, 0].ColumnSpan = 3;



                ssPrint_Sheet1.Cells[8, 0].Text = "Bed / 휠체어";
                ssPrint_Sheet1.Cells[8, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                ssPrint_Sheet1.Cells[8, 0].ColumnSpan = 3;


                if (strGBIO == "입원" && strH != "" && strW != "")
                {
                    ssPrint_Sheet1.Cells[9, 0].Text = "키/몸무게:" + strH + "/" + strW; ;
                }
                else
                {
                    ssPrint_Sheet1.Cells[9, 0].Text = "키/몸무게:";
                }
                ssPrint_Sheet1.Cells[9, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                ssPrint_Sheet1.Cells[9, 0].ColumnSpan = 3;

                ssPrint_Sheet1.Cells[10, 0].Text = "병동으로 연락 / 보호자있음 ";
                ssPrint_Sheet1.Cells[10, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                ssPrint_Sheet1.Cells[10, 0].ColumnSpan = 3;

                ssPrint_Sheet1.Cells[11, 0].Text = "사용금지 Arm";
                ssPrint_Sheet1.Cells[11, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                ssPrint_Sheet1.Cells[11, 0].ColumnSpan = 3;

                ssPrint_Sheet1.Cells[12, 0].Text = "혈압:" + strB1 + "/" + strB2;
                ssPrint_Sheet1.Cells[12, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                ssPrint_Sheet1.Cells[12, 0].ColumnSpan = 3;



                for (int i = 2; i < 13; i++)
                {
                    ssPrint_Sheet1.SetRowHeight(i, Convert.ToInt32(ssPrint_Sheet1.GetPreferredRowHeight(i) + 5));
                }

                ssPrint_Sheet1.SetRowHeight(7, Convert.ToInt32(ssPrint_Sheet1.GetPreferredRowHeight(7) + 20));

                if (ssPrint_Sheet1.GetRowHeight(7) < 80)
                {
                    ssPrint_Sheet1.SetRowHeight(7, 80);
                    ssPrint_Sheet1.Cells[7, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                }

                #endregion

                //string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                strHeader = "";
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(75, 5, 5, 10, 5, 10);
                setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

                SP.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);

                #endregion
            }

            dt.Dispose();
            dt = null;
            //}
            //catch (Exception ex)
            //{
            //    if (dt != null)
            //    {
            //        dt.Dispose();
            //        dt = null;
            //    }

            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //}
        }

        private string get_vital_Data(string strPano)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "^^^^";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     MAX(CHARTDate), extractValue(chartxml, '//it11') AS it11, extractValue(chartxml, '//it10') AS it10";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "     WHERE EMRNO IN (";
                SQL = SQL + ComNum.VBLF + "             SELECT EMRNO FROM " + ComNum.DB_EMR + "EMRXMLMST ";
                SQL = SQL + ComNum.VBLF + "                 WHERE FORMNO = 1562 ";
                SQL = SQL + ComNum.VBLF + "                     AND PTNO = '" + strPano + "'  ";
                SQL = SQL + ComNum.VBLF + "                     AND CHARTDATE <= '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "') ";
                SQL = SQL + ComNum.VBLF + "         AND extractValue(chartxml, '//it11') is not null";
                SQL = SQL + ComNum.VBLF + "GROUP BY extractValue(chartxml, '//it11'),extractValue(chartxml, '//it10')  ";
                //SQL = SQL + ComNum.VBLF + "ORDER BY 1 desc ";

                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     MAX(CHARTDate), R.ITEMVALUE AS it11, R2.ITEMVALUE AS it10";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
                SQL = SQL + ComNum.VBLF + "      ON A.EMRNO    = R.EMRNO";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNOHIS = R.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "     AND R.ITEMNO = 'I0000000002'";
                SQL = SQL + ComNum.VBLF + "     AND R.ITEMVALUE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R2";
                SQL = SQL + ComNum.VBLF + "      ON A.EMRNO    = R2.EMRNO";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNOHIS = R2.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "     AND R2.ITEMNO = 'I0000000418'";
                SQL = SQL + ComNum.VBLF + "     AND R2.ITEMVALUE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "WHERE FORMNO = 3150 ";
                SQL = SQL + ComNum.VBLF + "  AND PTNO   = '" + strPano + "'  ";
                SQL = SQL + ComNum.VBLF + "  AND CHARTDATE <= '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY R.ITEMVALUE, R2.ITEMVALUE";
                SQL = SQL + ComNum.VBLF + "ORDER BY 1 desc ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["it11"].ToString().Trim() + "^^" + dt.Rows[0]["it10"].ToString().Trim() + "^^";
                }

                dt.Dispose();
                dt = null;

                //혈압
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     MAX(CHARTDate), extractValue(chartxml, '//it3') AS it3 , extractValue(chartxml, '//it4') AS it4  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXML  ";
                SQL = SQL + ComNum.VBLF + "     WHERE EMRNO IN (";
                SQL = SQL + ComNum.VBLF + "             SELECT EMRNO FROM ADMIN.EMRXMLMST ";
                SQL = SQL + ComNum.VBLF + "                 WHERE FORMNO = 1562 ";
                SQL = SQL + ComNum.VBLF + "                     AND PTNO = '" + strPano + "'  ";
                SQL = SQL + ComNum.VBLF + "                     AND CHARTDATE <= '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "') ";
                SQL = SQL + ComNum.VBLF + "         AND extractValue(chartxml, '//it3') is not null";
                SQL = SQL + ComNum.VBLF + "GROUP BY extractValue(chartxml, '//it3'),extractValue(chartxml, '//it4')  ";
                //SQL = SQL + ComNum.VBLF + "ORDER BY 1 desc ";


                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     MAX(CHARTDate), R.ITEMVALUE AS it3, R2.ITEMVALUE AS it4";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
                SQL = SQL + ComNum.VBLF + "      ON A.EMRNO    = R.EMRNO";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNOHIS = R.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "     AND R.ITEMNO = 'I0000002018'";
                SQL = SQL + ComNum.VBLF + "     AND R.ITEMVALUE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R2";
                SQL = SQL + ComNum.VBLF + "      ON A.EMRNO    = R2.EMRNO";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNOHIS = R2.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "     AND R2.ITEMNO = 'I0000001765'";
                SQL = SQL + ComNum.VBLF + "     AND R2.ITEMVALUE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "WHERE FORMNO = 3150 ";
                SQL = SQL + ComNum.VBLF + "  AND PTNO   = '" + strPano + "'  ";
                SQL = SQL + ComNum.VBLF + "  AND CHARTDATE <= '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY R.ITEMVALUE, R2.ITEMVALUE";
                SQL = SQL + ComNum.VBLF + "ORDER BY 1 desc ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = rtnVal + dt.Rows[0]["it3"].ToString().Trim() + "^^" + dt.Rows[0]["it4"].ToString().Trim() + "^^";
                }
                else
                {
                    rtnVal = rtnVal + "^^^^";
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장

                return rtnVal;
            }
        }

        private void btnEcho_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SqlErr = "";

            string strPano = "";
            string strDeptCode = "";

            string strDeptCode2 = "";
            string strEkg = "";
            string strChest = "";
            string strEkgDate = "";
            string strBDate = "";
            string strTemp = "";
            string strH = "";
            string strW = "";
            string strGubun = "";
            string strRDate = "";
            string strSName = "";
            string strSex = "";
            string strJumin = "";
            string strNameE = "";
            string strOrderName = "";
            string strDrCode = "";
            string strRoomCode = "";
            string strGBIO = "";
            string strRemark = "";
            string strTeam_Tel = "";
            string strORDate = "";

            int intAge = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PTNO, A.SNAME, A.SEX, A.AGE, TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, A.GBIO,a.Gubun, A.ROOMCODE, A.DEPTCODE,A.DRCODE, A.REMARK,  B.ORDERNAME,c.Jumin1, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') RDate ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_JUPMST A , " + ComNum.DB_MED + "OCS_ORDERCODE B, " + ComNum.DB_PMPA + "BAS_PATIENT c ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.ROWID = '" + GstrROWID + "' ";
                SQL = SQL + ComNum.VBLF + "         AND a.Ptno = c.Pano ";
                SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE = B.ORDERCODE(+)";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strPano = dt.Rows[0]["PTNO"].ToString().Trim();
                    strBDate = dt.Rows[0]["BDATE"].ToString().Trim();

                    strTemp = get_vital_Data(strPano);
                    strH = VB.Pstr(strTemp, "^^", 1);
                    strW = VB.Pstr(strTemp, "^^", 2);

                    strGubun = dt.Rows[0]["GUBUN"].ToString().Trim();
                    strRDate = dt.Rows[0]["RDATE"].ToString().Trim();
                    strSName = dt.Rows[0]["SNAME"].ToString().Trim();
                    intAge = Convert.ToInt32(VB.Val(dt.Rows[0]["AGE"].ToString().Trim()));
                    strSex = dt.Rows[0]["SEX"].ToString().Trim();
                    strJumin = dt.Rows[0]["Jumin1"].ToString().Trim();

                    strNameE = clsVbfunc.GetPatientEName(clsDB.DbCon, strPano, "1");

                    strOrderName = dt.Rows[0]["ORDERNAME"].ToString().Trim();
                    strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    strDrCode = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                    strRoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    strGBIO = VB.IIf(dt.Rows[0]["GBIO"].ToString().Trim() == "O", "외래", "입원").ToString();
                    strRemark = dt.Rows[0]["REMARK"].ToString().Trim();

                    //2014-12-05
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     a.WardCode,a.Tel,a.Team  ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_TEAM a, " + ComNum.DB_PMPA + "NUR_TEAM_ROOMCODE b";
                    SQL = SQL + ComNum.VBLF + "     Where a.WardCode = b.WardCode";
                    SQL = SQL + ComNum.VBLF + "         AND a.TEAM = b.TEAM";
                    SQL = SQL + ComNum.VBLF + "         AND b.RoomCode = " + VB.Val(dt.Rows[0]["ROOMCODE"].ToString().Trim());

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count == 0)
                    {
                        SQL = "";
                        SQL = "SELECT NAME, CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE a, " + ComNum.DB_PMPA + "BAS_ROOM b ";
                        SQL = SQL + ComNum.VBLF + "     WHERE a.Code = b.WardCode(+)";
                        SQL = SQL + ComNum.VBLF + "         AND a.GUBUN = 'ETC_병동전화' ";
                        SQL = SQL + ComNum.VBLF + "         AND b.RoomCode = " + VB.Val(dt.Rows[0]["ROOMCODE"].ToString().Trim());

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt2.Rows.Count > 0)
                        {
                            strTeam_Tel = dt2.Rows[0]["CODE"].ToString().Trim() + " " + dt2.Rows[0]["NAME"].ToString().Trim();
                        }

                        dt2.Dispose();
                        dt2 = null;
                    }
                    else
                    {
                        strTeam_Tel = dt1.Rows[0]["WARDCODE"].ToString().Trim() + " " + dt1.Rows[0]["TEL"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //CHEST 체크
                    SQL = "";
                    SQL = "SELECT ROWID FROM " + ComNum.DB_PMPA + "XRAY_DETAIL";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND SEEKDATE >= TRUNC(SYSDATE-180) ";
                    SQL = SQL + ComNum.VBLF + "         AND SEEKDATE < TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + "         AND XJONG = '1'";
                    SQL = SQL + ComNum.VBLF + "         AND XSUBCODE = '01' ";
                    SQL = SQL + ComNum.VBLF + "         AND GBRESERVED = '7' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        strChest = "√";
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //EKG체크
                    SQL = "";
                    SQL = "SELECT ROWID FROM " + ComNum.DB_MED + "ETC_JUPMST";
                    SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND RDATE >= TRUNC(SYSDATE-180) ";
                    SQL = SQL + ComNum.VBLF + "         AND RDATE < TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + "         AND GUBUN = '1' ";
                    SQL = SQL + ComNum.VBLF + "         AND GBJOB = '3' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        strEkg = "√";
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //EKG체크
                    if (strGubun != "")
                    {
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     MAX(TO_CHAR(RDate,'YYYY-MM-DD')) AS MRDate ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_JUPMST";
                        SQL = SQL + ComNum.VBLF + "     WHERE PTNO ='" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND RDATE >= TRUNC(SYSDATE-365) ";
                        SQL = SQL + ComNum.VBLF + "         AND RDAte < TRUNC(SYSDATE)";
                        SQL = SQL + ComNum.VBLF + "         AND GUBUN IN ( '" + strGubun + "' ) ";
                        SQL = SQL + ComNum.VBLF + "         AND GBJOB = '3' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            strEkgDate = dt1.Rows[0]["MRDATE"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    //외래 MC 마지막 예약체크
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') AS DATE3 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND (TRANSDATE IS NULL OR TRUNC(TRANSDATE) = TRUNC(SYSDATE))";
                    SQL = SQL + ComNum.VBLF + "         AND RETDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "         AND DEPTCODE = 'MC' ";   //심장내과
                    SQL = SQL + ComNum.VBLF + "ORDER BY DATE3 DESC ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        strORDate = dt1.Rows[0]["DATE3"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (strDeptCode == "RA")
                    {
                        strDeptCode2 = "MD";
                    }
                    else
                    {
                        strDeptCode2 = strDeptCode;
                    }

                    //바코드 접수증 출력
                    #region 바코드 접수증 출력

                    clsSpread.SpdPrint_Margin setMargin;
                    clsSpread.SpdPrint_Option setOption;

                    clsSpread SP = new clsSpread();

                    #region 출력내용

                    //바코드
                    //ssPrint_Sheet1.Cells[1, 0].Text = strPano + strNameE.Trim();

                    BarcodeLib.Barcode b = new BarcodeLib.Barcode();

                    //250, 40

                    img = b.Encode(BarcodeLib.TYPE.CODE128, strPano + strNameE.Trim().ToUpper(), Color.Black, Color.White, 255, 40);
                    ssPrint_Sheet1.Cells[1, 0].Value = img;

                    mstrBaCode = strPano + strNameE.Trim().ToUpper();

                    //출력

                    ssPrint_Sheet1.RowCount = 2;
                    ssPrint_Sheet1.RowCount = 10;

                    ssPrint_Sheet1.Cells[2, 0].Text = "검 사 명:";
                    ssPrint_Sheet1.Cells[2, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[2, 1].Text = strOrderName;
                    ssPrint_Sheet1.Cells[2, 1].Font = new Font("굴림", 12, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[2, 1].ColumnSpan = 2;

                    ssPrint_Sheet1.Cells[3, 0].Text = "등록 번호:";
                    ssPrint_Sheet1.Cells[3, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[3, 1].Text = strPano + " " + strSName;
                    ssPrint_Sheet1.Cells[3, 1].Font = new Font("굴림", 14, FontStyle.Bold);
                    ssPrint_Sheet1.Cells[3, 1].ColumnSpan = 2;

                    if (strDeptCode == "MC" || strGBIO == "외래")
                    {
                        ssPrint_Sheet1.Cells[4, 0].Text = "나이(성별):" + Convert.ToString(intAge) + "세(" + strSex + ") 진료과: " + strDeptCode + "(" + strDrCode + ")";
                    }
                    else
                    {
                        //'심장내과 아니면
                        ssPrint_Sheet1.Cells[4, 0].Text = "나이(성별):" + Convert.ToString(intAge) + "세(" + strSex + ") consult:" + strDeptCode + "(" + strDrCode + ")";
                    }
                    ssPrint_Sheet1.Cells[4, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[4, 0].ColumnSpan = 3;

                    if (strGBIO == "외래")
                    {
                        //   Call Text_Print("굴림", 10, "", 2200, 180, )
                        ssPrint_Sheet1.Cells[5, 0].Text = "외래       " + "주민번호:" + strJumin;
                    }
                    else
                    {
                        if (strTeam_Tel != "")
                        {
                            ssPrint_Sheet1.Cells[5, 0].Text = "입원:" + strRoomCode + "호(" + strTeam_Tel + ") " + "주민번호:" + strJumin;
                        }
                        else
                        {
                            ssPrint_Sheet1.Cells[5, 0].Text = "입원:" + strRoomCode + "호 " + "주민번호:" + strJumin;
                        }
                    }

                    ssPrint_Sheet1.Cells[5, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[5, 0].ColumnSpan = 3;

                    ssPrint_Sheet1.Cells[6, 0].Text = "처방일자: " + strBDate
                                      + ComNum.VBLF + "최종검사: " + strEkgDate  //'해당검사의 오늘이전 마지막검사
                                      + ComNum.VBLF + "검사예약: " + strRDate  //'현재예약시간
                                      + ComNum.VBLF + "진료예약: " + strORDate;   //'외래예약시간
                    ssPrint_Sheet1.Cells[6, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[6, 0].ColumnSpan = 3;

                    ssPrint_Sheet1.Cells[7, 0].Text = "키:" + strH + " " + "cm  몸무게:" + strW + "  kg";
                    ssPrint_Sheet1.Cells[7, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[7, 0].ColumnSpan = 3;

                    ssPrint_Sheet1.Cells[8, 0].Text = "EKG( " + strEkg + " )" + "  CHEST PA (" + strChest + "  )";
                    ssPrint_Sheet1.Cells[8, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[8, 0].ColumnSpan = 3;

                    ssPrint_Sheet1.Cells[9, 0].CellType = TextCell;

                    if (strRemark.Trim() != "")
                    {
                        ssPrint_Sheet1.Cells[9, 0].Text = "REMARK" + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF + strRemark;
                    }
                    else
                    {
                        ssPrint_Sheet1.Cells[9, 0].Text = "REMARK";
                    }

                    ssPrint_Sheet1.Cells[9, 0].Font = new Font("굴림", 10, FontStyle.Regular);
                    ssPrint_Sheet1.Cells[9, 0].ColumnSpan = 3;

                    for (int i = 2; i < 9; i++)
                    {
                        ssPrint_Sheet1.SetRowHeight(i, Convert.ToInt32(ssPrint_Sheet1.GetPreferredRowHeight(i) + 5));
                    }

                    ssPrint_Sheet1.SetRowHeight(9, Convert.ToInt32(ssPrint_Sheet1.GetPreferredRowHeight(9) + 20));

                    if (ssPrint_Sheet1.GetRowHeight(9) < 80)
                    {
                        ssPrint_Sheet1.SetRowHeight(9, 80);
                        ssPrint_Sheet1.Cells[9, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                    }

                    #endregion

                    //string strTitle = "";
                    string strHeader = "";
                    string strFooter = "";
                    bool PrePrint = false;

                    strHeader = "";
                    strFooter = "";

                    setMargin = new clsSpread.SpdPrint_Margin(75, 5, 5, 10, 5, 10);
                    setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

                    SP.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);

                    #endregion
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_BAS_SR_Name2(string strPano)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            string rtnVal = "";

            string strJumin1 = "";
            string strJumin2 = "";
            string strJumin3 = "";
            string strSex = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JUMIN1,JUMIN2,JUMIN3,SEX";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    strJumin3 = dt.Rows[0]["JUMIN3"].ToString().Trim();

                    if (strJumin3 != "")
                    {
                        strJumin2 = clsAES.DeAES(strJumin3);
                    }
                    else
                    {
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }

                    strSex = dt.Rows[0]["SEX"].ToString().Trim();

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     GAMJUMIN, GAMSABUN, GAMGUBUN, GAMENTER, GAMOUT, GAMEND, GAMMESSAGE, GAMNAME, GAMSOSOK, GAMCODE, GAMJUMIN3, Pano ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_GAMF ";
                    SQL = SQL + ComNum.VBLF + "     WHERE (GAMJUMIN ='" + strJumin1 + strJumin2 + "'  OR GAMJUMIN ='" + clsAES.AES(strJumin1 + strJumin2) + "' ) ";
                    SQL = SQL + ComNum.VBLF + "         AND GamCode IN ('11','12','13','14') ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        switch (strSex)
                        {
                            case "M":
                                if (VB.I(dt1.Rows[0]["GAMMESSAGE"].ToString().Trim(), "신부") > 1 || VB.I(dt1.Rows[0]["GAMNAME"].ToString().Trim(), "신부") > 1)
                                {
                                    rtnVal = "Fr";
                                }
                                break;
                            case "F":
                                if (VB.I(dt1.Rows[0]["GAMMESSAGE"].ToString().Trim(), "수녀") > 1 || VB.I(dt1.Rows[0]["GAMNAME"].ToString().Trim(), "수녀") > 1)
                                {
                                    rtnVal = "Sr";
                                }
                                break;
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장

                return rtnVal;
            }
        }

        private void ssPrint_PrintHeaderFooterArea(object sender, FarPoint.Win.Spread.PrintHeaderFooterAreaEventArgs e)
        {
            if (e.IsHeader == true)
            {
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                e.Graphics.DrawImage(img, 5, 0);
                e.Graphics.DrawString(mstrBaCode, new Font("굴림", 8, FontStyle.Bold), Brushes.Black, 0, 60);
            }
        }

        private void print_IpdResv(string strPrintName)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strSName = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strBi = "";
            string strAge = "";
            string strSex = "";

            string strDrg = "N";
            string strDSC = "N";
            string strCAG = "N";
            string strWard = "";

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT JUMIN1, JUMIN2, SEX, SName, Bi ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + GstrPANO + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strSName = dt.Rows[0]["SName"].ToString().Trim();
                    strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
                    strSex = dt.Rows[0]["SEX"].ToString().Trim();
                    strAge = clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, GstrPANO);
                    strBi = dt.Rows[0]["BI"].ToString().Trim();
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT BI,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, AGE";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + GstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + GstrDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = " + ComFunc.ConvOraToDate(GstrDate, "D");
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    strAge = dt.Rows[0]["AGE"].ToString().Trim();
                    strBi = dt.Rows[0]["BI"].ToString().Trim();

                    if (strBi == "11" || strBi == "12" || strBi == "13")
                    {
                        strBi = "보험";
                    }
                    else if (strBi == "21")
                    {
                        strBi = "1종";
                    }
                    else if (strBi == "22")
                    {
                        strBi = "2종";
                    }
                    else if (strBi == "23")
                    {
                        strBi = "3종";
                    }
                    else if (strBi == "24")
                    {
                        strBi = "행려";
                    }
                    else if (strBi == "31" || strBi == "32")
                    {
                        strBi = "산재";
                    }
                    else if (strBi == "41" || strBi == "42" || strBi == "43" || strBi == "44" || strBi == "51")
                    {
                        strBi = "일반";
                    }
                    else if (strBi == "52" || strBi == "55")
                    {
                        strBi = "TA";
                    }
                }


                // 입원일
                SQL = " SELECT TO_CHAR(REDATE,'YYYY-MM-DD') RDATE  ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.IPD_RESERVED ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + GstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode ='" + GstrDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SDate =TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND (GbChk IS NULL OR GbChk <>'1')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    GstrRDate = dt.Rows[0]["RDATE"].ToString().Trim();
                }


                // 60병동 체크 ==> 40병동 체크 2018-12-22
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT GbWard FROM ADMIN.IPD_RESERVED ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + GstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + "    AND REDATE = TO_DATE('" + GstrRDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE = '" + GstrDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DRCODE= '" + GstrDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "    AND GBWARD = '40'";      //2018-12-22 40병동일 경우
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["GbWard"].ToString().Trim() != "")
                    {
                        strWard = dt.Rows[0]["GbWard"].ToString().Trim();
                    }
                }

                //'DRG CHK
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUCODE FROM ADMIN.OCS_OORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + GstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + GstrDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = " + ComFunc.ConvOraToDate(GstrDate, "D");
                SQL = SQL + ComNum.VBLF + "   AND SUCODE ='$$DRG' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strDrg = "Y";
                }



                if (GstrJob != "")
                {
                    strDrg = GstrJob.Split('/')[0];
                    strDSC = GstrJob.Split('/')[1];
                    strCAG = GstrJob.Split('/')[2];
                }


                if (strSex == "F") { strSex = "(여)"; } else { strSex = "(남)"; }

                ssIpdRegPrint.ActiveSheet.Cells[0, 1].Text = GstrPANO + VB.Asc(VB.Left(GstrDeptCode, 1)) + VB.Asc(VB.Right(GstrDeptCode, 1)); //바코드
                ssIpdRegPrint.ActiveSheet.Cells[1, 1].Text = CF.READ_DEPTNAMEK(clsDB.DbCon, GstrDeptCode) + "(" + CF.READ_DrName(clsDB.DbCon, GstrDrCode) + ")"; //진료과/의사명
                ssIpdRegPrint.ActiveSheet.Cells[2, 1].Text = GstrPANO + " " + strSName; //등록번호/성명
                ssIpdRegPrint.ActiveSheet.Cells[3, 1].Text = strJumin1 + "-" + VB.Left(strJumin2, 1) + "   나이: " + strAge + "세" + strSex;
                ssIpdRegPrint.ActiveSheet.Cells[4, 1].Text = string.Format("{0:yyyy/MM/dd}", GstrDate) + " 종류:" + strBi; //진료일자/자격 
                ssIpdRegPrint.ActiveSheet.Cells[5, 1].Text = CF.READ_DrName(clsDB.DbCon, GstrDrCode);
                //if (strWard.Trim() != "")
                if (strWard.Trim() == "40") //2019-01-14 40일때만(한번더 필터)
                {
                    ssIpdRegPrint.ActiveSheet.Cells[7, 0].Text = "[40병동 대상]" + "\r\n"; //Remark
                }
                else
                {
                    ssIpdRegPrint.ActiveSheet.Cells[7, 0].Text = "";        //2019-01-21    클리어 안되는 문제 있을까봐 한번더 필터링(김현욱)
                }


                if (strDrg.Trim() == "Y")
                {
                    ssIpdRegPrint.ActiveSheet.Cells[7, 0].Text = ssIpdRegPrint.ActiveSheet.Cells[7, 0].Text + " DRG:대상자"; //Remark
                }
                if (strCAG.Trim() == "Y")
                {
                    ssIpdRegPrint.ActiveSheet.Cells[7, 0].Text = ssIpdRegPrint.ActiveSheet.Cells[7, 0].Text + " 당일CAG:대상자"; //Remark
                }
                if (strDSC.Trim() == "Y")
                {
                    ssIpdRegPrint.ActiveSheet.Cells[7, 0].Text = ssIpdRegPrint.ActiveSheet.Cells[7, 0].Text + " DSC:대상자"; //Remark
                }


                if (GstrRDate != "" && GstrRDate == clsPublic.GstrSysDate)
                {
                    ssIpdRegPrint.ActiveSheet.Cells[7, 0].Text = ssIpdRegPrint.ActiveSheet.Cells[7, 0].Text + "\r\n" + "당일입원자]"; //Remark
                }
                else
                {
                    ssIpdRegPrint.ActiveSheet.Cells[7, 0].Text = ssIpdRegPrint.ActiveSheet.Cells[7, 0].Text + "\r\n" + "입원예약]" + GstrRDate; //Remark
                }

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                //string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                //strTitle = "타이틀";

                //strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader = "";

                //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                //strFooter += SPR.setSpdPrint_String("출력일자:" + DateTime.Now.ToString(), new Font("굴림체", 15), clsSpread.enmSpdHAlign.Right, false, true);
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
                setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

                SP.setSpdPrint(ssIpdRegPrint, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);

                // 입원장 출력완료 갱신
                UPDATE_IPDPRT(clsDB.DbCon, GstrPANO, GstrDeptCode);

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

        private void print_OpdJupsu(string strPrintName)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;

            string[] strGel = new string[41];
            string strDeptName = "";
            string strDeptCode = "";
            string strPano = "";
            string strBDATE = "";
            string strSName = "";
            string strGB_BLACK = "";    //입원제한환자(원무팀등록)
            string strSex = "";
            string strPart = "";
            string strBi = "";
            string strOutDate = "";
            string strWARD = "";
            string strDeptCnt = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strTelTime = "";
            string strWrtTime = "";
            string strChk = "";
            string strDrcode = "";
            string strDrname = "";
            string strEMR2 = "";
            string StrJin = "";
            string strDeptCode2 = "";
            string strEmrSinGu = "";
            string strChojae = "";
            string strLastDay = "";
            string strSisel = "";
            string strTel = "";
            string strHPhone = "";
            string strErDate = "";
            string strSms = "";
            string strJuso = "";
            string strMailJuso = "";
            string strZipCode = "";
            string strToSName = "";  //당일당일과 동명2인 변수

            string strMCode = "";  //희귀V  = V
            string strVCode = "";  //등록암 = C
            string strAiFlu = "";
            string strgbflu_vac = "";
            string strOldMan = "";  //어르신먼저구분
            string strNameE = "";  //영문 이름

            string strOT = "";  //안과검진구분
            string strGbSPC = "";  //선택진료

            string strGubun = "";
            string strNoExe = "";  //미시행여부
            string strSC = "";  //금연클리닉체크
            string strHC = "";  //해바라기센터구분

            string strPatChk = "";
            string strBi51 = "";

            string strFall = "";  //낙상고위험군

            int intAge = 0;
            int intSEQNO = 0;

            strPatChk = "";     //원무과 보내기 대상

            strPano = GstrPANO;
            strDeptCode = GstrDeptCode;

            // 스프레드 초기화
            SP.Spread_Clear(ssOpdRegPrint, ssOpdRegPrint.ActiveSheet.RowCount, ssOpdRegPrint.ActiveSheet.ColumnCount);
            SP.Spread_Clear(ssOpdRegPrint2, ssOpdRegPrint2.ActiveSheet.RowCount, ssOpdRegPrint2.ActiveSheet.ColumnCount);

            try
            {
                #region //QUERY
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.CHOJAE,";
                SQL = SQL + ComNum.VBLF + "     A.DEPTCODE, B.SNAME, A.BI, A.AGE, A.EMRSINGU,";
                SQL = SQL + ComNum.VBLF + "     B.JUMIN1, B.JUMIN2,B.JUMIN3, A.PART, A.TELTIME,";
                SQL = SQL + ComNum.VBLF + "     A.WRTTIME, A.SEQNO,A.DRCODE, B.SEX, A.JIN, a.gbflu_vac,a.OldMan,a.Gubun,    ";
                SQL = SQL + ComNum.VBLF + "     B.TEL, B.HPHONE,B.GBSMS, B.JUSO, B.ZIPCODE1 || B.ZIPCODE2 ZIPCODE, B.AIFLU, B.GB_BLACK ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_WORK A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND BDATE = TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+)";
                SQL = SQL + ComNum.VBLF + "         AND (A.DELMARK <> '*' OR A.DELMARK IS NULL)";

                if (GstrFluPrt != "Y")  //신플아닐경우
                {
                    SQL = SQL + ComNum.VBLF + "         AND (GbFlu <>'Y' OR gbFlu IS NULL  )";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         AND GbFlu ='Y'";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY SEQNO DESC";
                #endregion

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strBDATE = dt.Rows[0]["BDATE"].ToString().Trim();
                    strSName = dt.Rows[0]["SNAME"].ToString().Trim() + READ_BAS_SR_Name2(strPano);
                    if (dt.Rows[0]["GB_BLACK"].ToString().Trim() != "")
                    {
                        strGB_BLACK = "★";
                    }
                    else
                    {
                        strGB_BLACK = "";
                    }
                    strBi = dt.Rows[0]["BI"].ToString().Trim();
                    GstrBI = strBi;
                    intAge = Convert.ToInt32(VB.Val(dt.Rows[0]["AGE"].ToString().Trim()));
                    strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();

                    if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    {
                        strJumin2 = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    }
                    else
                    {
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }

                    strPart = dt.Rows[0]["PART"].ToString().Trim();
                    strTelTime = dt.Rows[0]["TELTIME"].ToString().Trim();
                    strWrtTime = dt.Rows[0]["WRTTIME"].ToString().Trim();
                    strDrcode = dt.Rows[0]["DRCODE"].ToString().Trim();
                    strSex = dt.Rows[0]["SEX"].ToString().Trim();
                    intSEQNO = Convert.ToInt32(VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim()));
                    StrJin = dt.Rows[0]["JIN"].ToString().Trim();
                    GstrJin = StrJin;
                    strTel = VB.IIf(dt.Rows[0]["TEL"].ToString().Trim() == "000-0000", "", dt.Rows[0]["TEL"].ToString().Trim()).ToString();
                    strHPhone = dt.Rows[0]["HPHONE"].ToString().Trim();
                    strSms = dt.Rows[0]["GBSMS"].ToString().Trim();
                    strJuso = dt.Rows[0]["JUSO"].ToString().Trim();
                    strZipCode = dt.Rows[0]["ZIPCODE"].ToString().Trim();
                    strAiFlu = dt.Rows[0]["AIFLU"].ToString().Trim();
                    GstrAiFlu = strAiFlu;
                    strgbflu_vac = dt.Rows[0]["GBFLU_VAC"].ToString().Trim();
                    strOldMan = dt.Rows[0]["OLDMAN"].ToString().Trim();
                    GstrOldMan = strOldMan;
                    strGubun = dt.Rows[0]["GUBUN"].ToString().Trim();
                    strMailJuso = "";

                    //우편번호로 동명칭을 읽음
                    SQL = "";
                    SQL = "SELECT MailJuso FROM " + ComNum.DB_PMPA + "BAS_MAILNEW ";
                    SQL = SQL + ComNum.VBLF + "     WHERE MailCode = '" + strZipCode + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                    
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        strMailJuso = dt1.Rows[0]["MAILJUSO"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    strNameE = clsVbfunc.GetPatientEName(clsDB.DbCon, strPano, "1");

                    strChojae = "";

                    if (dt.Rows[0]["CHOJAE"].ToString().Trim() == "1")
                    {
                        strChojae = "(초진)";
                    }

                    if (strChojae == "")
                    {
                        //과초진 여부 설정
                        SQL = "";
                        SQL = "SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_LASTEXAM ";
                        SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND DEPTCODE = '" + strDeptCode + "'";

                        if (strDeptCode == "MD")
                        {
                            if (strDrcode == "1107" || strDrcode == "1125")
                            {
                                SQL = SQL + ComNum.VBLF + "         AND DRCODE in ('1107','1125') ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "         AND DRCODE not in ( '1107','1125') ";
                            }
                        }

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            strChojae = "과재진";
                        }
                        else
                        {
                            strChojae = "과초진";
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    strEmrSinGu = "";

                    if (dt.Rows[0]["EMRSINGU"].ToString().Trim() == "1")
                    {
                        strEmrSinGu = "(신)";
                    }

                    switch (strBi)
                    {
                        case "11":
                        case "12":
                        case "13":
                            strBi = "보험";
                            break;
                        case "21":
                            strBi = "1종";
                            strPatChk = "OK";
                            break;
                        case "22":
                            strBi = "2종";
                            strPatChk = "OK";
                            break;
                        case "23":
                            strBi = "3종";
                            break;
                        case "24":
                            strBi = "행려";
                            break;
                        case "31":
                            strBi = "산재";
                            break;
                        case "32":
                            strBi = "공상";
                            break;
                        case "33":
                            strBi = "산재공상";
                            break;
                        case "41":
                        case "42":
                        case "43":
                        case "44":
                        case "51":
                            if (strBi == "51") { strBi51 = "Y"; }
                            strBi = "일반";
                            break;
                        case "52":
                            strBi = "TA";
                            break;
                        case "55":
                            strBi = "TA일반";
                            break;
                    }

                    //진료과명
                    strDeptName = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, strDeptCode);

                    //진료과 마지막진료일자
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(MAX(BDATE),'YYYY-MM-DD') AS BDATE";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "       AND BDATE <> TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + "       AND PANO = '" + strPano + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        strLastDay = dt1.Rows[0]["BDATE"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //시설여부
                    SQL = "";
                    SQL = "SELECT GUBUN FROM " + ComNum.DB_PMPA + "BAS_NAHOMEGAM ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        switch (dt1.Rows[0]["GUBUN"].ToString().Trim())
                        {
                            case "J":
                                strSisel = "나자렛";
                                break;
                            case "N":
                                strSisel = "햇빛";
                                break;
                            case "L":
                                strSisel = "마리아";
                                break;
                            case "M":
                                strSisel = "요양원";
                                break;
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //두과표시 다시 2014-11-20
                    SQL = "";
                    SQL = "SELECT DEPTCODE FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND ACTDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND DEPTCODE NOT IN ('" + strDeptCode + "') ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY DEPTCODE ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

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
                            if (i == 0) { strDeptCnt = "타과 "; }

                            strDeptCnt = strDeptCnt + dt1.Rows[i]["DEPTCODE"].ToString().Trim();
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //당일동명2인 점검함 - 2009-09-28
                    SQL = "";
                    SQL = "SELECT COUNT(PANO) AS CNT FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE = TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + "      AND SNAME ='" + strSName + "' ";
                    SQL = SQL + ComNum.VBLF + "      AND DeptCode ='" + strDeptCode + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        if (VB.Val(dt1.Rows[0]["CNT"].ToString().Trim()) > 1)
                        {
                            strToSName = "[동명인]";
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //희귀V, 암체크
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     MCode,VCode,GbOT,GbSPC,JinDtl,Jin";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE =  TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND Pano ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND DeptCode ='" + strDeptCode + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        if (dt1.Rows[0]["MCODE"].ToString().Trim() == "V000") { strMCode = "ⓥ"; }
                        if (dt1.Rows[0]["MCODE"].ToString().Trim() == "C000") { strMCode = "차상위C"; }
                        if (dt1.Rows[0]["MCODE"].ToString().Trim() == "E000") { strMCode = "차상위E"; strPatChk = "OK"; }
                        if (dt1.Rows[0]["MCODE"].ToString().Trim() == "F000") { strMCode = "차상위F"; strPatChk = "OK"; }
                        if (dt1.Rows[0]["VCODE"].ToString().Trim() == "V193" || dt1.Rows[0]["VCODE"].ToString().Trim() == "V194") { strVCode = "ⓒ"; }
                        if (dt1.Rows[0]["GBOT"].ToString().Trim() == "Y") { strOT = "Y"; }
                        if (dt1.Rows[0]["GBSPC"].ToString().Trim() == "1") { strGbSPC = "1"; }
                        if (dt1.Rows[0]["JIN"].ToString().Trim() == "5") { strPatChk = "OK"; }
                        if (dt1.Rows[0]["JIN"].ToString().Trim() == "E") { strPatChk = "OK"; }

                        if (dt1.Rows[0]["JINDTL"].ToString().Trim() == "12")
                        {
                            strSC = "★ 금연클리닉대상 ★";
                        }
                        else if (dt1.Rows[0]["JINDTL"].ToString().Trim() == "15" || dt1.Rows[0]["JINDTL"].ToString().Trim() == "16")
                        {
                            strHC = "♥";
                        }

                        if (dt1.Rows[0]["JINDTL"].ToString().Trim() == "22")
                        {
                            strSC = "★ 조산및저체중대상(@F016)";
                        }

                        GstrGbSPC = strGbSPC;
                        GstrPatChk = strPatChk;
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //미시행
                    strNoExe = clsVbfunc.CHECK_EXECUTE_new(clsDB.DbCon, strPano, strDeptCode);

                    strDrname = clsVbfunc.GetBASDoctorName(clsDB.DbCon, strDrcode);


                    //마지막퇴원일자
                    strOutDate = "";
                    strWARD = "";

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     MAX(TO_CHAR(OUTDATE,'YYYY-MM-DD')) AS OUTDATE, WARDCODE ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND GBSTS = '7' ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY WARDCODE ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY OutDate DESC ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        strOutDate = dt1.Rows[0]["OUTDATE"].ToString().Trim();
                        strWARD = dt1.Rows[0]["WARDCODE"].ToString().Trim();
                    }
                    else
                    {
                        strOutDate = "";
                        strWARD = "";
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //emr 영상 존재여부
                    SQL = "";
                    SQL = "SELECT PATID FROM " + ComNum.DB_EMR + "EMR_TREATT ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PATID = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND CLASS = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND CHECKED = '1' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        strEMR2 = "*S*";
                        GstrEMR2 = strEMR2;
                    }
                    else
                    {
                        strEMR2 = "";
                        GstrEMR2 = "";
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (strSms == "Y")
                    {
                        strErDate = strErDate + " 정보동의(SMS)";
                    }
                    else if (strSms == "N")
                    {
                        strErDate = strErDate + " 동의요청(SMS)";
                    }
                    else if (strSms == "X")
                    {
                        strErDate = strErDate + " 동의거부(SMS)";
                    }
                    else
                    {
                        strErDate = strErDate + " 동의요청(SMS)";
                    }

                    //ER환자내역확인
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     MAX(TO_CHAR(ACTDATE,'YYYY-MM-DD')) AS ACTDATE ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND ACTDATE >= TO_DATE('" + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-15).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND ACTDATE <= TO_DATE('" + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND DEPTCODE = 'ER' ";
                    SQL = SQL + ComNum.VBLF + "         AND REP <> '#' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        if (dt1.Rows[0]["ACTDATE"].ToString().Trim() != "")
                        {
                            strErDate = strErDate + " ER내원 : " + VB.Right(dt1.Rows[0]["ACTDATE"].ToString().Trim(), 5);
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;


                    // 2018-11-20 의료정보팀 당일대리접수조회
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT Pano, DeptCode, DrCode, ";
                    SQL = SQL + ComNum.VBLF + "        (SELECT '1' FROM ADMIN.BAS_USER B ";
                    SQL = SQL + ComNum.VBLF + "                   WHERE A.ENTSABUN = B.SABUN ";
                    SQL = SQL + ComNum.VBLF + "                     AND B.JOBGROUP IN('JOB015001', 'JOB015002', 'JOB015003')) GBTODAY ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.OPD_TELRESV A ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1 ";
                    SQL = SQL + ComNum.VBLF + "   AND RDATE = TO_DATE('" + strBDATE + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND GBFLAG = 'Y' ";
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "'";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        if (dt1.Rows[0]["GBTODAY"].ToString().Trim() == "1")
                        {
                            strPart = "222";
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;


                    if (clsVbfunc.READ_낙상고위험체크_OPD(clsDB.DbCon, strPano, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), intAge.ToString()) == "OK")
                    {
                        strFall = "OK";
                        GstrFall = strFall;
                    }

                    if (strDeptCode == "RA")
                    {
                        strDeptCode2 = "MD";
                    }
                    else
                    {
                        strDeptCode2 = strDeptCode;
                    }

                    //바코드 접수증 출력
                    #region 바코드 접수증 출력

                    clsSpread.SpdPrint_Margin setMargin;
                    clsSpread.SpdPrint_Option setOption;

                    clsSpread SP = new clsSpread();

                    #region 출력내용

                    //바코드                    
                    BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                    img = b.Encode(BarcodeLib.TYPE.CODE128, strPano + VB.Asc(VB.Left(strDeptCode2, 1)) + VB.Asc(VB.Right(strDeptCode2, 1)), Color.Black, Color.White, 200, 46);
                    mstrBaCode = strPano + VB.Asc(VB.Left(strDeptCode2, 1)) + VB.Asc(VB.Right(strDeptCode2, 1));

                    //출력
                    //ssPrint_Sheet1.Cells[1, 0].Text = "";
                    //ssPrint_Sheet1.Cells[1, 0].Font = new Font("굴림", 9, FontStyle.Regular);

                    //진료과목
                    ssOpdRegPrint_Sheet1.Cells[0, 0].Text = "진료과목";
                    //ssOpdRegPrint_Sheet1.Cells[0, 1].Text = strDeptName + "/" + "(" + strDrname + ")" + strToSName;
                    ssOpdRegPrint_Sheet1.Cells[0, 1].Text = strDeptName + "/" + "(" + strDrname + ")" + strGB_BLACK + strToSName; //입원제한환자 추가

                    //등록번호
                    ssOpdRegPrint_Sheet1.Cells[1, 0].Text = "등록번호";
                    ssOpdRegPrint_Sheet1.Cells[1, 1].Text = strPano + " " + strSName + strEmrSinGu;

                    //주소
                    ssOpdRegPrint_Sheet1.Cells[2, 0].Text = "주소";
                    ssOpdRegPrint_Sheet1.Cells[2, 1].Text = strMailJuso + ComNum.VBLF + strJuso;


                    if (intAge <= 12)
                    {
                        //주민번호
                        ssOpdRegPrint_Sheet1.Cells[3, 0].Text = "주민번호";
                        ssOpdRegPrint_Sheet1.Cells[3, 1].Text = strJumin1 + "-" + VB.Left(strJumin2, 1) + "(" + (strSex == "F" ? "여" : "남") + ")";
                        //나이
                        ssOpdRegPrint_Sheet1.Cells[3, 2].Text = "나이 : " + clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPano) + "세" + "(" + READ_AGE_GESAN2_emrprt(clsDB.DbCon, strPano) + ")";
                    }
                    else
                    {
                        //주민번호
                        ssOpdRegPrint_Sheet1.Cells[3, 1].Text = strJumin1 + "-" + VB.Left(strJumin2, 1);
                        //나이
                        ssOpdRegPrint_Sheet1.Cells[3, 2].Text = "나이 : " + clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPano) + "세" + "(" + (strSex == "F" ? "여" : "남") + ")";
                    }

                    ssOpdRegPrint_Sheet1.Cells[4, 0].Text = "진료일자";
                    ssOpdRegPrint_Sheet1.Cells[4, 1].Text = strBDATE;

                    ssOpdRegPrint_Sheet1.Cells[4, 2].Text = "종류 : " + strBi + " " + strChojae;

                    if (intSEQNO == -1)
                    {
                        ssOpdRegPrint_Sheet1.Cells[5, 0].Text = "예약시간";
                        ssOpdRegPrint_Sheet1.Cells[5, 1].Text = strWrtTime + "(" + VB.Trim(strPart) + ")";
                        ssOpdRegPrint_Sheet1.Cells[8, 0].Text = " " + "®";
                    }
                    else if (StrJin == "5" || VB.Trim(strPart) == "222")
                    {
                        ssOpdRegPrint_Sheet1.Cells[5, 0].Text = "대리접수";
                        ssOpdRegPrint_Sheet1.Cells[5, 1].Text = strTelTime + "(" + VB.Trim(strPart) + ")";
                        ssOpdRegPrint_Sheet1.Cells[8, 0].Text = " " + "♣";
                    }
                    else if (strGubun == "5")
                    {
                        ssOpdRegPrint_Sheet1.Cells[5, 0].Text = "전화(외래)";
                        ssOpdRegPrint_Sheet1.Cells[5, 1].Text = strTelTime + "(" + VB.Trim(strPart) + ")";
                        ssOpdRegPrint_Sheet1.Cells[8, 0].Text = " " + "☎+";
                    }
                    else if (VB.Trim(strPart) == "333" || VB.Trim(strPart) == "#")
                    {
                        if (strDeptCode2 == "PD")
                        {
                            ssOpdRegPrint_Sheet1.Cells[5, 0].Text = "전화접수";
                        }
                        else
                        {
                            ssOpdRegPrint_Sheet1.Cells[5, 0].Text = "전화예약";
                        }
                        ssOpdRegPrint_Sheet1.Cells[5, 1].Text = strTelTime + "(" + VB.Trim(strPart) + ")";
                        ssOpdRegPrint_Sheet1.Cells[8, 0].Text = " " + "☎";
                    }
                    else
                    {
                        ssOpdRegPrint_Sheet1.Cells[5, 0].Text = "접수시간";
                        ssOpdRegPrint_Sheet1.Cells[5, 1].Text = strWrtTime + "(" + VB.Trim(strPart) + ")";
                    }
                    ssOpdRegPrint_Sheet1.Cells[5, 2].Text = "시설 : " + strSisel;

                    ssOpdRegPrint_Sheet1.Cells[6, 0].Text = "최종진료";
                    ssOpdRegPrint_Sheet1.Cells[6, 1].Text = strLastDay;
                    ssOpdRegPrint_Sheet1.Cells[6, 2].Text = "비고 : " + strDeptCnt;


                    ssOpdRegPrint_Sheet1.Cells[7, 0].Text = "REMARK";
                    if (strHC != "")
                    {
                        ssOpdRegPrint_Sheet1.Cells[7, 1].Text = strHC;
                    }
                    if (strPatChk != "")
                    {
                        ssOpdRegPrint_Sheet1.Cells[7, 2].Text = "(원무과)";
                    }


                    ssOpdRegPrint_Sheet1.Cells[8, 0].Text = ssOpdRegPrint_Sheet1.Cells[8, 0].Text + " " + strErDate;

                    if (strMCode != "" || strVCode != "")
                    {
                        ssOpdRegPrint_Sheet1.Cells[8, 0].Text = ssOpdRegPrint_Sheet1.Cells[8, 0].Text + " " + strMCode + " " + strVCode;
                    }

                    if (strgbflu_vac != "")
                    {
                        ssOpdRegPrint_Sheet1.Cells[9, 0].Text = ssOpdRegPrint_Sheet1.Cells[9, 0].Text + " " + "★플루예방접종환자★";
                    }

                    if (strSC != "")
                    {
                        ssOpdRegPrint_Sheet1.Cells[9, 0].Text = ssOpdRegPrint_Sheet1.Cells[9, 0].Text + " " + strSC;
                    }

                    if (strOT != "")
                    {
                        ssOpdRegPrint_Sheet1.Cells[10, 0].Text = ssOpdRegPrint_Sheet1.Cells[10, 0].Text + " " + "★안과검진★";
                    }

                    if (strNoExe != "N")
                    {
                        ssOpdRegPrint_Sheet1.Cells[10, 0].Text = ssOpdRegPrint_Sheet1.Cells[10, 0].Text + " " + "★미시행검사확인★";
                    }

                    if (strOutDate != "")
                    {
                        ssOpdRegPrint_Sheet1.Cells[11, 0].Text = ssOpdRegPrint_Sheet1.Cells[11, 0].Text + " " + strOutDate;
                        if (strWARD != "")
                        {
                            ssOpdRegPrint_Sheet1.Cells[11, 0].Text = ssOpdRegPrint_Sheet1.Cells[11, 0].Text + " " + "(" + strWARD + ")";
                        }
                    }

                    if (StrJin == "B")
                    {
                        ssOpdRegPrint_Sheet1.Cells[12, 0].Text = ssOpdRegPrint_Sheet1.Cells[12, 0].Text + " " + "★ 예방접종환자 ★";
                    }
                    else
                    {
                        //'윤

                        #region // 2018-03-07 유진호 원무팀에서 현재 원거리 고객 전용창구 운영안함으로 제외요청
                        //SQL = "";
                        //SQL = SQL + ComNum.VBLF + "SELECT JINAME ";
                        //SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_PATIENT A, ADMIN.BAS_AREA B ";
                        //SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + strPano + "' ";
                        //SQL = SQL + ComNum.VBLF + "   AND A.JICODE = B.JICODE ";
                        //SQL = SQL + ComNum.VBLF + "   AND (A.JICODE = '63' OR A.JICODE >='77') ";
                        //SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        //if (SqlErr != "")
                        //{
                        //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        //    return;
                        //}
                        //if (dt1.Rows.Count > 0)
                        //{
                        //    ssOpdRegPrint_Sheet1.Cells[12, 0].Text = ssOpdRegPrint_Sheet1.Cells[12, 0].Text + " " + "★ 원거리:" + dt1.Rows[0]["JINAME"].ToString().Trim();
                        //    ssOpdRegPrint_Sheet1.Cells[12, 0].Text = ssOpdRegPrint_Sheet1.Cells[12, 0].Text + " " + "※원거리고객님은 7번전용창구이용";
                        //}
                        //dt1.Dispose();
                        //dt1 = null;
                        #endregion
                    }


                    if (Convert.ToDateTime(clsPublic.GstrSysDate) >= Convert.ToDateTime("2013-07-25"))
                    {
                        if (strGubun == "3" || strGubun == "4" || strGubun == "5")
                        {
                            ssOpdRegPrint_Sheet1.Cells[13, 0].Text = ssOpdRegPrint_Sheet1.Cells[13, 0].Text + " ▶ 무인수납[신용,체크카드만 해당됨]" + ComNum.VBLF;
                            ssOpdRegPrint_Sheet1.Cells[13, 0].Text = ssOpdRegPrint_Sheet1.Cells[13, 0].Text + "         1.가능        2.불가능" + ComNum.VBLF;
                            ssOpdRegPrint_Sheet1.Cells[13, 0].Text = ssOpdRegPrint_Sheet1.Cells[13, 0].Text + " ♣ 진료후 수납부탁드립니다.";
                        }
                        else
                        {
                            ssOpdRegPrint_Sheet1.Cells[13, 0].Text = ssOpdRegPrint_Sheet1.Cells[13, 0].Text + " ▶ 무인수납[신용,체크카드만 해당됨]" + ComNum.VBLF;
                            ssOpdRegPrint_Sheet1.Cells[13, 0].Text = ssOpdRegPrint_Sheet1.Cells[13, 0].Text + "         1.가능        2.불가능";
                        }
                    }

                    #endregion

                    //string strTitle = "";
                    string strHeader = "";
                    string strFooter = "";
                    bool PrePrint = false;

                    strHeader = "";
                    strFooter = "";

                    setMargin = new clsSpread.SpdPrint_Margin(75, 5, 5, 10, 5, 10);
                    setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

                    if (GstrAddPrt == "Y2")     //의사변경 및 도착처리 화면에서 추가 인쇄할 경우 환자확인용만 인쇄되도록 보완
                    {

                    }
                    else
                    {
                        SP.setSpdPrint(ssOpdRegPrint, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                    }
                    #endregion


                    if (GstrAddPrt == "Y" || GstrAddPrt == "Y2")
                    {
                        ssOpdRegPrint2_Sheet1.Cells[1, 0].Text = strPano;
                        ssOpdRegPrint2_Sheet1.Cells[1, 1].Text = strDeptCode;
                        ssOpdRegPrint2_Sheet1.Cells[1, 2].Text = "외래";

                        ssOpdRegPrint2_Sheet1.Cells[2, 0].Text = strSName;
                        ssOpdRegPrint2_Sheet1.Cells[2, 1].Text = strSex + "/" + clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPano);
                        ssOpdRegPrint2_Sheet1.Cells[2, 2].Text = strJumin1 + "-" + VB.Left(strJumin2, 1);

                        ssOpdRegPrint2_Sheet1.Cells[4, 0].Text = "진료 전 후 꼭 지참하세요 !!";

                        SP.setSpdPrint(ssOpdRegPrint2, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ssOpdRegPrint_PrintHeaderFooterArea(object sender, FarPoint.Win.Spread.PrintHeaderFooterAreaEventArgs e)
        {
            if (e.IsHeader == true)
            {
                RectangleF RectangleFX = new RectangleF(100, 5, 220, 50);
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                //e.Graphics.DrawImage(img, 100, 5);
                e.Graphics.DrawImage(img, RectangleFX);
                e.Graphics.DrawString(mstrBaCode, new Font("굴림", 10, FontStyle.Regular), Brushes.Black, 170, 60);

                if (READ_A_SUNAP_CHK(clsDB.DbCon, GstrPANO, GstrBI, GstrDeptCode) == "OK")
                {
                    e.Graphics.DrawString("<일괄수납대상>", new Font("굴림", 12, FontStyle.Bold), Brushes.Black, 0, 10);
                }

                if (GstrJin == "8" || GstrJin == "G" || GstrJin == "U")
                {
                    e.Graphics.DrawString("물리치료(PT)", new Font("굴림", 10, FontStyle.Bold), Brushes.Black, 60, 30);
                }

                if (GstrBi51 == "Y" || GstrPatChk == "OK")
                {
                    e.Graphics.DrawString("(원무과)", new Font("굴림", 10, FontStyle.Bold), Brushes.Black, 0, 30);
                }

                if (GstrGbSPC == "1")
                {
                    e.Graphics.DrawString("★선택★", new Font("굴림", 10, FontStyle.Bold), Brushes.Black, 0, 45);
                }

                if (GstrFall == "OK")
                {
                    e.Graphics.DrawString("(낙상)", new Font("굴림", 10, FontStyle.Bold), Brushes.Black, 70, 45);
                }

                if (GstrOldMan == "Y")
                {
                    e.Graphics.DrawString("★어르신먼저★", new Font("굴림", 10, FontStyle.Bold), Brushes.Black, 0, 60);
                }
                else
                {
                    e.Graphics.DrawString(GstrEMR2, new Font("굴림", 10, FontStyle.Bold), Brushes.Black, 30, 60);
                }

                if (READ_PRE_RESERVED(clsDB.DbCon, GstrPANO, GstrDeptCode) == "OK")
                {
                    e.Graphics.DrawString("★선예약", new Font("굴림", 10, FontStyle.Bold), Brushes.Black, 95, 60);
                }


                if (GstrAiFlu != "")
                {
                    e.Graphics.DrawString("★플루동의서 받으세요★", new Font("굴림", 10, FontStyle.Bold), Brushes.Black, 0, 400);
                }
            }
        }

        private void ssOpdRegPrint2_PrintHeaderFooterArea(object sender, FarPoint.Win.Spread.PrintHeaderFooterAreaEventArgs e)
        {
            if (e.IsHeader == true)
            {
                RectangleF RectangleFX = new RectangleF(100, 5, 220, 50);
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                //e.Graphics.DrawImage(img, 100, 5);
                e.Graphics.DrawImage(img, RectangleFX);
                e.Graphics.DrawString(mstrBaCode, new Font("굴림", 10, FontStyle.Regular), Brushes.Black, 170, 60);

                e.Graphics.DrawString("환 자 확 인 용", new Font("굴림", 13, FontStyle.Bold), Brushes.Black, 0, 10);
            }
        }

        private string READ_PRE_RESERVED(PsmhDb pDbCon, string ArgPano, string ArgDept)
        {

            //2018-12-10 계장 김현욱
            //예약 후불제 관련 선예약 한 사람 구분 추가
            //조건 : 당일 OPD_MASTER의 RESERVED = '1' 환자 (GBRES = '0' OR GBRES IS NULL)인 환자, GBRES = '1'은 후불 예약한 사람임
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            try
            {

                SQL = "";
                SQL += ComNum.VBLF + " SELECT BDATE ";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + ArgPano + "'";
                SQL += ComNum.VBLF + " AND BDATE = TRUNC(SYSDATE)";
                SQL += ComNum.VBLF + " AND BDATE >= TO_DATE('2018-12-13','YYYY-MM-DD')";  //12월 13일 접수증 부터 표기.(안전장치)
                SQL += ComNum.VBLF + " AND DEPTCODE = '" + ArgDept + "'";
                SQL += ComNum.VBLF + " AND RESERVED = '1'";
                SQL += ComNum.VBLF + " AND (GBRES = '0' OR GBRES IS NULL)";        //후불예약 구분 컬럼
                //SQL = "";
                //SQL += ComNum.VBLF + " SELECT A.RESERVED, B.DATE1 ";
                //SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_MASTER A,  " + ComNum.DB_PMPA + "OPD_RESERVED_NEW B";
                //SQL += ComNum.VBLF + " WHERE A.PANO = '" + ArgPano + "'";
                //SQL += ComNum.VBLF + " AND A.BDATE = TRUNC(SYSDATE)";
                //SQL += ComNum.VBLF + " AND A.BDATE >= TO_DATE('2018-12-13','YYYY-MM-DD')";  //12월 13일 접수증 부터 표기.(안전장치)
                //SQL += ComNum.VBLF + " AND A.DEPTCODE = '" + ArgDept + "'";
                //SQL += ComNum.VBLF + " AND A.PANO = B.PANO";
                //SQL += ComNum.VBLF + " AND A.DEPTCODE = B.DEPTCODE";
                //SQL += ComNum.VBLF + " AND A.BDATE = TRUNC(B.DATE3)";
                //SQL += ComNum.VBLF + " AND B.DATE1 <= TO_DATE('2018-12-12', 'YYYY-MM-DD')";
                //SQL += ComNum.VBLF + " AND A.RESERVED = '1'";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (Dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }

                Dt.Dispose();
                Dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }
        }


        // 후불수납
        private string READ_A_SUNAP_CHK(PsmhDb pDbCon, string ArgPano, string ArgBi, string ArgDept, string strBDate = "")
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Pano,TO_CHAR(EDATE,'YYYY-MM-DD') EDATE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_AUTO_MST ";
                SQL += ComNum.VBLF + "  WHERE Pano ='" + ArgPano + "' ";
                SQL += ComNum.VBLF + "    AND (DELDATE ='' OR DELDATE IS NULL) ";
                SQL += ComNum.VBLF + "    AND GUBUN ='1'";
                SQL += ComNum.VBLF + "  ORDER BY SDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (Dt.Rows.Count > 0)
                {
                    if (strBDate.Equals(""))
                    {
                        rtnVal = "OK";
                    }
                    else
                    {
                        if (Dt.Rows[0]["EDATE"].ToString().Trim() == "")
                        {
                            rtnVal = "OK";
                        }
                        else if (string.Compare(strBDate, Dt.Rows[0]["EDATE"].ToString().Trim()) <= 0)
                        {
                            rtnVal = "OK";
                        }
                        else
                        {
                            rtnVal = "";
                        }
                    }
                }

                Dt.Dispose();
                Dt = null;

                if (ArgBi != "")
                {
                    if (ArgBi.Equals("21") || ArgBi.Equals("22") || ArgBi.Equals("31") || ArgBi.Equals("32") || ArgBi.Equals("33") || ArgBi.Equals("52") || ArgBi.Equals("55"))
                    {
                        rtnVal = "";
                    }
                }

                if (ArgDept != "")
                {
                    if (ArgDept.Equals("HD"))
                    {
                        rtnVal = "";
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }
        }

        private string READ_AGE_GESAN2_emrprt(PsmhDb pDbCon, string argPTNO, string ArgAge = "")
        {
            DataTable dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            string strJumin = "";
            string strMonth = "";
            string strIlsu = "";

            try
            {

                SQL = "";
                SQL += ComNum.VBLF + "SELECT  TO_DATE(TODAY,'YYYY-MM-DD') - BIRTH + 1 ILSU,birth, ";
                SQL += ComNum.VBLF + " SUBSTR(MONTHS_BETWEEN(TO_DATE(TODAY,'YYYY-MM-DD'), BIRTH), 0, 10) MONTH, JUMIN1||JUMIN2 JUMIN";
                SQL += ComNum.VBLF + " FROM (";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD') TODAY, CASE SUBSTR(JUMIN2, 1, 1)";
                SQL += ComNum.VBLF + " WHEN '1' THEN '19'";
                SQL += ComNum.VBLF + " WHEN '2' THEN '19'";
                SQL += ComNum.VBLF + " WHEN '5' THEN '19'";
                SQL += ComNum.VBLF + " WHEN '6' THEN '19'";
                SQL += ComNum.VBLF + " WHEN '3' THEN '20'";
                SQL += ComNum.VBLF + " WHEN '4' THEN '20'";
                SQL += ComNum.VBLF + " WHEN '7' THEN '20'";
                SQL += ComNum.VBLF + " WHEN '8' THEN '20'";
                SQL += ComNum.VBLF + " WHEN '9' THEN '18'";
                SQL += ComNum.VBLF + " WHEN '0' THEN '18'";
                SQL += ComNum.VBLF + " END NYEAR, JUMIN1, JUMIN2, BIRTH ";
                SQL += ComNum.VBLF + " From ADMIN.BAS_PATIENT";
                SQL += ComNum.VBLF + " WHERE PANO = '" + argPTNO + "')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strJumin = dt.Rows[0]["JUMIN"].ToString().Trim();



                    if (dt.Rows[0]["MONTH"].ToString().Trim() != "" && dt.Rows[0]["Birth"].ToString().Trim() != "")  //'2014-05-16 birth 있는것만
                    {
                        strMonth = VB.Fix((int)VB.Val(dt.Rows[0]["MONTH"].ToString().Trim())).ToString();
                        strIlsu = VB.Fix((int)VB.Val(dt.Rows[0]["ILSU"].ToString().Trim())).ToString();


                        if ((int)VB.Val(strIlsu) <= 30)
                        {
                            rtnVal = strIlsu + "d";
                        }
                        else if ((int)VB.Val(strMonth) <= 156 && (int)VB.Val(strIlsu) >= 30)
                        {
                            rtnVal = strMonth + "m";
                        }
                    }
                    else
                    {
                        rtnVal = "";
                    }
                }
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장                
            }

            return rtnVal;
        }

        private bool UPDATE_IPDPRT(PsmhDb pDbCon, string strPano, string strDeptCode)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "  UPDATE ADMIN.OPD_DEPTJEPSU SET ";
                SQL += ComNum.VBLF + "   IPD_PRT ='Y' ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                SQL += ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "' ";
                SQL += ComNum.VBLF + "   AND ACTDATE = TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + "   AND IPD_PRT ='P' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }
    }
}
