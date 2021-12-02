using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmSafetyReport.cs
    /// Description     : 안전사고 보고서
    /// Author          : 이현종
    /// Create Date     : 2018-08-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\etc\kms\Frm안전사고보고서(Frm안전사고보고서.frm) >> frmSafetyReport.cs 폼이름 재정의" />
    public partial class frmSafetyReport : Form
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

        /// <summary>
        /// EMRVIEWER
        /// </summary>
        frmEmrViewer frmEmrViewer = null;

        public frmSafetyReport(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmSafetyReport(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }

        string strDepartment = "";
        string strRowid = "";

        public frmSafetyReport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// QI전용
        /// </summary>
        /// <param name="strDepartment">적정관리실</param>
        /// <param name="strRowid">ROWID</param>
        public frmSafetyReport(string strDepartment, string strRowid)
        {
            InitializeComponent();
            this.strDepartment = strDepartment;
            this.strRowid = strRowid;
        }

        private void frmSafetyReport_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssList_Sheet1.RowCount = 0;

            if (strDepartment == "적정관리실")
            {
                //label1.Visible = false;
                //label2.Visible = false;
                READ_DATA(strRowid);
                return;
            }
            else
            {
                btnSearch.PerformClick();
                btnNew.PerformClick();
                //GetSearchData();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ssList_Sheet1.RowCount = 0;

            SCREEN_CLEAR();

            if (strDepartment == "적정관리실")
            {
                READ_DATA(strRowid);
                return;
            }

            strRowid = "";
            GetSearchData();
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT TO_CHAR(RDATE, 'YYYY-MM-DD HH24:MI') RDATE, ROWID ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_SAFETY_REPORT1 ";
                SQL += ComNum.VBLF + " WHERE WSABUN = " + clsType.User.Sabun;
                SQL += ComNum.VBLF + " ORDER BY WDATE DESC";

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
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssList_Sheet1.RowCount = dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i <dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Rowid"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void READ_DATA(string strRowid)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SCREEN_CLEAR();

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT WDate, WSABUN, WBUSE, TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') RDATE, ";
                SQL += ComNum.VBLF + " TARGET1, TARGET2, TARGET3, TARGET4,";
                SQL += ComNum.VBLF + " TARGET5, TARGET6, TARGET7, TARGET8, ";
                SQL += ComNum.VBLF + " PNAME, PtNo, DeptCode, WardCode,";
                SQL += ComNum.VBLF + " RoomCode, TO_CHAR(InDate,'YYYY-MM-DD') INDATE, DIAGNOSIS, TO_CHAR(BALDATE,'YYYY-MM-DD HH24:MI') BALDATE,";
                SQL += ComNum.VBLF + " TO_CHAR(CONDATE,'YYYY-MM-DD HH24:MI') CONDATE,BALPLACE,JIKWON,ACCI01,";
                SQL += ComNum.VBLF + " ACCI02,ACCI03,ACCI04,ACCI05,";
                SQL += ComNum.VBLF + " ACCI06,ACCI07,ACCI08,ACCI09,";
                SQL += ComNum.VBLF + " ACCI10,ACCI11,ACCI12,ACCI13,";
                SQL += ComNum.VBLF + " ACCI14,ACCI15,ACCI16,ACCI17,";
                SQL += ComNum.VBLF + " ACCI18,ACCI19,ACCI20,";
                SQL += ComNum.VBLF + " ERRGUBUN, ERRGRADE, ";
                //'    SQL = SQL & vbCr & " ERRGUBUN1,ERRGUBUN2,ERRGUBUN3,ERRGRADE1,"
                //'    SQL = SQL & vbCr & " ERRGRADE2,ERRGRADE3,ERRGRADE4,ERRGRADE5,"
                //'    SQL = SQL & vbCr & " ERRGRADE6,ERRGRADE7,ERRGRADE8,ERRGRADE9,"
                SQL += ComNum.VBLF + " REPORT1,REPORT2,REPORT3,REPORT4,";
                SQL += ComNum.VBLF + " REULST1_1,REULST1_2,RESULT2_1,RESULT2_2,";
                SQL += ComNum.VBLF + " RESULT2_3,RESULT2_4,RESULT2_5,RESULT2_6,";
                SQL += ComNum.VBLF + " RESULT3_1,RESULT3_2,RESULT3_3,RESULT3_4,";
                SQL += ComNum.VBLF + " RESULT3_5,CONFIRM1DATE,CONFIRM1SABUN,CONFIRM2DATE,";
                SQL += ComNum.VBLF + " CONFIRM2SABUN";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_SAFETY_REPORT1 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

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
                    Cursor.Current = Cursors.Default;
                    return;
                }

                string strWDATE = dt.Rows[0]["WDate"].ToString().Trim();
                string strWSABUN = dt.Rows[0]["WSABUN"].ToString().Trim();
                string strWBUSE = dt.Rows[0]["WBUSE"].ToString().Trim();
                string StrRDate = dt.Rows[0]["RDate"].ToString().Trim();
                string strTARGET1 = dt.Rows[0]["TARGET1"].ToString().Trim();
                string strTARGET2 = dt.Rows[0]["TARGET2"].ToString().Trim();
                string strTARGET3 = dt.Rows[0]["TARGET3"].ToString().Trim();
                string strTARGET4 = dt.Rows[0]["TARGET4"].ToString().Trim();
                string strTARGET5 = dt.Rows[0]["TARGET5"].ToString().Trim();
                string strTARGET6 = dt.Rows[0]["TARGET6"].ToString().Trim();
                string strTARGET7 = dt.Rows[0]["TARGET7"].ToString().Trim();
                string strTARGET8 = dt.Rows[0]["TARGET8"].ToString().Trim();
                string strPname = dt.Rows[0]["PNAME"].ToString().Trim();
                string strPtNo = dt.Rows[0]["PtNo"].ToString().Trim();
                string strDeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                string strWardCode = dt.Rows[0]["WardCode"].ToString().Trim();
                string strRoomCode = dt.Rows[0]["RoomCode"].ToString().Trim();
                string strInDate = dt.Rows[0]["InDate"].ToString().Trim();
                string strDIAGNOSIS = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
                string strBalDate = dt.Rows[0]["BALDATE"].ToString().Trim();
                string strCONDATE = dt.Rows[0]["CONDATE"].ToString().Trim();
                string strBALPLACE = dt.Rows[0]["BALPLACE"].ToString().Trim();
                string strJIKWON = dt.Rows[0]["JIKWON"].ToString().Trim();
                string strACCI01 = dt.Rows[0]["ACCI01"].ToString().Trim();
                string strACCI02 = dt.Rows[0]["ACCI02"].ToString().Trim();
                string strACCI03 = dt.Rows[0]["ACCI03"].ToString().Trim();
                string strACCI04 = dt.Rows[0]["ACCI04"].ToString().Trim();
                string strACCI05 = dt.Rows[0]["ACCI05"].ToString().Trim();
                string strACCI06 = dt.Rows[0]["ACCI06"].ToString().Trim();
                string strACCI07 = dt.Rows[0]["ACCI07"].ToString().Trim();
                string strACCI08 = dt.Rows[0]["ACCI08"].ToString().Trim();
                string strACCI09 = dt.Rows[0]["ACCI09"].ToString().Trim();
                string strACCI10 = dt.Rows[0]["ACCI10"].ToString().Trim();
                string strACCI11 = dt.Rows[0]["ACCI11"].ToString().Trim();
                string strACCI12 = dt.Rows[0]["ACCI12"].ToString().Trim();
                string strACCI13 = dt.Rows[0]["ACCI13"].ToString().Trim();
                string strACCI14 = dt.Rows[0]["ACCI14"].ToString().Trim();
                string strACCI15 = dt.Rows[0]["ACCI15"].ToString().Trim();
                string strACCI16 = dt.Rows[0]["ACCI16"].ToString().Trim();
                string strACCI17 = dt.Rows[0]["ACCI17"].ToString().Trim();
                string strACCI18 = dt.Rows[0]["ACCI18"].ToString().Trim();
                string strACCI19 = dt.Rows[0]["ACCI19"].ToString().Trim();
                string strACCI20 = dt.Rows[0]["ACCI20"].ToString().Trim();
                string strERRGUBUN = dt.Rows[0]["ERRGUBUN"].ToString().Trim();
                string strERRGRADE = dt.Rows[0]["ERRGRADE"].ToString().Trim();
                //'        strERRGUBUN1 = dt.Rows[0]["ERRGUBUN1"].ToString().Trim();
                //'        strERRGUBUN2 = dt.Rows[0]["ERRGUBUN2"].ToString().Trim();
                //'        strERRGUBUN3 = dt.Rows[0]["ERRGUBUN3"].ToString().Trim();
                //'        strERRGRADE1 = dt.Rows[0]["ERRGRADE1"].ToString().Trim();
                //'        strERRGRADE2 = dt.Rows[0]["ERRGRADE2"].ToString().Trim();
                //'        strERRGRADE3 = dt.Rows[0]["ERRGRADE3"].ToString().Trim();
                //'        strERRGRADE4 = dt.Rows[0]["ERRGRADE4"].ToString().Trim();
                //'        strERRGRADE5 = dt.Rows[0]["ERRGRADE5"].ToString().Trim();
                //'        strERRGRADE6 = dt.Rows[0]["ERRGRADE6"].ToString().Trim();
                //'        strERRGRADE7 = dt.Rows[0]["ERRGRADE7"].ToString().Trim();
                //'        strERRGRADE8 = dt.Rows[0]["ERRGRADE8"].ToString().Trim();
                //'        strERRGRADE9 = dt.Rows[0]["ERRGRADE9"].ToString().Trim();
                string strREPORT1 = dt.Rows[0]["REPORT1"].ToString().Trim();
                string strREPORT2 = dt.Rows[0]["REPORT2"].ToString().Trim();
                string strREPORT3 = dt.Rows[0]["REPORT3"].ToString().Trim();
                string strREPORT4 = dt.Rows[0]["REPORT4"].ToString().Trim();
                string strRESULT1_1 = dt.Rows[0]["REULST1_1"].ToString().Trim();
                string strRESULT1_2 = dt.Rows[0]["REULST1_2"].ToString().Trim();
                string strRESULT2_1 = dt.Rows[0]["RESULT2_1"].ToString().Trim();
                string strRESULT2_2 = dt.Rows[0]["RESULT2_2"].ToString().Trim();
                string strRESULT2_3 = dt.Rows[0]["RESULT2_3"].ToString().Trim();
                string strRESULT2_4 = dt.Rows[0]["RESULT2_4"].ToString().Trim();
                string strRESULT2_5 = dt.Rows[0]["RESULT2_5"].ToString().Trim();
                string strRESULT2_6 = dt.Rows[0]["RESULT2_6"].ToString().Trim();
                string strRESULT3_1 = dt.Rows[0]["RESULT3_1"].ToString().Trim();
                string strRESULT3_2 = dt.Rows[0]["RESULT3_2"].ToString().Trim();
                string strRESULT3_3 = dt.Rows[0]["RESULT3_3"].ToString().Trim();
                string strRESULT3_4 = dt.Rows[0]["RESULT3_4"].ToString().Trim();
                string strRESULT3_5 = dt.Rows[0]["RESULT3_5"].ToString().Trim();
                string strCONFIRM1DATE = dt.Rows[0]["CONFIRM1DATE"].ToString().Trim();
                string strCONFIRM1SABUN = dt.Rows[0]["CONFIRM1SABUN"].ToString().Trim();
                string strCONFIRM2DATE = dt.Rows[0]["CONFIRM2DATE"].ToString().Trim();
                string strCONFIRM2SABUN = dt.Rows[0]["CONFIRM2SABUN"].ToString().Trim();

                dt.Dispose();
                dt = null;

                ss1_Sheet1.Cells[4, 2].Text = clsVbfunc.GetInSaName(clsDB.DbCon, strWSABUN);  //보고자
                ss1_Sheet1.Cells[4, 7].Text = strWSABUN;  //사번
                ss1_Sheet1.Cells[4, 13].Text = strWBUSE; //근무부서
                ss1_Sheet1.Cells[4, 20].Text = StrRDate; //보고일시

                ss1_Sheet1.Cells[7, 1].Value  = strTARGET1 == "1";    //환자
                ss1_Sheet1.Cells[7, 3].Value  = strTARGET2 == "1";    //입원
                ss1_Sheet1.Cells[7, 5].Value  = strTARGET3 == "1";    //외래
                ss1_Sheet1.Cells[7, 7].Value  = strTARGET4 == "1";    //응급센터
                ss1_Sheet1.Cells[7, 12].Value = strTARGET5 == "1";   //보호자
                ss1_Sheet1.Cells[7, 15].Value = strTARGET6 == "1";   //방문객
                ss1_Sheet1.Cells[7, 18].Value = strTARGET7 == "1";   //기타
                ss1_Sheet1.Cells[7, 20].Text = strTARGET8;   //기타(입력)

                ss1_Sheet1.Cells[8, 1].Text  = strPname;        //환자이름
                ss1_Sheet1.Cells[8, 5].Text  = strPtNo;        //등록번호
                ss1_Sheet1.Cells[8, 9].Text  = strDeptCode;        //진료과
                ss1_Sheet1.Cells[8, 13].Text = strRoomCode;       //병동/병실
                ss1_Sheet1.Cells[8, 17].Text = strInDate;       //입원실
                ss1_Sheet1.Cells[8, 21].Text = strDIAGNOSIS;       //진단명

                ss1_Sheet1.Cells[9, 1].Text  = strBalDate;        //발생일시
                ss1_Sheet1.Cells[9, 8].Text  = strCONDATE;        //확인일시
                ss1_Sheet1.Cells[9, 15].Text = strBALPLACE;       //발생장소
                ss1_Sheet1.Cells[9, 21].Text = strJIKWON;       //관련직원


                ss1_Sheet1.Cells[12, 1].Value  = strACCI01 == "1";   //수술
                ss1_Sheet1.Cells[12, 4].Value  = strACCI02 == "1";   //마취진정
                ss1_Sheet1.Cells[12, 8].Value  = strACCI03 == "1";   //수혈
                ss1_Sheet1.Cells[12, 11].Value = strACCI04 == "1";  //검사시술치료
                ss1_Sheet1.Cells[12, 16].Value = strACCI05 == "1";  //진단처치검사시술
                ss1_Sheet1.Cells[12, 19].Value = strACCI07 == "1";  //지연된치료

                ss1_Sheet1.Cells[13, 1].Value  = strACCI08 == "1";   //감염 
                ss1_Sheet1.Cells[13, 4].Value  = strACCI10 == "1";   //분만
                ss1_Sheet1.Cells[13, 8].Value  = strACCI11 == "1";   //화상 
                ss1_Sheet1.Cells[13, 11].Value = strACCI14 == "1";  //의료기구
                ss1_Sheet1.Cells[13, 16].Value = strACCI18 == "1";  //자살자해
                ss1_Sheet1.Cells[13, 19].Value = strACCI19 == "1";  //탈원


                ss1_Sheet1.Cells[14, 1].Value = strACCI13 == "1";   //식사
                ss1_Sheet1.Cells[14, 4].Value = strACCI20 == "1";   //시설환경
                ss1_Sheet1.Cells[14, 8].Value = strACCI16 == "1";   //기타
                ss1_Sheet1.Cells[14, 10].Text = strACCI17;      //기타

                ss1_Sheet1.Cells[15, 1].Text = strERRGUBUN;
                ss1_Sheet1.Cells[15, 4].Text = strERRGRADE;

                int height = 0;
                ss1_Sheet1.Cells[18, 3].Text = strREPORT1;
                height = Convert.ToInt32(ss1_Sheet1.GetPreferredRowHeight(18));
                if (height > 75)
                {
                    ss1_Sheet1.SetRowHeight(18, height + 3);
                }

                ss1_Sheet1.Cells[19, 3].Text = strREPORT2;
                height = Convert.ToInt32(ss1_Sheet1.GetPreferredRowHeight(19));
                if (height > 75)
                {
                    ss1_Sheet1.SetRowHeight(19, height + 3);
                }

               
                ss1_Sheet1.Cells[20, 3].Text = strREPORT3;
                height = Convert.ToInt32(ss1_Sheet1.GetPreferredRowHeight(20));
                if (height > 75)
                {
                    ss1_Sheet1.SetRowHeight(20, height + 3);
                }

                ss1_Sheet1.Cells[21, 3].Text = strREPORT4;
                height = Convert.ToInt32(ss1_Sheet1.GetPreferredRowHeight(21));
                if (height > 75)
                {
                    ss1_Sheet1.SetRowHeight(21, height + 3);
                }


                ss1_Sheet1.Cells[23, 3].Text = strRESULT1_1;
                ss1_Sheet1.Cells[23, 5].Text = strRESULT1_2;

                ss1_Sheet1.Cells[24, 3].Text  = strRESULT2_1;
                ss1_Sheet1.Cells[24, 8].Text  = strRESULT2_2;
                ss1_Sheet1.Cells[24, 13].Text = strRESULT2_3;
                ss1_Sheet1.Cells[24, 19].Text = strRESULT2_4;

                ss1_Sheet1.Cells[25, 3].Text = strRESULT2_5;
                ss1_Sheet1.Cells[25, 8].Text = strRESULT2_6;

                ss1_Sheet1.Cells[26, 3].Text  = strRESULT3_1;
                ss1_Sheet1.Cells[26, 7].Text  = strRESULT3_2;
                ss1_Sheet1.Cells[26, 11].Text = strRESULT3_3;
                ss1_Sheet1.Cells[26, 16].Text = strRESULT3_4;
                ss1_Sheet1.Cells[26, 19].Text = strRESULT3_5;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void SCREEN_CLEAR()
        {
            ss1_Sheet1.Cells[4, 2].Text = "";  //보고자
            ss1_Sheet1.Cells[4, 7].Text = "";  //사번
            ss1_Sheet1.Cells[4, 13].Text = ""; //근무부서
            ss1_Sheet1.Cells[4, 20].Text = ""; //보고일시

            ss1_Sheet1.Cells[7, 1].Value = false;    //환자
            ss1_Sheet1.Cells[7, 3].Value = false;    //입원
            ss1_Sheet1.Cells[7, 5].Value = false;    //외래
            ss1_Sheet1.Cells[7, 7].Value = false;    //응급센터
            ss1_Sheet1.Cells[7, 12].Value = false;   //보호자
            ss1_Sheet1.Cells[7, 15].Value = false;   //방문객
            ss1_Sheet1.Cells[7, 18].Value = false;   //기타
            ss1_Sheet1.Cells[7, 20].Text = "";   //기타(입력)

            ss1_Sheet1.Cells[8, 1].Text = "";        //환자이름
            ss1_Sheet1.Cells[8, 5].Text = "";        //등록번호
            ss1_Sheet1.Cells[8, 9].Text = "";        //진료과
            ss1_Sheet1.Cells[8, 13].Text = "";       //병동/병실
            ss1_Sheet1.Cells[8, 17].Text = "";       //입원실
            ss1_Sheet1.Cells[8, 21].Text = "";       //진단명

            ss1_Sheet1.Cells[9, 1].Text = "";        //발생일시
            ss1_Sheet1.Cells[9, 8].Text = "";        //확인일시
            ss1_Sheet1.Cells[9, 15].Text = "";       //발생장소
            ss1_Sheet1.Cells[9, 21].Text = "";       //관련직원


            ss1_Sheet1.Cells[12, 1].Value = false;   //수술
            ss1_Sheet1.Cells[12, 4].Value = false;   //마취진정
            ss1_Sheet1.Cells[12, 8].Value = false;   //수혈
            ss1_Sheet1.Cells[12, 11].Value = false;  //검사시술치료
            ss1_Sheet1.Cells[12, 16].Value = false;  //진단처치검사시술
            ss1_Sheet1.Cells[12, 19].Value = false;  //지연된치료

            ss1_Sheet1.Cells[13, 1].Value = false;   //감염 
            ss1_Sheet1.Cells[13, 4].Value = false;   //분만
            ss1_Sheet1.Cells[13, 8].Value = false;   //화상 
            ss1_Sheet1.Cells[13, 11].Value = false;  //의료기구
            ss1_Sheet1.Cells[13, 16].Value = false;  //자살자해
            ss1_Sheet1.Cells[13, 19].Value = false;  //탈원


            ss1_Sheet1.Cells[14, 1].Value = false;   //식사
            ss1_Sheet1.Cells[14, 4].Value = false;   //시설환경
            ss1_Sheet1.Cells[14, 8].Value = false;   //기타
            ss1_Sheet1.Cells[14, 10].Text = "";      //기타


            ss1_Sheet1.Cells[15, 1].Text = "";
            ss1_Sheet1.Cells[15, 4].Text = "";

            ss1_Sheet1.Cells[18, 3].Text = "";
            ss1_Sheet1.Cells[19, 3].Text = "";
            ss1_Sheet1.Cells[20, 3].Text = "";
            ss1_Sheet1.Cells[21, 3].Text = "";

            ss1_Sheet1.Cells[23, 3].Text = "";
            ss1_Sheet1.Cells[23, 5].Text = "";

            ss1_Sheet1.Cells[24, 3].Text = "";
            ss1_Sheet1.Cells[24, 8].Text = "";
            ss1_Sheet1.Cells[24, 13].Text = "";
            ss1_Sheet1.Cells[24, 19].Text = "";

            ss1_Sheet1.Cells[25, 3].Text = "";
            ss1_Sheet1.Cells[25, 8].Text = "";

            ss1_Sheet1.Cells[26, 3].Text = "";
            ss1_Sheet1.Cells[26, 7].Text = "";
            ss1_Sheet1.Cells[26, 11].Text = "";
            ss1_Sheet1.Cells[26, 16].Text = "";
            ss1_Sheet1.Cells[26, 19].Text = "";

            ss1_Sheet1.Cells[28, 6].Text = "";
            ss1_Sheet1.Cells[28, 18].Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (Save_Data() == false) return;

            if(strDepartment == "적정관리실")
            {
                Close();
            }
            else
            {
                //GetSearchData();
                btnSearch.PerformClick();
            }
        }

        bool Save_Data()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strWSABUN     = ss1_Sheet1.Cells[4, 7].Text.Trim();
            string strWBUSE = ss1_Sheet1.Cells[4, 13].Text.Trim();
            string StrRDate      = ss1_Sheet1.Cells[4, 20].Text.Trim();

            string strTARGET1    = ss1_Sheet1.Cells[7, 1].Text.Trim() == "True" ? "1" : "0";
            string strTARGET2    = ss1_Sheet1.Cells[7, 3].Text.Trim() == "True" ? "1" : "0";
            string strTARGET3    = ss1_Sheet1.Cells[7, 5].Text.Trim() == "True" ? "1" : "0";
            string strTARGET4    = ss1_Sheet1.Cells[7, 7].Text.Trim() == "True" ? "1" : "0";
            string strTARGET5    = ss1_Sheet1.Cells[7, 12].Text.Trim()== "True" ? "1" : "0";
            string strTARGET6    = ss1_Sheet1.Cells[7, 15].Text.Trim()== "True" ? "1" : "0";
            string strTARGET7    = ss1_Sheet1.Cells[7, 18].Text.Trim() == "True" ? "1" : "0";
            string strTARGET8    = ss1_Sheet1.Cells[7, 20].Text.Trim();

            string strPname      = ss1_Sheet1.Cells[8, 1].Text.Trim();
            string strPtNo       = ss1_Sheet1.Cells[8, 5].Text.Trim();
            string strDeptCode   = ss1_Sheet1.Cells[8, 9].Text.Trim();
            string strWardCode   = "";
            string strRoomCode   = ss1_Sheet1.Cells[8, 13].Text.Trim();
            string strInDate     = ss1_Sheet1.Cells[8, 17].Text.Trim();
            string strDIAGNOSIS  = ss1_Sheet1.Cells[8, 21].Text.Trim();

            string strBalDate    = ss1_Sheet1.Cells[9, 1].Text.Trim();
            string strCONDATE    = ss1_Sheet1.Cells[9, 8].Text.Trim();
            string strBALPLACE   = ss1_Sheet1.Cells[9, 15].Text.Trim();
            string strJIKWON     = ss1_Sheet1.Cells[9, 21].Text.Trim();

            string strACCI01     = ss1_Sheet1.Cells[12, 1].Text.Trim()  == "True" ? "1" : "0";
            string strACCI02     = ss1_Sheet1.Cells[12, 4].Text.Trim()  == "True" ? "1" : "0";
            string strACCI03     = ss1_Sheet1.Cells[12, 8].Text.Trim()  == "True" ? "1" : "0";
            string strACCI04     = ss1_Sheet1.Cells[12, 11].Text.Trim() == "True" ? "1" : "0";
            string strACCI05     = ss1_Sheet1.Cells[12, 16].Text.Trim() == "True" ? "1" : "0";
            string strACCI07     = ss1_Sheet1.Cells[12, 19].Text.Trim() == "True" ? "1" : "0";
            string strACCI06     = "";

            string strACCI08     = ss1_Sheet1.Cells[13, 1].Text.Trim()  == "True" ? "1" : "0";
            string strACCI10     = ss1_Sheet1.Cells[13, 4].Text.Trim()  == "True" ? "1" : "0";
            string strACCI11     = ss1_Sheet1.Cells[13, 8].Text.Trim()  == "True" ? "1" : "0";
            string strACCI14     = ss1_Sheet1.Cells[13, 11].Text.Trim() == "True" ? "1" : "0";
            string strACCI18     = ss1_Sheet1.Cells[13, 16].Text.Trim() == "True" ? "1" : "0";
            string strACCI19     = ss1_Sheet1.Cells[13, 19].Text.Trim() == "True" ? "1" : "0";

            string strACCI09 = "";
            string strACCI12 = "";
            string strACCI15 = "";


            string strACCI13 = ss1_Sheet1.Cells[14, 1].Text.Trim()== "True" ? "1" : "0";
            string strACCI20 = ss1_Sheet1.Cells[14, 4].Text.Trim()== "True" ? "1" : "0";
            string strACCI16 = ss1_Sheet1.Cells[14, 8].Text.Trim() == "True" ? "1" : "0";
            string strACCI17 = ss1_Sheet1.Cells[14, 10].Text.Trim();

            string strERRGUBUN    = ss1_Sheet1.Cells[15, 1].Text.Trim();
            string strERRGRADE    = ss1_Sheet1.Cells[15, 4].Text.Trim();

            //string strERRGUBUN1     = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strERRGUBUN2     = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strERRGUBUN3     = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strERRGRADE1     = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strERRGRADE2     = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strERRGRADE3     = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strERRGRADE4     = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strERRGRADE5     = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strERRGRADE6     = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strERRGRADE7     = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strERRGRADE8     = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strERRGRADE9     = ss1_Sheet1.Cells[4, 7].Text.Trim();

            string strREPORT1       = ss1_Sheet1.Cells[18, 3].Text.Trim();
            string strREPORT2       = ss1_Sheet1.Cells[19, 3].Text.Trim();
            string strREPORT3       = ss1_Sheet1.Cells[20, 3].Text.Trim();
            string strREPORT4       = ss1_Sheet1.Cells[21, 3].Text.Trim();

            string strREULST1_1     = ss1_Sheet1.Cells[23, 3].Text.Trim() == "True" ? "1" : "0";
            string strREULST1_2     = ss1_Sheet1.Cells[23, 5].Text.Trim() == "True" ? "1" : "0";

            string strRESULT2_1     = ss1_Sheet1.Cells[24, 3].Text.Trim() == "True" ? "1" : "0";
            string strRESULT2_2     = ss1_Sheet1.Cells[24, 8].Text.Trim() == "True" ? "1" : "0";
            string strRESULT2_3     = ss1_Sheet1.Cells[24, 13].Text.Trim() == "True" ? "1" : "0";
            string strRESULT2_4     = ss1_Sheet1.Cells[24, 19].Text.Trim() == "True" ? "1" : "0";

            string strRESULT2_5     = ss1_Sheet1.Cells[25, 3].Text.Trim() == "True" ? "1" : "0";
            string strRESULT2_6     = ss1_Sheet1.Cells[25, 8].Text.Trim() == "True" ? "1" : "0";

            string strRESULT3_1     = ss1_Sheet1.Cells[26, 3].Text.Trim()  == "True" ? "1" : "0";
            string strRESULT3_2     = ss1_Sheet1.Cells[26, 7].Text.Trim()  == "True" ? "1" : "0";
            string strRESULT3_3     = ss1_Sheet1.Cells[26, 11].Text.Trim() == "True" ? "1" : "0";
            string strRESULT3_4     = ss1_Sheet1.Cells[26, 16].Text.Trim() == "True" ? "1" : "0";
            string strRESULT3_5     = ss1_Sheet1.Cells[26, 19].Text.Trim() == "True" ? "1" : "0";

            //string strCONFIRM1DATE  = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strCONFIRM1SABUN = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strCONFIRM2DATE  = ss1_Sheet1.Cells[4, 7].Text.Trim();
            //string strCONFIRM2SABUN = ss1_Sheet1.Cells[4, 7].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                if(!string.IsNullOrEmpty(strRowid))
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.ETC_SAFETY_REPORT1_HISTORY ";
                    SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.ETC_SAFETY_REPORT1 ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = " DELETE KOSMOS_PMPA.ETC_SAFETY_REPORT1";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

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

                SQL = " INSERT INTO KOSMOS_PMPA.ETC_SAFETY_REPORT1(";
                SQL += "\r" + " WDATE, WSABUN, WBUSE, RDATE, ";
                SQL += "\r" + "  TARGET1, TARGET2, TARGET3, TARGET4, ";
                SQL += "\r" + "  TARGET5, TARGET6, TARGET7, TARGET8, ";
                SQL += "\r" + "  PNAME, PtNo, DeptCode, WardCode, ";
                SQL += "\r" + "  RoomCode, InDate, DIAGNOSIS, BALDATE, ";
                SQL += "\r" + "  CONDATE, BALPLACE, JIKWON, ACCI01,";
                SQL += "\r" + "  ACCI02, ACCI03, ACCI04, ACCI05,";
                SQL += "\r" + "  ACCI06, ACCI07, ACCI08, ACCI09,";
                SQL += "\r" + "  ACCI10, ACCI11, ACCI12, ACCI13,";
                SQL += "\r" + "  ACCI14, ACCI15, ACCI16, ACCI17,";
                SQL += "\r" + "  ACCI18, ACCI19, ACCI20, ";
            //'    SQL = SQL + vbCr + "  ERRGUBUN1, ERRGUBUN2, ERRGUBUN3, ERRGRADE1,"
            //'    SQL = SQL + vbCr + "  ERRGRADE2, ERRGRADE3, ERRGRADE4, ERRGRADE5,"
            //'    SQL = SQL + vbCr + "  ERRGRADE6, ERRGRADE7, ERRGRADE8, ERRGRADE9,"
                SQL += "\r" + "  ERRGUBUN, ERRGRADE,     ";
                SQL += "\r" + "  REPORT1, REPORT2, REPORT3, REPORT4,";
                SQL += "\r" + "  REULST1_1, REULST1_2, RESULT2_1, RESULT2_2,";
                SQL += "\r" + "  RESULT2_3, RESULT2_4, RESULT2_5, RESULT2_6,";
                SQL += "\r" + "  RESULT3_1, RESULT3_2, RESULT3_3, RESULT3_4,";
                SQL += "\r" + "  RESULT3_5) VALUES (";
                SQL += "\r" + " SYSDATE, '" + strWSABUN + "','" + strWBUSE + "',TO_DATE('" + StrRDate + "','YYYY-MM-DD HH24:MI'),";
                SQL += "\r" + "'" + strTARGET1 + "','" + strTARGET2 + "','" + strTARGET3 + "','" + strTARGET4 + "', ";
                SQL += "\r" + "'" + strTARGET5 + "','" + strTARGET6 + "','" + strTARGET7 + "','" + strTARGET8 + "', ";
                SQL += "\r" + "'" + strPname + "','" + strPtNo + "','" + strDeptCode + "','" + strWardCode + "', ";
                SQL += "\r" + "'" + strRoomCode + "',TO_DATE('" + strInDate + "','YYYY-MM-DD'),'" + strDIAGNOSIS + "',TO_DATE('" + strBalDate + "','YYYY-MM-DD HH24:MI'), ";
                SQL += "\r" + "TO_DATE('" + strCONDATE + "','YYYY-MM-DD HH24:MI'),'" + strBALPLACE + "','" + strJIKWON + "','" + strACCI01 + "', ";
                SQL += "\r" + "'" + strACCI02 + "','" + strACCI03 + "','" + strACCI04 + "','" + strACCI05 + "', ";
                SQL += "\r" + "'" + strACCI06 + "','" + strACCI07 + "','" + strACCI08 + "','" + strACCI09 + "', ";
                SQL += "\r" + "'" + strACCI10 + "','" + strACCI11 + "','" + strACCI12 + "','" + strACCI13 + "', ";
                SQL += "\r" + "'" + strACCI14 + "','" + strACCI15 + "','" + strACCI16 + "','" + strACCI17 + "', ";
                SQL += "\r" + "'" + strACCI18 + "','" + strACCI19 + "','" + strACCI20 + "',";
            //'    SQL = SQL + vbCr + "'" + strERRGUBUN1 + "','" + strERRGUBUN2 + "','" + strERRGUBUN3 + "','" + strERRGRADE1 + "', "
            //'    SQL = SQL + vbCr + "'" + strERRGRADE2 + "','" + strERRGRADE3 + "','" + strERRGRADE4 + "','" + strERRGRADE5 + "', "
            //'    SQL = SQL + vbCr + "'" + strERRGRADE6 + "','" + strERRGRADE7 + "','" + strERRGRADE8 + "','" + strERRGRADE9 + "', "
                SQL += "\r" + "'" + strERRGUBUN + "','" + strERRGRADE + "',     ";
                SQL += "\r" + "'" + strREPORT1 + "','" + strREPORT2 + "','" + strREPORT3 + "','" + strREPORT4 + "', ";
                SQL += "\r" + "'" + strREULST1_1 + "','" + strREULST1_2 + "','" + strRESULT2_1 + "','" + strRESULT2_2 + "', ";
                SQL += "\r" + "'" + strRESULT2_3 + "','" + strRESULT2_4 + "','" + strRESULT2_5 + "','" + strRESULT2_6 + "', ";
                SQL += "\r" + "'" + strRESULT3_1 + "','" + strRESULT3_2 + "','" + strRESULT3_3 + "','" + strRESULT3_4 + "', ";
                SQL += "\r" + "'" + strRESULT3_5 + "') ";

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("삭제 후 복구 불가능 합니다. 삭제하시겠습니까?") == DialogResult.No) return;

            if (Delete_Data() == false) return;

            btnSearch.PerformClick();
            //GetSearchData();
        }

        bool Delete_Data()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " INSERT INTO KOSMOS_PMPA.ETC_SAFETY_REPORT1_HISTORY ";
                SQL += ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.ETC_SAFETY_REPORT1 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = " DELETE KOSMOS_PMPA.ETC_SAFETY_REPORT1";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            ss1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ss1.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if(strDepartment == "적정관리실")
            {
                ComFunc.MsgBox("관리프로그램으로 해당 보고서를 열었을 경우 신규 작성이 제한 됩니다.");
                return;
            }

            SCREEN_CLEAR();
            strRowid = "";

            ss1_Sheet1.Cells[4, 2].Text = clsType.User.UserName;
            ss1_Sheet1.Cells[4, 7].Text = clsType.User.Sabun;
            ss1_Sheet1.Cells[4, 13].Text = clsVbfunc.GetBASBuSe(clsDB.DbCon, clsType.User.BuseCode);

            ss1_Sheet1.Cells[4, 20].Text = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("yyyy-MM-dd HH:mm");
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0) return;

            strRowid = ssList_Sheet1.Cells[e.Row, 1].Text;
            READ_DATA(strRowid);
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row == 8 && e.Column == 5)
            {
                TopMost = false;
                //clsVbEmr.EXECUTE_TextEmrViewEx(ss1_Sheet1.Cells[8, 5].Text.Trim(), clsType.User.Sabun, "vbNormalFocus");
                string strPano = ss1_Sheet1.Cells[8, 5].Text.Trim();

                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name.Equals("frmEmrViewer"))
                    {
                        if (frmEmrViewer == null)
                        {
                            break;
                        }
                        else
                        {
                            frmEmrViewer.SetNewPatient(strPano);
                            return;
                        }
                    }
                }

                frmEmrViewer = new frmEmrViewer(strPano);
                frmEmrViewer.StartPosition = FormStartPosition.CenterParent;
                frmEmrViewer.Show(this);
                return;
            }
        }


        private void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row != 15 && e.Column != 1) return;

            string strTemp = ss1_Sheet1.Cells[15, 1].Text.Trim();

            ss1_Sheet1.Cells[15, 4].Text = "";

            string[] strCode = null;

            switch (strTemp)
            {
                case "근접오류":
                    strCode = new string[2];
                    strCode[0] = "등급1. 오류가 발생할 위험이 있는 상황";
                    strCode[1] = "등급2. 오류가 발생하였으나 환자에게 도달하지 않음";
                    break;
                case "위해사건":
                    strCode = new string[5];
                    strCode[0] = "등급3. 환자에게 투여/적용되었으나 해가 없음";
                    strCode[1] = "등급4. 환자에게 투여/적용되었으며 추가적인 관찰이 필요함";
                    strCode[2] = "등급5. 일시적 손상으로 중재가 필요함";
                    strCode[3] = "등급6. 일시적 손상으로 입원기간이 연장됨 ";
                    strCode[4] = "등급7. 생명을 유지하기 위해 필수적인 중재가 필요함";
                    break;
                case "적신호사건":
                    strCode = new string[2];
                    strCode[0] = "등급8. 영구적 손상";
                    strCode[1] = "등급9. 환자사망";
                    break;
                default:
                    strCode = null;
                    break;
            }
            clsSpread.gSpreadComboDataSetEx(ss1, 15, 4, 15, 4, strCode);
        }

        private void frmIndicatorStatistics_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void frmIndicatorStatistics_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmSafetyReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrViewer != null)
            {
                frmEmrViewer.Dispose();
                frmEmrViewer = null;
            }
        }
    }
}
