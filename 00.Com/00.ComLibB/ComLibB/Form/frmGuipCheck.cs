using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmGuipCheck.cs
    /// Description     : 최종구입 신고 후 1년경과 명세 조회 관련
    /// Author          : 안정수
    /// Create Date     : 2017-05
    /// Update History  : try - catch 여러개 사용한 부분 수정
    /// <history>       
    /// D:\타병원\PSMHH\basic\busuga\BuSuga22.frm(FrmGuipCheck) => frmGuipCheck.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\BuSuga22.frm(FrmGuipCheck)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>

    public partial class frmGuipCheck : Form
    {
        struct TABLE_EDI_SUGA
        {
            public string ROWID;
            public string Code;
            public string Jong;
            public string Pname;
            public string Bun;
            public string Danwi1;
            public string Danwi2;
            public string Spec;
            public string COMPNY;
            public string Effect;
            public string Gubun;
            public string Dangn;
            public string JDate1;
            public string Price1;
            public string JDate2;
            public string Price2;
            public string JDate3;
            public string Price3;
            public string JDate4;
            public string Price4;
            public string JDate5;
            public string Price5;
        }

        TABLE_EDI_SUGA TES = new TABLE_EDI_SUGA();

        void READ_EDI_SUGA(string ArgCode, string argSUNNEXT = "", string ArgJong = "")
        {
            //TABLE_EDI_SUGA TES = new TABLE_EDI_SUGA();

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  ROWID vROWID,Code vCode,Jong vJong,";
                SQL += ComNum.VBLF + "  Pname vPname,Bun vBun,Danwi1 vDanwi1,";
                SQL += ComNum.VBLF + "  Danwi2 vDanwi2,Spec vSpec,Compny vCompny,";
                SQL += ComNum.VBLF + "  Effect vEffect,Gubun vGubun,Dangn vDangn,";
                SQL += ComNum.VBLF + "  TO_CHAR(JDate1,'YYYY-MM-DD') vJDate1,Price1 vPrice1,";
                SQL += ComNum.VBLF + "  TO_CHAR(JDate2,'YYYY-MM-DD') vJDate2,Price2 vPrice2,";
                SQL += ComNum.VBLF + "  TO_CHAR(JDate3,'YYYY-MM-DD') vJDate3,Price3 vPrice3,";
                SQL += ComNum.VBLF + "  TO_CHAR(JDate4,'YYYY-MM-DD') vJDate4,Price4 vPrice4,";
                SQL += ComNum.VBLF + "  TO_CHAR(JDate5,'YYYY-MM-DD') vJDate5,Price5 vPrice5 ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "EDI_SUGA";
                SQL += ComNum.VBLF + "WHERE Code = '" + VB.Trim(ArgCode) + "' ";

                if (ArgJong != "")
                {
                    if (ArgJong == "8")
                    {
                        SQL = SQL + ComNum.VBLF + " AND Jong='8' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND Jong<>'8' ";
                    }
                }
                else
                {
                    switch (ArgCode)
                    {
                        case "N0041001":
                        case "N0041002":
                        case "N0041003":
                        case "N0021001":
                        case "30050010":
                        case "J5010001":
                        case "C2302005":
                        case "N0051010":
                            if (argSUNNEXT == VB.Trim(ArgCode))
                            {
                                SQL = SQL + ComNum.VBLF + " AND Jong='8' ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + " AND Jong<>'8' ";
                            }
                            break;
                        default:
                            break;
                    }
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                TES.ROWID = "";

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 1)
                {
                    TES.ROWID = dt.Rows[0]["vROWID"].ToString().Trim();
                    TES.Code = dt.Rows[0]["vCode"].ToString().Trim();
                    TES.Jong = dt.Rows[0]["vJong"].ToString().Trim();
                    TES.Pname = dt.Rows[0]["vPname"].ToString().Trim();
                    TES.Bun = dt.Rows[0]["vBun"].ToString().Trim();
                    TES.Danwi1 = dt.Rows[0]["vDanwi1"].ToString().Trim();
                    TES.Danwi2 = dt.Rows[0]["vDanwi2"].ToString().Trim();
                    TES.Spec = dt.Rows[0]["vSpec"].ToString().Trim();
                    TES.COMPNY = dt.Rows[0]["vCompny"].ToString().Trim();
                    TES.Effect = dt.Rows[0]["vEffect"].ToString().Trim();
                    TES.Gubun = dt.Rows[0]["vGubun"].ToString().Trim();
                    TES.Dangn = dt.Rows[0]["vDangn"].ToString().Trim();
                    TES.JDate1 = dt.Rows[0]["vJDate1"].ToString().Trim();
                    TES.Price1 = dt.Rows[0]["vPrice1"].ToString().Trim();
                    TES.JDate2 = dt.Rows[0]["vJDate2"].ToString().Trim();
                    TES.Price2 = dt.Rows[0]["vPrice2"].ToString().Trim();
                    TES.JDate3 = dt.Rows[0]["vJDate3"].ToString().Trim();
                    TES.Price3 = dt.Rows[0]["vPrice3"].ToString().Trim();
                    TES.JDate4 = dt.Rows[0]["vJDate4"].ToString().Trim();
                    TES.Price4 = dt.Rows[0]["vPrice4"].ToString().Trim();
                    TES.JDate5 = dt.Rows[0]["vJDate5"].ToString().Trim();
                    TES.Price5 = dt.Rows[0]["vPrice5"].ToString().Trim();
                }
                else
                {
                    TES.ROWID = "";
                    TES.Code = "";
                    TES.Jong = "";
                    TES.Pname = "";
                    TES.Bun = "";
                    TES.Danwi1 = "";
                    TES.Danwi2 = "";
                    TES.Spec = "";
                    TES.COMPNY = "";
                    TES.Effect = "";
                    TES.Gubun = "";
                    TES.Dangn = "";
                    TES.JDate1 = "";
                    TES.Price1 = "";
                    TES.JDate2 = "";
                    TES.Price2 = "";
                    TES.JDate3 = "";
                    TES.Price3 = "";
                    TES.JDate4 = "";
                    TES.Price4 = "";
                    TES.JDate5 = "";
                    TES.Price5 = "";
                }

                dt.Dispose();
                dt = null;

            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
        public frmGuipCheck()
        {
            InitializeComponent();
        }

        void frmGuipCheck_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SCREEN_CLEAR();
        }

        void SCREEN_CLEAR()
        {
            ssGuipCheck_Sheet1.RowCount = 0;
            ssGuipCheck_Sheet1.RowCount = 20;
            txtIlsu.Text = "100";
            btnView.Enabled = true;
            btnPrint.Enabled = false;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인
            GetData();
        }

        void GetData()
        {
            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            int i = 0;
            int nRow = 0;
            int nREAD = 0;

            string strDate = "";
            string strBCode = "";
            string strGDate = "";
            string strSDate = "";
            string strSuCode = "";
            string strOK = "";

            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = "";

            ComFunc CF = new ComFunc();

            btnPrint.Enabled = true;
            btnView.Enabled = false;

            strDate = CF.DATE_ADD(clsDB.DbCon, strSysDate, (int)VB.Val(txtIlsu.Text) * -1);

            try
            {
                if (opt1.Checked == true)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + " BCode,MAX(TO_CHAR(GDate,'YYYY-MM-DD')) GDate,MAX(TO_CHAR(SDate,'YYYY-MM-DD')) SDate,COUNT(*) CNT";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "EDI_GUIP";
                    SQL = SQL + ComNum.VBLF + " GROUP BY BCode ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY GDate,BCode ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    nREAD = dt.Rows.Count;
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strOK = "OK";
                            strBCode = dt.Rows[i]["BCode"].ToString().Trim();
                            strGDate = dt.Rows[i]["GDate"].ToString().Trim();
                            strSDate = dt.Rows[i]["SDATE"].ToString().Trim();
                            strSuCode = "";

                            // 구입일자가 1년전 이후이면
                            if (String.Compare(strGDate, strDate) > 0)
                            {
                                break;
                            }
                            if (strOK == "OK")
                            {
                                READ_EDI_SUGA(strBCode);
                            }
                            //표준코드 종류가 수입약, 재료대가 아니면
                            if (strOK == "OK" && TES.Jong != "8")
                            {
                                strOK = "NO";
                            }
                            //수가 삭제여부 체크 및 수가코드를 READ

                            if (strOK == "OK")
                            {
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "SELECT";
                                SQL = SQL + ComNum.VBLF + "     a.BCode vBCode,a.SuNext vSuNext,b.SuCode vSuCode";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN a, " + ComNum.DB_PMPA + "BAS_SUT b";
                                SQL = SQL + ComNum.VBLF + " WHERE (a.BCode='" + strBCode + "'  ";
                                SQL = SQL + ComNum.VBLF + "     OR  a.OldBCode='" + strBCode + "') ";
                                SQL = SQL + ComNum.VBLF + "     AND a.SuNext=b.SuNext(+) ";
                                SQL = SQL + ComNum.VBLF + "     AND b.DelDate IS NULL ";
                                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                                SQL = SQL + ComNum.VBLF + "SELECT";
                                SQL = SQL + ComNum.VBLF + "     a.BCode vBCode,a.SuNext vSuNext,b.SuCode vSuCode";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN a, " + ComNum.DB_PMPA + "BAS_SUH b";
                                SQL = SQL + ComNum.VBLF + " WHERE (a.BCode='" + strBCode + "'  ";
                                SQL = SQL + ComNum.VBLF + "     OR  a.OldBCode='" + strBCode + "') ";
                                SQL = SQL + ComNum.VBLF + "     AND a.SuNext=b.SuNext(+) ";
                                SQL = SQL + ComNum.VBLF + "     AND b.DelDate IS NULL ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }
                                if (dt2.Rows.Count == 0)
                                {                                    
                                    strOK = "NO";
                                }

                                else
                                {
                                    strSuCode = dt2.Rows[0]["vSuCode"].ToString().Trim();
                                }
                                dt2.Dispose();
                                dt2 = null;
                            }

                            if (strOK == "OK")
                            {
                                nRow = nRow + 1;

                                if (nRow > ssGuipCheck_Sheet1.RowCount)
                                {
                                    ssGuipCheck_Sheet1.RowCount = nRow;
                                }

                                ssGuipCheck_Sheet1.Cells[nRow - 1, 0].Text = strGDate;
                                ssGuipCheck_Sheet1.Cells[nRow - 1, 1].Text = strSDate;

                                ssGuipCheck_Sheet1.Cells[nRow - 1, 2].Text = strBCode;
                                ssGuipCheck_Sheet1.Cells[nRow - 1, 3].Text = strSuCode;

                                ssGuipCheck_Sheet1.Cells[nRow - 1, 4].Text = TES.Pname + " " + TES.Spec;
                                ssGuipCheck_Sheet1.Cells[nRow - 1, 5].Text = TES.COMPNY;

                                //마지막 구입수량, 금액을 READ

                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "SELECT";
                                SQL = SQL + ComNum.VBLF + "   Qty,Amt,Price";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "EDI_GUIP";
                                SQL = SQL + ComNum.VBLF + "WHERE BCODE = '" + strBCode + "'";
                                SQL = SQL + ComNum.VBLF + "AND GDate=TO_DATE('" + strGDate + "','YYYY-MM-DD')";
                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                if (dt2.Rows.Count > 0)
                                {
                                    ssGuipCheck_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:###,##0.0}", dt2.Rows[0]["Qty"]);
                                    ssGuipCheck_Sheet1.Cells[nRow - 1, 7].Text = String.Format("{0:###,###,###,##0}", dt2.Rows[0]["PRICE"]);
                                    ssGuipCheck_Sheet1.Cells[nRow - 1, 8].Text = String.Format("{0:###,###,###,##0}", dt2.Rows[0]["Amt"]);
                                }

                                dt2.Dispose();
                                dt2 = null;
                            }
                        }
                    }
                    

                    dt.Dispose();
                    dt = null;
                }

                if (opt2.Checked == true)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "   BUN,BCode, SuNext, SuCode";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "VIEW_SUGA_CODE";
                    SQL = SQL + ComNum.VBLF + "WHERE DELDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND BCODE NOT IN ( '000000' ,'999999','AAAAAA') ";
                    SQL = SQL + ComNum.VBLF + " AND EDIJONG IN ('4','8') ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY BUN,BCODE, SUNEXT, SUCODE ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY BUN,BCODE, SUNEXT, SUCODE ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    nREAD = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strOK = "OK";
                        strBCode = dt.Rows[i]["BCODE"].ToString().Trim();
                        strSuCode = dt.Rows[i]["SuCode"].ToString().Trim();

                        if (strOK == "OK")
                        {
                            READ_EDI_SUGA(strBCode);
                        }

                        if (strOK == "OK")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT";
                            SQL = SQL + ComNum.VBLF + "   MAX(TO_CHAR(GDate,'YYYY-MM-DD')) GDate,MAX(TO_CHAR(SDate,'YYYY-MM-DD')) SDate ";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "EDI_GUIP";
                            SQL = SQL + ComNum.VBLF + "WHERE BCODE = '" + strBCode + "'";
                            SQL = SQL + ComNum.VBLF + " GROUP BY BCode";
                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dt2.Rows.Count == 0)
                            {
                                dt2.Dispose();
                                dt2 = null;
                                ComFunc.MsgBox("해당 DATA가 없습니다.");
                                strOK = "DEL";
                                return;
                            }

                            else
                            {
                                strGDate = dt2.Rows[0]["GDATE"].ToString().Trim();
                                strSDate = dt2.Rows[0]["SDATE"].ToString().Trim();
                            }

                            dt2.Dispose();
                            dt2 = null;
                        }


                        //구입일자가 1년전 이후이면
                        if (String.Compare(strGDate, strDate) > 0)
                        {
                            strOK = "NO";
                        }

                        if (strOK == "OK" || strOK == "DEL")
                        {
                            nRow = nRow + 1;

                            if (nRow > ssGuipCheck_Sheet1.RowCount)
                            {
                                ssGuipCheck_Sheet1.RowCount = nRow;
                            }

                            if (strOK == "DEL")
                            {
                                ssGuipCheck_Sheet1.Cells.Get(nRow - 1, 0, nRow - 1, ssGuipCheck_Sheet1.ColumnCount - 1).ForeColor = Color.Blue;
                            }

                            ssGuipCheck_Sheet1.Cells[nRow - 1, 0].Text = strGDate;
                            ssGuipCheck_Sheet1.Cells[nRow - 1, 1].Text = strSDate;

                            ssGuipCheck_Sheet1.Cells[nRow - 1, 2].Text = strBCode;
                            ssGuipCheck_Sheet1.Cells[nRow - 1, 3].Text = strSuCode;

                            ssGuipCheck_Sheet1.Cells[nRow - 1, 4].Text = TES.Pname + " " + TES.Spec;
                            ssGuipCheck_Sheet1.Cells[nRow - 1, 5].Text = TES.COMPNY;

                            //마지막 구입수량, 금액을 READ
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT";
                            SQL = SQL + ComNum.VBLF + "   Qty,Amt,Price ";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "EDI_GUIP";
                            SQL = SQL + ComNum.VBLF + "WHERE BCODE = '" + strBCode + "'";
                            SQL = SQL + ComNum.VBLF + "AND GDate=TO_DATE('" + strGDate + "','YYYY-MM-DD')";
                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                ssGuipCheck_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:###,##0.0}", dt2.Rows[i]["Qty"]);
                                ssGuipCheck_Sheet1.Cells[nRow - 1, 7].Text = String.Format("{0:###,###,###,##0}", dt2.Rows[i]["PRICE"]);
                                ssGuipCheck_Sheet1.Cells[nRow - 1, 8].Text = String.Format("{0:###,###,###,##0}", dt2.Rows[i]["Amt"]);
                            }
                            dt2.Dispose();
                            dt2 = null;
                        }
                        
                    }

                    dt.Dispose();
                    dt = null;
                    CF = null;

                    Cursor.Current = Cursors.Default;
                }

            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void txtIlsu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnView.Focus();
            }
        }

        void ssGuipCheck_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row != 0)
            {
                return;
            }

            ssGuipCheck_Sheet1.Cells[0, 0, ssGuipCheck_Sheet1.RowCount - 1, ssGuipCheck_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssGuipCheck_Sheet1.Cells[e.Row, 0, e.Row, ssGuipCheck_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strHead1 = "/l/f1" + VB.Space(19) + "최종 구입신고후 1년경과 명세" + "/n";

            strFont2 = "/fn\"굴림체\" /fz\"11\" /fs2";
            strHead2 = "/l/f2" + "인쇄일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            strHead2 = strHead2 + VB.Space(50) + "PAGE : /p";

            ssGuipCheck_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;

            ssGuipCheck_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;

            ssGuipCheck_Sheet1.PrintInfo.Margin.Top = 50;
            ssGuipCheck_Sheet1.PrintInfo.Margin.Bottom = 2000;
            ssGuipCheck_Sheet1.PrintInfo.Margin.Left = 0;
            ssGuipCheck_Sheet1.PrintInfo.Margin.Right = 0;

            ssGuipCheck_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssGuipCheck_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;

            ssGuipCheck_Sheet1.PrintInfo.ShowBorder = true;
            ssGuipCheck_Sheet1.PrintInfo.ShowColor = false;
            ssGuipCheck_Sheet1.PrintInfo.ShowGrid = true;
            ssGuipCheck_Sheet1.PrintInfo.ShowShadows = false;
            ssGuipCheck_Sheet1.PrintInfo.UseMax = false;
            ssGuipCheck_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssGuipCheck_Sheet1.PrintInfo.Preview = true;
            ssGuipCheck.PrintSheet(0);
        }
    }
}
