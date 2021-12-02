using ComBase; //기본 클래스
using ComEmrBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmAgreePrint.cs
    /// Description     : 동의서 자동 인쇄
    /// Author          : 박창욱
    /// Create Date     : 2017-07-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\Ocs\OpdOcs\ojumst\FrmAgreePrint.frm(FrmAgreePrint.frm) >> frmAgreePrint.cs 폼이름 재정의" />	
    public partial class frmAgreePrint : Form
    {
        int i = 0;
        string WebComplete = "";

        private string GstrPtNo = "";
        private string GstrAcpNo = "";
        private string GstrInOutCls = "";
        private string GstrMedFrDate = "";
        private string GstrMedFrTime = "";
        private string GstrMedEndDate = "";
        private string GstrMedEndTime = "";
        private string GstrMedDeptCd = "";
        private string GstrMedDeptName = "";
        private string GstrMedDrCd = "";
        private string GstrGubun = "";
        private string GstrFormNo = "";

        public delegate void EventClose();
        public event EventClose rEventClose;



        public frmAgreePrint()
        {
            InitializeComponent();
        }

        /// <summary>
        /// GstrHelpCode 변경 -> 각각 파라미터로 받기
        /// </summary>
        /// <param name="strPtNo">등록번호</param>
        /// <param name="strAcpNo">접수번호</param>
        /// <param name="strInOutCls">외래/입원</param>
        /// <param name="strMedFrDate">접수일자</param>
        /// <param name="strMedFrTime">접수시간</param>
        /// <param name="strMedEndDate">종료일자</param>
        /// <param name="strMedEndTime">종료시간</param>
        /// <param name="strMedDeptCd">진료과(코드)</param>
        /// <param name="strMedDeptName">진료과(한글)</param>
        /// <param name="strMedDrCd">의사코드</param>
        /// <param name="strGubun">구분자</param>
        /// <param name="strFormNo">폼번호</param>
        public frmAgreePrint(string strPtNo, string strAcpNo, string strInOutCls, string strMedFrDate, string strMedFrTime, string strMedEndDate, string strMedEndTime, 
            string strMedDeptCd, string strMedDeptName, string strMedDrCd, string strGubun, string strFormNo)
        {
            InitializeComponent();

            GstrPtNo = strPtNo;
            GstrAcpNo = VB.Val(strAcpNo).ToString();
            GstrInOutCls = strInOutCls;
            GstrMedFrDate = strMedFrDate;
            GstrMedFrTime = strMedFrTime;
            GstrMedEndDate = strMedEndDate;
            GstrMedEndTime = strMedEndTime;
            GstrMedDeptCd = strMedDeptCd;
            GstrMedDeptName = strMedDeptName;
            GstrMedDrCd = strMedDrCd;
            GstrGubun = strGubun;
            GstrFormNo = strFormNo;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClose();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            doPrint();
        }

        void doPrint()
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false)
                return; //권한 확인

            #region 이전 로직

            //string[] strData = null;
            //string strURL = "";
            //string strTemp = "";
            //string strPassWord = "";
            //int i = 0;
            //double lngXXX = 0;
            //DataTable dt = null;
            //string SQL = "";    //Query문
            //string SqlErr = ""; //에러문 받는 변수

            //try
            //{
            //    SQL = "";
            //    SQL = " SELECT PASSWORD, PassHash, passhash256 ";
            //    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PASSWORD ";
            //    SQL = SQL + ComNum.VBLF + " WHERE ID = " + clsType.User.IdNumber;
            //    SQL = SQL + ComNum.VBLF + "   AND EDATE IS NULL ";

            //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            //    if (SqlErr != "")
            //    {
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        return;
            //    }
            //    if (dt.Rows.Count > 0)
            //    {
            //        strPassWord = dt.Rows[0]["passhash256"].ToString().Trim();
            //    }
            //    dt.Dispose();
            //    dt = null;
            //}
            //catch (Exception ex)
            //{
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //    ComFunc.MsgBox(ex.Message);
            //}

            //web.Navigate("http://192.168.100.33:8090/Emr/doLogin.mts?useId=" + clsType.User.IdNumber + "&password=" + strPassWord + "&loginType=vb");
            //do
            //{
            //    Application.DoEvents();
            //} while (WebComplete != "YES");
            //WebComplete = "";

            //strURL = "http://192.168.100.33:8090/Emr/emrxmlInfo.mts";
            //strTemp = "?ptNo=" + GstrPtNo;
            //strTemp += "&acpNo=" + GstrAcpNo;
            //strTemp += "&inOutCls=" + GstrInOutCls;
            //strTemp += "&medFrDate=" + GstrMedFrDate;
            //strTemp += "&medFrTime=" + GstrMedFrTime;
            //strTemp += "&medEndDate=" + GstrMedEndDate;
            //strTemp += "&medEndTime=" + GstrMedEndTime;
            //strTemp += "&medDeptCd=" + GstrMedDeptCd;
            //strTemp += "&medDeptName=" + GstrMedDeptName;
            //strTemp += "&medDrCd=" + GstrMedDrCd;
            //strTemp += "&gubun=" + GstrGubun;
            //strTemp += "&formNo=" + GstrFormNo;

            //web.Navigate(strURL + strTemp);     //작성권한이 없으면 조회
            //do
            //{
            //    Application.DoEvents();
            //} while (WebComplete != "YES");
            //WebComplete = "";

            //strURL = "http://192.168.100.33:8090/Emr/chartFormPrint.mts?formNo=" + GstrFormNo;

            //web.Navigate(strURL);
            //do
            //{
            //    Application.DoEvents();
            //} while (WebComplete != "YES");
            //WebComplete = "";

            //Cursor.Current = Cursors.WaitCursor;

            #endregion

            EmrForm emrForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, GstrFormNo);
            EmrPatient AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, GstrPtNo, GstrInOutCls, GstrMedFrDate, GstrMedDeptCd);
            if (AcpEmr == null)
            {
                ComFunc.MsgBoxEx(this, "환자정보가 올바르지 않습니다.");
                return;
            }

            using (frmEmrChartNew frm = new frmEmrChartNew(emrForm.FmFORMNO.ToString(), emrForm.FmUPDATENO.ToString(), AcpEmr, "0", "W"))
            {
                frm.Show();
                frm.pPrintFormNull();
            }

            SetEMROCRPRTHIS();
        }

        private void btnCommand_Click(object sender, EventArgs e)
        {
            return;
            string[] strData = null;
            string strURL = "";
            string strTemp = "";

            double lngXXX = 0;
            
            strURL = "http://192.168.100.33:8090/Emr/emrxmlInfo.mts";
            strTemp = "?ptNo=" + GstrPtNo;
            strTemp += "+acpNo=" + GstrAcpNo;
            strTemp += "+inOutCls=" + GstrInOutCls;
            strTemp += "+medFrDate=" + GstrMedFrDate;
            strTemp += "+medFrTime=" + GstrMedFrTime;
            strTemp += "+medEndDate=" + GstrMedEndDate;
            strTemp += "+medEndTime=" + GstrMedEndTime;
            strTemp += "+medDeptCd=" + GstrMedDeptCd;
            strTemp += "+medDeptName=" + GstrMedDeptName;
            strTemp += "+medDrCd=" + GstrMedDrCd;
            strTemp += "+gubun=" + GstrGubun;
            strTemp += "+formNo=" + GstrFormNo;

            web.Navigate(strURL + strTemp); //작성권한인 없으면 조회
            do
            {
                if (lngXXX >= 300000)
                {
                    break;
                }
                lngXXX += 1;
                Application.DoEvents();
            } while (web.IsBusy);
        }

        private void btnCommand2_Click(object sender, EventArgs e)
        {
            return;
            //OCR출력 히스토리를 쌓는다.
            //현재상태의 환자정보를 가지고 와서 쌓는다.
            double lngXXX = 0;
            string strURL = "";
            string strFormNo = "";

            strURL = "http://192.168.100.33:8090/Emr/chartFormPrint.mts?formNo=" + GstrFormNo;

            web.Navigate(strURL);
            Cursor.Current = Cursors.WaitCursor;
            do
            {
                if (lngXXX >= 300000)
                {
                    break;
                }
                lngXXX += 1;
                Application.DoEvents();
            } while (web.IsBusy);

            Cursor.Current = Cursors.Default;

            rEventClose();
        }

        private void frmAgreePrint_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            
            doPrint();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            i += 1;
            lblTimer.Text = 2 - i + "초 후에 자동 종료";

            if (i > 1)
            {
                rEventClose();
            }
        }

        private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebComplete = "YES";
        }

        private void SetEMROCRPRTHIS()
        {
            string strWardCode = "";
            string strName = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (GstrInOutCls == "I")
                {
                    SQL = "";
                    SQL = " SELECT A.WARDCODE, B.SNAME";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER A,";
                    SQL = SQL + ComNum.VBLF + "  KOSMOS_PMPA.BAS_PATIENT B";
                    SQL = SQL + ComNum.VBLF + "  WHERE A.PANO = B.PANO";
                    SQL = SQL + ComNum.VBLF + "  AND TO_CHAR(A.INDATE,'YYYYMMDD') = '" + GstrMedFrDate + "'";
                    SQL = SQL + ComNum.VBLF + "  AND A.DEPTCODE = '" + GstrMedDeptCd + "'";
                    SQL = SQL + ComNum.VBLF + "  AND A.PANO = '" + GstrPtNo + "'";
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT '' AS WARDCODE, B.SNAME";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER A,";
                    SQL = SQL + ComNum.VBLF + "  KOSMOS_PMPA.BAS_PATIENT B";
                    SQL = SQL + ComNum.VBLF + "  WHERE A.PANO = B.PANO";
                    SQL = SQL + ComNum.VBLF + "  AND TO_CHAR(A.BDATE,'YYYYMMDD') = '" + GstrMedFrDate + "'";
                    SQL = SQL + ComNum.VBLF + "  AND A.DEPTCODE = '" + GstrMedDeptCd + "'";
                    SQL = SQL + ComNum.VBLF + "  AND A.PANO = '" + GstrPtNo + "'";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strWardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    strName = dt.Rows[0]["SNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = " INSERT INTO KOSMOS_EMR.EMROCRPRTHIS";
                SQL = SQL + ComNum.VBLF + " (OCRDATE,OCRTIME,PTNO,PTNAME,INOUTCLS,";
                SQL = SQL + ComNum.VBLF + "  MEDFRDATE,MEDDEPTCD,WARDCODE,";
                SQL = SQL + ComNum.VBLF + "  FORMNO,USEID,DEPTCD,DEPTCD1)";
                SQL = SQL + ComNum.VBLF + "  VALUES (";
                SQL = SQL + ComNum.VBLF + "  '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "',";
                SQL = SQL + ComNum.VBLF + "  '" + ComQuery.CurrentDateTime(clsDB.DbCon, "T") + "',";
                SQL = SQL + ComNum.VBLF + "  '" + GstrPtNo + "',";
                SQL = SQL + ComNum.VBLF + "  '" + strName + "',";
                SQL = SQL + ComNum.VBLF + "  '" + GstrInOutCls + "',";
                SQL = SQL + ComNum.VBLF + "  '" + GstrMedFrDate + "',";
                SQL = SQL + ComNum.VBLF + "  '" + GstrMedDeptCd + "',";
                SQL = SQL + ComNum.VBLF + "  '" + strWardCode + "',";
                SQL = SQL + ComNum.VBLF + "  '" + GstrFormNo + "',";
                SQL = SQL + ComNum.VBLF + "  '" + clsType.User.Sabun + "',";
                SQL = SQL + ComNum.VBLF + "  '" + GstrMedDeptCd + "',";
                SQL = SQL + ComNum.VBLF + "  '')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                SQL = "";
                SQL = " INSERT INTO KOSMOS_EMR.EMR_AGREE_PRINT(";
                SQL = SQL + ComNum.VBLF + " PRTDATE, PRTSABUN, PRTINFO, PTNO, ";
                SQL = SQL + ComNum.VBLF + " INOUTCLS, MEDFRDATE, MEDDEPTCD, MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + " FORMNO ) VALUES ( ";
                SQL = SQL + ComNum.VBLF + "SYSDATE, " + clsType.User.Sabun + ", '', '" + GstrPtNo + "', ";
                SQL = SQL + ComNum.VBLF + "'" + GstrInOutCls + "','" + GstrMedFrDate + "','" + GstrMedDeptCd + "','" + GstrMedDrCd + "', ";
                SQL = SQL + ComNum.VBLF + "'" + GstrFormNo + "') ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                timer.Enabled = true;
                i = 0;
                return;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
