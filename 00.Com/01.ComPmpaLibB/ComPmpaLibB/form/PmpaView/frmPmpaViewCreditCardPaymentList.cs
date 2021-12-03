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

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewCreditCardPaymentList.cs
    /// Description     : 신용카드결제조회
    /// Author          : 안정수
    /// Create Date     : 2017-07-06
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\jengsan\Frm카드결제조회.frm(Frm카드결제조회) => frmPmpaViewCreditCardPaymentList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jengsan\Frm카드결제조회.frm(Frm카드결제조회)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewCreditCardPaymentList : Form
    {
        public frmPmpaViewCreditCardPaymentList()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);

            this.txtPano2.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.txtPano2.LostFocus += new EventHandler(eControl_LostFocus);
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPano2)
            {
                txtPano2.Text = ComFunc.SetAutoZero(txtPano2.Text, 8);
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPano2)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
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

            optBun10.Checked = true;

            txtPano2.Text = "";
            txtPart2.Text = "";
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
        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;
            double nTotAmt = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  TO_CHAR(A.TRANDATE,'YYYY-MM-DD') TRANDATE, TO_CHAR(A.TRANDATE,'HH24:MI') TRANTIME,";
            SQL += ComNum.VBLF + "  A.PANO, B.SNAME, A.GBIO, A.DEPTCODE, A.CARDNO, A.PERIOD,";
            SQL += ComNum.VBLF + "  A.INSTPERIOD,";
            SQL += ComNum.VBLF + "  DECODE(A.TRANHEADER,'2',A.TRADEAMT * -1,A.TRADEAMT) TRADEAMT,";
            SQL += ComNum.VBLF + "  DECODE(A.TRANHEADER,'2',A.Ogamt * -1,A.OgAmt) Ogamt,";
            SQL += ComNum.VBLF + "  A.FICODE, A.ACCEPTERNAME, A.ORIGINNO, A.ROWID,a.OgAmt,a.Part,a.InputMethod,";
            SQL += ComNum.VBLF + "  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
            if (txtPart2.Text != "")
            {
                SQL += ComNum.VBLF + "  AND PART = '" + txtPart2.Text + "'";
            }

            if (optBun10.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND GUBUN  = '1'";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND GUBUN  = '2'";
            }
            SQL += ComNum.VBLF + "      AND A.PANO  = B.PANO ";
            if (txtPano2.Text != "")
            {
                SQL += ComNum.VBLF + "  AND A.PANO  = '" + txtPano2.Text + "'";
            }
            SQL += ComNum.VBLF + "ORDER BY TRANDATE, PANO ";

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
                    nTotAmt = 0;
                    ssList_Sheet1.Rows.Count = nRead + 1;

                    for (i = 0; i < nRead; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["TRANDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["TRANTIME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GBIO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["CARDNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["PERIOD"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["INSTPERIOD"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 9].Text = String.Format("{0:###,###,###,###}", VB.Val(dt.Rows[i]["TRADEAMT"].ToString().Trim())) + " ";

                        nTotAmt += VB.Val(dt.Rows[i]["TRADEAMT"].ToString().Trim());

                        ssList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["FICODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ACCEPTERNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ORIGINNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 13].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 14].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();

                        ssList_Sheet1.Cells[i, 15].Text = dt.Rows[i]["Part"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 16].Text = dt.Rows[i]["InputMethod"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 17].Text = String.Format("{0:###,###,###,###}", dt.Rows[i]["OgAmt"].ToString().Trim()) + " ";

                        if (VB.Val(dt.Rows[i]["OGAMT"].ToString().Trim()) != 0)
                        {
                            ssList_Sheet1.Rows[i].BackColor = Color.LightPink;
                        }
                        else
                        {
                            ssList_Sheet1.Rows[i].BackColor = Color.White;
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

            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 8].Text = "합 계";
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 9].Text = String.Format("{0:###,###,###,###}", nTotAmt);

        }

    }
}
