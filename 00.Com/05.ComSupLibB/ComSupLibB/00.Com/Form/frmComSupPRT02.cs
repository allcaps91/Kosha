using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.SupEnds;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;


namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupPRT02.cs
    /// Description     : 내시경 라벨출력 관련 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-09-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\ocs\endo\endo_new\Frm라벨인쇄.frm(Frm라벨인쇄) >> frmComSupPRT02.cs c#에서 폼 추가함" />
    public partial class frmComSupPRT02 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsSpread spread = new clsSpread();
        clsPrint cp = new clsPrint();
        clsComSup sup = new clsComSup();

        clsComSupEndsSQL cendsSQL = new clsComSupEndsSQL();
        clsComSup.SupPInfo cinfo = new clsComSup.SupPInfo();
        clsComSup.cBasPatient cBasPatient = null;
        

        string gROWID = "";
        string gTemp = "";
        string gPrintName = "";
        string gFall = "";
        string gdrname = "";
        bool gExit = true;
        bool gExit2 = false;

        Int32 PrtX = 0;
        Int32 PrtY = 0;

        #endregion

        public frmComSupPRT02(string argROWID,string argPrint,string argTemp="",string argFall="",bool argExit =true,bool argExit2 =false, string argdrname = "")
        {
            InitializeComponent();
            gROWID = argROWID;
            gTemp = argTemp;
            gPrintName = argPrint;
            gExit = argExit;
            gExit2 = argExit2;
            gFall = argFall;
            gdrname = argdrname;
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            //txtMonth.Text = VB.Left(cpublic.strSysDate, 4) + "년 " + VB.Mid(cpublic.strSysDate, 6, 2) + "월";

            lblDate.Text = cpublic.strSysDate;
            

        }

        void setCtrlInit()
        {
            clsCompuInfo.SetComputerInfo();
            DataTable dt = ComQuery.Select_BAS_PCCONFIG(clsDB.DbCon, clsCompuInfo.gstrCOMIP, "프로그램PC세팅", "프린트설정", "내시경인쇄XY");

            if (ComFunc.isDataTableNull(dt) == false)
            {
                string s = dt.Rows[0]["VALUEV"].ToString();
                if (s !="")
                {
                    PrtX = Convert.ToInt32(clsComSup.setP(s, ",", 1).Trim());
                    PrtY = Convert.ToInt32(clsComSup.setP(s, ",", 2).Trim());

                    lblPrtXY.Text = "인쇄좌표(X, Y) : " + s;
                }
                
            }           
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
    
            this.btnExit.Click += new EventHandler(eBtnClick);            

            this.btnPrint1.Click += new EventHandler(eBtnPrint);
            this.btnPrint2.Click += new EventHandler(eBtnPrint);
            this.btnPrint3.Click += new EventHandler(eBtnPrint);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                if (gExit == false)
                {
                    btnExit.Visible = false;
                }          
                     
                screen_clear();

                setCtrlData();

                setCtrlInit();

                screen_display();
                
            }

        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {

            //if (sender == this.btnSearch)
            //{
            //    screen_display();
            //}
            
        }

        void eBtnSave(object sender, EventArgs e)
        {
            

        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint1)
            {                
                ePrint("라벨인쇄", "혈액환자정보");
                if (gExit2 == true)
                {
                    this.Close();
                    return;
                }
            }
            else if (sender == this.btnPrint2)
            {
                ePrint("조직검사수납표", "접수증");
            }
            else if (sender == this.btnPrint3)
            {
                prtBarCodeBlood();
            }
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void ePrint(string s,string prtName)
        {
            gPrintName = prtName;

            if (s == "라벨인쇄")
            {
                if (setPrt1(ssPrt1) == true)
                {
                    for (int i = 1; i <= Convert.ToInt16(txtPage.Text); i++)
                    {
                        SpreadPrint(ssPrt1, false);
                    }
                    
                }
            }
            else  if (s =="조직검사수납표")
            {
                if (setPrt2(clsDB.DbCon, ssPrt2) == true)
                {
                    SpreadPrint(ssPrt2, false);
                }                
            }            
        }

        // 수혈라벨 출력
        private void prtBarCodeBlood()
        {
            string mstrPrintName = "혈액환자정보";
            string strPrintName1 = "";
            string strPrintName2 = "";

            clsPrint CP = new clsPrint();

            try
            {
                strPrintName1 = clsPrint.gGetDefaultPrinter();
                strPrintName2 = CP.getPrinter_Chk(mstrPrintName.ToUpper());            

                if (strPrintName2 == "")
                {
                    ComFunc.MsgBox("프린터 설정 오류입니다. 전산정보팀(☏29047)에 연락바랍니다.");
                    return;
                }
                
                PrintDocument pd = new PrintDocument();
                PrintController pc = new StandardPrintController();
                pd.PrintController = pc;
                pd.PrinterSettings.PrinterName = strPrintName2;
                pd.PrintPage += new PrintPageEventHandler(ePrintPage3);

                pd.Print();    //프린트             
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show(Ex.ToString());
                return;
            }
        }        

        private void ePrintPage3(object sender, PrintPageEventArgs ev)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            clsVbfunc CV = new clsVbfunc();
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Far;

            Font printFont;
            string sFont = "나눔고딕";
            int nX;
            int nY;
            string s;

            string strSName = "";
            string strPano = "";
            string strSex = "";
            string strAge = "";
            string strWard = "";
            string strJumin = "";
            string strNameE = "";
            string strRoom = "";
            string strABO = "";

            strPano = ComFunc.SetAutoZero(lblPtno.Text.Trim(), ComNum.LENPTNO);

            try
            {
                if (cBasPatient.IPDNO != "")
                {
                    //'환자정보를 READ
                    SQL = "SELECT b.Sname,b.Jumin1,b.Jumin2,b.Tel,b.HPhone,b.ZipCode1,b.ZipCode2,b.Juso,";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(a.InDate,'YYYY-MM-DD') InDate,a.Ilsu,a.Sex,a.Age, ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(a.InDate,'HH24:MI') InTime,a.Ilsu,a.Sex,a.Age, ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(a.OutDate,'YYYY-MM-DD') OutDate,a.GbKekli,a.AmSet1,";
                    SQL = SQL + ComNum.VBLF + "  a.WardCode,a.RoomCode,a.DeptCode,a.DrCode,a.IPDNO,a.OP_JIPYO ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER a,KOSMOS_PMPA.BAS_PATIENT b ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.Pano = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.GbSts NOT IN ('9')  ";
                    SQL = SQL + ComNum.VBLF + "  AND a.IPDNO =" + cBasPatient.IPDNO + " ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Pano=b.Pano(+) ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strSName = dt.Rows[0]["Sname"].ToString().Trim();
                        strSex = dt.Rows[0]["SEX"].ToString().Trim();
                        strAge = clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPano);
                        strWard = dt.Rows[0]["WARDCODE"].ToString().Trim();
                        strJumin = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1) + "******";
                        strNameE = VB.Left(CV.Read_Patient_Ename(clsDB.DbCon, strPano, "3"), 3);

                        //'입원환자는 진료과 대신에 병동을 바코드에 인쇄함

                        //'중환자실 SICU,MICU분리
                        if (strWard == "IU")
                        {
                            if (dt.Rows[0]["RoomCODE"].ToString().Trim() == "233")
                            {
                                strWard = "SICU";
                            }
                            else if (dt.Rows[0]["RoomCODE"].ToString().Trim() == "234")
                            {
                                strWard = "MICU";
                            }
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    strSName = cBasPatient.SName;
                    strSex = cBasPatient.Sex;
                    strAge = clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPano);
                    strWard = "OPD";
                    strJumin = VB.Left(cBasPatient.JuminFull, 6) + "-" + VB.Mid(cBasPatient.JuminFull, 7, 1) + "******";
                    strNameE = VB.Left(CV.Read_Patient_Ename(clsDB.DbCon, strPano, "3"), 3);
                }
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            try
            {
                SQL = "SELECT ABO from KOSMOS_OCS.EXAM_BLOOD_MASTER  WHERE PANO = '" + strPano + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strABO = dt.Rows[0]["abo"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            nX = PrtX;
            nY = PrtY;

            printFont = new Font(sFont, 12, FontStyle.Bold);
            s = strPano + " " + strSex + "/" + strAge;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 20, nY + 10);

            printFont = new Font(sFont, 9, FontStyle.Bold);
            s = strWard + (strRoom != "" ? "/" + strRoom : "");
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 160, nY + 10);

            printFont = new Font(sFont, 20, FontStyle.Bold);
            s = strABO;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 0, nY + 40);

            printFont = new Font(sFont, 18, FontStyle.Bold);
            s = strSName;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 60, nY + 40);


            printFont = new Font(sFont, 12, FontStyle.Bold);
            s = "수";
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 170, nY + 40);

            printFont = new Font(sFont, 12, FontStyle.Bold);
            s = "혈";
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 170, nY + 55);


            printFont = new Font(sFont, 10, FontStyle.Bold);
            s = "Date: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 20, nY + 80);
        }

        public void SpreadPrint(FpSpread o, bool prePrint)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                return;
            }
            else
            {
                string sPrtName = ""; //선택 프린트명
                string bPrtName = ""; //기본 프린트명
                                
                if (gPrintName.ToUpper() == "기본프린트")
                {
                    sPrtName = clsPrint.gGetDefaultPrinter();
                }
                else
                {
                    sPrtName = cp.getPrinter_Chk(gPrintName.ToUpper());
                }

                if (sPrtName != "")
                {
                    bPrtName = clsPrint.gGetDefaultPrinter();
                    clsPrint.gSetDefaultPrinter(sPrtName);

                    string header = string.Empty;
                    string foot = string.Empty;

                    clsSpread.SpdPrint_Margin margin = new clsSpread.SpdPrint_Margin(0, 0, PrtY + 0, 0, PrtX + 0, 0);
                    clsSpread.SpdPrint_Option option = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait
                                                    , PrintType.All, 0, 0, false, false, false, false, false, false, false);

                    spread.setSpdPrint(o, prePrint, margin, option, header, foot);

                    ComFunc.Delay(100);
                    clsPrint.gSetDefaultPrinter(bPrtName);

                }

            }

        }                

        /// <summary>
        /// 내시경 라벨인쇄
        /// </summary>
        /// <param name="Spd"></param>
        /// <returns></returns>
        bool setPrt1(FpSpread Spd)
        {
            bool b = true;

            #region 내시경 바코드

            spread.Spread_Clear(Spd, Spd.ActiveSheet.RowCount, Spd.ActiveSheet.ColumnCount);

            Spd.ActiveSheet.Cells[1, 1].Text = cBasPatient.SName + " " + cBasPatient.Pano + " (" + cBasPatient.Age + "/" + cBasPatient.Sex + ")";
            if (cBasPatient.infect=="Y")
            {
                Spd.ActiveSheet.Cells[2, 1].Text = "감염정보확인";
            }

            Spd.ActiveSheet.Cells[3, 1].Text = "Endoscopy Room(" + cBasPatient.DeptCode + ")  " + gdrname;
            Spd.ActiveSheet.Cells[4, 1].Text = "Date: " +cpublic.strSysDate.Replace("-", "/");

            #endregion

            return b;
        }

        /// <summary>
        /// 내시경 조직검사 수납표 인쇄
        /// </summary>
        /// <param name="Spd"></param>
        /// <returns></returns>
        bool setPrt2(PsmhDb pDbCon, FpSpread Spd)
        {
            bool b = true;
            string strPat = "";

            try
            {
                string s = cBasPatient.Pano + VB.Asc(VB.Left(cBasPatient.DeptCode, 1)) + VB.Asc(VB.Right(cBasPatient.DeptCode, 1));
                string s2 = "";

                strPat = cBasPatient.Pano;
                if (cBasPatient.RoomCode !="")
                {
                    strPat += "(" + cBasPatient.RoomCode +")";
                }
                strPat += "\r\n";
                strPat += cBasPatient.SName + "(" + cBasPatient.Sex +"/"+ cBasPatient.Age + ")" + "\r\n" ;

                if (cBasPatient.infect == "Y")
                {
                    s2 += strPat + "감염정보확인";
                }
                if (cBasPatient.fall == "Y" || gFall != "")
                {
                    if (s2 == "")
                    {
                        s2 += strPat+ "낙상확인";
                    }
                    else
                    {
                        s2 += " " + "낙상확인"; 
                    }
                }
                else
                {
                    s2 += strPat;
                }

                Spd.ActiveSheet.Cells[0, 4].Text = s;
                Spd.ActiveSheet.Cells[0, 1].Text = s2;
                sup.setColStyle_Text(ssPrt2, 1, 2, true, true, false, 500);                
                Spd.ActiveSheet.Cells[1, 2].Text = cBasPatient.RemarkC + "\r\n" + cBasPatient.RemarkD; //cc
                Spd.ActiveSheet.Rows.Get(1).Height = Spd.ActiveSheet.Rows[1].GetPreferredHeight();
                Spd.ActiveSheet.Cells[2, 2].Text = clsVbfunc.GetBASDoctorName(pDbCon, cBasPatient.DrCode); //의사
                Spd.ActiveSheet.Cells[2, 5].Text = cBasPatient.OrderName;

                return b;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }

        }

        void screen_clear()
        {

            read_sysdate();

            //txtSearch.Text = "";   
            //dtpFDate.Text =cpublic.strSysDate;  
            

        }

        void screen_display()
        {
            GetData(clsDB.DbCon,gROWID);
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void GetData(PsmhDb pDbCon, string argROWID)
        {
           
            DataTable dt = null;

            cBasPatient = new clsComSup.cBasPatient();                       

            dt = cendsSQL.sel_ENDO_JUPMST_REMARK(pDbCon, argROWID);

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {

                cBasPatient.Pano = dt.Rows[0]["Ptno"].ToString().Trim();
                cBasPatient.SName = dt.Rows[0]["SName"].ToString().Trim();
                cBasPatient.DeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                cBasPatient.DrCode = dt.Rows[0]["DrCode"].ToString().Trim(); 
                if (dt.Rows[0]["Jumin3"].ToString().Trim()!="")
                {
                    cBasPatient.JuminFull = dt.Rows[0]["Jumin1"].ToString().Trim() +  clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                }
                else
                {
                    cBasPatient.JuminFull = dt.Rows[0]["Jumin1"].ToString().Trim() + dt.Rows[0]["Jumin2"].ToString().Trim();
                }

                cBasPatient.Sex = dt.Rows[0]["Sex"].ToString().Trim();
                cBasPatient.Age = 0;
                if (dt.Rows[0]["FC_AGE"].ToString().Trim() !="")
                {
                    cBasPatient.Age = Convert.ToInt16(dt.Rows[0]["FC_AGE"].ToString().Trim());
                }
                
                
                cBasPatient.OrderName = dt.Rows[0]["OrderName"].ToString().Trim();
                cBasPatient.RemarkC = dt.Rows[0]["RemarkC"].ToString().Trim();
                cBasPatient.RemarkD = dt.Rows[0]["RemarkD"].ToString().Trim();

                if (dt.Rows[0]["FC_infect"].ToString().Trim() !="00000")
                {
                    cBasPatient.infect = "Y";
                }
                if (dt.Rows[0]["FC_fall"].ToString().Trim() != "")
                {
                    cBasPatient.fall = "Y";
                }

                cBasPatient.RoomCode = gTemp;

                cBasPatient.IPDNO = dt.Rows[0]["FC_Jaewon"].ToString().Trim();

                lblPtno.Text = dt.Rows[0]["Ptno"].ToString().Trim();
                lblSName.Text = dt.Rows[0]["SName"].ToString().Trim() + "(" + cBasPatient.Age + "/" + cBasPatient.Sex +")";
                lblDept.Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                                
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion

        }

    }
}
