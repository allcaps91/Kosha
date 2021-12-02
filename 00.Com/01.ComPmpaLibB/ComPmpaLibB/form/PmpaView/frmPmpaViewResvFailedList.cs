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
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewResvFailedList.cs
    /// Description     : 예약부도자 일자별 List 조회 폼
    /// Author          : 안정수
    /// Create Date     : 2017-08-31
    /// Update History  : 2017-10-02, d:\psmh\OPD\jepres\jepres03.frm(FrmReservedFailedView) => frmPmpaViewResvFailedList.cs으로 통합함
    /// <history>       
    /// d:\psmh\OPD\jepres\Frm예약부도자명단.frm(Frm예약부도자명단.frm) => frmPmpaViewResvFailedList.cs 으로 변경함    
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jepres\Frm예약부도자명단.frm(Frm예약부도자명단.frm)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewResvFailedList : Form, MainFormMessage
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        string mstrPassId = "";

        #region MainFormMessage InterFace

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

        #endregion

        public frmPmpaViewResvFailedList(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmPmpaViewResvFailedList()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewResvFailedList(string FstrPassId)
        {
            InitializeComponent();
            setEvent();
            mstrPassId = FstrPassId;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);

            this.optGubun0.CheckedChanged += new EventHandler(eControl_OptChange);
            this.optGubun1.CheckedChanged += new EventHandler(eControl_OptChange);

            this.optGB0.CheckedChanged += new EventHandler(eControl_OptChange);
            this.optGB1.CheckedChanged += new EventHandler(eControl_OptChange);
        }

        void eControl_OptChange(object sender, EventArgs e)
        {
            if (sender == this.optGB0 || sender == this.optGB1)
            {
                txtInspection.Visible = true;
                txtInspection.Text = "예약검사";

                //groupBox7.Enabled = false;
                optGubun0.Checked = false;
                optGubun1.Checked = false;
            }

            else if (sender == this.optGubun0 || sender == this.optGubun1)
            {
                txtInspection.Visible = false;
                //groupBox4.Enabled = false;
                optGB0.Checked = false;
                optGB1.Checked = false;
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등           
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            
            txtCnt.Text = "";
            txtCnt2.Text = "";
            txtCnt3.Text = "";
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }

            else if (sender == this.btnCancel)
            {
                optGB0.Checked = false;
                optGB1.Checked = false;
                optGubun0.Checked = false;
                optGubun1.Checked = false;
                groupBox4.Enabled = true;
                groupBox7.Enabled = true;

                ComFunc.SetAllControlClear(panel1);
                CS.Spread_All_Clear(ssList);
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


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            ssList_Sheet1.Cells[0, 8].Text = "zzz";
            ssList_Sheet1.Columns[8].Visible = false;
            


            #endregion

            if (optGB0.Checked == true || optGB1.Checked == true)
            {
                if(optGB0.Checked == true)
                {
                    strTitle = "( " + dtpDate.Text + " ~ "+ dtpDateTo.Text +" ) " + "예약검사 부도자 LIST";
                }

                else if(optGB1.Checked == true)
                {
                    strTitle = "( " + dtpDate.Text + " ~ " + dtpDateTo.Text + " ) " + "예약검사 환불자 LIST";
                }                
            }
            else if (optGubun0.Checked == true || optGubun1.Checked == true)
            {
                if (optGubun0.Checked == true)
                {
                    strTitle = "( " + dtpDate.Text + " ~ " + dtpDateTo.Text + " ) " + "예약 부도자 LIST";
                }

                else if (optGubun1.Checked == true)
                {
                    strTitle = "( " + dtpDate.Text + " ~ " + dtpDateTo.Text + " ) " + "예약 환불자 LIST";
                }
            }

            if(clsPublic.FstrPassId != "")
            {
                strSubTitle = "작성자: " + CF.Read_SabunName(clsDB.DbCon, clsPublic.FstrPassId) + VB.Space(15) + "출력시간: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + VB.Space(15) + "Page : " + "/p";
            }

            else
            {
                strSubTitle = VB.Space(23) + "출력시간: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + VB.Space(15) + "Page : " + "/p";
            }
            

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += SPR.setSpdPrint_String("작성자:" + CF.READ_PassName(clsDB.DbCon, mstrPassId) + "출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 55, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eGetData()
        {
            if(optGB0.Checked == false && optGB1.Checked == false && optGubun0.Checked == false && optGubun1.Checked == false)
            {
                ComFunc.MsgBox("구분을 선택해주세요.");
                return;
            }

            int i = 0;
            int nRead = 0;
            int nRow = 0;

            string strSDate = "";
            string strTDate = "";

            int nAmt = 0;
            int nTAmt = 0;

            string strPart = "";
            string strPart2 = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            CS.Spread_All_Clear(ssList);

            strSDate = dtpDate.Text;
            strTDate = dtpDateTo.Text;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,                                                                      ";
            SQL += ComNum.VBLF + "  PANO, DEPTCODE, DRCODE, CHOJAE, BI, SNAME, SEX, AGE, GBGAMEK, GBSPC, JIN , BOHUN,                           ";
            SQL += ComNum.VBLF + "  TO_CHAR(YDATE1,'YYYY-MM-DD HH24:MI') YDATE1,                                                                ";
            SQL += ComNum.VBLF + "  TO_CHAR(YDATE2,'YYYY-MM-DD HH24:MI') YDATE2,                                                                ";
            SQL += ComNum.VBLF + "  TO_CHAR(YDATE3,'YYYY-MM-DD HH24:MI') YDATE3,                                                                ";
            SQL += ComNum.VBLF + "  AMT1 , AMT2, AMT3, AMT4, AMT5, AMT6, AMT7,                                                                  ";
            SQL += ComNum.VBLF + "  GELCODE, CAMT, CSABUN, CPART,                                                                               ";
            SQL += ComNum.VBLF + "  TO_CHAR(RDATE,'YYYY-MM-DD HH24:MI') RDATE, RAMT, RSABUN, RPART, RREMARK                                     ";
            if (optGB0.Checked == true || optGB1.Checked)
            {
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_REFUND_EXAM                                                                ";
            }
            else if (optGubun0.Checked == true || optGubun1.Checked)
            {
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_REFUND                                                                     ";
            }
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";
            if (optGB0.Checked == true || optGubun0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND ACTDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')                                                  ";
                SQL += ComNum.VBLF + "  AND ACTDATE < TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')                                                  ";
            }
            else if (optGB1.Checked == true || optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND RDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')                                                   ";
                SQL += ComNum.VBLF + "  AND RDATE <  TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')";
            }

            if (optGubun0.Checked == true || optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND GUBUN = '00'                                                                                        ";
            }

            if (optGB0.Checked == true || optGubun0.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY DRCODE, PANO                                                                                     ";
            }
            else if (optGB1.Checked == true || optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY RPART DESC, DRCODE, PANO";
            }

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
                    ComFunc.MsgBox("해당하는 예약부도자가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;

                    ssList_Sheet1.Rows.Count = nRead;

                    if (optGB0.Checked == true || optGubun0.Checked == true)
                    {
                        ssList_Sheet1.ColumnHeader.Cells[0, 5].Text = "예약접수일자";
                        ssList_Sheet1.ColumnHeader.Cells[0, 6].Text = "예약부도금액";
                    }

                    if (optGB1.Checked == true || optGubun1.Checked == true)
                    {
                        ssList_Sheet1.ColumnHeader.Cells[0, 5].Text = "예약환불일자";
                        ssList_Sheet1.ColumnHeader.Cells[0, 6].Text = "예약환불금액";
                    }

                    if (optGB0.Checked == true || optGubun0.Checked == true)
                    {
                        strPart = dt.Rows[0]["CPART"].ToString().Trim();
                    }
                    else if (optGB1.Checked == true || optGubun1.Checked == true)
                    {
                        strPart = dt.Rows[0]["RPART"].ToString().Trim();
                    }

                    nTAmt = 0;
                    nAmt = 0;
                    nRow = 0;

                    for (i = 0; i < nRead; i++)
                    {
                        //display
                        if (optGB0.Checked == true || optGubun0.Checked == true)
                        {
                            strPart2 = dt.Rows[i]["CPART"].ToString().Trim();
                        }
                        else if (optGB1.Checked == true || optGubun1.Checked == true)
                        {
                            strPart2 = dt.Rows[i]["RPART"].ToString().Trim();
                        }

                        if (strPart != strPart2)
                        {
                            nRow += 1;
                            ssList_Sheet1.Rows.Count = nRow;

                            ssList_Sheet1.Rows[nRow - 1].BackColor = Color.Beige;
                            ssList_Sheet1.Cells[nRow - 1, 0].Text = "소  계";
                            ssList_Sheet1.Cells[nRow - 1, 5].Text = " 작업조 : [" + VB.Left(strPart + VB.Space(5), 5) + "]";
                            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", nAmt);

                            nAmt = 0;

                            strPart = strPart2;
                        }
                        nRow += 1;
                        ssList_Sheet1.Rows.Count = nRow;

                        ssList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 3].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                        ssList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["BI"].ToString().Trim();

                        if (optGB0.Checked == true || optGubun0.Checked == true)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", VB.Val(dt.Rows[i]["CAMT"].ToString().Trim()));
                            nAmt += Convert.ToInt32(VB.Val(dt.Rows[i]["CAMT"].ToString().Trim()));
                            nTAmt += Convert.ToInt32(VB.Val(dt.Rows[i]["CAMT"].ToString().Trim()));
                        }

                        else if (optGB1.Checked == true || optGubun1.Checked == true)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", VB.Val(dt.Rows[i]["RAMT"].ToString().Trim()));
                            nAmt += Convert.ToInt32(VB.Val(dt.Rows[i]["RAMT"].ToString().Trim()));
                            nTAmt += Convert.ToInt32(VB.Val(dt.Rows[i]["RAMT"].ToString().Trim()));
                        }
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

            nRow += 1;
            ssList_Sheet1.Rows.Count = nRow;

            ssList_Sheet1.Rows[nRow - 1].BackColor = Color.Beige;
            ssList_Sheet1.Cells[nRow - 1, 0].Text = "소  계";
            ssList_Sheet1.Cells[nRow - 1, 5].Text = " 작업사번 : [" + VB.Left(strPart + VB.Space(5), 5) + "]";
            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", nAmt);

            nRow += 1;
            ssList_Sheet1.Rows.Count = nRow;

            ssList_Sheet1.Rows[nRow - 1].BackColor = Color.Beige;
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "전체 합계";
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 6].Text = String.Format("{0:##,###,##0}", nTAmt);


            //부도환자수
            txtCnt2.Text = nRead.ToString();

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                               ";
            SQL += ComNum.VBLF + "  COUNT(PANO) CNT                                                                                                    ";
            if (optGubun0.Checked == true || optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW                                                                          ";
            }
            else
            {
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM                                                                         ";
            }
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                            ";
            if (optGB0.Checked == true || optGB1.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND BDATE >= TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')                                                      ";
                SQL += ComNum.VBLF + "      AND BDATE < TO_DATE('" + Convert.ToDateTime(dtpDate.Text).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')    ";
            }
            else if (optGubun0.Checked == true || optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND DATE3 >= TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')                                                      ";
                SQL += ComNum.VBLF + "      AND DATE3 < TO_DATE('" + Convert.ToDateTime(dtpDate.Text).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')    ";
            }
            SQL += ComNum.VBLF + "      AND TRANSDATE >= TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')                                                  ";
            SQL += ComNum.VBLF + "      AND TRANSDATE < TO_DATE('" + Convert.ToDateTime(dtpDate.Text).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')";

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
                    //예약환자수
                    txtCnt.Text = dt.Rows[0]["CNT"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            if (optGubun0.Checked == true || optGubun1.Checked == true)
            {
                txtCnt3.Text = String.Format("{0:###}", ((VB.Val(txtCnt2.Text) / VB.Val(txtCnt.Text)) * 100)) + "%";
            }

            Cursor.Current = Cursors.Default;

        }
    }
}
