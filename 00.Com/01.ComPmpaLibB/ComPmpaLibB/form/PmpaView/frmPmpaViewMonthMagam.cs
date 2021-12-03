using ComBase;
using ComDbB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibe
    /// File Name       : frmPmpaViewMonthMagam.cs
    /// Description     : 발생주의 월말 마감작업
    /// Author          : 김해수
    /// Create Date     : 2019-03-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "\misu\misubs\FrmMonthMagam.frm(misubs21.frm) >> frmPmpaViewMonthMagam.cs 폼이름 재정의" />
    public partial class frmPmpaViewMonthMagam : Form
    {
        #region 클래스 선언 및 etc....

        PsmhDb dbCon = new PsmhDb();
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();
        clsComPmpaSQL cSQL = new clsComPmpaSQL();
        clsPmpaFunc cPF = new clsPmpaFunc();

        string FstrYYMM = "";     //작업년월
        string FstrFDate = "";    //시작일자
        string FstrTDate = "";    //종료일자
        string FstrCOMMIT = "";   //오류발생여부(OK.오류없음,NO.오류발생)

        DataTable dt = null;
        DataTable rsFun = null;
        DataTable rsFun1 = null;
        DataTable rs1 = null;
        string SQL = "";    //Query문
        string SqlErr = ""; //에러문 받는 변수
        int intRowAffected = 0;
        #endregion

        #region //MainFormMessage
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

        public frmPmpaViewMonthMagam()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewMonthMagam(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        void setCtrlData()
        {
            setCombo();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);


            this.btnStart.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);

        }

        void setCombo()
        {
            CF.ComboMonth_Set(cboYYMM, 6);
            cboYYMM.SelectedIndex = 1;

            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }  
        }

        void setTxtTip()
        {

        }

        void setCtrlInit()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //else
            {

                //툴팁
                setTxtTip();

                screen_clear();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();

            }
        }

        void eFormResize(object sender, EventArgs e)
        {

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
            else if (sender == this.btnStart)
            {
                GetData();
            }
        }

        void screen_clear(string Job = "")
        {
            read_sysdate();

            if (Job == "")
            {
                //dtpDate.Text = cpublic.strSysDate;
                lblMsg.Text = " ";
            }
        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void GetData()
        {
            FstrYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            FstrFDate = VB.Left(FstrYYMM, 4) + "-" + VB.Right(FstrYYMM, 2) + "-01";
            FstrTDate = CF.READ_LASTDAY(clsDB.DbCon, FstrFDate);

            FstrCOMMIT = "OK";

            Cursor.Current = Cursors.WaitCursor;

            //월말현재 재원미수금 상세내역
            if(chkAuto1.Checked == true)
            {
                Month_JewonMisu_Build();
            }

            //월말현재 외래예약 예수금 내역 형성
            if(FstrYYMM.CompareTo("200308") >= 0 && chkAuto2.Checked == true)
            {
                //변경된 예약 선수금 명단 LIST 자료 형성루틴 입니다.
                Month_Yeyak_Sunsu_Build();

                Month_Exam_Yeyak_Sunsu_Build();
            }

            Cursor.Current = Cursors.Default;

            if(FstrCOMMIT == "OK")
            {
                ComFunc.MsgBox("통계형성 완료");
            }
        }

        void Month_JewonMisu_Build()
        {
            int i = 0;
            int j = 0;
            int nRead = 0;
            int nBonRate = 0;
            double nBonRate2 = 0.0;
            int nRateGasan = 0;

            string strPano = "";     
            string strBi = "";
            string strSuBi = "";
            string strInDate = "";
            string strROWID = "";
            
            string strActdate = "";
            string strReBuild = "";

            long nGubAmt = 0;
            long nBigubAmt = 0;
            long nCTAmt1 = 0;
            long nCTAmt2 = 0;

            long nBoninAmt = 0;     
            long nIpgumAmt = 0;
            long nIpgumAmt1 = 0;
            long nBoninMisu = 0;
            long nJohapAmt = 0;
            long nJungAmt = 0;
            long nJohapMisu = 0;

            progressBarX1.Minimum = 1;

            if (FstrCOMMIT != "OK") { return; }

            lblMsg.Text = "재원미수금 상세내역 형성중";

            #region 해당월의 최종 회계일자를 구함
            FstrYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            FstrFDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2) + "-01";
            FstrTDate = CF.READ_LASTDAY(clsDB.DbCon, FstrFDate);
            strActdate = FstrTDate;

            if(FstrTDate.CompareTo(cpublic.strSysDate) >= 0)
            {
                ComFunc.MsgBox("당월의 작업은 아직 불가능 합니다.");
                return;
            }

            lblMsg.Text = "재원미수금 상세내역 자료가 있는지 점검";


            //해당월 자료가 존재여부 CHECK
            SQL = "";
            SQL = SQL + "SELECT COUNT(*) CNT FROM MISU_BALJEWON" + ComNum.VBLF ;
            SQL = SQL + "WHERE YYMM='" + FstrYYMM + "' "+ ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            strReBuild = "";

            if(dt.Rows.Count > 0)
            {
                if (Convert.ToInt64(dt.Rows[0]["CNT"]) > 0) { strReBuild = "OK"; }
            }

            dt.Dispose();
            dt = null;
            #endregion

            clsDB.setBeginTran(clsDB.DbCon);

            Misu_BalJewon_Build(FstrYYMM, strActdate, ref intRowAffected);

            #region CmdBuild_MisuAmt_Gesan
            string strInYYMM = "";
            string nTRSNO = "";

            lblMsg.Text = "재원미수금 : 조합, 본인 미수금 계산";

            //자료를 읽어 조합부담, 본인부담액을 계산함
            SQL = "SELECT Pano,Bi,TO_CHAR(InDate,'YYYY-MM-DD') InDate,IpgumAmt,JungAmt,";
            SQL = SQL + "TRSNO,IPDNO,ROWID " + ComNum.VBLF;
            SQL = SQL + "FROM MISU_BALJEWON " + ComNum.VBLF;
            SQL = SQL + "WHERE YYMM='" + FstrYYMM + "' " + ComNum.VBLF;
            SQL = SQL + "ORDER BY Bi,Pano,InDate" + ComNum.VBLF;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            nRead = dt.Rows.Count;

            if (nRead > 0)
            {
                progressBarX1.Minimum = 1;
                progressBarX1.Maximum = nRead;
                progressBarX1.Value = progressBarX1.Minimum;
            }

            for (i = 0; i < nRead; i++)
            {
                progressBarX1.Value = VB.Fix(((i + 1) / nRead) * 100);
                progressBarX1.Text = Math.Truncate(Convert.ToDouble(i + 1) / dt.Rows.Count * 100) + "%";
                Application.DoEvents();

                strPano = dt.Rows[i]["Pano"].ToString().Trim();
                nTRSNO = dt.Rows[i]["TRSNO"].ToString().Trim();

                if(strPano == "05957758")
                {
                    i = i;

                }

                strBi = dt.Rows[i]["Bi"].ToString().Trim();
                strSuBi = VB.Format(READ_Bi_SuipTong(strBi, VB.Left(FstrYYMM, 4) + "-" + VB.Right(FstrYYMM, 2) + "-01"),"0");
                strInDate = dt.Rows[i]["InDate"].ToString().Trim();
                strInYYMM = VB.Left(strInDate, 4) + VB.Mid(strInDate, 6, 2);
                nIpgumAmt = Convert.ToInt64(dt.Rows[i]["IpgumAmt"]);
                nJungAmt = Convert.ToInt64(dt.Rows[i]["JungAmt"]);
                strROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                nGubAmt = 0;     nBigubAmt = 0;
                nCTAmt1 = 0;     nCTAmt2 = 0;
                nBoninAmt = 0;   nJohapAmt = 0;
                nBoninMisu = 0;  nJohapMisu = 0;

                //IPD_NEW_SLIP에서 급여, 비급여 금액을 읽음
                SQL = "SELECT Nu,SUM(Amt1+Amt2) Amt ";
                SQL = SQL + "FROM IPD_NEW_SLIP " + ComNum.VBLF;
                SQL = SQL + "WHERE TRSNO=" + nTRSNO + " " + ComNum.VBLF;
                SQL = SQL + "AND ActDate<=TO_DATE('" + FstrTDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL = SQL + "AND Bi='" + strBi + "' " + ComNum.VBLF;
                SQL = SQL + "GROUP BY Nu " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTable(ref rs1, SQL, clsDB.DbCon);

                for (j = 0; j < rs1.Rows.Count; j++)
                {
                    if (rs1.Rows[j]["Nu"].ToString().Trim().CompareTo("20") <= 0)
                    {
                        nGubAmt = nGubAmt + Convert.ToInt64(rs1.Rows[j]["Amt"]);
                    }
                    else
                    {
                        nBigubAmt = nBigubAmt + Convert.ToInt64(rs1.Rows[j]["Amt"]);
                    }

                    //해당 입원기간동안 CT 급여액을 READ
                    if (rs1.Rows[j]["Nu"].ToString().Trim().CompareTo("19") == 0)
                    {
                        if(strBi.CompareTo("11") >= 0 && strBi.CompareTo("19") <=0 )
                        {
                            if(strInDate.CompareTo("2001-07-01") <= 0)
                            {
                                nCTAmt1 = Convert.ToInt64(rs1.Rows[j]["Amt"]);
                            }
                            else
                            {
                                nCTAmt2 = Convert.ToInt64(rs1.Rows[j]["Amt"]);
                            }
                        }
                    }
                }

                rs1.Dispose();
                rs1 = null;

                //본인부담율, 병원가산율을 READ
                nBonRate = Convert.ToInt32(cPF.READ_BonRate(clsDB.DbCon, "IPD", strBi, strActdate));
                nRateGasan = cPF.READ_RateGasan(clsDB.DbCon, strBi, strActdate);

                //본인부담, 조합부담액을 계산
                if(nBonRate == 0 || nGubAmt == 0)
                {
                    nBoninAmt = nBigubAmt;
                }else
                {
                    nBonRate2 = Convert.ToDouble(String.Format("{0:N1}", ((double)nBonRate / (double)100))); // Convert.ToSingle(nBonRate / 100 ); //int함수계산에서 오버플로어가 발생하여 루틴 추가(예: 2000000 * 100/100) 순차계산됩니다. 2000000 * 100값에 의해 발생합니다.
                    //  nBoninAmt = nBigubAmt + Convert.ToInt64((nGubAmt - nCTAmt1 - nCTAmt2) * nBonRate) /100;
                    nBoninAmt = nBigubAmt + (long)Math.Truncate((nGubAmt - nCTAmt1 - nCTAmt2) * nBonRate2);
                    nBoninAmt = nBoninAmt + (long)Math.Truncate(nCTAmt1 * 0.55);// 2001.7.1 이전(long)Math.Round
                    nBoninAmt = nBoninAmt + (long)Math.Truncate(nCTAmt2 * 0.45);// 2001.7.1 이후
                }

                nBoninMisu = nBoninAmt - nIpgumAmt;
                nJohapAmt = nGubAmt + nBigubAmt - nBoninAmt;
                nJohapMisu = nJohapAmt - nJungAmt;

                //자료를 UPDATE 
                SQL = "UPDATE MISU_BALJEWON SET SuBi='" + strSuBi + "',";
                SQL = SQL + "InDate=TO_DATE('" + strInDate + "','YYYY-MM-DD')," + ComNum.VBLF;
                SQL = SQL + "BonRate=" + nBonRate + ",RateGasan=" + nRateGasan + "," + ComNum.VBLF;
                SQL = SQL + "GubCtAmt1=" + nCTAmt1 + ",GubCtAmt2=" + nCTAmt2 + "," + ComNum.VBLF;
                SQL = SQL + "GubTot=" + nGubAmt + ",BigubTot=" + nBigubAmt + "," + ComNum.VBLF;
                SQL = SQL + "JohapAmt=" + nJohapAmt + ",BoninAmt=" + nBoninAmt + "," + ComNum.VBLF;
                SQL = SQL + "BoninMisu=" + nBoninMisu + ",JohapMisu=" + nJohapMisu + "," + ComNum.VBLF;
                SQL = SQL + "TotAmt=" +( nGubAmt + nBigubAmt ) + " " + ComNum.VBLF;
                SQL = SQL + "WHERE ROWID='" + strROWID + "' " + ComNum.VBLF;

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if(SqlErr != "")
                {
                    FstrCOMMIT = "NO";
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("월말 재원미수내역에 UPDATE시 오류발생", "ROLLBACK");
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            #endregion

            clsDB.setCommitTran(clsDB.DbCon);

        }

        void Misu_BalJewon_Build(string ArgYYMM, string ArgDate, ref int intRowAffected)
        {
            //-----------------------------------------------------------
            //   1. 지정한 일자의 재원자 명단을 구함
            //   2. 재원자의 IPD_TRANS별도 IPD_NEW_SLIP의 금액을 합산함
            //   3. 총진료비,조합부담,본인부담,중간납 금액을 계산함
            //-----------------------------------------------------------
            int i = 0;
            int j = 0;
            int nRead = 0;
            int nIPDNO = 0;
            int nTRSNO = 0;
            int strOK = 0;
            int strActdate = 0;
            int nOldIPDNO = 0;
            int nBonRate = 0;
            long[] nAmt = new long[62];
            double nGub = 0;
            long nBigub = 0;
            double nBonin = 0;
            string strROWID = "";
            string strPano = "";
            string strInDate = "";
            string strBi = "";
            int nSuBi = 0;
            int nIlsu = 0;

            int ii = 0;
            int nRead2 = 0;
            double nMirAmt = 0;
            string strBi2 = "";
            string nSuBi2 = "";
            string strROWID2 = "";
            string strOldBi = "";
            string strNewBi = "";
            long nSlipAmt = 0;
            int NU = 0;


            long nDrugAmt = 0;
            long nBuildDrugAmt = 0;

            lblMsg.Text = "재원미수금 : 월말 현재 재원자 명단을 작성";

            SQL = "";
            SQL = SQL + "DELETE MISU_BALJEWON " + ComNum.VBLF;
            SQL = SQL + "WHERE YYMM='" + ArgYYMM + "' " + ComNum.VBLF;
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("기존의 자료를 삭제중 오류가 발생함", "ROLLBACK");
                return;
            }

            SQL = "INSERT INTO MISU_BALJEWON (YYMM,IPDNO,TRSNO,Pano,SName,Bi,InDate,";
            SQL = SQL + "WardCode,RoomCode,DeptCode,DrCode,Bohun,AmSet1,Amset4,AmSet6,BonRate," + ComNum.VBLF;
            SQL = SQL + "RateGasan,TotAmt,JungAmt,IpgumAmt, DrugAmt ) " + ComNum.VBLF;
            SQL = SQL + "SELECT '" + FstrYYMM + "',a.IPDNO,a.TRSNO,a.Pano,b.SName,a.Bi," + ComNum.VBLF;
            SQL = SQL + "TRUNC(a.InDate),b.WardCode,b.RoomCode,a.DeptCode,a.DrCode,a.Bohun," + ComNum.VBLF;
            SQL = SQL + "a.AmSet1,a.AmSet4,b.AmSet6,a.BonRate,a.GisulRate,0,0,0 , Amt64 " + ComNum.VBLF;
            SQL = SQL + "FROM IPD_TRANS a,IPD_NEW_MASTER b " + ComNum.VBLF;
            SQL = SQL + "WHERE (a.ActDate IS NULL OR a.ActDate>TO_DATE('" + ArgDate + "','YYYY-MM-DD')) " + ComNum.VBLF;
            SQL = SQL + "AND a.IPDNO=b.IPDNO(+) " + ComNum.VBLF;
            SQL = SQL + "AND b.INDATE<=TO_DATE('" + ArgDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            //-------------------------------------------------------------------------
            //  작업일의 재원자 명단 및 총진료비,조합부담,본인부담,중간납 금액을 계산
            //-------------------------------------------------------------------------

            SQL = "SELECT IPDNO,TRSNO,Pano,Bi,BonRate,ROWID,";
            SQL = SQL + "TO_CHAR(InDate,'YYYY-MM-DD') InDate " + ComNum.VBLF;
            SQL = SQL + "FROM MISU_BALJEWON" + ComNum.VBLF;
            SQL = SQL + "WHERE YYMM='" + ArgYYMM + "'" + ComNum.VBLF;
            SQL = SQL + "ORDER BY IPDNO, TRSNO, InDate" + ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref rsFun, SQL, clsDB.DbCon);

            nRead = rsFun.Rows.Count;
            nOldIPDNO = 0;

            if (nRead > 0)
            {
                progressBarX1.Minimum = 1;
                progressBarX1.Maximum = nRead;
                progressBarX1.Value = progressBarX1.Minimum;
            }

            for (i = 0; i < nRead; i++)
            {
                progressBarX1.Value = VB.Fix(((i + 1) / nRead) * 100);
                progressBarX1.Text = Math.Truncate(Convert.ToDouble(i + 1) / rsFun.Rows.Count * 100) + "%";
                Application.DoEvents();

                nIPDNO = Convert.ToInt32(rsFun.Rows[i]["IPDNO"]);
                nTRSNO = Convert.ToInt32(rsFun.Rows[i]["TRSNO"]);
                nBonRate = Convert.ToInt32(rsFun.Rows[i]["BonRate"]);
                strPano = rsFun.Rows[i]["Pano"].ToString().Trim();
                strBi = rsFun.Rows[i]["Bi"].ToString().Trim();
                strInDate = rsFun.Rows[i]["InDate"].ToString().Trim();
                strROWID = rsFun.Rows[i]["ROWID"].ToString().Trim();

                if (nTRSNO == 1422762)
                {
                    nTRSNO = nTRSNO;
                }

                //수입통계용 분류코드 설정
                nSuBi = READ_Bi_SuipTong(strBi, ArgDate);
                //기준일 현재 재원일수를 계산
                if(strInDate.CompareTo(ArgDate) > 0)
                {
                    nIlsu = 0;
                }else
                {
                    nIlsu = CF.DATE_ILSU(clsDB.DbCon, ArgDate, strInDate) + 1;
                }

                //IPD_NEW_SLIP의 금액을 누적
                #region Slip_Amt_Update
                for (j = 0; j < 62; j++)
                {
                    nAmt[j] = 0;
                }
                nGub = 0;
                nBigub = 0;
                nDrugAmt = 0;

                //작업일까지 SLIP의 금액을 누적함
                SQL = "SELECT Bi,Nu,SUM(Amt1+Amt2) Amt, SUM(DECODE(RTRIM(SUNEXT) , 'BBBBBB', AMT1 + AMT2, 0)) AMT3 ";
                SQL = SQL + "FROM IPD_NEW_SLIP " + ComNum.VBLF;
                SQL = SQL + "WHERE TRSNO=" + nTRSNO + " " + ComNum.VBLF;
                SQL = SQL + "AND ActDate<=TO_DATE('" + ArgDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL = SQL + "GROUP BY Bi,Nu " + ComNum.VBLF;
                SqlErr = clsDB.GetDataTable(ref rsFun1, SQL, clsDB.DbCon);

                nRead2 = rsFun1.Rows.Count;
                strOldBi = "";

                for(ii =0; ii < nRead2; ii++)
                {
                    if (nIPDNO == 1112840)
                    {
                        i = i;


                    }


                    strNewBi = rsFun1.Rows[ii]["Bi"].ToString().Trim();
                    if(strOldBi == "") { strOldBi = strNewBi; }
                    if(strOldBi != strNewBi)
                    {
                        #region Slip_Amt_Update_SUB
                        nSuBi = READ_Bi_SuipTong(strOldBi, ArgDate);

                        SQL = "SELECT ROWID FROM MISU_BALJEWON ";
                        SQL = SQL + "WHERE YYMM='" + ArgYYMM + "' " + ComNum.VBLF;
                        SQL = SQL + "AND TRSNO=" + nTRSNO + " " + ComNum.VBLF;
                        SQL = SQL + "AND Bi='" + strOldBi + "' " + ComNum.VBLF;
                        SqlErr = clsDB.GetDataTable(ref rs1, SQL, clsDB.DbCon);

                        strROWID2 = "";
                        if(rs1.Rows.Count > 0) { strROWID2 = rs1.Rows[0]["ROWID"].ToString().Trim(); }

                        rs1.Dispose();
                        rs1 = null;

                        if(strROWID2 != "")
                        {
                            SQL = "UPDATE MISU_BALJEWON SET ";
                            SQL = SQL + "SuBi=" + nSuBi + "," + ComNum.VBLF;
                            SQL = SQL + "Ilsu=" + nIlsu + "," + ComNum.VBLF;
                            SQL = SQL + "GubTot=" + nGub + "," + ComNum.VBLF;
                            SQL = SQL + "BigubTot=" + nBigub + "," + ComNum.VBLF;
                            SQL = SQL + "TotAmt=" + nAmt[50-1] + ", " + ComNum.VBLF;
                            SQL = SQL + "DRUGAMT = '" + nDrugAmt + "' " + ComNum.VBLF;
                            SQL = SQL + "WHERE ROWID='" + strROWID + "' " + ComNum.VBLF;
                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        }else
                        {
                            if(nTRSNO == 67045)
                            {
                                nTRSNO = nTRSNO;
                            }

                            SQL = "INSERT INTO MISU_BALJEWON (YYMM,IPDNO,TRSNO,Pano,SName,Bi,SuBi,InDate,";
                            SQL = SQL + "WardCode,RoomCode,DeptCode,DrCode,Bohun,AmSet1,Amset4,AmSet6,BonRate," + ComNum.VBLF;
                            SQL = SQL + "RateGasan,GubTot,BigubTot,TotAmt,JungAmt,IpgumAmt, DrugAmt ) " + ComNum.VBLF;
                            SQL = SQL + "SELECT '" + FstrYYMM + "',a.IPDNO,a.TRSNO,a.Pano,b.SName," + ComNum.VBLF;
                            SQL = SQL + "'" + strOldBi + "','" + nSuBi2 + "'," + ComNum.VBLF;
                            SQL = SQL + "TRUNC(a.InDate),b.WardCode,b.RoomCode,a.DeptCode,a.DrCode,a.Bohun," + ComNum.VBLF;
                            SQL = SQL + "a.AmSet1,a.AmSet4,'*',a.BonRate,a.GisulRate," + ComNum.VBLF;
                            SQL = SQL + "   " + nGub + "," + nBigub + "," + nAmt[50 - 1] + ",0,0 , '" + nDrugAmt + "'  " + ComNum.VBLF;
                            SQL = SQL + "FROM IPD_TRANS a,IPD_NEW_MASTER b " + ComNum.VBLF;
                            SQL = SQL + "WHERE a.TRSNO=" + nTRSNO + " " + ComNum.VBLF;
                            SQL = SQL + "AND a.IPDNO=b.IPDNO(+) " + ComNum.VBLF;
                            SQL = SQL + "AND ROWNUM <= 1 " + ComNum.VBLF;
                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        }
                        #endregion
                        strOldBi = strNewBi;
                        for (j = 0; j < 62; j++)
                        {
                            nAmt[j] = 0;
                        }
                        nGub = 0;
                        nBigub = 0;
                    }

                    nSlipAmt = Convert.ToInt64(rsFun1.Rows[ii]["Amt"]);
                    NU = Convert.ToInt32(rsFun1.Rows[ii]["Nu"]);
                    nAmt[NU -1] = nAmt[NU -1] + nSlipAmt;
                    nAmt[50 - 1] = nAmt[50 - 1] + nSlipAmt;
                    if(NU <= 20)
                    {
                        nGub = nGub + nSlipAmt;
                    }else
                    {
                        nBigub = nBigub + nSlipAmt;
                    }

                    nDrugAmt = nDrugAmt + Convert.ToInt64(rsFun1.Rows[ii]["AMT3"]);
                }

                rsFun1.Dispose();
                rsFun1 = null;

                #region Slip_Amt_Update_SUB
                nSuBi = READ_Bi_SuipTong(strOldBi, ArgDate);

                SQL = "SELECT ROWID FROM MISU_BALJEWON ";
                SQL = SQL + "WHERE YYMM='" + ArgYYMM + "' " + ComNum.VBLF;
                SQL = SQL + "AND TRSNO=" + nTRSNO + " " + ComNum.VBLF;
                SQL = SQL + "AND Bi='" + strOldBi + "' " + ComNum.VBLF;
                SqlErr = clsDB.GetDataTable(ref rs1, SQL, clsDB.DbCon);

                strROWID2 = "";
                if (rs1.Rows.Count > 0) { strROWID2 = rs1.Rows[0]["ROWID"].ToString().Trim(); }

                rs1.Dispose();
                rs1 = null;

                if (strROWID2 != "")
                {
                    SQL = "UPDATE MISU_BALJEWON SET ";
                    SQL = SQL + "SuBi=" + nSuBi + "," + ComNum.VBLF;
                    SQL = SQL + "Ilsu=" + nIlsu + "," + ComNum.VBLF;
                    SQL = SQL + "GubTot=" + nGub + "," + ComNum.VBLF;
                    SQL = SQL + "BigubTot=" + nBigub + "," + ComNum.VBLF;
                    SQL = SQL + "TotAmt=" + nAmt[50 - 1] + ", " + ComNum.VBLF;
                    SQL = SQL + "DRUGAMT = '" + nDrugAmt + "' " + ComNum.VBLF;
                    SQL = SQL + "WHERE ROWID='" + strROWID + "' " + ComNum.VBLF;
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                }
                else
                {
                    if (nTRSNO == 67045)
                    {
                        nTRSNO = nTRSNO;
                    }

                    SQL = "INSERT INTO MISU_BALJEWON (YYMM,IPDNO,TRSNO,Pano,SName,Bi,SuBi,InDate,";
                    SQL = SQL + "WardCode,RoomCode,DeptCode,DrCode,Bohun,AmSet1,Amset4,AmSet6,BonRate," + ComNum.VBLF;
                    SQL = SQL + "RateGasan,GubTot,BigubTot,TotAmt,JungAmt,IpgumAmt, DrugAmt ) " + ComNum.VBLF;
                    SQL = SQL + "SELECT '" + FstrYYMM + "',a.IPDNO,a.TRSNO,a.Pano,b.SName," + ComNum.VBLF;
                    SQL = SQL + "'" + strOldBi + "','" + nSuBi2 + "'," + ComNum.VBLF;
                    SQL = SQL + "TRUNC(a.InDate),b.WardCode,b.RoomCode,a.DeptCode,a.DrCode,a.Bohun," + ComNum.VBLF;
                    SQL = SQL + "a.AmSet1,a.AmSet4,'*',a.BonRate,a.GisulRate," + ComNum.VBLF;
                    SQL = SQL + "   " + nGub + "," + nBigub + "," + nAmt[50] + ",0,0 , '" + nDrugAmt + "'  " + ComNum.VBLF;
                    SQL = SQL + "FROM IPD_TRANS a,IPD_NEW_MASTER b " + ComNum.VBLF;
                    SQL = SQL + "WHERE a.TRSNO=" + nTRSNO + " " + ComNum.VBLF;
                    SQL = SQL + "AND a.IPDNO=b.IPDNO(+) " + ComNum.VBLF;
                    SQL = SQL + "AND ROWNUM <= 1 " + ComNum.VBLF;
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                }
                #endregion
                #endregion

                if(nIPDNO != nOldIPDNO)
                {
                    //중간납, 보증금 미대체액을 읽음
                    SQL = "SELECT Bun,SUM(Amt) Amt FROM IPD_NEW_CASH ";
                    SQL = SQL + "WHERE IPDNO=" + nIPDNO + " " + ComNum.VBLF;
                    SQL = SQL + "AND Bun IN ('85','87','88') " + ComNum.VBLF;
                    SQL = SQL + "AND ActDate<=TO_DATE('" + ArgDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                    SQL = SQL + "GROUP BY Bun " + ComNum.VBLF;
                    SqlErr = clsDB.GetDataTable(ref rsFun1, SQL, clsDB.DbCon);

                    nAmt[51 - 1] = 0;

                    for (j = 0; j < rsFun1.Rows.Count; j++)
                    {
                        switch (rsFun1.Rows[j]["Bun"].ToString().Trim())
                        {
                            case "85":
                                nAmt[51 - 1] = nAmt[51 - 1] + Convert.ToInt64(rsFun1.Rows[j]["Amt"]);//보증금
                                break;
                            case "87":
                                nAmt[51 - 1] = nAmt[51 - 1] + Convert.ToInt64(rsFun1.Rows[j]["Amt"]);//중간납
                                break;
                            case "88":
                                nAmt[51 - 1] = nAmt[51 - 1] - Convert.ToInt64(rsFun1.Rows[j]["Amt"]);//중간납대체
                                break;
                        }
                    }

                    rsFun1.Dispose();
                    rsFun1 = null;

                    SQL = "UPDATE MISU_BALJEWON SET ";
                    SQL = SQL + "IpgumAmt=" + nAmt[51 - 1] + " " + ComNum.VBLF;
                    SQL = SQL + "WHERE ROWID='" + strROWID + "' " + ComNum.VBLF;
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    #region Mir_Junggan_Amt_Update
                    //중간납, 보증금 미대체액을 읽음
                    SQL = "SELECT Bi,NVL(SUM(BuildJamt),0) BuildJamt , NVL(SUM(BuildDrugAmt),0) BuildDrugAmt FROM MIR_IPDID ";
                    SQL = SQL + "WHERE IPDNO=" + nIPDNO + " " + ComNum.VBLF;
                    SQL = SQL + "AND Flag='1' " + ComNum.VBLF;
                    SQL = SQL + "AND BuildDate<=TO_DATE('" + ArgDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                    SQL = SQL + "AND (TDate IS NULL OR TDate>TO_DATE('" + ArgDate + "','YYYY-MM-DD')) " + ComNum.VBLF;
                    SQL = SQL + "GROUP BY Bi " + ComNum.VBLF;
                    SqlErr = clsDB.GetDataTable(ref rsFun1, SQL, clsDB.DbCon);
                    nRead2 = rsFun1.Rows.Count; //과장님 확인 요망

                    for (ii = 0; ii < nRead2; ii++)
                    {
                        if (nIPDNO == 1112840)

                        {
                            i = i;

                        }
         
       

                        nMirAmt = Convert.ToInt64(rsFun1.Rows[ii]["BuildJamt"]);
                        nBuildDrugAmt = Convert.ToInt64(rsFun1.Rows[ii]["BuildDrugAmt"]);
                        strBi2 = rsFun1.Rows[ii]["Bi"].ToString().Trim();
                        nSuBi2 = READ_Bi_SuipTong(strBi2, ArgDate).ToString().Trim();
                        if( nMirAmt != 0)
                        {
                            //Update,Insert 여부 점검 
                            SQL = "SELECT ROWID FROM MISU_BALJEWON ";
                            SQL = SQL + "WHERE YYMM='" + ArgYYMM + "' " + ComNum.VBLF;
                            SQL = SQL + "AND IpdNo=" + nIPDNO + " " + ComNum.VBLF;
                            SQL = SQL + "AND Bi='" + strBi2 + "' " + ComNum.VBLF;
                            SQL = SQL + " order by indate " + ComNum.VBLF;
                            SqlErr = clsDB.GetDataTable(ref rs1, SQL, clsDB.DbCon);

                            strROWID2 = "";
                            if(rs1.Rows.Count > 0) { strROWID2 = rs1.Rows[0]["ROWID"].ToString().Trim(); }
                            rs1.Dispose();
                            rs1 = null;

                            if (strROWID2 != "")
                            {
                                SQL = "UPDATE MISU_BALJEWON SET ";
                                SQL = SQL + "JungAmt=" + nMirAmt + ", " + ComNum.VBLF;
                                SQL = SQL + "JungDrugAmt = '" + nDrugAmt + "' " + ComNum.VBLF;
                                SQL = SQL + "WHERE ROWID='" + strROWID2 + "' " + ComNum.VBLF;
                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            }
                            else
                            {
                                if (nTRSNO == 59950) { nTRSNO = nTRSNO; }

                                SQL = "INSERT INTO MISU_BALJEWON (YYMM,IPDNO,TRSNO,Pano,SName,Bi,SuBi,InDate,";
                                SQL = SQL + "WardCode,RoomCode,DeptCode,DrCode,Bohun,AmSet1,Amset4,AmSet6,BonRate," + ComNum.VBLF;
                                SQL = SQL + "RateGasan,TotAmt,JungAmt,IpgumAmt, DrugAmt, JungDrugAmt ) " + ComNum.VBLF;
                                SQL = SQL + "SELECT '" + FstrYYMM + "',a.IPDNO,a.TRSNO,a.Pano,b.SName,a.Bi,'" + nSuBi2 + "'," + ComNum.VBLF;
                                SQL = SQL + "TRUNC(a.InDate),b.WardCode,b.RoomCode,a.DeptCode,a.DrCode,a.Bohun," + ComNum.VBLF;
                                SQL = SQL + "a.AmSet1,a.AmSet4,'*',a.BonRate,a.GisulRate," + ComNum.VBLF;
                                SQL = SQL + "0," + nMirAmt + ",0 , Amt64, '" + nBuildDrugAmt + "' " + ComNum.VBLF;
                                SQL = SQL + "FROM IPD_TRANS a,IPD_NEW_MASTER b " + ComNum.VBLF;
                                SQL = SQL + "WHERE a.IPDNO=" + nIPDNO + " " + ComNum.VBLF;
                                SQL = SQL + "AND a.Bi='" + strBi2 + "' " + ComNum.VBLF;
                                SQL = SQL + "AND a.IPDNO=b.IPDNO(+) " + ComNum.VBLF;
                                SQL = SQL + "AND ROWNUM <= 1 " + ComNum.VBLF;

                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            }
                        }
                        rsFun1.Dispose();
                        rsFun1 = null;
                    }
                    #endregion

                    nOldIPDNO = nIPDNO;
                }
            }

            rsFun.Dispose();
            rsFun = null;

            //임시 200508월 MISU_BALJEWON_200508은 컨버젼 안함

            //총진료비, 중간납, 중간청구액이 모두 0원인것은 삭제함
            SQL = "DELETE MISU_BALJEWON ";
            SQL = SQL + "WHERE YYMM='" + ArgYYMM + "' " + ComNum.VBLF;
            SQL = SQL + "AND TotAmt=0 " + ComNum.VBLF;
            SQL = SQL + "AND JungAmt=0 " + ComNum.VBLF;
            SQL = SQL + "AND IpgumAmt=0 " + ComNum.VBLF;

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

        }

        int READ_Bi_SuipTong(string ArgBi,string ArgJobDate)
        {
            int ArgBiNo = 0;

            if(ArgJobDate.CompareTo("2003-11-03") >= 0)
            {
                switch (ArgBi)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "32":
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                        ArgBiNo = 1; //보험
                        break;
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                        ArgBiNo = 2; //보호
                        break;
                    case "31":
                    case "33":
                        ArgBiNo = 3; //산재
                        break;
                    case "52":
                        ArgBiNo = 4; //자보
                        break;
                    default:
                        ArgBiNo = 5; //일반
                        break;
                }
            }else
            {
                switch (ArgBi)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                        ArgBiNo = 1; //보험
                        break;
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                        ArgBiNo = 2; //보호
                        break;
                    case "31":
                    case "32":
                    case "33":
                        ArgBiNo = 3; //산재
                        break;
                    case "52":
                        ArgBiNo = 4; //자보
                        break;
                    default:
                        ArgBiNo = 5; //일반
                        break;
                }
            }

            return ArgBiNo;
        }

        void Month_Yeyak_Sunsu_Build()
        {
            int i = 0;
            int j = 0;
            int nRead = 0;
            string strReBuild = "";

            string strPano = "";
            string strBi = "";
            string strSuBi = "";
            string strActdate = "";
            string strDeptCode = "";
            string strDate1 = "";
            string strNextDate = "";

            if(FstrCOMMIT != "OK") { return; }

            lblMsg.Text = "외래 예약선수금 상세내역 형성중";

            strNextDate = CF.DATE_ADD(clsDB.DbCon, FstrTDate, 1);

            //해당월 자료가 존재여부 Check
            SQL = "SELECT COUNT(*) CNT FROM MISU_BALYEYAK ";
            SQL = SQL + "WHERE YYMM='" + FstrYYMM + "' " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            strReBuild = "";

            if(dt.Rows.Count > 0)
            {
                if(Convert.ToInt32(dt.Rows[0]["CNT"]) > 0) { strReBuild = "OK"; }
            }

            dt.Dispose();
            dt = null;

            if(strReBuild == "OK")
            {
                if (ComFunc.MsgBoxQ("해당월의 자료가 이미 있습니다." + ComNum.VBLF + "삭제후 다시 작업을 하시겟습니까 ?", "확인") == DialogResult.No)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
            }

            clsDB.setBeginTran(clsDB.DbCon);

            if(strReBuild == "OK")
            {
                SQL = "DELETE MISU_BALYEYAK ";
                SQL = SQL + "WHERE YYMM='" + FstrYYMM + "' " + ComNum.VBLF;
                SQL = SQL + "" + ComNum.VBLF;

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if(SqlErr != "")
                {
                    FstrCOMMIT = "NO";
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("기존의 자료를 삭제중 오류가 발생함", "ROLLBACK");
                    return;
                }
            }

            //해당월 이전에 예약수납한 명단을 INSERT
            SQL = "INSERT INTO MISU_BALYEYAK (";
            SQL = SQL + "YYMM,PANO,DEPTCODE,BI,SUBI,SNAME,DRCODE,DATE1,DATE2,DATE3,CHOJAE,GBGAMEK,GBSPC," + ComNum.VBLF;
            SQL = SQL + "JIN,AMT1,AMT2,AMT3,AMT4,AMT5,AMT6,AMT7,PART,PRTSEQNO,BOHUN,GELCODE) " + ComNum.VBLF;
            SQL = SQL + "SELECT '" + FstrYYMM + "',PANO,DEPTCODE,BI,'',SNAME,DRCODE,DATE1,DATE2,DATE3,CHOJAE,GBGAMEK,GBSPC," + ComNum.VBLF;
            SQL = SQL + "JIN,AMT1,AMT2,AMT3,AMT4,AMT5,AMT6,AMT7,PART,PRTSEQNO,BOHUN,GELCODE " + ComNum.VBLF;
            SQL = SQL + "FROM OPD_RESERVED_NEW " + ComNum.VBLF;
            SQL = SQL + "WHERE Date1 >= TO_DATE('2002-01-01','YYYY-MM-DD') " + ComNum.VBLF;
            SQL = SQL + "AND Date1 <= TO_DATE('" + FstrTDate + "','YYYY-MM-DD') " + ComNum.VBLF;
            SQL = SQL + "AND (TRANSDATE IS NULL OR TRUNC(TRANSDATE) > TO_DATE('" + FstrTDate + "','YYYY-MM-DD')) " + ComNum.VBLF;
            SQL = SQL + "AND (RETDATE IS NULL OR TRUNC(RETDATE) > TO_DATE('" + FstrTDate + "','YYYY-MM-DD')) " + ComNum.VBLF;

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if(SqlErr != "")
            {
                FstrCOMMIT = "NO";
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("OPD_RESERVER에서 자료를 복사중 오류가 발생함", "ROLLBACK");
                return;
            }

            //예약선수금 상세내역에 수입통계 환자종류를 UPDATE 
            SQL = "SELECT Bi,COUNT(*) CNT FROM MISU_BALYEYAK ";
            SQL = SQL + "WHERE YYMM='" + FstrYYMM + "' " + ComNum.VBLF;
            SQL = SQL + "GROUP BY Bi " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            nRead = dt.Rows.Count;

            for (i = 0; i < nRead; i++)
            {
                strBi = dt.Rows[i]["Bi"].ToString().Trim();
                strSuBi = VB.Format(READ_Bi_SuipTong(strBi, VB.Left(FstrYYMM, 4) + "-" + VB.Right(FstrYYMM, 2) + "-01"), "0");
                //환자구분을 UPDATE 
                SQL = "UPDATE MISU_BALYEYAK SET SuBi='" + strSuBi + "' ";
                SQL = SQL + "WHERE YYMM='" + FstrYYMM + "' " + ComNum.VBLF;
                SQL = SQL + "AND Bi='" + strBi + "' " + ComNum.VBLF;

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            dt.Dispose();
            dt = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void Month_Exam_Yeyak_Sunsu_Build()
        {
            int i = 0;
            int j = 0;
            int nRead = 0;
            string strReBuild = "";

            string strPano = "";
            string strBi = "";
            string strSuBi = "";
            string strActdate = "";
            string strDeptCode = "";
            string strDate1 = "";
            string strNextDate = "";

            progressBarX1.Minimum = 1;
            progressBarX1.Value = progressBarX1.Minimum;

            if(FstrCOMMIT != "OK") { return; }

            lblMsg.Text = "외래 예약 선수금 상세내역 형성중";

            strNextDate = CF.DATE_ADD(clsDB.DbCon, FstrTDate, 1);

            //해당월 자료가 존재 여부 Check
            SQL = "SELECT COUNT(*) CNT FROM MISU_BALYEYAK_EXAM ";
            SQL = SQL + "WHERE YYMM='" + FstrYYMM + "' " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            strReBuild = "";

            if(dt.Rows.Count > 0)
            {
                if(Convert.ToInt32(dt.Rows[0]["CNT"]) > 0) { strReBuild = "OK"; }
            }

            dt.Dispose();
            dt = null;

            if(strReBuild == "OK")
            {
                if (ComFunc.MsgBoxQ("해당월의 자료가 이미 있습니다." + ComNum.VBLF + "삭제후 다시 작업을 하시겟습니까 ?", "확인") == DialogResult.No)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
            }

            clsDB.setBeginTran(clsDB.DbCon);

            if(strReBuild == "OK")
            {
                SQL = "DELETE MISU_BALYEYAK_EXAM ";
                SQL = SQL + "WHERE YYMM='" + FstrYYMM + "' " + ComNum.VBLF;

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if(SqlErr != "")
                {
                    FstrCOMMIT = "NO";
                    clsDB.setRollbackTran(dbCon);
                    ComFunc.MsgBox("기존의 자료를 삭제중 오류가 발생함", "ROLLBACK");
                    return;
                }
            }

            lblMsg.Text = "OPD_RESERVED_EXAM의 내용을 COPY";

            //해당월 이전에 예약수납한 명단을 INSERT
            SQL = "INSERT INTO MISU_BALYEYAK_EXAM (";
            SQL = SQL + "YYMM,PANO,DEPTCODE,BI,SUBI,SNAME,DRCODE,DATE1,DATE2,DATE3,CHOJAE,GBGAMEK,GBSPC," + ComNum.VBLF;
            SQL = SQL + "JIN,AMT7,PART,PRTSEQNO,BOHUN,GELCODE) " + ComNum.VBLF;
            SQL = SQL + "SELECT '" + FstrYYMM + "'," + ComNum.VBLF;
            SQL = SQL + "PANO,DEPTCODE,BI,'',SNAME,DRCODE,ACTDATE,DATE2,DATE3,'3'," + ComNum.VBLF;
            SQL = SQL + "GBGAMEK,GBSPC," + ComNum.VBLF;
            SQL = SQL + "JIN,AMT6,PART,0,BOHUN,GELCODE " + ComNum.VBLF;
            SQL = SQL + "FROM OPD_RESERVED_EXAM " + ComNum.VBLF;
            SQL = SQL + "WHERE ActDate >= TO_DATE('2011-09-27','YYYY-MM-DD') " + ComNum.VBLF;
            SQL = SQL + "AND ActDate <= TO_DATE('" + FstrTDate + "','YYYY-MM-DD') " + ComNum.VBLF;
            SQL = SQL + "AND (TRANSDATE IS NULL OR TRUNC(TRANSDATE) > TO_DATE('" + FstrTDate + "','YYYY-MM-DD')) " + ComNum.VBLF;
            SQL = SQL + "AND (RETDATE IS NULL OR TRUNC(RETDATE) > TO_DATE('" + FstrTDate + "','YYYY-MM-DD')) " + ComNum.VBLF;
            SQL = SQL + "AND (DATE1 IS NULL OR TRUNC(DATE1) > TO_DATE('" + FstrTDate + "','YYYY-MM-DD')) " + ComNum.VBLF;

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if(SqlErr != "")
            {
                FstrCOMMIT = "NO";
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("OPD_RESERVED_EXAM에서 자료를 복사중 오류가 발생함", "ROLLBACK");
                return;
            }

            //예약선수금 상세내역에 수입통계 환자종류를 UPDATE 
            SQL = "SELECT Bi,COUNT(*) CNT FROM MISU_BALYEYAK_EXAM ";
            SQL = SQL + "WHERE YYMM='" + FstrYYMM + "' " + ComNum.VBLF;
            SQL = SQL + "GROUP BY Bi " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            nRead = dt.Rows.Count;

            for (i = 0; i < nRead; i++)
            {
                strBi = dt.Rows[i]["Bi"].ToString().Trim();
                strSuBi = VB.Format(READ_Bi_SuipTong(strBi, VB.Left(FstrYYMM, 4) + "-" + VB.Right(FstrYYMM, 2) + "-01"), "0");
                //환자구분을 UPDATE 

                SQL = "UPDATE MISU_BALYEYAK_EXAM SET SuBi='" + strSuBi + "' ";
                SQL = SQL + "WHERE YYMM='" + FstrYYMM + "' " + ComNum.VBLF;
                SQL = SQL + "AND Bi='" + strBi + "' " + ComNum.VBLF;

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            dt.Dispose();
            dt = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }
    }
}