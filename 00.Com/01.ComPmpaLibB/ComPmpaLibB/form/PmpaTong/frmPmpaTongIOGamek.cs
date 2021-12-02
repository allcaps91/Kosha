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
    /// File Name       : frmPmpaTongIOGamek.cs
    /// Description     : 입원외래감액통계
    /// Author          : 안정수
    /// Create Date     : 2017-09-26
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm입원외래감액통계.frm(Frm입원외래감액통계) => frmPmpaTongIOGamek.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm입원외래감액통계.frm(Frm입원외래감액통계)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaTongIOGamek : Form
    {
        ComFunc CF = new ComFunc();

        public frmPmpaTongIOGamek()
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
            this.btnCancel.Click += new EventHandler(eBtnEvent);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등     

            Set_Combo();
        }       

        void Set_Combo()
        {
            int i = 0;
            int nYY = 0;
            int nMM = 0;

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            nYY = Convert.ToInt32(VB.Left(CurrentDate, 4));
            nMM = Convert.ToInt32(VB.Mid(CurrentDate, 6, 2));

            for( i = 1; i <= 12; i++)
            {
                cboFYYMM.Items.Add(ComFunc.SetAutoZero(nYY.ToString(), 4) + "년 " + ComFunc.SetAutoZero(nMM.ToString(), 2) + "월");
                cboTYYMM.Items.Add(ComFunc.SetAutoZero(nYY.ToString(), 4) + "년 " + ComFunc.SetAutoZero(nMM.ToString(), 2) + "월");
                nMM -= 1;

                if(nMM == 0)
                {
                    nMM = 12;
                    nYY -= 1;
                }
            }

            cboFYYMM.SelectedIndex = 0;
            cboTYYMM.SelectedIndex = 0;

            ComFunc.Form_Center(this);
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
              //  if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
              //  {
              //       return; //권한 확인
              //   }
                eGetData();
            }

            else if (sender == this.btnCancel)
            {
                ssList_Sheet1.Rows.Count = 0;
                ssList_Sheet1.Rows.Count = 50;
            }

            else if (sender == this.btnPrint)
            {
              //  if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
              //  {
              //      return; //권한 확인
              //  }
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


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            //ssList.ActiveSheet.Cells[0, 9].Text = "zzz";
            //ssList.ActiveSheet.Columns[9].Visible = false;


            #endregion

            strTitle = "계약처 및 신자 수익 현황";
            strSubTitle = "작업일자: " + cboFYYMM.SelectedItem.ToString() + "~" + cboTYYMM.SelectedItem.ToString();

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("바탕체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("바탕체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("바탕체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 60, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion

        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;

            string strFDate = "";
            string strTDate = "";
            string strIO = "";

            double nJAmt = 0;
            double nBAmt = 0;
            double nGAmt = 0;
            double nMAmt = 0;
            double nCnt = 0;
            double nJAmt_Tot = 0;
            double nBAmt_Tot = 0;
            double nGAmt_Tot = 0;
            double nMAmt_Tot = 0;
            double nCnt_Tot = 0;

            string strGel_New = "";
            string strGel_Old = "";
            string strGelName = "";
            double nJAmt_Sum = 0;
            double nBAmt_Sum = 0;
            double nGAmt_Sum = 0;
            double nMAmt_Sum = 0;
            double nCnt_Sum = 0;

            ssList_Sheet1.Rows.Count = 0;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
             
            
            DataTable dt = null;
            DataTable dt1 = null;

            strFDate = VB.Left(cboFYYMM.SelectedItem.ToString(), 4) + "-" + VB.Mid(cboFYYMM.SelectedItem.ToString(), 7, 2) + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, VB.Left(cboTYYMM.SelectedItem.ToString(), 4) + "-" + VB.Mid(cboTYYMM.SelectedItem.ToString(), 7, 2) + "-01");

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = "";
            SQL += ComNum.VBLF + "DROP VIEW VIEW_PANO";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            SQL = "";
            SQL += ComNum.VBLF + "CREATE OR REPLACE VIEW VIEW_PANO                              ";
            SQL += ComNum.VBLF + "(PANO, GELCODE, IPDOPD) AS                                    ";
            SQL += ComNum.VBLF + "SELECT                                                        ";
            SQL += ComNum.VBLF + "  PANO, GELCODE, 'O'                                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND GBGAMEK = '55'                                      ";
            SQL += ComNum.VBLF + "      AND GELCODE IS NOT NULL                                 ";
            SQL += ComNum.VBLF + "      AND GELCODE <> ' '                                      ";
            SQL += ComNum.VBLF + "GROUP BY PANO, GELCODE                                        ";
            SQL += ComNum.VBLF + "UNION ALL                                                     ";
            SQL += ComNum.VBLF + "SELECT                                                        ";
            SQL += ComNum.VBLF + "  PANO, GELCODE, 'I'                                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "      AND OUTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND OUTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND GBGAMEK = '55'                                      ";
            SQL += ComNum.VBLF + "      AND GELCODE IS NOT NULL                                 ";
            SQL += ComNum.VBLF + "GROUP BY PANO, GELCODE                                        ";
            SQL += ComNum.VBLF + "UNION ALL                                                     ";
            SQL += ComNum.VBLF + "SELECT                                                        ";
            SQL += ComNum.VBLF + "  PANO, 'HHH', 'O'                                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND GBGAMEK = '51'                                      ";
            SQL += ComNum.VBLF + "GROUP BY PANO, GELCODE                                        ";
            SQL += ComNum.VBLF + "UNION ALL                                                     ";
            SQL += ComNum.VBLF + "SELECT                                                        ";
            SQL += ComNum.VBLF + "  PANO, 'HHH', 'I'                                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "      AND OUTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND OUTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND GBGAMEK = '51'                                      ";
            SQL += ComNum.VBLF + "GROUP BY PANO, GELCODE                                        ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            try
            {
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;                    
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;                
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;                
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                ";
            SQL += ComNum.VBLF + "  X.GELCODE, SUM(X.CNT) CNT, X.IPDOPD,                                                ";
            SQL += ComNum.VBLF + "  SUM(JAMT) JAMT, SUM(BAMT) BAMT, SUM(GAMT) GAMT, SUM(MAMT) MAMT                      ";
            SQL += ComNum.VBLF + "FROM (                                                                                ";
            SQL += ComNum.VBLF + "      SELECT B.GELCODE, COUNT(B.GELCODE) CNT, B.IPDOPD,                               ";
            SQL += ComNum.VBLF + "          CASE WHEN A.BUN IN ('98') THEN SUM(AMT1+AMT2) END JAMT,                     ";
            SQL += ComNum.VBLF + "          CASE WHEN A.BUN IN ('99') THEN SUM(AMT1+AMT2) END BAMT,                     ";
            SQL += ComNum.VBLF + "          CASE WHEN A.BUN IN ('92') THEN SUM(AMT1+AMT2) END GAMT,                     ";
            SQL += ComNum.VBLF + "          CASE WHEN A.BUN IN ('96') THEN SUM(AMT1+AMT2) END MAMT                      ";
            SQL += ComNum.VBLF + "      FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "VIEW_PANO B        ";
            SQL += ComNum.VBLF + "      WHERE A.ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                     ";
            SQL += ComNum.VBLF + "        AND A.ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                     ";
            SQL += ComNum.VBLF + "        AND B.IPDOPD = 'O'                                                            ";
            SQL += ComNum.VBLF + "        AND A.PANO = B.PANO(+)                                                        ";
            SQL += ComNum.VBLF + "        AND A.BUN IN ('99','98','92','96')                                            ";
            SQL += ComNum.VBLF + "        AND B.GELCODE IS NOT NULL                                                     ";
            SQL += ComNum.VBLF +        "GROUP BY B.GELCODE, A.BUN, B.IPDOPD                                            ";
            SQL += ComNum.VBLF + "UNION ALL                                                                             ";
            SQL += ComNum.VBLF + "      SELECT                                                                          ";
            SQL += ComNum.VBLF + "          B.GELCODE, COUNT(B.GELCODE) CNT, B.IPDOPD,                                  ";
            SQL += ComNum.VBLF + "          CASE WHEN A.BUN IN ('98') THEN SUM(AMT) END JAMT,                           ";
            SQL += ComNum.VBLF + "          CASE WHEN A.BUN IN ('89','88') THEN SUM(AMT) END BAMT,                      ";
            SQL += ComNum.VBLF + "          CASE WHEN A.BUN IN ('92') THEN SUM(AMT) END GAMT,                           ";
            SQL += ComNum.VBLF + "          CASE WHEN A.BUN IN ('96') THEN SUM(AMT) END MAMT                            ";
            SQL += ComNum.VBLF + "      FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH A, " + ComNum.DB_PMPA + "VIEW_PANO B    ";
            SQL += ComNum.VBLF + "      WHERE A.ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                     ";
            SQL += ComNum.VBLF + "        AND A.ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                     ";
            SQL += ComNum.VBLF + "        AND B.IPDOPD = 'I'                                                            ";
            SQL += ComNum.VBLF + "        AND A.PANO = B.PANO(+)                                                        ";
            SQL += ComNum.VBLF + "        AND A.BUN IN ('98','89','88','92','96')                                       ";
            SQL += ComNum.VBLF + "        AND B.GELCODE IS NOT NULL                                                     ";
            SQL += ComNum.VBLF + "      GROUP BY B.GELCODE, A.BUN, B.IPDOPD                                     ) X     ";
            SQL += ComNum.VBLF + "GROUP BY GELCODE, IPDOPD                                                              ";

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
                    strGel_Old = "";

                    nJAmt_Tot = 0;  nBAmt_Tot = 0;  nGAmt_Tot = 0;  nMAmt_Tot = 0;  nCnt_Tot = 0;
                    nJAmt_Sum = 0;  nBAmt_Sum = 0;  nGAmt_Sum = 0;  nMAmt_Sum = 0;  nCnt_Sum = 0;

                    for(i = 0; i < nRead; i++)
                    {
                        strGel_New = dt.Rows[i]["GELCODE"].ToString().Trim();

                        if(dt.Rows[i]["ipdopd"].ToString().Trim() == "0")
                        {
                            strIO = "외래";
                        }
                        else
                        {
                            strIO = "입원";
                        }

                        if(strGel_New != "HHH")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT                                    ";
                            SQL += ComNum.VBLF + "  MIANAME                                 ";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MIA        ";
                            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
                            SQL += ComNum.VBLF + "      AND MIACLASS = '90'                 ";
                            SQL += ComNum.VBLF + "      AND MIACODE  = '" + strGel_New + "' ";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if(dt1.Rows.Count > 0)
                            {
                                strGelName = dt1.Rows[0]["MIANAME"].ToString().Trim();
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        else
                        {
                            strGelName = "신자감액";
                        }

                        nCnt = VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                        nJAmt = VB.Val(dt.Rows[i]["JAMT"].ToString().Trim());
                        nBAmt = VB.Val(dt.Rows[i]["BAMT"].ToString().Trim());
                        nGAmt = VB.Val(dt.Rows[i]["GAMT"].ToString().Trim());
                        nMAmt = VB.Val(dt.Rows[i]["MAMT"].ToString().Trim());

                        if(i == 0)
                        {
                            strGel_Old = strGel_New;
                        }

                        if(strGel_New != strGel_Old)
                        {
                            ssList_Sheet1.Rows.Count += 1;

                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = "합계";
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = String.Format("{0:###,###}", nCnt_Tot);
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = String.Format("{0:###,###,###,###}", nJAmt_Tot + nBAmt_Tot + nGAmt_Tot + nMAmt_Tot);
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = String.Format("{0:###,###,###,###}", nJAmt_Tot);
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = String.Format("{0:###,###,###,###}", nBAmt_Tot + nGAmt_Tot + nMAmt_Tot);
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 6].Text = String.Format("{0:###,###,###,###}", nGAmt_Tot);                            

                            nJAmt_Tot = 0; nBAmt_Tot = 0; nGAmt_Tot = 0; nMAmt_Tot = 0; nCnt_Tot = 0;

                            ssList_Sheet1.Rows.Count += 1;

                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = strGelName;
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = strIO;
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = String.Format("{0:###,###}", nCnt);
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = String.Format("{0:###,###,###,###}", nJAmt + nBAmt + nGAmt + nMAmt); 
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = String.Format("{0:###,###,###,###}", nJAmt); 
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = String.Format("{0:###,###,###,###}", nBAmt + nGAmt + nMAmt); 
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 6].Text = String.Format("{0:###,###,###,###}", nGAmt);
                            strGel_Old = strGel_New;
                        }

                        else
                        {
                            ssList_Sheet1.Rows.Count += 1;

                            if(i == 0)
                            {
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = strGelName;
                            }
                            
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = strIO;
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = String.Format("{0:###,###}", nCnt);
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = String.Format("{0:###,###,###,###}", nJAmt + nBAmt + nGAmt + nMAmt);
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = String.Format("{0:###,###,###,###}", nJAmt);
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = String.Format("{0:###,###,###,###}", nBAmt + nGAmt + nMAmt);
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 6].Text = String.Format("{0:###,###,###,###}", nGAmt);
                            strGel_Old = strGel_New;
                        }

                        nJAmt_Tot = nJAmt_Tot + nJAmt;
                        nBAmt_Tot = nBAmt_Tot + nBAmt;
                        nGAmt_Tot = nGAmt_Tot + nGAmt;
                        nMAmt_Tot = nMAmt_Tot + nMAmt;
                        nCnt_Tot = nCnt_Tot + nCnt;


                        nJAmt_Sum = nJAmt_Sum + nJAmt_Tot;
                        nBAmt_Sum = nBAmt_Sum + nBAmt_Tot;
                        nGAmt_Sum = nGAmt_Sum + nGAmt_Tot;
                        nMAmt_Sum = nMAmt_Sum + nMAmt_Tot;
                        nCnt_Sum = nCnt_Sum + nCnt_Tot;
                    }

                    ssList_Sheet1.Rows.Count += 1;
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = "합계";
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = String.Format("{0:###,###}", nCnt_Tot);
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = String.Format("{0:###,###,###,###}", nJAmt_Tot + nBAmt_Tot + nGAmt_Tot + nMAmt_Tot);
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = String.Format("{0:###,###,###,###}", nJAmt_Tot);
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = String.Format("{0:###,###,###,###}", nBAmt_Tot + nGAmt_Tot + nMAmt_Tot);
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 6].Text = String.Format("{0:###,###,###,###}", nGAmt_Tot);

                    ssList_Sheet1.Rows.Count += 1;
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = "전체합계";
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = String.Format("{0:###,###}", nCnt_Sum);
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = String.Format("{0:###,###,###,###}", nJAmt_Sum + nBAmt_Sum + nGAmt_Sum + nMAmt_Sum);
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = String.Format("{0:###,###,###,###}", nJAmt_Sum);
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = String.Format("{0:###,###,###,###}", nBAmt_Sum + nGAmt_Sum + nMAmt_Sum);
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 6].Text = String.Format("{0:###,###,###,###}", nGAmt_Sum);

                    FarPoint.Win.LineBorder border1 = new FarPoint.Win.LineBorder(Color.Black);
                    for(int z = 0; z < ssList_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1; z++)
                    {
                        ssList_Sheet1.Cells[z, 0, z, ssList_Sheet1.Columns.Count -1].Border = border1;
                        ssList_Sheet1.Rows[z].Height = 30;                        
                    }

                    for(int z = 1; z < ssList_Sheet1.Rows.Count - 1; z+=2)
                    {
                        ssList_Sheet1.Cells[z, 1, z, ssList_Sheet1.Columns.Count - 1].BackColor = Color.LightBlue;
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
