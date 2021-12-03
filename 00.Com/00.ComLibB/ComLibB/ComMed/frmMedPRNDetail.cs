using ComBase; //기본 클래스
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComMedLibB
    /// File Name       : frmMedPRNDetail.cs
    /// Description     : 필요시 처방(PRN) 처방 상세내용 보기
    /// Author          : 박창욱
    /// Create Date     : 2017-11-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// TODO : 전역변수 GstrPRN_ROWID의 구조가 복잡하여 실제 데이터를 받아와서 테스트 필요
    /// </history>
    /// <seealso cref= "\Ocs\FrmPRN상세정보.frm(FrmPRN상세정보.frm) >> frmMedPRNDetail.cs 폼이름 재정의" />	
    public partial class frmMedPRNDetail : Form
    {
        string FstrDrugChk = "";
        string FstrDoStandardSub = "";
        string GstrPRN_ROWID = "";

        public frmMedPRNDetail()
        {
            InitializeComponent();
        }

        public frmMedPRNDetail(string strPRN_ROWID)
        {
            InitializeComponent();

            GstrPRN_ROWID = strPRN_ROWID;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMedPRNDetail_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strSuCode = "";
            string strSuName = "";

            Screen_Clear();

            FstrDoStandardSub = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (VB.Pstr(GstrPRN_ROWID, "^^", 1).Trim() == "XXXXX")
                {
                    strSuCode = VB.Pstr(GstrPRN_ROWID, "^^", 2).Trim();

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT b.MaxQty_Gubun1,c.HName,b.Insulin_Scale  ";
                    SQL = SQL + ComNum.VBLF + "  FROM  " + ComNum.DB_ERP + "DRUG_MASTER4 b, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW c";
                    SQL = SQL + ComNum.VBLF + " WHERE b.JepCode=c.SuNext(+)";
                    SQL = SQL + ComNum.VBLF + "   AND b.JepCode ='" + strSuCode + "' ";

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
                        strSuName = dt.Rows[0]["HName"].ToString().Trim();

                        txtS1.Text = dt.Rows[0]["MaxQty_Gubun1"].ToString().Trim(); //적응증
                        txtS2.Text = VB.Pstr(GstrPRN_ROWID, "^^", 3).Trim();    //실시기준(공통)
                        txtV1.Text = VB.Pstr(GstrPRN_ROWID, "^^", 4).Trim();    //일투량
                        txtV5.Text = VB.Pstr(GstrPRN_ROWID, "^^", 5).Trim();    //용법
                        txtV2.Text = VB.Pstr(GstrPRN_ROWID, "^^", 6).Trim();    //투여간격
                        txtV3.Text = VB.Pstr(GstrPRN_ROWID, "^^", 7).Trim();    //투여횟수
                        txtV4.Text = VB.Pstr(GstrPRN_ROWID, "^^", 8).Trim();    //Notify

                        FstrDoStandardSub = dt.Rows[0]["Insulin_Scale"].ToString().Trim();  //sub
                        FstrDrugChk = Read_Drug_SuCode(strSuCode);

                        lblInfo.Text = "[약품명 : " + strSuName + " ] [ 의약품코드 : " + strSuCode + " ]";

                        if (FstrDrugChk == "OK")
                        {
                            lblInfo2.Text = "마약류";
                        }
                        else
                        {
                            lblInfo2.Text = "일반약";
                        }

                        if (FstrDoStandardSub == "S")
                        {
                            lbl5.Enabled = false;
                            txtS1.Enabled = false;
                            //lbl7.Text = "적응증 및 실시기준";
                        }
                    }
                    dt.Dispose();
                    dt = null;

                    GstrPRN_ROWID = "";
                }
                else
                {
                    //오더 읽기
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT a.OrderCode,a.SuCode,b.MaxQty_Gubun1,c.HName,b.Insulin_Scale, ";
                    SQL = SQL + ComNum.VBLF + "       a.PRN_REMARK, a.VERB_PRT_DATE, a.PRN_INS_GBN, a.PRN_INS_UNIT,";
                    SQL = SQL + ComNum.VBLF + "       a.PRN_UNIT, a.PRN_INS_SDATE, a.PRN_INS_EDATE, a.PRN_INS_MAX,";
                    SQL = SQL + ComNum.VBLF + "       a.PRN_DOSCODE, a.PRN_TERM, a.PRN_NOTIFY ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_IORDER a, " + ComNum.DB_ERP + "DRUG_MASTER4 b, ";
                    SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_MED + "OCS_DRUGINFO_NEW c";
                    SQL = SQL + ComNum.VBLF + " WHERE a.SuCode=b.JepCode(+)";
                    SQL = SQL + ComNum.VBLF + "   AND a.SuCode=c.SuNext(+)";
                    SQL = SQL + ComNum.VBLF + "   AND a.ROWID ='" + GstrPRN_ROWID + "' ";
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
                        strSuCode = dt.Rows[0]["SuCode"].ToString().Trim();
                        strSuName = dt.Rows[0]["HName"].ToString().Trim();

                        txtS1.Text = dt.Rows[0]["MaxQty_Gubun1"].ToString().Trim();     //적응증
                        txtS2.Text = dt.Rows[0]["PRN_REMARK"].ToString().Trim();        //실시기준(공통)

                        //2015-11-25
                        if (dt.Rows[0]["PRN_UNIT"].ToString().Trim() != "")
                        {
                            txtV1.Text = dt.Rows[0]["PRN_UNIT"].ToString().Trim();  //일투량
                        }
                        else
                        {
                            txtV1.Text = dt.Rows[0]["PRN_INS_UNIT"].ToString().Trim();  //일투량
                        }

                        txtV5.Text = dt.Rows[0]["PRN_DOSCODE"].ToString().Trim();       //용법
                        txtV2.Text = dt.Rows[0]["PRN_TERM"].ToString().Trim();      //투여간격
                        txtV3.Text = dt.Rows[0]["PRN_INS_MAX"].ToString().Trim();   //투여횟수
                        txtV4.Text = dt.Rows[0]["PRN_NOTIFY"].ToString().Trim();    //Notify

                        FstrDoStandardSub = dt.Rows[0]["Insulin_Scale"].ToString().Trim();  //Sub
                        FstrDrugChk = Read_Drug_SuCode(strSuCode);

                        lblInfo.Text = "[약품명 : " + strSuName + " ] [ 의약품코드 : " + strSuCode + " ]";

                        if (FstrDrugChk == "OK")
                        {
                            lblInfo2.Text = "마약류";
                        }
                        else
                        {
                            lblInfo2.Text = "일반약";
                        }

                        if (FstrDoStandardSub == "S")
                        {
                            lbl5.Enabled = false;
                            txtS1.Enabled = false;
                            //lbl7.Text = "적응증 및 실시기준";
                        }

                    }

                    dt.Dispose();
                    dt = null;
                }

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Screen_Clear()
        {
            lblInfo.Text = "";
            lblInfo2.Text = "";

            txtS1.Text = "";
            txtS2.Text = "";

            txtV1.Text = "";
            txtV2.Text = "";
            txtV3.Text = "";
            txtV4.Text = "";
            txtV5.Text = "";

            lbl5.Enabled = true;
            txtS1.Enabled = true;
            lbl7.Text = "실시기준";
        }

        //마약류 수가 체크
        string Read_Drug_SuCode(string argSuCode)
        {
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  a.ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW A, " + ComNum.DB_ERP + "DRUG_MASTER2 B, " + ComNum.DB_ERP + "DRUG_JEP C";
                SQL = SQL + ComNum.VBLF + " WHERE A.SUNEXT = B.JEPCODE(+)";
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = C.JEPCODE";
                SQL = SQL + ComNum.VBLF + "   AND a.SuNext ='" + argSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND C.CHENGGU IN ('09','08')";
                SQL = SQL + ComNum.VBLF + "   AND EXISTS (";
                SQL = SQL + ComNum.VBLF + "       SELECT * FROM " + ComNum.DB_PMPA + "BAS_SUT S";
                SQL = SQL + ComNum.VBLF + "        WHERE DELDATE Is Null";
                SQL = SQL + ComNum.VBLF + "          AND (S.SUGBJ IN ('2','3','4') OR (S.BUN = '23' AND S.SUGBJ = '0'))";
                SQL = SQL + ComNum.VBLF + "          AND A.SUNEXT = S.SUNEXT)";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }
    }
}
