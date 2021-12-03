using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB.dll
    /// File Name       : frmNHICCancer.cs
    /// Description     : 암등록자자격확인
    /// Author          : 이정현
    /// Create Date     : 2017-06-17
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// VB\basic\buppat\frmNHICCancer.frm => frmNHICCancer.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\buppat\frmNHICCancer.frm
    /// </seealso>
    /// <vbp>
    /// default : VB\basic\buppat\buppat.vbp
    /// </vbp>
    public partial class frmNHICCancer : Form
    {
        /// <summary>
        /// 이벤트를 전달할 경우
        /// 중증등록번호/적용시작일자/적용종료일자/상병코드/지사/세대주명/기호/증번호
        /// </summary>
        /// <param name="strNHICNO"></param>
        /// <param name="strNHICFDATE"></param>
        /// <param name="strNHICTDATE"></param>
        /// <param name="strNHICILLCODE"></param>
        /// <param name="strNHICJISA"></param>
        /// <param name="strNHICNAME"></param>
        /// <param name="strNHICKIHO"></param>
        /// <param name="strNHICNUM"></param>
        public delegate void SetNHIC(string strNHICNO, string strNHICFDATE, string strNHICTDATE, 
            string strNHICILLCODE, string strNHICJISA, string strNHICNAME, string strNHICKIHO, string strNHICNUM, string strNHICBONIN);
        public event SetNHIC rSetSetNHIC;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        private string GstrPtNo = "";
        private string GstrDept = "";
        private string GstrSName = "";
        private string GstrJumin = "";
        private string GstrBDate = "";
        private string GstrGubun = "";
        private string GstrSabun = "";
        private int GintWrtNo = 0;
        private int GintTimerCnt = 0;

        string strNHICBONIN = "";

        public frmNHICCancer ()
        {
            InitializeComponent ();
        }

        public frmNHICCancer(string strPtNo, string strDept, string strSName, string strJumin, string strBDate, string strGubun, string strSabun)
        {
            InitializeComponent();

            GstrPtNo = strPtNo;
            GstrDept = strDept;
            GstrSName = strSName;
            GstrJumin = strJumin;
            GstrBDate = strBDate;
            GstrGubun = strGubun;
            GstrSabun = strSabun;
        }

        private void frmNHICCancer_Load (object sender , EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            timer.Enabled = false;
            lblMenu.Text = ""; 

            if (GstrGubun == "2")
            {
                this.Text = "희귀난치성 자격확인";
                lblTitle.Text = "희귀난치성 자격확인";
            }
            else if (GstrGubun == "3")
            {
                this.Text = "중증화상 자격확인";
                lblTitle.Text = "중증화상 자격확인";
            }
            else if (GstrGubun == "1")
            {
                this.Text = "중증암 자격확인";
                lblTitle.Text = "중증암 자격확인";
            }

            ComFunc.ReadSysDate(clsDB.DbCon);
            
            if(GstrPtNo == "")
            {
                ComFunc.MsgBox("등록번호를 확인하세요!!");
                return;
            }

            if (GstrJumin == "")
            {
                ComFunc.MsgBox("주민등록번호를 확인하세요!!");
                return;
            }

            GintWrtNo = READ_Next_NhicNo();

            ssView_Sheet1.Cells[0, 1].Text = GstrPtNo;
            ssView_Sheet1.Cells[1, 1].Text = GstrJumin;
            ssView_Sheet1.Cells[2, 1].Text = GstrSName;

            if (SaveNHIC() == true)
            {
                lblMenu.Text = "검색중입니다. 잠시 기다려 주세요.";
                timer.Enabled = true;
            }
        }

        private int READ_Next_NhicNo()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int rtnVal = 0;

            try
            {
                SQL = "";
                SQL = "SELECT SEQ_OPD_NHIC.NEXTVAL WRTNO FROM DUAL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = Convert.ToInt32(VB.Val(dt.Rows[0]["WRTNO"].ToString().Trim()));

                dt.Dispose();
                dt = null;
                
                return rtnVal;
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool SaveNHIC()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strSysDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC";
                SQL = SQL + ComNum.VBLF + "     (WRTNO, ACTDATE, PANO, ";
                SQL = SQL + ComNum.VBLF + "     DEPTCODE, SNAME, REQTIME, REQTYPE, ";
                SQL = SQL + ComNum.VBLF + "     JUMIN,JUMIN_NEW, JOB_STS, REQ_SABUN,BDATE)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         " + GintWrtNo + ", TO_DATE('" + strSysDate + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "         '" + GstrPtNo + "', '" + GstrDept + "', '" + GstrSName + "', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE, 'M1', '" + VB.Left(GstrJumin, 7) + "******" + "','" + clsAES.AES(GstrJumin) + "', '0', " + clsPublic.GnJobSabun + ",";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + GstrBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "     ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if(SqlErr != "")
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
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strNHICNO = ssView_Sheet1.Cells[3, 1].Text.Trim();           //중증등록번호

            if (strNHICNO.IndexOf("(") > 0)
            {
                strNHICNO = strNHICNO.Substring(0, strNHICNO.IndexOf("("));
            }
            strNHICNO = strNHICNO.Trim();


            string strNHICFDATE = ssView_Sheet1.Cells[4, 1].Text.Trim();        //적용시작일자
            string strNHICTDATE = ssView_Sheet1.Cells[5, 1].Text.Trim();        //적용종료일자
            string strNHICILLCODE = ssView_Sheet1.Cells[6, 1].Text.Trim();      //상병코드
            string strNHICJISA = ssView_Sheet1.Cells[7, 1].Text.Trim();         //지사
            string strNHICNAME = ssView_Sheet1.Cells[8, 1].Text.Trim();         //세대주명
            string strNHICKIHO = ssView_Sheet1.Cells[9, 1].Text.Trim();         //기호
            string strNHICNUM = ssView_Sheet1.Cells[10, 1].Text.Trim();         //증번호

            strNHICBONIN = ssView_Sheet1.Cells[3, 1].Text.Trim();  //번호 및 이름

            if (GstrGubun == "3")
            {
                strNHICFDATE = ssView_Sheet1.Cells[13, 1].Text.Trim();        //적용시작일자
                strNHICTDATE = ssView_Sheet1.Cells[14, 1].Text.Trim();        //적용종료일자
                strNHICILLCODE = ssView_Sheet1.Cells[15, 1].Text.Trim();      //상병코드
                strNHICJISA = ssView_Sheet1.Cells[7, 1].Text.Trim();         //지사
                strNHICNAME = ssView_Sheet1.Cells[8, 1].Text.Trim();         //세대주명
                strNHICKIHO = ssView_Sheet1.Cells[9, 1].Text.Trim();         //기호
                strNHICNUM = ssView_Sheet1.Cells[10, 1].Text.Trim();         //증번호

                strNHICBONIN = ssView_Sheet1.Cells[12, 1].Text.Trim();  //번호 및 이름

                strNHICNO = ssView_Sheet1.Cells[12, 1].Text.Trim();           //중증등록번호

                if (strNHICNO.IndexOf("(") > 0)
                {
                    strNHICNO = strNHICNO.Substring(0, strNHICNO.IndexOf("("));
                }
                strNHICNO = strNHICNO.Trim();

            }

            rSetSetNHIC(strNHICNO, strNHICFDATE, strNHICTDATE, strNHICILLCODE, strNHICJISA, strNHICNAME, strNHICKIHO, strNHICNUM, strNHICBONIN);
        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            rEventClosed();
            this.Close();
        }

        private void timer_Tick (object sender , EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strJob_STS = "";
            string strRegs1 = "";
            string strRegs2 = "";
            string strRegs3 = "";
            string strRegs4 = "";
            string strRegs5 = "";
            string strRegs9 = "";
            string strRegs11 = "";
            string strRegs20 = "";      //잠복결핵 V010 2021-11-10
            string strChk = "";
            string strOK = "";

            timer.Enabled = false;

            GintTimerCnt = GintTimerCnt + 1;

            if (GintTimerCnt >= 31)
            {
                lblMenu.Text = "자격확인 오류....";

                if (SaveErr() == true)
                {
                    btnSave.Enabled = false;
                    GintTimerCnt = 0;
                    return;
                }
            }

            try
            {
                SQL = "";
                SQL = "SELECT PANO,M2_JAGEK,M2_CDATE,M2_SUJIN_NAME,";
                SQL = SQL + ComNum.VBLF + "     M2_SEDAE_NAME,M2_KIHO,M2_GKIHO,M2_SANGSIL,";
                SQL = SQL + ComNum.VBLF + "     M2_BONIN,M2_GJAN_AMT,M2_CHULGUK,M2_JANG_DATE,";
                SQL = SQL + ComNum.VBLF + "     M2_SHOSPITAL1,M2_SHOSPITAL2,M2_SHOSPITAL3,";
                SQL = SQL + ComNum.VBLF + "     M2_SHOSPITAL4,M2_SHOSPITAL_NAME1,M2_SHOSPITAL_NAME2,";
                SQL = SQL + ComNum.VBLF + "     M2_SHOSPITAL_NAME3,M2_SHOSPITAL_NAME4,JOB_STS,";
                SQL = SQL + ComNum.VBLF + "     M2_DISREG1,M2_DISREG2,M2_DISREG2_A,M2_DISREG2_B,M2_DISREG2_C,M2_DISREG3,M2_DISREG4,";
                SQL = SQL + ComNum.VBLF + "     M2_DISREG5,M2_DISREG9 , M2_DISREG11, M2_DISREG20"; 
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_NHIC";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO  = '" + GintWrtNo + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strJob_STS = dt.Rows[0]["JOB_STS"].ToString().Trim();

                    if (strJob_STS == "2")
                    {
                        strRegs5 = dt.Rows[0]["M2_DISREG5"].ToString().Trim();


                        if (GstrGubun == "2") //희귀
                        {
                            strRegs4 = dt.Rows[0]["M2_DISREG2"].ToString().Trim();
                            if (dt.Rows[0]["M2_DISREG2_A"].ToString().Trim() != "") { strRegs4 = dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(); }
                            if (dt.Rows[0]["M2_DISREG2_B"].ToString().Trim() != "") { strRegs4 = dt.Rows[0]["M2_DISREG2_B"].ToString().Trim(); }
                            if (dt.Rows[0]["M2_DISREG2_C"].ToString().Trim() != "") { strRegs4 = dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(); }
                            strRegs9 = dt.Rows[0]["M2_DISREG9"].ToString().Trim();
                            strRegs11 = dt.Rows[0]["M2_DISREG11"].ToString().Trim();
                            strRegs20 = dt.Rows[0]["M2_DISREG20"].ToString().Trim();
                        }
                        else if (GstrGubun == "1")      //중증암
                        {
                            strRegs4 = dt.Rows[0]["M2_DISREG4"].ToString().Trim();
                        }
                        else if (GstrGubun == "3")
                        {
                            strRegs4 = "";
                        }

                        if (strRegs4 != "")
                        {
                            if (dt.Rows[0]["M2_DISREG2_B"].ToString().Trim() != "")
                            {
                                ssView_Sheet1.Cells[3, 1].Text = VB.Mid(strRegs4, 5, 15).Trim() + " (중증희귀)"; //난치
                            }
                            else if (dt.Rows[0]["M2_DISREG4"].ToString().Trim() != "")
                            {
                                ssView_Sheet1.Cells[3, 1].Text = VB.Mid(strRegs4, 5, 15).Trim() + " (중증암)";
                            }
                            else
                            {
                                ssView_Sheet1.Cells[3, 1].Text = VB.Mid(strRegs4, 5, 15).Trim() + " (희귀)";    //희귀
                            }
                            ssView_Sheet1.Cells[4, 1].Text = ComFunc.FormatStrToDateEx(VB.Mid(strRegs4, 20, 8), "D", "-");
                            ssView_Sheet1.Cells[5, 1].Text = ComFunc.FormatStrToDateEx(VB.Mid(strRegs4, 28, 8), "D", "-");
                            ssView_Sheet1.Cells[6, 1].Text = VB.Mid(strRegs4, 36, 5).Trim();
                            ssView_Sheet1.Cells[7, 1].Text = dt.Rows[0]["M2_SHOSPITAL_NAME1"].ToString().Trim();
                            ssView_Sheet1.Cells[8, 1].Text = dt.Rows[0]["M2_SEDAE_NAME"].ToString().Trim();     //세대주성명
                            ssView_Sheet1.Cells[9, 1].Text = dt.Rows[0]["M2_KIHO"].ToString().Trim();       //보장기관기호
                            ssView_Sheet1.Cells[10, 1].Text = dt.Rows[0]["M2_GKIHO"].ToString().Trim();      //시설기호
                        }

                        if (strRegs9 != "")
                        {
                            strOK = "";
                            if (strRegs4 != "")
                            {
                                if (ComFunc.MsgBoxQ("산정특례(결핵)대상 정보가 있습니다!! Yes=>조회, No=>취소", "추가자격확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    strOK = "OK";
                                }
                            }
                            else
                            {
                                strOK = "OK";
                            }

                            if (strOK == "OK")
                            {
                                ssView_Sheet1.Cells[3, 1].Text = VB.Mid(strRegs9, 5, 15).Trim() + " (결핵)";
                                ssView_Sheet1.Cells[4, 1].Text = ComFunc.FormatStrToDateEx(VB.Mid(strRegs9, 20, 8), "D", "-");
                                ssView_Sheet1.Cells[5, 1].Text = ComFunc.FormatStrToDateEx(VB.Mid(strRegs9, 28, 8), "D", "-");
                                ssView_Sheet1.Cells[6, 1].Text = VB.Mid(strRegs9, 36, 5).Trim();
                                ssView_Sheet1.Cells[7, 1].Text = dt.Rows[0]["M2_SHOSPITAL_NAME1"].ToString().Trim();
                                ssView_Sheet1.Cells[8, 1].Text = dt.Rows[0]["M2_SEDAE_NAME"].ToString().Trim();     //세대주성명
                                ssView_Sheet1.Cells[9, 1].Text = dt.Rows[0]["M2_KIHO"].ToString().Trim();       //보장기관기호
                                ssView_Sheet1.Cells[10, 1].Text = dt.Rows[0]["M2_GKIHO"].ToString().Trim();      //시설기호
                            }
                        }

                        if (strRegs20 != "")
                        {
                            strOK = "";
                            if (strRegs4 != "")
                            {
                                if (ComFunc.MsgBoxQ("산정특례(잠복결핵)대상 정보가 있습니다!! Yes=>조회, No=>취소", "추가자격확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    strOK = "OK";
                                }
                            }
                            else
                            {
                                strOK = "OK";
                            }

                            if (strOK == "OK")
                            {
                                ssView_Sheet1.Cells[3, 1].Text = VB.Mid(strRegs20, 5, 15).Trim() + " (잠복결핵)";
                                ssView_Sheet1.Cells[4, 1].Text = ComFunc.FormatStrToDateEx(VB.Mid(strRegs20, 20, 8), "D", "-");
                                ssView_Sheet1.Cells[5, 1].Text = ComFunc.FormatStrToDateEx(VB.Mid(strRegs20, 28, 8), "D", "-");
                                ssView_Sheet1.Cells[6, 1].Text = VB.Mid(strRegs20, 36, 5).Trim();
                                ssView_Sheet1.Cells[7, 1].Text = dt.Rows[0]["M2_SHOSPITAL_NAME1"].ToString().Trim();
                                ssView_Sheet1.Cells[8, 1].Text = dt.Rows[0]["M2_SEDAE_NAME"].ToString().Trim();     //세대주성명
                                ssView_Sheet1.Cells[9, 1].Text = dt.Rows[0]["M2_KIHO"].ToString().Trim();       //보장기관기호
                                ssView_Sheet1.Cells[10, 1].Text = dt.Rows[0]["M2_GKIHO"].ToString().Trim();      //시설기호
                            }
                        }

                        if (strRegs11 != "")
                        {
                            ssView_Sheet1.Cells[3, 1].Text = VB.Mid(strRegs11, 15, 17).Trim() + " (중증치매)";
                            ssView_Sheet1.Cells[4, 1].Text = ComFunc.FormatStrToDateEx(VB.Mid(strRegs11, 32, 8), "D", "-");
                            ssView_Sheet1.Cells[5, 1].Text = ComFunc.FormatStrToDateEx(VB.Mid(strRegs11, 40, 8), "D", "-");
                            ssView_Sheet1.Cells[6, 1].Text = VB.Mid(strRegs11, 5, 5).Trim();
                            ssView_Sheet1.Cells[7, 1].Text = dt.Rows[0]["M2_SHOSPITAL_NAME1"].ToString().Trim();
                            ssView_Sheet1.Cells[8, 1].Text = dt.Rows[0]["M2_SEDAE_NAME"].ToString().Trim();     //세대주성명
                            ssView_Sheet1.Cells[9, 1].Text = dt.Rows[0]["M2_KIHO"].ToString().Trim();       //보장기관기호
                            ssView_Sheet1.Cells[10, 1].Text = dt.Rows[0]["M2_GKIHO"].ToString().Trim();      //시설기호


                        }



                        if (strRegs5 != "")
                        {
                            //중증화상정보
                            ssView_Sheet1.Cells[12, 1].Text = VB.Mid(strRegs5, 5, 15).Trim() + " (중증화상)"; ;
                            ssView_Sheet1.Cells[13, 1].Text = ComFunc.FormatStrToDateEx(VB.Mid(strRegs5, 20, 8), "D", "-");
                            ssView_Sheet1.Cells[14, 1].Text = ComFunc.FormatStrToDateEx(VB.Mid(strRegs5, 28, 8), "D", "-");
                            ssView_Sheet1.Cells[15, 1].Text = VB.Left(strRegs5, 4).Trim();

                            ssView_Sheet1.Cells[7, 1].Text = dt.Rows[0]["M2_SHOSPITAL_NAME1"].ToString().Trim();
                            ssView_Sheet1.Cells[8, 1].Text = dt.Rows[0]["M2_SEDAE_NAME"].ToString().Trim();     //세대주성명
                            ssView_Sheet1.Cells[9, 1].Text = dt.Rows[0]["M2_KIHO"].ToString().Trim();       //보장기관기호
                            ssView_Sheet1.Cells[10, 1].Text = dt.Rows[0]["M2_GKIHO"].ToString().Trim();      //시설기호
                        }

                        GintTimerCnt = 0;
                        lblMenu.Text = "자격 조회 완료!!!";
                        return;
                    }
                    else
                    {
                        switch (strJob_STS)
                        {
                            case "0":
                            case "1":
                                lblMenu.Text = "자격 조회중입니다. 잠시 기다려 주세요!!!";
                                break;
                            case "3":
                                lblMenu.Text = "자격 조회 접속 오류!!!";
                                lblMenu.Text = "자격 조회 완료!!!";
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                btnSave.Enabled = true;
                timer.Enabled = true;
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

        private bool SaveErr()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "OPD_NHIC";
                SQL = SQL + ComNum.VBLF + "     SET";
                SQL = SQL + ComNum.VBLF + "         SENDTIME = SYSDATE,";
                SQL = SQL + ComNum.VBLF + "         JOB_STS = '3',";
                SQL = SQL + ComNum.VBLF + "         MESSAGE = '자격조회시 시간초과' ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + GintWrtNo;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if(SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
    }
}
