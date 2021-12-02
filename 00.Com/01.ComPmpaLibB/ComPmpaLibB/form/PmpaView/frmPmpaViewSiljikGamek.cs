using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    public partial class frmPmpaViewSiljikGamek : Form
    {
        /// <summary>
		/// Class Name      : ComPmpaLibB
		/// File Name       : frmPmpaViewSiljikGamek.cs
		/// Description     : 실직자 할인 현황 조회 폼
		/// Author          : 안정수
		/// Create Date     : 2017-08-19
		/// Update History  : 
		/// <history>       
		/// d:\psmh\OPD\oviewa\OVIEWA18.FRM(FrmSiljikGamek) => frmPmpaViewSiljikGamek.cs 으로 변경함
		/// </history>
        /// 쿼리실행시 테이블 또는 뷰가 존재하지 않는다는 오류가 발생... 쿼리 확인 및 테스트 필요함
		/// <seealso>
		/// d:\psmh\OPD\oviewa\OVIEWA18.FRM(FrmSiljikGamek)
		/// </seealso>
		/// </summary>
        ComFunc CF = new ComFunc();
        public frmPmpaViewSiljikGamek()
        {
            InitializeComponent();
            setEvent();
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

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            dtpFdate.Text = Convert.ToDateTime(CurrentDate).AddDays(-10).ToShortDateString();
            dtpTdate.Text = Convert.ToDateTime(CurrentDate).AddDays(-1).ToShortDateString();
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
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


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            strTitle = dtpFdate.Text + "부터 " + dtpTdate.Text + "까지 실직자 할인 현황";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, false, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eGetData()
        {
            int i = 0;
            int nRow = 0;
            int nREAD = 0;
            int nIlsu = 0;

            string strNewData = "";
            string strOldData = "";

            double nAmt1 = 0;
            double nAmt2 = 0;
            double nAmt3 = 0;

            double nSAmt1 = 0;
            double nSAmt2 = 0;
            double nSAmt3 = 0;

            double nTAmt1 = 0;
            double nTAmt2 = 0;
            double nTAmt3 = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            btnView.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = false;

            //누적할 배열을 Clear
            nSAmt1 = 0;
            nSAmt2 = 0;
            nSAmt3 = 0;

            nTAmt1 = 0;
            nTAmt2 = 0;
            nTAmt3 = 0;

            //입원 실직자 감액 할인 내역
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                            ";
            SQL += ComNum.VBLF + "  T.Pano, T.Sname, T.Bi, TO_CHAR(InDate, 'YY-MM-DD') InDate,                      ";
            SQL += ComNum.VBLF + "  TO_CHAR(OutDate, 'YY-MM-DD') OutDate, Amt50, Amt53, Amt54,                      ";
            SQL += ComNum.VBLF + "  Jumin1||'-'||Jumin2 JUMINNO                                                     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TM T, " + ComNum.DB_PMPA + "BAS_Patient P          ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                         ";
            SQL += ComNum.VBLF + "      AND ActDate >= TO_DATE('" + dtpFdate.Text + "','yyyy-mm-dd')                ";
            SQL += ComNum.VBLF + "      AND ActDate <= TO_DATE('" + dtpTdate.Text + "','yyyy-mm-dd')                ";
            SQL += ComNum.VBLF + "      AND t.pano  > ''                                                            ";
            SQL += ComNum.VBLF + "      AND T.PANO   = P.PANO                                                       ";
            SQL += ComNum.VBLF + "      AND T.GbGamek  = 'G'                                                        ";
            SQL += ComNum.VBLF + "      AND Amt54 <> 0                                                              ";
            SQL += ComNum.VBLF + "ORDER BY OutDate                                                                  ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnView.Enabled = true;
                    btnPrint.Enabled = true;
                    btnExit.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                   
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    btnView.Enabled = true;
                    btnPrint.Enabled = true;
                    btnExit.Enabled = true;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;
                    ssList_Sheet1.Rows.Count = nREAD;

                    nRow = 0;

                    for (i = 0; i < nREAD; i++)
                    {
                        nAmt1 = VB.Val(dt.Rows[i]["Amt50"].ToString().Trim()); // 총진료비
                        nAmt2 = VB.Val(dt.Rows[i]["Amt53"].ToString().Trim()); // 조합부담
                        nAmt3 = VB.Val(dt.Rows[i]["Amt54"].ToString().Trim()); // 할인액

                        if (nAmt3 != 0)
                        {
                            nSAmt1 += nAmt1;
                            nSAmt2 += nAmt2;
                            nSAmt3 += nAmt3;

                            nRow += 1;

                            if (nRow > ssList_Sheet1.Rows.Count)
                            {
                                ssList_Sheet1.Rows.Count = nRow + 10;
                            }

                            ssList_Sheet1.Cells[nRow, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow, 2].Text = "입원";
                            ssList_Sheet1.Cells[nRow, 3].Text = dt.Rows[i]["JuminNO"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow, 4].Text = CF.Read_Bi_Name(clsDB.DbCon, dt.Rows[i]["Bi"].ToString().Trim(), "1");
                            ssList_Sheet1.Cells[nRow, 5].Text = dt.Rows[i]["InDate"].ToString().Trim() + "~" + dt.Rows[i]["OutDate"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow, 6].Text = String.Format("{0:###,###,###,##0}", nAmt1);
                            ssList_Sheet1.Cells[nRow, 7].Text = String.Format("{0:###,###,###,##0}", nAmt1 - nAmt2);
                            ssList_Sheet1.Cells[nRow, 8].Text = String.Format("{0:###,###,###,##0}", nAmt1 - nAmt2 - nAmt3);
                            ssList_Sheet1.Cells[nRow, 9].Text = String.Format("{0:###,###,###,##0}", nAmt3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                btnView.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            //입원의 소계를 Display
            nTAmt1 += nSAmt1;
            nTAmt2 += nSAmt2;
            nTAmt3 += nSAmt3;

            if (nTAmt3 != 0)
            {
                nRow += 1;

                if (nRow > ssList_Sheet1.Rows.Count)
                {
                    ssList_Sheet1.Rows.Count = nRow + 10;
                }

                ssList_Sheet1.Cells[nRow, 3].Text = "** 입원소계 **";
                ssList_Sheet1.Cells[nRow, 6].Text = String.Format("{0:###,###,###,##0}", nSAmt1);
                ssList_Sheet1.Cells[nRow, 7].Text = String.Format("{0:###,###,###,##0}", nSAmt1 - nSAmt2);
                ssList_Sheet1.Cells[nRow, 8].Text = String.Format("{0:###,###,###,##0}", nSAmt1 - nSAmt2 - nSAmt3);
                ssList_Sheet1.Cells[nRow, 9].Text = String.Format("{0:###,###,###,##0}", nSAmt3);
            }

            //외래의 감액내역을 READ
            nSAmt1 = 0;
            nSAmt2 = 0;
            nSAmt3 = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  TO_CHAR(Actdate,'YYYY-MM-DD') ActDate,Pano,DeptCode,Bi";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_Master";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ActDate >= TO_DATE('" + dtpFdate.Text + "','yyyy-mm-dd')";
            SQL += ComNum.VBLF + "      AND ActDate <= TO_DATE('" + dtpTdate.Text + "','yyyy-mm-dd')";
            SQL += ComNum.VBLF + "      AND GbGamek  = 'G' ";   //실직자 감액
            SQL += ComNum.VBLF + "ORDER BY Pano,ActDate,DeptCode,Bi ";

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
                    btnView.Enabled = true;
                    btnPrint.Enabled = true;
                    btnExit.Enabled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                btnView.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            strOldData = "";

            if (dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    //총진료비, 조합부담, 본인부담 READ
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                                        ";
                    SQL += ComNum.VBLF + "  SUM(DECODE(Bun,'96',Amt1+Amt2,0)) MAmt,                                                     ";
                    SQL += ComNum.VBLF + "  SUM(DECODE(Bun,'92',Amt1+Amt2,0)) HAmt,                                                     ";
                    SQL += ComNum.VBLF + "  SUM(DECODE(Bun,'98',Amt1+Amt2,0)) JAmt,                                                     ";
                    SQL += ComNum.VBLF + "  SUM(DECODE(Bun,'99',Amt1+Amt2,0)) BAmt,                                                     ";
                    SQL += ComNum.VBLF + "  Sname, Jumin1||'-'|| Jumin2 JUMINNO                                                         ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP S, " + ComNum.DB_PMPA + "BAS_Patient P                    ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
                    SQL += ComNum.VBLF + "      AND S.Pano = '" + dt.Rows[i]["Pano"].ToString().Trim() + "'                             ";
                    SQL += ComNum.VBLF + "      AND S.ActDate = TO_DATE('" + dt.Rows[i]["ActDate"].ToString().Trim() + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "      AND S.DeptCode = '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'                     ";
                    SQL += ComNum.VBLF + "      AND S.Bi = '" + dt.Rows[i]["Bi"].ToString().Trim() + "'                                 ";
                    SQL += ComNum.VBLF + "GROUP BY Sname, Jumin1||'-'|| Jumin2                                                          ";

                    try
                    {

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            btnView.Enabled = true;
                            btnPrint.Enabled = true;
                            btnExit.Enabled = true;
                            return;
                        }

                        if (dt1.Rows.Count == 1)
                        {
                            nAmt2 = VB.Val(dt1.Rows[0]["JAmt"].ToString().Trim()); // 조합부담
                            nAmt3 = VB.Val(dt1.Rows[0]["HAmt"].ToString().Trim()); // 할인액
                            nAmt1 = nAmt2 + nAmt3 + VB.Val(dt1.Rows[0]["BAmt"].ToString().Trim()) + VB.Val(dt1.Rows[0]["MAmt"].ToString().Trim()); // 총진료비
                        }

                    }
                    catch (Exception ex)
                    {
                        btnView.Enabled = true;
                        btnPrint.Enabled = true;
                        btnExit.Enabled = true;
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (nAmt3 != 0)
                    {
                        nRow += 1;
                        if (nRow > ssList_Sheet1.Rows.Count)
                        {
                            ssList_Sheet1.Rows.Count = nRow + 10;
                        }

                        if (i == 0)
                        {
                            ssList_Sheet1.Cells[nRow, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow, 2].Text = "외래";
                            ssList_Sheet1.Cells[nRow, 3].Text = dt.Rows[i]["JUMINNO"].ToString().Trim();
                        }
                        else if (strNewData != dt.Rows[i]["Pano"].ToString().Trim())
                        {
                            ssList_Sheet1.Cells[nRow, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow, 2].Text = "외래";
                            ssList_Sheet1.Cells[nRow, 3].Text = dt.Rows[i]["JUMINNO"].ToString().Trim();
                        }

                        strNewData = dt.Rows[i]["Pano"].ToString().Trim();

                        ssList_Sheet1.Cells[nRow, 4].Text = CF.Read_Bi_Name(clsDB.DbCon, dt.Rows[i]["Bi"].ToString().Trim(), "1");
                        ssList_Sheet1.Cells[nRow, 5].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow, 6].Text = String.Format("{0:###,###,###,##0}", nAmt1);
                        ssList_Sheet1.Cells[nRow, 7].Text = String.Format("{0:###,###,###,##0}", nAmt1 - nAmt2);
                        ssList_Sheet1.Cells[nRow, 8].Text = String.Format("{0:###,###,###,##0}", nAmt1 - nAmt2 - nAmt3);
                        ssList_Sheet1.Cells[nRow, 9].Text = String.Format("{0:###,###,###,##0}", nAmt3);

                        //외래소계에 누적
                        nSAmt1 += nAmt1;
                        nSAmt2 += nAmt2;
                        nSAmt3 += nAmt3;
                    }
                }
            }

            dt.Dispose();
            dt = null;

            //외래의 소계를 Display
            nTAmt1 += nSAmt1;
            nTAmt2 += nSAmt2;
            nTAmt3 += nSAmt3;

            nRow += 1;

            if (nRow > ssList_Sheet1.Rows.Count)
            {
                ssList_Sheet1.Rows.Count = nRow + 10;
            }

            ssList_Sheet1.Cells[nRow, 3].Text = "** 외래소계 **";
            ssList_Sheet1.Cells[nRow, 6].Text = String.Format("{0:###,###,###,##0}", nSAmt1);
            ssList_Sheet1.Cells[nRow, 7].Text = String.Format("{0:###,###,###,##0}", nSAmt1 - nSAmt2);
            ssList_Sheet1.Cells[nRow, 8].Text = String.Format("{0:###,###,###,##0}", nSAmt1 - nSAmt2 - nSAmt3);
            ssList_Sheet1.Cells[nRow, 9].Text = String.Format("{0:###,###,###,##0}", nSAmt3);

            nRow += 1;
            ssList_Sheet1.Rows.Count = nRow;

            ssList_Sheet1.Cells[nRow, 3].Text = "** 합    계 **";
            ssList_Sheet1.Cells[nRow, 6].Text = String.Format("{0:###,###,###,##0}", nTAmt1);
            ssList_Sheet1.Cells[nRow, 7].Text = String.Format("{0:###,###,###,##0}", nTAmt1 - nTAmt2);
            ssList_Sheet1.Cells[nRow, 8].Text = String.Format("{0:###,###,###,##0}", nTAmt1 - nTAmt2 - nTAmt3);
            ssList_Sheet1.Cells[nRow, 9].Text = String.Format("{0:###,###,###,##0}", nTAmt3);

            btnView.Enabled = true;
            btnPrint.Enabled = true;
            btnExit.Enabled = true;

        }

    }
}
