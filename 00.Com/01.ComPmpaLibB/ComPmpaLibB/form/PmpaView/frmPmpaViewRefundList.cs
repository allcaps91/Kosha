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
    /// File Name       : frmPmpaViewRefundList.cs
    /// Description     : 일반환불대상자 일자별 List
    /// Author          : 안정수
    /// Create Date     : 2017-08-31
    /// Update History  : 2017-11-02
    /// <history>       
    /// d:\psmh\OPD\jepres\Frm환불내역List.frm(Frm환불내역List) => frmPmpaViewRefundList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jepres\Frm환불내역List.frm(Frm환불내역List)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewRefundList : Form, MainFormMessage
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

        public frmPmpaViewRefundList(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmPmpaViewRefundList()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewRefundList(string FstrPassid)
        {
            InitializeComponent();
            setEvent();
            mstrPassId = FstrPassid;
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
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등 

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            optGb0.Checked = true;
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

            #region 칼럼 히든
            ssList_Sheet1.Cells[0, 8].Text = "zzz";
            ssList_Sheet1.Columns[8].Visible = false;
            #endregion

            if (optGb0.Checked == true)
            {
                strTitle = "( " + dtpDate.Text + " ) 보관자 LIST" + "/n";
            }
            else if (optGb1.Checked == true)
            {
                strTitle = "( " + dtpDate.Text + " ) 환불자 LIST" + "/n";
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            if (mstrPassId != "")
            {
                strSubTitle = "작성자 : " + CF.READ_PassName(clsDB.DbCon, mstrPassId) + "/r";
                strHeader = SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);
            }

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 60, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            int nRead = 0;
            int nRow = 0;

            string strStatus = "";
            string strCheck = "";
            string strYTime1 = "";
            string strYTime2 = "";
            string strYTime3 = "";

            string strSDate = "";
            string strFDate = "";

            int nAmt = 0;
            int nTAmt = 0;

            string strPart = "";
            string strPart2 = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strSDate = dtpDate.Text;

            if (optGb0.Checked == true)
            {
                ssList_Sheet1.ColumnHeader.Cells[0, 5].Text = "보관일자";
                ssList_Sheet1.ColumnHeader.Cells[0, 6].Text = "보관금액";
            }

            else if (optGb1.Checked == true)
            {
                ssList_Sheet1.ColumnHeader.Cells[0, 5].Text = "환불일자";
                ssList_Sheet1.ColumnHeader.Cells[0, 6].Text = "환불금액";
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                            ";
            SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(BDATE,'YYYY-MM-DD') BDate,                                       ";
            SQL += ComNum.VBLF + "  PANO, DEPTCODE, DRCODE, BI, SNAME, PART, SABUN, CAMT,CSABUN,CPART,                                              ";
            SQL += ComNum.VBLF + "  TO_CHAR(RDATE,'YYYY-MM-DD HH24:MI') RDATE, RAMT, RSABUN, RPART, RREMARK,Year, CREMARK                           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_REFUND_ETC                                                                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                         ";
            if (optGb0.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strSDate + "','YYYY-MM-DD')                                                  ";
                SQL += ComNum.VBLF + "ORDER BY DRCODE, PANO                                                                                         ";
            }
            else
            {
                SQL += ComNum.VBLF + "      AND RDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')                                                   ";
                SQL += ComNum.VBLF + "      AND RDATE < TO_DATE('" + Convert.ToDateTime(strSDate).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "ORDER BY RPART DESC, DRCODE, PANO                                                                             ";
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
                    ComFunc.MsgBox("해당하는 대상자가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;

                    ssList_Sheet1.Rows.Count = nRead;

                    if (optGb0.Checked == true)
                    {
                        strPart = dt.Rows[0]["CPART"].ToString().Trim();
                    }
                    else if (optGb1.Checked == true)
                    {
                        strPart = dt.Rows[0]["RPART"].ToString().Trim();
                    }

                    nTAmt = 0;
                    nAmt = 0;
                    nRow = 0;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        //display
                        if (optGb0.Checked == true)
                        {
                            strPart2 = dt.Rows[i]["CPART"].ToString().Trim();
                        }
                        else if (optGb1.Checked == true)
                        {
                            strPart2 = dt.Rows[i]["RPART"].ToString().Trim();
                        }

                        if (strPart != strPart2)
                        {
                            nRow += 1;

                            ssList_Sheet1.Rows.Count = nRow;

                            ssList_Sheet1.Rows[nRow - 1].BackColor = Color.Beige;

                            ssList_Sheet1.Cells[nRow - 1, 0].Text = "소   계";
                            ssList_Sheet1.Cells[nRow - 1, 5].Text = " 작업조: [" + VB.Left(strPart + VB.Space(5), 5) + "]";
                            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", nAmt);
                            strPart = strPart2;
                        }

                        nRow += 1;

                        ssList_Sheet1.Rows.Count = nRow;

                        ssList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 3].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                        ssList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Bi"].ToString().Trim();

                        if (optGb0.Checked == true)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", dt.Rows[i]["CAMT"].ToString().Trim());
                            nAmt += Convert.ToInt32(VB.Val(dt.Rows[i]["CAMT"].ToString().Trim()));
                            nTAmt += Convert.ToInt32(VB.Val(dt.Rows[i]["CAMT"].ToString().Trim()));
                        }
                        else if (optGb1.Checked == true)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", dt.Rows[i]["RAMT"].ToString().Trim());
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
            ssList_Sheet1.Cells[nRow - 1, 5].Text = "작업사번: [" + VB.Left(strPart + VB.Space(5), 5) + "]";
            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", nAmt);

            nRow += 1;
            ssList_Sheet1.Rows.Count = nRow;

            ssList_Sheet1.Rows[nRow - 1].BackColor = Color.LightPink;

            ssList_Sheet1.Cells[nRow - 1, 0].Text = "전체 합계";
            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:##,###,##0}", nTAmt);
        }

    }
}
