using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewSunap.cs
    /// Description     : 조별 수납 리스트
    /// Author          : 안정수
    /// Create Date     : 2017-12-20
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\olrepa\OLREPA18_New.FRM(FrmSunap_New) => frmPmpaViewSunap.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\OLREPA18_New.FRM(FrmSunap_New)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewSunap : Form
    {
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;        

        string mstrJobName = "";

        //int nSelect = 0;
        //int nSelect1 = 0;
        //int nSel1 = 0;
        //int nSel2 = 0;
        //int nSel3 = 0;

        //string strFDate = "";
        //string strFDate1 = "";

        //string strTDate = "";
        //string strTDate1 = "";
        ///////////////////////////
        //string strGwa = "";
        //int nPano = 0;
        //string strname = "";
        //string strBiGubun = "";
        //int nBi = 0;
        //int nBun = 0;

        //double nAmt = 0;

        double[] nAmt1 = new double[5];
        double[] nAmt2 = new double[5];
        double[] nAmt3 = new double[5];
        double[] nAmt4 = new double[5];
        double[] nAmt5 = new double[5];
        double[] nAmt6 = new double[5];
        double[] nAmt7 = new double[5];

        //int nCount1 = 0;

        public frmPmpaViewSunap()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewSunap(string GstrJobName)
        {
            InitializeComponent();
            setEvent();
            mstrJobName = GstrJobName;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnClose.Click += new EventHandler(eBtnEvent);

            this.btnPrint.Click += new EventHandler(eBtnEvent);
            

            this.btnOK.Click += new EventHandler(eBtnEvent);
            this.btnOK2.Click += new EventHandler(eBtnEvent);
            this.ssList1.CellClick += new CellClickEventHandler(Spread_Click);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnView2.Click += new EventHandler(eBtnEvent);
            this.btnView3.Click += new EventHandler(eBtnEvent);
            this.btnView4.Click += new EventHandler(eBtnEvent);

            this.btn점검.Click += new EventHandler(eBtnEvent);

            this.txtPano.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtPart.LostFocus += new EventHandler(eControl_LostFocus);

            this.txtPano.GotFocus += new EventHandler(eControl_GotFocus);

            this.txtPano.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
        }
        
        //스프레드정렬

        void Spread_Click(object sender, CellClickEventArgs e)
        {
            clsSpread CS = new clsSpread();
            ComFunc CF = new ComFunc();
            clsPmpaFunc CPF = new clsPmpaFunc();

            if (sender == this.ssList1)
            {
                if (e.ColumnHeader == true)
                {
                    CS.setSpdSort(ssList1, e.Column, true);
                    return;
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

            //else
            {
                //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  

                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

                // if(clsType.User.Sabun != "0")
                // {
                //     txtPart.Text = ComFunc.SetAutoZero(clsType.User.Sabun, 5);
                // }
                if (clsType.User.IdNumber != "0")
                {
                    txtPart.Text = clsType.User.IdNumber;
                }

              
                txtPano.Text = "";
                txtAmt.Text = "";

                txtPaCnt2.Text = "";
                txtPaCnt3.Text = "";

                btnOK.Visible = true;

                panel3.Visible = false;
                ssList3.Visible = false;
                collapsibleSplitContainer1.SplitterDistance = 1004;

                ssList2.Visible = false;
                optSort0.Checked = true;
                chkEnd.Checked = true;
                chkTax.Checked = true;
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
              //                
              //  if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
             //   {
              //      return; //권한 확인
             //   }
                btnView_Click();
            }

            else if (sender == this.btnView2)
            {
                btnView2_Click();
            }

            else if (sender == this.btnView3)
            {
                btnView3_Click();
            }

            else if (sender == this.btnView4)
            {
                btnView4_Click();
            }

            else if (sender == this.btn점검)
            {
                panel3.Visible = true;
                ssList3.Visible = true;
                collapsibleSplitContainer1.SplitterDistance = 753;
            }

            else if (sender == this.btnClose)
            {
                panel3.Visible = false;
                ssList3.Visible = false;
                collapsibleSplitContainer1.SplitterDistance = 1004;
            }

            else if (sender == this.btnOK)
            {
                btnOK_Click();
            }

            else if (sender == this.btnOK2)
            {
                btnOK2_Click();
            }

            else if (sender == this.btnPrint)
            {
                //                
               // if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
               // {
               //     return; //권한 확인
              //  }
                ePrint();
            }
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPano)
            {
                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
            }

            else if (sender == this.txtPart)
            {
                txtPart.Text = txtPart.Text.ToUpper();
            }
        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            txtPart.SelectionStart = 0;
            txtPart.SelectionLength = VB.Len(txtPart.Text);
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void btnView2_Click()
        {
            string strSabun = "";
            string strDate = "";

            strSabun = txtPart.Text;
            strDate = dtpDate.Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  COUNT(PANO) CNT";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND PART  = '" + strSabun + "'";
            SQL += ComNum.VBLF + "      AND RESERVED <> '1'";
            SQL += ComNum.VBLF + "      AND (AGE >= 65 OR BOHUN = '3')";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                txtPaCnt.Text = String.Format("{0:###,###}", dt.Rows[0]["CNT"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;
        }

        void btnView3_Click()
        {
            string strSabun = "";
            string strDate = "";

            strSabun = txtPart.Text;
            strDate = dtpDate.Text;

            if(strSabun == "")
            {
                ComFunc.MsgBox("작업자가 없으면 야간소아과 및 주간소아과 모두 조회됩니다..");
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  COUNT(PANO) CNT";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            if(strSabun != "")
            {
                SQL += ComNum.VBLF + "  AND PART  = '" + strSabun + "' ";
            }            
            SQL += ComNum.VBLF + "      AND RESERVED <> '1'";
            SQL += ComNum.VBLF + "      AND DeptCode ='PD'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                txtPaCnt2.Text = String.Format("{0:###,###}", dt.Rows[0]["CNT"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;
        }

        void btnView4_Click()
        {
            string strSabun = "";
            string strDate = "";

            strSabun = txtPart.Text;
            strDate = dtpDate.Text;

            if (strSabun == "")
            {
                ComFunc.MsgBox("작업자가 없으면 모두 조회됩니다..!");
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  COUNT(PANO) CNT";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            if (strSabun != "")
            {
                SQL += ComNum.VBLF + "  AND PART  = '" + strSabun + "' ";
            }
            SQL += ComNum.VBLF + "      AND RESERVED <> '1'";
            SQL += ComNum.VBLF + "      AND GbFlu_Ltd ='Y'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                txtPaCnt3.Text = String.Format("{0:###,###}", dt.Rows[0]["CNT"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;
        }

        void btnOK_Click()
        {
            int i = 0;

            string strBun = "";
            string strPano = "";
            string strDate = "";
            string strSabun = "";
            string strDept = "";

            double nAmt = 0;
            double nAmt2 = 0;

            int nSeqno = 0;

            strSabun = txtPart.Text;
            strDate = dtpDate.Text;
            Cursor.Current = Cursors.WaitCursor;

            for (i = 0; i < ssList1.ActiveSheet.Rows.Count; i++)
            {
                strPano = ssList1.ActiveSheet.Cells[i, 0].Text;
                strBun = ssList1.ActiveSheet.Cells[i, 4].Text;
                nAmt = VB.Val(String.Format("{0:##########0}", ssList1.ActiveSheet.Cells[i, 2].Text));
                nSeqno = Convert.ToInt32(VB.Val(ssList1.ActiveSheet.Cells[i, 12].Text));
                strDept = ssList1.ActiveSheet.Cells[i, 5].Text;
                ssList1.ActiveSheet.Cells[i, 14].Text = "";

                if(strBun == "접수비" || strBun == "접수비(전화)" || strBun == "대리접수")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  AMT7";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_master";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND PANO = '" + strPano + "'";
                    SQL += ComNum.VBLF + "      AND PART  = '" + strSabun + "'";
                    SQL += ComNum.VBLF + "      AND DEPTCODE = '" + strDept + "'";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if(dt.Rows.Count == 0)
                    {
                        nAmt2 = 0;
                    }

                    else
                    {
                        nAmt2 = VB.Val(dt.Rows[0]["AMT7"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;

                    if(nAmt != nAmt2)
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인요망";
                    }

                    else
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인";
                    }
                }

                else if(strBun == "수납" || strBun == "환불")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  SUM(AMT1+amt2) AMT1";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND PANO = '" + strPano + "'";
                    SQL += ComNum.VBLF + "      AND PART  = '" + strSabun + "'";

                    if(strDept == "II")
                    {
                        SQL += ComNum.VBLF + "  AND WARDCODE = 'II'";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "  AND DEPTCODE  = '" + strDept + "'";
                    }
                    
                    SQL += ComNum.VBLF + "      AND BUN  = '99'";
                    SQL += ComNum.VBLF + "      AND SEQNO = " + nSeqno + " ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if(dt.Rows.Count == 0)
                    {
                        nAmt2 = 0;
                    }

                    else
                    {
                        nAmt2 = VB.Val(dt.Rows[0]["AMT1"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;

                    if(nAmt != nAmt2)
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인요망";
                    }
                    else
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인";
                    }
                }

                else if(strBun == "수납+예약")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  SUM(AMT1+amt2) AMT1";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND PART  = '" + strSabun + "'";
                    SQL += ComNum.VBLF + "      AND PANO = '" + strPano + "'";
                    SQL += ComNum.VBLF + "      AND DEPTCODE  = '" + strDept + "'";
                    SQL += ComNum.VBLF + "      AND BUN  = '99'";
                    SQL += ComNum.VBLF + "      AND SEQNO = " + nSeqno + " ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if(dt.Rows.Count == 0)
                    {
                        nAmt2 = 0;
                    }

                    else
                    {
                        nAmt2 = VB.Val(dt.Rows[0]["AMT1"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  AMT7";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND DATE1 = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND PART  = '" + strSabun + "'";
                    SQL += ComNum.VBLF + "      AND DEPTCODE  = '" + strDept + "'";
                    SQL += ComNum.VBLF + "      AND PANO = '" + strPano + "'";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if(dt.Rows.Count > 0)
                    {
                        nAmt2 += VB.Val(dt.Rows[0]["AMT7"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;

                    if(nAmt != nAmt2)
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인요망";
                    }

                    else
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인";
                    }
                }

                else if(strBun == "부분취소")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  SUM(AMT1+amt2) AMT1";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "      AND PANO = '" + strPano + "'";
                    SQL += ComNum.VBLF + "      AND PART  = '" + strSabun + "'";
                    SQL += ComNum.VBLF + "      AND DEPTCODE  = '" + strDept + "'";
                    SQL += ComNum.VBLF + "      AND BUN  = '99'";
                    SQL += ComNum.VBLF + "      AND SEQNO = " + nSeqno + "";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if(dt.Rows.Count == 0)
                    {
                        nAmt2 = 0;
                    }

                    else
                    {
                        nAmt2 = VB.Val(dt.Rows[0]["AMT1"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;

                    if(nAmt != nAmt2)
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인요망";
                    }
                    else
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인";
                    }
                }

                else if(strBun == "예약비")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  AMT7";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND DATE1 = TO_DATE('" + strDate + "','YYYY-MM-DD') ";                    
                    SQL += ComNum.VBLF + "      AND PART  = '" + strSabun + "'";
                    SQL += ComNum.VBLF + "      AND DEPTCODE  = '" + strDept + "'";
                    SQL += ComNum.VBLF + "      AND PANO = " + strPano + "";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if(dt.Rows.Count > 0)
                    {
                        nAmt2 = VB.Val(dt.Rows[0]["AMT7"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;

                    if (nAmt != nAmt2)
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인요망";
                    }
                    else
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인";
                    }
                }

                else if(strBun == "예약취소")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  RETAMT";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND RETDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "      AND RETPART  = '" + strSabun + "'";
                    SQL += ComNum.VBLF + "      AND DEPTCODE  = '" + strDept + "'";
                    SQL += ComNum.VBLF + "      AND PANO = " + strPano + "";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        nAmt2 = VB.Val(dt.Rows[0]["RETAMT"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;

                    if (nAmt != nAmt2)
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인요망";
                    }
                    else
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인";
                    }
                }

                else if(strBun == "미수입금")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  AMT";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "      AND PANO = " + strPano + "";
                    SQL += ComNum.VBLF + "      AND GUBUN1 = '2'";                    

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        nAmt2 = VB.Val(dt.Rows[0]["AMT"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;

                    if (nAmt != nAmt2)
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인요망";
                    }
                    else
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인";
                    }
                }

                else if(strBun == "접수취소")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  AMT7 * -1 AMT7";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER_DEL";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "      AND PANO = " + strPano + "";
                    SQL += ComNum.VBLF + "      AND DEPTCODE = '" + strDept + "' ";
                    SQL += ComNum.VBLF + "ORDER BY DELDATE DESC";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        nAmt2 = VB.Val(dt.Rows[0]["AMT7"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;

                    if (nAmt != nAmt2)
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인요망";
                    }
                    else
                    {
                        ssList1.ActiveSheet.Cells[i, 14].Text = "확인";
                    }
                }
                nAmt2 = 0;
            }

            Cursor.Current = Cursors.Default;

        }

        void btnOK2_Click()
        {
            int i = 0;
            int k = 0;
            int nRead = 0;
            long nAmt = 0;

            panel3.Visible = true;
            ssList3.Visible = true;
            collapsibleSplitContainer1.SplitterDistance = 753;

            CS.Spread_All_Clear(ssList3);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  Pano,SUM(Amt) SAMT";
            SQL += ComNum.VBLF + "From " + ComNum.DB_PMPA + "OPD_SUNAP";
            SQL += ComNum.VBLF + "Where 1=1";
            SQL += ComNum.VBLF + "      AND Actdate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND PART ='" + txtPart.Text + "'";
            SQL += ComNum.VBLF + "      AND DELDATE IS NULL";
            SQL += ComNum.VBLF + "GROUP By Pano";
            SQL += ComNum.VBLF + "ORDER By PANO";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                nRead = dt.Rows.Count;
                ssList3.ActiveSheet.Rows.Count = nRead;

                for(i = 0; i < nRead; i++)
                {
                    ssList3.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssList3.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SAmt"].ToString().Trim();

                    nAmt = 0;

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  SUM(AMT1+AMT2) SAMT";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "      AND Actdate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND BUN = '99' ";
                    SQL += ComNum.VBLF + "      AND PART = '" + txtPart.Text + "'";

                    SQL += ComNum.VBLF + "Union All";

                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  SUM(AMT1+AMT2) SAMT";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP_RES";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "      AND Actdate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND BUN = '99' ";
                    SQL += ComNum.VBLF + "      AND PART = '" + txtPart.Text + "'";

                    SQL += ComNum.VBLF + "Union All";

                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  AMT7 AS SAMT";
                    SQL += ComNum.VBLF + "From " + ComNum.DB_PMPA + "OPD_MASTER";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "      AND Actdate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND PART = '" + txtPart.Text + "'";
                    SQL += ComNum.VBLF + "      AND RESERVED <> '1'";

                    SQL += ComNum.VBLF + "Union All";

                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  AMT7 AS SAMT";
                    SQL += ComNum.VBLF + "From " + ComNum.DB_PMPA + "OPD_MASTER_DEL";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "      AND Actdate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND PART = '" + txtPart.Text + "'";

                    SQL += ComNum.VBLF + "Union All";

                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  AMT7*-1 AS SAMT";
                    SQL += ComNum.VBLF + "From " + ComNum.DB_PMPA + "OPD_MASTER_DEL";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "      AND Actdate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND DELPART = '" + txtPart.Text + "'";

                    SQL += ComNum.VBLF + "Union All";

                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  AMT7 AS SAMT";
                    SQL += ComNum.VBLF + "From " + ComNum.DB_PMPA + "OPD_RESERVED_NEW";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "      AND DATE1=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND PART = '" + txtPart.Text + "'";
                    SQL += ComNum.VBLF + "Union All";

                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  AMT AS SAMT";
                    SQL += ComNum.VBLF + "From " + ComNum.DB_PMPA + "misu_gainslip";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "      AND bdate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND PART = '" + txtPart.Text + "'  AND gubun1 ='2'   ";


                    SQL += ComNum.VBLF + "Union All";

                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  RETAMT AS SAMT";
                    SQL += ComNum.VBLF + "From " + ComNum.DB_PMPA + "OPD_RESERVED_NEW";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "      AND RETDATE=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND RETPART = '" + txtPart.Text + "'";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if(dt1.Rows.Count > 0)
                    {
                        for(k = 0; k < dt1.Rows.Count; k++)
                        {
                            nAmt += Convert.ToInt64(VB.Val(dt1.Rows[k]["SAMT"].ToString().Trim()));
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  ROWID";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SUNAP";
                    SQL += ComNum.VBLF + "Where 1=1";
                    SQL += ComNum.VBLF + "      AND Actdate=Trunc(sysdate)";
                    SQL += ComNum.VBLF + "      AND Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                    SQL += ComNum.VBLF + "      AND PART ='" + txtPart.Text + "'";
                    SQL += ComNum.VBLF + "      AND BI = 'SS'"; //당일수진예약변경자는 예약취소금액을 빼줌
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if(dt1.Rows.Count > 0)
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  SUM(AMT) SAMT";
                        SQL += ComNum.VBLF + "From " + ComNum.DB_PMPA + "OPD_SUNAP";
                        SQL += ComNum.VBLF + "Where 1=1";
                        SQL += ComNum.VBLF + "      AND Actdate=Trunc(sysdate)";
                        SQL += ComNum.VBLF + "      AND Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "      AND PART ='" + txtPart.Text + "'";
                        SQL += ComNum.VBLF + "      AND DELDATE IS NULL ";
                        SQL += ComNum.VBLF + "      AND TRIM(REMARK) = '예약취소' ";
                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if(dt2.Rows.Count > 0)
                        {
                            nAmt -= Convert.ToInt64(VB.Val(dt2.Rows[0]["SAMT"].ToString().Trim()));
                        }

                        dt2.Dispose();
                        dt2 = null;
                    }

                    dt1.Dispose();
                    dt1 = null;

                    ssList3.ActiveSheet.Cells[i, 2].Text = nAmt.ToString();

                    if(Convert.ToInt64(VB.Val(ssList3.ActiveSheet.Cells[i, 1].Text)) != nAmt)
                    {
                        ssList3.ActiveSheet.Rows[i].BackColor = Color.HotPink;
                    }

                    else
                    {
                        ssList3.ActiveSheet.Rows[i].BackColor = Color.White;
                    }

                }
            }

            dt.Dispose();
            dt = null;

        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            string JobMan = mstrJobName;

            bool PrePrint = true;    

            if(ssList1.ActiveSheet.Rows.Count == 0)
            {
                return;
            }

            btnPrint.Enabled = false;            

            strTitle = "조별 수납 리스트";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, true, false, false);

            SPR.setSpdPrint(ssList1, PrePrint, setMargin, setOption, strHeader, strFooter);

            btnPrint.Enabled = true;
          
        }

        void btnView_Click()
        {
            Cursor.Current = Cursors.WaitCursor;
            string strDate = "";
            
            //ROWID
            ssList1.ActiveSheet.Columns[15].Visible = false;

            int i = 0;

            double nTot = 0;
            double nTotAmt1 = 0;
            double nTotAmt2 = 0;
            double nTotAmt3 = 0;
            double nTotAmt4 = 0;
            double nTotAmt5 = 0;
            double nTotAmt7 = 0;    //2014-02-24 부가세
            double nTotAmt8 = 0;    //2014-02-24 부가세

            long nSTot = 0;
            long nDanAmt = 0;       //2014-04-05

            double nAmt7 = 0;

            ssList1.ActiveSheet.Rows.Count = 0;

            if (chkTax.Checked == true)
            {
                ssList1.ActiveSheet.Columns[22].Visible = true;
                ssList1.ActiveSheet.Columns[23].Visible = true;
            }
            else
            {
                ssList1.ActiveSheet.Columns[22].Visible = false;
                ssList1.ActiveSheet.Columns[23].Visible = false;
            }

            strDate = dtpDate.Text;
            nTot = 0;
            nTotAmt1 = 0;
            nTotAmt2 = 0;
            nTotAmt3 = 0;
            nTotAmt4 = 0;
            nTotAmt5 = 0;
            nTotAmt7 = 0;
            nTotAmt8 = 0;
            nDanAmt = 0;    //2014-04-05

            nSTot = 0;

            CS.Spread_All_Clear(ssList2);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  A.PANO, nvl(B.SNAME,'기타수납')  SNAME , AMT  , STIME,A.REMARK, A.BIGO,a.Part,a.Part2,";
            SQL += ComNum.VBLF + "  A.AMT1   AMT1 , A.AMT2, A.AMT3, A.AMT4, A.AMT5, A.DEPTCODE,";
            SQL += ComNum.VBLF + "  A.BI, A.SEQNO2, A.CARDGB,a.CardAmt,a.EtcAmt, A.YAKAMT,a.WorkGbn,";
            SQL += ComNum.VBLF + "  a.GbSPC,A.ROWID,A.AMT7,A.TaxDan";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SUNAP A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND A.PANO = B.PANO(+)";

            if(chkJoETC.Checked == false)
            {
                if(txtPart.Text != "")
                {
                    SQL += ComNum.VBLF + "AND PART  = '" + txtPart.Text + "' ";
                }
            }
            
            if(txtPano.Text != "")
            {
                SQL += ComNum.VBLF + "  AND a.Pano  = '" + txtPano.Text + "' ";
            }

            if(txtAmt.Text != "")
            {
                if(txtAmt.Text == "-")
                {
                    SQL += ComNum.VBLF + "AND a.AMT  <  0";
                }
                else
                {
                    SQL += ComNum.VBLF + "AND to_char(a.AMT)  = '" + txtAmt.Text + "' ";
                }
            }

            if(clsType.User.Sabun == "4349")
            {
                if (MessageBox.Show("전산실 연습도 포함시키겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    SQL += ComNum.VBLF + "AND A.PANO  <> '81000004'";
                    SQL += ComNum.VBLF + "AND A.PART <> '4349'";
                }             
            }
            else
            {
                SQL += ComNum.VBLF + "  AND A.PANO  <> '81000004'";
                SQL += ComNum.VBLF + "  AND A.PART <> '4349' ";
            }

            SQL += ComNum.VBLF + "      AND (A.DELDATE IS NULL OR A.DELDATE ='')";

            if(chkJepC.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND A.Part2 IS NOT NULL";
            }

            if(chkJepsu.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND A.REMARK IN ( '접수비','접수비(전화)','접수취소','대리','대리접수','대리취소')";
            }

            if(chkAmt0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND  a.Amt <> 0";
            }

            if(optSort0.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY STIME";
            }
            else
            {
                SQL += ComNum.VBLF + "ORDER BY SNAME, STIME";
            }

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ssList1.ActiveSheet.Rows.Count = dt.Rows.Count;
                ssList1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    nAmt7 = VB.Val(dt.Rows[i]["Amt7"].ToString().Trim());
                    nDanAmt = Convert.ToInt64(VB.Val(dt.Rows[i]["TaxDan"].ToString().Trim()));

                    //if(dt.Rows[i]["Amt7"].ToString().Trim() != "0")
                    //{
                    //    i = i;
                    //}

                    nTot += VB.Val(dt.Rows[i]["AMT"].ToString().Trim()) + (nAmt7 - nDanAmt);    //2014-04-05(JJY) 부가세추가
                    nTotAmt1 += VB.Val(dt.Rows[i]["AMT1"].ToString().Trim());
                    nTotAmt2 += VB.Val(dt.Rows[i]["YAKAMT"].ToString().Trim());
                    nTotAmt3 += VB.Val(dt.Rows[i]["AMT3"].ToString().Trim());
                    nTotAmt4 += VB.Val(dt.Rows[i]["AMT4"].ToString().Trim());
                    nTotAmt5 += VB.Val(dt.Rows[i]["AMT5"].ToString().Trim());
                    nTotAmt7 += VB.Val(dt.Rows[i]["AMT7"].ToString().Trim());
                    nTotAmt8 += VB.Val(dt.Rows[i]["TaxDan"].ToString().Trim());

                    //if(dt.Rows[i]["Pano"].ToString().Trim() == "04032746")
                    //{
                    //    i = i;
                    //}

                    ssList1.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[i, 2].Text = String.Format("{0:###,###,##0}", (VB.Val(dt.Rows[i]["AMT"].ToString().Trim()) + (nAmt7 - nDanAmt)));  //2014-04-05(JJY) 부가세 추가
                    ssList1.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["STIME"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[i, 4].Text = " " + dt.Rows[i]["REMARK"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["BI"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[i, 7].Text =  String.Format("{0:###,###,###}", dt.Rows[i]["AMT1"]);
                    ssList1.ActiveSheet.Cells[i, 8].Text =  String.Format("{0:###,###,###}", dt.Rows[i]["YAKAMT"]);
                    ssList1.ActiveSheet.Cells[i, 9].Text =  String.Format("{0:###,###,###}", dt.Rows[i]["AMT3"]);
                    ssList1.ActiveSheet.Cells[i, 10].Text = String.Format("{0:###,###,###}", dt.Rows[i]["AMT4"]);
                    ssList1.ActiveSheet.Cells[i, 11].Text = String.Format("{0:###,###,###}", dt.Rows[i]["AMT5"]);
                    ssList1.ActiveSheet.Cells[i, 12].Text = String.Format("{0:###,###,###}", dt.Rows[i]["SEQNO2"]);

                    switch (dt.Rows[i]["CARDGB"].ToString().Trim())
                    {
                        case "1":
                            ssList1.ActiveSheet.Cells[i, 13].Text = "카드승인";
                            break;
                        case "2":
                            ssList1.ActiveSheet.Cells[i, 13].Text = "현금영수";
                            break;
                    }

                    ssList1.ActiveSheet.Cells[i, 14].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[i, 15].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[i, 16].Text = dt.Rows[i]["WorkGbn"].ToString().Trim();

                    if(dt.Rows[i]["GbSPC"].ToString().Trim() == "1")
                    {
                        ssList1.ActiveSheet.Cells[i, 17].Text = "1";
                    }

                    ssList1.ActiveSheet.Cells[i, 18].Text = dt.Rows[i]["Part"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[i, 19].Text = dt.Rows[i]["Part2"].ToString().Trim();

                    if(VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim()) != 0)
                    {
                        ssList1.ActiveSheet.Cells[i, 20].Text = String.Format("{0:###,###,###}", dt.Rows[i]["EtcAmt"]);
                    }

                    if (VB.Val(dt.Rows[i]["CardAmt"].ToString().Trim()) != 0)
                    {
                        ssList1.ActiveSheet.Cells[i, 21].Text = String.Format("{0:###,###,###}", dt.Rows[i]["CardAmt"]);
                    }

                    if (VB.Val(dt.Rows[i]["Amt7"].ToString().Trim()) != 0)
                    {
                        ssList1.ActiveSheet.Cells[i, 22].Text = String.Format("{0:###,###,###}", dt.Rows[i]["Amt7"]);
                    }

                    if (VB.Val(dt.Rows[i]["TaxDan"].ToString().Trim()) != 0)
                    {
                        ssList1.ActiveSheet.Cells[i, 23].Text = String.Format("{0:###,###,###}", dt.Rows[i]["TaxDan"]);
                    }

                    ssList1.ActiveSheet.Rows[i].Height = 23;
                }

                dt.Dispose();
                dt = null;

                //2006-11-02 원무과 수녀님요청
                ssList1.ActiveSheet.Rows.Count += 1;

                ssList1.ActiveSheet.Cells[ssList1.ActiveSheet.Rows.Count - 1, 1].Text = "합   계";
                ssList1.ActiveSheet.Cells[ssList1.ActiveSheet.Rows.Count - 1, 2].Text = String.Format("{0:###,###,###,##0}", nTot);
                ssList1.ActiveSheet.Cells[ssList1.ActiveSheet.Rows.Count - 1, 7].Text = String.Format("{0:###,###,###,##0}", nTotAmt1);
                ssList1.ActiveSheet.Cells[ssList1.ActiveSheet.Rows.Count - 1, 8].Text = String.Format("{0:###,###,###,##0}", nTotAmt2);
                ssList1.ActiveSheet.Cells[ssList1.ActiveSheet.Rows.Count - 1, 9].Text = String.Format("{0:###,###,###,##0}", nTotAmt3);
                ssList1.ActiveSheet.Cells[ssList1.ActiveSheet.Rows.Count - 1, 10].Text = String.Format("{0:###,###,###,##0}", nTotAmt4);
                ssList1.ActiveSheet.Cells[ssList1.ActiveSheet.Rows.Count - 1, 11].Text = String.Format("{0:###,###,###,##0}", nTotAmt5);
                ssList1.ActiveSheet.Cells[ssList1.ActiveSheet.Rows.Count - 1, 22].Text = String.Format("{0:###,###,###,##0}", nTotAmt7);
                ssList1.ActiveSheet.Cells[ssList1.ActiveSheet.Rows.Count - 1, 23].Text = String.Format("{0:###,###,###,##0}", nTotAmt8);

                //SS1.RowHeight(-1) = 13.78

                if (ssList1.ActiveSheet.Rows.Count > 30 && chkEnd.Checked == true)
                {
                    //-------------------------------
                    //스크롤 이동
                    //SS1.Row = RowIndicator - 26
                    //SS1.Col = 1
                    //SS1.Position = SS_POSITION_UPPER_LEFT
                    //SS1.Action = SS_ACTION_GOTO_CELL

                    ssList1.ShowRow(0, ssList1.ActiveSheet.Rows.Count - 1, FarPoint.Win.Spread.VerticalPosition.Bottom);
                }

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  Remark,COUNT(Pano) CNT";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SUNAP";
                SQL += ComNum.VBLF + "OPD_SUNAP";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                if(txtPart.Text != "")
                {
                    SQL += ComNum.VBLF + "  AND PART  = '" + txtPart.Text + "'";
                }
                
                SQL += ComNum.VBLF + "      AND PANO  <> '81000004'";
                SQL += ComNum.VBLF + "      AND PART NOT IN ('4349','7777' )";
                SQL += ComNum.VBLF + "      AND (DELDATE IS NULL OR DELDATE ='')";
                SQL += ComNum.VBLF + "GROUP BY Remark";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    ssList2.ActiveSheet.Rows.Count = dt.Rows.Count + 1;

                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList2.ActiveSheet.Cells[0, i + 2].Text = dt.Rows[i]["Remark"].ToString().Trim();
                        ssList2.ActiveSheet.Cells[1, i + 2].Text = dt.Rows[i]["Cnt"].ToString().Trim();
                        nSTot += Convert.ToInt64(VB.Val(ssList2.ActiveSheet.Cells[1, i + 2].Text));

                        if(i == ssList2.ActiveSheet.Columns.Count - 3)
                        {
                            break;
                        }
                    }
                }

                ssList2.ActiveSheet.Cells[0, 0].Text = "건수합계";
                ssList2.ActiveSheet.Cells[1, 0].Text = nSTot.ToString();

                dt.Dispose();
                dt = null;


                for(i = 0; i < ssList1_Sheet1.ColumnCount; i ++)
                {
                    ssList1_Sheet1.Columns[i].Width = ssList1_Sheet1.Columns[i].GetPreferredWidth() + 4;
                }

                Cursor.Current = Cursors.Default;
            }
        }
    }
}
