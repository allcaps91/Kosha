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
    /// File Name       : frmPmpaViewJinDan.cs
    /// Description     : 특수진단서 수입일보
    /// Author          : 안정수
    /// Create Date     : 2017-08-24
    /// Update History  : 2017-11-02
    /// <history>       
    /// d:\psmh\OPD\olrepa\OLREPA07.FRM(FrmJinDan) => frmPmpaViewJinDan.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\OLREPA07.FRM(FrmJinDan)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewJinDan : Form
    {
        clsSpread CS = new clsSpread();
        int mJobSabun = 0;
        string mstrJobName = "";

        string strFDate = "";

        string strTDate = "";
        //*****************************

        double[] nAmt1 = new double[5];
        double[] nAmt2 = new double[5];
        double[] nAmt3 = new double[5];
        double[] nAmt4 = new double[5];
        double[] nAmt5 = new double[5];
        double[] nAmt6 = new double[5];
        double[] nAmt7 = new double[5];

        int nCount1 = 0;

        public frmPmpaViewJinDan()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewJinDan(string GnJobSabun, string GstrJobName)
        {
            InitializeComponent();
            setEvent();
            mJobSabun = Convert.ToInt32(VB.Val(GnJobSabun));
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

            chkOrder.Checked = true;

            txtPart.Text = clsType.User.Sabun;

            if (mJobSabun != 0)
            {
                txtPart.Text = mJobSabun.ToString();
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
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                Cursor.Current = Cursors.WaitCursor;
                eGetData();
                Cursor.Current = Cursors.Default;
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
            int i = 0;
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread(); 
            ssPrint_Sheet1.RowCount = 6;
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

      
            ssPrint_Sheet1.Cells[2, 0].Text = "작업기간 : " + dtpFDate.Text + "=>" + dtpTDate.Text;
            ssPrint_Sheet1.Cells[3, 0].Text = "출력시간 : " + VB.Now().ToString();
            ssPrint_Sheet1.Cells[1, 0].Text = "작 성 자 :" +  clsType.User.JobName ;
            FarPoint.Win.ComplexBorder BorderBottom = new FarPoint.Win.ComplexBorder(
              new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
              new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
              new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
              new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
              new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), false, false);

            for (i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                ssPrint_Sheet1.RowCount += 1;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = (i + 1).ToString();
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = ssList_Sheet1.Cells[i, 0].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = ssList_Sheet1.Cells[i, 1].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].Text = ssList_Sheet1.Cells[i, 2].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].Text = ssList_Sheet1.Cells[i, 3].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].Text = ssList_Sheet1.Cells[i, 4].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 6].Text = ssList_Sheet1.Cells[i, 5].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 10].Text = ssList_Sheet1.Cells[i, 6].Text;
                
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0, ssPrint_Sheet1.RowCount - 1, ssPrint_Sheet1.ColumnCount - 1].Border = BorderBottom;
                //ssPrint_Sheet1.CellSpan
                CS.CellSpan(ssPrint, ssPrint_Sheet1.RowCount - 1, 6, 1, 4);
            }
            ssPrint_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            strTitle = "특수진단서 수입일보";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, false);

            CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void eGetData()
        {
            if(dtpFDate.Text == "")
            {
                ComFunc.MsgBox("From 일자 가 비어있습니다.");
                dtpFDate.Focus();
                return;
            }
            else if(String.Compare(dtpFDate.Text, dtpTDate.Text) > 0)
            {
                ComFunc.MsgBox("From 일자 가 To 일자보다 큽니다.");
                dtpFDate.Focus();
                return;
            }

            ClearProcess();

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            ssListBuild();            
        }

        void ClearProcess()
        {
            CS.Spread_All_Clear(ssList);
        }

        void ssListBuild()
        {
            int i = 0;
            int nRow = 0;
            int ChkFlag = 0;
            int nCount2 = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            nCount1 = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "   G.DeptNameK, nvl(DR.RE_DRNAME,D.DrName) DrName, S.Pano, P.SName, S.Sucode, Su.SuNameK,";
            if(chkOrder.Checked == true)
            {
                SQL += ComNum.VBLF + "   SUM(S.Amt1 + S.Amt2)  SAmt1, S.DeptCode,  DR.ROWID,";
            }
            else
            {
                SQL += ComNum.VBLF + "   DECODE(S.BUN, '75', S.Amt1 + S.Amt2, 0) SAmt1,S.DeptCode, DR.ROWID,DR.RE_DRNAME, ";
            }
            
            SQL += ComNum.VBLF + "   TO_CHAR(S.BDate,'YYYY-MM-DD') ACTDATE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT P, " + ComNum.DB_PMPA + "BAS_CLINICDEPT G ,";
            SQL += ComNum.VBLF +           ComNum.DB_PMPA + "BAS_DOCTOR D, " + ComNum.DB_PMPA + "BAS_SUN Su, ";
            SQL += ComNum.VBLF +           ComNum.DB_PMPA + "OPD_SLIP S, " + ComNum.DB_PMPA + "OPD_JINDAN_DRNAME DR ";
            //SQL += ComNum.VBLF + "WHERE 1=1";
            //SQL += ComNum.VBLF + "       AND S.BDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "       WHERE S.BDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "       AND S.BDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "       AND (S.WardCode IS NULL OR S.WardCode <> '99')";
            SQL += ComNum.VBLF + "       AND S.Bun = '75'";
            SQL += ComNum.VBLF + "       AND P.Pano = S.Pano";
            SQL += ComNum.VBLF + "       AND G.DeptCode = S.DeptCode ";
            SQL += ComNum.VBLF + "       AND D.DrCode = S.DrCode";
            SQL += ComNum.VBLF + "       AND Su.SuNext = S.SuCode";
            SQL += ComNum.VBLF + "       AND DR.Pano(+) = S.Pano";
            SQL += ComNum.VBLF + "       AND DR.DeptCode(+) = S.DeptCode";
            SQL += ComNum.VBLF + "       AND DR.BDate(+) = S.BDate";
            SQL += ComNum.VBLF + "       AND DR.sunext(+) = S.SuCode";
            if(txtPart.Text.Length  > 0)
            {
                SQL += ComNum.VBLF + "   AND S.Part ='" + txtPart.Text + "' ";
            }
            SQL += ComNum.VBLF + "       AND S.PANO <>'81000004'";
            if(chkOrder.Checked == true)
            {
                SQL += ComNum.VBLF + "group by G.DeptNameK, nvl(DR.RE_DRNAME,D.DrName) , S.Pano, P.SName, S.DeptCode, S.ActDate ,S.Sucode, Su.SuNameK, TO_CHAR(S.BDate,'YYYY-MM-DD'),DR.ROWID";
                SQL += ComNum.VBLF + "  having sum(QTY * NAL) > 0 ";
            }
            SQL += ComNum.VBLF + "ORDER BY Su.SuNameK, S.ActDate, S.Pano, G.DeptNameK, P.SName ";

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
                    nCount2 = 0;
                    nCount1 = 0;
                    btnPrint.Visible = true;

                    ssList_Sheet1.Rows.Count = 0;

                    #region ForAddProcess(GoSub)
                    nRow = dt.Rows.Count;
                    ChkFlag = 0;

                    for(i = 0; i < nRow; i++)
                    {
                        nCount2 += 1;

                        //특수진단서
                        switch (dt.Rows[i]["SuCode"].ToString().Trim())
                        {
                            //case "ZA11":
                            //    ChkFlag = 1;
                            //    break;
                            //case "ZA11A":
                            //    ChkFlag = 1;
                            //    break;
                            case "ZA28":
                                ChkFlag = 1;
                                break;
                            case "ZA29":
                                ChkFlag = 1;
                                break;
                            case "ZA49":
                                ChkFlag = 1;
                                break;
                            case "ZA60":
                                ChkFlag = 1;
                                break;
                            case "ZA18":
                                ChkFlag = 1;
                                break;
                            default:
                                ChkFlag = 0;
                                break;
                        }

                        if(ChkFlag == 1)
                        {
                            nCount1 += 1;
                            ssList_Sheet1.Rows.Count = nCount1;

                            ssList_Sheet1.Cells[nCount1 - 1, 0].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                            ssList_Sheet1.Cells[nCount1 - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssList_Sheet1.Cells[nCount1 - 1, 2].Text = dt.Rows[i]["DeptNameK"].ToString().Trim();
                            ssList_Sheet1.Cells[nCount1 - 1, 3].Text = dt.Rows[i]["SName"].ToString().Trim();
                            ssList_Sheet1.Cells[nCount1 - 1, 4].Text = VB.Val(dt.Rows[i]["SAmt1"].ToString().Trim()).ToString("###,###,##0");
                            ssList_Sheet1.Cells[nCount1 - 1, 5].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                            ssList_Sheet1.Cells[nCount1 - 1, 6].Text = dt.Rows[i]["DrName"].ToString().Trim();

                            ssList_Sheet1.Cells[nCount1 - 1, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                            ssList_Sheet1.Cells[nCount1 - 1, 9].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                            ssList_Sheet1.Cells[nCount1 - 1, 10].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        }
                    }


                    #endregion ForAddProcess(GoSub) End
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

        void ssList_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string strActDate = "";
            string strPano = "";
            string strDeptCode = "";
            string strDrName = "";
            string strSuNext = "";
            string strRowID = "";
            string strTemp = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            if (ssList_Sheet1.Cells[e.Row, 8].Text == "")
            {
                #region INSERT_DR_RESET(GoSub)

                strActDate = ssList_Sheet1.Cells[e.Row, 0].Text;
                strPano = ssList_Sheet1.Cells[e.Row, 1].Text;
                strDeptCode = ssList_Sheet1.Cells[e.Row, 10].Text;
                strSuNext = ssList_Sheet1.Cells[e.Row, 9].Text;
                strDrName = ssList_Sheet1.Cells[e.Row, 6].Text;

                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "OPD_JINDAN_DRNAME";
                SQL += ComNum.VBLF + "( ACTDATE,PANO,BDATE,ENTDATE,SUNEXT, DEPTCODE,RE_DRNAME )";
                SQL += ComNum.VBLF + "VALUES (";
                SQL += ComNum.VBLF + "TO_DATE('" + strActDate + "','YYYY-MM-DD'), '" + strPano + "', TO_DATE('" + strActDate + "','YYYY-MM-DD'),  SYSDATE ,  ";
                SQL += ComNum.VBLF + "'" + strSuNext + "','" + strDeptCode + "','" + strDrName + "' )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                try
                {
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                #endregion INSERT_DR_RESET(GoSub) End
            }
            else
            {
                #region UPDATE_DR_RESET(GoSub)

                strTemp = ssList_Sheet1.Cells[e.Row, 6].Text;
                strRowID = ssList_Sheet1.Cells[e.Row, 8].Text;

                SQL = "";
                SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "OPD_JINDAN_DRNAME SET    ";
                SQL += ComNum.VBLF + "RE_DRNAME = '" + strTemp + "'                         ";
                SQL += ComNum.VBLF + "WHERE ROWID = '" + strRowID + "'                      ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                try
                {
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                #endregion UPDATE_DR_RESET(GoSub) End
            }

            clsDB.setCommitTran(clsDB.DbCon);
            ComFunc.MsgBox("저장하였습니다.");
            Cursor.Current = Cursors.Default;

            eGetData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }
    }
}


