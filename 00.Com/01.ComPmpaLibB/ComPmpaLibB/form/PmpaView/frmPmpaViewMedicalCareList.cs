using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMedicalCareList.cs
    /// Description     : 의료급여 건강생활유지비 집계표
    /// Author          : 안정수
    /// Create Date     : 2017-09-25 
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm의료급여집계표.frm(Frm의료급여집계표) => frmPmpaViewMedicalCareList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm의료급여집계표.frm(Frm의료급여집계표)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewMedicalCareList : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        public frmPmpaViewMedicalCareList()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
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

            CF.ComboMonth_Set(cboYYMM, 10);

            optGubun2.Checked = true;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
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

            string strFDate = "";
            string strTDate = "";

            if (ssList1_Sheet1.Cells[0, 0].Text == "")
            {
                return;
            }

            strFDate = ssList1_Sheet1.Cells[0, 0].Text;
            strTDate = ssList1_Sheet1.Cells[ssList1_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) - 1, 0].Text;

            //2015-08-11 결제라인에 계장 추가(원무과 박시철계장 요청)
            string strLine1 = VB.Space(30) + "┌─┬────┬────┬────┬────┐ ";
            string strLine2 = VB.Space(30) + "│결│담　  당│계　  장│팀　  장│병 원 장│ ";
            string strLine3 = VB.Space(30) + "│　├────┼────┼────┼────┤ ";
            string strLine4 = VB.Space(30) + "│　│　　　　│　　　　│　　　　│　　　　│ ";
            string strLine5 = VB.Space(30) + "│재│　　　　│　　　　│　　　　│　　　  │ ";
            string strLine6 = VB.Space(30) + "└─┴────┴────┴────┴────┘ ";

            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            ssList1_Sheet1.Cells[0, 4].Text = "zzz";
            ssList1_Sheet1.Columns[4].Visible = false;

            #endregion

            if (optGubun0.Checked == true)
            {
                strTitle = "(의료급여) 건강생활유지비입금표";
            }

            else if (optGubun1.Checked == true)
            {
                strTitle = "(의료급여) 산전지원금입금표";
            }

            else
            {
                strTitle = "(의료급여) 입금표";
            }

            strSubTitle = ("회계기간 : " + strFDate + " ~ " + strTDate + "\r\n" + "경 리 과 : " + "\r\n");

            strHeader = SPR.setSpdPrint_String(strTitle + "\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strHeader += SPR.setSpdPrint_String("\r\n" + strLine1, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n" + strLine2, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n" + strLine3, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n" + strLine4, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n" + strLine5, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n" + strLine6, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 10, 50, 65, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, false, true, false, false, false);

            SPR.setSpdPrint(ssList1, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            ssList1_Sheet1.Columns[4].Visible = true;
            #endregion
        }

        void eGetData()
        {
            string strFDate = "";
            string strTDate = "";
            int i = 0;
            double nTot = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList2_Sheet1.Rows.Count = 0;

            strFDate = VB.Left(cboYYMM.SelectedItem.ToString(), 4) + "-" + VB.Mid(cboYYMM.SelectedItem.ToString(), 7, 2) + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);

            nTot = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                ";
            SQL += ComNum.VBLF + "  TO_CHAR(IPGUMDATE,'YYYY-MM-DD') IPGUMDATE, SUM(DECODE(GbBun,'1',AMT,'2',AMT1)) AMT  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI                                             ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
            SQL += ComNum.VBLF + "      AND IPGUMDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                       ";
            SQL += ComNum.VBLF + "      AND IPGUMDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                       ";
            if (optGubun0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND GbBun ='1'                                                                  ";   //생활유지비만
            }
            else if (optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND GbBun ='2'                                                                  ";   //산전지원
            }
            SQL += ComNum.VBLF + "GROUP BY TO_CHAR(IPGUMDATE,'YYYY-MM-DD')                                              ";
            SQL += ComNum.VBLF + "ORDER BY IPGUMDATE                                                                    ";

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
                    ssList2_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IPGUMDATE"].ToString().Trim();
                        ssList2_Sheet1.Cells[i, 1].Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                        nTot += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());

                        ssList2.ActiveSheet.Rows[i].Height = 20;
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

            ssList2_Sheet1.Rows.Count += 1;
            ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 0].Text = "합  계";
            ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 1].Text = String.Format("{0:###,###,###}", nTot);
        }

        void ssList2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            double nTotAmt = 0;
            double nTotCnt = 0;
            string strDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            ssList1_Sheet1.Rows.Count = 0;

            strDate = ssList2_Sheet1.Cells[e.Row, 0].Text;

            if (strDate == "")
            {
                return;
            }

            nTotAmt = 0;
            nTotCnt = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                        ";
            SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, COUNT(ACTDATE) CNT,  ";
            SQL += ComNum.VBLF + "  SUM(DECODE(GbBun,'1',AMT,'2',AMT1)) AMT                     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "      AND IPGUMDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND IPGUMDATE IS NOT NULL                               ";
            if (optGubun0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND GbBun ='1'                                          ";  //생활유지비만
            }
            else if (optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND GbBun ='2'                                          ";  //산전지원
            }
            SQL += ComNum.VBLF + "GROUP BY ACTDATE                                              ";

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
                        ssList1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 1].Text = String.Format("{0:###,###,###}", dt.Rows[i]["CNT"]).ToString().Trim();
                        ssList1_Sheet1.Cells[i, 2].Text = String.Format("{0:###,###,###}", dt.Rows[i]["AMT"]).ToString().Trim();
                        nTotCnt += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                        nTotAmt += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());

                        ssList1.ActiveSheet.Rows[i].Height = 20;
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

            ssList1_Sheet1.Rows.Count += 1;
            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 0].Text = "합    계";
            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 1].Text = String.Format("{0:###,###,###}", nTotCnt);
            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 2].Text = String.Format("{0:###,###,###}", nTotAmt);

            ssList1.ActiveSheet.Rows[ssList1_Sheet1.Rows.Count - 1].Height = 20;
        }
    }
}
