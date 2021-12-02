using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmViewGoji.cs
    /// Description     : 특정약제(고지혈증) 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-05
    /// Update History  : try-catch문 수정, 쓰레드 적용
    /// <history>       
    /// D:\타병원\PSMHH\basic\busuga\BuSuga27.frm(FrmViewGoji) => frmViewGoji.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\BuSuga27.frm(FrmViewGoji)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmViewGoji : Form
    {
        Thread thread;
        public frmViewGoji()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.btnSearch.Click += new EventHandler(eBtnClick);
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            thread = new Thread(reTrive);
            thread.Start();
        }

        void reTrive()
        {
            this.Invoke(new threadProcessDelegate(runCircular), new object[] { true });

            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));

            //실제 쿼리부분
            
            DataTable dt = null;
           
            string SQL = "";
            string SqlErr = "";
            ssViewGoji_Sheet1.RowCount = 0;

            if (dtpFDate.Text == "" || dtpTDate.Text == "")
            {
                ComFunc.MsgBox("조회날짜가 오류입니다.");
            }

            //외래 처방 Read
            try
            {
                if (optPRN.Checked == true)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "   TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE  ,  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, ";
                    SQL = SQL + ComNum.VBLF + "    A.PANO, B.SNAME, A.DEPTCODE,  A.SUNEXT, C.SUNAMEK, SUM(A.QTY *A.NAL) QTY , A.GBSELF ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_PMPA + "BAS_SUN C";
                    SQL = SQL + ComNum.VBLF + "   WHERE 1 = 1 ";
                    SQL = SQL + ComNum.VBLF + "     AND  A.ACTDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND A.SUNEXT = C.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "     AND C.GBGOJI ='Y' ";
                    SQL = SQL + ComNum.VBLF + "     AND A.PANO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.ACTDATE, A.BDATE, A.PANO, B.SNAME, A.DEPTCODE,  A.SUNEXT, C.SUNAMEK, A.GBSELF ";
                    SQL = SQL + ComNum.VBLF + "HAVING SUM(QTY * NAL ) <> 0  ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY 1, 2 ";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "   TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE  ,  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, ";
                    SQL = SQL + ComNum.VBLF + "    A.PANO, B.SNAME, A.DEPTCODE,  A.SUNEXT, C.SUNAMEK, SUM(A.QTY *A.NAL) QTY , A.GBSELF ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_PMPA + "BAS_SUN C";
                    SQL = SQL + ComNum.VBLF + "   WHERE 1 = 1 ";
                    SQL = SQL + ComNum.VBLF + "     AND  A.ACTDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND A.SUNEXT = C.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "     AND C.GBGOJI ='Y' ";
                    SQL = SQL + ComNum.VBLF + "     AND A.PANO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.ACTDATE, A.BDATE, A.PANO, B.SNAME, A.DEPTCODE,  A.SUNEXT, C.SUNAMEK, A.GBSELF ";
                    SQL = SQL + ComNum.VBLF + "HAVING SUM(QTY * NAL ) <> 0  ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY 1, 2 ";
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }                
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            this.Invoke(new threadSpdTypeDelegate(disPlaySpread), dt);
            this.Invoke(new threadProcessDelegate(runCircular), new object[] { false });
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));

        }

        delegate void threadSpdTypeDelegate(DataTable dt);

        //스프레드에 뿌려주는 부분
        void disPlaySpread(DataTable dt)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";
            DataTable dt2 = null;
            
            ssViewGoji_Sheet1.RowCount = dt.Rows.Count;

            for (i = 0; i < ssViewGoji_Sheet1.RowCount; i++)
            {
                ssViewGoji_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDate"].ToString().Trim() + "";
                ssViewGoji_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDate"].ToString().Trim() + "";
                ssViewGoji_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim() + "";
                ssViewGoji_Sheet1.Cells[i, 3].Text = dt.Rows[i]["sName"].ToString().Trim() + "";
                ssViewGoji_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SuNext"].ToString().Trim() + "";
                ssViewGoji_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SuNameK"].ToString().Trim() + "";
                ssViewGoji_Sheet1.Cells[i, 6].Text = dt.Rows[i]["QTY"].ToString().Trim() + "";

                rtnVal = dt.Rows[i]["GBSELF"].ToString().Trim() + "";
                if (rtnVal == "0")
                {
                    ssViewGoji_Sheet1.Cells[i, 7].Text = "급여";
                }
                else
                {
                    ssViewGoji_Sheet1.Cells[i, 7].Text = "비급여";
                }

                if (optPRN.Checked == true)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "   CHOJAE";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                    SQL = SQL + ComNum.VBLF + "   WHERE 1 = 1 ";
                    SQL = SQL + ComNum.VBLF + "     AND PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "" + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND BDATE = TO_DATE('" + dt.Rows[i]["BDate"].ToString().Trim() + "" + "' ,'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND DEPTCODE = '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "" + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt2.Rows[0]["CHOJAE"].ToString().Trim() + "" == "1" || dt2.Rows[0]["CHOJAE"].ToString().Trim() + "" == "2")
                    {
                        ssViewGoji_Sheet1.Cells[i, 8].Text = "◎";
                    }

                    dt2.Dispose();
                    dt2 = null;
                }
            }
        }

        delegate void threadProcessDelegate(bool b);

        void runCircular(bool b)
        {
            this.barProgress.Visible = b;
            this.barProgress.IsRunning = b;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmViewGoji_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            optPRN.Checked = true;

            dtpFDate.Text = DATE_ADD(DateTime.Now.Date.ToString("yyyy-MM-dd"), -7);
            dtpTDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");

            SCREEN_CLEAR();
        }

        string DATE_ADD(string ArDate, int ArgIlsu)
        {
            DataTable dt = null;

            string SQL = string.Empty;
            string SqlErr = "";
            string rntVal = "";

            if(VB.Len(ArDate) != 10)
            {
                rntVal = "";
                return rntVal;
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT";
            SQL = SQL + ComNum.VBLF + "   TO_CHAR(TO_DATE('" + ArDate + "','YYYY-MM-DD')";
            if (ArgIlsu < 0)
            {
                SQL = SQL + ComNum.VBLF + "-" + (ArgIlsu * -1);
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "+" + ArgIlsu;
            }
            SQL = SQL + ComNum.VBLF + ",'YYYY-MM-DD') AddDate FROM DUAL";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }

            if (dt.Rows.Count == 1)
            {
                rntVal = dt.Rows[0]["AddDate"].ToString().Trim();
            }
            else
                rntVal = "";

            dt.Dispose();
            dt = null;
            return rntVal;
        }

        void SCREEN_CLEAR()
        {
            ssViewGoji_Sheet1.RowCount = 20;
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            string strHead1 = "";
            string strHead2 = "";

            string strFont1 = "";
            string strFont2 = "";
            
            string PrintDate = "";
            string JobMan = "";

            //JobMan = GstrJobName;

            PrintDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            //Print Head 지정
            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs0 ";
            strFont2 = "/fn\"바탕체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs0";

            strHead1 = "/c/f1" + "고지혈증약제 처방조회";
            strHead2 = "/l/f2" + "작업기간 : " + dtpFDate.Text + " ==> " + dtpTDate.Text + VB.Space(20);
            strHead2 = strHead2 + "/r/f2" + "인쇄일자 : " + PrintDate + "/n";

            if(optPRN.Checked == true)
            {
                strHead2 = strHead2 + "/l/f2" + "조회구분 : 외래 ";
            }
            else
            {
                strHead2 = strHead2 + "/l/f2" + "조회구분 : 입원 ";
            }

            ssViewGoji_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;

            ssViewGoji_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;

            ssViewGoji_Sheet1.PrintInfo.Margin.Top = 50;
            ssViewGoji_Sheet1.PrintInfo.Margin.Bottom = 2000;
            ssViewGoji_Sheet1.PrintInfo.Margin.Left = 0;
            ssViewGoji_Sheet1.PrintInfo.Margin.Right = 0;

            ssViewGoji_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssViewGoji_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;

            ssViewGoji_Sheet1.PrintInfo.ShowBorder = true;
            ssViewGoji_Sheet1.PrintInfo.ShowColor = false;
            ssViewGoji_Sheet1.PrintInfo.ShowGrid = true;
            ssViewGoji_Sheet1.PrintInfo.ShowShadows = false;
            ssViewGoji_Sheet1.PrintInfo.UseMax = false;
            ssViewGoji_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssViewGoji_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssViewGoji_Sheet1.PrintInfo.Preview = true;
            ssViewGoji.PrintSheet(0);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            //GetData();
        }

        void GetData()
        {
            int i = 0;

            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            ssViewGoji_Sheet1.RowCount = 0;

            if(dtpFDate.Text == "" || dtpTDate.Text == "")
            {
                ComFunc.MsgBox("조회날짜가 오류입니다.");
            }

            //외래 처방 Read
            try
            {
                if (optPRN.Checked == true)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "   TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE  ,  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, ";
                    SQL = SQL + ComNum.VBLF + "    A.PANO, B.SNAME, A.DEPTCODE,  A.SUNEXT, C.SUNAMEK, SUM(A.QTY *A.NAL) QTY , A.GBSELF ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_PMPA + "BAS_SUN C";
                    SQL = SQL + ComNum.VBLF + "   WHERE 1 = 1 ";
                    SQL = SQL + ComNum.VBLF + "     AND  A.ACTDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND A.SUNEXT = C.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "     AND C.GBGOJI ='Y' ";
                    SQL = SQL + ComNum.VBLF + "     AND A.PANO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.ACTDATE, A.BDATE, A.PANO, B.SNAME, A.DEPTCODE,  A.SUNEXT, C.SUNAMEK, A.GBSELF ";
                    SQL = SQL + ComNum.VBLF + "HAVING SUM(QTY * NAL ) <> 0  ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY 1, 2 ";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "   TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE  ,  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, ";
                    SQL = SQL + ComNum.VBLF + "    A.PANO, B.SNAME, A.DEPTCODE,  A.SUNEXT, C.SUNAMEK, SUM(A.QTY *A.NAL) QTY , A.GBSELF ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_PMPA + "BAS_SUN C";
                    SQL = SQL + ComNum.VBLF + "   WHERE 1 = 1 ";
                    SQL = SQL + ComNum.VBLF + "     AND  A.ACTDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND A.SUNEXT = C.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "     AND C.GBGOJI ='Y' ";
                    SQL = SQL + ComNum.VBLF + "     AND A.PANO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.ACTDATE, A.BDATE, A.PANO, B.SNAME, A.DEPTCODE,  A.SUNEXT, C.SUNAMEK, A.GBSELF ";
                    SQL = SQL + ComNum.VBLF + "HAVING SUM(QTY * NAL ) <> 0  ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY 1, 2 ";
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssViewGoji_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssViewGoji_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDate"].ToString().Trim() + "";
                    ssViewGoji_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDate"].ToString().Trim() + "";
                    ssViewGoji_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim() + "";
                    ssViewGoji_Sheet1.Cells[i, 3].Text = dt.Rows[i]["sName"].ToString().Trim() + "";
                    ssViewGoji_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SuNext"].ToString().Trim() + "";
                    ssViewGoji_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SuNameK"].ToString().Trim() + "";
                    ssViewGoji_Sheet1.Cells[i, 6].Text = dt.Rows[i]["QTY"].ToString().Trim() + "";

                    rtnVal = dt.Rows[i]["GBSELF"].ToString().Trim() + "";
                    if (rtnVal == "0")
                    {
                        ssViewGoji_Sheet1.Cells[i, 7].Text = "급여";
                    }
                    else
                    {
                        ssViewGoji_Sheet1.Cells[i, 7].Text = "비급여";
                    }

                    if (optPRN.Checked == true)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT";
                        SQL = SQL + ComNum.VBLF + "   CHOJAE";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                        SQL = SQL + ComNum.VBLF + "   WHERE 1 = 1 ";
                        SQL = SQL + ComNum.VBLF + "     AND PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "" + "' ";
                        SQL = SQL + ComNum.VBLF + "     AND BDATE = TO_DATE('" + dt.Rows[i]["BDate"].ToString().Trim() + "" + "' ,'YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND DEPTCODE = '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "" + "' ";
                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt2.Rows[0]["CHOJAE"].ToString().Trim() + "" == "1" || dt2.Rows[0]["CHOJAE"].ToString().Trim() + "" == "2")
                        {
                            ssViewGoji_Sheet1.Cells[i, 8].Text = "◎";
                        }

                        dt2.Dispose();
                        dt2 = null;

                    }
                }

                dt.Dispose();
                dt = null;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }
    }
}
