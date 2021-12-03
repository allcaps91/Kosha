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
    /// File Name       : frmPmpaViewMisuTran1.cs
    /// Description     : 미수금변동총괄표
    /// Author          : 박창욱
    /// Create Date     : 2017-09-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUM202.FRM(FrmMisuTran1.frm) >> frmPmpaViewMisuTran1.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMisuTran1 : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        double[,] nTotAmt = new double[22, 12];

        public frmPmpaViewMisuTran1()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nClass = 0;
            string strIpdOpd = "";
            string strYYMM = "";
            string strBackYYMM = "";
            string strFDate = "";
            string strTDate = "";

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strBackYYMM = clsVbfunc.DateYYMMAdd(strYYMM, -1);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strYYMM, 4)), (int)VB.Val(VB.Right(strYYMM, 2)));

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.Cells[2, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            for (i = 0; i < 22; i++)
            {
                for (k = 0; k < 12; k++)
                {
                    nTotAmt[i, k] = 0;
                }
            }

            try
            {
                //전월이월 READ
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Class, IpdOpd, SUM(JanAmt) cIwolAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GELTOT";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strBackYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND Class <= '07'";
                SQL = SQL + ComNum.VBLF + "  GROUP BY Class,IpdOpd";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Class,IpdOpd";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nClass = (int)VB.Val(dt.Rows[i]["Class"].ToString().Trim());
                    strIpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    if (nClass == 20)
                    {
                        nClass = 6;
                    }
                    if (nClass == 7)
                    {
                        nClass = 6;
                    }
                    if (strIpdOpd == "I")
                    {
                        k = nClass * 3 - 2;
                    }  //입원
                    if (strIpdOpd != "I")
                    {
                        k = nClass * 3 - 1;
                    }  //외래
                    nTotAmt[k, 1] += VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim());  //전월이월
                }
                dt.Dispose();
                dt = null;


                //당월 삭감율 계산용 금액을 READ
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Class, IpdOpd, SUM(TotMAmt) cTotMAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(TotSAmt) cTotSAmt, SUM(MisuAmt) cMisuAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GELTOT";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND Class <= '07'";
                SQL = SQL + ComNum.VBLF + "  GROUP BY Class,IpdOpd";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Class,IpdOpd";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nClass = (int)VB.Val(dt.Rows[i]["Class"].ToString().Trim());
                    strIpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    if (nClass == 20)
                    {
                        nClass = 6;
                    }
                    if (nClass == 7)
                    {
                        nClass = 6;
                    }
                    if (strIpdOpd == "I")
                    {
                        k = nClass * 3 - 2;
                    }  //입원
                    if (strIpdOpd != "I")
                    {
                        k = nClass * 3 - 1;
                    }  //외래
                    nTotAmt[k, 10] += VB.Val(dt.Rows[i]["cTotMAmt"].ToString().Trim());  //삭감 총진료비
                    nTotAmt[k, 11] += VB.Val(dt.Rows[i]["cTotSAmt"].ToString().Trim());  //삭감 총삭감액

                    if (nClass == 6)    //자보
                    {
                        nTotAmt[k, 2] += VB.Val(dt.Rows[i]["cMisuAmt"].ToString().Trim());  //전월이월 
                    }
                }
                dt.Dispose();
                dt = null;


                //반송, 과지급금, 계산착오액 READ & ADD
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Class, IpdOpd,";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(SUBSTR(Gubun,1,1),'1',Amt,0)) cMirAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(SUBSTR(Gubun,1,1),'2',Amt,0)) cIpgumAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(Gubun,'31',Amt, '35', Amt, 0)) cSakAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(Gubun,'32',Amt,0)) cBanAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(Gubun,'33',Amt,0)) cGaipAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(Gubun,'34',Amt,0)) cEtcAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND Gubun >  '10'";
                SQL = SQL + ComNum.VBLF + "    AND Class <= '07'";
                SQL = SQL + ComNum.VBLF + "  GROUP BY Class,IpdOpd";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Class,IpdOpd";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nClass = (int)VB.Val(dt.Rows[i]["Class"].ToString().Trim());
                    strIpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    if (nClass == 7)
                    {
                        nClass = 6;
                    }
                    if (strIpdOpd == "I")
                    {
                        k = nClass * 3 - 2;
                    }  //입원
                    if (strIpdOpd != "I")
                    {
                        k = nClass * 3 - 1;
                    }  //외래

                    nTotAmt[k, 2] += VB.Val(dt.Rows[i]["cMirAmt"].ToString().Trim());   //청구
                    nTotAmt[k, 4] += VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());   //삭감
                    nTotAmt[k, 3] += VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim()); //입금
                    nTotAmt[k, 6] += VB.Val(dt.Rows[i]["cBanAmt"].ToString().Trim());   //반송
                    nTotAmt[k, 7] += VB.Val(dt.Rows[i]["cGaipAmt"].ToString().Trim());  //과지급금
                    nTotAmt[k, 8] += VB.Val(dt.Rows[i]["cEtcAmt"].ToString().Trim());   //계산착오
                }
                dt.Dispose();
                dt = null;



                //소계 계산 및 Sheet에 Display
                for (i = 1; i < 7; i++)
                {
                    //미수종류별 외래, 입원 월말잔액을 구하기
                    nTotAmt[i * 3 - 2, 9] = nTotAmt[i * 3 - 2, 1] + nTotAmt[i * 3 - 2, 2];
                    for (k = 3; k < 9; k++)
                    {
                        nTotAmt[i * 3 - 2, 9] -= nTotAmt[i * 3 - 2, k];
                    }
                    nTotAmt[i * 3 - 1, 9] = nTotAmt[i * 3 - 1, 1] + nTotAmt[i * 3 - 1, 2];
                    for (k = 3; k < 9; k++)
                    {
                        nTotAmt[i * 3 - 1, 9] -= nTotAmt[i * 3 - 1, k];
                    }

                    for (k = 1; k < 12; k++)
                    {
                        //미수종류별 소계 구하기
                        nTotAmt[i * 3, k] += nTotAmt[i * 3 - 2, k];
                        nTotAmt[i * 3, k] += nTotAmt[i * 3 - 1, k];
                        //전체합계
                        nTotAmt[19, k] += nTotAmt[i * 3 - 2, k];  //입원
                        nTotAmt[20, k] += nTotAmt[i * 3 - 1, k];  //외래
                        nTotAmt[21, k] += nTotAmt[i * 3, k];  //소계
                    }
                }


                //Sheet에 Display
                for (i = 1; i < 22; i++)
                {
                    //삭감율 구하기
                    if (nTotAmt[i, 10] != 0 && nTotAmt[i, 11] != 0)
                    {
                        nTotAmt[i, 5] = nTotAmt[i, 11] / nTotAmt[i, 10] * 100;
                    }

                    for (k = 1; k < 10; k++)
                    {
                        if (k != 5)
                        {
                            ssView_Sheet1.Cells[i + 1, k + 1].Text = nTotAmt[i, k].ToString("###,###,###,##0 ");
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i + 1, k + 1].Text = nTotAmt[i, k].ToString("###0.00") + "% ";
                        }
                    }
                }


                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = cboYYMM.Text + "미수금 변동현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력시간 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("작성자 : " + clsType.User.JobMan, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void frmPmpaViewMisuTran1_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 15, "", "1");
            cboYYMM.SelectedIndex = 0;
        }
    }
}
