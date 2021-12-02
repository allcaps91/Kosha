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
    /// File Name       : frmPmpaViewResvFailedMonth.cs
    /// Description     : 예약부도자 월별 조회
    /// Author          : 안정수
    /// Create Date     : 2017-08-31
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\jepres\Frm예약부도월별조회.frm(Frm예약부도월별조회) => frmPmpaViewResvFailedMonth.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jepres\Frm예약부도월별조회.frm(Frm예약부도월별조회)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewResvFailedMonth : Form
    {
        ComFunc CF = new ComFunc();
        string mstrJobName = "";

        public frmPmpaViewResvFailedMonth()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewResvFailedMonth(string GstrJobName)
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

            CF.ComboMonth_Set(cboYYMM, 12);
            cboYYMM.SelectedIndex = 1;

            ssList_Sheet1.Columns[11].Visible = false;
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
            ssList.ActiveSheet.Cells[0, 14].Text = "zzz";
            ssList.ActiveSheet.Columns[14].Visible = false;

            #endregion

            strTitle = "예약부도 기간별 조회";
            if (mstrJobName != "")
            {
                strSubTitle = "작업일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + "   작성자 : " + mstrJobName;
            }

            else
            {
                strSubTitle = "작업일자 : " + DateTime.Now.ToString("yyyy-MM-dd");
            }


            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 35, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, true, true, false, true, false, false, false);

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

            string strFDate = "";
            string strTDate = "";

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            strFDate = VB.Left(cboYYMM.SelectedItem.ToString(), 4) + "-" + VB.Mid(cboYYMM.SelectedItem.ToString(), 7, 2) + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);

            nSum = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  PANO,DEPTCODE,BI,SNAME,SEX,GBGAMEK,JIN,BOHUN,ROWID,CAmt,Gubun,              ";
            SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(YDate1,'YYYY-MM-DD') YDate1, ";
            SQL += ComNum.VBLF + "  TO_CHAR(YDate2,'YYYY-MM-DD') YDate2, TO_CHAR(YDate3,'YYYY-MM-DD') YDate3    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_REFUND                                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')                  ";
            SQL += ComNum.VBLF + "      AND ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')                  ";
            SQL += ComNum.VBLF + "      AND (RDATE IS NULL OR RDATE ='')                                        ";
            SQL += ComNum.VBLF + "ORDER BY ACTDATE,PANO                                                         ";

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
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GBGAMEK"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Jin"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Bohun"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["YDate3"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 10].Text = String.Format("{0:###,###,##0}", Convert.ToInt32(dt.Rows[i]["CAmt"].ToString().Trim()));
                        nSum += VB.Val(dt.Rows[i]["CAmt"].ToString().Trim());
                        ssList_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 12].Text = (dt.Rows[i]["Gubun"].ToString().Trim() == "00" ? "예약접수" : "당일접수");
                        ssList_Sheet1.Cells[i, 13].Text = "";

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT";
                        SQL += ComNum.VBLF + "  PANO";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND ACTDATE >=TO_DATE('" + Convert.ToDateTime(strFDate).AddDays(365 * 3).ToShortDateString() + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND ACTDATE <=TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND Pano ='" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssList_Sheet1.Cells[i, 13].Text = "Y";
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    ssList_Sheet1.Rows.Count += 1;

                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 9].Text = "합계";
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 10].Text = String.Format("{0:###,###,###,##0}", nSum);
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            for(int k = 0; k < ssList_Sheet1.Rows.Count; k++)
            {
                ssList_Sheet1.Rows[k].Height = 30;
            }

            Cursor.Current = Cursors.Default;

        }

    }
}
