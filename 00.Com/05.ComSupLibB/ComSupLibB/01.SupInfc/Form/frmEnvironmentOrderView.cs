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
using ComSupLibB.SupLbEx;
using ComSupLibB.Com;

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
    /// <seealso cref= " >> frmEnvironmentOrderView.cs" />
    ///
    public partial class frmEnvironmentOrderView : Form, MainFormMessage  
    {
        #region 프로퍼티
        
        private clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        private clsInFcSQL fcSQL = new clsInFcSQL();
        clsComSQL comSql = new clsComSQL();
        ComFunc CF = new ComFunc();

        /// <summary>
        /// 환자번호 고정 테스트 환자임
        /// </summary>
        //private string PANO = "81000014";
        //2019-09-23 안정수 81000014 -> 11077917로 변경
        private string PANO = "11077917";

        string gStrDEPT = "";

        #endregion

        #region 생성자 && 로드

        public frmEnvironmentOrderView()
        {
            InitializeComponent();
        }

        public frmEnvironmentOrderView(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
        }

        private void frmEnvironmentOrderView_Load(object sender, EventArgs e)
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
        }

        #endregion 

        #region 컨트롤 이벤트

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

            string startDate = dtpStart.Value.ToString("yyyyMMdd");
            string endDate = dtpEnd.Value.ToString("yyyyMMdd");
            try
            {
                ssMain_Sheet1.Rows.Clear();
                Cursor.Current = Cursors.WaitCursor;

                fcSQL.ExamResultClone(startDate, endDate);

                sql = new StringBuilder();
                sql.Append("SELECT A.ORDERNO, A.BUCODE, A.REMARK, A.SPECNO                                 ").Append("\n");
                sql.Append("     , DECODE(J.STATUS, '01', '검사중'                                          ").Append("\n");
                sql.Append("                      , '04', '부분완료'                                        ").Append("\n");
                sql.Append("                      , '05', '검사완료'                                        ").Append("\n");
                sql.Append("                      , '06', '취소'                                            ").Append("\n");
                sql.Append("                      , '미접수') AS STATUS                                     ").Append("\n");
                sql.Append("     , A.ORDERDATE, A.ORDERTIME, A.ORDERUSER                                    ").Append("\n");
                sql.Append("     , G.USERNAME AS ORDERUSERNAME                                              ").Append("\n");
                sql.Append("     , A.BARCODE, A.BARCODEDATE, A.BARCODETIME                                  ").Append("\n");
                sql.Append("     , H.USERNAME AS BARCODEUSERNAME                                            ").Append("\n");
                sql.Append("     , TO_CHAR(J.RECEIVEDATE, 'YYYY-MM-DD') AS RECEIPTDATE                      ").Append("\n");
                sql.Append("     , TO_CHAR(J.RECEIVEDATE, 'HH24:MI:SS') AS RECEIPTTIME                      ").Append("\n");
                sql.Append("     , J.JEPSUNAME AS RECEIPTUSER                                               ").Append("\n");
                sql.Append("     , C.CODENAME AS GRADENAM1                                                  ").Append("\n");
                sql.Append("     , D.CODENAME AS GRADENAM2                                                  ").Append("\n");
                sql.Append("     , E.CODENAME AS GRADENAM3                                                  ").Append("\n");
                sql.Append("     , F.CODENAME AS GRADENAM4                                                  ").Append("\n");
                sql.Append("     , E.NAME AS DEPTNAME                                                       ").Append("\n");
                sql.Append("     , (                                                                        ").Append("\n");
                sql.Append("            SELECT LISTAGG(BB.EXAMNAME, ',') WITHIN GROUP(ORDER BY BB.EXAMNAME) ").Append("\n");
                sql.Append("              FROM ENVIRONMENT_EXAM_DETAIL AA                                   ").Append("\n");
                sql.Append("              INNER JOIN KOSMOS_OCS.EXAM_MASTER BB                              ").Append("\n");
                sql.Append("                      ON AA.EXAMCODE = BB.MASTERCODE                            ").Append("\n");
                sql.Append("                     AND AA.USEYN = 'Y'                                         ").Append("\n");
                sql.Append("             WHERE AA.EXAMMASTERCODE = B.CODE                                   ").Append("\n");
                sql.Append("           GROUP BY AA.EXAMMASTERCODE                                           ").Append("\n");
                sql.Append("       ) AS EXAMNAME                                                            ").Append("\n");
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
                sql.Append("  LEFT OUTER JOIN BAS_BUCODE E                                                  ").Append("\n");
                sql.Append("               ON C.BUCODE = E.BUCODE                                           ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_USER G                                                    ").Append("\n");
                sql.Append("               ON A.ORDERUSER   = TRIM(G.SABUN)                                 ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_USER H                                                    ").Append("\n");
                sql.Append("               ON A.BARCODEUSER = TRIM(H.SABUN)                                 ").Append("\n");
                //sql.Append("  LEFT OUTER JOIN BAS_USER I                                                    ").Append("\n");
                //sql.Append("               ON A.RECEIPTUSER = TRIM(I.SABUN)                                 ").Append("\n");
                sql.Append("  LEFT OUTER JOIN KOSMOS_OCS.EXAM_SPECMST J                                     ").Append("\n");
                sql.Append("               ON A.SPECNO = J.SPECNO                                           ").Append("\n");
                sql.Append(" WHERE 1 = 1                                                                    ").Append("\n");
                sql.Append("   AND A.USEYN = 'Y'                                                            ").Append("\n");
                sql.Append("   AND A.ORDERDATE BETWEEN '" + startDate + "' AND '" + endDate + "'            ").Append("\n");
                //sql.Append("   AND A.BUCODE = '" + clsType.User.BuseCode + "'                               ").Append("\n");

                //  바코드 미출력
                if (rdoNoBarcode.Checked)
                {
                    sql.Append("   AND BARCODE IS NULL                                              ").Append("\n");
                }
                //  바코드 출력
                else if (rdoBarcode.Checked)
                {
                    sql.Append("   AND BARCODE IS NOT NULL                                          ").Append("\n");
                }
                //  접수완료
                else if (rdoReceipt.Checked)
                {
                    sql.Append("   AND RECEIPTUSER IS NOT NULL                                      ").Append("\n");
                }

                if(this.cboBuse.SelectedItem.ToString().Trim() != "**.전체")
                {
                    sql.Append("   AND C.CODENAME = '" + cboBuse.SelectedItem.ToString().Trim() + "'").Append("\n"); 
                }

                sql.Append("ORDER BY ORDERNO                                                        ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssMain_Sheet1.RowCount = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    ssMain_Sheet1.Cells[i, 0].Value = false;
                    ssMain_Sheet1.Cells[i, 1].Value = row["ORDERNO"];
                    ssMain_Sheet1.Cells[i, 2].Value = ComFunc.FormatStrToDate(row["ORDERDATE"].ToString(), "D");
                    ssMain_Sheet1.Cells[i, 3].Value = ComFunc.FormatStrToDate(row["ORDERTIME"].ToString(), "T");
                    //ssMain_Sheet1.Cells[i, 4].Value = row["DEPTNAME"];
                    ssMain_Sheet1.Cells[i, 4].Value = row["GRADENAM1"];
                    ssMain_Sheet1.Cells[i, 5].Value = row["ORDERUSERNAME"];
                    ssMain_Sheet1.Cells[i, 6].Value = row["REMARK"];
                    ssMain_Sheet1.Cells[i, 7].Value = string.Concat(row["GRADENAM1"], "-", row["GRADENAM2"], "-", row["GRADENAM3"]);
                    ssMain_Sheet1.Cells[i, 8].Value = row["EXAMNAME"];
                    ssMain_Sheet1.Cells[i, 9].Value = ComFunc.FormatStrToDate(row["BARCODEDATE"].ToString(), "D");
                    ssMain_Sheet1.Cells[i, 10].Value = ComFunc.FormatStrToDate(row["BARCODETIME"].ToString(), "T");
                    ssMain_Sheet1.Cells[i, 11].Value = row["BARCODEUSERNAME"];
                    ssMain_Sheet1.Cells[i, 12].Value = row["RECEIPTDATE"];
                    ssMain_Sheet1.Cells[i, 13].Value = row["RECEIPTTIME"];
                    ssMain_Sheet1.Cells[i, 14].Value = row["RECEIPTUSER"];
                    ssMain_Sheet1.Cells[i, 15].Value = row["SPECNO"];
                    ssMain_Sheet1.Cells[i, 16].Value = row["STATUS"];
                    ssMain_Sheet1.Cells[i, 17].Value = row["BARCODE"];
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
        /// 바코드 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBarcode_Click(object sender, EventArgs e)
        {
            //BarcodePrint("201812050301");

            //return;
            List<int> checkList = new List<int>();
            for (int i = 0; i < ssMain_Sheet1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(ssMain_Sheet1.Cells[i, 0].Value))
                {
                    checkList.Add(i);
                }
            }

            if (checkList.Count < 1)
            {
                ComFunc.MsgBox("선택된 항목이 없습니다.");
                return;
            }

            if (ComFunc.MsgBoxQ("선택항목의 바코드를 출력 하시겠습니까?") != DialogResult.Yes)
            {
                return;
            }

            //  7 바코드 출력일
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;
            int intRowAffected = 0;

            try
            {
                foreach (int row in checkList)
                {
                    string orderNo = ssMain_Sheet1.Cells[row, 1].Text;
                    string strSPECNO = ssMain_Sheet1.Cells[row, 15].Text;

                    if (string.IsNullOrWhiteSpace(strSPECNO))
                    {
                        string date = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                        DateTime dtm = DateTime.ParseExact(date, "yyyyMMdd", null);
                        strSPECNO = lbExSQL.sel_SPECNO_NEXTVAL(clsDB.DbCon, dtm.ToString("yyyy-MM-dd"));

                        sql = new StringBuilder();
                        sql.Append("UPDATE ENVIRONMENT_ORDER                                    ").Append("\n");
                        sql.Append("   SET BARCODE              = '" + strSPECNO + "'           ").Append("\n");
                        sql.Append("     , BARCODEDATE          = TO_CHAR(SYSDATE, 'YYYYMMDD')  ").Append("\n");
                        sql.Append("     , BARCODETIME          = TO_CHAR(SYSDATE, 'HH24MMSS')  ").Append("\n");
                        sql.Append("     , BARCODEUSER          = '" + clsType.User.Sabun + "'  ").Append("\n");
                        sql.Append("     , BARCODEPRINTCOUNT    = BARCODEPRINTCOUNT + 1         ").Append("\n");
                        sql.Append("     , SPECNO               = '" + strSPECNO + "'           ").Append("\n");
                        sql.Append(" WHERE ORDERNO = '" + orderNo + "'                          ").Append("\n");
                        SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);

                        if (InsEXAM_SPECMST(strSPECNO))
                        {
                            if (!InsEXAM_RESULTC(strSPECNO))
                            {
                                ComFunc.MsgBox("바코드 출력 중 오류가 발생함.");
                                return;
                            }
                        }
                        else
                        {
                            ComFunc.MsgBox("바코드 출력 중 오류가 발생함.");
                            return;
                        }
                    }
                    else
                    {
                        sql = new StringBuilder();
                        sql.Append("UPDATE ENVIRONMENT_ORDER                                    ").Append("\n");
                        sql.Append("   SET BARCODEPRINTCOUNT    = BARCODEPRINTCOUNT + 1         ").Append("\n");
                        sql.Append("     , UPDDATE              = TO_CHAR(SYSDATE, 'YYYYMMDD')  ").Append("\n");
                        sql.Append("     , UPDTIME              = TO_CHAR(SYSDATE, 'HH24MMSS')  ").Append("\n");
                        sql.Append(" WHERE ORDERNO = '" + orderNo + "'                          ").Append("\n");
                        SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);
                    }

                    //  바코드 출력
                    BarcodePrint(orderNo);
                }

                ssMain_Sheet1.ColumnHeader.Cells[0, 0].Value = false;
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
        /// 체크박스 전체 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssMain_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader)
            {
                return;
            }

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

        /// <summary>
        /// 출력물 팝업
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssMain_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //frmEnvironmentPRT pRT = new frmEnvironmentPRT();
            //pRT.ShowDialog();
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
        /// 폼초기 설정
        /// </summary>
        private void Init()
        {
            ssMain_Sheet1.Rows.Clear();            
            SetCombo();
            setCtrlInit();

        }

        void SetCombo()
        {
            DataTable dt = null;

            string SQL = "";
            string SqlErr = "";

            cboBuse.Items.Clear();
            cboBuse.Items.Add("**.전체");

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  CODENAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_GRADE_ENVIRONMENT";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND GRADE = '1'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    cboBuse.Items.Add(dt.Rows[i]["CODENAME"].ToString().Trim());
                }
            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 바코드 프린터 출력
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="barCode"></param>
        /// <returns></returns>
        private bool BarcodePrint(string orderNo)
        {
            clsLbExBarCodePrint cls = new clsLbExBarCodePrint();
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                sql = new StringBuilder();
                sql.Append("SELECT A.BARCODEDATE                                                                ").Append("\n");
                //2019-02-11 안정수, 이미경j 요청으로 바코드타임 -> 오더타임으로 변경
                sql.Append("     , A.ORDERTIME                                                                  ").Append("\n");
                sql.Append("     , A.BARCODE                                                                    ").Append("\n");
                sql.Append("     , G.NAME AS DEPTNAME                                                           ").Append("\n");
                sql.Append("     , D.CODENAME AS GRADENAME2                                                     ").Append("\n");
                sql.Append("     , E.CODENAME AS GRADENAME3                                                     ").Append("\n");
                sql.Append("     , F.CODENAME AS GRADENAME4                                                     ").Append("\n");
                sql.Append("     , (                                                                            ").Append("\n");
                sql.Append("            SELECT LISTAGG(DD.BCODENAME, ',') WITHIN GROUP(ORDER BY DD.BCODENAME)   ").Append("\n");
                sql.Append("              FROM ENVIRONMENT_ORDER AA                                             ").Append("\n");
                sql.Append("              INNER JOIN ENVIRONMENT_EXAM_MASTER BB                                 ").Append("\n");
                sql.Append("                      ON AA.ENVIRONMENTCODE = BB.CODE                               ").Append("\n");
                sql.Append("              INNER JOIN ENVIRONMENT_EXAM_DETAIL CC                                 ").Append("\n");
                sql.Append("                      ON BB.CODE = CC.EXAMMASTERCODE                                ").Append("\n");
                sql.Append("              INNER JOIN KOSMOS_OCS.EXAM_MASTER DD                                  ").Append("\n");
                sql.Append("                      ON CC.EXAMCODE = DD.MASTERCODE                                ").Append("\n");
                sql.Append("             WHERE AA.ORDERNO = A.ORDERNO                                           ").Append("\n");
                sql.Append("       ) AS BARCODE_EXAM                                                            ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_ORDER A                                                          ").Append("\n");
                sql.Append("  INNER JOIN ENVIRONMENT_EXAM_MASTER B                                              ").Append("\n");
                sql.Append("          ON A.ENVIRONMENTCODE = B.CODE                                             ").Append("\n");
                sql.Append("  INNER JOIN BAS_GRADE_ENVIRONMENT C                                                ").Append("\n");
                sql.Append("          ON B.GRADE1   = C.CODE                                                    ").Append("\n");
                sql.Append("         AND C.GRADE    = 1                                                         ").Append("\n");
                sql.Append("  INNER JOIN BAS_GRADE_ENVIRONMENT D                                                ").Append("\n");
                sql.Append("          ON B.GRADE2   = D.CODE                                                    ").Append("\n");
                sql.Append("         AND D.GRADE    = 2                                                         ").Append("\n");
                sql.Append("  INNER JOIN BAS_GRADE_ENVIRONMENT E                                                ").Append("\n");
                sql.Append("          ON B.GRADE3   = E.CODE                                                    ").Append("\n");
                sql.Append("         AND E.GRADE    = 3                                                         ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT F                                           ").Append("\n");
                sql.Append("               ON B.GRADE4   = F.CODE                                               ").Append("\n");
                sql.Append("              AND F.GRADE    = 4                                                    ").Append("\n");
                sql.Append("  INNER JOIN BAS_BUCODE G                                                           ").Append("\n");
                sql.Append("          ON C.BUCODE = G.BUCODE                                                    ").Append("\n");
                sql.Append(" WHERE A.ORDERNO = '" + orderNo + "'                                                ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                }

                if(dt == null || dt.Rows.Count == 0)
                {
                    return false;
                }

                string barCode = dt.Rows[0]["BARCODE"].ToString();
                string printTime = ComFunc.FormatStrToDate(dt.Rows[0]["ORDERTIME"].ToString(), "M");
                string examCode = dt.Rows[0]["BARCODE_EXAM"].ToString();
                string deptName = dt.Rows[0]["DEPTNAME"].ToString();

                if(deptName == "종합건강진단센터")
                {
                    deptName = "종합건진";
                }
                string gradeName2 = dt.Rows[0]["GRADENAME2"].ToString();
                string gradeName3 = dt.Rows[0]["GRADENAME3"].ToString();
                string gradeName4 = dt.Rows[0]["GRADENAME4"].ToString();

                //gradeName3 = "G20 1234567";

                StringBuilder barcodeData = new StringBuilder();
                barcodeData.Append("^XA^BY2,2.0^FS^SEE:UHANGUL.DAT^FS^CW1,E:KFONT3.FNT^FS");
                barcodeData.Append("^FO10,25^CI26^A1N,30,30^FD");
                barcodeData.Append(deptName).Append("^FS");
                barcodeData.Append("^FO180,30^A1N,20,20^FD");
                barcodeData.Append(gradeName2);
                barcodeData.Append("^FS");
                //barcodeData.Append(gradeName3).Append("^FS");
                barcodeData.Append("^FO10,70^A1N,43,43^FD");
                barcodeData.Append("감").Append("^FS");
                barcodeData.Append("^FO15,155^A0N,45,45^FD^FS");
                barcodeData.Append("^FO60,60^B3N,N,48,N,N^BY2,2:1,83^FD");
                barcodeData.Append(barCode).Append("^FS");
                barcodeData.Append("^FO30,148^A1N,25,20^FD");
                barcodeData.Append(barCode.Insert(6, "-"));
                barcodeData.Append("  ").Append(printTime);
                barcodeData.Append("  ").Append(gradeName3);
                barcodeData.Append("^FS");
                barcodeData.Append("^FO35,173^A0N,20,20^FD");
                barcodeData.Append(examCode).Append("^FS");
                barcodeData.Append("^FO65,193^A0N,20,20^FD^FS");
                barcodeData.Append("^FO65,213^A0N,20,20^FD^FS");
                barcodeData.Append("^XZ");

                Console.WriteLine(barcodeData.ToString());
                return ComPrintApi.SendStringToPrinter("혈액환자정보", barcodeData.ToString());
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

                return false;
            }
        }

        #endregion


        private bool InsEXAM_SPECMST(string specNo)
        {
            int intRowAffected = 0;
            string SqlErr = string.Empty;
            StringBuilder sql = null;

            try
            {
                sql = new StringBuilder();
                sql.Append("INSERT INTO KOSMOS_OCS.EXAM_SPECMST(                                        ").Append("\n");
                sql.Append("      SPECNO                                                                ").Append("\n");    //  검체번호
                sql.Append("    , PANO                                                                  ").Append("\n");    //  등록번호
                sql.Append("    , BI                                                                    ").Append("\n");    //  환자구분
                sql.Append("    , SNAME                                                                 ").Append("\n");    //  이름
                sql.Append("    , IPDOPD                                                                ").Append("\n");    //  입원(I), 외래(O)
                sql.Append("    , AGE                                                                   ").Append("\n");    //  나이
                sql.Append("    , AGEMM                                                                 ").Append("\n");    //  유아개월수
                sql.Append("    , SEX                                                                   ").Append("\n");    //  성별(M:남, F:여)
                sql.Append("    , DEPTCODE                                                              ").Append("\n");    //  진료과
                sql.Append("    , WARD                                                                  ").Append("\n");    //  병동
                sql.Append("    , ROOM                                                                  ").Append("\n");    //  병실
                sql.Append("    , DRCODE                                                                ").Append("\n");    //  의사코드
                sql.Append("    , DRCOMMENT                                                             ").Append("\n");    //  의사 Comment(형태:검사코드 + 의사코멘트)
                sql.Append("    , STRT                                                                  ").Append("\n");    //  응급여부(S:응급, R:Routine)
                sql.Append("    , SPECCODE                                                              ").Append("\n");    //  검체코드
                sql.Append("    , TUBE                                                                  ").Append("\n");    //  용기코드
                sql.Append("    , WORKSTS                                                               ").Append("\n");    //  BarCode에 인쇄될 문자
                sql.Append("    , BDATE                                                                 ").Append("\n");    //  처방일
                sql.Append("    , BLOODDATE                                                             ").Append("\n");    //  채혈일시
                sql.Append("    , RECEIVEDATE                                                           ").Append("\n");    //  접수일시
                sql.Append("    , STATUS                                                                ").Append("\n");    //  상태 : 00 : 미접수, 01 : 접수/검사중, 02 : 부분입력, 03 : 모두입력, 04 : 부분완료, 05 : 검사완료, 06 : 취소, 14 : 임시저장
                sql.Append("    , EMR                                                                   ").Append("\n");    //  EMR(검사서식변환여부) 0.결과입력/변경 1.서식변환
                sql.Append("    , ORDERDATE                                                             ").Append("\n");    //  오더시간
                sql.Append("    , SENDDATE                                                              ").Append("\n");    //  오더전송시간
                sql.Append("    , GB_GWAEXAM                                                            ").Append("\n");    //  응급실 등 과검사(Y, 과검사)
                sql.Append("    , GB_GWAEXAM2                                                           ").Append("\n");    //  2018-06-15 안정수, GB-GWAEXAM2 추가, 과검사
                sql.Append("    , HICNO                                                                 ").Append("\n");    //  검진, 종검 접수번호(결과전송용)
                sql.Append("    , ORDERNO                                                               ").Append("\n");    //  OCS오더번호
                sql.Append("    , INPS                                                                  ").Append("\n");    //  입력자
                sql.Append("    , INPT_DT                                                               ").Append("\n");    //  입력일시
                sql.Append("    , UPPS                                                                  ").Append("\n");    //  수정자
                sql.Append("    , UPDT                                                                  ").Append("\n");    //수정일시
                sql.Append(")                                                                           ").Append("\n");    //수정일시
                sql.Append("SELECT A.SPECNO                                                             ").Append("\n");
                sql.Append("     , A.PANO, A.BI, A.SNAME                                                ").Append("\n");
                sql.Append("     , 'O' AS IPDOPD                                                        ").Append("\n");
                sql.Append("     , 99 AS AGE                                                            ").Append("\n");
                sql.Append("     , '' AS AGENM                                                          ").Append("\n");
                sql.Append("     , A.SEX, A.DEPTCODE                                                    ").Append("\n");
                sql.Append("     , '' AS WARD                                                           ").Append("\n");
                sql.Append("     , '' AS ROOM                                                           ").Append("\n");
                sql.Append("     , A.DRCODE                                                             ").Append("\n");
                sql.Append("     , '' AS DRCOMMENT                                                      ").Append("\n");
                sql.Append("     , 'R' AS STRT                                                          ").Append("\n");
                sql.Append("     , D.SPECCODE                                                           ").Append("\n");
                sql.Append("     , D.TUBECODE AS TUBE                                                   ").Append("\n");
                sql.Append("     , '감' AS WORKSTS                                                      ").Append("\n");
                sql.Append("     , TO_DATE(B.ORDERDATE || B.ORDERTIME, 'YYYYMMDDHH24MISS') AS BDATE     ").Append("\n");
                //2019-09-25 안정수, blooddate 공백으로 들어가는 부분, 처방날짜와 같도록 보완 
                sql.Append("     , TO_DATE(B.ORDERDATE || B.ORDERTIME, 'YYYYMMDDHH24MISS') AS BLDATE    ").Append("\n");                
                sql.Append("     , '' AS RECEIVEDATE                                                    ").Append("\n");
                sql.Append("     , '00' AS STATUS                                                       ").Append("\n");
                sql.Append("     , '0' AS EMR                                                           ").Append("\n");
                sql.Append("     , TO_DATE(B.ORDERDATE || B.ORDERTIME, 'YYYYMMDDHH24MISS') AS ORDERDATE ").Append("\n");
                sql.Append("     , TO_DATE(B.ORDERDATE || B.ORDERTIME, 'YYYYMMDDHH24MISS') AS SENDDATE  ").Append("\n");
                sql.Append("     , '' AS GB_GWAEXAM                                                     ").Append("\n");
                sql.Append("     , '' AS GB_GWAEXAM2                                                    ").Append("\n");
                sql.Append("     , '' AS HICNO1                                                         ").Append("\n");
                sql.Append("     , '' AS ORDERNO                                                        ").Append("\n");
                sql.Append("     , '' AS INPS                                                           ").Append("\n");
                sql.Append("     , SYSDATE                                                              ").Append("\n");
                sql.Append("     , '' AS UPPS                                                           ").Append("\n");
                sql.Append("     , SYSDATE                                                              ").Append("\n");
                sql.Append("  FROM                                                                      ").Append("\n");
                sql.Append("  (                                                                         ").Append("\n");
                sql.Append("        SELECT '" + specNo + "' AS SPECNO                                   ").Append("\n");
                sql.Append("             , PANO                                                         ").Append("\n");
                sql.Append("             , BI                                                           ").Append("\n");
                sql.Append("             , SNAME                                                        ").Append("\n");
                sql.Append("             , SEX                                                          ").Append("\n");
                sql.Append("             , DEPTCODE                                                     ").Append("\n");
                sql.Append("             , DRCODE                                                       ").Append("\n");
                sql.Append("          FROM BAS_PATIENT                                                  ").Append("\n");
                sql.Append("         WHERE PANO = '" + PANO + "'                                        ").Append("\n");
                sql.Append("  ) A                                                                       ").Append("\n");
                sql.Append("  INNER JOIN ENVIRONMENT_ORDER B                                            ").Append("\n");
                sql.Append("          ON A.SPECNO = B.BARCODE                                           ").Append("\n");
                sql.Append("  INNER JOIN ENVIRONMENT_EXAM_DETAIL C                                      ").Append("\n");
                sql.Append("          ON B.ENVIRONMENTCODE = C.EXAMMASTERCODE                           ").Append("\n");
                sql.Append("  INNER JOIN KOSMOS_OCS.EXAM_MASTER D                                       ").Append("\n");
                sql.Append("          ON D.MASTERCODE = C.EXAMCODE                                      ").Append("\n");
                sql.Append(" WHERE ROWNUM = 1                                                           ").Append("\n");
                SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, sql.ToString(), clsDB.DbCon); //에러로그 저장
                return false;
            }

            return true;
        }

        private bool InsEXAM_RESULTC(string specNo)
        {
            int intRowAffected = 0;
            string SqlErr = string.Empty;
            StringBuilder sql = null;
            DataTable dt = null;

            try
            {
                sql = new StringBuilder();
                sql.Append("SELECT A.ENVIRONMENTCODE AS EQUCODE                 ").Append("\n");
                sql.Append("     , A.SPECNO                                     ").Append("\n");
                sql.Append("     , B.EXAMCODE AS MASTERCODE                     ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_ORDER A                          ").Append("\n");
                sql.Append("  INNER JOIN ENVIRONMENT_EXAM_DETAIL B              ").Append("\n");
                sql.Append("          ON A.ENVIRONMENTCODE = B.EXAMMASTERCODE   ").Append("\n");
                sql.Append("         AND B.USEYN = 'Y'                          ").Append("\n");
                sql.Append(" WHERE A.SPECNO = '" + specNo + "'                  ").Append("\n");
                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                foreach(DataRow row in dt.Rows)
                {
                    sql = new StringBuilder();
                    sql.Append("INSERT INTO KOSMOS_OCS.EXAM_RESULTC (                                           ").Append("\n");
                    sql.Append("    SPECNO, RESULTWS, EQUCODE, SEQNO, PANO                                      ").Append("\n");
                    sql.Append("  , MASTERCODE, SUBCODE, UNIT, STATUS, INPS, INPT_DT, UPPS, UPDT                ").Append("\n");
                    sql.Append(")                                                                               ").Append("\n");
                    sql.Append("SELECT '" + specNo + "'                 AS SPECNO                               ").Append("\n");
                    sql.Append("     , 'I'                              AS RESULTWS                             ").Append("\n");
                    sql.Append("     , '" + row["EQUCODE"] + "'         AS EQUCODE                              ").Append("\n");
                    sql.Append("     , TRIM(TO_CHAR(ROWNUM,'000'))      AS SEQNO                                ").Append("\n");
                    sql.Append("     , '" + PANO + "'                   AS PANO                                 ").Append("\n");
                    sql.Append("     , '" + row["MASTERCODE"] + "'      AS MASTERCODE                           ").Append("\n");
                    sql.Append("     ,  SUBCODE                         AS SUBCODE                              ").Append("\n");
                    sql.Append("     ,  UNIT                            AS UNIT                                 ").Append("\n");
                    sql.Append("     ,  STATUS                          AS STATUS                               ").Append("\n");
                    sql.Append("     ,  '" + clsType.User.IdNumber + "' AS INPS                                 ").Append("\n");
                    sql.Append("     ,  SYSDATE                         AS INPT_DT                              ").Append("\n");
                    sql.Append("     ,  '" + clsType.User.IdNumber + "' AS UPPS                                 ").Append("\n");
                    sql.Append("     ,  SYSDATE                         AS UPDT                                 ").Append("\n");
                    sql.Append("  FROM                                                                          ").Append("\n");
                    sql.Append("  (                                                                             ").Append("\n");
                    sql.Append("        SELECT A.MASTERCODE                                         AS SUBCODE  ").Append("\n");
                    sql.Append("             , A.EQUCODE1                                           AS EQUCODE  ").Append("\n");
                    sql.Append("             , KOSMOS_OCS.FC_EXAM_SPECMST_NM('20' ,A.UNITCODE, 'N') AS UNIT     ").Append("\n");
                    sql.Append("             , CASE WHEN A.RESULTIN = '1' THEN 'H' ELSE 'N' END     AS STATUS   ").Append("\n");
                    sql.Append("             , 0                                                    AS SORT     ").Append("\n");
                    sql.Append("          FROM KOSMOS_OCS.EXAM_MASTER A                                         ").Append("\n");
                    sql.Append("         WHERE 1=1                                                              ").Append("\n");
                    sql.Append("           AND A.MASTERCODE = '" + row["MASTERCODE"] + "'                       ").Append("\n");
                    sql.Append("        UNION ALL                                                               ").Append("\n");
                    sql.Append("        SELECT A.MASTERCODE                                         AS SUBCODE  ").Append("\n");
                    sql.Append("             , A.EQUCODE1                                           AS EQUCODE  ").Append("\n");
                    sql.Append("             , KOSMOS_OCS.FC_EXAM_SPECMST_NM('20', A.UNITCODE, 'N') AS UNIT     ").Append("\n");
                    sql.Append("             , CASE WHEN A.RESULTIN ='1' THEN 'H' ELSE 'N' END      AS STATUS   ").Append("\n");
                    sql.Append("             , B.SORT +1                                            AS SORT     ").Append("\n");
                    sql.Append("          FROM KOSMOS_OCS.EXAM_MASTER         A                                 ").Append("\n");
                    sql.Append("             , KOSMOS_OCS.EXAM_MASTER_SUB     B                                 ").Append("\n");
                    sql.Append("         WHERE 1=1                                                              ").Append("\n");
                    sql.Append("           AND A.MASTERCODE = TRIM(B.NORMAL)                                    ").Append("\n");
                    sql.Append("           AND B.MASTERCODE = '" + row["MASTERCODE"] + "'                       ").Append("\n");
                    sql.Append("           AND B.GUBUN = '31'                                                   ").Append("\n");
                    sql.Append("           AND B.NORMAL > ' '                                                   ").Append("\n");
                    sql.Append("        ORDER BY SORT                                                           ").Append("\n");
                    sql.Append("  )                                                                             ").Append("\n");

                    SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);
                }
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, sql.ToString(), clsDB.DbCon); //에러로그 저장
                return false;
            }
            return true;
        }

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
        
        private void frmEnvironmentOrderView_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmEnvironmentOrderView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        #endregion

        #endregion

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //bool isCheck = false;
            List<string> specList = new List<string>();
            for (int i = 0; i < ssMain_Sheet1.Rows.Count; i++)
            {
                if (ssMain.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    specList.Add(string.Concat("'", ssMain_Sheet1.Cells[i, 15].Text, "'"));
                }     
                //isCheck = true;
            }

            if (specList.Count == 0)
            {
                ComFunc.MsgBox("선택된 항목이 없습니다.");
                return;
            }

            frmEnvironmentPRT pRT = new frmEnvironmentPRT(specList);
            pRT.ShowDialog();
        }               

        void setCtrlInit()
        {           
            string strBuCode = "";

            strBuCode = clsType.User.BuseCode;
            if (strBuCode != "")
            {
                this.gStrDEPT = CF.Read_BuseName(clsDB.DbCon, strBuCode);
                setCtrlCombo();
            }
            
        }

        void setCtrlCombo()
        {
            if (this.gStrDEPT != "")
            {
                if (this.gStrDEPT == "감염관리실")
                {
                    cboBuse.SelectedIndex = 0;
                }
                else
                {
                    if (cboBuse.Items.Contains(this.gStrDEPT))
                    {
                        cboBuse.SelectedItem = this.gStrDEPT;
                    }
                    else
                    {
                        cboBuse.SelectedIndex = 0;
                    }
                }
            }
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}