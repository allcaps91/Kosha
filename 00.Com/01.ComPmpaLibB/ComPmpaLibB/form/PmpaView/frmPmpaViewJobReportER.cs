using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewJobReportER.cs
    /// Description     : 원무팀 업무일지(응급실)
    /// Author          : 안정수
    /// Create Date     : 2017-08-18
    /// Update History  : 2017-10-24    
    /// <history>       
    /// d:\psmh\OPD\Wonmu\Frm업무일지_응급실.frm(Frm업무일지_응급실) => frmPmpaViewJobReportER.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\Wonmu\Frm업무일지_응급실.frm(Frm업무일지_응급실)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewJobReportER : Form, MainFormMessage
    {
        ComFunc CF = new ComFunc();
        string FstrFDate = "";
        string FstrTDate = "";
        string FstrFTime = "";
        string FstrTTime = "";
        string FstrRowid = "";

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

        public frmPmpaViewJobReportER(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmPmpaViewJobReportER()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);
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
            string CurrentTime = DateTime.Now.ToString("HH:mm");

            tabControl1.SelectedTabIndex = 0;

            if (String.Compare(CurrentTime, "12:00") >= 0)
            {
                dtpFDate.Text = CurrentDate;
                dtpFTime.Text = "08:00";
                dtpTTime.Text = "17:00";
            }
            else
            {
                dtpFDate.Text = Convert.ToDateTime(CurrentDate).AddDays(-1).ToShortDateString();
                dtpFTime.Text = "17:00";
                dtpTTime.Text = "08:00";
            }

            dtpTDate.Text = CurrentDate;

            Screen_Clear();
        }

        void Screen_Clear()
        {
            int i = 0;
            int j = 0;

            //날짜일시
            ssList1_Sheet1.Cells[2, 1].Text = "";
            ssList1_Sheet1.Cells[2, 1].Text = VB.Left(dtpFDate.Text, 4) + " 년  ";
            ssList1_Sheet1.Cells[2, 1].Text += ComFunc.SetAutoZero(VB.Mid(dtpFDate.Text, 6, 2), 1) + " 월  ";
            ssList1_Sheet1.Cells[2, 1].Text += ComFunc.SetAutoZero(VB.Mid(dtpFDate.Text, 9, 2), 1) + " 일  ";
            ssList1_Sheet1.Cells[2, 1].Text += CF.READ_YOIL(clsDB.DbCon, dtpFDate.Text);

            //DAY, KEEP,NIGHT
            ssList1_Sheet1.Cells[5, 7].Text = ""; ssList1_Sheet1.Cells[5, 19].Text = "";
            ssList1_Sheet1.Cells[6, 7].Text = ""; ssList1_Sheet1.Cells[6, 19].Text = "";
            ssList1_Sheet1.Cells[7, 7].Text = ""; ssList1_Sheet1.Cells[7, 19].Text = "";

            ssList1_Sheet1.Cells[10, 20].Text = "";

            //당일진료횟수
            ssList1_Sheet1.Cells[11, 2].Text = ""; ssList1_Sheet1.Cells[11, 5].Text = "";
            ssList1_Sheet1.Cells[11, 8].Text = ""; ssList1_Sheet1.Cells[11, 12].Text = ""; ssList1_Sheet1.Cells[11, 20].Text = "";

            ssList1_Sheet1.Cells[12, 2].Text = ""; ssList1_Sheet1.Cells[12, 5].Text = ""; ssList1_Sheet1.Cells[12, 8].Text = "";
            ssList1_Sheet1.Cells[12, 7].Text = ""; ssList1_Sheet1.Cells[12, 12].Text = "";

            //당일진료비 내역
            ssList1_Sheet1.Cells[16, 1].Text = ""; ssList1_Sheet1.Cells[16, 3].Text = "";
            ssList1_Sheet1.Cells[16, 8].Text = ""; ssList1_Sheet1.Cells[16, 12].Text = "";
            ssList1_Sheet1.Cells[16, 16].Text = ""; ssList1_Sheet1.Cells[16, 18].Text = "";
            ssList1_Sheet1.Cells[16, 20].Text = ""; ssList1_Sheet1.Cells[16, 22].Text = "";

            ssList1_Sheet1.Cells[17, 3].Text = ""; ssList1_Sheet1.Cells[17, 8].Text = ""; ssList1_Sheet1.Cells[17, 12].Text = "";

            //진료과별 환자 현황
            ssList1_Sheet1.Cells[21, 1].Text = "";
            ssList1_Sheet1.Cells[21, 2].Text = ""; ssList1_Sheet1.Cells[21, 3].Text = ""; ssList1_Sheet1.Cells[21, 5].Text = "";
            ssList1_Sheet1.Cells[21, 7].Text = ""; ssList1_Sheet1.Cells[21, 8].Text = ""; ssList1_Sheet1.Cells[21, 10].Text = "";
            ssList1_Sheet1.Cells[21, 11].Text = ""; ssList1_Sheet1.Cells[21, 13].Text = ""; ssList1_Sheet1.Cells[21, 15].Text = "";
            ssList1_Sheet1.Cells[21, 16].Text = ""; ssList1_Sheet1.Cells[21, 18].Text = ""; ssList1_Sheet1.Cells[21, 20].Text = "";
            ssList1_Sheet1.Cells[21, 22].Text = "";

            ssList1_Sheet1.Cells[23, 2].Text = ""; ssList1_Sheet1.Cells[23, 3].Text = ""; ssList1_Sheet1.Cells[23, 5].Text = "";
            ssList1_Sheet1.Cells[23, 7].Text = ""; ssList1_Sheet1.Cells[23, 8].Text = ""; ssList1_Sheet1.Cells[23, 10].Text = "";
            ssList1_Sheet1.Cells[23, 11].Text = ""; ssList1_Sheet1.Cells[23, 18].Text = "";

            //입원환자
            for (i = 27; i <= 47; i++)
            {
                ssList1_Sheet1.Cells[i, 1].Text = "";
                ssList1_Sheet1.Cells[i, 2].Text = "";
                ssList1_Sheet1.Cells[i, 3].Text = "";
                ssList1_Sheet1.Cells[i, 7].Text = "";
                ssList1_Sheet1.Cells[i, 8].Text = "";
                ssList1_Sheet1.Cells[i, 15].Text = "";
                ssList1_Sheet1.Cells[i, 16].Text = "";
                ssList1_Sheet1.Cells[i, 18].Text = "";
                ssList1_Sheet1.Cells[i, 20].Text = "";
            }

            for (i = 3; i <= 22; i++)
            {
                ssList2_Sheet1.Cells[i, 1].Text = ""; ssList2_Sheet1.Cells[i, 3].Text = ""; ssList2_Sheet1.Cells[i, 4].Text = "";
                ssList2_Sheet1.Cells[i, 6].Text = ""; ssList2_Sheet1.Cells[i, 9].Text = ""; ssList2_Sheet1.Cells[i, 12].Text = "";
                ssList2_Sheet1.Cells[i, 14].Text = ""; ssList2_Sheet1.Cells[i, 18].Text = "";
            }

            for (i = 26; i <= 28; i++)
            {
                ssList2_Sheet1.Cells[i, 1].Text = ""; ssList2_Sheet1.Cells[i, 3].Text = ""; ssList2_Sheet1.Cells[i, 4].Text = "";
                ssList2_Sheet1.Cells[i, 6].Text = ""; ssList2_Sheet1.Cells[i, 9].Text = ""; ssList2_Sheet1.Cells[i, 12].Text = "";
                ssList2_Sheet1.Cells[i, 16].Text = ""; ssList2_Sheet1.Cells[i, 18].Text = "";
            }

            for (i = 32; i <= 34; i++)
            {
                ssList2_Sheet1.Cells[i, 1].Text = ""; ssList2_Sheet1.Cells[i, 3].Text = ""; ssList2_Sheet1.Cells[i, 4].Text = "";
                ssList2_Sheet1.Cells[i, 6].Text = ""; ssList2_Sheet1.Cells[i, 9].Text = ""; ssList2_Sheet1.Cells[i, 12].Text = "";
                ssList2_Sheet1.Cells[i, 15].Text = ""; ssList2_Sheet1.Cells[i, 17].Text = "";
            }

            for (i = 38; i <= 40; i++)
            {
                ssList2_Sheet1.Cells[i, 1].Text = ""; ssList2_Sheet1.Cells[i, 3].Text = ""; ssList2_Sheet1.Cells[i, 4].Text = "";
                ssList2_Sheet1.Cells[i, 6].Text = ""; ssList2_Sheet1.Cells[i, 12].Text = ""; ssList2_Sheet1.Cells[i, 15].Text = "";
            }

            ssList2_Sheet1.Cells[42, 1].Text = "기타 특이 사항";
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                //
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                //
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }

            else if (sender == this.btnSave)
            {
                //
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                //2017-07-06 저장 기능 추가
                Screen_DisPlay_진료비저장내용저장(dtpFDate.Text + dtpFTime.Text);
            }
        }

        void ePrint()
        {
            if (tabItem1.IsSelected == true)
            {
                clsSpread SPR = new clsSpread();
                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;              

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 65, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, true, false);

                SPR.setSpdPrint(ssList1, PrePrint, setMargin, setOption, strHeader, strFooter);              
            }

            else
            {
                clsSpread SPR = new clsSpread();
                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;            

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 65, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, true, false);

                SPR.setSpdPrint(ssList2, PrePrint, setMargin, setOption, strHeader, strFooter);              
            }
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            string strDay = "";
            string strDUTY = "";
            string StrJikCode = "";

            string strDname1 = "";
            string strDname2 = "";
            string strDname3 = "";
            string strDname4 = "";
            string strDname5 = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            Screen_Clear();

            Cursor.Current = Cursors.WaitCursor;

            FstrFDate = dtpFDate.Text;
            FstrTDate = dtpTDate.Text;
            FstrFTime = dtpFTime.Text;
            FstrTTime = dtpTTime.Text;

            //2017-07-06 저장기능 추가
            Screen_DisPlay_진료비저장내용호출(FstrFDate + " " + FstrFTime);

            Screen_DisPlay_응급실환자현황();

            Screen_DisPlay_응급실입원리스트();

            Screen_DisPlay_의뢰환자리스트();

            //총재원자수
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  COUNT(PANO) CNT                                                                         ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND ( OUTDATE IS NULL OR OUTDATE >=TO_DATE('" + FstrTDate + "','YYYY-MM-DD') )      ";
            SQL += ComNum.VBLF + "      AND ACTDATE IS NULL                                                                 ";

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
                    if (Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim()) > 0)
                    {
                        ssList1_Sheet1.Cells[10, 20].Text = dt.Rows[0]["CNT"].ToString().Trim() + " 명";
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

            //장례식장 현황
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                           ";
            SQL += ComNum.VBLF + "  COUNT(*) CNT                                                                                                   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "FMS_CUST                                                                               ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                        ";
            SQL += ComNum.VBLF + "      AND ANDATE <TO_DATE('" + dtpTDate.Text + " " + dtpTTime.Text + "','YYYY-MM-DD HH24:MI')                         ";
            SQL += ComNum.VBLF + "      AND nvl(BALDATE,sysdate) >=TO_DATE('" + dtpTDate.Text + " " + dtpTTime.Text + "','YYYY-MM-DD HH24:MI')     ";
            SQL += ComNum.VBLF + "      AND GBDEL = '0'                                                                                            ";

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
                    if (Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim()) > 0)
                    {
                        ssList1_Sheet1.Cells[11, 20].Text = "안치  " + dt.Rows[0]["CNT"].ToString().Trim() + " 구";
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

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                      ";
            SQL += ComNum.VBLF + "* FROM " + ComNum.DB_PMPA + "ETC_DANGJIK                                    ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                   ";
            SQL += ComNum.VBLF + "      AND GUBUN='20'                                                        ";
            SQL += ComNum.VBLF + "      AND tdate = TO_DATE('" + FstrFDate + "','YYYY-MM-DD')                 ";
            SQL += ComNum.VBLF + "      AND ROWNUM = 1                                                        ";

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
                    strDname1 = dt.Rows[0]["DNAME1"].ToString().Trim();
                    strDname2 = dt.Rows[0]["DNAME2"].ToString().Trim();
                    strDname3 = dt.Rows[0]["DNAME3"].ToString().Trim();
                    strDname4 = dt.Rows[0]["DNAME4"].ToString().Trim();

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                      ";
                    SQL += ComNum.VBLF + "* FROM " + ComNum.DB_PMPA + "ETC_DANGJIK                                    ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                   ";
                    SQL += ComNum.VBLF + "      AND GUBUN='19'                                                        ";
                    SQL += ComNum.VBLF + "      AND tdate = TO_DATE('" + FstrFDate + "','YYYY-MM-DD')                 ";
                    SQL += ComNum.VBLF + "      AND ROWNUM = 1                                                        ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        strDname5 = dt1.Rows[0]["DNAME1"].ToString().Trim();

                    }

                    ssList1.ActiveSheet.Cells[5, 19].Text = strDname1;
                    ssList1.ActiveSheet.Cells[6, 19].Text = strDname3;
                    ssList1.ActiveSheet.Cells[7, 19].Text = strDname4 + ", " + strDname5;

                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt1.Dispose();
            dt1 = null;
            dt.Dispose();
            dt = null;


            Screen_DisPlay_응급실진료비내역();
            Screen_DisPlay_응급실과별환자();
            Screen_DisPlay_응급실종별환자();

            Cursor.Current = Cursors.Default;
        }

        //2017-07-06 저장기능 추가
        void Screen_DisPlay_진료비저장내용호출(string ArgDate)
        {
            int i = 0;
            int j = 0;
            int nTotal = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  a.rowid,a.* ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_WONMUDAILY a";
            SQL += ComNum.VBLF +  "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND JOBDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD HH24:MI')  ";
            SQL += ComNum.VBLF + "      AND GBIO = 'E' ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                FstrRowid = "";

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }             

                if (dt.Rows.Count > 0)
                {
                    FstrRowid = dt.Rows[0]["rowid"].ToString().Trim();
                    ssList2.ActiveSheet.Cells[26, 1].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_01"].ToString().Trim(), "@@", 1);
                    ssList2.ActiveSheet.Cells[26, 3].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_01"].ToString().Trim(), "@@", 2);
                    ssList2.ActiveSheet.Cells[26, 4].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_01"].ToString().Trim(), "@@", 3);
                    ssList2.ActiveSheet.Cells[26, 6].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_01"].ToString().Trim(), "@@", 4);
                    ssList2.ActiveSheet.Cells[26, 9].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_01"].ToString().Trim(), "@@", 5);
                    ssList2.ActiveSheet.Cells[26, 12].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_01"].ToString().Trim(), "@@", 6);
                    ssList2.ActiveSheet.Cells[26, 16].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_01"].ToString().Trim(), "@@", 7);
                    ssList2.ActiveSheet.Cells[26, 18].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_01"].ToString().Trim(), "@@", 8);

                    ssList2.ActiveSheet.Cells[27, 1].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_02"].ToString().Trim(), "@@", 1);
                    ssList2.ActiveSheet.Cells[27, 3].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_02"].ToString().Trim(), "@@", 2);
                    ssList2.ActiveSheet.Cells[27, 4].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_02"].ToString().Trim(), "@@", 3);
                    ssList2.ActiveSheet.Cells[27, 6].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_02"].ToString().Trim(), "@@", 4);
                    ssList2.ActiveSheet.Cells[27, 9].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_02"].ToString().Trim(), "@@", 5);
                    ssList2.ActiveSheet.Cells[27, 12].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_02"].ToString().Trim(), "@@", 6);
                    ssList2.ActiveSheet.Cells[27, 16].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_02"].ToString().Trim(), "@@", 7);
                    ssList2.ActiveSheet.Cells[27, 18].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_02"].ToString().Trim(), "@@", 8);

                    ssList2.ActiveSheet.Cells[28, 1].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_03"].ToString().Trim(), "@@", 1);
                    ssList2.ActiveSheet.Cells[28, 3].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_03"].ToString().Trim(), "@@", 2);
                    ssList2.ActiveSheet.Cells[28, 4].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_03"].ToString().Trim(), "@@", 3);
                    ssList2.ActiveSheet.Cells[28, 6].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_03"].ToString().Trim(), "@@", 4);
                    ssList2.ActiveSheet.Cells[28, 9].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_03"].ToString().Trim(), "@@", 5);
                    ssList2.ActiveSheet.Cells[28, 12].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_03"].ToString().Trim(), "@@", 6);
                    ssList2.ActiveSheet.Cells[28, 16].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_03"].ToString().Trim(), "@@", 7);
                    ssList2.ActiveSheet.Cells[28, 18].Text = VB.Pstr(dt.Rows[0]["TRANS_INFO_03"].ToString().Trim(), "@@", 8);

                    ssList2.ActiveSheet.Cells[32, 1].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_01"].ToString().Trim(), "@@", 1);
                    ssList2.ActiveSheet.Cells[32, 3].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_01"].ToString().Trim(), "@@", 2);
                    ssList2.ActiveSheet.Cells[32, 4].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_01"].ToString().Trim(), "@@", 3);
                    ssList2.ActiveSheet.Cells[32, 6].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_01"].ToString().Trim(), "@@", 4);
                    ssList2.ActiveSheet.Cells[32, 9].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_01"].ToString().Trim(), "@@", 5);
                    ssList2.ActiveSheet.Cells[32, 12].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_01"].ToString().Trim(), "@@", 6);
                    ssList2.ActiveSheet.Cells[32, 15].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_01"].ToString().Trim(), "@@", 7);
                    ssList2.ActiveSheet.Cells[32, 17].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_01"].ToString().Trim(), "@@", 8);

                    ssList2.ActiveSheet.Cells[33, 1].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_02"].ToString().Trim(), "@@", 1);
                    ssList2.ActiveSheet.Cells[33, 3].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_02"].ToString().Trim(), "@@", 2);
                    ssList2.ActiveSheet.Cells[33, 4].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_02"].ToString().Trim(), "@@", 3);
                    ssList2.ActiveSheet.Cells[33, 6].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_02"].ToString().Trim(), "@@", 4);
                    ssList2.ActiveSheet.Cells[33, 9].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_02"].ToString().Trim(), "@@", 5);
                    ssList2.ActiveSheet.Cells[33, 12].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_02"].ToString().Trim(), "@@", 6);
                    ssList2.ActiveSheet.Cells[33, 15].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_02"].ToString().Trim(), "@@", 7);
                    ssList2.ActiveSheet.Cells[33, 17].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_02"].ToString().Trim(), "@@", 8);

                    ssList2.ActiveSheet.Cells[34, 1].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_03"].ToString().Trim(), "@@", 1);
                    ssList2.ActiveSheet.Cells[34, 3].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_03"].ToString().Trim(), "@@", 2);
                    ssList2.ActiveSheet.Cells[34, 4].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_03"].ToString().Trim(), "@@", 3);
                    ssList2.ActiveSheet.Cells[34, 6].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_03"].ToString().Trim(), "@@", 4);
                    ssList2.ActiveSheet.Cells[34, 9].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_03"].ToString().Trim(), "@@", 5);
                    ssList2.ActiveSheet.Cells[34, 12].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_03"].ToString().Trim(), "@@", 6);
                    ssList2.ActiveSheet.Cells[34, 15].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_03"].ToString().Trim(), "@@", 7);
                    ssList2.ActiveSheet.Cells[34, 17].Text = VB.Pstr(dt.Rows[0]["MISU_INFO_03"].ToString().Trim(), "@@", 8);

                    ssList2.ActiveSheet.Cells[38, 1].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_01"].ToString().Trim(), "@@", 1);
                    ssList2.ActiveSheet.Cells[38, 3].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_01"].ToString().Trim(), "@@", 2);
                    ssList2.ActiveSheet.Cells[38, 4].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_01"].ToString().Trim(), "@@", 3);
                    ssList2.ActiveSheet.Cells[38, 6].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_01"].ToString().Trim(), "@@", 4);
                    ssList2.ActiveSheet.Cells[38, 9].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_01"].ToString().Trim(), "@@", 5);
                    ssList2.ActiveSheet.Cells[38, 12].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_01"].ToString().Trim(), "@@", 6);
                    ssList2.ActiveSheet.Cells[38, 15].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_01"].ToString().Trim(), "@@", 7);

                    ssList2.ActiveSheet.Cells[39, 1].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_02"].ToString().Trim(), "@@", 1);
                    ssList2.ActiveSheet.Cells[39, 3].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_02"].ToString().Trim(), "@@", 2);
                    ssList2.ActiveSheet.Cells[39, 4].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_02"].ToString().Trim(), "@@", 3);
                    ssList2.ActiveSheet.Cells[39, 6].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_02"].ToString().Trim(), "@@", 4);
                    ssList2.ActiveSheet.Cells[39, 9].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_02"].ToString().Trim(), "@@", 5);
                    ssList2.ActiveSheet.Cells[39, 12].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_02"].ToString().Trim(), "@@", 6);
                    ssList2.ActiveSheet.Cells[39, 15].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_02"].ToString().Trim(), "@@", 7);

                    ssList2.ActiveSheet.Cells[40, 1].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_03"].ToString().Trim(), "@@", 1);
                    ssList2.ActiveSheet.Cells[40, 3].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_03"].ToString().Trim(), "@@", 2);
                    ssList2.ActiveSheet.Cells[40, 4].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_03"].ToString().Trim(), "@@", 3);
                    ssList2.ActiveSheet.Cells[40, 6].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_03"].ToString().Trim(), "@@", 4);
                    ssList2.ActiveSheet.Cells[40, 9].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_03"].ToString().Trim(), "@@", 5);
                    ssList2.ActiveSheet.Cells[40, 12].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_03"].ToString().Trim(), "@@", 6);
                    ssList2.ActiveSheet.Cells[40, 15].Text = VB.Pstr(dt.Rows[0]["DEATH_INFO_03"].ToString().Trim(), "@@", 7);

                    ssList2.ActiveSheet.Cells[42, 1].Text = VB.Pstr(dt.Rows[0]["REMARK"].ToString().Trim(), "@@", 1);
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

        //2017-07-06 저장기능 추가
        void Screen_DisPlay_진료비저장내용저장(string ArgDate)
        {
            #region 변수선언부
            string strBUN1 = "";
            string strBUN2 = "";
            string strBUN3 = "";
            string strBUN4 = "";
            string strBUN5 = "";
            string strBUN6 = "";
            string strBUN7 = "";
            string strBUN8 = "";
            string strBUN9 = "";
            string strBUN10 = "";
            
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0; //변경된 Row 받는 변수
            #endregion

            clsDB.setBeginTran(clsDB.DbCon);


            if (FstrRowid != "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "DELETE FROM " + ComNum.DB_PMPA + "TONG_WONMUDAILY";
                SQL += ComNum.VBLF + "WHERE ROWID  = '" + FstrRowid + "'               ";
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

            strBUN1 = ssList2.ActiveSheet.Cells[26, 1].Text + "@@";
            strBUN1 += ssList2.ActiveSheet.Cells[26, 3].Text + "@@";
            strBUN1 += ssList2.ActiveSheet.Cells[26, 4].Text + "@@";
            strBUN1 += ssList2.ActiveSheet.Cells[26, 6].Text + "@@";
            strBUN1 += ssList2.ActiveSheet.Cells[26, 9].Text + "@@";
            strBUN1 += ssList2.ActiveSheet.Cells[26, 12].Text + "@@";
            strBUN1 += ssList2.ActiveSheet.Cells[26, 16].Text + "@@";
            strBUN1 += ssList2.ActiveSheet.Cells[26, 18].Text + "@@";

            strBUN2 = ssList2.ActiveSheet.Cells[27, 1].Text + "@@";
            strBUN2 += ssList2.ActiveSheet.Cells[27, 3].Text + "@@";
            strBUN2 += ssList2.ActiveSheet.Cells[27, 4].Text + "@@";
            strBUN2 += ssList2.ActiveSheet.Cells[27, 6].Text + "@@";
            strBUN2 += ssList2.ActiveSheet.Cells[27, 9].Text + "@@";
            strBUN2 += ssList2.ActiveSheet.Cells[27, 12].Text + "@@";
            strBUN2 += ssList2.ActiveSheet.Cells[27, 16].Text + "@@";
            strBUN2 += ssList2.ActiveSheet.Cells[27, 18].Text + "@@";

            strBUN3 = ssList2.ActiveSheet.Cells[28, 1].Text + "@@";
            strBUN3 += ssList2.ActiveSheet.Cells[28, 3].Text + "@@";
            strBUN3 += ssList2.ActiveSheet.Cells[28, 4].Text + "@@";
            strBUN3 += ssList2.ActiveSheet.Cells[28, 6].Text + "@@";
            strBUN3 += ssList2.ActiveSheet.Cells[28, 9].Text + "@@";
            strBUN3 += ssList2.ActiveSheet.Cells[28, 12].Text + "@@";
            strBUN3 += ssList2.ActiveSheet.Cells[28, 16].Text + "@@";
            strBUN3 += ssList2.ActiveSheet.Cells[28, 18].Text + "@@";

            strBUN4 = ssList2.ActiveSheet.Cells[32, 1].Text + "@@";
            strBUN4 += ssList2.ActiveSheet.Cells[32, 3].Text + "@@";
            strBUN4 += ssList2.ActiveSheet.Cells[32, 4].Text + "@@";
            strBUN4 += ssList2.ActiveSheet.Cells[32, 6].Text + "@@";
            strBUN4 += ssList2.ActiveSheet.Cells[32, 9].Text + "@@";
            strBUN4 += ssList2.ActiveSheet.Cells[32, 12].Text + "@@";
            strBUN4 += ssList2.ActiveSheet.Cells[32, 15].Text + "@@";
            strBUN4 += ssList2.ActiveSheet.Cells[32, 17].Text + "@@";

            strBUN5 = ssList2.ActiveSheet.Cells[33, 1].Text + "@@";
            strBUN5 += ssList2.ActiveSheet.Cells[33, 3].Text + "@@";
            strBUN5 += ssList2.ActiveSheet.Cells[33, 4].Text + "@@";
            strBUN5 += ssList2.ActiveSheet.Cells[33, 6].Text + "@@";
            strBUN5 += ssList2.ActiveSheet.Cells[33, 9].Text + "@@";
            strBUN5 += ssList2.ActiveSheet.Cells[33, 12].Text + "@@";
            strBUN5 += ssList2.ActiveSheet.Cells[33, 15].Text + "@@";
            strBUN5 += ssList2.ActiveSheet.Cells[33, 17].Text + "@@";

            strBUN6 = ssList2.ActiveSheet.Cells[34, 1].Text + "@@";
            strBUN6 += ssList2.ActiveSheet.Cells[34, 3].Text + "@@";
            strBUN6 += ssList2.ActiveSheet.Cells[34, 4].Text + "@@";
            strBUN6 += ssList2.ActiveSheet.Cells[34, 6].Text + "@@";
            strBUN6 += ssList2.ActiveSheet.Cells[34, 9].Text + "@@";
            strBUN6 += ssList2.ActiveSheet.Cells[34, 12].Text + "@@";
            strBUN6 += ssList2.ActiveSheet.Cells[34, 15].Text + "@@";
            strBUN6 += ssList2.ActiveSheet.Cells[34, 17].Text + "@@";

            strBUN7 = ssList2.ActiveSheet.Cells[38, 1].Text + "@@";
            strBUN7 += ssList2.ActiveSheet.Cells[38, 3].Text + "@@";
            strBUN7 += ssList2.ActiveSheet.Cells[38, 4].Text + "@@";
            strBUN7 += ssList2.ActiveSheet.Cells[38, 6].Text + "@@";
            strBUN7 += ssList2.ActiveSheet.Cells[38, 9].Text + "@@";
            strBUN7 += ssList2.ActiveSheet.Cells[38, 12].Text + "@@";
            strBUN7 += ssList2.ActiveSheet.Cells[38, 15].Text + "@@";

            strBUN8 = ssList2.ActiveSheet.Cells[39, 1].Text + "@@";
            strBUN8 += ssList2.ActiveSheet.Cells[39, 3].Text + "@@";
            strBUN8 += ssList2.ActiveSheet.Cells[39, 4].Text + "@@";
            strBUN8 += ssList2.ActiveSheet.Cells[39, 6].Text + "@@";
            strBUN8 += ssList2.ActiveSheet.Cells[39, 9].Text + "@@";
            strBUN8 += ssList2.ActiveSheet.Cells[39, 12].Text + "@@";
            strBUN8 += ssList2.ActiveSheet.Cells[39, 15].Text + "@@";

            strBUN9 = ssList2.ActiveSheet.Cells[40, 1].Text + "@@";
            strBUN9 += ssList2.ActiveSheet.Cells[40, 3].Text + "@@";
            strBUN9 += ssList2.ActiveSheet.Cells[40, 4].Text + "@@";
            strBUN9 += ssList2.ActiveSheet.Cells[40, 6].Text + "@@";
            strBUN9 += ssList2.ActiveSheet.Cells[40, 9].Text + "@@";
            strBUN9 += ssList2.ActiveSheet.Cells[40, 12].Text + "@@";
            strBUN9 += ssList2.ActiveSheet.Cells[40, 15].Text + "@@";

            strBUN10 += ssList2.ActiveSheet.Cells[43, 15].Text + "@@";

            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "TONG_WONMUDAILY";
            SQL += ComNum.VBLF + "(";
            SQL += ComNum.VBLF + "  JOBDATE,GBIO,TRANS_INFO_01,TRANS_INFO_02,TRANS_INFO_03,";
            SQL += ComNum.VBLF + "  MISU_INFO_01,MISU_INFO_02,MISU_INFO_03,DEATH_INFO_01,DEATH_INFO_02,";
            SQL += ComNum.VBLF + "  DEATH_INFO_03,REMARK,ENTSABUN,ENTDATE";
            SQL += ComNum.VBLF + ")";
            SQL += ComNum.VBLF + "VALUES(";
            SQL += ComNum.VBLF + "  TO_DATE('" + ArgDate + "','YYYY-MM-DD HH24:MI')  ,";
            SQL += ComNum.VBLF + "  'E',";
            SQL += ComNum.VBLF + "  '" + strBUN1 + "',";
            SQL += ComNum.VBLF + "  '" + strBUN2 + "',";
            SQL += ComNum.VBLF + "  '" + strBUN3 + "',";
            SQL += ComNum.VBLF + "  '" + strBUN4 + "',";
            SQL += ComNum.VBLF + "  '" + strBUN5 + "',";
            SQL += ComNum.VBLF + "  '" + strBUN6 + "',";
            SQL += ComNum.VBLF + "  '" + strBUN7 + "',";
            SQL += ComNum.VBLF + "  '" + strBUN8 + "',";
            SQL += ComNum.VBLF + "  '" + strBUN9 + "',";
            SQL += ComNum.VBLF + "  '" + strBUN10 + "',";            
            SQL += ComNum.VBLF + clsPublic.GnJobSabun + ", ";
            SQL += ComNum.VBLF + "SYSDATE";
            SQL += ComNum.VBLF + ")";

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

        void Screen_DisPlay_의뢰환자리스트()
        {
            int i = 0;
            int ii = 0;
            int j = 0;
            int nRow = 0;
            int nREAD = 0;
            int nREAD2 = 0;
            int nAge = 0;

            string strPano = "";
            string strBDate = "";

            
            string strSname = "";            
            string strFDate = "";
            string strTDate = "";
            string strFTime = "";
            string strTTime = "";            
            string strDate = "";
            string strDate1 = "";
            string strInTime = "";
            string strOutTime = "";
            string strInDate2 = "";
            string strInTime2 = "";            

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = "";


            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            strFTime = dtpFTime.Text;
            strTTime = dtpTTime.Text;

            strDate = dtpFDate.Text;
            strDate1 = dtpTDate.Text;

            //NUR_ER_PATIENT VIEW
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                                      ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.InTime,'YYYY-MM-DD HH24:MI') InTime,TO_CHAR(a.InTime,'YYYYMMDD HH24MI') InTime2,                                ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.OutTime,'YYYY-MM-DD HH24:MI') OutTime,                                                                          ";
            SQL += ComNum.VBLF + "  a.DeptCode,a.WardCode,a.Room,a.Study,a.Disease,a.OutGbn,nvl(IPDRNAME,DRname) DRname,                                      ";
            SQL += ComNum.VBLF + "  DECODE(TRIM(InGbn),'1','직접내원','2','타병원의뢰','3','119','4','129','5','택시','6','엠블런스','9','기타','기타') InGbn,";
            SQL += ComNum.VBLF + "  b.Pano,b.SName,b.Age,b.Sex,b.Singu,b.Bi,TO_CHAR(b.BDate,'YYYY-MM-DD') BDate,                                              ";
            SQL += ComNum.VBLF + "  c.Tel,c.Hphone,c.ZipCode1 || c.ZipCode2 AS  ZipCode,c.Juso,a.ErCar,a.Rowid";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT a, " + ComNum.DB_PMPA + "OPD_MASTER b, " + ComNum.DB_PMPA + "BAS_PATIENT c        ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                                   ";
            SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                                                                  ";
            SQL += ComNum.VBLF + "      AND b.pano=c.pano(+)  and trim(ErCar) is not null                                                                     ";
            SQL += ComNum.VBLF + "      AND a.InTime>=TO_DATE('" + strFDate + " " + strFTime + "','YYYY-MM-DD HH24:MI')                                       ";
            SQL += ComNum.VBLF + "      AND a.InTime<=TO_DATE('" + strTDate + " " + strTTime + "' ,'YYYY-MM-DD HH24:MI')                                      ";
            SQL += ComNum.VBLF + "      AND TRUNC(a.InTime) =b.BDATE(+)                                                                                       ";
            SQL += ComNum.VBLF + "      AND b.DeptCode ='ER'                                                                                                  ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (dt.Rows.Count > 0)
            {
                //VB에서는 nRow 3
                nRow = 2;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 21)
                    {
                        ComFunc.MsgBox("20건 이상입니다. 특이사항에 수기 등록하세요. ");
                        return;
                    }
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();

                    strBDate = dt.Rows[i]["BDate"].ToString().Trim();
                    strSname = dt.Rows[i]["SName"].ToString().Trim();

                    strOutTime = dt.Rows[i]["OutTime"].ToString().Trim();
                    strInTime = dt.Rows[i]["InTime"].ToString().Trim();

                    strInDate2 = VB.Left(dt.Rows[i]["InTime2"].ToString().Trim(), 8);
                    strInTime2 = VB.Right(dt.Rows[i]["InTime2"].ToString().Trim(), 4);

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                        ";
                    SQL += ComNum.VBLF + "  PTMIEMCD,PTMIIDNO,PTMIINdt,PTMIINTM,PTMISTAT,               ";
                    SQL += ComNum.VBLF + "  PTMINAME,PTMIBRTD,PTMISEXX,PTMIIUKD,PTMIHSCD,               ";
                    SQL += ComNum.VBLF + "  PTMIDRLC,PTMIAKdt,PTMIAKTM,PTMIDGKD,PTMIARCF,               ";
                    SQL += ComNum.VBLF + "  PTMIARCS,PTMIINRT,PTMIINMN,PTMIMNSY,PTMIMSSR,               ";
                    SQL += ComNum.VBLF + "  PTMISYM2,PTMISYS2,PTMISYM3,PTMISYS3,PTMIETSY,               ";
                    SQL += ComNum.VBLF + "  PTMIEMSY,PTMIRESP,PTMIHIBP,PTMILOBP,PTMIPULS,               ";
                    SQL += ComNum.VBLF + "  PTMIBRTH,PTMIBDHT,PTMIEMRT,PTMIDEPT,PTMIOTdt,               ";
                    SQL += ComNum.VBLF + "  PTMIOTTM,PTMIDCRT,PTMIDCdt,PTMIDCTM,PTMITAIP,               ";
                    SQL += ComNum.VBLF + "  PTMITSBT,PTMITSCS,PTMITSFA,PTMITSSA,PTMITSHM,               ";
                    SQL += ComNum.VBLF + "  PTMITSPT,PTMITSNO,PTMITSUR,PTMITSUK,GBSEND, SEQNO           ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI                    ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                    SQL += ComNum.VBLF + "      AND SEQNO IN ( SELECT MAX(SEQNO) FROM NUR_ER_EMIHPTMI   ";
                    SQL += ComNum.VBLF + "                     WHERE PTMIIDNO = '" + strPano + "'       ";
                    SQL += ComNum.VBLF + "                      AND PTMIINdt = '" + strInDate2 + "'     ";
                    SQL += ComNum.VBLF + "                      AND PTMIINTM = '" + strInTime2 + "')    ";
                    SQL += ComNum.VBLF + "      AND PTMIIDNO = '" + strPano + "'                        ";
                    SQL += ComNum.VBLF + "      AND PTMIINdt = '" + strInDate2 + "'                     ";
                    SQL += ComNum.VBLF + "      AND PTMIINTM = '" + strInTime2 + "'                     ";

                    try
                    {
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                    }

                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        nRow += 1;

                        ssList2_Sheet1.Cells[nRow, 1].Text = VB.Right(strInTime, 5);
                        ssList2_Sheet1.Cells[nRow, 3].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList2_Sheet1.Cells[nRow, 4].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList2_Sheet1.Cells[nRow, 6].Text = dt.Rows[i]["SEX"].ToString().Trim() + "/" + dt.Rows[i]["Age"].ToString().Trim();

                        ssList2_Sheet1.Cells[nRow, 9].Text = dt.Rows[i]["DeptCode"].ToString().Trim() + "/" + dt.Rows[i]["DRname"].ToString().Trim();
                        ssList2_Sheet1.Cells[nRow, 12].Text = dt.Rows[i]["ErCar"].ToString().Trim() + "." + CF.READ_ERCAR_NAME(clsDB.DbCon, dt.Rows[i]["ErCar"].ToString().Trim());

                        switch (dt.Rows[i]["OutGbn"].ToString().Trim())
                        {
                            case "1":
                                ssList2_Sheet1.Cells[nRow, 16].Text = "입원";
                                break;
                            case "2":
                                ssList2_Sheet1.Cells[nRow, 16].Text = "귀가";
                                break;
                            case "3":
                                ssList2_Sheet1.Cells[nRow, 16].Text = "DOA";
                                break;
                            case "4":
                                ssList2_Sheet1.Cells[nRow, 16].Text = "사망";
                                break;
                            case "5":
                                ssList2_Sheet1.Cells[nRow, 16].Text = "취소";
                                break;
                            case "6":
                                ssList2_Sheet1.Cells[nRow, 16].Text = "이송";
                                break;
                            case "7":
                                ssList2_Sheet1.Cells[nRow, 16].Text = "DAMA";
                                break;
                            case "8":
                                ssList2_Sheet1.Cells[nRow, 16].Text = "OPD";
                                break;
                            case "9":
                                ssList2_Sheet1.Cells[nRow, 16].Text = "OR입원";
                                break;
                        }

                        ssList2_Sheet1.Cells[nRow, 18].Text = "";
                        dt1.Dispose();
                        dt1 = null;

                    }
                }
            }

            dt.Dispose();
            dt = null;
        }

        void Screen_DisPlay_응급실입원리스트()
        {
            int i = 0;
            
            int j = 0;
            int nRow = 0;
            int nREAD = 0;            
            
            string strFDate = "";
            string strTDate = "";
            string strFTime = "";
            string strTTime = "";            
            string strDate = "";
            string strDate1 = "";            
            string strGbn = "";
            

            DataTable dt = null;            
            string SQL = "";
            string SqlErr = "";

            for (i = 27; i < ssList1_Sheet1.Rows.Count; i++)
            {
                for (j = 0; j < ssList1_Sheet1.Columns.Count; j++)
                {
                    ssList1_Sheet1.Cells[i, j].Text = "";
                }
            }

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            strFTime = dtpFTime.Text;
            strTTime = dtpTTime.Text;

            strDate = dtpFDate.Text;
            strDate1 = dtpTDate.Text;
            nRow = 0;
            strGbn = "";

            //NUR_ER_PATIENT VIEW
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                                    ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.indate,'HH24:MI') InTime,TO_CHAR(a.wardintime,'HH24:MI') InTime2, a.SName ,a.Age,a.Sex,a.Pano,                ";
            SQL += ComNum.VBLF + "  a.DeptCode,b.drname ,a.Roomcode,a.Bi ,TO_CHAR(a.indate,'YYYY-MM-DD') arvdate,TO_CHAR(a.indate,'YYYY-MM-DD') arvdate2    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ipd_new_master a, " + ComNum.DB_PMPA + "BAS_DOCTOR b, " + ComNum.DB_PMPA + "BAS_PATIENT c      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                                 ";
            SQL += ComNum.VBLF + "      AND a.pano=c.pano(+)                                                                                                ";
            SQL += ComNum.VBLF + "      AND  a.drcode=b.drcode(+)                                                                                           ";
            SQL += ComNum.VBLF + "      AND a.wardintime>=TO_DATE('" + strFDate + " " + strFTime + "','YYYY-MM-DD HH24:MI')                                 ";
            SQL += ComNum.VBLF + "      AND a.wardintime<=TO_DATE('" + strTDate + " " + strTTime + "' ,'YYYY-MM-DD HH24:MI')                                ";
            SQL += ComNum.VBLF + "      AND a.AMSET7  in ('3','4','5')                                                                                      ";
            SQL += ComNum.VBLF + "ORDER BY wardintime                                                                                                       ";

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

                    nREAD = dt.Rows.Count;

                    nRow = 27;

                    for (i = 0; i < nREAD; i++)
                    {
                        ssList1_Sheet1.Cells[nRow, 1].Text = dt.Rows[i]["InTime"].ToString().Trim();
                        ssList1_Sheet1.Cells[nRow, 2].Text = dt.Rows[i]["InTime2"].ToString().Trim();
                        ssList1_Sheet1.Cells[nRow, 3].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList1_Sheet1.Cells[nRow, 7].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();

                        ssList1_Sheet1.Cells[nRow, 8].Text = READ_최종진단(dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["arvdate"].ToString().Trim(), dt.Rows[i]["arvdate2"].ToString().Trim());
                        ssList1_Sheet1.Cells[nRow, 16].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList1_Sheet1.Cells[nRow, 17].Text = dt.Rows[i]["drname"].ToString().Trim();
                        ssList1_Sheet1.Cells[nRow, 18].Text = dt.Rows[i]["Roomcode"].ToString().Trim();
                        ssList1_Sheet1.Cells[nRow, 19].Text = dt.Rows[i]["BI"].ToString().Trim();
                        ssList1_Sheet1.Cells[nRow, 20].Text = "";
                        nRow += 1;

                        if (nRow == ssList1_Sheet1.Rows.Count)
                        {
                            ComFunc.MsgBox("입원 환자 리스트가 20명 이상입니다. 수기 등록 바랍니다.");
                            return;
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
        }

        string READ_최종진단(string ArgPano, string argINdt, string argINdt2)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + " A.ILLCODE, B.ILLNAMEK, B.GUBUN, DECODE(A.SEQNO, '1', '주',  '부') GBN ,ILLNAMEE           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IILLS a, " + ComNum.DB_PMPA + "BAS_ILLS B                                               ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                             ";
            SQL += ComNum.VBLF + "      AND A.PTNO  = '" + ArgPano + "'                                                                                 ";
            SQL += ComNum.VBLF + "      AND A.BDATE between TO_DATE('" + argINdt + "','YYYY-MM-DD')                                                     ";
            SQL += ComNum.VBLF + "      AND TO_DATE('" + argINdt2 + "','YYYY-MM-DD') + 1                                                                ";
            SQL += ComNum.VBLF + "      AND A.ILLCODE = B.ILLCODE(+)                                                                                    ";
            SQL += ComNum.VBLF + "      AND SEQNO  = '1'                                                                                             ";
            SQL += ComNum.VBLF + "ORDER BY SEQNO ASC                                                                                                 ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        void Screen_DisPlay_응급실환자현황()
        {
            int i = 0;
            int j = 0;

            int[] nCnt = new int[4];
            int[] nCntBi = new int[6];

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  a.DeptCode,a.Pano , DECODE(a.WardCode,'','0','1') AS WardCode, InTime ,";
            SQL += ComNum.VBLF + "  DECODE(b.BI,'11','1','12','1','13','1','21','2','22','2','31','3','33','3','52','5','55','5','6') AS aBi";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT a, " + ComNum.DB_PMPA + "OPD_MASTER b, " + ComNum.DB_PMPA + "BAS_PATIENT c";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)";
            SQL += ComNum.VBLF + "      AND b.pano=c.pano(+) ";
            SQL += ComNum.VBLF + "      AND a.InTime>=TO_DATE('" + FstrFDate + " " + FstrFTime + "','YYYY-MM-DD HH24:MI')";
            SQL += ComNum.VBLF + "      AND a.InTime<=TO_DATE('" + FstrTDate + " " + FstrTTime + "','YYYY-MM-DD HH24:MI') ";
            SQL += ComNum.VBLF + "      AND TRUNC(a.InTime) =b.BDATE(+) ";
            SQL += ComNum.VBLF + "      AND b.DeptCode ='ER'  and NVL(DGKD,'0' ) <> '4' ";
            SQL += ComNum.VBLF + "GROUP By a.DeptCode,a.Pano , DECODE(a.WardCode,'','0','1'), InTime ,";
            SQL += ComNum.VBLF + "         DECODE(b.BI,'11','1','12','1','13','1','21','2','22','2','31','3','33','3','52','5','55','5','6') ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["WARDCODE"].ToString().Trim() == "1")
                        {
                            nCnt[2] += 1;
                        }
                        else
                        {
                            switch (READ_관리료(dt.Rows[i]["Pano"].ToString().Trim(), FstrFDate, FstrTDate))
                            {
                                case "본인":
                                    nCntBi[1] += 1;
                                    break;
                                case "공단":
                                    nCntBi[2] += 1;
                                    break;
                                case "감액":
                                    nCntBi[3] += 1;
                                    break;
                                case "기타":
                                    nCntBi[4] += 1;
                                    break;

                                default:
                                    nCntBi[5] += 1;
                                    break;
                            }
                            nCnt[3] += 1;
                        }

                        nCnt[1] += 1;
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

            if (String.Compare(FstrFTime, "17:00") < 0)
            {
                ssList1_Sheet1.Cells[11, 2].Text = nCnt[1].ToString();
                ssList1_Sheet1.Cells[11, 5].Text = nCnt[2].ToString();
                ssList1_Sheet1.Cells[11, 8].Text = (nCnt[3] - nCntBi[5]).ToString();
                ssList1_Sheet1.Cells[11, 12].Text = nCntBi[5].ToString();
            }
            else
            {
                ssList1_Sheet1.Cells[12, 2].Text = nCnt[1].ToString();
                ssList1_Sheet1.Cells[12, 5].Text = nCnt[2].ToString();
                ssList1_Sheet1.Cells[12, 8].Text = (nCnt[3] - nCntBi[5]).ToString();
                ssList1_Sheet1.Cells[12, 12].Text = nCntBi[5].ToString();
            }


            //2017-03-03, 원무팀 김성준 요청
            ssList1_Sheet1.Cells[16, 16].Text = nCntBi[1].ToString();
            ssList1_Sheet1.Cells[16, 18].Text = nCntBi[2].ToString();
            ssList1_Sheet1.Cells[16, 20].Text = nCntBi[3].ToString();
            ssList1_Sheet1.Cells[16, 22].Text = nCntBi[4].ToString();
        }

        string READ_관리료(string ArgPano, string argBdate, string argBdate2)
        {
            int i = 0;
            int j = 0;
                        
            int[] nCnt = new int[4];

            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                        ";
            SQL += ComNum.VBLF + "  PANO,DECODE(TRIM(SUNEXT),'V1200A','감액',DECODE(GBSELF,'0','공단','2','본인')) GUBUN,       ";
            SQL += ComNum.VBLF + "  SUM(QTY*NAL) CNT                                                                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP                                                           ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
            SQL += ComNum.VBLF + "      AND BDATE >= TO_DATE('" + argBdate + "','YYYY-MM-DD')                                   ";
            SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + argBdate2 + "','YYYY-MM-DD')                                  ";
            SQL += ComNum.VBLF + "      AND PANO ='" + ArgPano + "'                                                             ";
            SQL += ComNum.VBLF + "      AND SUNEXT LIKE 'V1200%'                                                                ";
            SQL += ComNum.VBLF + "      AND Part <> '#'                                                                         ";
            SQL += ComNum.VBLF + "GROUP BY PANO,DECODE(TRIM(SUNEXT),'V1200A','감액',DECODE(GBSELF,'0','공단','2','본인'))       ";
            SQL += ComNum.VBLF + "HAVING SUM(Qty * NAL) > 0                                                                     ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rtnVal = dt.Rows[i]["GUBUN"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;

            if (rtnVal == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                                        ";
                SQL += ComNum.VBLF + "  PANO,SUNEXT ,                                                                               ";
                SQL += ComNum.VBLF + "  SUM(QTY*NAL) CNT                                                                            ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP                                                           ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
                SQL += ComNum.VBLF + "      AND BDATE >= TO_DATE('" + argBdate + "','YYYY-MM-DD')                                   ";
                SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + argBdate2 + "','YYYY-MM-DD')                                  ";
                SQL += ComNum.VBLF + "      AND PANO ='" + ArgPano + "'                                                             ";
                SQL += ComNum.VBLF + "      AND Part <> '#'                                                                         ";
                SQL += ComNum.VBLF + "GROUP BY PANO,SUNEXT                                                                          ";
                SQL += ComNum.VBLF + "HAVING SUM(Qty * NAL) > 0                                                                     ";

                try
                {
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    }
                }

                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        rtnVal = "기타";
                    }
                }

                dt.Dispose();
                dt = null;
            }


            return rtnVal;
        }

        void Screen_DisPlay_응급실진료비내역()
        {
            int i = 0;
            int j = 0;
            double dubAMT = 0;
            int nCount = 0;
            int[] nCnt = new int[4];

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                                        ";
            SQL += ComNum.VBLF + "  COUNT(PANO) CNT, SUM(AMT) SAMT                                                                                              ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SUNAP                                                                                          ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                                     ";
            SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + FstrFDate + "','YYYY-MM-DD')                                                                ";
            SQL += ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + FstrTDate + "','YYYY-MM-DD')                                                                ";
            SQL += ComNum.VBLF + "      AND to_date(to_char(ACTDATE,'YYYY-MM-DD')||STIME,'YYYY-MM-DD HH24:MI') >= TO_DATE('" + FstrFDate + " " + FstrFTime + "','YYYY-MM-DD HH24:MI') ";
            SQL += ComNum.VBLF + "      AND to_date(to_char(ACTDATE,'YYYY-MM-DD')||STIME,'YYYY-MM-DD HH24:MI') <= TO_DATE('" + FstrTDate + " " + FstrTTime + "','YYYY-MM-DD HH24:MI') ";
            SQL += ComNum.VBLF + "      AND PANO  <> '81000004'                                                                                                 ";
            SQL += ComNum.VBLF + "      AND PART <> '4349'                                                                                                      ";
            SQL += ComNum.VBLF + "      AND AMT > 0                                                                                                             ";
            SQL += ComNum.VBLF + "      AND DEPTCODE = 'ER'                                                                                                     ";
            SQL += ComNum.VBLF + "      AND (DELDATE IS NULL OR DELDATE ='')                                                                                    ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (dt.Rows.Count > 0)
            {
                nCount = Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim());
                dubAMT = VB.Val(dt.Rows[0]["SAMT"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            ssList1_Sheet1.Cells[17, 3].Text = String.Format("{0:#,###}", dubAMT) + " 원";

        }

        void Screen_DisPlay_응급실과별환자()
        {
            int i = 0;            
            int nTotal = 0;
            int W_nTotal = 0;

            int[,] nCnt = new int[3, 8];
            int[,] nIPD = new int[3, 8];

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  a.DeptCode,a.Pano, DECODE(a.WardCode,'','0','1') AS WardCode";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT a, " + ComNum.DB_PMPA + "OPD_MASTER b, " + ComNum.DB_PMPA + "BAS_PATIENT c";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)";
            SQL += ComNum.VBLF + "      AND b.pano=c.pano(+)";
            SQL += ComNum.VBLF + "      AND a.InTime>=TO_DATE('" + FstrFDate + " " + FstrFTime + "','YYYY-MM-DD HH24:MI')  ";
            SQL += ComNum.VBLF + "      AND a.InTime<=TO_DATE('" + FstrTDate + " " + FstrTTime + "','YYYY-MM-DD HH24:MI')";
            SQL += ComNum.VBLF + "      AND TRUNC(a.InTime) =b.BDATE(+)";
            SQL += ComNum.VBLF + "      AND b.DeptCode ='ER' and NVL(DGKD,'0' ) <> '4' ";
            SQL += ComNum.VBLF + "GROUP By a.DeptCode, a.Pano, DECODE(a.WardCode,'','0','1') ,InTime";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i]["DEPTCODE"].ToString().Trim())
                    {
                        case "ER":
                            nCnt[1, 1] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[1, 1] += 1;
                            }
                            break;
                        case "PD":
                            nCnt[1, 3] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[1, 3] += 1;
                            }
                            break;
                        case "OS":
                            nCnt[1, 4] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[1, 4] += 1;
                            }
                            break;
                        case "GS":
                            nCnt[1, 5] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[1, 5] += 1;
                            }
                            break;
                        case "CS":
                            nCnt[1, 6] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[1, 6] += 1;
                            }
                            break;
                        case "NS":
                            nCnt[1, 7] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[1, 7] += 1;
                            }
                            break;
                        case "NE":
                            nCnt[2, 1] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[2, 1] += 1;
                            }
                            break;
                        case "OG":
                            nCnt[2, 2] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[2, 2] += 1;
                            }
                            break;
                        case "EN":
                            nCnt[2, 3] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[2, 3] += 1;
                            }
                            break;
                        case "UR":
                            nCnt[2, 4] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[2, 4] += 1;
                            }
                            break;
                        case "RM":
                            nCnt[2, 5] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[2, 5] += 1;
                            }
                            break;
                        case "OT":
                            nCnt[2, 6] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[2, 6] += 1;
                            }
                            break;

                        default:
                            if (VB.Left(dt.Rows[i]["DEPTCODE"].ToString().Trim(), 1) == "M")
                            {
                                nCnt[1, 2] += 1;
                                if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                                {
                                    nIPD[1, 2] += 1;
                                }
                            }

                            else
                            {
                                nCnt[2, 7] += 1;
                                if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                                {
                                    nIPD[2, 7] += 1;
                                }
                            }
                            break;
                    }

                    nTotal += 1;

                    if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                    {
                        W_nTotal += 1;
                    }
                }
            }

            dt.Dispose();
            dt = null;

            ssList1_Sheet1.Cells[21, 1].Text = nTotal + "(" + W_nTotal + ")";
            ssList1_Sheet1.Cells[21, 2].Text = nCnt[1, 1] + "(" + nIPD[1, 1] + ")";
            ssList1_Sheet1.Cells[21, 3].Text = nCnt[1, 2] + "(" + nIPD[1, 2] + ")";
            ssList1_Sheet1.Cells[21, 5].Text = nCnt[1, 3] + "(" + nIPD[1, 3] + ")";
            ssList1_Sheet1.Cells[21, 7].Text = nCnt[1, 4] + "(" + nIPD[1, 4] + ")";
            ssList1_Sheet1.Cells[21, 8].Text = nCnt[1, 5] + "(" + nIPD[1, 5] + ")";
            ssList1_Sheet1.Cells[21, 10].Text = nCnt[1, 6] + "(" + nIPD[1, 6] + ")";
            ssList1_Sheet1.Cells[21, 11].Text = nCnt[1, 7] + "(" + nIPD[1, 7] + ")";

            //ssList1_Sheet1.Cells[23, 1].Text = nTotal + "(" + W_nTotal + ")";
            ssList1_Sheet1.Cells[23, 2].Text = nCnt[2, 1] + "(" + nIPD[2, 1] + ")";
            ssList1_Sheet1.Cells[23, 3].Text = nCnt[2, 2] + "(" + nIPD[2, 2] + ")";
            ssList1_Sheet1.Cells[23, 5].Text = nCnt[2, 3] + "(" + nIPD[2, 3] + ")";
            ssList1_Sheet1.Cells[23, 7].Text = nCnt[2, 4] + "(" + nIPD[2, 4] + ")";
            ssList1_Sheet1.Cells[23, 8].Text = nCnt[2, 5] + "(" + nIPD[2, 5] + ")";
            ssList1_Sheet1.Cells[23, 10].Text = nCnt[2, 6] + "(" + nIPD[2, 6] + ")";
            ssList1_Sheet1.Cells[23, 11].Text = nCnt[2, 7] + "(" + nIPD[2, 7] + ")";
        }

        void Screen_DisPlay_응급실종별환자()
        {
            int i = 0;
            int j = 0;
            int[] nCnt = new int[7];
            int[] nIPD = new int[7];

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                                ";
            SQL += ComNum.VBLF + "  a.Pano,DECODE(a.WardCode,'','0','1') AS WardCode,                                                                   ";
            SQL += ComNum.VBLF + "  DECODE(b.BI,'11','1','12','1','13','1','21','2','22','2','31','3','33','3','52','5','55','5','6') AS aBi            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT a, " + ComNum.DB_PMPA + "OPD_MASTER b, " + ComNum.DB_PMPA + "BAS_PATIENT c  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                             ";
            SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                                                            ";
            SQL += ComNum.VBLF + "      AND b.pano=c.pano(+)                                                                                            ";
            SQL += ComNum.VBLF + "      AND a.InTime>=TO_DATE('" + FstrFDate + " " + FstrFTime + "','YYYY-MM-DD HH24:MI')                               ";
            SQL += ComNum.VBLF + "      AND a.InTime<=TO_DATE('" + FstrTDate + " " + FstrTTime + "','YYYY-MM-DD HH24:MI')                               ";
            SQL += ComNum.VBLF + "      AND TRUNC(a.InTime) =b.BDATE(+)                                                                                 ";
            SQL += ComNum.VBLF + "      AND b.DeptCode ='ER' and NVL(DGKD,'0' ) <> '4'                                                                  ";
            SQL += ComNum.VBLF + "GROUP By a.Pano,DECODE(a.WardCode,'','0','1'), a.InTime ,                                                             ";
            SQL += ComNum.VBLF + "         DECODE(b.BI,'11','1','12','1','13','1','21','2','22','2','31','3','33','3','52','5','55','5','6')            ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i]["aBi"].ToString().Trim())
                    {
                        case "1":
                            nCnt[2] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[2] += 1;
                            }
                            break;
                        case "2":
                            nCnt[3] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[3] += 1;
                            }
                            break;
                        case "5":
                            nCnt[4] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[4] += 1;
                            }
                            break;
                        case "3":
                            nCnt[5] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[5] += 1;
                            }
                            break;
                        case "6":
                            nCnt[6] += 1;
                            if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                            {
                                nIPD[6] += 1;
                            }
                            break;
                    }
                    nCnt[1] += 1;
                    if (dt.Rows[i]["WardCode"].ToString().Trim() == "1")
                    {
                        nIPD[1] += 1;
                    }
                }
            }

            ssList1_Sheet1.Cells[21, 13].Text = nCnt[1] + "(" + nIPD[1] + ")";
            ssList1_Sheet1.Cells[21, 15].Text = nCnt[2] + "(" + nIPD[2] + ")";
            ssList1_Sheet1.Cells[21, 16].Text = nCnt[3] + "(" + nIPD[3] + ")";
            ssList1_Sheet1.Cells[21, 18].Text = nCnt[4] + "(" + nIPD[4] + ")";
            ssList1_Sheet1.Cells[21, 20].Text = nCnt[5] + "(" + nIPD[5] + ")";
            ssList1_Sheet1.Cells[21, 22].Text = nCnt[6] + "(" + nIPD[6] + ")";


            dt.Dispose();
            dt = null;
        }

        void ssList1_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            double dblTotal = 0;

            if (e.Row == 17 && (e.Column >= 3 || e.Column <= 12))
            {
                dblTotal = 0;

                dblTotal += VB.Val(ssList1_Sheet1.Cells[e.Row, 3].Text.Replace(",", ""));
                dblTotal += VB.Val(ssList1_Sheet1.Cells[e.Row, 8].Text.Replace(",", ""));
                dblTotal += VB.Val(ssList1_Sheet1.Cells[e.Row, 12].Text.Replace(",", ""));

                //ssList1_Sheet1.Cells[16, 1].Text = String.Format("{0:#,###}", dblTotal) + " 원";
                ssList1_Sheet1.Cells[16, 1].Text = dblTotal.ToString("###,###,##0") + " 원";
            }
        }
    }
}
