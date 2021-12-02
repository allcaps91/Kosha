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
    /// File Name       : frmPmpaViewBoho.cs
    /// Description     : 의료급여 승인내역
    /// Author          : 안정수
    /// Create Date     : 2017-08-23
    /// Update History  : 2017-11-02
    /// <history>       
    /// d:\psmh\OPD\olrepa\olrepa19.frm(FrmBoho) => frmPmpaViewBoho.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\olrepa19.frm(FrmBoho)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewBoho : Form
    {
        string mstrJobName = "";

        public frmPmpaViewBoho()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewBoho(string GstrJobName)
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

            optIO0.Checked = true;
            optGbn2.Checked = true;
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
            string JobMan = "";
            bool PrePrint = true;

            btnPrint.Enabled = false;
            ssList_Sheet1.Columns[9].Visible = false;

            JobMan = mstrJobName;

            if (chkCen.Checked == true)
            {
                if (optGbn0.Checked == true)
                {
                    strTitle = " 의료급여 승인 내역 (입원승인취소)";
                }
                else if (optGbn1.Checked == true)
                {
                    strTitle = " 의료급여 산전 승인 내역 (입원승인취소)";
                }
            }
            else
            {
                if (optGbn0.Checked == true)
                {
                    strTitle = " 의료급여 승인 내역";
                }
                else if (optGbn1.Checked == true)
                {
                    strTitle = " 의료급여 산전 승인 내역";
                }
            }

            if (JobMan != "")
            {
                strSubTitle = "▶조회일자: " + dtpFDate.Text + VB.Space(10) + "▶출력자: " + JobMan;
            }

            else
            {
                strSubTitle = "▶조회일자: " + dtpFDate.Text;
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3)+ "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            btnPrint.Enabled = true;

            ssList_Sheet1.Columns[9].Visible = true;
        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;

            double nTot = 0;
            double nSubTot = 0;

            string strPart_New = "";
            string strPart_Old = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  DECODE(A.GbBun,'1',a.AMT,'2',a.AMT1) AMT, A.ACTDATE, A.PANO,B.SNAME, A.BI,              ";
            SQL += ComNum.VBLF + "  A.DEPTCODE,DECODE(a.GbBun,'1','생활','2','산전') GbBunName ,                            ";
            SQL += ComNum.VBLF + "  A.PART, A.BDATE, A.GUBUN,A.MSEQNO,TO_CHAR(a.EntDate,'YYYY-MM-DD HH24:MI') EntDate       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI A, " + ComNum.DB_PMPA + "BAS_PATIENT B          ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND A.PANO = B.PANO(+)                                                              ";
            if (txtPart.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "  AND A.PART = '" + txtPart.Text.Trim() + "'                                          ";
            }
            if (chkSS.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND A.AMT <> 0                                                                      ";
            }
            if (chkCen.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND A.PART = '#'                                                                    ";
            }
            if (optGbn0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND A.GbBun ='1'                                                                    ";  //생활유지비만
            }
            else if (optGbn1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND A.GbBun ='2'                                                                    ";   //산전지원금만
            }
            if (optIO1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND A.GbIO ='O'                                                                     ";   //외래
            }
            else if (optIO2.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND A.GbIO ='I'                                                                     ";  //외래
            }
            SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')                         ";
            SQL += ComNum.VBLF + "ORDER BY A.PART,A.PANO, A.ACTDATE                                                         ";

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
                    ssList_Sheet1.Rows.Count = 0;

                    for (i = 0; i < nRead; i++)
                    {
                        strPart_New = dt.Rows[i]["PART"].ToString().Trim();
                        if (strPart_New != strPart_Old)
                        {
                            if (i == 0)
                            {
                                ssList_Sheet1.Rows.Count += 1;

                                if (strPart_New == "#")
                                {
                                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "입원";
                                }
                                else
                                {
                                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = clsVbfunc.GetInSaName(clsDB.DbCon, ComFunc.SetAutoZero(strPart_New, 5));
                                }

                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = VB.Left(dt.Rows[i]["ACTDATE"].ToString().Trim(), 10);
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = VB.Left(dt.Rows[i]["BDATE"].ToString().Trim(), 10);
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = dt.Rows[i]["Pano"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = dt.Rows[i]["SName"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 6].Text = dt.Rows[i]["Bi"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 8].Text = dt.Rows[i]["MSEQNO"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 9].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 10].Text = dt.Rows[i]["GbBunName"].ToString().Trim();

                                strPart_Old = strPart_New;
                                nSubTot += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                                nTot += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                            }
                            else
                            {
                                ssList_Sheet1.Rows.Count += 1;

                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "소계";
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = String.Format("{0:###,###,###,##0}", nSubTot);
                                ssList_Sheet1.Rows[ssList_Sheet1.Rows.Count - 1].BackColor = Color.LightGreen;

                                nSubTot = 0;

                                ssList_Sheet1.Rows.Count += 1;

                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = clsVbfunc.GetInSaName(clsDB.DbCon, ComFunc.SetAutoZero(strPart_New, 5));
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = String.Format("{0:###,###,###,##0}", dt.Rows[i]["AMT"].ToString().Trim());
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = VB.Left(dt.Rows[i]["ACTDATE"].ToString().Trim(), 10);
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = VB.Left(dt.Rows[i]["BDATE"].ToString().Trim(), 10);
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = dt.Rows[i]["Pano"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = dt.Rows[i]["SName"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 6].Text = dt.Rows[i]["Bi"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 8].Text = dt.Rows[i]["MSEQNO"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 9].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 10].Text = dt.Rows[i]["GbBunName"].ToString().Trim();

                                strPart_Old = strPart_New;
                                nSubTot += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                                nTot += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());

                            }
                        }
                        else
                        {
                            ssList_Sheet1.Rows.Count += 1;

                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = String.Format("{0:###,###,###,##0}", dt.Rows[i]["AMT"].ToString().Trim());
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = VB.Left(dt.Rows[i]["ACTDATE"].ToString().Trim(), 10);
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = VB.Left(dt.Rows[i]["BDATE"].ToString().Trim(), 10);
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = dt.Rows[i]["SName"].ToString().Trim();
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 6].Text = dt.Rows[i]["Bi"].ToString().Trim();
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 8].Text = dt.Rows[i]["MSEQNO"].ToString().Trim();
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 9].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 10].Text = dt.Rows[i]["GbBunName"].ToString().Trim();

                            strPart_Old = strPart_New;
                            nSubTot += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                            nTot += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            ssList_Sheet1.Rows.Count += 1;
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "소계";
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = String.Format("{0:###,###,###,##0}", nSubTot);
            ssList_Sheet1.Rows[ssList_Sheet1.Rows.Count - 1].BackColor = Color.LightGreen;

            ssList_Sheet1.Rows.Count += 1;
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "총계";
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = String.Format("{0:###,###,###,##0}", nTot);
            ssList_Sheet1.Rows[ssList_Sheet1.Rows.Count - 1].BackColor = Color.LightBlue;

            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            dt.Dispose();
            dt = null;

        }

        void txtPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
