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
    /// File Name       : frmPmpaViewTimeListByPart.cs
    /// Description     : 외래 수납 행위별 시간대 조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-26
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm조별시간대건수.frm(Frm조별시간대건수) => frmPmpaViewTimeListByPart.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm조별시간대건수.frm(Frm조별시간대건수)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewTimeListByPart : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        string mstrJobName = "";

        public frmPmpaViewTimeListByPart()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewTimeListByPart(string GstrJobName)
        {
            InitializeComponent();
            mstrJobName = GstrJobName;
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);

            this.txtPart.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpFDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtFTime.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtTTime.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(sender == this.txtPart)
            {
                if(e.KeyChar == 13)
                {
                    dtpFDate.Focus();
                }
            }

            else if (sender == this.dtpFDate)
            {
                if (e.KeyChar == 13)
                {
                    dtpTDate.Focus();
                }
            }

            else if (sender == this.dtpTDate)
            {
                if (e.KeyChar == 13)
                {
                    txtFTime.Focus();
                }
            }

            else if (sender == this.txtFTime)
            {
                if (e.KeyChar == 13)
                {
                    txtTTime.Focus();
                }
            }

            else if (sender == this.txtTTime)
            {
                if (e.KeyChar == 13)
                {
                    btnView.Focus();
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
            txtPCnt.Text = "";

            txtFTime.Text = "08:00";
            txtTTime.Text = "17:30";

            txtPart.Focus();
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

            //else if (sender == this.btnPrint)
            //{
            //    if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            //    {
            //        return; //권한 확인
            //    }
            //    ePrint();
            //}
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

            string JobMan = "";

            string PrintDate = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            string JobDate = dtpFDate.Text;

            if (mstrJobName != "")
            {
                JobMan = mstrJobName;
            }

            if (ssList2_Sheet1.Rows.Count == 0)
            {
                return;
            }


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            //ssList.ActiveSheet.Cells[0, 9].Text = "zzz";
            //ssList.ActiveSheet.Columns[9].Visible = false;


            #endregion

            strTitle = "시간대별 수납건수";

            strSubTitle = "▶조회시간: " + dtpFDate.Text + "~" + dtpTDate.Text + " " + txtFTime.Text + "~" + txtTTime.Text + "▶출력자: " + JobMan;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("바탕체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strTitle, new Font("바탕체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("바탕체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, true, true, false);

            SPR.setSpdPrint(ssList2, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion

        }

        void eGetData()
        {
            int i = 0;
            int nRow = 0;
            int nCol = 0;
            int nRead = 0;
            int nSTot = 0;
            int nTot = 0;

            string strNew = "";
            string strOld = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if (txtFTime.Text == "" || VB.Len(txtFTime.Text) != 5)
            {
                ComFunc.MsgBox("시간1을 [12:00] 넣으세요!!");
                txtFTime.Focus();
                return;
            }

            if (txtTTime.Text == "" || VB.Len(txtTTime.Text) != 5)
            {
                ComFunc.MsgBox("시간2을 [12:30] 넣으세요!!");
                txtTTime.Focus();
                return;
            }

            CS.Spread_All_Clear(ssList1);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  A.PANO, B.SNAME, AMT, STIME,A.REMARK, A.BIGO,                               ";
            SQL += ComNum.VBLF + "  A.AMT1, A.AMT2, A.AMT3, A.AMT4, A.AMT5, A.DEPTCODE,                         ";
            SQL += ComNum.VBLF + "  A.BI, A.SEQNO2, A.CARDGB,a.CardAmt, A.YAKAMT,a.WorkGbn,a.GbSPC,A.ROWID      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SUNAP A, " + ComNum.DB_PMPA + "BAS_PATIENT B   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                    ";
            SQL += ComNum.VBLF + "      AND a.ACTDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')          ";
            SQL += ComNum.VBLF + "      AND a.ACTDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')          ";
            SQL += ComNum.VBLF + "      AND a.STime >='" + txtFTime.Text + "'                                   ";
            SQL += ComNum.VBLF + "      AND a.STime <='" + txtTTime.Text + "'                                   ";
            if (txtPart.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "  AND PART  = '" + txtPart.Text.Trim() + "'                               ";
            }
            SQL += ComNum.VBLF + "      AND a.PANO  <> '81000004'                                               ";
            SQL += ComNum.VBLF + "      AND a.PART NOT IN ('4349','7777','7778' )                               ";
            SQL += ComNum.VBLF + "      AND (a.DELDATE IS NULL OR a.DELDATE ='')                                ";
            SQL += ComNum.VBLF + "ORDER BY STIme                                                                ";

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
                    ssList1_Sheet1.Rows.Count = nRead;
                    ssList1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    txtPCnt.Text = nRead + " 건수";

                    for (i = 0; i < nRead; i++)
                    {
                        ssList1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 2].Text = String.Format("{0:###,###,##0}", VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                        ssList1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["STIME"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 4].Text = " " + dt.Rows[i]["REMARK"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 7].Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[i]["SEQNO2"].ToString().Trim()));

                        switch (dt.Rows[i]["CARDGB"].ToString().Trim())
                        {
                            case "1":
                                ssList1_Sheet1.Cells[i, 8].Text = "카드승인";
                                break;
                            case "2":
                                ssList1_Sheet1.Cells[i, 8].Text = "현금영수";
                                break;
                        }

                        ssList1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["BIGO"].ToString().Trim();
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

            CS.Spread_All_Clear(ssList2);

            nSTot = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  Part,Remark,COUNT(Pano) CNT                                                 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SUNAP                                          ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')            ";
            SQL += ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')            ";
            SQL += ComNum.VBLF + "      AND STime >='" + txtFTime.Text + "'                                     ";
            SQL += ComNum.VBLF + "      AND STime <='" + txtTTime.Text + "'                                     ";
            if (txtPart.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "  AND PART  = '" + txtPart.Text.Trim() + "'                               ";
            }
            SQL += ComNum.VBLF + "      AND PANO  <> '81000004'                                                 ";
            SQL += ComNum.VBLF + "      AND PART NOT IN ('4349','7777','7778' )                                 ";
            SQL += ComNum.VBLF + "      AND (DELDATE IS NULL OR DELDATE ='')                                    ";
            SQL += ComNum.VBLF + "GROUP BY Part,Remark                                                          ";

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
                    ssList2_Sheet1.Rows.Count = dt.Rows.Count + 1;

                    nRow = 0;
                    nCol = 0;
                    nSTot = 0;
                    nTot = 0;
                    ssList2_Sheet1.Rows.Count = 1000;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strNew = dt.Rows[i]["Part"].ToString().Trim();

                        if (strNew != strOld)
                        {
                            nRow += 1;

                            ssList2_Sheet1.Rows.Count = nRow;

                            ssList2_Sheet1.Cells[nRow - 1, 0].Text = strNew;

                            nCol = 0;
                            nSTot = 0;
                        }

                        nCol += 1;

                        nSTot += Convert.ToInt32(VB.Val(dt.Rows[i]["CNT"].ToString().Trim()));
                        nTot += Convert.ToInt32(VB.Val(dt.Rows[i]["CNT"].ToString().Trim()));

                        ssList2_Sheet1.Cells[nRow - 1, nCol + 1].Text = dt.Rows[i]["Remark"].ToString().Trim() + " (" + dt.Rows[i]["Cnt"].ToString().Trim() + ")";

                        ssList2_Sheet1.Cells[nRow - 1, 1].Text = nSTot.ToString();

                        strOld = dt.Rows[i]["Part"].ToString().Trim();
                    }

                    ssList2_Sheet1.Rows.Count = nRow + 1;
                    ssList2_Sheet1.Cells[nRow, 0].Text = "전체합계";
                    ssList2_Sheet1.Cells[nRow, 1].Text = nTot.ToString();

                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            //ssList2_Sheet1.Columns.Count = ssList2_Sheet1.GetLastNonEmptyColumn(FarPoint.Win.Spread.NonEmptyItemFlag.Data);
            ssList2_Sheet1.ColumnCount = ssList2_Sheet1.NonEmptyColumnCount;
            ssList2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

        }

    }
}
