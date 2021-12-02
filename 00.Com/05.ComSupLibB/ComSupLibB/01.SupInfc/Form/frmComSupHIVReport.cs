using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComSupLibB
{
    /// <summary>
    /// Class Name      : ComSupLibB
    /// File Name       : frmComSupHIVReport.cs
    /// Description     : HIV 신고서
    /// Author          : 이정현
    /// Create Date     : 2018-05-15
    /// <history> 
    /// HIV 신고서
    /// </history>
    /// <seealso>
    /// PSMH\exam\exinfect\Exinfect30.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\exam\exinfect\exinfect.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupHIVReport : Form
    {
        private string GstrPANO = "";
        private double GdblIPDNO = 0;
        private string GstrROWID = "";
        private string GstrDept = "";
        private string GstrAge = "";
        private string GstrWard = "";
        private string GstrRoom = "";
        private string GstrDrSabun = "";

        public frmComSupHIVReport()
        {
            InitializeComponent();
        }

        public frmComSupHIVReport(string strPANO, double dblIPDNO, string strDeptCode)
        {
            InitializeComponent();

            GstrPANO = strPANO;
            GdblIPDNO = dblIPDNO;
            GstrDept = strDeptCode;
        }

        private void frmComSupHIVReport_Load(object sender, EventArgs e)
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

            if (txtPano.Text.Trim() != "")
            {
                lblSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPano.Text.Trim());
            }

            panPat.Visible = false;

            if (clsType.User.Sabun == "12306"
                || clsType.User.Sabun == "39005"
                || clsType.User.Sabun == "44794")
            {
                panPat.Visible = true;
            }

            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept);

            SCREEN_CLEAR();

            Screen_display();

            GetData();
        }

        private void SCREEN_CLEAR()
        {
            //신고구분
            ssView_Sheet1.Cells[7, 6].Value = false;
            ssView_Sheet1.Cells[7, 22].Value = false;

            //성별
            ssView_Sheet1.Cells[8, 6].Value = false;
            ssView_Sheet1.Cells[8, 10].Value = false;

            //생년월일
            ssView_Sheet1.Cells[8, 21].Text = "";

            //가검물번호
            ssView_Sheet1.Cells[8, 34].Text = "";

            //최초진단일
            ssView_Sheet1.Cells[9, 6].Text = "";

            //확인검사기관
            ssView_Sheet1.Cells[9, 22].Value = false;
            ssView_Sheet1.Cells[9, 24].Text = "";

            ssView_Sheet1.Cells[10, 22].Value = false;

            //확인 진단일
            ssView_Sheet1.Cells[11, 6].Text = "";

            //확인검사기관
            ssView_Sheet1.Cells[11, 22].Value = false;
            ssView_Sheet1.Cells[11, 24].Text = "";

            ssView_Sheet1.Cells[12, 22].Value = false;

            //검사소견 - 면역기능
            ssView_Sheet1.Cells[14, 6].Value = false;
            ssView_Sheet1.Cells[14, 14].Text = "";

            //검사소견 - 바이러스양
            ssView_Sheet1.Cells[15, 6].Value = false;
            ssView_Sheet1.Cells[15, 14].Text = "";

            //검사소견 - 검사안함
            ssView_Sheet1.Cells[16, 6].Value = false;

            //추정감염경로
            ssView_Sheet1.Cells[13, 28].Value = false;          //이성과의 성접촉
            ssView_Sheet1.Cells[14, 28].Value = false;          //동성과의 성접촉
            ssView_Sheet1.Cells[15, 28].Value = false;          //마약주사기 공동사용
            ssView_Sheet1.Cells[16, 28].Value = false;          //수혈
            ssView_Sheet1.Cells[16, 34].Value = false;          //수직감염

            ssView_Sheet1.Cells[17, 28].Value = false;          //모름
            ssView_Sheet1.Cells[18, 28].Value = false;          //기타
            ssView_Sheet1.Cells[18, 31].Text = "";              //기타 상세내역

            //사망여부
            ssView_Sheet1.Cells[19, 6].Value = false;           //사망
            ssView_Sheet1.Cells[19, 10].Value = false;          //생존

            //주요사망원인(진단명)
            ssView_Sheet1.Cells[20, 6].Text = "";

            //사망일
            ssView_Sheet1.Cells[22, 6].Text = "";

            //사망과 후천성면역결핍증과의 관련성
            ssView_Sheet1.Cells[22, 32].Value = false;          //유
            ssView_Sheet1.Cells[22, 36].Value = false;          //무

            //후천성면역결핍증환자관련 임상증상(사망전 주요증상)
            ssView_Sheet1.Cells[23, 6].Value = false;           //기관지, 기도, 또는 칸디다증
            ssView_Sheet1.Cells[23, 22].Value = false;          //카포지 육종

            ssView_Sheet1.Cells[24, 6].Value = false;           //기관지, 기도, 또는 칸디다증
            ssView_Sheet1.Cells[24, 22].Value = false;          //버키트 림프종

            ssView_Sheet1.Cells[25, 6].Value = false;           //침습성 자궁경부암 파종성 또는 폐외 콕시디오데스진균증
            ssView_Sheet1.Cells[25, 22].Value = false;          //원발성 뇌 림프종

            ssView_Sheet1.Cells[26, 22].Value = false;          //파종성 또는 폐외 결핵

            ssView_Sheet1.Cells[27, 6].Value = false;           //폐외 크립코쿠스증
            ssView_Sheet1.Cells[27, 22].Value = false;          //Mycobacterium avium complex, m..

            ssView_Sheet1.Cells[28, 6].Value = false;           //만성(1개월 이상) 장 크립토스포로디움

            ssView_Sheet1.Cells[29, 6].Value = false;           //간, 비장, 림프절 이뢰의 거대세포 바이러스 감염증
            ssView_Sheet1.Cells[29, 22].Value = false;          //그 밖에 균종의 Mycobacterium에 의한 폐외감염증

            ssView_Sheet1.Cells[30, 22].Value = false;          //주폐포자중 폐렴

            ssView_Sheet1.Cells[31, 6].Value = false;           //거대세포 바이러스 망막염
            ssView_Sheet1.Cells[31, 22].Value = false;          //반복되는 폐렴

            ssView_Sheet1.Cells[32, 6].Value = false;           //hiv 관련 뇌증
            ssView_Sheet1.Cells[32, 22].Value = false;          //진행성 다발성 백질뇌증

            ssView_Sheet1.Cells[33, 6].Value = false;           //단손포진 바이러스 감염에 의한 만성 궤양(1개월이상), 기관지염, 폐렴, 또는 ..
            ssView_Sheet1.Cells[33, 22].Value = false;          //반복성 살모넬라 폐혈증

            ssView_Sheet1.Cells[34, 22].Value = false;          //톡소플라즈마증

            ssView_Sheet1.Cells[35, 6].Value = false;           //파종성 또는 폐외 히스토플리스마증
            ssView_Sheet1.Cells[35, 22].Value = false;          //hiv에 의한 소모증후군

            ssView_Sheet1.Cells[36, 6].Value = false;           //만성(1개월 이상) 장 이소스포라증
            ssView_Sheet1.Cells[36, 22].Value = false;          //기타
            ssView_Sheet1.Cells[36, 25].Text = "";              //기타 상세내역

            ssView_Sheet1.Cells[38, 7].Text = "";               //진단한의사
            ssView_Sheet1.Cells[38, 20].Text = "";              //면허번호
        }

        private void Screen_display()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //환자 정보 READ
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     AGE, DEPTCODE, DRCODE, WARDCODE, ROOMCODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE IPDNO = '" + GdblIPDNO + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    txtWard.Text = "";
                    txtRoom.Text = "";

                    cboDept.Text = GstrDept + "." + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, GstrDept);

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

                        ssView_Sheet1.Cells[38, 7].Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[38, 18].Text = dt.Rows[0]["DRBUNHO"].ToString().Trim();
                    }
                }
                else
                {
                    GstrAge = dt.Rows[0]["AGE"].ToString().Trim();
                    GstrDept = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    GstrWard = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    GstrRoom = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    GstrDrSabun = clsType.User.Sabun;
                    cboDept.Text = GstrDept + "." + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, GstrDept);
                    cboDr.Text = dt.Rows[0]["DRCODE"].ToString().Trim() + "." + clsVbfunc.GetOCSDrCodeDrName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                    txtWard.Text = GstrWard;
                    txtRoom.Text = GstrRoom;

                    ssView_Sheet1.Cells[38, 7].Text = clsVbfunc.GetOCSDrCodeDrName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                    ssView_Sheet1.Cells[38, 18].Text = clsVbfunc.GetOCSDoctorDRBUNHO(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                //환자 인적사항
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SNAME, PNAME, JUMIN1, JUMIN2 , JIKUP, SEX, TEL, HPHONE, ";
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
                    ssView_Sheet1.Cells[8, 6].Value = dt.Rows[0]["SEX"].ToString().Trim() == "M" ? true : false;
                    ssView_Sheet1.Cells[8, 6].Value = dt.Rows[0]["SEX"].ToString().Trim() == "F" ? true : false;
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

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
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT8 A ";
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (SaveData() == true)
            {
                SCREEN_CLEAR();
                GetData();
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strPano = "";
            string strSname = "";
            string strGUBUN = "";
            string strSex = "";
            string strBDAY = "";
            string strGNO = "";
            string strJDATE1 = "";
            string strInfect1 = "";
            string strINFECT1_ETC = "";
            string strJDATE2 = "";
            string strinfect2 = "";
            string strINFECT2_ETC = "";
            string strinfect3 = "";
            string strINFECT3_ETC = "";
            string strinfect4 = "";
            string strINFECT4_ETC = "";
            string strinfect5 = "";
            string strINFECT6 = "";
            string strINFECT6_ETC = "";
            string strINFECT7 = "";
            string strINFECT8 = "";
            string strINFECT9 = "";
            string strINFECT10 = "";
            string strINFECT11 = "";
            string strINFECT11_ETC = "";
            string strDRSABUN = "";
            string strDRBUNHO = "";
            string strENTSABUN = "";
            string strEntDate = "";
            string strIPDNO = "";

            if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록환자의 등록번호를 조회 후 작업해주세요.");
                return rtnVal;
            }

            GstrDrSabun = clsVbfunc.GetOCSDrCodeSabun(clsDB.DbCon, VB.Left(cboDr.Text, 4));
            strDRBUNHO = clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Left(cboDr.Text, 4));

            GstrDept = VB.Left(cboDept.Text, 2);
            GstrWard = txtWard.Text;
            GstrRoom = txtRoom.Text;

            strPano = txtPano.Text;
            strSname = lblSName.Text;

            //신고구분
            strGUBUN = Convert.ToBoolean(ssView_Sheet1.Cells[7, 6].Value) == true ? "1" : "";
            strGUBUN = Convert.ToBoolean(ssView_Sheet1.Cells[7, 22].Value) == true ? "2" : "";

            //성별
            strSex = Convert.ToBoolean(ssView_Sheet1.Cells[8, 6].Value) == true ? "M" : "";
            strSex = Convert.ToBoolean(ssView_Sheet1.Cells[8, 10].Value) == true ? "F" : "";

            //생년월일
            strBDAY = ssView_Sheet1.Cells[8, 21].Text.Trim();

            //가검물번호
            strGNO = ssView_Sheet1.Cells[8, 34].Text.Trim();

            //최초진단일
            strJDATE1 = ssView_Sheet1.Cells[9, 6].Text.Trim();

            //확인검사기관
            strInfect1 = Convert.ToBoolean(ssView_Sheet1.Cells[9, 22].Value) == true ? "1" : "";
            strINFECT1_ETC = ssView_Sheet1.Cells[9, 24].Text.Trim();

            strInfect1 = Convert.ToBoolean(ssView_Sheet1.Cells[10, 22].Value) == true ? "2" : "";

            //확인 진단일
            strJDATE2 = ssView_Sheet1.Cells[11, 6].Text.Trim();

            //확인검사기관
            strinfect2 = Convert.ToBoolean(ssView_Sheet1.Cells[11, 22].Value) == true ? "1" : "";
            strINFECT2_ETC = ssView_Sheet1.Cells[11, 24].Text.Trim();

            strinfect2 = Convert.ToBoolean(ssView_Sheet1.Cells[12, 22].Value) == true ? "2" : "";

            //검사소견 - 면역기능
            strinfect3 = Convert.ToBoolean(ssView_Sheet1.Cells[14, 6].Value) == true ? "1" : "";
            strINFECT3_ETC = ssView_Sheet1.Cells[14, 14].Text.Trim();

            //검사소견 - 바이러스양
            strinfect4 = Convert.ToBoolean(ssView_Sheet1.Cells[15, 6].Value) == true ? "1" : "";
            strINFECT4_ETC = ssView_Sheet1.Cells[15, 14].Text.Trim();

            //검사소견 - 검사안함
            strinfect5 = Convert.ToBoolean(ssView_Sheet1.Cells[16, 6].Value) == true ? "1" : "";

            //추정감염경로
            strINFECT6 = Convert.ToBoolean(ssView_Sheet1.Cells[13, 28].Value) == true ? "1" : "";       //이성과의 성접촉
            strINFECT6 = Convert.ToBoolean(ssView_Sheet1.Cells[14, 28].Value) == true ? "2" : "";       //동성과의 성접촉
            strINFECT6 = Convert.ToBoolean(ssView_Sheet1.Cells[15, 28].Value) == true ? "3" : "";       //마약주사기 공동사용
            strINFECT6 = Convert.ToBoolean(ssView_Sheet1.Cells[16, 28].Value) == true ? "4" : "";       //수혈
            strINFECT6 = Convert.ToBoolean(ssView_Sheet1.Cells[16, 34].Value) == true ? "5" : "";       //수직감염

            strINFECT6 = Convert.ToBoolean(ssView_Sheet1.Cells[17, 28].Value) == true ? "6" : "";       //모름
            strINFECT6 = Convert.ToBoolean(ssView_Sheet1.Cells[18, 28].Value) == true ? "7" : "";       //기타
            strINFECT6_ETC = ssView_Sheet1.Cells[18, 31].Text.Trim();                                   //기타 상세내역

            //사망여부
            strINFECT7 = Convert.ToBoolean(ssView_Sheet1.Cells[19, 6].Value) == true ? "1" : "";        //사망
            strINFECT7 = Convert.ToBoolean(ssView_Sheet1.Cells[19, 10].Value) == true ? "2" : "";       //생존

            //주요사망원인(진단명)
            strINFECT8 = ssView_Sheet1.Cells[20, 6].Text.Trim();

            //사망일
            strINFECT9 = ssView_Sheet1.Cells[22, 6].Text.Trim();

            //사망과 후천성면역결핍증과의 관련성
            strINFECT10 = Convert.ToBoolean(ssView_Sheet1.Cells[22, 32].Value) == true ? "Y" : "";      //유
            strINFECT10 = Convert.ToBoolean(ssView_Sheet1.Cells[22, 36].Value) == true ? "N" : "";      //무

            //후천성면역결핍증환자관련 임상증상(사망전 주요증상)
            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[23, 6].Value) == true ? "1" : "";       //기관지, 기도, 또는 칸디다증
            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[23, 22].Value) == true ? "2" : "";      //카포지 육종

            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[24, 6].Value) == true ? "3" : "";       //기관지, 기도, 또는 칸디다증
            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[24, 22].Value) == true ? "4" : "";      //버키트 림프종

            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[25, 6].Value) == true ? "5" : "";       //침습성 자궁경부암 파종성 또는 폐외 콕시디오데스진균증
            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[25, 22].Value) == true ? "6" : "";      //원발성 뇌 림프종

            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[26, 22].Value) == true ? "7" : "";      //파종성 또는 폐외 결핵

            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[27, 6].Value) == true ? "8" : "";       //폐외 크립코쿠스증
            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[27, 22].Value) == true ? "9" : "";      //Mycobacterium avium complex, m..

            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[28, 6].Value) == true ? "10" : "";      //만성(1개월 이상) 장 크립토스포로디움

            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[29, 6].Value) == true ? "11" : "";      //간, 비장, 림프절 이뢰의 거대세포 바이러스 감염증
            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[29, 22].Value) == true ? "12" : "";     //그 밖에 균종의 Mycobacterium에 의한 폐외감염증

            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[30, 22].Value) == true ? "13" : "";     //주폐포자중 폐렴

            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[31, 6].Value) == true ? "14" : "";      //거대세포 바이러스 망막염
            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[31, 22].Value) == true ? "15" : "";     //반복되는 폐렴

            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[32, 6].Value) == true ? "16" : "";      //hiv 관련 뇌증
            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[32, 22].Value) == true ? "17" : "";     //진행성 다발성 백질뇌증

            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[33, 6].Value) == true ? "18" : "";      //단손포진 바이러스 감염에 의한 만성 궤양(1개월이상), 기관지염, 폐렴, 또는 ..
            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[33, 22].Value) == true ? "19" : "";     //반복성 살모넬라 폐혈증

            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[34, 22].Value) == true ? "20" : "";     //톡소플라즈마증

            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[35, 6].Value) == true ? "21" : "";      //파종성 또는 폐외 히스토플리스마증
            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[35, 22].Value) == true ? "22" : "";     //hiv에 의한 소모증후군

            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[36, 6].Value) == true ? "23" : "";      //만성(1개월 이상) 장 이소스포라증
            strINFECT11 = Convert.ToBoolean(ssView_Sheet1.Cells[36, 22].Value) == true ? "24" : "";     //기타
            strINFECT11_ETC = ssView_Sheet1.Cells[36, 25].Text.Trim();

            //필수 입력 내용 점검
            if (strGUBUN == "")
            {
                ComFunc.MsgBox("신고구분을 반드시 체크 요망합니다.");
                return rtnVal;
            }

            if (strJDATE1 == "")
            {
                ComFunc.MsgBox("최초진단일을 반드시 입력 해주십시오.");
                return rtnVal;
            }

            if (strJDATE2 == "")
            {
                ComFunc.MsgBox("확진일 반드시 입력 해주십시오.");
                return rtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (GstrROWID != "")
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_PMPA + "NUR_STD_INFECT8";
                    SQL = SQL + ComNum.VBLF + "     SET ";
                    SQL = SQL + ComNum.VBLF + "         GUBUN ='" + strGUBUN + "', ";
                    SQL = SQL + ComNum.VBLF + "         SEX = '" + strSex + "', ";
                    SQL = SQL + ComNum.VBLF + "         BDAY = TO_DATE('" + strBDAY + "','YYYY-MM-DD')  , ";
                    SQL = SQL + ComNum.VBLF + "         GNO = '" + strGNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         JDATE1 = TO_DATE( '" + strJDATE1 + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + "         INFECT1 = '" + strInfect1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT1_ETC = '" + strINFECT1_ETC + "', ";
                    SQL = SQL + ComNum.VBLF + "         JDATE2 = TO_DATE('" + strJDATE2 + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + "         INFECT2 = '" + strinfect2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT2_ETC = '" + strINFECT2_ETC + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT3 = '" + strinfect3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT3_ETC = '" + strINFECT3_ETC + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT4 = '" + strinfect4 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT4_ETC ='" + strINFECT4_ETC + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT5 = '" + strinfect5 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT6 = '" + strINFECT6 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT6_ETC = '" + strINFECT6_ETC + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT7 = '" + strINFECT7 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT8 = '" + strINFECT8 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT9 = TO_DATE( '" + strINFECT9 + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + "         INFECT10 = '" + strINFECT10 + "',  ";
                    SQL = SQL + ComNum.VBLF + "         INFECT11 = '" + strINFECT11 + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFECT11_ETC = '" + strINFECT11_ETC + "', ";
                    SQL = SQL + ComNum.VBLF + "         DRSABUN = '" + GstrDrSabun + "', ";
                    SQL = SQL + ComNum.VBLF + "         DRBUNHO = '" + strDRBUNHO + "', ";
                    SQL = SQL + ComNum.VBLF + "         ENTSABUN = '" + clsType.User.Sabun + "', ";
                    SQL = SQL + ComNum.VBLF + "         ENTDATE = SYSDATE , ";
                    SQL = SQL + ComNum.VBLF + "         IPDNO = '" + GdblIPDNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         WARD = '" + GstrWard + "', ";
                    SQL = SQL + ComNum.VBLF + "         ROOM = '" + GstrRoom + "'   ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + GstrROWID + "' ";
                }
                else
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "NUR_STD_INFECT8";
                    SQL = SQL + ComNum.VBLF + "     (ACTDATE, PANO, SNAME,  GUBUN, SEX, BDAY, GNO, JDATE1, INFECT1, INFECT1_ETC,  JDATE2, INFECT2, INFECT2_ETC, ";
                    SQL = SQL + ComNum.VBLF + "     INFECT3, INFECT3_ETC, INFECT4, INFECT4_ETC, INFECT5, INFECT6, INFECT6_ETC, INFECT7, INFECT8, INFECT9, INFECT10, INFECT11, INFECT11_ETC, DRSABUN, ";
                    SQL = SQL + ComNum.VBLF + "     DRBUNHO, ENTSABUN, ENTDATE, IPDNO, WARD, ROOM)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         TRUNC(SYSDATE), '" + txtPano.Text + "',  '" + strSname + "',  ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGUBUN + "'  , '" + strSex + "', TO_DATE('" + strBDAY + "','YYYY-MM-DD')  , '" + strGNO + "', ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE( '" + strJDATE1 + "' ,'YYYY-MM-DD') , '" + strInfect1 + "', '" + strINFECT1_ETC + "', ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strJDATE2 + "','YYYY-MM-DD') , '" + strinfect2 + "' ,";
                    SQL = SQL + ComNum.VBLF + "         '" + strINFECT2_ETC + "', '" + strinfect3 + "' ,";
                    SQL = SQL + ComNum.VBLF + "         '" + strINFECT3_ETC + "', '" + strinfect4 + "', '" + strINFECT4_ETC + "' , '" + strinfect5 + "' , ";
                    SQL = SQL + ComNum.VBLF + "         '" + strINFECT6 + "', '" + strINFECT6_ETC + "', '" + strINFECT7 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strINFECT8 + "', TO_DATE( '" + strINFECT9 + "','YYYY-MM-DD') , '" + strINFECT10 + "',  ";
                    SQL = SQL + ComNum.VBLF + "         '" + strINFECT11 + "', '" + strINFECT11_ETC + "', '" + GstrDrSabun + "', '" + strDRBUNHO + "',";
                    SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "', SYSDATE , '" + GdblIPDNO + "', '" + GstrWard + "', '" + GstrRoom + "' ";
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

                clsOrderEtc.INFECT_MSGSEND_HIV(clsDB.DbCon, txtPano.Text.Trim(), GstrWard, GstrROWID, strSname, (int)VB.Val(clsType.User.Sabun));

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
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Margin.Top = 20;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.Margin.Header = 10;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView_Sheet1.PrintInfo.Preview = false;
            ssView.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPano.Text.Trim() == "") { return; }

                txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");
                lblSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPano.Text.Trim());

                if (lblSName.Text.Trim() != "") { Screen_display(); }
            }
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsVbfunc.SetDrCodeCombo(clsDB.DbCon, cboDr, VB.Left(cboDept.Text, 2), "0", 1, "");
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true || e.RowHeader == true) { return; }

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRow = 0;

            Cursor.Current = Cursors.WaitCursor;
            
            GstrROWID = ssList_Sheet1.Cells[e.Row, 3].Text.Trim();

            SCREEN_CLEAR();

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ACTDATE, PANO, SNAME, GUBUN, SEX, BDAY, GNO, TO_CHAR(JDATE1, 'YYYY-MM-DD') JDATE1 , INFECT1, INFECT1_ETC,  JDATE2, INFECT2, INFECT2_ETC, ";
                SQL = SQL + ComNum.VBLF + "     INFECT3, INFECT3_ETC, INFECT4, INFECT4_ETC, INFECT5, INFECT6, INFECT6_ETC, INFECT7, INFECT8, INFECT9, INFECT10, INFECT11, INFECT11_ETC, DRSABUN, ";
                SQL = SQL + ComNum.VBLF + "     DRBUNHO, ENTSABUN, ENTDATE, IPDNO, WARD, ROOM ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT8 ";
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
                    ComFunc.MsgBox("데이터오류 전산정보팀으로 연락요망");
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                //신고구분
                switch (dt.Rows[0]["GUBUN"].ToString().Trim())
                {
                    case "1": ssView_Sheet1.Cells[7, 6].Value = true; break;
                    case "2": ssView_Sheet1.Cells[7, 22].Value = true; break;
                }

                //성별
                switch (dt.Rows[0]["SEX"].ToString().Trim())
                {
                    case "M": ssView_Sheet1.Cells[8, 6].Value = true; break;
                    case "F": ssView_Sheet1.Cells[8, 10].Value = true; break;
                }

                //생년월일
                ssView_Sheet1.Cells[8, 21].Text = dt.Rows[0]["BDAY"].ToString().Trim();

                //가검물번호
                ssView_Sheet1.Cells[8, 34].Text = dt.Rows[0]["GNO"].ToString().Trim();

                //최초진단일
                ssView_Sheet1.Cells[9, 6].Text = dt.Rows[0]["JDATE1"].ToString().Trim();

                //확인검사기관
                switch (dt.Rows[0]["INFECT1"].ToString().Trim())
                {
                    case "1": ssView_Sheet1.Cells[9, 22].Value = true; break;
                    case "2": ssView_Sheet1.Cells[10, 22].Value = true; break;
                }

                ssView_Sheet1.Cells[9, 24].Text = dt.Rows[0]["INFECT1_ETC"].ToString().Trim();

                //확인 진단일
                ssView_Sheet1.Cells[11, 6].Text = dt.Rows[0]["JDATE2"].ToString().Trim();

                //확인검사기관
                switch (dt.Rows[0]["INFECT2"].ToString().Trim())
                {
                    case "1": ssView_Sheet1.Cells[11, 22].Value = true; break;
                    case "2": ssView_Sheet1.Cells[12, 22].Value = true; break;
                }

                ssView_Sheet1.Cells[11, 24].Text = dt.Rows[0]["INFECT2_ETC"].ToString().Trim();

                //검사소견 - 면역기능
                ssView_Sheet1.Cells[14, 6].Value = dt.Rows[0]["INFECT3"].ToString().Trim() == "1" ? true : false;
                ssView_Sheet1.Cells[14, 14].Text = dt.Rows[0]["INFECT3_ETC"].ToString().Trim();

                //검사소견 - 바이러스양
                ssView_Sheet1.Cells[15, 6].Value = dt.Rows[0]["INFECT4"].ToString().Trim() == "1" ? true : false;
                ssView_Sheet1.Cells[15, 14].Text = dt.Rows[0]["INFECT4_ETC"].ToString().Trim();

                //검사소견 - 검사안함
                ssView_Sheet1.Cells[16, 6].Value = dt.Rows[0]["INFECT5"].ToString().Trim() == "1" ? true : false;

                //추정감염경로
                switch (dt.Rows[0]["INFECT6"].ToString().Trim())
                {
                    case "1": ssView_Sheet1.Cells[13, 28].Value = true; break;          //이성과의 성접촉
                    case "2": ssView_Sheet1.Cells[14, 28].Value = true; break;          //동성과의 성접촉
                    case "3": ssView_Sheet1.Cells[15, 28].Value = true; break;          //마약주사기 공동사용
                    case "4": ssView_Sheet1.Cells[16, 28].Value = true; break;          //수혈
                    case "5": ssView_Sheet1.Cells[16, 34].Value = true; break;          //수직감염

                    case "6": ssView_Sheet1.Cells[17, 28].Value = true; break;          //모름
                    case "7": ssView_Sheet1.Cells[18, 28].Value = true; break;          //기타
                }

                if (dt.Rows[0]["INFECT6_ETC"].ToString().Trim() != "")
                {
                    ssView_Sheet1.Cells[18, 28].Value = true;
                    ssView_Sheet1.Cells[18, 31].Text = dt.Rows[0]["INFECT6_ETC"].ToString().Trim();     //기타 상세내역
                }

                //사망여부
                switch (dt.Rows[0]["INFECT7"].ToString().Trim())
                {
                    case "1": ssView_Sheet1.Cells[19, 6].Value = true; break;           //사망
                    case "2": ssView_Sheet1.Cells[19, 10].Value = true; break;          //생존
                }

                //주요사망원인(진단명)
                ssView_Sheet1.Cells[20, 6].Text = dt.Rows[0]["INFECT8"].ToString().Trim();

                //사망일
                ssView_Sheet1.Cells[22, 6].Text = dt.Rows[0]["INFECT9"].ToString().Trim();

                //사망과 후천성면역결핍증과의 관련성
                if (dt.Rows[0]["INFECT10"].ToString().Trim() == "Y")
                {
                    ssView_Sheet1.Cells[22, 32].Value = true;           //유
                }
                else
                {
                    ssView_Sheet1.Cells[22, 36].Value = true;           //무
                }

                //후천성면역결핍증환자관련 임상증상(사망전 주요증상)
                switch (dt.Rows[0]["INFECT"].ToString().Trim())
                {
                    case "2": intRow = 23; ssView_Sheet1.Cells[23, 22].Value = true; break;      //카포지 육종
                    case "1": intRow = 23; ssView_Sheet1.Cells[23, 6].Value = true; break;       //기관지, 기도, 또는 칸디다증
                    case "3": intRow = 24; ssView_Sheet1.Cells[24, 6].Value = true; break;       //기관지, 기도, 또는 칸디다증
                    case "4": intRow = 24; ssView_Sheet1.Cells[24, 22].Value = true; break;      //버키트 림프종
                    case "5": intRow = 25; ssView_Sheet1.Cells[25, 6].Value = true; break;       //침습성 자궁경부암 파종성 또는 폐외 콕시디오데스진균증
                    case "6": intRow = 25; ssView_Sheet1.Cells[25, 22].Value = true; break;      //원발성 뇌 림프종
                    case "7": intRow = 26; ssView_Sheet1.Cells[26, 22].Value = true; break;      //파종성 또는 폐외 결핵
                    case "8": intRow = 27; ssView_Sheet1.Cells[27, 6].Value = true; break;       //폐외 크립코쿠스증
                    case "9": intRow = 27; ssView_Sheet1.Cells[27, 22].Value = true; break;      //Mycobacterium avium complex, m..
                    case "10": intRow = 28; ssView_Sheet1.Cells[28, 6].Value = true; break;      //만성(1개월 이상) 장 크립토스포로디움
                    case "11": intRow = 29; ssView_Sheet1.Cells[29, 6].Value = true; break;      //간, 비장, 림프절 이뢰의 거대세포 바이러스 감염증
                    case "12": intRow = 29; ssView_Sheet1.Cells[29, 22].Value = true; break;     //그 밖에 균종의 Mycobacterium에 의한 폐외감염증
                    case "13": intRow = 30; ssView_Sheet1.Cells[30, 22].Value = true; break;     //주폐포자중 폐렴
                    case "14": intRow = 31; ssView_Sheet1.Cells[31, 6].Value = true; break;      //거대세포 바이러스 망막염
                    case "15": intRow = 31; ssView_Sheet1.Cells[31, 22].Value = true; break;     //반복되는 폐렴
                    case "16": intRow = 32; ssView_Sheet1.Cells[32, 6].Value = true; break;      //hiv 관련 뇌증
                    case "17": intRow = 32; ssView_Sheet1.Cells[32, 22].Value = true; break;     //진행성 다발성 백질뇌증
                    case "18": intRow = 33; ssView_Sheet1.Cells[33, 6].Value = true; break;      //단손포진 바이러스 감염에 의한 만성 궤양(1개월이상), 기관지염, 폐렴, 또는 ..
                    case "19": intRow = 33; ssView_Sheet1.Cells[33, 22].Value = true; break;     //반복성 살모넬라 폐혈증
                    case "20": intRow = 34; ssView_Sheet1.Cells[34, 22].Value = true; break;     //톡소플라즈마증
                    case "21": intRow = 35; ssView_Sheet1.Cells[35, 6].Value = true; break;      //파종성 또는 폐외 히스토플리스마증
                    case "22": intRow = 35; ssView_Sheet1.Cells[35, 22].Value = true; break;     //hiv에 의한 소모증후군
                    case "23": intRow = 36; ssView_Sheet1.Cells[36, 6].Value = true; break;      //만성(1개월 이상) 장 이소스포라증
                    case "24": intRow = 36; ssView_Sheet1.Cells[36, 22].Value = true; break;     //기타
                }

                ssView_Sheet1.Cells[intRow, 25].Text = dt.Rows[0]["INFECT11_ETC"].ToString().Trim();    //기타 상세내역

                //면허번호
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DRNAME, DEPTCODE, DRBUNHO, DRCODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "     WHERE SABUN = '" + ComFunc.LPAD(dt.Rows[0]["DRSABUN"].ToString().Trim(), 5, "0") + "' ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt1.Rows.Count > 0)
                {
                    ssView_Sheet1.Cells[38, 7].Text = dt1.Rows[0]["DRNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[38, 18].Text = dt1.Rows[0]["DRBUNHO"].ToString().Trim();

                    cboDept.Text = dt1.Rows[0]["DEPTCODE"].ToString().Trim() + "." + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt1.Rows[0]["DEPTCODE"].ToString().Trim());
                    cboDr.Text = dt1.Rows[0]["DRCODE"].ToString().Trim() + "." + clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt1.Rows[0]["DRCODE"].ToString().Trim());
                }

                dt1.Dispose();
                dt1 = null;

                txtWard.Text = dt.Rows[0]["WARD"].ToString().Trim();
                txtRoom.Text = dt.Rows[0]["ROOM"].ToString().Trim();

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

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strOK = "NO";

            if (e.Row == 7)
            {
                ssView_Sheet1.Cells[e.Row, 6].Text = "";
                ssView_Sheet1.Cells[e.Row, 22].Text = "";
            }

            //최초진단일 확인검사기관
            if ((e.Row == 10 && (e.Column >= 23 && e.Column <= 24)) || (e.Row == 11 && (e.Column >= 23 && e.Column <= 34)))
            {
                ssView_Sheet1.Cells[9, 22].Text = "";
                ssView_Sheet1.Cells[10, 22].Text = "";
            }

            //확인진단일 확인검사기관
            if ((e.Row == 12 && (e.Column >= 23 && e.Column <= 24)) || (e.Row == 13 && (e.Column >= 23 && e.Column <= 34)))
            {
                ssView_Sheet1.Cells[11, 22].Text = "";
                ssView_Sheet1.Cells[12, 22].Text = "";
            }

            //검사소견
            if (e.Row == 14 && (e.Column >= 6 && e.Column <= 13)) { strOK = "OK"; }
            if (e.Row == 15 && (e.Column >= 6 && e.Column <= 13)) { strOK = "OK"; }
            if (e.Row == 16 && (e.Column >= 6 && e.Column <= 11)) { strOK = "OK"; }

            if (strOK == "OK")
            {
                ssView_Sheet1.Cells[14, 6].Text = "";
                ssView_Sheet1.Cells[14, 14].Text = "";

                ssView_Sheet1.Cells[15, 6].Text = "";
                ssView_Sheet1.Cells[15, 14].Text = "";

                ssView_Sheet1.Cells[16, 6].Text = "";
            }

            strOK = "NO";

            //추정 감염경로
            if (e.Row == 13 && (e.Column >= 28 && e.Column <= 39)) { strOK = "OK"; }
            if (e.Row == 14 && (e.Column >= 28 && e.Column <= 39)) { strOK = "OK"; }

            if (e.Row == 15 && (e.Column >= 28 && e.Column <= 39)) { strOK = "OK"; }

            if (e.Row == 16 && (e.Column >= 28 && e.Column <= 33)) { strOK = "OK"; }
            if (e.Row == 16 && (e.Column >= 34 && e.Column <= 39)) { strOK = "OK"; }

            if (e.Row == 17 && (e.Column >= 28 && e.Column <= 33)) { strOK = "OK"; }

            if (e.Row == 18 && (e.Column >= 28 && e.Column <= 30)) { strOK = "OK"; }

            if (strOK == "OK")
            {
                ssView_Sheet1.Cells[13, 28].Text = "";
                ssView_Sheet1.Cells[14, 28].Text = "";
                ssView_Sheet1.Cells[15, 28].Text = "";

                ssView_Sheet1.Cells[16, 28].Text = "";
                ssView_Sheet1.Cells[16, 34].Text = "";

                ssView_Sheet1.Cells[17, 28].Text = "";

                ssView_Sheet1.Cells[18, 28].Text = "";
                ssView_Sheet1.Cells[18, 31].Text = "";
            }

            //사망여부
            if (e.Row == 19)
            {
                ssView_Sheet1.Cells[e.Row, 6].Text = "";
                ssView_Sheet1.Cells[e.Row, 10].Text = "";
            }

            //사망과 후천성면역결핍증과의 관련성
            if (e.Row == 22)
            {
                ssView_Sheet1.Cells[e.Row, 32].Text = "";
                ssView_Sheet1.Cells[e.Row, 36].Text = "";
            }

            //후천성면역 결핍증환자 관련 임상증상
            strOK = "NO";

            if (e.Row == 23 && ((e.Column >= 6 && e.Column <= 20) || (e.Column >= 22 && e.Column <= 37))) { strOK = "OK"; }
            if (e.Row == 24 && ((e.Column >= 6 && e.Column <= 20) || (e.Column >= 22 && e.Column <= 37))) { strOK = "OK"; }
            if (e.Row == 25 && ((e.Column >= 6 && e.Column <= 20) || (e.Column >= 22 && e.Column <= 37))) { strOK = "OK"; }
            if (e.Row == 26 && (e.Column >= 22 && e.Column <= 37)) { strOK = "OK"; }
            if (e.Row == 27 && ((e.Column >= 6 && e.Column <= 20) || (e.Column >= 22 && e.Column <= 37))) { strOK = "OK"; }
            if (e.Row == 28 && ((e.Column >= 6 && e.Column <= 20) || (e.Column >= 22 && e.Column <= 37))) { strOK = "OK"; }
            if (e.Row == 29 && ((e.Column >= 6 && e.Column <= 20) || (e.Column >= 22 && e.Column <= 37))) { strOK = "OK"; }

            if (e.Row == 30 && (e.Column >= 22 && e.Column <= 37)) { strOK = "OK"; }

            if (e.Row == 31 && ((e.Column >= 6 && e.Column <= 20) || (e.Column >= 22 && e.Column <= 37))) { strOK = "OK"; }

            if (e.Row == 32 && ((e.Column >= 6 && e.Column <= 20) || (e.Column >= 22 && e.Column <= 37))) { strOK = "OK"; }
            if (e.Row == 33 && ((e.Column >= 6 && e.Column <= 20) || (e.Column >= 22 && e.Column <= 37))) { strOK = "OK"; }
            if (e.Row == 34 && ((e.Column >= 6 && e.Column <= 20) || (e.Column >= 22 && e.Column <= 37))) { strOK = "OK"; }

            if (e.Row == 35 && ((e.Column >= 6 && e.Column <= 20) || (e.Column >= 22 && e.Column <= 37))) { strOK = "OK"; }

            if (e.Row == 36 && ((e.Column >= 6 && e.Column <= 20) || (e.Column >= 22 && e.Column <= 24))) { strOK = "OK"; }

            if (strOK == "OK")
            {
                ssView_Sheet1.Cells[23, 6].Text = "";
                ssView_Sheet1.Cells[23, 22].Text = "";
                ssView_Sheet1.Cells[24, 6].Text = "";
                ssView_Sheet1.Cells[24, 22].Text = "";
                ssView_Sheet1.Cells[25, 6].Text = "";
                ssView_Sheet1.Cells[25, 22].Text = "";
                ssView_Sheet1.Cells[26, 22].Text = "";
                ssView_Sheet1.Cells[27, 6].Text = "";
                ssView_Sheet1.Cells[27, 22].Text = "";
                ssView_Sheet1.Cells[28, 6].Text = "";
                ssView_Sheet1.Cells[29, 6].Text = "";
                ssView_Sheet1.Cells[29, 22].Text = "";
                ssView_Sheet1.Cells[30, 22].Text = "";
                ssView_Sheet1.Cells[31, 6].Text = "";
                ssView_Sheet1.Cells[31, 22].Text = "";
                ssView_Sheet1.Cells[32, 6].Text = "";
                ssView_Sheet1.Cells[32, 22].Text = "";
                ssView_Sheet1.Cells[33, 6].Text = "";
                ssView_Sheet1.Cells[33, 22].Text = "";
                ssView_Sheet1.Cells[34, 22].Text = "";
                ssView_Sheet1.Cells[35, 6].Text = "";
                ssView_Sheet1.Cells[35, 22].Text = "";
                ssView_Sheet1.Cells[36, 6].Text = "";
                ssView_Sheet1.Cells[36, 22].Text = "";
                ssView_Sheet1.Cells[36, 25].Text = "";
            }
        }
    }
}
