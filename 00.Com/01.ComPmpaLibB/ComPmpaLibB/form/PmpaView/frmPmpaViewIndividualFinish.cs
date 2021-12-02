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
    /// File Name       : frmPmpaViewIndividualFinish.cs
    /// Description     : 개인별 마감
    /// Author          : 안정수
    /// Create Date     : 2017-09-05
    /// Update History  : 2017-11-03
    /// <history> 
    /// 출력 및 마감 버튼 기능 구현 안되어있음.. 실제 사용여부 확인 필요
    /// d:\psmh\OPD\olrepa\Frm개인별마감.frm(Frm개인별마감) => frmPmpaViewIndividualFinish.cs 으로 변경함
    /// </history>    
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm개인별마감.frm(Frm개인별마감)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewIndividualFinish : Form
    {        
        int mnJobSabun = 0;
        ComFunc CF = new ComFunc();
        public frmPmpaViewIndividualFinish()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewIndividualFinish(int GnJobSabun)
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
            this.btnOK.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);
            //this.btnPrint.Click += new EventHandler(eBtnEvent);

            this.txtAmt0.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtAmt1.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtAmt2.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtAmt3.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtAmt4.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtAmt5.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtAmt6.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtToAmt.LostFocus += new EventHandler(eControl_LostFocus);

            this.txtAmt0.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtAmt1.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtAmt2.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtAmt3.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtAmt4.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtAmt5.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtAmt6.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.txtAmt0.LostFocus += new EventHandler(eControl_GotFocus);
            this.txtAmt1.LostFocus += new EventHandler(eControl_GotFocus);
            this.txtAmt2.LostFocus += new EventHandler(eControl_GotFocus);
            this.txtAmt3.LostFocus += new EventHandler(eControl_GotFocus);
            this.txtAmt4.LostFocus += new EventHandler(eControl_GotFocus);
            this.txtAmt5.LostFocus += new EventHandler(eControl_GotFocus);
            this.txtAmt6.LostFocus += new EventHandler(eControl_GotFocus);

            this.txtToAmt.TextChanged += new EventHandler(eControl_TextChanged);
            

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

            btnPrint.Enabled = false;
            btnOK.Enabled = false;

            if (mnJobSabun != 0)
            {
                txtSabun.Text = mnJobSabun.ToString("00000");
            }

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

            else if (sender == this.btnOK)
            {
                
            }

            else if (sender == this.btnCancel)
            {
                ComFunc.SetAllControlClear(panel1);
                txtAmt0.Text = "";
                txtAmt1.Text = "";
                txtAmt2.Text = "";
                txtAmt3.Text = "";
                txtAmt4.Text = "";
                txtAmt5.Text = "";
                txtAmt6.Text = "";
                txtAmt8.Text = "";
                txtAmt9.Text = "";

                txtPAmt0.Text = "";
                txtPAmt1.Text = "";
                txtPAmt2.Text = "";
                txtPAmt3.Text = "";
                txtPAmt4.Text = "";
                txtPAmt5.Text = "";
                txtPAmt6.Text = "";               
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(sender == txtAmt0 && e.KeyChar == 13)
            {
                txtAmt1.Focus();
            }

            else if (sender == txtAmt1 && e.KeyChar == 13)
            {
                txtAmt2.Focus();
            }

            else if (sender == txtAmt2 && e.KeyChar == 13)
            {
                txtAmt3.Focus();
            }

            else if (sender == txtAmt3 && e.KeyChar == 13)
            {
                txtAmt4.Focus();
            }

            else if (sender == txtAmt4 && e.KeyChar == 13)
            {
                txtAmt5.Focus();
            }

            else if (sender == txtAmt5 && e.KeyChar == 13)
            {
                txtAmt6.Focus();
            }

            else if (sender == txtAmt6 && e.KeyChar == 13)
            {
                txtAmt8.Focus();
            }
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            int i = 0;
            double nTotAmt = 0;
            double nSumAmt = 0;
           

            if (sender == this.txtAmt0)
            {
                //txtPAmt0.Text = String.Format("{0:###,###,##0}", VB.Val(txtAmt0.Text) * 10);
                txtPAmt0.Text = (VB.Val(txtAmt0.Text) * 10).ToString();
            }

            else if (sender == this.txtAmt1)
            {
                //txtPAmt1.Text = String.Format("{0:###,###,##0}", VB.Val(txtAmt1.Text) * 50);
                txtPAmt1.Text = (VB.Val(txtAmt1.Text) * 50).ToString();
            }

            else if (sender == this.txtAmt2)
            {
                //txtPAmt2.Text = String.Format("{0:###,###,##0}", VB.Val(txtAmt2.Text) * 100);
                txtPAmt2.Text = (VB.Val(txtAmt2.Text) * 100).ToString();
            }


            else if (sender == this.txtAmt3)
            {
                //txtPAmt3.Text = String.Format("{0:###,###,##0}", VB.Val(txtAmt3.Text) * 500);
                txtPAmt3.Text = (VB.Val(txtAmt3.Text) * 500).ToString();
            }


            else if (sender == this.txtAmt4)
            {
                //txtPAmt4.Text = String.Format("{0:###,###,##0}", VB.Val(txtAmt4.Text) * 1000);
                txtPAmt4.Text = (VB.Val(txtAmt4.Text) * 1000).ToString();
            }


            else if (sender == this.txtAmt5)
            {
                //txtPAmt5.Text = String.Format("{0:###,###,##0}", VB.Val(txtAmt5.Text) * 5000);
                txtPAmt5.Text = (VB.Val(txtAmt5.Text) * 5000).ToString();
            }

            else if (sender == this.txtAmt6)
            {
                //txtPAmt6.Text = String.Format("{0:###,###,##0}", VB.Val(txtAmt6.Text) * 10000);
                txtPAmt6.Text = (VB.Val(txtAmt6.Text) * 10000).ToString();
            }

            else if (sender == this.txtToAmt)
            {
                //txtPToAmt.Text = String.Format("{0:###########}", VB.Val(txtAmt8.Text)) + String.Format("{0:###########}", VB.Val(txtAmt9.Text)) +
                //        String.Format("{0:##########}", VB.Val(txtToAmt.Text));
                //txtPToAmt.Text = Convert.ToDouble((Convert.ToInt32(VB.Val(txtAmt8.Text)) + 
                //                 Convert.ToInt32(VB.Val(txtAmt9.Text)) + 
                //                 Convert.ToInt32(VB.Val(txtPToAmt.Text)))).ToString();
            }
            //TODO : 합 부분 다시 확인 필요
            nSumAmt += Convert.ToDouble(
                        Convert.ToInt32(VB.Val(txtPAmt0.Text)) +
                        Convert.ToInt32(VB.Val(txtPAmt1.Text)) +
                        Convert.ToInt32(VB.Val(txtPAmt2.Text)) +
                        Convert.ToInt32(VB.Val(txtPAmt3.Text)) +
                        Convert.ToInt32(VB.Val(txtPAmt4.Text)) +
                        Convert.ToInt32(VB.Val(txtPAmt5.Text)) +
                        Convert.ToInt32(VB.Val(txtPAmt6.Text))
                       );
            
            txtAmt8.Text = nSumAmt.ToString();           

            nTotAmt += Convert.ToInt32(txtAmt8.Text) + Convert.ToInt32(VB.Val(txtAmt9.Text)) + VB.Val(txtToAmt.Text);            

            txtPToAmt.Text = String.Format("{0:###,###,###,##0}", nTotAmt);
        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            if ( sender == this.txtAmt0)
            {
                TextBox tP = sender as TextBox;
                tP.SelectionStart = 0;
                tP.SelectionLength = tP.Text.Length;
            }

            else if (sender == this.txtAmt1)
            {
                TextBox tP = sender as TextBox;
                tP.SelectionStart = 0;
                tP.SelectionLength = tP.Text.Length;
            }

            else if (sender == this.txtAmt2)
            {
                TextBox tP = sender as TextBox;
                tP.SelectionStart = 0;
                tP.SelectionLength = tP.Text.Length;
            }

            else if (sender == this.txtAmt3)
            {
                TextBox tP = sender as TextBox;
                tP.SelectionStart = 0;
                tP.SelectionLength = tP.Text.Length;
            }

            else if (sender == this.txtAmt4)
            {
                TextBox tP = sender as TextBox;
                tP.SelectionStart = 0;
                tP.SelectionLength = tP.Text.Length;
            }

            else if (sender == this.txtAmt5)
            {
                TextBox tP = sender as TextBox;
                tP.SelectionStart = 0;
                tP.SelectionLength = tP.Text.Length;
            }

            else if (sender == this.txtAmt6)
            {
                TextBox tP = (TextBox)sender;
                tP.SelectionStart = 0;
                tP.SelectionLength = tP.Text.Length;
            }
        }

        void eControl_TextChanged(object sender, EventArgs e)
        {
            if( sender == this.txtToAmt)
            {
                txtToAmt.Text = String.Format("{0:###,###,###0}", txtToAmt.Text);
                txtToAmt.SelectionStart = VB.Len(txtToAmt.Text);
            }
        }

        void eGetData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  SUM(CASE WHEN TRANHEADER ='11' OR TRANHEADER = '1' THEN TRADEAMT            ";
            SQL += ComNum.VBLF + "  WHEN TRANHEADER ='22' OR TRANHEADER = '2' THEN  (-1)*TRADEAMT END ) TAMT    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV                                        ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND ACTDATE    = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')           ";
            SQL += ComNum.VBLF + "      AND TRANHEADER IN ('11','22','1','2')                                   ";
            SQL += ComNum.VBLF + "      AND PART       = '" + txtSabun.Text.Trim() + "'                         ";
            SQL += ComNum.VBLF + "      AND PART       <> 'Y'                                                   ";
            SQL += ComNum.VBLF + "      AND PART <> '4349'                                                      ";
            SQL += ComNum.VBLF + "      AND PANO       <> '81000004'                                            ";
            SQL += ComNum.VBLF + "      AND (GUBUN = '1' OR GUBUN IS NULL)                                      ";
            
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
                    //카드마감
                    //txtAmt9.Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[0]["TAMT"].ToString().Trim()));
                    txtAmt9.Text = VB.Val(dt.Rows[0]["TAMT"].ToString().Trim()).ToString();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            btnOK.Enabled = true;

            txtAmt0.Focus();
        }
       
    }
}
