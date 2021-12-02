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
    /// File Name       : frmPmpaViewResJepsu.cs
    /// Description     : 일자별 예약자조회
    /// Author          : 안정수
    /// Create Date     : 2017-08-14
    /// Update History  : 2017-10-24
    /// 출력 부분 수정
    /// <history>         
    /// d:\psmh\OPD\ovchrt\frm예약일자별.frm(Frm예약일자별) => frmPmpaViewResJepsu.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\ovchrt\frm예약일자별.frm(Frm예약일자별)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewResJepsu : Form
    {
        ComFunc CF = new ComFunc();

        public frmPmpaViewResJepsu()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            string CurrentDay = DateTime.Now.ToString("yyyy-MM-dd");
            dtpFdate.Text = Convert.ToDateTime(CurrentDay).AddDays(-1).ToShortDateString();
            dtpTdate.Text = Convert.ToDateTime(CurrentDay).AddDays(-1).ToShortDateString();

            btnPrint.Enabled = false;

            optJob0.Checked = true;
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
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strTitle2 = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            if (dtpFdate.Text != dtpTdate.Text)
            {
                ComFunc.MsgBox("조회기간을 하루로 선택해주세요.");
                return;
            }

            btnPrint.Enabled = false;

            strTitle = "(" + dtpFdate.Text + ") 예약자명부(의무기록실)";

            if (optJob0.Checked == true)
            {
                strTitle2 = "작업구분: 외래예약자";
            }

            else if (optJob1.Checked == true)
            {
                strTitle2 = "작업구분: 예약부도자";
            }

            else if (optJob2.Checked == true)
            {
                strTitle2 = "작업구분: 전화예약자";
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strTitle2, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 60, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, false, true, true, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion

            btnPrint.Enabled = true;
        }

        void eGetData()
        {
            int i = 0;
            int nREAD = 0;

            string strFDate = "";
            string strTDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            strFDate = dtpFdate.Text;
            strTDate = dtpTdate.Text;
            Cursor.Current = Cursors.WaitCursor;

            //예약자 LIST
            //예약진료건수

            if (optJob0.Checked == true)
            {
                //외래예약자
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                         ";
                SQL += ComNum.VBLF + "  A.DEPTCODE, C.DEPTNAMEK, A.PANO, A.BI, A.SNAME, A.DRCODE,                    ";
                SQL += ComNum.VBLF + "  D.DRNAME, B.JUMIN1, A.SEX, B.TEL,                                            ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE , TO_CHAR(A.JTIME,'HH24:MI') JTIME,  ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.CHOJAE                                ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_PATIENT B,  ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_DOCTOR D, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                      ";
                SQL += ComNum.VBLF + "      AND A.ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                ";
                SQL += ComNum.VBLF + "      AND A.ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                ";
                SQL += ComNum.VBLF + "      AND A.RESERVED= '1'                                                      ";
                SQL += ComNum.VBLF + "      AND NOT (A.DeptCode <> 'ER'  AND A.Jin = '2')                            "; //응급실의 타과는 접수ll 인원제외, ~1
                SQL += ComNum.VBLF + "      AND A.Jin <> 'D'                                                         "; //일반건진인원통계제외
                SQL += ComNum.VBLF + "      AND A.Pano <> '81000004'                                                 ";  //전산실 연습
                SQL += ComNum.VBLF + "      AND A.PANO = B.PANO(+)                                                   ";
                SQL += ComNum.VBLF + "      AND A.DeptCode =   C.DeptCode(+)                                         ";
                SQL += ComNum.VBLF + "      AND A.DrCode   =   D.DrCode(+)                                           ";
            }
            else if (optJob1.Checked == true)
            {
                //전화예약
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                         ";
                SQL += ComNum.VBLF + "  A.DEPTCODE, C.DEPTNAMEK, A.PANO, A.BI, A.SNAME, A.DRCODE,                    ";
                SQL += ComNum.VBLF + "  D.DRNAME, B.JUMIN1, A.SEX, B.TEL,                                            ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE , TO_CHAR(A.JTIME,'HH24:MI') JTIME,  ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.CHOJAE                                ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_PATIENT B,  ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_DOCTOR D, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                      ";
                SQL += ComNum.VBLF + "      AND A.BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                  ";
                SQL += ComNum.VBLF + "      AND A.BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                  ";
                SQL += ComNum.VBLF + "      AND A.JIN ='H'                                                           ";
                SQL += ComNum.VBLF + "      AND (A.PANO, A.DEPTCODE, A.ACTDATE) NOT IN (                             ";
                SQL += ComNum.VBLF + "          SELECT PANO, DEPTCODE,RDATE FROM KOSMOS_PMPA.OPD_TELRESV             ";
                SQL += ComNum.VBLF + "              WHERE RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')          ";
                SQL += ComNum.VBLF + "                AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')          ";
                SQL += ComNum.VBLF + "                AND GBINTERNET = 'Y')                                          ";
                SQL += ComNum.VBLF + "      AND NOT (A.DeptCode <> 'ER'  AND A.Jin = '2')                            "; //응급실의 타과는 접수ll 인원제외, ~1
                SQL += ComNum.VBLF + "      AND A.Jin <> 'D'                                                         "; //일반건진인원통계제외
                SQL += ComNum.VBLF + "      AND A.Pano <> '81000004'                                                 ";  //전산실 연습
                SQL += ComNum.VBLF + "      AND A.PANO = B.PANO(+)                                                   ";
                SQL += ComNum.VBLF + "      AND A.DeptCode =   C.DeptCode(+)                                         ";
                SQL += ComNum.VBLF + "      AND A.DrCode   =   D.DrCode(+)                                           ";
            }
            else if (optJob2.Checked == true)
            {
                //인터넷예약
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                         ";
                SQL += ComNum.VBLF + "  A.DEPTCODE, C.DEPTNAMEK, A.PANO, A.BI, A.SNAME, A.DRCODE,                    ";
                SQL += ComNum.VBLF + "  D.DRNAME, B.JUMIN1, A.SEX, B.TEL,                                            ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE , TO_CHAR(A.JTIME,'HH24:MI') JTIME,  ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.CHOJAE                                ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_PATIENT B,  ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_DOCTOR D, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                      ";
                SQL += ComNum.VBLF + "      AND A.BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                  ";
                SQL += ComNum.VBLF + "      AND A.BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                  ";
                SQL += ComNum.VBLF + "      AND A.JIN ='H'                                                           ";
                SQL += ComNum.VBLF + "      AND (A.PANO, A.DEPTCODE, A.ACTDATE) NOT IN (                             ";
                SQL += ComNum.VBLF + "          SELECT PANO, DEPTCODE,RDATE FROM KOSMOS_PMPA.OPD_TELRESV             ";
                SQL += ComNum.VBLF + "              WHERE RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')          ";
                SQL += ComNum.VBLF + "                AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')          ";
                SQL += ComNum.VBLF + "                AND GBINTERNET = 'Y')                                          ";
                SQL += ComNum.VBLF + "      AND NOT (A.DeptCode <> 'ER'  AND A.Jin = '2')                            "; //응급실의 타과는 접수ll 인원제외, ~1
                SQL += ComNum.VBLF + "      AND A.Jin <> 'D'                                                         "; //일반건진인원통계제외
                SQL += ComNum.VBLF + "      AND A.Pano <> '81000004'                                                 ";  //전산실 연습
                SQL += ComNum.VBLF + "      AND A.PANO = B.PANO(+)                                                   ";
                SQL += ComNum.VBLF + "      AND A.DeptCode =   C.DeptCode(+)                                         ";
                SQL += ComNum.VBLF + "      AND A.DrCode   =   D.DrCode(+)                                           ";
            }

            else if (optJob3.Checked == true)
            {
                //Fax예약
                //인터넷예약
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                         ";
                SQL += ComNum.VBLF + "  A.DEPTCODE, C.DEPTNAMEK, A.PANO, A.BI, A.SNAME, A.DRCODE,                    ";
                SQL += ComNum.VBLF + "  D.DRNAME, B.JUMIN1, A.SEX, B.TEL,                                            ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE , TO_CHAR(A.JTIME,'HH24:MI') JTIME,  ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.CHOJAE                                ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_PATIENT B,  ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_DOCTOR D, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                      ";
                SQL += ComNum.VBLF + "      AND A.BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                  ";
                SQL += ComNum.VBLF + "      AND A.BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                  ";
                SQL += ComNum.VBLF + "      AND A.JIN ='H'                                                           ";
                SQL += ComNum.VBLF + "      AND (A.PANO, A.DEPTCODE, A.ACTDATE) NOT IN (                             ";
                SQL += ComNum.VBLF + "          SELECT PANO, DEPTCODE,RDATE FROM KOSMOS_PMPA.OPD_TELRESV             ";
                SQL += ComNum.VBLF + "              WHERE RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')          ";
                SQL += ComNum.VBLF + "                AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')          ";
                SQL += ComNum.VBLF + "                AND GBFAX = 'Y')                                               ";
                SQL += ComNum.VBLF + "      AND NOT (A.DeptCode <> 'ER'  AND A.Jin = '2')                            "; //응급실의 타과는 접수ll 인원제외, ~1
                SQL += ComNum.VBLF + "      AND A.Jin <> 'D'                                                         "; //일반건진인원통계제외
                SQL += ComNum.VBLF + "      AND A.Pano <> '81000004'                                                 ";  //전산실 연습
                SQL += ComNum.VBLF + "      AND A.PANO = B.PANO(+)                                                   ";
                SQL += ComNum.VBLF + "      AND A.DeptCode =   C.DeptCode(+)                                         ";
                SQL += ComNum.VBLF + "      AND A.DrCode   =   D.DrCode(+)                                           ";
            }

            else if (optJob4.Checked == true)
            {
                //예약부도
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                         ";
                SQL += ComNum.VBLF + "  A.DEPTCODE, C.DEPTNAMEK, A.PANO, A.BI, A.SNAME, A.DRCODE,                    ";
                SQL += ComNum.VBLF + "  D.DRNAME, B.JUMIN1, A.SEX, B.TEL,                                            ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE , TO_CHAR(A.JTIME,'HH24:MI') JTIME,  ";
                SQL += ComNum.VBLF + "  A.CHOJAE                                                                     ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_PATIENT B,  ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_DOCTOR D, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                      ";
                SQL += ComNum.VBLF + "      AND A.ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                ";
                SQL += ComNum.VBLF + "      AND A.ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                ";
                SQL += ComNum.VBLF + "      AND NOT (A.DeptCode <> 'ER'  AND A.Jin = '2')                            "; //응급실의 타과는 접수ll 인원제외, ~1
                SQL += ComNum.VBLF + "      AND A.Jin <> 'D'                                                         "; //일반건진인원통계제외
                SQL += ComNum.VBLF + "      AND A.Pano <> '81000004'                                                 ";  //전산실 연습
                SQL += ComNum.VBLF + "      AND A.PANO = B.PANO(+)                                                   ";
                SQL += ComNum.VBLF + "      AND A.DeptCode =   C.DeptCode(+)                                         ";
                SQL += ComNum.VBLF + "      AND A.DrCode   =   D.DrCode(+)                                           ";
            }

            else if (optJob5.Checked == true)
            {
                //전화예약 부도
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                         ";
                SQL += ComNum.VBLF + "  A.DEPTCODE, C.DEPTNAMEK, A.PANO, A.BI, A.SNAME, A.DRCODE,                    ";
                SQL += ComNum.VBLF + "  D.DRNAME, B.JUMIN1, A.SEX, B.TEL,                                            ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE , TO_CHAR(A.JTIME,'HH24:MI') JTIME,  ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.CHOJAE                                ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_PATIENT B,  ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_DOCTOR D, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                      ";
                SQL += ComNum.VBLF + "      AND A.BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                  ";
                SQL += ComNum.VBLF + "      AND A.BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                  ";
                SQL += ComNum.VBLF + "      AND A.JIN ='H'                                                           ";
                SQL += ComNum.VBLF + "      AND (A.PANO, A.DEPTCODE, A.ACTDATE) NOT IN (                             ";
                SQL += ComNum.VBLF + "          SELECT PANO, DEPTCODE,RDATE FROM KOSMOS_PMPA.OPD_TELRESV             ";
                SQL += ComNum.VBLF + "              WHERE RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')          ";
                SQL += ComNum.VBLF + "                AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')          ";
                SQL += ComNum.VBLF + "                AND GBINTERNET = 'Y')                                          ";
                SQL += ComNum.VBLF + "      AND NOT (A.DeptCode <> 'ER'  AND A.Jin = '2')                            "; //응급실의 타과는 접수ll 인원제외, ~1
                SQL += ComNum.VBLF + "      AND A.Jin <> 'D'                                                         "; //일반건진인원통계제외
                SQL += ComNum.VBLF + "      AND A.Pano <> '81000004'                                                 ";  //전산실 연습
                SQL += ComNum.VBLF + "      AND A.PANO = B.PANO(+)                                                   ";
                SQL += ComNum.VBLF + "      AND A.DeptCode =   C.DeptCode(+)                                         ";
                SQL += ComNum.VBLF + "      AND A.DrCode   =   D.DrCode(+)                                           ";
            }

            else if (optJob6.Checked == true)
            {
                //인터넷예약 부도
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                         ";
                SQL += ComNum.VBLF + "  A.DEPTCODE, C.DEPTNAMEK, A.PANO, A.BI, A.SNAME, A.DRCODE,                    ";
                SQL += ComNum.VBLF + "  D.DRNAME, B.JUMIN1, A.SEX, B.TEL,                                            ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE , TO_CHAR(A.JTIME,'HH24:MI') JTIME,  ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.CHOJAE                                ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_PATIENT B,  ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_DOCTOR D, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                      ";
                SQL += ComNum.VBLF + "      AND A.BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                  ";
                SQL += ComNum.VBLF + "      AND A.BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                  ";
                SQL += ComNum.VBLF + "      AND A.JIN ='H'                                                           ";
                SQL += ComNum.VBLF + "      AND (A.PANO, A.DEPTCODE, A.ACTDATE) NOT IN (                             ";
                SQL += ComNum.VBLF + "          SELECT PANO, DEPTCODE,RDATE FROM KOSMOS_PMPA.OPD_TELRESV             ";
                SQL += ComNum.VBLF + "              WHERE RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')          ";
                SQL += ComNum.VBLF + "                AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')          ";
                SQL += ComNum.VBLF + "                AND GBINTERNET = 'Y')                                          ";
                SQL += ComNum.VBLF + "      AND NOT (A.DeptCode <> 'ER'  AND A.Jin = '2')                            "; //응급실의 타과는 접수ll 인원제외, ~1
                SQL += ComNum.VBLF + "      AND A.Jin <> 'D'                                                         "; //일반건진인원통계제외
                SQL += ComNum.VBLF + "      AND A.Pano <> '81000004'                                                 ";  //전산실 연습
                SQL += ComNum.VBLF + "      AND A.PANO = B.PANO(+)                                                   ";
                SQL += ComNum.VBLF + "      AND A.DeptCode =   C.DeptCode(+)                                         ";
                SQL += ComNum.VBLF + "      AND A.DrCode   =   D.DrCode(+)                                           ";
            }

            else if (optJob7.Checked == true)
            {
                //Fax예약 부도
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                         ";
                SQL += ComNum.VBLF + "  A.DEPTCODE, C.DEPTNAMEK, A.PANO, A.BI, A.SNAME, A.DRCODE,                    ";
                SQL += ComNum.VBLF + "  D.DRNAME, B.JUMIN1, A.SEX, B.TEL,                                            ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE , TO_CHAR(A.JTIME,'HH24:MI') JTIME,  ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.CHOJAE                                ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_PATIENT B,  ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_DOCTOR D, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                      ";
                SQL += ComNum.VBLF + "      AND A.BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                  ";
                SQL += ComNum.VBLF + "      AND A.BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                  ";
                SQL += ComNum.VBLF + "      AND A.JIN ='H'                                                           ";
                SQL += ComNum.VBLF + "      AND (A.PANO, A.DEPTCODE, A.ACTDATE) NOT IN (                             ";
                SQL += ComNum.VBLF + "          SELECT PANO, DEPTCODE,RDATE FROM KOSMOS_PMPA.OPD_TELRESV             ";
                SQL += ComNum.VBLF + "              WHERE RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')          ";
                SQL += ComNum.VBLF + "                AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')          ";
                SQL += ComNum.VBLF + "                AND GBFAX = 'Y')                                               ";
                SQL += ComNum.VBLF + "      AND NOT (A.DeptCode <> 'ER'  AND A.Jin = '2')                            "; //응급실의 타과는 접수ll 인원제외, ~1
                SQL += ComNum.VBLF + "      AND A.Jin <> 'D'                                                         "; //일반건진인원통계제외
                SQL += ComNum.VBLF + "      AND A.Pano <> '81000004'                                                 ";  //전산실 연습
                SQL += ComNum.VBLF + "      AND A.PANO = B.PANO(+)                                                   ";
                SQL += ComNum.VBLF + "      AND A.DeptCode =   C.DeptCode(+)                                         ";
                SQL += ComNum.VBLF + "      AND A.DrCode   =   D.DrCode(+)                                           ";
            }

            SQL += ComNum.VBLF + "      ORDER BY A.CHOJAE, JTIME                                                     ";

            try
            {
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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;
                    ssList_Sheet1.Rows.Count = nREAD;

                    for (i = 0; i < nREAD; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = (i + 1).ToString();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTNAMEK"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BI"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                                ";
                        SQL += ComNum.VBLF + "  PATID                                                               ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_TREATT                                  ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
                        SQL += ComNum.VBLF + "      AND PATID = '" + dt.Rows[i]["Pano"].ToString().Trim() + "'      ";
                        SQL += ComNum.VBLF + "      AND CLASS ='O'                                                  ";
                        SQL += ComNum.VBLF + "      AND CHECKED ='1'                                                ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssList_Sheet1.Cells[i, 5].Text = "SCN";
                        }

                        dt1.Dispose();
                        dt1 = null;

                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = VB.Left(dt.Rows[i]["JUMIN1"].ToString().Trim(), 2) + "/" +
                                                            VB.Mid(dt.Rows[i]["JUMIN1"].ToString().Trim(), 3, 2) + "/" +
                                                                VB.Right(dt.Rows[i]["JUMIN1"].ToString().Trim(), 2);
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["TEL"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 11].Text = dt.Rows[i]["JTIME"].ToString().Trim();

                        switch (dt.Rows[i]["CHOJAE"].ToString().Trim())
                        {
                            case "1":
                                ssList_Sheet1.Cells[i, 12].Text = "초진";
                                break;
                            case "2":
                                ssList_Sheet1.Cells[i, 12].Text = "초진심야";
                                break;
                            case "3":
                                ssList_Sheet1.Cells[i, 12].Text = "재진";
                                break;
                            case "4":
                                ssList_Sheet1.Cells[i, 12].Text = "재진심야";
                                break;
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

            btnPrint.Enabled = true;
            Cursor.Current = Cursors.Default;

            ComFunc.MsgBox("조회가 완료되었습니다.");
        }
    }
}

