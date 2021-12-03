using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewOptJinRequest.cs
    /// Description     : 일자별 선택진료 신청서 명단 출력
    /// Author          : 안정수
    /// Create Date     : 2017-09-18
    /// Update History  : 2017-11-06
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm선택진료신청명단.frm(Frm선택진료신청명단) => frmPmpaViewOptJinRequest.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm선택진료신청명단.frm(Frm선택진료신청명단)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewOptJinRequest : Form
    {
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        string mstrJobMan = "";
        public frmPmpaViewOptJinRequest()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewOptJinRequest(string GstrJobName)
        {
            InitializeComponent();
            setEvent();
            mstrJobMan = GstrJobName;

        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);

            this.btnExit2.Click += new EventHandler(eBtnEvent);
            this.btnView2.Click += new EventHandler(eBtnEvent);
            this.btnPrint2.Click += new EventHandler(eBtnEvent);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등     

            //출력시 잘림방지 위함
            ssList2_Sheet1.Columns[6].Visible = false;

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            txtPart.Text = "";
            CS.Spread_All_Clear(ssList1);
            optIO0.Checked = true;
            optIO20.Checked = true;
            optJob0.Checked = true;

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit || sender == this.btnExit2)
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

            else if (sender == this.btnPrint2)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ssList2_Sheet1.Columns[6].Visible = true;
                ePrint2();
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
            string strJobGbn = "";
            bool PrePrint = true;

            string PrintDate = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            string JobDate = dtpDate.Text;
            string JobMan = mstrJobMan;           


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            //ssList.ActiveSheet.Cells[0, 9].Text = "zzz";
            //ssList.ActiveSheet.Columns[9].Visible = false;
            #endregion

            if (ssList1_Sheet1.Rows.Count == 0)
            {
                return;
            }

            string strLine0 = "\r\n";
            string strLine1 = VB.Space(30) + "┌─┬────┬────┬────┐ " + "\n";
            string strLine2 = VB.Space(30) + "│결│담　  당│계　  장│팀　  장│ " + "\n";
            string strLine3 = VB.Space(30) + "│　├────┼────┼────┤ " + "\n";
            string strLine4 = VB.Space(30) + "│　│　　　　│　　　　│　　　　│ " + "\n";
            string strLine5 = VB.Space(30) + "│재│　　　　│　　　　│　　　  │ " + "\n";
            string strLine6 = VB.Space(30) + "└─┴────┴────┴────┘ " + "\n";


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;



            #endregion

            strJobGbn = "";
            if (chkEnd.Checked == false)
            {
                if (optJob0.Checked == true)
                {
                    strJobGbn = "[등록일자기준]";
                }
                else if (optJob1.Checked == true)
                {
                    strJobGbn = "[시작일자기준]";
                }
            }
            else
            {
                strJobGbn = "[해지조회]";
            }

            if (optIO0.Checked == true)
            {
                strJobGbn += "외래";
            }
            else if (optIO1.Checked == true)
            {
                strJobGbn += "입원";
            }
            else if (optIO2.Checked == true)
            {
                strJobGbn += "전체";
            }

            strTitle = "선택진료 신청서 명단" + strJobGbn + "\r\n";

            if (JobMan == "")
            {
                strSubTitle = "작업일자 : " + JobDate + "\r\n" + "출력시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                strSubTitle = "작업자 : " + JobMan + "\r\n" + "작업일자 : " + JobDate + "\r\n" + "출력시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strHeader += SPR.setSpdPrint_String(strLine0, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine1, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine2, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine3, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine4, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine5, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine6, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);

            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 13, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String("/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 70, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, true, true, true, true, false, true, false);

            SPR.setSpdPrint(ssList1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void ePrint2()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            string strJobGbn = "";
            bool PrePrint = true;

            string PrintDate = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            string JobDate = dtpDate.Text;
            string JobMan = mstrJobMan;

            if (ssList2_Sheet1.Rows.Count == 0)
            {
                return;
            }

            ssList2_Sheet1.Cells[0, 6].Text = "zzzz";
            ssList2_Sheet1.Columns[6].Visible = false;

            string strLine0 = "\r\n";
            string strLine1 = VB.Space(30) + "┌─┬────┬────┬────┐ " + "\n";
            string strLine2 = VB.Space(30) + "│결│담　  당│계　  장│팀　  장│ " + "\n";
            string strLine3 = VB.Space(30) + "│　├────┼────┼────┤ " + "\n";
            string strLine4 = VB.Space(30) + "│　│　　　　│　　　　│　　　　│ " + "\n";
            string strLine5 = VB.Space(30) + "│재│　　　　│　　　　│　　　  │ " + "\n";
            string strLine6 = VB.Space(30) + "└─┴────┴────┴────┘ " + "\n";

            strJobGbn = "";

            if (optIO0.Checked == true)
            {
                strJobGbn += "[외래]";
            }
            else if (optIO1.Checked == true)
            {
                strJobGbn += "[입원]";
            }
            else if (optIO2.Checked == true)
            {
                strJobGbn += "[전체]";
            }

            PrintDate = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            JobDate = dtpDate.Text;
            JobMan = mstrJobMan;

            strTitle = "기간별 선택진료 과별 건수" + strJobGbn;

            if (JobMan == "")
            {
                strSubTitle = "작업일자 : " + JobDate + "\r\n" + "출력시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                strSubTitle = "작업자 : " + JobMan + "\r\n" + "작업일자 : " + JobDate + "\r\n" + "출력시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            
            strHeader += SPR.setSpdPrint_String(strLine0, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine1, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine2, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine3, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine4, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine5, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine6, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);

            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 13, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String("/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 70, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, true, true, true, true, false, true, false);

            SPR.setSpdPrint(ssList2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;

            string strDate = "";

            CS.Spread_All_Clear(ssList1);

            strDate = dtpDate.Text;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  Pano,SName,DECODE(Gubun,'O','외래','I','입원',Gubun) Gubun,DeptCode,DrCode, ";
            SQL += ComNum.VBLF + "  TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, TO_CHAR(EDATE,'YYYY-MM-DD') EDATE,       ";
            SQL += ComNum.VBLF + "  TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI') EntDate,ENTSABUN                      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SELECT_MST                                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND (DELDATE IS NULL OR DELDATE ='')                                    ";
            SQL += ComNum.VBLF + "      AND Pano <>'81000004'                                                   ";
            if (txtPart.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "  AND EntSabun ='" + txtPart.Text + "'                                    ";
            }

            if (chkEnd.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND EDATE =TO_DATE('" + strDate + "','YYYY-MM-DD')                      ";
            }
            else if (chkAgree.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND EDATE =TO_DATE('" + strDate + "','YYYY-MM-DD')                      ";
                SQL += ComNum.VBLF + "  AND Work ='1'                                                           ";
            }
            else
            {
                if (optJob0.Checked == true)
                {
                    SQL += ComNum.VBLF + "AND TRUNC(ENTDATE) =TO_DATE('" + strDate + "','YYYY-MM-DD')           ";
                }
                else if (optJob1.Checked == true)
                {
                    SQL += ComNum.VBLF + "AND SDATE =TO_DATE('" + strDate + "','YYYY-MM-DD')                    ";
                }
            }

            if (optIO0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND GUBUN ='O'                                                          ";
            }
            else if (optIO1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND GUBUN ='1'                                                          ";
            }

            SQL += ComNum.VBLF + "ORDER BY PANO,GUBUN,EntDate                                                   ";

            try
            {
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

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    ssList1_Sheet1.Rows.Count = 0;
                    ssList1_Sheet1.Rows.Count = nRead;

                    for (i = 0; i < nRead; i++)
                    {
                        ssList1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 5].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());

                        ssList1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["SDate"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["EDate"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 8].Text = CF.Read_SabunName(clsDB.DbCon, dt.Rows[i]["EntSabun"].ToString().Trim());
                        ssList1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["EntDate"].ToString().Trim();

                        ssList1.ActiveSheet.Rows[i].Height = 22;
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
        }

        void eGetData2()
        {
            int i = 0;
            int nRow = 0;
            int nRead = 0;

            string strDate1 = "";
            string strDate2 = "";

            string strNew = "";
            string strOld = "";

            int nCnt1 = 0;
            int nCnt2 = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            nCnt1 = 0;
            nCnt2 = 0;

            CS.Spread_All_Clear(ssList2);
            ssList2_Sheet1.Rows.Count = 0;

            strDate1 = dtpFDate.Text;
            strDate2 = dtpTDate.Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  DECODE(a.Gubun,'O','외래','I','입원',Gubun) Gubun,b.PRINTRANKING,                       ";
            SQL += ComNum.VBLF + "  a.DeptCode,a.DrCode,COUNT(a.DrCode) CNT                                                 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SELECT_MST a, " + ComNum.DB_PMPA + "BAS_CLINICDEPT b       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND a.DEPTCODE=b.DEPTCODE(+)                                                        ";
            SQL += ComNum.VBLF + "      AND (a.DELDATE IS NULL OR a.DELDATE ='')                                            ";
            SQL += ComNum.VBLF + "      AND a.Pano <>'81000004'                                                             ";
            SQL += ComNum.VBLF + "      AND TRUNC(a.ENTDATE) >=TO_DATE('" + strDate1 + "','YYYY-MM-DD')                     ";
            SQL += ComNum.VBLF + "      AND TRUNC(a.ENTDATE) <=TO_DATE('" + strDate2 + "','YYYY-MM-DD')                     ";

            if (optIO20.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND a.GUBUN ='O'                                                                    ";
            }
            else if (optIO21.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND a.GUBUN ='I'                                                                    ";
            }
            SQL += ComNum.VBLF + "GROUP BY DECODE(a.Gubun,'O','외래','I','입원',Gubun) , b.PRINTRANKING,a.DeptCode,a.DrCode ";
            SQL += ComNum.VBLF + "ORDER BY 1,b.PRINTRANKING                                                                 ";

            try
            {
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

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    ssList2_Sheet1.Rows.Count = 0;
                    if (optIO22.Checked == true)
                    {
                        ssList2_Sheet1.Rows.Count = nRead + 2;
                    }
                    else
                    {
                        ssList2_Sheet1.Rows.Count = nRead + 1;
                    }

                    for (i = 0; i < nRead; i++)
                    {
                        strNew = dt.Rows[i]["Gubun"].ToString().Trim();

                        nRow += 1;

                        if (strNew == strOld || i == 0)
                        {
                            ssList2_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                            ssList2_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            ssList2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                            ssList2_Sheet1.Cells[nRow - 1, 3].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());

                            ssList2_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["CNT"].ToString().Trim();
                            nCnt1 += Convert.ToInt32(VB.Val(ssList2_Sheet1.Cells[nRow - 1, 4].Text));
                        }
                        else
                        {
                            ssList2_Sheet1.Cells[nRow - 1, 3].Text = "건수합";
                            ssList2_Sheet1.Cells[nRow - 1, 4].Text = nCnt1.ToString();

                            nCnt1 = 0;

                            nRow += 1;

                            ssList2_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                            ssList2_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            ssList2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                            ssList2_Sheet1.Cells[nRow - 1, 3].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());

                            ssList2_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["CNT"].ToString().Trim();
                            nCnt1 += Convert.ToInt32(VB.Val(ssList2_Sheet1.Cells[nRow - 1, 4].Text));
                        }

                        strOld = dt.Rows[i]["Gubun"].ToString().Trim();

                        ssList2.ActiveSheet.Rows[i].Height = 20;
                    }

                    ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 3].Text = "건수합";
                    ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 4].Text = nCnt1.ToString();

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
