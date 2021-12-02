using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using System.Drawing;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

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
    /// <seealso cref= " >> frmEnvironmentOrder.cs" />
    ///
    public partial class frmEnvironmentState : Form, MainFormMessage
    {
        #region 프로퍼티
        
        /// <summary>
        /// 스프레드 수정여부
        /// </summary>
        private Dictionary<string, object> SpreadChanged = null;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
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

        private void frm_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        #endregion

        #region 생성자 && 로드

        public frmEnvironmentState()
        {
            InitializeComponent();
        }

        public frmEnvironmentState(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
        }

        private void frmEnvironmentState_Load(object sender, EventArgs e)
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

            //Init();
        }

        #endregion

        #region 컨트롤 이벤트


        #endregion

        #region 메소드

        private void Init()
        {
            ssMain_Sheet1.Rows.Clear();
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                sql = new StringBuilder();
                sql.Append("SELECT GRADE, CODE, CODENAME    ").Append("\n");
                sql.Append("  FROM BAS_GRADE_ENVIRONMENT    ").Append("\n");
                sql.Append(" WHERE GRADE = 1                ").Append("\n");
                sql.Append("   AND USEYN = 'Y'              ").Append("\n");
                sql.Append("ORDER BY CODE                   ").Append("\n");
                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    dt.Dispose();
                    dt = null;

                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["CODENAME"] = "전체";
                    newRow["CODE"] = DBNull.Value;
                    newRow["GRADE"] = 1;
                    dt.Rows.InsertAt(newRow, 0);

                    cboGrade1.DisplayMember = "CODENAME";
                    cboGrade1.ValueMember = "CODE";
                    cboGrade1.DataSource = dt;
                }
      
            }
            catch (Exception ex)
            {
              
                clsDB.SaveSqlErrLog(ex.Message, sql.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
            
            dt.Dispose();
            dt = null;
            
            CS.setSpdFilter(ssMain, 18, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
        }

        #endregion

        /// <summary>
        /// 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;
            ssMain_Sheet1.Rows.Clear();
            try
            {
                string startDt = dtpStartDate.Value.ToString("yyyyMMdd");
                string endDt = dtpEndDate.Value.ToString("yyyyMMdd");
                Cursor.Current = Cursors.WaitCursor;

                sql = new StringBuilder();
                sql.Append("SELECT A.ORDERNO, A.BUCODE, A.REMARK, A.ORDERDATE, A.ORDERTIME                 ").Append("\n");
                sql.Append("     , CASE K.STATUS WHEN '00' THEN '미접수'                                   ").Append("\n");
                sql.Append("                     WHEN '01' THEN '검사중'                                   ").Append("\n");
                sql.Append("                     WHEN '02' THEN '부분입력'                                  ").Append("\n");
                sql.Append("                     WHEN '03' THEN '모두입력'                                  ").Append("\n");
                sql.Append("                     WHEN '04' THEN '부분완료'                                  ").Append("\n");
                sql.Append("                     WHEN '05' THEN '검사완료'                                  ").Append("\n");
                sql.Append("                     WHEN '06' THEN '취소'                                      ").Append("\n");
                sql.Append("       END AS STATUS                                                            ").Append("\n");
                sql.Append("     , A.BARCODEDATE, A.BARCODETIME                                             ").Append("\n");
                sql.Append("     , A.SPECNO                                                                 ").Append("\n");
                sql.Append("     , C.CODENAME AS GRADENAM1                                                  ").Append("\n");
                sql.Append("     , D.CODENAME AS GRADENAM2                                                  ").Append("\n");
                sql.Append("     , E.CODENAME AS GRADENAM3                                                  ").Append("\n");
                sql.Append("     , F.CODENAME AS GRADENAM4                                                  ").Append("\n");
                sql.Append("     , G.NAME AS DEPTNAME                                                       ").Append("\n");
                sql.Append("     , H.USERNAME AS ORDERUSERNAME                                              ").Append("\n");
                sql.Append("     , I.USERNAME AS BARCODEUSERNAME                                            ").Append("\n");
                sql.Append("     , K.JEPSUNAME AS RECEIPTUSERNAME                                           ").Append("\n");
                sql.Append("     , TO_CHAR(K.RECEIVEDATE, 'YYYY-MM-DD') AS RECEIPTDATE                      ").Append("\n");
                sql.Append("     , TO_CHAR(K.RECEIVEDATE, 'HH24:MI:SS') AS RECEIPTTIME                      ").Append("\n");
                sql.Append("     , (                                                                        ").Append("\n");
                sql.Append("            SELECT LISTAGG(BB.EXAMNAME, ',') WITHIN GROUP(ORDER BY BB.EXAMNAME) ").Append("\n");
                sql.Append("              FROM ENVIRONMENT_EXAM_DETAIL AA                                   ").Append("\n");
                sql.Append("              INNER JOIN KOSMOS_OCS.EXAM_MASTER BB                              ").Append("\n");
                sql.Append("                      ON AA.EXAMCODE = BB.MASTERCODE                            ").Append("\n");
                sql.Append("                     AND AA.USEYN = 'Y'                                         ").Append("\n");
                sql.Append("             WHERE AA.EXAMMASTERCODE = B.CODE                                   ").Append("\n");
                sql.Append("           GROUP BY AA.EXAMMASTERCODE                                           ").Append("\n");
                sql.Append("       ) AS EXAMNAME                                                            ").Append("\n");
                //2019-02-28 안정수, 결과일, 결과시간, 검사자 추가
                sql.Append("     , K.RESULTDATE                                                             ").Append("\n");
                sql.Append("     , K.JEPSUNAME2                                                             ").Append("\n");
                sql.Append("     , K.UPPS                                                                   ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_ORDER A                                                      ").Append("\n");
                sql.Append("  INNER JOIN ENVIRONMENT_EXAM_MASTER B                                          ").Append("\n");
                sql.Append("          ON A.ENVIRONMENTCODE = B.CODE                                         ").Append("\n");
                sql.Append("         AND B.USEYN = 'Y'                                                      ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT C                                       ").Append("\n");
                sql.Append("               ON B.GRADE1  = C.CODE                                            ").Append("\n");
                sql.Append("              AND C.GRADE   = 1                                                 ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT D                                       ").Append("\n");
                sql.Append("               ON B.GRADE2  = D.CODE                                            ").Append("\n");
                sql.Append("              AND D.GRADE   = 2                                                 ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT E                                       ").Append("\n");
                sql.Append("               ON B.GRADE3  = E.CODE                                            ").Append("\n");
                sql.Append("              AND E.GRADE   = 3                                                 ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT F                                       ").Append("\n");
                sql.Append("               ON B.GRADE4  = F.CODE                                            ").Append("\n");
                sql.Append("              AND F.GRADE   = 4                                                 ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_BUCODE G                                                  ").Append("\n");
                sql.Append("               ON C.BUCODE = G.BUCODE                                           ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_USER H                                                    ").Append("\n");
                sql.Append("               ON A.ORDERUSER = TRIM(H.SABUN)                                   ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_USER I                                                    ").Append("\n");
                sql.Append("               ON A.BARCODEUSER = TRIM(I.SABUN)                                 ").Append("\n");
                //sql.Append("  LEFT OUTER JOIN BAS_USER J                                                    ").Append("\n");
                //sql.Append("               ON A.RECEIPTUSER = TRIM(J.SABUN)                                 ").Append("\n");
                sql.Append("  LEFT OUTER JOIN KOSMOS_OCS.EXAM_SPECMST K                                     ").Append("\n");
                sql.Append("               ON A.SPECNO = K.SPECNO                                           ").Append("\n");
                sql.Append(" WHERE A.ORDERDATE BETWEEN '" + startDt + "' AND '" + endDt + "'                ").Append("\n");
                sql.Append("   AND B.GRADE1 LIKE '%" + cboGrade1.SelectedValue + "%'                        ").Append("\n");
                sql.Append("   AND B.GRADE2 LIKE '%" + cboGrade2.SelectedValue + "%'                        ").Append("\n");
                
                //  바코드 미출력
                if (chkNoBarcode.Checked)
                {
                    sql.Append("   AND A.BARCODE IS NULL                                                        ").Append("\n");
                }

                if(rdoNotAll.Checked)
                {
                    sql.Append("   AND K.STATUS NOT IN('00', '05')                                              ").Append("\n");
                }

                sql.Append("ORDER BY ORDERNO                                                            ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr)) 
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssMain_Sheet1.Rows.Count = dt.Rows.Count;
                for(int i=0; i<dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];

                    ssMain_Sheet1.Cells[i, 0].Value = false;
                    ssMain_Sheet1.Cells[i, 1].Value = row["ORDERNO"];
                    ssMain_Sheet1.Cells[i, 2].Value = ComFunc.FormatStrToDate(row["ORDERDATE"].ToString(), "D");
                    ssMain_Sheet1.Cells[i, 3].Value = ComFunc.FormatStrToDate(row["ORDERTIME"].ToString(), "T");
                    ssMain_Sheet1.Cells[i, 4].Value = row["ORDERUSERNAME"];
                    //ssMain_Sheet1.Cells[i, 5].Value = row["DEPTNAME"];
                    ssMain_Sheet1.Cells[i, 5].Value = row["GRADENAM1"];
                    ssMain_Sheet1.Cells[i, 6].Value = row["REMARK"];

                    ssMain_Sheet1.Cells[i, 7].Value = string.Concat(row["GRADENAM1"], "-", row["GRADENAM2"], "-", row["GRADENAM3"]);
                    ssMain_Sheet1.Cells[i, 8].Value = row["EXAMNAME"];

                    ssMain_Sheet1.Cells[i, 9].Value = ComFunc.FormatStrToDate(row["BARCODEDATE"].ToString(), "D");
                    ssMain_Sheet1.Cells[i, 10].Value = ComFunc.FormatStrToDate(row["BARCODETIME"].ToString(), "T");
                    ssMain_Sheet1.Cells[i, 11].Value = row["BARCODEUSERNAME"];
                    ssMain_Sheet1.Cells[i, 12].Value = row["RECEIPTDATE"];
                    ssMain_Sheet1.Cells[i, 13].Value = row["RECEIPTTIME"];
                    ssMain_Sheet1.Cells[i, 14].Value = row["RECEIPTUSERNAME"];

                    //2019-02-28 안정수, 검사관련 날짜 시간 검사자 추가 
                    ssMain_Sheet1.Cells[i, 15].Value = VB.Left(row["RESULTDATE"].ToString(), 10);
                    ssMain_Sheet1.Cells[i, 16].Value = VB.Right(row["RESULTDATE"].ToString(), 8);
                    if (string.IsNullOrEmpty(VB.Left(row["RESULTDATE"].ToString(), 10)) == false)
                    {
                        ssMain_Sheet1.Cells[i, 17].Value = CF.Read_SabunName(clsDB.DbCon, row["UPPS"].ToString().Trim()); 
                    }

                    ssMain_Sheet1.Cells[i, 18].Value = row["STATUS"];
                    ssMain_Sheet1.Cells[i, 19].Value = row["SPECNO"];
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

        private void cboGrade1_SelectedIndexChanged(object sender, EventArgs e)
        {
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;

            object grade = cboGrade1.SelectedValue;
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                sql = new StringBuilder();
                sql.Append("SELECT B.CODE, B.CODENAME           ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_EXAM_MASTER A    ").Append("\n");
                sql.Append("  INNER JOIN BAS_GRADE_ENVIRONMENT B").Append("\n");
                sql.Append("          ON A.GRADE2   = B.CODE    ").Append("\n");
                sql.Append("         AND B.GRADE    = 2         ").Append("\n");
                sql.Append("         AND B.USEYN    = 'Y'       ").Append("\n");
                sql.Append(" WHERE GRADE1 LIKE '%" + grade + "%'").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                DataRow newRow = dt.NewRow();
                newRow["CODENAME"] = "전체";
                newRow["CODE"] = DBNull.Value;
                dt.Rows.InsertAt(newRow, 0);

                cboGrade2.DisplayMember = "CODENAME";
                cboGrade2.ValueMember = "CODE";
                cboGrade2.DataSource = dt;
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

        private void ssMain_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //frmEnvironmentPRT pRT = new frmEnvironmentPRT();
            //pRT.ShowDialog();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            bool isCheck = false;
            string deptName = string.Empty;
            List<string> specList = new List<string>();
            for(int i=0; i<ssMain_Sheet1.Rows.Count; i++)
            {
                deptName = ssMain_Sheet1.Cells[i, 5].Text;
                if(true.Equals(ssMain_Sheet1.Cells[i, 0].Value))
                {
                    isCheck = true;
                    specList.Add(string.Concat("'", ssMain_Sheet1.Cells[i, 19].Text, "'"));
                }
            }

            if(!isCheck)
            {
                ComFunc.MsgBox("선택된 항목이 없습니다.");
                return;
            }

            frmEnvironmentPRT pRT = new frmEnvironmentPRT(specList);
            pRT.ShowDialog();
        }

        private void ssMain_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if(!true.Equals(ssMain_Sheet1.Cells[e.Row, 0].Value))
            {
                return;
            }

            string deptName = ssMain_Sheet1.Cells[e.Row, 5].Text;
            for (int i=0; i<ssMain_Sheet1.Rows.Count; i++)
            {
                if(true.Equals(ssMain_Sheet1.Cells[i, 0].Value))
                {
                    if(!string.IsNullOrWhiteSpace(deptName))
                    {
                        //  부서가 다른경우 선택을 하지 못하게 한다.
                        if (!deptName.Equals(ssMain_Sheet1.Cells[i, 5].Text))
                        {
                            ssMain_Sheet1.Cells[e.Row, 0].Value = false;
                            ComFunc.MsgBox("서로다른 부서는 출력 할수 없습니다.");
                        }
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void ssMain_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                if (e.Column == 0)
                {
                    bool check = Convert.ToBoolean(ssMain_Sheet1.ColumnHeader.Cells[0, 0].Value);

                    ssMain_Sheet1.ColumnHeader.Cells[0, 0].Value = !check;

                    for (int i = 0; i < ssMain_Sheet1.RowCount; i++)
                    {
                        ssMain_Sheet1.Cells[i, 0].Value = !check;
                    }
                }
            }
        }
    }
}