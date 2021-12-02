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
    /// File Name       : frmPmpaViewNotJinList2.cs
    /// Description     : 당일 접수비 수납후 미진료 환자 리스트
    /// Author          : 안정수
    /// Create Date     : 2017-09-08
    /// Update History  : 2017-11-04
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm당일접수비수납후미진료.frm(Frm당일접수비수납후미진료) => frmPmpaViewNotJinList2.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm당일접수비수납후미진료.frm(Frm당일접수비수납후미진료)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewNotJinList2 : Form
    {
        ComFunc CF = new ComFunc();
        public frmPmpaViewNotJinList2()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);
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

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            dtpFDate.Text = Convert.ToDateTime(CurrentDate).AddDays(-1).ToShortDateString();
            dtpTDate.Text = Convert.ToDateTime(CurrentDate).AddDays(-1).ToShortDateString();
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

            else if (sender == this.btnCancel)
            {
                ssList_Sheet1.Rows.Count = 0;
                ssList_Sheet1.Rows.Count = 30;
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

            strTitle = "당일 접수비 수납후 미진료 환자" + "/n";
            strSubTitle = "작업일자 : " + dtpFDate.Text + "~" + dtpTDate.Text;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 120, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, true, false, false, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);       
        }

        void eGetData()
        {
            string strFDate = "";
            string strTDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            ssList_Sheet1.Rows.Count = 0;
            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                        ";
            SQL += ComNum.VBLF + "  TO_CHAR(DELDATE,'YYYY-MM-DD HH24:MI') DELDATE,              ";
            SQL += ComNum.VBLF + "  PANO,SNAME,DEPTCODE,AMT7,DELPART                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER_DEL                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "      AND BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "      AND GBCANCEL = '1'                                      ";
            SQL += ComNum.VBLF + "ORDER BY DELDATE,PANO                                         ";

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
                    ssList_Sheet1.Rows.Count += 1;

                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = dt.Rows[i]["DELDATE"].ToString().Trim();
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[i]["AMT7"].ToString().Trim()));
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = dt.Rows[i]["DELPART"].ToString().Trim();
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 6].Text = " ";
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;


        }

    }
}
