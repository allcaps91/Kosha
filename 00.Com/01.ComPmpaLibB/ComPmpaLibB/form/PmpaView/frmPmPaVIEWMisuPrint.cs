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
    /// File Name       : frmPmPaVIEWMisuPrint
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\misu\misuta.vbp\MISUT205.FRM (CmdPrint.frm)>> frmPmPaVIEWSakPrint.cs 폼이름 재정의" />
    /// 
    public partial class frmPmPaVIEWMisuPrint : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string[] GstrGels = new string[51];

        public frmPmPaVIEWMisuPrint()
        {
            InitializeComponent();
        }


        private void frmPmPaVIEWMisuPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            cboGel.Items.Add("전   체");
            GstrGels[0] = "9999";
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT MiaCode GelCode, MiaName                   ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA                  ";
                SQL = SQL + ComNum.VBLF + "  WHERE (MiaCode LIKE 'JK%' OR MiaCode LIKE 'KB%') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY MiaCode                                 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboGel.Items.Add(dt.Rows[i]["MiaName"].ToString().Trim());
                    GstrGels[i + 1] = dt.Rows[i]["GelCode"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                cboGel.SelectedIndex = 0;
                clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "1");
                cboYYMM.SelectedIndex = 1;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int nRowCnt = 0;
            int i = 0;
            int j = 0;
            int nCnt = 0;
            int nSCnt = 0;
            int nTCnt = 0;
            int nTICnt = 0;
            int nTOCnt = 0;
            double nSAmt6 = 0;
            double nTAmt6 = 0;
            double nSAmt1 = 0;
            double nTAmt1 = 0;
            double nSIpGum = 0;
            double nTIpGum = 0;
            double nGTIAmt6 = 0;
            double nGTOAmt6 = 0;
            double nGTIAmt1 = 0;
            double nGTOAmt1 = 0;
            double nGTIIpGum = 0;
            double nGTOIpGum = 0;
            string strYYMM = "";
            string strLdate = "";
            string strSqlGel = "";
            string strCoIPDOPD = "";
            string strCoGel = "";
            string strCoWrtno = "";
            string strFirst = "";
            string strSysDate = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //
            int nSeq = 0;
            string strIpdOpd = "";
            string strPano = "";
            string strSname = "";
            string strGelCode = "";
            string strDeptCode = "";
            string strRemark = "";
            string strCarNo = "";
            string strBname = "";
            string strDate1 = "";
            string strDate2 = "";
            string strDate3 = "";
            string strDate4 = "";
            string strDname = "";
            string strGelName = "";
            double nAmt6 = 0;
            double nAmt1 = 0;
            double nIpGum = 0;
            DataTable dtFn = null;



            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strLdate = CF.READ_LASTDAY(clsDB.DbCon, VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01");

            if (string.Compare(strLdate, strDTP) > 0)
            {
                strLdate = strDTP;
            }

            ssView_Sheet1.RowCount = 0;

            nGTIAmt6 = 0;
            nGTOAmt6 = 0;
            nGTIAmt1 = 0;
            nGTIIpGum = 0;
            nGTOAmt1 = 0;
            nGTOIpGum = 0;
            nTICnt = 0;
            nTOCnt = 0;

            strFirst = "OK";

            strSqlGel = GstrGels[cboGel.SelectedIndex];

            progressBar1.Value = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = " ";
                SQL = SQL + ComNum.VBLF + " SELECT M.MisuID,M.Remark,M.GelCode,M.DeptCode,M.IpdOpd,M.WRTNO, M.MGRRANK,    ";
                SQL = SQL + ComNum.VBLF + "        To_CHAR(M.Bdate,'YYYY-MM-DD') BDate,                                   ";
                SQL = SQL + ComNum.VBLF + "        To_CHAR(M.FromDate,'YYYY-MM-DD') FDate,                                ";
                SQL = SQL + ComNum.VBLF + "        To_CHAR(M.ToDate,'YYYY-MM-DD') TDate,a.JanAmt, M.Amt1, M.Amt3, b.Dname ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_Monthly a,MISU_IDMST M, BAS_SANID b         ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                                                    ";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM = '" + strYYMM + "'                                             ";
                SQL = SQL + ComNum.VBLF + "    AND a.Class = '07'                                                         ";  //'자보
                SQL = SQL + ComNum.VBLF + "    AND a.JanAmt > 0                                                            ";   //'분쟁심의 함계장 요청
                SQL = SQL + ComNum.VBLF + "    AND M.MisuId = b.Pano                                                      ";
                SQL = SQL + ComNum.VBLF + "    AND B.BI ='52'                                                             ";

                if (strSqlGel != "9999")
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.GelCode = '" + strSqlGel.Trim() + "'                              ";
                }

                SQL = SQL + ComNum.VBLF + "    And a.WRTNO = M.WRTNO                                                      ";

                if (rdo0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY M.Gelcode,M.MisuID,M.BDate,M.IpdOpd                             ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY M.Gelcode,M.BDate,M.MisuID,M.IpdOpd                             ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                nRowCnt = dt.Rows.Count;

                progressBar1.Maximum = dt.Rows.Count;
                for (i = 0; i < nRowCnt; i++)
                {
                    progressBar1.Value = i + 1;

                    #region ADD_SPREAD
                    nCnt = nCnt + 1;
                    nSCnt = nSCnt + 1;
                    nTCnt = nTCnt + 1;

                    strPano = dt.Rows[i]["MisuID"].ToString().Trim();
                    strSname = CPF.Get_BasPatient(clsDB.DbCon, strPano).Rows[0]["SNAME"].ToString().Trim();
                    strGelCode = dt.Rows[i]["GelCode"].ToString().Trim();
                    strDeptCode = dt.Rows[i]["Deptcode"].ToString().Trim();
                    strRemark = VB.Left(dt.Rows[i]["Remark"].ToString().Trim() + VB.Space(40), 40);
                    strCarNo = (VB.Mid(strRemark, 17, 14)).Trim();
                    strBname = (VB.Right(strRemark, 14)).Trim();
                    strDate1 = dt.Rows[i]["BDate"].ToString().Trim();
                    strDate2 = dt.Rows[i]["FDate"].ToString().Trim();
                    strDate3 = dt.Rows[i]["TDate"].ToString().Trim();
                    strDate4 = (VB.Mid(strRemark, 1, 8)).Trim();
                    strDname = dt.Rows[i]["Dname"].ToString().Trim();
                    nAmt6 = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());
                    nAmt1 = VB.Val(dt.Rows[i]["Amt1"].ToString().Trim());

                    if (strPano == "05121724")
                    {
                        i = i;
                    }
                    // 'MISU_SLIP에서 해당월말 이전에 입금된 금액을 READ

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT SUM(Amt) JungIpgum                                 ";
                    SQL = SQL + ComNum.VBLF + "   FROM MISU_SLIP                                          ";
                    SQL = SQL + ComNum.VBLF + "  WHERE WRTNO=" + VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim()) + "  ";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun>='21' AND Gubun<='29'                        ";
                    SQL = SQL + ComNum.VBLF + "    AND BDate<=TO_DATE('" + strLdate + "','YYYY-MM-DD')    ";

                    SqlErr = clsDB.GetDataTable(ref dtFn, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    nIpGum = 0;

                    if (dtFn.Rows.Count > 0)
                    {
                        nIpGum = VB.Val(dtFn.Rows[0]["JungIpgum"].ToString().Trim());
                    }

                    dtFn.Dispose();
                    dtFn = null;

                    if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                    {
                        strIpdOpd = "입원";
                    }
                    if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "O")
                    {
                        strIpdOpd = "외래";
                    }

                    if (strIpdOpd == "입원")
                    {
                        nTICnt = nTICnt + 1;
                        nGTIAmt6 = nGTIAmt6 + nAmt6;
                        nGTIAmt1 = nGTIAmt1 + nAmt1;
                        nGTIIpGum = nGTIIpGum + nIpGum;
                    }
                    else
                    {
                        nTOCnt = nTOCnt + 1;
                        nGTOAmt6 = nGTOAmt6 + nAmt6;
                        nGTOAmt1 = nGTOAmt1 + nAmt1;
                        nGTOIpGum = nGTOIpGum + nIpGum;
                    }


                    if (strFirst == "OK")
                    {
                        strCoGel = strGelCode;
                        strCoIPDOPD = strIpdOpd;
                        strFirst = "NO";
                        nCnt = 0;
                        nSCnt = 0;
                        nTCnt = 0;
                    }

                    if (strGelCode != strCoGel)
                    {
                        Misu_Sub_Rtn(ref nSCnt, ref nTAmt1, ref nTAmt6, ref nSeq, ref nTIpGum, ref strCoGel, ref strCoIPDOPD, strGelCode, strIpdOpd);
                    }

                    strGelName = CPF.GET_BAS_MIA(clsDB.DbCon, (strGelCode).Trim());

                    nSeq = nSeq + 1;


                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

                    if (nSeq != 1)
                    {
                        strGelName = "";
                    }
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strGelName;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2 - 1].Text = nSeq.ToString();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3 - 1].Text = strDate1;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4 - 1].Text = strPano;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5 - 1].Text = strSname;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6 - 1].Text = (strDate2.Trim() + "-" + strDate3.Trim());
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7 - 1].Text = strIpdOpd;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8 - 1].Text = strDeptCode;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9 - 1].Text = strDate4;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10 - 1].Text = strBname;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11 - 1].Text = strCarNo;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12 - 1].Text = strDname;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13 - 1].Value = nAmt1;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14 - 1].Value = nIpGum;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 15 - 1].Value = nAmt6;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 16 - 1].Text = VB.DateDiff("d", Convert.ToDateTime(strDate1), Convert.ToDateTime(strLdate)).ToString();

                    nSAmt6 = nSAmt6 + nAmt6;  //'현잔액
                    nTAmt6 = nTAmt6 + nAmt6;  //'현잔액

                    nSAmt1 = nSAmt1 + nAmt1;  //'총청구액
                    nTAmt1 = nTAmt1 + nAmt1;  //'총청구액

                    nSIpGum = nSIpGum + nIpGum;  //'입금액
                    nTIpGum = nTIpGum + nIpGum;  //'입금액

                    switch (dt.Rows[i]["MGRRANK"].ToString().Trim())
                    {
                        case "4":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 17 - 1].Text = "재판중";
                            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 200, 200);
                            break;
                        case "6":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 17 - 1].Text = "분쟁심의";
                            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 200, 200);
                            break;
                        case "A":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 17 - 1].Text = "분심(중)";
                            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 200, 200);
                            break;
                        case "B":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 17 - 1].Text = "수납예정";
                            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 200, 200);
                            break;
                        case "C":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 17 - 1].Text = "자문보류";
                            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 200, 200);
                            break;
                        case "D":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 17 - 1].Text = "소송(중)";
                            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 200, 200);
                            break;
                        case "E":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 17 - 1].Text = "이의제기";
                            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 200, 200);
                            break;
                        case "7":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 17 - 1].Text = "일부입금";
                            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 200, 200);
                            break;
                        case "8":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 17 - 1].Text = "문제건";
                            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 200, 200);
                            break;

                    }
                    #endregion

                }
                dt.Dispose();
                dt = null;

                if (strSqlGel == "9999")
                {
                    Misu_Sub_Rtn(ref nSCnt, ref nTAmt1, ref nTAmt6, ref nSeq, ref nTIpGum, ref strCoGel, ref strCoIPDOPD, strGelCode, strIpdOpd);
                }
                MISU_Total_Rtn(ref nTOCnt, ref nGTOAmt6, ref nGTOAmt1, ref nGTOIpGum, ref nTICnt, ref nGTIAmt6, ref nGTIAmt1, ref nGTIIpGum);

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        #region GoSub

        private void Misu_Sub_Rtn(ref int nSCnt, ref double nTAmt1, ref double nTAmt6, ref int nSeq, ref double nTIpGum, ref string strCoGel, ref string strCoIPDOPD, string strGelCode, string strIpdOpd)
        {
            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(80, 240, 180);
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = "소  계";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nSCnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = nTAmt1.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = nTIpGum.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Text = nTAmt6.ToString();

            nTAmt6 = 0;
            nSeq = 0;
            nSCnt = 0;
            nTAmt1 = 0;
            nTIpGum = 0;
            strCoGel = strGelCode;
            strCoIPDOPD = strIpdOpd;
        }

        private void MISU_Total_Rtn(ref int nTOCnt, ref double nGTOAmt6, ref double nGTOAmt1, ref double nGTOIpGum, ref int nTICnt, ref double nGTIAmt6, ref double nGTIAmt1, ref double nGTIIpGum)
        {
            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(120, 240, 180);
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "  외 래 합 계 ";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nTOCnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Value = nGTOAmt1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Value = nGTOIpGum;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Value = nGTOAmt6;


            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(120, 240, 180);
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "  입 원 합 계 ";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nTICnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Value = nGTIAmt1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Value = nGTIIpGum;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Value = nGTIAmt6;

            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(120, 240, 180);
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "  전 체 합 계 ";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = (nTOCnt + nTICnt) + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Value = (nGTOAmt1 + nGTIAmt1);
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Value = (nGTOIpGum + nGTIIpGum);
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Value = (nGTOAmt6 + nGTIAmt6);

            nTOCnt = 0;
            nGTOAmt6 = 0;
            nGTOAmt1 = 0;
            nGTOIpGum = 0;
            nTICnt = 0;
            nGTIAmt6 = 0;
            nGTIAmt1 = 0;
            nGTIIpGum = 0;

        }

        #endregion

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            //if (ComQuery.IsJobAuth (this , "P", clsDB.DbCon) == false) { return; }     //권한확인

            strTitle = "자보 진료비 미수 현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(90) + "담당자:" + clsType.User.JobName + " 인", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            //strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 25, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, true,0.89f);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
