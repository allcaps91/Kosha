using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;
using ComBase;

using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using FarPoint.Win.Spread;

namespace ComSupLibB.SupInfc
{
    /// <summary>
    /// Class Name      : MedOrder
    /// File Name       : frmComSupfection2_2020.cs
    /// Description     : 법정 감염병 신고서
    /// Author          : 안정수
    /// Create Date     : 2019-12-27
    /// <history> 
    /// 법정 감염병 신고서
    /// </summary>
    public partial class frmComSupfection2_2020 : Form
    {
        private string GstrPANO = "";
        private string GstrROWID = "";
        private double GdblIpdNO_OCS = 0;
        private string GstrGbnER = "";
        private bool GbolEXINFECT = false;
        string READERROR = "";
        private string GstrAge = "";
        private string GstrDept = "";
        private string GstrDrSabun = "";
        private string GstrWard = "";
        private string GstrRoom = "";
        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();
        private clsInFc Infect;

        public delegate void SendText(string strText);
        public event SendText rSendText;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmComSupfection2_2020()
        {
            InitializeComponent();
        }

        public frmComSupfection2_2020(bool bolEXINFECT, string strPANO, double dblIPDNO, string strGbnER = "")
        {
            InitializeComponent();

            GbolEXINFECT = bolEXINFECT;
            GstrPANO = strPANO;
            GdblIpdNO_OCS = dblIPDNO;
            GstrGbnER = strGbnER;
        }

        private void frmComSupfection2_2020_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            txtPano.Text = GstrPANO;

            if (GstrPANO != "") { lblSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, GstrPANO); }

            panPat.Visible = false;

            if (clsType.User.Sabun == "49834" || clsType.User.Sabun == "4349" ||
                clsType.User.Sabun == "33950" || clsType.User.Sabun == "44794" ||
                clsType.User.Sabun == "41827" || clsType.User.Sabun == "41596" || clsType.User.Sabun == "49880")
            {
                panPat.Visible = true;
            }

            panWeb.Visible = false;
            panHelp.Visible = false;
            webBrowser1.Navigate("https://www.cdc.go.kr/npt/biz/npp/portal/nppLwcrIcdMain.do");

            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept);

            read_sysdate();

            Screen_display();

            SCREEN_CLEAR();

            GetData();

            
        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
        }

       
        
        private void Screen_display()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strAge = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     AGE, DEPTCODE, DRCODE, WARDCODE, ROOMCODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE IPDNO = '" + GdblIpdNO_OCS + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GstrAge = dt.Rows[0]["AGE"].ToString().Trim();
                    GstrDept = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    GstrWard = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    GstrRoom = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    GstrDrSabun = clsType.User.Sabun;
                    cboDept.Text = GstrDept + "." + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, GstrDept);
                    cboDr.Text = dt.Rows[0]["DRCODE"].ToString().Trim() + "." + clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                    txtWard.Text = GstrWard;
                    txtRoom.Text = GstrRoom;
                }
                else
                {
                    txtWard.Text = "";
                    txtRoom.Text = "";
                    GstrDept = clsOrdFunction.Pat.DeptCode;

                    if (GstrDept != null)
                    {
                        cboDept.Text = GstrDept + "." + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, GstrDept);
                    }

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     DRCODE, DRNAME, DEPTCODE, DRBUNHO";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                    SQL = SQL + ComNum.VBLF + "     WHERE SABUN = '" + clsType.User.Sabun + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        cboDr.Text = dt.Rows[0]["DRCODE"].ToString().Trim() + "." + dt.Rows[0]["DRNAME"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SNAME, PNAME, JUMIN1, JUMIN2 , JIKUP, SEX, TEL, HPHONE, GBFOREIGNER,";
                SQL = SQL + ComNum.VBLF + "     A.ZIPCODE1, A.ZIPCODE2, B.ZIPNAME1 || ' ' || B.ZIPNAME2 ||' ' || B.ZIPNAME3 AS ZIPNAME, JUSO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "BAS_ZIPSNEW B ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.ZIPCODE1 = B.ZIPCODE1(+) ";
                SQL = SQL + ComNum.VBLF + "         AND A.ZIPCODE2 = B.ZIPCODE2(+) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    if(dt.Rows[0]["GBFOREIGNER"].ToString().Trim() == "Y")
                    {
                        ComFunc.MsgBox("해당 환자는 외국인 입니다. 국적을 입력 해주세요. ex) 미국, 필리핀");
                        ssView2_Sheet1.Cells[11, 9].Text = "";
                    }
                    //환자명
                    ssView2_Sheet1.Cells[7, 2].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    //등록번호
                    ssView2_Sheet1.Cells[7, 4].Text = ComFunc.SetAutoZero(txtPano.Text.Trim(), 8);

                    //성별
                    ssView2_Sheet1.Cells[8, 10].Value = dt.Rows[0]["SEX"].ToString().Trim() == "M" ? true : false;
                    ssView2_Sheet1.Cells[9, 10].Value = dt.Rows[0]["SEX"].ToString().Trim() == "F" ? true : false;

                    //직업
                    ssView2_Sheet1.Cells[8, 7].Text = clsVbfunc.GetJikupName(clsDB.DbCon, dt.Rows[0]["JIKUP"].ToString().Trim());

                    clsAES.Read_Jumin_AES(clsDB.DbCon, txtPano.Text);

                    //주민등록번호
                    ssView2_Sheet1.Cells[7, 8].Text = clsAES.GstrAesJumin1 + "-" + clsAES.GstrAesJumin2;

                    //연령
                    strAge = ComFunc.AgeCalc(clsDB.DbCon, clsAES.GstrAesJumin1 + clsAES.GstrAesJumin2).ToString();

                    if (VB.Val(strAge) < 19)
                    {
                        ssView2_Sheet1.Cells[8, 1].Text = "보호자성명 ( " + dt.Rows[0]["PNAME"].ToString().Trim() + " )";
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[8, 1].Text = "보호자성명 (         )";
                    }

                    GstrAge = strAge;

                    //우편번호
                    ssView2_Sheet1.Cells[11, 3].Text = dt.Rows[0]["ZIPCODE1"].ToString().Trim() + "-" + dt.Rows[0]["ZIPCODE2"].ToString().Trim();

                    //전화번호
                    ssView2_Sheet1.Cells[9, 2].Text = dt.Rows[0]["HPHONE"].ToString().Trim() + "/" + dt.Rows[0]["TEL"].ToString().Trim();

                    //주소
                    ssView2_Sheet1.Cells[10, 2].Text = dt.Rows[0]["ZIPNAME"].ToString().Trim() + " " + dt.Rows[0]["JUSO"].ToString().Trim();

                    //의사성명
                    ssView2_Sheet1.Cells[48, 3].Text = clsVbfunc.GetOCSDrNameSabun(clsDB.DbCon, clsType.User.Sabun);
                    ssView2_Sheet1.Cells[48, 8].Text = clsVbfunc.GetOCSDoctorDRBUNHO(clsDB.DbCon, clsType.User.Sabun);
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                ssView2_Sheet1.Cells[37, 1].Text = cpublic.strSysDate;
                ssView2_Sheet1.Cells[37, 5].Text = cpublic.strSysDate;
                ssView2_Sheet1.Cells[37, 9].Text = cpublic.strSysDate;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void SCREEN_CLEAR()
        {
            #region 개정 전 내용 주석처리 
            ////제1군
            //ssView2_Sheet1.Cells[14, 2].Value = false;      //콜레라
            //ssView2_Sheet1.Cells[14, 5].Value = false;      //장티푸스
            //ssView2_Sheet1.Cells[14, 8].Value = false;      //파라티푸스
            //ssView2_Sheet1.Cells[15, 2].Value = false;      //세균성이질
            //ssView2_Sheet1.Cells[15, 5].Value = false;      //장출혈성대장균감염증
            //ssView2_Sheet1.Cells[15, 8].Value = false;      //A형간염

            ////제2군
            //ssView2_Sheet1.Cells[16, 2].Value = false;      //디프테리아
            //ssView2_Sheet1.Cells[16, 5].Value = false;      //백일해
            //ssView2_Sheet1.Cells[16, 8].Value = false;      //파상풍

            //ssView2_Sheet1.Cells[17, 2].Value = false;      //홍역
            //ssView2_Sheet1.Cells[17, 5].Value = false;      //유행성이하선염
            //ssView2_Sheet1.Cells[17, 8].Value = false;      //풍진

            //ssView2_Sheet1.Cells[18, 2].Value = false;      //폴리오
            //ssView2_Sheet1.Cells[18, 5].Value = false;      //일본뇌염
            //ssView2_Sheet1.Cells[18, 8].Value = false;      //수두

            //ssView2_Sheet1.Cells[19, 2].Value = false;      //B형간염(급성)
            //ssView2_Sheet1.Cells[19, 5].Value = false;      //b형헤모필루스인플루엔자
            //ssView2_Sheet1.Cells[19, 8].Value = false;      //폐렴구균

            ////제3군
            //ssView2_Sheet1.Cells[20, 2].Value = false;      //말라리아
            //ssView2_Sheet1.Cells[20, 5].Value = false;      //한센병
            //ssView2_Sheet1.Cells[20, 8].Value = false;      //선홍열

            //ssView2_Sheet1.Cells[21, 2].Value = false;      //수막구균성수막염
            //ssView2_Sheet1.Cells[21, 5].Value = false;      //레지오넬라증
            //ssView2_Sheet1.Cells[21, 8].Value = false;      //비브리오패혈증

            //ssView2_Sheet1.Cells[22, 2].Value = false;      //발진티푸스
            //ssView2_Sheet1.Cells[22, 5].Value = false;      //발진열
            //ssView2_Sheet1.Cells[22, 8].Value = false;      //쯔쯔가무시증

            //ssView2_Sheet1.Cells[23, 2].Value = false;      //렙토스피라증
            //ssView2_Sheet1.Cells[23, 5].Value = false;      //브루셀라증
            //ssView2_Sheet1.Cells[23, 8].Value = false;      //탄저

            //ssView2_Sheet1.Cells[24, 2].Value = false;      //공수병
            //ssView2_Sheet1.Cells[24, 5].Value = false;      //신증후군출혈열

            //ssView2_Sheet1.Cells[25, 2].Value = false;      //매독(1기)
            //ssView2_Sheet1.Cells[25, 5].Value = false;      //매독(2기)
            //ssView2_Sheet1.Cells[25, 8].Value = false;      //매독(선천성)

            //ssView2_Sheet1.Cells[26, 2].Value = false;      //크로이츠밸트-야콥병 및 변종 크로이츠밸트-야콥병

            //ssView2_Sheet1.Cells[27, 2].Value = false;      //C형간염
            //ssView2_Sheet1.Cells[27, 5].Value = false;      //VRSA 감염증

            //ssView2_Sheet1.Cells[28, 2].Value = false;      //CRE 감염증

            ////제4군
            //ssView2_Sheet1.Cells[29, 2].Value = false;      //페스트
            //ssView2_Sheet1.Cells[29, 5].Value = false;      //황열
            //ssView2_Sheet1.Cells[29, 8].Value = false;      //뎅기열

            //ssView2_Sheet1.Cells[30, 2].Value = false;      //두창
            //ssView2_Sheet1.Cells[30, 5].Value = false;      //보툴리눔독소증
            //ssView2_Sheet1.Cells[30, 8].Value = false;      //중증급성호흡기증후군

            //ssView2_Sheet1.Cells[31, 2].Value = false;      //조류인플루엔자 인체감염증
            //ssView2_Sheet1.Cells[31, 5].Value = false;      //지카바이러스
            //ssView2_Sheet1.Cells[31, 8].Value = false;      //신종인플루엔자

            //ssView2_Sheet1.Cells[32, 2].Value = false;      //야토병
            //ssView2_Sheet1.Cells[32, 5].Value = false;      //큐열
            //ssView2_Sheet1.Cells[32, 8].Value = false;      //웨스트나일열

            //ssView2_Sheet1.Cells[33, 2].Value = false;      //라임병
            //ssView2_Sheet1.Cells[33, 5].Value = false;      //진드기매개뇌염
            //ssView2_Sheet1.Cells[33, 8].Value = false;      //바이러스성출혈열

            //ssView2_Sheet1.Cells[34, 2].Value = false;      //유비저
            //ssView2_Sheet1.Cells[34, 5].Value = false;      //치쿤구니아열
            //ssView2_Sheet1.Cells[34, 8].Value = false;      //중증열성혈소판감소증후군

            //ssView2_Sheet1.Cells[35, 2].Value = false;      //중동호흡기증후군
            //ssView2_Sheet1.Cells[35, 5].Value = false;      //신종전염병증후군(증상 및 징후)
            //ssView2_Sheet1.Cells[35, 9].Text = "";          //신종전염병증후군(증상 및 징후)

            //ssView2_Sheet1.Cells[36, 1].Text = "";          //발병일
            //ssView2_Sheet1.Cells[36, 5].Text = "";          //진단일
            //ssView2_Sheet1.Cells[36, 9].Text = "";          //신고일

            ////확진검사 결과
            //ssView2_Sheet1.Cells[37, 1].Value = false;      //양성
            //ssView2_Sheet1.Cells[37, 3].Value = false;      //음성

            //ssView2_Sheet1.Cells[37, 5].Value = false;      //검사진행중
            //ssView2_Sheet1.Cells[37, 7].Value = false;      //검사미실시

            ////환자등 분류
            //ssView2_Sheet1.Cells[38, 1].Value = false;      //환자
            //ssView2_Sheet1.Cells[38, 2].Value = false;      //의사환자
            //ssView2_Sheet1.Cells[38, 4].Value = false;      //병원체보유자

            ////입원여부
            //ssView2_Sheet1.Cells[38, 7].Value = false;      //외래
            //ssView2_Sheet1.Cells[38, 8].Value = false;      //입원
            //ssView2_Sheet1.Cells[38, 9].Value = false;      //기타

            ////추정감염경로
            //ssView2_Sheet1.Cells[39, 1].Value = false;      //집단감염환자와 접촉
            //ssView2_Sheet1.Cells[40, 1].Value = false;      //개별감염환자와 접촉
            //ssView2_Sheet1.Cells[41, 1].Value = false;      //불확실함
            //ssView2_Sheet1.Cells[41, 3].Value = false;      //접촉없었음

            ////추정감염지역 
            //ssView2_Sheet1.Cells[39, 6].Value = false;      //국내
            //ssView2_Sheet1.Cells[40, 6].Value = false;      //국외
            //ssView2_Sheet1.Cells[40, 8].Text = "";          //국명

            //ssView2_Sheet1.Cells[41, 7].Text = "";          //체류기간 FROM :
            //ssView2_Sheet1.Cells[41, 9].Text = "";          //체류기간 TO:
            //ssView2_Sheet1.Cells[41, 11].Text = ")";        //체류기간:

            ////사망여부
            //ssView2_Sheet1.Cells[43, 1].Value = false;      //생존
            //ssView2_Sheet1.Cells[43, 2].Value = false;      //사망
            //ssView2_Sheet1.Cells[43, 6].Value = false;      //기타(환자아님)

            ////비고(특이사항)

            ////변경신고
            //ssView2_Sheet1.Cells[45, 3].Text = "";

            #endregion

            //제1급
            ssView2_Sheet1.Cells[14, 2].Value = false;
            ssView2_Sheet1.Cells[14, 5].Value = false;
            ssView2_Sheet1.Cells[14, 8].Value = false;

            ssView2_Sheet1.Cells[15, 2].Value = false;
            ssView2_Sheet1.Cells[15, 5].Value = false;
            ssView2_Sheet1.Cells[15, 8].Value = false;

            ssView2_Sheet1.Cells[16, 2].Value = false;
            ssView2_Sheet1.Cells[16, 5].Value = false;
            ssView2_Sheet1.Cells[16, 8].Value = false;

            ssView2_Sheet1.Cells[17, 2].Value = false;
            ssView2_Sheet1.Cells[17, 5].Value = false;
            ssView2_Sheet1.Cells[17, 8].Value = false;

            ssView2_Sheet1.Cells[18, 2].Value = false;
            ssView2_Sheet1.Cells[18, 5].Value = false;
            ssView2_Sheet1.Cells[18, 8].Value = false;

            ssView2_Sheet1.Cells[19, 2].Value = false;
            ssView2_Sheet1.Cells[19, 5].Value = false;
            ssView2_Sheet1.Cells[19, 9].Text = "";  // 증상 및 징후

            //제2급
            ssView2_Sheet1.Cells[20, 2].Value = false;
            ssView2_Sheet1.Cells[20, 5].Value = false;
            ssView2_Sheet1.Cells[20, 8].Value = false;

            ssView2_Sheet1.Cells[21, 2].Value = false;
            ssView2_Sheet1.Cells[21, 5].Value = false;
            ssView2_Sheet1.Cells[21, 8].Value = false;

            ssView2_Sheet1.Cells[22, 2].Value = false;
            ssView2_Sheet1.Cells[22, 5].Value = false;
            ssView2_Sheet1.Cells[22, 8].Value = false;

            //ssView2_Sheet1.Cells[23, 2].Value = false; 
            ssView2_Sheet1.Cells[23, 5].Value = false;
            ssView2_Sheet1.Cells[23, 8].Value = false;

            ssView2_Sheet1.Cells[24, 2].Value = false;
            ssView2_Sheet1.Cells[24, 5].Value = false;

            ssView2_Sheet1.Cells[25, 2].Value = false;
            ssView2_Sheet1.Cells[25, 5].Value = false;
            ssView2_Sheet1.Cells[25, 8].Value = false;

            ssView2_Sheet1.Cells[26, 2].Value = false;
            ssView2_Sheet1.Cells[26, 5].Value = false;

            ssView2_Sheet1.Cells[27, 2].Value = false;
            ssView2_Sheet1.Cells[27, 8].Value = false;

            //제3급             
            ssView2_Sheet1.Cells[28, 2].Value = false;
            ssView2_Sheet1.Cells[28, 5].Value = false;
            ssView2_Sheet1.Cells[28, 8].Value = false;

            ssView2_Sheet1.Cells[29, 2].Value = false;
            ssView2_Sheet1.Cells[29, 5].Value = false;
            ssView2_Sheet1.Cells[29, 8].Value = false;

            ssView2_Sheet1.Cells[30, 2].Value = false;
            ssView2_Sheet1.Cells[30, 5].Value = false;
            ssView2_Sheet1.Cells[30, 8].Value = false;

            ssView2_Sheet1.Cells[31, 2].Value = false;
            ssView2_Sheet1.Cells[31, 5].Value = false;
            ssView2_Sheet1.Cells[31, 8].Value = false;

            ssView2_Sheet1.Cells[32, 2].Value = false;
            ssView2_Sheet1.Cells[32, 5].Value = false;
            ssView2_Sheet1.Cells[32, 8].Value = false;

            ssView2_Sheet1.Cells[33, 2].Value = false;
            ssView2_Sheet1.Cells[33, 5].Value = false;
            ssView2_Sheet1.Cells[33, 8].Value = false;

            ssView2_Sheet1.Cells[34, 2].Value = false;

            ssView2_Sheet1.Cells[35, 2].Value = false;
            ssView2_Sheet1.Cells[35, 5].Value = false;
            ssView2_Sheet1.Cells[35, 9].Value = false;

            ssView2_Sheet1.Cells[36, 2].Value = false;
            ssView2_Sheet1.Cells[36, 5].Value = false;
            ssView2_Sheet1.Cells[36, 9].Value = false;


            //발병일, 진단일, 신고일 
            //ssView2_Sheet1.Cells[37, 1].Value = false; //발병일
            //ssView2_Sheet1.Cells[37, 5].Value = false; //진단일
            //ssView2_Sheet1.Cells[37, 9].Value = false; //신고일

            //확진검사 결과
            ssView2_Sheet1.Cells[38, 1].Value = false; //양성
            ssView2_Sheet1.Cells[38, 3].Value = false; //음성
            ssView2_Sheet1.Cells[38, 5].Value = false; //검사진행중
            ssView2_Sheet1.Cells[38, 7].Value = false; //검사미실시

            //환자등 불류
            ssView2_Sheet1.Cells[39, 1].Value = false; //환자
            ssView2_Sheet1.Cells[39, 2].Value = false; //의사환자
            ssView2_Sheet1.Cells[40, 1].Value = false; //병원체보유자
            ssView2_Sheet1.Cells[40, 3].Value = false; //그밖의 경우
            ssView2_Sheet1.Cells[39, 3].Value = false; //검사거부자

            //입원여부
            ssView2_Sheet1.Cells[39, 7].Value = false; //외래
            ssView2_Sheet1.Cells[39, 8].Value = false; //입원
            ssView2_Sheet1.Cells[40, 7].Value = false; //그밖의 경우

            //추정감염경로
            ssView2_Sheet1.Cells[41, 1].Value = false; //짐단감염환자와 접촉
            ssView2_Sheet1.Cells[42, 1].Value = false; //개별감염환자와 접촉
            ssView2_Sheet1.Cells[43, 1].Value = false; //불확실함
            ssView2_Sheet1.Cells[43, 3].Value = false; //접촉없었음

            //추정감염지역
            ssView2_Sheet1.Cells[41, 6].Value = false; //국내
            ssView2_Sheet1.Cells[42, 6].Value = false; //국외
            ssView2_Sheet1.Cells[42, 8].Text = ""; //국명:
            ssView2_Sheet1.Cells[43, 7].Text = ""; //체류기간 FROM
            ssView2_Sheet1.Cells[43, 9].Text = ""; //체류기간 TO
            ssView2_Sheet1.Cells[43, 10].Text = ")"; //체류기간:

            //사망여부
            ssView2_Sheet1.Cells[45, 1].Value = false; //생존
            ssView2_Sheet1.Cells[45, 2].Value = false; //사망
            ssView2_Sheet1.Cells[45, 6].Value = false; //기타(환자아님)

            //비고(특이사항)

            //변경신고
            ssView2_Sheet1.Cells[45, 6].Text = "";
        }

        private void toolHelp_Click(object sender, EventArgs e)
        {
            panHelp.BringToFront();
            panHelp.Visible = true;
        }

        private void toolLaw_Click(object sender, EventArgs e)
        {
            panWeb.BringToFront();
            panWeb.Visible = true;
        }

        private void toolWebsend_Click(object sender, EventArgs e)
        {

            int i = 0;
            int j = 0;
            string strInfect1 = "";
            string strinfect2 = "";
            string strinfect3 = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ChkAutoSin.Checked == true)
            {
                GetData2();
            }

            if (ChkAutoSin.Checked == false)
            {
                if (GstrROWID.Length == 0)
                {
                    ComFunc.MsgBox("전송할 신고서를 선택하여 주십시요.");
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    SQL = "";
                    SQL = "SELECT AUTOSEND ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT2 ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + GstrROWID + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["AUTOSEND"].ToString().Trim() != "")
                        {
                            if (ComFunc.MsgBoxQ("이미 전송한 신고서가 있습니다. 재전송하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {
                                dt.Dispose();
                                dt = null;
                                return;
                            }
                        }
                    }
                    else
                    {
                        ComFunc.MsgBox("전송할 신고서를 선택하여 주십시요.");
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    dt.Dispose();
                    dt = null;


                    Infect = new clsInFc();
                    //본서버
                    Infect.OGCR = "cn=(재)포항성모병원,ou=건강보험,ou=MOHW RA센터,ou=등록기관,ou=licensedCA,o=KICA,c=KR";
                    //개발서버
                    //Infect.OGCR = "cn=포항성모병원,ou=경상북도,ou=포항시,ou=질병관리본부,o=CDC,c=KR";
                    Infect.OGCR = Infect.OGCR.Replace("`", "''");

                    Infect.PATNT_NM = ssView2_Sheet1.Cells[7, 2].Text;
                    Infect.PATNT_IHIDNUM = ssView2_Sheet1.Cells[7, 8].Text.Trim().Replace("-", "");

                    Infect.PATNT_REGIST_NO = ssView2_Sheet1.Cells[0, 1].Text.Trim();

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[8, 10].Value) == true)
                    {
                        Infect.PATNT_SEXDSTN_CD = "1";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[9, 10].Value) == true)
                    {
                        Infect.PATNT_SEXDSTN_CD = "2";
                    }

                    SQL = "";
                    SQL = "SELECT SNAME, TEL, HPHONE";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                    SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + txtPano.Text.Trim() + "'";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        Infect.PATNT_NM = dt.Rows[0]["SNAME"].ToString().Trim();
                        Infect.PATNT_TELNO = dt.Rows[0]["TEL"].ToString().Trim();
                        Infect.PATNT_MBTLNUM = dt.Rows[0]["HPHONE"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "SELECT JUSO_1, JUSO_2 || ' ' || JUSO_3  JUSO_2, ZIPCODE1 || ZIPCODE2 ZIPCODE";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "VIEW_PATIENT_JUSO";
                    //SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + Infect.PATNT_REGIST_NO + "'";
                    SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + txtPano.Text.Trim() + "'";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        Infect.PATNT_RN_ZIP = dt.Rows[0]["ZIPCODE"].ToString().Trim();
                        Infect.PATNT_RDNMADR = dt.Rows[0]["JUSO_1"].ToString().Trim();
                        Infect.PATNT_RDNMADR_DTL = dt.Rows[0]["JUSO_2"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[12, 1].Value) == true)
                    {
                        Infect.RESDNC_INDSTNCT_AT = "Y";
                    }
                    else
                    {
                        Infect.RESDNC_INDSTNCT_AT = "N";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[12, 5].Value) == true)
                    {
                        Infect.IDNTY_UKNWN_AT = "Y";
                    }
                    else
                    {
                        Infect.IDNTY_UKNWN_AT = "N";
                    }

                    Infect.PATNT_OCCP_CD = VB.Left(ssView2_Sheet1.Cells[8, 7].Text, 1);

                    //strOccp_dtl_info 직업 상세 필수 아님

                    //제1급
                    strInfect1 = "";

                    for (i = 15; i < 21; i++)
                    {
                        for (j = 1; j <= 3; j++)
                        {
                            if (Convert.ToBoolean(ssView2_Sheet1.Cells[i - 1, (j * 3) - 1].Value) == true)
                            {
                                strInfect1 = READ_BCODE_Name3("INFECT_법정전염병", ((FarPoint.Win.Spread.CellType.CheckBoxCellType)(ssView2_Sheet1.Cells[i - 1, (j * 3) - 1].CellType)).Caption);
                            }
                        }
                    }

                    //제2급
                    strinfect2 = "";

                    for (i = 21; i < 29; i++)
                    {
                        for (j = 1; j <= 3; j++)
                        {
                            if (i == 24 && (j * 3) - 1 == 2)
                            {

                            }
                            else
                            {
                                if (Convert.ToBoolean(ssView2_Sheet1.Cells[i - 1, (j * 3) - 1].Value) == true)
                                {
                                    strinfect2 = READ_BCODE_Name3("INFECT_법정전염병", ((FarPoint.Win.Spread.CellType.CheckBoxCellType)(ssView2_Sheet1.Cells[i - 1, (j * 3) - 1].CellType)).Caption);
                                }
                            }
                        }
                    }

                    //제3군
                    strinfect3 = "";

                    for (i = 29; i < 38; i++)
                    {
                        for (j = 1; j <= 3; j++)
                        {
                            if (Convert.ToBoolean(ssView2_Sheet1.Cells[i - 1, (j * 3) - 1].Value) == true)
                            {
                                strinfect3 = READ_BCODE_Name3("INFECT_법정전염병", ((FarPoint.Win.Spread.CellType.CheckBoxCellType)(ssView2_Sheet1.Cells[i - 1, (j * 3) - 1].CellType)).Caption);
                            }
                        }
                    }

                    if (strInfect1 != "")
                    {
                        Infect.ICDGRP_CD = "01";
                        Infect.ICD_CD = strInfect1;
                    }

                    if (strinfect2 != "")
                    {
                        Infect.ICDGRP_CD = "02";
                        Infect.ICD_CD = strinfect2;
                    }

                    if (strinfect3 != "")
                    {
                        Infect.ICDGRP_CD = "03";
                        Infect.ICD_CD = strinfect3;
                    }

                    Infect.ATFSS_DE = ssView2_Sheet1.Cells[37, 1].Text.Replace("-", "");
                    Infect.DGNSS_DE = ssView2_Sheet1.Cells[37, 5].Text.Replace("-", "");
                    Infect.STTEMNT_DE = ssView2_Sheet1.Cells[37, 9].Text.Replace("-", "");


                    //확진검사결과
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[38, 1].Value) == true)
                    {
                        Infect.DSNDGNSS_INSPCT_RESULT_TY_CD = "1";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[38, 3].Value) == true)
                    {
                        Infect.DSNDGNSS_INSPCT_RESULT_TY_CD = "2";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[38, 5].Value) == true)
                    {
                        Infect.DSNDGNSS_INSPCT_RESULT_TY_CD = "3";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[38, 7].Value) == true)
                    {
                        Infect.DSNDGNSS_INSPCT_RESULT_TY_CD = "4";
                    }


                    //입원유형코드
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[39, 7].Value) == true)
                    {
                        Infect.HSPTLZ_TY_CD = "2";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[39, 8].Value) == true)
                    {
                        Infect.HSPTLZ_TY_CD = "1";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[40, 7].Value) == true)
                    {
                        Infect.HSPTLZ_TY_CD = "3";
                    }

                    //환자분류코드
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[39, 1].Value) == true)
                    {
                        Infect.PATNT_CL_CD = "1";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[39, 2].Value) == true)
                    {
                        Infect.PATNT_CL_CD = "2";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[40, 1].Value) == true)
                    {
                        Infect.PATNT_CL_CD = "3";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[40, 3].Value) == true)
                    {
                        Infect.PATNT_CL_CD = "4";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[39, 3].Value) == true)
                    {
                        Infect.PATNT_CL_CD = "5";
                    }

                    //사망여부
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[45, 1].Value) == true)
                    {
                        Infect.DEATH_AT_CD = "N";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[45, 2].Value) == true)
                    {
                        Infect.DEATH_AT_CD = "Y";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[45, 1].Value) == false && Convert.ToBoolean(ssView2_Sheet1.Cells[45, 2].Value) == false)
                    {
                        Infect.DEATH_AT_CD = "N";
                    }

                    Infect.DOCTR_NM = ssView2_Sheet1.Cells[48, 3].Text.Trim();

                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }

                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
            }
            else if (ChkAutoSin.Checked == true)
            {
                try
                {


                    Infect = new clsInFc();
                    //본서버
                    Infect.OGCR = "cn=(재)포항성모병원,ou=건강보험,ou=MOHW RA센터,ou=등록기관,ou=licensedCA,o=KICA,c=KR";
                    //개발서버
                    //Infect.OGCR = "cn=포항성모병원,ou=경상북도,ou=포항시,ou=질병관리본부,o=CDC,c=KR";
                    Infect.OGCR = Infect.OGCR.Replace("`", "''");

                    Infect.PATNT_NM = ssView2_Sheet1.Cells[7, 2].Text;
                    Infect.PATNT_IHIDNUM = ssView2_Sheet1.Cells[7, 8].Text.Trim().Replace("-", "");

                    Infect.PATNT_REGIST_NO = ssView2_Sheet1.Cells[0, 1].Text.Trim();

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[8, 10].Value) == true)
                    {
                        Infect.PATNT_SEXDSTN_CD = "1";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[9, 10].Value) == true)
                    {
                        Infect.PATNT_SEXDSTN_CD = "2";
                    }

                    SQL = "";
                    SQL = "SELECT SNAME, TEL, HPHONE";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                    SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + txtPano.Text.Trim() + "'";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        Infect.PATNT_NM = dt.Rows[0]["SNAME"].ToString().Trim();
                        Infect.PATNT_TELNO = dt.Rows[0]["TEL"].ToString().Trim();
                        Infect.PATNT_MBTLNUM = dt.Rows[0]["HPHONE"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "SELECT JUSO_1, JUSO_2 || ' ' || JUSO_3  JUSO_2, ZIPCODE1 || ZIPCODE2 ZIPCODE";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "VIEW_PATIENT_JUSO";
                    //SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + Infect.PATNT_REGIST_NO + "'";
                    SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + txtPano.Text.Trim() + "'";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        Infect.PATNT_RN_ZIP = dt.Rows[0]["ZIPCODE"].ToString().Trim();
                        Infect.PATNT_RDNMADR = dt.Rows[0]["JUSO_1"].ToString().Trim();
                        Infect.PATNT_RDNMADR_DTL = dt.Rows[0]["JUSO_2"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[12, 1].Value) == true)
                    {
                        Infect.RESDNC_INDSTNCT_AT = "Y";
                    }
                    else
                    {
                        Infect.RESDNC_INDSTNCT_AT = "N";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[12, 5].Value) == true)
                    {
                        Infect.IDNTY_UKNWN_AT = "Y";
                    }
                    else
                    {
                        Infect.IDNTY_UKNWN_AT = "N";
                    }

                    Infect.PATNT_OCCP_CD = VB.Left(ssView2_Sheet1.Cells[8, 7].Text, 1);

                    //strOccp_dtl_info 직업 상세 필수 아님

                    //제1급
                    strInfect1 = "";

                    for (i = 15; i < 21; i++)
                    {
                        for (j = 1; j <= 3; j++)
                        {
                            if (Convert.ToBoolean(ssView2_Sheet1.Cells[i - 1, (j * 3) - 1].Value) == true)
                            {
                                strInfect1 = READ_BCODE_Name3("INFECT_법정전염병", ((FarPoint.Win.Spread.CellType.CheckBoxCellType)(ssView2_Sheet1.Cells[i - 1, (j * 3) - 1].CellType)).Caption);
                            }
                        }
                    }

                    //제2급
                    strinfect2 = "";

                    for (i = 21; i < 29; i++)
                    {
                        for (j = 1; j <= 3; j++)
                        {
                            if (i == 24 && (j * 3) - 1 == 2)
                            {

                            }
                            else
                            {
                                if (Convert.ToBoolean(ssView2_Sheet1.Cells[i - 1, (j * 3) - 1].Value) == true)
                                {
                                    strinfect2 = READ_BCODE_Name3("INFECT_법정전염병", ((FarPoint.Win.Spread.CellType.CheckBoxCellType)(ssView2_Sheet1.Cells[i - 1, (j * 3) - 1].CellType)).Caption);
                                }
                            }
                        }
                    }

                    //제3군
                    strinfect3 = "";

                    for (i = 29; i < 38; i++)
                    {
                        for (j = 1; j <= 3; j++)
                        {
                            if (Convert.ToBoolean(ssView2_Sheet1.Cells[i - 1, (j * 3) - 1].Value) == true)
                            {
                                strinfect3 = READ_BCODE_Name3("INFECT_법정전염병", ((FarPoint.Win.Spread.CellType.CheckBoxCellType)(ssView2_Sheet1.Cells[i - 1, (j * 3) - 1].CellType)).Caption);
                            }
                        }
                    }

                    if (strInfect1 != "")
                    {
                        Infect.ICDGRP_CD = "01";
                        Infect.ICD_CD = strInfect1;
                    }

                    if (strinfect2 != "")
                    {
                        Infect.ICDGRP_CD = "02";
                        Infect.ICD_CD = strinfect2;
                    }

                    if (strinfect3 != "")
                    {
                        Infect.ICDGRP_CD = "03";
                        Infect.ICD_CD = strinfect3;
                    }

                    Infect.ATFSS_DE = ssView2_Sheet1.Cells[37, 1].Text.Replace("-", "");
                    Infect.DGNSS_DE = ssView2_Sheet1.Cells[37, 5].Text.Replace("-", "");
                    Infect.STTEMNT_DE = ssView2_Sheet1.Cells[37, 9].Text.Replace("-", "");


                    //확진검사결과
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[38, 1].Value) == true)
                    {
                        Infect.DSNDGNSS_INSPCT_RESULT_TY_CD = "1";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[38, 3].Value) == true)
                    {
                        Infect.DSNDGNSS_INSPCT_RESULT_TY_CD = "2";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[38, 5].Value) == true)
                    {
                        Infect.DSNDGNSS_INSPCT_RESULT_TY_CD = "3";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[38, 7].Value) == true)
                    {
                        Infect.DSNDGNSS_INSPCT_RESULT_TY_CD = "4";
                    }


                    //입원유형코드
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[39, 7].Value) == true)
                    {
                        Infect.HSPTLZ_TY_CD = "2";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[39, 8].Value) == true)
                    {
                        Infect.HSPTLZ_TY_CD = "1";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[40, 7].Value) == true)
                    {
                        Infect.HSPTLZ_TY_CD = "3";
                    }

                    //환자분류코드
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[39, 1].Value) == true)
                    {
                        Infect.PATNT_CL_CD = "1";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[39, 2].Value) == true)
                    {
                        Infect.PATNT_CL_CD = "2";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[40, 1].Value) == true)
                    {
                        Infect.PATNT_CL_CD = "3";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[40, 3].Value) == true)
                    {
                        Infect.PATNT_CL_CD = "4";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[39, 3].Value) == true)
                    {
                        Infect.PATNT_CL_CD = "5";
                    }


                    //사망여부
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[45, 1].Value) == true)
                    {
                        Infect.DEATH_AT_CD = "N";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[45, 2].Value) == true)
                    {
                        Infect.DEATH_AT_CD = "Y";
                    }

                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[45, 1].Value) == false && Convert.ToBoolean(ssView2_Sheet1.Cells[45, 2].Value) == false)
                    {
                        Infect.DEATH_AT_CD = "N";
                    }

                    Infect.DOCTR_NM = ssView2_Sheet1.Cells[48, 3].Text.Trim();

                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }

                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }

                READERROR = READ_ERROR(sendData());

                if (READERROR == "2001" || READERROR == "4005")
                {
                  Cursor.Current = Cursors.WaitCursor;
                  clsDB.setBeginTran(clsDB.DbCon);

                    try
                    {
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "NUR_STD_INFECT2 SET ";
                        SQL = SQL + ComNum.VBLF + "  AUTOSEND = SYSDATE,  ";
                        SQL = SQL + ComNum.VBLF + "  AUTOSENDSABUN = " + clsType.User.Sabun;
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + GstrROWID + "' ";

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
            }
        }
        private string sendData()
        {
            string strUri = "https://152.99.73.139:8443/indigo/InfctnRgstr";
            string strICDGRP_CD = "";

            StringBuilder dataParams = new StringBuilder();

            dataParams.Append("dplct_at=1");           // 0:테스트 전송, 1:실제 전송
            //dataParams.Append("dplct_at=0");           // 0:테스트 전송, 1:실제 전송

            dataParams.AppendLine("&rspns_mssage_ty=0");    // 0:XML, 1:JSON

            //dataParams.AppendLine("&ogcr=" + StringToUTF8(Infect.OGCR));
            dataParams.AppendLine("&ogcr=" + Infect.OGCR);
            dataParams.AppendLine("&patnt_nm=" + Infect.PATNT_NM);
            dataParams.AppendLine("&patnt_ihidnum=" + Infect.PATNT_IHIDNUM);
            dataParams.AppendLine("&patnt_regist_no=" + Infect.PATNT_REGIST_NO);
            dataParams.AppendLine("&prtctor_nm=" + Infect.PRTCTOR_NM);
            dataParams.AppendLine("&patnt_sexdstn_cd=" + Infect.PATNT_SEXDSTN_CD);
            dataParams.AppendLine("&patnt_telno=" + Regex.Replace(Infect.PATNT_TELNO, @"\D", ""));
            dataParams.AppendLine("&patnt_mbtlnum=" + Regex.Replace(Infect.PATNT_MBTLNUM, @"\D", ""));
            dataParams.AppendLine("&patnt_rn_zip=" + Infect.PATNT_RN_ZIP);
            dataParams.AppendLine("&patnt_rdnmadr=" + Infect.PATNT_RDNMADR);
            dataParams.AppendLine("&patnt_rdnmadr_dtl=" + Infect.PATNT_RDNMADR_DTL);
            dataParams.AppendLine("&resdnc_indstnct_at=" + Infect.RESDNC_INDSTNCT_AT);
            dataParams.AppendLine("&idnty_uknwn_at=" + Infect.IDNTY_UKNWN_AT);
            dataParams.AppendLine("&patnt_occp_cd=" + Infect.PATNT_OCCP_CD);
            dataParams.AppendLine("&occp_dtl_info=" + Infect.OCCP_DTL_INFO);
            strICDGRP_CD = Infect.ICDGRP_CD;
            //dataParams.AppendLine("&icdgrp_cd=" + Infect.ICDGRP_CD);
            dataParams.AppendLine("&icdgrp_cd=" + strICDGRP_CD);

            dataParams.AppendLine("&icd_cd=" + Infect.ICD_CD);
            //dataParams.AppendLine("&eids_symptms=" + Infect.EIDS_SYMPTMS);
            if (Infect.ATFSS_DE == "")
            {
                Infect.ATFSS_DE = "00000000";
            }

            dataParams.AppendLine("&atfss_de=" + Infect.ATFSS_DE);

            dataParams.AppendLine("&dgnss_de=" + Infect.DGNSS_DE);
            dataParams.AppendLine("&sttemnt_de=" + Infect.STTEMNT_DE);
            dataParams.AppendLine("&dsndgnss_inspct_result_ty_cd=" + Infect.DSNDGNSS_INSPCT_RESULT_TY_CD);
            dataParams.AppendLine("&hsptlz_ty_cd=" + Infect.HSPTLZ_TY_CD);
            dataParams.AppendLine("&patnt_cl_cd=" + Infect.PATNT_CL_CD);
            dataParams.AppendLine("&death_at_cd=" + Infect.DEATH_AT_CD);  //사망여부

            //dataParams.AppendLine("&rm_info=" + Infect.RM_INFO);  //기타 특이사항


            Infect.MDLCNST_KCN_INSTT_ID = "37100068";
            Infect.HSPTL_SWBSER = "자체개발";
            Infect.HSPTL_SWKND = "1";

            dataParams.AppendLine("&mdlcnst_kcn_instt_id=" + Infect.MDLCNST_KCN_INSTT_ID);
            dataParams.AppendLine("&doctr_nm=" + Infect.DOCTR_NM);
            dataParams.AppendLine("&hsptl_swbser=" + Infect.HSPTL_SWBSER);
            dataParams.AppendLine("&hsptl_swknd=" + Infect.HSPTL_SWKND);
            //dataParams.AppendLine("&paratyphoid_germ_info=" + Infect.PARATYPHOID_GERM_INFO);
            //dataParams.AppendLine("&dysentery_germ_info=" + Infect.DYSENTERY_GERM_INFO);
            //dataParams.AppendLine("&entgerm_germ_info=" + Infect.ENTGERM_GERM_INFO);
            //dataParams.AppendLine("&entgerm_germ_etc_info=" + Infect.ENTGERM_GERM_ETC_INFO);
            //dataParams.AppendLine("&scrbtyph_inspct_mth=" + Infect.SCRBTYPH_INSPCT_MTH);
            //dataParams.AppendLine("&scrbtyph_inspct_rate=" + Infect.SCRBTYPH_INSPCT_RATE);
            //dataParams.AppendLine("&scrbtyph_inspct_rate_etc=" + Infect.SCRBTYPH_INSPCT_RATE_ETC);
            //dataParams.AppendLine("&sfts_tick_bite=" + Infect.SFTS_TICK_BITE);
            //dataParams.AppendLine("&sfts_symptms=" + Infect.SFTS_SYMPTMS);
            //dataParams.AppendLine("&sfts_hsptlz_info=" + Infect.SFTS_HSPTLZ_INFO);
            //dataParams.AppendLine("&rabies_bite_info=" + Infect.RABIES_BITE_INFO);
            //dataParams.AppendLine("&rabies_hsptlz_info=" + Infect.RABIES_HSPTLZ_INFO);
            //dataParams.AppendLine("&rabies_trtmnt=" + Infect.RABIES_TRTMNT);


            // 요청 String -> 요청 Byte 변환
            byte[] byteDataParams = Encoding.UTF8.GetBytes(dataParams.ToString());


            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;

            // HttpWebRequest 객체 생성, 설정

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUri);
            request.Method = "POST";    // 기본값 "GET"
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;


            // 요청 Byte -> 요청 Stream 변환
            Stream stDataParams = request.GetRequestStream();
            stDataParams.Write(byteDataParams, 0, byteDataParams.Length);
            stDataParams.Close();

            // 요청, 응답 받기
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // 응답 Stream 읽기
            Stream stReadData = response.GetResponseStream();
            StreamReader srReadData = new StreamReader(stReadData, Encoding.GetEncoding("utf-8"));

            // 응답 Stream -> 응답 String 변환
            //  수신 메시지 (4.3 수신 메시지 구성 참고)  
            string strResult = srReadData.ReadToEnd();

            return strResult;
        }

        private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
        {
            // all Certificates are accepted
            return true;
        }


        private string READ_BCODE_Name3(string ArgGubun, string ArgName)
        {
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT GUBUN2 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun='" + ArgGubun + "' ";
                SQL = SQL + ComNum.VBLF + "  AND Name LIKE '" + ArgName.Trim() + "%'";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    rtnVal = "";
                }
                else
                {
                    rtnVal = dt.Rows[0]["GUBUN2"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string READ_ERROR(string arg)
        {
            string rtnVal = "";
            string strCODE = "";

            strCODE = VB.Mid(arg, VB.InStr(arg, "<code_dt>") + 9, 4);

            switch (strCODE)
            {
                case "3001":
                    ComFunc.MsgBox("환자 성명은 필수 입력사항입니다.");
                    break;
                case "3002":
                    ComFunc.MsgBox("주민등록번호는 필수 입력사항입니다.");
                    break;
                case "3003":
                    ComFunc.MsgBox("직업은 필수 입력사항입니다.");
                    break;
                case "3004":
                    ComFunc.MsgBox("환자연락처는 필수 입력사항입니다." + ComNum.VBLF + "전화번호 또는 이동전화번호를 입력하세요.");
                    break;
                case "3005":
                    ComFunc.MsgBox("발병일은은 필수 입력사항입니다.");
                    break;
                case "3006":
                    ComFunc.MsgBox("진단일은 필수 입력사항입니다.");
                    break;
                case "3007":
                    ComFunc.MsgBox("확진검사결과는 필수 입력사항입니다.");
                    break;
                case "3008":
                    ComFunc.MsgBox("환자분류는 필수 입력사항입니다.");
                    break;
                case "3009":
                    ComFunc.MsgBox("진단의사 성명은 필수 입력사항입니다.");
                    break;
                case "3010":
                    ComFunc.MsgBox("감염병코드는 필수 입력사항입니다.");
                    break;
                case "4001":
                    ComFunc.MsgBox("중복 전송된 감염병 발생 신고입니다." + ComNum.VBLF + "감염병 발생신고가 가능합니다.");
                    break;
                case "4002":
                    ComFunc.MsgBox("환자 전화번호가 잘못되었습니다.");
                    break;
                case "4003":
                    ComFunc.MsgBox("발병일은 신고일보다 빨라야합니다.");
                    break;
                case "4004":
                    ComFunc.MsgBox("발병일은 진단일보다 빨라야합니다.");
                    break;
                case "4005":
                    ComFunc.MsgBox("진단일은 신고일보다 같거나 빨라야합니다.");
                    break;
                case "4006":
                    ComFunc.MsgBox("성별코드는 1 또는 2이어야 합니다.");
                    break;
                case "4007":
                    ComFunc.MsgBox("사망여부코드는 Y 또는 N 이어야 합니다.");
                    break;
                case "4008":
                    ComFunc.MsgBox("신원미상여부코드는 Y 또는 N 이어야 합니다.");
                    break;
                case "4009":
                    ComFunc.MsgBox("거주지불명여부코드는 Y 또는 N 이어야 합니다.");
                    break;
                case "4010":
                    ComFunc.MsgBox("입원유형코드는 1, 2, 또는 3 중 하나여야 합니다.");
                    break;
                case "4017":
                    ComFunc.MsgBox("환자분류는 1,2,3,4중 하나여야 합니다.");
                    break;
                case "4999":
                    ComFunc.MsgBox("사망신고는 자동신고에서 등록할 수 없습니다. 웹 신고를 통해서 진행하여 주세요.");
                    break;
                case "5001":
                    ComFunc.MsgBox("사용자(기관) 인증정보가 웹신고시스템에 등록된 인증정보와 같지 않습니다.");
                    break;
                case "2001":
                    ComFunc.MsgBox("전송에 성공하였습니다.", "확인");
                    break;
                default:
                    ComFunc.MsgBox("전송 에러 코드 : " + strCODE + " 전산정보팀 연락 바랍니다.");
                    break;
            }

            rtnVal = strCODE;
            return rtnVal;
        }

        private void SaveAUTOSEND()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "NUR_STD_INFECT2";
                SQL = SQL + ComNum.VBLF + "     SET ";
                SQL = SQL + ComNum.VBLF + "         AUTOSEND = SYSDATE,  ";
                SQL = SQL + ComNum.VBLF + "         AUTOSENDSABUN = " + clsType.User.Sabun;
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + GstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ChkAutoSin.Checked = true;
            SCREEN_CLEAR();
            GetData();
            btnDelete.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int i = 0;
           
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            if (string.IsNullOrEmpty(GstrROWID) == false)
            {
                if (ComFunc.MsgBoxQ("정말로 삭제하시겠습니까? 삭제한 신고서는 복구 불가능합니다.", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                try
                {
                    if (string.IsNullOrEmpty(lbAutogu.Text) == true)
                    {

                        clsDB.setBeginTran(clsDB.DbCon);

                    SQL = "";
                    SQL = "DELETE " + ComNum.DB_PMPA + "NUR_STD_INFECT2  WHERE ROWID = '" + GstrROWID + "'";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            lbAutogu.Text = "";
                            SCREEN_CLEAR();
                            GetData();
                            btnDelete.Enabled = false;
                            return;
                        }
                    }

                    else
                    {
                        ComFunc.MsgBox("감염병 신고서가 질병관리본부로 자동전송 시간대에 작성되거나, 자동전송 시간대가 아닌 경우 감염관리실에서 신고서를 수동전송한 경우는 삭제가 되지 않습니다. 삭제가 필요한 경우 감염관리실로 연락하십시오.");
                        lbAutogu.Text = "";
                        SCREEN_CLEAR();
                        GetData();
                        btnDelete.Enabled = false;
                        return;

                    }
                    

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("삭제하였습니다.");
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    lbAutogu.Text = "";
                    SCREEN_CLEAR();
                    GetData();
                    btnDelete.Enabled = false;
                    return;
                }


            }
            else
            {
                ComFunc.MsgBox("선택된 신고서가 없습니다. 더블클릭하여 삭제하고자 하는 신고서를 선택하여 주세요.");
                return;
            }
         
        }

        private void GetData()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.IPDNO, A.ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT2 A ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }


        private void GetData2()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.IPDNO, A.ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT2 A ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "     AND A.AUTOSEND IS NULL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("환자 등록중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GstrROWID = dt.Rows[i]["ROWID"].ToString().Trim();
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
        

        private void btnSave_Click(object sender, EventArgs e)
        {
            int InfectChkCount = 0;

            for (int i = 15; i < 21; i++)
            {
                for (int k = 2; k <= 8; k = k + 3)
                {
                    if (ssView2_Sheet1.Cells[i - 1, k].CellType != null)
                    {
                        if (ssView2_Sheet1.Cells[i - 1, k].CellType.ToString() == "CheckBoxCellType")
                        {
                            if (Convert.ToBoolean(ssView2_Sheet1.Cells[i - 1, k].Value) == true)
                            {
                                InfectChkCount = InfectChkCount + 1;
                            }
                        }
                    }
                }
            }


            for (int i = 21; i < 29; i++)
            {
                for (int k = 2; k <= 8; k = k + 3)
                {
                    if (ssView2_Sheet1.Cells[i - 1, k].CellType != null)
                    {
                        if (ssView2_Sheet1.Cells[i - 1, k].CellType.ToString() == "CheckBoxCellType")
                        {
                            if (Convert.ToBoolean(ssView2_Sheet1.Cells[i - 1, k].Value) == true)
                            {
                                InfectChkCount = InfectChkCount + 1;
                            }
                        }
                    }
                }
            }
            

            for (int i = 29; i < 38; i++)
            {
                for (int k = 2; k <= 8; k = k + 3)
                {
                    if (ssView2_Sheet1.Cells[i - 1, k].CellType != null)
                    {
                        if (ssView2_Sheet1.Cells[i - 1, k].CellType.ToString() == "CheckBoxCellType")
                        {
                            if (Convert.ToBoolean(ssView2_Sheet1.Cells[i - 1, k].Value) == true)
                            {
                                InfectChkCount = InfectChkCount + 1;
                            }
                        }
                    }
                }
            }

            if (InfectChkCount > 1)
            {
                ComFunc.MsgBox("선택한 감염병이 2가지 입니다. 2가지 이상 감염병 신고를 원할 경우 각각 작성해 주시기 바랍니다.");
                return;
            }
            if (ssView2.ActiveSheet.Cells[38,5].Text == "True" && ssView2.ActiveSheet.Cells[40, 3].Text == "True")
            {
                ComFunc.MsgBox("확진검사결과가 진행중일 경우 환자분류가 그 밖의 경우(환자아님)일 수 없습니다. 다시 선택해주세요.");
                return;
            }
            else if(ssView2.ActiveSheet.Cells[38, 5].Text == "True" && ssView2.ActiveSheet.Cells[39, 1].Text == "True")
            {
                ComFunc.MsgBox("확진검사결과가 진행중일 경우 환자분류가 환자(확진)일 수 없습니다. 다시 선택해주세요.");
                return;
            }

            if (ssView2_Sheet1.Cells[19, 5].Text == "True")
            {
                if (ssView2.ActiveSheet.Cells[38, 7].Text == "True" && ssView2.ActiveSheet.Cells[39, 2].Text == "True")
                {
                    ComFunc.MsgBox("확진검사결과가 미실시일 경우 환자분류가 의사환자일 수 없습니다. 다시 선택해주세요.");
                    return;
                }
                if (ssView2.ActiveSheet.Cells[38, 7].Text == "True" && ssView2.ActiveSheet.Cells[39, 1].Text == "True")
                {
                    ComFunc.MsgBox("확진검사결과가 미실시일 경우 환자분류가 환자(확진)일 수 없습니다. 다시 선택해주세요.");
                    return;
                }
            }

            if (string.IsNullOrEmpty(ssView2_Sheet1.Cells[11, 9].Text) == true)
            {
                    ComFunc.MsgBox("국적은 빈칸일 수 없습니다. 국적을 작성하여 주세요.");
                    return;
            }

            if (SaveData() == true) 
            {

                //2020-03-11 김욱동. 신종감염 관련 신고서 등록 하면 자동 전송 되도록 보완 요청
                //2020-04-02 KUD 자동신고 막음 요청.
                //2020-06-13 KUD 자동신고 다시 품.
                string stryoil = "";
                bool GuHuIl = false;
                string strTime = cpublic.strSysTime;
                stryoil = CF.READ_YOIL(clsDB.DbCon, cpublic.strSysDate);

                GuHuIl = clsVbfunc.ChkDateHuIl(clsDB.DbCon, cpublic.strSysDate);

                if (((stryoil == "월요일" || stryoil == "화요일" || stryoil == "수요일" || stryoil == "목요일" || stryoil == "금요일") && (string.Compare(strTime, "17:30") > 0 || string.Compare(strTime, "08:30") < 0)) || (stryoil == "토요일" && (string.Compare(strTime, "12:30") > 0 || string.Compare(strTime, "08:30") < 0)) || (stryoil == "일요일") || (GuHuIl == true))
                //if (((stryoil == "월요일" || stryoil == "화요일" || stryoil == "수요일" || stryoil == "목요일" || stryoil == "금요일") && (string.Compare(strTime, "17:30") > 0 || string.Compare(strTime, "08:30") < 0)) || (stryoil == "토요일") || (stryoil == "일요일"))
                {
                    //if (ssView2_Sheet1.Cells[19, 5].Text == "True")
                   // {
                        toolWebsend.PerformClick();
                    //}
                }
                SCREEN_CLEAR();
            }
            else
            {
                return;
            }
           
            GetData();
            btnDelete.Enabled = false;

            //2020-11-06 추가
            if (GstrGbnER == "ER")
            {
                rSendText("OK");
                rEventClosed();
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            int k = 0;

            string strSName = "";
            string strInfect1 = "";
            string strinfect2 = "";
            string strinfect3 = "";
            string strinfect4 = "";
            string strInfect1ETC = "";
            string strinfect4ETC = "";
            string strinfect5 = "";
            string strinfect5ETC = "";
            string strBDATE = "";
            string strJDate = "";
            string strSDate = "";      //신고일
            string strExResult = "";
            string strPatBun = "";
            string strSaMang = "";
            string strSaMangDTL = "";
            string strExPath = "";
            string strExRegion = "";
            string strNation = "";
            string strDay = "";
            string strDayFR = "";
            string strDayTO = "";
            string strGbio = "";
            string strRemark = "";  //비고(특이사항)
            string strChange = "";
            string strChangeETC = "";
            string strSex = "";
            string strAge = "";
            string strJuso1 = "";
            string strJuso2 = "";
            string strExNotPat = "";
            string strJobName = "";
            string strJumin = "";
            string strTel = "";
            string strJuso = "";
            string strDrname = "";
            string strGBNEW = "";
            string strNational = "";

            if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록 환자의 등록번호를 조회 후 작업해주세요.");
                return rtnVal;
            }

            GstrDept = VB.Left(cboDept.Text, 2);
            GstrDrSabun = clsVbfunc.GetOCSDrCodeSabun(clsDB.DbCon, VB.Left(cboDr.Text, 4));

            if (string.IsNullOrEmpty(GstrDrSabun) == true || VB.Left(cboDr.Text, 4) == "1199")
            {
                if (VB.Len(cboDr.Text) != 8)
                {
                    if (GstrDept == "**")
                    {
                        GstrDrSabun = clsVbfunc.GetOCSDrCodeNaSabun(clsDB.DbCon, VB.Right(cboDr.Text, 2));
                    }
                    else
                    {
                        GstrDrSabun = clsVbfunc.GetOCSDrCodeNaSabun(clsDB.DbCon, VB.Right(cboDr.Text, 2), GstrDept);
                    }

                }
                else
                {
                    if (GstrDept == "**")
                    {
                        GstrDrSabun = clsVbfunc.GetOCSDrCodeNaSabun(clsDB.DbCon, VB.Right(cboDr.Text, 3));
                    }
                    else
                    {
                        GstrDrSabun = clsVbfunc.GetOCSDrCodeNaSabun(clsDB.DbCon, VB.Right(cboDr.Text, 3), GstrDept);
                    }
                }
            }


            GstrWard = txtWard.Text;
            GstrRoom = txtRoom.Text;

            strNational = ssView2_Sheet1.Cells[11, 9].Text.Trim();

            //주민번호
            strJumin = ssView2_Sheet1.Cells[7, 8].Text.Trim();
            
            //휴대폰번호
            strTel = ssView2_Sheet1.Cells[9, 2].Text.Trim();

            //주소
            strJuso = ssView2_Sheet1.Cells[10, 2].Text.Trim();

            //이름
            strSName = ssView2_Sheet1.Cells[7, 2].Text.Trim();

            //직업
            strJobName = ssView2_Sheet1.Cells[8, 7].Text.Trim();

            strSex = Convert.ToBoolean(ssView2_Sheet1.Cells[8, 10].Value) == true ? "M" : strSex;
            strSex = Convert.ToBoolean(ssView2_Sheet1.Cells[9, 10].Value) == true ? "F" : strSex;

            //거주지 불명
            strJuso1 = Convert.ToBoolean(ssView2_Sheet1.Cells[12, 1].Value) == true ? "Y" : strJuso1;

            //신원미상
            strJuso2 = Convert.ToBoolean(ssView2_Sheet1.Cells[12, 5].Value) == true ? "Y" : strJuso2;

            //제1급
            strInfect1 = "";

            for (i = 15; i < 21; i++)
            {
                for (k = 2; k <= 8; k = k + 3)
                {
                    if (ssView2_Sheet1.Cells[i - 1, k].CellType != null)
                    {
                        if (ssView2_Sheet1.Cells[i - 1, k].CellType.ToString() == "CheckBoxCellType")
                        {
                            if (Convert.ToBoolean(ssView2_Sheet1.Cells[i - 1, k].Value) == true)
                            {
                                strInfect1 = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "2", "INFECT_법정전염병", ((FarPoint.Win.Spread.CellType.CheckBoxCellType)ssView2_Sheet1.Cells[i - 1, k].CellType).Caption);
                            }
                        }
                    }
                }
            }

            strInfect1ETC = ssView2_Sheet1.Cells[19, 9].Text; //증상 및 징후

            //제2급
            strinfect2 = "";

            for (i = 21; i < 29; i++)
            {
                for (k = 2; k <= 8; k = k + 3)
                {
                    if (ssView2_Sheet1.Cells[i - 1, k].CellType != null)
                    {
                        if (ssView2_Sheet1.Cells[i - 1, k].CellType.ToString() == "CheckBoxCellType")
                        {
                            if (Convert.ToBoolean(ssView2_Sheet1.Cells[i - 1, k].Value) == true)
                            {
                                strinfect2 = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "2", "INFECT_법정전염병", ((FarPoint.Win.Spread.CellType.CheckBoxCellType)ssView2_Sheet1.Cells[i - 1, k].CellType).Caption);
                            }
                        }
                    }
                }
            }

            //제3급
            strinfect3 = "";

            for (i = 29; i < 38; i++)
            {
                for (k = 2; k <= 8; k = k + 3)
                {
                    if (ssView2_Sheet1.Cells[i - 1, k].CellType != null)
                    {
                        if (ssView2_Sheet1.Cells[i - 1, k].CellType.ToString() == "CheckBoxCellType")
                        {
                            if (Convert.ToBoolean(ssView2_Sheet1.Cells[i - 1, k].Value) == true)
                            {
                                strinfect3 = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "2", "INFECT_법정전염병", ((FarPoint.Win.Spread.CellType.CheckBoxCellType)ssView2_Sheet1.Cells[i - 1, k].CellType).Caption);
                            }
                        }
                    }
                }
            }

            //발병일
            strBDATE = ssView2_Sheet1.Cells[37, 1].Text.Trim();

            //진단일
            strJDate = ssView2_Sheet1.Cells[37, 5].Text.Trim();

            //신고일
            strSDate = ssView2_Sheet1.Cells[37, 9].Text.Trim();

            //확진검사결과
            strExResult = Convert.ToBoolean(ssView2_Sheet1.Cells[38, 1].Value) == true ? "0" : strExResult;      //양성
            strExResult = Convert.ToBoolean(ssView2_Sheet1.Cells[38, 3].Value) == true ? "1" : strExResult;      //음성
            strExResult = Convert.ToBoolean(ssView2_Sheet1.Cells[38, 5].Value) == true ? "2" : strExResult;      //검사진행중
            strExResult = Convert.ToBoolean(ssView2_Sheet1.Cells[38, 7].Value) == true ? "3" : strExResult;      //검사미실시

            //환자등 분류
            strPatBun = Convert.ToBoolean(ssView2_Sheet1.Cells[39, 1].Value) == true ? "0" : strPatBun;        //환자
            strPatBun = Convert.ToBoolean(ssView2_Sheet1.Cells[39, 2].Value) == true ? "1" : strPatBun;        //의사환자
            strPatBun = Convert.ToBoolean(ssView2_Sheet1.Cells[40, 1].Value) == true ? "2" : strPatBun;        //병원체보유자
            strPatBun = Convert.ToBoolean(ssView2_Sheet1.Cells[40, 3].Value) == true ? "3" : strPatBun;        //그밖의 경우
            strPatBun = Convert.ToBoolean(ssView2_Sheet1.Cells[39, 3].Value) == true ? "4" : strPatBun;        //검사거부자

            //입원여부
            strGbio = Convert.ToBoolean(ssView2_Sheet1.Cells[39, 7].Value) == true ? "1" : strGbio;          //외래
            strGbio = Convert.ToBoolean(ssView2_Sheet1.Cells[39, 8].Value) == true ? "2" : strGbio;          //입원
            strGbio = Convert.ToBoolean(ssView2_Sheet1.Cells[40, 7].Value) == true ? "3" : strGbio;          //그밖의 경우

            //추정감염경로
            strExPath = Convert.ToBoolean(ssView2_Sheet1.Cells[41, 1].Value) == true ? "0" : strExPath;        //집단감염환자와 접촉
            strExPath = Convert.ToBoolean(ssView2_Sheet1.Cells[42, 1].Value) == true ? "1" : strExPath;        //개별감염환자와 접촉
            strExPath = Convert.ToBoolean(ssView2_Sheet1.Cells[43, 1].Value) == true ? "2" : strExPath;        //불확실함
            strExPath = Convert.ToBoolean(ssView2_Sheet1.Cells[43, 3].Value) == true ? "3" : strExPath;        //접촉없었음

            //추정감염지역
            strExRegion = Convert.ToBoolean(ssView2_Sheet1.Cells[41, 6].Value) == true ? "0" : strExRegion;      //국내
            strExRegion = Convert.ToBoolean(ssView2_Sheet1.Cells[42, 6].Value) == true ? "1" : strExRegion;      //국외

            strNation = ssView2_Sheet1.Cells[42, 8].Text.Trim();        //국명

            strDayFR = ssView2_Sheet1.Cells[43, 7].Text.Trim();         //체류기간
            strDayTO = ssView2_Sheet1.Cells[43, 9].Text.Trim();         //체류기간

            if (strDayFR != "" && strDayTO != "")
            {
                //strDay = VB.DateDiff("d", strDayTO, strDayFR);     //체류기간
                //19-06-04 날짜 계산 오류로 수정 함.
                strDay = (Convert.ToDateTime(strDayTO) - Convert.ToDateTime(strDayFR)).TotalDays.ToString();
            }

            //사망여부
            strSaMang = Convert.ToBoolean(ssView2_Sheet1.Cells[45, 1].Value) == true ? "0" : strSaMang;        //생존
            strSaMang = Convert.ToBoolean(ssView2_Sheet1.Cells[45, 2].Value) == true ? "1" : strSaMang;        //사망

            //strExNotPat = Convert.ToBoolean(ssView2_Sheet1.Cells[44, 6].Value) == true ? "1" : "0";            //기타(환자아님)

            //비고 특이사항
            strRemark = ssView2_Sheet1.Cells[45, 6].Text.Trim();

            //필수 입력 내용 점검
            if (strInfect1 == "" && strinfect2 == "" && strinfect3 == "" && strinfect4 == "")
            {
                ComFunc.MsgBox("감염병명을 반드시 체크 요망합니다.");
                return rtnVal;
            }

            if (strBDATE == "")
            {
                ComFunc.MsgBox("발병일을 반드시 입력 해주십시오.");
                return rtnVal;
            }

            if (strJDate == "")
            {
                ComFunc.MsgBox("진단일을 반드시 입력 해주십시오.");
                return rtnVal;
            }

            if (strExResult == "")
            {
                ComFunc.MsgBox("확진검사 결과를 반드시 입력 해주십시오.");
                return rtnVal;
            }

            if (strPatBun == "")
            {
                ComFunc.MsgBox("환자 등 분류를 반드시 입력 해주십시오.");
                return rtnVal;
            }

            if (strGbio == "")
            {
                ComFunc.MsgBox("입원여부를 반드시 입력 해주십시오.");
                return rtnVal;
            }

            if (strExPath == "")
            {
                ComFunc.MsgBox("추정감염경로를 반드시 입력 해주십시오.");
                return rtnVal;
            }

            if (strExRegion == "")
            {
                ComFunc.MsgBox("추정감염지역을 반드시 입력 해주십시오.");
                return rtnVal;
            }

            if (strSName == "")
            {
                ComFunc.MsgBox("환자 이름을 반드시 입력 해주십시오.");
                return rtnVal;
            }

            if (strJobName == "")
            {
                ComFunc.MsgBox("환자의 직업을 반드시 선택해주십시오.");
                return rtnVal;
            }

            if (strNational == "")
            {
                ComFunc.MsgBox("환자의 국적을 반드시 입력해주십시오.");
                return rtnVal;
            }

            strGBNEW = "Y";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (GstrROWID != "")
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_PMPA + "NUR_STD_INFECT2";
                    SQL = SQL + ComNum.VBLF + "     SET ";
                    SQL = SQL + ComNum.VBLF + "         INFECT1 = '" + strInfect1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT1ETC = '" + strInfect1ETC + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT2 = '" + strinfect2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT3 = '" + strinfect3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT4 = '" + strinfect4 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT4ETC = '" + strinfect4ETC + "',  ";
                    SQL = SQL + ComNum.VBLF + "         INFECT5 = '" + strinfect5 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT5ETC = '" + strinfect5ETC + "', ";
                    SQL = SQL + ComNum.VBLF + "         BDATE = '" + (strBDATE != "" ? Convert.ToDateTime(strBDATE).ToString("yyyyMMdd") : "") + "',  "; //발병일
                    SQL = SQL + ComNum.VBLF + "         JDATE = '" + (strJDate != "" ? Convert.ToDateTime(strJDate).ToString("yyyyMMdd") : "") + "',  "; //진단일
                    SQL = SQL + ComNum.VBLF + "         SDATE = '" + (strSDate != "" ? Convert.ToDateTime(strSDate).ToString("yyyyMMdd") : "") + "',  "; //신고일자   2016-01-01
                    SQL = SQL + ComNum.VBLF + "         EXRESULT = '" + strExResult + "', "; //확진검사결과
                    SQL = SQL + ComNum.VBLF + "         PATBUN = '" + strPatBun + "', ";
                    SQL = SQL + ComNum.VBLF + "         SAMANG = '" + strSaMang + "', ";
                    SQL = SQL + ComNum.VBLF + "         JOBNAME = '" + strJobName + "', ";
                    SQL = SQL + ComNum.VBLF + "         EXNOTPAT = '" + strExNotPat + "', ";    //2016-01-01
                    SQL = SQL + ComNum.VBLF + "         EXPATH = '" + strExPath + "', ";
                    SQL = SQL + ComNum.VBLF + "         EXREGION = '" + strExRegion + "',  ";
                    SQL = SQL + ComNum.VBLF + "         EXREGIONETC = '" + strNation + "', ";
                    SQL = SQL + ComNum.VBLF + "         EXREGIONDAY = '" + strDay + "', ";
                    SQL = SQL + ComNum.VBLF + "         EXREGIONDAY_FR = TO_DATE('" + strDayFR + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + "         EXREGIONDAY_TO = TO_DATE('" + strDayTO + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + "         CHANGE = '" + strChange + "', ";
                    SQL = SQL + ComNum.VBLF + "         JUSOETC = '" + strChangeETC + "', ";

                    if (GbolEXINFECT == false)
                    {
                        SQL = SQL + ComNum.VBLF + "         DRSABUN = '" + GstrDrSabun + "', ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         DRSABUN = '" + GstrDrSabun + "', ";
                        SQL = SQL + ComNum.VBLF + "         DEPTCODE = '" + GstrDept + "', ";
                        SQL = SQL + ComNum.VBLF + "         WARD = '" + GstrWard + "', ";
                        SQL = SQL + ComNum.VBLF + "         ROOM = '" + GstrRoom + "', ";
                    }

                    SQL = SQL + ComNum.VBLF + "         ENTSABUN = '" + clsType.User.Sabun + "', ";
                    SQL = SQL + ComNum.VBLF + "         ENTDATE = SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         JUSO1 = '" + strJuso1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         JUSO2 = '" + strJuso2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         GBIO = '" + strGbio + "', ";
                    SQL = SQL + ComNum.VBLF + "         GBNEW = '" + strGBNEW + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMARK  = '" + strRemark + "',";
                    SQL = SQL + ComNum.VBLF + "         SAMANGDTL  = '" + strNational + "'";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + GstrROWID + "' ";
                }
                else
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "NUR_STD_INFECT2";
                    SQL = SQL + ComNum.VBLF + "     (ACTDATE, PANO, IPDNO, SNAME, SEX, AGE,  ";
                    SQL = SQL + ComNum.VBLF + "     INFECT1, INFECT1ETC, INFECT2, INFECT3, INFECT4, INFECT4ETC, INFECT5, INFECT5ETC, ";
                    SQL = SQL + ComNum.VBLF + "     BDATE, JDATE, SDATE,EXRESULT, PATBUN, SAMANG, EXPATH, EXREGION, ";
                    SQL = SQL + ComNum.VBLF + "     EXREGIONETC, EXREGIONDAY, EXREGIONDAY_FR, EXREGIONDAY_TO,  ";
                    SQL = SQL + ComNum.VBLF + "     CHANGE, JUSOETC, DRSABUN, ENTSABUN, ENTDATE, JUSO1, JUSO2 , GBIO, REMARK , ";
                    SQL = SQL + ComNum.VBLF + "     DEPTCODE, WARD, ROOM, EXNOTPAT, GBNEW, JOBNAME, SAMANGDTL)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         TRUNC(SYSDATE), ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtPano.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + GdblIpdNO_OCS + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSName + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSex + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + GstrAge + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strInfect1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strInfect1ETC + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strinfect2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strinfect3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strinfect4 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strinfect4ETC + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strinfect5 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strinfect5ETC + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + (strBDATE != "" ? Convert.ToDateTime(strBDATE).ToString("yyyyMMdd") : "") + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + (strJDate != "" ? Convert.ToDateTime(strJDate).ToString("yyyyMMdd") : "") + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + (strSDate != "" ? Convert.ToDateTime(strSDate).ToString("yyyyMMdd") : "") + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strExResult + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPatBun + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSaMang + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strExPath + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strExRegion + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strNation + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strDay + "', ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDayFR + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDayTO + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + strChange + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strChangeETC + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + GstrDrSabun + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strJuso1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strJuso2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGbio + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strRemark + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + GstrDept + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + GstrWard + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + GstrRoom + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strExNotPat + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGBNEW + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strJobName + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strNational + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                if (GstrROWID != "")
                {
                    switch (clsType.User.Sabun)
                    {
                        case "49834":
                        case "39005":
                        case "44794":
                        //2018-10-08 안정수, 감염관리팀 직원 추가
                        case "33950":
                        case "41596":
                            break;
                        default:
                            clsOrderEtc.INFECT_MSGSEND_NEW(clsDB.DbCon, strInfect1, strInfect1ETC, strinfect2, strinfect3, strinfect4, strinfect4ETC, strinfect5, strinfect5ETC, txtPano.Text, GstrWard, GstrRoom, strSName, clsType.User.Sabun, strDrname, strJumin, strTel, strJobName, strJuso, strBDATE, strJDate, strSDate, strExResult, strGbio, strPatBun, strSaMang);
                            break;
                    }
                }
                else
                {
                    //clsOrderEtc.INFECT_MSGSEND(clsDB.DbCon, strInfect1, strinfect2, strinfect3, strinfect4, strinfect4ETC, strinfect5, strinfect5ETC, txtPano.Text, GstrWard, GstrRoom, strSName, clsType.User.Sabun, strDrname, strJumin, strTel, strJobName, strJuso, strBDATE, strJDate, strSDate, strExResult, strGbio, strPatBun, strSaMang);                    
                    clsOrderEtc.INFECT_MSGSEND_NEW(clsDB.DbCon, strInfect1, strInfect1ETC, strinfect2, strinfect3, strinfect4, strinfect4ETC, strinfect5, strinfect5ETC, txtPano.Text, GstrWard, GstrRoom, strSName, clsType.User.Sabun, strDrname, strJumin, strTel, strJobName, strJuso, strBDATE, strJDate, strSDate, strExResult, strGbio, strPatBun, strSaMang);
                }

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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ssView2_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView2_Sheet1.PrintInfo.Margin.Top = 20;
            ssView2_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView2_Sheet1.PrintInfo.Margin.Header = 10;
            ssView2_Sheet1.PrintInfo.ShowColor = false;
            ssView2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView2_Sheet1.PrintInfo.ShowBorder = false;
            ssView2_Sheet1.PrintInfo.ShowGrid = false;
            ssView2_Sheet1.PrintInfo.ShowShadows = false;
            ssView2_Sheet1.PrintInfo.UseMax = true;
            ssView2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView2_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView2_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView2_Sheet1.PrintInfo.Preview = false;
            ssView2.PrintSheet(0);
            btnDelete.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChkAutoSin.Checked = true;
                btnDelete.Enabled = false;
                if (txtPano.Text.Trim() == "") { return; }

                txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");
                lblSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPano.Text);

                if (lblSName.Text != "")
                {
                    GetData();
                    SCREEN_CLEAR();
                    Screen_display();
                }

            }
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsVbfunc.SetDrCodeCombo(clsDB.DbCon, cboDr, VB.Left(cboDept.Text, 2), "1", 1, "");
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ChkAutoSin.Checked = false;

            if (e.ColumnHeader == true || e.RowHeader == true) { return; }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            int intRow = 0;
            int intCol = 0;

            btnDelete.Enabled = true;

            string strBDATE = "";
            string strJDate = "";
            string strSDate = "";

            GstrROWID = ssList_Sheet1.Cells[e.Row, 3].Text.Trim();
            GdblIpdNO_OCS = VB.Val(ssList_Sheet1.Cells[e.Row, 4].Text.Trim());

            SCREEN_CLEAR();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ACTDATE, PANO, IPDNO, SNAME, SEX, AGE, INFECT1, INFECT1ETC, INFECT2, INFECT3, INFECT4, INFECT4ETC, INFECT5, ";
                SQL = SQL + ComNum.VBLF + "     INFECT5ETC, TO_DATE(BDATE,'YYYY-MM-DD') BDATE, TO_DATE(JDATE,'YYYY-MM-DD') JDATE,TO_DATE(SDATE,'YYYY-MM-DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + "     EXRESULT, PATBUN, SAMANG, EXPATH, EXREGION, EXREGIONETC, EXREGIONDAY, EXREGIONDAY_FR, EXREGIONDAY_TO, ";
                SQL = SQL + ComNum.VBLF + "     CHANGE, JUSOETC, DRSABUN, ENTSABUN, ENTDATE, JUSO1, JUSO2 , REMARK, GBIO, DEPTCODE, WARD, ROOM,EXNOTPAT,JOBNAME,SAMANGDTL,AUTOSEND";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT2 ";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + GstrROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    lbAutogu.Text = dt.Rows[0]["AUTOSEND"].ToString().Trim();
                    ssView2_Sheet1.Cells[0, 1].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssView2_Sheet1.Cells[8, 7].Text = dt.Rows[0]["JOBNAME"].ToString().Trim();

                    ssView2_Sheet1.Cells[11, 9].Text = dt.Rows[0]["SAMANGDTL"].ToString().Trim();

                    ssView2_Sheet1.Cells[12, 1].Value = dt.Rows[0]["JUSO1"].ToString().Trim() == "Y" ? true : false;
                    ssView2_Sheet1.Cells[12, 5].Value = dt.Rows[0]["JUSO2"].ToString().Trim() == "Y" ? true : false;
                    GstrAge = dt.Rows[0]["AGE"].ToString().Trim();

                    //제1급
                    if (dt.Rows[0]["INFECT1"].ToString().Trim() != "")
                    {
                        switch (clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "INFECT_법정전염병", dt.Rows[0]["INFECT1"].ToString().Trim()))
                        {
                            //case "콜레라": intRow = 14; intCol = 2; break;
                            //case "장티푸스": intRow = 14; intCol = 5; break;
                            //case "파라티푸스": intRow = 14; intCol = 8; break;
                            //case "세균성이질": intRow = 15; intCol = 2; break;
                            //case "장출혈성대장균감염증": intRow = 15; intCol = 5; break;
                            //case "A형간염": intRow = 15; intCol = 8; break;

                            case "에볼라바이러스병":
                                intRow = 15;
                                intCol = 3;
                                break;
                            case "마버그열":
                                intRow = 15;
                                intCol = 6;
                                break;
                            case "라싸열":
                                intRow = 15;
                                intCol = 9;
                                break;

                            case "크리미안콩고출혈열":
                                intRow = 16;
                                intCol = 3;
                                break;
                            case "남아메리카출혈열":
                                intRow = 16;
                                intCol = 6;
                                break;
                            case "리프트밸리열":
                                intRow = 16;
                                intCol = 9;
                                break;

                            case "두창":
                                intRow = 17;
                                intCol = 3;
                                break;
                            case "페스트":
                                intRow = 17;
                                intCol = 6;
                                break;
                            case "탄저":
                                intRow = 17;
                                intCol = 9;
                                break;

                            case "보툴리눔독소증":
                                intRow = 18;
                                intCol = 3;
                                break;
                            case "야토병":
                                intRow = 18;
                                intCol = 6;
                                break;
                            case "디프테리아":
                                intRow = 18;
                                intCol = 9;
                                break;

                            case "중증급성호흡기증후군(SARS)":
                                intRow = 19;
                                intCol = 3;
                                break;
                            case "중동호흡기증후군(MERS)":
                                intRow = 19;
                                intCol = 6;
                                break;
                            case "동물인플루엔자 인체감염증":
                                intRow = 19;
                                intCol = 9;
                                break;

                            case "신종인플루엔자":
                                intRow = 20;
                                intCol = 3;
                                break;
                            case "신종감염병증후군":
                                intRow = 20;
                                intCol = 6;
                                break;


                        }

                        ssView2_Sheet1.Cells[intRow - 1, intCol - 1].Value = true;
                    }

                    if (dt.Rows[0]["INFECT1ETC"].ToString().Trim() != "")
                    {
                        ssView2_Sheet1.Cells[19, 9].Value = dt.Rows[0]["INFECT1ETC"].ToString().Trim();
                    }

                    //제2급
                    if (dt.Rows[0]["INFECT2"].ToString().Trim() != "")
                    {
                        switch (clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "INFECT_법정전염병", dt.Rows[0]["INFECT2"].ToString().Trim()))
                        {
                            //case "디프테리아": intRow = 16; intCol = 2; break;
                            //case "백일해": intRow = 16; intCol = 5; break;
                            //case "파상풍": intRow = 16; intCol = 8; break;
                            //case "홍역": intRow = 17; intCol = 2; break;
                            //case "유행성이하선염": intRow = 17; intCol = 5; break;
                            //case "풍진": intRow = 17; intCol = 8; break;
                            //case "폴리오": intRow = 18; intCol = 2; break;
                            //case "일본뇌염": intRow = 18; intCol = 5; break;
                            //case "수두": intRow = 18; intCol = 8; break;
                            //case "B형간염(급성)": intRow = 19; intCol = 2; break;
                            //case "b형헤모필루스인플루엔자": intRow = 19; intCol = 5; break;
                            //case "폐렴구균": intRow = 19; intCol = 8; break;

                            case "수두":
                                intRow = 21;
                                intCol = 3;
                                break;
                            case "홍역":
                                intRow = 21;
                                intCol = 6;
                                break;
                            case "콜레라":
                                intRow = 21;
                                intCol = 9;
                                break;

                            case "장티푸스":
                                intRow = 22;
                                intCol = 3;
                                break;
                            case "파라티푸스":
                                intRow = 22;
                                intCol = 6;
                                break;
                            case "세균성이질":
                                intRow = 22;
                                intCol = 9;
                                break;

                            case "장출혈성대장균감염증":
                                intRow = 23;
                                intCol = 3;
                                break;
                            case "A형간염":
                                intRow = 23;
                                intCol = 6;
                                break;
                            case "백일해":
                                intRow = 23;
                                intCol = 9;
                                break;

                            case "선천성 풍진":
                                intRow = 24;
                                intCol = 6;
                                break;
                            case "후천성 풍진":
                                intRow = 24;
                                intCol = 9;
                                break;

                            case "폴리오":
                                intRow = 25;
                                intCol = 3;
                                break;
                            case "수막구균 감염증":
                                intRow = 25;
                                intCol = 6;
                                break;
                            case "유행성이하선염":
                                intRow = 25;
                                intCol = 9;
                                break;

                            case "b형헤모필루스인플루엔자":
                                intRow = 26;
                                intCol = 3;
                                break;
                            case "폐렴구균 감염증":
                                intRow = 26;
                                intCol = 6;
                                break;
                            case "한센병":
                                intRow = 26;
                                intCol = 9;
                                break;

                            case "성홍열":
                                intRow = 27;
                                intCol = 3;
                                break;
                            case "반코마이신내성황색포도얄균(VRSA) 감염증":
                                intRow = 27;
                                intCol = 6;
                                break;

                            case "카바페넴내성장내세균속균종(CRE) 감염증":
                                intRow = 28;
                                intCol = 3;
                                break;

                            case "E형간염":
                                intRow = 28;
                                intCol = 9;
                                break;
                        }

                        ssView2_Sheet1.Cells[intRow - 1, intCol - 1].Value = true;
                    }

                    //제3급
                    if (dt.Rows[0]["INFECT3"].ToString().Trim() != "")
                    {
                        switch (clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "INFECT_법정전염병", dt.Rows[0]["INFECT3"].ToString().Trim()))
                        {
                            //case "말라리아": intRow = 20; intCol = 2; break;
                            //case "한센병": intRow = 20; intCol = 5; break;
                            //case "성홍열": intRow = 20; intCol = 8; break;
                            //case "수막구균성수막염": intRow = 21; intCol = 2; break;
                            //case "레지오넬라증": intRow = 21; intCol = 5; break;
                            //case "비브리오패혈증": intRow = 21; intCol = 8; break;
                            //case "발진티푸스": intRow = 22; intCol = 2; break;
                            //case "발진열": intRow = 22; intCol = 5; break;
                            //case "쯔쯔가무시증": intRow = 22; intCol = 8; break;
                            //case "렙토스피라증": intRow = 23; intCol = 2; break;
                            //case "브루셀라증": intRow = 23; intCol = 5; break;
                            //case "탄저": intRow = 23; intCol = 8; break;
                            //case "공수병": intRow = 24; intCol = 2; break;
                            //case "신증후군출혈열": intRow = 24; intCol = 5; break;
                            //case "매독(1기)": intRow = 25; intCol = 2; break;
                            //case "매독(2기)": intRow = 25; intCol = 5; break;
                            //case "매독(선천성)": intRow = 25; intCol = 8; break;
                            //case "크로이츠벨트-야콥병 및 변종 크로이츠펠트-야콥병": intRow = 26; intCol = 2; break;
                            //case "C형간염": intRow = 27; intCol = 2; break;
                            //case "반코마이신내성황색포도얄균(VRSA) 감염증": intRow = 27; intCol = 5; break;
                            //case "카바페넴내성장내세균속균증(CRE) 감염증": intRow = 28; intCol = 2; break;

                            case "파상풍":
                                intRow = 29;
                                intCol = 3;
                                break;
                            case "B형간염":
                                intRow = 29;
                                intCol = 6;
                                break;
                            case "일본뇌염":
                                intRow = 29;
                                intCol = 9;
                                break;

                            case "C형간염":
                                intRow = 30;
                                intCol = 3;
                                break;
                            case "말라리아":
                                intRow = 30;
                                intCol = 6;
                                break;
                            case "레지오넬라증":
                                intRow = 30;
                                intCol = 9;
                                break;

                            case "비브리오패혈증":
                                intRow = 31;
                                intCol = 3;
                                break;
                            case "발진티푸스":
                                intRow = 31;
                                intCol = 6;
                                break;
                            case "발진열":
                                intRow = 31;
                                intCol = 9;
                                break;

                            case "쯔쯔가무시증":
                                intRow = 32;
                                intCol = 3;
                                break;
                            case "렙토스피라증":
                                intRow = 32;
                                intCol = 6;
                                break;
                            case "브루셀라증":
                                intRow = 32;
                                intCol = 9;
                                break;

                            case "공수병":
                                intRow = 33;
                                intCol = 3;
                                break;
                            case "신증후군출혈열":
                                intRow = 33;
                                intCol = 6;
                                break;
                            case "황열":
                                intRow = 33;
                                intCol = 3;
                                break;

                            case "뎅기열":
                                intRow = 34;
                                intCol = 6;
                                break;
                            case "큐열":
                                intRow = 34;
                                intCol = 9;
                                break;
                            case "웨스트나일열":
                                intRow = 34;
                                intCol = 3;
                                break;

                            case "크로이츠펠트-야콥병(CJD) 및 변종크로이츠펠트-야콥병(vCJD)":
                                intRow = 35;
                                intCol = 3;
                                break;

                            case "라임병":
                                intRow = 36;
                                intCol = 3;
                                break;
                            case "진드기매개뇌염":
                                intRow = 36;
                                intCol = 6;
                                break;
                            case "유비저":
                                intRow = 36;
                                intCol = 3;
                                break;

                            case "치쿤구니아열":
                                intRow = 37;
                                intCol = 3;
                                break;
                            case "중증열성혈소판감소증후군(SFTS)":
                                intRow = 37;
                                intCol = 6;
                                break;
                            case "지카바이러스 감염증":
                                intRow = 37;
                                intCol = 3;
                                break;
                        }

                        ssView2_Sheet1.Cells[intRow - 1, intCol - 1].Value = true;
                    }

                    //발병일
                    ssView2_Sheet1.Cells[37, 1].Text = dt.Rows[0]["BDATE"].ToString().Trim();
                    strBDATE = dt.Rows[0]["BDATE"].ToString().Trim();

                    //진단일
                    ssView2_Sheet1.Cells[37, 5].Text = dt.Rows[0]["JDATE"].ToString().Trim();
                    strJDate = dt.Rows[0]["JDATE"].ToString().Trim();

                    //신고일
                    ssView2_Sheet1.Cells[37, 9].Text = dt.Rows[0]["SDATE"].ToString().Trim();
                    strSDate = dt.Rows[0]["SDATE"].ToString().Trim();

                    //확진검사결과
                    if (dt.Rows[0]["EXRESULT"].ToString().Trim() != "")
                    {
                        intRow = 39;

                        switch (dt.Rows[0]["EXRESULT"].ToString().Trim())
                        {
                            case "0": intCol = 2; break;
                            case "1": intCol = 4; break;
                            case "2": intCol = 6; break;
                            case "3": intCol = 8; break;
                        }

                        ssView2_Sheet1.Cells[intRow - 1, intCol - 1].Value = true;
                    }

                    //환자등 분류
                    if (dt.Rows[0]["PATBUN"].ToString().Trim() != "")
                    {
                        switch (dt.Rows[0]["PATBUN"].ToString().Trim())
                        {
                            case "0": intRow = 40; intCol = 2; break;
                            case "1": intRow = 40; intCol = 3; break;
                            case "2": intRow = 41; intCol = 2; break;
                            case "3": intRow = 41; intCol = 4; break;
                            case "4": intRow = 40; intCol = 4; break;
                        }

                        ssView2_Sheet1.Cells[intRow - 1, intCol - 1].Value = true;
                    }

                    //입원여부
                    if (dt.Rows[0]["GbIO"].ToString().Trim() != "")
                    {
                        switch (dt.Rows[0]["GbIO"].ToString().Trim())
                        {
                            case "1": intRow = 40; intCol = 8; break;
                            case "2": intRow = 40; intCol = 9; break;
                            case "3": intRow = 41; intCol = 8; break;
                        }

                        ssView2_Sheet1.Cells[intRow - 1, intCol - 1].Value = true;
                    }

                    //추정감염경로
                    if (dt.Rows[0]["EXPATH"].ToString().Trim() != "")
                    {
                        switch (dt.Rows[0]["EXPATH"].ToString().Trim())
                        {
                            case "0": intRow = 42; intCol = 2; break;
                            case "1": intRow = 43; intCol = 2; break;
                            case "2": intRow = 44; intCol = 2; break;
                            case "3": intRow = 44; intCol = 4; break;
                        }

                        ssView2_Sheet1.Cells[intRow - 1, intCol - 1].Value = true;
                    }

                    //추정감염지역
                    if (dt.Rows[0]["EXREGION"].ToString().Trim() != "")
                    {
                        switch (dt.Rows[0]["EXREGION"].ToString().Trim())
                        {
                            case "0": intRow = 42; intCol = 7; break;
                            case "1": intRow = 43; intCol = 7; break;
                        }

                        ssView2_Sheet1.Cells[intRow - 1, intCol - 1].Value = true;
                    }

                    //국명
                    ssView2_Sheet1.Cells[42, 8].Text = dt.Rows[0]["EXREGIONETC"].ToString().Trim();

                    //체류기간
                    ssView2_Sheet1.Cells[43, 7].Text = dt.Rows[0]["EXREGIONDAY_FR"].ToString().Trim();
                    ssView2_Sheet1.Cells[43, 9].Text = dt.Rows[0]["EXREGIONDAY_TO"].ToString().Trim();
                    ssView2_Sheet1.Cells[43, 10].Text = "(" + dt.Rows[0]["EXREGIONDAY"].ToString().Trim() + "일)";

                    //사망여부
                    if (dt.Rows[0]["SAMANG"].ToString().Trim() != "")
                    {
                        intRow = 46;

                        switch (dt.Rows[0]["SAMANG"].ToString().Trim())
                        {
                            case "0": intCol = 2; break;
                            case "1": intCol = 3; break;
                        }

                        ssView2_Sheet1.Cells[intRow - 1, intCol - 1].Value = true;
                    }

                    ////기타(환자아님)
                    //ssView2_Sheet1.Cells[44, 6].Value = dt.Rows[0]["EXNOTPAT"].ToString().Trim() == "1" ? true : false;

                    //비고특이사항
                    ssView2_Sheet1.Cells[45, 6].Text = dt.Rows[0]["REMARK"].ToString().Trim();

                    ssView2_Sheet1.Cells[48, 3].Text = clsVbfunc.GetOCSDrNameSabun(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());
                    ssView2_Sheet1.Cells[48, 8].Text = clsVbfunc.GetOCSDoctorDRBUNHO(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());

                    cboDept.Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + "." + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[0]["DEPTCODE"].ToString().Trim());
                    cboDr.Text = clsVbfunc.GetOCSDoctorCode(clsDB.DbCon, ssView2_Sheet1.Cells[48, 3].Text) + "." + ssView2_Sheet1.Cells[48, 3].Text;

                    txtWard.Text = dt.Rows[0]["WARD"].ToString().Trim();
                    txtRoom.Text = dt.Rows[0]["ROOM"].ToString().Trim();

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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            panHelp.Visible = false;
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            ssLow_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssLow_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssLow_Sheet1.PrintInfo.Margin.Top = 20;
            ssLow_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssLow_Sheet1.PrintInfo.Margin.Header = 10;
            ssLow_Sheet1.PrintInfo.ShowColor = false;
            ssLow_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssLow_Sheet1.PrintInfo.ShowBorder = false;
            ssLow_Sheet1.PrintInfo.ShowGrid = false;
            ssLow_Sheet1.PrintInfo.ShowShadows = false;
            ssLow_Sheet1.PrintInfo.UseMax = true;
            ssLow_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssLow_Sheet1.PrintInfo.UseSmartPrint = false;
            ssLow_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssLow_Sheet1.PrintInfo.Preview = false;
            ssLow.PrintSheet(0);
        }

        private void btnLawClose_Click(object sender, EventArgs e)
        {

        }

        private void panWeb_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnWebClose_Click(object sender, EventArgs e)
        {
            panWeb.Visible = false;
        }
    }
}
