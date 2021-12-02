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
    /// File Name       : frmPmPaVIEWIpdMirBuildCheck
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\misu\misubs\misubs.vbp\misubs38.frm (FrmIpdMirBuildCheck.frm)>> frmPmPaVIEWSakPrint.cs 폼이름 재정의" />

    public partial class frmPmPaVIEWIpdMirBuildCheck : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        string GstrRetValue = "";

        public frmPmPaVIEWIpdMirBuildCheck(string strRetValue)
        {
            GstrRetValue = strRetValue;

            InitializeComponent();
        }

        public frmPmPaVIEWIpdMirBuildCheck()
        {
            InitializeComponent();
        }

        private void frmPmPaVIEWIpdMirBuildCheck_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "1");

            cboJong.Items.Clear();
            cboJong.Items.Add("0.전체");
            cboJong.Items.Add("1.건강보험");
            cboJong.Items.Add("2.의료급여");
            cboJong.Items.Add("3.산재");
            cboJong.Items.Add("4.자보");
            cboJong.SelectedIndex = 1;
            ssView_Sheet1.RowCount = 30;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            int i = 0;
            //int j = 0;
            //int nRead = 0;
            int nRow = 0;
            int nWRTNO = 0;
            string strYYMM = "";
            string strFDate = "";
            string strTdate = "";
            string strJong = "";
            //string strOldData = "";
            //string strNewData = "";
            string strBi = "";
            string strDeptCode = "";
            string strPano = "";
            string strInDate = "";
            string strOutDate = "";
            string strOK = "";
            double nToiAmt = 0;
            double nMirAmt = 0;
            double nChaAmt = 0;
            double nChaRate = 0;
            double nTotToiAmt = 0;
            double nTotMirAmt = 0;
            double nTotChaAmt = 0;

            DataTable dt = null;
            DataTable dtFn = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            strJong = VB.Left(cboJong.Text, 1);



            Cursor.Current = Cursors.WaitCursor;

            //try
            //{
            //'퇴원자 명단을 조회
            if (rdoMir1.Checked == true || rdoMir3.Checked == true)
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano,Bi,TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,SName,Gubun,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(InDate,'YYYY-MM-DD') InDate,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(OutDate,'YYYY-MM-DD') OutDate,DeptCode,Johap ";
                SQL = SQL + ComNum.VBLF + " FROM MISU_BALTEWON ";
                SQL = SQL + ComNum.VBLF + "WHERE ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";

                if (rdoMir1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND Gubun = '1' "; //'퇴원자
                }
                else if (rdoMir2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND Gubun = '2' "; //'중간청구
                }
                else if (rdoMir3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND Gubun = '3' "; //'응급실6시간이상,정신과 낮병동
                }
                SQL = SQL + ComNum.VBLF + "  AND Johap <> 0 ";
                SQL = SQL + ComNum.VBLF + "  AND (WRTNO IS NULL OR WRTNO >= 0) ";
                if (strJong != "0")
                {
                    SQL = SQL + ComNum.VBLF + " AND SuBi = '" + strJong + "' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY Bi,Pano,ActDate,DeptCode ";
            }
            else //'중간청구
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano,Bi,'' ActDate,SName,'2' Gubun,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(StartDate,'YYYY-MM-DD') InDate,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EndDate,'YYYY-MM-DD') OutDate,DeptCode,BuildJAmt Johap ";
                SQL = SQL + ComNum.VBLF + " FROM MIR_IPDID ";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM = '" + strYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "  AND Flag = '1' ";
                if (strJong != "0")
                {
                    SQL = SQL + ComNum.VBLF + " AND SuBi = '" + strJong + "' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY Bi,Pano,DeptCode ";
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

            nRow = 0;
            //strOldData = "";

            //스프레드 출력문
            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {

                strPano = dt.Rows[i]["Pano"].ToString().Trim();
                strBi = dt.Rows[i]["Bi"].ToString().Trim();
                strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                strInDate = dt.Rows[i]["InDate"].ToString().Trim();
                strOutDate = dt.Rows[i]["OutDate"].ToString().Trim();

                //'청구 Build 내역을 READ
                if (strBi == "52")//  '자보
                {
                    SQL = "SELECT WRTNO,JAmt,UpCnt1 ";
                    SQL = SQL + ComNum.VBLF + " FROM MIR_TAID ";
                    SQL = SQL + ComNum.VBLF + "WHERE YYMM='" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND IpdOpd='I' ";
                    SQL = SQL + ComNum.VBLF + "  AND Pano='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND UpCnt1 <> '9' ";  //'보류자
                    SQL = SQL + ComNum.VBLF + "  AND FrDate>=TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                    if (strOutDate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND ToDate<=TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                    }
                }
                else if (strBi == "31")// '산재
                {
                    SQL = "SELECT WRTNO,EDITAMT JAmt,UpCnt1 ";
                    SQL = SQL + ComNum.VBLF + " FROM MIR_SANID ";
                    SQL = SQL + ComNum.VBLF + "WHERE YYMM='" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND IpdOpd='I' ";
                    SQL = SQL + ComNum.VBLF + "  AND UpCnt1 <> '9' ";//'보류자
                    SQL = SQL + ComNum.VBLF + "  AND Pano='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND FrDate>=TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                    if (strOutDate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND ToDate<=TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                    }
                }
                else // '의료보험
                {
                    SQL = "SELECT WRTNO, (EDIJAmt + EDIBOAMT) JAMT,UpCnt1 ";
                    SQL = SQL + ComNum.VBLF + " FROM MIR_INSID ";
                    SQL = SQL + ComNum.VBLF + "WHERE YYMM='" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND IpdOpd='I' ";
                    SQL = SQL + ComNum.VBLF + "  AND Pano='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND Bi='" + strBi + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND UpCnt1 <> '9' ";// '보류자
                    if (strInDate != "")
                    {
                        //SQL = SQL + ComNum.VBLF + "  AND JinDate1>='" + VB.Format (strInDate , "yyyy-MM-dd") + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND JinDate1>='" + Convert.ToDateTime(strInDate).ToString("yyyyMMdd") + "' ";
                    }
                    if (strOutDate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND JinDate1<='" + Convert.ToDateTime(strOutDate).ToString("yyyyMMdd") + "' ";
                    }
                }

                SqlErr = clsDB.GetDataTable(ref dtFn, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (strPano == "94128800")
                {
                    strPano = strPano;
                }

                nMirAmt = 0;
                nWRTNO = 0;
                if (dtFn.Rows.Count > 0)
                {
                    nMirAmt = VB.Val(dtFn.Rows[0]["JAmt"].ToString().Trim());
                    nWRTNO = (int)VB.Val(dtFn.Rows[0]["WRTNO"].ToString().Trim());
                    if (dtFn.Rows[0]["UpCnt1"].ToString().Trim() == "9")
                    {
                        nMirAmt = 0;
                    }
                }

                dtFn.Dispose();
                dtFn = null;

                nToiAmt = (int)VB.Val(dt.Rows[i]["Johap"].ToString().Trim());
                nChaAmt = nMirAmt - nToiAmt;
                nChaRate = 0;

                if (nChaAmt != 0 && (nMirAmt != 0))
                {
                    nChaRate = nChaAmt / nMirAmt * 100;
                }

                //'누계에 ADD
                nTotToiAmt = nTotToiAmt + nToiAmt;
                nTotMirAmt = nTotMirAmt + nMirAmt;
                nTotChaAmt = nTotChaAmt + nChaAmt;


                strOK = "NO";
                if (rdoJob0.Checked == true)//     (0).Value == true ))
                {
                    strOK = "OK";
                }
                else if (rdoJob1.Checked == true)
                {
                    if (nToiAmt != 0 && nMirAmt == 0)
                    {
                        strOK = "OK";
                    }
                }
                else if (rdoJob2.Checked == true)
                {
                    if (nToiAmt != 0 && nMirAmt == 0)
                    {
                        strOK = "OK";
                    }

                    if (nChaRate > 5 || nChaRate < -5)
                    {
                        strOK = "OK";
                    }
                }
                else
                {
                    if (nToiAmt != 0 && nMirAmt == 0)
                    {
                        strOK = "OK";
                    }
                    if (nChaRate > 10 || nChaRate < -10)
                    {
                        strOK = "OK";
                    }
                }



                if (strOK == "OK")
                {
                    nRow = nRow + 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.RowCount = nRow;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = (nToiAmt).ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = nWRTNO.ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = nMirAmt.ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = nChaAmt.ToString("###,###,###,##0");
                }
            }
            dt.Dispose();
            dt = null;

            nRow = nRow + 1;
            ssView_Sheet1.RowCount = nRow;
            ssView_Sheet1.Cells[nRow - 1, 5].Text = "** 합계 **";
            ssView_Sheet1.Cells[nRow - 1, 7].Text = nTotToiAmt.ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotMirAmt.ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotChaAmt.ToString("###,###,###,##0");

            Cursor.Current = Cursors.Default;

            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox (ex.Message);
            //    clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //}
        }


        private void btnreView_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "신 자 감 액 명 단";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            //strHeader += CS.setSpdPrint_String ("등록기간 : " + dtpFdate.Text + " ~ " + dtpTdate.Text , new Font ("굴림체" , 10) , clsSpread.enmSpdHAlign.Left , false , true);
            //strHeader += CS.setSpdPrint_String ("증빙서류 : " + cboBun.Text , new Font ("굴림체" , 10) , clsSpread.enmSpdHAlign.Left , false , true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strPano = "";
            string strSname = "";
            string strBi = "";
            string strDeptCode = "";
            string strInDate = "";
            string strOutDate = "";
            string strActdate = "";
            string strYYMM = "";
            int nWRTNO = 0;
            int eRow = ssView_Sheet1.ActiveRowIndex;

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

            strPano = ssView_Sheet1.Cells[eRow, 1].Text;
            strSname = ssView_Sheet1.Cells[eRow, 2].Text;
            strBi = ssView_Sheet1.Cells[eRow, 3].Text;
            strDeptCode = ssView_Sheet1.Cells[eRow, 4].Text;
            strInDate = ssView_Sheet1.Cells[eRow, 5].Text;
            strOutDate = ssView_Sheet1.Cells[eRow, 6].Text;
            strActdate = ssView_Sheet1.Cells[eRow, 13].Text;
            nWRTNO = (int)VB.Val(ssView_Sheet1.Cells[eRow, 9].Text);
            GstrRetValue = strPano + strBi + "I";
            frmPmpaViewPanoMir frm = new frmPmpaViewPanoMir(GstrRetValue);
            frm.Show();

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                {
                    if (GstrRetValue != "")
                    {
                        ssView_Sheet1.Cells[eRow, 9].Text = VB.Left(GstrRetValue, 10);
                        ssView_Sheet1.Cells[eRow, 10].Text = VB.Val(VB.Mid(GstrRetValue, 11, 10)).ToString();
                        ssView_Sheet1.Cells[eRow, 11].Text = VB.Val(VB.Mid(GstrRetValue, 21, 10)).ToString();
                    }
                }
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strPano = "";
            string strSname = "";
            string strBi = "";
            string strDeptCode = "";
            string strInDate = "";
            string strOutDate = "";
            string strActdate = "";
            string strYYMM = "";
            int nWRTNO = 0;
            int eRow = ssView_Sheet1.ActiveRowIndex;

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

            strPano = ssView_Sheet1.Cells[eRow, 1].Text;
            strSname = ssView_Sheet1.Cells[eRow, 2].Text;
            strBi = ssView_Sheet1.Cells[eRow, 3].Text;
            strDeptCode = ssView_Sheet1.Cells[eRow, 4].Text;
            strInDate = ssView_Sheet1.Cells[eRow, 5].Text;
            strOutDate = ssView_Sheet1.Cells[eRow, 6].Text;
            strActdate = ssView_Sheet1.Cells[eRow, 13].Text;
            nWRTNO = (int)VB.Val(ssView_Sheet1.Cells[eRow, 9].Text);

            if (strBi == "31" || strBi == "52")
            {
                GstrRetValue = strPano + strBi + "I";
                frmPmpaViewPanoMisu frm = new frmPmpaViewPanoMisu(GstrRetValue);
                frm.Show();
                if (GstrRetValue != "")
                {
                    ssView_Sheet1.Cells[eRow, 9].Text = VB.Left(GstrRetValue, 10);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strPano = "";
            string strSname = "";
            string strBi = "";
            string strDeptCode = "";
            string strInDate = "";
            string strOutDate = "";
            string strActdate = "";
            string strYYMM = "";
            int nWRTNO = 0;
            int eRow = ssView_Sheet1.ActiveRowIndex;

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

            strPano = ssView_Sheet1.Cells[eRow, 1].Text;
            strSname = ssView_Sheet1.Cells[eRow, 2].Text;
            strBi = ssView_Sheet1.Cells[eRow, 3].Text;
            strDeptCode = ssView_Sheet1.Cells[eRow, 4].Text;
            strInDate = ssView_Sheet1.Cells[eRow, 5].Text;
            strOutDate = ssView_Sheet1.Cells[eRow, 6].Text;
            strActdate = ssView_Sheet1.Cells[eRow, 13].Text;
            nWRTNO = (int)VB.Val(ssView_Sheet1.Cells[eRow, 9].Text);

            GstrRetValue = nWRTNO.ToString("#########0") + ",";
            GstrRetValue = GstrRetValue + strActdate + ",";
            GstrRetValue = GstrRetValue + strPano + ",";
            GstrRetValue = GstrRetValue + strSname + ",";
            GstrRetValue = GstrRetValue + strBi + ",";
            GstrRetValue = GstrRetValue + strInDate + ",";
            GstrRetValue = GstrRetValue + strYYMM + ",";

            frmPmpaViewSlip2MirCheckTwo frm = new frmPmpaViewSlip2MirCheckTwo(GstrRetValue);
            frm.Show();
        }
    }
}
