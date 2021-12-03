using ComBase; //기본 클래스
using ComSupLibB.SupLbEx;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ComSupLibB.SupInfc
{
    /// <summary>
    /// Class Name      : frmEnvironmentQi
    /// File Name       : 환경 미생물 의뢰등록
    /// Description     :
    /// Author          : 전상원
    /// Create Date     : 2018-09-20
    /// Update History  :
    /// </summary>
    /// <history>
    ///
    /// </history>
    /// <seealso cref= " >> frmEnvironmentQi.cs 폼이름 재정의" />
    ///
    public partial class frmEnvironmentQi : Form, MainFormMessage
    {
        #region 프로퍼티
        
        private const string PANO = "81000014";
        private clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        private clsInFcSQL fcSQL = new clsInFcSQL();
        private string SqlErr = string.Empty;

        #endregion

        #region 생성자 && 로드 이벤트

        public frmEnvironmentQi()
        {
            InitializeComponent();
        }

        public frmEnvironmentQi(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
        }

        private void frmEnvironmentQi_Load(object sender, EventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
                {
                    this.Close();
                    return;
                } //폼 권한 조회
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            string GstrSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            ssReceipt_Sheet1.RowCount = 0;
            txtBarcode.Focus();
            SetInit();
        }

        #endregion

        #region 컨트롤 이벤트

        /// <summary>
        /// 접수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceipt_Click(object sender, EventArgs e)
        {
            //if (cboGrade1.SelectedValue == null
            //    || cboGrade2.SelectedValue == null
            //    || cboGrade3.SelectedValue == null)
            //{
            //    return;
            //}
            //else
            //{
            //    if (cboGrade4.Items.Count > 0 && cboGrade4.SelectedValue == null)
            //    {
            //        return;
            //    }
            //}
            string barCode = txtBarcode.Text;

            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;

            sql = new StringBuilder();
            sql.Append("SELECT *                                    ").Append("\n");
            sql.Append("  FROM ENVIRONMENT_ORDER                    ").Append("\n");
            sql.Append(" WHERE USEYN = 'Y'                          ").Append("\n");
            sql.Append("   AND BARCODE = '" + txtBarcode.Text + "'  ").Append("\n");

            SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

            if (!string.IsNullOrWhiteSpace(SqlErr))
            {
                clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt == null || dt.Rows.Count != 1)
            {
                ComFunc.MsgBox("정상적인 바코드가 아닙니다.");
                return;
            }

            string orderDate = DateTime.ParseExact(dt.Rows[0]["ORDERDATE"].ToString(), "yyyyMMdd", null).ToString("yyyy-MM-dd");
            string strSPECNO = txtBarcode.Text;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            int intRowAffected = 0;

            sql = new StringBuilder();
            sql.Append("INSERT INTO KOSMOS_OCS.EXAM_SPECMST(    ").Append("\n");
            sql.Append("    SPECNO                              ").Append("\n");
            sql.Append("  , PANO                                ").Append("\n");
            sql.Append("  , IPDOPD                              ").Append("\n");
            sql.Append("  , SNAME                               ").Append("\n");
            sql.Append("  , BI                                  ").Append("\n");
            sql.Append("  , SEX                                 ").Append("\n");
            sql.Append("  , DEPTCODE                            ").Append("\n");
            sql.Append("  , DRCODE                              ").Append("\n");
            sql.Append("  , RECEIVEDATE                         ").Append("\n");
            sql.Append("  , BDATE                               ").Append("\n");
            sql.Append("  , STATUS                              ").Append("\n");
            sql.Append("  , AGE                                 ").Append("\n");
            sql.Append("  , INPS                                ").Append("\n");
            sql.Append("  , INPT_DT                             ").Append("\n");
            sql.Append(")                                       ").Append("\n");
            sql.Append("SELECT '" + strSPECNO + "'              ").Append("\n");
            sql.Append("     , PANO                             ").Append("\n");
            sql.Append("     , 'O'                              ").Append("\n");
            sql.Append("     , SNAME                            ").Append("\n");
            sql.Append("     , BI                               ").Append("\n");
            sql.Append("     , SEX                              ").Append("\n");
            sql.Append("     , DEPTCODE                         ").Append("\n");
            sql.Append("     , DRCODE                           ").Append("\n");
            sql.Append("     , SYSDATE                          ").Append("\n");
            sql.Append("     , '" + orderDate + "'              ").Append("\n");
            sql.Append("     , '01'                             ").Append("\n");
            sql.Append("     , 0                                ").Append("\n");
            sql.Append("     , '" + clsType.User.Sabun + "'     ").Append("\n");
            sql.Append("     , SYSDATE                          ").Append("\n");
            sql.Append("  FROM KOSMOS_PMPA.BAS_PATIENT          ").Append("\n");
            sql.Append(" WHERE PANO = '" + PANO + "'            ").Append("\n");
            SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);

            if (!string.IsNullOrWhiteSpace(SqlErr))
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return;
            }

            sql = new StringBuilder();
            sql.Append("INSERT INTO KOSMOS_OCS.EXAM_RESULTC(                    ").Append("\n");
            sql.Append("    SPECNO                                              ").Append("\n");
            sql.Append("  , EQUCODE                                             ").Append("\n");
            sql.Append("  , SEQNO                                               ").Append("\n");
            sql.Append("  , PANO                                                ").Append("\n");
            sql.Append("  , MASTERCODE                                          ").Append("\n");
            sql.Append("  , SUBCODE                                             ").Append("\n");
            sql.Append("  , INPS                                                ").Append("\n");
            sql.Append("  , INPT_DT                                             ").Append("\n");
            sql.Append("  , UPPS                                                ").Append("\n");
            sql.Append("  , UPDT                                                ").Append("\n");
            sql.Append(")                                                       ").Append("\n");
            sql.Append("SELECT '" + strSPECNO + "'          AS SPACENO          ").Append("\n");
            sql.Append("     , ORDERNO                      AS ORDERNO          ").Append("\n");
            sql.Append("     , TRIM(TO_CHAR(ROWNUM, '000')) AS SEQNO            ").Append("\n");
            sql.Append("     , '" + PANO + "'               AS PANO             ").Append("\n");
            sql.Append("     , MASTERCODE                   AS MASTERCODE       ").Append("\n");
            sql.Append("     , SUBCODE                      AS SUBCODE          ").Append("\n");
            sql.Append("     , '" + clsType.User.Sabun + "' AS INPS             ").Append("\n");
            sql.Append("     , SYSDATE                      AS INPT_DT          ").Append("\n");
            sql.Append("     , '" + clsType.User.Sabun + "' AS UPPS             ").Append("\n");
            sql.Append("     , SYSDATE                      AS UPDT             ").Append("\n");
            sql.Append("  FROM                                                  ").Append("\n");
            sql.Append("  (                                                     ").Append("\n");
            sql.Append("        SELECT C.EXAMCODE   AS MASTERCODE               ").Append("\n");
            sql.Append("             , C.EXAMCODE   AS SUBCODE                  ").Append("\n");
            sql.Append("             , 0            AS SORT                     ").Append("\n");
            sql.Append("             , B.CODE       AS ORDERNO                  ").Append("\n");
            sql.Append("          FROM ENVIRONMENT_ORDER A                      ").Append("\n");
            sql.Append("          LEFT OUTER JOIN ENVIRONMENT_EXAM_MASTER B     ").Append("\n");
            sql.Append("                       ON A.ENVIRONMENTCODE = B.CODE    ").Append("\n");
            sql.Append("                      AND B.USEYN = 'Y'                 ").Append("\n");
            sql.Append("          LEFT OUTER JOIN ENVIRONMENT_EXAM_DETAIL C     ").Append("\n");
            sql.Append("                       ON B.CODE = C.EXAMMASTERCODE     ").Append("\n");
            sql.Append("                      AND C.USEYN = 'Y'                 ").Append("\n");
            sql.Append("         WHERE A.BARCODE = '" + strSPECNO + "'          ").Append("\n");
            sql.Append("        UNION ALL                                       ").Append("\n");
            sql.Append("        SELECT D.MASTERCODE AS MASTERCODE               ").Append("\n");
            sql.Append("             , D.NORMAL     AS SUBCODE                  ").Append("\n");
            sql.Append("             , D.SORT       AS SORT                     ").Append("\n");
            sql.Append("             , B.CODE       AS ORDERNO                  ").Append("\n");
            sql.Append("          FROM ENVIRONMENT_ORDER A                      ").Append("\n");
            sql.Append("          LEFT OUTER JOIN ENVIRONMENT_EXAM_MASTER B     ").Append("\n");
            sql.Append("                       ON A.ENVIRONMENTCODE = B.CODE    ").Append("\n");
            sql.Append("                      AND B.USEYN = 'Y'                 ").Append("\n");
            sql.Append("          LEFT OUTER JOIN ENVIRONMENT_EXAM_DETAIL C     ").Append("\n");
            sql.Append("                       ON B.CODE = C.EXAMMASTERCODE     ").Append("\n");
            sql.Append("                      AND C.USEYN = 'Y'                 ").Append("\n");
            sql.Append("          LEFT OUTER JOIN KOSMOS_OCS.EXAM_MASTER CC     ").Append("\n");
            sql.Append("                       ON C.EXAMCODE = CC.MASTERCODE    ").Append("\n");
            sql.Append("          LEFT OUTER JOIN KOSMOS_OCS.EXAM_MASTER_SUB D  ").Append("\n");
            sql.Append("                       ON C.EXAMCODE = D.MASTERCODE     ").Append("\n");
            sql.Append("                      AND D.GUBUN = 31                  ").Append("\n");
            sql.Append("          LEFT OUTER JOIN KOSMOS_OCS.EXAM_MASTER E      ").Append("\n");
            sql.Append("                       ON D.NORMAL = E.MASTERCODE       ").Append("\n");
            sql.Append("         WHERE A.BARCODE = '" + strSPECNO + "'          ").Append("\n");
            sql.Append("  )                                                     ").Append("\n");
            sql.Append(" WHERE MASTERCODE IS NOT NULL                           ").Append("\n");
            sql.Append("ORDER BY SORT                                           ").Append("\n");

            SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);

            if (!string.IsNullOrWhiteSpace(SqlErr))
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return;
            }

            sql = new StringBuilder();
            sql.Append("UPDATE ENVIRONMENT_ORDER                            ").Append("\n");
            sql.Append("   SET SPECNO       = '" + strSPECNO + "'           ").Append("\n");
            sql.Append("     , RECEIPTDATE  = TO_CHAR(SYSDATE, 'YYYYMMDD')  ").Append("\n");
            sql.Append("     , RECEIPTTIME  = TO_CHAR(SYSDATE, 'HH24MMSS')  ").Append("\n");
            sql.Append("     , RECEIPTUSER  = '" + clsType.User.Sabun + "'  ").Append("\n");
            sql.Append(" WHERE BARCODE  = '" + barCode + "'                 ").Append("\n");

            SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);

            if (!string.IsNullOrWhiteSpace(SqlErr))
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
            ComFunc.MsgBox("저장하였습니다.");
        }

        /// <summary>
        /// 접수 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SetReceiptDataBind();
        }

        /// <summary>
        /// 접수 리스트 숨김/표시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHide_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;

            btnHide.Text = "◀";
            if (splitContainer1.Panel1Collapsed)
            {
                btnHide.Text = "▶";
            }
        }

        /// <summary>
        /// 단계별 조회 (화면상 표시 되지않음 추후 삭제 가능함)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbo = sender as ComboBox;
            if (cbo.SelectedValue == null || string.IsNullOrWhiteSpace(cbo.SelectedValue.ToString()))
            {
                return;
            }

            string grade = cbo.Name.Substring(cbo.Name.Length - 1);
            int pCode = Convert.ToInt32(cbo.SelectedValue);
            switch (grade)
            {
                case "1": GetGradeCode(cboGrade2, 2, pCode); break;
                case "2": GetGradeCode(cboGrade3, 3, pCode); break;
                case "3": GetGradeCode(cboGrade4, 4, pCode); break;
                case "4": break;
            }
        }

        /// <summary>
        /// 바코드 리딩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

            }
        }

        /// <summary>
        /// 검체조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSpecSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSpec.Text))
            {
                return;
            }

            ExamResultDataBind(txtSpec.Text);
        }

        /// <summary>
        /// 스펙 바코드 리딩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSpec_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                fcSQL.ExamResultClone(txtSpec.Text);
            }
        }

        /// <summary>
        /// 바코드 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBarcodePrint_Click(object sender, EventArgs e)
        {
            clsLbExBarCodePrint cls = new clsLbExBarCodePrint();
            //strBARCODE = setBarCodeSpecimen("201811190001", "P / C / 2.7 ml");

            string Prdata = "";
            Prdata = Prdata + "^XA^BY2,2.0^FS";
            Prdata = Prdata + "^SEE:UHANGUL.DAT^FS";
            Prdata = Prdata + "^CW1,E:KFONT3.FNT^FS";
            Prdata = Prdata + "^FO10,25^CI26^A1N,30,30^FD전산실^FS";

            Prdata = Prdata + "^FO160,30^A0N,30,25^FD81000014^FS";

            Prdata = "^XA^LH0,0^FS^SEE:UHANGUL.DAT^FS^CW1,E:KFONT3.FNT^FS^FO20,20^CI26^A1N,30,30^FD";
            Prdata += " 감염검사 ";
            Prdata += "^FS^FO150,20^A0N,30,25^FD";
            //Prdata += "AU5800A        ";
            //Prdata += "Low ";
            Prdata += "^FS^FO20,70^A0N,45,45^FDQ^FS^FO50,50^B3N,N,48,N,N^BY2,2:1,85^FD";
            Prdata += "201811190001";
            Prdata += "^FS^FO50,140^A0N,25,20^FD";
            Prdata += "20181119-0001   ";
            //Prdata += "Lot : 0001 ITA-2";
            Prdata += "^FS^FO50,165^A0N,18,18^FD";
            Prdata += "SE04A,SE04B^FS^XZ";
            bool d = ComPrintApi.SendStringToPrinter("혈액환자정보", Prdata);
            //cls.printSpecimenBarCode(clsDB.DbCon, "201811190001", clsLbExBarCodePrint.enmPrintType.COM_PORT);
        }

        /// <summary>
        /// 결과항목 선택 풋노트 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssSpec_CellClick(object sender, CellClickEventArgs e)
        {
            ssFootNote_Sheet1.Rows.Clear();

            if (ssSpec_Sheet1.Cells[e.Row, 2].Text.Equals("F"))
            {
                DataTable dt = null;
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT SPECNO, SEQNO                    ").Append("\n");
                sql.Append("     , FOOTNOTE, SORT                   ").Append("\n");
                sql.Append("     , ANATNO,  PANO                    ").Append("\n");
                sql.Append("  FROM KOSMOS_OCS.EXAM_RESULTCF         ").Append("\n");
                sql.Append(" WHERE SPECNO = '" + txtSpec.Text + "'  ").Append("\n");
                sql.Append("ORDER BY SORT                           ").Append("\n");
                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssFootNote_Sheet1.RowCount = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssFootNote_Sheet1.Cells[i, 0].Value = dt.Rows[i]["FOOTNOTE"];
                }
            }
        }

        /// <summary>
        /// 접수된 검체 상세보기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssView_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader || e.RowHeader)
            {
                return;
            }

            int row = e.Row;
            if (e.Row % 2 != 0)
            {
                row--;
            }

            string specNo = ssReceipt_Sheet1.Cells[row, 1].Text;
            txtSpec.Text = specNo;

            ExamResultDataBind(specNo);
        }

        /// <summary>
        /// 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region 메소드
        
        /// <summary>
        /// init
        /// </summary>
        private void SetInit()
        {
            GetGradeCode(cboGrade1, 1, 0);
        }

        /// <summary>
        /// 콤보박스 데이터 바인딩
        /// </summary>
        /// <param name="cbo"></param>
        /// <param name="grade"></param>
        /// <param name="pCode"></param>
        private void GetGradeCode(ComboBox cbo, int grade, int pCode = 0)
        {
            StringBuilder sql = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                sql = new StringBuilder();
                sql.Append("SELECT CODE, CODENAME           ").Append("\n");
                sql.Append("  FROM BAS_ENVIRONMENT          ").Append("\n");
                sql.Append(" WHERE GRADE    = " + grade + " ").Append("\n");
                sql.Append("   AND PCODE    = " + pCode + " ").Append("\n");
                sql.Append("   AND DEL      = 'N'           ").Append("\n");
                sql.Append("ORDER BY CODE                   ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                cbo.DisplayMember = "CODENAME";
                cbo.ValueMember = "CODE";
                cbo.DataSource = dt;
                cbo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, sql.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 접수현황 데이터 바인딩
        /// </summary>
        private void SetReceiptDataBind()
        {
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;

            ssReceipt_Sheet1.Rows.Clear();
            try
            {
                string receiptDate = dtpJobDate.Value.ToString("yyyyMMdd");
                Cursor.Current = Cursors.WaitCursor;

                sql = new StringBuilder();
                sql.Append("SELECT A.SPECNO, C.BUCODE, G.NAME, H.USERNAME   ").Append("\n");
                sql.Append("     , C.CODENAME AS GRADENAME1                 ").Append("\n");
                sql.Append("     , D.CODENAME AS GRADENAME2                 ").Append("\n");
                sql.Append("     , E.CODENAME AS GRADENAME3                 ").Append("\n");
                sql.Append("     , F.CODENAME AS GRADENAME4                 ").Append("\n");
                sql.Append("     , A.RECEIPTDATE                            ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_ORDER A                      ").Append("\n");
                sql.Append("  LEFT OUTER JOIN ENVIRONMENT_EXAM_MASTER B     ").Append("\n");
                sql.Append("               ON A.ENVIRONMENTCODE = B.CODE    ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT C       ").Append("\n");
                sql.Append("               ON B.GRADE1 = C.CODE             ").Append("\n");
                sql.Append("              AND C.GRADE = 1                   ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT D       ").Append("\n");
                sql.Append("               ON B.GRADE2 = D.CODE             ").Append("\n");
                sql.Append("              AND D.GRADE = 2                   ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT E       ").Append("\n");
                sql.Append("               ON B.GRADE3 = E.CODE             ").Append("\n");
                sql.Append("              AND E.GRADE = 3                   ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT F       ").Append("\n");
                sql.Append("               ON B.GRADE4 = F.CODE             ").Append("\n");
                sql.Append("              AND F.GRADE = 4                   ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_BUCODE G                  ").Append("\n");
                sql.Append("               ON C.BUCODE = G.BUCODE           ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_USER H                    ").Append("\n");
                sql.Append("               ON A.BARCODEUSER = TRIM(H.SABUN) ").Append("\n");
                sql.Append(" WHERE RECEIPTDATE = '" + receiptDate + "'      ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                ssReceipt_Sheet1.RowCount = dt.Rows.Count * 2;
                int cnt = 0;
                for(int i=0; i< ssReceipt_Sheet1.RowCount; i++)
                {
                    ssReceipt_Sheet1.Cells[i, 0].Value = false;
                    ssReceipt_Sheet1.Cells[i, 1].Value = dt.Rows[cnt]["SPECNO"];
                    ssReceipt_Sheet1.Cells[i, 2].Value = dt.Rows[cnt]["NAME"];
                    ssReceipt_Sheet1.Cells[i, 3].Value = dt.Rows[cnt]["USERNAME"];
                    ssReceipt_Sheet1.Cells[i, 4].Value = ComFunc.FormatStrToDate(dt.Rows[cnt]["RECEIPTDATE"].ToString(), "D");

                    ssReceipt_Sheet1.Cells[i + 1, 1].Value = dt.Rows[cnt]["GRADENAME1"];
                    ssReceipt_Sheet1.Cells[i + 1, 2].Value = dt.Rows[cnt]["GRADENAME2"];
                    ssReceipt_Sheet1.Cells[i + 1, 3].Value = dt.Rows[cnt]["GRADENAME3"];
                    ssReceipt_Sheet1.Cells[i + 1, 4].Value = dt.Rows[cnt]["GRADENAME4"];

                    if (i % 2 == 0)
                    {
                        ssReceipt_Sheet1.Cells[i, 0].RowSpan = 2;
                    }

                    i = i + 1;
                    cnt++;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, sql.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 검체 결과조회
        /// </summary>
        /// <param name="specNo"></param>
        private void ExamResultDataBind(string specNo)
        {
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                ssSpec_Sheet1.Rows.Clear();

                Cursor.Current = Cursors.WaitCursor;
                sql = new StringBuilder();
                sql.Append("SELECT A.STATUS                                         ").Append("\n");
                sql.Append("     , B.EXAMNAME                                       ").Append("\n");
                sql.Append("     , A.RESULT                                         ").Append("\n");
                sql.Append("     , A.RESULTDATE                                     ").Append("\n");
                sql.Append("     , A.SEQNO                                          ").Append("\n");
                sql.Append("     , (SELECT COUNT(*) FROM KOSMOS_OCS.EXAM_RESULTCF   ").Append("\n");
                sql.Append("         WHERE SPECNO = A.SPECNO AND SEQNO = A.SEQNO    ").Append("\n");
                sql.Append("       ) AS FOOTNOTE                                    ").Append("\n");
                sql.Append("  FROM KOSMOS_OCS.EXAM_RESULTC A                        ").Append("\n");
                sql.Append("  LEFT OUTER JOIN KOSMOS_OCS.EXAM_MASTER B              ").Append("\n");
                sql.Append("               ON A.SUBCODE = B.MASTERCODE              ").Append("\n");
                sql.Append("  LEFT OUTER JOIN KOSMOS_OCS.EXAM_RESULTCF C            ").Append("\n");
                sql.Append("               ON A.SPECNO  = C.SPECNO                  ").Append("\n");
                sql.Append("              AND A.SEQNO   = C.SEQNO                   ").Append("\n");
                sql.Append(" WHERE A.SPECNO = '" + specNo + "'                      ").Append("\n");
                sql.Append("ORDER BY A.SEQNO                                        ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssSpec_Sheet1.RowCount = dt.Rows.Count;
                DateTime dtm;
                for(int i=0; i<dt.Rows.Count; i++)
                {
                    if ("V".Equals(dt.Rows[i]["STATUS"]))
                    {
                        ssSpec_Sheet1.Cells[i, 0].BackColor = Color.Red;
                        ssSpec_Sheet1.Cells[i, 0].ForeColor = Color.White;
                        ssSpec_Sheet1.Cells[i, 0].Value = "전";
                    }

                    ssSpec_Sheet1.Cells[i, 1].Value = dt.Rows[i]["EXAMNAME"];
                    if((decimal)dt.Rows[i]["FOOTNOTE"] > 0)
                    {
                        ssSpec_Sheet1.Cells[i, 2].BackColor = Color.Red;
                        ssSpec_Sheet1.Cells[i, 2].ForeColor = Color.White;
                        ssSpec_Sheet1.Cells[i, 2].Value = "F";
                    }
                    
                    ssSpec_Sheet1.Cells[i, 3].Value = dt.Rows[i]["RESULT"];

                    if(dt.Rows[0]["RESULTDATE"] != null && dt.Rows[0]["RESULTDATE"] != DBNull.Value)
                    {
                        dtm = (DateTime)dt.Rows[0]["RESULTDATE"];
                        txtResultDate.Text = dtm.ToString("yyyy-MM-dd HH:mm");
                    }
                }

                dt = fcSQL.GetEnvironmentSpecInfo(specNo);
                if(dt != null && dt.Rows.Count > 0)
                {
                    lblGrade1.Text = dt.Rows[0]["GRADENAME1"].ToString();
                    lblGrade2.Text = dt.Rows[0]["GRADENAME2"].ToString();
                    lblGrade3.Text = dt.Rows[0]["GRADENAME3"].ToString();
                    lblGrade4.Text = dt.Rows[0]["GRADENAME4"].ToString();
                }

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, sql.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        #endregion

        #region 인터페이스

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

        #region 인터페이스 상속시 필수 이벤트

        private void frmEnvironmentQi_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmEnvironmentQi_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        #endregion

        #endregion

    }
}