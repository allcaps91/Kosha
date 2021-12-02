using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewJobReportPRN.cs
    /// Description     : 원무팀 업무일지(외래)
    /// Author          : 안정수
    /// Create Date     : 2017-08-16
    /// Update History  : 2017-10-24    
    /// <history>       
    /// d:\psmh\OPD\Wonmu\Frm업무일지_외래.frm(Frm업무일지_외래) => frmPmpaViewJobReportPRN.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\Wonmu\Frm업무일지_외래.frm(Frm업무일지_외래)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewJobReportPRN : Form, MainFormMessage
    {
        int[,] FnCnt = new int[30, 5];
        string FstrRowid = "";
        ComFunc CF = new ComFunc();

        string mstrJobSabun = "";

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

        public frmPmpaViewJobReportPRN(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmPmpaViewJobReportPRN()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewJobReportPRN(string GstrJobSabun)
        {
            InitializeComponent();
            setEvent();
            mstrJobSabun = GstrJobSabun;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
            dtpDate.Text = Convert.ToDateTime(CurrentDate).AddDays(-1).ToShortDateString();

            Screen_Clear();
        }

        void Screen_Clear()
        {
            int i = 0;
            int j = 0;

            for (i = 0; i < 30; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    FnCnt[i, j] = 0;
                }
            }

            ssList1_Sheet1.Cells[3, 1].Text = ""; //날짜일시
            ssList1_Sheet1.Cells[3, 1].Text = VB.Left(dtpDate.Text, 4) + " 년  ";
            ssList1_Sheet1.Cells[3, 1].Text += ComFunc.SetAutoZero(VB.Mid(dtpDate.Text, 6, 2), 1) + " 월  ";
            ssList1_Sheet1.Cells[3, 1].Text += ComFunc.SetAutoZero(VB.Mid(dtpDate.Text, 9, 2), 1) + " 일  ";
            ssList1_Sheet1.Cells[3, 1].Text += CF.READ_YOIL(clsDB.DbCon,dtpDate.Text);

            //환자현황 Clear
            for(i = 7; i <= 36; i++)
            {
                for(j = 2; j <= 5; j++)
                {
                    ssList1_Sheet1.Cells[i, j].Text = "";
                }

                //보험종류별 입원/외래 환자수
                if(i == 7 || i == 11)
                {
                    ssList1_Sheet1.Cells[i, 7].Text = "";
                    ssList1_Sheet1.Cells[i, 8].Text = "";
                    ssList1_Sheet1.Cells[i, 11].Text = "";
                    ssList1_Sheet1.Cells[i, 13].Text = "";
                    ssList1_Sheet1.Cells[i, 15].Text = "";
                    ssList1_Sheet1.Cells[i, 17].Text = "";
                }

                //무인수납/진단서현황 첫줄
                else if(i == 17 || i == 28)
                {
                    ssList1_Sheet1.Cells[i, 7].Text = "";
                    ssList1_Sheet1.Cells[i, 8].Text = "";
                    ssList1_Sheet1.Cells[i, 10].Text = "";
                    ssList1_Sheet1.Cells[i, 12].Text = "";
                    ssList1_Sheet1.Cells[i, 14].Text = "";
                    ssList1_Sheet1.Cells[i, 16].Text = "";
                    ssList1_Sheet1.Cells[i, 18].Text = "";
                }

                //예약부도자 현황수
                else if(i >= 22 && i <= 24)
                {
                    ssList1_Sheet1.Cells[i, 8].Text = "";
                    ssList1_Sheet1.Cells[i, 10].Text = "";
                    ssList1_Sheet1.Cells[i, 13].Text = "";
                    ssList1_Sheet1.Cells[i, 15].Text = "";
                    ssList1_Sheet1.Cells[i, 18].Text = "";
                }

                //진단서현황 두번째줄부터 ~
                else if(i == 30)
                {
                    ssList1_Sheet1.Cells[i, 8].Text = "";
                    ssList1_Sheet1.Cells[i, 10].Text = "";
                    ssList1_Sheet1.Cells[i, 12].Text = "";
                    ssList1_Sheet1.Cells[i, 14].Text = "";
                    ssList1_Sheet1.Cells[i, 16].Text = "";
                    ssList1_Sheet1.Cells[i, 18].Text = "";
                }

                //진단서현황 마지막줄
                else if(i == 32)
                {
                    ssList1_Sheet1.Cells[i, 8].Text = "";
                }
            }

            //외래신환환자수
            ssList1_Sheet1.Cells[35, 5].Text = "";

            //뒷장
            //일일 수의 집계
            for(i = 3; i <= 6; i++)
            {
                ssList2_Sheet1.Cells[i, 2].Text = "";
                ssList2_Sheet1.Cells[i, 5].Text = "";
                ssList2_Sheet1.Cells[i, 7].Text = "";
                ssList2_Sheet1.Cells[i, 8].Text = "";
                ssList2_Sheet1.Cells[i, 9].Text = "";
                ssList2_Sheet1.Cells[i, 10].Text = "";
                ssList2_Sheet1.Cells[i, 11].Text = "";
                ssList2_Sheet1.Cells[i, 12].Text = "";
            }

            //진료비 감액 현황
            for(i = 11; i <= 20; i++)
            {
                if (i < 14)
                {
                    ssList2_Sheet1.Cells[i, 7].Text = "";
                    ssList2_Sheet1.Cells[i, 11].Text = "";
                }

                ssList2_Sheet1.Cells[i, 2].Text = "";
                ssList2_Sheet1.Cells[i, 5].Text = "";
                ssList2_Sheet1.Cells[i, 9].Text = "";
            }

            //일반미수금
            for(i = 24; i <= 27; i++)
            {
                ssList2_Sheet1.Cells[i, 2].Text = "";
                ssList2_Sheet1.Cells[i, 5].Text = "";
                ssList2_Sheet1.Cells[i, 8].Text = "";
                ssList2_Sheet1.Cells[i, 10].Text = "";
            }

            //직원근무현황
            ssList2_Sheet1.Cells[31, 1].Text = "";
            ssList2_Sheet1.Cells[31, 3].Text = "";
            ssList2_Sheet1.Cells[31, 6].Text = "";
            ssList2_Sheet1.Cells[31, 10].Text = "";

            //기타
            ssList2_Sheet1.Cells[35, 1].Text = "";
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                ePrint();
            }
        }

        void ePrint()
        {
            if(tabItem1.IsSelected == true)
            {
                clsSpread SPR = new clsSpread();
                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;


                #region //시트 히든

                //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


                #endregion 

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 50, 100, 10, 30, 50);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, true, false);
                

                SPR.setSpdPrint(ssList1, PrePrint, setMargin, setOption, strHeader, strFooter);

                #region //시트 히든 복원

                //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
                #endregion
            }

            else if(tabItem2.IsSelected == true)
            {
                clsSpread SPR = new clsSpread();
                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;


                #region //시트 히든

                //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


                #endregion

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 50, 100, 10, 30, 50);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, true, false, (float)0.9);
                

                SPR.setSpdPrint(ssList2, PrePrint, setMargin, setOption, strHeader, strFooter);

                #region //시트 히든 복원

                //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
                #endregion

                Screen_DisPlay_진료비저장내용저장(dtpDate.Text);
            }
        }
        
        void Screen_DisPlay_진료비저장내용저장(string ArgDate)
        {
            string StrBUN1 = "", StrBUN2 = "", StrBUN3 = "", StrBUN4 = "", StrBUN5 = "";
            string StrBUN6 = "", StrBUN7 = "", StrBUN8 = "", StrBUN9 = "", StrBUN10 = "";
            string StrBUN11 = "", StrBUN12 = "", StrBUN13 = "", StrBUN14 = "", StrBUN15 = "";
            string StrBUN16 = "", StrBUN17 = "", StrBUN18 = "", StrBUN19 = "", StrBUN20 = "";
            string StrBUN21 = "", StrBUN22 = "", StrBUN23 = "", StrBUN24 = "";

            string StrDuty1 = "", StrDuty2 = "", StrDuty3 = "", StrDuty4 = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

             
            clsDB.setBeginTran(clsDB.DbCon);
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (FstrRowid != "")
            {
                //해당일 자료를 삭제함
                SQL = "";
                SQL += ComNum.VBLF + "DELETE                                        ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_WONMUDAILY    ";
                SQL += ComNum.VBLF + "WHERE 1=1                                     ";
                SQL += ComNum.VBLF + "      AND rowid = '"+ FstrRowid +"'           ";
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

            StrBUN1 = ssList2_Sheet1.Cells[3, 2].Text;
            StrBUN2 = ssList2_Sheet1.Cells[3, 5].Text;
            StrBUN3 = ssList2_Sheet1.Cells[3, 7].Text;
            StrBUN4 = ssList2_Sheet1.Cells[3, 8].Text;
            StrBUN5 = ssList2_Sheet1.Cells[3, 9].Text;
            StrBUN6 = ssList2_Sheet1.Cells[3, 10].Text;
            StrBUN7 = ssList2_Sheet1.Cells[3, 11].Text;
            StrBUN8 = ssList2_Sheet1.Cells[3, 12].Text;

            StrBUN9 = ssList2_Sheet1.Cells[4, 2].Text;
            StrBUN10 = ssList2_Sheet1.Cells[4, 5].Text;
            StrBUN11 = ssList2_Sheet1.Cells[4, 7].Text;
            StrBUN12 = ssList2_Sheet1.Cells[4, 8].Text;
            StrBUN13 = ssList2_Sheet1.Cells[4, 9].Text;
            StrBUN14 = ssList2_Sheet1.Cells[4, 10].Text;
            StrBUN15 = ssList2_Sheet1.Cells[4, 11].Text;
            StrBUN16 = ssList2_Sheet1.Cells[4, 12].Text;

            StrBUN17 = ssList2_Sheet1.Cells[6, 2].Text;
            StrBUN18 = ssList2_Sheet1.Cells[6, 5].Text;
            StrBUN19 = ssList2_Sheet1.Cells[6, 7].Text;
            StrBUN20 = ssList2_Sheet1.Cells[6, 8].Text;
            StrBUN21 = ssList2_Sheet1.Cells[6, 9].Text;
            StrBUN22 = ssList2_Sheet1.Cells[6, 10].Text;
            StrBUN23 = ssList2_Sheet1.Cells[6, 11].Text;
            StrBUN24 = ssList2_Sheet1.Cells[6, 12].Text;

            StrDuty1 = ssList2_Sheet1.Cells[31, 10].Text;
            StrDuty2 = ssList2_Sheet1.Cells[31, 3].Text;
            StrDuty3 = ssList2_Sheet1.Cells[31, 6].Text;
            StrDuty4 = ssList2_Sheet1.Cells[31, 1].Text;

            if (mstrJobSabun != "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "TONG_WONMUDAILY";
                SQL += ComNum.VBLF + "(                              ";
                SQL += ComNum.VBLF + "JOBDATE,GBIO,BUN1,BUN2,BUN3,BUN4,BUN5,BUN6,BUN7,BUN8,BUN9,BUN10,BUN11,BUN12,BUN13,BUN14,BUN15,BUN16, ";
                SQL += ComNum.VBLF + "BUN17,BUN18,BUN19,BUN20,BUN21,BUN22,BUN23,BUN24,DUTY1,DUTY2,DUTY3,DUTY4,ENTSABUN,ENTDATE";
                SQL += ComNum.VBLF + ")";
                SQL += ComNum.VBLF + "Values( TO_DATE('" + ArgDate + "','YYYY-MM-DD')  , 'O', '" + StrBUN1 + "','" + StrBUN2 + "','" + StrBUN3 + "','" + StrBUN4 + "',";
                SQL += ComNum.VBLF + "'" + StrBUN5 + "','" + StrBUN6 + "','" + StrBUN7 + "','" + StrBUN8 + "','" + StrBUN9 + "','" + StrBUN10 + "','" + StrBUN11 + "','" + StrBUN12 + "','" + StrBUN13 + "','" + StrBUN14 + "',";
                SQL += ComNum.VBLF + "'" + StrBUN15 + "','" + StrBUN16 + "','" + StrBUN17 + "','" + StrBUN18 + "','" + StrBUN19 + "','" + StrBUN20 + "','" + StrBUN21 + "','" + StrBUN22 + "','" + StrBUN23 + "','" + StrBUN24 + "','" + StrDuty1 + "','" + StrDuty2 + "','" + StrDuty3 + "','" + StrDuty4 + "',";
                SQL += ComNum.VBLF + mstrJobSabun + ", SYSDATE )";
            }

            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "TONG_WONMUDAILY";
                SQL += ComNum.VBLF + "(                              ";
                SQL += ComNum.VBLF + "JOBDATE,GBIO,BUN1,BUN2,BUN3,BUN4,BUN5,BUN6,BUN7,BUN8,BUN9,BUN10,BUN11,BUN12,BUN13,BUN14,BUN15,BUN16, ";
                SQL += ComNum.VBLF + "BUN17,BUN18,BUN19,BUN20,BUN21,BUN22,BUN23,BUN24,DUTY1,DUTY2,DUTY3,DUTY4,ENTDATE";
                SQL += ComNum.VBLF + ")";
                SQL += ComNum.VBLF + "Values( TO_DATE('" + ArgDate + "','YYYY-MM-DD')  , 'O', '" + StrBUN1 + "','" + StrBUN2 + "','" + StrBUN3 + "','" + StrBUN4 + "',";
                SQL += ComNum.VBLF + "'" + StrBUN5 + "','" + StrBUN6 + "','" + StrBUN7 + "','" + StrBUN8 + "','" + StrBUN9 + "','" + StrBUN10 + "','" + StrBUN11 + "','" + StrBUN12 + "','" + StrBUN13 + "','" + StrBUN14 + "',";
                SQL += ComNum.VBLF + "'" + StrBUN15 + "','" + StrBUN16 + "','" + StrBUN17 + "','" + StrBUN18 + "','" + StrBUN19 + "','" + StrBUN20 + "','" + StrBUN21 + "','" + StrBUN22 + "','" + StrBUN23 + "','" + StrBUN24 + "','" + StrDuty1 + "','" + StrDuty2 + "','" + StrDuty3 + "','" + StrDuty4 + "',";
                SQL += ComNum.VBLF + "SYSDATE )";
            }

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

        }

        void Screen_DisPlay_진료비저장내용호출(string ArgDate)
        {
            int i = 0;
            int j = 0;
            int nTotal = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  a.rowid,a.*";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_WONMUDAILY a";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND JOBDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND GBIO = 'O'";
            SQL += ComNum.VBLF + "";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                FstrRowid = "";

                if(dt.Rows.Count > 0)
                {
                    ssList2_Sheet1.Cells[3, 2].Text = dt.Rows[0]["BUN1"].ToString().Trim();
                    ssList2_Sheet1.Cells[3, 5].Text = dt.Rows[0]["BUN2"].ToString().Trim();
                    ssList2_Sheet1.Cells[3, 7].Text = dt.Rows[0]["BUN3"].ToString().Trim();
                    ssList2_Sheet1.Cells[3, 8].Text = dt.Rows[0]["BUN4"].ToString().Trim();
                    ssList2_Sheet1.Cells[3, 9].Text = dt.Rows[0]["BUN5"].ToString().Trim();
                    ssList2_Sheet1.Cells[3, 10].Text = dt.Rows[0]["BUN6"].ToString().Trim();
                    ssList2_Sheet1.Cells[3, 11].Text = dt.Rows[0]["BUN7"].ToString().Trim();
                    ssList2_Sheet1.Cells[3, 12].Text = dt.Rows[0]["BUN8"].ToString().Trim();

                    ssList2_Sheet1.Cells[4, 2].Text = dt.Rows[0]["BUN9"].ToString().Trim();
                    ssList2_Sheet1.Cells[4, 5].Text = dt.Rows[0]["BUN10"].ToString().Trim();
                    ssList2_Sheet1.Cells[4, 7].Text = dt.Rows[0]["BUN11"].ToString().Trim();
                    ssList2_Sheet1.Cells[4, 8].Text = dt.Rows[0]["BUN12"].ToString().Trim();
                    ssList2_Sheet1.Cells[4, 9].Text = dt.Rows[0]["BUN13"].ToString().Trim();
                    ssList2_Sheet1.Cells[4, 10].Text = dt.Rows[0]["BUN14"].ToString().Trim();
                    ssList2_Sheet1.Cells[4, 11].Text = dt.Rows[0]["BUN15"].ToString().Trim();
                    ssList2_Sheet1.Cells[4, 12].Text = dt.Rows[0]["BUN16"].ToString().Trim();

                    ssList2_Sheet1.Cells[6, 2].Text = dt.Rows[0]["BUN17"].ToString().Trim();
                    ssList2_Sheet1.Cells[6, 5].Text = dt.Rows[0]["BUN18"].ToString().Trim();
                    ssList2_Sheet1.Cells[6, 7].Text = dt.Rows[0]["BUN19"].ToString().Trim();
                    ssList2_Sheet1.Cells[6, 8].Text = dt.Rows[0]["BUN20"].ToString().Trim();
                    ssList2_Sheet1.Cells[6, 9].Text = dt.Rows[0]["BUN21"].ToString().Trim();
                    ssList2_Sheet1.Cells[6, 10].Text = dt.Rows[0]["BUN22"].ToString().Trim();
                    ssList2_Sheet1.Cells[6, 11].Text = dt.Rows[0]["BUN23"].ToString().Trim();
                    ssList2_Sheet1.Cells[6, 12].Text = dt.Rows[0]["BUN24"].ToString().Trim();

                    ssList2_Sheet1.Cells[31, 10].Text = dt.Rows[0]["DUTY1"].ToString().Trim();
                    ssList2_Sheet1.Cells[31, 3].Text = dt.Rows[0]["DUTY2"].ToString().Trim();
                    ssList2_Sheet1.Cells[31, 6].Text = dt.Rows[0]["DUTY3"].ToString().Trim();
                    ssList2_Sheet1.Cells[31, 1].Text = dt.Rows[0]["DUTY4"].ToString().Trim();

                    FstrRowid = dt.Rows[0]["Rowid"].ToString().Trim();
                }
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;


        }

        void eGetData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Screen_Clear();

            //외래 환자현황
            Screen_DisPlay_환자현황_입원(ssList1_Sheet1, dtpDate.Text);
            Screen_DisPlay_환자현황_퇴원(ssList1_Sheet1, dtpDate.Text);
            Screen_DisPlay_환자현황_재원(ssList1_Sheet1, dtpDate.Text);
            Screen_DisPlay_환자현황_외래(ssList1_Sheet1, dtpDate.Text);
            Screen_DisPlay_환자현황_외래외국인(ssList1_Sheet1, dtpDate.Text);

            //신환건수
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  COUNT(PANO) CNT";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND BDate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND Singu = '1'";
            SQL += ComNum.VBLF + "      AND Reserved = '0' ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim()) > 0)
                {
                    ssList1_Sheet1.Cells[34, 7].Text = dt.Rows[0]["CNT"].ToString().Trim();
                }
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

            Screen_DisPlay_보험별입원환자(ssList1_Sheet1, dtpDate.Text);
            Screen_DisPlay_보험별외래환자(ssList1_Sheet1, dtpDate.Text);
            Screen_DisPlay_무인수납(ssList1_Sheet1, dtpDate.Text);
            Screen_DisPlay_예약부도(ssList1_Sheet1, dtpDate.Text);
            Screen_DisPlay_진단서(ssList1_Sheet1, dtpDate.Text);

            Screen_DisPlay_일일수입(ssList2_Sheet1, dtpDate.Text);
            Screen_DisPlay_감액내역(ssList2_Sheet1, dtpDate.Text);
            Screen_DisPlay_미수내역(ssList2_Sheet1, dtpDate.Text);

            Screen_DisPlay_진료비저장내용호출(dtpDate.Text);


        }

        void Screen_DisPlay_환자현황_입원(FarPoint.Win.Spread.SheetView Spd, string ArgDate)
        {
            int i = 0;
            int j = 0;
            int nToTal = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  DEPTCODE, COUNT(PANO) CNT                                                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND (ActDate IS NULL OR ActDate>TO_DATE('" + ArgDate + "','YYYY-MM-DD'))            ";
            SQL += ComNum.VBLF + "      AND INDATE <= TO_DATE('" + ArgDate + " 23:59','YYYY-MM-DD HH24:MI')                 ";
            SQL += ComNum.VBLF + "      AND IpwonTime >=TO_DATE('" + ArgDate + "','YYYY-MM-DD')                             ";
            SQL += ComNum.VBLF + "      AND IpwonTime <=TO_DATE('" + ArgDate + " 23:59','YYYY-MM-DD HH24:MI')               ";
            SQL += ComNum.VBLF + "      AND PANO <> '81000004'                                                              ";
            SQL += ComNum.VBLF + "GROUP BY DEPTCODE                                                                         ";
            SQL += ComNum.VBLF + "ORDER BY DEPTCODE                                                                         ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        for(j = 8; j <= 36; j++)
                        {
                            if(Spd.Cells[j, 1].Text == dt.Rows[i]["DEPTCODE"].ToString().Trim())
                            {
                                Spd.Cells[j, 2].Text += dt.Rows[i]["CNT"].ToString().Trim();
                                nToTal += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                            }
                        }
                    }
                }
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

            Spd.Cells[7, 2].Text = String.Format("{0:#,###}", nToTal);
        }

        void Screen_DisPlay_환자현황_퇴원(FarPoint.Win.Spread.SheetView Spd, string ArgDate)
        {
            int i = 0;
            int j = 0;
            int nToTal = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  DEPTCODE, COUNT(PANO) CNT                                                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND ActDate=TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                 "; //전산퇴원기준
            SQL += ComNum.VBLF + "      AND GbSTS <> 'D'                                                                    ";
            SQL += ComNum.VBLF + "GROUP BY DEPTCODE                                                                         ";
            SQL += ComNum.VBLF + "ORDER BY DEPTCODE                                                                         ";

            try
            {
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
                        for (j = 8; j <= 36; j++)
                        {
                            if (Spd.Cells[j, 1].Text == dt.Rows[i]["DEPTCODE"].ToString().Trim())
                            {
                                Spd.Cells[j, 3].Text += dt.Rows[i]["CNT"].ToString().Trim();
                                nToTal += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

            Spd.Cells[7, 3].Text = String.Format("{0:#,###}", nToTal);
        }

        void Screen_DisPlay_환자현황_재원(FarPoint.Win.Spread.SheetView Spd, string ArgDate)
        {
            int i = 0;
            int j = 0;
            int nToTal = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ArgDate == DateTime.Now.ToString("yyyy-MM-dd"))
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                                    ";
                SQL += ComNum.VBLF + "  DEPTCODE, COUNT(PANO) CNT                                                               ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                                 ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                SQL += ComNum.VBLF + "      AND (ACTDATE IS NULL OR ACTDATE >= TRUNC(SYSDATE))                                  "; 
                SQL += ComNum.VBLF + "      AND (OUTDATE >= TRUNC(SYSDATE) OR OUTDATE IS NULL)                                  ";
                SQL += ComNum.VBLF + "      AND INDATE <= TO_DATE('" + ArgDate + " 23:59','YYYY-MM-DD HH24:MI')                 ";
                SQL += ComNum.VBLF + "      AND PANO <> '81000004'                                                              ";
                SQL += ComNum.VBLF + "      AND GBSTS NOT IN ('1','7','9')                                                      ";
                SQL += ComNum.VBLF + "GROUP BY DEPTCODE                                                                         ";
                SQL += ComNum.VBLF + "ORDER BY DEPTCODE                                                                         ";
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                                    ";
                SQL += ComNum.VBLF + "  DEPTCODE, COUNT(PANO) CNT                                                               ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_PATIENT                                                   ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                SQL += ComNum.VBLF + "      AND JOBDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD')                               ";
                SQL += ComNum.VBLF + "      AND PACLASS  = '3'                                                                  "; // 재원자
                SQL += ComNum.VBLF + "      AND PANO <> '81000004'                                                              ";
                SQL += ComNum.VBLF + "GROUP BY DEPTCODE                                                                         ";
                SQL += ComNum.VBLF + "ORDER BY DEPTCODE                                                                         ";
            }
            try
            {
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
                        for (j = 8; j <= 36; j++)
                        {
                            if (Spd.Cells[j, 1].Text == dt.Rows[i]["DEPTCODE"].ToString().Trim())
                            {
                                Spd.Cells[j, 4].Text += dt.Rows[i]["CNT"].ToString().Trim();
                                nToTal += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

            Spd.Cells[7, 4].Text = String.Format("{0:#,###}", nToTal);
        }

        void Screen_DisPlay_환자현황_외래(FarPoint.Win.Spread.SheetView Spd, string ArgDate)
        {
            int i = 0;
            int j = 0;
            int nToTal = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  DECODE(DEPTCODE,'R6','수탁','ER','EM','HD','MN',DEPTCODE) DeptCode, COUNT(Pano) CNT     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND ActDate=TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                 "; //전산퇴원기준
            SQL += ComNum.VBLF + "      AND Jin NOT IN ('D')                                                            ";
            SQL += ComNum.VBLF + "GROUP BY DECODE(DEPTCODE,'R6','수탁','ER','EM','HD','MN',DEPTCODE)                        ";
            SQL += ComNum.VBLF + "ORDER BY DECODE(DEPTCODE,'R6','수탁','ER','EM','HD','MN',DEPTCODE)                        ";

            try
            {
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
                        for (j = 8; j <= 36; j++)
                        {
                            if (Spd.Cells[j, 1].Text == dt.Rows[i]["DEPTCODE"].ToString().Trim())
                            {
                                Spd.Cells[j, 5].Text += dt.Rows[i]["CNT"].ToString().Trim();
                                nToTal += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

            Spd.Cells[7, 5].Text = String.Format("{0:#,###}", nToTal);
        }

        void Screen_DisPlay_환자현황_외래외국인(FarPoint.Win.Spread.SheetView Spd, string ArgDate)
        {
            int i = 0;
            int j = 0;
            int nToTal = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  COUNT(a.Pano) CNT                                                                       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER a, " + ComNum.DB_PMPA + "bas_patient b              ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND ActDate=TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                 ";
            SQL += ComNum.VBLF + "      AND Jin NOT IN ('D')                                                            ";
            SQL += ComNum.VBLF + "      AND a.pano=b.pano                                                                   ";
            SQL += ComNum.VBLF + "      AND GBCOUNTRY <> 'KR'                                                               ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    Spd.Cells[34, 10].Text = dt.Rows[0]["CNT"].ToString().Trim();
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;            
        }

        void Screen_DisPlay_보험별입원환자(FarPoint.Win.Spread.SheetView Spd, string ArgDate)
        {
            int i = 0;
            int j = 0;
            int[] nCnt = new int[6];

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if(ArgDate == DateTime.Now.ToString("yyyy-MM-dd"))
            {
                //윤종필, 입원수속에 병동현황하고 일치 작업함. 2006-03-10
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                                                    ";
                SQL += ComNum.VBLF + "  DECODE(BI,'11','1','12','1','13','1','21','2','22','2','31','3','33','3','52','5','55','5','6') AS aBi, ";
                SQL += ComNum.VBLF + "  COUNT(PANO) CNT                                                                                         ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                                                 ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
                SQL += ComNum.VBLF + "      AND (ACTDATE IS NULL OR ACTDATE >= TRUNC(SYSDATE))                                                  ";
                SQL += ComNum.VBLF + "      AND (OUTDATE >= TRUNC(SYSDATE) OR OUTDATE IS NULL)                                                  ";
                SQL += ComNum.VBLF + "      AND INDATE <= TO_DATE('" + ArgDate + " 23:59','YYYY-MM-DD HH24:MI')                                 ";
                SQL += ComNum.VBLF + "      AND PANO <> '81000004'                                                                              ";
                SQL += ComNum.VBLF + "      AND GBSTS NOT IN ('1','7','9')                                                                      ";
                SQL += ComNum.VBLF + "GROUP BY DECODE(BI,'11','1','12','1','13','1','21','2','22','2','31','3','33','3','52','5','55','5','6')  ";
                SQL += ComNum.VBLF + "ORDER BY DECODE(BI,'11','1','12','1','13','1','21','2','22','2','31','3','33','3','52','5','55','5','6')  ";
            }
            else
            {
                SQL += ComNum.VBLF + "SELECT                                                                                                    ";
                SQL += ComNum.VBLF + "  DECODE(BI,'11','1','12','1','13','1','21','2','22','2','31','3','33','3','52','5','55','5','6') AS aBi, ";
                SQL += ComNum.VBLF + "  COUNT(PANO) CNT                                                                                         ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_PATIENT                                                                   ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
                SQL += ComNum.VBLF + "      AND JOBDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                               ";
                SQL += ComNum.VBLF + "      AND PACLASS  = '3'                                                                                  ";
                SQL += ComNum.VBLF + "      AND PANO <> '81000004'                                                                              ";
                SQL += ComNum.VBLF + "GROUP BY DECODE(BI,'11','1','12','1','13','1','21','2','22','2','31','3','33','3','52','5','55','5','6')  ";
                SQL += ComNum.VBLF + "ORDER BY DECODE(BI,'11','1','12','1','13','1','21','2','22','2','31','3','33','3','52','5','55','5','6')  ";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["aBi"].ToString().Trim())
                        {
                            case "1":
                                nCnt[1] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "2":
                                nCnt[2] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "3":
                                nCnt[3] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "5":
                                nCnt[4] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "6":
                                nCnt[5] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                        }

                        nCnt[0] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                    }
                }

                Spd.Cells[7, 7].Text = String.Format("{0:#,###}", nCnt[0]);
                Spd.Cells[7, 8].Text = String.Format("{0:#,###}", nCnt[1]);
                Spd.Cells[7, 11].Text = String.Format("{0:#,###}", nCnt[2]);
                Spd.Cells[7, 15].Text = String.Format("{0:#,###}", nCnt[3]);
                Spd.Cells[7, 13].Text = String.Format("{0:#,###}", nCnt[4]);
                Spd.Cells[7, 17].Text = String.Format("{0:#,###}", nCnt[5]);
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

        }

        void Screen_DisPlay_보험별외래환자(FarPoint.Win.Spread.SheetView Spd, string ArgDate)
        {
            int i = 0;
            int j = 0;
            int[] nCnt = new int[6];

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                    ";
            SQL += ComNum.VBLF + "  DECODE(BI,'11','1','12','1','13','1','21','2','22','2','23','2','24','2','31','3','32','3','33','3','41','1','42','1','43','1','52','5','6') AS aBi, ";
            SQL += ComNum.VBLF + "  COUNT(Pano) CNT                                                                                         ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
            SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                               ";
            SQL += ComNum.VBLF + "      AND Jin NOT IN ('D')                                                                            ";
            SQL += ComNum.VBLF + "GROUP BY DECODE(BI,'11','1','12','1','13','1','21','2','22','2','23','2','24','2','31','3','32','3','33','3','41','1','42','1','43','1','52','5','6')  ";
            SQL += ComNum.VBLF + "ORDER BY DECODE(BI,'11','1','12','1','13','1','21','2','22','2','23','2','24','2','31','3','32','3','33','3','41','1','42','1','43','1','52','5','6')  ";

            try
            {
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
                        switch (dt.Rows[i]["aBi"].ToString().Trim())
                        {
                            case "1":
                                nCnt[1] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "2":
                                nCnt[2] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "3":
                                nCnt[3] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "5":
                                nCnt[4] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "6":
                                nCnt[5] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                        }

                        nCnt[0] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                    }
                }

                Spd.Cells[11, 7].Text = String.Format("{0:#,###}", nCnt[0]);
                Spd.Cells[11, 8].Text = String.Format("{0:#,###}", nCnt[1]);
                Spd.Cells[11, 11].Text = String.Format("{0:#,###}", nCnt[2]);
                Spd.Cells[11, 15].Text = String.Format("{0:#,###}", nCnt[3]);
                Spd.Cells[11, 13].Text = String.Format("{0:#,###}", nCnt[4]);
                Spd.Cells[11, 17].Text = String.Format("{0:#,###}", nCnt[5]);
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
        }

        void Screen_DisPlay_무인수납(FarPoint.Win.Spread.SheetView Spd, string ArgDate)
        {
            int i = 0;
            int j = 0;
            int nTotal = 0;
            int[] nCnt = new int[8];

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                    ";
            SQL += ComNum.VBLF + "  PART, COUNT(Pano) CNT                                                                                   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SUNAP                                                                      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
            SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                               ";
            SQL += ComNum.VBLF + "      AND PART IN ('5001','5002','5003','5004','5005','5006','5050' )                                     ";
            SQL += ComNum.VBLF + "GROUP BY PART                                                                                             ";
            SQL += ComNum.VBLF + "ORDER BY PART                                                                                             ";

            try
            {
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
                        switch (dt.Rows[i]["PART"].ToString().Trim())
                        {
                            case "5001":
                                nCnt[1] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "5002":
                                nCnt[2] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "5003":
                                nCnt[3] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "5004":
                                nCnt[4] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "5005":
                                nCnt[5] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "5006":
                                nCnt[6] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            case "5050":
                                nCnt[7] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                        }

                        nCnt[0] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                    }
                }

                Spd.Cells[14, 18].Text = String.Format("{0:#,###}", nCnt[7]);

                Spd.Cells[17, 7].Text = String.Format("{0:#,###}", nCnt[0]);
                Spd.Cells[17, 8].Text = String.Format("{0:#,###}", nCnt[1]);
                Spd.Cells[17, 10].Text = String.Format("{0:#,###}", nCnt[2]);
                Spd.Cells[17, 12].Text = String.Format("{0:#,###}", nCnt[3]);
                Spd.Cells[17, 14].Text = String.Format("{0:#,###}", nCnt[4]);
                Spd.Cells[17, 16].Text = String.Format("{0:#,###}", nCnt[5]);
                Spd.Cells[17, 18].Text = String.Format("{0:#,###}", nCnt[6]);
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null; 
        }

        void Screen_DisPlay_예약부도(FarPoint.Win.Spread.SheetView Spd, string ArgDate)
        {
            int i = 0;
            int j = 0;
            int nTotal = 0;
            int[,] nCnt = new int[3, 5];

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                    ";
            SQL += ComNum.VBLF + "  GUBUN, SUM(CAMT) CSamt, 0 RSAmt, COUNT(PANO) CNT                                                        ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_REFUND                                                                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
            SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                               ";
            SQL += ComNum.VBLF + "GROUP BY GUBUN                                                                                            ";

            SQL += ComNum.VBLF + "UNION ALL                                                                                                 ";

            SQL += ComNum.VBLF + "SELECT                                                                                                    ";
            SQL += ComNum.VBLF + "  GUBUN, 0 CSamt, SUM(RAMT) RSAmt ,COUNT(PANO) CNT                                                        ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_REFUND                                                                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
            SQL += ComNum.VBLF + "      AND RDATE >= TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                                ";
            SQL += ComNum.VBLF + "      AND RDATE < TO_DATE('" + Convert.ToDateTime(ArgDate).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "GROUP BY GUBUN                                                                                            ";
            SQL += ComNum.VBLF + "ORDER BY 1 DESC                                                                                           ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        //예약
                        if(dt.Rows[i]["GUBUN"].ToString().Trim() == "00")
                        {
                            if(Convert.ToInt32(dt.Rows[i]["CSamt"].ToString().Trim()) > 0)
                            {
                                nCnt[0, 1] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim()); //건수
                            }
                            nCnt[0, 2] += Convert.ToInt32(dt.Rows[i]["CSamt"].ToString().Trim());   //부도금액
                            nCnt[0, 3] += Convert.ToInt32(dt.Rows[i]["RSAmt"].ToString().Trim());   //환불금액
                            nCnt[0, 4] += Convert.ToInt32(dt.Rows[i]["CSamt"].ToString().Trim()) - Convert.ToInt32(dt.Rows[i]["RSAmt"].ToString().Trim()); //차액금
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[i]["CSamt"].ToString().Trim()) > 0)
                            {
                                nCnt[1, 1] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim()); //건수
                            }
                            nCnt[1, 2] += Convert.ToInt32(dt.Rows[i]["CSamt"].ToString().Trim());   //부도금액
                            nCnt[1, 3] += Convert.ToInt32(dt.Rows[i]["RSAmt"].ToString().Trim());   //환불금액
                            nCnt[1, 4] += Convert.ToInt32(dt.Rows[i]["CSamt"].ToString().Trim()) - Convert.ToInt32(dt.Rows[i]["RSAmt"].ToString().Trim()); //차액금
                        }
                    }

                    //예약
                    Spd.Cells[22, 8].Text = String.Format("{0:#,###}", nCnt[0, 1]);
                    Spd.Cells[22, 10].Text = String.Format("{0:#,###}", nCnt[0, 2]);
                    Spd.Cells[22, 13].Text = String.Format("{0:#,###}", nCnt[0, 3]);
                    Spd.Cells[22, 15].Text = String.Format("{0:#,###}", nCnt[0, 4]);

                    //접수
                    Spd.Cells[23, 8].Text = String.Format("{0:#,###}", nCnt[1, 1]);
                    Spd.Cells[23, 10].Text = String.Format("{0:#,###}", nCnt[1, 2]);
                    Spd.Cells[23, 13].Text = String.Format("{0:#,###}", nCnt[1, 3]);
                    Spd.Cells[23, 15].Text = String.Format("{0:#,###}", nCnt[1, 4]);

                }
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

            //예약검사 부도
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                    ";
            SQL += ComNum.VBLF + "  SUM(CAMT) CSamt, 0 RSAmt, COUNT(PANO) CNT                                                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_REFUND_EXAM                                                                ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
            SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                               ";

            SQL += ComNum.VBLF + "UNION ALL                                                                                                 ";

            SQL += ComNum.VBLF + "SELECT                                                                                                    ";
            SQL += ComNum.VBLF + "  0 CSamt, SUM(RAMT) RSAmt ,COUNT(PANO) CNT                                                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_REFUND_EXAM                                                                ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
            SQL += ComNum.VBLF + "      AND RDATE >= TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                                ";
            SQL += ComNum.VBLF + "      AND RDATE < TO_DATE('" + Convert.ToDateTime(ArgDate).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "ORDER BY 1 DESC                                                                                           ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["CSamt"].ToString().Trim() != "")
                        {
                            if (Convert.ToInt32(dt.Rows[i]["CSamt"].ToString().Trim()) > 0)
                            {
                                nCnt[2, 1] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());     //건수
                            }
                        }
                        if (dt.Rows[i]["CSamt"].ToString().Trim() != "")
                        {
                            nCnt[2, 2] += Convert.ToInt32(dt.Rows[i]["CSamt"].ToString().Trim());       //부도금액
                        }
                        if (dt.Rows[i]["RSAmt"].ToString().Trim() != "")
                        {
                            nCnt[2, 3] += Convert.ToInt32(dt.Rows[i]["RSAmt"].ToString().Trim());       //환불금액
                        }
                        if(dt.Rows[i]["CSamt"].ToString().Trim() == "" || dt.Rows[i]["RSAmt"].ToString().Trim() == "")
                        {
                            if(dt.Rows[i]["CSamt"].ToString().Trim() == "")
                            {
                                nCnt[2, 4] += 0 - Convert.ToInt32(dt.Rows[i]["RSAmt"].ToString().Trim());       //차액금
                            }
                            else if(dt.Rows[i]["RSAmt"].ToString().Trim() == "")
                            {
                                nCnt[2, 4] += Convert.ToInt32(dt.Rows[i]["CSamt"].ToString().Trim()) - 0;       //차액금
                            }
                            else
                            {
                                nCnt[2, 4] += Convert.ToInt32(dt.Rows[i]["CSamt"].ToString().Trim()) - Convert.ToInt32(dt.Rows[i]["RSAmt"].ToString().Trim());       //차액금
                            }
                        }
                    }
                }

                //검사
                Spd.Cells[24, 8].Text = String.Format("{0:#,###}", nCnt[2, 1]);
                Spd.Cells[24, 10].Text = String.Format("{0:#,###}", nCnt[2, 2]);
                Spd.Cells[24, 13].Text = String.Format("{0:#,###}", nCnt[2, 3]);
                Spd.Cells[24, 15].Text = String.Format("{0:#,###}", nCnt[2, 4]);

            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
        }

        void Screen_DisPlay_진단서(FarPoint.Win.Spread.SheetView Spd, string ArgDate)
        {
            int i = 0;
            int j = 0;
            int nTotal = 0;
            int[] nCnt = new int[13];

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                    ";
            SQL += ComNum.VBLF + "  SUNEXT, COUNT(PANO) CNT                                                                                 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP                                                                       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
            SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                               ";
            SQL += ComNum.VBLF + "      AND (SUNEXT IN (SELECT SUCODE FROM BAS_SUT WHERE BUN  = '75'                                        ";
            SQL += ComNum.VBLF + "      AND SUCODE LIKE 'ZA%' ) OR                                                                          ";
            SQL += ComNum.VBLF + "          SUNEXT ='AA333A'  OR  SUNEXT ='Y75' )                                                           ";
            SQL += ComNum.VBLF + "      AND ROWID NOT IN (                                                                                  ";
            SQL += ComNum.VBLF + "          SELECT TableROWID FROM KOSMOS_PMPA.OPD_SLIP_JDEL                                                ";
            SQL += ComNum.VBLF + "          WHERE ACTDATE >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') )                                      ";
            SQL += ComNum.VBLF + "GROUP BY SUNEXT                                                                                           ";            

            try
            {
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
                        switch (dt.Rows[i]["SUNEXT"].ToString().Trim())
                        {
                            //일반
                            case "ZA01":
                                nCnt[1] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            //사망, 사체
                            case "ZA02":
                            case "ZA11":
                                nCnt[2] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            //병사용
                            case "ZA03":
                                nCnt[3] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            //심신장애
                            case "ZA48":
                            case "ZA49":
                                nCnt[4] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            //후유장애
                            case "ZA60":
                                nCnt[5] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            //소견서
                            case "ZA06":
                                nCnt[6] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            //입사증
                            case "ZA08":
                                nCnt[7] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            //보장구
                            case "AA333A":
                                nCnt[8] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            //출생증명
                            case "ZA07":
                                nCnt[9] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            //근로능력
                            case "ZA67":
                                nCnt[10] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            //진료의뢰
                            case "Y75":
                                nCnt[11] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            //기타
                            default:
                                nCnt[12] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;

                        }

                        nCnt[0] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                    }
                }

                Spd.Cells[28, 7].Text = String.Format("{0:#,###}", nCnt[0]);
                Spd.Cells[28, 8].Text = String.Format("{0:#,###}", nCnt[1]);
                Spd.Cells[28, 10].Text = String.Format("{0:#,###}", nCnt[2]);
                Spd.Cells[28, 12].Text = String.Format("{0:#,###}", nCnt[3]);
                Spd.Cells[28, 14].Text = String.Format("{0:#,###}", nCnt[4]);
                Spd.Cells[28, 16].Text = String.Format("{0:#,###}", nCnt[5]);
                Spd.Cells[28, 18].Text = String.Format("{0:#,###}", nCnt[6]);

                Spd.Cells[30, 8].Text = String.Format("{0:#,###}", nCnt[7]);
                Spd.Cells[30, 10].Text = String.Format("{0:#,###}", nCnt[8]);
                Spd.Cells[30, 12].Text = String.Format("{0:#,###}", nCnt[9]);
                Spd.Cells[30, 14].Text = String.Format("{0:#,###}", nCnt[10]);
                Spd.Cells[30, 16].Text = String.Format("{0:#,###}", nCnt[11]);
                Spd.Cells[30, 18].Text = String.Format("{0:#,###}", nCnt[12]);
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
        }

        void Screen_DisPlay_일일수입(FarPoint.Win.Spread.SheetView Spd, string ArgDate)
        {
            int i = 0;
            int j = 0;
            int[,] nAMT = new int[6, 3];
            int[] nCnt = new int[4];
            int nOpdAmt = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                    ";
            SQL += ComNum.VBLF + "  Bun, SUM(Amt) SAMT,COUNT(BUN) CNT                                                                       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH                                                                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
            SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                               ";
            SQL += ComNum.VBLF + "      AND Bun IN ('85','87','89','91')                                                                    ";
            SQL += ComNum.VBLF + "      AND Part <> '#'                                                                                     ";
            SQL += ComNum.VBLF + "GROUP BY BUN                                                                                              ";
            SQL += ComNum.VBLF + "ORDER BY BUN                                                                                              ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["BUN"].ToString().Trim())
                        {
                            //가퇴원금
                            case "85":
                                nAMT[3, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nCnt[1] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            //중간납
                            case "87":
                                nAMT[4, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nCnt[2] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                            //본인부담
                            case "89":
                                nAMT[2, 1] += (Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim()) / 10) * 10; 
                                break;
                            //환불
                            case "91":
                                ///////////////////////// 여기서 오류남

                                nAMT[5, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nCnt[3] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                                break;
                        }

                        if(dt.Rows[i]["Bun"].ToString().Trim() == "89")
                        {
                            nAMT[1, 1] += (Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim()) / 10) * 10;    //계
                        }
                        else
                        {
                            nAMT[1, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());    //계
                        }
                    }

                    Spd.Cells[3, 2].Text = String.Format("{0:#,###}", nAMT[1, 1]);
                    Spd.Cells[3, 5].Text = String.Format("{0:#,###}", nAMT[2, 1]);
                    Spd.Cells[3, 7].Text = String.Format("{0:#,###}", nCnt[1]);
                    Spd.Cells[3, 8].Text = String.Format("{0:#,###}", nAMT[3, 1]);
                    Spd.Cells[3, 9].Text = String.Format("{0:#,###}", nCnt[2]);
                    Spd.Cells[3, 10].Text = String.Format("{0:#,###}", nAMT[4, 1]);
                    Spd.Cells[3, 11].Text = String.Format("{0:#,###}", nCnt[3]);
                    Spd.Cells[3, 12].Text = String.Format("{0:#,###}", nAMT[5, 1]);
                    
                }
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  SUM(AMT43) SAMT";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALDAILY";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ActDate=To_date('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND IPDOPD ='O'";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    nOpdAmt = Convert.ToInt32(dt.Rows[0]["SAMT"].ToString().Trim());
                }

                Spd.Cells[4, 2].Text = String.Format("{0:#,###}", nOpdAmt);
                Spd.Cells[4, 5].Text = String.Format("{0:#,###}", nOpdAmt);

                Spd.Cells[6, 2].Text = String.Format("{0:#,###}", (nAMT[1, 1] + nOpdAmt));
                Spd.Cells[6, 5].Text = String.Format("{0:#,###}", (nAMT[2, 1] + nOpdAmt));
                Spd.Cells[6, 7].Text = nCnt[1].ToString();
                Spd.Cells[6, 8].Text = String.Format("{0:#,###}", nAMT[3, 1]);
                Spd.Cells[6, 9].Text = nCnt[2].ToString();
                Spd.Cells[6, 10].Text = String.Format("{0:#,###}", nAMT[4, 1]);
                Spd.Cells[6, 11].Text = nCnt[3].ToString();
                Spd.Cells[6, 12].Text = String.Format("{0:#,###}", nAMT[5, 1]);
            }   
            
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

        }

        void Screen_DisPlay_감액내역(FarPoint.Win.Spread.SheetView Spd, string ArgDate)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int[,] nAMT = new int[9, 6];
            
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                    ";
            SQL += ComNum.VBLF + "  A.GbGamek, SUM(C.Amt) SAMT                                                                              ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH C, " + ComNum.DB_PMPA + "IPD_TRANS A                              ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
            SQL += ComNum.VBLF + "      AND C.ActDate = TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                             ";
            SQL += ComNum.VBLF + "      AND C.Bun     = '92'                                                                                ";
            SQL += ComNum.VBLF + "      AND C.TRSNO = A.TRSNO(+)                                                                            ";
            SQL += ComNum.VBLF + "GROUP BY A.GbGamek                                                                                        ";
            SQL += ComNum.VBLF + "ORDER BY A.GbGamek                                                                                        ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["GbGameK"].ToString().Trim())
                        {
                            case "11":
                            case "12":
                                nAMT[1, 2] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[1, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;
                            case "13":
                            case "14":
                                nAMT[1, 3] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[1, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;
                            case "21":
                            case "22":
                            case "23":
                            case "24":
                                nAMT[2, 3] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[2, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;
                            case "26":
                                nAMT[3, 2] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[3, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;
                            case "27":
                                nAMT[3, 3] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[3, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;
                            case "55":
                                nAMT[4, 2] = nAMT[3, 2] + Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[4, 1] += + Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;
                            case "51":
                                nAMT[6, 2] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[6, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;
                            default:
                                nAMT[7, 2] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[7, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;
                        }
                        nAMT[8, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                        nAMT[8, 2] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                    }
                }
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                    ";
            SQL += ComNum.VBLF + "  SUNEXT, SUM(AMT1+AMT2) SAMT                                                                             ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Opd_Slip                                                                       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
            SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                               ";
            SQL += ComNum.VBLF + "      AND Bun     = '92'                                                                                  ";
            SQL += ComNum.VBLF + "      AND PANO <> '81000004'                                                                              ";
            SQL += ComNum.VBLF + "GROUP BY SUNEXT                                                                                           ";
            SQL += ComNum.VBLF + "ORDER BY SUNEXT                                                                                           ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["SUNEXT"].ToString().Trim())
                        {
                            case "Y92A":
                            case "Y92B":
                                nAMT[1, 4] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[1, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;

                            case "Y92C":
                            case "Y92D":
                                nAMT[1, 5] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[1, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;

                            case "Y92I":
                                nAMT[2, 4] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[2, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;

                            case "Y92J":
                            case "Y92K":
                            case "Y92L":
                                nAMT[2, 5] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[2, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;

                            case "Y92Q":
                                nAMT[3, 4] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[3, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;

                            case "Y92R":
                                nAMT[3, 5] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[3, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;

                            case "Y92O":
                                nAMT[4, 4] = nAMT[3, 4] + Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[4, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;

                            case "Y92X":
                                nAMT[5, 4] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[5, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;

                            case "Y925":
                                nAMT[6, 4] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[6, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;

                            default:
                                nAMT[7, 4] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                nAMT[7, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                                break;

                        }
                        nAMT[8, 4] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                        nAMT[8, 1] += Convert.ToInt32(dt.Rows[i]["SAMT"].ToString().Trim());
                    }

                    for(i = 11; i <= 18; i++)
                    {
                        Spd.Cells[i-1 , 2].Text = String.Format("{0:#,###}", nAMT[i - 11, 1]);
                    }

                    for (i = 12; i <= 18; i++)
                    {
                        Spd.Cells[i-1 , 5].Text = String.Format("{0:#,###}", nAMT[i - 11, 2]);
                    }

                    for (i = 12; i <= 14; i++)
                    {
                        Spd.Cells[i-1 , 7].Text = String.Format("{0:#,###}", nAMT[i - 11, 3]);
                    }

                    for (i = 12; i <= 18; i++)
                    {
                        Spd.Cells[i -1, 9].Text = String.Format("{0:#,###}", nAMT[i - 11, 4]);
                    }

                    for (i = 12; i <= 18; i++)
                    {
                        Spd.Cells[i -1, 11].Text = String.Format("{0:#,###}", nAMT[i - 11, 5]);
                    }

                    Spd.Cells[19, 2].Text = String.Format("{0:#,###}", nAMT[8, 1]);
                    Spd.Cells[19, 5].Text = String.Format("{0:#,###}", nAMT[8, 2]);
                    Spd.Cells[19, 7].Text = String.Format("{0:#,###}", nAMT[8, 3]);
                    Spd.Cells[19, 9].Text = String.Format("{0:#,###}", nAMT[8, 4]);
                    Spd.Cells[19, 11].Text = String.Format("{0:#,###}", nAMT[8, 5]);
                }


            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
        }

        void Screen_DisPlay_미수내역(FarPoint.Win.Spread.SheetView Spd, string ArgDate)
        {
            int i = 0;
            int j = 0;
            int k = 0;

            int[] nCnt = new int[5];
            int[] nAMT = new int[5];
            int[] nTCNT = new int[3];
            int[] nTAMT = new int[3];

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  sum(Amt) Amt,substr(misudtl,1,1) IO ,gubun1,count(*)cnt ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND BDate >= TO_DATE('" + ArgDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND BDate <= TO_DATE('" + ArgDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "GROUP BY rollup(substr(misudtl,1,1),gubun1)";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        switch (dt.Rows[i]["IO"].ToString().Trim())
                        {
                            //입원
                            case "I":
                                switch (dt.Rows[i]["gubun1"].ToString().Trim())
                                {
                                    //미수
                                    case "1":
                                        nCnt[1] = Convert.ToInt32(dt.Rows[i]["cnt"].ToString().Trim());     
                                        nAMT[1] = Convert.ToInt32(dt.Rows[i]["AMT"].ToString().Trim());
                                        nTCNT[1] += nCnt[1];
                                        nTAMT[1] += nAMT[1];
                                        break;

                                    //입금
                                    case "2":
                                        nCnt[2] = Convert.ToInt32(dt.Rows[i]["cnt"].ToString().Trim());
                                        nAMT[2] = Convert.ToInt32(dt.Rows[i]["AMT"].ToString().Trim());
                                        nTCNT[2] += nCnt[2];
                                        nTAMT[2] += nAMT[2];
                                        break;
                                }
                                break;

                            //외래
                            case "O":
                                switch (dt.Rows[i]["gubun1"].ToString().Trim())
                                {
                                    //미수
                                    case "1":
                                        nCnt[3] = Convert.ToInt32(dt.Rows[i]["cnt"].ToString().Trim());
                                        nAMT[3] = Convert.ToInt32(dt.Rows[i]["AMT"].ToString().Trim());
                                        nTCNT[1] += nCnt[3];
                                        nTAMT[1] += nAMT[3];
                                        break;

                                    //입금
                                    case "2":
                                        nCnt[4] = Convert.ToInt32(dt.Rows[i]["cnt"].ToString().Trim());
                                        nAMT[4] = Convert.ToInt32(dt.Rows[i]["AMT"].ToString().Trim());
                                        nTCNT[2] += nCnt[4];
                                        nTAMT[2] += nAMT[4];
                                        break;
                                }
                                break;
                        }
                    }

                    Spd.Cells[24, 2].Text = String.Format("{0:#,###}", nCnt[1]);
                    Spd.Cells[24, 5].Text = String.Format("{0:#,###}", nAMT[1]);
                    Spd.Cells[24, 8].Text = String.Format("{0:#,###}", nCnt[2]);
                    Spd.Cells[24, 10].Text = String.Format("{0:#,###}", nAMT[2]);

                    Spd.Cells[25, 2].Text = String.Format("{0:#,###}", nCnt[3]);
                    Spd.Cells[25, 5].Text = String.Format("{0:#,###}", nAMT[3]);
                    Spd.Cells[25, 8].Text = String.Format("{0:#,###}", nCnt[4]);
                    Spd.Cells[25, 10].Text = String.Format("{0:#,###}", nAMT[4]);

                    Spd.Cells[27, 2].Text = String.Format("{0:#,###}", nTCNT[1]);
                    Spd.Cells[27, 5].Text = String.Format("{0:#,###}", nTAMT[1]);
                    Spd.Cells[27, 8].Text = String.Format("{0:#,###}", nTCNT[2]);
                    Spd.Cells[27, 10].Text = String.Format("{0:#,###}", nTAMT[2]);
                }


            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
        }


    }
}
