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
    /// File Name       : frmPmpaViewScheduledJinFinish.cs
    /// Description     : 예약접수비 일별 마감 조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-19
    /// Update History  : 2017-11-06
    /// <history>   
    /// 사용 안하는것으로 판단됨, 실제사용여부 확인 필요
    /// d:\psmh\OPD\olrepa\Frm예약진찰마감.frm(Frm예약진찰마감) => frmPmpaViewScheduledJinFinish.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm예약진찰마감.frm(Frm예약진찰마감)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewScheduledJinFinish : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        string mstrPassId = "";
        int mnJobSabun = 0;

        public frmPmpaViewScheduledJinFinish()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewScheduledJinFinish(string FstrPassId)
        {
            InitializeComponent();
            setEvent();
            mstrPassId = FstrPassId;
        }

        public frmPmpaViewScheduledJinFinish(int GnJobSabun)
        {
            InitializeComponent();
            setEvent();
            mnJobSabun = GnJobSabun;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);

            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnView2.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
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

            optGubun0.Checked = true;

            txtPart.Text = clsType.User.Sabun; 

            if (mnJobSabun != 0)
            {
                txtPart.Text = ComFunc.SetAutoZero(mnJobSabun.ToString(), 5);
            }

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnView2)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData2();
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
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


            strTitle = "( " + dtpDate.Text + " ) 예약접수비 수납,대체금액 집계표";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            if (mstrPassId != "")
            {
                strSubTitle = "작성자:" + CF.READ_PassName(clsDB.DbCon, ComFunc.SetAutoZero(mstrPassId, 5));

                strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);
            }

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 155, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList1, PrePrint, setMargin, setOption, strHeader, strFooter);            
        }

        void eGetData()
        {
            string strPart = "";

            int i = 0;
            int nSum = 0;
            int nSum2 = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            nSum = 0;
            nSum2 = 0;

            ssList1_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                    ";
            SQL += ComNum.VBLF + "  a.PART Part                                                             ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW a                             ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
            SQL += ComNum.VBLF + "      AND a.Date1 =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')           ";
            SQL += ComNum.VBLF + "GROUP BY a.PART                                                           ";

            SQL += ComNum.VBLF + "UNION                                                                     ";

            SQL += ComNum.VBLF + "SELECT                                                                    ";
            SQL += ComNum.VBLF + "  a.PART Part                                                             ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW a                             ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
            SQL += ComNum.VBLF + "      AND TRUNC(a.TransDATE) =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "GROUP BY a.PART                                                           ";

            SQL += ComNum.VBLF + "UNION                                                                     ";

            SQL += ComNum.VBLF + "SELECT                                                                    ";
            SQL += ComNum.VBLF + "  a.RETPART Part                                                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW a                             ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
            SQL += ComNum.VBLF + "      AND a.RETDATE >=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')        ";
            SQL += ComNum.VBLF + "      AND a.RETDATE < TO_DATE('" +
                  Convert.ToDateTime(dtpDate.Text).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')        ";
            SQL += ComNum.VBLF + "GROUP BY a.RETPART                                                       ";
            SQL += ComNum.VBLF + "ORDER BY 1                                                                ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            nSum = 0;

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당하는 예약 대체금액이 없습니다.");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ssList1_Sheet1.Rows.Count = dt.Rows.Count + 1;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strPart = dt.Rows[i]["PART"].ToString().Trim();

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                    ";
                    SQL += ComNum.VBLF + "  a.PART,SUM(a.Amt7) Amt7                                                 ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW a                             ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
                    SQL += ComNum.VBLF + "      AND a.DATE1 =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')           ";
                    SQL += ComNum.VBLF + "      AND a.Part ='" + strPart + "'                                       ";
                    SQL += ComNum.VBLF + "GROUP BY a.PART                                                           ";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssList1_Sheet1.Cells[i, 0].Text = dt1.Rows[0]["PART"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 1].Text = CF.READ_PassName(clsDB.DbCon, dt1.Rows[0]["PART"].ToString().Trim());
                        ssList1_Sheet1.Cells[i, 2].Text = String.Format("{0:###,###,##0}", VB.Val(dt1.Rows[0]["Amt7"].ToString().Trim()));
                        nSum += Convert.ToInt32(VB.Val(dt1.Rows[0]["Amt7"].ToString().Trim()));
                    }

                    dt1.Dispose();
                    dt1 = null;

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                    ";
                    SQL += ComNum.VBLF + "  a.RETPART,SUM(a.RETAMT) Amt7                                            ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW a                             ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
                    SQL += ComNum.VBLF + "      AND TRUNC(a.RETDATE) =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')  ";
                    SQL += ComNum.VBLF + "      AND a.RETPart ='" + strPart + "'                                    ";
                    SQL += ComNum.VBLF + "GROUP BY a.RETPART                                                        ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssList1_Sheet1.Cells[i, 0].Text = dt1.Rows[0]["RETPART"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 1].Text = CF.READ_PassName(clsDB.DbCon, dt1.Rows[0]["RETPART"].ToString().Trim());
                        ssList1_Sheet1.Cells[i, 2].Text = String.Format("{0:###,###,##0}", VB.Val(dt1.Rows[0]["Amt7"].ToString().Trim()));
                        nSum += Convert.ToInt32(VB.Val(dt1.Rows[0]["Amt7"].ToString().Trim()));
                    }

                    dt1.Dispose();
                    dt1 = null;


                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                        ";
                    SQL += ComNum.VBLF + "  a.PART,SUM(a.TransAmt) TransAmt                                             ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW a                                 ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                    SQL += ComNum.VBLF + "      AND TRUNC(a.TransDATE) =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')    ";
                    SQL += ComNum.VBLF + "      AND a.Part ='" + strPart + "'                                           ";
                    SQL += ComNum.VBLF + "      AND a.RETDATE IS NULL                                                   ";
                    SQL += ComNum.VBLF + "GROUP BY a.PART                                                               ";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssList1_Sheet1.Cells[i, 3].Text = String.Format("{0:###,###,##0}", VB.Val(dt1.Rows[0]["TransAmt"].ToString().Trim()));

                        nSum2 += Convert.ToInt32(VB.Val(dt1.Rows[0]["TransAmt"].ToString().Trim()));
                    }

                    dt1.Dispose();
                    dt1 = null;
                }
            }

            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 1].Text = "합계";
            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 2].Text = String.Format("{0:###,###,##0}", nSum);
            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 3].Text = String.Format("{0:###,###,##0}", nSum2);

            dt.Dispose();
            dt = null;

        }

        void eGetData2()
        {
            int i = 0;
            string strPart = "";
            int nSum = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            nSum = 0;
            ssList2_Sheet1.Rows.Count = 0;

            if (optGubun0.Checked == true)
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                        ";
                SQL += ComNum.VBLF + "  Pano,SName,DeptCode,Amt6 Amt,Part                                           ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM                                  ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                SQL += ComNum.VBLF + "      AND ActDATE =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')               ";
                if (txtPart.Text != "")
                {
                    SQL += ComNum.VBLF + "  AND  Part ='" + txtPart.Text + "'                                       ";
                }
                SQL += ComNum.VBLF + "UNION ALL                                                                     ";
                SQL += ComNum.VBLF + "SELECT                                                                        ";
                SQL += ComNum.VBLF + "  Pano,SName,DeptCode,RetAmt Amt,RetPart Part                                 ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM                                  ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                SQL += ComNum.VBLF + "      AND RETDATE =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')               ";
                if (txtPart.Text != "")
                {
                    SQL += ComNum.VBLF + "  AND  RetPart ='" + txtPart.Text + "'                                    ";
                }
                SQL += ComNum.VBLF + "ORDER BY Pano                                                                 ";
            }

            else if (optGubun1.Checked == true)
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                        ";
                SQL += ComNum.VBLF + "  Pano,SName,DeptCode,TransAmt Amt,TransPart Part                             ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM                                  ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                SQL += ComNum.VBLF + "      AND TransDATE =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')             ";
                if (txtPart.Text != "")
                {
                    SQL += ComNum.VBLF + "  AND  TransPart ='" + txtPart.Text + "'                                  ";
                }

                SQL += ComNum.VBLF + "ORDER BY Pano                                                                 ";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                nSum = 0;

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

                if (dt.Rows.Count > 0)
                {
                    ssList2_Sheet1.Rows.Count = dt.Rows.Count + 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList2_Sheet1.Cells[i, 3].Text = String.Format("{0:###,###,##0}", VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));
                        ssList2_Sheet1.Cells[i, 4].Text = CF.READ_PassName(clsDB.DbCon, dt.Rows[i]["Part"].ToString().Trim());
                        nSum += Convert.ToInt32(VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));
                    }

                    ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 2].Text = "합계";
                    ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 3].Text = String.Format("{0:###,###,##0}", nSum);
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

        }
    }
}
