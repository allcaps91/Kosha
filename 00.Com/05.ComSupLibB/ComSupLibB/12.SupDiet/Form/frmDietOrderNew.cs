
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComDbB;
using FarPoint.Win.Spread;

namespace ComSupLibB
{

    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-04-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\nurse\nrinfo\FrmDietOrderNew1" >> frmDietOrderNew.cs 폼이름 재정의" />


    public partial class frmDietOrderNew : Form, MainFormMessage
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

        #endregion


        string GstrWardCode = "";
        string GstrWardCodes = "";
        string GstrPANO = "";
        string GstrDietPano = "";
        string GstrHelpCode = "";

        int FnRow = 0;
        int FnDietROW = -1;
        int FnDietCol = -1;
        int FnHight = 0;
        int FnMaxHight = 0;
        int fnCnt = 0;

        double FNIPDNO = 0;

        //string FnstrRoom = "";
        string FstrSname = "";  //환자 성명
        string FstrBi = "";
        string FstrDeptCode = "";
        string FstrDietDate = "";
        string FstrDrCode = "";
        string FstrRoomCode = "";  //호실
        string FstrDietDay = "";  //1.아침 2.점심 3.저녁
        string FstrDietPano = "";  //병록번호
        string FstrGbSunap = "";  //퇴원환자 수납 구분
        //string FstrGbAdd = "";  //추가상 확인 구분
        string FstrCureFood = "";

        string strDietOrder = "";
        string strMsg = ""; //식이 마감시간 Check
        string strMsg2 = ""; //치료식이 마감시간 Check

        bool FstrDietJikwon = false;
        bool FbMagam = false;

        string FstrFlag = "";
        string FstrPano = "";

        public frmDietOrderNew()
        {
            InitializeComponent();
        }

        public frmDietOrderNew(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
        }

        public frmDietOrderNew(string strFlag, string strPano)
        {
            InitializeComponent();
            FstrFlag = strFlag;
            FstrPano = strPano;
        }

        public frmDietOrderNew(string strFlag, string strPano, string strWard)
        {
            InitializeComponent();
            FstrFlag = strFlag;
            FstrPano = strPano;
            GstrWardCode = strWard;
        }


        private void frmDietOrderNew_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            if (FstrFlag == "SUB")
            {
                panTitle.Visible = false;
                panTitleSub0.Visible = false;
            }


            SSCode1_Sheet1.Columns[2].Visible = false;
            SSCode1_Sheet1.Columns[3].Visible = false;

            SSCode2_Sheet1.Columns[2].Visible = false;
            SSCode2_Sheet1.Columns[3].Visible = false;

            SSCode3_Sheet1.Columns[2].Visible = false;
            SSCode3_Sheet1.Columns[3].Visible = false;

            SSCode4_Sheet1.Columns[2].Visible = false;
            SSCode4_Sheet1.Columns[3].Visible = false;

            SSBIGO_Sheet1.Columns[2].Visible = false;
            SSBIGO_Sheet1.Columns[3].Visible = false;

            ssMemoList_Sheet1.Columns[2].Visible = false;
            ssMemoList_Sheet1.Columns[3].Visible = false;
            ssMemoList_Sheet1.Columns[4].Visible = false;

            ssMemoList_Sheet1.Columns[2].Visible = false;
            ssMemoList_Sheet1.Columns[3].Visible = false;
            ssMemoList_Sheet1.Columns[4].Visible = false;

            SS1_Sheet1.Columns[6].Visible = false;
            SS1_Sheet1.Columns[7].Visible = false;
            SS1_Sheet1.Columns[10].Visible = false;
            SS1_Sheet1.Columns[11].Visible = false;
            SS1_Sheet1.Columns[12].Visible = false;
            SS1_Sheet1.Columns[13].Visible = false;
            SS1_Sheet1.Columns[14].Visible = false;
            SS1_Sheet1.Columns[15].Visible = false;
            SS1_Sheet1.Columns[16].Visible = false;
            SS1_Sheet1.Columns[17].Visible = false;

            if (GstrWardCode == "")
            {
                GstrWardCode = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");
            }

            if (GstrWardCodes == "")
            {
                GstrWardCodes = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");
            }


            //식이 마감시간
            strMsg = " - 일반식이 마감시각 - ";
            strMsg += ComNum.VBLF + "아침 마감시각 06:30";
            strMsg += ComNum.VBLF + "점심 마감시각 11:30";
            strMsg += ComNum.VBLF + "저녁 마감시각 16:30";

            //치료식이 마감시간
            strMsg2 = " - 치료식이 마감시각 - ";
            strMsg2 += ComNum.VBLF + "아침 마감시각 05:00";
            strMsg2 += ComNum.VBLF + "점심 마감시각 10:00";
            strMsg2 += ComNum.VBLF + "저녁 마감시각 15:00";

            SS1_Sheet1.ColumnHeader.Cells[0, 2].Text = "치료계획";
            TxtDietDate.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            FstrDietJikwon = false;


            SCREEN_CLEAR();

            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            SSOrder.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT SABUN FROM KOSMOS_ADM.INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE BUSE  = '044301' ";
                SQL = SQL + ComNum.VBLF + "   AND SABUN  = '" + clsType.User.Sabun + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                //TODO  clsPublic.GstrWardCodes
                if (dt.Rows.Count > 0 || clsType.User.Sabun == "04349" || clsType.User.Sabun == "04444" || GstrWardCodes == "TOP" || clsType.User.JobGroup == "JOB018003") 
                {
                    TxtDietDate.Enabled = true;
                    SS1_Sheet1.Columns[7].Visible = true;
                    btnSOrder.Visible = true;
                    FstrDietJikwon = true;
                    panDietJikwon0.Visible = true;
                    panDietJikwon01.Visible = true;
                }
                else
                {
                    SS1_Sheet1.Columns[7].Visible = false;
                    btnSOrder.Visible = false;
                    panDietJikwon0.Visible = false;
                    panDietJikwon01.Visible = false;
                }

                if (clsType.User.JobGroup == "JOB018001")       //영일 여사님들 권한. 추가 2019-07-04
                {
                    FstrDietJikwon = true;
                }

                WARD_SET();

                fnCnt = 0;

                lblTime.Text = VB.Left(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"), 16);

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            btnRefreshSecrch();

        }

        private void READ_DIET_ADD()
        {
            string strPano = "";
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT A.PANO , B.SNAME , A.DEPTCODE, A.BI, A.WARDCODE,  A.ROOMCODE, A.DRCODE   ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DIET_NEWORDER A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
                SQL = SQL + ComNum.VBLF + "   WHERE A.ACTDATE =TO_DATE('" + FstrDietDate + "','YYYY-MM-DD')";

                if (GstrWardCode == "A1") //'이침추가분"
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE >= TO_DATE('" + FstrDietDate + " 05:01" + "' ,'YYYY-MM-DD HH24:MI') ";
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE <= TO_DATE('" + FstrDietDate + " 09:00" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.DIETDAY='1'";
                }
                else if (GstrWardCode == "A2")// '점심추가분
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE >= TO_DATE('" + FstrDietDate + " 11:01" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE <= TO_DATE('" + FstrDietDate + " 12:30" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.DIETDAY='2'";
                }
                else if (GstrWardCode == "A3")// '저녁추가분
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE >= TO_DATE('" + FstrDietDate + " 16:01" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE <= TO_DATE('" + FstrDietDate + " 17:30" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.DIETDAY='3'";
                }
                else if (GstrWardCode == "A4")// ' 아침 변경분
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE >= TO_DATE('" + FstrDietDate + " 00:01" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE <= TO_DATE('" + FstrDietDate + " 05:00" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.DIETDAY='1'";
                }
                SQL = SQL + ComNum.VBLF + " AND A.PANO = B.PANO ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.ROOMCODE   ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                SS1_Sheet1.Rows.Count = dt.Rows.Count;
                SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim() + ComNum.VBLF + dt.Rows[i]["PANO"].ToString().Trim();

                    FnHight = 0;
                    FnMaxHight = 2;

                    SS1_Sheet1.Cells[i, 8].Text = "재원중";

                    SS1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["BI"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["DRCODE"].ToString().Trim();

                    READ_DIETORDER(strPano, i);

                    //byte[] a = System.Text.Encoding.Default.GetBytes(SS1_Sheet1.Cells[i, 1].Text);
                    //int intHeight = Convert.ToInt32(a.Length / 8);

                    //SS1_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + (intHeight * 16));

                    SS1_Sheet1.SetRowHeight(i, Convert.ToInt32(SS1_Sheet1.GetPreferredRowHeight(i)));
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void READ_OPD_ER()
        {

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strPano = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //최근 4일내원환자 중 퇴실 시간 아직 입력안된 환자 목록(진료외방문 및 접수후 취소 방문자 제외)
                SQL = " SELECT PANO, SNAME, DEPTCODE, BI, 'ER' WARDCODE, '100' ROOMCODE, DRCODE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.OPD_MASTER ";
                //SQL += ComNum.VBLF + "  WHERE BDATE >= TRUNC(SYSDATE-4) ";
                //SQL += ComNum.VBLF + "    AND BDATE <= TRUNC(SYSDATE) " ;
                SQL += ComNum.VBLF + "  WHERE BDATE >= TO_DATE('2020-03-08','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND BDATE <= TO_DATE('2020-03-12','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DEPTCODE = 'ER' ";
                SQL += ComNum.VBLF + "    AND PANO IN ( ";
                SQL += ComNum.VBLF + "              SELECT PTMIIDNO ";
                SQL += ComNum.VBLF + "                FROM KOSMOS_PMPA.VIEW_ER_EMIHPTMI ";
                //SQL += ComNum.VBLF + "               WHERE PTMIINDT >= TO_CHAR(TRUNC(SYSDATE-4),'YYYYMMDD') ";
                //SQL += ComNum.VBLF + "                 AND PTMIINDT <= TO_CHAR(TRUNC(SYSDATE),'YYYYMMDD') ";
                SQL += ComNum.VBLF + "               WHERE PTMIINDT >= '20200308' ";
                SQL += ComNum.VBLF + "                 AND PTMIINDT <= '20200312' ";
                SQL += ComNum.VBLF + "                 AND PTMIOTDT = ' ' ";
                SQL += ComNum.VBLF + "                 AND PTMIDGKD NOT IN ('3', '4')) ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim() + ComNum.VBLF + dt.Rows[i]["PANO"].ToString().Trim();

                        FnHight = 0;
                        FnMaxHight = 2;

                        SS1_Sheet1.Cells[i, 8].Text = "재원중";

                        SS1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        strPano = dt.Rows[i]["Pano"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["BI"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["DRCODE"].ToString().Trim();

                        READ_DIETORDER(strPano, i);

                        //byte[] a = System.Text.Encoding.Default.GetBytes(SS1_Sheet1.Cells[i, 1].Text);
                        //int intHeight = Convert.ToInt32(a.Length / 8);

                        //SS1_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + (intHeight * 16));

                        SS1_Sheet1.SetRowHeight(i, Convert.ToInt32(SS1_Sheet1.GetPreferredRowHeight(i)));
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_OPD()
        {

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strPano = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT PANO , SNAME , DEPTCODE, BI, 'HD' WARDCODE, '500' ROOMCODE, DRCODE   ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE  ACTDATE = TO_DATE('" + FstrDietDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE='HD' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PANO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim() + ComNum.VBLF + dt.Rows[i]["PANO"].ToString().Trim();

                        FnHight = 0;
                        FnMaxHight = 2;

                        SS1_Sheet1.Cells[i, 8].Text = "재원중";

                        SS1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        strPano = dt.Rows[i]["Pano"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["BI"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["DRCODE"].ToString().Trim();

                        READ_DIETORDER(strPano, i);

                        //byte[] a = System.Text.Encoding.Default.GetBytes(SS1_Sheet1.Cells[i, 1].Text);
                        //int intHeight = Convert.ToInt32(a.Length / 8);

                        //SS1_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + (intHeight * 16));

                        SS1_Sheet1.SetRowHeight(i, Convert.ToInt32(SS1_Sheet1.GetPreferredRowHeight(i)));
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_IPD()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dtTemp = null;
            string SqlErr = "";
            //int nREAD2 = 0;
            double nIPDNO = 0;
            string strAmSet1 = "";
            //string strAmSet3 = "";
            //string strAmSet6 = "";
            string strStat = "";
            //string nLineCnt = "";
            string strPano = "";
            string strOK = "";
            //string strInDate = "";
            //string strBEDNUM = "";
            //string strRoom = "";

            ComFunc CF = new ComFunc();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT PANO,SNAME,DEPTCODE,BI,GBSTS,WARDCODE,ROOMCODE, ";
                SQL = SQL + ComNum.VBLF + " DRCODE,IPDNO, TO_CHAR(INDATE, 'YYYY-MM-DD') AS INDATE, SEX  ";

                if (string.Compare(FstrDietDate, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) < 0)
                {
                    SQL = SQL + ComNum.VBLF + " FROM IPD_BM ";
                    SQL = SQL + ComNum.VBLF + " WHERE JOBDATE = TO_DATE('" + FstrDietDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND GBSTS NOT IN ('9') ";
                    SQL = SQL + ComNum.VBLF + "   AND GBBACKUP IN ('T','J') ";// 'T:퇴원자 J:재원자
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";

                    if (ChkTewon.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE  OUTDATE = TO_DATE('" + FstrDietDate + "','YYYY-MM-DD') ";
                        //'2016-07-08 김경동 수정(의뢰서)
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE  (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND ACTDATE IS NULL) ";
                        SQL = SQL + ComNum.VBLF + " AND GBSTS NOT IN ('6','7','9') ";//       '퇴원계산서인쇄, 퇴원수납완료,취소 제외
                    }

                    if (chkIpwon.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND INDATE >= TO_DATE('" + FstrDietDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + " AND INDATE <= TO_DATE('" + FstrDietDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                    }
                }

                if (GstrWardCode == "IU")
                {
                    if (GstrWardCodes == "SICU" || VB.Left(ComboWard.Text, 2) == "외과")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode = 'IU' AND RoomCode=233 ";
                    }
                    else if (GstrWardCodes == "MICU" || VB.Left(ComboWard.Text, 2) == "내과")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode = 'IU' AND RoomCode=234 ";
                    }
                }
                else if (GstrWardCode == "AL")
                {
                }
                else
                {
                    if (FstrPano != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND PANO = '" + FstrPano + "' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode = '" + GstrWardCode + "' ";
                    }                    
                }

                SQL = SQL + ComNum.VBLF + "     AND PANO <'90000000' ";
                                               
                if (VB.Right((ComboWard.Text).Trim(), 2) == "33" || VB.Right(ComboWard.Text.Trim(), 2) == "35")
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY ROOMCODE ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY ROOMCODE ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                SS1_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strAmSet1 = dt.Rows[i]["GBSTS"].ToString().Trim();
                        //선택식 확인
                        strOK = "";
                        SQL = " SELECT TO_CHAR(MDate,'YYYY-MM-DD') MDate,Pano,Menu1,Menu2,Menu3,Menu4,Menu5,Remark,ROWID  ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DIET_SORDER ";
                        SQL = SQL + ComNum.VBLF + "   WHERE Pano='" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND MDate=TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, TxtDietDate.Text, -1) + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.GetDataTable(ref dtTemp, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dtTemp.Rows.Count > 0)
                        {
                            if (dtTemp.Rows[0]["Menu1"].ToString().Trim() == "Y")
                            {
                                strOK = "OK";
                            }
                            if (dtTemp.Rows[0]["Menu2"].ToString().Trim() == "Y")
                            {
                                strOK = "OK";
                            }
                            if (dtTemp.Rows[0]["Menu3"].ToString().Trim() == "Y")
                            {
                                strOK = "OK";
                            }
                            if (dtTemp.Rows[0]["Menu4"].ToString().Trim() != "")
                            {
                                strOK = "OK";
                            }
                            if (dtTemp.Rows[0]["Menu5"].ToString().Trim() == "Y")
                            {
                                strOK = "OK";
                            }
                        }
                        dtTemp.Dispose();
                        dtTemp = null;

                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();

                        if (strOK == "OK")
                        {
                            SS1_Sheet1.Cells[i, 0].BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                        }
                        if (READ_INFLU_H1N1(dt.Rows[i]["PANO"].ToString().Trim()) == "YES")
                        {
                            SS1_Sheet1.Cells[i, 0].Text = "★" + SS1_Sheet1.Cells[i, 0].Text;
                        }
                        // 2016 - 05 - 03 김경동

                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim() + " " + ComNum.VBLF + dt.Rows[i]["PANO"].ToString().Trim();

                        FnHight = 0;
                        FnMaxHight = 2;

                        if (strAmSet1 == "6")
                        {
                            strStat = "계산중";
                        }
                        else if (strAmSet1 == "7")
                        {
                            strStat = "퇴  원";
                        }
                        else if (strAmSet1 == "1")
                        {
                            strStat = "가퇴원";
                        }
                        else if (strAmSet1 == "3")
                        {
                            strStat = "부분심사";
                        }
                        else if (strAmSet1 == "4")
                        {
                            strStat = "심사완료";
                        }
                        else if (strAmSet1 == "0")
                        {
                            strStat = "재원중";
                        }
                        else if (strAmSet1 == "2")
                        {
                            strStat = "퇴원등록";
                        }

                        SS1_Sheet1.Cells[i, 8].Text = strStat;

                        //COLOR
                        if (strStat == "재원중")
                        {
                            SS1_Sheet1.Rows[i].ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                        }
                        else
                        {
                            SS1_Sheet1.Rows[i].ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
                        }

                        SS1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();

                        SS1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["BI"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["DRCODE"].ToString().Trim();

                        SS1_Sheet1.Cells[i, 14].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                        nIPDNO = VB.Val(SS1_Sheet1.Cells[i, 14].Text);

                        SQL = "";
                        SQL = " SELECT UDATE FROM DIET_S_MANAGER ";
                        SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + nIPDNO;
                        SQL = SQL + ComNum.VBLF + "  ORDER BY UDATE DESC ";

                        SqlErr = clsDB.GetDataTable(ref dtTemp, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dtTemp.Rows.Count > 0)
                        {
                            SS1_Sheet1.Cells[i, 17].Text = dtTemp.Rows[0]["UDATE"].ToString().Trim();
                        }

                        dtTemp.Dispose();
                        dtTemp = null;

                        SS1_Sheet1.Cells[i, 15].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 16].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = "▦";

                        READ_DIETORDER(strPano, i);

                        if (READ_PATIENTMEMO(nIPDNO) == true)
                        {
                            SS1_Sheet1.Cells[i, 1].ForeColor = System.Drawing.Color.FromArgb(0, 0, 255);
                        }
                        else
                        {
                            SS1_Sheet1.Cells[i, 1].ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                        }

                        //byte[] a = System.Text.Encoding.Default.GetBytes(SS1_Sheet1.Cells[i, 1].Text);
                        //int intHeight = Convert.ToInt32(a.Length / 8);

                        //SS1_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + (intHeight * 16));

                        SS1_Sheet1.SetRowHeight(i, Convert.ToInt32(SS1_Sheet1.GetPreferredRowHeight(i)));
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// READ_퇴원자
        /// </summary>
        private void READ_IPD_OUT(string ArgPano)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dtTemp = null;
            string SqlErr = "";
            //int nREAD2 = 0;
            //double nIPDNO = 0;
            string strAmSet1 = "";
            //string strAmSet3 = "";
            //string strAmSet6 = "";
            string strStat = "";
            //string nLineCnt = "";
            string strPano = "";
            string strOK = "";
            //string strInDate = "";
            //string strBEDNUM = "";
            //string strRoom = "";

            ComFunc CF = new ComFunc();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT PANO , SNAME , DEPTCODE, BI,GBSTS,  WARDCODE, ROOMCODE, DRCODE ";

                if (string.Compare(FstrDietDate, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) < 0)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_BM ";
                    SQL = SQL + ComNum.VBLF + " WHERE JOBDATE = TO_DATE('" + FstrDietDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND GBSTS NOT IN ('9') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";

                    if (ChkTewon.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE  ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND OUTDATE IS NULL) OR OUTDATE = TO_DATE('" + FstrDietDate + "','YYYY-MM-DD')) ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE  ACTDATE IS NULL ";
                    }
                    if (GstrWardCodes == "TOP" || FstrDietJikwon)
                    {
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND GBSTS NOT IN ('6','7','9') ";//       '퇴원계산서인쇄, 퇴원수납완료,취소 제외
                    }
                }

                if (FstrDietJikwon == false)
                {
                    if (GstrWardCode == "IU")
                    {
                        if (GstrWardCodes == "SICU" || VB.Left(ComboWard.Text, 2) == "외과")
                        {
                            SQL = SQL + ComNum.VBLF + "  AND  WardCode = 'IU' AND RoomCode=233 ";
                        }
                        else if (GstrWardCodes == "MICU" || VB.Left(ComboWard.Text, 2) == "내과")
                        {
                            SQL = SQL + ComNum.VBLF + "  AND  WardCode = 'IU' AND RoomCode=234 ";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode = '" + GstrWardCode + "' ";
                    }
                }
                SQL = SQL + ComNum.VBLF + "    AND PANO  = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND PANO <'90000000' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY ROOMCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.RowCount++;

                        strAmSet1 = dt.Rows[i]["GBSTS"].ToString().Trim();
                        //선택식 확인
                        strOK = "";

                        SQL = "";
                        SQL = " SELECT TO_CHAR(MDate,'YYYY-MM-DD') MDate,Pano,Menu1,Menu2,Menu3,Menu4,Menu5,Remark,ROWID  ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_SORDER ";
                        SQL = SQL + ComNum.VBLF + "   WHERE Pano='" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND MDate=TO_DATE('" + (TxtDietDate.Text).Trim() + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.GetDataTableEx(ref dtTemp, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dtTemp.Rows.Count > 0)
                        {
                            if (dtTemp.Rows[0]["Menu1"].ToString().Trim() == "Y")
                            {
                                strOK = "OK";
                            }
                            if (dtTemp.Rows[0]["Menu2"].ToString().Trim() == "Y")
                            {
                                strOK = "OK";
                            }
                            if (dtTemp.Rows[0]["Menu3"].ToString().Trim() == "Y")
                            {
                                strOK = "OK";
                            }
                            if (dtTemp.Rows[0]["Menu4"].ToString().Trim() != "")
                            {
                                strOK = "OK";
                            }
                            if (dtTemp.Rows[0]["Menu5"].ToString().Trim() == "Y")
                            {
                                strOK = "OK";
                            }
                        }
                        dtTemp.Dispose();
                        dtTemp = null;

                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();

                        if (strOK == "OK")
                        {
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                        }

                        //신종인플루 환자일 경우 별모양
                        if (READ_INFLU_H1N1(dt.Rows[i]["PANO"].ToString().Trim()) == "YES")
                        {
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text = "★" + SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text;
                        }
                        // 2016 - 05 - 03 김경동

                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim() + ComNum.VBLF + dt.Rows[i]["PANO"].ToString().Trim();

                        FnHight = 0;
                        FnMaxHight = 2;

                        if (strAmSet1 == "6")
                        {
                            strStat = "계산중";
                        }
                        else if (strAmSet1 == "7")
                        {
                            strStat = "퇴  원";
                        }
                        else if (strAmSet1 == "1")
                        {
                            strStat = "가퇴원";
                        }
                        else if (strAmSet1 == "3")
                        {
                            strStat = "부분심사";
                        }
                        else if (strAmSet1 == "4" || strAmSet1 == "5")
                        {
                            strStat = "심사완료";
                        }
                        else if (strAmSet1 == "0")
                        {
                            strStat = "재원중";
                        }
                        else if (strAmSet1 == "2")
                        {
                            strStat = "퇴원등록";
                        }

                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 8].Text = strStat;
                        //COLOR
                        if (strStat == "재원중")
                        {
                            SS1_Sheet1.Rows[SS1_Sheet1.RowCount - 1].ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                        }
                        else
                        {
                            SS1_Sheet1.Rows[SS1_Sheet1.RowCount - 1].ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
                        }

                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();

                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["BI"].ToString().Trim();
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["DRCODE"].ToString().Trim();

                        READ_DIETORDER(strPano, SS1_Sheet1.RowCount - 1);

                        //byte[] a = System.Text.Encoding.Default.GetBytes(SS1_Sheet1.Cells[i, 1].Text);
                        //int intHeight = Convert.ToInt32(a.Length / 8);

                        //SS1_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + (intHeight * 16));

                        SS1_Sheet1.SetRowHeight(SS1_Sheet1.RowCount - 1, Convert.ToInt32(SS1_Sheet1.GetPreferredRowHeight(i)));
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_DIETORDER(string ArgPano, int nRow)
        {
            int j = 0;
            string strDayOld = "";
            int strDay = 0;

            string SQL = "";
            DataTable dt = null;
            DataTable dtSub = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            ComFunc CF = new ComFunc();

            strDayOld = "";

            try
            {
                SQL = "";
                SQL = " SELECT ACTDATE, PANO, BI, DEPTCODE, DRCODE, ";
                SQL = SQL + ComNum.VBLF + " WARDCODE, ROOMCODE, DIETCODE, DIETNAME, SUCODE, DIETDAY,";
                SQL = SQL + ComNum.VBLF + " QTY, UNIT, BUN, ENTDATE, INPUTID, GBSUNAP, PRINT, GBADD, GBADD1, GBADD2 ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DIET_NEWORDER";
                SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + FstrDietDate + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND PANO ='" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "    ORDER BY DIETDAY, BUN ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (j = 0; j < dt.Rows.Count; j++)
                    {
                        if (strDayOld != dt.Rows[j]["DIETDAY"].ToString().Trim())
                        {
                            if (FnMaxHight < FnHight)
                            {
                                FnMaxHight = FnHight;
                            }
                            FnHight = 0;
                            strDayOld = dt.Rows[j]["DIETDAY"].ToString().Trim();
                        }

                        switch (dt.Rows[j]["DIETDAY"].ToString().Trim())
                        {
                            case "1":
                                strDay = 4;
                                break;
                            case "2":
                                strDay = 5;
                                break;
                            default:
                                strDay = 6;
                                break;
                        }
                        //SS1.Col = strDay

                        if (SS1_Sheet1.Cells[nRow, strDay - 1].Text.Trim() == "")
                        {
                            if (VB.Val(dt.Rows[j]["QTY"].ToString().Replace(",", "")) > 1)
                            {
                                SS1_Sheet1.Cells[nRow, strDay - 1].Text = dt.Rows[j]["DIETNAME"].ToString().Trim() + ComNum.VBLF + dt.Rows[j]["QTY"].ToString().Trim();
                                FnHight += 1;
                            }
                            else
                            {
                                SS1_Sheet1.Cells[nRow, strDay - 1].Text = dt.Rows[j]["DIETNAME"].ToString().Trim();
                            }
                            FnHight += 1;
                        }
                        else
                        {
                            if (VB.Val(dt.Rows[j]["QTY"].ToString().Replace(",", "")) > 1)
                            {
                                SS1_Sheet1.Cells[nRow, strDay - 1].Text += ComNum.VBLF + dt.Rows[j]["DIETNAME"].ToString().Trim() + ComNum.VBLF + dt.Rows[j]["QTY"].ToString().Trim();
                                FnHight += 1;
                            }
                            else
                            {
                                SS1_Sheet1.Cells[nRow, strDay - 1].Text += ComNum.VBLF + dt.Rows[j]["DIETNAME"].ToString().Trim();
                            }
                            FnHight += 1;
                        }

                        if (dt.Rows[j]["GBSUNAP"].ToString().Trim() == "1")
                        {
                            SS1_Sheet1.Cells[nRow, 6].Value = true;
                        }

                        if (GstrWardCode == "A1" || GstrWardCode == "A2" || GstrWardCode == "A3" || GstrWardCode == "A4")
                        {
                            if (GstrWardCode == "A1" || GstrWardCode == "A4")
                            {
                                if (dt.Rows[j]["GBADD"].ToString().Trim() == "1")
                                {
                                    SS1_Sheet1.Cells[nRow, 7].Value = true;
                                }
                            }
                            else if (GstrWardCode == "A2")
                            {
                                if (dt.Rows[j]["GBADD1"].ToString().Trim() == "1")
                                {
                                    SS1_Sheet1.Cells[nRow, 7].Value = true;
                                }
                            }
                            else if (GstrWardCode == "A3")
                            {
                                if (dt.Rows[j]["GBADD2"].ToString().Trim() == "1")
                                {
                                    SS1_Sheet1.Cells[nRow, 7].Value = true;
                                }
                            }
                        }
                        else
                        {
                            if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "07:30") > 0 &&
                                string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "13:00") <= 0)
                            {
                                if (dt.Rows[j]["GBADD1"].ToString().Trim() == "1")
                                {
                                    SS1_Sheet1.Cells[nRow, 7].Value = true;
                                }
                            }
                            else if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "13:00") > 0 &&
                                string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "17:30") <= 0)
                            {
                                if (dt.Rows[j]["GBADD2"].ToString().Trim() == "1")
                                {
                                    SS1_Sheet1.Cells[nRow, 7].Value = true;
                                }
                            }
                            else
                            {
                                if (dt.Rows[j]["GBADD"].ToString().Trim() == "1")
                                {
                                    SS1_Sheet1.Cells[nRow, 7].Value = true;
                                }
                            }
                        }

                        if (dt.Rows[j]["BUN"].ToString().Trim() == "01")
                        {
                            SQL = "";
                            SQL = " SELECT MENU" + dt.Rows[j]["DIETDAY"].ToString().Trim() + " FROM DIET_SORDER ";
                            SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + ArgPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND MDATE = TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, FstrDietDate, -1) + "' ,'YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND MENU" + dt.Rows[j]["DIETDAY"].ToString().Trim() + "= 'Y'";

                            SqlErr = clsDB.GetDataTableEx(ref dtSub, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dtSub.Rows.Count > 0)
                            {
                                if (dtSub.Rows[0]["MENU" + dt.Rows[j]["DIETDAY"].ToString().Trim()].ToString().Trim() == "Y")
                                {
                                    SS1_Sheet1.Cells[nRow, strDay - 1].Text = SS1_Sheet1.Cells[nRow, strDay - 1].Text + " (선)";
                                }
                            }

                            dtSub.Dispose();
                            dtSub = null;
                        }
                    }

                    if (FnMaxHight < FnHight)
                    {
                        FnMaxHight = FnHight;
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void WARD_SET()
        {

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strWardCode = "";

            int nTemp = 0;

            ComboWard.Items.Clear();


            Cursor.Current = Cursors.WaitCursor;

            if (GstrWardCodes == "TOP" || FstrDietJikwon || ComQuery.NURSE_System_Manager_Check(Convert.ToDouble(clsType.User.Sabun)))
            {
                ComboWard.Enabled = true;
                strWardCode = "";
            }
            else
            {
                if (GstrWardCodes == "HD")
                {
                    strWardCode = "HD";
                }
                else if (GstrWardCodes == "ER")
                {
                    strWardCode = "ER";
                }

                else
                {
                    if (GstrWardCodes == "MICU" || GstrWardCodes == "SICU")
                    {
                        if (string.Compare(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), "2013-06-22") < 0)
                        {
                            strWardCode = "IU";
                        }
                        else
                        {
                            strWardCode = "32";
                        }
                    }
                    else
                    {
                        strWardCode = GstrWardCodes;
                    }
                }
            }

            try
            {
                //콤보 set
                if (strWardCode == "HD")
                {
                    ComboWard.Items.Add(VB.Left("외래(HD)" + VB.Space(20), 20) + "HD");
                }
                else if (strWardCode == "ER")
                {
                    ComboWard.Items.Add(VB.Left("응급실(ER)" + VB.Space(20), 20) + "ER");
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT WARDCODE, WARDNAME FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                    SQL = SQL + ComNum.VBLF + " WHERE USED = 'Y' ";
                    //SQL = SQL + ComNum.VBLF + " AND WardCode NOT IN ('IQ','2W', '3B','3C','ND') ";
                    //2018-10-02 간호부 요청으로 인해 IQ 풀어줌
                    SQL = SQL + ComNum.VBLF + " AND WardCode NOT IN ('2W', '3B','3C','ND') ";

                    if (strWardCode != "")
                    {
                        if (strWardCode == "NR")
                        {
                            SQL = SQL + ComNum.VBLF + " AND WARDCODE IN ('NR', 'IQ')";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " AND WARDCODE = '" + strWardCode + "'";
                        }                        
                    }

                    SQL = SQL + ComNum.VBLF + " Order By WardNAME ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["WardCODE"].ToString().Trim() == "IU")
                            {
                                if (GstrWardCodes == "TOP" || FstrDietJikwon)
                                {
                                    ComboWard.Items.Add(VB.Left("내과" + dt.Rows[i]["WardNAME"].ToString().Trim() + VB.Space(20), 20) + "IU");
                                    ComboWard.Items.Add(VB.Left("외과" + dt.Rows[i]["WardNAME"].ToString().Trim() + VB.Space(20), 20) + "IU");
                                }
                                else
                                {
                                    if (GstrWardCodes == "MICU")
                                    {
                                        ComboWard.Items.Add(VB.Left("내과" + dt.Rows[i]["WardNAME"].ToString().Trim() + VB.Space(20), 20) + "IU");
                                    }
                                    else if (GstrWardCodes == "SICU")
                                    {
                                        ComboWard.Items.Add(VB.Left("외과" + dt.Rows[i]["WardNAME"].ToString().Trim() + VB.Space(20), 20) + "IU");
                                    }
                                }
                            }
                            else
                            {
                                ComboWard.Items.Add(VB.Left(dt.Rows[i]["WardNAME"].ToString().Trim() + VB.Space(20), 20) + dt.Rows[i]["WardCode"].ToString().Trim());
                            }

                            if (FstrDietJikwon == false)
                            {
                                if (dt.Rows[i]["WardCode"].ToString().Trim() == strWardCode)
                                {
                                    nTemp = i;
                                }
                            }
                        }

                        ComboWard.Items.Add(VB.Left("외래(HD)" + VB.Space(20), 20) + "HD");
                        ComboWard.Items.Add(VB.Left("응급실(ER)" + VB.Space(20), 20) + "ER");
                        ComboWard.Items.Add(VB.Left("(아침)추가분" + VB.Space(20), 20) + "A1");
                        ComboWard.Items.Add(VB.Left("(점심)추가분" + VB.Space(20), 20) + "A2");
                        ComboWard.Items.Add(VB.Left("(저녁)추가분" + VB.Space(20), 20) + "A3");
                        ComboWard.Items.Add(VB.Left("(아침)변경분" + VB.Space(20), 20) + "A4");
                        ComboWard.Items.Add(VB.Left("AL.병동전체" + VB.Space(20), 20) + "AL");
                    }
                    ComboWard.SelectedIndex = nTemp;



                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void SS_BackColor_SET(FarPoint.Win.Spread.SheetView ssSpread_sheet)
        {
            if (FnDietROW == -1 || FnDietCol == -1)
            {
                return;
            }

            ssSpread_sheet.Cells[FnDietROW, FnDietCol].ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
            ssSpread_sheet.Cells[FnDietROW, FnDietCol].BackColor = System.Drawing.Color.FromArgb(255, 255, 223);

        }

        private void SS_BackColor(FarPoint.Win.Spread.SheetView ssSpread_sheet, int ArgRed, int ArgGreen, int ArgBlue)
        {
            if (FnDietROW == -1 || FnDietCol == -1)
            {
                return;
            }

            ssSpread_sheet.Cells[FnDietROW, FnDietCol].ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            ssSpread_sheet.Cells[FnDietROW, FnDietCol].BackColor = System.Drawing.Color.FromArgb(ArgRed, ArgGreen, ArgBlue);

        }

        private void SCREEN_CLEAR()
        {
            SS_BackColor(SS1_Sheet1, 236, 255, 255);

            SSOrder_Sheet1.RowCount = 0;
            SSOrder_Sheet1.RowCount = 15;

            btnMemoDel.Visible = false;

            if (clsType.User.Sabun != "04349")
            {
                SSCode1_Sheet1.Columns[3].Visible = false;
                SSCode2_Sheet1.Columns[3].Visible = false;
                SSCode3_Sheet1.Columns[3].Visible = false;
                SSCode4_Sheet1.Columns[3].Visible = false;

                SSCode1_Sheet1.Columns[2].Visible = false;
                SSCode2_Sheet1.Columns[2].Visible = false;
                SSCode3_Sheet1.Columns[2].Visible = false;
                SSCode4_Sheet1.Columns[2].Visible = false;

                SSBIGO_Sheet1.Columns[2].Visible = false;
                SSBIGO_Sheet1.Columns[3].Visible = false;

                SSOrder_Sheet1.Columns[1].Visible = false;
                SSOrder_Sheet1.Columns[5].Visible = false;
                SSOrder_Sheet1.Columns[6].Visible = false;
                SSOrder_Sheet1.Columns[7].Visible = false;

                SS1_Sheet1.Columns[9].Visible = false;
                SS1_Sheet1.Columns[10].Visible = false;
                SS1_Sheet1.Columns[11].Visible = false;
                SS1_Sheet1.Columns[12].Visible = false;
                SS1_Sheet1.Columns[13].Visible = false;
            }

            chkDietDay0.Checked = false;
            chkDietDay1.Checked = false;
            chkDietDay2.Checked = false;

            FnDietCol = -1;
            FnDietROW = -1;
            FnRow = 0;

            SSOrder.Enabled = false;
            SS1.Enabled = true;
            btnRefresh.Enabled = true;
            btnSave.Enabled = false;
            panUpdate.Enabled = true;

            TxtWeight.Text = "";
            TxtHeight.Text = "";
            lbllKcal.Text = "";

            txtMemo.Text = "";
            txtMemoRowid.Text = "";
            ssMemoList_Sheet1.RowCount = 0;

            FNIPDNO = 0;

            cboDiet1.Items.Clear();
            cboDiet1.Items.Add("");
            cboDiet1.Items.Add("200");
            cboDiet1.Items.Add("300");
            cboDiet1.Items.Add("400");
            cboDiet1.Items.Add("500");
            cboDiet1.SelectedIndex = 0;


            cboDiet2.Items.Clear();
            cboDiet2.Items.Add("");
            cboDiet2.Items.Add("200");
            cboDiet2.Items.Add("300");
            cboDiet2.Items.Add("400");
            cboDiet2.Items.Add("500");
            cboDiet2.SelectedIndex = 0;


            cboDiet3.Items.Clear();
            cboDiet3.Items.Add("");
            cboDiet3.Items.Add("200");
            cboDiet3.Items.Add("300");
            cboDiet3.Items.Add("400");
            cboDiet3.Items.Add("500");
            cboDiet3.SelectedIndex = 0;
        }

        private void Insert_Diet_Order(FarPoint.Win.Spread.SheetView ArgSS, int argROW, string argBun)
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strDIETCODE = "";
            //string strDietName = "";
            string strSucode = "";
            string strQTY = "";
            //string strArgBun = "";


            Cursor.Current = Cursors.WaitCursor;

            if (FnDietCol == -1 && FnDietROW == -1)
            {
                return;
            }

            strQTY = ArgSS.Cells[argROW, 1].Text;
            strDIETCODE = ArgSS.Cells[argROW, 2].Text;

            if (strQTY.Trim() == "")
            {
                MessageBox.Show("수량을 입력해주세요", "확인");
                return;
            }

            try
            {
                SQL = "";
                SQL = " SELECT DIETCODE, CODENAME, SUCODE, DIETUNIT, GUBUN FROM " + ComNum.DB_PMPA + "DIET_NEWCODE";
                SQL = SQL + ComNum.VBLF + " WHERE DIETCODE = '" + strDIETCODE + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BUN = '" + argBun + "' ";//  '정규식

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 1 || dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("식이코드 오류입니다." + ComNum.VBLF + "영양실로 연락바립니다", "확인");
                    return;
                }

                FnRow = FnRow + 1;

                if (SSOrder_Sheet1.RowCount < FnRow)
                {
                    SSOrder_Sheet1.RowCount = FnRow;
                }
                SSOrder_Sheet1.Cells[FnRow - 1, 1].Text = dt.Rows[0]["DIETCode"].ToString().Trim();
                SSOrder_Sheet1.Cells[FnRow - 1, 2].Text = dt.Rows[0]["CodeName"].ToString().Trim();
                SSOrder_Sheet1.Cells[FnRow - 1, 3].Text = dt.Rows[0]["SUCODE"].ToString().Trim();
                SSOrder_Sheet1.Cells[FnRow - 1, 4].Text = strQTY;
                SSOrder_Sheet1.Cells[FnRow - 1, 5].Text = dt.Rows[0]["DIETUNIT"].ToString().Trim();
                SSOrder_Sheet1.Cells[FnRow - 1, 6].Text = argBun;
                SSOrder_Sheet1.Cells[FnRow - 1, 7].Text = "";
                SSOrder_Sheet1.Cells[FnRow - 1, 8].Text = dt.Rows[0]["GUBUN"].ToString().Trim();
                strSucode = dt.Rows[0]["SUCODE"].ToString().Trim();

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void Read_Diet_Order(string argDietDay, string ArgPano, string ArgDate)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            SSOrder_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            FnRow = 0;

            try
            {
                SQL = "";
                SQL = "SELECT ACTDATE, PANO, BI, DEPTCODE, DRCODE, WARDCODE, ROOMCODE, ";
                SQL = SQL + ComNum.VBLF + " DIETCODE, DIETNAME, SUCODE, DIETDAY, QTY, UNIT, BUN, ";
                SQL = SQL + ComNum.VBLF + " ENTDATE, INPUTID, GBSUNAP, PRINT, GBADD, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NEWORDER";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND PANO ='" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DIETDAY = '" + argDietDay + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SSOrder_Sheet1.RowCount = dt.Rows.Count + 5;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SSOrder_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DIETCODE"].ToString().Trim();
                        SSOrder_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DIETNAME"].ToString().Trim();
                        SSOrder_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        SSOrder_Sheet1.Cells[i, 4].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        SSOrder_Sheet1.Cells[i, 5].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                        SSOrder_Sheet1.Cells[i, 6].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        SSOrder_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        if (dt.Rows[i]["BUN"].ToString().Trim() == "02" || dt.Rows[i]["BUN"].ToString().Trim() == "03")
                        {
                            FstrCureFood = "OK";
                        }

                        if (dt.Rows[i]["DIETNAME"].ToString().Trim().IndexOf("경관") > -1)
                        {
                            FstrCureFood = "OK";
                        }
                        FnRow = FnRow + 1;
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void Read_Diet_Code()
        {
            //int nNextRow = 0;
            //double nStart = 0;
            //double nLast = 0;
            //double nInterval = 0;
            //string strComboList = "";
            //int cGbData = 0;

            //nNextRow = 1;

            //'정규식이
            Display_Diet_Code(SSCode1, "01");
            //'일반치료식
            Display_Diet_Code(SSCode2, "02");
            //'저염치료식
            Display_Diet_Code(SSCode3, "03");
            //'추가식
            Display_Diet_Code(SSCode4, "04");
            //'비고
            Display_Diet_Code(SSBIGO, "05");
        }

        /// <summary>
        /// Display_Diet_Code
        /// </summary>
        /// <param name="ArgSS"></param>
        /// <param name="argBun">01: 정규식이 , 02. 저염치료식 03. 일반치료식 04 추가식</param>
        private void Display_Diet_Code(FpSpread ArgSS, string argBun)
        {
            int j = 0;
            int nBun = 0;
            int nRow = 0;
            int nStart = 0;
            int nLast = 0;
            int nInterval = 0;
            //string strComboList = "";
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            FarPoint.Win.Spread.CellType.ComboBoxCellType CellCbo = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_PMPA + "DIET_NEWCODE ";
                SQL = SQL + ComNum.VBLF + "   WHERE BUN = '" + argBun + "'";
                SQL = SQL + ComNum.VBLF + "     AND GBUSED <> '1' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY RANK ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRow = dt.Rows.Count;
                    ArgSS.ActiveSheet.RowCount = nRow;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nBun = (int)VB.Val(dt.Rows[i]["BUN"].ToString().Trim());
                        ArgSS.ActiveSheet.Cells[i, 0].Text = VB.Space(3) + dt.Rows[i]["CODEName"].ToString().Trim();

                        if (VB.Val(dt.Rows[i]["DIETINTERVAL"].ToString().Trim()) > 0)
                        {
                            //ArgSS.Cells[i, 1].Text
                            #region Spread_ComboBox_List        

                            CellCbo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();    //일반 셀을 콤보박스로 변경

                            ArgSS.ActiveSheet.Cells[i, 1].CellType = CellCbo;

                            nStart = (int)VB.Val(dt.Rows[i]["DIETFROM"].ToString().Trim());
                            nLast = (int)VB.Val(dt.Rows[i]["DIETTO"].ToString().Trim());
                            nInterval = (int)VB.Val(dt.Rows[i]["DIeTInterval"].ToString().Trim());
                            //Split
                            //strComboList = "";

                            int k = 0;

                            for (j = nStart; j <= nLast; j += nInterval)
                            {
                                k++;
                            }

                            string[] arrComboList = new string[k];

                            k = 0;
                            for (j = nStart; j <= nLast; j += nInterval)
                            {
                                arrComboList[k] = j.ToString().Trim();
                                k++;
                            }

                            clsSpread.gSpreadComboDataSetEx1(ArgSS, i, 1, i, 1, arrComboList, false);

                            ArgSS.ActiveSheet.Cells[i, 1].Locked = false;

                            #endregion
                        }
                        else
                        {
                            ArgSS.ActiveSheet.Cells[i, 1].Text = "1";
                        }

                        ArgSS.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DietCode"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["bigo"].ToString().Trim();
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ChkDay_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkDay.Checked == true)
            {
                MessageBox.Show("해당 하는 환자의 아침, 점심, 저녁 식이오더" + ComNum.VBLF + "모두 변경합니다", "확인");
            }
        }

        private void chkDietDay_CheckedChanged(object sender, EventArgs e)
        {

            if (chkDietDay0.Checked == true)
            {
                if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "06:30") >= 0 && FstrDietJikwon == false)
                {
                    MessageBox.Show("지금 시간에는 식이변경이 불가능합니다", "확인");
                    chkDietDay0.Checked = false;
                }
            }

            else if (chkDietDay1.Checked == true)
            {
                if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "11:30") >= 0 && FstrDietJikwon == false)
                {
                    MessageBox.Show("지금 시간에는 식이변경이 불가능합니다", "확인");
                    chkDietDay0.Checked = false;
                }
            }

            else if (chkDietDay2.Checked == true)
            {
                if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "16:30") >= 0 && FstrDietJikwon == false)
                {
                    MessageBox.Show("지금 시간에는 식이변경이 불가능합니다", "확인");
                    chkDietDay0.Checked = false;
                }
            }
        }

        private void btnALL_Click(object sender, EventArgs e)
        {
            frmDietOrderAuto f = new frmDietOrderAuto(SS1);
            f.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void btnExit2_Click(object sender, EventArgs e)
        {
            panMEMO.Visible = false;
        }

        private void btnMemoNew_Click(object sender, EventArgs e)
        {
            txtMemo.Text = "";
            txtMemoRowid.Text = "";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefreshSecrch();
        }

        /// <summary>
        /// btnRefresh_Click
        /// </summary>
        private void btnRefreshSecrch()
        {
            SS1_Sheet1.RowCount = 0;

            if (FstrDietJikwon == false)
            {
                TxtDietDate.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            }

            Read_Diet_Code();
            Read_Diet_Code();

            FstrDietDate = TxtDietDate.Text;
            GstrWardCode = VB.Mid(ComboWard.Text, 21, ComboWard.Text.Trim().Length);

            if (GstrWardCode == "HD")
            {
                btnALL.Visible = true;
            }
            else
            {
                btnALL.Visible = false;
            }

            timer1.Enabled = false;

            if (GstrWardCode == "HD")
            {
                READ_OPD();
            }
            else if (GstrWardCode == "ER")
            {
                READ_OPD_ER();
            }
            else if (GstrWardCode == "A1" || GstrWardCode == "A2" || GstrWardCode == "A3" || GstrWardCode == "A4")
            {
                READ_DIET_ADD();
            }
            else
            {
                READ_IPD();
            }

            if (FstrDietJikwon)
            {
                READ_CHANGE_SORDER();
                READ_MEMO_LIST_New();
                READ_CONSULT();
            }

            timer1.Enabled = true;
            fnCnt = 0;

        }

        /// <summary>
        /// 치료식
        /// </summary>
        /// <returns></returns>
        private bool TreatmentMeal()
        {
            int i = 0;
            bool rtnVal = false;

            for (i = 1; i <= SSOrder_Sheet1.RowCount; i++)
            {
                if (SSOrder_Sheet1.Cells[i - 1, 6].Text == "02" || SSOrder_Sheet1.Cells[i - 1, 6].Text == "03")
                {
                    rtnVal = true;
                    break;
                }
            }

            for (i = 1; i <= SSOrder_Sheet1.RowCount; i++)
            {
                if (SSOrder_Sheet1.Cells[i - 1, 8].Text == "8")
                {
                    rtnVal = true;
                    break;
                }
            }

            return rtnVal;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Save() == false)
            {
                MessageBox.Show("저장 실패, 다시 확인 바랍니다.");
            }

            after_Save();
        }

        private bool Save()
        {
            string strROWID = "";
            string strDel = "";
            string strDIETCODE = "";
            string strDietName = "";
            string strSucode = "";
            string strQTY = "";
            string strUnit = "";
            string strDietDay = "";
            //string strOK = "";
            string strBun = "";
            string strChk = "";
            string strBigoCheck = "";  //비고만 있는 경우 체크
            //string strWard = "";
            string strChkDietDay = "";
            //string strOutChk = "";
            int nCnt = 0;
            int nCnt2 = 0;  //연하곤란식 카운트
            //int k = 0;
            int j = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strCheckWard = "";       //호실에 따른 병동 체크

            Cursor.Current = Cursors.WaitCursor;
            strDietOrder = "";

            if (TreatmentMeal() == true)
            {
                if (chkDietDay0.Checked == true)
                {
                    if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "05:00") >= 0 && FstrDietJikwon == false)
                    {
                        MessageBox.Show("지금 시간에는 치료식이 변경이 불가능합니다." + ComNum.VBLF + ComNum.VBLF + strMsg2, "확인");
                        chkDietDay0.Checked = false;
                        return rtnVal;
                    }
                }
                if (chkDietDay1.Checked == true)
                {
                    if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "10:00") >= 0 && FstrDietJikwon == false)
                    {
                        MessageBox.Show("지금 시간에는 치료식이 변경이 불가능합니다." + ComNum.VBLF + ComNum.VBLF + strMsg2, "확인");
                        chkDietDay1.Checked = false;
                        return rtnVal;
                    }
                }
                if (chkDietDay2.Checked == true)
                {
                    if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "15:00") >= 0 && FstrDietJikwon == false)
                    {
                        MessageBox.Show("지금 시간에는 치료식이 변경이 불가능합니다." + ComNum.VBLF + ComNum.VBLF + strMsg2, "확인");
                        chkDietDay2.Checked = false;
                        return rtnVal;
                    }
                }
            }
            else
            {
                if (chkDietDay0.Checked == true)
                {
                    if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "06:30") >= 0 && FstrDietJikwon == false)
                    {
                        MessageBox.Show("지금 시간에는 식이변경이 불가능합니다." + ComNum.VBLF + ComNum.VBLF + strMsg, "확인");
                        chkDietDay0.Checked = false;
                        return rtnVal;
                    }
                }
                if (chkDietDay1.Checked == true)
                {
                    if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "11:30") >= 0 && FstrDietJikwon == false)
                    {
                        MessageBox.Show("지금 시간에는 식이변경이 불가능합니다." + ComNum.VBLF + ComNum.VBLF + strMsg, "확인");
                        chkDietDay1.Checked = false;
                        return rtnVal;
                    }
                }
                if (chkDietDay2.Checked == true)
                {
                    if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "16:30") >= 0 && FstrDietJikwon == false)
                    {
                        MessageBox.Show("지금 시간에는 식이변경이 불가능합니다." + ComNum.VBLF + ComNum.VBLF + strMsg, "확인");
                        chkDietDay2.Checked = false;
                        return rtnVal;
                    }
                }
            }

            if (GET_DIETPRT_LOCK() == true)
            {
                MessageBox.Show("영양실에서 식표인쇄 중입니다. 3분후에 작업하시기 바랍니다.", "확인");
                return rtnVal;
            }

            FstrRoomCode = FstrRoomCode.Replace("★", "");

            if (FstrDietJikwon == false)
            {
                TxtDietDate.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            }

            FstrDietDate = TxtDietDate.Text;

            if (TxtDietDate.Text != ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") && FstrDietJikwon == false)
            {
                MessageBox.Show("식이 오더는 당일 날짜만 입력가능합니다.프로그램 종료하시고, 다시입력하세요.", "확인");
                return rtnVal;
            }

            //식이 등록 루틴은 무조건 삭제후 다시입력되로록 처리 하였습니다.
            if (GstrWardCode != "ER")
            {
                if (VB.Val(TxtHeight.Text) <= 130)
                {
                    if (ComFunc.MsgBoxQ("키가 130cm 이하입니다. 계속 입력하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return rtnVal;
                    }
                }
                if (VB.Val(TxtWeight.Text) >= 130)
                {
                    if (ComFunc.MsgBoxQ("체중이 130kg 이상입니다.계속 입력하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return rtnVal;
                    }
                }
            }
            //strOK = "OK";
            strDietOrder = "";
            strChkDietDay = "";

            if (chkDietDay0.Checked == true)
            {
                if (strChkDietDay == "")
                {
                    strChkDietDay = "'" + 1 + "'";
                }
                else
                {
                    strChkDietDay = strChkDietDay + ",'" + 1 + "'";
                }
            }
            if (chkDietDay1.Checked == true)
            {
                if (strChkDietDay == "")
                {
                    strChkDietDay = "'" + 2 + "'";
                }
                else
                {
                    strChkDietDay = strChkDietDay + ",'" + 2 + "'";
                }
            }
            if (chkDietDay2.Checked == true)
            {
                if (strChkDietDay == "")
                {
                    strChkDietDay = "'" + 3 + "'";
                }
                else
                {
                    strChkDietDay = strChkDietDay + ",'" + 3 + "'";
                }
            }




            //하단 조건들이 이상해서 보존 해놓고 신규 조건 추가(2019-07-11) 조건에 걸리면 무조건 입력 안되도록 막음!
            //2019-07-11
            strBigoCheck = "";

            for (i = 1; i <= SSOrder_Sheet1.NonEmptyRowCount; i++)
            {
                if (SSOrder_Sheet1.Cells[i - 1, 0].Text == "")
                {
                    if (SSOrder_Sheet1.Cells[i - 1, 3].Text != "########" && SSOrder_Sheet1.Cells[i - 1, 3].Text.Trim() != "")
                    {
                        strBigoCheck = "PASS";
                    }
                }
            }

            if (strBigoCheck != "PASS")
            {
                MessageBox.Show("정규식 없이 비고만 입력된 경우 저장이 불가능합니다." + ComNum.VBLF + "정규식을 입력하시기 바랍니다.");
                return rtnVal;
            }

            strChk = "KO";


            if (strChk == "NO")
            {
                for (i = 1; i <= SSOrder_Sheet1.NonEmptyRowCount; i++)
                {
                    if (SSOrder_Sheet1.Cells[i - 1, 0].Text == "")
                    {
                        if (SSOrder_Sheet1.Cells[i - 1, 6].Text == "04" || SSOrder_Sheet1.Cells[i - 1, 6].Text == "99" || SSOrder_Sheet1.Cells[i - 1, 6].Text == "01")
                        {
                            strChk = "OK";
                        }
                    }
                }
            }

            for (j = 1; j <= SSOrder_Sheet1.NonEmptyRowCount; j++)
            {
                if (SSOrder_Sheet1.Cells[j - 1, 0].Text == "")
                {
                    if (SSOrder_Sheet1.Cells[j - 1, 6].Text == "02" || SSOrder_Sheet1.Cells[j - 1, 6].Text == "03")
                    {
                        for (i = 1; i <= SSOrder_Sheet1.NonEmptyRowCount; i++)
                        {
                            if (SSOrder_Sheet1.Cells[i - 1, 0].Text == "")
                            {
                                if (SSOrder_Sheet1.Cells[i - 1, 6].Text == "01")
                                {
                                    strChk = "OK";
                                    break;
                                }
                                else
                                {
                                    strChk = "NO";
                                }
                            }
                        }
                    }
                }
            }

            //2019-01-19 조건추가
            if (strChk == "NO")
            {
                for (j = 1; j <= SSOrder_Sheet1.NonEmptyRowCount; j++)
                {
                    if (SSOrder_Sheet1.Cells[j - 1, 6].Text == "03")
                    {
                        for (i = 1; i <= SSOrder_Sheet1.NonEmptyRowCount; i++)
                        {
                            if (SSOrder_Sheet1.Cells[i - 1, 3].Text == "FD029" || SSOrder_Sheet1.Cells[i - 1, 3].Text == "FD030" || SSOrder_Sheet1.Cells[i - 1, 3].Text == "FD031")
                            {
                                strChk = "OK";
                                break;
                            }
                            else
                            {
                                strChk = "NO";
                            }
                        }
                    }
                }
            }

            if (strChk == "NO")
            {
                for (j = 1; j <= SSOrder_Sheet1.NonEmptyRowCount; j++)
                {
                    if (SSOrder_Sheet1.Cells[j - 1, 6].Text == "03")
                    {
                        for (i = 1; i <= SSOrder_Sheet1.NonEmptyRowCount; i++)
                        {
                            if (SSOrder_Sheet1.Cells[i - 1, 3].Text == "FD031A" && SSOrder_Sheet1.Rows.Count > 1)
                            {
                                strChk = "OK"; 
                                break;
                            }
                            else
                            {
                                strChk = "NO";
                                MessageBox.Show("연하곤란식4단계는 정규식의 밥,죽 2개중 선택해주셔야합니다.", "확인");
                                return rtnVal;
                            }
                        }
                    }
                }
            }

            if (strChk == "NO")
            {
                MessageBox.Show("정규식이 반드시 입력하세요", "확인");
                return rtnVal;
            }

            //HD 실의 경우 키 몸무게 1로 입력되게.
            if (FstrRoomCode != "500" && FstrRoomCode != "100")
            {
                if (VB.Val(TxtWeight.Text.Trim()) <= 0 || VB.Val(TxtHeight.Text.Trim()) < 0 || VB.Val(TxtWeight.Text.Trim()) == VB.Val(TxtHeight.Text.Trim()))
                {
                    if (string.Compare(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), "2007-11-15") <= 0)
                    {
                        MessageBox.Show("2007-11-15 이후부터는 환자의 몸무게,키 입력후 식이 오더가 전송이 됩니다.", "확인");
                    }
                    else
                    {
                        MessageBox.Show("환자의 몸무게,키 입력해주세요! 식이 오더 전송 안됨.!!!", "확인");
                        return rtnVal;
                    }
                }
            }
            else if (FstrRoomCode == "500" || FstrRoomCode == "100")
            {
                TxtHeight.Text = "1";
                TxtWeight.Text = "1";
            }

            nCnt = 0;
            //정규식이 중복확인
            for (j = 1; j <= SSOrder_Sheet1.NonEmptyRowCount; j++)
            {
                if (Convert.ToBoolean(SSOrder_Sheet1.Cells[j - 1, 0].Value) == false)
                {
                    if (SSOrder_Sheet1.Cells[j - 1, 6].Text == "01")
                    {
                        nCnt += 1;
                    }
                }
            }


            for (j = 1; j <= SSOrder_Sheet1.NonEmptyRowCount; j++)
            {
                if (Convert.ToBoolean(SSOrder_Sheet1.Cells[j - 1, 0].Value) == false)
                {
                    switch (SSOrder_Sheet1.Cells[j - 1, 3].Text)
                    {
                        case "FD029":
                        case "FD030":
                        case "FD031":
                        //case "FD031A":        //4단계는 제외
                            nCnt2 += 1;
                            break;
                    }
                }
            }

            if (nCnt >= 2)
            {
                MessageBox.Show("정규식이 2개가 입력되었음. 정규식을 1개만 입력 요망");
                return rtnVal;
            }

            if (nCnt >= 1 && nCnt2 >= 1)
            {
                MessageBox.Show("정규식과 연하곤란식이 함께 입력되었습니다. 한가지만 입력이 가능합니다.");
                return rtnVal;
            }

            //2019-04-08
            strCheckWard = Check_WardCode(FstrRoomCode);
            if (strCheckWard != GstrWardCode) 
            {
                MessageBox.Show("병동과 호실이 맞질 않습니다.(예를 들어 753호인데 33병동인 경우)" + ComNum.VBLF + "확인하시기 바랍니다.");
                return rtnVal;
            }

            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                //식이 삭제
                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NEWORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + FstrDietDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND PANO ='" + GstrPANO + "'";
                SQL = SQL + ComNum.VBLF + "   AND DIETDAY IN ( " + strChkDietDay + ")          ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //식이 재등록
                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "DIET_NEWORDER SET";
                SQL = SQL + ComNum.VBLF + " GBADD = NULL,";
                SQL = SQL + ComNum.VBLF + " GBADD1 = NULL,";
                SQL = SQL + ComNum.VBLF + " GBADD2 = NULL";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + FstrDietDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND PANO ='" + GstrPANO + "'";
                SQL = SQL + ComNum.VBLF + "   AND DIETDAY IN ( " + strChkDietDay + ")          ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (i = 1; i <= FnRow; i++)
                {
                    strDel = Convert.ToBoolean(SSOrder_Sheet1.Cells[i - 1, 0].Value) == true ? "1" : "0";
                    strDIETCODE = SSOrder_Sheet1.Cells[i - 1, 1].Text;
                    strDietName = SSOrder_Sheet1.Cells[i - 1, 2].Text;

                    if (strDel != "1")
                    {
                        strDietOrder = strDietOrder + strDietName + ComNum.VBLF;
                    }

                    strSucode = SSOrder_Sheet1.Cells[i - 1, 3].Text;
                    strQTY = SSOrder_Sheet1.Cells[i - 1, 4].Text;

                    if (strDel != "1" && Convert.ToDouble(strQTY) > 1)
                    {
                        strDietOrder = strDietOrder + strQTY + ComNum.VBLF;
                    }

                    strUnit = SSOrder_Sheet1.Cells[i - 1, 5].Text;
                    strBun = SSOrder_Sheet1.Cells[i - 1, 6].Text;
                    strROWID = SSOrder_Sheet1.Cells[i - 1, 7].Text;
                    strDietDay = FstrDietDay;

                    if (strDel != "1")
                    {
                        #region GoSub DIET_ORDER_INSERT1

                        if (chkDietDay0.Checked == true)
                        {
                            strDietDay = "1";

                            if (DIET_ORDER_INSERT(clsDB.DbCon, strDIETCODE, strDietName, strSucode, strDietDay, strQTY, strUnit, strBun) == false)
                            {
                                return rtnVal;
                            }
                        }

                        if (chkDietDay1.Checked == true)
                        {
                            strDietDay = "2";

                            if (DIET_ORDER_INSERT(clsDB.DbCon, strDIETCODE, strDietName, strSucode, strDietDay, strQTY, strUnit, strBun) == false)
                            {
                                return rtnVal;
                            }
                        }

                        if (chkDietDay2.Checked == true)
                        {
                            strDietDay = "3";

                            if (DIET_ORDER_INSERT(clsDB.DbCon, strDIETCODE, strDietName, strSucode, strDietDay, strQTY, strUnit, strBun) == false)
                            {
                                return rtnVal;
                            }
                        }

                        #endregion

                    }
                }

                if (DIET_MEMO_INSERT() == false)
                {
                    return rtnVal;
                }

                //키와 몸무게 업데이트
                TxtWeight.Text = TxtWeight.Text == "" ? "0" : TxtWeight.Text;
                TxtHeight.Text = TxtHeight.Text == "" ? "0" : TxtHeight.Text;

                SQL = "";
                SQL = " UPDATE IPD_NEW_MASTER SET WEIGHT = " + VB.Val(TxtWeight.Text).ToString("##0.0") + ", ";
                SQL = SQL + ComNum.VBLF + "  HEIGHT = " + VB.Val(TxtHeight.Text).ToString("##0.0") + " ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + FNIPDNO + " ";

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
                SQL = " SELECT IPDNO FROM DIET_S_MANAGER ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + FNIPDNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    SQL = "";
                    SQL = " UPDATE DIET_S_SEARCH SET ";
                    SQL = SQL + ComNum.VBLF + " WEIGHT = " + VB.Val(TxtWeight.Text).ToString("##0.0") + ", ";
                    SQL = SQL + ComNum.VBLF + " HEIGHT = " + VB.Val(TxtHeight.Text).ToString("##0.0") + " ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + FNIPDNO + " ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);

                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }

                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                dt.Dispose();
                dt = null;

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        void after_Save()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strChkDietDay = "";

            Cursor.Current = Cursors.WaitCursor;

            if (chkDietDay0.Checked == true)
            {
                if (strChkDietDay == "")
                {
                    strChkDietDay = "'" + 1 + "'";
                }
                else
                {
                    strChkDietDay = strChkDietDay + ",'" + 1 + "'";
                }
            }
            if (chkDietDay1.Checked == true)
            {
                if (strChkDietDay == "")
                {
                    strChkDietDay = "'" + 2 + "'";
                }
                else
                {
                    strChkDietDay = strChkDietDay + ",'" + 2 + "'";
                }
            }
            if (chkDietDay2.Checked == true)
            {
                if (strChkDietDay == "")
                {
                    strChkDietDay = "'" + 3 + "'";
                }
                else
                {
                    strChkDietDay = strChkDietDay + ",'" + 3 + "'";
                }
            }

            try
            {
                //금식이나 사식인 경우 보호자식 추가 가능하고 나머지 오더 등록 할 수 없다.  윤
                SQL = "";
                SQL = " SELECT DIETCODE, BUN FROM DIET_NEWORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + FstrDietDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND PANO ='" + GstrPANO + "'";
                SQL = SQL + ComNum.VBLF + "   AND DIETDAY IN ( " + strChkDietDay + ")          ";
                SQL = SQL + ComNum.VBLF + "   AND BUN = '01' ";
                SQL = SQL + ComNum.VBLF + "   AND DIETCODE IN ('29','30') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count >= 1)
                {
                    SQL = "";
                    SQL = " SELECT DIETCODE, BUN FROM DIET_NEWORDER";
                    SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + FstrDietDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND PANO ='" + GstrPANO + "'";
                    SQL = SQL + ComNum.VBLF + "   AND DIETDAY IN ( " + strChkDietDay + ")          ";
                    SQL = SQL + ComNum.VBLF + "   AND SUCODE NOT IN ('FD021','FD020') ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (dt1.Rows[i]["BUN"].ToString().Trim() == "04")
                        {
                            switch (dt1.Rows[i]["DIETCODE"].ToString().Trim())
                            {
                                case "13":
                                case "20":
                                case "21":
                                case "22":
                                case "23":
                                    break;
                                default:
                                    ComFunc.MsgBox("금식이나 사식인 경우 등록 할수 없습니다.");
                                    dt.Dispose();
                                    dt = null;
                                    dt1.Dispose();
                                    dt1 = null;
                                    Cursor.Current = Cursors.Default;
                                    return;
                            }
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;
                }

                dt.Dispose();
                dt = null;

                if (chkDietDay0.Checked == true)
                {
                    SS1_Sheet1.Cells[FnDietROW, 3].Text = strDietOrder;
                }

                if (chkDietDay1.Checked == true)
                {
                    SS1_Sheet1.Cells[FnDietROW, 4].Text = strDietOrder;
                }

                if (chkDietDay2.Checked == true)
                {
                    SS1_Sheet1.Cells[FnDietROW, 5].Text = strDietOrder;
                }

                SS1_Sheet1.Cells[FnDietROW, 7].Value = false;

                if (Get_FirstTreatMeal(GstrPANO, FstrDietDate, strChkDietDay) == true)
                {
                    ComFunc.MsgBox("영양협진이 필요한 환자입니다.");
                }

                SCREEN_CLEAR();

                timer1.Enabled = true;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private string Check_WardCode(string argRoomCode)
        {
            string SQL = "";
            string SqlErr = "";
            string strWard = "";

            DataTable dt = new DataTable();

            switch (GstrWardCode)
            {
                case "33":
                case "35":
                case "40":
                case "50":
                case "53":
                case "55":
                case "60":
                case "63":
                case "65":
                case "70":
                case "73":
                case "75":
                case "80":
                case "83":
                    SQL = " SELECT WARDCODE FROM KOSMOS_PMPA.BAS_ROOM ";
                    SQL += ComNum.VBLF + " WHERE ROOMCODE = '" + argRoomCode + "'";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return GstrWardCode;
                    }
                    strWard = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    dt.Dispose();
                    dt = null;

                    break;

                default:
                    strWard = GstrWardCode;
                    break;

            }

            return strWard;

        }


        bool Get_FirstTreatMeal(string argPano, string argDate, string argDiet)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strGINDATE = "";
            string strGOUTDATE = "";

            bool rtnVar = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT INDATE, OUTDATE FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRUNC(INDATE) <= TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND (OUTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD') OR (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND OUTDATE IS NULL)) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    strGINDATE = VB.Left(dt.Rows[0]["INDATE"].ToString().Trim(), 10);
                    strGOUTDATE = VB.Left(dt.Rows[0]["OUTDATE"].ToString().Trim(), 10);
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT MIN(ACTDATE) ACTDATE, DIETDAY  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NEWORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + strGINDATE + "','YYYY-MM-DD') ";
                if (strGOUTDATE != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + strGOUTDATE + "','YYYY-MM-DD') ";
                }
                SQL = SQL + ComNum.VBLF + " AND (DIETCODE IN ('15','16','17','35','27','44','45') ";
                SQL = SQL + ComNum.VBLF + "      or BUN IN ('02','03')) ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY DIETDAY ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY ACTDATE ASC, DIETDAY ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["ACTDATE"].ToString().Trim() == argDate && dt.Rows[0]["DIETDAY"].ToString().Trim() == argDiet.Replace("'", ""))
                    {
                        rtnVar = true;
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        bool DIET_MEMO_INSERT()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strDietCert = "";

            if (txtMemo.Text.Trim() == "" && cboDiet1.Text.Trim() == "" && cboDiet2.Text.Trim() == "" && cboDiet3.Text.Trim() == "")
            {
                return true;
            }

            strDietCert = "";

            if (cboDiet1.Text.Trim() == "" && cboDiet2.Text.Trim() == "" && cboDiet3.Text.Trim() == "")
            {
                strDietCert = "1";
            }

            if (txtMemoRowid.Text != "")
            {
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_MEMO_HIS ";
                SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.DIET_MEMO";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + (txtMemoRowid.Text).Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "DIET_MEMO";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + (txtMemoRowid.Text).Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
            }

            SQL = "";
            SQL = "INSERT INTO " + ComNum.DB_PMPA + "DIET_MEMO (";
            SQL = SQL + ComNum.VBLF + " IPDNO, PANO, WRITEDATE, ";
            SQL = SQL + ComNum.VBLF + " WRITESABUN, MEMO, DIETCERT, GUBUN, ";
            SQL = SQL + ComNum.VBLF + " DIET1, DIET2, DIET3) VALUES (";
            SQL = SQL + ComNum.VBLF + FNIPDNO + ",'" + GstrPANO + "', SYSDATE, ";
            SQL = SQL + ComNum.VBLF + clsType.User.Sabun + ",'" + (txtMemo.Text).Trim() + "','" + strDietCert + "','1',";
            SQL = SQL + ComNum.VBLF + "'" + (cboDiet1.Text).Trim() + "','" + (cboDiet2.Text).Trim() + "','" + (cboDiet3.Text).Trim() + "') ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            return true;
        }

        /// <summary>
        /// DIET_ORDER_INSERT
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strDIETCODE"></param>
        /// <param name="strDietName"></param>
        /// <param name="strSucode"></param>
        /// <param name="strDietDay"></param>
        /// <param name="strQTY"></param>
        /// <param name="strUnit"></param>
        /// <param name="strBun"></param>
        /// <returns></returns>
        private bool DIET_ORDER_INSERT(PsmhDb pDbCon, string strDIETCODE, string strDietName, string strSucode, string strDietDay, string strQTY, string strUnit, string strBun)
        {
            bool rtnVal = false;
            int intRowAffected = 0;
            string SqlErr = "";
            string SQL = "";

            try
            {
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NEWORDER(ACTDATE, PANO, BI, DEPTCODE, DRCODE, ";
                SQL = SQL + ComNum.VBLF + " WARDCODE, ROOMCODE, DIETCODE, DIETNAME, SUCODE, DIETDAY, ";
                SQL = SQL + ComNum.VBLF + " QTY, UNIT, BUN, ENTDATE, INPUTID, GBSUNAP, PRINT )";
                SQL = SQL + ComNum.VBLF + " VALUES (TO_DATE('" + FstrDietDate + "', 'YYYY-MM-DD') , '" + GstrPANO + "', '" + FstrBi + "',";
                SQL = SQL + ComNum.VBLF + "         '" + FstrDeptCode + "', '" + FstrDrCode + "', '" + GstrWardCode + "', '" + FstrRoomCode + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strDIETCODE + "' , '" + strDietName + "' ,'" + strSucode + "' , '" + strDietDay + "', '" + strQTY + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strUnit + "', '" + strBun + "', SYSDATE,";
                SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "', '" + FstrGbSunap + "','' ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //HISTORY

                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NEWORDER_HIS(ACTDATE, PANO, BI, DEPTCODE, DRCODE, ";
                SQL = SQL + ComNum.VBLF + " WARDCODE, ROOMCODE, DIETCODE, DIETNAME, SUCODE, DIETDAY, ";
                SQL = SQL + ComNum.VBLF + " QTY, UNIT, BUN, ENTDATE, INPUTID, GBSUNAP, PRINT )";
                SQL = SQL + ComNum.VBLF + " VALUES (TO_DATE('" + FstrDietDate + "', 'YYYY-MM-DD') , '" + GstrPANO + "', '" + FstrBi + "',";
                SQL = SQL + ComNum.VBLF + "         '" + FstrDeptCode + "', '" + FstrDrCode + "', '" + GstrWardCode + "', '" + FstrRoomCode + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strDIETCODE + "' , '" + strDietName + "' ,'" + strSucode + "' , '" + strDietDay + "', '" + strQTY + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strUnit + "', '" + strBun + "', SYSDATE,";
                SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "', '" + FstrGbSunap + "','' ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }



        private void btnSelect_Click(object sender, EventArgs e)
        {
            //Frm선택식등록
            frmSelectSeve f = new frmSelectSeve();
            f.ShowDialog();
        }

        private void btnSOrder_Click(object sender, EventArgs e)
        {
            if (btnSOrder.Text == "변경닫기")
            {
                panSorder.Visible = false;
                btnSOrder.Text = "변경보기";
            }
            else
            {
                panSorder.Visible = true;
                btnSOrder.Text = "변경닫기";
            }
        }

        private void ComboWard_Enter(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
        }

        private void btnBiGo1_Click(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            //Frm소독
            frmSoDok f = new frmSoDok();
            f.StartPosition = FormStartPosition.CenterParent;
            f.rSetHelpCode += F_rSetHelpCode;
            f.ShowDialog();
            f = null;

            if (GstrHelpCode != "YES")
            {
                return;
            }

            FnRow += 1;
            if (SSOrder_Sheet1.RowCount < FnRow)
            {
                SSOrder_Sheet1.RowCount = FnRow;
            }

            SSOrder_Sheet1.Cells[FnRow - 1, 1].Text = "20";
            //SSOrder_Sheet1.Cells[FnRow - 1, 2].Text = "소독(식사 후 식기는 병실에 두세요)";
            SSOrder_Sheet1.Cells[FnRow - 1, 2].Text = "격리식사(식사후 식기를 병실에 두세요)";
            SSOrder_Sheet1.Cells[FnRow - 1, 3].Text = "########";
            SSOrder_Sheet1.Cells[FnRow - 1, 4].Text = "1";
            SSOrder_Sheet1.Cells[FnRow - 1, 5].Text = "";
            SSOrder_Sheet1.Cells[FnRow - 1, 6].Text = "99";
            SSOrder_Sheet1.Cells[FnRow - 1, 7].Text = "";
        }

        private void F_rSetHelpCode(string strHelpCode)
        {
            GstrHelpCode = strHelpCode;
        }

        private void btnBiGo2_Click(object sender, EventArgs e)
        {
            FnRow += 1;
            if (SSOrder_Sheet1.RowCount < FnRow)
            {
                SSOrder_Sheet1.RowCount = FnRow;
            }

            SSOrder_Sheet1.Cells[FnRow - 1, 1].Text = "21";
            SSOrder_Sheet1.Cells[FnRow - 1, 2].Text = "익힌 것만";
            SSOrder_Sheet1.Cells[FnRow - 1, 3].Text = "########";
            SSOrder_Sheet1.Cells[FnRow - 1, 4].Text = "1";
            SSOrder_Sheet1.Cells[FnRow - 1, 5].Text = "";
            SSOrder_Sheet1.Cells[FnRow - 1, 6].Text = "99";
            SSOrder_Sheet1.Cells[FnRow - 1, 7].Text = "";
        }

        private void SS1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string strPano = "";
            string strChk = "";
            string strDate = "";
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            //int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (e.Column == 6)
                {
                    if (SS1_Sheet1.Cells[e.Row, 8].Text != "심사완료" || SS1_Sheet1.Cells[e.Row, 8].Text != "계산중" || SS1_Sheet1.Cells[e.Row, 8].Text != "퇴원")
                    {
                        strDate = TxtDietDate.Text.Trim();
                        strChk = Convert.ToBoolean(SS1_Sheet1.Cells[e.Row, 6].Value) == true ? "1" : "0";
                        strPano = SS1_Sheet1.Cells[e.Row, 9].Text.Trim();

                        SQL = "";
                        SQL = " UPDATE " + ComNum.DB_PMPA + "DIET_NEWORDER SET GBSUNAP = '" + strChk + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("재원중인 경우 체크를 할 수 없습니다.", "확인");
                        return;
                    }
                }
                else if (e.Column == 7)
                {
                    strDate = (TxtDietDate.Text).Trim();
                    strChk = Convert.ToBoolean(SS1_Sheet1.Cells[e.Row, 7].Value) == true ? "1" : "0";
                    strPano = SS1_Sheet1.Cells[e.Row, 9].Text.Trim();

                    //추가상 업데이트 아침 점심 저녁별로 컬럼 값 다르게 입력
                    if (GstrWardCode == "A1" || GstrWardCode == "A2" || GstrWardCode == "A3" || GstrWardCode == "A4")
                    {
                        if (GstrWardCode == "A1" || GstrWardCode == "A4")
                        {
                            SQL = "";
                            SQL = " UPDATE DIET_NEWORDER SET GBADD = '" + strChk + "' ";
                        }
                        else if (GstrWardCode == "A2")
                        {
                            SQL = "";
                            SQL = " UPDATE DIET_NEWORDER SET GBADD1 = '" + strChk + "' ";
                        }
                        else if (GstrWardCode == "A3")
                        {
                            SQL = "";
                            SQL = " UPDATE DIET_NEWORDER SET GBADD2 = '" + strChk + "' ";
                        }
                    }
                    else
                    {
                        if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "07:30") > 0
                            && string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "13:00") > 0)
                        {
                            SQL = "";
                            SQL = " UPDATE DIET_NEWORDER SET GBADD1 = '" + strChk + "' ";
                        }
                        else if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "13:00") > 0
                            && string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "17:30") > 0)
                        {
                            SQL = "";
                            SQL = " UPDATE DIET_NEWORDER SET GBADD2 = '" + strChk + "' ";
                        }
                        else
                        {
                            SQL = "";
                            SQL = " UPDATE DIET_NEWORDER SET GBADD = '" + strChk + "' ";
                        }
                    }

                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
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

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPano = "";
            string strIPDNO = "";
            string strInDate = "";
            string strFormNo = "";
            string strEMRNO = "";
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            //string strTemp = "";

            Cursor.Current = Cursors.WaitCursor;

            FstrCureFood = "";

            timer1.Enabled = false;

            if (GET_DIETPRT_LOCK() == true)
            {
                MessageBox.Show("영양실에서 식표인쇄 중입니다. 3분후에 작업하시기 바랍니다.", "확인");
                timer1.Enabled = true;
                return;
            }

            if (e.ColumnHeader == true || e.RowHeader == true)
            {
                timer1.Enabled = true;
                return;
            }

            strInDate = SS1_Sheet1.Cells[e.Row, 15].Text.Trim();
            clsSupDiet.dst.Sex = SS1_Sheet1.Cells[e.Row, 16].Text.Trim();

            if (e.Column == 0)
            {
                FstrDietPano = "";
                FstrDietPano = SS1_Sheet1.Cells[e.Row, 9].Text;

                frmHistory2 f = new frmHistory2(FstrDietPano, "1");
                f.ShowDialog();
                f = null;

                FstrDietPano = "";
                timer1.Enabled = true;
                return;
            }
            if (e.Column == 1)
            {
                FstrDietPano = "";

                FstrDietPano = SS1_Sheet1.Cells[e.Row, 9].Text;
                FstrDietPano = SS1_Sheet1.Cells[e.Row, 9].Text.Trim();
                GstrDietPano = FstrDietPano;

                frmHistory2 f2 = new frmHistory2(FstrDietPano, "1");
                f2.ShowDialog();
                f2 = null;

                FstrDietPano = "";
                GstrDietPano = "";
                timer1.Enabled = true;
                return;
            }

            try
            {
                if (e.Column == 2)
                {
                    strPano = SS1_Sheet1.Cells[e.Row, 9].Text.Trim();
                    strInDate = SS1_Sheet1.Cells[e.Row, 15].Text.Trim();

                    SQL = "";
                    SQL = " SELECT PANO, IPDNO, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + strInDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                    SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + strInDate + " 23:59','YYYY-MM-DD HH24:MI') ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        timer1.Enabled = true;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    if (strIPDNO != "")
                    {
                        //if (File.Exists(@"C:\cmc\ocsexe\careplan.exe") == false)
                        //{
                        //    Ftpedt FtpedtX = new Ftpedt();
                        //    FtpedtX.FtpDownload("192.168.100.33", "pcnfs", "pcnfs1", @"C:\cmc\ocsexe\careplan.exe", "careplan.exe", "/pcnfs/ocsexe");
                        //    FtpedtX = null;
                        //    ComFunc.MsgBox("Care Plan 설치 중입니다. 버튼을 다시 클릭하십시오", "확인");
                        //    timer1.Enabled = true;
                        //    return;
                        //}

                        //VB.Shell(@"C:\cmc\ocsexe\careplan.exe " + strPano + "|" + strInDate + "|" + strIPDNO + "|" + clsType.User.Sabun + " ");

                        //clsVbEmr.CarePlan_View(strPano, strInDate, strIPDNO, clsType.User.Sabun);

                        frmCarePlan frmCarePlanX = new frmCarePlan(strPano, strInDate, strIPDNO, clsType.User.Sabun);
                        frmCarePlanX.ShowDialog();
                        frmCarePlanX.Dispose();
                        frmCarePlanX = null;
                    }
                }

                if (e.Column != 3 && e.Column != 4 && e.Column != 5)
                {
                    timer1.Enabled = true;
                    return;
                }

                if (e.Column == 3)
                {
                    FstrDietDay = "1";
                    chkDietDay0.Checked = true;
                }
                if (e.Column == 4)
                {
                    FstrDietDay = "2";
                    chkDietDay1.Checked = true;
                }
                if (e.Column == 5)
                {
                    FstrDietDay = "3";
                    chkDietDay2.Checked = true;
                }

                //선택식일경우 체크
                if (SS1_Sheet1.Cells[e.Row, 0].BackColor == System.Drawing.Color.FromArgb(128, 255, 128))
                {
                    MessageBox.Show("선태식 환자입니다. 확인하세요", "확인");
                }

                //'식이마감시간 CHECK
                //'아침 1차마감 05:00
                //'점심 1차마감 10:00  2차마감 11:00
                //'저녁 1차마감 15:00  2차마감 16:00

                FbMagam = false;

                if (FstrDietDay == "1")
                {
                    if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "06:30") >= 0)
                    {
                        MessageBox.Show("지금 시간에는 식이변경이 불가능합니다");
                        if (FstrDietJikwon == false)
                        {
                            FbMagam = true;
                        }
                    }
                }
                if (FstrDietDay == "2")
                {
                    if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "11:30") >= 0)
                    {
                        MessageBox.Show("지금 시간에는 식이변경이 불가능합니다");
                        if (FstrDietJikwon == false)
                        {
                            FbMagam = true;
                        }
                    }
                }
                if (FstrDietDay == "3")
                {
                    if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "16:30") >= 0)
                    {
                        MessageBox.Show("지금 시간에는 식이변경이 불가능합니다");
                        if (FstrDietJikwon == false)
                        {
                            FbMagam = true;
                        }
                    }
                }

                if (VB.Right(ComboWard.Text.Trim(), 2) == "33" || VB.Right(ComboWard.Text.Trim(), 2) == "35")
                {
                    FstrRoomCode = VB.Left(SS1_Sheet1.Cells[e.Row, 0].Text, 3).Trim();
                }
                else
                {
                    FstrRoomCode = VB.Right(SS1_Sheet1.Cells[e.Row, 0].Text, 3).Trim();
                }
                FstrGbSunap = SS1_Sheet1.Cells[e.Row, 6].Text.Trim();
                GstrPANO = SS1_Sheet1.Cells[e.Row, 9].Text.Trim();
                FstrDeptCode = SS1_Sheet1.Cells[e.Row, 10].Text.Trim();
                FstrBi = SS1_Sheet1.Cells[e.Row, 11].Text.Trim();
                FstrSname = SS1_Sheet1.Cells[e.Row, 12].Text.Trim();
                FstrDrCode = SS1_Sheet1.Cells[e.Row, 13].Text.Trim();
                FNIPDNO = VB.Val(SS1_Sheet1.Cells[e.Row, 14].Text);
                FstrDietDate = TxtDietDate.Text;

                if (GstrWardCodes == "TOP" || FstrDietJikwon == true)
                {

                }
                else
                {
                    if (SS1_Sheet1.Cells[e.Row, 8].Text == "재원중" || SS1_Sheet1.Cells[e.Row, 8].Text == "퇴원등록" || SS1_Sheet1.Cells[e.Row, 8].Text == "부분심사")
                    {
                    }
                    else
                    {
                        MessageBox.Show("현재 환자는 [" + SS1_Sheet1.Cells[e.Row, 8].Text + "]상태입니다. " + ComNum.VBLF + " 식이오더를 입력할수 없습니다.", "확인");
                        timer1.Enabled = true;
                        return;
                    }
                }

                if (GstrPANO == "")
                {
                    MessageBox.Show("프로그램 오류입니다.종료후 다시작업해주세요" + ComNum.VBLF + " 식이오더를 입력할수 없습니다.", "확인");
                    timer1.Enabled = true;
                    return;
                }


                txtMemo.Text = "";
                txtMemoRowid.Text = "";

                ReadMemo();

                if (OptUpdate1.Checked == true)
                {
                    Read_Diet_Order(FstrDietDay, GstrPANO, FstrDietDate);
                }

                if (GstrWardCodes == "TOP" || FstrDietJikwon)
                {
                }
                else
                {
                    if (FstrDietDay == "1" && FstrCureFood == "OK" && string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "05:00") >= 0)
                    {
                        MessageBox.Show("지금은 치료식(경관식) 식이변경이 불가능합니다." + ComNum.VBLF + ComNum.VBLF + strMsg2);
                    }
                    if (FstrDietDay == "2" && FstrCureFood == "OK" && string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "10:00") >= 0)
                    {
                        MessageBox.Show("지금은 치료식(경관식) 식이변경이 불가능합니다." + ComNum.VBLF + ComNum.VBLF + strMsg2);
                    }
                    if (FstrDietDay == "3" && FstrCureFood == "OK" && string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "15:00") >= 0)
                    {
                        MessageBox.Show("지금은 치료식(경관식) 식이변경이 불가능합니다." + ComNum.VBLF + ComNum.VBLF + strMsg2);
                    }
                }

                SSOrder.Enabled = true;

                FnDietCol = e.Column;
                FnDietROW = e.Row;

                SS_BackColor(SS1_Sheet1, 0, 128, 255);

                panel4.Enabled = true;
                panel5.Enabled = true;

                SS1.Enabled = false;
                btnRefresh.Enabled = false;

                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                panUpdate.Enabled = false;

                SQL = "";
                SQL = " SELECT WEIGHT, HEIGHT  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + FNIPDNO + " ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    timer1.Enabled = true;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    TxtHeight.Text = dt.Rows[0]["HEIGHT"].ToString().Trim();
                    TxtWeight.Text = dt.Rows[0]["WEIGHT"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                if (TxtHeight.Text.Trim() == "0" || TxtWeight.Text.Trim() == "0" || TxtHeight.Text.Trim() == "" || TxtWeight.Text.Trim() == "")
                {
                    SQL = "";
                    SQL = " SELECT EMRNO, FORMNO";
                    SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_EMR.EMRXMLMST ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + GstrPANO + "'";
                    SQL = SQL + ComNum.VBLF + "      AND CHARTDATE >= '" + strInDate.Replace("-", "") + "'";
                    SQL = SQL + ComNum.VBLF + "      AND CHARTDATE <= '" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-").Replace("-", "") + "'";
                    SQL = SQL + ComNum.VBLF + "      AND FORMNO IN ('1545','1553','1554','1556','1558','2285','2294','2295','2305','2311','2356')";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        timer1.Enabled = true;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strEMRNO = dt.Rows[0]["EMRNO"].ToString().Trim();
                        strFormNo = dt.Rows[0]["FORMNO"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    if (strEMRNO != "" && strFormNo != "")
                    {
                        SQL = "";
                        switch (strFormNo)
                        {
                            case "1545":
                            case "1553":
                                SQL = " SELECT extractValue(chartxml, '//it14') HEIGHT, extractValue(chartxml, '//it15') WEIGHT";
                                break;
                            case "1554":
                                SQL = " SELECT extractValue(chartxml, '//it47') HEIGHT, extractValue(chartxml, '//it48') WEIGHT";
                                break;
                            case "1556":
                                SQL = " SELECT extractValue(chartxml, '//it2') HEIGHT, extractValue(chartxml, '//it1') WEIGHT";
                                break;
                            case "1558":
                                SQL = " SELECT extractValue(chartxml, '//it13') HEIGHT, extractValue(chartxml, '//it14') WEIGHT";
                                break;
                            case "2285":
                                SQL = " SELECT extractValue(chartxml, '//it1271') HEIGHT, extractValue(chartxml, '//it83') WEIGHT";
                                break;
                            case "2294":
                                SQL = " SELECT extractValue(chartxml, '//it51') HEIGHT, extractValue(chartxml, '//it52') WEIGHT";
                                break;
                            case "2295":
                                SQL = " SELECT extractValue(chartxml, '//it96') HEIGHT, extractValue(chartxml, '//it45') WEIGHT";
                                break;
                            case "2305":
                                SQL = " SELECT extractValue(chartxml, '//it51') HEIGHT, extractValue(chartxml, '//it49') WEIGHT";
                                break;
                            case "2311":
                                SQL = " SELECT extractValue(chartxml, '//it45') HEIGHT, extractValue(chartxml, '//it46') WEIGHT";
                                break;
                            case "2356":
                                SQL = " SELECT extractValue(chartxml, '//it50') HEIGHT, extractValue(chartxml, '//it49') WEIGHT";
                                break;
                        }
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                        SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEMRNO;

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            timer1.Enabled = true;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            if (VB.IsNumeric(dt.Rows[0]["HEIGHT"].ToString().Trim()))
                            {
                                TxtHeight.Text = dt.Rows[0]["HEIGHT"].ToString().Trim();
                            }
                            else
                            {
                                TxtHeight.Text = "0";
                            }
                            if (VB.IsNumeric(dt.Rows[0]["WEIGHT"].ToString().Trim()))
                            {
                                TxtWeight.Text = dt.Rows[0]["WEIGHT"].ToString().Trim();
                            }
                            else
                            {
                                TxtWeight.Text = "0";
                            }
                        }

                        dt.Dispose();
                        dt = null;
                    }
                    else
                    {
                        //신규기록지에서 값을 읽어 표시한다
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT ";
                        SQL = SQL + ComNum.VBLF + "   (SELECT R.ITEMVALUE ";
                        SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRCHARTROW R ";
                        SQL = SQL + ComNum.VBLF + "    WHERE R.EMRNO =  ";
                        SQL = SQL + ComNum.VBLF + "                (SELECT  ";
                        SQL = SQL + ComNum.VBLF + "                    MAX(A.EMRNO) ";
                        SQL = SQL + ComNum.VBLF + "                FROM  KOSMOS_EMR.AEMRCHARTMST A ";
                        SQL = SQL + ComNum.VBLF + "                INNER JOIN KOSMOS_EMR.AEMRCHARTROW B ";
                        SQL = SQL + ComNum.VBLF + "                    ON A.EMRNO = B.EMRNO ";
                        SQL = SQL + ComNum.VBLF + "                    AND A.EMRNOHIS = B.EMRNOHIS ";
                        SQL = SQL + ComNum.VBLF + "                    AND B.ITEMNO  = 'I0000000002' ";
                        SQL = SQL + ComNum.VBLF + "                    AND B.ITEMVALUE IS NOT NULL ";
                        SQL = SQL + ComNum.VBLF + "                WHERE A.PTNO = '" + GstrPANO + "' ";
                        SQL = SQL + ComNum.VBLF + "                    AND A.FORMNO IN(2294, 2295, 2311, 2356, 3150) ";
                        SQL = SQL + ComNum.VBLF + "                    AND A.CHARTDATE >= '" + strInDate.Replace("-", "") + "' ";
                        SQL = SQL + ComNum.VBLF + "                    AND A.CHARTDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') ";
                        SQL = SQL + ComNum.VBLF + "                ) ";
                        SQL = SQL + ComNum.VBLF + "         AND R.ITEMNO  = 'I0000000002' ";
                        SQL = SQL + ComNum.VBLF + "    ) AS HEIGHT, ";
                        SQL = SQL + ComNum.VBLF + "    (SELECT R.ITEMVALUE ";
                        SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRCHARTROW R ";
                        SQL = SQL + ComNum.VBLF + "    WHERE R.EMRNO =  ";
                        SQL = SQL + ComNum.VBLF + "                (SELECT  ";
                        SQL = SQL + ComNum.VBLF + "                    MAX(A.EMRNO) ";
                        SQL = SQL + ComNum.VBLF + "                FROM  KOSMOS_EMR.AEMRCHARTMST A ";
                        SQL = SQL + ComNum.VBLF + "                INNER JOIN KOSMOS_EMR.AEMRCHARTROW B ";
                        SQL = SQL + ComNum.VBLF + "                    ON A.EMRNO = B.EMRNO ";
                        SQL = SQL + ComNum.VBLF + "                    AND A.EMRNOHIS = B.EMRNOHIS ";
                        SQL = SQL + ComNum.VBLF + "                    AND B.ITEMNO  = 'I0000000418' ";
                        SQL = SQL + ComNum.VBLF + "                    AND B.ITEMVALUE IS NOT NULL ";
                        SQL = SQL + ComNum.VBLF + "                WHERE A.PTNO = '" + GstrPANO + "' ";
                        SQL = SQL + ComNum.VBLF + "                    AND A.FORMNO IN(2294, 2295, 2311, 2356, 3150) ";
                        SQL = SQL + ComNum.VBLF + "                    AND A.CHARTDATE >= '" + strInDate.Replace("-", "") + "' ";
                        SQL = SQL + ComNum.VBLF + "                    AND A.CHARTDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') ";
                        SQL = SQL + ComNum.VBLF + "                ) ";
                        SQL = SQL + ComNum.VBLF + "         AND R.ITEMNO  = 'I0000000418' ";
                        SQL = SQL + ComNum.VBLF + "    ) AS WEIGHT ";
                        SQL = SQL + ComNum.VBLF + "FROM DUAL ";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            TxtHeight.Text = dt.Rows[0]["HEIGHT"].ToString().Trim();
                            TxtWeight.Text = dt.Rows[0]["WEIGHT"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }

                if (clsSupDiet.GET_Kcal(TxtHeight.Text, TxtWeight.Text, clsSupDiet.dst.Sex) != "")
                {
                    lbllKcal.Text = "해당환자의 권장칼로리는 " + clsSupDiet.GET_Kcal(TxtHeight.Text, TxtWeight.Text, clsSupDiet.dst.Sex) + "Kcal 입니다.";
                }
                else
                {
                    lbllKcal.Text = "";
                }

                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SSBIGO_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            string strOK = "";
            string strCode = "";
            //string strSalt = "";

            strCode = SSBIGO_Sheet1.Cells[e.Row, 0].Text;

            strOK = "NO";

            if (VB.Trim(strCode) == "다져서") 
            {
                for (i = 1; i <= SSOrder_Sheet1.RowCount; i++)
                {
                    if (SSOrder_Sheet1.Cells[i - 1, 6].Text == "02" || SSOrder_Sheet1.Cells[i - 1, 6].Text == "03")
                    {
                        strOK = "OK";
                        break;
                    }
                }
            }
            else
            {
                for (i = 1; i <= SSOrder_Sheet1.RowCount; i++)
                {
                    if (SSOrder_Sheet1.Cells[i - 1, 6].Text == "01" || VB.Left(SSOrder_Sheet1.Cells[i-1, 2].Text, 4) == "연하곤란" )
                    {
                        strOK = "OK";
                        break;
                    }
                }
            }

            if (strOK == "NO" && VB.Trim(strCode) == "다져서")
            {
                MessageBox.Show("다져서는 일반치료, 저염치료식만 입력 할 수 있습니다.");
                return;
            }


            if (strOK == "NO")
            {
                MessageBox.Show("정규식이 입력 후 '비고'란의 식이를 입력 할 수 있습니다");
                return;
            }

            lblImfor.Text = SSBIGO_Sheet1.Cells[e.Row, 3].Text;
        }

        private void SSBIGO_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            string strBigo = "";
            string strCODE = "";

            if (SSBIGO_Sheet1.RowCount == 0)
            {
                return;
            }
            if (FbMagam == true)
            {
                return;
            }

            for (i = 1; i <= SSOrder_Sheet1.RowCount; i++)
            {
                if (SSOrder_Sheet1.Cells[i - 1, 3].Text == "FD020" || SSOrder_Sheet1.Cells[i - 1, 3].Text == "FD020A" || SSOrder_Sheet1.Cells[i - 1, 3].Text == "FD020D")
                {
                    MessageBox.Show("식이가 '금식', '금식(SIPS OF WATER)' 일 경우 비고항목 추가는 안됩니다.");
                    return;
                }
            }

            ///자체적으로 내부 함수에서 막음
            //NOT_INPUT_CHECK_CLICK

            if (SSBIGO_Sheet1.Cells[e.Row, 2].Text == "20")
            {
                GstrHelpCode = "";

                //Frm소독
                frmSoDok f = new frmSoDok();
                f.rSetHelpCode += F_rSetHelpCode1;
                f.ShowDialog();
                f = null;

                if (GstrHelpCode != "YES")
                {
                    return;
                }
            }

            strBigo = SSBIGO_Sheet1.Cells[e.Row, 0].Text;
            strCODE = SSBIGO_Sheet1.Cells[e.Row, 2].Text;

            FnRow = FnRow + 1;
            if (SSOrder_Sheet1.RowCount < FnRow)
            {
                SSOrder_Sheet1.RowCount = FnRow;
            }
            SSOrder_Sheet1.Cells[FnRow - 1, 1].Text = strCODE;
            SSOrder_Sheet1.Cells[FnRow - 1, 2].Text = (strBigo).Trim();
            SSOrder_Sheet1.Cells[FnRow - 1, 3].Text = "########";
            SSOrder_Sheet1.Cells[FnRow - 1, 4].Text = "1";
            SSOrder_Sheet1.Cells[FnRow - 1, 5].Text = "";
            SSOrder_Sheet1.Cells[FnRow - 1, 6].Text = "99";
            SSOrder_Sheet1.Cells[FnRow - 1, 7].Text = "";

        }

        private void F_rSetHelpCode1(string strHelpCode)
        {
            GstrHelpCode = strHelpCode;
        }

        private void SSChk_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string strPano = "";
            string strBDATE = "";
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            //bool rtnVal = false;
            //int i = 0;

            if (e.Column != 2)
            {
                return;
            }

            if (SSChk_Sheet1.Cells[e.Row, 0].Text == "")
            {
                return;
            }

            strPano = SSChk_Sheet1.Cells[e.Row, 3].Text.PadLeft(8, '0');
            strBDATE = SSChk_Sheet1.Cells[e.Row, 4].Text;


            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "DIET_CONSULT SET ";
                SQL = SQL + ComNum.VBLF + " CHKVALID = 'Y' ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRUNC(BDATE) = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }


                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);

                READ_CONSULT();
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

        private void SSCode1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            lblImfor.Text = SSCode1_Sheet1.Cells[e.Row, 3].Text; 
        }

        private void SSCode1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            //int j = 0;

            if (SSCode1_Sheet1.RowCount == 0)
            {
                return;
            }

            if (FbMagam == true)
            {
                switch (SSCode1_Sheet1.Cells[e.Row, 2].Text)
                {
                    case "29":
                    case "43":
                        return;
                }
            }

            //MSG_치료식마감
            MSG_TreatmentMeal(FstrDietDay, SSCode1_Sheet1.Cells[e.Row, 2].Text, "01");

            if (FstrDietJikwon == false)
            {
                if (MSG_TreatmentMeal_Last(FstrDietDay, SSCode1_Sheet1.Cells[e.Row, 2].Text, "01") == true)
                {
                    return;
                }
            }

            //저장시점에서만 체크하는것으로 함 함수내부에서 막음.
            //if (NOT_INPUT_CHECK_CLICK())
            //{
            //    return;
            //}

            if (FstrDietJikwon == false)
            {
                switch (SSCode1_Sheet1.Cells[e.Row, 2].Text)
                {
                    case "29":
                    case "43":
                    case "30":
                        if (READ_SORDER(FstrDietDay, GstrPANO, TxtDietDate.Text.Trim()) == true)
                        {
                            MessageBox.Show("선택식이 입력되어 있을경우 '금식'과 '사식'은 입력이 불가능합니다.");
                            if (FstrDietJikwon == false)
                            {
                                return;
                            }
                        }
                        break;
                }
            }

            switch (SSCode1_Sheet1.Cells[e.Row, 2].Text)
            {
                case "29":
                case "43":

                    for (i = 1; i <= SSOrder_Sheet1.RowCount; i++)
                    {
                        if (SSOrder_Sheet1.Cells[i - 1, 3].Text == "########")
                        {
                            MessageBox.Show("정규식이 '금식', '금식(SIPS OF WATER)' 일 경우 비고항목 추가는 안됩니다.");
                            SSOrder_Sheet1.Cells[i - 1, 0].Value = true;
                        }
                    }
                    break;
            }

            if (SSCode1_Sheet1.Cells[e.Row, 2].Text == "28")
            {
                MessageBox.Show("영양팀으로 필히 연락 주세요");
            }

            //정규식이
            Insert_Diet_Order(SSCode1_Sheet1, e.Row, "01");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// alse 일 때 저장 가능
        /// 저장 시점에 사식,금식,외출 체크
        /// </returns>
        private bool NOT_INPUT_CHECK_SAVE()
        {
            bool rtnVal = false;
            string strBun = "";
            string strCODE = "";
            string strSucode = "";
            string strGubun = "";
            string strCheck = "";
            string strNOT = "";
            int nCnt = 0;
            int i = 0;

            rtnVal = false;
            strNOT = "";
            nCnt = 0;

            for (i = 1; i <= SSOrder_Sheet1.RowCount; i++)
            {
                strCheck = SSOrder_Sheet1.Cells[i - 1, 0].Text.Trim();
                strCODE = SSOrder_Sheet1.Cells[i - 1, 1].Text.Trim();
                strSucode = SSOrder_Sheet1.Cells[i - 1, 3].Text.Trim();
                strGubun = SSOrder_Sheet1.Cells[i - 1, 7].Text.Trim();

                //보호자식은 예외  
                //추가 예외 수가 발생시 코드화 작업 예정

                switch (strSucode)
                {
                    case "F01E":
                    case "F01D":
                        break;
                    default:
                        if (strCODE != "" && strCheck != "1")
                        {
                            nCnt += 1;
                        }
                        break;
                }

                strBun = SSOrder_Sheet1.Cells[i - 1, 6].Text.Trim();

                if (strBun == "01")
                {
                    switch (strCODE)
                    {
                        case "46":
                        case "30":
                        case "29":
                        case "43":
                            if (Convert.ToBoolean(SSOrder_Sheet1.Cells[i - 1, 0].Value) == false)
                            {
                                strNOT = "OK";
                            }
                            break;
                    }
                }

                if (nCnt > 1 && strNOT == "OK")
                {
                    MessageBox.Show("정규식이 '금식', '금식(SIPS OF WATER)', '외출', 사식' 일 경우 식이 추가는 안됩니다. 식이를 확인하십시요.");
                    rtnVal = false;
                }
            }

            return rtnVal;
        }

        private void SSCode2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            lblImfor.Text = SSCode2_Sheet1.Cells[e.Row, 3].Text;
        }

        private void SSCode2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SSCode2_Sheet1.RowCount == 0)
            {
                return;
            }

            if (FbMagam == true)
            {
                return;
            }

            if (TreatmentMeal_Overlap() == true)
            {
                MessageBox.Show("치료식 중복입력은 제한되어 있습니다.");
                return;
            }

            //자체적으로 함수내부 막음
            //If NOT_INPUT_CHECK_CLICK Then Exit Sub

            if (MSG_TreatmentMeal(FstrDietDay, SSCode2.Text, "02") == true)
            {
                return;
            }

            Insert_Diet_Order(SSCode2_Sheet1, e.Row, "02");
        }

        private void SSCode3_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            lblImfor.Text = SSCode3_Sheet1.Cells[e.Row, 3].Text;
        }

        private void SSCode3_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SSCode3_Sheet1.RowCount == 0)
            {
                return;
            }

            if (FbMagam == true)
            {
                return;
            }

            if (TreatmentMeal_Overlap() == true)
            {
                MessageBox.Show("치료식 중복입력은 제한되어 있습니다.");
                return;
            }

            //자체적으로 함수내부 막음
            //If NOT_INPUT_CHECK_CLICK Then Exit Sub


            //'   2011년 1월 24일 김현욱 추가
            //'   식이등록 창에서 `일반치료` 중 <고단백식>의 경우
            //'   경관급식과 함께 입력하지 않을 시 입력이 되지 않도록 부탁드립니다.

            if (READ_ORAL_DIET(SSCode3_Sheet1.Cells[e.Row, 2].Text) == false)
            {
                MessageBox.Show("고단백식은 정규식이 '경관급식'가 포함되어야 선택 가능합니다.");
                return;
            }

            if (MSG_TreatmentMeal(FstrDietDay, SSCode3_Sheet1.Cells[e.Row, 2].Text, "03") == true)
            {
                return;
            }

            Insert_Diet_Order(SSCode3_Sheet1, e.Row, "03");
        }

        private void SSCode4_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            lblImfor.Text = SSCode4_Sheet1.Cells[e.Row, 3].Text;
        }

        private void SSCode4_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            string strDIETCODE = "";

            if (SSCode4_Sheet1.RowCount == 0)
            {
                return;
            }

            if (FbMagam == true)
            {
                return;
            }

            strDIETCODE = SSCode4_Sheet1.Cells[e.Row, 2].Text.Trim();


            //자체적으로 함수내부에서 막음
            //    If NOT_INPUT_CHECK_CLICK Then Exit Sub

            if (strDIETCODE == "11")
            {
                for (i = 1; i <= SSOrder_Sheet1.RowCount; i++)
                {
                    switch (SSOrder_Sheet1.Cells[i - 1, 3].Text)
                    {
                        case "FD020":
                        case "FD020A":
                        case "FD021":
                        case "FD020D":
                            MessageBox.Show("환자가 금식 또는 사식일 경우" + ComNum.VBLF + "[보호자+공추로 입력 바랍니다]");
                            return;
                    }
                }
            }

            Insert_Diet_Order(SSCode4_Sheet1, e.Row, "04"); //추가식이
        }

        private void SSListMemo_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            int intRowAffected = 0;
            //bool rtnVal = false;
            string SqlErr = "";
            string strCERT1 = "";
            string strCERT2 = "";
            string strCERT3 = "";
            string strCERT4 = "";
            string strCERT5 = "";
            string strMemo = "";
            string strROWID = "";
            Cursor.Current = Cursors.WaitCursor;

            if (e.Column != 1)
            {
                return;
            }
            if (e.Row < 0)
            {
                return;
            }

            strROWID = SSListMemo_Sheet1.Cells[e.Row, 2].Text.Trim();
            strMemo = SSListMemo_Sheet1.Cells[e.Row, 3].Text.Trim();

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT DIETCERT1, DIETCERT2, DIETCERT3, DIETCERT4, DIETCERT5, ROWID  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DIET_MEMO MST";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "' ";
                SQL = SQL + ComNum.VBLF + "       AND NOT EXISTS";
                SQL = SQL + ComNum.VBLF + "       ( SELECT * FROM KOSMOS_PMPA.DIET_MEMO SUB";
                SQL = SQL + ComNum.VBLF + "       WHERE MST.ROWID = SUB.ROWID";
                SQL = SQL + ComNum.VBLF + "       AND (DIETCERT1 = " + clsType.User.Sabun + " OR DIETCERT2 = " + clsType.User.Sabun + " OR DIETCERT3 = " + clsType.User.Sabun + " OR DIETCERT4 = " + clsType.User.Sabun + " OR DIETCERT5 = " + clsType.User.Sabun + "))";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strCERT1 = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DIETCERT1"].ToString().Trim());
                    strCERT2 = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DIETCERT2"].ToString().Trim());
                    strCERT3 = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DIETCERT3"].ToString().Trim());
                    strCERT4 = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DIETCERT4"].ToString().Trim());
                    strCERT5 = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DIETCERT5"].ToString().Trim());

                    if (strCERT1 != "" && strCERT2 != "" && strCERT3 != "" && strCERT4 != "" && strCERT5 != "")
                    {
                    }
                    else
                    {
                        SQL = "";
                        SQL = " UPDATE KOSMOS_PMPA.DIET_MEMO SET";
                        if (strCERT1 == "")
                        {
                            SQL = SQL + ComNum.VBLF + " DIETCERT1 = " + clsType.User.Sabun + ", ";
                        }
                        else if (strCERT2 == "")
                        {
                            SQL = SQL + ComNum.VBLF + " DIETCERT2 = " + clsType.User.Sabun + ", ";
                        }
                        else if (strCERT3 == "")
                        {
                            SQL = SQL + ComNum.VBLF + " DIETCERT3 = " + clsType.User.Sabun + ", ";
                        }
                        else if (strCERT4 == "")
                        {
                            SQL = SQL + ComNum.VBLF + " DIETCERT4 = " + clsType.User.Sabun + ", ";
                        }
                        else if (strCERT5 == "")
                        {
                            SQL = SQL + ComNum.VBLF + " DIETCERT5 = " + clsType.User.Sabun + ", ";
                        }
                        SQL = SQL + ComNum.VBLF + " DIETCERT = '1' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //메모확인
                if (ComFunc.MsgBoxQ(strMemo, "확인", MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "DIET_MEMO SET";
                    SQL = SQL + ComNum.VBLF + " GUBUN1 = 'Y' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_MEMO_HIS ";
                    SQL = SQL + ComNum.VBLF + " SELECT * FROM " + ComNum.DB_PMPA + "DIET_MEMO";
                    SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = " DELETE " + ComNum.DB_PMPA + "DIET_MEMO";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SSListMemo_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void SSOrder_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            int i = 0;
            int j = 0;
            int nCnt = 0;
            //int nBigoI = 0;
            //int nBigoJ = 0;
            string strBigo1 = "";
            string strBigo2 = "";
            string strDiet = "";

            if (e.Column != 0)
            {
                return;
            }

            if (FbMagam == true && FstrDietJikwon == false)
            {
                SSOrder_Sheet1.Cells[e.Row, 0].Value = false;

                return;
            }

            if (Convert.ToBoolean(SSOrder_Sheet1.Cells[e.Row, 0].Value) == false)
            {

                for (i = 1; i <= SSOrder_Sheet1.RowCount; i++)
                {
                    if (SSOrder_Sheet1.Cells[i - 1, 3].Text.Trim() == "FD020" || SSOrder_Sheet1.Cells[i - 1, 3].Text.Trim() == "FD020A" || SSOrder_Sheet1.Cells[i - 1, 3].Text.Trim() == "FD020D")
                    {
                        for (j = 1; i <= SSOrder_Sheet1.RowCount; j++)
                        {
                            strBigo1 = SSOrder_Sheet1.Cells[j - 1, 3].Text.Trim();
                            strBigo1 = Convert.ToBoolean(SSOrder_Sheet1.Cells[j - 1, 0].Value) == true ? "1" : "0";

                            if (strBigo1 == "########" && strBigo2 == "0")
                            {
                                MessageBox.Show("식이가 '금식', '금식(SIPS OF WATER)' 일 경우 비고항목 추가는 안됩니다.");
                                SSOrder_Sheet1.Cells[j - 1, 0].Value = true;
                            }
                        }
                    }
                }
            }

            if (Convert.ToBoolean(SSOrder_Sheet1.Cells[e.Row, 0].Value) != true)
            {
                return;
            }

            if (SSOrder_Sheet1.Cells[e.Row, 6].Text == "01")
            {
                for (i = 1; i <= SSOrder_Sheet1.RowCount; i++)
                {
                    strDiet = SSOrder_Sheet1.Cells[i - 1, 6].Text;

                    if (Convert.ToBoolean(SSOrder_Sheet1.Cells[i - 1, 0].Value) == true && strDiet == "01")
                    {
                        nCnt += 1;
                    }
                }

                if (nCnt < 1)
                {
                    MessageBox.Show("정규식은 1개 이상 포함되어야 합니다.", "삭제 불가능");

                    for (i = 1; i <= SSOrder_Sheet1.RowCount; i++)
                    {
                        SSOrder_Sheet1.Cells[i - 1, 0].Value = false;
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (fnCnt == 5)
            {
                if (SS1.Enabled == true)
                {
                    Exit_TreatmentMeal_Select();
                    MessageView();
                }

                fnCnt = 0;
            }
            else
            {
                fnCnt++;
            }

            lblTime.Text = VB.Left(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"), 16);
        }

        /// <summary>
        /// 퇴원식이_Select
        /// </summary>
        private void Exit_TreatmentMeal_Select()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strWard = "";

            Cursor.Current = Cursors.WaitCursor;

            strWard = VB.Right(ComboWard.Text.Trim(), 2);

            try
            {
                SQL = "";
                SQL = " SELECT A.PANO ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "DIET_NEWORDER B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.GBSTS <> '0' ";
                SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE = TRUNC(SYSDATE) ";

                if (FstrDietJikwon == false)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE = '" + strWard + "' ";
                }
                SQL = SQL + ComNum.VBLF + "   AND B.ACTDATE = TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE = TO_DATE('" + TxtDietDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND (GBSUNAP IS NULL OR GBSUNAP = '' OR GBSUNAP = '0') ";

                if (strWard == "IU")
                {
                    if (VB.Left(ComboWard.Text.Trim(), 2) == "내과")
                    {
                        SQL = SQL + ComNum.VBLF + " AND A.ROOMCODE = '234' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND A.ROOMCODE = '233' ";
                    }
                }

                SQL = SQL + ComNum.VBLF + " GROUP BY A.PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.RowCount = 0;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        READ_IPD_OUT(dt.Rows[i]["PANO"].ToString().Trim());
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void MessageView()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            panMEMO.Visible = false;

            try
            {
                //switch (clsType.User.Sabun)
                switch(clsType.User.JobGroup)
                {
                    //case "04349":
                    //case "20433":
                    //case "20442":
                    //case "20193":
                    //case "04444":
                    case "JOB018003":

                        SQL = "";
                        SQL = " SELECT  B.WARDCODE, B.ROOMCODE, B.SNAME, A.WRITEDATE, A.WRITESABUN, DIET1, DIET2, DIET3, A.ROWID ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_MEMO A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
                        SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = B.IPDNO ";
                        SQL = SQL + ComNum.VBLF + "   AND A.DIETCERT IS NULL ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            ssMessage_Sheet1.RowCount = dt.Rows.Count;
                            panMEMO.Visible = true;

                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                ssMessage_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WARDCODE"].ToString().Trim() + "/" + dt.Rows[i]["ROOMCODE"].ToString().Trim();
                                ssMessage_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                                ssMessage_Sheet1.Cells[i, 2].Text = dt.Rows[i]["WRITEDATE"].ToString().Trim();
                                ssMessage_Sheet1.Cells[i, 3].Text = "※아침:" + dt.Rows[i]["DIET1"].ToString().Trim() + "  ※점심:" + dt.Rows[i]["DIET2"].ToString().Trim() + "  ※저녁:" + dt.Rows[i]["DIET3"].ToString().Trim();
                                ssMessage_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                            }
                        }
                        dt.Dispose();
                        dt = null;
                        break;

                    default:
                        panMEMO.Visible = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_MEMO_LIST_New()
        {

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strMemo = "";
            string StrTemp = "";
            string strTEMP1 = "";
            string strTemp2 = "";
            string strTEMP3 = "";

            Cursor.Current = Cursors.WaitCursor;

            btnMemoDel.Visible = true;

            ssMemo_Sheet1.Rows.Count = 0;
            ssMemo_Sheet1.SetColumnWidth(0, 40);
            ssMemo_Sheet1.SetColumnWidth(1, 40);
            ssMemo_Sheet1.SetColumnWidth(2, 55);
            ssMemo_Sheet1.SetColumnWidth(3, 285);
            ssMemo_Sheet1.SetColumnWidth(4, 40);

            try
            {
                SQL = "";
                SQL = " SELECT A.WARDCODE, A.ROOMCODE, A.PANO, A.SNAME, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(B.WRITEDATE,'YYYY-MM-DD HH24:MI') WRITEDATE, B.WRITESABUN, B.ROWID, ";
                SQL = SQL + ComNum.VBLF + " DIET1, DIET2, DIET3, MEMO, DIETCERT1, DIETCERT2, DIETCERT3, DIETCERT4, DIETCERT5 ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "DIET_MEMO B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = B.IPDNO";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND B.GUBUN1 IS NULL ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY A.WARDCODE, A.ROOMCODE, A.PANO, B.WRITEDATE ASC ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssMemo_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssMemo_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssMemo_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssMemo_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                        StrTemp = dt.Rows[i]["MEMO"].ToString().Trim();
                        strTEMP1 = dt.Rows[i]["DIET1"].ToString().Trim();
                        strTemp2 = dt.Rows[i]["DIET2"].ToString().Trim();
                        strTEMP3 = dt.Rows[i]["DIET3"].ToString().Trim();

                        //strMemo = "※ 메모내용 : " + StrTemp;
                        strMemo = " " + StrTemp;
                        if (strTEMP1 != "" || strTemp2 != "" || strTEMP3 != "")
                        {
                            strMemo = strMemo + ComNum.VBLF + "※ 경관급식 ※" + ComNum.VBLF;
                            strMemo = strMemo + ComNum.VBLF + "※아침:" + strTEMP1 + "  ※점심:" + strTemp2 + "  ※저녁:" + strTEMP3;
                            strMemo = strMemo + ComNum.VBLF + "  메모삭제시 확인 버튼클릭 하세요.";
                        }
                        ssMemo_Sheet1.Cells[i, 3].Text = strMemo;

                        //if ( dt.Rows[i]["DIETCERT1"].ToString().Trim() == clsType.User.Sabun ||
                        //     dt.Rows[i]["DIETCERT2"].ToString().Trim() == clsType.User.Sabun ||
                        //     dt.Rows[i]["DIETCERT3"].ToString().Trim() == clsType.User.Sabun ||
                        //     dt.Rows[i]["DIETCERT4"].ToString().Trim() == clsType.User.Sabun ||
                        //     dt.Rows[i]["DIETCERT5"].ToString().Trim() == clsType.User.Sabun )
                        //한명이라도 확인 하면 더 확인 할 필요 없도록 요청(지연쌤 2020-02-12 15:30 통화)
                        if ( dt.Rows[i]["DIETCERT1"].ToString().Trim() != "" ||
                             dt.Rows[i]["DIETCERT2"].ToString().Trim() != "" ||
                             dt.Rows[i]["DIETCERT3"].ToString().Trim() != "" ||
                             dt.Rows[i]["DIETCERT4"].ToString().Trim() != "" ||
                             dt.Rows[i]["DIETCERT5"].ToString().Trim() != "" )
                        {
                            ssMemo_Sheet1.Cells[i, 4].Text = "True";
                        }
                        else
                        {
                            ssMemo_Sheet1.Cells[i, 4].Text = "False";
                        }
                        ssMemo_Sheet1.SetRowHeight(i, 30);

                        ssMemo_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_MEMO_LIST()
        {

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strMemo = "";
            string StrTemp = "";
            string strTEMP1 = "";
            string strTemp2 = "";
            string strTEMP3 = "";

            Cursor.Current = Cursors.WaitCursor;

            SSListMemo_Sheet1.Rows.Count = 0;

            try
            {
                SQL = "";
                SQL = " SELECT A.WARDCODE, A.ROOMCODE, A.PANO, A.SNAME, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(B.WRITEDATE,'YYYY-MM-DD HH24:MI') WRITEDATE, B.WRITESABUN, B.ROWID, ";
                SQL = SQL + ComNum.VBLF + " DIET1, DIET2, DIET3, MEMO ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "DIET_MEMO B ";
                SQL = SQL + ComNum.VBLF + " WHERE (DIETCERT1 IS NULL OR DIETCERT2 IS NULL OR DIETCERT3 IS NULL OR DIETCERT4 IS NULL OR DIETCERT5 IS NULL) ";
                SQL = SQL + ComNum.VBLF + "     AND A.IPDNO = B.IPDNO";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND B.GUBUN1 IS NULL ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY A.WARDCODE, A.ROOMCODE, A.PANO, B.WRITEDATE ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SSListMemo_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SSListMemo_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim() + "/" + dt.Rows[i]["SNAME"].ToString().Trim();
                        SSListMemo_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        StrTemp = dt.Rows[i]["MEMO"].ToString().Trim();
                        strTEMP1 = dt.Rows[i]["DIET1"].ToString().Trim();
                        strTemp2 = dt.Rows[i]["DIET2"].ToString().Trim();
                        strTEMP3 = dt.Rows[i]["DIET3"].ToString().Trim();

                        strMemo = "※ 메모내용 : " + StrTemp;
                        if (strTEMP1 != "" || strTemp2 != "" || strTEMP3 != "")
                        {
                            strMemo = strMemo + ComNum.VBLF + "※ 경관급식 ※" + ComNum.VBLF;
                            strMemo = strMemo + ComNum.VBLF + "※아침:" + strTEMP1 + "  ※점심:" + strTemp2 + "  ※저녁:" + strTEMP3;
                            strMemo = strMemo + ComNum.VBLF + "  메모삭제시 확인 버튼클릭 하세요.";
                        }
                        SSListMemo_Sheet1.Cells[i, 3].Text = strMemo;
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_CHANGE_SORDER()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dtSub = null;
            string SqlErr = "";
            string strComp1 = "";
            string strComp2 = "";
            string strComp3 = "";
            string strROOMCODE = "";
            string strSName = "";

            ComFunc CF = new ComFunc();

            Cursor.Current = Cursors.WaitCursor;

            SS1c_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = " SELECT MDATE, PANO, MENU1, MENU2, MENU3 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_SORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE MDATE = TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, TxtDietDate.Text, -1) + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND (MENU1 = 'Y' OR MENU2 = 'Y' OR MENU3 = 'Y')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strComp1 = "S";
                        strComp2 = "S";
                        strComp3 = "S";

                        SQL = "";
                        SQL = " SELECT A.ROOMCODE, A.ACTDATE, A.PANO, A.SUCODE, A.DIETDAY, B.SNAME ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DIET_NEWORDER A, KOSMOS_PMPA.BAS_PATIENT B ";
                        SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE = TO_DATE('" + TxtDietDate.Text + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.SUCODE IN 'FD01'";
                        SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO ";

                        SqlErr = clsDB.GetDataTable(ref dtSub, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dtSub.Rows.Count > 0)
                        {
                            strROOMCODE = dtSub.Rows[0]["ROOMCODE"].ToString().Trim();
                            strSName = dtSub.Rows[0]["SNAME"].ToString().Trim();

                            for (int j = 0; j < dtSub.Rows.Count; j++)
                            {
                                if (dt.Rows[i]["MENU1"].ToString().Trim() == "Y")
                                {
                                    if (strComp1 == "S")
                                    {
                                        strComp1 = "NO";
                                    }
                                    if (dtSub.Rows[j]["DIETDAY"].ToString().Trim() == "1")
                                    {
                                        strComp1 = "OK";
                                    }
                                }

                                if (dt.Rows[i]["MENU2"].ToString().Trim() == "Y")
                                {
                                    if (strComp2 == "S")
                                    {
                                        strComp2 = "NO";
                                    }
                                    if (dtSub.Rows[j]["DIETDAY"].ToString().Trim() == "1")
                                    {
                                        strComp2 = "OK";
                                    }
                                }
                                if (dt.Rows[i]["MENU3"].ToString().Trim() == "Y")
                                {
                                    if (strComp3 == "S")
                                    {
                                        strComp3 = "NO";
                                    }
                                    if (dtSub.Rows[j]["DIETDAY"].ToString().Trim() == "1")
                                    {
                                        strComp3 = "OK";
                                    }
                                }
                            }
                        }
                        dtSub.Dispose();
                        dtSub = null;

                        if (strComp1 == "NO" || strComp2 == "NO" || strComp3 == "NO")
                        {
                            SS1c_Sheet1.RowCount = SS1c_Sheet1.RowCount + 1;
                            SS1c_Sheet1.Cells[SS1c_Sheet1.RowCount - 1, 0].Text = strROOMCODE;
                            SS1c_Sheet1.Cells[SS1c_Sheet1.RowCount - 1, 1].Text = strSName;

                            if (strComp1 == "NO")
                            {
                                SS1c_Sheet1.Cells[SS1c_Sheet1.RowCount - 1, 2].BackColor = System.Drawing.Color.FromArgb(200, 200, 255);
                            }
                            else if (strComp2 == "NO")
                            {
                                SS1c_Sheet1.Cells[SS1c_Sheet1.RowCount - 1, 3].BackColor = System.Drawing.Color.FromArgb(200, 200, 255);
                            }
                            else if (strComp3 == "NO")
                            {
                                SS1c_Sheet1.Cells[SS1c_Sheet1.RowCount - 1, 4].BackColor = System.Drawing.Color.FromArgb(200, 200, 255);
                            }
                            READ_DIETORDER_C(dt.Rows[i]["PANO"].ToString().Trim(), SS1c_Sheet1.RowCount - 1);
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_DIETORDER_C(string ArgPano, int nRow)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dtSub = null;
            string SqlErr = "";
            string strDayOld = "";
            int strDay = 0;

            Cursor.Current = Cursors.WaitCursor;

            ComFunc CF = new ComFunc();

            strDayOld = "";

            try
            {
                SQL = "";
                SQL = " SELECT ACTDATE, PANO, BI, DEPTCODE, DRCODE, ";
                SQL = SQL + ComNum.VBLF + " WARDCODE, ROOMCODE, DIETCODE, DIETNAME, SUCODE, DIETDAY,";
                SQL = SQL + ComNum.VBLF + " QTY, UNIT, BUN, ENTDATE, INPUTID, GBSUNAP, PRINT, GBADD, GBADD1, GBADD2 ";
                SQL = SQL + ComNum.VBLF + "  FROM DIET_NEWORDER_HIS";
                SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + FstrDietDate + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND PANO ='" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "    ORDER BY DIETDAY, BUN ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (strDayOld != dt.Rows[j]["DIETDAY"].ToString().Trim())
                        {
                            if (FnMaxHight < FnHight)
                            {
                                FnMaxHight = FnHight;
                            }
                            FnHight = 0;
                            strDayOld = dt.Rows[j]["DIETDAY"].ToString().Trim();
                        }

                        switch (dt.Rows[j]["DIETDAY"].ToString().Trim())
                        {
                            case "1":
                                strDay = 3;
                                break;
                            case "2":
                                strDay = 4;
                                break;
                            case "3":
                                strDay = 5;
                                break;
                        }

                        if (SS1c_Sheet1.Cells[nRow, strDay - 1].Text == "")
                        {
                            if (VB.Val(dt.Rows[j]["QTY"].ToString().Trim()) > 1)
                            {
                                SS1c_Sheet1.Cells[nRow, strDay - 1].Text = dt.Rows[j]["DIETNAME"].ToString().Trim() + ComNum.VBLF + dt.Rows[j]["QTY"].ToString().Trim();
                                FnHight = FnHight + 1;
                            }
                            else
                            {
                                SS1c_Sheet1.Cells[nRow, strDay - 1].Text = dt.Rows[j]["DIETNAME"].ToString().Trim();
                            }
                            FnHight = FnHight + 1;
                        }
                        else
                        {
                            if (VB.Val(dt.Rows[j]["QTY"].ToString().Trim()) > 1)
                            {
                                SS1c_Sheet1.Cells[nRow, strDay - 1].Text = SS1c_Sheet1.Cells[nRow, strDay - 1].Text + dt.Rows[j]["DIETNAME"].ToString().Trim() + ComNum.VBLF + dt.Rows[j]["QTY"].ToString().Trim();
                                FnHight = FnHight + 1;
                            }
                            else
                            {
                                SS1c_Sheet1.Cells[nRow, strDay - 1].Text = SS1c_Sheet1.Cells[nRow, strDay - 1].Text + dt.Rows[j]["DIETNAME"].ToString().Trim();
                            }
                            FnHight = FnHight + 1;
                        }

                        if (dt.Rows[j]["BUN"].ToString().Trim() == "01")
                        {
                            SQL = "";
                            SQL = " SELECT MENU" + dt.Rows[j]["DIETDAY"].ToString().Trim() + " FROM DIET_SORDER ";
                            SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + ArgPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND MDATE = TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, FstrDietDate, -1) + "' ,'YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND MENU" + dt.Rows[j]["DIETDAY"].ToString().Trim() + "= 'Y'";

                            SqlErr = clsDB.GetDataTable(ref dtSub, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dtSub.Rows.Count > 0)
                            {
                                if (dtSub.Rows[0]["MENU"].ToString().Trim() + dt.Rows[j]["DIETDAY"].ToString().Trim() == "Y")
                                {
                                    SS1c_Sheet1.Cells[nRow, strDay - 1].Text = SS1c_Sheet1.Cells[nRow, strDay - 1].Text + " (선)";
                                }
                            }

                            dtSub.Dispose();
                            dtSub = null;
                        }
                    }
                }

                if (FnMaxHight < FnHight)
                {
                    FnMaxHight = FnHight;
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
        
        private bool READ_SORDER(string argDietDay, string argPTNO, string ArgDate)
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;
            ComFunc CF = new ComFunc();

            Cursor.Current = Cursors.WaitCursor;

            if (argDietDay == "3")
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL = " SELECT MENU1, MENU2";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_SORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE MDATE = TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, ArgDate, -1) + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND PANO = '" + argPTNO + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (argDietDay == "1" && dt.Rows[0]["MENU1"].ToString().Trim() == "Y")
                    {
                        rtnVal = true;
                    }
                    if (argDietDay == "2" && dt.Rows[0]["MENU2"].ToString().Trim() == "Y")
                    {
                        rtnVal = true;
                    }
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        ///  MSG_사식마감
        /// </summary>
        private bool MSG_TreatmentMeal_Last(string argDietDay, string ArgCode, string argBun)
        {
            bool rtnVal = false;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            if (FstrDietJikwon)
            {
                return rtnVal;
            }


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NEWCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE SUCODE = 'FD021' ";
                SQL = SQL + ComNum.VBLF + " AND DIETCODE = '" + ArgCode + "' ";
                SQL = SQL + ComNum.VBLF + " AND BUN = '" + argBun + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (argDietDay == "2")
                    {
                        //2020-02-21 의뢰서
                        //if (string.Compare(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M", "-"), "11:00") >= 0)
                        if (string.Compare(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M", "-"), "11:30") >= 0)
                            {
                            MessageBox.Show("11시 이후에는 사식이 불가능합니다." + ComNum.VBLF + "변경을 원하시는 경우에는 영양실로 연락바랍니다.");
                            rtnVal = true;
                        }
                    }
                    if (argDietDay == "3")
                    {
                        //2020-02-21 의뢰서
                        //if (string.Compare(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M", "-"), "16:00") >= 0)
                        if (string.Compare(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M", "-"), "16:30") >= 0)
                        {
                            MessageBox.Show("16시 이후에는 사식이 불가능합니다." + ComNum.VBLF + "변경을 원하시는 경우에는 영양실로 연락바랍니다.");
                            rtnVal = true;
                        }
                    }
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        ///  MSG_치료식마감
        /// </summary>
        /// <returns></returns>
        private bool MSG_TreatmentMeal(string argDietDay, string ArgCode, string argBun)
        {
            bool rtnVal = false;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            if (FstrDietJikwon)
            {
                return rtnVal;
            }


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NEWCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE (CODENAME LIKE '%경관%' OR BUN IN ('02','03')) ";
                SQL = SQL + ComNum.VBLF + " AND GBUSED = '0' ";
                SQL = SQL + ComNum.VBLF + " AND DIETCODE = '" + ArgCode + "' ";
                SQL = SQL + ComNum.VBLF + " AND BUN = '" + argBun + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (argDietDay == "1")
                    {
                        if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "05:00") >= 0)
                        {
                            MessageBox.Show("지금 시간에는 치료식이 변경이 불가능합니다." + ComNum.VBLF + ComNum.VBLF + strMsg2);
                            rtnVal = true;
                        }
                    }
                    if (argDietDay == "2")
                    {
                        if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "10:00") >= 0)
                        {
                            MessageBox.Show("지금 시간에는 치료식이 변경이 불가능합니다." + ComNum.VBLF + ComNum.VBLF + strMsg2);
                            rtnVal = true;
                        }
                    }
                    if (argDietDay == "3")
                    {
                        if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M"), "15:00") >= 0)
                        {
                            MessageBox.Show("지금 시간에는 치료식이 변경이 불가능합니다." + ComNum.VBLF + ComNum.VBLF + strMsg2);
                            rtnVal = true;
                        }
                    }
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// READ_치료식중복
        /// </summary>
        private bool TreatmentMeal_Overlap()
        {
            bool rtnVal = false;
            int i = 0;

            rtnVal = false;
            if (FstrDietJikwon)
            {
                return rtnVal;
            }

            for (i = 1; i <= SSOrder_Sheet1.RowCount; i++)
            {
                if (SSOrder_Sheet1.Cells[i - 1, 6].Text == "02" || SSOrder_Sheet1.Cells[i - 1, 6].Text == "03")
                {
                    rtnVal = true;
                }
            }

            return rtnVal;
        }

        private bool READ_ORAL_DIET(string ArgCode)
        {
            int i = 0;
            int j = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string[] strDIETCODE = new string[21];
            string strSucode = "";
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT SUCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NEWCODE";
                SQL = SQL + ComNum.VBLF + " WHERE DIETCODE = '" + ArgCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BUN = '03' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strSucode = dt.Rows[0]["SUCODE"].ToString().Trim();
                }

                if (strSucode == "FT01")
                {

                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    rtnVal = true;
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT SUCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NEWCODE";
                SQL = SQL + ComNum.VBLF + " WHERE CODENAME LIKE '%경관%' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDIETCODE[i] = dt.Rows[i]["SUCODE"].ToString().Trim();
                    }
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    rtnVal = true;
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;

                for (i = 1; i <= SSOrder_Sheet1.RowCount; i++)
                {
                    if (SSOrder_Sheet1.Cells[i - 1, 3].Text != "")
                    {
                        for (j = 0; j <= 19; j++)
                        {
                            if (strDIETCODE[j] == SSOrder_Sheet1.Cells[i - 1, 3].Text)
                            {
                                rtnVal = true;
                                return rtnVal;
                            }
                        }
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }


        private void READ_CONSULT()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            //string StrTemp = "";

            Cursor.Current = Cursors.WaitCursor;
            ComFunc CF = new ComFunc();

            SSChk_Sheet1.Cells[0, 0, SSChk_Sheet1.RowCount - 1, SSChk_Sheet1.ColumnCount - 1].Text = "";

            try
            {
                SQL = "";
                SQL = " SELECT DIETBUN, WARDCODE, SNAME, PANO, TO_CHAR(BDATE, 'YYYY-MM-DD') AS BDATE  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_CONSULT ";

                if (FstrDietJikwon == false)
                {

                    SSChk_Sheet1.Columns[2].Visible = false;
                    SSChk_Sheet1.Columns[2].Width = 15;
                    groupBox3.Text = "협진의뢰 완료현황";

                    SQL = SQL + ComNum.VBLF + "   WHERE DIETBUN = '3' ";// '완료된것
                    SQL = SQL + ComNum.VBLF + "    AND TRUNC(ENTDATE) = TO_DATE('" + TxtDietDate.Text + "','YYYY-MM-DD')  ";

                    if (GstrWardCode == "IU")
                    {
                        if (GstrWardCodes == "SICU" || VB.Left(ComboWard.Text, 2) == "외과")
                        {
                            SQL = SQL + ComNum.VBLF + "  AND  WardCode = 'IU' AND RoomCode=233 ";
                        }
                        else if (GstrWardCodes == "MICU" || VB.Left(ComboWard.Text, 2) == "내과")
                        {
                            SQL = SQL + ComNum.VBLF + "  AND  WardCode = 'IU' AND RoomCode=234 ";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode = '" + GstrWardCode + "' ";
                    }
                }
                else//영양실의 경우 지정일로부터 오일전까지 의뢰확인을 하지 않은 모든 명단을 불러온다.
                {
                    SSChk_Sheet1.Columns[2].Visible = true;
                    groupBox3.Text = "협진의뢰 신청현황";
                    SQL = SQL + ComNum.VBLF + "  WHERE CHKVALID IS NULL ";
                    SQL = SQL + ComNum.VBLF + "    AND TRUNC(BDATE) >= TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, TxtDietDate.Text, -7) + "','YYYY-MM-DD')";//  '의뢰 확인안한건 일주일치까지만 보이기
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY WARDCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SSChk_Sheet1.RowCount = dt.Rows.Count + 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["DIETBUN"].ToString().Trim())
                        {
                            case "1":
                                SSChk_Sheet1.Cells[i, 0].Text = "당뇨";
                                break;
                            case "2":
                                SSChk_Sheet1.Cells[i, 0].Text = "신장";
                                break;
                            case "3":
                                SSChk_Sheet1.Cells[i, 0].Text = "기타";
                                break;
                        }
                        SSChk_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WARDCODE"].ToString().Trim() + "/" + dt.Rows[i]["SNAME"].ToString().Trim();
                        SSChk_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        SSChk_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BDATE"].ToString().Trim();
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_INFLU_H1N1(string ArgPano)
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strRtn = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT Pano FROM  KOSMOS_OCS.EXAM_INFECTMASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE (INFLUAG Is Not Null Or INFLUAPR Is Not Null) ";
                SQL = SQL + ComNum.VBLF + "    AND PANO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND PANO NOT IN ( SELECT PANO FROM KOSMOS_PMPA.ETC_INFLU_OK WHERE ( DELDATE IS NULL OR DELDATE ='') ) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    strRtn = "YES";
                }

                dt.Dispose();
                dt = null;

                return strRtn;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strRtn;
            }
        }

        private bool READ_PATIENTMEMO(double ArgIpdNo)
        {

            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT PANO FROM KOSMOS_PMPA.DIET_MEMO ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIpdNo;

                //switch (clsType.User.Sabun)
                switch (clsType.User.JobGroup)
                {
                    //case "04349":
                    //case "20433":
                    //case "20442":
                    //case "20193":
                    //case "04444":
                    case "JOB018003":
                        SQL = SQL + ComNum.VBLF + "   AND (DIET1 IS NOT NULL OR DIET2 IS NOT NULL OR DIET3 IS NOT NULL)  ";
                        break;
                }

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

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

        }

        private bool GET_DIETPRT_LOCK()
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;
            string strDate = "";

            Cursor.Current = Cursors.WaitCursor;

            ComFunc CF = new ComFunc();

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(LOCKDATE, 'YYYY-MM-DD HH:MM') AS LOCKDATE FROM " + ComNum.DB_PMPA + "DIET_LOCK ";
                SQL = SQL + ComNum.VBLF + " ORDER BY LOCKDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strDate = dt.Rows[0]["LOCKDATE"].ToString().Trim();

                    if (CF.DATE_TIME(clsDB.DbCon, strDate,
                        ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + " " +
                        ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M", ":")) <= 3)
                    {
                        rtnVal = true;
                    }
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장

                return rtnVal;
            }

        }

        private void ReadMemo()
        {

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            txtMemo.Text = "";
            txtMemoRowid.Text = "";
            ssMemoList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT WRITEDATE, WRITESABUN, MEMO, DIETCERT, rowid, DIET1, DIET2, DIET3";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_MEMO ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + FNIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + GstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY WRITEDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssMemoList_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssMemoList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["writedate"].ToString().Trim();
                        ssMemoList_Sheet1.Cells[i, 1].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["WRITESABUN"].ToString().Trim());
                        ssMemoList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DIETCERT"].ToString().Trim();
                        ssMemoList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["rowid"].ToString().Trim();
                        ssMemoList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["MEMO"].ToString().Trim();
                    }

                    txtMemo.Text = dt.Rows[0]["MEMO"].ToString().Trim();
                    txtMemoRowid.Text = dt.Rows[0]["ROWID"].ToString().Trim();
                    cboDiet1.Text = dt.Rows[0]["DIET1"].ToString().Trim();
                    cboDiet2.Text = dt.Rows[0]["DIET2"].ToString().Trim();
                    cboDiet3.Text = dt.Rows[0]["DIET3"].ToString().Trim();
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void DOWNLOADnCREATE(string strArgPath, string strEXE, string strName = "", string strBUSE = "", string strGUBUN = "")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strIP1 = clsCompuInfo.gstrCOMIP;
            string[] strIP = VB.Split(strIP1, ".");
            string strIP2 = "";
            string strLocal = "";
            string strPath = "";
            string strHost = "";

            for (i = 0; i < strIP.Length; i++)
            {
                strIP2 = strIP2 + VB.Val(strIP[i]).ToString("000") + ".";
            }

            strIP2 = VB.Mid(strIP2, 1, strIP2.Length - 1);

            if (strBUSE != "")
            {
                try
                {
                    SQL = "";
                    SQL = "SELECT * FROM " + ComNum.DB_ERP + "JAS_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE IPADDR = '" + strIP2 + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND BUCODE = '" + strBUSE + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {

                        dt.Dispose();
                        dt = null;
                        return;
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

            DirectoryInfo Dir = new DirectoryInfo(strArgPath.ToLower() + "\\" + strEXE);

            if (Dir.Exists == false)
            {
                if (strGUBUN != "FIRSTDIS")
                {
                    strEXE = strEXE.ToLower();
                    strArgPath = strArgPath.ToLower();
                }

                strLocal = strArgPath + "\\" + strEXE;

                Ftpedt FtpedtX = new Ftpedt();

                if (FtpedtX.FtpConnetBatch("192.168.100.33", "pcnfs", "pcnfs1") == false)
                {
                    ComFunc.MsgBox("FTP Server Connect ERROR !!!", "오류");
                    return;
                }

                if (strArgPath == "c:\\cmc\\exe")
                {
                    strPath = "/pcnfs/exe/" + strEXE;
                    strHost = "/pcnfs/exe";
                }
                else
                {
                    strPath = "/pcnfs/ocsexe/" + strEXE;
                    strHost = "/pcnfs/ocsexe";
                }

                if (FtpedtX.FtpDownload("192.168.100.33", "pcnfs", "pcnfs1", strLocal, strPath, strHost) == false)
                {
                    ComFunc.MsgBox("다운로드 실패", "종료");
                    FtpedtX.FtpDisConnetBatch();
                    FtpedtX = null;
                    return;
                }

                FtpedtX.FtpDisConnetBatch();
                FtpedtX = null;
            }

            //if (strName != "")
            //{
            //    VB.Shell(strArgPath + "\\" + strEXE);
            //}
            this.Close();

        }

        private void frmDietOrderNew_Activated(object sender, EventArgs e)
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

        private void frmDietOrderNew_FormClosed(object sender, FormClosedEventArgs e)
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

        private void ssMemoList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0)
            {
                return;
            }

            if (ssMemoList_Sheet1.Cells[e.Row, 4].Text == "")
            {
                return;
            }

            MessageBox.Show(ssMemoList_Sheet1.Cells[e.Row, 4].Text.Trim());
        }

        private void ssMemo_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            int intRowAffected = 0;
            //bool rtnVal = false;
            string SqlErr = "";
            string strCERT1 = "";
            string strCERT2 = "";
            string strCERT3 = "";
            string strCERT4 = "";
            string strCERT5 = "";
            string strMemo = "";
            string strROWID = "";
            Cursor.Current = Cursors.WaitCursor;

            if (e.Column != 4)
            {
                return;
            }
            if (e.Row < 0)
            {
                return;
            }

            strROWID = ssMemo_Sheet1.Cells[e.Row, 5].Text.Trim();
            strMemo = ssMemo_Sheet1.Cells[e.Row, 3].Text.Trim();

            

            try
            {
                SQL = "";
                SQL = " SELECT DIETCERT1, DIETCERT2, DIETCERT3, DIETCERT4, DIETCERT5, ROWID  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DIET_MEMO MST";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "' ";
                SQL = SQL + ComNum.VBLF + "       AND NOT EXISTS";
                SQL = SQL + ComNum.VBLF + "       ( SELECT * FROM KOSMOS_PMPA.DIET_MEMO SUB";
                SQL = SQL + ComNum.VBLF + "       WHERE MST.ROWID = SUB.ROWID";
                SQL = SQL + ComNum.VBLF + "       AND (DIETCERT1 = " + clsType.User.Sabun + " OR DIETCERT2 = " + clsType.User.Sabun + " OR DIETCERT3 = " + clsType.User.Sabun + " OR DIETCERT4 = " + clsType.User.Sabun + " OR DIETCERT5 = " + clsType.User.Sabun + "))";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strCERT1 = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DIETCERT1"].ToString().Trim());
                    strCERT2 = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DIETCERT2"].ToString().Trim());
                    strCERT3 = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DIETCERT3"].ToString().Trim());
                    strCERT4 = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DIETCERT4"].ToString().Trim());
                    strCERT5 = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DIETCERT5"].ToString().Trim());

                    if (strCERT1 != "" && strCERT2 != "" && strCERT3 != "" && strCERT4 != "" && strCERT5 != "")
                    {
                    }
                    else
                    {

                        clsDB.setBeginTran(clsDB.DbCon);

                        SQL = "";
                        SQL = " UPDATE KOSMOS_PMPA.DIET_MEMO SET";
                        if (strCERT1 == "")
                        {
                            SQL = SQL + ComNum.VBLF + " DIETCERT1 = " + clsType.User.Sabun + ", ";
                        }
                        else if (strCERT2 == "")
                        {
                            SQL = SQL + ComNum.VBLF + " DIETCERT2 = " + clsType.User.Sabun + ", ";
                        }
                        else if (strCERT3 == "")
                        {
                            SQL = SQL + ComNum.VBLF + " DIETCERT3 = " + clsType.User.Sabun + ", ";
                        }
                        else if (strCERT4 == "")
                        {
                            SQL = SQL + ComNum.VBLF + " DIETCERT4 = " + clsType.User.Sabun + ", ";
                        }
                        else if (strCERT5 == "")
                        {
                            SQL = SQL + ComNum.VBLF + " DIETCERT5 = " + clsType.User.Sabun + ", ";
                        }
                        SQL = SQL + ComNum.VBLF + " DIETCERT = '1' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

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
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnMemoDel_Click(object sender, EventArgs e)
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            int intRowAffected = 0;
            //bool rtnVal = false;
            string SqlErr = "";
            //string strCERT1 = "";
            //string strCERT2 = "";
            //string strCERT3 = "";
            //string strCERT4 = "";
            //string strCERT5 = "";
            //string strMemo = "";
            string strROWID = "";
            Cursor.Current = Cursors.WaitCursor;

            int nRow = 0;

            nRow = ssMemo_Sheet1.ActiveRowIndex;

            if (nRow < 0)
            {
                return;
            }

            if (FstrDietJikwon)
            {
            }
            else
            {
                return;
            }


            strROWID = ssMemo_Sheet1.Cells[nRow, 5].Text.Trim();

            if (ComFunc.MsgBoxQ("선택한 메모를 삭제 하시겠습니까 ??", "삭제확인", MessageBoxDefaultButton.Button2) == DialogResult.No) return;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " INSERT INTO KOSMOS_PMPA.DIET_MEMO_HIS ";
                SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.DIET_MEMO WHERE ROWID = '"  + strROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " DELETE KOSMOS_PMPA.DIET_MEMO WHERE ROWID = '" + strROWID + "' ";
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

                READ_MEMO_LIST_New();

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}
