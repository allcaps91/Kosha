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
    public partial class frmEnvironmentOrder : Form, MainFormMessage
    {
        #region 프로퍼티
        
        /// <summary>
        /// 스프레드 수정여부
        /// </summary>
        private Dictionary<string, object> SpreadChanged = null;

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

        public frmEnvironmentOrder()
        {
            InitializeComponent();
        }

        public frmEnvironmentOrder(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
        }

        private void frmEnvironmentOrder_Load(object sender, EventArgs e)
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

            Init();

            cboGrade1.SelectedIndexChanged += cboGrade_SelectedIndexChanged;
            cboGrade2.SelectedIndexChanged += cboGrade_SelectedIndexChanged;
            GetMasterDataBind();
        }

        #endregion

        #region 컨트롤 이벤트

        /// <summary>
        /// 단계별 조회 조건
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string grade = comboBox.Name.Substring(comboBox.Name.Length - 1);

            if (grade.Equals("1"))
            {
                GetGradeData(cboGrade2);
            }

            GetMasterDataBind();
        }

        /// <summary>
        /// 스프레드 변경 체크
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssOrderList_Change(object sender, ChangeEventArgs e)
        {
            if (SpreadChanged == null)
            {
                SpreadChanged = new Dictionary<string, object>();
            }

            if (!SpreadChanged.ContainsKey(ssOrderList.Name))
            {
                SpreadChanged.Add(ssOrderList.Name, true);
            }
        }

        /// <summary>
        /// 검사항목 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssMain_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                if (e.Column == 0)
                {
                    bool check = Convert.ToBoolean(ssMain_Sheet1.ColumnHeader.Cells[e.Row, e.Column].Value);
                    ssMain_Sheet1.ColumnHeader.Cells[e.Row, e.Column].Value = !check;

                    for (int i = 0; i < ssMain_Sheet1.RowCount - 2; i++)
                    {
                        if(ssMain_Sheet1.Cells[i, 0].Locked)
                        {
                            i = i + 2;
                            continue;
                        }

                        ssMain_Sheet1.Cells[i, 0].Value = !check;
                        ssMain_Sheet1.Cells[i + 2, 1].Locked = check;
                        i = i + 2;
                    }

                    EditorNotifyEventArgs eventArgs = new EditorNotifyEventArgs(e.View, null, e.Row, e.Column);
                    ssMain_EditChange(sender, eventArgs);
                }
                return;
            }
        }

        /// <summary>
        /// 오더 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrderSave_Click(object sender, EventArgs e)
        {
            if (SpreadChanged == null)
            {
                ComFunc.MsgBox("수정 내용이 없습니다.");
                return;
            }

            if (ComFunc.MsgBoxQ("선택항목으로 처방을 전송 하시겠습니까?") != DialogResult.Yes)
            {
                return;
            }

            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;
            int intRowAffected = 0;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < ssMain_Sheet1.RowCount; i++)
                {
                    if (!true.Equals(ssMain_Sheet1.Cells[i, 0].Value))
                    {
                        i = i + 2;
                        continue;
                    }

                    object environmentCode = ssMain_Sheet1.Cells[i, 6].Value;
                    sql = new StringBuilder();
                    sql.Append("SELECT *                                            ").Append("\n");
                    sql.Append("  FROM ENVIRONMENT_ORDER                            ").Append("\n");
                    sql.Append(" WHERE ORDERDATE = TO_CHAR(SYSDATE, 'YYYYMMDD')     ").Append("\n");
                    sql.Append("   AND ENVIRONMENTCODE = '" + environmentCode + "'  ").Append("\n");
                    sql.Append("   AND USEYN = 'Y'                                  ").Append("\n");

                    SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        i = i + 2;
                        continue;
                    }

                    sql = new StringBuilder();
                    sql.Append("SELECT TO_CHAR(SYSDATE, 'YYYYMMDD') ||                      ").Append("\n");
                    sql.Append("       TRIM(TO_CHAR(SEQ_ENVIRONMENT_ORDER.NEXTVAL, '0000')) ").Append("\n");
                    sql.Append("  FROM DUAL                                                 ").Append("\n");
                    SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    string orderNo = dt.Rows[0][0].ToString();
                    string remark = ssMain_Sheet1.Cells[i + 2, 1].Text;
                    string buCode = ssMain_Sheet1.Cells[i, 5].Text;

                    sql = new StringBuilder();
                    sql.Append("INSERT INTO ENVIRONMENT_ORDER (                                         ").Append("\n");
                    sql.Append("    ORDERNO                                                             ").Append("\n");
                    sql.Append("  , ENVIRONMENTCODE                                                     ").Append("\n");
                    sql.Append("  , BUCODE                                                              ").Append("\n");
                    sql.Append("  , REMARK                                                              ").Append("\n");
                    sql.Append("  , ORDERDATE                                                           ").Append("\n");
                    sql.Append("  , ORDERTIME                                                           ").Append("\n");
                    sql.Append("  , ORDERUSER                                                           ").Append("\n");
                    sql.Append("  , USEYN                                                               ").Append("\n");
                    sql.Append("  , INSDATE                                                             ").Append("\n");
                    sql.Append("  , INSTIME                                                             ").Append("\n");
                    sql.Append("  , INSUSER                                                             ").Append("\n");
                    sql.Append(")                                                                       ").Append("\n");
                    sql.Append("VALUES (                                                                ").Append("\n");
                    sql.Append("    '" + orderNo + "'                                                   ").Append("\n");
                    sql.Append("  , '" + environmentCode + "'                                           ").Append("\n");
                    sql.Append("  , '" + buCode + "'                                                    ").Append("\n");
                    sql.Append("  , '" + remark + "'                                                    ").Append("\n");
                    sql.Append("  , TO_CHAR(SYSDATE, 'YYYYMMDD')                                        ").Append("\n");
                    sql.Append("  , TO_CHAR(SYSDATE, 'HH24MMSS')                                        ").Append("\n");
                    sql.Append("  , '" + clsType.User.Sabun + "'                                        ").Append("\n");
                    sql.Append("  , 'Y'                                                                 ").Append("\n");
                    sql.Append("  , TO_CHAR(SYSDATE, 'YYYYMMDD')                                        ").Append("\n");
                    sql.Append("  , TO_CHAR(SYSDATE, 'HH24MMSS')                                        ").Append("\n");
                    sql.Append("  , '" + clsType.User.Sabun + "'                                        ").Append("\n");
                    sql.Append(")                                                                       ").Append("\n");

                    SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);

                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    i = i + 2;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");

                ssMain_Sheet1.ColumnHeader.Cells[0, 0].Value = false;
                //  스프레드 재조회
                GetMasterDataBind();

                btnSearch.PerformClick();
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
        /// 체크박스 체인지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssMain_Change(object sender, ChangeEventArgs e)
        {
            EditorNotifyEventArgs eventArgs = new EditorNotifyEventArgs(e.View, null, e.Row, e.Column);
            ssMain_EditChange(sender, eventArgs);

            bool chcek = Convert.ToBoolean(ssMain_Sheet1.Cells[e.Row, 0].Value);
            ssMain_Sheet1.Cells[e.Row + 2, 1].Locked = !chcek;
        }

        /// <summary>
        /// 스프레드 수정체크
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssMain_EditChange(object sender, EditorNotifyEventArgs e)
        {
            if (SpreadChanged == null)
            {
                SpreadChanged = new Dictionary<string, object>();
            }

            if (!SpreadChanged.ContainsKey((sender as Control).Name))
            {
                SpreadChanged.Add((sender as Control).Name, true);
            }
        }

        /// <summary>
        /// 오더조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (SpreadChanged != null && SpreadChanged.ContainsKey(ssOrderList.Name))
            {
                SpreadChanged.Remove(ssOrderList.Name);
            }

            SetOrderListDataBind();
        }

        /// <summary>
        /// 오더 삭제
        /// 바코드 출력된 오더는 삭제 하지 못함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrderDelete_Click(object sender, EventArgs e)
        {
            if (SpreadChanged == null || !SpreadChanged.ContainsKey(ssOrderList.Name))
            {
                ComFunc.MsgBox("선택된 항목이 없습니다.");
                return;
            }

            if (ComFunc.MsgBoxQ("선택된 항목을 삭제 하시겠습니까?") != DialogResult.Yes)
            {
                return;
            }

            int intRowAffected = 0;
            string SqlErr = string.Empty;
            StringBuilder sql = null;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < ssOrderList_Sheet1.RowCount; i++)
                {
                    if (!Convert.ToBoolean(ssOrderList_Sheet1.Cells[i, 0].Value))
                    {
                        continue;
                    }

                    object order = ssOrderList_Sheet1.Cells[i, 1].Value;

                    sql = new StringBuilder();
                    sql.Append("UPDATE ENVIRONMENT_ORDER                        ").Append("\n");
                    sql.Append("   SET USEYN    = 'N'                           ").Append("\n");
                    sql.Append("     , UPDDATE  = TO_CHAR(SYSDATE, 'YYYYMMDD')  ").Append("\n");
                    sql.Append("     , UPDTIME  = TO_CHAR(SYSDATE, 'HH24MMSS')  ").Append("\n");
                    sql.Append("     , UPDUSER  = '" + clsType.User.Sabun + "'  ").Append("\n");
                    sql.Append(" WHERE ORDERNO  = '" + order + "'               ").Append("\n");

                    SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);

                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, sql.ToString(), clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 화면닫기
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
        /// 초기설정
        /// </summary>
        private void Init()
        {
            ssMain_Sheet1.Rows.Clear();
            ssOrderList_Sheet1.Rows.Clear();

            GetGradeData(cboGrade1);
            ssMain.AllowDragFill = true;
            ssMain.RangeDragFillMode = DragFillMode.Copy;
        }

        /// <summary>
        /// 단계별 데이터 조회
        /// </summary>
        /// <param name="comboBox"></param>
        private void GetGradeData(ComboBox comboBox)
        {
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;

            string grade = comboBox.Name.Substring(comboBox.Name.Length - 1);
            try
            {
                string gradeColumn = string.Concat("A.GRADE", grade);

                sql = new StringBuilder();
                sql.Append("SELECT " + gradeColumn + ", B.CODENAME              ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_EXAM_MASTER A                    ").Append("\n");
                sql.Append("  INNER JOIN BAS_GRADE_ENVIRONMENT B                ").Append("\n");
                sql.Append("          ON " + gradeColumn + " = B.CODE           ").Append("\n");
                sql.Append("         AND B.GRADE = " + grade + "                ").Append("\n");
                sql.Append(" WHERE A.USEYN = 'Y'                                ").Append("\n");
                sql.Append("   AND B.USEYN = 'Y'                                ").Append("\n");

                if (grade.Equals("2"))
                {
                    sql.Append("   AND A.GRADE1 = '" + cboGrade1.SelectedValue + "' ").Append("\n");
                }

                sql.Append("GROUP BY " + gradeColumn + ", B.CODENAME, B.CODE    ").Append("\n");
                sql.Append("ORDER BY B.CODE                                     ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                DataRow row = dt.NewRow();
                //row[string.Concat("GRADE", grade)] = DBNull.Value;                
                row[string.Concat("GRADE", grade)] = 0;
                row["CODENAME"] = "전체";

                dt.Rows.InsertAt(row, 0);

                comboBox.DisplayMember = "CODENAME";
                comboBox.ValueMember = string.Concat("GRADE", grade);
                comboBox.DataSource = dt;

                if (grade.Equals("1"))
                {
                    DataTable cloneDt = dt.Clone();

                    foreach (DataRow r in dt.Rows)
                    {
                        DataRow newRow = cloneDt.NewRow();
                        newRow.ItemArray = r.ItemArray;
                        cloneDt.Rows.Add(newRow);
                    }

                    cboSearch.DisplayMember = "CODENAME";
                    cboSearch.ValueMember = string.Concat("GRADE", grade);
                    cboSearch.DataSource = cloneDt;
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
        /// 감염 검사대상 데이터 스프레드 바인딩
        /// </summary>
        private void GetMasterDataBind()
        {
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                sql = new StringBuilder();
                sql.Append("SELECT A.CODE, B.BUCODE                                                         ").Append("\n");
                sql.Append("     , B.CODENAME AS GRADENAME1                                                 ").Append("\n");
                sql.Append("     , C.CODENAME AS GRADENAME2                                                 ").Append("\n");
                sql.Append("     , D.CODENAME AS GRADENAME3                                                 ").Append("\n");
                sql.Append("     , E.CODENAME AS GRADENAME4                                                 ").Append("\n");
                sql.Append("     , (                                                                        ").Append("\n");
                sql.Append("            SELECT LISTAGG(BB.EXAMNAME, ',') WITHIN GROUP(ORDER BY BB.EXAMNAME) ").Append("\n");
                sql.Append("              FROM ENVIRONMENT_EXAM_DETAIL AA                                   ").Append("\n");
                sql.Append("              INNER JOIN KOSMOS_OCS.EXAM_MASTER BB                              ").Append("\n");
                sql.Append("                      ON AA.EXAMCODE = BB.MASTERCODE                            ").Append("\n");
                sql.Append("                     AND AA.USEYN = 'Y'                                         ").Append("\n");
                sql.Append("             WHERE AA.EXAMMASTERCODE = A.CODE                                   ").Append("\n");
                sql.Append("           GROUP BY AA.EXAMMASTERCODE                                           ").Append("\n");
                sql.Append("       ) AS EXAMNAME                                                            ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_EXAM_MASTER A                                                ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT B                                       ").Append("\n");
                sql.Append("               ON A.GRADE1   = B.CODE                                           ").Append("\n");
                sql.Append("              AND B.GRADE    = 1                                                ").Append("\n");
                sql.Append("              AND B.USEYN    = 'Y'                                              ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT C                                       ").Append("\n");
                sql.Append("               ON A.GRADE2   = C.CODE                                           ").Append("\n");
                sql.Append("              AND C.GRADE    = 2                                                ").Append("\n");
                sql.Append("              AND C.USEYN    = 'Y'                                              ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT D                                       ").Append("\n");
                sql.Append("               ON A.GRADE3   = D.CODE                                           ").Append("\n");
                sql.Append("              AND D.GRADE    = 3                                                ").Append("\n");
                sql.Append("              AND D.USEYN    = 'Y'                                              ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT E                                       ").Append("\n");
                sql.Append("               ON A.GRADE4   = E.CODE                                           ").Append("\n");
                sql.Append("              AND E.GRADE    = 4                                                ").Append("\n");
                sql.Append("              AND E.USEYN    = 'Y'                                              ").Append("\n");
                sql.Append(" WHERE 1 = 1                                                                    ").Append("\n");

                //if (cboGrade1.SelectedValue != DBNull.Value)
                if (cboGrade1.SelectedIndex != 0)
                {
                    sql.Append("   AND A.GRADE1 = '" + cboGrade1.SelectedValue + "' ").Append("\n");
                }

                //if (cboGrade2.SelectedValue != DBNull.Value && cboGrade2.SelectedValue != null)
                if (cboGrade2.SelectedIndex != -1 && cboGrade2.SelectedIndex != 0)
                {
                    sql.Append("   AND A.GRADE2 = '" + cboGrade2.SelectedValue + "' ").Append("\n");
                }

                sql.Append("   AND A.USEYN = 'Y'                                ").Append("\n");
                sql.Append("ORDER BY C.CODENAME, D.CODENAME                     ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssMain_Sheet1.Rows.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssMain_Sheet1.RowCount = ssMain_Sheet1.RowCount + 3;
                    int row = ssMain_Sheet1.RowCount - 3;

                    ssMain_Sheet1.Cells[row, 0].Value = false;
                    ssMain_Sheet1.Cells[row, 0].Locked = dt.Rows[i]["EXAMNAME"].Equals(DBNull.Value);
                    ssMain_Sheet1.Cells[row, 1].Value = string.Concat(dt.Rows[i]["GRADENAME1"], "-", dt.Rows[i]["GRADENAME2"], "-", dt.Rows[i]["GRADENAME3"]);
                    ssMain_Sheet1.Cells[row, 1].ColumnSpan = 4;
                    //ssMain_Sheet1.Cells[row, 2].Value = dt.Rows[i]["GRADENAME2"];
                    //ssMain_Sheet1.Cells[row, 3].Value = dt.Rows[i]["GRADENAME3"];
                    //ssMain_Sheet1.Cells[row, 4].Value = dt.Rows[i]["GRADENAME4"];
                    ssMain_Sheet1.Cells[row, 5].Value = dt.Rows[i]["BUCODE"];
                    ssMain_Sheet1.Cells[row, 6].Value = dt.Rows[i]["CODE"];

                    ssMain_Sheet1.Cells[row, 0].RowSpan = 3;
                    ssMain_Sheet1.Cells[row, 0].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssMain_Sheet1.Cells[row, 0].VerticalAlignment = CellVerticalAlignment.Center;

                    ssMain_Sheet1.Cells[row + 1, 1].Value = dt.Rows[i]["EXAMNAME"];
                    ssMain_Sheet1.Cells[row + 1, 1].ColumnSpan = 4;

                    ssMain_Sheet1.Cells[row + 2, 1].Value = string.Empty;
                    ssMain_Sheet1.Cells[row + 2, 1].ColumnSpan = 4;

                    if(i % 2 != 0)
                    {
                        ssMain_Sheet1.Rows[row].BackColor = Color.FromArgb(210, 234, 240);
                        ssMain_Sheet1.Rows[row + 1].BackColor = Color.FromArgb(210, 234, 240);
                        ssMain_Sheet1.Rows[row + 2].BackColor = Color.FromArgb(210, 234, 240);
                    }
                    
                    if (ssMain_Sheet1.Cells[row, 0].Locked)
                    {
                        ssMain_Sheet1.Rows[row].BackColor = Color.FromArgb(210, 210, 210);
                        ssMain_Sheet1.Rows[row + 1].BackColor = Color.FromArgb(210, 210, 210);
                        ssMain_Sheet1.Rows[row + 2].BackColor = Color.FromArgb(210, 210, 210);
                    }

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
        /// 오더스프레드 데이터 바인딩
        /// </summary>
        private void SetOrderListDataBind()
        {
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                ssOrderList_Sheet1.Rows.Clear();
                Cursor.Current = Cursors.WaitCursor;

                string startDate = dtpStartDate.Value.ToString("yyyyMMdd");
                string endDate = dtpEndDate.Value.ToString("yyyyMMdd");

                sql = new StringBuilder();
                sql.Append("SELECT A.ORDERNO,   A.ENVIRONMENTCODE,  A.BUCODE,       A.REMARK                ").Append("\n");
                sql.Append("     , A.ORDERDATE, A.ORDERTIME,        A.ORDERUSER                             ").Append("\n");
                sql.Append("     , H.USERNAME AS ORDERUSERNAME                                              ").Append("\n");
                sql.Append("     , A.BARCODE,   A.BARCODEDATE,      A.BARCODETIME,  A.BARCODEUSER           ").Append("\n");
                sql.Append("     , I.USERNAME AS BARCODEUSERNAME                                            ").Append("\n");
                sql.Append("     , G.NAME AS DEPTNAME,              H.USERNAME                              ").Append("\n");
                sql.Append("     , C.CODENAME AS GRADENAME1                                                 ").Append("\n");
                sql.Append("     , D.CODENAME AS GRADENAME2                                                 ").Append("\n");
                sql.Append("     , E.CODENAME AS GRADENAME3                                                 ").Append("\n");
                sql.Append("     , F.CODENAME AS GRADENAME4                                                 ").Append("\n");
                sql.Append("     , (                                                                        ").Append("\n");
                sql.Append("            SELECT LISTAGG(BB.EXAMNAME, ',') WITHIN GROUP(ORDER BY BB.EXAMNAME) ").Append("\n");
                sql.Append("              FROM ENVIRONMENT_EXAM_DETAIL AA                                   ").Append("\n");
                sql.Append("              INNER JOIN KOSMOS_OCS.EXAM_MASTER BB                              ").Append("\n");
                sql.Append("                      ON AA.EXAMCODE = BB.MASTERCODE                            ").Append("\n");
                sql.Append("                     AND AA.USEYN = 'Y'                                         ").Append("\n");
                sql.Append("             WHERE AA.EXAMMASTERCODE = B.CODE                                   ").Append("\n");
                sql.Append("           GROUP BY AA.EXAMMASTERCODE                                           ").Append("\n");
                sql.Append("       ) AS EXAMNAME                                                            ").Append("\n");
                sql.Append("     , DECODE(K.STATUS, '01', '검사중'                                           ").Append("\n");
                sql.Append("                      , '04', '부분완료'                                         ").Append("\n");
                sql.Append("                      , '05', '검사완료'                                         ").Append("\n");
                sql.Append("                      , '06', '취소'                                             ").Append("\n");
                sql.Append("                      , '미접수') AS STATUS                                      ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_ORDER A                                                      ").Append("\n");
                sql.Append("  LEFT OUTER JOIN ENVIRONMENT_EXAM_MASTER B                                     ").Append("\n");
                sql.Append("               ON A.ENVIRONMENTCODE = B.CODE                                    ").Append("\n");
                //sql.Append("              AND B.USEYN           = 'Y'                                       ").Append("\n");
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
                sql.Append("  LEFT OUTER JOIN BAS_BUSE G                                                    ").Append("\n");
                sql.Append("               ON A.BUCODE = G.BUCODE                                           ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_USER H                                                    ").Append("\n");
                sql.Append("               ON A.ORDERUSER   = TRIM(H.SABUN)                                 ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_USER I                                                    ").Append("\n");
                sql.Append("               ON A.BARCODEUSER = TRIM(I.SABUN)                                 ").Append("\n");
                sql.Append("  LEFT OUTER JOIN KOSMOS_OCS.EXAM_SPECMST K                                     ").Append("\n");
                sql.Append("               ON A.SPECNO = K.SPECNO                                           ").Append("\n");
                sql.Append(" WHERE A.ORDERDATE BETWEEN '" + startDate + "'                                  ").Append("\n");
                sql.Append("                       AND '" + endDate + "'                                    ").Append("\n");
                sql.Append("   AND A.USEYN = 'Y'                                                            ").Append("\n");

                if (cboSearch.SelectedIndex != -1 && cboSearch.SelectedIndex != 0)
                {
                    sql.Append("   AND B.GRADE1 LIKE '%" + cboSearch.SelectedValue + "%'       ").Append("\n");
                }

                sql.Append("ORDER BY ORDERNO                                                                ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssOrderList_Sheet1.RowCount = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++) 
                {
                    DataRow row = dt.Rows[i];

                    ssOrderList_Sheet1.Cells[i, 0].Value = false;
                    ssOrderList_Sheet1.Cells[i, 1].Value = row["ORDERNO"];
                    ssOrderList_Sheet1.Cells[i, 2].Value = ComFunc.FormatStrToDate(row["ORDERDATE"].ToString(), "D");
                    ssOrderList_Sheet1.Cells[i, 3].Value = ComFunc.FormatStrToDate(row["ORDERTIME"].ToString(), "T");
                    ssOrderList_Sheet1.Cells[i, 4].Value = row["ORDERUSERNAME"];
                    ssOrderList_Sheet1.Cells[i, 5].Value = row["DEPTNAME"];
                    ssOrderList_Sheet1.Cells[i, 6].Value = row["REMARK"];
                    ssOrderList_Sheet1.Cells[i, 7].Value = string.Concat(row["GRADENAME1"], "-", row["GRADENAME2"], "-", row["GRADENAME3"], "-", row["GRADENAME4"]);
                    ssOrderList_Sheet1.Cells[i, 8].Value = row["EXAMNAME"];
                    ssOrderList_Sheet1.Cells[i, 9].Value = ComFunc.FormatStrToDate(row["BARCODEDATE"].ToString(), "D");
                    ssOrderList_Sheet1.Cells[i, 10].Value = ComFunc.FormatStrToDate(row["BARCODETIME"].ToString(), "T");
                    ssOrderList_Sheet1.Cells[i, 11].Value = row["BARCODEUSERNAME"];
                    ssOrderList_Sheet1.Cells[i, 12].Value = row["STATUS"];

                    //if (row["BARCODE"] != DBNull.Value)
                    //{
                    //    ssOrderList_Sheet1.Cells[i, 0].Locked = true;
                    //}

                    //2019-04-17 안정수, 조건 변경 (검체 미접수일경우에는 오더취소 되도록 변경)
                    if(row["STATUS"].ToString() == "미접수")
                    {
                        ssOrderList_Sheet1.Cells[i, 0].Locked = false;
                    }
                    else
                    {
                        ssOrderList_Sheet1.Cells[i, 0].Locked = true; 
                    }
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
        /// 날짜형식 변경
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <param name="isDate"></param>
        /// <returns></returns>
        private string GetDateTimeConvert(object value, string format, bool isDate = true)
        {
            if (value == null || value == DBNull.Value || value.ToString().Length == 0)
            {
                return string.Empty;
            }
            else
            {
                DateTime dtm;
                if (isDate)
                {
                    dtm = DateTime.ParseExact(value.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                else
                {
                    dtm = DateTime.ParseExact(value.ToString(), "HHmmss", CultureInfo.InvariantCulture);
                }
                return dtm.ToString(format);
            }
        }

        #endregion

        void ssOrderList_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader) 
            {
                return;
            }

            if (e.ColumnHeader)
            {
                if (e.Column == 0)
                {
                    bool check = Convert.ToBoolean(ssOrderList_Sheet1.ColumnHeader.Cells[1, 0].Value);

                    ssOrderList.ActiveSheet.ColumnHeader.Cells[1, 0].Value = !check;

                    for (int i = 0; i < ssOrderList.ActiveSheet.Rows.Count; i++)
                    {
                        ssOrderList.ActiveSheet.Cells[i, 0].Value = !check;
                    }
                }
            }
        }
    }
}