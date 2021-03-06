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
    /// File Name       : frmPmpaViewJepsuFailedPeriod.cs
    /// Description     : 접수부도자 기간별 조회
    /// Author          : 안정수
    /// Create Date     : 2017-08-31
    /// Update History  : 2017-11-02
    /// <history>       
    /// d:\psmh\OPD\jepres\Frm접수부도기간별조회.frm(Frm접수부도기간별조회) => frmPmpaViewJepsuFailedPeriod.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jepres\Frm접수부도기간별조회.frm(Frm접수부도기간별조회)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewJepsuFailedPeriod : Form, MainFormMessage
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        string mstrJobName = "";

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

        public frmPmpaViewJepsuFailedPeriod(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmPmpaViewJepsuFailedPeriod()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewJepsuFailedPeriod(string GstrJobName)
        {
            InitializeComponent();
            setEvent();
            mstrJobName = GstrJobName;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);

            this.optDate0.CheckedChanged += new EventHandler(eBtnEvent);
            this.optDate1.CheckedChanged += new EventHandler(eBtnEvent);
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

            optDate0.Checked = true;
            optGbn0.Checked = true;

            CF.ComboMonth_Set1(cboYYMM, 12);
            CS.Spread_All_Clear(ssList);

            cboYYMM.Visible = false;
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

            else if (sender == this.optDate0 || sender == this.optDate1)
            {
                if (sender == this.optDate0)
                {
                    cboYYMM.Visible = false;
                    dtpFDate.Visible = true;
                    dtpTDate.Visible = true;
                }
                else if (sender == this.optDate1)
                {
                    cboYYMM.Visible = true;
                    dtpFDate.Visible = false;
                    dtpTDate.Visible = false;
                }
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
            //ssList.ActiveSheet.Cells[0, 9].Text = "zzz";
            //ssList.ActiveSheet.Columns[9].Visible = false;


            #endregion

            strTitle = "예약부도 기간별 조회";
            if (mstrJobName != "")
            {
                strSubTitle = "작업일자 : " + dtpFDate.Text + "~" + dtpTDate.Text + "   작성자 : " + mstrJobName;
            }

            else
            {
                strSubTitle = "작업일자 : " + dtpFDate.Text + "~" + dtpTDate.Text;
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 25, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;

            double nSum = 0;
            double nRSum = 0;

            int nSCnt = 0;
            int nRSCnt = 0;

            string strFDate = "";
            string strTDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            Cursor.Current = Cursors.WaitCursor;

            if (optDate0.Checked == true)
            {
                strFDate = dtpFDate.Text;
                strTDate = dtpTDate.Text;
            }
            else
            {
                strFDate = VB.Left(cboYYMM.SelectedItem.ToString(), 5) + ComFunc.SetAutoZero(VB.Mid(cboYYMM.SelectedItem.ToString(), 6, 2), 2) + "-01";
                strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            }

            nSum = 0;
            nRSum = 0;
            nSCnt = 0;
            nRSCnt = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  PANO,DEPTCODE,BI,SNAME,SEX,GBGAMEK,JIN,BOHUN,ROWID,CAmt,RAmt,Gubun,";
            SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,TO_CHAR(YDate1,'YYYY-MM-DD') YDate1,";
            SQL += ComNum.VBLF + "  TO_CHAR(YDate2,'YYYY-MM-DD') YDate2,TO_CHAR(YDate3,'YYYY-MM-DD') YDate3,";
            SQL += ComNum.VBLF + "  TO_CHAR(RDATE,'YYYY-MM-DD') RDATE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_REFUND";
            SQL += ComNum.VBLF + "WHERE 1=1";

            if (optGbn0.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "      AND ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND GUBUN = '01'";
                SQL += ComNum.VBLF + "ORDER BY ACTDATE,PANO";
            }

            else if (optGbn1.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND RDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND RDATE <TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND GUBUN = '01'";
                SQL += ComNum.VBLF + "ORDER BY RDATE,PANO";
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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    ssList_Sheet1.Rows.Count = 0;
                    ssList_Sheet1.Rows.Count = nRead;

                    for (i = 0; i < nRead; i++)
                    {
                        if (optGbn0.Checked == true)
                        {
                            ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        }
                        else
                        {
                            ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RDate"].ToString().Trim();
                        }
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GBGAMEK"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Jin"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Bohun"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["YDate3"].ToString().Trim();





                        ssList_Sheet1.Cells[i, 10].Text = String.Format("{0:###,###,##0}", VB.Val(dt.Rows[i]["CAmt"].ToString().Trim()));
                        nSum += VB.Val(dt.Rows[i]["CAmt"].ToString().Trim());
                        nSCnt += 1;

                        ssList_Sheet1.Cells[i, 11].Text = String.Format("{0:###,###,##0}", VB.Val(dt.Rows[i]["RAmt"].ToString().Trim()));
                        nRSum += VB.Val(dt.Rows[i]["RAmt"].ToString().Trim());
                        nRSCnt += 1;

                        ssList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 13].Text = (dt.Rows[i]["Gubun"].ToString().Trim() == "00" ? "예약접수" : "당일접수");
                        ssList_Sheet1.Cells[i, 14].Text = "";

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  PANO";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND ACTDATE >=TO_DATE('" + Convert.ToDateTime(dtpTDate.Text).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND ACTDATE <=TO_DATE('" + clsPmpaPb.GstrSysDate + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND Pano ='" + dt.Rows[i]["Pano"].ToString().Trim() + "'";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssList_Sheet1.Cells[i, 14].Text = "Y";
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    ssList_Sheet1.Rows.Count += 1;

                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 9].Text = "합계";
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 10].Text = String.Format("{0:###,###,###,##0}", nSum);
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 11].Text = String.Format("{0:###,###,###,##0}", nRSum);
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;



            if (optGbn0.Checked == true)
            {
                label4.Text = "보관건수";
                txtGunsu.Text = nSCnt + "건";
            }

            else
            {
                label4.Text = "환불건수";
                txtGunsu.Text = nRSCnt + "건";
            }

            for(int k = 0; k < ssList_Sheet1.Rows.Count; k++)
            {
                ssList_Sheet1.Rows[k].Height = 30;
            }

            Cursor.Current = Cursors.Default;
        }
    }
}
