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
    public partial class FrmGumeCodeCheck : Form
    {
        string GstrHelpCode = "";

        public FrmGumeCodeCheck()
        {
            InitializeComponent();
        }

        public FrmGumeCodeCheck(string strHelpCode)
        {
            InitializeComponent();
            GstrHelpCode = strHelpCode;
        }

        private void FrmGumeCodeCheck_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            int nRow = 0;
            int nREAD = 0;
            string strOK = "";
            string strSuCode = "";
            string strBCode = "";
            string strGbSunap = "";
            string strERROR = "";
            double dblGumePrice = 0;

            double dblSuga_BAmt = 0;
            string strSuga_DelDate = "";
            string strSuga_BCode = "";
            string strSuga_SugbN = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "SELECT JepCode,JepName,Buse_Unit,Buse_Gesu,TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,";
                SQL = SQL + ComNum.VBLF + "  GbSunap,Price,SuCode,BCode,GbReUse,GbExchange ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "ORD_JEP ";
                SQL = SQL + ComNum.VBLF + " WHERE JepCode IN (SELECT JepCode FROM " + ComNum.DB_PMPA + "OPR_BuseJepum ";
                SQL = SQL + ComNum.VBLF + "       WHERE CodeGbn='2' ";     //관리과물품
                SQL = SQL + ComNum.VBLF + "       GROUP BY JepCode) ";
                SQL = SQL + ComNum.VBLF + "    OR GbOpLental='Y' ";

                if (rdoOut.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND DelDate IS NULL ";
                }

                if (rdoSunap.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GBSUNAP ='1'";
                }

                if (rdoSSunap.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GBSUNAP ='2'";
                }

                if (rdoNSunap.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GBSUNAP ='3'";
                }

                if (rdoCode.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY JepCode";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY JepName";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nREAD = dt.Rows.Count;
                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strSuCode = dt.Rows[i]["SuCode"].ToString().Trim();
                    strBCode = dt.Rows[i]["BCode"].ToString().Trim();
                    strGbSunap = dt.Rows[i]["GbSunap"].ToString().Trim();

                    strERROR = "";

                    //최종 구매단가를 READ
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT LastDate,Price,CovQty ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "ORD_HIS";
                    SQL = SQL + ComNum.VBLF + " WHERE JepCode='" + dt.Rows[i]["JepCode"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY LastDate DESC ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    dblGumePrice = 0;

                    if (dt1.Rows.Count > 0)
                    {
                        dblGumePrice = VB.Val(dt1.Rows[0]["Price"].ToString().Trim());
                        if (VB.Val(dt1.Rows[0]["CovQty"].ToString().Trim()) > 0)
                        {
                            dblGumePrice = dblGumePrice / VB.Val(dt1.Rows[0]["CovQty"].ToString().Trim());
                            if (VB.Val(dt.Rows[i]["Buse_Gesu"].ToString().Trim()) > 0)
                            {
                                dblGumePrice = dblGumePrice / VB.Val(dt.Rows[i]["Buse_Gesu"].ToString().Trim());
                            }
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //수가코드를 읽음
                    dblSuga_BAmt = 0;
                    strSuga_DelDate = "";
                    strSuga_BCode = "";

                    if (strSuCode != "")
                    {
                        SQL = "";
                        SQL = "SELECT a.BAmt,TO_CHAR(a.DelDate,'YYYY-MM-DD') DelDate,b.BCode,b.SugbN ";
                        SQL = SQL + ComNum.VBLF +"  FROM " + ComNum.DB_PMPA + "BAS_SUT a, " + ComNum.DB_PMPA + "BAS_SUN b ";
                        SQL = SQL + ComNum.VBLF +" WHERE a.SuCode='" + strSuCode + "' ";
                        SQL = SQL + ComNum.VBLF +"   AND a.SuNext=b.SuNext(+) ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (dt1.Rows.Count == 0)
                        {
                            strERROR = strERROR + "수가없음, ";
                        }
                        else
                        {
                            dblSuga_BAmt = VB.Val(dt1.Rows[0]["BAmt"].ToString().Trim());
                            strSuga_DelDate = dt1.Rows[0]["DelDate"].ToString().Trim();
                            strSuga_BCode = dt1.Rows[0]["BCode"].ToString().Trim();
                            strSuga_SugbN = dt1.Rows[0]["SugbN"].ToString().Trim();
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }

                    //수납구분이 수납, 선수납인 물품코드가 수가코드가 없으면 오류
                    if (dt.Rows[i]["GbSunap"].ToString().Trim() != "3" && strSuCode == "")
                    {
                        strERROR = strERROR + "수가누락, ";
                    }

                    //수납구분이 수납, 선수납, 수불이 아닌경우 오류처리
                    if (dt.Rows[i]["GbSunap"].ToString().Trim() != "1" || VB.Val(dt.Rows[i]["GbSunap"].ToString().Trim()) > 3)
                    {
                        strERROR = strERROR + "수납구분, ";
                    }

                    //삭제일자 오류 점검
                    if (dt.Rows[i]["DelDate"].ToString().Trim() == "" && strSuga_DelDate != "")
                    {
                        strERROR = strERROR + "수가삭제, ";
                    }

                    //표준코드, 선수나 오류점검
                    if (strSuCode != "")
                    {
                        if (strBCode != "")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_ERP + "ORD_JEP";
                            SQL = SQL + ComNum.VBLF + "   SET BCODE='" + strSuga_BCode + "' ";
                            SQL = SQL + ComNum.VBLF + " WHERE JepCode='" + dt.Rows[i]["JepCode"].ToString().Trim() + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);         //해당 트랜젝션을 바로 실행하기
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }

                        if (strSuga_SugbN == "1" && strGbSunap != "2")
                        {
                            strERROR = strERROR + "선수납, ";
                        }
                        else if(strGbSunap == "2" && strSuga_SugbN != "1")
                        {
                            strERROR = strERROR + "선수납, ";
                        }
                    }

                    //단가오류점검
                    if (strSuCode != "" && chkDanga.Checked == true)
                    {
                        if (dblSuga_BAmt != dblGumePrice)
                        {
                            strERROR = strERROR + "단가차이, ";
                        }
                    }

                    //수불 수가점검(구매코드는 수불로 되어있으나 수가코드가 등록된것)
                    if (chkSuga.Checked == true)
                    {
                        if (strGbSunap == "3" && strSuCode != "")
                        {
                            strERROR = strERROR + "수불수가, ";
                        }
                    }

                    strOK = "NO";

                    if (rdoAll.Checked == true)
                    {
                        strOK = "OK";
                    }

                    if (rdoError.Checked == true && strERROR != "")
                    {
                        strOK = "OK";
                    }

                    if (strOK == "OK")
                    {
                        nRow = nRow + 1;

                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }

                        ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["JepCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = " " + dt.Rows[i]["JepName"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = " " + dt.Rows[i]["Buse_Unit"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(dblGumePrice, "###,###,##0 ");
                        
                        switch(dt.Rows[i]["GbSunap"].ToString().Trim())
                        {
                            case "1":
                                ssView_Sheet1.Cells[nRow - 1, 4].Text = "수납";
                                break;

                            case "2":
                                ssView_Sheet1.Cells[nRow - 1, 4].Text = "선수";
                                break;

                            case "3":
                                ssView_Sheet1.Cells[nRow - 1, 4].Text = "안함";
                                break;

                            default:
                                ssView_Sheet1.Cells[nRow - 1, 4].Text = "??";
                                break;
                        }

                        if (dt.Rows[i]["DelDate"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = "Y";
                        }

                        //수가내역
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = strSuCode;
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Format(dblSuga_BAmt, "###,###,###");

                        if (strSuga_DelDate != "")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = "Y";
                        }

                        ssView_Sheet1.Cells[nRow - 1, 10].Text = strSuga_BCode;

                        if (dt.Rows[i]["GbReUse"].ToString().Trim() == "Y")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 11].Text = "Y";
                        }

                        if (dt.Rows[i]["GbExchange"].ToString().Trim() == "1")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 12].Text = "Y";
                        }

                        ssView_Sheet1.Cells[nRow - 1, 13].Text = strERROR;
                    }

                }
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = nRow;
                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "물품코드 점검 LIST" + "/f1/n";
            strHead2 = "/l/f2" + "인쇄일자 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + " / "
                 + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":") + "/f2/n";

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            //ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 20;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.Preview = false;
            ssView.PrintSheet(0);
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column < 8)
            {
                GstrHelpCode = ssView_Sheet1.Cells[e.Row, 0].Text;

                frmGumeUpdate frmGumeUpdateX = new frmGumeUpdate(GstrHelpCode);
                frmGumeUpdateX.ShowDialog();

            }
        }
    }
}
