using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB;
using ComBase;
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmNrstdMain.cs
    /// Description     : 간호 지표 관리 프로그램
    /// Author          : 안정수
    /// Create Date     : 2018-01-25
    /// TODO : frm감염감시보고서, frmInfection 폼 구현 필요
    /// TODO : ini파일 Read하는 함수 ReadIniFile(), ReadIniFile2() 구현필요(ini파일 사용여부 몰라서 구현 안함)
    /// TODO : 상단 메뉴는 만들어놨으나, 각 메뉴별 폼 사용여부 확인 및 구현필요함
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 FrmMain.frm(FrmMain) 폼 frmNrstdMain.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrstd\FrmMain.frm(FrmMain) >> frmNrstdMain.cs 폼이름 재정의" />
    public partial class frmNrstdMain : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsIpdNr CIN = new clsIpdNr();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;

        string FstrPanoList = "";

        #endregion


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

        public frmNrstdMain(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmNrstdMain()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnPrint);

            //this.eControl.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            //this.eControl.LostFocus += new EventHandler(eControl_LostFocus);
            //this.eControl.GotFocus += new EventHandler(eControl_GotFocus);
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
                ComFunc.ReadSysDate(clsDB.DbCon);

                optSort0.Checked = true;
                Set_Init();
            }
        }

        void eFormActivated(object sender, EventArgs e)
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

        void eFormClosed(object sender, FormClosedEventArgs e)
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

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void Set_Init()
        {
            int i = 0;

            //string strTitle = "";
            string strSabun = "";

            //If App.PrevInstance Then
            //    Title$ = App.Title
            //    App.Title = "Temp"
            //    AppActivate Title$
            //    SendKeys "%{ }{ENTER}"
            //    End
            //End If

            ssList.ActiveSheet.Rows[0, ssList.ActiveSheet.Rows.Count - 1].Height = 18;
            cboWard.Items.Clear();
            cboJob.Items.Clear();

            ssList.ActiveSheet.Columns[26].Visible = false; //입원일자
            ssList.ActiveSheet.Columns[27].Visible = false; //호싱
            ssList.ActiveSheet.Columns[28].Visible = false; //병동
            ssList.ActiveSheet.Columns[29].Visible = false; //IPDNO

            clsNurse.gsWard = READ_IniFile();           //PC의 병동코드
            clsPublic.GstrWardCode = clsNurse.gsWard;
            clsPublic.GstrWardCodes = READ_IniFile2();  //물품청구 부서코드

            cboWard_SET();

            //진료과 ComboSET
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  DeptCode";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ClinicDept";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND DeptCode NOT IN ('II','HR','TO','R6','HD','PT','AN')";
            SQL += ComNum.VBLF + "ORDER BY PrintRanking";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                cboDept.Items.Clear();
                cboDept.Items.Add("전체");

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                }

                cboDept.SelectedIndex = 0;
            }

            dt.Dispose();
            dt = null;

            cboJob.Items.Clear();
            cboJob.Items.Add("1.재원자명단");
            cboJob.SelectedIndex = 0;

            cboStd.Items.Clear();
            cboStd.Items.Add(" ");
            cboStd.Items.Add("1.육체적구속");
            cboStd.Items.Add("2.ICU 재입원");
            cboStd.Items.Add("3.기관지절개");
            cboStd.Items.Add("4.드레싱교환");

            clsPublic.GstrPassProgramID = "";
            menu_bal.Visible = false;

            menu_edu_Rpt1.Visible = false;
            menu_edu_Rpt2.Visible = false;

            switch (clsType.User.IdNumber)
            {
                case "4349":
                case "18319":
                case "13662":
                case "3020":
                case "22394":
                case "22948":
                case "7306":
                case "12306":
                case "23758":
                case "13386":
                case "19700":
                case "8822":
                case "28262":
                case "28093":
                case "31301":                    
                    cboWard.Enabled = true;
                    menu_bal.Visible = true;
                    menu_edu_power.Enabled = true;
                    break;
            }

            if(String.Compare(clsType.User.IdNumber, "99999") <= 0)
            {
                strSabun = ComFunc.SetAutoZero(clsType.User.IdNumber, 5);
            }

            else
            {
                strSabun = ComFunc.SetAutoZero(clsType.User.IdNumber, 6);
            }

            //교육권한
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  ROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Gubun ='1'";
            SQL += ComNum.VBLF + "      AND Remark ='" + strSabun + "' ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                menu_edu_Rpt1.Enabled = true;
                menu_edu_Rpt2.Enabled = true;
                menu_edu_Rpt3.Enabled = true;
            }

            dt.Dispose();
            dt = null;
        }

        void cboWard_SET()
        {
            int i = 0;
            //int j = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";            
            SQL += ComNum.VBLF + "  WardCode, WardName ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND WARDCODE NOT IN ('IU','NP','2W','NR','DR','IQ')";
            SQL += ComNum.VBLF + "ORDER BY WardCode";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                cboWard.Items.Clear();
                cboWard.Items.Add("전체");

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }

                cboWard.Items.Add("SICU");
                cboWard.Items.Add("MICU");
                cboWard.Items.Add("ER");
            }

            foreach(string a in cboWard.Items)
            {
                if(a == clsNurse.gsWard)
                {
                    cboWard.SelectedText = clsNurse.gsWard;
                    cboWard.Enabled = false;
                    break;
                }
            }
        }

        public string READ_IniFile()
        {
            //TODO 
            string rtnVal = "";

            return rtnVal;
        }

        public string READ_IniFile2()
        {
            //TODO 
            string rtnVal = "";

            return rtnVal;
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            btnPrint.Enabled = false;

            strTitle = "병  동   환  자   현  황";
            strSubTitle = "병동 : " + cboWard.SelectedItem.ToString().Trim();
            strSubTitle += "\r\n" + VB.Left("작업방법 : " + cboJob.SelectedItem.ToString().Trim() + VB.Space(50), 50);
            strSubTitle += "\r\n" + "출력시간 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);            

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, false, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            btnPrint.Enabled = true;
        }

        void eGetData()
        {

            #region 변수 선언부
            int i = 0;
            int j = 0;
            int nLine = 0;
            int nREAD = 0;
            int nRow = 0;

            //int b = 0;
            //int P = 0;
            //int X = 0;
            //int nGrade = 0;

            //string strWARD = "";
            string strOK = "";

            string strJob = "";
            string strPano = "";
            string strInDate = "";
            string strRoom = "";
            //string strOpSche = "";
            //string strSpecialExam = "";
            string strOldCode = "";
            string strNextDate = "";
            string strToDate = "";
            string strPriDate = "";

            //bool bToday = false;

            string strRoutDate = "";
            string strAmSet1 = "";
            string strAmSet3 = "";
            //string strAmSetB = "";
            string strROutGbPrt = "";
            //string strICU = "";
            string strSTS = "";

            long nIpdNo = 0;
            long[] nCNT = new long[50];
            #endregion

            Cursor.Current = Cursors.WaitCursor;

            if (cboWard.SelectedItem.ToString().Trim() == "MICU" || cboWard.SelectedItem.ToString().Trim() == "SICU")
            {
                //strICU = "OK";
            }

            else
            {
                //strICU = "";
            }

            if(cboWard.SelectedItem.ToString().Trim() == "")
            {
                ComFunc.MsgBox("병동이 공란입니다.", "오류");
                return;
            }

            if (cboJob.SelectedItem.ToString().Trim() == "")
            {
                ComFunc.MsgBox("작업방법이 공란입니다.", "오류");
                return;
            }

            strJob = VB.Left(cboJob.SelectedItem.ToString().Trim(), 1);

            clsNurse.gsWard = cboWard.SelectedItem.ToString().Trim();

            ssList.ActiveSheet.Rows.Count = 0;
            ssList.ActiveSheet.Rows.Count = 30;


            strPriDate = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-1).ToShortDateString();
            strToDate = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(0).ToShortDateString();
            strNextDate = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(1).ToShortDateString();

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName,";
            SQL += ComNum.VBLF + "  TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,M.Ilsu,M.IpdNo,M.GbSts,";
            SQL += ComNum.VBLF + "  TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate,";
            SQL += ComNum.VBLF + "  TO_CHAR(M.ROutDate,'YYYY-MM-DD') ROutDate,M.AmSet3,";
            SQL += ComNum.VBLF + "  M.DeptCode,M.DrCode,D.DrName,M.AmSet1,M.AmSet6,M.AmSet7";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER M, " + ComNum.DB_PMPA + "BAS_PATIENT P, " + ComNum.DB_PMPA + "BAS_DOCTOR D";
            SQL += ComNum.VBLF + "WHERE 1=1";
            switch (cboWard.SelectedItem.ToString().Trim())
            {
                case "전체":
                    SQL += ComNum.VBLF + "AND M.WardCode>' ' ";
                    break;

                case "MICU":
                    SQL += ComNum.VBLF + "AND M.RoomCode='234' ";
                    break;

                case "SICU":
                    SQL += ComNum.VBLF + "AND M.RoomCode='233' ";
                    break;

                case "ND":
                    SQL += ComNum.VBLF + "AND M.WardCode IN ('ND','IQ') ";
                    break;

                default:
                    SQL += ComNum.VBLF + "AND M.WardCode='" + cboWard.SelectedItem.ToString().Trim() + "'";
                    break;
            }

            if(clsType.User.IdNumber != "4349")
            {
                SQL += ComNum.VBLF + "    AND M.Pano<>'81000004'";
            }

            //진료과
            if(cboDept.SelectedItem.ToString().Trim() != "전체")
            {
                SQL += ComNum.VBLF + "    AND M.DeptCode='" + cboDept.SelectedItem.ToString().Trim() + "'";
            }

            //작업분류
            if(strJob == "1")       //재원자
            {
                SQL += ComNum.VBLF + "    AND (M.OutDate IS NULL OR M.OutDate>=TO_DATE('" + strToDate + "','YYYY-MM-DD'))";
                SQL += ComNum.VBLF + "    AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND M.Pano < '90000000'";
                SQL += ComNum.VBLF + "    AND M.GbSTS <> '9'";                
            }

            SQL += ComNum.VBLF + "        AND M.Pano=P.Pano(+)";
            SQL += ComNum.VBLF + "        AND M.DrCode=D.DrCode(+)";

            //SORT
            if(optSort0.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY M.RoomCode,M.SName, M.Indate DESC";
            }

            else if(optSort1.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY M.SName,M.Pano,M.Indate DESC";
            }

            else
            {
                SQL += ComNum.VBLF + "ORDER BY M.DeptCode,D.DrName,M.SName ,M.Indate DESC";
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                
                return;
            }

            if(dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;
                
                for(i = 0; i < nREAD; i++)
                {                    
                    strOK = "OK";
                    nIpdNo = Convert.ToInt64(VB.Val(dt.Rows[i]["IpdNo"].ToString().Trim()));
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    strInDate = dt.Rows[i]["InDate"].ToString().Trim();
                    strRoom = dt.Rows[i]["RoomCode"].ToString().Trim();
                    strRoutDate = dt.Rows[i]["RoutDate"].ToString().Trim();

                    if(String.Compare(strRoutDate, clsPublic.GstrSysDate) < 0)
                    {
                        strRoutDate = "";
                    }

                    strSTS = dt.Rows[i]["GbSts"].ToString().Trim();
                    strAmSet1 = dt.Rows[i]["AmSet1"].ToString().Trim();
                    strAmSet3 = dt.Rows[i]["AmSet3"].ToString().Trim();

                    //재원자
                    if(strJob == "1")
                    {
                        if(strSTS == "7")
                        {
                            strOK = "NO";
                        }

                        else if(strSTS != "7")
                        {
                            strOK = "OK";
                        }
                    }

                    //억제대 사용여부
                    if(cboStd.SelectedIndex != -1)
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  Remark";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_MASTER";
                        SQL += ComNum.VBLF + "WHERE IPDNO = " + nIpdNo + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저
                            return;
                        }

                        if(dt2.Rows.Count > 0)
                        {
                            switch (VB.Left(cboStd.SelectedItem.ToString().Trim(), 1))
                            {
                                case "1":
                                    if(VB.I(dt2.Rows[0]["Remark"].ToString().Trim(), "▶육체적구속,") > 1)
                                    {
                                        strOK = "OK";
                                    }
                                    break;

                                case "2":
                                    if (VB.I(dt2.Rows[0]["Remark"].ToString().Trim(), "▶ICU재입원,") > 1)
                                    {
                                        strOK = "OK";
                                    }
                                    break;

                                case "3":
                                    if (VB.I(dt2.Rows[0]["Remark"].ToString().Trim(), "▶기관지절개,") > 1)
                                    {
                                        strOK = "OK";
                                    }
                                    break;

                                case "4":
                                    if (VB.I(dt2.Rows[0]["Remark"].ToString().Trim(), "▶드레싱교환,") > 1)
                                    {
                                        strOK = "OK";
                                    }
                                    break;
                            }
                        }

                        else
                        {
                            strOK = "";
                        }

                        dt2.Dispose();
                        dt2 = null;
                    }

                    if(strOK == "OK")
                    {
                        FstrPanoList = FstrPanoList + "'" + strPano + "',";
                        nRow += 1;

                        if(nRow > ssList.ActiveSheet.Rows.Count)
                        {
                            ssList.ActiveSheet.Rows.Count = nRow;
                        }

                        //호실별로 조회시 같은 병실명은 생략
                        if(optSort0.Checked == true)
                        {
                            if(strOldCode != strRoom)
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 0].Text = strRoom;
                                strOldCode = strRoom;
                            }
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 0].Text = strRoom;
                        }

                        ssList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 3].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 6].Text = VB.Mid(strInDate, 6, 2) + "/" + VB.Right(strInDate, 2);
                        ssList.ActiveSheet.Cells[nRow - 1, 7].Text = Convert.ToInt32(VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim())).ToString();

                        //3줄마다 배경색 표시
                        nLine += 1;

                        if(nLine == 3)
                        {
                            ssList.ActiveSheet.Rows[nRow - 1].BackColor = Color.LightSalmon;
                            nLine = 0;
                        }

                        if(strSTS == "7")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, ssList.ActiveSheet.Columns.Count - 1].BackColor = Color.OrangeRed;                            
                            ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "퇴원완료";
                        }

                        else if(strSTS == "6")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, ssList.ActiveSheet.Columns.Count - 1].BackColor = Color.Yellow;
                            ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "계산발부";
                        }

                        else if (strSTS == "5")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, ssList.ActiveSheet.Columns.Count - 1].BackColor = Color.Yellow;
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT ";
                            SQL += ComNum.VBLF + "  PANO";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "      AND IPDNO =" + nIpdNo + "";
                            SQL += ComNum.VBLF + "      AND OUTDATE =TRUNC(SYSDATE)";
                            SQL += ComNum.VBLF + "      AND GbSts ='6'";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("21");
                                return;
                            }

                            if(dt2.Rows.Count > 0)
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "게산발부";
                            }

                            ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "심사중";

                            dt2.Dispose();
                            dt2 = null;
                        }

                        else if (strSTS == "1")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, ssList.ActiveSheet.Columns.Count - 1].BackColor = Color.Yellow;
                            ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "가퇴원";
                        }

                        else if (strRoutDate == clsPublic.GstrSysDate)
                        {
                            if (strSTS == "5")
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, ssList.ActiveSheet.Columns.Count - 1].BackColor = Color.LightBlue;
                                ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "심사완료";
                            }

                            else if(strSTS == "4")
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "심사중";
                            }
                        }

                        else if(String.Compare(strRoutDate, clsPublic.GstrSysDate) > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 24].Text = VB.Right(strRoutDate, 5);
                        }

                        ssList.ActiveSheet.Cells[nRow - 1, 26].Text = strInDate;
                        ssList.ActiveSheet.Cells[nRow - 1, 27].Text = dt.Rows[i]["RoomCode"].ToString().Trim();

                        switch (dt.Rows[i]["RoomCode"].ToString().Trim())
                        {
                            case "233":
                                ssList.ActiveSheet.Cells[nRow - 1, 28].Text = "SICU";
                                break;

                            case "234":
                                ssList.ActiveSheet.Cells[nRow - 1, 28].Text = "MICU";
                                break;

                            default:
                                ssList.ActiveSheet.Cells[nRow - 1, 28].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                                break;
                        }

                        ssList.ActiveSheet.Cells[nRow - 1, 29].Text = dt.Rows[i]["IPDNO"].ToString().Trim();

                        //병동 환자마스터를 읽음
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  TO_CHAR(ROutDate,'YYYY-MM-DD') ROutDate,ROutGbPrt,Grade,Diagnosis,Remark,IpdNo,Bun1,Bun2,Bun4";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_MASTER";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND Pano='" + strPano + "'";
                        SQL += ComNum.VBLF + "      AND InDate=TO_DATE('" + strInDate + "','YYYY-MM-DD')";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");                            
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장     
                            ComFunc.MsgBox("20");
                            return;
                        }

                        if(dt1.Rows.Count > 0)
                        {
                            strROutGbPrt = dt1.Rows[0]["RoutGbPrt"].ToString().Trim();

                            ssList.ActiveSheet.Cells[nRow - 1, 8].Text = dt1.Rows[0]["Diagnosis"].ToString().Trim();

                            //중증도 체크
                            if(VB.Right(cboWard.SelectedItem.ToString().Trim(), 3)!= "ICU")
                            {
                                if (Convert.ToInt32(VB.Val(dt1.Rows[0]["Bun1"].ToString().Trim())) + Convert.ToInt32(VB.Val(dt1.Rows[0]["Bun2"].ToString().Trim())) + Convert.ToInt32(VB.Val(dt1.Rows[0]["Bun4"].ToString().Trim())) >= 7)
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1 , 9].Text = dt1.Rows[0]["Grade"].ToString().Trim() + "/F";
                                }

                                else
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 9].Text = dt1.Rows[0]["Grade"].ToString().Trim();
                                }
                            }

                            else
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 9].Text = dt1.Rows[0]["Grade"].ToString().Trim();
                            }

                            ssList.ActiveSheet.Cells[nRow - 1, 25].Text = dt1.Rows[0]["Remark"].ToString().Trim();

                            //퇴원예고
                            if(String.Compare(dt1.Rows[0]["ROutDate"].ToString().Trim(), clsPublic.GstrSysDate) >= 0)
                            {
                                if(ssList.ActiveSheet.Cells[nRow - 1, 24].Text == "")
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "☞" + VB.Right(dt1.Rows[0]["RoutDate"].ToString().Trim(), 5);
                                }
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        //간호진단 체크
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_JIN_ITEM";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                        SQL += ComNum.VBLF + "      AND InDate =TO_DATE('" + dt.Rows[i]["Indate"].ToString().Trim() + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND WARD ='" + dt.Rows[i]["WardCode"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "      AND GUBUN ='1'";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("1");
                            return;
                        }

                        if(dt2.Rows.Count > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 11].Text = "◎";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 11].Text = "";
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //욕창
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_PRESSURE_SORE";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND IPDNO=" + dt.Rows[i]["IPDNO"].ToString().Trim() + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("2");
                            return;
                        }

                        if(dt2.Rows.Count > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 13].Text = "◎";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 13].Text = "";
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //낙상
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND IPDNO=" + dt.Rows[i]["IPDNO"].ToString().Trim() + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("3");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 14].Text = "◎";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 14].Text = "";
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //사망
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_DEATH";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND IPDNO=" + dt.Rows[i]["IPDNO"].ToString().Trim() + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("4");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 15].Text = "◎";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 15].Text = "";
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //투약
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_DRUG";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND IPDNO=" + dt.Rows[i]["IPDNO"].ToString().Trim() + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("5");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 16].Text = "◎";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 16].Text = "";
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //감염감시
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND IPDNO=" + dt.Rows[i]["IPDNO"].ToString().Trim() + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("6");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 17].Text = "◎";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 17].Text = "";
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //법정전염병
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT2";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND IPDNO=" + dt.Rows[i]["IPDNO"].ToString().Trim() + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("7");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 18].Text = "◎";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 18].Text = "";
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //결핵
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT2";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND IPDNO=" + dt.Rows[i]["IPDNO"].ToString().Trim() + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("8");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 19].Text = "◎";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 19].Text = "";
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //매독
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT4";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND IPDNO=" + dt.Rows[i]["IPDNO"].ToString().Trim() + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("9");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 20].Text = "◎";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 20].Text = "";
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //VRE
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT5";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND IPDNO=" + dt.Rows[i]["IPDNO"].ToString().Trim() + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("10");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 21].Text = "◎";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 21].Text = "";
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //성병
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT6";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND IPDNO=" + dt.Rows[i]["IPDNO"].ToString().Trim() + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("11");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 22].Text = "◎";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 22].Text = "";
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //바이러스성 감염
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_INFECT7";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND IPDNO=" + dt.Rows[i]["IPDNO"].ToString().Trim() + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("12");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 23].Text = "◎";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 23].Text = "";
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //퇴원계산서 인쇄
                        if(ssList.ActiveSheet.Cells[nRow - 1, 24].Text == "계산발부")
                        {
                            if(strROutGbPrt == "Y")
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "퇴원통보";
                            }
                        }
                    }
                }
            }
            
            dt.Dispose();
            dt = null;

            ssList.ActiveSheet.Rows.Count = nRow;

            if(FstrPanoList != "")
            {
                FstrPanoList = VB.Left(FstrPanoList, FstrPanoList.Length - 1);

                #region btnView_EMR

                //EMR 스캔여부를 Display
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  Patid,COUNT(*) CNT ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_TREATT";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND Patid IN (" + FstrPanoList + ")";
                SQL += ComNum.VBLF + "      AND Checked='1'";
                SQL += ComNum.VBLF + "GROUP BY Patid ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < ssList.ActiveSheet.Rows.Count; i++)
                    {
                        strPano = ssList.ActiveSheet.Cells[i, 1].Text;

                        for(j = 0; j < dt.Rows.Count; j++)
                        {
                            if(dt.Rows[j]["Patid"].ToString().Trim() == strPano)
                            {
                                ssList.ActiveSheet.Cells[i, 10].Text = "▦";
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion
            }


            Display_OpSche();

            Cursor.Current = Cursors.Default;
        }

        void Display_OpSche()
        {
            int i = 0;
            int j = 0;
            //int nRow = 0;
            int nREAD = 0;

            //string strData = "";
            string strChkDate = "";

            //오늘 수술여부 비교용 날짜
            strChkDate = VB.Mid(clsPublic.GstrSysDate, 6, 2) + "/" + VB.Right(clsPublic.GstrSysDate, 2);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  TO_CHAR(S.OpDate,'MM/DD') OpDate,";
            SQL += ComNum.VBLF + "  S.OpSeq,    S.OpTime,   S.Pano,";
            SQL += ComNum.VBLF + "  M.SName,    M.Sex,      M.Age,         M.RoomCode,";
            SQL += ComNum.VBLF + "  S.DeptCode, S.OpStaff,  O.DrName OpDr, S.OpIll,  S.PreDiagnosis,";
            SQL += ComNum.VBLF + "  S.Anesth,   S.AnDrCode, A.DrName AnDr, S.OpTime_New";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OPSCHE S, " + ComNum.DB_PMPA + "BAS_DOCTOR O, ";
            SQL += ComNum.VBLF +           ComNum.DB_PMPA + "BAS_DOCTOR A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER M";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND S.OpDate  >= TRUNC(SYSDATE)";
            SQL += ComNum.VBLF + "      AND  S.GbDel  <> '*'";
            SQL += ComNum.VBLF + "      AND  S.OpStaff  = O.DrCode(+)";
            SQL += ComNum.VBLF + "      AND  S.AnDrCode = A.DrCode(+)";
            SQL += ComNum.VBLF + "      AND  S.Pano     = M.Pano(+)";

            switch (cboWard.SelectedItem.ToString().Trim())
            {
                case "전체":
                    SQL += ComNum.VBLF + "AND M.WardCode>' ' ";
                    break;

                case "MICU":
                    SQL += ComNum.VBLF + "AND M.RoomCode='234' ";
                    break;

                case "SICU":
                    SQL += ComNum.VBLF + "AND M.RoomCode='233' ";
                    break;

                case "ND":
                    SQL += ComNum.VBLF + "AND M.WardCode IN ('ND','IQ') ";
                    break;

                default:
                    SQL += ComNum.VBLF + "AND M.WardCode='" + cboWard.SelectedItem.ToString().Trim() + "'";
                    break;
            }
            
            SQL += ComNum.VBLF + "      AND  M.GbSts IN ('0','2')";
            SQL += ComNum.VBLF + "      AND  M.OutDate IS NULL";
            SQL += ComNum.VBLF + "ORDER  BY S.OpDate,S.OpTime ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                
                return;
            }

            if(dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;
                //nRow = 0;
                
                for(i = 0; i < nREAD; i++)
                {
                    //환자명단에 오늘 수술 표시
                    if(dt.Rows[i]["OpDate"].ToString().Trim() == strChkDate)
                    {
                        for(j = 0; j < ssList.ActiveSheet.Rows.Count; j++)
                        {
                            if(dt.Rows[i]["Pano"].ToString().Trim() == ssList.ActiveSheet.Cells[j, 1].Text)
                            {
                                ssList.ActiveSheet.Cells[j, 12].Text = "◎";
                                break;
                            }
                        }
                    }
                }
            }

            dt.Dispose();
            dt = null;
        }

        void ssList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //string strPano = "";
            //string strDept = "";
            //string strInDate = "";

            clsPublic.GstrRetValue = "";

            clsPublic.GstrHelpCode = ssList.ActiveSheet.Cells[e.Row, 1].Text;   //등록번호
            clsPublic.GstrPANO = ssList.ActiveSheet.Cells[e.Row, 1].Text;   
            CIN.GnIPDNO = Convert.ToInt64(VB.Val(ssList.ActiveSheet.Cells[e.Row, 1].Text));   

            if(CIN.GnIPDNO != 0 && clsPublic.GstrPANO != "")
            {
                switch (e.Column)
                {
                    case 17:
                        // TODO : Frm감염감시보고서.Show '감염감시
                        break;
                    case 18:
                        clsPublic.GstrHelpCode = "1";
                        // TODO : FrmInfection.Show      '법정전염
                        break;
                    case 19:
                        clsPublic.GstrHelpCode = "3";
                        // TODO : FrmInfection.Show      '결핵
                        break;
                    case 20:
                        clsPublic.GstrHelpCode = "2";
                        // TODO : FrmInfection.Show      '선천성매독
                        break;
                    case 21:
                        clsPublic.GstrHelpCode = "4";
                        // TODO : FrmInfection.Show      'VER 감염
                        break;
                    case 22:
                        clsPublic.GstrHelpCode = "5";
                        // TODO : FrmInfection.Show      '성병
                        break;
                    case 23:
                        clsPublic.GstrHelpCode = "6";
                        // TODO : FrmInfection.Show      '바이러스성  감염
                        break;
                }
            }

            if(e.Column == 25)
            {
                #region SS1_Memo_Rtn(GoSub)

                if(ssList.ActiveSheet.Cells[e.Row, e.Column].Text == "")
                {
                    return;
                }

                ComFunc.MsgBox(ssList.ActiveSheet.Cells[e.Row, e.Column].Text, "확인");

                #endregion
            }

        }
    }
}
