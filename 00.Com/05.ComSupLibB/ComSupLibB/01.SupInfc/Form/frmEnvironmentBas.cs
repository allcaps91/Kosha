using ComBase;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;

namespace ComSupLibB.SupInfc
{
    /// <summary>
    /// Class Name      : frmEnvironmentBas
    /// File Name       : 환경미생물 기초코드 등록
    /// Description     :
    /// Author          : 전상원
    /// Create Date     : 2018-09-20
    /// Update History  :
    /// </summary>
    ///
    /// <history>
    /// </history>
    /// <seealso cref= " >> frmEnvironmentBas.cs 폼이름 재정의" />
    public partial class frmEnvironmentBas : Form, MainFormMessage
    {
        #region 프로퍼티

        private clsComSQL comSql = new clsComSQL();
        private clsMethod method = new clsMethod();

        private List<string> gArrCODE = new List<string>();

        private Dictionary<string, object> GradeSpreadChanged;

        private StringBuilder sql;

        #endregion 프로퍼티

        #region 생성자 && 로드이벤트

        public frmEnvironmentBas()
        {
            InitializeComponent();
        }

        public frmEnvironmentBas(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
        }

        /// <summary>
        /// 로드 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEnvironmentBas_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SetInit();
            //SetSpreadClear();
        }

        #endregion 생성자 && 로드이벤트

        #region 메인폼 컨트롤 이벤트

        /// <summary>
        /// 창닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion 메인폼 컨트롤 이벤트

        #region 메인폼 메소드

        private void SetInit()
        {
            InputMap im = ssGrade1.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.Tab, Keys.None), SpreadActions.MoveToNextColumn);

            SetSpreadClear();
            SpreadComboBoxDataBind();

            SetGradeDataBind("1");
            SetGradeDataBind("2");
            SetGradeDataBind("3");
            SetGradeDataBind("4");

            SetInitTab2();
        }

        /// <summary>
        /// 2,3단계 스프레드에 1단계 코드 표시
        /// </summary>
        private void SpreadComboBoxDataBind()
        {
            string SqlErr = string.Empty;
            DataTable dt = null;
            sql = new StringBuilder();
            sql.Append("SELECT CODE, CODENAME       ").Append("\n");
            sql.Append("  FROM BAS_GRADE_ENVIRONMENT").Append("\n");
            sql.Append(" WHERE GRADE = 1            ").Append("\n");
            sql.Append("   AND USEYN = 'Y'          ").Append("\n");
            sql.Append("ORDER BY CODE               ").Append("\n");

            SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

            if (!string.IsNullOrWhiteSpace(SqlErr))
            {
                clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return;
            }

            List<string> itemsDisplay = new List<string>();
            List<string> itemData = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                itemsDisplay.Add(dt.Rows[i]["CODENAME"].ToString());
                itemData.Add(dt.Rows[i]["CODE"].ToString());
            }

            itemData.Insert(0, "0");
            itemsDisplay.Insert(0, "전체");

            ComboBoxCellType comboBoxCellType = new ComboBoxCellType();
            comboBoxCellType.ItemData = itemData.ToArray();
            comboBoxCellType.Items = itemsDisplay.ToArray();
            comboBoxCellType.EditorValue = EditorValue.ItemData;
            ssGrade2_Sheet1.Columns[2].CellType = comboBoxCellType;
            ssGrade3_Sheet1.Columns[2].CellType = comboBoxCellType;
        }

        private void SetInitTab2()
        {
            ssExamMaster_Sheet1.Rows.Clear();

            DataTable dt = GetGradeData();
            List<string> itemsDisplay = new List<string>();
            List<string> itemData = new List<string>();

            int prevGrade = 0;
            ComboBoxCellType comboBoxCellType = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int grade = Convert.ToInt32(dt.Rows[i]["GRADE"].ToString());

                if (i > 0)
                {
                    if (prevGrade != grade)
                    {
                        itemData.Insert(0, "");
                        itemsDisplay.Insert(0, "");
                        comboBoxCellType = (ComboBoxCellType)ssMaster_Sheet1.Columns.Get(prevGrade).CellType;
                        comboBoxCellType.ItemData = itemData.ToArray();
                        comboBoxCellType.Items = itemsDisplay.ToArray();
                        comboBoxCellType.EditorValue = EditorValue.ItemData;

                        itemsDisplay.Clear();
                        itemData.Clear();
                    }
                }

                itemsDisplay.Add(dt.Rows[i]["CODENAME"].ToString());
                itemData.Add(dt.Rows[i]["CODE"].ToString());

                prevGrade = grade;
            }

            if (itemsDisplay.Count > 0)
            {
                itemData.Insert(0, "");
                itemsDisplay.Insert(0, "");
                comboBoxCellType = (ComboBoxCellType)ssMaster_Sheet1.Columns.Get(prevGrade).CellType;
                comboBoxCellType.ItemData = itemData.ToArray();
                comboBoxCellType.Items = itemsDisplay.ToArray();
                comboBoxCellType.EditorValue = EditorValue.ItemData;
            }

            SetMasterDataBind();
            SetExamCodeDataBind();
        }

        private void SetExamCodeDataBind(string masterCode = "")
        {
            string SqlErr = string.Empty;
            DataTable dt = null;
            ssExamList_Sheet1.Rows.Clear();

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                sql = new StringBuilder();

                if(string.IsNullOrWhiteSpace(masterCode))
                {
                    sql.Append("SELECT MASTERCODE, EXAMNAME             ").Append("\n");
                    sql.Append("  FROM KOSMOS_OCS.EXAM_MASTER           ").Append("\n");
                    sql.Append(" WHERE MASTERCODE IN ('MI32', 'MI35')   ").Append("\n");
                    sql.Append("ORDER BY MASTERCODE                     ").Append("\n");
                }
                else
                {
                    sql.Append("SELECT A.MASTERCODE, A.EXAMNAME                     ").Append("\n");
                    sql.Append("  FROM KOSMOS_OCS.EXAM_MASTER A                     ").Append("\n");
                    sql.Append(" WHERE MASTERCODE IN ('MI32', 'MI35')               ").Append("\n");
                    sql.Append("   AND NOT EXISTS (                                 ").Append("\n");
                    sql.Append("        SELECT 1                                    ").Append("\n");
                    sql.Append("          FROM ENVIRONMENT_EXAM_DETAIL              ").Append("\n");
                    sql.Append("         WHERE USEYN = 'Y'                          ").Append("\n");
                    sql.Append("           AND EXAMMASTERCODE = '" + masterCode + "'").Append("\n");
                    sql.Append("           AND EXAMCODE = A.MASTERCODE              ").Append("\n");
                    sql.Append("   )                                                ").Append("\n");
                }

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssExamList_Sheet1.RowCount = dt.Rows.Count;
                for(int i=0; i<dt.Rows.Count; i++)
                {
                    ssExamList_Sheet1.Cells[i, 0].Value = false;
                    ssExamList_Sheet1.Cells[i, 1].Value = dt.Rows[i]["MASTERCODE"];
                    ssExamList_Sheet1.Cells[i, 2].Value = dt.Rows[i]["EXAMNAME"].ToString().Trim();
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
        /// 스프레드 초기화
        /// </summary>
        private void SetSpreadClear()
        {
            //  Tab 1 - 단계별 기초코드 관리
            ssGrade1_Sheet1.Rows.Clear();
            ssGrade2_Sheet1.Rows.Clear();
            ssGrade3_Sheet1.Rows.Clear();
            ssGrade4_Sheet1.Rows.Clear();
        }

        #endregion 메인폼 메소드

        #region Tab 1 - 단계별 코드관리

        #region 컨트롤 이벤트

        /// <summary>
        /// 단계별 스프레드 로우 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddGrade_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string grade = btn.Name.Substring(btn.Name.Length - 1);
            SheetView sheetView = GetGradeSheetView(grade);

            string SqlErr = string.Empty;
            DataTable dt = null;


            //2019-01-25 Code번호 추가 전상원
            string strCode = string.Empty;

            try
            {
                sql = new StringBuilder();

                sql.Append("SELECT MAX(CODE) + 1 AS NEWCODE ").Append("\n");
                sql.Append("  FROM BAS_GRADE_ENVIRONMENT    ").Append("\n");
                sql.Append(" WHERE GRADE = '" + grade + "'  ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                sheetView.Rows.Count++;

                sheetView.Cells[sheetView.Rows.Count - 1, 0].Text = dt.Rows[0]["NEWCODE"].ToString().Trim();

                dt.Dispose();
                dt = null;
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

            sheetView.FpSpread.Focus();
            sheetView.Rows[sheetView.Rows.Count - 1].Locked = false;
            sheetView.Cells[sheetView.Rows.Count - 1, 0].Locked = true;
            sheetView.Cells[sheetView.Rows.Count - 1, sheetView.Columns.Count - 1].Value = "N";
            sheetView.SetActiveCell(sheetView.Rows.Count - 1, 1);
            sheetView.FpSpread.ShowRow(0, sheetView.Rows.Count - 1, VerticalPosition.Nearest);
            sheetView.FpSpread.ShowColumn(0, 1, HorizontalPosition.Nearest);

            GradeSpreadChangedCheck(sheetView.FpSpread);
        }

        /// <summary>
        /// 단계별 데이터 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveGrade_Click(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            string grade = btn.Name.Substring(btn.Name.Length - 1);
            string spName = string.Concat("ssGrade", grade);

            if (GradeSpreadChanged == null || !GradeSpreadChanged.ContainsKey(spName))
            {
                ComFunc.MsgBox("수정된 내용이 없습니다.");
                return;
            }

            if(grade.Equals("1"))
            {
                int col = ssGrade1_Sheet1.Columns.Count - 1;
                for(int i=0; i<ssGrade1_Sheet1.RowCount; i++)
                {
                    if(!string.IsNullOrWhiteSpace(ssGrade1_Sheet1.Cells[i, col].Text))
                    {
                        if(string.IsNullOrWhiteSpace(ssGrade1_Sheet1.Cells[i, 2].Text) || string.IsNullOrWhiteSpace(ssGrade1_Sheet1.Cells[i, 3].Text))
                        {
                            ComFunc.MsgBox("부서/담당자는 필수 입력항목 입니다.");
                            return;
                        }
                    }
                }
            }

            SheetView sheetView = GetGradeSheetView(grade);

            int intRowAffected = 0;
            string SqlErr = string.Empty;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);
                for (int i = 0; i < sheetView.Rows.Count; i++)
                {
                    object crud = sheetView.Cells[i, sheetView.Columns.Count - 1].Value;
                    if (crud == null || string.IsNullOrWhiteSpace(crud.ToString()))
                    {
                        continue;
                    }

                    string buCode = string.Empty;
                    string useYn = string.Empty;
                    string idNumber = string.Empty;
                    string parentCode = string.Empty;
                    string codeName = sheetView.Cells[i, 1].Text;
                    string code = sheetView.Cells[i, 0].Text;

                    if (string.IsNullOrWhiteSpace(codeName))
                    {
                        continue;
                    }

                    if (grade.Equals("1"))
                    {
                        buCode = sheetView.Cells[i, 7].Text;
                        idNumber = sheetView.Cells[i, 6].Text;
                        useYn = Convert.ToBoolean(sheetView.Cells[i, 5].Value) ? "N" : "Y";

                        if (string.IsNullOrWhiteSpace(buCode))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if(sheetView.Cells[i, 2].Value != null)
                        {
                            parentCode = sheetView.Cells[i, 2].Value.ToString();
                        }
                        
                        useYn = Convert.ToBoolean(sheetView.Cells[i, 3].Value) ? "N" : "Y";
                    }

                    sql = new StringBuilder();
                    sql.Append("MERGE INTO BAS_GRADE_ENVIRONMENT A USING(                   ").Append("\n");
                    sql.Append("    SELECT '" + grade + "'              AS GRADE            ").Append("\n");
                    sql.Append("         , " + code + "                 AS CODE             ").Append("\n");
                    sql.Append("         , '" + codeName + "'           AS CODENAME         ").Append("\n");
                    sql.Append("         , '" + buCode + "'             AS BUCODE           ").Append("\n");
                    sql.Append("         , '" + idNumber + "'           AS IDNUMBER         ").Append("\n");
                    sql.Append("         , '" + parentCode + "'         AS PARENTCODE       ").Append("\n");
                    sql.Append("         , '" + useYn + "'              AS USEYN            ").Append("\n");
                    sql.Append("         , TO_CHAR(SYSDATE, 'YYYYMMDD') AS CURDATE          ").Append("\n");
                    sql.Append("         , TO_CHAR(SYSDATE, 'HH24MMSS') AS CURTIME          ").Append("\n");
                    sql.Append("         , '" + clsType.User.Sabun + "' AS USERID           ").Append("\n");
                    sql.Append("     FROM DUAL                                              ").Append("\n");
                    sql.Append(") B ON (                                                    ").Append("\n");
                    sql.Append("        A.GRADE = B.GRADE                                   ").Append("\n");
                    sql.Append("    AND A.CODE  = B.CODE                                    ").Append("\n");
                    sql.Append(")                                                           ").Append("\n");
                    sql.Append("WHEN NOT MATCHED THEN                                       ").Append("\n");
                    sql.Append("    INSERT(                                                 ").Append("\n");
                    sql.Append("        A.GRADE,    A.CODE,     A.CODENAME,     A.BUCODE    ").Append("\n");
                    sql.Append("      , A.USEYN,    A.INPDATE,  A.INPTIME,      A.INPUSER   ").Append("\n");
                    sql.Append("      , A.IDNUMBER, A.PARENTCODE                            ").Append("\n");
                    sql.Append("    ) VALUES (                                              ").Append("\n");
                    sql.Append("        B.GRADE,    B.CODE,     B.CODENAME,     B.BUCODE    ").Append("\n");
                    sql.Append("      , B.USEYN,    B.CURDATE,  B.CURTIME,      B.USERID    ").Append("\n");
                    sql.Append("      , B.IDNUMBER, B.PARENTCODE                            ").Append("\n");
                    sql.Append("    )                                                       ").Append("\n");
                    sql.Append("WHEN MATCHED THEN                                           ").Append("\n");
                    sql.Append("    UPDATE                                                  ").Append("\n"); 
                    sql.Append("       SET A.CODENAME   = B.CODENAME                        ").Append("\n");
                    sql.Append("         , A.BUCODE     = B.BUCODE                          ").Append("\n");
                    sql.Append("         , A.USEYN      = B.USEYN                           ").Append("\n");
                    sql.Append("         , A.UPDDATE    = B.CURDATE                         ").Append("\n");
                    sql.Append("         , A.UPDTIME    = B.CURTIME                         ").Append("\n");
                    sql.Append("         , A.UPDUSER    = B.USERID                          ").Append("\n");
                    sql.Append("         , A.IDNUMBER   = B.IDNUMBER                        ").Append("\n");
                    sql.Append("         , A.PARENTCODE = B.PARENTCODE                      ").Append("\n");

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

                SpreadComboBoxDataBind();
                GradeSpreadChanged.Remove(spName);
                SetGradeDataBind(grade);
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
        /// 단계별 행 삭제
        /// 저장 후 삭제 안됨
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteGrade_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string grade = btn.Name.Substring(btn.Name.Length - 1);
            SheetView sheetView = GetGradeSheetView(grade);

            //if (sheetView.Cells[sheetView.ActiveRowIndex, 0].Value != null &&
            //    !string.IsNullOrWhiteSpace(sheetView.Cells[sheetView.ActiveRowIndex, 0].Value.ToString()))
            //{
            //    return;
            //}

            //if (!string.IsNullOrWhiteSpace(sheetView.Cells[sheetView.ActiveRowIndex, 0].Value.ToString()))
            //{
            //    return;
            //}

            sheetView.Rows.Remove(sheetView.ActiveRowIndex, 1);
        }

        /// <summary>
        /// 스프레드 수정 엔터키 팝업 호출
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssGrade1_KeyUp(object sender, KeyEventArgs e)
        {
            if (ssGrade1_Sheet1.ActiveColumnIndex != 2 || ssGrade1_Sheet1.ActiveRowIndex < 0)
            {
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                BusePopupOpen();
            }
        }

        /// <summary>
        /// 스프레드 부서조회 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssGrade1_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if(e.Column != 4)
            {
                return;
            }
            BusePopupOpen();
        }

        /// <summary>
        /// 부서찾기 팝업오픈
        /// </summary>
        private void BusePopupOpen()
        {
            string name = ssGrade1_Sheet1.Cells[ssGrade1_Sheet1.ActiveRowIndex, 2].Text;
            frmBusePopup popup = new frmBusePopup(name);
            popup.ShowDialog();

            if (!string.IsNullOrWhiteSpace(popup.BUCODE))
            {
                ssGrade1_Sheet1.Cells[ssGrade1_Sheet1.ActiveRowIndex, 2].Text = popup.BUNAME;
                ssGrade1_Sheet1.Cells[ssGrade1_Sheet1.ActiveRowIndex, 3].Text = popup.USERNAME;
                ssGrade1_Sheet1.Cells[ssGrade1_Sheet1.ActiveRowIndex, 6].Text = popup.USERID;
                ssGrade1_Sheet1.Cells[ssGrade1_Sheet1.ActiveRowIndex, 7].Text = popup.BUCODE;
                ssGrade1_Sheet1.Cells[ssGrade1_Sheet1.ActiveRowIndex, 8].Text = "U";

                EditorNotifyEventArgs eventArgs = new EditorNotifyEventArgs(new SpreadView(ssGrade1), ssGrade1, ssGrade1_Sheet1.ActiveRowIndex, 3);
                ssGrade_EditChange(ssGrade1, eventArgs);
            }
        }

        /// <summary>
        /// Cell 수정여부체크
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssGrade_EditChange(object sender, EditorNotifyEventArgs e)
        {
            FpSpread spread = sender as FpSpread;
            if (spread.ActiveSheet.Cells[e.Row, spread.ActiveSheet.Columns.Count - 1].Text.Equals("N"))
            {
                return;
            }

            spread.ActiveSheet.Cells[e.Row, spread.ActiveSheet.Columns.Count - 1].Value = "U";
            GradeSpreadChangedCheck(spread);
        }

        /// <summary>
        /// 체크박스 수정여부 체크
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssGrade_Change(object sender, ChangeEventArgs e)
        {
            EditorNotifyEventArgs eventArgs = new EditorNotifyEventArgs(e.View, null, e.Row, e.Column);
            ssGrade_EditChange(sender, eventArgs);
        }

        #endregion

        #region 메소드

        /// <summary>
        /// 단계별 데이티 저장
        /// </summary>
        /// <param name="grade"></param>
        private void SetGradeDataBind(string grade)
        {
            string SqlErr = string.Empty;
            DataTable dt = null;
            DateTime dtm = DateTime.MinValue;
            int cnt = 0; 

            try
            {
                sql = new StringBuilder();
                sql.Append("SELECT A.CODE, A.CODENAME, A.BUCODE, A.USEYN    ").Append("\n");
                sql.Append("     , B.NAME AS DEPTNAME                       ").Append("\n");
                sql.Append("     , C.USERNAME                               ").Append("\n");
                sql.Append("     , A.IDNUMBER                               ").Append("\n");
                sql.Append("     , A.PARENTCODE                             ").Append("\n");
                sql.Append("     , D.CODENAME AS PARENTCODENAME             ").Append("\n");
                sql.Append("  FROM BAS_GRADE_ENVIRONMENT A                  ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_BUSE B                    ").Append("\n");
                sql.Append("               ON A.BUCODE = B.BUCODE           ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_USER C                    ").Append("\n");
                sql.Append("               ON A.IDNUMBER = C.IDNUMBER       ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT D       ").Append("\n");
                sql.Append("               ON A.PARENTCODE = D.CODE         ").Append("\n");
                sql.Append("              AND D.GRADE = '1'                 ").Append("\n");
                sql.Append(" WHERE A.GRADE = '" + grade + "'                ").Append("\n");
                //2019-06-25 안정수 추가
                sql.Append("   AND A.USEYN = 'Y'                            ").Append("\n");
                sql.Append("ORDER BY CODE                                   ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SheetView sheetView = GetGradeSheetView(grade);
                sheetView.Rows.Clear();
                sheetView.Rows.Count = cnt;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sheetView.Rows.Count++;
                    sheetView.Cells[i, 0].Value = dt.Rows[i]["CODE"];
                    sheetView.Cells[i, 1].Value = dt.Rows[i]["CODENAME"]; sheetView.Cells[i, 1].Locked = false;

                    if (grade.Equals("1"))
                    {
                        sheetView.Cells[i, 2].Value = dt.Rows[i]["DEPTNAME"];
                        sheetView.Cells[i, 3].Value = dt.Rows[i]["USERNAME"];
                        sheetView.Cells[i, 5].Value = dt.Rows[i]["USEYN"].Equals("N");
                        sheetView.Cells[i, 6].Value = dt.Rows[i]["IDNUMBER"];
                        sheetView.Cells[i, 7].Value = dt.Rows[i]["BUCODE"];
                    }
                    else
                    {
                        sheetView.Cells[i, 2].Value = dt.Rows[i]["PARENTCODE"];
                        sheetView.Cells[i, 3].Value = dt.Rows[i]["USEYN"].Equals("N");
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
        /// 단계별 스프레드 수정여부체크
        /// </summary>
        /// <param name="spread"></param>
        private void GradeSpreadChangedCheck(FpSpread spread)
        {
            if (GradeSpreadChanged == null)
            {
                GradeSpreadChanged = new Dictionary<string, object>();
            }

            if (!GradeSpreadChanged.ContainsKey(spread.Name))
            {
                GradeSpreadChanged.Add(spread.Name, true);
            }
        }

        /// <summary>
        /// 그리드뷰 선택
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        private SheetView GetGradeSheetView(string grade)
        {
            SheetView sheetView = null;
            switch (grade)
            {
                case "1": sheetView = ssGrade1_Sheet1; break;
                case "2": sheetView = ssGrade2_Sheet1; break;
                case "3": sheetView = ssGrade3_Sheet1; break;
                case "4": sheetView = ssGrade4_Sheet1; break;
            }

            return sheetView;
        }

        #endregion

        #endregion

        #region Tab 2 - 검사코드 관리

        #region 컨트롤 이벤트

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTab2_Click(object sender, EventArgs e)
        {
            int intRowAffected = 0;
            string SqlErr = string.Empty;

            try
            {                
                //if (GradeSpreadChanged == null || !GradeSpreadChanged.ContainsKey(ssMaster.Name))                    
                //{
                //    ComFunc.MsgBox("수정된 내용이 없습니다.");
                //    return;
                //}

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < ssMaster_Sheet1.RowCount; i++)
                {
                    string code = ssMaster_Sheet1.Cells[i, 0].Text;
                    object grade1 = ssMaster_Sheet1.Cells[i, 1].Value;
                    object grade2 = ssMaster_Sheet1.Cells[i, 2].Value;
                    object grade3 = ssMaster_Sheet1.Cells[i, 3].Value;
                    object grade4 = ssMaster_Sheet1.Cells[i, 4].Value;
                    string useYn = Convert.ToBoolean(ssMaster_Sheet1.Cells[i, 5].Value) ? "N" : "Y";
                    object crud = ssMaster_Sheet1.Cells[i, 6].Value;
                    if (grade1 == null || string.IsNullOrWhiteSpace(grade1.ToString()) || crud == null)
                    {
                        continue;
                    }
                    sql = new StringBuilder();
                    sql.Append("MERGE INTO ENVIRONMENT_EXAM_MASTER A USING(                     ").Append("\n");
                    sql.Append("    SELECT '" + code + "'               AS CODE                 ").Append("\n");
                    sql.Append("         , '" + grade1 + "'             AS GRADE1               ").Append("\n");
                    sql.Append("         , '" + grade2 + "'             AS GRADE2               ").Append("\n");
                    sql.Append("         , '" + grade3 + "'             AS GRADE3               ").Append("\n");
                    sql.Append("         , '" + grade4 + "'             AS GRADE4               ").Append("\n");
                    sql.Append("         , '" + useYn + "'              AS USEYN                ").Append("\n");
                    sql.Append("         , TO_CHAR(SYSDATE, 'YYYYMMDD') AS CURDATE              ").Append("\n");
                    sql.Append("         , TO_CHAR(SYSDATE, 'HH24MMSS') AS CURTIME              ").Append("\n");
                    sql.Append("         , '" + clsType.User.Sabun + "' AS USERID               ").Append("\n");
                    sql.Append("      FROM DUAL                                                 ").Append("\n");
                    sql.Append(") B ON (                                                        ").Append("\n");
                    sql.Append("    A.CODE = B.CODE                                             ").Append("\n");
                    sql.Append(")                                                               ").Append("\n");
                    sql.Append("WHEN NOT MATCHED THEN                                           ").Append("\n");
                    sql.Append("    INSERT(                                                     ").Append("\n");
                    sql.Append("        A.GRADE1,   A.GRADE2,   A.GRADE3,   A.GRADE4            ").Append("\n"); 
                    sql.Append("      , A.USEYN,    A.INSDATE,  A.INSTIME,  A.INSUSER           ").Append("\n");
                    sql.Append("      , A.CODE                                                  ").Append("\n");
                    sql.Append("    ) VALUES (                                                  ").Append("\n");
                    sql.Append("        B.GRADE1,   B.GRADE2,   B.GRADE3,   B.GRADE4            ").Append("\n");
                    sql.Append("      , B.USEYN,    B.CURDATE,  B.CURTIME,  B.USERID            ").Append("\n");
                    sql.Append("      , (SELECT TRIM(TO_CHAR(NVL(MAX(CODE), 0) + 1, '000000'))  ").Append("\n");
                    sql.Append("           FROM ENVIRONMENT_EXAM_MASTER                         ").Append("\n");
                    sql.Append("        )                                                       ").Append("\n");
                    sql.Append("    )                                                           ").Append("\n");
                    sql.Append("WHEN MATCHED THEN                                               ").Append("\n");
                    sql.Append("  UPDATE                                                        ").Append("\n");
                    sql.Append("     SET A.USEYN = B.USEYN                                      ").Append("\n");
                    sql.Append("       , A.UPDDATE = B.CURDATE                                  ").Append("\n");
                    sql.Append("       , A.UPDTIME = B.CURTIME                                  ").Append("\n");
                    sql.Append("       , A.UPDUSER = B.USERID                                   ").Append("\n");

                    SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);
                }

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }


                ComFunc.MsgBox("저장하였습니다.");
                //GradeSpreadChanged.Remove(ssMaster.Name);
                clsDB.setCommitTran(clsDB.DbCon);

                SetMasterDataBind();
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
        /// 새로고침
        /// 기초코드 다시 불러옴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SetInitTab2();
        }

        /// <summary>
        /// 마스터 스프레드 행 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddMaster_Click(object sender, EventArgs e)
        {
            ssMaster_Sheet1.Rows.Count++;
            ssMaster_Sheet1.Cells[ssMaster_Sheet1.ActiveRowIndex, ssMaster_Sheet1.Columns.Count - 1].Value = "N";
            ssMaster_Sheet1.SetActiveCell(ssMaster_Sheet1.Rows.Count - 1, 1);

            ssMaster.ShowRow(0, ssMaster_Sheet1.Rows.Count - 1, VerticalPosition.Nearest);
            ssMaster.ShowColumn(0, 1, HorizontalPosition.Nearest);
            GradeSpreadChangedCheck(ssMaster);
        }

        /// <summary>
        /// 마스터 스프레드 행 삭제 
        /// 저장 후 삭제 못함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteMaster_Click(object sender, EventArgs e)
        {
            if (ssMaster_Sheet1.ActiveRowIndex < 0 || !string.IsNullOrWhiteSpace(ssMaster_Sheet1.Cells[ssMaster_Sheet1.ActiveRowIndex, 0].Text))
            {
                return;
            }

            ssMaster_Sheet1.Rows.Remove(ssMaster_Sheet1.ActiveRowIndex, 1);
        }

        /// <summary>
        /// 스프레드 변경체크
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssMaster_EditChange(object sender, EditorNotifyEventArgs e)
        {
            if (ssMaster_Sheet1.Cells[e.Row, ssMaster_Sheet1.Columns.Count - 1].Value == null)
            {
                ssMaster_Sheet1.Cells[e.Row, ssMaster_Sheet1.Columns.Count - 1].Value = "U";
                GradeSpreadChangedCheck(ssMaster);
            }

            string value = string.Empty;
            if(e.Column == 1 || e.Column == 2)
            {
                value = ssMaster_Sheet1.Cells[e.Row, 1].Value.ToString();

                //  건강검진인 경우 내시경 코드와 동일하게 적용한다.
                if(e.Column == 2 && value.Equals("8"))
                {
                    value = "2";
                }
                SetMasterGradeBind(e.Column + 1, e.Row, e.Column + 1, value);
            }           
        }

        private void SetMasterGradeBind(int col, int row, int grade, string value)
        {
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                sql = new StringBuilder();
                sql.Append("SELECT CODE, CODENAME, 1 AS SORT        ").Append("\n");
                sql.Append("  FROM BAS_GRADE_ENVIRONMENT            ").Append("\n");
                sql.Append(" WHERE GRADE        = '" + grade + "'   ").Append("\n");
                sql.Append("   AND PARENTCODE   = '" + value + "'   ").Append("\n");
                sql.Append("   AND USEYN        = 'Y'               ").Append("\n");
                sql.Append("UNION ALL                               ").Append("\n");
                sql.Append("SELECT CODE, CODENAME, 2 AS SORT        ").Append("\n");
                sql.Append("  FROM BAS_GRADE_ENVIRONMENT            ").Append("\n");
                sql.Append(" WHERE GRADE        = '" + grade + "'   ").Append("\n");
                sql.Append("   AND PARENTCODE   = '0'               ").Append("\n");
                sql.Append("   AND USEYN        = 'Y'               ").Append("\n");
                sql.Append("ORDER BY SORT, CODENAME                 ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                List<string> itemsDisplay = new List<string>();
                List<string> itemData = new List<string>();
                ComboBoxCellType comboBoxCellType = new ComboBoxCellType();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBoxCellType.ItemData = itemData.ToArray();
                    comboBoxCellType.Items = itemsDisplay.ToArray();
                    comboBoxCellType.EditorValue = EditorValue.ItemData;

                    itemsDisplay.Add(dt.Rows[i]["CODENAME"].ToString());
                    itemData.Add(dt.Rows[i]["CODE"].ToString());
                }

                itemData.Insert(0, "");
                itemsDisplay.Insert(0, "");
                comboBoxCellType.ItemData = itemData.ToArray();
                comboBoxCellType.Items = itemsDisplay.ToArray();
                comboBoxCellType.EditorValue = EditorValue.ItemData;
                ssMaster_Sheet1.Cells[row, col].CellType = comboBoxCellType;
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 스프레드 변경체크
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssMaster_Change(object sender, ChangeEventArgs e)
        {
            //EditorNotifyEventArgs(SpreadView view, Control editingControl, int row, int column);
            EditorNotifyEventArgs eventArgs = new EditorNotifyEventArgs(e.View, null, e.Row, e.Column);
            ssMaster_EditChange(sender, eventArgs);
        }

        /// <summary>
        /// ssExamMaster, ssExam 전체선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssExam_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                if (e.Column == 0)
                {
                    FpSpread fpSpread = sender as FpSpread;

                    bool check = Convert.ToBoolean(fpSpread.ActiveSheet.ColumnHeader.Cells[0, 0].Value);
                    fpSpread.ActiveSheet.ColumnHeader.Cells[0, 0].Value = !check;
                    for (int i = 0; i < fpSpread.ActiveSheet.Rows.Count; i++)
                    {
                        fpSpread.ActiveSheet.Cells[i, 0].Value = !check;
                    }

                    GradeSpreadChangedCheck(fpSpread);
                }
                return;
            }
        }

        /// <summary>
        /// ssExamMaster, ssExam 변경이력체크
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssExam_Change(object sender, ChangeEventArgs e)
        {
            GradeSpreadChangedCheck(sender as FpSpread);
        }

        /// <summary>
        /// 검사코드 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExamAdd_Click(object sender, EventArgs e)
        {
            if (ssMaster_Sheet1.ActiveRowIndex < 0)
            {
                ComFunc.MsgBox("선택된 부서가 없습니다.");
                return;
            }

            if (ssMaster_Sheet1.Cells[ssMaster_Sheet1.ActiveRowIndex, 0].Value == null)
            {
                ComFunc.MsgBox("부서를 먼저 저장 하셔야합니다.");
                return;
            }

            if (GradeSpreadChanged == null || !GradeSpreadChanged.ContainsKey(ssExamList.Name))
            {
                ComFunc.MsgBox("선택된 항목이 없습니다.");
                return;
            }

            //  검사코드 저장을 위해 이동
            for (int i = 0; i < ssExamList_Sheet1.RowCount; i++)
            {
                bool check = Convert.ToBoolean(ssExamList_Sheet1.Cells[i, 0].Value);
                if (!check)
                {
                    continue;
                }

                //  중복 체크
                bool isContains = false;
                for (int j = 0; j < ssExamMaster_Sheet1.RowCount; j++)
                {
                    if (ssExamMaster_Sheet1.Cells[j, 1].Value.Equals(ssExamList_Sheet1.Cells[i, 1].Value))
                    {
                        ssExamList_Sheet1.Cells[i, 0].Value = false;
                        isContains = true;
                        break;
                    }
                }

                if (isContains)
                {
                    continue;
                }

                ssExamList_Sheet1.Cells[i, 0].Value = false;

                ssExamMaster_Sheet1.Rows.Count++;
                ssExamMaster_Sheet1.Cells[ssExamMaster_Sheet1.Rows.Count - 1, 0].Value = false;
                ssExamMaster_Sheet1.Cells[ssExamMaster_Sheet1.Rows.Count - 1, 1].Value = ssExamList_Sheet1.Cells[i, 1].Value;
                ssExamMaster_Sheet1.Cells[ssExamMaster_Sheet1.Rows.Count - 1, 2].Value = ssExamList_Sheet1.Cells[i, 2].Value;
            }

            ssExamList_Sheet1.ColumnHeader.Cells[0, 0].Value = false;
            //SetExamDetailSave(ssExamList_Sheet1, "Y");
        }

        /// <summary>
        /// 검사코드 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExamDelete_Click(object sender, EventArgs e)
        {
            if (GradeSpreadChanged == null || !GradeSpreadChanged.ContainsKey(ssExamMaster.Name))
            {
                ComFunc.MsgBox("선택된 항목이 없습니다.");
                return;
            }

            //SetExamDetailSave(ssExamMaster_Sheet1, "N");

            //  검사코드 삭제
            for (int i = ssExamMaster_Sheet1.RowCount - 1; i >= 0; i--)
            {
                bool check = Convert.ToBoolean(ssExamMaster_Sheet1.Cells[i, 0].Value);
                if (!check)
                {
                    continue;
                }

                ssExamMaster_Sheet1.Rows.Remove(i, 1);
            }

            ssExamMaster_Sheet1.ColumnHeader.Cells[0, 0].Value = false;
        }

        /// <summary>
        /// 부서별 검사단계 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssMaster_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader || e.RowHeader)
            {
                return;
            }

            if (ssMaster_Sheet1.Cells[e.Row, 0].Value == null || string.IsNullOrWhiteSpace(ssMaster_Sheet1.Cells[e.Row, 0].Value.ToString()))
            {
                return;
            }

            //  스프레드 OperationMode = ExtendedSelect 
            if (e.Column == 5)
            {
                bool check = Convert.ToBoolean(ssMaster_Sheet1.Cells[e.Row, 5].Value);
                ssMaster_Sheet1.Cells[e.Row, 5].Value = !check;                
                ssMaster_Sheet1.Cells[e.Row, ssMaster_Sheet1.Columns.Count - 1].Value = "U";
                
            }

            string SqlErr = string.Empty;
            DataTable dt = null;
            ssExamMaster_Sheet1.Rows.Clear();
            ssExamMaster_Sheet1.ColumnHeader.Cells[0, 0].Value = false;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                object masterCode = ssMaster_Sheet1.Cells[e.Row, 0].Value;

                sql = new StringBuilder();
                sql.Append("SELECT A.EXAMMASTERCODE, A.EXAMCODE             ").Append("\n");
                sql.Append("     , B.EXAMNAME                               ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_EXAM_DETAIL A                ").Append("\n");
                sql.Append("  INNER JOIN KOSMOS_OCS.EXAM_MASTER B           ").Append("\n");
                sql.Append("          ON A.EXAMCODE = B.MASTERCODE          ").Append("\n");
                sql.Append(" WHERE A.USEYN = 'Y'                            ").Append("\n");
                sql.Append("   AND A.EXAMMASTERCODE = '" + masterCode + "'  ").Append("\n");


                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssExamMaster_Sheet1.RowCount = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssExamMaster_Sheet1.Cells[i, 0].Value = false;
                    ssExamMaster_Sheet1.Cells[i, 1].Value = dt.Rows[i]["EXAMCODE"];
                    ssExamMaster_Sheet1.Cells[i, 2].Value = dt.Rows[i]["EXAMNAME"];
                }

                SetExamCodeDataBind(masterCode.ToString());
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
        /// 저장 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExamSave_Click(object sender, EventArgs e)
        {
            int intRowAffected = 0;
            string SqlErr = string.Empty;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);

                CellRange[] cells = ssMaster_Sheet1.GetSelections();

                if(cells.Length == 0)
                {
                    if(ssMaster_Sheet1.ActiveRowIndex < 0)
                    {
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        clsDB.setCommitTran(clsDB.DbCon);
                        return;
                    }
                    cells = new CellRange[] { new CellRange(ssMaster_Sheet1.ActiveRowIndex, 0, 1, 6) };
                }
                for (int k = 0; k < cells.Length; k++)
                {
                    CellRange range = cells[k];
                    int max = (range.Row + range.RowCount);
                    for (int i = range.Row; i < max; i++)
                    {
                        string examMasterCode = ssMaster_Sheet1.Cells[i, 0].Text;

                        sql = new StringBuilder();
                        sql.Append("UPDATE ENVIRONMENT_EXAM_DETAIL                  ").Append("\n");
                        sql.Append("   SET USEYN = 'N'                              ").Append("\n");
                        sql.Append(" WHERE EXAMMASTERCODE = '" + examMasterCode + "'").Append("\n");
                        SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);

                        for (int j = 0; j < ssExamMaster_Sheet1.RowCount; j++)
                        {
                            string examCode = ssExamMaster_Sheet1.Cells[j, 1].Text;

                            sql = new StringBuilder();
                            sql.Append("MERGE INTO ENVIRONMENT_EXAM_DETAIL A USING(                 ").Append("\n");
                            sql.Append("    SELECT '" + examMasterCode + "'     AS EXAMMASTERCODE   ").Append("\n");
                            sql.Append("         , '" + examCode + "'           AS EXAMCODE         ").Append("\n");
                            sql.Append("         , 'Y'                          AS USEYN            ").Append("\n");
                            sql.Append("         , TO_CHAR(SYSDATE, 'YYYYMMDD') AS CURDATE          ").Append("\n");
                            sql.Append("         , TO_CHAR(SYSDATE, 'HH24MMSS') AS CURTIME          ").Append("\n");
                            sql.Append("         , '" + clsType.User.Sabun + "' AS USERID           ").Append("\n");
                            sql.Append("      FROM DUAL                                             ").Append("\n");
                            sql.Append(") B ON(                                                     ").Append("\n");
                            sql.Append("        A.EXAMMASTERCODE    = B.EXAMMASTERCODE              ").Append("\n");
                            sql.Append("    AND A.EXAMCODE          = B.EXAMCODE                    ").Append("\n");
                            sql.Append(")                                                           ").Append("\n");
                            sql.Append("WHEN NOT MATCHED THEN                                       ").Append("\n");
                            sql.Append("    INSERT(                                                 ").Append("\n");
                            sql.Append("        A.EXAMMASTERCODE,   A.EXAMCODE, A.USEYN             ").Append("\n");
                            sql.Append("      , A.INSDATE,          A.INSTIME,  A.INSUSER           ").Append("\n");
                            sql.Append("    ) VALUES (                                              ").Append("\n");
                            sql.Append("        B.EXAMMASTERCODE,   B.EXAMCODE, B.USEYN             ").Append("\n");
                            sql.Append("      , B.CURDATE,          B.CURTIME,  B.USERID            ").Append("\n");
                            sql.Append("    )                                                       ").Append("\n");
                            sql.Append("WHEN MATCHED THEN                                           ").Append("\n");
                            sql.Append("    UPDATE                                                  ").Append("\n");
                            sql.Append("       SET A.USEYN      = B.USEYN                           ").Append("\n");
                            sql.Append("         , A.UPDDATE    = B.CURDATE                         ").Append("\n");
                            sql.Append("         , A.UPDTIME    = B.CURTIME                         ").Append("\n");
                            sql.Append("         , A.UPDUSER    = B.USERID                          ").Append("\n");
                            SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);
                        }
                    }
                }

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
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, sql.ToString(), clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region 메소드

        /// <summary>
        /// 단계별 아이템 가져오기
        /// </summary>
        /// <returns></returns>
        private DataTable GetGradeData()
        {
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                sql = new StringBuilder();
                sql.Append("SELECT CODE, CODENAME, GRADE    ").Append("\n");
                sql.Append("  FROM BAS_GRADE_ENVIRONMENT    ").Append("\n");
                sql.Append(" WHERE USEYN    = 'Y'           ").Append("\n");
                sql.Append("ORDER BY GRADE, CODE            ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return null;
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

            return dt;
        }


        /// <summary>
        /// 저장된 단계별 마스터 정보 가져오기
        /// </summary>
        private void SetMasterDataBind()
        {
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                sql = new StringBuilder();
                sql.Append("SELECT CODE, GRADE1, GRADE2, GRADE3, GRADE4             ").Append("\n");
                sql.Append("     , (SELECT COUNT(*) FROM ENVIRONMENT_EXAM_DETAIL A  ").Append("\n");
                sql.Append("         WHERE A.EXAMMASTERCODE = CODE                  ").Append("\n");
                sql.Append("           AND A.USEYN = 'Y'                            ").Append("\n");
                sql.Append("       ) AS CNT                                         ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_EXAM_MASTER                          ").Append("\n");
                sql.Append(" WHERE USEYN = 'Y'                                      ").Append("\n");
                sql.Append("ORDER BY CODE                                           ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssMaster_Sheet1.Rows.Clear();
                ssMaster_Sheet1.RowCount = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssMaster_Sheet1.Rows.Get(i).Locked = true;
                    DataRow row = dt.Rows[i];
                    ssMaster_Sheet1.Cells[i, 0].Value = row["CODE"];
                    ssMaster_Sheet1.Cells[i, 1].Value = row["GRADE1"];
                    ssMaster_Sheet1.Cells[i, 2].Value = row["GRADE2"];
                    ssMaster_Sheet1.Cells[i, 3].Value = row["GRADE3"];
                    ssMaster_Sheet1.Cells[i, 4].Value = row["GRADE4"];
                    ssMaster_Sheet1.Cells[i, 5].Locked = false;


                    if((decimal)row["CNT"] == 0)
                    {
                        ssMaster_Sheet1.Rows.Get(i).BackColor = Color.Gray;
                    }
                    //ssMaster_Sheet1.Cells[i, 5].Value = row["USEYN"].Equals("N");
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

        #endregion Tab 2 - 검사코드 관리

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

        private void frmEnvironmentBas_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmEnvironmentBas_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        #endregion

        #endregion

        private void chkGrade4_Click(object sender, EventArgs e)
        {
            tableLayoutPanel2.ColumnStyles[3].SizeType = SizeType.Percent;
            if (chkGrade4.Checked)
            {
                tableLayoutPanel2.ColumnStyles[3].Width = 23; 
            }
            else
            {
                tableLayoutPanel2.ColumnStyles[3].Width = 0;
            }
        }

        //private void SetExamDetailSave(SheetView spread, string useYn)
        //{
        //    int intRowAffected = 0;
        //    string SqlErr = string.Empty;
        //    StringBuilder sql = null;
        //    try
        //    {
        //        Cursor.Current = Cursors.WaitCursor;

        //        for(int i=0; i<spread.RowCount; i++)
        //        {

        //        }

        //        //clsDB.setBeginTran(clsDB.DbCon);
        //        //object examMasterCode = ssMaster_Sheet1.Cells[ssMaster_Sheet1.ActiveRowIndex, 0].Value;

        //        //for(int i=0; i< spread.RowCount; i++)
        //        //{
        //        //    if(!Convert.ToBoolean(spread.Cells[i, 0].Value))
        //        //    {
        //        //        continue;
        //        //    }

        //        //    object examCode = spread.Cells[i, 1].Value;
        //        //    object examName = spread.Cells[i, 2].Value;
        //        //    sql = new StringBuilder();
        //        //    sql.Append("MERGE INTO ENVIRONMENT_EXAM_DETAIL A USING(                 ").Append("\n");
        //        //    sql.Append("    SELECT '" + examMasterCode + "'     AS EXAMMASTERCODE   ").Append("\n");
        //        //    sql.Append("         , '" + examCode + "'           AS EXAMCODE         ").Append("\n");
        //        //    sql.Append("         , '" + useYn + "'              AS USEYN            ").Append("\n");
        //        //    sql.Append("         , TO_CHAR(SYSDATE, 'YYYYMMDD') AS CURDATE          ").Append("\n");
        //        //    sql.Append("         , TO_CHAR(SYSDATE, 'HH24MMSS') AS CURTIME          ").Append("\n");
        //        //    sql.Append("         , '" + clsType.User.Sabun + "' AS USERID           ").Append("\n");
        //        //    sql.Append("      FROM DUAL                                             ").Append("\n");
        //        //    sql.Append(") B ON(                                                     ").Append("\n");
        //        //    sql.Append("        A.EXAMMASTERCODE    = B.EXAMMASTERCODE              ").Append("\n");
        //        //    sql.Append("    AND A.EXAMCODE          = B.EXAMCODE                    ").Append("\n");
        //        //    sql.Append(")                                                           ").Append("\n");
        //        //    sql.Append("WHEN NOT MATCHED THEN                                       ").Append("\n");
        //        //    sql.Append("    INSERT(                                                 ").Append("\n");
        //        //    sql.Append("        A.EXAMMASTERCODE,   A.EXAMCODE, A.USEYN             ").Append("\n");
        //        //    sql.Append("      , A.INSDATE,          A.INSTIME,  A.INSUSER           ").Append("\n");
        //        //    sql.Append("    ) VALUES (                                              ").Append("\n");
        //        //    sql.Append("        B.EXAMMASTERCODE,   B.EXAMCODE, B.USEYN             ").Append("\n");
        //        //    sql.Append("      , B.CURDATE,          B.CURTIME,  B.USERID            ").Append("\n");
        //        //    sql.Append("    )                                                       ").Append("\n");
        //        //    sql.Append("WHEN MATCHED THEN                                           ").Append("\n");
        //        //    sql.Append("    UPDATE                                                  ").Append("\n");
        //        //    sql.Append("       SET A.USEYN      = B.USEYN                           ").Append("\n");
        //        //    sql.Append("         , A.UPDDATE    = B.CURDATE                         ").Append("\n");
        //        //    sql.Append("         , A.UPDTIME    = B.CURTIME                         ").Append("\n");
        //        //    sql.Append("         , A.UPDUSER    = B.USERID                          ").Append("\n");

        //        //    SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);

        //        //    if (!string.IsNullOrWhiteSpace(SqlErr))
        //        //    {
        //        //        clsDB.setRollbackTran(clsDB.DbCon);
        //        //        clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
        //        //        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
        //        //        Cursor.Current = Cursors.Default;
        //        //        return;
        //        //    }
        //        //}

        //        //clsDB.setCommitTran(clsDB.DbCon);
        //        //ComFunc.MsgBox("저장하였습니다.");

        //        //SpreadView view = new SpreadView(ssMaster);
        //        //CellClickEventArgs eventArgs = new CellClickEventArgs(view, ssMaster_Sheet1.ActiveRowIndex, ssMaster_Sheet1.ActiveColumnIndex, 0, 0, MouseButtons.Left, false, false);
        //        //ssMaster_CellClick(ssMaster, eventArgs);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsDB.setRollbackTran(clsDB.DbCon);
        //        clsDB.SaveSqlErrLog(ex.Message, sql.ToString(), clsDB.DbCon); //에러로그 저장
        //        ComFunc.MsgBox(ex.Message);
        //        Cursor.Current = Cursors.Default;
        //    }
        //}
    }
}