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
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewPatient.cs
    /// Description     : 환자조회
    /// Author          : 안정수
    /// Create Date     : 2017-08-23
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\OPDHNIP\OpdHNIP01.frm(FrmView) => frmPmpaViewPatient.cs 으로 변경함
    /// TODO : KoppUPdate_HNIP(strTemp, "PT"), KPPO_JOB.KPPO_Update함수 구현 필요, 
    ///        FrmPanoView폼 구현 및 실제 사용여부확인 및 테스트 필요함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\OPDHNIP\OpdHNIP01.frm(FrmView)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewPatient : Form
    {
        ComFunc CF = new ComFunc();
        string mstrPano = "";
        string mstrJobSabun = "";
        int mnJobSabun = 0;
        string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

        string FstrBdate = "";
        string FstrDept = "";
        string FstrIO = "";
        string FstrNum = "";
        string FstrBI = "";

        string FstrINDate = "";
        string FstrOUTDate = "";

        public frmPmpaViewPatient()
        {
            InitializeComponent();
            setEvent();
        }
        public frmPmpaViewPatient(string GstrPano, string GstrJobSabun)
        {
            InitializeComponent();
            mstrPano = GstrPano;
            mstrJobSabun = GstrJobSabun;
            mnJobSabun = Convert.ToInt32(GstrJobSabun);
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnHelp.Click += new EventHandler(eBtnEvent);
            this.btnSend.Click += new EventHandler(eBtnEvent);
            this.btnSendSlip.Click += new EventHandler(eBtnEvent);
            this.btnSendDtl.Click += new EventHandler(eBtnEvent);
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

            optIO0.Checked = true;

            Screen_Clear();

            txtPano.Text = "";
            dtpFdate.Text = Convert.ToDateTime(CurrentDate).AddDays(-365).ToShortDateString();
            dtpTdate.Text = CurrentDate;
        }

        void Screen_Clear()
        {
            txtName.Text = "";
            txtAge.Text = "";
            txtSex.Text = "";
            txtBirth.Text = "";
            txtJumin.Text = "";
            txtTel.Text = "";
            txtHPhone.Text = "";
            txtZipCode.Text = "";
            txtJuso.Text = "";
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnHelp)
            {
                mstrPano = "";
                //TODO : FrmPanoView.Show1 구현필요
                           

                if (mstrPano != "")
                {
                    txtPano.Text = mstrPano;
                }
                eGetData();
            }

            else if (sender == this.btnView)
            {
                eGetData();
            }

            else if (sender == this.btnView02)
            {
                eGetData02();
            }

            else if (sender == this.btnSend)
            {
                eSend();
            }

            else if (sender == this.btnSendSlip)
            {
                eSendSlip();
            }

            else if (sender == this.btnSendDtl)
            {
                eSenddtl();
            }
        }

        void eGetData()
        {
            Screen_Clear();

            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

            //정보를 Display
            Screen_Display();
        }

        void Screen_Display()
        {
            string strPano = "";
            string strJumin = "";
            string strZipCode = "";
            string strBirth = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;


            DataTable dt = null;
            DataTable dt1 = null;

            strPano = ComFunc.SetAutoZero(txtPano.Text, 8);
            txtPano.Text = strPano;

            //환자마스타의 정보를 READ
            //고객정보 Display
            #region Screen_BAS_Patient(GoSub)

            //환자마스타의 정보를 READ
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  SName,Sex,Jumin1,Jumin2,ZipCode1,ZipCode2,Juso,Tel,                         ";
            SQL += ComNum.VBLF + "  TO_CHAR(StartDate,'YYYY-MM-DD') StartDate,                                  ";
            SQL += ComNum.VBLF + "  TO_CHAR(LastDate,'YYYY-MM-DD')  LastDate,                                   ";
            SQL += ComNum.VBLF + "  TO_CHAR(Birth,'YYYY-MM-DD')  Birth,GbBirth,                                 ";
            SQL += ComNum.VBLF + "  DeptCode,HPhone,EMail,Jikup,GbJuger,Religion,GbInfor,GbSMS                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT                                        ";
            SQL += ComNum.VBLF + "WHERE Pano = '" + strPano + "'                                                ";

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
                    txtName.Text = dt.Rows[0]["SName"].ToString().Trim();
                    txtJumin.Text = dt.Rows[0]["Jumin1"].ToString().Trim() + "-" + dt.Rows[0]["Jumin2"].ToString().Trim();
                    strJumin = dt.Rows[0]["Jumin1"].ToString().Trim() + dt.Rows[0]["Jumin2"].ToString().Trim();
                    txtAge.Text = ComFunc.AgeCalcEx(strJumin, CurrentDate).ToString();

                    txtZipCode.Text = dt.Rows[0]["ZipCode1"].ToString().Trim() + "-" + dt.Rows[0]["ZipCode2"].ToString().Trim();
                    strZipCode = dt.Rows[0]["ZipCode1"].ToString().Trim() + dt.Rows[0]["ZipCode2"].ToString().Trim();

                    txtSex.Text = dt.Rows[0]["Sex"].ToString().Trim();
                    txtTel.Text = dt.Rows[0]["Tel"].ToString().Trim();
                    txtHPhone.Text = dt.Rows[0]["HPhone"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            //우편번호로 주소를 READ
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  MailJuso                                                                    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MAILNEW                                        ";
            SQL += ComNum.VBLF + "WHERE MailCode = '" + strZipCode + "'                                         ";

            try
            {

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count == 0)
                {
                    dt1.Dispose();
                    dt1 = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtJuso.Text = dt1.Rows[0]["MailJuso"].ToString().Trim() + " " + dt.Rows[0]["Juso"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt1.Dispose();
            dt1 = null;

            //생일을 Display
            strBirth = dt.Rows[0]["Birth"].ToString().Trim();
            txtBirth.Text = strBirth;

            dt.Dispose();
            dt = null;

            clsDB.setBeginTran(clsDB.DbCon);
            //병원 db에 history

            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_HNIP_HISTORY";
            SQL += ComNum.VBLF + "( ACTDATE, GBN, CDATA , SABUN , ENTDATE, FLAG  )";
            SQL += ComNum.VBLF + "VALUES(";
            SQL += ComNum.VBLF + "TRUNC(SYSDATE), 'PV', '" + txtPano.Text + "',    '" + mstrJobSabun + "', SYSDATE , '' ";
            SQL += ComNum.VBLF + ")";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            try
            {
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBox("저장하였습니다.");
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
            #endregion Screen_BAS_Patient(GoSub) End

        }

        void eSend()
        {
            string strTemp = "";

            if (txtPano.Text == "" || txtName.Text == "")
            {
                return;
            }

            if (MessageBox.Show("환자정보를 전송 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            //전문 생성 ---------------------------------------------------------------------------
            strTemp = "";
            strTemp += txtPano.Text + "|";                                  //15필수
            strTemp += txtName.Text + "|";                                  //20필수
            strTemp += txtJumin.Text.Replace("-", "") + "|";                //13필수
            strTemp += VB.IIf(txtSex.Text == "", "N", txtSex.Text) + "|";   // 1필수
            strTemp += txtBirth.Text.Replace("-", "") + "|";                // 8필수
            strTemp += txtAge.Text + "|";

            //필수 항목만 우선 전송
            strTemp += txtJuso.Text + "|";                                  // 100
            strTemp += txtZipCode.Text + "|";                               //  7
            strTemp += txtTel.Text + "|";                                   // 15
            strTemp += txtHPhone.Text + "|";                                // 15
            //--------------------------------------------------------------------------------------

            //TODO : KoppUPdate_HNIP(strTemp, "PT");
        }

        void eGetData02()
        {
            int nREAD = 0;
            int i = 0;
            int j = 0;

            string strDeptCode = "";
            string strIO = "";
            string strJinDate = "";
            string strRowid = "";

            string strFDate = "";
            string strTDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            strFDate = dtpFdate.Text;
            strTDate = dtpTdate.Text;

            #region READ_OPDIPD(GoSub)            

            SQL = "CREATE OR REPLACE VIEW VIEW_SUNAP AS                                                                                                 ";
            SQL += ComNum.VBLF + "SELECT                                                                                                                ";
            SQL += ComNum.VBLF + "  '외래' GBN ,  A.BDATE INDATE , A.BDATE OUTDATE , 1 ILSU,   A.SEQNO NUM,  A.PANO,  A.DEPTCODE, A.DRCODE, A.BI,       ";
            SQL += ComNum.VBLF + "  B.DRNAME,  SUM(A.AMT1 + A.AMT2) AMT                                                                                 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_DOCTOR B                                             ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                             ";
            SQL += ComNum.VBLF + "      AND A.ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')                                                        ";
            SQL += ComNum.VBLF + "      AND A.ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')                                                        ";
            SQL += ComNum.VBLF + "      AND A.DRCODE = B.DRCODE                                                                                         ";
            SQL += ComNum.VBLF + "      AND A.PANO ='" + txtPano.Text + "'                                                                              ";
            SQL += ComNum.VBLF + "      AND A.BUN ='99'                                                                                                 ";
            SQL += ComNum.VBLF + "GROUP BY A.BDATE, A.SEQNO,  A.PANO,  A.DEPTCODE, A .DRCODE, A.BI, B.DRNAME                                            ";

            SQL += ComNum.VBLF + "UNION ALL                                                                                                             ";

            SQL += ComNum.VBLF + "SELECT                                                                                                                ";
            SQL += ComNum.VBLF + "  'ETC' GBN ,  A.BDATE INDATE , A.BDATE OUTDATE , 1 ILSU,   A.SEQNO NUM,  A.PANO, 'R6' DEPTCODE,                      ";
            SQL += ComNum.VBLF + "  '' DRCODE, A.BI, '외부의뢰' DRNAME,  SUM(A.AMT) AMT                                                                 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_ETCSLIP A                                                                              ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                             ";
            SQL += ComNum.VBLF + "      AND A.ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')                                                        ";
            SQL += ComNum.VBLF + "      AND A.ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')                                                        ";
            SQL += ComNum.VBLF + "      AND A.PANO ='" + txtPano.Text + "'                                                                              ";
            SQL += ComNum.VBLF + "      AND A.BUN ='99'                                                                                                 ";
            SQL += ComNum.VBLF + "GROUP BY A.BDATE, A.SEQNO,  A.PANO,   A.BI                                                                            ";

            SQL += ComNum.VBLF + "UNION ALL                                                                                                             ";

            SQL += ComNum.VBLF + "SELECT                                                                                                                ";
            SQL += ComNum.VBLF + "  '입원' GBN, A.INDATE, A.OUTDATE, A.ILSU,  A.TRSNO NUM ,  A.PANO, A.DEPTCODE, A.DRCODE ,A.BI, B.DRNAME ,  AMT55  AMT ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                             ";
            SQL += ComNum.VBLF + "      AND A.ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')                                                        ";
            SQL += ComNum.VBLF + "      AND A.ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')                                                        ";
            SQL += ComNum.VBLF + "      AND A.DRCODE = B.DRCODE                                                                                         ";
            SQL += ComNum.VBLF + "      AND A.PANO ='" + txtPano.Text + "'                                                                              ";
            SQL += ComNum.VBLF + "ORDER BY 2 DESC, 1                                                                                                    ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            try
            {
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBox("저장하였습니다.");
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

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  GBN, INDATE, OUTDATE, ILSU, NUM, PANO, DEPTCODE, DRCODE, BI, DRNAME, AMT    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "VIEW_SUNAP                                         ";

            if (optIO1.Checked == true)
            {
                SQL += ComNum.VBLF + "WHERE GBN IN ('외래','ETC')                                               ";
            }
            else if (optIO2.Checked == true)
            {
                SQL += ComNum.VBLF + "WHERE GBN ='입원'                                                         ";
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

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssList1_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Gbn"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ILSU"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["num"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 9].Text = String.Format("{0:#,###}", dt.Rows[i]["Amt"].ToString().Trim());

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

            #endregion READ_OPDIPD(GoSub) End


            #region READ_WONSELU(GoSub)

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  SEQNO, IPDOPD, DEPTCODE, BDATE, TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,      ";
            SQL += ComNum.VBLF + "  REMARK, ROWID                                                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_WONSELU                                        ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'                                       ";
            SQL += ComNum.VBLF + "      AND (SEQNO <> '' OR SEQNO IS NOT NULL)                                  ";
            SQL += ComNum.VBLF + "ORDER BY seqno DESC                                                           ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;

                    for (i = 0; i < nREAD; i++)
                    {
                        strDeptCode = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        strIO = dt.Rows[i]["IPDOPD"].ToString().Trim();
                        strJinDate = dt.Rows[i]["BDATE"].ToString().Trim();
                        strRowid = dt.Rows[i]["ROWID"].ToString().Trim();

                        ssList2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        ssList2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssList2_Sheet1.Cells[i, 3].Text = strIO;
                        ssList2_Sheet1.Cells[i, 4].Text = strDeptCode;
                        ssList2_Sheet1.Cells[i, 5].Text = strJinDate;
                        ssList2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssList2_Sheet1.Cells[i, 7].Text = strRowid;
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

            #endregion READ_WONSELU(GoSub) End
        }

        void eSendSlip()
        {
            string strTemp = "";

            //데이타 점검
            if (txtPano.Text == "" || txtName.Text == "")
            {
                return;
            }

            if (MessageBox.Show("진료비내역를 전송 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            else
            {
                SLIP_SEND();
            }
        }

        void SLIP_SEND()
        {
            string strBi = "";
            string strSeqNo = "";
            string strPano = "";
            string strJumin = "";
            string strSName = "";
            string strDept = "";
            string strDeptName = "";
            string strIO = "";
            string strRowid = "";
            string strBalDate = "";
            string strJinDate = "";
            string strSuSuDate = "";
            string strRemark = "";
            string strJuso = "";
            string strJinName = "";
            string strSuSuName = "";

            int i = 0;
            int j = 0;
            int nLen = 0;

            string strIlsu = "";
            string strGuBun = "";

            string DD = "";
            string strBunHo = "";
            string strDrName = "";
            string strPartTel = "";
            string strSend = "";
            string strTemp = "";
            string strJSDate = "";
            string strJEDate = "";
            string strTDate = "";
            string strNGT = "";

            int[,] nAmt = new int[24, 3];
            int[] nSAmt = new int[24];

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            for (i = 0; i <= 23; i++)
            {
                nAmt[i, 1] = 0;
                nAmt[i, 2] = 0;
            }
            for (i = 0; i <= 23; i++)
            {
                nSAmt[i] = 0;
            }

            strPano = txtPano.Text;
            strJumin = txtJumin.Text;
            strSName = txtName.Text;

            switch (FstrBI)
            {
                case "11":
                    strBi = "공단";
                    break;
                case "12":
                    strBi = "연합회";
                    break;
                case "13":
                    strBi = "지역";
                    break;
                case "21":
                    strBi = "보호1종";
                    break;
                case "22":
                    strBi = "보호2종";
                    break;
                case "23":
                    strBi = "의료부조";
                    break;
                case "24":
                    strBi = "행려환자";
                    break;
                case "31":
                    strBi = "산재";
                    break;
                case "32":
                    strBi = "공무원공상";
                    break;
                case "33":
                    strBi = "산재공상";
                    break;
                case "41":
                    strBi = "공단100%";
                    break;
                case "42":
                    strBi = "직장100%";
                    break;
                case "43":
                    strBi = "지역100%";
                    break;
                case "44":
                    strBi = "가족계획";
                    break;
                case "45":
                    strBi = "보험계약";
                    break;
                case "51":
                    strBi = "일반";
                    break;
                case "52":
                    strBi = "TA보험";
                    break;
                case "53":
                    strBi = "계약";
                    break;
                case "54":
                    strBi = "미확인";
                    break;
                case "55":
                    strBi = "TA일반";
                    break;
            }

            //진료과명
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                        ";
            SQL += ComNum.VBLF + "  DEPTNAMEK                                   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT     ";
            SQL += ComNum.VBLF + "WHERE DEPTCODE  = '" + FstrDept + "'          ";

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
                    strDeptName = dt.Rows[0]["DEPTNAMEK"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            strNGT = "D";

            //야간/공휴 정보 읽기, 처방내역에 gbngt 칼럼 읽기 행위료
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                        ";
            SQL += ComNum.VBLF + "  GBNGT                                                       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP                           ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "      AND PANO = '" + mstrPano + "'                           ";
            SQL += ComNum.VBLF + "      AND DEPTCODE = '" + FstrDept + "'                       ";
            SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + FstrBdate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "      AND SEQNO = '" + FstrNum + "'                           ";
            SQL += ComNum.VBLF + "      AND GBNGT <> 0                                          ";

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
                    switch (dt.Rows[0]["GBNGT"].ToString().Trim())
                    {
                        case "1":
                            strNGT = "H";
                            break;
                        case "2":
                            strNGT = "N";
                            break;
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

            for (i = 1; i < ssAmt_Sheet1.Rows.Count; i++)
            {
                nAmt[i, 1] = Convert.ToInt32(VB.Val(ssAmt_Sheet1.Cells[i, 1].Text));
                nAmt[i, 2] = Convert.ToInt32(VB.Val(ssAmt_Sheet1.Cells[i, 2].Text));
                nSAmt[i] = Convert.ToInt32(VB.Val(ssAmt_Sheet1.Cells[i, 5].Text));
            }

            //전문 생성 ---------------------------------------------------------------------------
            strTemp = "";
            strTemp = mstrPano + "|";                                               // 15 필수 '병록번호
            strTemp += CurrentDate.Replace("-", "");                                // 8  필수 '발행일자
            strTemp += FstrNum + "|";                                               // 30 필수 '문서연번호
            strTemp += VB.IIf(FstrIO == "입원", "1", "2") + "|";                    // 1       입원외래
            strTemp += FstrNum + "|";                                               // 30 필수 '영수증 번호
            strTemp += strBi + "|";                                                 // 50 필수 '환자보험구분
            strTemp += strDeptName + "|";                                           // 40 필수 '진료과명
            strTemp += FstrINDate.Replace("-", "") + "|";                           // 8 필수 '입원시작
            strTemp += FstrOUTDate.Replace("-", "") + "|";                          // 8 필수 '입원종료
            strTemp += strNGT + "|";                                                // 8 필수 '야간 공휴
            strTemp += "" + "|";                                                    // 10      '병동
            strTemp += "" + "|";                                                    // 50      '병실등급

            strTemp += nAmt[1, 1] + "|";                                            // 9      '진찰료(급여)
            strTemp += nAmt[1, 2] + "|";                                            // 9      '진찰료(비급여)
            strTemp += "0" + "|";                                                   // 9      '진찰료(선택진료)

            strTemp += nAmt[2, 1] + "|";                                            // 9      '입원료(급여)
            strTemp += nAmt[2, 2] + "|";                                            // 9      '입원료(비급여)
            strTemp += "0" + "|";                                                   // 9      '입원료(선택진료)

            strTemp += nAmt[17, 1] + "|";                                           // 9      '보철료(급여)
            strTemp += nAmt[17, 2] + "|";                                           // 9      '보철료(비급여)
            strTemp += "0" + "|";                                                   // 9      '보철료(선택진료)

            strTemp += nAmt[3, 1] + "|";                                            // 9      '식대(급여)
            strTemp += nAmt[3, 2] + "|";                                            // 9      '식대(비급여)
            strTemp += "0" + "|";                                                   // 9      '식대(선택진료)

            strTemp += nAmt[4, 1] + "|";                                            // 9      '투약 및 조제료(급여)
            strTemp += nAmt[4, 2] + "|";                                            // 9      '투약 및 조제료(비급여)
            strTemp += "0" + "|";                                                   // 9      '투약 및 조제료(선택진료)

            strTemp += nAmt[5, 1] + "|";                                            // 9      '주사료(급여)
            strTemp += nAmt[5, 2] + "|";                                            // 9      '주사료(비급여)
            strTemp += "0" + "|";                                                   // 9      '주사료(선택진료)

            strTemp += nAmt[6, 1] + "|";                                            // 9      '마취료(급여)
            strTemp += nAmt[6, 2] + "|";                                            // 9      '마취료(비급여)
            strTemp += "0" + "|";                                                   // 9      '마취료(선택진료)

            strTemp += nAmt[7, 1] + "|";                                            // 9      '처치 및 수술(급여)
            strTemp += nAmt[7, 2] + "|";                                            // 9      '처치 및 수술(비급여)
            strTemp += "0" + "|";                                                   // 9      '처치 및 수술(선택진료)

            strTemp += nAmt[8, 1] + "|";                                            // 9      '검사료(급여)
            strTemp += nAmt[8, 2] + "|";                                            // 9      '검사료(비급여)
            strTemp += "0" + "|";                                                   // 9      '검사료(선택진료)

            strTemp += nAmt[9, 1] + "|";                                            // 9      '영상 (급여)
            strTemp += nAmt[9, 2] + "|";                                            // 9      '영상(비급여)
            strTemp += "0" + "|";                                                   // 9      '식대(선택진료)

            strTemp += nAmt[10, 1] + "|";                                           // 9      '치료재료대(급여)
            strTemp += nAmt[10, 2] + "|";                                           // 9      '치료재료대(비급여)
            strTemp += "0" + "|";                                                   // 9      '치료재료대(선택진료)

            strTemp += nAmt[11, 1] + "|";                                           // 9      '전액본인부담(급여)
            strTemp += nAmt[11, 2] + "|";                                           // 9      '전액본인부담(비급여)
            strTemp += "0" + "|";                                                   // 9      '전액본인부담(선택진료)

            strTemp += nAmt[12, 1] + "|";                                           // 9      '이학요법(급여)
            strTemp += nAmt[12, 2] + "|";                                           // 9      '이학요법(비급여)
            strTemp += "0" + "|";                                                   // 9      '이학요법(선택진료)

            strTemp += nAmt[13, 1] + "|";                                           // 9      '정신요법(급여)
            strTemp += nAmt[13, 2] + "|";                                           // 9      '정신요법(비급여)
            strTemp += "0" + "|";                                                   // 9      '정신요법(선택진료)

            strTemp += nAmt[14, 1] + "|";                                           // 9      'ct(급여)
            strTemp += nAmt[14, 2] + "|";                                           // 9      'ct(비급여)
            strTemp += "0" + "|";                                                   // 9      'ct(선택진료)

            strTemp += nAmt[15, 1] + "|";                                           // 9      'mri(급여)
            strTemp += nAmt[15, 2] + "|";                                           // 9      'mri(비급여)
            strTemp += "0" + "|";                                                   // 9      'mri(선택진료)

            strTemp += nAmt[16, 1] + "|";                                           // 9      '초음파(급여)
            strTemp += nAmt[16, 2] + "|";                                           // 9      '초음파(비급여)
            strTemp += "0" + "|";                                                   // 9      '초음파(선택진료)

            strTemp += nAmt[18, 1] + "|";                                           // 9      '수혈(급여)
            strTemp += nAmt[18, 2] + "|";                                           // 9      '수혈(비급여)
            strTemp += "0" + "|";                                                   // 9      '수혈(선택진료)

            strTemp += nAmt[22, 1] + "|";                                           // 9      '기타(급여)
            strTemp += nAmt[22, 2] + "|";                                           // 9      '기타(비급여)
            strTemp += "0" + "|";                                                   // 9      '기타(선택진료)

            strTemp += nSAmt[3] + "|";                                              // 9      '본인부담금(급여)
            strTemp += "0" + "|";                                                   // 9      '본인부담금(비급여)
            strTemp += "0" + "|";                                                   // 9      '본인부담금(선택진료)

            strTemp += nSAmt[2] + "|";                                              // 9      '보험자부담금(급여)
            strTemp += "0" + "|";                                                   // 9      '보험자부담금(비급여)
            strTemp += "0" + "|";                                                   // 9      '보덤자부담금(선택진료)

            strTemp += nSAmt[1] + "|";                                              // 9      '진료비총금액

            strTemp += nSAmt[3] + "|";                                              // 9      '환자부담총금액
            strTemp += nSAmt[12] + "|";                                             // 9      '수납금액

            strTemp += "" + "|";                                                    // 9      '수납일시
            strTemp += FstrINDate.Replace("-", "") + "|";                           // 9      '입원일자

            strTemp += "" + "|";                                                    // 50      '환자유형

            strTemp += "" + "|";                                                    // 9      '유형보조
            strTemp += "" + "|";                                                    // 50     '통원시작일
            strTemp += "" + "|";                                                    // 9      '통원종료일

            strTemp += nAmt[20, 1] + "|";                                           // 9      '증명료(급여)
            strTemp += nAmt[20, 2] + "|";                                           // 9      '증명료(비급여)
            strTemp += "0" + "|";                                                   // 9      '증명료(선택진료)

            strTemp += VB.IIf(FstrIO == "입원", "0", "") + "|";                     // 9      수납구분

            strTemp += "" + "|";                                                    // 9      수납납자

            strTemp += nSAmt[4] + "|";                                              // 9      중간납부액

            strTemp += nSAmt[5] + "|";                                              // 9      감액
            strTemp += nSAmt[6] + "|";                                              // 9      미수금액

            strTemp += "37100068" + "|";                                            // 1     의료기관기호 
            strTemp += "포항성모병원" + "|";                                        // 1     의료기관명

            strTemp += "790-825" + "|";                                             // 1     의료기관우편번호
            strTemp += "경북 포항시 남구 대잠동 270-1" + "|";                       // 1     의료기관주소
            strTemp += "(054)272-0151" + "|";                                       // 1     의료기관전화
            strTemp += "" + "|";                                                    // 1     의료기관팩스

            strTemp += "5068200896" + "|";                                          // 1     사업자등록번호

            strTemp += "손경옥" + "|";                                              // 1     병원장

            strTemp += "" + "|";                                                    // 1     진료형태

            strTemp += "0" + "|";                                                   // 9      '응급관리료(급여)
            strTemp += "0" + "|";                                                   // 9      '응급관리료(비급여)
            strTemp += "0" + "|";                                                   // 9       '응급관리료(선택진료)

            strTemp += "0" + "|";                                                   // 9      '행위가산(급여)
            strTemp += "0" + "|";                                                   // 9      '행위가산(비급여)
            strTemp += "0" + "|";                                                   // 9       '행위가산(선택진료)

            strTemp += nAmt[20, 1] + "|";                                           // 9      '병실차액(급여)
            strTemp += nAmt[20, 2] + "|";                                           // 9      '병실차액(비급여)
            strTemp += "0" + "|";                                                   // 9       '응급관리료(선택진료)

            strTemp += "" + "|";                                                    // 9      'tv수신료

            strTemp += "" + "|";                                                    // 9      '전화료

            strTemp += nAmt[19, 1] + nAmt[19, 2] + "|";                             // 9      '예약진찰료

            strTemp += nAmt[19, 1] + nAmt[19, 2] + "|";                             // 9      '재증명료
            strTemp += nSAmt[5] + "|";                                              // 9      '할인액

            strTemp += "" + "|";                                                    // 9      '기타사항

            //TODO : KoppUPdate_HNIP(strTemp, "RC")
        }

        void eSenddtl()
        {
            string strTemp = "";

            //데이타 점검

            if (txtPano.Text == "" || txtName.Text == "")
            {
                return;
            }

            if (MessageBox.Show("진료사실증명서를 전송 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            else
            {
                WONSELU_SEND();
            }
        }

        void WONSELU_SEND()
        {
            string strSeqNo;
            string strPano = "";
            string strJumin = "";
            string strSName = "";
            string strDept = "";

            string strDeptName = "";
            string strIO = "";
            string strRowid = "";
            string strBalDate = "";
            string strJinDate = "";
            string strSuSulDate = "";
            string strRemark = "";
            string strJuso = "";
            string strJinName = "";
            string strSuSulName = "";

            int i = 0;
            int j = 0;
            int nLen = 0;

            string strIlsu = "";
            string strGuBun = "";

            string DD = "";
            string strBunHo = "";
            string strDrName = "";

            string strPartTel = "";

            string strSend = "";

            string strTemp = "";

            string strJSDate = "";
            string strJEDate = "";

            string strTDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strPano = txtPano.Text;
            strJumin = txtJumin.Text;
            strSName = txtName.Text;

            for (i = 1; i < ssList2_Sheet1.Rows.Count; i++)
            {
                strSend = ssList2_Sheet1.Cells[i, 0].Text;
                strBalDate = ssList2_Sheet1.Cells[i, 1].Text.Replace("-", "");
                strSeqNo = ssList2_Sheet1.Cells[i, 2].Text;
                strIO = ssList2_Sheet1.Cells[i, 3].Text;
                strDept = ssList2_Sheet1.Cells[i, 4].Text;
                strJinDate = ssList2_Sheet1.Cells[i, 5].Text;
                strRemark = ssList2_Sheet1.Cells[i, 6].Text;
                strRowid = ssList2_Sheet1.Cells[i, 7].Text;

                //진료과명
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                    ";
                SQL += ComNum.VBLF + "  DEPTNAMEK                               ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL += ComNum.VBLF + "WHERE DEPTCODE  = '" + strDept + "'       ";

                try
                {
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    //if (dt.Rows.Count == 0)
                    //{
                    //    dt.Dispose();
                    //    dt = null;
                    //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    //    return;
                    //}

                    if (dt.Rows.Count > 0)
                    {
                        strDeptName = dt.Rows[0]["DEPTNAMEK"].ToString().Trim();
                    }
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
                dt.Dispose();
                dt = null;

                //주소
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                        ";
                SQL += ComNum.VBLF + "  B.MAILJUSO, A.JUSO                                                          ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "BAS_MAILNEW B ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                SQL += ComNum.VBLF + "      AND A.PANO  = '" + strPano + "'                                         ";
                SQL += ComNum.VBLF + "      AND A.ZIPCODE1||A.ZIPCODE2 = B.MAILCODE                                 ";

                try
                {
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    //if (dt.Rows.Count == 0)
                    //{
                    //    dt.Dispose();
                    //    dt = null;
                    //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    //    return;
                    //}

                    if (dt.Rows.Count > 0)
                    {
                        strJuso = dt.Rows[0]["MAILJUSO"].ToString().Trim() + " " + dt.Rows[0]["JUSO"].ToString().Trim();
                    }
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
                dt.Dispose();
                dt = null;

                //진단명 및 수술명
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                                         ";
                SQL += ComNum.VBLF + "  JINDAN, SUSUL, GUBUN, DRCODE, IPDOPD, BDATE ,                                                ";
                SQL += ComNum.VBLF + "  TO_CHAR(SUSULDATE, 'YYYY-MM-DD') SUSULDATE1, TO_CHAR(SUSULDATE2, 'YYYY-MM-DD') SUSULDATE2,   ";
                SQL += ComNum.VBLF + "  TO_CHAR(SUSULDATE3, 'YYYY-MM-DD') SUSULDATE3, TO_CHAR(SUSULDATE4, 'YYYY-MM-DD') SUSULDATE4   ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_WONSELU                                                         ";
                SQL += ComNum.VBLF + "WHERE ROWID  = '" + strRowid + "'                                                              ";

                try
                {
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    //if (dt.Rows.Count == 0)
                    //{
                    //    dt.Dispose();
                    //    dt = null;
                    //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    //    return;
                    //}

                    if (dt.Rows.Count > 0)
                    {
                        strJinName = dt.Rows[0]["JINDAN"].ToString().Trim();
                        strSuSulName = dt.Rows[0]["SUSUL"].ToString().Trim();
                        strGuBun = "";

                        switch (dt.Rows[0]["GUBUN"].ToString().Trim())
                        {
                            case "1":
                                strGuBun = "1";
                                break;
                            case "2":
                                strGuBun = "0";
                                break;
                            default:
                                strGuBun = "";
                                break;
                        }

                        switch (dt.Rows[0]["IPDOPD"].ToString().Trim())
                        {
                            case "1":
                                strJSDate = VB.Left(dt.Rows[0]["BDATE"].ToString().Trim(), 10);
                                strJEDate = VB.Mid(dt.Rows[0]["BDATE"].ToString().Trim(), 12, 10);
                                break;

                            case "0":
                                strJSDate = VB.Left(dt.Rows[0]["BDATE"].ToString().Trim(), 10);
                                strJEDate = VB.Right(dt.Rows[0]["BDATE"].ToString().Trim(), 10);
                                break;
                        }

                        strJSDate = strJSDate.Replace("-", "");
                        strJEDate = strJEDate.Replace("-", "");

                        if (String.Compare(strJSDate, strJEDate) > 0)
                        {
                            strTDate = strJSDate;
                            strJSDate = strJEDate;
                            strJEDate = strTDate;
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
                SQL += ComNum.VBLF + "SELECT                                                                        ";
                SQL += ComNum.VBLF + "  a.DRBUNHO, a.DRNAME,b.DeptCode                                              ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR A, " + ComNum.DB_PMPA + "ETC_WONSELU B   ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                SQL += ComNum.VBLF + "      AND B.ROWID = '" + strRowid + "'                                        ";
                SQL += ComNum.VBLF + "      AND A.SABUN = B.DRCODE                                                  ";

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
                        strBunHo = dt.Rows[0]["DRBUNHO"].ToString().Trim();
                        strDrName = dt.Rows[0]["DRNAME"].ToString().Trim();
                        strPartTel = READ_Dept_Tel(dt.Rows[0]["DeptCode"].ToString().Trim());
                        if (dt.Rows[0]["DeptCode"].ToString().Trim() == "MD" && strBunHo == "51177")
                        {
                            strPartTel = "054-289-4210";
                        }
                        else
                        {
                            strBunHo = "";
                            strDrName = "";
                            strPartTel = "";
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

                if (strSend == "1")
                {
                    if (strIO == "I")
                    {
                        //전문 생성 ---------------------------------------------------------------------------
                        strTemp = "";                                                       // 상단 전문 Example 값 할당
                        strTemp += "|";                                                     // 15 필수 '병록번호
                        strTemp += strBalDate + "|";                                        // 8  필수 '발행일자
                        strTemp += strSeqNo + "|";                                          // 20 필수 '문서연번호
                        strTemp += strJinName + "|";                                        // 500     '병명
                        strTemp += "N" + " | ";                                             // 50      '질병분류기호

                        strTemp += strDeptName + "|";                                       // 40      '통원진료과

                        //진료일자 로직 수정 해야함...??
                        strTemp += strJSDate + "|";                                         // 8  필수  '입원일
                        strTemp += strJEDate + "|";                                         // 8  필수  '퇴원일

                        strTemp += strJinDate + "|";                                        // 1000     '비고

                        strTemp += strBunHo + "|";                                          // 10       '의사면허번호
                        strTemp += strDrName + "|";                                         // 20       '의사성명

                        strTemp += "|";                                                     // 20       '발행자명
                        strTemp += strSName + "|";                                          // 20  필수 '성명

                        strTemp += strJumin.Replace("-", "") + "|";                         // 13  필수 '주민등록번호
                        strTemp += VB.IIf(txtSex.Text == "", "N", txtSex.Text) + "|";       // 1  필수  '성별
                        strTemp += txtBirth.Text.Replace("-", "") + "|";                    // 8  필수   생년월일
                        strTemp += txtAge.Text + "|";                                       // 3  필수   연령

                        strTemp += "37100068" + "|";                                        // 1     의료기관기호
                        strTemp += "포항성모병원" + "|";                                    // 1     의료기관명

                        strTemp += "790-825" + "|";                                         // 1     의료기관우편번호
                        strTemp += "경북 포항시 남구 대잠동 270-1" + "|";                   // 1     의료기관주소
                        strTemp += "(054)272-0151" + "|";                                   // 1     의료기관전화
                        strTemp += "" + "|";                                                // 1     의료기관팩스

                        //TODO : KoppUPdate_HNIP(strTemp, "IN")
                    }

                    else
                    {
                        //전문 생성 ---------------------------------------------------------------------------
                        strTemp = "";                                                       // 상단 전문 Example 값 할당
                        strTemp += "|";                                                     // 15 필수 '병록번호
                        strTemp += strBalDate + "|";                                        // 8  필수 '발행일자
                        strTemp += strSeqNo + "|";                                          // 20 필수 '문서연번호
                        strTemp += strJinName + "|";                                        // 500     '병명
                        strTemp += "N" + " | ";                                             // 50      '질병분류기호

                        strTemp += strDeptName + "|";                                       // 40      '통원진료과

                        //진료일자 로직 수정 해야함...??
                        strTemp += "|";                                                     // 8  필수  '입원일
                        strTemp += "|";                                                     // 8  필수  '퇴원일

                        strTemp += "" + "|";                                                // 1        '일수

                        strTemp += "|";                                                     // 20       '발행자명
                        strTemp += strSName + "|";                                          // 20  필수 '성명

                        strTemp += strJumin.Replace("-", "") + "|";                         // 13  필수 '주민등록번호
                        strTemp += VB.IIf(txtSex.Text == "", "N", txtSex.Text) + "|";       // 1  필수  '성별
                        strTemp += txtBirth.Text.Replace("-", "") + "|";                    // 8  필수   생년월일
                        strTemp += txtAge.Text + "|";                                       // 3  필수   연령

                        strTemp += txtJuso.Text + "|";                                      // 100    주소
                        strTemp += txtTel.Text + "|";                                       // 15     전화번호
                        strTemp += "|";                                                     // 1     병명구분

                        strTemp += "37100068" + "|";                                        // 1     의료기관기호
                        strTemp += "포항성모병원" + "|";                                    // 1     의료기관명

                        strTemp += "790-825" + "|";                                         // 1     의료기관우편번호
                        strTemp += "경북 포항시 남구 대잠동 270-1" + "|";                   // 1     의료기관주소
                        strTemp += "(054)272-0151" + "|";                                   // 1     의료기관전화
                        strTemp += "" + "|";                                                // 1     의료기관팩스
                        strTemp += strDeptName + " " + strJinDate + "|";                    // 1000     '비고

                        //TODO : KoppUPdate_HNIP(strTemp, "OT")
                    }
                }

            }


            //strJumin = txtJumin.Text;
            //strSName = txtName.Text;
            //if (strPano == "")
            //{
            //    return;
            //}

            //nLen = VB.Len(strSName);

            //switch (nLen)
            //{
            //    case 2:
            //        strSName = VB.Left(strSName, 1) + " " + VB.Right(strSName, 1);
            //        break;

            //    case 3:
            //        strSName = VB.Left(strSName, 1) + " " + VB.Mid(strSName, 2, 1) + " " + VB.Right(strSName, 1);
            //        break;

            //    case 4:
            //        strSName = VB.Left(strSName, 1) + " " + VB.Mid(strSName, 2, 1) + " " + VB.Mid(strSName, 3, 1) + " " + VB.Right(strSName, 1);
            //        break;
            //}

            //ssList3_Sheet1.Cells[2, 2].Text = strPano;          //등록번호
            //ssList3_Sheet1.Cells[2, 3].Text = strSeqNo;         //연번호
            //ssList3_Sheet1.Cells[2, 5].Text = strSName;         //환자명
            //ssList3_Sheet1.Cells[2, 6].Text = " " + strJuso;    //주소
            //ssList3_Sheet1.Cells[2, 10].Text = " " + strJinName;  
            //ssList3_Sheet1.Cells[2, 11].Text = " " + strSuSulName;  
            //ssList3_Sheet1.Cells[2, 12].Text = " " + strRemark;

            //ssList3_Sheet1.Cells[5, 5].Text = strJumin;

            //ssList3_Sheet1.Cells[3, 7].Text = strDeptName + strGuBun;

            //if(strIO == ")")
            //{
            //    ssList3_Sheet1.Cells[8, 3].Text = "□ 입  원       ■ 외  래";
            //    ssList3_Sheet1.Cells[9, 3].Text = strJinDate;

            //}
            //else
            //{
            //    ssList3_Sheet1.Cells[8, 3].Text = "■ 입  원       □ 외  래";
            //    ssList3_Sheet1.Cells[9, 3].Text = strJinDate;                
            //}

            //ssList3_Sheet1.Cells[20, 1].Text = "                면허  번호 : " + strBunHo + "            의사성명 : " + strDrName;
            //ssList3_Sheet1.Cells[16, 1].Text = "                발  행  일 : " + VB.Left(CurrentDate, 4) + "년 " + VB.Mid(CurrentDate, 6, 2) + "월 " + VB.Right(CurrentDate, 2) + "일";

            ////과전화번호 - 없으면 진단서 창구로 세팅
            //if(strPartTel != "")
            //{
            //    ssList3_Sheet1.Cells[19, 4].Text = strPartTel;
            //}
            //else
            //{
            //    ssList3_Sheet1.Cells[19, 4].Text = "054-289-4767";   //기본진단서 창구
            //    ComFunc.MsgBox("과 전화번호설정을 확인하세요..");
            //}
        }

        public string READ_Dept_Tel(string ArgCode)
        {
            string rtnVal = "";

            if (ArgCode == "")
            {
                return rtnVal;
            }

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                            ";
            SQL += ComNum.VBLF + "  Code,Name,Part,ROWID                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE              ";
            SQL += ComNum.VBLF + "WHERE 1=1                                         ";
            SQL += ComNum.VBLF + "      AND Gubun  = '진료사실증명서_전화번호'      ";
            SQL += ComNum.VBLF + "      AND CODE ='" + ArgCode + "'                 ";

            try
            {

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["Name"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;


            return rtnVal;
        }

        void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
            {
                return;
            }

            else
            {
                if (VB.Val(txtPano.Text) == 0)
                {
                    return;
                }

                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

                Screen_Clear();

                eGetData02();
                eGetData();
            }
        }

        void ssList2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strSeqNo;
            string strPano = "";
            string strJumin = "";
            string strSName = "";
            string strDept = "";
            string strDeptName = "";
            string strIO = "";
            string strRowid = "";
            string strJinDate = "";
            string strSuSulDate = "";
            string strRemark = "";
            string strJuso = "";
            string strJinName = "";
            string strSuSulName = "";

            int i = 0;
            int j = 0;
            int nLen = 0;

            string strIlsu = "";
            string strGuBun = "";

            string DD = "";
            string strBunHo = "";
            string strDrName = "";

            string strPartTel = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strPano = txtPano.Text;
            strJumin = txtJumin.Text;
            strSName = txtName.Text;

            if (strPano == "")
            {
                return;
            }

            nLen = VB.Len(strSName);

            switch (nLen)
            {
                case 2:
                    strSName = VB.Left(strSName, 1) + " " + VB.Right(strSName, 1);
                    break;
                case 3:
                    strSName = VB.Left(strSName, 1) + " " + VB.Mid(strSName, 2, 1) + " " + VB.Right(strSName, 1);
                    break;
                case 4:
                    strSName = VB.Left(strSName, 1) + " " + VB.Mid(strSName, 2, 1) + VB.Mid(strSName, 3, 1) + " " + VB.Right(strSName, 1);
                    break;
            }

            strSeqNo = ssList2_Sheet1.Cells[e.Row, 2].Text;
            strIO = ssList2_Sheet1.Cells[e.Row, 3].Text;
            strDept = ssList2_Sheet1.Cells[e.Row, 4].Text;
            strJinDate = ssList2_Sheet1.Cells[e.Row, 5].Text;
            strRemark = ssList2_Sheet1.Cells[e.Row, 6].Text;
            strRowid = ssList2_Sheet1.Cells[e.Row, 7].Text;

            //진료과명
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                    ";
            SQL += ComNum.VBLF + "  DEPTNAMEK                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
            SQL += ComNum.VBLF + "WHERE DEPTCODE  = '" + strDept + "'       ";

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
                    strDeptName = dt.Rows[0]["DEPTNAMEK"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            //주소
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  B.MAILJUSO, A.JUSO                                                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "BAS_MAILNEW B ";
            SQL += ComNum.VBLF + "WHERE A.PANO  = '" + strPano + "'                                             ";
            SQL += ComNum.VBLF + "      AND A.ZIPCODE1||A.ZIPCODE2 = B.MAILCODE                                 ";

            try
            {

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    strJuso = dt.Rows[0]["MAILJUSO"].ToString().Trim() + " " + dt.Rows[0]["JUSO"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            //진단명 및 수술명
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                         ";
            SQL += ComNum.VBLF + "  JINDAN, SUSUL, GUBUN, DRCODE,                                                                ";
            SQL += ComNum.VBLF + "  TO_CHAR(SUSULDATE, 'YYYY-MM-DD') SUSULDATE1, TO_CHAR(SUSULDATE2, 'YYYY-MM-DD') SUSULDATE2,   ";
            SQL += ComNum.VBLF + "  TO_CHAR(SUSULDATE3, 'YYYY-MM-DD') SUSULDATE3, TO_CHAR(SUSULDATE4, 'YYYY-MM-DD') SUSULDATE4   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_WONSELU                                                         ";
            SQL += ComNum.VBLF + "WHERE ROWID = '" + strRowid + "'                                                               ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    strJinName = dt.Rows[0]["JINDAN"].ToString().Trim();
                    strSuSulName = dt.Rows[0]["SUSUL"].ToString().Trim();
                    strGuBun = "";

                    switch (dt.Rows[0]["GUBUN"].ToString().Trim())
                    {
                        case "1":
                            strGuBun = " (퇴원)";
                            break;
                        case "2":
                            strGuBun = " (가료중)";
                            break;
                        default:
                            strGuBun = "";
                            break;
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
            SQL += ComNum.VBLF + "SELECT                                                                                         ";
            SQL += ComNum.VBLF + "  a.DRBUNHO, a.DRNAME,b.DeptCode                                                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR A, " + ComNum.DB_PMPA + "ETC_WONSELU B                    ";
            SQL += ComNum.VBLF + "WHERE B.ROWID = '" + strRowid + "'                                                             ";
            SQL += ComNum.VBLF + "      AND A.SABUN = B.DRCODE                                                                   ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    strBunHo = dt.Rows[0]["DRBUNHO"].ToString().Trim();
                    strDrName = dt.Rows[0]["DRNAME"].ToString().Trim();
                    strPartTel = READ_Dept_Tel(dt.Rows[0]["DeptCode"].ToString().Trim());

                    if (dt.Rows[0]["DRNAME"].ToString().Trim() == "MD" && strBunHo == "51177")
                    {
                        strPartTel = "054-289-4210";
                    }
                    else
                    {
                        strBunHo = "";
                        strDrName = "";
                        strPartTel = "";
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

            ssList3_Sheet1.Cells[2, 2].Text = strPartTel;    //등록번호
            ssList3_Sheet1.Cells[3, 2].Text = strSeqNo;      //연번호
            ssList3_Sheet1.Cells[5, 2].Text = strSName;      //환자명
            ssList3_Sheet1.Cells[6, 2].Text = " " + strJuso; //주소
            ssList3_Sheet1.Cells[10, 2].Text = " " + strJinName;
            ssList3_Sheet1.Cells[11, 2].Text = " " + strSuSulName;
            ssList3_Sheet1.Cells[12, 2].Text = " " + strRemark;

            ssList3_Sheet1.Cells[5, 5].Text = strJumin;
            ssList3_Sheet1.Cells[7, 3].Text = strDeptName + strGuBun;

            if (strIO == "O")
            {
                ssList3_Sheet1.Cells[8, 3].Text = "□ 입  원       ■ 외  래";
                ssList3_Sheet1.Cells[9, 3].Text = strJinDate;
            }
            else
            {
                ssList3_Sheet1.Cells[8, 3].Text = "■ 입  원       □ 외  래";
                ssList3_Sheet1.Cells[9, 3].Text = strJinDate;
            }

            ssList3_Sheet1.Cells[20, 1].Text = "                면허  번호 : " + strBunHo + "            의사성명 : " + strDrName;
            ssList3_Sheet1.Cells[16, 1].Text = "                발  행  일 : " + VB.Left(CurrentDate, 4) + "년 " + VB.Mid(CurrentDate, 6, 2) + "월 " + VB.Right(CurrentDate, 2) + "일";
            //과전화번호 - 없으면 진단서 창구로 세팅

            if (strPartTel != "")
            {
                ssList3_Sheet1.Cells[19, 4].Text = strPartTel;
            }
            else
            {
                ssList3_Sheet1.Cells[19, 4].Text = "054-289-4767";  //기본진단서 창구
                ComFunc.MsgBox("과 전화번호설정을 확인하세요..");
            }


        }

        void ssList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            for (int i = 0; i < ssAmt_Sheet1.Rows.Count; i++)
            {
                for (int j = 1; j <= 2; j++)
                {
                    ssAmt_Sheet1.Cells[i, j].Text = "";
                }
            }

            for (int i = 0; i < ssAmt_Sheet1.Rows.Count; i++)
            {
                ssAmt_Sheet1.Cells[i, 5].Text = "";
            }

            mstrPano = txtPano.Text;

            FstrIO = ssList1_Sheet1.Cells[e.Row, 1].Text;
            FstrBdate = VB.Left(ssList1_Sheet1.Cells[e.Row, 2].Text, 10);
            FstrINDate = VB.Left(ssList1_Sheet1.Cells[e.Row, 2].Text, 10);
            FstrOUTDate = VB.Left(ssList1_Sheet1.Cells[e.Row, 3].Text, 10);
            FstrDept = ssList1_Sheet1.Cells[e.Row, 5].Text;
            FstrBI = ssList1_Sheet1.Cells[e.Row, 7].Text;
            FstrNum = ssList1_Sheet1.Cells[e.Row, 8].Text;

            if (FstrIO == "입원")
            {
                IPD_TRANS_Amt_Display_NEW(ssAmt_Sheet1, 0, VB.Val(FstrNum));
            }
            else
            {
                Report_Print_Sunap_Sign_new(mstrPano, FstrDept, "", "", FstrNum, "", "", FstrBI, FstrBdate, "", "", FstrDept, "", FstrIO);
            }
        }

        //'-----------------------------------------------------------------------------------------------
        //'   IPD_TRANS을 읽어 금액을 SSAmt Sheet에 표시함
        //'   ex: Call IPD_TRANS_AMT_Display(SSAmt,0,12324) -> IPD_TRANS TRSNO=12324의 금액을 표시
        //'       Call IPD_TRANS_AMT_Display(SSAmt,12345,0) -> IPD_TRANS IPDNO=12345 합계 금액을 표시
        //'------------------------------------------------------------------------------------------------
        void IPD_TRANS_Amt_Display_NEW(FarPoint.Win.Spread.SheetView Spd, int ArgIPDNO, double ArgTRSNO)
        {
            int i = 0;
            int j = 0;
            int nREAD = 0;

            int[] nAmt = new int[61];
            int nTot1 = 0;
            int nTot2 = 0;
            int nIpdno1 = 0;
            int nAmtSang = 0;

            string strGbSTS = "";
            string strBi = "";
            string strOBPDBun = "";
            string strOBPDBundtl = "";
            int[,] RAmt = new int[41, 3];

            int AMT61 = 0;
            int AMT62 = 0;
            int AMT63 = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //누적할 배열 변수를 Clear
            for (i = 0; i < nAmt.Length; i++)
            {
                nAmt[i] = 0;
            }
            nAmtSang = 0;
            AMT61 = 0;
            AMT62 = 0;
            AMT63 = 0;

            //누적별 금액을 합산함
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                             ";
            SQL += ComNum.VBLF + "  Bi,TO_CHAR(InDate,'YYYY-MM-DD') InDate, IPDNO, GBSTS, OGPDBUN,OGPDBUNdtl,        ";
            SQL += ComNum.VBLF + "  Amt01,Amt02,Amt03,Amt04,Amt05,Amt06,Amt07,Amt08,Amt09,Amt10,                     ";
            SQL += ComNum.VBLF + "  Amt11,Amt12,Amt13,Amt14,Amt15,Amt16,Amt17,Amt18,Amt19,Amt20,                     ";
            SQL += ComNum.VBLF + "  Amt21,Amt22,Amt23,Amt24,Amt25,Amt26,Amt27,Amt28,Amt29,Amt30,                     ";
            SQL += ComNum.VBLF + "  Amt31,Amt32,Amt33,Amt34,Amt35,Amt36,Amt37,Amt38,Amt39,Amt40,                     ";
            SQL += ComNum.VBLF + "  Amt41,Amt42,Amt43,Amt44,Amt45,Amt46,Amt47,Amt48,Amt49,Amt50,                     ";
            SQL += ComNum.VBLF + "  Amt51,Amt52,Amt53,Amt54,Amt55,Amt56,Amt57,Amt58,Amt59,Amt60,                     ";
            SQL += ComNum.VBLF + "  AMT61,AMT62,AMT63,SANGAMT                                                        ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS                                               ";
            if (ArgTRSNO == 0)
            {
                SQL += ComNum.VBLF + "WHERE IPDNO = " + ArgIPDNO + "                                                 ";
            }
            else
            {
                SQL += ComNum.VBLF + "WHERE TrsNo = " + ArgTRSNO + "                                                 ";
            }
            SQL += ComNum.VBLF + "ORDER BY InDate,Bi                                                                 ";

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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    for (j = 0; j < nAmt.Length; j++)
                    {
                        nAmt[j] += Convert.ToInt32(VB.Val(dt.Rows[i]["Amt"].ToString().Trim() + ComFunc.SetAutoZero(j.ToString(), 2)));
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    nIpdno1 = Convert.ToInt32(VB.Val(dt.Rows[0]["IPDNO"].ToString().Trim()));
                    strGbSTS = dt.Rows[0]["GBSTS"].ToString().Trim();
                    nAmtSang = Convert.ToInt32(VB.Val(dt.Rows[0]["SANGAMT"].ToString().Trim()));
                    strBi = dt.Rows[0]["BI"].ToString().Trim();

                    AMT61 = Convert.ToInt32(VB.Val(dt.Rows[0]["AMT61"].ToString().Trim()));
                    AMT62 = Convert.ToInt32(VB.Val(dt.Rows[0]["AMT62"].ToString().Trim()));
                    AMT63 = Convert.ToInt32(VB.Val(dt.Rows[0]["AMT63"].ToString().Trim()));

                    strOBPDBun = dt.Rows[0]["OGPDBUN"].ToString().Trim();
                    strOBPDBundtl = dt.Rows[0]["OGPDBUNdtl"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            //'-----------------------------------------------------------
            //'      영수증 발행용 금액을 계산함
            //'-----------------------------------------------------------
            for (i = 1; i < RAmt.Length; i++)
            {
                RAmt[1, 2] = 0;
            }

            //1.진찰료
            RAmt[1, 1] = nAmt[1];

            //2.입원료
            RAmt[2, 1] = nAmt[2] + nAmt[3];
            RAmt[2, 2] = nAmt[21];

            //3.식대
            RAmt[3, 1] = nAmt[16];
            RAmt[3, 2] = nAmt[34];

            //4.투약 및 조제료
            RAmt[4, 1] = nAmt[4];
            RAmt[4, 2] = nAmt[22];

            //5.주사료
            RAmt[5, 1] = nAmt[5];
            RAmt[5, 2] = nAmt[23];

            //6.마취료
            RAmt[6, 1] = nAmt[6];
            RAmt[6, 2] = nAmt[24];

            //7.처치 및 수술료
            RAmt[7, 1] = nAmt[9] + nAmt[10] + nAmt[12];
            RAmt[7, 2] = nAmt[27] + nAmt[28] + nAmt[30];

            //8.검사료
            RAmt[8, 1] = nAmt[13] + nAmt[14];
            RAmt[8, 2] = nAmt[31] + nAmt[32];

            //9.방사선료
            RAmt[9, 1] = nAmt[15];
            RAmt[9, 2] = nAmt[33];

            //10.치료재료대(아래에서 별도 계산함)
            RAmt[10, 1] = 0;
            RAmt[10, 2] = 0;

            //11.전액본인부담
            RAmt[11, 1] = 0;
            RAmt[11, 2] = 0;

            //12.재활 및 물리치료
            RAmt[12, 1] = nAmt[7];
            RAmt[12, 2] = nAmt[25];

            //13.정신요법료
            RAmt[13, 1] = nAmt[8];
            RAmt[13, 2] = nAmt[26];

            //14.CT
            RAmt[14, 1] = nAmt[19];
            RAmt[14, 2] = nAmt[37];

            //15.MRI
            RAmt[15, 1] = nAmt[18];
            RAmt[15, 2] = nAmt[38];

            //TA환자는 초음파 보험회사에 청구함. 2006-01-06
            if (strBi == "52")
            {
                RAmt[16, 1] = nAmt[36];
                RAmt[16, 2] = 0;

                //2006 - 10 - 31 박시철 팀장 요청..
                RAmt[17, 1] = 0;
                RAmt[17, 2] = nAmt[40];
            }
            else
            {
                //16.초음파
                RAmt[16, 1] = 0;
                RAmt[16, 2] = nAmt[36];

                //17.보철.교정료
                RAmt[17, 1] = 0;
                RAmt[17, 2] = nAmt[40];
            }

            //18.수혈료
            RAmt[18, 1] = nAmt[11];
            RAmt[18, 2] = nAmt[29];

            //19.예약진찰료
            RAmt[19, 1] = 0;
            RAmt[19, 2] = 0;

            //20.증명료
            RAmt[20, 1] = 0;
            RAmt[20, 2] = nAmt[47];

            //21.병실차액
            RAmt[21, 1] = 0;
            RAmt[21, 2] = nAmt[35];

            //22.기타
            RAmt[22, 1] = nAmt[17] + nAmt[20];
            RAmt[22, 2] = nAmt[39] + nAmt[41] + nAmt[42] + nAmt[43] + nAmt[44];
            RAmt[22, 2] = RAmt[22, 2] + nAmt[45] + nAmt[46] + nAmt[48];

            //23.합계
            RAmt[23, 1] = 0;
            RAmt[23, 2] = 0;
            for (i = 1; i <= 22; i++)
            {
                RAmt[23, 1] += RAmt[i, 1];
                RAmt[23, 2] += RAmt[i, 2];
            }

            //24.급여 본인부담액(비급여합계를 뺀금액) =
            //NAMT(53)의 금액은 비급여금액이 포함된금액입니다.

            if (strBi == "52")
            {
                nAmt[53] = nAmt[53];
                RAmt[24, 1] = RAmt[23, 1];
            }
            else
            {
                RAmt[24, 1] = RAmt[23, 1] - nAmt[53];
            }

            //25.급여 보험자부담액
            RAmt[25, 1] = nAmt[53];

            //27.진료비총액
            RAmt[26, 1] = RAmt[23, 1] + RAmt[23, 2];

            //28.환자부담총액 = 급여본인부담 + 비급여합계
            RAmt[27, 1] = RAmt[24, 1] + RAmt[23, 2];

            //29.이미 납부한 금액
            RAmt[28, 1] = nAmt[51];

            //30.할인액
            RAmt[29, 1] = nAmt[54];

            //31.미수액
            RAmt[30, 1] = nAmt[56];

            //32.수납금액
            RAmt[31, 1] = nAmt[57];

            if (nAmt[58] != 0)
            {
                RAmt[31, 1] = nAmt[58] * -1;
            }

            //--------------------------------
            // 치료재료대 금액을 계산함
            //--------------------------------
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                            ";
            SQL += ComNum.VBLF + "  Nu,SUM(Amt1+Amt2) Amt                           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP           ";
            if (ArgTRSNO == 0)
            {
                SQL += ComNum.VBLF + "WHERE IPDNO = " + ArgIPDNO + "                ";
            }
            else
            {
                SQL += ComNum.VBLF + "WHERE TrsNo = " + ArgTRSNO + "                ";
            }
            SQL += ComNum.VBLF + "      AND Bun IN ('29','31','32','33','36','39')  ";
            SQL += ComNum.VBLF + "GROUP BY Nu                                       ";

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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        //급여 처치재료대
                        if (String.Compare(dt.Rows[i]["NU"].ToString().Trim(), "20") <= 0)
                        {
                            RAmt[10, 1] += Convert.ToInt32(VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));
                            RAmt[7, 1] -= Convert.ToInt32(VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));
                        }
                        //비급여 처치재료대
                        else
                        {
                            RAmt[10, 2] += Convert.ToInt32(VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));
                            RAmt[7, 2] -= Convert.ToInt32(VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));
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

            nAmt[51] = 0;
            nAmt[52] = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                    ";
            SQL += ComNum.VBLF + "  SUM(SUM(CASE WHEN SUNEXT = 'Y88' THEN AMT END)) Y88     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "      AND TRSNO =" + ArgTRSNO + "                         ";
            SQL += ComNum.VBLF + "      AND SuNext IN ('Y88')                               ";
            SQL += ComNum.VBLF + "GROUP BY SuNext                                           ";

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
                    nAmt[51] = Convert.ToInt32(VB.Val(dt.Rows[0]["Y88"].ToString().Trim()));
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            if (nAmt[51] > 0)
            {
                #region SSAmt_DisPlay(GoSub)

                //-----------( 진료비 영수증을 Display )------------------------

                for (i = 1; i <= 23; i++)
                {
                    ssAmt_Sheet1.Cells[i, 1].Text = String.Format("{0:#,##0}", RAmt[i, 1]);
                    ssAmt_Sheet1.Cells[i, 2].Text = String.Format("{0:#,##0}", RAmt[i, 2]);
                }

                //보증금
                if (nAmt[52] > 0)
                {
                    ssAmt_Sheet1.Cells[11, 2].Text = String.Format("{0:#,##0}", nAmt[52]);
                }

                //환자부담총액
                ssAmt_Sheet1.Cells[0, 5].Text = String.Format("{0:#,##0}", RAmt[26, 1]);

                //조합부담금
                ssAmt_Sheet1.Cells[1, 5].Text = String.Format("{0:#,##0}", RAmt[25, 1]);

                //본인부담금
                ssAmt_Sheet1.Cells[2, 5].Text = String.Format("{0:#,##0}", RAmt[27, 1]);

                //이미 납부한 금액
                ssAmt_Sheet1.Cells[3, 5].Text = String.Format("{0:#,##0}", RAmt[28, 1]);

                //할인액
                ssAmt_Sheet1.Cells[4, 5].Text = String.Format("{0:#,##0}", RAmt[29, 1]);

                //미수액
                ssAmt_Sheet1.Cells[5, 5].Text = String.Format("{0:#,##0}", RAmt[30, 1]);

                //대불금
                ssAmt_Sheet1.Cells[6, 5].Text = String.Format("{0:#,##0}", nAmtSang);

                //희귀, 난치성 지원금
                if (strOBPDBun == "H")
                {
                    ssAmt_Sheet1.Cells[7, 5].Text = String.Format("{0:#,##0}", AMT62);

                    //수납액
                    if (strGbSTS == "7")
                    {
                        ssAmt_Sheet1.Cells[11, 5].Text = String.Format("{0:#,##0}", RAmt[31, 1]);   // - nAmtSang
                    }
                    else
                    {
                        RAmt[31, 1] = RAmt[26, 1] - RAmt[25, 1] - RAmt[28, 1] - RAmt[29, 1] - RAmt[30, 1] - nAmt[52]; // - nAmtSang;
                        ssAmt_Sheet1.Cells[11, 5].Text = String.Format("{ 0:#,##0}", RAmt[31, 1]);   // - nAmtSang
                    }

                }

                #endregion SSAmt_DisPlay(GoSub) End
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                    ";
                SQL += ComNum.VBLF + "  SUM(SUM(CASE WHEN SUNEXT = 'Y88' THEN AMT END)) Y88,                    ";
                SQL += ComNum.VBLF + "  SUM(SUM(CASE WHEN SUNEXT IN ('Y85','Y87') THEN AMT END )) Y8785         ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH                                   ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
                SQL += ComNum.VBLF + "      AND IPDNO=" + nIpdno1 + "                                           ";
                SQL += ComNum.VBLF + "      AND SuNext IN ('Y88','Y85','Y87')                                   ";
                SQL += ComNum.VBLF + "GROUP BY SuNext                                                           ";

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
                        nAmt[52] = Convert.ToInt32(VB.Val(dt.Rows[0]["Y8785"].ToString().Trim())) - Convert.ToInt32(VB.Val(dt.Rows[0]["Y88"].ToString().Trim()));
                    }
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
                dt.Dispose();
                dt = null;

                #region SSAmt_DisPlay(GoSub)

                //-----------( 진료비 영수증을 Display )------------------------

                for (i = 1; i <= 23; i++)
                {
                    ssAmt_Sheet1.Cells[i, 1].Text = String.Format("{0:#,##0}", RAmt[i, 1]);
                    ssAmt_Sheet1.Cells[i, 2].Text = String.Format("{0:#,##0}", RAmt[i, 2]);
                }

                //보증금
                if (nAmt[52] > 0)
                {
                    ssAmt_Sheet1.Cells[11, 2].Text = String.Format("{0:#,##0}", nAmt[52]);
                }

                //환자부담총액
                ssAmt_Sheet1.Cells[0, 5].Text = String.Format("{0:#,##0}", RAmt[26, 1]);

                //조합부담금
                ssAmt_Sheet1.Cells[1, 5].Text = String.Format("{0:#,##0}", RAmt[25, 1]);

                //본인부담금
                ssAmt_Sheet1.Cells[2, 5].Text = String.Format("{0:#,##0}", RAmt[27, 1]);

                //이미 납부한 금액
                ssAmt_Sheet1.Cells[3, 5].Text = String.Format("{0:#,##0}", RAmt[28, 1]);

                //할인액
                ssAmt_Sheet1.Cells[4, 5].Text = String.Format("{0:#,##0}", RAmt[29, 1]);

                //미수액
                ssAmt_Sheet1.Cells[5, 5].Text = String.Format("{0:#,##0}", RAmt[30, 1]);

                //대불금
                ssAmt_Sheet1.Cells[6, 5].Text = String.Format("{0:#,##0}", nAmtSang);

                //희귀, 난치성 지원금
                if (strOBPDBun == "H")
                {
                    ssAmt_Sheet1.Cells[7, 5].Text = String.Format("{0:#,##0}", AMT62);

                    //수납액
                    if (strGbSTS == "7")
                    {
                        ssAmt_Sheet1.Cells[11, 5].Text = String.Format("{0:#,##0}", RAmt[31, 1]);   // - nAmtSang
                    }
                    else
                    {
                        RAmt[31, 1] = RAmt[26, 1] - RAmt[25, 1] - RAmt[28, 1] - RAmt[29, 1] - RAmt[30, 1] - nAmt[52]; // - nAmtSang;
                        ssAmt_Sheet1.Cells[11, 5].Text = String.Format("{0:#,##0}", RAmt[31, 1]);   // - nAmtSang
                    }

                }
                #endregion SSAmt_DisPlay(GoSub)
            }


        }

        void Report_Print_Sunap_Sign_new(string ArgPno, string ArgGwa, string ArgNam, string ArgRetn, string ArgSeq,
                                         string ArgRdate, string ArgDr, string ArgBi, string ArgBDate, string ArgCardBun,
                                         string ArgSunap, string ArgDept, string Arg입원정밀, string argGBN)
        {

            int i = 0;
            int j = 0;
            int k = 0;
            int h = 0;
            int g = 0;
            int nPo = 0;

            int[,] nPAmt = new int[41, 3];
            int nSelf = 0;

            int AmtJin = 0;
            double nToAmt = 0;

            string[] strSite = new string[10];
            string strAMT_1 = "";   //금액 2
            string strAMT_2 = "";   //금액 3
            string strAMt1 = "";    //금액 1
            string strAMT2 = "";    //금액 2
            string strPno = "";
            string strGwa = "";
            string strBi = "";
            string strNam = "";
            string strSpc = "";
            string strlds = "";

            int nCnt = 0;
            int nAmt8 = 0;

            string strFname = "";
            int nSeqNo = 0;
            string strMsgChk2 = "";
            string strTimeSS = "";  //수납시간초

            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            DataTable dt3 = null;

            string SQL = "";
            string SqlErr = "";

            for (i = 0; i <= 40; i++)
            {
                nPAmt[i, 1] = 0;
                nPAmt[i, 2] = 0;
            }

            //기타수납 재발행 아닐경우
            if (argGBN == "외래")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                        ";
                SQL += ComNum.VBLF + "  BUN, GBSELF, SUM(AMT1+AMT2) NAMT,SUM(OGAMT) OGAMT           ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP                           ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                SQL += ComNum.VBLF + "      AND BDate  = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')   ";
                SQL += ComNum.VBLF + "      AND Pano     = '" + ArgPno + "'                         ";
                SQL += ComNum.VBLF + "      AND Bi       = '" + ArgBi + "'                          ";
                SQL += ComNum.VBLF + "      AND DeptCode = '" + ArgDept + "'                        ";
                SQL += ComNum.VBLF + "      AND SeqNo    = " + ArgSeq + "                           ";
                SQL += ComNum.VBLF + "GROUP BY BUN, GBSELF                                          ";
            }

            //기타수납 재발행인 경우
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                        ";
                SQL += ComNum.VBLF + "  BUN, GBSELF, SUM(AMT) NAMT                                  ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_ETCSLIP                        ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                SQL += ComNum.VBLF + "      AND BDate  = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')   ";
                SQL += ComNum.VBLF + "      AND Pano     = '" + ArgPno + "'                         ";
                SQL += ComNum.VBLF + "      AND Bi       = '" + ArgBi + "'                          ";
                SQL += ComNum.VBLF + "      AND Sunext NOT IN  ( 'Y96C' )                           ";
                SQL += ComNum.VBLF + "      AND SeqNo    = " + ArgSeq + "                           ";
                SQL += ComNum.VBLF + "GROUP BY BUN, GBSELF                                          ";
            }

            try
            {

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count == 0)
                {
                    dt1.Dispose();
                    dt1 = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (dt1.Rows[i]["GBSELF"].ToString().Trim() == "0")
                        {
                            nSelf = 1;
                        }
                        else
                        {
                            nSelf = 2;
                        }

                        switch (dt1.Rows[i]["BUN"].ToString().Trim())
                        {
                            case "01":
                            case "02":
                                nPAmt[1, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //진찰료
                                break;

                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                                nPAmt[4, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //투약료 및 조제료
                                break;

                            case "16":
                            case "17":
                            case "18":
                            case "19":
                            case "20":
                            case "21":
                                nPAmt[5, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //주사료
                                break;

                            case "22":
                            case "23":
                                nPAmt[6, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //마취료
                                break;

                            case "24":
                            case "25":
                                nPAmt[12, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //이학요법료(물리치료)
                                break;

                            case "26":
                            case "27":
                                nPAmt[13, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //정신요법료
                                break;

                            case "28":
                            case "29":
                            case "30":
                            case "31":
                            case "32":
                            case "33":
                            case "34":
                            case "35":
                            case "36":
                            case "38":
                            case "39":
                                nPAmt[7, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //처치 및 수술
                                break;

                            case "37":
                                nPAmt[18, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //수혈
                                break;

                            case "41":
                            case "42":
                            case "43":
                            case "44":
                            case "45":
                            case "46":
                            case "47":
                            case "48":
                            case "49":
                            case "50":
                            case "51":
                            case "52":
                            case "53":
                            case "54":
                            case "55":
                            case "56":
                            case "57":
                            case "58":
                            case "59":
                            case "60":
                            case "61":
                            case "62":
                            case "63":
                            case "64":
                                nPAmt[8, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //검사료
                                break;

                            case "65":
                            case "66":
                            case "67":
                            case "68":
                            case "69":
                            case "70":
                                nPAmt[9, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //방사선료
                                break;

                            case "72":
                                nPAmt[14, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //CT
                                break;
                            case "73":
                                nPAmt[15, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //MRI
                                break;
                            case "71":
                                nPAmt[16, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //초음파
                                break;
                            case "40":
                                nPAmt[17, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //보철료
                                break;
                            case "75":
                                nPAmt[20, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //증명료
                                break;

                            case "99":
                                nPAmt[31, 1] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //영수액
                                break;
                            case "98":
                                nPAmt[25, 1] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //조합부담액
                                break;
                            case "92":
                                nPAmt[29, 1] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //감액
                                break;
                            case "96":
                                nPAmt[30, 1] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //미수액
                                break;

                            default:
                                nPAmt[22, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //기타
                                break;
                        }

                        if (String.Compare(dt1.Rows[i]["BUN"].ToString().Trim(), "84") <= 0)
                        {
                            nPAmt[23, nSelf] += Convert.ToInt32(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));          //합계
                        }
                        //산전승인금액 SUM 2009-01-0 윤조연 추가
                        if (dt1.Rows[i]["BUN"].ToString().Trim() == "99" && dt1.Rows[i]["OGAMT"].ToString().Trim() != "0")
                        {
                            //GnOgAmt = GnOgAmt + AdoGetNumber(rsSub1, "OGAMT", i)
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            nPAmt[26, 1] = nPAmt[23, 1] + nPAmt[23, 2];     //진료비 총액
            nPAmt[27, 1] = nPAmt[26, 1] - nPAmt[25, 1];     //보험자부담금

            dt1.Dispose();
            dt1 = null;

            for (i = 1; i <= 23; i++)
            {
                ssAmt_Sheet1.Cells[i - 1, 1].Text = String.Format("{0:#,##0}", nPAmt[i, 1]);
                ssAmt_Sheet1.Cells[i - 1, 2].Text = String.Format("{0:#,##0}", nPAmt[i, 2]);
            }

            //환자부담총액
            ssAmt_Sheet1.Cells[0, 5].Text = String.Format("{0:#,##0}", nPAmt[26, 1]);

            //조합부담금
            ssAmt_Sheet1.Cells[1, 5].Text = String.Format("{0:#,##0}", nPAmt[25, 1]);

            //본인부담금
            ssAmt_Sheet1.Cells[2, 5].Text = String.Format("{0:#,##0}", nPAmt[27, 1]);

            //할인액            
            ssAmt_Sheet1.Cells[4, 5].Text = String.Format("{0:#,##0}", nPAmt[29, 1]);

            //미수액            
            ssAmt_Sheet1.Cells[5, 5].Text = String.Format("{0:#,##0}", nPAmt[30, 1]);


            ssAmt_Sheet1.Cells[11, 5].Text = String.Format("{0:#,##0}", nPAmt[31, 1]);
        }



        // TODO : BAS_HNIP_HISTORY 테이블에 2011년 이후 데이터가 존재 하지 않음 사용여부 확인 필요
        public void KoppUPdate_HNIP(string ArgData, string ArgGb)
        {
            int intVal = 0;
            object KPPO_JOB = "";
            string strTmp = "";
            string strGB = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;



            switch (ArgGb)
            {
                case "PT":
                    strGB = "[환자인적정보]";
                    break;

                case "IN":
                    strGB = "[입퇴원확인서]";
                    break;

                case "RC":
                    strGB = "[영   수   증]";
                    break;

                case "OT":
                    strGB = "[통원  확인서]";
                    break;
            }

            KPPO_JOB = "KppoUpdate.kppodb_update";

            strTmp = ArgData;

            //intVal = KPPO_JOB.KPPO_Update(strTmp, ArgGb);

            if (intVal == 0)
            {
                ComFunc.MsgBox(" 전송 error 의료정보과로 연락바람!!");
            }
            else
            {
                ComFunc.MsgBox(" 전송 완료 !!");
            }

            //병원 db에 history
            clsDB.setBeginTran(clsDB.DbCon);

            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_HNIP_HISTORY";
            SQL += ComNum.VBLF + "( ACTDATE, GBN, CDATA , SABUN , ENTDATE, FLAG  )";
            SQL += ComNum.VBLF + "VALUES(";
            SQL += ComNum.VBLF + "  TRUNC(SYSDATE),";
            //2017-10-27 argGBN 변수가 없음... 
            //SQL += ComNum.VBLF + "  '" + argGBN + "',";
            SQL += ComNum.VBLF + "  '" + ArgData + "',";
            SQL += ComNum.VBLF + "  '" + mnJobSabun + "',";
            SQL += ComNum.VBLF + "  SYSDATE ,";
            SQL += ComNum.VBLF + "  '" + intVal + "'";
            SQL += ComNum.VBLF + ")";




        }
    }
}

