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
using ComPmpaLibB.Properties;

namespace ComPmpaLibB
{
    public partial class frmAutoPrintIPD : Form
    {
        int FnCntPic = 0;
        int FnCntWork = 0;
        int FnDelay = 0;
        bool FnWorkFlag = false;
        string FstrRout = ""; //퇴원예고자 인쇄
        string Fstr임의출력 = ""; //퇴원예고자 인쇄
        string[] FstrJSimSabun = null;
  
        //int FnDeptCNT = 0;
        //string[] GstrSETNus = null;


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

        public frmAutoPrintIPD()
        {
            InitializeComponent();
        }

        public frmAutoPrintIPD(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmAutoPrintIPD(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }

        private void frmAutoPrint_Load(object sender, EventArgs e)
        {
            clsBasAcct CBA = new clsBasAcct();
            clsOumsad COS = new clsOumsad();
            clsCall Call = new clsCall();
            clsPmpaPrint CPP = new clsPmpaPrint();
            clsPmpaFunc cPF = new clsPmpaFunc();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);
                        
            CBA.Bas_Opd_Bon();              //외래본인부담율
            CBA.Bas_Ipd_Bon();              //입원본인부담율
            CBA.Bas_Gisul();                //병원기술료가산
            CBA.Bas_Night();                //심야가산
            CBA.Bas_Night_22();             //중복가산(신생아,소아,노인등)
            CBA.Bas_Gamek();                //감액율(진찰료, 보험, 일반, 보험100%)
            CBA.Bas_PedAdd();               //만6세미만
            CBA.Bas_Bon_Tax();              //본인 부가세율
            CBA.IPD_BON_SANG();             //본인부담 상한액
            
            FnDelay = 50;            //'Timer 지연 간격
            FnCntPic = 1;            //'초기화
            FnWorkFlag = false;

            lblID.Text = "";
            lblSDate.Text = "";
            lblFDate.Text = "";
            lblSDate.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            lblMsg.Text = "인쇄를 누르면 작업이 진행 됩니다.";
            Fstr임의출력 = "";
           // Gstr누적계산New = "OK";
           // Gstr장기입원부담율시작일 = "2016-07-01";

        }

        private void SCREEN_CLEAR()
        {
            txtPano.Text = "";
            
            btnRePrint.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            FstrRout = "N";
            LabelRoutDate.Text = "";
            FnCntPic = 1;
            FnWorkFlag = true;

            Fstr임의출력 = "";

            ComFunc.ReadSysDate(clsDB.DbCon);


            FnCntWork = 1;
            AUTO_PRINT_MAIN();

            timer1.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            FnCntPic = 1;
            FnWorkFlag = false;
        }

        private void btnRePrint_Click(object sender, EventArgs e)
        {
            if(txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호공란");
             
                return; 
            }
            else if (VB.Len(txtPano.Text.Trim()) != 8 )
            {
                ComFunc.MsgBox("등록번호오류");
                return; 
            }

            timer1.Enabled = false;
            Fstr임의출력 = "Y";

            if (VB.Len(txtPano.Text.Trim()) != 8)
            {
                ComFunc.MsgBox("등록번호오류");
                return;
            }
            AUTO_PRINT_MAIN();

            timer1.Enabled = true;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = FnWorkFlag;
            if (FnWorkFlag == false) return;

            FnCntPic = FnCntPic + 1;
            if (FnCntPic > 5)
            {
                FnCntPic = 1;
                FnCntWork = FnCntWork + 1;
            }

            if (FnCntPic == 1) pictureBox1.Image = Resources.pic_0;
            else if (FnCntPic == 2) pictureBox1.Image = Resources.pic_1;
            else if (FnCntPic == 3) pictureBox1.Image = Resources.pic_2;
            else if (FnCntPic == 4) pictureBox1.Image = Resources.pic_3;
            else if (FnCntPic == 5) pictureBox1.Image = Resources.pic_4;

            Application.DoEvents();

            if (FnCntWork > FnDelay)
            {
                FnCntWork = 1;
                if (FstrRout =="Y")
                {
                    AUTO_PRINT_MAIN_RES();
                }
                else
                {
                    AUTO_PRINT_MAIN();
                }
               
              
            }

            lblMsg.Text = "작업이 대기중입니다.";
        }

        private void AUTO_PRINT_MAIN()
        {
            int i=0 , j=0 , K=0 ;
            int nTRSCNT = 0, nREAD = 0, nLine = 0, nPage = 0, nNu = 0, nGETcount = 0, nInitNo = 0;
            long nIPDNO = 0, nTRSNo = 0, nTilsu = 0, nAmt1 = 0, nAmt2 = 0, nCnt2 = 0;
            long nNuChk = 0, nStot1 = 0, nStot2 = 0, nGtot1 = 0, nGtot2 = 0, nRowInx = 0;
            long nJ0041 = 0, nJ0042 = 0, nJ0043 = 0, nTal = 0 ;
            string strPano = "", strSname = "", strInDate = "", strOutDate = "", strBi = "";
            string strDeptCode = "", strWardCode = "", strRoomCode = "", nIlsu = "", strSex = "";
            string StrAge = "", strDume = "";
            string strSimDate = "", strSimSaDate = "", strSDate = "", strEdate = "", strEOF = "";
            string[] strNujuk = new string[10];
            string[] strIllCode = new string[30];
            string[] strIllName = new string[30];
            string[] strFrBiDate = new string[10];
            string[] strFrBi = new string[10];
            string[] strToBi = new string[10];
            string[] strFrDeptDate = new string[10];
            string[] strFrDeptCode = new string[10];
            string[] strToDeptCode = new string[10];
            string[] strFrRoomDate = new string[10];
            string[] strFrRoomCode = new string[10];
            string[] strToRoomCode = new string[10];

            //  Dim LvCurr            As Variant
            string strRowId = "", strTitleF = "", strPrintData = "", strSang = "", strGbSTS = "";
            string strHead = "", strTest = "", strMCode = "", strFCode = "", strRoutDate = "", strVCode = "";
            string strGBDiv = "", strDRG = "", strDrgCode = "", strPFlag = "", strSelMsg = "";


            //int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strROWID = "";
            bool isPrint = false;

            try
            {
                ComFunc.ReadSysDate(clsDB.DbCon);

                lblFDate.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

                SQL = " SELECT IPDNO, TRSNO, ROutDate, GBSTS, VCODE, DRGCODE, FCODE ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.IPD_TRANS  ";

                if(FstrRout =="Y")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE PANO IN (  ";
                    SQL = SQL + ComNum.VBLF + "     SELECT PANO  FROM KOSMOS_PMPA.NUR_MASTER  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ROUTDATE = TO_DATE('" + TxtRDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "      ) ";
                    SQL = SQL + ComNum.VBLF + "     AND GbCheckList IS NULL ";
                    SQL = SQL + ComNum.VBLF + "     AND ACTDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "     AND AMSET4 <> '3'";
                    SQL = SQL + ComNum.VBLF + "     AND GBIPD <>  '9' ";

                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE ROUTDATE > TRUNC(SYSDATE -4) ";
                    if (strTest =="")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + "     AND AMSET4 <> '3'";
                        SQL = SQL + ComNum.VBLF + "     AND GBIPD IN ('1') " ;
                        if(Fstr임의출력 != "Y")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND GbCheckList IS NULL  ";
                            SQL = SQL + ComNum.VBLF + "     AND GbSTS IN ('1', '2', '4')";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "     AND GbCheckList IS NOT NULL   ";
                            SQL = SQL + ComNum.VBLF + "     AND GbSTS IN ('1', '2','3', '4','5','6')";
                            SQL = SQL + ComNum.VBLF + "     AND Pano = '" + txtPano.Text.Trim() + "'";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND TRSNO = '" + strTest + "'";
                    }
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY RoutDate";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    //Convert.ToInt32(VB.Val(dt.Rows[0]["Cnt"].ToString()
                    nIPDNO = Convert.ToInt64(dt.Rows[0]["IPDNO"].ToString()); 
                    nTRSNo = Convert.ToInt64(dt.Rows[0]["TRSNO"].ToString());
                    strGbSTS = dt.Rows[0]["GBSTS"].ToString().Trim(); 
                    strVCode = dt.Rows[0]["VCode"].ToString().Trim(); 
                    if (dt.Rows[0]["DRGCODE"].ToString().Trim() !=""  )
                    {
                        strDRG = "◀DRG▶";
                        strDrgCode = dt.Rows[0]["DRGcode"].ToString().Trim(); 
                    }

                }
                dt.Dispose();
                dt = null;


                if (dt1.Rows.Count == 0)
                {
                    isPrint = false;
                    ss1_Sheet1.Cells[2, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ss1_Sheet1.Cells[3, 1].Text = dt.Rows[0]["SEX"].ToString().Trim();
                    ss1_Sheet1.Cells[3, 3].Text = dt.Rows[0]["AGE"].ToString().Trim();
                    ss1_Sheet1.Cells[4, 1].Text = VB.Space(5) + dt.Rows[0]["PANO"].ToString().Trim();
                    ss1_Sheet1.Cells[5, 1].Text = VB.Space(5) +
                                                ComFunc.FormatStrToDate(dt.Rows[0]["BDATE"].ToString().Trim().Replace("-", ""), "DK") + " " +
                                                ComFunc.FormatStrToDate(dt.Rows[0]["WRTTIME"].ToString().Trim(), "MK");
                }
                else
                {
                    isPrint = true;
                }
                dt.Dispose();
                dt = null;
                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                dt1.Dispose();
                dt1 = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            if (isPrint == false)
            {
                ss1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;   //세로
                ss1_Sheet1.PrintInfo.Header = "";
                ss1_Sheet1.PrintInfo.Margin.Left = 0;
                ss1_Sheet1.PrintInfo.Margin.Right = 0;
                ss1_Sheet1.PrintInfo.Margin.Top = 0;
                ss1_Sheet1.PrintInfo.Margin.Bottom = 0;
                ss1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ss1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ss1_Sheet1.PrintInfo.ShowBorder = false;
                ss1_Sheet1.PrintInfo.ShowColor = false;
                ss1_Sheet1.PrintInfo.ShowGrid = true;
                ss1_Sheet1.PrintInfo.ShowShadows = false;
                ss1_Sheet1.PrintInfo.UseMax = false;
                ss1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                ss1.PrintSheet(0);
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " UPDATE OPD_WORK SET ERNAME ='Y' WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }


        private void AUTO_PRINT_MAIN_RES()
        {
            //int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strROWID = "";
            bool isPrint = false;

            try
            {
                ComFunc.ReadSysDate(clsDB.DbCon);

                lblFDate.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

                SQL = " SELECT A.PANO , A.SNAME, A.AGE, A.ROWID, B.SEX, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE , A.WRTTIME ";
                SQL = SQL + ComNum.VBLF + "   FROM OPD_WORK A, BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.BDATE >= TRUNC(SYSDATE-1)";
                SQL = SQL + ComNum.VBLF + "     AND DELMARK <>'*' ";
                //2020-02-26 진단서 입사증도 이름표 인쇄 되도록 요청함
                //SQL = SQL + ComNum.VBLF + "     AND JIN NOT IN ('4','7') ";
                SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE ='ER'";
                SQL = SQL + ComNum.VBLF + "     AND A.ERNAME IS NULL ";
                SQL = SQL + ComNum.VBLF + "     AND A.PANO = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "     AND (A.GBFLU IS NULL OR A.GBFLU <>'Y' ) ";
                SQL = SQL + ComNum.VBLF + "     AND A.Pano <> '81000004' "; //'전산실연습제외
                SQL = SQL + ComNum.VBLF + "     AND ROWNUM =1 ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.BDATE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                //'당일같은 인쇄건 있으면 2009-12-17
                SQL = " SELECT PANO FROM OPD_WORK ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DELMARK <>'*'  ";
                //2020-02-26 진단서 입사증도 이름표 인쇄되도록 보완
                //SQL = SQL + ComNum.VBLF + "  AND JIN NOT IN ('4','7') ";
                SQL = SQL + ComNum.VBLF + "  AND DEPTCODE ='ER' ";
                SQL = SQL + ComNum.VBLF + "  AND ERNAME='Y'  ";
                SQL = SQL + ComNum.VBLF + "  AND (GBFLU IS NULL OR GBFLU <>'Y' )  ";
                SQL = SQL + ComNum.VBLF + "  AND BDATE =TO_DATE('" + dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND Pano <> '81000004' "; //'전산실연습제외

                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt1.Rows.Count == 0)
                {
                    isPrint = false;
                    ss1_Sheet1.Cells[2, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ss1_Sheet1.Cells[3, 1].Text = dt.Rows[0]["SEX"].ToString().Trim();
                    ss1_Sheet1.Cells[3, 3].Text = dt.Rows[0]["AGE"].ToString().Trim();
                    ss1_Sheet1.Cells[4, 1].Text = VB.Space(5) + dt.Rows[0]["PANO"].ToString().Trim();
                    ss1_Sheet1.Cells[5, 1].Text = VB.Space(5) +
                                                ComFunc.FormatStrToDate(dt.Rows[0]["BDATE"].ToString().Trim().Replace("-", ""), "DK") + " " +
                                                ComFunc.FormatStrToDate(dt.Rows[0]["WRTTIME"].ToString().Trim(), "MK");
                }
                else
                {
                    isPrint = true;
                }
                dt.Dispose();
                dt = null;
                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                dt1.Dispose();
                dt1 = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            if (isPrint == false)
            {
                ss1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;   //세로
                ss1_Sheet1.PrintInfo.Header = "";
                ss1_Sheet1.PrintInfo.Margin.Left = 0;
                ss1_Sheet1.PrintInfo.Margin.Right = 0;
                ss1_Sheet1.PrintInfo.Margin.Top = 0;
                ss1_Sheet1.PrintInfo.Margin.Bottom = 0;
                ss1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ss1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ss1_Sheet1.PrintInfo.ShowBorder = false;
                ss1_Sheet1.PrintInfo.ShowColor = false;
                ss1_Sheet1.PrintInfo.ShowGrid = true;
                ss1_Sheet1.PrintInfo.ShowShadows = false;
                ss1_Sheet1.PrintInfo.UseMax = false;
                ss1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                ss1.PrintSheet(0);
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " UPDATE OPD_WORK SET ERNAME ='Y' WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }
        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void frmAutoPrint_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmAutoPrint_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void frmAutoPrint_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;   // 창을 보이지 않게 한다.
                this.ShowIcon = false;  // 작업표시줄에서 제거
                notifyIcon1.Visible = true;
            }
        }
        string Read_INOUT(PsmhDb pDbCon, string ArgPtno, string ArgInDate, string ArgOutDate)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strROWID = "";
            string returnVal = "";
            

            SQL = " SELECT TO_CHAR(MIN(OUTDATE),'YYYY-MM-DD') OutDate, ";
            SQL = SQL + ComNum.VBLF + "   TO_CHAR(MAX(INDATE),'YYYY-MM-DD') InDate, COUNT(*) Cnt ";
            SQL = SQL + ComNum.VBLF + "   FROM  KOSMOS_PMPA.GUN_OPDINOUT  ";
            SQL = SQL + ComNum.VBLF + "  WHERE  Pano = '" + ArgPtno + "' ";
            SQL = SQL + ComNum.VBLF + "     AND  OUTDATE >= to_date('" + ArgInDate + "','YYYY-MM-DD') ) ";
            SQL = SQL + ComNum.VBLF + "     AND  INDATE <=  to_date('" + ArgOutDate + "','YYYY-MM-DD') ) ";


            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return returnVal;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return returnVal;
            }
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(VB.Val(dt.Rows[0]["Cnt"].ToString())) > 0)
                {
                    returnVal = dt.Rows[0]["OUTDATE"].ToString() + "/";
                    returnVal = returnVal + dt.Rows[0]["OUTDATE"].ToString() + "/";
                    returnVal = returnVal + dt.Rows[0]["Cnt"].ToString() ;
                }
            }

            return returnVal;
        }
        string Read_Simsa_Sabun(PsmhDb pDbCon, string ArgPtno)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int x = 0;
            string rtnVal = "";
            string strOK = "";
            string strOK2 = "";
            string strOK3 = "";
            string strOK4 = "";
            string[] strBi = new string[4];
            string[] strWard = new string[4];
            string[] strRoom = new string[4];
            string[] strDept = new string[4];
            DataTable DtSim = new DataTable();

            for (int i = 0; i < 12; i++)
            {
                strOK = "OK";

                for (int j = 0; j < 4; j++)
                {
                    strBi[j] = "";
                    strWard[j] = "";
                    strRoom[j] = "";
                    strDept[j] = "";
                }

                if (Convert.ToInt32(FstrJSimSabun[i]) > 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SABUN,GUBUN,REMARK, ";
                    SQL += ComNum.VBLF + "        ENTDATE,Seqno ";
                    SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "ETC_JSIM_SET ";
                    SQL += ComNum.VBLF + "  WHERE Sabun = " + Convert.ToInt32(FstrJSimSabun[i]) + " ";
                    SQL += ComNum.VBLF + "  ORDER By Seqno,Gubun ";
                    SqlErr = clsDB.GetDataTable(ref DtSim, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtSim.Dispose();
                        DtSim = null;
                        return rtnVal;
                    }

                    if (DtSim.Rows.Count > 0)
                    {
                        for (int j = 0; j < DtSim.Rows.Count; j++)
                        {
                            x = Convert.ToInt32(DtSim.Rows[j]["SEQNO"].ToString());

                            switch (DtSim.Rows[j]["GUBUN"].ToString().Trim())
                            {
                                case "1":
                                    strBi[x] = strBi[x] + "'" + DtSim.Rows[j]["REMARK"].ToString().Trim() + "',";
                                    break;

                                case "2":
                                    if (VB.I(strWard[x], VB.Pstr(DtSim.Rows[j]["REMARK"].ToString().Trim(), "/", 1)) < 2)
                                        strWard[x] = strWard[x] + "'" + VB.Pstr(DtSim.Rows[j]["REMARK"].ToString().Trim(), "/", 1) + "',";

                                    strRoom[x] = strRoom[x] + VB.Pstr(DtSim.Rows[j]["REMARK"].ToString().Trim(), "/", 2) + ",";
                                    break;

                                case "3":
                                    strDept[x] = strDept[x] + "'" + DtSim.Rows[j]["REMARK"].ToString().Trim() + "',";
                                    break;
                            }
                        }
                    }
                    else
                        strOK = "";

                    DtSim.Dispose();
                    DtSim = null;

                    if (strOK == "OK")
                    {
                        for (x = 0; x < 4; x++)
                        {
                            if (strBi[x] != "") { strBi[x] = VB.Right(strBi[x], 1) == "," ? strBi[x].Substring(0, strBi[x].Length - 1) : strBi[x]; }
                            if (strWard[x] != "") { strWard[x] = VB.Right(strWard[x], 1) == "," ? strWard[x].Substring(0, strWard[x].Length - 1) : strWard[x]; }
                            if (strRoom[x] != "") { strRoom[x] = VB.Right(strRoom[x], 1) == "," ? strRoom[x].Substring(0, strRoom[x].Length - 1) : strRoom[x]; }
                            if (strDept[x] != "") { strDept[x] = VB.Right(strDept[x], 1) == "," ? strDept[x].Substring(0, strDept[x].Length - 1) : strDept[x]; }
                        }

                        strOK2 = "OK";
                        if (strBi[1] == "" && strWard[1] == "" && strRoom[1] == "" && strDept[1] == "") { strOK2 = ""; }
                        strOK3 = "OK";
                        if (strBi[2] == "" && strWard[2] == "" && strRoom[2] == "" && strDept[2] == "") { strOK3 = ""; }
                        strOK4 = "OK";
                        if (strBi[3] == "" && strWard[3] == "" && strRoom[3] == "" && strDept[3] == "") { strOK4 = ""; }

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT /*+ INDEX_DESC(kosmos_ocs.ipd_trans INDEX_IPDTRS0) */ ";
                        SQL += ComNum.VBLF + "        A.IPDNO, A.TRSNO, A.GBSTS, ";
                        SQL += ComNum.VBLF + "        A.Pano, A.GBIPD, B.SName, ";
                        SQL += ComNum.VBLF + "        B.WARDCODE, B.RoomCode, B.ILLCODE1, ";
                        SQL += ComNum.VBLF + "        B.ILLCODE2 , B.ILLCODE3, B.ILLCODE4 , ";
                        SQL += ComNum.VBLF + "        A.DeptCode, A.DrCode,  A.ILSU, ";
                        SQL += ComNum.VBLF + "        A.VCODE, A.OGPDBUN, A.OGPDBUNDTL,";
                        SQL += ComNum.VBLF + "        A.Bi, ";
                        SQL += ComNum.VBLF + "        TO_CHAR(A.InDate,'YYYY-MM-DD') InDate,  ";
                        SQL += ComNum.VBLF + "        TO_CHAR(A.JSIM_LDATE ,'YYYY-MM-DD') JSIM_LDATE , ";
                        SQL += ComNum.VBLF + "        JSIM_SABUN ,JSIM_SET, B.JSIM_REMARK, ";
                        SQL += ComNum.VBLF + "        a.Ilsu , b.ilsu ilsu2, ";
                        SQL += ComNum.VBLF + "        a.DrgCode, a.GbDrg  ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS A, ";
                        SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_MASTER B  ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND A.ACTDATE IS NULL ";
                        SQL += ComNum.VBLF + "    AND A.INDATE  < TRUNC(SYSDATE)  ";
                        SQL += ComNum.VBLF + "    AND A.GBIPD  NOT IN ('D') ";
                        SQL += ComNum.VBLF + "    AND ((A.JSIM_SET IS NULL ";

                        if (strBi[0] != "")
                            SQL += ComNum.VBLF + "AND A.BI IN (" + strBi[0] + ") ";
                        if (strDept[0] != "")
                            SQL += ComNum.VBLF + "AND A.DEPTCODE IN (" + strDept[0] + ") ";
                        if (strWard[0] != "")
                            SQL += ComNum.VBLF + "AND B.WARDCODE IN (" + strWard[0] + ") ";
                        if (strRoom[0] != "")
                            SQL += ComNum.VBLF + "AND B.ROOMCODE IN (" + strRoom[0] + ") ";

                        if (strOK2 != "")
                        {
                            SQL += ComNum.VBLF + "            ) OR ( A.JSIM_SET IS NULL ";

                            if (strBi[1] != "")
                                SQL += ComNum.VBLF + "AND A.BI IN (" + strBi[1] + ") ";
                            if (strDept[1] != "")
                                SQL += ComNum.VBLF + "AND A.DEPTCODE IN (" + strDept[1] + ") ";
                            if (strWard[1] != "")
                                SQL += ComNum.VBLF + "AND B.WARDCODE IN (" + strWard[1] + ") ";
                            if (strRoom[1] != "")
                                SQL += ComNum.VBLF + "AND B.ROOMCODE IN (" + strRoom[1] + ") ";
                        }

                        if (strOK3 != "")
                        {
                            SQL += ComNum.VBLF + "            ) OR ( A.JSIM_SET IS NULL ";

                            if (strBi[2] != "")
                                SQL += ComNum.VBLF + "AND A.BI IN (" + strBi[2] + ") ";
                            if (strDept[2] != "")
                                SQL += ComNum.VBLF + "AND A.DEPTCODE IN (" + strDept[2] + ") ";
                            if (strWard[2] != "")
                                SQL += ComNum.VBLF + "AND B.WARDCODE IN (" + strWard[2] + ") ";
                            if (strRoom[2] != "")
                                SQL += ComNum.VBLF + "AND B.ROOMCODE IN (" + strRoom[2] + ") ";
                        }


                        if (strOK4 != "")
                        {
                            SQL += ComNum.VBLF + "            ) OR ( A.JSIM_SET IS NULL ";

                            if (strBi[3] != "")
                                SQL += ComNum.VBLF + "AND A.BI IN (" + strBi[3] + ") ";
                            if (strDept[3] != "")
                                SQL += ComNum.VBLF + "AND A.DEPTCODE IN (" + strDept[3] + ") ";
                            if (strWard[3] != "")
                                SQL += ComNum.VBLF + "AND B.WARDCODE IN (" + strWard[3] + ") ";
                            if (strRoom[3] != "")
                                SQL += ComNum.VBLF + "AND B.ROOMCODE IN (" + strRoom[3] + ") ";
                        }

                        SQL += ComNum.VBLF + "           )  OR A.JSIM_SET = '" + FstrJSimSabun[i] + "' ) ";
                        SQL += ComNum.VBLF + "       AND A.IPDNO    = B.IPDNO ";
                        SQL += ComNum.VBLF + "       AND A.PANO     = '" + ArgPtno + "' ";
                        SQL += ComNum.VBLF + "     ORDER BY B.ROOMCODE ";
                        SqlErr = clsDB.GetDataTable(ref DtSim, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            DtSim.Dispose();
                            DtSim = null;
                            return rtnVal;
                        }

                        if (DtSim.Rows.Count > 0)
                        {
                            rtnVal = FstrJSimSabun[i];
                            DtSim.Dispose();
                            DtSim = null;
                            return rtnVal;
                        }

                        DtSim.Dispose();
                        DtSim = null;
                    }
                }
            }

            return rtnVal;
        }
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void CmdRout_Click(object sender, EventArgs e)
        {
            FstrRout = "Y";
            LabelRoutDate.Text = "퇴원예고자 인쇄중";
            FnCntPic = 1;
            FnWorkFlag = true;


            Fstr임의출력 = "";


            ComFunc.ReadSysDate(clsDB.DbCon);


            FnCntWork = 1;
            AUTO_PRINT_MAIN_RES();


            timer1.Enabled = true;
        }
    }
}
