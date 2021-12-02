using ComBase;
using FarPoint.Win.Spread;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// 2020-02-26 
    /// 신규 폼(이중 차트 신청서)
    /// 시퀀스: KOSMOS_PMPA.SEQ_DBLAPPNO.CURRVAL
    /// 테이블: KOSMOS_PMPA.MID_DBLCHART_APPLICATION
    /// </summary>
    public partial class frmEmrJobConvPano_Application : Form, MainFormMessage
    {
        #region //MainFormMessage
        string mPara1 = string.Empty;
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

        #region 생성자
        public frmEmrJobConvPano_Application()
        {
            InitializeComponent();
        }

        public frmEmrJobConvPano_Application(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;

        }

        public frmEmrJobConvPano_Application(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }
        #endregion

        #region 스프레드 위치
        private enum ApplicationList
        {
            /// <summary>
            /// 신청일
            /// </summary>
            ReqDate,
            /// <summary>
            /// 신청부서
            /// </summary>
            ReqBuse,
            /// <summary>
            /// 신청자
            /// </summary>
            ReqSabun,
            /// <summary>
            /// 등록번호1
            /// </summary>
            Pano1,
            /// <summary>
            /// 등록번호2
            /// </summary>
            Pano2,
            /// <summary>
            /// 등록번호3
            /// </summary>
            Pano3,
            /// <summary>
            /// 등록번호4
            /// </summary>
            Pano4,
            /// <summary>
            /// 등록번호5
            /// </summary>
            Pano5,
            /// <summary>
            /// 내선번호
            /// </summary>
            Tel,
            /// <summary>
            /// 구분
            /// </summary>
            Gbn,
            /// <summary>
            /// 처리결과
            /// </summary>
            Result
        }
        #endregion

        #region 폼 변수
        /// <summary>
        /// 신청서 시퀀스
        /// </summary>
        string strSeqNo = string.Empty;

        /// <summary>
        /// 의료정보팀 직원 리스트
        /// </summary>
        List<string> lstSabun = null;
        #endregion

        #region 폼 이벤트
        private void frmEmrJobConvPano_Application_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y");

            if (clsType.User.BuseCode.Equals("044201") == false)
            {
                tabControl1.TabPages.Remove(tabControl1.TabPages[2]);
            }

            ssListIncomplete_Sheet1.RowCount = 0;
            ssListComplete_Sheet1.RowCount = 0;
            ssDel_Sheet1.RowCount = 0;

            lstSabun = new List<string>();

            cboReceptionist.Items.Insert(0, "  ");
            cboConfirm1.Items.Insert(0, "  ");

            #region 사용삭제 콤보박스
            cboUse1.Items.Add(" ");
            cboUse1.Items.Add("사용");
            cboUse1.Items.Add("삭제");
            cboUse1.Items.Add("폐기");
            cboUse1.SelectedIndex = 0;

            cboUse2.Items.Add(" ");
            cboUse2.Items.Add("사용");
            cboUse2.Items.Add("삭제");
            cboUse2.Items.Add("폐기");
            cboUse2.SelectedIndex = 0;

            cboUse3.Items.Add(" ");
            cboUse3.Items.Add("사용");
            cboUse3.Items.Add("삭제");
            cboUse3.Items.Add("폐기");
            cboUse3.SelectedIndex = 0;

            cboUse4.Items.Add(" ");
            cboUse4.Items.Add("사용");
            cboUse4.Items.Add("삭제");
            cboUse4.Items.Add("폐기");
            cboUse4.SelectedIndex = 0;

            cboUse5.Items.Add(" ");
            cboUse5.Items.Add("사용");
            cboUse5.Items.Add("삭제");
            cboUse5.Items.Add("폐기");
            cboUse5.SelectedIndex = 0;
            #endregion

            SetMedicalList();
            FormClear();
            //SetBuseList();

            if (clsType.User.BuseCode.Equals("044201") == false)
            {
                panWrite.Height = 362;
            }

            dtpEdate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            dtpSdate.Value = dtpEdate.Value.AddDays(-7);

            GetSearchData();
            NewWrite();
        }

        private void frmEmrJobConvPano_Application_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmEmrJobConvPano_Application_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }
        #endregion

        #region 함수 모음
        /// <summary>
        /// 의료정보팀 부서원 리스트
        /// </summary>
        private void SetMedicalList()
        {
            #region 의료정보팀이 아니면 콤보박스에 바로 추가하고 쿼리 날리지 않음.
            if (clsType.User.BuseCode.Equals("044201") == false)
            {
                return;
            }
            #endregion

            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int index = 0;

            try
            {
                SQL = "SELECT A.KORNAME, A.SABUN3";
                SQL += ComNum.VBLF + " FROM KOSMOS_ADM.INSA_MST A";
                SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_ADM.INSA_CODE B";
                SQL += ComNum.VBLF + "      ON A.JIK  = B.CODE";
                SQL += ComNum.VBLF + "     AND B.GUBUN  = '2'";
                SQL += ComNum.VBLF + "WHERE BUSE  = '044201'";
                SQL += ComNum.VBLF + "  AND TOIDAY  IS NULL";
                SQL += ComNum.VBLF + "ORDER BY B.SORT, B.SORT2 ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cboConfirm1.Items.Add(reader.GetValue(0).ToString().Trim() + "                      ." + reader.GetValue(1).ToString().Trim());
                        cboReceptionist.Items.Add(reader.GetValue(0).ToString().Trim() + "                      ." + reader.GetValue(1).ToString().Trim());
                        lstSabun.Add(reader.GetValue(1).ToString().Trim());
                    }
                }

                reader.Dispose();
                cboReceptionist.SelectedIndex = index;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }


        /// <summary>
        /// 입력화면 초기화
        /// </summary>
        private void FormClear()
        {
            DateTime dtpSysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            strSeqNo = string.Empty;

            btnDelete.Text = "삭제";

            dtpEdate.Value = dtpSysDate;
            dtpSdate.Value = dtpSysDate.AddDays(-7);
            txtUsePano.Clear();

            cboReceptionist.SelectedIndex = 0;
            foreach (Control control in ComFunc.GetAllControls(panApplication))
            {
                if (control is TextBox)
                {                   
                    control.Text = "";
                    control.Tag = "";
                }
                else if (control is RadioButton)
                {
                    (control as RadioButton).Checked = false;
                }
                else if (control is DateTimePicker)
                {
                    (control as DateTimePicker).Value = dtpSysDate;

                    if ((control as DateTimePicker).ShowCheckBox)
                    {
                        (control as DateTimePicker).Checked = false;
                    }
                }
                else if (control is ComboBox)
                {
                    (control as ComboBox).SelectedIndex = 0;
                }
            }

            rdoDuplication4.Checked = true;

        }

        /// <summary>
        /// 스프레드 클리어
        /// </summary>
        private void Spread_Clear()
        {
            #region 신청라인
            //신청일
            ssPrint_Sheet1.Cells[2, 2].Text = string.Empty;
            //신청부서
            ssPrint_Sheet1.Cells[2, 8].Text = string.Empty;
            //신청자
            ssPrint_Sheet1.Cells[2, 13].Text = string.Empty;
            //연락처
            ssPrint_Sheet1.Cells[2, 19].Text = string.Empty;
            #endregion

            #region 사용삭제 ~ 처리내용 까지
            //3: 사용삭제
            //4: 차트번호
            //5: 성명
            //6: 주민번호
            //7: 기타사항
            //9: 발생내용
            //10: 처리내용
            ssPrint_Sheet1.Cells[3, 2, 10, 19].Text = string.Empty;
            #endregion

            #region 접수라인
            //접수일
            ssPrint_Sheet1.Cells[11, 2].Text = string.Empty;
            //접수자
            ssPrint_Sheet1.Cells[11, 10].Text = string.Empty;
            //완료일
            ssPrint_Sheet1.Cells[11, 18].Text = string.Empty;
            #endregion

            #region 이중구분
            ssPrint_Sheet1.Cells[12, 2].Text = string.Empty;
            #endregion

            #region 담당자 확인/메모
            ssPrint_Sheet1.Cells[14, 2, 15, 19].Text = string.Empty;
            #endregion
        }

        /// <summary>
        /// 환자정보(이름, 주민번호)
        /// </summary>
        private void SetPatInfo(string strPano, TextBox txtName, TextBox txtJm1, TextBox txtJm3)
        {

            if (txtName == null || txtJumin1 == null || txtJumin3 == null)
                return;

            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            #endregion

            try
            {
                #region 쿼리
                SQL += ComNum.VBLF + "SELECT SNAME, JUMIN1, JUMIN3";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL += ComNum.VBLF + "WHERE PANO = '" +  VB.Val(strPano).ToString("00000000") + "'";
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    txtName.Text   = reader.GetValue(0).ToString().Trim();
                    txtJm1.Text = reader.GetValue(1).ToString().Trim();
                    txtJm3.Tag = clsAES.DeAES(reader.GetValue(2).ToString().Trim());
                    txtJm3.Text = clsType.User.BuseCode.Equals("044201") ? txtJm3.Tag.ToString() : txtJm3.Tag.ToString().Substring(0, 1);

                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 조회 함수
        /// </summary>
        private void GetSearchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            ssListComplete_Sheet1.RowCount = 0;
            ssListIncomplete_Sheet1.RowCount = 0;
            ssDel_Sheet1.RowCount = 0;

            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            string strBuse = cboBuse.Text.Substring(cboBuse.Text.LastIndexOf(".") + 1);
            #endregion

            try
            {
                #region 쿼리

                #region 미완료
                SQL = "SELECT '신청' AS GBN, SEQNO, TO_CHAR(REQDATE, 'YYYY-MM-DD') REQDATE, REQBUSE, B.NAME AS BUSENAME, REQSABUN, U.USERNAME AS REQNAME,";
                SQL += ComNum.VBLF + "TEL, PANO1, PANO2, DOUBLEGBN, PANO3, '' AS ITCONFIRM, '' AS PRTYN ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MID_DBLCHART_APPLICATION A";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_BUSE B";
                SQL += ComNum.VBLF + "     ON A.REQBUSE = B.BUCODE";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_USER U";
                SQL += ComNum.VBLF + "     ON A.REQSABUN = U.IDNUMBER";
                SQL += ComNum.VBLF + "WHERE REQDATE >= TO_DATE('" + dtpSdate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') --";
                SQL += ComNum.VBLF + "  AND REQDATE <= TO_DATE('" + dtpEdate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') --";
                SQL += ComNum.VBLF + "  AND A.DELDATE IS NULL --삭제 제외";
                SQL += ComNum.VBLF + "  AND COMPLETEDATE IS NULL -- 미완료";
                if (string.IsNullOrWhiteSpace(txtUsePano.Text) == false)
                {
                    SQL += ComNum.VBLF + "  AND EXISTS";
                    SQL += ComNum.VBLF + "  (";
                    SQL += ComNum.VBLF + "  SELECT 1";
                    SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "MID_DBLCHART_APPLICATION";
                    SQL += ComNum.VBLF + "   WHERE SEQNO = A.SEQNO";
                    SQL += ComNum.VBLF + "     AND";
                    SQL += ComNum.VBLF + "       (";
                    SQL += ComNum.VBLF + "          (USE1 = '사용' AND PANO1 = '" + txtUsePano.Text.Trim() + "') OR";
                    SQL += ComNum.VBLF + "          (USE2 = '사용' AND PANO2 = '" + txtUsePano.Text.Trim() + "') OR";
                    SQL += ComNum.VBLF + "          (USE3 = '사용' AND PANO3 = '" + txtUsePano.Text.Trim() + "') OR";
                    SQL += ComNum.VBLF + "          (USE4 = '사용' AND PANO4 = '" + txtUsePano.Text.Trim() + "') OR";
                    SQL += ComNum.VBLF + "          (USE5 = '사용' AND PANO5 = '" + txtUsePano.Text.Trim() + "')";
                    SQL += ComNum.VBLF + "       )";
                    SQL += ComNum.VBLF + "  )";
                }
                #endregion

                #region 완료
                SQL += ComNum.VBLF + "UNION ALL";
                SQL += ComNum.VBLF + "SELECT '완료' AS GBN, SEQNO, TO_CHAR(REQDATE, 'YYYY-MM-DD') REQDATE, REQBUSE, B.NAME AS BUSENAME, REQSABUN, U.USERNAME AS REQNAME,";
                SQL += ComNum.VBLF + "TEL, (SELECT (DECODE(USE1, '사용', PANO1, NULL) || DECODE(USE2, '사용', PANO2, NULL) || DECODE(USE3, '사용', PANO3, NULL)";
                SQL += ComNum.VBLF + "              || DECODE(USE4, '사용', PANO4, NULL) || DECODE(USE5, '사용', PANO5, NULL))";
                SQL += ComNum.VBLF + "        FROM KOSMOS_PMPA.MID_DBLCHART_APPLICATION";
                SQL += ComNum.VBLF + "       WHERE SEQNO = A.SEQNO";
                SQL += ComNum.VBLF + "         AND ROWNUM = 1";
                SQL += ComNum.VBLF + "     ) AS PANO1,";
                SQL += ComNum.VBLF + "     (SELECT (DECODE(USE1, '사용', SNAME1, NULL) || DECODE(USE2, '사용', SNAME2, NULL) || DECODE(USE3, '사용', SNAME3, NULL)";
                SQL += ComNum.VBLF + "              || DECODE(USE4, '사용', SNAME4, NULL) || DECODE(USE5, '사용', SNAME5, NULL)) AS SNAME";
                SQL += ComNum.VBLF + "        FROM KOSMOS_PMPA.MID_DBLCHART_APPLICATION";
                SQL += ComNum.VBLF + "       WHERE SEQNO = A.SEQNO";
                SQL += ComNum.VBLF + "         AND ROWNUM = 1";
                SQL += ComNum.VBLF + "     ) AS PANO2,";
                SQL += ComNum.VBLF + "DOUBLEGBN, TO_CHAR(COMPLETEDATE, 'YYYY-MM-DD') COMPLETEDATE, U2.USERNAME AS ITCONFIRM , PRTYN ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MID_DBLCHART_APPLICATION A";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_BUSE B";
                SQL += ComNum.VBLF + "     ON A.REQBUSE = B.BUCODE";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_USER U  -- 신청자";
                SQL += ComNum.VBLF + "     ON A.REQSABUN = U.IDNUMBER";
                SQL += ComNum.VBLF + "   LEFT JOIN KOSMOS_PMPA.BAS_USER U2 -- 작업자";
                SQL += ComNum.VBLF + "     ON A.ITCONFIRM = U2.IDNUMBER";
                SQL += ComNum.VBLF + "WHERE REQDATE >= TO_DATE('" + dtpSdate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') --";
                SQL += ComNum.VBLF + "  AND REQDATE <= TO_DATE('" + dtpEdate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') --";
                SQL += ComNum.VBLF + "  AND A.DELDATE IS NULL --삭제 제외";
                SQL += ComNum.VBLF + "  AND COMPLETEDATE IS NOT NULL -- 완료";

                if (string.IsNullOrWhiteSpace(txtUsePano.Text) == false)
                {
                    SQL += ComNum.VBLF + "  AND EXISTS";
                    SQL += ComNum.VBLF + "  (";
                    SQL += ComNum.VBLF + "  SELECT 1";
                    SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "MID_DBLCHART_APPLICATION";
                    SQL += ComNum.VBLF + "   WHERE SEQNO = A.SEQNO";
                    SQL += ComNum.VBLF + "     AND";
                    SQL += ComNum.VBLF + "       (";
                    SQL += ComNum.VBLF + "          (USE1 = '사용' AND PANO1 = '" + txtUsePano.Text.Trim() + "') OR";
                    SQL += ComNum.VBLF + "          (USE2 = '사용' AND PANO2 = '" + txtUsePano.Text.Trim() + "') OR";
                    SQL += ComNum.VBLF + "          (USE3 = '사용' AND PANO3 = '" + txtUsePano.Text.Trim() + "') OR";
                    SQL += ComNum.VBLF + "          (USE4 = '사용' AND PANO4 = '" + txtUsePano.Text.Trim() + "') OR";
                    SQL += ComNum.VBLF + "          (USE5 = '사용' AND PANO5 = '" + txtUsePano.Text.Trim() + "')";
                    SQL += ComNum.VBLF + "       )";
                    SQL += ComNum.VBLF + "  )";
                }
                #endregion

                #region 삭제
                if (clsType.User.BuseCode.Equals("044201"))
                {
                    SQL += ComNum.VBLF + "UNION ALL";
                    SQL += ComNum.VBLF + "SELECT '삭제' AS GBN, SEQNO, TO_CHAR(REQDATE, 'YYYY-MM-DD') REQDATE, REQBUSE, B.NAME AS BUSENAME, REQSABUN, U.USERNAME AS REQNAME,";
                    SQL += ComNum.VBLF + "TEL, PANO1, PANO2, DOUBLEGBN, PANO3, '' AS ITCONFIRM , '' AS PRTYN";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MID_DBLCHART_APPLICATION A";
                    SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_BUSE B";
                    SQL += ComNum.VBLF + "     ON A.REQBUSE = B.BUCODE";
                    SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_USER U";
                    SQL += ComNum.VBLF + "     ON A.REQSABUN = U.IDNUMBER";
                    SQL += ComNum.VBLF + "WHERE REQDATE >= TO_DATE('" + dtpSdate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') --";
                    SQL += ComNum.VBLF + "  AND REQDATE <= TO_DATE('" + dtpEdate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') --";
                    SQL += ComNum.VBLF + "  AND A.DELDATE IS NOT NULL --삭제만";
                    if (string.IsNullOrWhiteSpace(txtUsePano.Text) == false)
                    {
                        SQL += ComNum.VBLF + "  AND EXISTS";
                        SQL += ComNum.VBLF + "  (";
                        SQL += ComNum.VBLF + "  SELECT 1";
                        SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "MID_DBLCHART_APPLICATION";
                        SQL += ComNum.VBLF + "   WHERE SEQNO = A.SEQNO";
                        SQL += ComNum.VBLF + "     AND";
                        SQL += ComNum.VBLF + "       (";
                        SQL += ComNum.VBLF + "          (USE1 = '사용' AND PANO1 = '" + txtUsePano.Text.Trim() + "') OR";
                        SQL += ComNum.VBLF + "          (USE2 = '사용' AND PANO2 = '" + txtUsePano.Text.Trim() + "') OR";
                        SQL += ComNum.VBLF + "          (USE3 = '사용' AND PANO3 = '" + txtUsePano.Text.Trim() + "') OR";
                        SQL += ComNum.VBLF + "          (USE4 = '사용' AND PANO4 = '" + txtUsePano.Text.Trim() + "') OR";
                        SQL += ComNum.VBLF + "          (USE5 = '사용' AND PANO5 = '" + txtUsePano.Text.Trim() + "')";
                        SQL += ComNum.VBLF + "       )";
                        SQL += ComNum.VBLF + "  )";
                    }
                }
                #endregion

                SQL += ComNum.VBLF + "ORDER BY REQDATE";

                #endregion

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SheetView sheet = null;
                    string strGubun = string.Empty;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["GBN"].ToString().Trim().Equals("신청"))
                        {
                            sheet = ssListIncomplete_Sheet1;
                        }
                        else if(dt.Rows[i]["GBN"].ToString().Trim().Equals("완료"))
                        {
                            sheet = ssListComplete_Sheet1;
                        }
                        else
                        {
                            sheet = ssDel_Sheet1;
                        }


                        switch (dt.Rows[i]["DOUBLEGBN"].ToString().Trim())
                        {
                            case "0":
                                strGubun = "이중";
                                break;
                            case "1":
                                strGubun = "폐기";
                                break;
                            case "2":
                                strGubun = "부분이중";
                                break;
                            case "3":
                                strGubun = "취소";
                                break;
                            case "4":
                                strGubun = "처리중";
                                break;
                        }

                        sheet.RowCount += 1;
                        sheet.Cells[sheet.RowCount - 1, 0].Text = dt.Rows[i]["REQDATE"].ToString().Trim();
                        sheet.Cells[sheet.RowCount - 1, 1].Tag = dt.Rows[i]["REQBUSE"].ToString().Trim();
                        sheet.Cells[sheet.RowCount - 1, 1].Text = dt.Rows[i]["BUSENAME"].ToString().Trim();
                        sheet.Cells[sheet.RowCount - 1, 2].Tag = dt.Rows[i]["REQSABUN"].ToString().Trim();
                        sheet.Cells[sheet.RowCount - 1, 2].Text = dt.Rows[i]["REQNAME"].ToString().Trim();

                        if (dt.Rows[i]["GBN"].ToString().Trim().Equals("완료"))
                        {
                            sheet.Cells[sheet.RowCount - 1, 3].Text = dt.Rows[i]["TEL"].ToString().Trim();
                            sheet.Cells[sheet.RowCount - 1, 4].Text = dt.Rows[i]["PANO1"].ToString().Trim();
                            sheet.Cells[sheet.RowCount - 1, 5].Text = dt.Rows[i]["PANO2"].ToString().Trim();
                            sheet.Cells[sheet.RowCount - 1, 6].Text = strGubun;
                            sheet.Cells[sheet.RowCount - 1, 8].Text = dt.Rows[i]["PANO3"].ToString().Trim();
                            sheet.Cells[sheet.RowCount - 1, 9].Text = dt.Rows[i]["ITCONFIRM"].ToString().Trim();
                        }
                        else
                        {
                            sheet.Cells[sheet.RowCount - 1, 3].Text = dt.Rows[i]["PANO1"].ToString().Trim();
                            sheet.Cells[sheet.RowCount - 1, 4].Text = dt.Rows[i]["PANO2"].ToString().Trim();
                            sheet.Cells[sheet.RowCount - 1, 5].Text = dt.Rows[i]["PANO3"].ToString().Trim();
                            sheet.Cells[sheet.RowCount - 1, 8].Text = dt.Rows[i]["TEL"].ToString().Trim();
                            sheet.Cells[sheet.RowCount - 1, 9].Text = strGubun;
                        }

                        sheet.Cells[sheet.RowCount - 1, 11].Text = dt.Rows[i]["SEQNO"].ToString().Trim();

                        if (dt.Rows[i]["PRTYN"].ToString().Trim() == "" && dt.Rows[i]["GBN"].ToString().Trim().Equals("완료"))
                        {
                            sheet.Cells[sheet.RowCount - 1, 9].BackColor = Color.FromArgb(255, 255, 128);
                        }
                        else if (dt.Rows[i]["PRTYN"].ToString().Trim() == "Y" && dt.Rows[i]["GBN"].ToString().Trim().Equals("완료"))
                        {
                            sheet.Cells[sheet.RowCount - 1, 9].BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                }

                dt.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 시퀀스로 조회후 데이터 뿌려줌
        /// </summary>
        private bool SetAppData()
        {
            bool rtnVal = false;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return rtnVal;
            }

            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            #endregion

            try
            {
                #region 쿼리
                SQL = "SELECT SEQNO, REQDATE, REQBUSE, B.NAME AS BUSENAME, REQSABUN, U.USERNAME AS REQNAME,  ";
                SQL += ComNum.VBLF + "TEL, USE1, USE2, USE3, USE4, USE5,";
                SQL += ComNum.VBLF + "PANO1,PANO2,PANO3,PANO4,PANO5,";
                SQL += ComNum.VBLF + "SNAME1, SNAME2, SNAME3, SNAME4, SNAME5,";
                SQL += ComNum.VBLF + "JUMIN1, JUMIN1_2, JUMIN2, JUMIN2_2, JUMIN3, JUMIN3_2, JUMIN4, JUMIN4_2, JUMIN5, JUMIN5_2,";
                SQL += ComNum.VBLF + "ETC1, ETC2, ETC3, ETC4, ETC5,";
                SQL += ComNum.VBLF + "HAPPEN, CONDUCT,";
                SQL += ComNum.VBLF + "SUBMITDATE, RECEPTIONIST, COMPLETEDATE,";
                SQL += ComNum.VBLF + "DOUBLEGBN, ";
                SQL += ComNum.VBLF + "ITCONFIRM, ITCONFIRM_MEMO,";
                SQL += ComNum.VBLF + "RADIATIONCONFIRM,RADIATIONCONFIRM_MEMO,";
                SQL += ComNum.VBLF + "CHARTCONFIRM,CHARTCONFIRM_MEMO,";
                SQL += ComNum.VBLF + "ETCCONFIRM,ETCCONFIRM_MEMO,";
                SQL += ComNum.VBLF + "HEADCONFIRM";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MID_DBLCHART_APPLICATION A";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_BUSE B";
                SQL += ComNum.VBLF + "     ON A.REQBUSE = B.BUCODE";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_USER U";
                SQL += ComNum.VBLF + "     ON A.REQSABUN = U.IDNUMBER";
                SQL += ComNum.VBLF + "WHERE SEQNO = " + strSeqNo;
                #endregion

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (clsType.User.BuseCode.Equals("044201") == false)
                    {
                        switch(dt.Rows[0]["REQBUSE"].ToString().Trim())
                        {                          
                            //원무팀
                            case "077401":
                            case "077402":
                            case "077403":
                            case "077404":
                                if (clsType.User.BuseCode.Equals("077401") || clsType.User.BuseCode.Equals("077402") || 
                                    clsType.User.BuseCode.Equals("077403") || clsType.User.BuseCode.Equals("077404"))
                                {

                                }
                                else
                                {
                                    dt.Dispose();
                                    return rtnVal;
                                }
                                break;
                            default:
                                if (dt.Rows[0]["REQBUSE"].ToString().Trim().Equals(clsType.User.BuseCode) == false)
                                {
                                    dt.Dispose();
                                    return rtnVal;
                                }
                                break;
                        }
                    }

                    dtpReqDate.Value = Convert.ToDateTime(dt.Rows[0]["REQDATE"].ToString().Trim());
                    txtReqBuse.Text = dt.Rows[0]["BUSENAME"].ToString().Trim();
                    txtReqBuse.Tag = dt.Rows[0]["REQBUSE"].ToString().Trim();

                    txtReqName.Text = dt.Rows[0]["REQNAME"].ToString().Trim();
                    txtReqName.Tag = dt.Rows[0]["REQSABUN"].ToString().Trim();

                    txtTel.Text = dt.Rows[0]["TEL"].ToString().Trim();

                    #region 차트번호1~5
                    txtPano1.Text = dt.Rows[0]["PANO1"].ToString().Trim();
                    txtPano2.Text = dt.Rows[0]["PANO2"].ToString().Trim();
                    txtPano3.Text = dt.Rows[0]["PANO3"].ToString().Trim();
                    txtPano4.Text = dt.Rows[0]["PANO4"].ToString().Trim();
                    txtPano5.Text = dt.Rows[0]["PANO5"].ToString().Trim();
                    #endregion

                    #region  이름1~5
                    txtName1.Text = dt.Rows[0]["SNAME1"].ToString().Trim();
                    txtName2.Text = dt.Rows[0]["SNAME2"].ToString().Trim();
                    txtName3.Text = dt.Rows[0]["SNAME3"].ToString().Trim();
                    txtName4.Text = dt.Rows[0]["SNAME4"].ToString().Trim();
                    txtName5.Text = dt.Rows[0]["SNAME5"].ToString().Trim();
                    #endregion

                    #region 주민번호1~5
                    txtJumin1.Text = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    txtJumin1_2.Tag = clsAES.DeAES(dt.Rows[0]["JUMIN1_2"].ToString().Trim());
                    
                    txtJumin2.Text = dt.Rows[0]["JUMIN2"].ToString().Trim();
                    txtJumin2_2.Tag = clsAES.DeAES(dt.Rows[0]["JUMIN2_2"].ToString().Trim());

                    txtJumin3.Text = dt.Rows[0]["JUMIN3"].ToString().Trim();
                    txtJumin3_2.Tag = clsAES.DeAES(dt.Rows[0]["JUMIN3_2"].ToString().Trim());

                    txtJumin4.Text = dt.Rows[0]["JUMIN4"].ToString().Trim();
                    txtJumin4_2.Tag = clsAES.DeAES(dt.Rows[0]["JUMIN4_2"].ToString().Trim());

                    txtJumin5.Text = dt.Rows[0]["JUMIN5"].ToString().Trim();
                    txtJumin5_2.Tag = clsAES.DeAES(dt.Rows[0]["JUMIN5_2"].ToString().Trim());
                    if (clsType.User.BuseCode.Equals("044201"))
                    {
                        txtJumin1_2.Text = txtJumin1_2.Tag.ToString();
                        txtJumin2_2.Text = txtJumin2_2.Tag.ToString();
                        txtJumin3_2.Text = txtJumin3_2.Tag.ToString();
                        txtJumin4_2.Text = txtJumin4_2.Tag.ToString();
                        txtJumin5_2.Text = txtJumin5_2.Tag.ToString();
                    }
                    else
                    {
                        txtJumin2_2.Text = string.IsNullOrWhiteSpace(txtJumin2_2.Tag.ToString()) ? "" : txtJumin2_2.Tag.ToString().Substring(0, 1);
                        txtJumin3_2.Text = string.IsNullOrWhiteSpace(txtJumin3_2.Tag.ToString()) ? "" : txtJumin3_2.Tag.ToString().Substring(0, 1);
                        txtJumin4_2.Text = string.IsNullOrWhiteSpace(txtJumin4_2.Tag.ToString()) ? "" : txtJumin4_2.Tag.ToString().Substring(0, 1);
                        txtJumin5_2.Text = string.IsNullOrWhiteSpace(txtJumin5_2.Tag.ToString()) ? "" : txtJumin5_2.Tag.ToString().Substring(0, 1);
                    }
                    #endregion

                    #region 사용삭제 1~5
                    cboUse1.SelectedIndex = cboUse1.FindString(dt.Rows[0]["USE1"].ToString().Trim());
                    cboUse2.SelectedIndex = cboUse2.FindString(dt.Rows[0]["USE2"].ToString().Trim());
                    cboUse3.SelectedIndex = cboUse3.FindString(dt.Rows[0]["USE3"].ToString().Trim());
                    cboUse4.SelectedIndex = cboUse4.FindString(dt.Rows[0]["USE4"].ToString().Trim());
                    cboUse5.SelectedIndex = cboUse5.FindString(dt.Rows[0]["USE5"].ToString().Trim());
                    #endregion

                    #region  기타사항1~5
                    txtEtc1.Text = dt.Rows[0]["ETC1"].ToString().Trim();
                    txtEtc2.Text = dt.Rows[0]["ETC2"].ToString().Trim();
                    txtEtc3.Text = dt.Rows[0]["ETC3"].ToString().Trim();
                    txtEtc4.Text = dt.Rows[0]["ETC4"].ToString().Trim();
                    txtEtc5.Text = dt.Rows[0]["ETC5"].ToString().Trim();
                    #endregion

                    #region 발생내용
                    txtHappen.Text = dt.Rows[0]["Happen"].ToString().Trim();
                    #endregion

                    #region 의료정보팀만
                    //if (clsType.User.BuseCode.Equals("044201"))
                    //{
                        #region 처리내용
                        txtConduct.Text = dt.Rows[0]["Conduct"].ToString().Trim();
                        #endregion

                        #region 접수라인
                        if (string.IsNullOrWhiteSpace(dt.Rows[0]["SUBMITDATE"].ToString().Trim()))
                        {
                            dtpSubmit.Checked = false;
                        }
                        else
                        {
                            dtpSubmit.Checked = true;
                            dtpSubmit.Value = Convert.ToDateTime(dt.Rows[0]["SUBMITDATE"].ToString().Trim());
                        }

                        if (string.IsNullOrWhiteSpace(dt.Rows[0]["RECEPTIONIST"].ToString().Trim()) == false)
                        {
                            cboReceptionist.SelectedIndex = lstSabun.IndexOf(dt.Rows[0]["RECEPTIONIST"].ToString().Trim()) + 1;
                        }

                        if (string.IsNullOrWhiteSpace(dt.Rows[0]["COMPLETEDATE"].ToString().Trim()))
                        {
                            dtpComplete.Checked = false;
                        }
                        else
                        {
                            dtpComplete.Checked = true;
                            dtpComplete.Value = Convert.ToDateTime(dt.Rows[0]["COMPLETEDATE"].ToString().Trim());
                        }
                        txtConduct.Text = dt.Rows[0]["Conduct"].ToString().Trim();
                        #endregion

                        #region 이중구분
                        switch (dt.Rows[0]["DOUBLEGBN"].ToString().Trim())
                        {
                            case "0":
                                rdoDuplication0.Checked = true;
                                break;
                            case "1":
                                rdoDuplication1.Checked = true;
                                break;
                            case "2":
                                rdoDuplication2.Checked = true;
                                break;
                            case "3":
                                rdoDuplication3.Checked = true;
                                break;
                            case "4":
                                rdoDuplication4.Checked = true;
                                break;
                        }
                        #endregion

                        #region 담당자확인 및 메모
                        if (string.IsNullOrWhiteSpace(dt.Rows[0]["ITCONFIRM"].ToString().Trim()) == false)
                        {
                            cboConfirm1.SelectedIndex = lstSabun.IndexOf(dt.Rows[0]["ITCONFIRM"].ToString().Trim()) + 1;
                        }

                        txtMemo1.Text = dt.Rows[0]["ITCONFIRM_MEMO"].ToString().Trim();

                        txtConfirm2.Text = dt.Rows[0]["RADIATIONCONFIRM"].ToString().Trim();
                        txtMemo2.Text = dt.Rows[0]["RADIATIONCONFIRM_MEMO"].ToString().Trim();

                        txtConfirm3.Text = dt.Rows[0]["CHARTCONFIRM"].ToString().Trim();
                        txtMemo3.Text = dt.Rows[0]["CHARTCONFIRM_MEMO"].ToString().Trim();

                        txtConfirm4.Text = dt.Rows[0]["ETCCONFIRM"].ToString().Trim();
                        txtMemo4.Text = dt.Rows[0]["ETCCONFIRM_MEMO"].ToString().Trim();

                        txtConfirm5.Text = dt.Rows[0]["HEADCONFIRM"].ToString().Trim();
                        #endregion

                    //}
                    #endregion
                }

                dt.Dispose();

                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 저장 함수
        /// </summary>
        private bool SaveData()
        {
            bool rtnVal = false;
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return rtnVal;
            }

            #region 바이트 및 필수입력사항 점검
            if (string.IsNullOrWhiteSpace(txtTel.Text.Trim()))
            {
                ComFunc.MsgBoxEx(this, "내선번호가 입력되지 않았습니다.");
                txtTel.Focus();
                return rtnVal;
            }

            #region 주민번호 점검
            if (string.IsNullOrWhiteSpace(txtPano1.Text.Trim()))
            {
                ComFunc.MsgBoxEx(this, "등록번호1이 입력되지 않았습니다.\r\n등록번호 입력후 엔터를 입력해주세요.");
                txtPano1.Focus();
                return rtnVal;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtJumin1_2.Tag.ToString().Trim()))
                {
                    ComFunc.MsgBoxEx(this, "등록번호1 조회후 엔터를 입력해주세요.");
                    txtPano1.Focus();
                    return rtnVal;
                }
            }

            if (string.IsNullOrWhiteSpace(txtPano2.Text.Trim()))
            {
                ComFunc.MsgBoxEx(this, "등록번호2이 입력되지 않았습니다.\r\n등록번호 입력후 엔터를 입력해주세요.");
                txtPano2.Focus();
                return rtnVal;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtJumin2_2.Tag.ToString().Trim()))
                {
                    ComFunc.MsgBoxEx(this, "등록번호2 조회후 엔터를 입력해주세요.");
                    txtPano2.Focus();
                    return rtnVal;
                }
            }

            if (string.IsNullOrWhiteSpace(txtPano3.Text.Trim()) == false && string.IsNullOrWhiteSpace(txtJumin3_2.Tag.ToString().Trim()))
            {
                ComFunc.MsgBoxEx(this, "등록번호3 조회후 엔터를 입력해주세요.");
                txtPano3.Focus();
                return rtnVal;
            }

            if (string.IsNullOrWhiteSpace(txtPano4.Text.Trim()) == false && string.IsNullOrWhiteSpace(txtJumin4_2.Tag.ToString().Trim()))
            {
                ComFunc.MsgBoxEx(this, "등록번호4 조회후 엔터를 입력해주세요.");
                txtPano4.Focus();
                return rtnVal;
            }

            if (string.IsNullOrWhiteSpace(txtPano5.Text.Trim()) == false && string.IsNullOrWhiteSpace(txtJumin5_2.Tag.ToString().Trim()))
            {
                ComFunc.MsgBoxEx(this, "등록번호5 조회후 엔터를 입력해주세요.");
                txtPano5.Focus();
                return rtnVal;
            }
            #endregion

            if (string.IsNullOrWhiteSpace(txtHappen.Text.Trim()))
            {
                ComFunc.MsgBoxEx(this, "발생내용은 필수입력 사항입니다.");
                txtHappen.Focus();
                return rtnVal;
            }


            List<Control> lstControl = tableLayoutPanel2.Controls.OfType<Control>().Where(c => c.Name.IndexOf("txtMemo") != -1 && !string.IsNullOrWhiteSpace(c.Text.Trim())).ToList();
            for (int i = 0; i < lstControl.Count; i++)
            {
                if (System.Text.Encoding.Default.GetByteCount(lstControl[i].Text.Trim()) > 100)
                {
                    ComFunc.MsgBoxEx(this, "메모는 100바이트 까지만 입력가능합니다.");
                    lstControl[i].Focus();
                    return rtnVal;
                }
            }

            if (System.Text.Encoding.Default.GetByteCount(txtHappen.Text.Trim()) > 1000)
            {
                ComFunc.MsgBoxEx(this, "발생내용은 1000바이트 까지만 입력가능합니다.\r\n더 입력을 원하실경우 해당 프로그램 담당자에게 문의바랍니다.");
                txtHappen.Focus();
                return rtnVal;
            }

            if (System.Text.Encoding.Default.GetByteCount(txtConduct.Text.Trim()) > 2000)
            {
                ComFunc.MsgBoxEx(this, "처리내용은 2000바이트 까지만 입력가능합니다.\r\n더 입력을 원하실경우 해당 프로그램 담당자에게 문의바랍니다.");
                txtConduct.Focus();
                return rtnVal;
            }

            if (clsType.User.BuseCode.Equals("044201") == false && ComFunc.MsgBoxQEx(this, "정말 저장하시겠습니까?\r\n저장후 의료정보팀으로\r\n반드시 연락바랍니다.") == DialogResult.No)
                return rtnVal;

            #region 등록번호 미입력시 삭제
            lstControl = tableLayoutPanel2.Controls.OfType<Control>().Where(c => c.Name.IndexOf("txtPano") != -1 && string.IsNullOrWhiteSpace(c.Text.Trim())).ToList();
            for (int i = 0; i < lstControl.Count; i++)
            {
                double ControlNo = double.Parse(lstControl[i].Name.Substring(lstControl[i].Name.Length - 1));

                TextBox txtName = tableLayoutPanel2.Controls.OfType<TextBox>().Where(c => c.Name.Equals("txtName" + ControlNo)).FirstOrDefault();
                TextBox txtJ1 = tableLayoutPanel2.Controls.OfType<TextBox>().Where(c => c.Name.Equals("txtJumin" + ControlNo)).FirstOrDefault();
                TextBox txtJ2 = tableLayoutPanel2.Controls.OfType<TextBox>().Where(c => c.Name.Equals("txtJumin" + ControlNo + "_2")).FirstOrDefault();

                if (txtName != null)
                {
                    txtName.Clear();
                    txtJ1.Clear();
                    txtJ2.Clear();
                }
            }
            #endregion

            #endregion

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                if (string.IsNullOrWhiteSpace(strSeqNo))
                {
                    lstControl = tableLayoutPanel2.Controls.OfType<Control>().Where(c => c.Name.IndexOf("txtPano") != -1 && !string.IsNullOrWhiteSpace(c.Text.Trim())).ToList();
                    for (int i = 0; i < lstControl.Count; i++)
                    {
                        if (SavePano(lstControl[i].Text.Trim()))
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "등록번호: " + lstControl[i].Text.Trim() + "는 이미 저장 되어있습니다.");
                            lstControl[i].Focus();
                            return rtnVal;
                        }
                    }

                    SQL = "INSERT INTO KOSMOS_PMPA.MID_DBLCHART_APPLICATION ";
                    SQL += ComNum.VBLF + "(";
                    SQL += ComNum.VBLF + "SEQNO,";
                    SQL += ComNum.VBLF + "REQDATE, REQBUSE, REQSABUN, TEL, ";
                    SQL += ComNum.VBLF + "PANO1,PANO2,PANO3,PANO4,PANO5,";
                    SQL += ComNum.VBLF + "SNAME1, SNAME2, SNAME3, SNAME4, SNAME5,";
                    SQL += ComNum.VBLF + "JUMIN1, JUMIN1_2, JUMIN2, JUMIN2_2, JUMIN3, JUMIN3_2, JUMIN4, JUMIN4_2, JUMIN5, JUMIN5_2,";
                    SQL += ComNum.VBLF + "ETC1, ETC2, ETC3, ETC4, ETC5,";
                    SQL += ComNum.VBLF + "HAPPEN";

                    #region 의료정보팀에서 저장시
                    if (clsType.User.BuseCode.Equals("044201"))
                    {
                        SQL += ComNum.VBLF + ", USE1, USE2, USE3, USE4, USE5";
                        SQL += ComNum.VBLF + ", CONDUCT";
                        SQL += ComNum.VBLF + ", SUBMITDATE, RECEPTIONIST, COMPLETEDATE";
                        SQL += ComNum.VBLF + ", DOUBLEGBN";
                        SQL += ComNum.VBLF + ", ITCONFIRM, ITCONFIRM_MEMO";
                        SQL += ComNum.VBLF + ", RADIATIONCONFIRM, RADIATIONCONFIRM_MEMO";
                        SQL += ComNum.VBLF + ", CHARTCONFIRM, CHARTCONFIRM_MEMO";
                        SQL += ComNum.VBLF + ", ETCCONFIRM, ETCCONFIRM_MEMO";
                        SQL += ComNum.VBLF + ", HEADCONFIRM";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + ", DOUBLEGBN";
                    }
                    #endregion

                    SQL += ComNum.VBLF + ")";
                    SQL += ComNum.VBLF + "VALUES";
                    SQL += ComNum.VBLF + "(";

                    #region 시퀀스
                    SQL += ComNum.VBLF + "KOSMOS_PMPA.SEQ_DBLAPPNO.NEXTVAL, ";
                    #endregion

                    #region 신청라인
                    SQL += ComNum.VBLF + "TO_DATE('" + dtpReqDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD'), '" + txtReqBuse.Tag.ToString() + "', '" + txtReqName.Tag.ToString() + "', ";
                    SQL += ComNum.VBLF + "'" + txtTel.Text.Trim() + "', ";
                    #endregion

                    #region 차트번호
                    SQL += ComNum.VBLF + "'" + txtPano1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "'" + txtPano2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "'" + txtPano3.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "'" + txtPano4.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "'" + txtPano5.Text.Trim() + "', ";
                    #endregion

                    #region 성명
                    SQL += ComNum.VBLF + "'" + txtName1.Text.Trim().Replace("'", "`") + "', ";
                    SQL += ComNum.VBLF + "'" + txtName2.Text.Trim().Replace("'", "`") + "', ";
                    SQL += ComNum.VBLF + "'" + txtName3.Text.Trim().Replace("'", "`") + "', ";
                    SQL += ComNum.VBLF + "'" + txtName4.Text.Trim().Replace("'", "`") + "', ";
                    SQL += ComNum.VBLF + "'" + txtName5.Text.Trim().Replace("'", "`") + "', ";
                    #endregion

                    #region 주민
                    SQL += ComNum.VBLF + "'" + txtJumin1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "'" + (string.IsNullOrWhiteSpace(txtJumin1_2.Tag.ToString()) == false ? clsAES.AES(txtJumin1_2.Tag.ToString().Trim()) : "") + "', ";
                    SQL += ComNum.VBLF + "'" + txtJumin2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "'" + (string.IsNullOrWhiteSpace(txtJumin2_2.Tag.ToString()) == false ? clsAES.AES(txtJumin2_2.Tag.ToString().Trim()) : "") + "', ";
                    SQL += ComNum.VBLF + "'" + txtJumin3.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "'" + (string.IsNullOrWhiteSpace(txtJumin3_2.Tag.ToString()) == false ? clsAES.AES(txtJumin3_2.Tag.ToString().Trim()) : "") + "', ";
                    SQL += ComNum.VBLF + "'" + txtJumin4.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "'" + (string.IsNullOrWhiteSpace(txtJumin4_2.Tag.ToString()) == false ? clsAES.AES(txtJumin4_2.Tag.ToString().Trim()) : "") + "', ";
                    SQL += ComNum.VBLF + "'" + txtJumin5.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "'" + (string.IsNullOrWhiteSpace(txtJumin5_2.Tag.ToString()) == false ? clsAES.AES(txtJumin5_2.Tag.ToString().Trim()) : "") + "', ";
                    #endregion

                    #region 기타사항
                    SQL += ComNum.VBLF + "'" + txtEtc1.Text.Trim().Replace("'", "`") + "', ";
                    SQL += ComNum.VBLF + "'" + txtEtc2.Text.Trim().Replace("'", "`") + "', ";
                    SQL += ComNum.VBLF + "'" + txtEtc3.Text.Trim().Replace("'", "`") + "', ";
                    SQL += ComNum.VBLF + "'" + txtEtc4.Text.Trim().Replace("'", "`") + "', ";
                    SQL += ComNum.VBLF + "'" + txtEtc5.Text.Trim().Replace("'", "`") + "', ";
                    #endregion

                    #region 발생내용
                    SQL += ComNum.VBLF + "'" + txtHappen.Text.Trim().Replace("'", "`") + "'";
                    #endregion

                    #region 의료정보팀에서 저장시
                    if (clsType.User.BuseCode.Equals("044201"))
                    {
                        #region 사용/삭제
                        SQL += ComNum.VBLF + ",'" + (cboUse1.SelectedIndex != 0 ? cboUse1.Text.Trim().Replace("'", "`") : "") + "'";
                        SQL += ComNum.VBLF + ",'" + (cboUse2.SelectedIndex != 0 ? cboUse2.Text.Trim().Replace("'", "`") : "") + "'";
                        SQL += ComNum.VBLF + ",'" + (cboUse3.SelectedIndex != 0 ? cboUse3.Text.Trim().Replace("'", "`") : "") + "'";
                        SQL += ComNum.VBLF + ",'" + (cboUse4.SelectedIndex != 0 ? cboUse4.Text.Trim().Replace("'", "`") : "") + "'";
                        SQL += ComNum.VBLF + ",'" + (cboUse5.SelectedIndex != 0 ? cboUse5.Text.Trim().Replace("'", "`") : "") + "'";
                        #endregion

                        #region 처리내용
                        SQL += ComNum.VBLF + ",'" + txtConduct.Text.Trim().Replace("'", "`") + "'";
                        #endregion

                        #region 접수라인
                        SQL += ComNum.VBLF + ", TO_DATE('" + (dtpSubmit.Checked ? dtpSubmit.Value.ToString("yyyy-MM-dd") : "") + "', 'YYYY-MM-DD')";
                        SQL += ComNum.VBLF + ", '" + (cboReceptionist.SelectedIndex != 0 ? lstSabun[cboReceptionist.SelectedIndex - 1] : "") + "'";
                        SQL += ComNum.VBLF + ", TO_DATE('" + (dtpComplete.Checked ? dtpComplete.Value.ToString("yyyy-MM-dd") : "") + "', 'YYYY-MM-DD')";
                        #endregion

                        #region 이중구분
                        string Duplication = string.Empty;
                        if (rdoDuplication0.Checked)
                        {
                            Duplication = "0";
                        }
                        else if (rdoDuplication1.Checked)
                        {
                            Duplication = "1";
                        }
                        else if (rdoDuplication2.Checked)
                        {
                            Duplication = "2";
                        }
                        else if (rdoDuplication3.Checked)
                        {
                            Duplication = "3";
                        }
                        else if (rdoDuplication4.Checked)
                        {
                            Duplication = "4";
                        }

                        SQL += ComNum.VBLF + ", '" + Duplication + "'";
                        #endregion

                        #region 담당자 확인/메모
                        SQL += ComNum.VBLF + ", '" + (cboConfirm1.SelectedIndex != 0 ? lstSabun[cboConfirm1.SelectedIndex - 1] : "") + "'";
                        SQL += ComNum.VBLF + ", '" + txtMemo1.Text.Trim().Replace("'", "`") + "'";

                        SQL += ComNum.VBLF + ", '" + txtConfirm2.Text.Trim().Replace("'", "`") + "'";
                        SQL += ComNum.VBLF + ", '" + txtMemo2.Text.Trim().Replace("'", "`") + "'";

                        SQL += ComNum.VBLF + ", '" + txtConfirm3.Text.Trim().Replace("'", "`") + "'";
                        SQL += ComNum.VBLF + ", '" + txtMemo3.Text.Trim().Replace("'", "`") + "'";

                        SQL += ComNum.VBLF + ", '" + txtConfirm4.Text.Trim().Replace("'", "`") + "'";
                        SQL += ComNum.VBLF + ", '" + txtMemo4.Text.Trim().Replace("'", "`") + "'";

                        SQL += ComNum.VBLF + ", '" + txtConfirm5.Text.Trim().Replace("'", "`") + "'";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + ", '4'";
                    }
                    #endregion
                    SQL += ComNum.VBLF + ")";
                    #endregion
                }
                else
                {
                    lstControl = tableLayoutPanel2.Controls.OfType<Control>().Where(c => c.Name.IndexOf("txtPano") != -1 && !string.IsNullOrWhiteSpace(c.Text.Trim())).ToList();
                    for (int i = 0; i < lstControl.Count; i++)
                    {
                        if (SavePano(lstControl[i].Text.Trim(), strSeqNo))
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "등록번호: " + lstControl[i].Text.Trim() + "는 이미 저장 되어있습니다.");
                            lstControl[i].Focus();
                            return rtnVal;
                        }
                    }

                    #region 수정 
                    SQL = "UPDATE KOSMOS_PMPA.MID_DBLCHART_APPLICATION SET ";

                    #region 신청라인
                    SQL += ComNum.VBLF + "REQDATE = TO_DATE('" + dtpReqDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                    SQL += ComNum.VBLF + ", REQBUSE  = '" + txtReqBuse.Tag.ToString() + "'";
                    SQL += ComNum.VBLF + ", REQSABUN = '" + txtReqName.Tag.ToString() + "'";
                    SQL += ComNum.VBLF + ", TEL = '" + txtTel.Text.Trim() + "'";
                    #endregion

                    #region 차트번호
                    SQL += ComNum.VBLF + ", PANO1 = '" + txtPano1.Text.Trim() + "'";
                    SQL += ComNum.VBLF + ", PANO2 = '" + txtPano2.Text.Trim() + "'";
                    SQL += ComNum.VBLF + ", PANO3 = '" + txtPano3.Text.Trim() + "'";
                    SQL += ComNum.VBLF + ", PANO4 = '" + txtPano4.Text.Trim() + "'";
                    SQL += ComNum.VBLF + ", PANO5 = '" + txtPano5.Text.Trim() + "'";
                    #endregion

                    #region 성명
                    SQL += ComNum.VBLF + ", SNAME1 = '" + txtName1.Text.Trim().Replace("'", "`") + "'";
                    SQL += ComNum.VBLF + ", SNAME2 = '" + txtName2.Text.Trim().Replace("'", "`") + "'";
                    SQL += ComNum.VBLF + ", SNAME3 = '" + txtName3.Text.Trim().Replace("'", "`") + "'";
                    SQL += ComNum.VBLF + ", SNAME4 = '" + txtName4.Text.Trim().Replace("'", "`") + "'";
                    SQL += ComNum.VBLF + ", SNAME5 = '" + txtName5.Text.Trim().Replace("'", "`") + "'";
                    #endregion

                    #region 주민
                    SQL += ComNum.VBLF + ", JUMIN1 = '" + txtJumin1.Text.Trim() + "'";
                    SQL += ComNum.VBLF + ", JUMIN1_2 = '" + (string.IsNullOrWhiteSpace(txtJumin1_2.Tag.ToString().Trim()) == false ? clsAES.AES(txtJumin1_2.Tag.ToString().Trim()) : "") + "'";
                    SQL += ComNum.VBLF + ", JUMIN2 = '" + txtJumin2.Text.Trim() + "'";
                    SQL += ComNum.VBLF + ", JUMIN2_2 = '" + (string.IsNullOrWhiteSpace(txtJumin2_2.Tag.ToString().Trim()) == false ? clsAES.AES(txtJumin2_2.Tag.ToString().Trim()) : "") + "'";
                    SQL += ComNum.VBLF + ", JUMIN3 = '" + txtJumin3.Text.Trim() + "'";
                    SQL += ComNum.VBLF + ", JUMIN3_2 = '" + (string.IsNullOrWhiteSpace(txtJumin3_2.Tag.ToString().Trim()) == false ? clsAES.AES(txtJumin3_2.Tag.ToString().Trim()) : "") + "'";
                    SQL += ComNum.VBLF + ", JUMIN4 = '" + txtJumin4.Text.Trim() + "'";
                    SQL += ComNum.VBLF + ", JUMIN4_2 = '" + (string.IsNullOrWhiteSpace(txtJumin4_2.Tag.ToString().Trim()) == false ? clsAES.AES(txtJumin4_2.Tag.ToString().Trim()) : "") + "'";
                    SQL += ComNum.VBLF + ", JUMIN5 = '" + txtJumin5.Text.Trim() + "'";
                    SQL += ComNum.VBLF + ", JUMIN5_2 = '" + (string.IsNullOrWhiteSpace(txtJumin5_2.Tag.ToString().Trim()) == false ? clsAES.AES(txtJumin5_2.Tag.ToString().Trim()) : "") + "'";
                    #endregion

                    #region 기타사항
                    SQL += ComNum.VBLF + ", ETC1 = '" + txtEtc1.Text.Trim().Replace("'", "`") + "'";
                    SQL += ComNum.VBLF + ", ETC2 = '" + txtEtc2.Text.Trim().Replace("'", "`") + "'";
                    SQL += ComNum.VBLF + ", ETC3 = '" + txtEtc3.Text.Trim().Replace("'", "`") + "'";
                    SQL += ComNum.VBLF + ", ETC4 = '" + txtEtc4.Text.Trim().Replace("'", "`") + "'";
                    SQL += ComNum.VBLF + ", ETC5 = '" + txtEtc5.Text.Trim().Replace("'", "`") + "'";
                    #endregion

                    #region 발생내용
                    SQL += ComNum.VBLF + ", HAPPEN = '" + txtHappen.Text.Trim().Replace("'", "`") + "'";
                    #endregion

                    #region 의료정보팀에서 저장시
                    if (clsType.User.BuseCode.Equals("044201"))
                    {
                        #region 사용/삭제
                        SQL += ComNum.VBLF + ", USE1 = '" + (cboUse1.SelectedIndex != 0 ? cboUse1.Text.Trim().Replace("'", "`") : "") + "'";
                        SQL += ComNum.VBLF + ", USE2 = '" + (cboUse2.SelectedIndex != 0 ? cboUse2.Text.Trim().Replace("'", "`") : "") + "'";
                        SQL += ComNum.VBLF + ", USE3 = '" + (cboUse3.SelectedIndex != 0 ? cboUse3.Text.Trim().Replace("'", "`") : "") + "'";
                        SQL += ComNum.VBLF + ", USE4 = '" + (cboUse4.SelectedIndex != 0 ? cboUse4.Text.Trim().Replace("'", "`") : "") + "'";
                        SQL += ComNum.VBLF + ", USE5 = '" + (cboUse5.SelectedIndex != 0 ? cboUse5.Text.Trim().Replace("'", "`") : "") + "'";
                        #endregion

                        #region 처리내용
                        SQL += ComNum.VBLF + ", CONDUCT = '" + txtConduct.Text.Trim().Replace("'", "`") + "'";
                        #endregion

                        #region 접수라인
                        SQL += ComNum.VBLF + ", SUBMITDATE   = TO_DATE('" + (dtpSubmit.Checked ? dtpSubmit.Value.ToString("yyyy-MM-dd") : "") + "', 'YYYY-MM-DD')";
                        SQL += ComNum.VBLF + ", RECEPTIONIST = '" + (cboReceptionist.SelectedIndex != 0 ? lstSabun[cboReceptionist.SelectedIndex - 1] : "") + "'";
                        SQL += ComNum.VBLF + ", COMPLETEDATE = TO_DATE('" + (dtpComplete.Checked ? dtpComplete.Value.ToString("yyyy-MM-dd") : "") + "', 'YYYY-MM-DD')";
                        #endregion

                        #region 이중구분
                        string Duplication = string.Empty;
                        if (rdoDuplication0.Checked)
                        {
                            Duplication = "0";
                        }
                        else if (rdoDuplication1.Checked)
                        {
                            Duplication = "1";
                        }
                        else if (rdoDuplication2.Checked)
                        {
                            Duplication = "2";
                        }
                        else if (rdoDuplication3.Checked)
                        {
                            Duplication = "3";
                        }
                        else if (rdoDuplication4.Checked)
                        {
                            Duplication = "4";
                        }

                        SQL += ComNum.VBLF + ", DOUBLEGBN = '" + Duplication + "'";
                        #endregion

                        #region 담당자 확인/메모
                        SQL += ComNum.VBLF + ", ITCONFIRM = '" + (cboConfirm1.SelectedIndex != 0 ? lstSabun[cboConfirm1.SelectedIndex - 1] : "") + "'";
                        SQL += ComNum.VBLF + ", ITCONFIRM_MEMO = '" + txtMemo1.Text.Trim().Replace("'", "`") + "'";

                        SQL += ComNum.VBLF + ", RADIATIONCONFIRM = '" + txtConfirm2.Text.Trim().Replace("'", "`") + "'";
                        SQL += ComNum.VBLF + ", RADIATIONCONFIRM_MEMO = '" + txtMemo2.Text.Trim().Replace("'", "`") + "'";

                        SQL += ComNum.VBLF + ", CHARTCONFIRM = '" + txtConfirm3.Text.Trim().Replace("'", "`") + "'";
                        SQL += ComNum.VBLF + ", CHARTCONFIRM_MEMO = '" + txtMemo3.Text.Trim().Replace("'", "`") + "'";

                        SQL += ComNum.VBLF + ", ETCCONFIRM = '" + txtConfirm4.Text.Trim().Replace("'", "`") + "'";
                        SQL += ComNum.VBLF + ", ETCCONFIRM_MEMO = '" + txtMemo4.Text.Trim().Replace("'", "`") + "'";

                        SQL += ComNum.VBLF + ", HEADCONFIRM = '" + txtConfirm5.Text.Trim().Replace("'", "`") + "'";
                        #endregion
                    }
                    #endregion

                    SQL += ComNum.VBLF + "WHERE SEQNO = " + strSeqNo;
                    #endregion
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message + "\r\n이중차트신청서 저장중 오류발생", "", clsDB.DbCon);
            }

            return rtnVal;
        }

        /// <summary>
        /// 이미 저장한 등록번호인지 확인
        /// </summary>
        /// <param name="strPano"></param>
        /// <returns></returns>
        private bool SavePano(string strPano, string strSeqNo = "")
        {
            bool rtnVal = false;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return rtnVal;
            }

            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            #endregion

            try
            {
                SQL = "  SELECT 1 AS CNT";
                SQL += ComNum.VBLF + "    FROM DUAL";
                SQL += ComNum.VBLF + "   WHERE EXISTS";
                SQL += ComNum.VBLF + "       (";
                SQL += ComNum.VBLF + "         SELECT 1";
                SQL += ComNum.VBLF + "           FROM " + ComNum.DB_PMPA + "MID_DBLCHART_APPLICATION";
                SQL += ComNum.VBLF + "           WHERE ";
                SQL += ComNum.VBLF + "           (";
                SQL += ComNum.VBLF + "             (PANO1 = '" + strPano + "') OR";
                SQL += ComNum.VBLF + "             (PANO2 = '" + strPano + "') OR";
                SQL += ComNum.VBLF + "             (PANO3 = '" + strPano + "') OR";
                SQL += ComNum.VBLF + "             (PANO4 = '" + strPano + "') OR";
                SQL += ComNum.VBLF + "             (PANO5 = '" + strPano + "')";
                SQL += ComNum.VBLF + "           )";
                SQL += ComNum.VBLF + "             AND DELDATE IS NULL";

                if (string.IsNullOrWhiteSpace(strSeqNo) == false)
                {
                    SQL += ComNum.VBLF + "             AND SEQNO <> " + strSeqNo;
                }
                SQL += ComNum.VBLF + "       )";
                
  

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();
            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }

            return rtnVal;
        }

        private bool PrintData()
        {
            bool rtnVal = false;
            
            if (string.IsNullOrWhiteSpace(strSeqNo))
                return rtnVal;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "UPDATE KOSMOS_PMPA.MID_DBLCHART_APPLICATION SET ";
                SQL += ComNum.VBLF + "PRTYN = 'Y'";
                SQL += ComNum.VBLF + "WHERE SEQNO = " + strSeqNo;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message + "\r\n이중차트신청서 MID_DBLCHART_APPLICATION , PRTYN 출력처리 오류발생", "", clsDB.DbCon);
            }

            return rtnVal;
        }

        /// <summary>
        /// 삭제 함수
        /// </summary>
        private bool DeleteData()
        {
            bool rtnVal = false;
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return rtnVal;
            }

            if (string.IsNullOrWhiteSpace(strSeqNo))
                return rtnVal;

            if (ComFunc.MsgBoxQEx(this, "정말 해당 신청서를 " + btnDelete.Text.Trim() + " 하시겠습니까?") == DialogResult.No)
                return rtnVal;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "UPDATE KOSMOS_PMPA.MID_DBLCHART_APPLICATION SET ";
                if (btnDelete.Text.Equals("삭제"))
                {
                    SQL += ComNum.VBLF + "DELDATE = SYSDATE";
                    SQL += ComNum.VBLF + ", DELSABUN = '" + clsType.User.IdNumber + "'";
                }
                else
                {
                    SQL += ComNum.VBLF + "DELDATE = NULL";
                    SQL += ComNum.VBLF + ", DELSABUN = NULL";
                }
                
                SQL += ComNum.VBLF + "WHERE SEQNO = "+ strSeqNo;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message + "\r\n이중차트신청서 삭제중 오류발생", "", clsDB.DbCon);
            }

            return rtnVal;
        }

        /// <summary>
        /// 출력 함수
        /// </summary>
        private void Set_Print()
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return;

            #region 스프레드 내용 반영
            Size txtSize;

            #region 신청
            ssPrint_Sheet1.Cells[2, 2].Text = dtpReqDate.Value.ToString("yyyy-MM-dd");
            ssPrint_Sheet1.Cells[2, 8].Text = txtReqBuse.Text.Trim();
            ssPrint_Sheet1.Cells[2, 13].Text = txtReqName.Text.Trim();
            ssPrint_Sheet1.Cells[2, 19].Text = txtTel.Text.Trim();
            #endregion

            #region 사용삭제
            ssPrint_Sheet1.Cells[3, 2].Text  = cboUse1.Text.Trim();
            ssPrint_Sheet1.Cells[3, 2].BackColor = cboUse1.SelectedIndex == 2 || cboUse1.SelectedIndex == 3 ? Color.FromArgb(255, 225, 225) : Color.White;
            ssPrint_Sheet1.Cells[3, 6].Text  = cboUse2.Text.Trim();
            ssPrint_Sheet1.Cells[3, 6].BackColor = cboUse2.SelectedIndex == 2 || cboUse2.SelectedIndex == 3 ? Color.FromArgb(255, 225, 225) : Color.White;
            ssPrint_Sheet1.Cells[3, 10].Text = cboUse3.Text.Trim();
            ssPrint_Sheet1.Cells[3, 10].BackColor = cboUse3.SelectedIndex == 2 || cboUse3.SelectedIndex == 3 ? Color.FromArgb(255, 225, 225) : Color.White;
            ssPrint_Sheet1.Cells[3, 14].Text = cboUse4.Text.Trim();
            ssPrint_Sheet1.Cells[3, 14].BackColor = cboUse4.SelectedIndex == 2 || cboUse4.SelectedIndex == 3 ? Color.FromArgb(255, 225, 225) : Color.White;
            ssPrint_Sheet1.Cells[3, 18].Text = cboUse5.Text.Trim();
            ssPrint_Sheet1.Cells[3, 18].BackColor = cboUse5.SelectedIndex == 2 || cboUse5.SelectedIndex == 3 ? Color.FromArgb(255, 225, 225) : Color.White;
            #endregion

            #region 차트번호
            ssPrint_Sheet1.Cells[4, 2].Text  = txtPano1.Text.Trim();
            ssPrint_Sheet1.Cells[4, 6].Text  = txtPano2.Text.Trim();
            ssPrint_Sheet1.Cells[4, 10].Text = txtPano3.Text.Trim();
            ssPrint_Sheet1.Cells[4, 14].Text = txtPano4.Text.Trim();
            ssPrint_Sheet1.Cells[4, 18].Text = txtPano5.Text.Trim();
            #endregion

            #region 성명
            ssPrint_Sheet1.Cells[5, 2].Text  = txtName1.Text.Trim();
            ssPrint_Sheet1.Cells[5, 6].Text  = txtName2.Text.Trim();
            ssPrint_Sheet1.Cells[5, 10].Text = txtName3.Text.Trim();
            ssPrint_Sheet1.Cells[5, 14].Text = txtName4.Text.Trim();
            ssPrint_Sheet1.Cells[5, 18].Text = txtName5.Text.Trim();
            #endregion

            #region 주민번호
            if (!string.IsNullOrWhiteSpace(txtJumin1.Text.Trim()) && !string.IsNullOrWhiteSpace(txtJumin1_2.Tag.ToString().Trim()))
            {
                ssPrint_Sheet1.Cells[6, 2].Text = txtJumin1.Text.Trim() + "-" + txtJumin1_2.Tag.ToString().Trim();
            }
            if (!string.IsNullOrWhiteSpace(txtJumin2.Text.Trim()) && !string.IsNullOrWhiteSpace(txtJumin2_2.Tag.ToString().Trim()))
            {
                ssPrint_Sheet1.Cells[6, 6].Text = txtJumin2.Text.Trim() + "-" + txtJumin2_2.Tag.ToString().Trim();
            }
            if (!string.IsNullOrWhiteSpace(txtJumin3.Text.Trim()) && !string.IsNullOrWhiteSpace(txtJumin3_2.Tag.ToString().Trim()))
            {
                ssPrint_Sheet1.Cells[6, 10].Text = txtJumin3.Text.Trim() + "-" + txtJumin3_2.Tag.ToString().Trim();
            }
            if (!string.IsNullOrWhiteSpace(txtJumin4.Text.Trim()) && !string.IsNullOrWhiteSpace(txtJumin4_2.Tag.ToString().Trim()))
            {
                ssPrint_Sheet1.Cells[6, 14].Text = txtJumin4.Text.Trim() + "-" + txtJumin4_2.Tag.ToString().Trim();
            }
            if (!string.IsNullOrWhiteSpace(txtJumin5.Text.Trim()) && !string.IsNullOrWhiteSpace(txtJumin5_2.Tag.ToString().Trim()))
            {
                ssPrint_Sheet1.Cells[6, 18].Text = txtJumin5.Text.Trim() + "-" + txtJumin5_2.Tag.ToString().Trim();
            }
            #endregion

            #region 기타사항
            List<int> lstHeight = new List<int>();
            ssPrint_Sheet1.Cells[7, 2].Text  = txtEtc1.Text.Trim();
            txtSize = TextRenderer.MeasureText(txtEtc1.Text.Trim(), txtEtc1.Font, new Size(120, 100), TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
            if (txtSize.Height >= 100)
            {
                lstHeight.Add(txtSize.Height);
            }
            ssPrint_Sheet1.Cells[7, 6].Text  = txtEtc2.Text.Trim();
            txtSize = TextRenderer.MeasureText(txtEtc2.Text.Trim(), txtEtc2.Font, new Size(120, 100), TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
            if (txtSize.Height >= 100)
            {
                lstHeight.Add(txtSize.Height);
            }
            ssPrint_Sheet1.Cells[7, 10].Text = txtEtc3.Text.Trim();
            txtSize = TextRenderer.MeasureText(txtEtc3.Text.Trim(), txtEtc3.Font, new Size(120, 100), TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
            if (txtSize.Height >= 100)
            {
                lstHeight.Add(txtSize.Height);
            }
            ssPrint_Sheet1.Cells[7, 14].Text = txtEtc4.Text.Trim();
            txtSize = TextRenderer.MeasureText(txtEtc4.Text.Trim(), txtEtc4.Font, new Size(120, 100), TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
            if (txtSize.Height >= 100)
            {
                lstHeight.Add(txtSize.Height);
            }
            ssPrint_Sheet1.Cells[7, 18].Text = txtEtc5.Text.Trim();
            txtSize = TextRenderer.MeasureText(txtEtc5.Text.Trim(), txtEtc5.Font, new Size(120, 100), TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
            if (txtSize.Height >= 100)
            {
                lstHeight.Add(txtSize.Height);
            }

            if (lstHeight.Count > 0)
            {
                lstHeight.Reverse();
                ssPrint_Sheet1.Rows[7].Height = lstHeight[0] / 2 + 10;
                ssPrint_Sheet1.Rows[8].Height = lstHeight[0] / 2 + 10;
            }
            else
            {
                ssPrint_Sheet1.Rows[7].Height = 50;
            }
            #endregion

            #region 발생내용
            ssPrint_Sheet1.Cells[9, 2].Text = txtHappen.Text.Trim();
            txtSize = TextRenderer.MeasureText(txtHappen.Text.Trim(), txtHappen.Font, new Size(600, 150), TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
            if (txtSize.Height >= 150)
            {
                ssPrint_Sheet1.Rows[9].Height = txtSize.Height + 10;
            }
            else
            {
                ssPrint_Sheet1.Rows[9].Height = 150;
            }
            #endregion

            #region 처리내용
            ssPrint_Sheet1.Cells[10, 2].Text = txtConduct.Text.Trim();
            txtSize = TextRenderer.MeasureText(txtConduct.Text.Trim(), txtConduct.Font, new Size(600, 300), TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
            if (txtSize.Height >= 300)
            {
                ssPrint_Sheet1.Rows[10].Height = txtSize.Height + 10;
            }
            else
            {
                ssPrint_Sheet1.Rows[10].Height = 260;
            }
            #endregion

            #region 접수
            ssPrint_Sheet1.Cells[11, 2].Text = dtpSubmit.Checked ? dtpSubmit.Value.ToString("yyyy-MM-dd") : "";
            ssPrint_Sheet1.Cells[11, 10].Text = cboReceptionist.SelectedIndex != 0 ?  cboReceptionist.Text.Substring(0, cboReceptionist.Text.IndexOf(" " )) : "";
            ssPrint_Sheet1.Cells[11, 18].Text = dtpComplete.Checked ? dtpComplete.Value.ToString("yyyy-MM-dd") : "";
            #endregion

            #region 이중구분
            int Duplication = 0;
            if (rdoDuplication0.Checked)
            {
                Duplication = 0;
            }
            else if (rdoDuplication1.Checked)
            {
                Duplication = 1;
            }
            else if (rdoDuplication2.Checked)
            {
                Duplication = 2;
            }
            else if (rdoDuplication3.Checked)
            {
                Duplication = 3;
            }
            else if (rdoDuplication4.Checked)
            {
                Duplication = 4;
            }

            switch (Duplication)
            {
                case 0:
                    ssPrint_Sheet1.Cells[12, 2].Text = "● 이중 ○ 폐기 ○ 부분이중 ○ 취소 ○ 처리중";
                    break;
                case 1:
                    ssPrint_Sheet1.Cells[12, 2].Text = "○ 이중 ● 폐기 ○ 부분이중 ○ 취소 ○ 처리중";
                    break;
                case 2:
                    ssPrint_Sheet1.Cells[12, 2].Text = "○ 이중 ○ 폐기 ● 부분이중 ○ 취소 ○ 처리중";
                    break;
                case 3:
                    ssPrint_Sheet1.Cells[12, 2].Text = "○ 이중 ○ 폐기 ○ 부분이중 ● 취소 ○ 처리중";
                    break;
                case 4:
                    ssPrint_Sheet1.Cells[12, 2].Text = "○ 이중 ○ 폐기 ○ 부분이중 ○ 취소 ● 처리중";
                    break;
            }
            #endregion

            #region 담당자 확인/메모
            ssPrint_Sheet1.Cells[14, 2].Text = cboConfirm1.SelectedIndex != 0 ? cboConfirm1.Text.Substring(0, cboConfirm1.Text.IndexOf(".") - 1).Trim() : "";
            ssPrint_Sheet1.Cells[15, 2].Text = txtMemo1.Text.Trim();

            ssPrint_Sheet1.Cells[14, 6].Text = txtConfirm2.Text.Trim();
            ssPrint_Sheet1.Cells[15, 6].Text = txtMemo2.Text.Trim();

            ssPrint_Sheet1.Cells[14, 10].Text = txtConfirm3.Text.Trim();
            ssPrint_Sheet1.Cells[15, 10].Text = txtMemo3.Text.Trim();

            ssPrint_Sheet1.Cells[14, 14].Text = txtConfirm4.Text.Trim();
            ssPrint_Sheet1.Cells[15, 14].Text = txtMemo4.Text.Trim();
            #endregion

            //부서장
            ssPrint_Sheet1.Cells[14, 18].Text = txtConfirm5.Text.Trim();
            #endregion

            ssPrint_Sheet1.PrintInfo.Margin.Top = 40;
            ssPrint_Sheet1.PrintInfo.Centering = Centering.Horizontal;
            ssPrint_Sheet1.PrintInfo.ShowBorder = false;
            ssPrint_Sheet1.PrintInfo.ShowGrid = false;

            ssPrint.PrintSheet(0);
        }

        #endregion

        #region 버튼 이벤트

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewWrite();
        }

        /// <summary>
        /// 신규작성
        /// </summary>
        private void NewWrite()
        {
            FormClear();
            txtReqBuse.Tag = clsType.User.BuseCode;
            txtReqBuse.Text = clsVbfunc.GetBASBuSe(clsDB.DbCon, clsType.User.BuseCode);

            txtReqName.Tag = clsType.User.IdNumber;
            txtReqName.Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);

            dtpReqDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

            #region 의료정보팀
            if (clsType.User.BuseCode.Equals("044201"))
            {

                foreach (Control control in ComFunc.GetAllControls(panApplication))
                {
                    if (control is Panel || control is TableLayoutPanel)
                    {
                        continue;
                    }

                    if (control is TextBox)
                    {
                        if (control.BackColor == Color.White || control.BackColor == Color.FromArgb(255, 225, 225) || control.BackColor == Color.FromArgb(255, 255, 192))
                        {
                            (control as TextBox).ReadOnly = !clsType.User.BuseCode.Equals("044201");
                        }
                    }

                    control.Enabled = clsType.User.BuseCode.Equals("044201");
                }

                dtpSubmit.Enabled = clsType.User.BuseCode.Equals("044201");
                dtpComplete.Enabled = dtpSubmit.Enabled;
            }
            #endregion

            txtTel.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData())
            {
                FormClear();
                ComFunc.MsgBoxEx(this, "저장되었습니다.");
                GetSearchData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (DeleteData())
            {
                FormClear();
                ComFunc.MsgBoxEx(this, btnDelete.Text.Equals("삭제") ?  "삭제되었습니다." : "복구되었습니다.");
                GetSearchData();
            }
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            Set_Print();
            PrintData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region 스프레드 이벤트

        private void ssList_CellClick(object sender, CellClickEventArgs e)
        {
            FpSpread spd = (sender as FpSpread);
            if (spd.ActiveSheet.RowCount == 0)
                return;

            if (e.ColumnHeader)
            {
                clsSpread.gSpdSortRow(spd, e.Column);
            }
        }

        private void ssList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            strSeqNo = string.Empty;

            FpSpread spd = (sender as FpSpread);
            if (spd.ActiveSheet.RowCount == 0)
                return;

            Spread_Clear();
            FormClear();

            if (clsType.User.BuseCode.Equals("044201"))
            {
                if (e.Column >= 3 && e.Column <= 5)
                {
                    if (sender.Equals(ssListComplete) && e.Column != 5 || sender.Equals(ssListComplete) == false)
                    {
                        ViewEmr(this, spd.ActiveSheet.Cells[e.Row, e.Column].Text.Trim());
                    }
                }
            }
            

            if (sender.Equals(ssDel))
            {
                btnDelete.Text = "복구";
            }
            else
            {
                btnDelete.Text = "삭제";
            }

            strSeqNo = spd.ActiveSheet.Cells[e.Row, 11].Text.Trim();
            if (SetAppData() == false)
            {
                ComFunc.MsgBoxEx(this, "같은 부서에서 작성한 항목만 조회가능합니다.");
                return;
            }

            if ((sender.Equals(ssListComplete) || dtpSubmit.Checked) && clsType.User.BuseCode.Equals("044201") == false)
            {
                btnSave.Enabled = false;
                ComFunc.MsgBoxEx(this, (dtpSubmit.Checked ? "접수" : "완료") + "된 신청서의 수정을 원하실경우\r\n의료정보팀으로 문의바랍니다.(8042~44)");
                return;
            }

            if (clsType.User.BuseCode.Equals("044201"))
            {
                if (dtpComplete.Checked)
                {
                    btnSave.Enabled = clsType.User.Sabun.Equals("16109") || clsType.User.Sabun.Equals("25500");
                }
                else
                {
                    btnSave.Enabled = true;
                }
            }
            else
            {
                btnSave.Enabled = clsType.User.IdNumber.Equals(txtReqName.Tag.ToString());
            }
         

            dtpSubmit.Enabled = clsType.User.BuseCode.Equals("044201");
            dtpComplete.Enabled = dtpSubmit.Enabled;


            foreach (Control control in ComFunc.GetAllControls(panApplication))
            {
                if (control is Panel || control is TableLayoutPanel)
                {
                    continue;
                }

                if (control is TextBox)
                {
                    if (control.BackColor == Color.White || control.BackColor == Color.FromArgb(255, 225, 225))
                    {
                        (control as TextBox).ReadOnly = sender.Equals(ssListComplete);
                    }
                }
            }
        }


        #endregion

        /// <summary>
        /// EMR 뷰어 실행
        /// </summary>
        private void ViewEmr(Form pForm, string strPtno)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name.Equals("frmEmrViewer"))
                {
                    (frm as frmEmrViewer).SetNewPatient(strPtno);
                    return;
                }
                else
                {
                    //fEmrViewer.SetNewPatient(GstrPANO);
                }
            }
            ComFunc.MsgBoxEx(pForm, "EMR 뷰어를 실행해주세요.");

        }

        #region 텍스트 이벤트

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            TextBox textBox = (sender as TextBox);

            if (string.IsNullOrWhiteSpace(textBox.Text.Trim()))
                return;

            textBox.Text = VB.Val(textBox.Text.Trim()).ToString("00000000");

            double ControlNo = double.Parse(textBox.Name.Substring(textBox.Name.Length - 1));

            TextBox txtName   = tableLayoutPanel2.Controls.OfType<TextBox>().Where(c => c.Name.Equals("txtName" + ControlNo)).FirstOrDefault();
            TextBox txtJm1 = tableLayoutPanel2.Controls.OfType<TextBox>().Where(c => c.Name.Equals("txtJumin" + ControlNo)).FirstOrDefault();
            TextBox txtJm2 = tableLayoutPanel2.Controls.OfType<TextBox>().Where(c => c.Name.Equals("txtJumin" + ControlNo + "_2")).FirstOrDefault();

            int PanoCnt = tableLayoutPanel2.Controls.OfType<Control>().Where(c => c.Name.IndexOf("txtPano") != -1 && c.Equals(textBox) == false && c.Text.Trim().Equals(textBox.Text)).Count();
            if (PanoCnt > 0)
            {
                ComFunc.MsgBoxEx(this, "이미 등록된 등록번호입니다.");
                textBox.Clear();
                return;
            }

            if (txtName != null)
            {
                SetPatInfo(textBox.Text.Trim(), txtName, txtJm1, txtJm2);
            }

            txtName = tableLayoutPanel2.Controls.OfType<TextBox>().Where(c => c.Name.Equals("txtPano" + (ControlNo + 1))).FirstOrDefault();
            if (txtName != null)
            {
                txtName.Focus();
            }
            else
            {
                txtHappen.Focus();
            }
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (sender.Equals(txtTel))
            {
                txtPano1.Focus();
                return;
            }

            TextBox textBox = (sender as TextBox);
            if (textBox.Multiline == true)
                return;

            SelectNextControl((Control)sender, true, true, true, false);
        }

        private void txtHappen_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHappen.Text.Trim()))
                return;

            if (System.Text.Encoding.Default.GetByteCount(txtHappen.Text.Trim()) > 1000)
            {
                ComFunc.MsgBoxEx(this, "발생내용은 1000바이트 까지만 입력가능합니다.\r\n더 입력을 원하실경우 해당 프로그램 담당자에게 문의바랍니다.");
                txtHappen.Focus();
            }
        }

        private void txtConduct_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConduct.Text.Trim()))
                return;

            if (System.Text.Encoding.Default.GetByteCount(txtConduct.Text.Trim()) > 2000)
            {
                ComFunc.MsgBoxEx(this, "처리내용은 2000바이트 까지만 입력가능합니다.\r\n더 입력을 원하실경우 해당 프로그램 담당자에게 문의바랍니다.");
                txtConduct.Focus();
            }
        }

        private void txtMemo_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (sender as TextBox);
            if (string.IsNullOrWhiteSpace(textBox.Text.Trim()))
                return;

            if (System.Text.Encoding.Default.GetByteCount(textBox.Text.Trim()) > 100)
            {
                ComFunc.MsgBoxEx(this, "메모는 100바이트 까지만 입력가능합니다.\r\n더 입력을 원하실경우 해당 프로그램 담당자에게 문의바랍니다.");
                textBox.Focus();
            }
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (sender as TextBox);
            double ControlNo = double.Parse(textBox.Name.Substring(textBox.Name.Length - 1));

            if (string.IsNullOrWhiteSpace(textBox.Text.Trim()))
            {
                TextBox txtName = tableLayoutPanel2.Controls.OfType<TextBox>().Where(c => c.Name.Equals("txtName" + ControlNo)).FirstOrDefault();
                TextBox txtJ1 = tableLayoutPanel2.Controls.OfType<TextBox>().Where(c => c.Name.Equals("txtJumin" + ControlNo)).FirstOrDefault();
                TextBox txtJ2 = tableLayoutPanel2.Controls.OfType<TextBox>().Where(c => c.Name.Equals("txtJumin" + ControlNo + "_2")).FirstOrDefault();

                if (txtName != null)
                {
                    txtName.Clear();
                    txtJ1.Clear();
                    txtJ2.Clear();
                }
            }
            else
            {
                int PanoCnt = tableLayoutPanel2.Controls.OfType<Control>().Where(c => c.Name.IndexOf("txtPano") != -1 && c.Equals(textBox) == false && c.Text.Trim().Equals(textBox.Text)).Count();
                if (PanoCnt > 0)
                {
                    ComFunc.MsgBoxEx(this, "이미 등록된 등록번호입니다.");
                    textBox.Clear();
                    return;
                }
            }
        }

        private void txtUsePano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            GetSearchData();
        }

        #endregion

        #region 콤보박스 이벤트

        private void cboUse1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (sender as ComboBox);
            double ControlNo = double.Parse(comboBox.Name.Substring(comboBox.Name.Length - 1));
            TextBox txtPano = tableLayoutPanel2.Controls.OfType<TextBox>().Where(c => c.Name.Equals("txtPano" + ControlNo)).FirstOrDefault();

            if (comboBox.SelectedIndex == 1)
            {
                if (txtPano != null)
                {
                    txtPano.BackColor = Color.White;
                }

                int UseCount = tableLayoutPanel2.Controls.OfType<ComboBox>().Where(c => c.Equals(sender) == false && c.SelectedIndex == 1).Count();
                if (UseCount > 0)
                {
                    comboBox.SelectedIndex = 0;
                    ComFunc.MsgBoxEx(this, "'사용' 항목은 이미 선택되어 있습니다.\r\n다시 확인해보세요.");
                    return;
                }
            }
            else if (comboBox.SelectedIndex == 2 || comboBox.SelectedIndex == 3)
            {
                if (txtPano != null)
                {
                    txtPano.BackColor = Color.FromArgb(255, 225, 225);
                }
            }
            else
            {
                if (txtPano != null)
                {
                    txtPano.BackColor = Color.White;
                }
            }
        }

        #endregion

        #region 탭 이벤트

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            NewWrite();
        }

        #endregion

        #region 데이트타임피커 이벤트

        private void dtpComplete_ValueChanged(object sender, EventArgs e)
        {
            int UseCount = tableLayoutPanel2.Controls.OfType<ComboBox>().Where(c => c.SelectedIndex != 0).Count();
            if (UseCount < 2)
            {
                dtpComplete.Checked = false;
            }
        }
        #endregion
    }
}
