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
    /// File Name       : frmPmpaViewJinRyoHistory.cs
    /// Description     : 진료내역조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-18
    /// Update History  : 2017-11-06
    /// frmPmpaCheckNhic 폼 호출시 ArgGkiho에 들어가는 값 확인필요
    /// <history>           
    /// d:\psmh\OPD\oviewa\OVIEWA05.FRM(FrmJinRuHistory) => frmPmpaViewJinRyoHistory.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA05.FRM(FrmJinRuHistory)
    /// </seealso>
    /// </summary>    
    public partial class frmPmpaViewJinRyoHistory : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        private frmPmpaViewSname frmPmpaViewSnameX = null;

        int nFlag = 0;
        string[] strBis = new string[100];
        string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

        string mstrRetValue = "";
        string mstrHelpCode = "";
        string GstrPANO = "";

        string mstrView2 = clsPmpaPb.gstrView2;

        public frmPmpaViewJinRyoHistory()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnPanExit.Click += new EventHandler(eBtnEvent);

            //자격조회
            this.btnNHic.Click += new EventHandler(eBtnEvent);

            this.btnOk.Click += new EventHandler(eBtnEvent);
            this.btnNextAcc.Click += new EventHandler(eBtnEvent);
            this.btnSnameShow.Click += new EventHandler(eBtnEvent);
            this.btnJin_View.Click += new EventHandler(eBtnEvent);

            this.optChoice0.CheckedChanged += new EventHandler(eBtnEvent);
            this.optChoice1.CheckedChanged += new EventHandler(eBtnEvent);
            this.optChoice2.CheckedChanged += new EventHandler(eBtnEvent);

            this.optIO0.CheckedChanged += new EventHandler(eBtnEvent);
            this.optIO1.CheckedChanged += new EventHandler(eBtnEvent);
            this.optIO2.CheckedChanged += new EventHandler(eBtnEvent);

            this.txtAcc.GotFocus += new EventHandler(eControl_GotFocus);
            this.txtAcc.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtAcc.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            if(nFlag == 1)
            {
                txtAcc.ImeMode = ImeMode.Hangul;
            }

            TextBox tP = sender as TextBox;
            tP.SelectionStart = 0;
            tP.SelectionLength = tP.Text.Length;
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if(optChoice1.Checked == true)
            {
                txtAcc.Text = ComFunc.SetAutoZero(txtAcc.Text, 8);
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if(txtAcc.Text.Trim() == "")
                {
                    return;
                }
                if(optChoice1.Checked == true)
                {
                    txtAcc.Text = ComFunc.SetAutoZero(txtAcc.Text, 8);
                }
                btnOk.Focus();
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    return;
            //    this.Close(); //폼 권한 조회
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등        

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            panList.Visible = false;
            optChoice0.Checked = true;
            optIO0.Checked = true;

            Str_Bis();
            optChoice0.Checked = true;
            nFlag = 1;

            panList.Visible = false;

            if(mstrRetValue != "")
            {
                optChoice1.Checked = true;

                if(txtAcc.Text != "")
                {
                    txtAcc.Text = mstrRetValue;
                    btnOk_Click();
                }
                else
                {
                    txtAcc.Text = mstrRetValue;
                    btnOk_Click();
                }

            }

            mstrRetValue = "";
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnPanExit)
            {
                panList.Visible = false;
            }

            else if (sender == this.btnJin_View)
            {
                panList.Visible = true;
                panList.Dock = DockStyle.Right;
            }

            else if (sender == this.btnNHic)
            {
                btnNhic_Click();
            }

            else if (sender == this.btnNextAcc)
            {
                btnNextAcc_Click();
            }

            else if (sender == this.btnOk)
            {
                btnOk_Click();
            }          

            else if (sender == this.btnSnameShow)
            {
                btnSnameShow_Click();
            }

            else if (sender == this.optChoice0 || sender == this.optChoice1 || sender == this.optChoice2)
            {
                btnSnameShow.Visible = false;

                if(sender == this.optChoice0)
                {
                    nFlag = 1;
                }
                else
                {
                    nFlag = 2;
                }

                //등록번호 자동입력 2012-04-03 이주형
                if(sender == optChoice1)
                {
                    if (GstrPANO != "" || GstrPANO != null)
                    {
                        txtAcc.Text = GstrPANO;
                    }
                }
                else
                {
                    txtAcc.Text = "";
                }
            }

            else if ( sender == this.optIO0 || sender == this.optIO1 || sender == this.optIO2)
            {
                btnOk_Click();
            }
        }

        void btnNhic_Click()
        {
            int i = 0;
            string strSname = "";
            string strPano = "";
            string strDeptCode = "";
            string strJumin = "";
            string strToDate = "";

            strToDate = CurrentDate;
            strSname = txt1.Text;
            strPano = txt0.Text;
            strJumin = txt3.Text + txt4.Text;

            //최종마지막 과선택
            strDeptCode = ssList1_Sheet1.Cells[0, 4].Text;

            if(strPano == "")
            {
                ComFunc.MsgBox("환자등록번호가 공란입니다.");
                return;
            }

            if (strDeptCode == "")
            {
                strDeptCode = "Me";
                ComFunc.MsgBox("최종진료과가 없습니다.. 자격조회시 ME로 자격조회합니다...");
            }

            if(strPano == "06927136")
            {
                strSname = "마리벨시파곤산";
            }

            //2012-09-11
            if(VB.I(strSname, "A") > 1 || VB.I(strSname, "B") > 1 || VB.I(strSname, "C") > 1 || VB.I(strSname, "D") > 1)
            {
                if (MessageBox.Show("성명에 A,B,C,D가 포함되어 있습니다...성명수정후 자격조회 하시겠습니까??", "작업선택", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    strSname = VB.InputBox("성명을 다시 확인하십시오.");
                }
                else
                {
                    strSname = strSname;
                }
            }

            mstrHelpCode = "";
            mstrHelpCode = strPano + "," + strDeptCode + "," + strSname + "," + strJumin + "," + strToDate;

            //TODO : frmPmpaCheckNhic 불러오는 부분 확인 필요
            //등록번호, 진료과, 이름, 주민번호1, 주민번호2, 날짜, Gkiho
            frmPmpaCheckNhic f = new frmPmpaCheckNhic(strPano, strDeptCode, strSname, txt3.Text, txt4.Text, strToDate, ssList1_Sheet1.Cells[ssList1_Sheet1.ActiveRowIndex, 9].Text.Replace("-", ""));
            //frmPmpaCheckNhic f = new frmPmpaCheckNhic(strPano, strDeptCode, strSname, txt3.Text, txt4.Text, strToDate, null);
            f.Show();
            //Frm보험자격확인NEW2.Show 1

            mstrHelpCode = "";

        }

        void btnNextAcc_Click()
        {
            int i = 0;

            ComFunc.SetAllControlClear(panel3);

            SS_Setting();

            btnSnameShow.Visible = false;
            txtAcc.Enabled = true;
            btnOk.Enabled = true;
            txtAcc.Text = "";
            txtAcc.Focus();

            panList.Visible = false;
        }

        void SS_Setting()
        {
            ssList1_Sheet1.Rows.Count = 20;
            ssList1_Sheet1.Cells[0, 0, ssList1_Sheet1.Rows.Count - 1, ssList1_Sheet1.Columns.Count - 1].Text = "";
        }

        void btnOk_Click()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            clsPmpaPb.gstrView2 = "";

            if(txtAcc.Text.Trim() == "")
            {
                return;
            }

            btnSnameShow.Visible = false;
            
            if(optChoice0.Checked == true)
            {
                clsPmpaPb.GstrView1 = "1^^";

                if(VB.Len(txtAcc.Text) < 2)
                {
                    return;
                }

                btnSnameShow.Visible = true;

                clsPmpaPb.GnStart = 1;
                clsPmpaPb.GstrView1 += txtAcc.Text + "^^";
                
                frmPmpaViewSnameX = new frmPmpaViewSname(clsPmpaPb.GstrView1);                
                frmPmpaViewSnameX.rSendText += new frmPmpaViewSname.SendText(GetText);
                frmPmpaViewSnameX.rEventExit += new frmPmpaViewSname.EventExit(frmPmpaViewSnameX_rEventExit);
                frmPmpaViewSnameX.Show();
               
                
                clsPmpaPb.GnChoice = 0;
                
                //PanelCap();
            }

            else if(optChoice1.Checked == true)
            {
                clsPmpaPb.GstrView1 = "2^^";

                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                                                        ";
                SQL += ComNum.VBLF + "  Pano,Sname,Sex,Jumin1,Jumin2,                                                                               ";
                SQL += ComNum.VBLF + "  TO_CHAR(StartDate,'yyyy-mm-dd') StartDate,                                                                  ";
                SQL += ComNum.VBLF + "  TO_CHAR(LastDate,'yyyy-mm-dd') LastDate,JiName,P.ZipCode1,                                                  ";
                SQL += ComNum.VBLF + "  P.ZipCode2,ZipName1,ZipName2,ZipName3,Juso,Tel, Hphone, P.ROWID                                             ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT P, " + ComNum.DB_PMPA + "BAS_AREA A, " + ComNum.DB_PMPA + "BAS_ZIPS Z  ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";
                SQL += ComNum.VBLF + "      AND P.JiCode = A.JiCode(+)                                                                              ";
                SQL += ComNum.VBLF + "      AND P.ZipCode1 = Z.ZipCode1(+)                                                                          ";
                SQL += ComNum.VBLF + "      AND P.ZipCode2 = Z.ZipCode2(+)                                                                          ";
                SQL += ComNum.VBLF + "      AND Pano = '" + txtAcc.Text + "'                                                                        ";
                SQL += ComNum.VBLF + "ORDER BY Jumin1,Jumin2                                                                                        ";                

                try
                {
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    clsPmpaPb.gstrView2 = "";

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
                        clsPmpaPb.gstrView2 = dt.Rows[0]["ROWID"].ToString().Trim();
                    }
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
                dt.Dispose();
                dt = null;

                clsPmpaPb.GnChoice = 0;
                mstrView2 = clsPmpaPb.gstrView2;
                PanelCap();
                HistoryView();
                All_JepsuData_View();
                Read_Opd_Nhic_Display();
            }

            else if(optChoice2.Checked == true)
            {
                clsPmpaPb.GstrView1 = "3^^";

                if(VB.Len(txtAcc.Text) < 2)
                {
                    return;
                }
                btnSnameShow.Visible = true;
                clsPmpaPb.GnStart = 1;
                clsPmpaPb.GstrView1 += txtAcc.Text + "^^";
                
                frmPmpaViewSnameX = new frmPmpaViewSname(clsPmpaPb.GstrView1);
                frmPmpaViewSnameX.rSendText += new frmPmpaViewSname.SendText(GetText);
                frmPmpaViewSnameX.rEventExit += new frmPmpaViewSname.EventExit(frmPmpaViewSnameX_rEventExit);
                frmPmpaViewSnameX.Show();

                clsPmpaPb.GnChoice = 0;
                mstrView2 = clsPmpaPb.gstrView2;
                PanelCap();
                HistoryView();
                All_JepsuData_View();
                Read_Opd_Nhic_Display();
            }
            txtAcc.Enabled = false;
            btnOk.Enabled = false;
            panList.Visible = false;
        }

        void GetText(string str)
        {
            clsPmpaPb.gstrView2 = str;
            mstrView2 = clsPmpaPb.gstrView2;
            PanelCap();
            HistoryView();
            All_JepsuData_View();
            Read_Opd_Nhic_Display();
        }

        void frmPmpaViewSnameX_rEventExit()
        {
            frmPmpaViewSnameX.Dispose();
            frmPmpaViewSnameX = null;
        }

        void Read_Opd_Nhic_Display()
        {
            int i = 0;
            int nREAD = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList9_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                        ";
            SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,DEPTCODE,JUMIN_NEW    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_NHIC                           ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "      AND PANO = '" + txt0.Text + "'                          ";
            SQL += ComNum.VBLF + "      AND MESSAGE IS NULL                                     ";
            SQL += ComNum.VBLF + "      AND JOB_STS ='2'                                        ";
            SQL += ComNum.VBLF + "GROUP BY TO_CHAR(ACTDATE,'YYYY-MM-DD') ,DEPTCODE,JUMIN_NEW    ";
            SQL += ComNum.VBLF + "ORDER BY 1 DESC, 2,3                                          ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;
                    ssList9_Sheet1.Rows.Count = nREAD;

                    for(i = 0; i < nREAD; i++)
                    {
                        ssList9_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList9_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        ssList9_Sheet1.Cells[i, 2].Text = VB.Left(clsAES.DeAES(dt.Rows[i]["JUMIN_NEW"].ToString().Trim()), 6)
                                                        + "-" + VB.Right(clsAES.DeAES(dt.Rows[i]["JUMIN_NEW"].ToString().Trim()), 7);

                        ssList9.ActiveSheet.Rows[i].Height = 18;

                        if(i >= 300)
                        {
                            break;
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
        }

        void All_JepsuData_View()
        {
            int i = 0;
            int nREAD = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            CS.Spread_All_Clear(ssList5);
            ssList5_Sheet1.Rows.Count = 0;

            if(optIO0.Checked == true)
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                        ";
                SQL += ComNum.VBLF + "  'O' GBN,TO_CHAR(ACTDATE,'YYYY-MM-DD') INDATE,DEPTCODE,      ";
                SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') OUTDATE                       ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                         ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                SQL += ComNum.VBLF + "      AND PANO = '" + txt0.Text + "'                          ";

                SQL += ComNum.VBLF + "UNION ALL                                                     ";

                SQL += ComNum.VBLF + "SELECT                                                        ";
                SQL += ComNum.VBLF + "  'I' GBN,TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,DEPTCODE,       ";
                SQL += ComNum.VBLF + "  TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE                       ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                     ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                SQL += ComNum.VBLF + "      AND PANO = '" + txt0.Text + "'                          ";
                SQL += ComNum.VBLF + "      AND GBSTS <> 'D'                                        ";
            }

            else if(optIO1.Checked == true)
            {
                SQL += ComNum.VBLF + "SELECT                                                        ";
                SQL += ComNum.VBLF + "  'O' GBN,TO_CHAR(ACTDATE,'YYYY-MM-DD') INDATE,DEPTCODE,      ";
                SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') OUTDATE                       ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                         ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                SQL += ComNum.VBLF + "      AND PANO = '" + txt0.Text + "'                          ";                
            }

            else if(optIO2.Checked == true)
            {
                SQL += ComNum.VBLF + "SELECT                                                        ";
                SQL += ComNum.VBLF + "  'I' GBN,TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,DEPTCODE,       ";
                SQL += ComNum.VBLF + "  TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE                       ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                     ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                SQL += ComNum.VBLF + "      AND PANO = '" + txt0.Text + "'                          ";
                SQL += ComNum.VBLF + "      AND GBSTS <> 'D'                                        ";
            }

            SQL += ComNum.VBLF + "ORDER BY 1 DESC,2 DESC                                            ";

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
                    ssList5_Sheet1.Rows.Count = nREAD;

                    for(i = 0; i < nREAD; i++)
                    {
                        ssList5_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GBN"].ToString().Trim();
                        ssList5_Sheet1.Cells[i, 1].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssList5_Sheet1.Cells[i, 2].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                        ssList5_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();

                        ssList5.ActiveSheet.Rows[i].Height = 18;
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

        void btnSnameShow_Click()
        {
            frmPmpaViewSnameX = new frmPmpaViewSname(clsPmpaPb.GstrView1);
            frmPmpaViewSnameX.rSendText += new frmPmpaViewSname.SendText(GetText);
            frmPmpaViewSnameX.rEventExit += new frmPmpaViewSname.EventExit(frmPmpaViewSnameX_rEventExit);
            frmPmpaViewSnameX.Show();
            //FrmSname.Show 1
            PanelCap();
            SS_Setting();
            HistoryView();
            
        }

        void HistoryView()
        {
            int i = 0;
            int j = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                            ";
            SQL += ComNum.VBLF + "  IpdOpd,TO_CHAR(SDate,'yyyy-mm-dd') Sdate,                                       ";
            SQL += ComNum.VBLF + "  TO_CHAR(EDate,'yyyy-mm-dd') Edate,Ilsu,DeptCode,                                ";
            SQL += ComNum.VBLF + "  DrName,Bi,Pname,Kiho,Gkiho,Tamt,Jamt,Gamt,                                      ";
            SQL += ComNum.VBLF + "  Mamt,Yamt,BAmt, Samt, IllCode1,IllCode2,IllCode3                                ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_HISTORY H, " + ComNum.DB_PMPA + "BAS_DOCTOR D      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                         ";
            SQL += ComNum.VBLF + "      AND H.Pano = '" + txt0.Text + "'                                            ";
            SQL += ComNum.VBLF + "      AND  H.DrCode = D.DrCode(+)                                                 ";
            
            if(optIO1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND  H.IpdOpd ='O'                                                          ";
            }
            else if(optIO2.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND  H.IpdOpd ='I'                                                          ";
            }
            else
            {
                SQL += ComNum.VBLF + "ORDER BY Sdate DESC                                                           ";
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
                    ssList1_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sdate"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Edate"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Ilsu"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["bi"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Pname"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Kiho"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["Gkiho"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["Tamt"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["Jamt"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Gamt"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["Mamt"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 14].Text = dt.Rows[i]["Yamt"].ToString().Trim() + dt.Rows[i]["Bamt"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 15].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["Samt"].ToString().Trim()));
                        ssList1_Sheet1.Cells[i, 16].Text = dt.Rows[i]["IllCode1"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 17].Text = dt.Rows[i]["IllCode2"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 18].Text = dt.Rows[i]["IllCode3"].ToString().Trim();
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
        //사용 X
        void IDSetting()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                        ";
            SQL += ComNum.VBLF + "  Pano,Sname,Sex,Jumin1,Jumin2,                                                                               ";
            SQL += ComNum.VBLF + "  TO_CHAR(StartDate,'yyyy-mm-dd') StartDate,                                                                  ";
            SQL += ComNum.VBLF + "  TO_CHAR(LastDate,'yyyy-mm-dd') LastDate,JiName,P.ZipCode1,                                                  ";
            SQL += ComNum.VBLF + "  P.ZipCode2,ZipName1,ZipName2,ZipName3,Juso,Tel, Hphone                                                      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT P, " + ComNum.DB_PMPA + "BAS_AREA A, " + ComNum.DB_PMPA + "BAS_ZIPS Z  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";
            SQL += ComNum.VBLF + "      AND P.JiCode = A.JiCode(+)                                                                              ";
            SQL += ComNum.VBLF + "      AND P.ZipCode1 = Z.ZipCode1(+)                                                                          ";
            SQL += ComNum.VBLF + "      AND P.ZipCode2 = Z.ZipCode2(+)                                                                          ";
            if(nFlag == 1)
            {
                SQL += ComNum.VBLF + "  AND SName LIKE '" + txtAcc.Text + "%'                                                                   ";
                SQL += ComNum.VBLF + "ORDER BY Jumin1,Jumin2                                                                                    ";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND Pano = '" + txtAcc.Text + "'                                                                        ";
                SQL += ComNum.VBLF + "ORDER BY Jumin1,Jumin2                                                                                    ";
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

        void PanelCap()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if(mstrView2 == "")
            {
                return;
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                        ";
            SQL += ComNum.VBLF + "  Pano,Sname,Sex,Jumin1,Jumin2,Jumin3,                                                                        ";
            SQL += ComNum.VBLF + "  TO_CHAR(StartDate,'yyyy-mm-dd') StartDate,                                                                  ";
            SQL += ComNum.VBLF + "  TO_CHAR(LastDate,'yyyy-mm-dd') LastDate,JiName,P.ZipCode1,p.Remark,                                         ";
            SQL += ComNum.VBLF + "  P.ZipCode2,ZipName1,ZipName2,ZipName3,Juso,Tel, Hphone                                                      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT P, " + ComNum.DB_PMPA + "BAS_AREA A, " + ComNum.DB_PMPA + "BAS_ZIPS Z  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";
            SQL += ComNum.VBLF + "      AND P.JiCode = A.JiCode(+)                                                                              ";
            SQL += ComNum.VBLF + "      AND P.ZipCode1 = Z.ZipCode1(+)                                                                          ";
            SQL += ComNum.VBLF + "      AND P.ZipCode2 = Z.ZipCode2(+)                                                                          ";
            SQL += ComNum.VBLF + "      AND P.ROWID ='" + mstrView2 + "'                                                                        ";


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
                    txt0.Text = dt.Rows[0]["Pano"].ToString().Trim();
                    txt1.Text = dt.Rows[0]["Sname"].ToString().Trim();
                    txt2.Text = dt.Rows[0]["Sex"].ToString().Trim();
                    txt3.Text = dt.Rows[0]["Jumin1"].ToString().Trim();
                    txt4.Text = clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                    txt5.Text = dt.Rows[0]["StartDate"].ToString().Trim();
                    txt6.Text = dt.Rows[0]["LastDate"].ToString().Trim();
                    txt7.Text = dt.Rows[0]["JiName"].ToString().Trim();
                    txt8.Text = dt.Rows[0]["ZipCode1"].ToString().Trim();
                    txt8.Text += dt.Rows[0]["ZipCode2"].ToString().Trim();
                    txt9.Text = dt.Rows[0]["ZipName1"].ToString().Trim();
                    txt9.Text += dt.Rows[0]["ZipName2"].ToString().Trim();
                    txt9.Text += dt.Rows[0]["ZipName3"].ToString().Trim();
                    txt10.Text = dt.Rows[0]["Juso"].ToString().Trim();
                    txt11.Text = dt.Rows[0]["Tel"].ToString().Trim();
                    txt11.Text += "  H.P(" + dt.Rows[0]["Hphone"].ToString().Trim() + ")";

                    txt12.Text = dt.Rows[0]["Remark"].ToString().Trim();

                    //등록번호 자동입력을 위한 전역 변수 2012-04-03 이주형
                    GstrPANO = dt.Rows[0]["Pano"].ToString().Trim();
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

        void Str_Bis()
        {
            strBis[11] = "공단";
            strBis[12] = "직장";
            strBis[13] = "지역";
            strBis[14] = "";
            strBis[15] = "";

            strBis[21] = "보호1";
            strBis[22] = "보호2";
            strBis[23] = "보호3";
            strBis[24] = "행려";
            strBis[25] = "";

            strBis[31] = "산재";
            strBis[32] = "공상";
            strBis[33] = "산재공상";
            strBis[34] = "";
            strBis[35] = "";

            strBis[41] = "공단180";
            strBis[42] = "직장180";
            strBis[43] = "지역180";
            strBis[44] = "가족계획";
            strBis[45] = "보험계약";

            strBis[51] = "일반";
            strBis[52] = "TA보험";
            strBis[53] = "계약";
            strBis[54] = "미확인";
            strBis[55] = "TA일반";
        }
    }
}
