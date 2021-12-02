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
    /// File Name       : frmPmpaViewScheduledScansDeptList.cs
    /// Description     : 예약검사 당일 타과 명단
    /// Author          : 안정수
    /// Create Date     : 2017-09-19 
    /// Update History  : 2017-11-06
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm예약검사타과명단.frm(Frm예약검사타과명단) => frmPmpaViewScheduledScansDeptList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm예약검사타과명단.frm(Frm예약검사타과명단)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewScheduledScansDeptList : Form
    {
        ComFunc CF = new ComFunc();
        string mstrPassId = "";
        public frmPmpaViewScheduledScansDeptList()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewScheduledScansDeptList(string FstrPassId)
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

            strTitle = "( " + dtpDate.Text + " ) 예약검사 대체금액 집계표";

            if (mstrPassId != "")
            {
                strSubTitle = "작성자:" + CF.READ_PassName(clsDB.DbCon, mstrPassId);
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            if(mstrPassId != "")
            {
                strHeader += SPR.setSpdPrint_String(strTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);
            }

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 170, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

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
            SQL += ComNum.VBLF + "SELECT                                                                                                                    ";
            SQL += ComNum.VBLF + "  Ptno,TO_CHAR(JDate,'YYYY-MM-DD') JDate,SName,DeptCode,DrCode,                                                           ";
            SQL += ComNum.VBLF + "  TO_CHAR(BDate,'YYYY-MM-DD') BDate,                                                                                      ";
            SQL += ComNum.VBLF + "  TO_CHAR(RDate,'YYYY-MM-DD') RDate                                                                                       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "ENDO_JUPMST                                                                                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                                 ";
            SQL += ComNum.VBLF + "      AND RDATE >=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')                                                            ";
            SQL += ComNum.VBLF + "      AND RDATE <TO_DATE('" + Convert.ToDateTime(dtpDate.Text).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')          ";
            SQL += ComNum.VBLF + "      AND RES ='1'                                                                                                        ";
            SQL += ComNum.VBLF + "      AND GBSUNAP <> '*'                                                                                                  ";
            SQL += ComNum.VBLF + "      AND DEPTCODE NOT IN ('MG')                                                                                          ";
            SQL += ComNum.VBLF + "GROUP BY Ptno,TO_CHAR(JDate,'YYYY-MM-DD') ,SName,DeptCode,DrCode,TO_CHAR(BDate,'YYYY-MM-DD') ,TO_CHAR(RDate,'YYYY-MM-DD') ";

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

                    for (i = 0; i < nRead; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["RDate"].ToString().Trim();
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
