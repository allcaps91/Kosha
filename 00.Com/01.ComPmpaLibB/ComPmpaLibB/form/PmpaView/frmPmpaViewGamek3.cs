using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewGamek3.cs
    /// Description     : 독감접종 감액 (BOFLU-A, BOFLU-P)
    /// Author          : 안정수
    /// Create Date     : 2017-09-04
    /// Update History  : 2017-11-03
    /// <history>           
    /// d:\psmh\OPD\olrepa\Frm감액조회3.frm(Frm감액조회3) => frmPmpaViewGamek3.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm감액조회3.frm(Frm감액조회3)
    /// </seealso>
    /// </summary> 
    public partial class frmPmpaViewGamek3 : Form
    {
        string mstrJobName = "";
        ComFunc CF = new ComFunc();
        public frmPmpaViewGamek3()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewGamek3(string GstrJobName)
        {
            InitializeComponent();
            setEvent();
            mstrJobName = GstrJobName;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등             

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            txtPInfo.Text = "";


        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
             //   if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
             //   {
             //       return; //권한 확인
             //   }
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
             //   if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
             //   {
              //      return; //권한 확인
              //  }
                ePrint();
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ssList_Sheet1.Rows.Count == 0)
            {
                return;
            }

            string JobDate = dtpFDate.Text;
            string JobMan = mstrJobName;

            //Print Head 지정
            strTitle = "감  액  명  세  서( 신부님 )";

            if (JobMan != "")
            {
                strSubTitle = "작성자" + JobMan + "\r\n" + "작업일자: " + JobDate;
            }

            else
            {
                strSubTitle = "작업일자: " + JobDate;
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 120, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, true, false, false, true, false, true, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void eGetData()
        {
            int i = 0;
            int nRow = 0;
            int nRead = 0;

            int nCnt1 = 0;
            int nCnt2 = 0;

            int nSum1 = 0;
            int nSum2 = 0;

            string strNew = "";
            string strOld = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            nRow = 0;

            nCnt1 = 0;
            nCnt2 = 0;
            nSum1 = 0;
            nSum2 = 0;

            txtPInfo.Text = "";
            ssList_Sheet1.Rows.Count = 0;
            Cursor.Current = Cursors.WaitCursor;


            //외래
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                    ";
            SQL += ComNum.VBLF + "  a.Pano,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,a.DeptCode,b.SName,                                          ";
            SQL += ComNum.VBLF + "  SUM(DECODE(a.Bun,'92',a.Amt1+a.Amt2,0)) GamAmt,                                                         ";
            SQL += ComNum.VBLF + "  SUM(DECODE(a.Bun,'92',a.Amt1+a.Amt2,'96',a.Amt1+a.Amt2,'98',a.Amt1+a.Amt2,'99',a.Amt1+a.Amt2,0)) tAmt   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP a, " + ComNum.DB_PMPA + "BAS_PATIENT b                                ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
            SQL += ComNum.VBLF + "      AND ( a.Pano,a.BDate,a.Deptcode) IN (                                                               ";
            SQL += ComNum.VBLF + "                                          SELECT Ptno,BDate,DeptCode                                      ";
            SQL += ComNum.VBLF + "                                          FROM " + ComNum.DB_MED + "OCS_OORDER                            ";
            SQL += ComNum.VBLF + "                                          WHERE 1=1                                                       ";
            SQL += ComNum.VBLF + "                                              AND BDate >=TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "                                              AND BDate <=TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "                                              AND SUCODE IN ('BOFLU-A','BOFLU-P') )                       ";
            SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                                                ";
            SQL += ComNum.VBLF + "      AND a.Bun > '90'                                                                                    ";
            SQL += ComNum.VBLF + "GROUP BY a.Pano,TO_CHAR(a.BDate,'YYYY-MM-DD'),a.DeptCode,b.SName                                          ";
            SQL += ComNum.VBLF + "ORDER BY TO_CHAR(a.BDate,'YYYY-MM-DD'),a.DeptCode,b.SName,a.Pano                                          ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    nCnt1 = dt.Rows.Count;

                    ssList_Sheet1.Rows.Count = nRead;

                    for (i = 0; i < nRead; i++)
                    {
                        nRow += 1;

                        ssList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 4].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["tAmt"].ToString().Trim()));
                        ssList_Sheet1.Cells[nRow - 1, 5].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["GamAmt"].ToString().Trim()));

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                                                    ";
                        SQL += ComNum.VBLF + " GbFlu_Ltd,GbGamek                                                                        ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                     ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                        SQL += ComNum.VBLF + "      AND PANO ='" + dt.Rows[i]["Pano"].ToString().Trim() + "'                            ";
                        SQL += ComNum.VBLF + "      AND BDate = TO_DATE('" + dt.Rows[i]["BDate"].ToString().Trim() + "','YYYY-MM-DD')   ";
                        SQL += ComNum.VBLF + "      AND DeptCode ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'                    ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        //if (dt1.Rows.Count == 0)
                        //{
                        //    dt1.Dispose();
                        //    dt1 = null;
                        //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                        //    return;
                        //}

                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["GbFlu_Ltd"].ToString().Trim() == "Y")
                            {
                                ssList_Sheet1.Cells[nRow - 1, 6].Text = "회사예방접종";
                                nCnt2 += 1;
                            }
                            else
                            {
                                ssList_Sheet1.Cells[nRow - 1, 6].Text = Bas_Gamek_Name(dt1.Rows[0]["GbGamek"].ToString().Trim());
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;

                        nSum1 += Convert.ToInt32(VB.Val(dt.Rows[i]["tAmt"].ToString().Trim()));
                        nSum2 += Convert.ToInt32(VB.Val(dt.Rows[i]["GamAmt"].ToString().Trim()));

                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            ssList_Sheet1.Rows.Count += 1;

            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = "합계";
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = String.Format("{0:###,###,###,##0}", nSum1);
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = String.Format("{0:###,###,###,##0}", nSum2);

            txtCount.Text = "총 건수: " + nCnt1 + "건, 회사접종: " + nCnt2 + "건";
            txtPInfo.Text = "외래 건수: " + (nRow) + "    총액:" + String.Format("{0:###,###,###,##0}", nSum1) + "    감액:" + String.Format("{0:###,###,###,##0}", nSum2);
            Cursor.Current = Cursors.Default;
        }

        public string Bas_Gamek_Name(string strGamek)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                ";
            SQL += ComNum.VBLF + "  NAME                                ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE  ";
            SQL += ComNum.VBLF + "WHERE 1=1                             ";
            SQL += ComNum.VBLF + "      AND GUBUN = 'BAS_감액코드명'    ";
            SQL += ComNum.VBLF + "      AND CODE = '" + strGamek + "'   ";

            try
            {

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    rtnVal = "감액무";
                    return rtnVal;
                }

                else
                {
                    rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        void dtpFDate_ValueChanged(object sender, EventArgs e)
        {
            dtpTDate.Text = CF.READ_LASTDAY(clsDB.DbCon, dtpFDate.Text);
        }
    }
}
