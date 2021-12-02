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
    /// File Name       : frmPmpaViewMirHisPrint.cs
    /// Description     : 연간 진료일수 조회(청구기준)
    /// Author          : 안정수
    /// Create Date     : 2017-09-19
    /// Update History  : 2017-11-06
    /// <history>           
    /// d:\psmh\OPD\oviewa\OVIEWA15.FRM(FrmMirHisPrint) => frmPmpaViewMirHisPrint.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA15.FRM(FrmMirHisPrint)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewMirHisPrint : Form
    {
        int nFlag = 0;
        string[] strBis = new string[61];

        double nOpdAmt = 0;
        double nIpdAmt = 0;
        double nTotAmt = 0;
        string[] strDay = new string[367];

        string mstrView2 = "";

        int nYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));        

        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        private frmPmpaViewSname frmPmpaViewSnameX = null;

        public frmPmpaViewMirHisPrint()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnNextAcc.Click += new EventHandler(eBtnEvent);
            this.btnOK.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnSnameShow.Click += new EventHandler(eBtnEvent);

            //this.dtpYear.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtAcc.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.txtAcc.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtAcc.GotFocus += new EventHandler(eControl_GotFocus);

            this.optChoice0.CheckedChanged += new EventHandler(eControl_Change);
            this.optChoice1.CheckedChanged += new EventHandler(eControl_Change);
        }

        void eControl_Change(object sender, EventArgs e)
        {
            if (sender == this.optChoice0)
            {
                nFlag = 1;
            }
            else if (sender == this.optChoice1)
            {
                nFlag = 2;
            }
        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            txtAcc.ImeMode = ImeMode.Hangul;

            txtAcc.SelectionStart = 0;
            txtAcc.SelectionLength = txtAcc.TextLength;
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if (optChoice1.Checked == true)
            {
                txtAcc.Text = ComFunc.SetAutoZero(txtAcc.Text, 8);
            }
            else
            {
                txtAcc.ImeMode = ImeMode.Hangul;
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

            #region 콤보박스 년도 세팅
            for (int i = 0; i <= 5; i++)
            {
                cboYear.Items.Add(nYear - i);
            }
            cboYear.SelectedIndex = 0;
            #endregion

            ssList_Sheet1.Columns[ssList_Sheet1.Columns.Count - 1].Visible = false;

            btnPrint.Enabled = false;
            optChoice0.Checked = true;
            nFlag = 1;

        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtAcc)
            {
                if (e.KeyChar == 13)
                {
                    if (txtAcc.Text == "")
                    {
                        return;
                    }
                    if (optChoice1.Checked == true)
                    {
                        txtAcc.Text = ComFunc.SetAutoZero(txtAcc.Text, 8);
                    }
                    btnOK.Focus();
                }
            }

            //else if (sender == this.dtpYear)
            //{
            //    if (e.KeyChar == 13)
            //    {
            //        SendKeys.Send("{TAB}");
            //    }
            //}
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnNextAcc)
            {
                ComFunc.SetAllControlClear(panel2);

                ssList_Sheet1.Rows.Count = 0;
                ssList_Sheet1.Rows.Count = 23;

                btnSnameShow.Visible = false;
                txtAcc.Enabled = true;
                btnOK.Enabled = true;
                btnPrint.Enabled = false;
                txtAcc.Focus();
            }

            else if (sender == this.btnOK)
            {
                btnOK_Click();
            }

            else if (sender == this.btnSnameShow)
            {
                btnSnameShow_Click();
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ssList_Sheet1.Columns[ssList_Sheet1.Columns.Count - 1].Visible = true;
                ePrint();
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            btnPrint.Enabled = false;

            ssList_Sheet1.Cells[0, ssList_Sheet1.Columns.Count - 1].Text = "zzz";
            ssList_Sheet1.Columns[ssList_Sheet1.Columns.Count - 1].Visible = false;

            //Print Head 지정
            strTitle = cboYear.SelectedItem.ToString() + "년 청구기준 진료일수";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 60, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, false, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
    

            btnPrint.Enabled = true;
        }

        void btnOK_Click()
        {
            DataTable dt = null;

            if (txtAcc.Text.Trim() == "")
            {
                return;
            }

            btnSnameShow.Visible = false;

            if (optChoice0.Checked == true)
            {
                clsPmpaPb.GstrView1 = "1^^";
                clsPmpaPb.GstrView1 += txtAcc.Text + "^^";

                dt = IDSetting();

                switch (dt.Rows.Count)
                {
                    case 0:
                        dt.Dispose();
                        dt = null;
                        break;

                    case 1:
                        PanelCap();
                        break;

                    default:
                        btnSnameShow.Visible = true;
                        clsPmpaPb.GnStart = 1;

                        frmPmpaViewSnameX = new frmPmpaViewSname(clsPmpaPb.GstrView1);
                        frmPmpaViewSnameX.rSendText += new frmPmpaViewSname.SendText(GetText);
                        frmPmpaViewSnameX.rEventExit += new frmPmpaViewSname.EventExit(frmPmpaViewSnameX_rEventExit);
                        frmPmpaViewSnameX.Show();
                        break;
                }
            }

            else
            {
                clsPmpaPb.GstrView1 = "2^^";
                clsPmpaPb.GstrView1 += txtAcc.Text + "^^";
                dt = IDSetting();
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsPmpaPb.GnChoice = 0;

                PanelCap();
            }

            HistoryView();

            txtAcc.Enabled = false;
            btnOK.Enabled = false;
            btnPrint.Enabled = true;
        }

        void btnSnameShow_Click()
        {
            ssList_Sheet1.Rows.Count = 0;
            ssList_Sheet1.Rows.Count = 23;

            frmPmpaViewSnameX = new frmPmpaViewSname(clsPmpaPb.GstrView1);
            frmPmpaViewSnameX.rSendText += new frmPmpaViewSname.SendText(GetText);
            frmPmpaViewSnameX.rEventExit += new frmPmpaViewSname.EventExit(frmPmpaViewSnameX_rEventExit);
            frmPmpaViewSnameX.Show();
        }

        void GetText(string str)
        {
            clsPmpaPb.gstrView2 = str;
            mstrView2 = clsPmpaPb.gstrView2;
            PanelCap("1");
            HistoryView();
        }

        void frmPmpaViewSnameX_rEventExit()
        {
            frmPmpaViewSnameX.Dispose();
            frmPmpaViewSnameX = null;
        }

        void PanelCap(string Gubun = "")
        {
            DataTable dt = null;
            if (Gubun == "")
            {
                dt = IDSetting();
                txt0.Text = dt.Rows[clsPmpaPb.GnChoice]["Pano"].ToString().Trim();
                txt1.Text = dt.Rows[clsPmpaPb.GnChoice]["Sname"].ToString().Trim();
                txt2.Text = dt.Rows[clsPmpaPb.GnChoice]["Sex"].ToString().Trim();
                txt3.Text = dt.Rows[clsPmpaPb.GnChoice]["Jumin1"].ToString().Trim();
                txt4.Text = dt.Rows[clsPmpaPb.GnChoice]["Jumin2"].ToString().Trim();
            }

            else if (Gubun != "")
            {
                dt = IDSetting("1");
                txt0.Text = dt.Rows[0]["Pano"].ToString().Trim();
                txt1.Text = dt.Rows[0]["Sname"].ToString().Trim();
                txt2.Text = dt.Rows[0]["Sex"].ToString().Trim();
                txt3.Text = dt.Rows[0]["Jumin1"].ToString().Trim();
                txt4.Text = dt.Rows[0]["Jumin2"].ToString().Trim();
            }
        }

        void HistoryView()
        {
            if(txt0.Text == "")
            {
                return;
            }

            int i = 0;
            int j = 0;
            int k = 0;
            int nRow = 0;

            double nTotAmt = 0;
            int nTotIlsu = 0;
            string strSDate = "";
            int nIlsu = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            for (i = 0; i < strDay.Length; i++)
            {
                strDay[i] = " ";
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  YYMM,IpdOpd,Pano,Bi,DeptCode,TO_CHAR(SDate,'yyyy-mm-dd') Sdate,Ilsu,Amt     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MIRHIS                                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND PANO = '" + txt0.Text + "'                                          ";
            SQL += ComNum.VBLF + "      AND SDate BETWEEN TO_DATE('" + cboYear.SelectedItem.ToString() + "-01-01','yyyy-mm-dd')    ";
            SQL += ComNum.VBLF + "                    AND TO_DATE('" + cboYear.SelectedItem.ToString() + "-12-31','yyyy-mm-dd')    ";
            SQL += ComNum.VBLF + "ORDER BY YYMM,SDate                                                           ";

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

                nTotAmt = 0;
                nTotIlsu = 0;

                if (dt.Rows.Count > 0)
                {
                    nRow = dt.Rows.Count;
                    ssList_Sheet1.Rows.Count = nRow;

                    for (i = 0; i < nRow; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["YYMM"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = VB.Right(dt.Rows[i]["Sdate"].ToString().Trim(), 8);
                        ssList_Sheet1.Cells[i, 5].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim()));
                        ssList_Sheet1.Cells[i, 6].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));

                        nTotAmt += VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                        nTotIlsu += Convert.ToInt32(VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim()));

                        #region Day_Jin_Set(GoSub)

                        strSDate = dt.Rows[i]["Sdate"].ToString().Trim();
                        nIlsu = Convert.ToInt32(VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim()));

                        if (strSDate == "")
                        {
                            return;
                        }
                        if (VB.Left(strSDate, 4) != cboYear.SelectedItem.ToString())
                        {
                            return;
                        }

                        j = CF.DATE_ILSU(clsDB.DbCon, strSDate, (cboYear.SelectedItem.ToString() + "-01-01")) + 1;

                        for (k = j; k < (j + nIlsu - 1); k++)
                        {
                            if (k < strDay.Length)
                            {
                                strDay[k] = "*";
                            }
                        }

                        #endregion Day_Jin_Set(GoSub) End


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

            ssList_Sheet1.Cells[nRow - 1, 4].Text = "** 합 계 **";
            ssList_Sheet1.Cells[nRow - 1, 5].Text = String.Format("{0:###,###,###,##0}", nTotIlsu);
            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:###,###,###,##0}", nTotAmt);

            j = 0;

            for (i = 0; i < strDay.Length; i++)
            {
                if (strDay[i] == "*")
                {
                    j += 1;
                }
            }

            //txt5.Text = String.Format("{0:##0}", j);
            txt5.Text = ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count -1, 5].Text;
        }

        public DataTable IDSetting(string Gubun = "")
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
            if (Gubun == "")
            {
                if (nFlag == 1)
                {
                    SQL += ComNum.VBLF + "      AND SName LIKE '" + txtAcc.Text + "%'                                                           ";
                    SQL += ComNum.VBLF + "ORDER BY Sname                                                                                        ";
                }

                else
                {
                    SQL += ComNum.VBLF + "      AND Pano = '" + txtAcc.Text + "'                                                                ";
                    SQL += ComNum.VBLF + "ORDER BY Pano                                                                                         ";
                }
            }

            else if (Gubun != "")
            {
                if (nFlag == 1)
                {
                    SQL += ComNum.VBLF + "  AND P.ROWID ='" + mstrView2 + "'                                                                    ";
                    SQL += ComNum.VBLF + "ORDER BY Sname                                                                                        ";
                }

                else
                {
                    SQL += ComNum.VBLF + "  AND P.ROWID ='" + mstrView2 + "'                                                                    ";
                    SQL += ComNum.VBLF + "ORDER BY Pano                                                                                         ";
                }
            }
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            return dt;
        }

    }
}
