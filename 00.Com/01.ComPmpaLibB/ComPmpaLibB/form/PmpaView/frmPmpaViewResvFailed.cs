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
    /// File Name       : frmPmpaViewResvFailed.cs
    /// Description     : 예약부도자 기간별 조회
    /// Author          : 안정수
    /// Create Date     : 2017-08-29
    /// Update History  : 2017-11-02
    /// <history>       
    /// d:\psmh\OPD\jepres\Frm예약부도기간별조회.frm(Frm예약부도기간별조회) => frmPmpaViewResvFailed.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jepres\Frm예약부도기간별조회.frm(Frm예약부도기간별조회)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewResvFailed : Form
    {
        ComFunc CF = new ComFunc();
        string mstrJobName = "";
        string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

        public frmPmpaViewResvFailed()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewResvFailed(string GstrJobName)
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

            optGbn0.Checked = true;
            optDate0.Checked = true;

            CF.ComboMonth_Set1(cboYYMM, 12);

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
                //
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                //
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }

            else if (sender == this.optDate0)
            {
                cboYYMM.Visible = false;
                dtpFDate.Visible = true;
                dtpTDate.Visible = true;
                label3.Visible = true;
            }

            else if (sender == this.optDate1)
            {
                cboYYMM.Visible = true;
                dtpFDate.Visible = false;
                dtpTDate.Visible = false;
                label3.Visible = false;
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

            strTitle = "내시경예약부도 기간별 조회";

            if (mstrJobName != "")
            {
                strSubTitle = "작업일자 : " + dtpFDate.Text + " ~ " + dtpTDate.Text + "\r\n" + "  작성자 : " + mstrJobName;
            }
            else
            {
                strSubTitle = "작업일자 : " + dtpFDate.Text + "~" + dtpTDate.Text;
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 170, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);           
        }

        void eGetData()
        {
            int i = 0;
            int nREAD = 0;

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
                if (VB.Mid(cboYYMM.SelectedItem.ToString(), 6, 1) != "1")
                {
                    strFDate = VB.Left(cboYYMM.SelectedItem.ToString(), 5) + "0" + VB.Mid(cboYYMM.SelectedItem.ToString(), 6, 1) + "-01";
                }
                else
                {
                    strFDate = cboYYMM.SelectedItem.ToString() + "-01";
                }
                strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            }

            nSum = 0;
            nRSum = 0;
            nSCnt = 0;
            nRSCnt = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                        ";
            SQL += ComNum.VBLF + "  PANO,DEPTCODE,BI,SNAME,SEX,GBGAMEK,JIN,BOHUN,ROWID,CAmt,RAmt,Gubun,                                         ";
            SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,TO_CHAR(YDate1,'YYYY-MM-DD') YDate1,                                  ";
            SQL += ComNum.VBLF + "  TO_CHAR(YDate2,'YYYY-MM-DD') YDate2,TO_CHAR(YDate3,'YYYY-MM-DD') YDate3,                                    ";
            SQL += ComNum.VBLF + "  TO_CHAR(RDATE,'YYYY-MM-DD') RDATE                                                                           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_REFUND                                                                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";

            if (optGbn0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')                                                  ";
                SQL += ComNum.VBLF + "  AND ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')                                                  ";
                SQL += ComNum.VBLF + "  AND GUBUN = '00'                                                                                        ";
                SQL += ComNum.VBLF + "ORDER BY ACTDATE,PANO                                                                                     ";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND RDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')                                                    ";
                SQL += ComNum.VBLF + "  AND RDATE < TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND GUBUN = '00'                                                                                        ";
                SQL += ComNum.VBLF + "ORDER BY RDATE,PANO                                                                                       ";
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
                    nREAD = dt.Rows.Count;
                    ssList_Sheet1.Rows.Count = 0;
                    ssList_Sheet1.Rows.Count = nREAD;

                    for (i = 0; i < nREAD; i++)
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
                        SQL += ComNum.VBLF + "SELECT";
                        SQL += ComNum.VBLF + "  PANO";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND ACTDATE >=TO_DATE('" + Convert.ToDateTime(dtpTDate.Text).AddDays(365 * 3).ToShortDateString() + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND ACTDATE <=TO_DATE('" + CurrentDate + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND Pano ='" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                        SQL += ComNum.VBLF + "";
                        SQL += ComNum.VBLF + "";

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
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            ssList_Sheet1.Rows.Count += 1;

            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 9].Text = "합계";
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 10].Text = String.Format("{0:###,###,###,##0}", nSum);
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 11].Text = String.Format("{0:###,###,###,##0}", nRSum);

            dt.Dispose();
            dt = null;

            if (optGbn0.Checked == true)
            {
                label2.Text = "보관건수: " + nSCnt + "건";
            }
            else
            {
                label2.Text = "환불건수: " + nRSCnt + "건";
            }

            #region ROW 높이 설정
            for (int k = 0; k < ssList_Sheet1.Rows.Count; k++)
            {

                ssList_Sheet1.Rows[k].Height = 30;
            }
            #endregion

            Cursor.Current = Cursors.Default;
            ComFunc.MsgBox("조회가 완료되었습니다.");

        }
    }
}
