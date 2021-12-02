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
    /// File Name       : frmPmpaViewReceiptIssueList.cs
    /// Description     : 영수증 발급 리스트
    /// Author          : 안정수
    /// Create Date     : 2017-09-18
    /// Update History  : 2017-11-06
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm영수증발급.frm(Frm영수증발급) => frmPmpaViewReceiptIssueList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm영수증발급.frm(Frm영수증발급)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewReceiptIssueList : Form
    {
        int mnJobSabun = 0;

        public frmPmpaViewReceiptIssueList()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewReceiptIssueList(int GnJobSabun)
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
            if (mnJobSabun != 4349)
            {
                ssList_Sheet1.Columns[9].Visible = true;
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
                //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                //{
                //    return; //권한 확인
                //}
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
             ////   if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
             //   {
             //       return; //권한 확인
             //   }
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
            string strGubun = "";

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (optGubun0.Checked == true)
            {
                strGubun += "[외래]";
            }
            else if (optGubun1.Checked == true)
            {
                strGubun += "[입원]";
            }

            strTitle += strGubun + "/n/n/n/n";

            strSubTitle = "/l" + "조회일자 : " + dtpFDate.Text + " ~ " + dtpTDate.Text;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 60, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, false, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.ActDate,'YYYY-MM-DD') ActDate,a.Pano,a.Remark,                                ";
            SQL += ComNum.VBLF + "  a.USE1,a.USE2,a.REMARK,a.ENTPART,a.PJUMIN1,a.PJUMIN2, a.pJumin3,a.PSNAME,a.PTEL,b.SName ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_PRINT_HIS a, " + ComNum.DB_PMPA + "BAS_PATIENT b           ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                                ";
            SQL += ComNum.VBLF + "      AND a.ACTDATE >=TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')                       ";
            SQL += ComNum.VBLF + "      AND a.ACTDATE <=TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')                       ";
            SQL += ComNum.VBLF + "      AND a.Pano <> '81000004'                                                            ";
            SQL += ComNum.VBLF + "      AND a.EntPart <> 4349                                                               ";
            if (optGubun0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND a.Remark Like '외래%'                                                           ";
            }
            else if (optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND a.Remark Like '입원%'                                                           ";
            }
            SQL += ComNum.VBLF + "ORDER BY PANO                                                                             ";

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
                    ssList_Sheet1.Rows.Count = nRead;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < nRead; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PSname"].ToString().Trim();

                        if (dt.Rows[i]["PJumin3"].ToString().Trim() != "")
                        {
                            ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["PJumin1"].ToString().Trim() + "-" + VB.Left(clsAES.DeAES(dt.Rows[i]["PJumin3"].ToString().Trim()), 1) + "******";
                        }
                        else
                        {
                            ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["PJumin1"].ToString().Trim() + "-" + VB.Left(dt.Rows[i]["PJumin2"].ToString().Trim(), 1) + "******";
                        }

                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["PTel"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Use1"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Use2"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["Remark"].ToString().Trim();
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

    }
}
