using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : FrmDrugSubScrip.cs
    /// Description     : 신규약품구입신청서
    /// Author          : 이정현
    /// Create Date     : 2017-11-29
    /// <history> 
    /// 신규약품구입신청서
    /// </history>
    /// <seealso>
    /// PSMH\drug\drcode\FrmDrugSubScripSign.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drcode\drcode.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstDrugSubScrip : Form
    {
        private string GstrSign = "";
        private string GstrSign_Chasu = "";
        private string GstrSign_Gbn = "";
        private string GstrSign_Date = "";
        private string GstrSign_Sabun = "";
        private string GstrSign_Etc = "";
        private string GstrSign_EM_OK = "";
        private string GstrROWID = "";
        private string GstrSEQNO = "";
        private string GstrDRUGOK = "";
        private string GstrFast = "";

        public frmSupDrstDrugSubScrip()
        {
            InitializeComponent();
        }

        private void frmSupDrstDrugSubScrip_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpSDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddMonths(-1);
            dtpEDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            CLEAR_SCREEN();

            GetData();
        }

        private void CLEAR_SCREEN()
        {
            GstrSign = "";
            GstrSign_Chasu = "";
            GstrSign_Gbn = "";
            GstrSign_Date = "";
            GstrSign_Sabun = "";
            GstrSign_Etc = "";
            GstrROWID = "";
            GstrSEQNO = "";
            GstrDRUGOK = "";
            GstrFast = "";

            lblProgress.Text = "";

            SetSpreadPrint();
        }

        private void SetSpreadPrint()
        {
            //CheckBox
            ssPrint_Sheet1.Cells[3, 3].Value = false;       //긴급성 : 정상처리
            ssPrint_Sheet1.Cells[4, 3].Value = false;       //긴급성 : 긴급구입
            ssPrint_Sheet1.Cells[7, 24].Value = false;      //보험급여 : 보험약
            ssPrint_Sheet1.Cells[7, 27].Value = false;      //보험급여 : 비보험약
            ssPrint_Sheet1.Cells[8, 3].Value = false;       //제형 : 경구약
            ssPrint_Sheet1.Cells[8, 7].Value = false;       //제형 : 주사약
            ssPrint_Sheet1.Cells[8, 11].Value = false;      //제형 : 외용약
            ssPrint_Sheet1.Cells[8, 15].Value = false;      //제형 : 기타
            ssPrint_Sheet1.Cells[11, 24].Value = false;     //유사약품 : 있음
            ssPrint_Sheet1.Cells[12, 24].Value = false;     //유사약품 : 없음
            ssPrint_Sheet1.Cells[13, 4].Value = false;      //사용방법 : 원내외혼용
            ssPrint_Sheet1.Cells[13, 11].Value = false;     //사용방법 : 원외전용
            ssPrint_Sheet1.Cells[14, 4].Value = false;      //사용방법 : 입원전용
            ssPrint_Sheet1.Cells[14, 11].Value = false;     //사용방법 : 원내전용
            ssPrint_Sheet1.Cells[13, 24].Value = false;     //대체약품 : 있음
            ssPrint_Sheet1.Cells[14, 24].Value = false;     //대체약품 : 없음
            ssPrint_Sheet1.Cells[15, 3].Value = false;      //소모예정량 : 많음
            ssPrint_Sheet1.Cells[15, 7].Value = false;      //소모예정량 : 중간
            ssPrint_Sheet1.Cells[15, 11].Value = false;     //소모예정량 : 적음
            ssPrint_Sheet1.Cells[20, 23].Value = false;     //신청인 : 확인함
            ssPrint_Sheet1.Cells[20, 27].Value = false;     //신청인 : 필요없음
            ssPrint_Sheet1.Cells[26, 4].Value = false;      //처리방법 : 긴급처리
            ssPrint_Sheet1.Cells[26, 10].Value = false;     //처리방법 : 정규처리
            ssPrint_Sheet1.Cells[29, 9].Value = false;      //긴급처리 : 승인
            ssPrint_Sheet1.Cells[29, 13].Value = false;     //긴급처리 : 비승인
            ssPrint_Sheet1.Cells[34, 7].Value = false;      //정규처리내용 : 가결
            ssPrint_Sheet1.Cells[34, 10].Value = false;     //정규처리내용 : 조건부 가결
            ssPrint_Sheet1.Cells[34, 16].Value = false;     //정규처리내용 : 보류
            ssPrint_Sheet1.Cells[34, 20].Value = false;     //정규처리내용 : 부결

            //TextBox
            ssPrint_Sheet1.Cells[4, 11].Text = "";          //긴급성 : 긴급구입사유
            ssPrint_Sheet1.Cells[6, 6].Text = "";           //약품명 : 한글
            ssPrint_Sheet1.Cells[7, 6].Text = "";           //약품명 : 영문
            ssPrint_Sheet1.Cells[6, 24].Text = "";          //분류번호
            ssPrint_Sheet1.Cells[8, 24].Text = "";          //보험코드
            ssPrint_Sheet1.Cells[9, 3].Text = "";           //성분/규격
            ssPrint_Sheet1.Cells[9, 24].Text = "";          //제약회사
            ssPrint_Sheet1.Cells[10, 24].Text = "";         //국내시판일
            ssPrint_Sheet1.Cells[11, 3].Text = "";          //약가
            ssPrint_Sheet1.Cells[11, 27].Text = "";         //유사약품
            ssPrint_Sheet1.Cells[13, 27].Text = "";         //대체약품
            ssPrint_Sheet1.Cells[15, 24].Text = "";         //월간소모예정량
            ssPrint_Sheet1.Cells[16, 3].Text = "";          //약리작용
            ssPrint_Sheet1.Cells[17, 3].Text = "";          //보험인정적응증
            ssPrint_Sheet1.Cells[18, 3].Text = "";          //신청사유
            ssPrint_Sheet1.Cells[20, 3].Text = "";          //신청일
            ssPrint_Sheet1.Cells[20, 12].Text = "";         //신청인
            ssPrint_Sheet1.Cells[21, 3].Text = "";          //제출일
            ssPrint_Sheet1.Cells[21, 12].Text = "";         //접수확인
            ssPrint_Sheet1.Cells[28, 9].Text = "";          //처리일자
            ssPrint_Sheet1.Cells[29, 20].Text = "";         //비승인사유
            ssPrint_Sheet1.Cells[30, 9].Text = "";          //약무위원장
            ssPrint_Sheet1.Cells[33, 4].Text = "";          //정규처리차수
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

            CLEAR_SCREEN();

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SINDATE, NAMEK, DEPTCODE, SINNAME, SIGN, SIGN_GBN, ROWID, DRUGOK, DRUGJEPSU, DRUGSEND";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT";
                SQL = SQL + ComNum.VBLF + "     WHERE SINDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND SINDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";

                if (chkMy.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND WSABUN = " + clsType.User.Sabun;
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY SINDATE DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = Convert.ToDateTime(dt.Rows[i]["SINDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAMEK"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SINNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = SIGN_GUBUN(dt.Rows[i]["SIGN"].ToString().Trim(), dt.Rows[i]["SIGN_GBN"].ToString().Trim(), dt.Rows[i]["DRUGOK"].ToString().Trim());

                        if (ssList_Sheet1.Cells[i, 4].Text.Trim() == "신청" && dt.Rows[i]["DRUGJEPSU"].ToString().Trim() != "")
                        {
                            ssList_Sheet1.Cells[i, 4].Text = "접수";
                        }

                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        private string SIGN_GUBUN(string strArg1, string strArg2, string strArg3)
        {
            string rtnVal = "";

            if (strArg3 == "1") { }
            else
            {
                rtnVal = "미신청";
                return rtnVal;
            }

            if (strArg1 == "1")
            {
                rtnVal = "긴급승인";
                return rtnVal;
            }

            switch (strArg2)
            {
                case "1": rtnVal = "가결"; break;
                case "2": rtnVal = "조건부가결"; break;
                case "3": rtnVal = "보류"; break;
                case "4": rtnVal = "부결"; break;
            }

            if (strArg1 == "" && strArg2 == "")
            {
                rtnVal = "신청";
            }

            return rtnVal;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (SAVE_OK() != "OK")
            {
                ComFunc.MsgBox("신청기간 이외엔 '긴급구입 희망' 약제만 신청이 가능합니다.");
                //return;
            }

            CLEAR_SCREEN();

            if (ChkYakuk(clsType.User.Sabun) == false)
            {
                string SQL = "";
                DataTable dt = null;
                string SqlErr = "";

                try
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     DEPTCODE, DRNAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                    SQL = SQL + ComNum.VBLF + "     WHERE SABUN = '" + clsType.User.Sabun + "'";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        ComFunc.MsgBox("진료과장님만 신청이 가능합니다.");
                        dt.Dispose();
                        dt = null;
                        return;
                    }
                    else
                    {
                        //신청과, 신청인
                        ssPrint_Sheet1.Cells[20, 12].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + " " + dt.Rows[0]["DRNAME"].ToString().Trim();
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

            //신청일
            ssPrint_Sheet1.Cells[20, 3].Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssPrint_Sheet1.SetActiveCell(6, 6);
        }

        private bool ChkYakuk(string strSabun)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT* FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_ERP + "INSA_CODE B";
                SQL = SQL + ComNum.VBLF + "    WHERE A.SABUN = '" + strSabun + "'";
                SQL = SQL + ComNum.VBLF + "        AND B.GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "        AND B.CODE IN ('40', '41', '42', '43')";
                SQL = SQL + ComNum.VBLF + "        AND A.JIK = TRIM(B.CODE)";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string SAVE_OK()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ""; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_DC_JOB ";
                SQL = SQL + ComNum.VBLF + "     WHERE JOBDATE = TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GstrROWID == "")
            {
                GetData();
            }
            else
            {
                READ_DATA_SPREAD(GstrROWID);
            }
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true && e.RowHeader == true) { return; }

            string strROWID = ssList_Sheet1.Cells[e.Row, 5].Text.Trim();

            CLEAR_SCREEN();

            READ_DATA_SPREAD(strROWID);
        }

        private void READ_DATA_SPREAD(string strROWID)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            CLEAR_SCREEN();

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SEQNO, WDATE, WSABUN, SINNAME, SINDATE, DEPTCODE, CERTI, NAMEK,";
                SQL = SQL + ComNum.VBLF + "     NAMEE, BUN, BUHUM, BUHUMCODE, JEHYENG, SUNGBUN, JEYAK, FIRSTPANME,";
                SQL = SQL + ComNum.VBLF + "     YAKCOST, SAME, SAMENAME, USED, DIFF, DIFFNAME, FAST, FASTSAYU,";
                SQL = SQL + ComNum.VBLF + "     YAKRI, JUKNG, SINCHENG, SOMOGBN, SOMO_MONTH, SIGN, SIGN_CHASU, SIGN_GBN,";
                SQL = SQL + ComNum.VBLF + "     SIGN_DATE, SIGN_SABUN, SIGN_ETC, ROWID, DRUGSEND, DRUGJEPSU, SIGN_EM_OK, DRUGOK";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT ";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GstrSign = dt.Rows[0]["SIGN"].ToString().Trim();
                    GstrSign_Chasu = dt.Rows[0]["SIGN_CHASU"].ToString().Trim();
                    GstrSign_Gbn = dt.Rows[0]["SIGN_GBN"].ToString().Trim();
                    GstrSign_Date = dt.Rows[0]["SIGN_DATE"].ToString().Trim();
                    GstrSign_Sabun = dt.Rows[0]["SIGN_SABUN"].ToString().Trim();
                    GstrSign_Etc = dt.Rows[0]["SIGN_ETC"].ToString().Trim();
                    GstrSign_EM_OK = dt.Rows[0]["SIGN_EM_OK"].ToString().Trim();
                    GstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    GstrSEQNO = dt.Rows[0]["SEQNO"].ToString().Trim();
                    
                    switch (dt.Rows[0]["FAST"].ToString().Trim())
                    {
                        case "1":
                            ssPrint_Sheet1.Cells[3, 3].Value = true;
                            break;
                        case "2":
                            ssPrint_Sheet1.Cells[4, 3].Value = true;
                            ssPrint_Sheet1.Cells[4, 11].Text = dt.Rows[0]["FASTSAYU"].ToString().Trim();
                            break;
                    }

                    ssPrint_Sheet1.Cells[6, 6].Text = dt.Rows[0]["NAMEK"].ToString().Trim();
                    ssPrint_Sheet1.Cells[7, 6].Text = dt.Rows[0]["NAMEE"].ToString().Trim();

                    ssPrint_Sheet1.Cells[6, 24].Text = dt.Rows[0]["BUN"].ToString().Trim();

                    switch (dt.Rows[0]["BUHUM"].ToString().Trim())
                    {
                        case "1": ssPrint_Sheet1.Cells[7, 24].Value = true; break;
                        case "2": ssPrint_Sheet1.Cells[7, 27].Value = true; break;
                    }

                    switch (dt.Rows[0]["JEHYENG"].ToString().Trim())
                    {
                        case "1": ssPrint_Sheet1.Cells[8, 3].Value = true; break;
                        case "2": ssPrint_Sheet1.Cells[8, 7].Value = true; break;
                        case "3": ssPrint_Sheet1.Cells[8, 11].Value = true; break;
                        case "4": ssPrint_Sheet1.Cells[8, 15].Value = true; break;
                    }

                    ssPrint_Sheet1.Cells[8, 24].Text = dt.Rows[0]["BUHUMCODE"].ToString().Trim();

                    ssPrint_Sheet1.Cells[9, 3].Text = dt.Rows[0]["SUNGBUN"].ToString().Trim();
                    ssPrint_Sheet1.Cells[9, 24].Text = dt.Rows[0]["JEYAK"].ToString().Trim();

                    ssPrint_Sheet1.Cells[10, 24].Text = dt.Rows[0]["FIRSTPANME"].ToString().Trim();

                    ssPrint_Sheet1.Cells[11, 3].Text = dt.Rows[0]["YAKCOST"].ToString().Trim();

                    switch (dt.Rows[0]["SAME"].ToString().Trim())
                    {
                        case "1":
                            ssPrint_Sheet1.Cells[11, 24].Value = true;
                            ssPrint_Sheet1.Cells[11, 27].Text = dt.Rows[0]["SAMENAME"].ToString().Trim();
                            break;
                        case "2":
                            ssPrint_Sheet1.Cells[12, 24].Value = true;
                            break;
                    }

                    switch (dt.Rows[0]["USED"].ToString().Trim())
                    {
                        case "1": ssPrint_Sheet1.Cells[13, 4].Value = true; break;
                        case "2": ssPrint_Sheet1.Cells[13, 11].Value = true; break;
                        case "3": ssPrint_Sheet1.Cells[14, 4].Value = true; break;
                        case "4": ssPrint_Sheet1.Cells[14, 11].Value = true; break;
                    }

                    switch (dt.Rows[0]["DIFF"].ToString().Trim())
                    {
                        case "1":
                            ssPrint_Sheet1.Cells[13, 24].Value = true;
                            ssPrint_Sheet1.Cells[13, 27].Text = dt.Rows[0]["DIFFNAME"].ToString().Trim();
                            break;
                        case "2":
                            ssPrint_Sheet1.Cells[14, 24].Value = true;
                            break;
                    }

                    switch (dt.Rows[0]["SOMOGBN"].ToString().Trim())
                    {
                        case "1": ssPrint_Sheet1.Cells[15, 3].Value = true; break;
                        case "2": ssPrint_Sheet1.Cells[15, 7].Value = true; break;
                        case "3": ssPrint_Sheet1.Cells[15, 11].Value = true; break;
                    }

                    ssPrint_Sheet1.Cells[15, 24].Text = dt.Rows[0]["SOMO_MONTH"].ToString().Trim();

                    ssPrint_Sheet1.Cells[16, 3].Text = dt.Rows[0]["YAKRI"].ToString().Trim();
                    ssPrint_Sheet1.Cells[17, 3].Text = dt.Rows[0]["JUKNG"].ToString().Trim();
                    ssPrint_Sheet1.Cells[18, 3].Text = dt.Rows[0]["SINCHENG"].ToString().Trim();

                    ssPrint_Sheet1.Cells[20, 3].Text = VB.Left(dt.Rows[0]["SINDATE"].ToString().Trim(),10);
                    ssPrint_Sheet1.Cells[20, 12].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + " " + dt.Rows[0]["SINNAME"].ToString().Trim();

                    switch (dt.Rows[0]["CERTI"].ToString().Trim())
                    {
                        case "1": ssPrint_Sheet1.Cells[20, 23].Value = true; break;
                        case "2": ssPrint_Sheet1.Cells[20, 27].Value = true; break;
                    }

                    ssPrint_Sheet1.Cells[21, 3].Text = dt.Rows[0]["DRUGSEND"].ToString().Trim();
                    ssPrint_Sheet1.Cells[21, 12].Text = dt.Rows[0]["DRUGJEPSU"].ToString().Trim();

                    switch (GstrSign)
                    {
                        case "1": ssPrint_Sheet1.Cells[26, 4].Value = true; break;
                        case "2": ssPrint_Sheet1.Cells[26, 10].Value = true; break;
                    }

                    if (GstrSign == "1")
                    {
                        ssPrint_Sheet1.Cells[28, 9].Text = GstrSign_Date;

                        switch (GstrSign_EM_OK)
                        {
                            case "1":
                                ssPrint_Sheet1.Cells[29, 9].Value = true;
                                break;
                            case "2":
                                ssPrint_Sheet1.Cells[29, 13].Value = true;
                                ssPrint_Sheet1.Cells[29, 20].Text = GstrSign_Etc;
                                break;
                        }
                    }
                    else
                    {
                        ssPrint_Sheet1.Cells[33, 4].Text = GstrSign_Chasu;

                        switch (GstrSign_Gbn)
                        {
                            case "1": ssPrint_Sheet1.Cells[24, 7].Value = true; break;
                            case "2": ssPrint_Sheet1.Cells[24, 10].Value = true; break;
                            case "3": ssPrint_Sheet1.Cells[24, 16].Value = true; break;
                            case "4": ssPrint_Sheet1.Cells[24, 20].Value = true; break;
                        }
                    }

                    switch (SIGN_GUBUN(GstrSign, GstrSign_Gbn, dt.Rows[0]["DRUGOK"].ToString().Trim()))
                    {
                        case "미신청":
                            lblProgress.Text = "약제팀으로 제출되지 않은 신청서입니다.";
                            break;
                        case "긴급승인":
                            lblProgress.Text = "해당 신청서는 긴급승인 되었습니다.";
                            break;
                        case "가결":
                            lblProgress.Text = "해당 신청서는 가결 되었습니다.";
                            break;
                        case "조건부가결":
                            lblProgress.Text = "해당 신청서는 조건부 가결되었습니다.";
                            break;
                        case "보류":
                            lblProgress.Text = "해당 신청서는 보류 되었습니다.";
                            break;
                        case "부결":
                            lblProgress.Text = "해당 신청서는 부결 되었습니다.";
                            break;
                        default:
                            if (dt.Rows[0]["DRUGJEPSU"].ToString().Trim() != "")
                            {
                                lblProgress.Text = "약제팀에서 처리 중인 신청서 입니다.";
                            }
                            else
                            {
                                lblProgress.Text = "약제팀에서 접수 대기 중인 신청서 입니다.";
                            }
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("신청서를 저장하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                if (SAVE_DATA(GstrROWID, GstrSEQNO) == true)
                {
                    GetData();
                }
            }
        }

        private bool SAVE_DATA(string strROWID, string strSEQNO)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strWDATE = "";
            string strWSABUN = "";
            string strSINNAME = "";
            string strSINDATE = "";
            string strDeptCode = "";
            string strCERTI = "";
            string strNAMEK = "";
            string strNAMEE = "";
            string strBun = "";
            string strBUHUM = "";
            string strBUHUMCODE = "";
            string strJEHYENG = "";
            string strSUNGBUN = "";
            string strJeyak = "";
            string strFIRSTPANME = "";
            string strYAKCOST = "";
            string strSAME = "";
            string strSAMENAME = "";
            string strUSED = "";
            string strDIFF = "";
            string strDIFFNAME = "";
            string strFAST = "";
            string strFASTSAYU = "";
            string strYAKRI = "";
            string strJUKNG = "";
            string strSINCHENG = "";
            string strSOMOGBN = "";
            string strSOMO_MONTH = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strROWID == "")
                {
                    strSEQNO = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_ERP.Replace(".", ""), "SEQ_DRUGDCSUBSCRIPT").ToString();
                }

                strWDATE = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                strWSABUN = clsType.User.Sabun;
                strSINNAME = VB.Mid(ssPrint_Sheet1.Cells[20, 12].Text, 3, ssPrint_Sheet1.Cells[20, 12].Text.Length).Trim();     //신청인
                strSINDATE = ssPrint_Sheet1.Cells[20, 3].Text;                          //신청일
                strDeptCode = VB.Left(ssPrint_Sheet1.Cells[20, 12].Text, 3).Trim();     //신청과

                //신청인 확인
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[20, 23].Value) == true) { strCERTI = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[20, 27].Value) == true) { strCERTI = "2"; }

                strNAMEK = ssPrint_Sheet1.Cells[6, 6].Text;         //약품명(한글)
                strNAMEE = ssPrint_Sheet1.Cells[7, 6].Text;         //약품명(영문)
                strBun = ssPrint_Sheet1.Cells[6, 24].Text;          //분류번호

                //보험급여
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[7, 24].Value) == true) { strBUHUM = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[7, 27].Value) == true) { strBUHUM = "2"; }

                //보험코드
                strBUHUMCODE = ssPrint_Sheet1.Cells[8, 24].Text;

                //제형
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[8, 3].Value) == true) { strJEHYENG = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[8, 7].Value) == true) { strJEHYENG = "2"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[8, 11].Value) == true) { strJEHYENG = "3"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[8, 15].Value) == true) { strJEHYENG = "4"; }

                strSUNGBUN = ssPrint_Sheet1.Cells[9, 3].Text;           //성분/규격
                strJeyak = ssPrint_Sheet1.Cells[9, 24].Text;            //제약회사
                strFIRSTPANME = ssPrint_Sheet1.Cells[10, 24].Text;      //국내시판일
                strYAKCOST = ssPrint_Sheet1.Cells[11, 3].Text;          //약가

                //유사약품
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[11, 24].Value) == true) { strSAME = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[12, 24].Value) == true) { strSAME = "2"; }
                
                strSAMENAME = ssPrint_Sheet1.Cells[11, 27].Text;        //유사약품

                //사용방법
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[13, 4].Value) == true) { strUSED = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[13, 11].Value) == true) { strUSED = "2"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[14, 4].Value) == true) { strUSED = "3"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[14, 11].Value) == true) { strUSED = "4"; }

                //유사약품
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[13, 24].Value) == true) { strDIFF = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[14, 24].Value) == true) { strDIFF = "2"; }
                
                strDIFFNAME = ssPrint_Sheet1.Cells[13, 27].Text;        //대체약품

                //긴급성
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[3, 3].Value) == true) { strFAST = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[4, 3].Value) == true) { strFAST = "2"; }

                strFASTSAYU = ssPrint_Sheet1.Cells[4, 11].Text;         //긴급구입사유
                strYAKRI = ssPrint_Sheet1.Cells[16, 3].Text;            //약리작용
                strJUKNG = ssPrint_Sheet1.Cells[17, 3].Text;            //보험인정
                strSINCHENG = ssPrint_Sheet1.Cells[18, 3].Text;         //신청사유

                //소모예정량
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[15, 3].Value) == true) { strSOMOGBN = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[15, 7].Value) == true) { strSOMOGBN = "2"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[15, 11].Value) == true) { strSOMOGBN = "3"; }
                
                strSOMO_MONTH = ssPrint_Sheet1.Cells[15, 24].Text;      //월간소모예정량

                if (strROWID != "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT_HISTORY";
                    SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT";
                    SQL = SQL + ComNum.VBLF + "         WHERE ROWID = '" + strROWID + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = "DELETE " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT";
                SQL = SQL + ComNum.VBLF + "     (SEQNO, WDATE, WSABUN, SINNAME, SINDATE, DeptCode, CERTI, NAMEK,";
                SQL = SQL + ComNum.VBLF + "     NAMEE, BUN, BUHUM, BUHUMCODE, JEHYENG, SUNGBUN, JEYAK, FIRSTPANME,";
                SQL = SQL + ComNum.VBLF + "     YAKCOST, SAME, SAMENAME, USED, DIFF, DIFFNAME, FAST, FASTSAYU,";
                SQL = SQL + ComNum.VBLF + "     YAKRI, JUKNG, SINCHENG, SOMOGBN, SOMO_MONTH)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         " + strSEQNO + ", ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ", ";
                SQL = SQL + ComNum.VBLF + "         '" + strSINNAME + "', ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strSINDATE + "', 'YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "         '" + strDeptCode + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strCERTI + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strNAMEK + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strNAMEE + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strBun + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strBUHUM + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strBUHUMCODE + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strJEHYENG + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strSUNGBUN + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strJeyak + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strFIRSTPANME + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strYAKCOST + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strSAME + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strSAMENAME + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strUSED + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strDIFF + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strDIFFNAME + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strFAST + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strFASTSAYU + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strYAKRI + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strJUKNG + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strSINCHENG + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strSOMOGBN + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strSOMO_MONTH + "' ";
                SQL = SQL + ComNum.VBLF + "     )";

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

        bool SAVE_DATA(string argROWID)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            //string strSEQNO ;
            string strWDATE ;
            string strWSABUN ;
            string strSINNAME ;
            string strSINDATE ;
            string strDEPTCODE ;
            string strCERTI ;
            string strNAMEK ;
            string strNAMEE ;
            string strBun ;
            string strBUHUM ;
            string strBUHUMCODE ;
            string strJEHYENG ;
            string strSUNGBUN ;
            string strJeyak ;
            string strFIRSTPANME ;
            string strYAKCOST ;
            string strSAME ;
            string strSAMENAME ;
            string strUSED ;
            string strDIFF ;
            string strDIFFNAME ;
            string strFAST ;
            string strFASTSAYU ;
            string strYAKRI ;
            string strJUKNG ;
            string strSINCHENG ;
            string strSOMOGBN ;
            string strSOMO_MONTH ;
            //string strSIGN ;
            //string strSIGN_CHASU ;
            //string strSIGN_GBN ;
            //string strSIGN_DATE ;
            //string strSIGN_SABUN ;
            //string strSIGN_ETC ;

            try
            {
                if(argROWID == "")
                {
                    ComFunc.MsgBox("수정할 신청서를 선택하여 주십시오.");
                    return rtnVal;
                }

                if (MessageBox.Show("신청서를 수정하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return rtnVal;
                }

                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(clsDB.DbCon);

                strWDATE = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                strWSABUN = clsType.User.Sabun;
                strSINNAME = VB.Mid(ssPrint_Sheet1.Cells[20, 12].Text, 3, ssPrint_Sheet1.Cells[20, 12].Text.Length).Trim();     //신청인
                strSINDATE = ssPrint_Sheet1.Cells[20, 3].Text;                          //신청일
                strDEPTCODE = VB.Left(ssPrint_Sheet1.Cells[20, 12].Text, 3).Trim();     //신청과

                //신청인 확인
                strCERTI = "";
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[20, 23].Value) == true) { strCERTI = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[20, 27].Value) == true) { strCERTI = "2"; }

                strNAMEK = ssPrint_Sheet1.Cells[6, 6].Text;         //약품명(한글)
                strNAMEE = ssPrint_Sheet1.Cells[7, 6].Text;         //약품명(영문)
                strBun = ssPrint_Sheet1.Cells[6, 24].Text;          //분류번호

                //보험급여
                strBUHUM = "";
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[7, 24].Value) == true) { strBUHUM = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[7, 27].Value) == true) { strBUHUM = "2"; }

                //보험코드
                strBUHUMCODE = ssPrint_Sheet1.Cells[8, 24].Text;

                //제형
                strJEHYENG = "";
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[8, 3].Value) == true) { strJEHYENG = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[8, 7].Value) == true) { strJEHYENG = "2"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[8, 11].Value) == true) { strJEHYENG = "3"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[8, 15].Value) == true) { strJEHYENG = "4"; }

                strSUNGBUN = ssPrint_Sheet1.Cells[9, 3].Text;           //성분/규격
                strJeyak = ssPrint_Sheet1.Cells[9, 24].Text;            //제약회사
                strFIRSTPANME = ssPrint_Sheet1.Cells[10, 24].Text;      //국내시판일
                strYAKCOST = ssPrint_Sheet1.Cells[11, 3].Text;          //약가

                //유사약품
                strSAME = "";
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[11, 24].Value) == true) { strSAME = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[12, 24].Value) == true) { strSAME = "2"; }

                strSAMENAME = ssPrint_Sheet1.Cells[11, 27].Text;        //유사약품

                //사용방법
                strUSED = "";
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[13, 4].Value) == true) { strUSED = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[13, 11].Value) == true) { strUSED = "2"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[14, 4].Value) == true) { strUSED = "3"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[14, 11].Value) == true) { strUSED = "4"; }

                //유사약품
                strDIFF = "";
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[13, 24].Value) == true) { strDIFF = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[14, 24].Value) == true) { strDIFF = "2"; }

                strDIFFNAME = ssPrint_Sheet1.Cells[13, 27].Text;        //대체약품

                //긴급성
                strFAST = "";
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[3, 3].Value) == true) { strFAST = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[4, 3].Value) == true) { strFAST = "2"; }

                strFASTSAYU = ssPrint_Sheet1.Cells[4, 11].Text;         //긴급구입사유
                strYAKRI = ssPrint_Sheet1.Cells[16, 3].Text;            //약리작용
                strJUKNG = ssPrint_Sheet1.Cells[17, 3].Text;            //보험인정
                strSINCHENG = ssPrint_Sheet1.Cells[18, 3].Text;         //신청사유

                //소모예정량
                strSOMOGBN = "";
                if (Convert.ToBoolean(ssPrint_Sheet1.Cells[15, 3].Value) == true) { strSOMOGBN = "1"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[15, 7].Value) == true) { strSOMOGBN = "2"; }
                else if (Convert.ToBoolean(ssPrint_Sheet1.Cells[15, 11].Value) == true) { strSOMOGBN = "3"; }

                strSOMO_MONTH = ssPrint_Sheet1.Cells[15, 24].Text;      //월간소모예정량

                if (argROWID != "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT_HISTORY";
                    SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT";
                    SQL = SQL + ComNum.VBLF + "         WHERE ROWID = '" + argROWID + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }                    
                }

                SQL = " UPDATE ADMIN.DRUG_DC_SUBSCRIPT SET ";
                SQL += ComNum.VBLF + "     D_UPDATE = SYSDATE, ";
                SQL += ComNum.VBLF + "     D_SABUN = " + strWSABUN + ", ";
                SQL += ComNum.VBLF + "     CERTI = '" + strCERTI + "', ";
                SQL += ComNum.VBLF + "     NAMEK= '" + strNAMEK + "', ";
                SQL += ComNum.VBLF + "     NAMEE= '" + strNAMEE + "', ";
                SQL += ComNum.VBLF + "     BUN= '" + strBun + "', ";
                SQL += ComNum.VBLF + "     BUHUM= '" + strBUHUM + "', ";
                SQL += ComNum.VBLF + "     BUHUMCODE= '" + strBUHUMCODE + "', ";
                SQL += ComNum.VBLF + "     JEHYENG= '" + strJEHYENG + "', ";
                SQL += ComNum.VBLF + "     SUNGBUN= '" + strSUNGBUN + "', ";
                SQL += ComNum.VBLF + "     JEYAK= '" + strJeyak + "', ";
                SQL += ComNum.VBLF + "     FIRSTPANME= '" + strFIRSTPANME + "', ";
                SQL += ComNum.VBLF + "     YAKCOST= '" + strYAKCOST + "', ";
                SQL += ComNum.VBLF + "     SAME= '" + strSAME + "', ";
                SQL += ComNum.VBLF + "     SAMENAME= '" + strSAMENAME + "', ";
                SQL += ComNum.VBLF + "     USED= '" + strUSED + "', ";
                SQL += ComNum.VBLF + "     DIFF= '" + strDIFF + "', ";
                SQL += ComNum.VBLF + "     DIFFNAME= '" + strDIFFNAME + "', ";
                SQL += ComNum.VBLF + "     FAST= '" + strFAST + "', ";
                SQL += ComNum.VBLF + "     FASTSAYU= '" + strFASTSAYU + "', ";
                SQL += ComNum.VBLF + "     YAKRI= '" + strYAKRI + "', ";
                SQL += ComNum.VBLF + "     JUKNG= '" + strJUKNG + "', ";
                SQL += ComNum.VBLF + "     SINCHENG= '" + strSINCHENG + "', ";
                SQL += ComNum.VBLF + "     SOMOGBN= '" + strSOMOGBN + "', ";
                SQL += ComNum.VBLF + "     SOMO_MONTH= '" + strSOMO_MONTH + "'  ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + argROWID + "' ";
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (GstrROWID == "")
            {
                ComFunc.MsgBox("수정할 신청서를 선택하시기 바랍니다.");
                return;
            }

            if (GstrDRUGOK == "1")
            {
                ComFunc.MsgBox("약제팀으로 제출이 완료된 신청서입니다."
                    + ComNum.VBLF + "수정이 불가능합니다.");
                return;
            }

            if (GstrSign_Date != "")
            {
                ComFunc.MsgBox("결재가 완료된 신청서입니다."
                    + ComNum.VBLF + "수정이 불가능합니다.");
                return;
            }

            READ_DATA(GstrSEQNO);
        }

        private void READ_DATA(string strSEQNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SEQNO, WDATE, WSABUN, SINNAME, SINDATE, DEPTCODE, CERTI, NAMEK,";
                SQL = SQL + ComNum.VBLF + "     NAMEE, BUN, BUHUM, BUHUMCODE, JEHYENG, SUNGBUN, JEYAK, FIRSTPANME,";
                SQL = SQL + ComNum.VBLF + "     YAKCOST, SAME, SAMENAME, USED, DIFF, DIFFNAME, FAST, FASTSAYU,";
                SQL = SQL + ComNum.VBLF + "     YAKRI, JUKNG, SINCHENG, SOMOGBN, SOMO_MONTH, SIGN, SIGN_CHASU, SIGN_GBN,";
                SQL = SQL + ComNum.VBLF + "     SIGN_DATE, SIGN_SABUN, SIGN_ETC, ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GstrSign = dt.Rows[0]["SIGN"].ToString().Trim();
                    GstrSign_Chasu = dt.Rows[0]["SIGN_CHASU"].ToString().Trim();
                    GstrSign_Gbn = dt.Rows[0]["SIGN_GBN"].ToString().Trim();
                    GstrSign_Date = dt.Rows[0]["SIGN_DATE"].ToString().Trim();
                    GstrSign_Sabun = dt.Rows[0]["SIGN_SABUN"].ToString().Trim();
                    GstrSign_Etc = dt.Rows[0]["SIGN_ETC"].ToString().Trim();
                    GstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    GstrSEQNO = dt.Rows[0]["SEQNO"].ToString().Trim();

                    if (dt.Rows[0]["FAST"].ToString().Trim() == "1")
                    {
                        ssPrint_Sheet1.Cells[3, 3].Value = true;       //긴급성 : 정상처리
                    }
                    else if (dt.Rows[0]["FAST"].ToString().Trim() == "2")
                    {
                        ssPrint_Sheet1.Cells[4, 3].Value = true;       //긴급성 : 긴급구입
                    }

                    ssPrint_Sheet1.Cells[4, 11].Text = dt.Rows[0]["FASTSAYU"].ToString().Trim();            //긴급성 : 긴급구입사유
                    ssPrint_Sheet1.Cells[6, 6].Text = dt.Rows[0]["NAMEK"].ToString().Trim();                //약품명 : 한글
                    ssPrint_Sheet1.Cells[7, 6].Text = dt.Rows[0]["NAMEE"].ToString().Trim();                //약품명 : 영문
                    ssPrint_Sheet1.Cells[6, 24].Text = dt.Rows[0]["BUN"].ToString().Trim();                 //분류번호

                    if (dt.Rows[0]["BUHUM"].ToString().Trim() == "1")
                    {
                        ssPrint_Sheet1.Cells[7, 24].Value = true;      //보험급여 : 보험약
                    }
                    else if (dt.Rows[0]["BUHUM"].ToString().Trim() == "2")
                    {
                        ssPrint_Sheet1.Cells[7, 27].Value = true;      //보험급여 : 비보험약
                    }

                    ssPrint_Sheet1.Cells[8, 24].Text = dt.Rows[0]["BUHUMCODE"].ToString().Trim();           //보험코드

                    if (dt.Rows[0]["JEHYENG"].ToString().Trim() == "1")
                    {
                        ssPrint_Sheet1.Cells[8, 3].Value = true;       //제형 : 경구약
                    }
                    else if (dt.Rows[0]["JEHYENG"].ToString().Trim() == "2")
                    {
                        ssPrint_Sheet1.Cells[8, 7].Value = true;       //제형 : 주사약
                    }
                    else if (dt.Rows[0]["JEHYENG"].ToString().Trim() == "3")
                    {
                        ssPrint_Sheet1.Cells[8, 11].Value = true;      //제형 : 외용약
                    }
                    else if (dt.Rows[0]["JEHYENG"].ToString().Trim() == "4")
                    {
                        ssPrint_Sheet1.Cells[8, 15].Value = true;      //제형 : 기타
                    }

                    ssPrint_Sheet1.Cells[9, 3].Text = dt.Rows[0]["SUNGBUN"].ToString().Trim();              //성분/규격
                    ssPrint_Sheet1.Cells[9, 24].Text = dt.Rows[0]["JEYAK"].ToString().Trim();               //제약회사
                    ssPrint_Sheet1.Cells[10, 24].Text = dt.Rows[0]["FIRSTPANME"].ToString().Trim();         //국내시판일

                    if (dt.Rows[0]["SAME"].ToString().Trim() == "1")
                    {
                        ssPrint_Sheet1.Cells[11, 24].Value = true;     //유사약품 : 있음
                    }
                    else if (dt.Rows[0]["SAME"].ToString().Trim() == "2")
                    {
                        ssPrint_Sheet1.Cells[12, 24].Value = true;     //유사약품 : 없음
                    }

                    ssPrint_Sheet1.Cells[11, 3].Text = dt.Rows[0]["YAKCOST"].ToString().Trim();             //약가
                    ssPrint_Sheet1.Cells[11, 27].Text = dt.Rows[0]["SAMENAME"].ToString().Trim();           //유사약품

                    if (dt.Rows[0]["USED"].ToString().Trim() == "1")
                    {
                        ssPrint_Sheet1.Cells[13, 4].Value = true;      //사용방법 : 원내외혼용
                    }
                    else if (dt.Rows[0]["USED"].ToString().Trim() == "2")
                    {
                        ssPrint_Sheet1.Cells[13, 11].Value = true;     //사용방법 : 원외전용
                    }
                    else if (dt.Rows[0]["USED"].ToString().Trim() == "3")
                    {
                        ssPrint_Sheet1.Cells[14, 4].Value = true;      //사용방법 : 입원전용
                    }
                    else if (dt.Rows[0]["USED"].ToString().Trim() == "4")
                    {
                        ssPrint_Sheet1.Cells[14, 11].Value = true;     //사용방법 : 원내전용
                    }

                    if (dt.Rows[0]["DIFF"].ToString().Trim() == "1")
                    {
                        ssPrint_Sheet1.Cells[13, 24].Value = false;     //대체약품 : 있음
                    }
                    else if (dt.Rows[0]["DIFF"].ToString().Trim() == "2")
                    {
                        ssPrint_Sheet1.Cells[14, 24].Value = false;     //대체약품 : 없음
                    }

                    ssPrint_Sheet1.Cells[13, 27].Text = dt.Rows[0]["DIFFNAME"].ToString().Trim();           //대체약품

                    if (dt.Rows[0]["SOMOGBN"].ToString().Trim() == "1")
                    {
                        ssPrint_Sheet1.Cells[15, 3].Value = false;      //소모예정량 : 많음
                    }
                    else if (dt.Rows[0]["SOMOGBN"].ToString().Trim() == "2")
                    {
                        ssPrint_Sheet1.Cells[15, 7].Value = false;      //소모예정량 : 중간
                    }
                    else if (dt.Rows[0]["SOMOGBN"].ToString().Trim() == "3")
                    {
                        ssPrint_Sheet1.Cells[15, 11].Value = false;     //소모예정량 : 적음
                    }

                    ssPrint_Sheet1.Cells[15, 24].Text = dt.Rows[0]["SOMO_MONTH"].ToString().Trim();         //월간소모예정량
                    ssPrint_Sheet1.Cells[16, 3].Text = dt.Rows[0]["YAKRI"].ToString().Trim();               //약리작용
                    ssPrint_Sheet1.Cells[17, 3].Text = dt.Rows[0]["JUKNG"].ToString().Trim();               //보험인정적응증
                    ssPrint_Sheet1.Cells[18, 3].Text = dt.Rows[0]["SINCHENG"].ToString().Trim();            //신청사유

                    if (dt.Rows[0]["CERTI"].ToString().Trim() == "1")
                    {
                        ssPrint_Sheet1.Cells[20, 23].Value = true;     //신청인 : 확인함
                    }
                    else if (dt.Rows[0]["CERTI"].ToString().Trim() == "2")
                    {
                        ssPrint_Sheet1.Cells[20, 27].Value = true;     //신청인 : 필요없음
                    }
                    
                    ssPrint_Sheet1.Cells[20, 3].Text = dt.Rows[0]["SINDATE"].ToString().Trim();             //신청일
                    ssPrint_Sheet1.Cells[20, 12].Text = dt.Rows[0]["DeptCode"].ToString().Trim() + " " + dt.Rows[0]["SINNAME"].ToString().Trim();         //신청인
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (GstrDRUGOK == "1")
            {
                ComFunc.MsgBox("약제팀으로 제출이 완료된 신청서입니다."
                    + ComNum.VBLF + "수정이 불가능합니다.");
                return;
            }

            if (GstrSign_Date != "")
            {
                ComFunc.MsgBox("결재가 완료된 신청서입니다."
                    + ComNum.VBLF + "수정이 불가능합니다.");
                return;
            }

            if (GstrROWID == "")
            {
                ComFunc.MsgBox("삭제할 신청서를 선택하시기 바랍니다.");
                return;
            }

            if (ComFunc.MsgBoxQ("신청서를 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                if (DELETE_DATA(GstrROWID) == true)
                {
                    GetData();
                }
            }
        }

        private bool DELETE_DATA(string strROWID)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT_HISTORY";
                SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT ";
                SQL = SQL + ComNum.VBLF + "         WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "";
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "' ";

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

        private void btnDrugSend_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;

            if (CHECK_DATA_SEND() == false) { return; }

            if (GstrDRUGOK == "1")
            {
                ComFunc.MsgBox("이미 약제팀으로 제출된 신청서입니다.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (SAVE_OK() != "OK")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     ROWID ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + GstrROWID + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND FAST = '2'";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("신청기간 이외엔 '긴급구입 희망' 약제만 신청이 가능합니다.");
                        Cursor.Current = Cursors.Default;
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (GstrROWID == "")
                {
                    ComFunc.MsgBox("제출할 신청서가 선택되지 않았거나 저장되지 않은 신청서입니다."
                        + ComNum.VBLF + "확인하시기 바랍니다.");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (ComFunc.MsgBoxQ("해당 신청서를 약제팀으로 제출하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT";
                SQL = SQL + ComNum.VBLF + "     SET ";
                SQL = SQL + ComNum.VBLF + "         DRUGOK = '1', ";
                SQL = SQL + ComNum.VBLF + "         DRUGSEND = TRUNC(SYSDATE), ";
                SQL = SQL + ComNum.VBLF + "         SINDATE = TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + GstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
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
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private bool CHECK_DATA_SEND()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = true;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_ERP + "DRUG_DC_SUBSCRIPT ";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + GstrROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("전송할 신청서를 선택하여 주십시요.");

                    dt.Dispose();
                    dt = null;

                    rtnVal = false;
                    return rtnVal;
                }

                if (dt.Rows[0]["NameK"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("한글 약품명이 공란입니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["JEHYENG"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("제형이 공란입니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["Sungbun"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("성분 및 규격이 공란입니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["Jeyak"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("제약회사가 공란입니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["Used"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("사용방법이 공란입니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["SomoGbn"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("소모예정란이 공란입니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["Somo_Month"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("월간 소모예정란이 공란입니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["SinCheng"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("신청사유가 공란입니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["DEPTCODE"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("신청과가 공란입니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["SinName"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("신청인이 공란입니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["Same"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("원내사용 동효,유사약품 여부가 공란입니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["Same"].ToString().Trim() == "1" && dt.Rows[0]["SameName"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("원내사용 동효, 유사약품 여부가 '있음'일 경우 해당 약제코드를 입력하시기 바랍니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["Diff"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("대체약품이 공란입니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["Diff"].ToString().Trim() == "1" && dt.Rows[0]["DiffName"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("대체약품이 '있음'일 경우 해당 약제코드를 입력하시기 바랍니다.");
                    rtnVal = false;
                }

                if (dt.Rows[0]["Fast"].ToString().Trim() == "2" && dt.Rows[0]["FastSayu"].ToString().Trim() == "")
                {
                    ComFunc.MsgBox("'긴급구입 희망'일 경우 긴급성 사유를 입력하시기 바랍니다.");
                    rtnVal = false;
                }

                if (SAVE_OK() != "OK" && dt.Rows[0]["Fast"].ToString().Trim() != "2")
                {
                    ComFunc.MsgBox("신청기간 이외에 신규약품을 신청은 긴급성이 '긴급 구입 희망'일 때만 가능합니다.");
                    rtnVal = false;
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            
            if (SAVE_DATA(GstrROWID) == true)
            {
                GetData();
            }
            
        }
    }
}
