using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-03-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\Ocs\ipdocs\nurview\Nview16.frm" >> frmChulgo.cs 폼이름 재정의" />


    public partial class FrmEmIPationtList : Form
    {
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        //string FstrREAD = "";

        public FrmEmIPationtList()
        {
            InitializeComponent();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            string[] colHeader = null;


            colHeader = new string[SS1_Sheet1.ColumnCount];

            for (i = 0; i < SS1_Sheet1.ColumnCount; i++)
            {
                colHeader[i] = SS1_Sheet1.ColumnHeader.Cells[0, i].Text.Trim();
            }

            SS1_Sheet1.AddRows(0, 1);

            for (i = 0; i < SS1_Sheet1.ColumnCount; i++)
            {
                SS1_Sheet1.Cells[0, i].Text = colHeader[i];
            }

            clsSpread CS = new clsSpread();
            CS.ExportToXLS(SS1);
            CS = null;

            //AS-IS

            //Dim x               As Integer
            //Dim y               As Integer
            //Dim strDate         As String
            //Dim strDir          As String
            //Dim strMyName       As String
            //Dim strMyPath1      As String
            //Dim strPathName     As String

            //strMyPath1 = "C:\CMC"
            //Screen.MousePointer = 11

            //SS1.RowHeight(-1) = 11      '셀 사이즈 조절

            //SS1.Protect = False

            //strDate = TR(GstrSysDate, "-", "") & "_" & TR(GstrSysTime, ":", "")
            //SS1.ReDraw = False

            //For i = 1 To SS1.ColHeaderRows

            //    '헤더를 배열에 넣기
            //    ReDim Header(SS1.MaxCols)As String
            //    SS1.Row = SpreadHeader + (i - 1)
            //    For j = 2 To SS1.MaxCols
            //        SS1.Col = j
            //        Header(j) = SS1.Text & ""
            //    Next j
            //    '배열값을 Spread에 넣기
            //    SS1.MaxRows = SS1.MaxRows + 1
            //    SS1.Row = i
            //    SS1.Action = ActionInsertRow
            //    For j = 1 To SS1.MaxCols
            //        SS1.Col = j
            //        SS1.CellType = CellTypeEdit
            //        SS1.TypeHAlign = TypeHAlignCenter
            //        SS1.TypeVAlign = TypeVAlignCenter
            //        SS1.Text = Header(j) & ""
            //    Next j
            //Next i


            //x = SS1.ExportToExcel(strMyPath1 & "\" & strDate & ".xls", "BOOK1", "")

            //If x = True Then
            //    MsgBox " C:\CMC" & strDate & ".xls 파일생성 완료되었습니다.", vbInformation, "작업확인"
            //Else
            //    MsgBox "파일생성에 실패하였습니다..", vbInformation, "작업확인"
            //End If

            //SS1.Protect = True

            //Screen.MousePointer = 0

            //SS1.RowHeight(-1) = 14

            //If WorkbookOpen("C:\CMC\" & strDate & ".xls") Then
            //     MsgBox "통합문서를 열었습니다.", vbSystemModal
            //Else
            //     MsgBox "통합문서 열기 실패"
            //End If
        }

        //Function WorkbookOpen(sPath As String) As Boolean
        //    Dim oExcel As Object

        //    If Len(sPath) > 0 And Len(Dir$(sPath)) Then
        //        On Error GoTo ErrFailed
        //        Set oExcel = CreateObject("Excel.Application")
        //        oExcel.Workbooks.Open sPath
        //        oExcel.WindowState = 2
        //        oExcel.Visible = True
        //        Set oExcel = Nothing
        //        WorkbookOpen = True
        //    Else
        //        MsgBox "통합문서를 열 수 없습니다. 경로명을 확인바랍니다. " & sPath
        //        WorkbookOpen = False
        //    End If

        //    Exit Function

        //ErrFailed:
        //    MsgBox "열기실패: " & Err.description
        //    WorkbookOpen = False
        //    On Error GoTo 0
        //End Function



        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string READ_경로(string ArgPano, string ArgInDate, string ArgOutDate)
        {
            string strRtn = "";
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";

                SQL = "SELECT  EXTRACTVALUE(CHARTXML, '//it7')   CHARTXML ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                SQL += ComNum.VBLF + "WHERE Ptno='" + ArgPano + "' ";
                SQL += ComNum.VBLF + "  AND CHARTDATE =replace('" + ArgInDate + "','-','') ";
                SQL += ComNum.VBLF + "  AND FORMNO = '2506' ";
                SQL += ComNum.VBLF + "   ORDER BY CHARTDATE DESC, TRIM(CHARTTIME) DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    strRtn = dt.Rows[0]["CHARTXML"].ToString().Trim();
                }                
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return strRtn;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            READ_DATA();
        }

        private void FrmEmIPationtList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            SS1_Sheet1.RowCount = 0;
            TxtSDATE.Text = Convert.ToDateTime(strDTP).AddDays(-20).ToString("yyyy-MM-dd");
            TxtEDATE.Text = strDTP;
            ComboWard_SET();
        }

        private void ComboWard_SET()
        {
            int i = 0;
            //int j = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  WardCode, WardName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND WARDCODE NOT IN ('IU','NP','2W','NR','DR','IQ') ";
            SQL += ComNum.VBLF + "      AND WARDCODE  IN ('65','33','35')";
            SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";
            SQL += ComNum.VBLF + "ORDER BY WardCode";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            cboWARD.Items.Clear();

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWARD.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }
            }
            
            dt.Dispose();
            dt = null;

            cboWARD.SelectedIndex = 0;
        }

        private string Read_Bed_History(string arg, string argWARD, string argSort = "")
        {
            string strRtn = "";
            string StrTemp = "";
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {                
                SQL = "SELECT a.Roomcode,b.NAME, b.CODE ";
                SQL += ComNum.VBLF + " from NUR_BED_TRANSFER a,BAS_BCODE b  ";
                
                if (argWARD == "33")
                {
                    SQL += ComNum.VBLF + " where GUBUN = 'NUR_ICU_침상번호' AND DELDATE IS NULL ";
                }
                else
                {
                    SQL += ComNum.VBLF + " where GUBUN = 'NUR_EWARD_침상번호' AND DELDATE IS NULL ";
                }
                SQL += ComNum.VBLF + "   AND ipdno = '" + arg + "' ";
                SQL += ComNum.VBLF + "   AND wardcode = '" + argWARD + "' ";
                SQL += ComNum.VBLF + "   and a.bednum=b.code ";
                SQL += ComNum.VBLF + " ORDER BY trsdate  " + argSort + " ";
                

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    StrTemp = dt.Rows[0]["Roomcode"].ToString().Trim() + "-" + dt.Rows[0]["name"].ToString().Trim();
                    if (StrTemp.IndexOf("격리") != -1)
                    {
                        StrTemp = StrTemp.Replace("격리", "격");
                    }
                    strRtn = StrTemp;
                }
                dt.Dispose();
                dt = null;
                return strRtn;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return strRtn;
        }

        private string READ_INDATE_ER(string ArgPano, string ArgInDate)
        {

            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strRtn = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT CHARTTIME,  to_char(to_date(CHARTDATE ||' '||SUBSTR(CHARTTIME,1,4),'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')  CHARTDATE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                SQL += ComNum.VBLF + "WHERE Ptno='" + ArgPano + "' ";
                SQL += ComNum.VBLF + "  AND CHARTDATE =replace('" + ArgInDate + "','-','') ";
                SQL += ComNum.VBLF + "  AND FORMNO in ('2506','2678')";

                SQL += ComNum.VBLF + " UNION ALL";
                SQL += ComNum.VBLF + " SELECT CHARTTIME, to_char(to_date(CHARTDATE ||' '||SUBSTR(CHARTTIME,1,4),'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')  CHARTDATE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST ";
                SQL += ComNum.VBLF + "WHERE Ptno = '" + ArgPano + "' ";
                SQL += ComNum.VBLF + "  AND CHARTDATE = replace('" + ArgInDate + "','-','') ";
                SQL += ComNum.VBLF + "  AND FORMNO in (2506, 2678) ";
                SQL += ComNum.VBLF + "ORDER BY  CHARTTIME asc ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    strRtn = dt.Rows[0]["CHARTDATE"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
                return strRtn;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return strRtn;
        }

        private string READ_OUTDATE_ER(string ArgPano, string ArgInDate)
        {

            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strRtn = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT nvl(max(EXTRACTVALUE(CHARTXML, '//it104')),max(EXTRACTVALUE(CHARTXML, '//dt4'))||' '||max(EXTRACTVALUE(CHARTXML, '//it106'))) OUTTIME ";
                //SQL = " SELECT EXTRACTVALUE(CHARTXML, '//it104') OUTTIME ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                SQL += ComNum.VBLF + "WHERE Ptno='" + ArgPano + "' ";
                SQL += ComNum.VBLF + "  AND CHARTDATE =replace('" + ArgInDate + "','-','') ";
                SQL += ComNum.VBLF + "  AND FORMNO IN ('2506','2678')";


                //SQL += ComNum.VBLF + " UNION ALL";
                //SQL += ComNum.VBLF + " SELECT CHARTTIME, to_char(to_date(CHARTDATE ||' '||SUBSTR(CHARTTIME,1,4),'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')  CHARTDATE ";
                //SQL += ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST ";
                //SQL += ComNum.VBLF + "WHERE Ptno = '" + ArgPano + "' ";
                //SQL += ComNum.VBLF + "  AND CHARTDATE = replace('" + ArgInDate + "','-','') ";
                //SQL += ComNum.VBLF + "  AND FORMNO in (2506, 2678) ";
                SQL += ComNum.VBLF + "ORDER BY  CHARTTIME asc ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    strRtn = dt.Rows[0]["OUTTIME"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
                return strRtn;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return strRtn;
        }

        private string READ_INDATE_ICU(string ArgPano, string ArgInDate, string ArgWardCode)
        {

            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strRtn = "";
            //string strWard = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {                
                SQL = " SELECT EXTRACTVALUE(b.CHARTXML, '//ta3') WardCode , to_char(to_date(min(a.CHARTDATE||a.CHARTTIME),'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')  CHARTDATE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B  ";
                SQL += ComNum.VBLF + " WHERE a.PTNO = '" + ArgPano + "' ";
                SQL += ComNum.VBLF + " AND a.FORMNO = '965'  AND a.emrno = b.emrno ";
                SQL += ComNum.VBLF + " AND EXTRACTVALUE(b.CHARTXML, '//ta3')  = '" + ArgWardCode + "' ";
                SQL += ComNum.VBLF + " AND a.MEDFRDATE =replace('" + ArgInDate + "','-','') ";
                SQL += ComNum.VBLF + "  group by EXTRACTVALUE(b.CHARTXML, '//ta3') ";

                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT B.WARDCODE, to_char(to_date(min(a.CHARTDATE || SUBSTR(a.CHARTTIME, 0, 4)),'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')  CHARTDATE";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRNURSRECORD B";
                SQL = SQL + ComNum.VBLF + "       ON A.EMRNO = B.EMRNO";
                SQL = SQL + ComNum.VBLF + "      AND B.WARDCODE = '" + ArgWardCode + "'";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + ArgPano + "'";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO = 965";
                SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE = '" + ArgInDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + " GROUP BY B.WARDCODE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    strRtn = dt.Rows[0]["CHARTDATE"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
                return strRtn;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return strRtn;
        }

        private string READ_OUTDATE_ICU(string ArgPano, string ArgInDate, string ArgWardCode, string ArgOutDate)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strRtn = "";
            //string strWard = "";
            
            Cursor.Current = Cursors.WaitCursor;

            try
            {                
                SQL = " SELECT EXTRACTVALUE(b.CHARTXML, '//ta3') WardCode , to_char(to_date(max(a.CHARTDATE || SUBSTR(a.CHARTTIME, 0, 4)),'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')  CHARTDATE ";                
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B  ";
                SQL += ComNum.VBLF + " WHERE a.PTNO = '" + ArgPano + "' ";
                SQL += ComNum.VBLF + " AND a.FORMNO = '965'  AND a.emrno = b.emrno ";
                SQL += ComNum.VBLF + " AND a.MEDFRDATE = replace('" + ArgInDate + "','-','') ";
                SQL += ComNum.VBLF + "   group by EXTRACTVALUE(b.CHARTXML, '//ta3')  ";

                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT B.WARDCODE, to_char(to_date(max(a.CHARTDATE || SUBSTR(a.CHARTTIME, 0, 4)),'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')  CHARTDATE";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRNURSRECORD B";
                SQL = SQL + ComNum.VBLF + "       ON A.EMRNO = B.EMRNO";
                SQL = SQL + ComNum.VBLF + "      AND B.WARDCODE = '" + ArgWardCode + "'";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + ArgPano + "'";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO = 965";
                SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE = '" + ArgInDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + " GROUP BY B.WARDCODE";
                SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if(dt.Rows.Count == 1 && dt.Rows[i]["WardCode"].ToString().Trim() == ArgWardCode && ArgOutDate != "" )
                        {
                            strRtn = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                        }
                        else if(dt.Rows.Count > 1 && dt.Rows[i]["WardCode"].ToString().Trim() == ArgWardCode && i != 0 )
                        {
                            strRtn = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                        }
                        else if( dt.Rows.Count > 1 && dt.Rows[i]["WardCode"].ToString().Trim() == ArgWardCode && i == 0 && ArgOutDate != "" )
                        {
                            strRtn = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                        }
                     }             
                }
                dt.Dispose();
                dt = null;
                return strRtn;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strRtn;
            }            
        }

        private string READ_재입실_ICU(string ArgPano, string ArgInDate)
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strRtn = "";
            //string strWard = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT to_char(to_date(B.CHARTDATE ||' '||SUBSTR(B.CHARTTIME,1,4),'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')  CHARTDATE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML A,KOSMOS_EMR.EMRXMLMST B";
                SQL += ComNum.VBLF + " WHERE B.Ptno='" + ArgPano + "' ";
                SQL += ComNum.VBLF + "  AND B.medfrdate =replace('" + VB.Mid(ArgInDate, 1, 10) + "','-','') ";
                SQL += ComNum.VBLF + "  AND B.FORMNO = '2280' ";
                SQL += ComNum.VBLF + "  AND B.INOUTCLS = 'I' ";
                SQL += ComNum.VBLF + "  AND (EXTRACTVALUE(CHARTXML, '//it22') in( '33','35')  AND EXTRACTVALUE(CHARTXML, '//it19') not in( '33','35')  ) ";
                SQL += ComNum.VBLF + "  AND B.CHARTDATE||' '||B.CHARTTIME >replace('" + ArgInDate + "00','-','')   ";
                SQL += ComNum.VBLF + "  AND B.EMRNO =  A.EMRNO  ";
                SQL += ComNum.VBLF + "   ORDER BY B.CHARTDATE DESC, TRIM(B.CHARTTIME) DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    strRtn = dt.Rows[0]["CHARTDATE"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
                return strRtn;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return strRtn;
        }

        private void READ_DATA()
        {
            int i = 0;
            int j = 0;
            //int nIlsuA = 0;
            int nIlsuB = 0;
            //string StrICUTime = "";
            //int nRead = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            SS1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT DISTINCT A.SNAME,A.PANO,B.KTASLEVL , A.DEPTCODE ,C.DIAGNOSIS ,B.BEDNUM ,B.OUTDATE ";
                SQL += ComNum.VBLF + " ,TO_CHAR(B.OUTDATE, 'YYYY-MM-DD') OUTDATE ,TO_CHAR(A.INDATE, 'YYYY-MM-DD HH24:MI') INDATE ,A.IPDNO ,D.CHECKSABUN,B.IPDNO, C.PATH_IN, C.ROWID, B.KTASLEVL  ";
                SQL += ComNum.VBLF + " FROM IPD_BM A,IPD_NEW_MASTER B,NUR_MASTER C,NUR_ERICU_NOTECHECK D ";
                SQL += ComNum.VBLF + " WHERE A.JOBDATE >= TO_DATE('" + TxtSDATE.Text + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "      AND A.JOBDATE <= TO_DATE('" + TxtEDATE.Text + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "      AND B.INDATE >= TO_DATE('2017-07-19 00:00','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "      AND A.IPDNO = B.IPDNO ";
                SQL += ComNum.VBLF + "      AND A.IPDNO = C.IPDNO ";
                SQL += ComNum.VBLF + "      AND A.IPDNO = D.IPDNO(+) ";
                if (string.Compare(TxtSDATE.Text, "2017-07-10") < 0)
                {
                    SQL += ComNum.VBLF + "      AND decode(A.wardcode,'40','65',A.wardcode) ='" + cboWARD.Text + "'";
                }
                else
                {
                    SQL += ComNum.VBLF + "      AND A.wardcode ='" + cboWARD.Text + "'";
                }
                SQL += ComNum.VBLF + "      AND GBBACKUP='J' ";
                SQL += ComNum.VBLF + "      order by INDATE desc  ";
                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.Rows.Count = dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["CHECKSABUN"].ToString().Trim();
                        //nIlsuA = 0;
                        nIlsuB = 0;

                        if (dt.Rows[i]["CHECKSABUN"].ToString().Trim() != "")
                        {
                            for (j = 1; j <= 17; j++)
                            {
                                SS1_Sheet1.Cells[i, j - 1].BackColor = System.Drawing.Color.FromArgb(200, 200, 220);
                            }
                            SS1_Sheet1.Cells[i, 0].Value = true;
                        }
                        else
                        {
                            for (j = 1; j <= 17; j++)
                            {
                                SS1_Sheet1.Cells[i, j - 1].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                            }
                            SS1_Sheet1.Cells[i, 0].Value = false;
                        }

                        SS1_Sheet1.Cells[i, 1].Text = Read_Bed_History(dt.Rows[i]["IPDNO"].ToString().Trim(), cboWARD.Text, "DESC");
                        
                        SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["pano"].ToString().Trim();

                        SS1_Sheet1.Cells[i, 6].Text = VB.Left(READ_INDATE_ER(dt.Rows[i]["pano"].ToString().Trim(), VB.Mid(dt.Rows[i]["INDATE"].ToString().Trim(), 1, 10)), 10); //응급실입실시간
                        SS1_Sheet1.Cells[i, 7].Text = VB.Right(READ_INDATE_ER(dt.Rows[i]["pano"].ToString().Trim(), VB.Mid(dt.Rows[i]["INDATE"].ToString().Trim(), 1, 10)), 5); //응급실입실시간
                        
                        if (SS1_Sheet1.Cells[i, 7].Text == "")
                        {
                            SS1_Sheet1.Cells[i, 12].Text =
                            READ_경로(dt.Rows[i]["PANO"].ToString().Trim(), VB.Mid(dt.Rows[i]["INDATE"].ToString().Trim(), 1, 10), VB.Mid(dt.Rows[i]["OUTDATE"].ToString().Trim(), 1, 10));   //'내원경로 병원/요양병원
                        }
                        else
                        {
                            SS1_Sheet1.Cells[i, 12].Text = "ER";
                        }
                        
                        SS1_Sheet1.Cells[i, 8].Text = VB.Left(READ_OUTDATE_ER(dt.Rows[i]["pano"].ToString().Trim(), VB.Mid(dt.Rows[i]["INDATE"].ToString().Trim(), 1, 10)), 10); //응급실퇴실시간
                        SS1_Sheet1.Cells[i, 9].Text = VB.Right(READ_OUTDATE_ER(dt.Rows[i]["pano"].ToString().Trim(), VB.Mid(dt.Rows[i]["INDATE"].ToString().Trim(), 1, 10)), 5); //응급실퇴실시간

                        if (SS1_Sheet1.Cells[i, 8].Text != "")
                        {
                            SS1_Sheet1.Cells[i, 2].Text = "Y";
                            SS1_Sheet1.Cells[i, 3].Text = "Y";
                        }
                        else
                        {
                            SS1_Sheet1.Cells[i, 2].Text = "N";
                            SS1_Sheet1.Cells[i, 3].Text = "N";
                        }
                        
                        SS1_Sheet1.Cells[i, 10].Text = VB.Left(READ_INDATE_ICU(dt.Rows[i]["pano"].ToString().Trim(), VB.Mid(dt.Rows[i]["INDATE"].ToString().Trim(), 1, 10), cboWARD.Text), 10); //ICU입실시간
                        SS1_Sheet1.Cells[i, 11].Text = VB.Right(READ_INDATE_ICU(dt.Rows[i]["pano"].ToString().Trim(), VB.Mid(dt.Rows[i]["INDATE"].ToString().Trim(), 1, 10), cboWARD.Text), 5); //ICU입실시간

                        if (READ_INDATE_ICU(dt.Rows[i]["Pano"].ToString().Trim(), VB.Mid(dt.Rows[i]["INDATE"].ToString().Trim(), 1, 10), cboWARD.Text) != "")
                        {
                            //nIlsuA = Convert.ToInt32(READ_INDATE_ICU(dt.Rows[i]["PANO"].ToString().Trim(), VB.Mid(dt.Rows[i]["INDATE"].ToString().Trim(), 1, 10), cboWARD.Text));
                        }

                        
                        SS1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();

                        SS1_Sheet1.Cells[i, 14].Text = VB.Left(READ_OUTDATE_ICU(dt.Rows[i]["PANO"].ToString().Trim(), VB.Mid(dt.Rows[i]["INDATE"].ToString().Trim(), 1, 10), cboWARD.Text, dt.Rows[i]["OUTDATE"].ToString().Trim()), 10);//퇴실일자
                        SS1_Sheet1.Cells[i, 15].Text = VB.Right(READ_OUTDATE_ICU(dt.Rows[i]["PANO"].ToString().Trim(), VB.Mid(dt.Rows[i]["INDATE"].ToString().Trim(), 1, 10), cboWARD.Text, dt.Rows[i]["OUTDATE"].ToString().Trim()), 5); // 'ICU퇴실시간

                        if (READ_OUTDATE_ICU(dt.Rows[i]["PANO"].ToString().Trim(), VB.Mid(dt.Rows[i]["INDATE"].ToString().Trim(), 1, 10), cboWARD.Text, dt.Rows[i]["OUTDATE"].ToString().Trim()) != "")
                        {
                            nIlsuB = Convert.ToInt32(VB.Val(READ_OUTDATE_ICU(dt.Rows[i]["PANO"].ToString().Trim(), VB.Mid(dt.Rows[i]["INDATE"].ToString().Trim(), 1, 10), cboWARD.Text, dt.Rows[i]["OUTDATE"].ToString().Trim())));
                        }

                        
                        //SS1_Sheet1.Cells[i, 17].Text = READ_재입실_ICU(dt.Rows[i]["PANO"].ToString().Trim(), VB.Left(StrICUTime, 10)); //'ICU재입실

                        //if (nIlsuA > 0 && nIlsuB > 0)
                        //{
                        //    //Round((nIlsuB - nIlsuA) * 24, 0)
                        //    SS1_Sheet1.Cells[i, 16].Text = ((nIlsuB - nIlsuA) * 24).ToString();
                        //}

                        if (Read_Bed_History(dt.Rows[i]["IPDNO"].ToString().Trim(), cboWARD.Text, "ASC") == Read_Bed_History(dt.Rows[i]["IPDNO"].ToString().Trim(), cboWARD.Text, "DESC"))
                        {
                            SS1_Sheet1.Cells[i, 16].Text = Read_Bed_History(dt.Rows[i]["IPDNO"].ToString().Trim(), cboWARD.Text, "DESC");
                        }
                        else
                        {
                            SS1_Sheet1.Cells[i, 16].Text = Read_Bed_History(dt.Rows[i]["IPDNO"].ToString().Trim(), cboWARD.Text, "ASC") + "->" + Read_Bed_History(dt.Rows[i]["IPDNO"].ToString().Trim(), cboWARD.Text, "DESC");
                        }

                        SS1_Sheet1.Cells[i, 17].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        SS1_Sheet1.Cells[i, 18].Text = dt.Rows[i]["KTASLEVL"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            //string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
            
            }
                //strTitle = "";

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void cboWARD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWARD.Text == "33" || cboWARD.Text == "35")
            {
                SS1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "EICU입실일자";
                SS1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "EICU입실시간";
                SS1_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "EICU퇴실일자";
                SS1_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "EICU퇴실시간";
            }
            else
            {
                SS1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "응급병동입실일자";
                SS1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "응급병동입실시간";
                SS1_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "응급병동퇴실일자";
                SS1_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "응급병동퇴실시간";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strPath = "";
            string strROWID = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < SS1_Sheet1.RowCount; i++)
                {
                    strROWID = SS1_Sheet1.Cells[i, 20].Text.Trim();
                    strPath = SS1_Sheet1.Cells[i, 12].Text.Trim();

                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_MASTER SET ";
                    SQL = SQL + ComNum.VBLF + " PATH_IN = '" + strPath + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                READ_DATA();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }
    }
}
