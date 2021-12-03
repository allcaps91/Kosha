using ComBase; //기본 클래스
using ComLibB;
using DevComponents.DotNetBar;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewPoscoT.cs
    /// Description     : 포스코 결과 통보서
    /// Author          : 유진호
    /// Create Date     : 2018-04-10
    /// Update History  : 
    /// <history>       
    /// d:\psmh\Ocs\OpdOcs\Oorder\Frm포스코결과통보서.frm(Frm포스코결과통보서) => frmPmpaViewPoscoT.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\Ocs\OpdOcs\Oorder\Frm포스코결과통보서.frm(Frm포스코결과통보서)    
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewPoscoT : Form
    {
        private string GstrPoscoExamPano = "";
        private string GnJobSabun = "";
        private string GstrDrCode = "";

        private ComFunc CF = null;

        TextBox mtxtMcrt = null;
        TextBox mtxtMcrt2 = null;
        TextBox mtxtMcrt3 = null;
        TextBox mtxtMcrt4 = null;


        /// <summary>
        /// EMR뷰어
        /// </summary>
        frmEmrViewer fEmrViewer = null;

        public frmPmpaViewPoscoT()
        {
            InitializeComponent();
        }

        public frmPmpaViewPoscoT(string strPoscoExamPano, string strDrCode, string strJobSabun)
        {
            InitializeComponent();
            this.GstrPoscoExamPano = strPoscoExamPano;
            this.GstrDrCode = strDrCode;
            this.GnJobSabun = strJobSabun;
        }

        private void frmPmpaViewPoscoT_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등     

            CF = new ComFunc();                       

            txtPtNo.Text = GstrPoscoExamPano;

            #region 19-10-24 EMR뷰어 모달 관련 처리
            fEmrViewer = new frmEmrViewer(txtPtNo.Text);
            fEmrViewer.Show(this);
            #endregion

            GetData();

            setAuth(); //특정사번은 기타결과 같이 보이게

        }

        void setAuth()
        {
            clsQuery Query = new clsQuery();

            DataTable dt = Query.Get_BasBcode(clsDB.DbCon, "C#_처방_포스코결과입력_확장", GnJobSabun.ToString(), " Code || '.' || Name CodeName, Code ,Name   ", "", " Sort ASC, Code ASC ");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                exSpliter2.Visible = true;

                this.exSpliter2.Click += new EventHandler(eSpliterClick);

                pan_resultM.Visible = true;

                this.WindowState = FormWindowState.Maximized;
                this.pan_resultM.Size = new Size(950, 981);

                frmViewResult f = new frmViewResult(GstrPoscoExamPano);
                setCtrlLoad(pan_result, f);

            }

            dt.Dispose();
            dt = null;
            
            Query = null;
        }

        private void SCREEN_CLEAR()
        {
            ssView_Sheet1.RowCount = 0;
            txtMcNo.Text = "";
            txtJDate.Text = "";
            txtSex.Text = "";
            txtSabun.Text = "";
            txtSName.Text = "";
            txtJumin1.Text = "";
            txtJumin2.Text = "";
            txtBuse.Text = "";
            txtJuso.Text = "";

            txtExam01_01.Text = "";
            txtExam01_02.Text = "";
            txtExam01_03.Text = "";
            txtExam01_04.Text = "";
            txtExam01_05.Text = "";
            txtExam01_06.Text = "";
            txtExam01_07.Text = "";
            txtExam01_08.Text = "";
            txtExam01_09.Text = "";
            txtExam01_10.Text = "";
            txtExam01_Remark.Text = "";

            txtExam02_01.Text = "";
            txtExam02_02.Text = "";
            txtExam02_03.Text = "";
            txtExam02_04.Text = "";
            txtExam02_05.Text = "";
            txtExam02_06.Text = "";
            txtExam02_07.Text = "";
            txtExam02_08.Text = "";
            txtExam02_09.Text = "";
            txtExam02_10.Text = "";
            txtExam02_11.Text = "";

            txtExam02_Remark.Text = "";
            
            //MRI주가
            txtExam02_08.Text = "";
            txtExam02_09.Text = "";
            txtExam02_10.Text = "";
            txtExam02_11.Text = "";

            txtExam03_01.Text = "";
            txtExam03_02.Text = "";
            txtExam03_03.Text = "";
            txtExam03_04.Text = "";
            txtExam03_05.Text = "";
            txtExam03_Remark.Text = "";

            txtExamDate.Text = "";
            txtResultDate.Text = "";

            chkExam02_01.Checked = false;
            chkExam02_02.Checked = false;
            chkExam02_03.Checked = false;
            chkExam02_04.Checked = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (SaveData() == true)
            {
                GetData();
                SCREEN_CLEAR();
                btnExit.PerformClick();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            string GstrPANO = VB.Trim(txtPtNo.Text);

            if (GstrPANO != "")
            {
                frmViewResult frmViewResultX = new frmViewResult(GstrPANO);
                frmViewResultX.StartPosition = FormStartPosition.CenterParent;
                //frmViewResultX.ShowDialog();
                frmViewResultX.Show();
            }
            else
            {
                ComFunc.MsgBox("등록번호가 공란입니다.", "확인");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (DeleteData() == true)
            {
                SCREEN_CLEAR();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool SaveData()
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strMCCLASS = "";             //'진단서코드
            string strPtNo = "";                //'등록번호
            string strMCNO = "";                //'연번호
            string strJumin1 = "";              //'주민번호1
            string strJumin2 = "";              //'주민번호2
            string strSex = "";                 //'성별
            string strSname = "";               //'성명
            string strJuso = "";                //'주소1
            string strJDate = "";               //'접수일자
            string strSabun = "";               //'직번
            string strBuse = "";                //'부서


            string strExam01_01 = "";          //'위-위장조영촬영
            string strExam01_02 = "";          //'위-상부소화관 내시경검사
            string strExam01_03 = "";          //'위-조직검사
            string strExam01_04 = "";          //'위-헬리코박터균검사
            string strExam01_05 = "";          //'위-기타
            string strExam01_06 = "";          //'대장-결장경검사
            string strExam01_07 = "";          //'대장-조직검사
            string strExam01_08 = "";          //'대장-기타
            string strExam01_09 = "";          //'복부초음파-초음파검사(간,췌장,비장,신장)
            string strExam01_10 = "";          //'폐-저선량폐CT
            string strExam01_Remark = "";          //'위|대장|복부초음파|폐 종합소견


            string strExam02_01 = "";          //'CT-뇌
            string strExam02_02 = "";          //'CT-경추
            string strExam02_03 = "";          //'CT-요추
            string strExam02_04 = "";          //'CT-심장
            string strExam02_05 = "";          //'초음파-심장
            string strExam02_06 = "";          //'초음파-경동맥
            string strExam02_07 = "";          //'초음파-뇌혈류
            string strExam02_08 = "";          //'무릎 MRI 
            string strExam02_09 = "";          //'뇌 MRI 
            string strExam02_10 = "";          //'경추 MRI
            string strExam02_11 = "";          //'요추 MRI 
            string strExam02_Remark = "";          //'CT|초음파 종합소견


            string strExam03_01 = "";          //'자궁검진-자궁경부세포검사
            string strExam03_02 = "";          //'자궁검진-자궁난소초음파
            string strExam03_03 = "";          //'자궁검진-인유두종바이러스검사
            string strExam03_04 = "";          //'유방검진-유방단순촬영
            string strExam03_05 = "";          //'유방검진-유방검진
            string strExam03_Remark = "";          //'자궁검진|유방검진 종합소견


            string strExamDate = "";          //'검사일
            string strRESULTDATE = "";          //'결과통보일
            string strDrname = "";          //'검진의
            string strHospital = "";          //'검진기관명
            string strDrCode = "";          //'의사코드
            string strLicense = "";          //'면허번호


            string strChkExam02_01 = "";          //'CT-뇌(조영제 사용여부)
            string strChkExam02_02 = "";          //'CT-경추(조영제 사용여부)
            string strChkExam02_03 = "";          //'CT-요추(조영제 사용여부)
            string strChkExam02_04 = "";          //'CT-심장(조영제 사용여부)


            strMCCLASS = "28";
            strPtNo = txtPtNo.Text.Trim();
            strMCNO = txtMcNo.Text.Trim();
            strJumin1 = txtJumin1.Text.Trim();
            strJumin2 = txtJumin2.Text.Trim();
            strSex = (txtSex.Text.Trim() == "남자" ? "M" : "F");
            strSname = txtSName.Text.Trim();
            strJuso = txtJuso.Text.Trim();
            strJDate = txtJDate.Text.Trim();
            strSabun = txtSabun.Text.Trim();
            strBuse = txtBuse.Text.Trim();

            strExam01_01 = txtExam01_01.Text.Trim();
            strExam01_02 = txtExam01_02.Text.Trim();
            strExam01_03 = txtExam01_03.Text.Trim();
            strExam01_04 = txtExam01_04.Text.Trim();
            strExam01_05 = txtExam01_05.Text.Trim();
            strExam01_06 = txtExam01_06.Text.Trim();
            strExam01_07 = txtExam01_07.Text.Trim();
            strExam01_08 = txtExam01_08.Text.Trim();
            strExam01_09 = txtExam01_09.Text.Trim();
            strExam01_10 = txtExam01_10.Text.Trim();
            strExam01_Remark = txtExam01_Remark.Text.Trim();

            strExam02_01 = txtExam02_01.Text.Trim();
            strExam02_02 = txtExam02_02.Text.Trim();
            strExam02_03 = txtExam02_03.Text.Trim();
            strExam02_04 = txtExam02_04.Text.Trim();
            strExam02_05 = txtExam02_05.Text.Trim();
            strExam02_06 = txtExam02_06.Text.Trim();
            strExam02_07 = txtExam02_07.Text.Trim();
            strExam02_08 = txtExam02_08.Text.Trim();
            strExam02_09 = txtExam02_09.Text.Trim();
            strExam02_10 = txtExam02_10.Text.Trim();
            strExam02_11 = txtExam02_11.Text.Trim();
            strExam02_Remark = txtExam02_Remark.Text.Trim();

            strExam03_01 = txtExam03_01.Text.Trim();
            strExam03_02 = txtExam03_02.Text.Trim();
            strExam03_03 = txtExam03_03.Text.Trim();
            strExam03_04 = txtExam03_04.Text.Trim();
            strExam03_05 = txtExam03_05.Text.Trim();
            strExam03_Remark = txtExam03_Remark.Text.Trim();

            strExamDate = txtExamDate.Text.Trim();
            strRESULTDATE = txtResultDate.Text.Trim();
            strDrname = txtDrName.Text.Trim();
            strHospital = txtHospital.Text.Trim();
            strDrCode = txtDrCode.Text.Trim();
            strLicense = txtLicense.Text.Trim();

            strChkExam02_01 = chkExam02_01.Checked == true ? "1" : "0";
            strChkExam02_02 = chkExam02_02.Checked == true ? "1" : "0";
            strChkExam02_03 = chkExam02_03.Checked == true ? "1" : "0";
            strChkExam02_04 = chkExam02_04.Checked == true ? "1" : "0";

            if (strPtNo == "")
            {
                ComFunc.MsgBox("환자를 선택하시기 바랍니다.");
                return rtVal;
            }
            else if (strMCNO == "")
            {
                //'insert 는 oiguide.exe 에서만 사용하므로 사용금지(고객지원과에서 전송한 건에 대해서만 update 가능함.)
                ComFunc.MsgBox("진단서 List 에서 검사자 선택후 등록하시기 바랍니다.", "확인");
                return rtVal;
            }


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strMCNO == "")
                {
                    strMCNO = GetMcNo_new();
                    if (strMCNO == "-1") return rtVal;
                    txtMcNo.Text = strMCNO;

                    SQL = "INSERT INTO ADMIN.OCS_MCCERTIFI28(";
                    SQL = SQL + ComNum.VBLF + " MCCLASS, PTNO, MCNO,";
                    SQL = SQL + ComNum.VBLF + " JUMIN1, JUMIN3, SEX, SNAME, JUSO,";
                    SQL = SQL + ComNum.VBLF + " JDATE, SABUN, BUSE,";
                    SQL = SQL + ComNum.VBLF + " EXAM01_01, EXAM01_02, EXAM01_03, EXAM01_04, EXAM01_05,";
                    SQL = SQL + ComNum.VBLF + " EXAM01_06, EXAM01_07, EXAM01_08, EXAM01_09, EXAM01_10,";
                    SQL = SQL + ComNum.VBLF + " EXAM01_REMARK,";
                    SQL = SQL + ComNum.VBLF + " EXAM02_01, EXAM02_02, EXAM02_03, EXAM02_04, EXAM02_05,";
                    SQL = SQL + ComNum.VBLF + " EXAM02_06, EXAM02_07,EXAM02_08,EXAM02_09,EXAM02_10,EXAM02_11,";
                    SQL = SQL + ComNum.VBLF + " EXAM02_REMARK,";
                    SQL = SQL + ComNum.VBLF + " EXAM03_01, EXAM03_02, EXAM03_03, EXAM03_04, EXAM03_05,";
                    SQL = SQL + ComNum.VBLF + " EXAM03_REMARK,";
                    SQL = SQL + ComNum.VBLF + " HOSPITAL, EXAM_DATE,";
                    SQL = SQL + ComNum.VBLF + " RESULT_DATE, DRNAME,";
                    SQL = SQL + ComNum.VBLF + " DRCODE, LICENSE,";
                    SQL = SQL + ComNum.VBLF + " CHK_EXAM02_01, CHK_EXAM02_02, CHK_EXAM02_03, CHK_EXAM02_04) VALUES(";
                    SQL = SQL + ComNum.VBLF + " '" + strMCCLASS + "', '" + strPtNo + "', '" + strMCNO + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strJumin1 + "', '" + clsAES.AES(strJumin2) + "', '" + strSex + "', '" + strSname + "', '" + strJuso + "',";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strJDate + "', 'YYYY-MM-DD'), '" + strSabun + "', '" + strBuse + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strExam01_01 + "', '" + strExam01_02 + "', '" + strExam01_03 + "', '" + strExam01_04 + "', '" + strExam01_05 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strExam01_06 + "', '" + strExam01_07 + "', '" + strExam01_08 + "', '" + strExam01_09 + "', '" + strExam01_10 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strExam01_Remark + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strExam02_01 + "', '" + strExam02_02 + "', '" + strExam02_03 + "', '" + strExam02_04 + "', '" + strExam02_05 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strExam02_06 + "', '" + strExam02_07 + "','" + strExam02_08 + "', '" + strExam02_09 + "','" + strExam02_10 + "', '" + strExam02_11 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strExam02_Remark + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strExam03_01 + "', '" + strExam03_02 + "', '" + strExam03_03 + "', '" + strExam03_04 + "', '" + strExam03_05 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strExam03_Remark + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strHospital + "', TO_DATE('" + strExamDate + "', 'YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strRESULTDATE + "', 'YYYY-MM-DD'), '" + strDrname + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strDrCode + "', '" + strLicense + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strChkExam02_01 + "', '" + strChkExam02_02 + "', '" + strChkExam02_03 + "', '" + strChkExam02_04 + "')";
                }
                else
                {
                    SQL = "UPDATE ADMIN.OCS_MCCERTIFI28 SET";
                    SQL = SQL + ComNum.VBLF + " EXAM01_01 = '" + strExam01_01 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM01_02 = '" + strExam01_02 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM01_03 = '" + strExam01_03 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM01_04 = '" + strExam01_04 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM01_05 = '" + strExam01_05 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM01_06 = '" + strExam01_06 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM01_07 = '" + strExam01_07 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM01_08 = '" + strExam01_08 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM01_09 = '" + strExam01_09 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM01_10 = '" + strExam01_10 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM01_REMARK = '" + strExam01_Remark + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM02_01 = '" + strExam02_01 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM02_02 = '" + strExam02_02 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM02_03 = '" + strExam02_03 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM02_04 = '" + strExam02_04 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM02_05 = '" + strExam02_05 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM02_06 = '" + strExam02_06 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM02_07 = '" + strExam02_07 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM02_08 = '" + strExam02_08 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM02_09 = '" + strExam02_09 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM02_10 = '" + strExam02_10 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM02_11 = '" + strExam02_11 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM02_REMARK = '" + strExam02_Remark + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM03_01 = '" + strExam03_01 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM03_02 = '" + strExam03_02 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM03_03 = '" + strExam03_03 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM03_04 = '" + strExam03_04 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM03_05 = '" + strExam03_05 + "',";
                    SQL = SQL + ComNum.VBLF + " EXAM03_REMARK = '" + strExam03_Remark + "',";
                    SQL = SQL + ComNum.VBLF + " CHK_EXAM02_01 = '" + strChkExam02_01 + "',";
                    SQL = SQL + ComNum.VBLF + " CHK_EXAM02_02 = '" + strChkExam02_02 + "',";
                    SQL = SQL + ComNum.VBLF + " CHK_EXAM02_03 = '" + strChkExam02_03 + "',";
                    SQL = SQL + ComNum.VBLF + " CHK_EXAM02_04 = '" + strChkExam02_04 + "',";
                    SQL = SQL + ComNum.VBLF + " RESULT_DATE = TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "  AND MCNO = '" + strMCNO + "'";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private bool DeleteData()
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (VB.Trim(txtMcNo.Text) == "")
            {
                ComFunc.MsgBox("연번호가 공란입니다." + ComNum.VBLF + ComNum.VBLF + "삭제할 진단서를 선택하여 주십시요.", "확인");
                return rtVal;
            }

            if (ComFunc.MsgBoxQ("해당 진단서를 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return rtVal;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " INSERT INTO ADMIN.OCS_MCCERTIFI28_HISTORY ";
                SQL = SQL + ComNum.VBLF + " SELECT * FROM ADMIN.OCS_MCCERTIFI28";
                SQL = SQL + ComNum.VBLF + " WHERE MCNO = '" + ComFunc.SetAutoZero(VB.Trim(txtMcNo.Text), 8) + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                SQL = " DELETE ADMIN.OCS_MCCERTIFI28 ";
                SQL = SQL + ComNum.VBLF + " WHERE MCNO = '" + ComFunc.SetAutoZero(VB.Trim(txtMcNo.Text), 8) + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void btnPrint1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            ssPrint1_Sheet1.Cells[1, 1].Text = VB.Trim(txtSName.Text);
            ssPrint1_Sheet1.Cells[1, 3].Text = VB.Trim(txtJumin1.Text) + "-" + VB.Trim(txtJumin2.Text);
            ssPrint1_Sheet1.Cells[1, 5].Text = VB.Trim(txtPtNo.Text);

            ssPrint1_Sheet1.Cells[2, 1].Text = VB.Trim(txtSabun.Text);
            ssPrint1_Sheet1.Cells[1, 3].Text = VB.Trim(txtBuse.Text);
                        
            ssPrint1_Sheet1.Cells[5, 3].Text = VB.Trim(txtExam01_01.Text);
            ssPrint1_Sheet1.Cells[6, 3].Text = VB.Trim(txtExam01_02.Text);
            ssPrint1_Sheet1.Cells[7, 3].Text = VB.Trim(txtExam01_03.Text);
            ssPrint1_Sheet1.Cells[8, 3].Text = VB.Trim(txtExam01_04.Text);
            ssPrint1_Sheet1.Cells[9, 3].Text = VB.Trim(txtExam01_05.Text);
            ssPrint1_Sheet1.Cells[10, 3].Text = VB.Trim(txtExam01_06.Text);
            ssPrint1_Sheet1.Cells[11, 3].Text = VB.Trim(txtExam01_07.Text);
            ssPrint1_Sheet1.Cells[12, 3].Text = VB.Trim(txtExam01_08.Text);
            ssPrint1_Sheet1.Cells[13, 3].Text = VB.Trim(txtExam01_09.Text);
            ssPrint1_Sheet1.Cells[14, 3].Text = VB.Trim(txtExam01_10.Text);
                        
            ssPrint1_Sheet1.Cells[16, 1].Text = VB.Trim(txtExam01_Remark.Text);
                        
            ssPrint1_Sheet1.Cells[17, 1].Text = VB.Trim(txtExamDate.Text);
            ssPrint1_Sheet1.Cells[18, 1].Text = VB.Trim(txtDrName.Text);
                        
            ssPrint1_Sheet1.Cells[17, 4].Text = VB.Trim(txtResultDate.Text);
            ssPrint1_Sheet1.Cells[17, 4].Text = VB.Trim(txtHospital.Text);
            
            #region //출력1
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            //strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            //strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            //strHead1 = "/n/n/f1/C 수 술 실 대 장" + "/n/n/n/n";
            //strHead2 = "/l/f2" + "조회기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + "~" + dtpTDate.Value.ToString("yyyy-MM-dd") + VB.Space(10) + "인쇄일자 : " + SysDate;

            //ssView1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;	//가로
            ssPrint1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;   //세로

            //ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssPrint1_Sheet1.PrintInfo.Margin.Left = 35;
            ssPrint1_Sheet1.PrintInfo.Margin.Right = 0;
            ssPrint1_Sheet1.PrintInfo.Margin.Top = 35;
            ssPrint1_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssPrint1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssPrint1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssPrint1_Sheet1.PrintInfo.ShowBorder = true;
            ssPrint1_Sheet1.PrintInfo.ShowColor = false;
            ssPrint1_Sheet1.PrintInfo.ShowGrid = true;
            ssPrint1_Sheet1.PrintInfo.ShowShadows = false;
            ssPrint1_Sheet1.PrintInfo.UseMax = false;
            ssPrint1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPrint1.PrintSheet(0);
            #endregion
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {            
            ssPrint2_Sheet1.Cells[1, 1].Text = VB.Trim(txtSName.Text);
            ssPrint2_Sheet1.Cells[1, 3].Text = VB.Trim(txtJumin1.Text) + "-" + VB.Trim(txtJumin2.Text);
            ssPrint2_Sheet1.Cells[1, 5].Text = VB.Trim(txtPtNo.Text);
                        
            ssPrint2_Sheet1.Cells[2, 1].Text = VB.Trim(txtSabun.Text);
            ssPrint2_Sheet1.Cells[2, 3].Text = VB.Trim(txtBuse.Text);
                        
            if (chkExam02_01.Checked == true)
            {
                ssPrint2_Sheet1.Cells[5, 3].Text = VB.Trim(txtExam02_01.Text);
            }
            else
            {
                ssPrint2_Sheet1.Cells[6, 3].Text = VB.Trim(txtExam02_01.Text);
            }
            
            if (chkExam02_02.Checked == true)
            {
                ssPrint2_Sheet1.Cells[7, 3].Text = VB.Trim(txtExam02_02.Text);
            }
            else
            {
                ssPrint2_Sheet1.Cells[8, 3].Text = VB.Trim(txtExam02_02.Text);
            }

            if (chkExam02_03.Checked == true)
            {
                ssPrint2_Sheet1.Cells[9, 3].Text = VB.Trim(txtExam02_03.Text);
            }
            else
            {
                ssPrint2_Sheet1.Cells[10, 3].Text = VB.Trim(txtExam02_03.Text);
            }
            
            if( chkExam02_04.Checked == true )
            {
                ssPrint2_Sheet1.Cells[11, 3].Text = VB.Trim(txtExam02_04.Text);
            }
            else
            {
                ssPrint2_Sheet1.Cells[12, 3].Text = VB.Trim(txtExam02_04.Text);
            }
            //MRI 추가
            ssPrint2_Sheet1.Cells[13, 2].Text = VB.Trim(txtExam02_08.Text);
            ssPrint2_Sheet1.Cells[14, 2].Text = VB.Trim(txtExam02_09.Text);
            ssPrint2_Sheet1.Cells[15, 2].Text = VB.Trim(txtExam02_10.Text);
            ssPrint2_Sheet1.Cells[16, 2].Text = VB.Trim(txtExam02_11.Text);  

            ssPrint2_Sheet1.Cells[17, 2].Text = VB.Trim(txtExam02_05.Text);
            ssPrint2_Sheet1.Cells[18, 2].Text = VB.Trim(txtExam02_06.Text);
            ssPrint2_Sheet1.Cells[19, 2].Text = VB.Trim(txtExam02_07.Text);

            ssPrint2_Sheet1.Cells[21, 1].Text = VB.Trim(txtExam02_Remark.Text);
            
            ssPrint2_Sheet1.Cells[22, 1].Text = VB.Trim(txtExamDate.Text);
            ssPrint2_Sheet1.Cells[23, 1].Text = VB.Trim(txtDrName.Text);
                        
            ssPrint2_Sheet1.Cells[22, 4].Text = VB.Trim(txtResultDate.Text);
            ssPrint2_Sheet1.Cells[23, 4].Text = VB.Trim(txtHospital.Text);
        }

        private void btnPrint3_Click(object sender, EventArgs e)
        {
            ssPrint3_Sheet1.Cells[1, 1].Text = VB.Trim(txtSName.Text);
            ssPrint3_Sheet1.Cells[1, 3].Text = VB.Trim(txtJumin1.Text) + "-" + VB.Trim(txtJumin2.Text);
            ssPrint3_Sheet1.Cells[1, 5].Text = VB.Trim(txtPtNo.Text);

            ssPrint3_Sheet1.Cells[2, 1].Text = VB.Trim(txtSabun.Text);
            ssPrint3_Sheet1.Cells[2, 3].Text = VB.Trim(txtBuse.Text);
            
            ssPrint3_Sheet1.Cells[5, 3].Text = VB.Trim(txtExam03_01.Text);
            ssPrint3_Sheet1.Cells[6, 3].Text = VB.Trim(txtExam03_02.Text);
            ssPrint3_Sheet1.Cells[7, 3].Text = VB.Trim(txtExam03_03.Text);
            ssPrint3_Sheet1.Cells[8, 3].Text = VB.Trim(txtExam03_04.Text);
            ssPrint3_Sheet1.Cells[9, 3].Text = VB.Trim(txtExam03_05.Text);

            ssPrint3_Sheet1.Cells[11, 1].Text = VB.Trim(txtExam03_Remark.Text);
            
            ssPrint3_Sheet1.Cells[12, 1].Text = VB.Trim(txtExamDate.Text);
            ssPrint3_Sheet1.Cells[13, 1].Text = VB.Trim(txtDrName.Text);
            
            ssPrint3_Sheet1.Cells[12, 4].Text = VB.Trim(txtResultDate.Text);
            ssPrint3_Sheet1.Cells[12, 4].Text = VB.Trim(txtHospital.Text);
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strMCNO = "";

            if (e.RowHeader == true || e.ColumnHeader == true) return;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                strMCNO = ssView_Sheet1.Cells[e.Row, 3].Text;

                SQL = "SELECT PTNO, MCNO, JDATE, SEX, SABUN, SNAME, JUMIN1, JUMIN3, BUSE, JUSO, GUBUN,";
                SQL = SQL + ComNum.VBLF + " EXAM01_01, EXAM01_02, EXAM01_03, EXAM01_04, EXAM01_05,";
                SQL = SQL + ComNum.VBLF + " EXAM01_06, EXAM01_07, EXAM01_08, EXAM01_09, EXAM01_10, EXAM01_REMARK,";
                SQL = SQL + ComNum.VBLF + " EXAM02_01, EXAM02_02, EXAM02_03, EXAM02_04, EXAM02_05,";
                SQL = SQL + ComNum.VBLF + " EXAM02_06, EXAM02_07, EXAM02_08, EXAM02_09, EXAM02_10,  EXAM02_11, EXAM02_REMARK,";
                SQL = SQL + ComNum.VBLF + " EXAM03_01, EXAM03_02, EXAM03_03, EXAM03_04, EXAM03_05, EXAM03_REMARK,";
                SQL = SQL + ComNum.VBLF + " EXAM_DATE, RESULT_DATE , DRNAME, HOSPITAL, DRCODE, LICENSE,";
                SQL = SQL + ComNum.VBLF + " CHK_EXAM02_01, CHK_EXAM02_02, CHK_EXAM02_03, CHK_EXAM02_04 ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.OCS_MCCERTIFI28 ";
                SQL = SQL + ComNum.VBLF + "   WHERE MCNO = '" + strMCNO + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    tabControl1.SelectedTabIndex = dt.Rows[0]["GUBUN"].ToString().Trim() != "" ? (int)VB.Val(dt.Rows[0]["GUBUN"].ToString().Trim()) : 0;

                    txtPtNo.Text = dt.Rows[0]["PTNO"].ToString().Trim();
                    txtMcNo.Text = dt.Rows[0]["MCNO"].ToString().Trim();
                    if (VB.IsDate(dt.Rows[0]["JDATE"].ToString().Trim()) == true)
                    {
                        txtJDate.Text = Convert.ToDateTime(dt.Rows[0]["JDATE"].ToString().Trim()).ToShortDateString();
                    }
                    else
                    {
                        txtJDate.Text = ""; ;
                    }
                    txtSex.Text = (dt.Rows[0]["SEX"].ToString().Trim() == "M" ? "남자" : "여자");
                    txtSabun.Text = dt.Rows[0]["SABUN"].ToString().Trim();
                    txtSName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    txtJumin1.Text = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    txtJumin2.Text = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    txtBuse.Text = dt.Rows[0]["BUSE"].ToString().Trim();
                    txtJuso.Text = dt.Rows[0]["JUSO"].ToString().Trim();

                    txtExam01_01.Text = dt.Rows[0]["EXAM01_01"].ToString().Trim();
                    txtExam01_02.Text = dt.Rows[0]["EXAM01_02"].ToString().Trim();
                    txtExam01_03.Text = dt.Rows[0]["EXAM01_03"].ToString().Trim();
                    txtExam01_04.Text = dt.Rows[0]["EXAM01_04"].ToString().Trim();
                    txtExam01_05.Text = dt.Rows[0]["EXAM01_05"].ToString().Trim();
                    txtExam01_06.Text = dt.Rows[0]["EXAM01_06"].ToString().Trim();
                    txtExam01_07.Text = dt.Rows[0]["EXAM01_07"].ToString().Trim();
                    txtExam01_08.Text = dt.Rows[0]["EXAM01_08"].ToString().Trim();
                    txtExam01_09.Text = dt.Rows[0]["EXAM01_09"].ToString().Trim();
                    txtExam01_10.Text = dt.Rows[0]["EXAM01_10"].ToString().Trim();
                    txtExam01_Remark.Text = dt.Rows[0]["EXAM01_REMARK"].ToString().Trim();

                    txtExam02_01.Text = dt.Rows[0]["EXAM02_01"].ToString().Trim();
                    txtExam02_02.Text = dt.Rows[0]["EXAM02_02"].ToString().Trim();
                    txtExam02_03.Text = dt.Rows[0]["EXAM02_03"].ToString().Trim();
                    txtExam02_04.Text = dt.Rows[0]["EXAM02_04"].ToString().Trim();
                    txtExam02_05.Text = dt.Rows[0]["EXAM02_05"].ToString().Trim();
                    txtExam02_06.Text = dt.Rows[0]["EXAM02_06"].ToString().Trim();
                    txtExam02_07.Text = dt.Rows[0]["EXAM02_07"].ToString().Trim();
                    txtExam02_08.Text = dt.Rows[0]["EXAM02_08"].ToString().Trim();
                    txtExam02_09.Text = dt.Rows[0]["EXAM02_09"].ToString().Trim();
                    txtExam02_10.Text = dt.Rows[0]["EXAM02_10"].ToString().Trim();
                    txtExam02_11.Text = dt.Rows[0]["EXAM02_11"].ToString().Trim();
                    txtExam02_Remark.Text = dt.Rows[0]["EXAM02_REMARK"].ToString().Trim();

                    txtExam03_01.Text = dt.Rows[0]["EXAM03_01"].ToString().Trim();
                    txtExam03_02.Text = dt.Rows[0]["EXAM03_02"].ToString().Trim();
                    txtExam03_03.Text = dt.Rows[0]["EXAM03_03"].ToString().Trim();
                    txtExam03_04.Text = dt.Rows[0]["EXAM03_04"].ToString().Trim();
                    txtExam03_05.Text = dt.Rows[0]["EXAM03_05"].ToString().Trim();
                    txtExam03_Remark.Text = dt.Rows[0]["EXAM03_REMARK"].ToString().Trim();

                    txtExamDate.Text = dt.Rows[0]["EXAM_DATE"].ToString().Trim();
                    txtResultDate.Text = dt.Rows[0]["RESULT_DATE"].ToString().Trim();
                    txtDrName.Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                    txtHospital.Text = dt.Rows[0]["HOSPITAL"].ToString().Trim();
                    txtDrCode.Text = dt.Rows[0]["DRCODE"].ToString().Trim();
                    txtLicense.Text = dt.Rows[0]["LICENSE"].ToString().Trim();

                    if (dt.Rows[0]["CHK_EXAM02_01"].ToString().Trim() != "")
                    {
                        chkExam02_01.Checked = dt.Rows[0]["CHK_EXAM02_01"].ToString().Trim() == "1" ? true : false;
                    }
                    if (dt.Rows[0]["CHK_EXAM02_02"].ToString().Trim() != "")
                    {
                        chkExam02_02.Checked = dt.Rows[0]["CHK_EXAM02_02"].ToString().Trim() == "1" ? true : false;
                    }
                    if (dt.Rows[0]["CHK_EXAM02_03"].ToString().Trim() != "")
                    {
                        chkExam02_03.Checked = dt.Rows[0]["CHK_EXAM02_03"].ToString().Trim() == "1" ? true : false;
                    }
                    if (dt.Rows[0]["CHK_EXAM02_04"].ToString().Trim() != "")
                    {
                        chkExam02_04.Checked = dt.Rows[0]["CHK_EXAM02_04"].ToString().Trim() == "1" ? true : false;
                    }
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
        }

        private void GetData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strPano = "";

            strPano = VB.Trim(txtPtNo.Text);
            SCREEN_CLEAR();

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "SELECT JDATE, PANO, SNAME, SEX, JUMIN1, JUMIN3, BUSE, SABUN, JUSO ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_PATIENT_POSCO ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPano + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (VB.IsDate(dt.Rows[0]["JDATE"].ToString().Trim()) == true)
                    {
                        txtJDate.Text = Convert.ToDateTime(dt.Rows[0]["JDATE"].ToString().Trim()).ToShortDateString();
                    }
                    else
                    {
                        txtJDate.Text = ""; ;
                    }
                    txtSex.Text = (dt.Rows[i]["SEX"].ToString().Trim() == "M" ? "남자" : "여자");
                    txtSName.Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    txtJumin1.Text = dt.Rows[i]["JUMIN1"].ToString().Trim();
                    txtJumin2.Text = clsAES.DeAES(dt.Rows[i]["JUMIN3"].ToString().Trim());
                    txtBuse.Text = dt.Rows[i]["BUSE"].ToString().Trim();
                    txtSabun.Text = dt.Rows[i]["SABUN"].ToString().Trim();
                    txtJuso.Text = dt.Rows[i]["JUSO"].ToString().Trim();
                }

                txtHospital.Text = "포항성모병원";
                txtResultDate.Text = clsPublic.GstrSysDate;
                txtLicense.Text = READ_LICNO(VB.Trim(GnJobSabun));
                txtDrName.Text = clsVbfunc.GetInSaName(clsDB.DbCon, ComFunc.SetAutoZero(GnJobSabun, 5));


                //''TxtDrCode.Text = GetDrCode(TxtLicense.Text)
                txtDrCode.Text = GstrDrCode;

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(RESULT_DATE, 'YYYY-MM-DD') AS RESULT_DATE, DRNAME, SNAME, MCNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI28";
                SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + txtPtNo.Text.Trim() + "'";
                SQL = SQL + ComNum.VBLF + "         AND DRCODE = '" + txtDrCode.Text.Trim() + "'";
                SQL = SQL + ComNum.VBLF + "ORDER BY MCNO DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RESULT_DATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                    }
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
        }

        private void txtMcNo_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void txtMcNo_Leave(object sender, EventArgs e)
        {
            
        }

        private string READ_LICNO(string argSABUN)
        {
            string rtnVal = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT MYEN_BUNHO FROM ADMIN.INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + ComFunc.SetAutoZero(argSABUN, 5) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["MYEN_BUNHO"].ToString().Trim();
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

            return rtnVal;
        }

        private string GetMcNo_new()
        {
            string rtnVal = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT  ADMIN.SEQ_MCNO.NEXTVAL SEQ ";
                SQL = SQL + ComNum.VBLF + " FROM    DUAL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = ComFunc.SetAutoZero(dt.Rows[0]["SEQ"].ToString().Trim(), 8);
                }
                else
                {
                    rtnVal = "-1";
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

            return rtnVal;
        }

        private void txtPtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtPtNo_Leave(object sender, EventArgs e)
        {
            GetData();
        }

        void setCtrlLoad(System.Windows.Forms.Panel pan, System.Windows.Forms.Form frm)
        {

            pan.Controls.Clear();


            frm.TopLevel = false;
            frm.Dock = System.Windows.Forms.DockStyle.Fill;
            frm.ControlBox = false;
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            frm.Show();

            pan.Controls.Add(frm);

        }

        void eSpliterClick(object sender, EventArgs e)
        {
            ExpandableSplitter o = (ExpandableSplitter)sender;

            if (sender == exSpliter2)
            {
                if (o.Expanded == true)
                {
                    pan_resultM.Size = new System.Drawing.Size(10, 981);
                }
                else
                {
                    pan_resultM.Size = new System.Drawing.Size(950, 981);
                }
            }
            

        }

        private void frmPmpaViewPoscoT_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(fEmrViewer != null)
            {
                fEmrViewer.Dispose();
                fEmrViewer = null;
            }
        }

        private void btnSearchFav_Click(object sender, EventArgs e)
        {
            frmMcrtJobBas02 frmBas02 = new frmMcrtJobBas02();
            frmBas02.StartPosition = FormStartPosition.CenterParent;
            frmBas02.ShowDialog();
        }

        private void frmMcrtJobViewFav_SendMsg(string strMsg)
        {
            if (mtxtMcrt == null) return;

            mtxtMcrt.Text = strMsg;
            mtxtMcrt.Focus();
        }

        private void btnSearchCause_Click(object sender, EventArgs e)
        {
            txtExam02_08.Focus();
            mtxtMcrt = txtExam02_08;

            frmMcrtJobViewFav frmMcrtJobViewFavEvent = new frmMcrtJobViewFav("28", "45");
            frmMcrtJobViewFavEvent.rSendMsg += new frmMcrtJobViewFav.SendMsg(frmMcrtJobViewFav_SendMsg);
            frmMcrtJobViewFavEvent.StartPosition = FormStartPosition.CenterParent;
            frmMcrtJobViewFavEvent.ShowDialog();
            frmMcrtJobViewFavEvent.Dispose();
            frmMcrtJobViewFavEvent = null;
        }
        private void btnSearchCause2_Click(object sender, EventArgs e)
        {
            txtExam02_09.Focus();
            mtxtMcrt = txtExam02_09;

            frmMcrtJobViewFav frmMcrtJobViewFavEvent = new frmMcrtJobViewFav("28", "46");
            frmMcrtJobViewFavEvent.rSendMsg += new frmMcrtJobViewFav.SendMsg(frmMcrtJobViewFav_SendMsg);
            frmMcrtJobViewFavEvent.StartPosition = FormStartPosition.CenterParent;
            frmMcrtJobViewFavEvent.ShowDialog();
            frmMcrtJobViewFavEvent.Dispose();
            frmMcrtJobViewFavEvent = null;
        }

        private void btnSearchCause3_Click(object sender, EventArgs e)
        {
            txtExam02_10.Focus();
            mtxtMcrt = txtExam02_10;

            frmMcrtJobViewFav frmMcrtJobViewFavEvent = new frmMcrtJobViewFav("28", "47");
            frmMcrtJobViewFavEvent.rSendMsg += new frmMcrtJobViewFav.SendMsg(frmMcrtJobViewFav_SendMsg);
            frmMcrtJobViewFavEvent.StartPosition = FormStartPosition.CenterParent;
            frmMcrtJobViewFavEvent.ShowDialog();
            frmMcrtJobViewFavEvent.Dispose();
            frmMcrtJobViewFavEvent = null;
        }

        private void btnSearchCause4_Click(object sender, EventArgs e)
        {
            txtExam02_11.Focus();
            mtxtMcrt = txtExam02_11;

            frmMcrtJobViewFav frmMcrtJobViewFavEvent = new frmMcrtJobViewFav("28", "48");
            frmMcrtJobViewFavEvent.rSendMsg += new frmMcrtJobViewFav.SendMsg(frmMcrtJobViewFav_SendMsg);
            frmMcrtJobViewFavEvent.StartPosition = FormStartPosition.CenterParent;
            frmMcrtJobViewFavEvent.ShowDialog();
            frmMcrtJobViewFavEvent.Dispose();
            frmMcrtJobViewFavEvent = null;
        }
    }
}
