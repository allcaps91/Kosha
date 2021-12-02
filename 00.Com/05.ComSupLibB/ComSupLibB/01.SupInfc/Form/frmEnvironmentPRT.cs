using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static ComBase.clsSpread;

namespace ComSupLibB.SupInfc
{
    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : frmComSupLbExPrt02.cs
    /// Title or Description : 검사결과지 출력
    /// Author : 김홍록
    /// Create Date : 2017-06-02
    /// Update History : 
    /// </summary>
    public partial class frmEnvironmentPRT : Form
    {
        DataTable gDt = null;
        clsDB ClsDb = new clsDB();
        clsSpread ClsSpread = new clsSpread();
        ComFunc CF = new ComFunc();
        string gStrSpecNo;
        int nHeaderCnt = 4;
        int nMaxRowSize = 30;
        int nRowHeight = 15;

        List<string> specList;


        public frmEnvironmentPRT()
        {
            InitializeComponent();
        }

        public frmEnvironmentPRT(List<string> _specList)
        {
            InitializeComponent();
            specList = _specList;

            Init();
        }

        public void Init()
        {
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            string strFootNote = string.Empty;
            DataTable dt = null;
            int row = 0;
            ssOp.BringToFront();
            FarPoint.Win.Spread.CellType.TextCellType tct = new FarPoint.Win.Spread.CellType.TextCellType();

            try
            {
                string param = string.Join(",", specList.ToArray());

                Cursor.Current = Cursors.WaitCursor;
                sql = new StringBuilder();
                sql.Append("SELECT A.ORDERUSER                          ").Append("\n");
                sql.Append("     , A.ORDERDATE                          ").Append("\n");
                //2019-02-22 안정수, ORDERNO 추가 
                sql.Append("     , A.ORDERNO                            ").Append("\n");
                //2020-01-22 안정수, BUCODE 추가 
                sql.Append("     , A.BUCODE                             ").Append("\n");
                //2019-02-28 안정수, RECEIVEDATE 추가
                sql.Append("     , B.RECEIVEDATE                        ").Append("\n");
                sql.Append("     , C.RESULT                             ").Append("\n");
                sql.Append("     , C.RESULTDATE                         ").Append("\n");
                sql.Append("     , C.RESULTSABUN                        ").Append("\n");
                sql.Append("     , I.USERNAME AS RESULTNAME             ").Append("\n");
                sql.Append("     , E.BUCODE                             ").Append("\n");
                sql.Append("     , G.NAME AS DEPTNAME                   ").Append("\n");
                sql.Append("     , F.CODENAME                           ").Append("\n");
                sql.Append("     , H.USERNAME AS ORDERUSERNAME          ").Append("\n");
                sql.Append("     , K.NAME AS ORDERDEPT                  ").Append("\n");
                sql.Append("     , E.IDNUMBER                           ").Append("\n");
                sql.Append("     , L.USERNAME                           ").Append("\n");
                //2019-02-11 안정수 추가
                sql.Append("     , H.CODENAME AS EXAMNAME               ").Append("\n");
                //2019-07-15 안정수 추가
                sql.Append("     , A.BARCODE                            ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_ORDER A                  ").Append("\n");
                sql.Append("  INNER JOIN KOSMOS_OCS.EXAM_SPECMST B      ").Append("\n");
                sql.Append("          ON A.SPECNO = B.SPECNO            ").Append("\n");
                sql.Append("         AND B.STATUS = '05'                ").Append("\n");
                sql.Append("  INNER JOIN KOSMOS_OCS.EXAM_RESULTC C      ").Append("\n");
                sql.Append("          ON B.SPECNO = C.SPECNO            ").Append("\n");
                //sql.Append("         AND C.SUBCODE = 'MI32'             ").Append("\n");
                sql.Append("  INNER JOIN ENVIRONMENT_EXAM_MASTER D      ").Append("\n");
                sql.Append("          ON A.ENVIRONMENTCODE = D.CODE     ").Append("\n");
                sql.Append("  INNER JOIN BAS_GRADE_ENVIRONMENT E        ").Append("\n");
                sql.Append("          ON D.GRADE1 = E.CODE              ").Append("\n");
                sql.Append("         AND E.GRADE = '1'                  ").Append("\n");
                sql.Append("  INNER JOIN BAS_GRADE_ENVIRONMENT F        ").Append("\n");
                sql.Append("          ON D.GRADE2 = F.CODE              ").Append("\n");
                sql.Append("         AND F.GRADE = '2'                  ").Append("\n");
                //2019-02-11 안정수, 추가함
                sql.Append("  INNER JOIN BAS_GRADE_ENVIRONMENT H        ").Append("\n");
                sql.Append("          ON D.GRADE3 = H.CODE              ").Append("\n");
                sql.Append("         AND H.GRADE = '3'                  ").Append("\n");
                sql.Append("  INNER JOIN BAS_BUCODE G                   ").Append("\n");
                sql.Append("          ON E.BUCODE = G.BUCODE            ").Append("\n");
                sql.Append("  INNER JOIN BAS_USER H                     ").Append("\n");
                sql.Append("          ON A.ORDERUSER = TRIM(H.SABUN)    ").Append("\n");
                sql.Append("  INNER JOIN BAS_USER I                     ").Append("\n");
                sql.Append("          ON C.RESULTSABUN = TRIM(I.SABUN)  ").Append("\n");
                sql.Append("  LEFT OUTER JOIN KOSMOS_ADM.INSA_MST J     ").Append("\n");
                sql.Append("               ON H.SABUN = J.SABUN         ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_BUSE K                ").Append("\n");
                sql.Append("               ON J.BUSE = K.BUCODE         ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_USER L                ").Append("\n");
                sql.Append("               ON L.IDNUMBER = E.IDNUMBER   ").Append("\n");
                sql.Append(" WHERE 1 = 1                                ").Append("\n");
                sql.Append("   AND A.SPECNO IN(" + param + ")           ").Append("\n");
                //sql.Append("ORDER BY F.CODENAME                         ").Append("\n");
                sql.Append("ORDER BY A.ORDERNO                          ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if(dt != null && dt.Rows.Count > 0)
                {
                    //  오더자
                    ssOp_Sheet1.Cells[5, 3].Text = string.Concat(dt.Rows[0]["ORDERDEPT"], " / ", dt.Rows[0]["ORDERUSERNAME"]);
                    //  오더일
                    //2019-02-28 안정수, 이미경j 요청으로 의뢰일을 접수날짜로 변경 
                    ssOp_Sheet1.Cells[5, 9].Text = VB.Left(dt.Rows[0]["RECEIVEDATE"].ToString(), 10);
                    //  담당자
                    ssOp_Sheet1.Cells[6, 3].Text = string.Concat(CF.Read_BuseName(clsDB.DbCon, dt.Rows[0]["BUCODE"].ToString().Trim()), " / ", dt.Rows[0]["USERNAME"]);

                    DateTime resultTime = DateTime.MinValue;
                    string resultName = string.Empty;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        DateTime dtm = (DateTime)dt.Rows[i]["RESULTDATE"];

                        if (DateTime.Compare(resultTime, dtm) < 0)
                        {
                            resultTime = dtm;
                            resultName = dt.Rows[i]["RESULTNAME"].ToString();
                        }

                        if (i < 20)
                        { 
                            row = i + 11;

                            //2019-02-11 안정수 추가
                            ssOp.ActiveSheet.Cells[row, 3].CellType = tct;
                            tct.Multiline = true;
                            tct.WordWrap = true;
                            //ssOp.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 3);

                            //2019-02-11 안정수, 첫번째 칼럼에 검사명도 같이 보이도록 추가 EXAMNAME
                            ssOp_Sheet1.Cells[row, 0].Text = dt.Rows[i]["EXAMNAME"].ToString().Trim() + " / " + dt.Rows[i]["CODENAME"].ToString();

                            //2019-07-15 안정수, 풋노트 추가
                            strFootNote = Read_FootNote(dt.Rows[i]["BARCODE"].ToString().Trim());
                            if(strFootNote != "")
                                    {
                                ssOp_Sheet1.Cells[row, 3].Text = dt.Rows[i]["RESULT"].ToString() + " " + strFootNote;
                            }
                            else
                            {
                                ssOp_Sheet1.Cells[row, 3].Text = dt.Rows[i]["RESULT"].ToString();
                            }                            

                            //  검사완료일
                            ssOp_Sheet1.Cells[32, 10].Text = resultTime.ToString("yyyy-MM-dd");
                            //  검사자
                            ssOp_Sheet1.Cells[33, 10].Text = resultName;
                            //  과장님
                            ssOp_Sheet1.Cells[34, 10].Text = "양선문";
                        }

                        if(i >= 20 && i <= 40)
                        {
                            row = i - 10;
                            
                            //  오더자
                            ssOp2_Sheet1.Cells[5, 3].Text = string.Concat(dt.Rows[0]["ORDERDEPT"], " / ", dt.Rows[0]["ORDERUSERNAME"]);
                            //  오더일
                            ssOp2_Sheet1.Cells[5, 9].Text = ComFunc.FormatStrToDate(dt.Rows[0]["ORDERDATE"].ToString(), "D");
                            //  담당자
                            ssOp2_Sheet1.Cells[6, 3].Text = string.Concat(dt.Rows[0]["DEPTNAME"], " / ", dt.Rows[0]["USERNAME"]);


                            ssOp2_Sheet1.Cells[row, 0].Text = dt.Rows[i]["EXAMNAME"].ToString().Trim() + " / " + dt.Rows[i]["CODENAME"].ToString();

                            //2019-07-15 안정수, 풋노트 추가
                            strFootNote = Read_FootNote(dt.Rows[i]["BARCODE"].ToString().Trim());
                            if (strFootNote != "")
                            {
                                ssOp2_Sheet1.Cells[row, 3].Text = dt.Rows[i]["RESULT"].ToString() + " " + strFootNote;
                            }
                            else
                            {
                                ssOp2_Sheet1.Cells[row, 3].Text = dt.Rows[i]["RESULT"].ToString();
                            }

                            //  검사완료일
                            ssOp2_Sheet1.Cells[32, 10].Text = resultTime.ToString("yyyy-MM-dd");
                            //  검사자
                            ssOp2_Sheet1.Cells[33, 10].Text = resultName;
                            //  과장님
                            ssOp2_Sheet1.Cells[34, 10].Text = "양선문";
                        }
                        if (i >= 41)
                        {
                            row = i - 31;

                            //  오더자
                            ssOp3_Sheet1.Cells[5, 3].Text = string.Concat(dt.Rows[0]["ORDERDEPT"], " / ", dt.Rows[0]["ORDERUSERNAME"]);
                            //  오더일
                            ssOp3_Sheet1.Cells[5, 9].Text = ComFunc.FormatStrToDate(dt.Rows[0]["ORDERDATE"].ToString(), "D");
                            //  담당자
                            ssOp3_Sheet1.Cells[6, 3].Text = string.Concat(dt.Rows[0]["DEPTNAME"], " / ", dt.Rows[0]["USERNAME"]);


                            ssOp3_Sheet1.Cells[row, 0].Text = dt.Rows[i]["EXAMNAME"].ToString().Trim() + " / " + dt.Rows[i]["CODENAME"].ToString();

                            //2019-07-15 안정수, 풋노트 추가
                            strFootNote = Read_FootNote(dt.Rows[i]["BARCODE"].ToString().Trim());
                            if (strFootNote != "")
                            {
                                ssOp3_Sheet1.Cells[row, 3].Text = dt.Rows[i]["RESULT"].ToString() + " " + strFootNote;
                            }
                            else
                            {
                                ssOp3_Sheet1.Cells[row, 3].Text = dt.Rows[i]["RESULT"].ToString();
                            }

                            //  검사완료일
                            ssOp3_Sheet1.Cells[32, 10].Text = resultTime.ToString("yyyy-MM-dd");
                            //  검사자
                            ssOp3_Sheet1.Cells[33, 10].Text = resultName;
                            //  과장님
                            ssOp3_Sheet1.Cells[34, 10].Text = "양선문";
                        }
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            SpdPrint_Margin setMargin = new SpdPrint_Margin(1, 1, 10, 10, 10, 10);
            SpdPrint_Option setOption = new SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, false);

            string strHeader = string.Empty;
            string strFoot = string.Empty;

            ClsSpread.setSpdPrint(this.ssOp, false, setMargin, setOption, strHeader, strFoot, Centering.Horizontal);

            if(ssOp2.ActiveSheet.Cells[11, 0].Text != "")
            {
                ComFunc.Delay(10);
                ClsSpread.setSpdPrint(this.ssOp2, false, setMargin, setOption, strHeader, strFoot, Centering.Horizontal);
            }
            if (ssOp3.ActiveSheet.Cells[11, 0].Text != "")
            {
                ComFunc.Delay(10);
                ClsSpread.setSpdPrint(this.ssOp3, false, setMargin, setOption, strHeader, strFoot, Centering.Horizontal);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Create : 안정수
        /// 풋노트를 읽어온다.
        /// </summary>
        /// <param name="argSpecno"></param>
        /// <returns></returns>
        public string Read_FootNote(string argSpecno)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT FOOTNOTE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_RESULTCF";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND SPECNO = '" + argSpecno + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    rtnVal += dt.Rows[i]["FOOTNOTE"].ToString().Trim() + " ";
                }
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ssOp.BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ssOp2.ActiveSheet.Cells[11, 0].Text != "")
            {
                ssOp2.BringToFront();
            }
         
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ssOp3.ActiveSheet.Cells[11, 0].Text != "")
            {
                ssOp3.BringToFront();
            }
        }
    }
}
