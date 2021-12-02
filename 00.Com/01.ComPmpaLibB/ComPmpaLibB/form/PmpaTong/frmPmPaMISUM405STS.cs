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
    /// File Name       : frmPmPaMISUM405STS.cs
    /// Description     : 조합청구 예상액과 실청구액 점검
    /// Author          : 김효성
    /// Create Date     : 2017-09-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 이 프로그램은 2001-12-31일까지만 사용이 가능함 현재 날짜에서 실행이 되지 않도록 막음
    /// 
    /// </history>
    /// <seealso cref= psmh\misu\misumir.vbp\MISUM405 . FRM  >> frmPmPaMISUM405STS.cs 폼이름 재정의" />	

    public partial class frmPmPaMISUM405STS : Form
    {
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();

        public frmPmPaMISUM405STS()
        {
            InitializeComponent();
        }

        private void frmPmPaMISUM405STS_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;
            int i = 0;

            nYY = (int)VB.Val(VB.Left(strDTP, 4));
            nMM = (int)VB.Val(VB.Mid(strDTP, 6, 2));

            for (i = 1; i <= 15; i++)
            {
                cboyyyy.Items.Add((nYY).ToString("0000") + "년 " + (nMM).ToString("00") + "월분");

                nMM = nMM - 1;
                if (nMM == 0)
                {
                    nMM = 12;
                    nYY = nYY - 1;
                }
            }
            cboyyyy.SelectedIndex = 0;

            cboJong.Items.Add("1.국민공단");
            cboJong.Items.Add("2.직장조합");
            cboJong.Items.Add("3.의료보호");
            cboJong.Items.Add("4.산재");
            cboJong.Items.Add("5.자보");
            cboJong.Items.Add("6.보험+보호");
            cboJong.Items.Add("7.산재+자보");
            cboJong.SelectedIndex = 0;

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            int nRow = 0;
            string strJong = "";
            string strYYMM = "";
            string strOldData = "";
            string strNewData = "";
            string strSname = "";
            string strDeptCode = "";
            long nMirAmt1 = 0;
            long nMirAmt2 = 0;
            long nMirAmt3 = 0;
            long nMirTot1 = 0;
            long nMirTot2 = 0;
            long nMirTot3 = 0;
            long nChaek = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strYYMM = VB.Left(cboyyyy.Text, 4) + VB.Mid(cboyyyy.Text, 7, 2);
            strJong = VB.Left(cboJong.Text, 1);

            if (string.Compare(strYYMM, "200201") >= 0)
            {
                ComFunc.MsgBox("이 프로그램은 2001-12-31일까지만 사용이 가능함", "확인");
                cboyyyy.Focus();
                return;
            }

            if (string.Compare(strYYMM, "199904") < 0)
            {
                ComFunc.MsgBox("1999년 4월분부터 가능합니다.", "확인");
                cboyyyy.Focus();
                return;
            }
            btnView.Enabled = false;
            btnPrint.Enabled = false;

            nMirTot1 = 0;
            nMirTot2 = 0;
            nMirTot3 = 0;

            ssView_Sheet1.RowCount = 50;
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                if (Convert.ToInt32(strJong) <= Convert.ToInt32("3") || strJong == "6")  //'국민공단,직장,보호
                {

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT * FROM " + ComNum.DB_PMPA + "BBASCD";  //BAS_BASCD

                    //'청구예상액 SELECT
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT a.Bi,a.Pano,b.Sname,a.DeptCode,                        ";
                    SQL = SQL + ComNum.VBLF + "        TO_CHAR(a.ActDate,'YYYY-MM-DD') OutDate,               ";
                    SQL = SQL + ComNum.VBLF + "        0 JAmt,MirAmt YAmt                                     ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SUMMARY a," + ComNum.DB_PMPA + "BAS_Patient b  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1=1                             ";
                    SQL = SQL + ComNum.VBLF + "    AND a.YYMM = '" + strYYMM + "'                             ";
                    SQL = SQL + ComNum.VBLF + "    AND a.IpdOpd = 'I'                                         ";
                    SQL = SQL + ComNum.VBLF + "    AND a.MirAmt <> 0                                           ";



                    if (Convert.ToInt32(strYYMM) >= Convert.ToInt32("200007"))
                    {
                        switch (strJong)
                        {
                            case "1":
                                SQL = SQL + ComNum.VBLF + "    AND a.Bi IN ('11','12','13','32','44')  ";
                                break;
                            case "3":
                                SQL = SQL + ComNum.VBLF + "    AND a.Bi IN ('21','22','23','24')       ";
                                break;
                            case "6":
                                SQL = SQL + ComNum.VBLF + "    AND a.Bi NOT IN ('31','52')             ";
                                break;
                        }
                    }
                    else
                    {
                        switch (strJong)
                        {
                            case "1":
                                SQL = SQL + ComNum.VBLF + "    AND a.Bi IN ('11','13','32','44')       ";
                                break;
                            case "2":
                                SQL = SQL + ComNum.VBLF + "    AND a.Bi = '12'                         ";
                                break;
                            case "3":
                                SQL = SQL + ComNum.VBLF + "    AND a.Bi IN ('21','22','23','24')       ";
                                break;
                            case "6":
                                SQL = SQL + ComNum.VBLF + "    AND a.Bi NOT IN ('31','52')             ";
                                break;
                        }
                    }

                    SQL = SQL + ComNum.VBLF + "    AND a.Pano = b.Pano(+)                                     ";
                    SQL = SQL + ComNum.VBLF + "  UNION ALL                                                    ";
                    //'실청구액을 Select
                    SQL = SQL + ComNum.VBLF + " SELECT Bi,Pano,Sname,DeptCode1 DeptCode,          ";
                    SQL = SQL + ComNum.VBLF + "        TO_CHAR(OutDate,'YYYY-MM-DD') OutDate,     ";
                    SQL = SQL + ComNum.VBLF + "        JAmt,0 YAmt                                ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MIR_INSID                                  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1=1                 ";
                    SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strYYMM + "'                   ";
                    SQL = SQL + ComNum.VBLF + "    AND IpdOpd = 'I'                               ";
                    SQL = SQL + ComNum.VBLF + "    AND UpCnt1 <> '9'                              ";// '보류자가 아닌것
                    SQL = SQL + ComNum.VBLF + "    AND JAmt <> 0                                  ";

                    if (Convert.ToInt32(strYYMM) >= Convert.ToInt32("200007"))
                    {
                        switch (strJong)
                        {
                            case "1":
                                SQL = SQL + ComNum.VBLF + "    AND Johap IN ('1','2')      ";
                                break;
                            case "3":
                                SQL = SQL + ComNum.VBLF + "    AND Johap = '5'             ";
                                break;
                            case "6":
                                SQL = SQL + ComNum.VBLF + "    AND Johap IN ('1','2','5')  ";
                                break;
                        }

                    }
                    else
                    {
                        switch (strJong)
                        {
                            case "1":
                                SQL = SQL + ComNum.VBLF + "    AND Johap = '1'             ";
                                break;
                            case "2":
                                SQL = SQL + ComNum.VBLF + "    AND Johap = '2'             ";
                                break;
                            case "3":
                                SQL = SQL + ComNum.VBLF + "    AND Johap = '5'             ";
                                break;
                            case "6":
                                SQL = SQL + ComNum.VBLF + "    AND Johap IN ('1','2','5')  ";
                                break;
                        }
                    }
                }
                else    //  '산재,자보
                {
                    //'청구예상액 SELECT
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT a.Bi,a.Pano,b.Sname,a.DeptCode,            ";
                    SQL = SQL + ComNum.VBLF + "        TO_CHAR(a.OutDate,'YYYY-MM-DD') OutDate,   ";
                    SQL = SQL + ComNum.VBLF + "        0 JAmt,MirAmt YAmt                         ";
                    SQL = SQL + ComNum.VBLF + "   FROM MISU_SUMMARY a,BAS_Patient b               ";
                    SQL = SQL + ComNum.VBLF + "  WHERE a.YYMM = '" + strYYMM + "'                 ";
                    SQL = SQL + ComNum.VBLF + "    AND a.IpdOpd = 'I'                             ";
                    SQL = SQL + ComNum.VBLF + "    AND a.MirAmt <> 0                              ";

                    switch (strJong)
                    {
                        case "4":
                            SQL = SQL + ComNum.VBLF + "    AND a.Bi = '31'                 ";
                            break;
                        case "5":
                            SQL = SQL + ComNum.VBLF + "    AND a.Bi = '52'                 ";
                            break;
                        case "7":
                            SQL = SQL + ComNum.VBLF + "    AND a.Bi IN ('31','52')         ";
                            break;
                    }

                    SQL = SQL + ComNum.VBLF + "    AND a.Pano = b.Pano(+)                         ";
                    SQL = SQL + ComNum.VBLF + "  UNION ALL                                        ";
                    //'실청구액을 Select

                    SQL = SQL + ComNum.VBLF + " SELECT a.Bi,a.MisuID Pano,b.Sname,a.DeptCode,     ";
                    SQL = SQL + ComNum.VBLF + "        TO_CHAR(a.ToDate,'YYYY-MM-DD') OutDate,    ";
                    SQL = SQL + ComNum.VBLF + "        a.Amt2 JAmt,0 YAmt                         ";
                    SQL = SQL + ComNum.VBLF + "   FROM MISU_IDMST a,BAS_Patient b                 ";
                    SQL = SQL + ComNum.VBLF + "  WHERE a.MirYYMM = '" + strYYMM + "'              ";
                    SQL = SQL + ComNum.VBLF + "    AND a.IpdOpd = 'I'                             ";

                    switch (strJong)
                    {
                        case "4":
                            SQL = SQL + ComNum.VBLF + "    AND a.Class = '05'              ";
                            break;
                        case "5":
                            SQL = SQL + ComNum.VBLF + "    AND a.Class = '07'              ";
                            break;
                        case "7":
                            SQL = SQL + ComNum.VBLF + "    AND a.Class IN ('05','07')      ";
                            break;

                    }

                    SQL = SQL + ComNum.VBLF + "    AND a.Amt2 <> 0                                ";
                    SQL = SQL + ComNum.VBLF + "    AND a.MisuID = b.Pano(+)                       ";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY 1,2,3,4,5                                   ";

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

                nMirAmt1 = 0;
                nMirAmt2 = 0;
                nChaek = 0;
                strOldData = "";

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BSNSCLS"].ToString().Trim();

                    strNewData = VB.Left(dt.Rows[i]["Bi"].ToString().Trim() + VB.Space(2), 2);
                    strNewData = strNewData + VB.Left(dt.Rows[i]["Pano"] + VB.Space(8), 8);

                    if (dt.Rows[i]["OutDate"].ToString().Trim() == "")
                    {
                        strNewData = strNewData + VB.Right(strOldData, 10);
                    }
                    else
                    {
                        strNewData = strNewData + VB.Left(dt.Rows[i]["OutDate"].ToString().Trim() + VB.Space(10), 10);
                    }

                    if (strOldData == "")
                    {
                        strOldData = strNewData;
                        strSname = dt.Rows[i]["Sname"].ToString().Trim();
                        strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                    }

                    nMirTot1 = nMirTot1 + (long)(VB.Val(dt.Rows[i]["YAmt"].ToString().Trim()));
                    nMirTot2 = nMirTot2 + (long)(VB.Val(dt.Rows[i]["JAmt"].ToString().Trim()));

                    if (strOldData == strNewData)
                    {
                        nMirAmt1 = nMirAmt1 + (long)(VB.Val(dt.Rows[i]["YAmt"].ToString().Trim()));
                        nMirAmt2 = nMirAmt2 + (long)(VB.Val(dt.Rows[i]["JAmt"].ToString().Trim()));
                    }
                    else
                    {
                        Display_Rtn(ref nMirAmt1, ref nMirAmt2, ref nMirAmt3, ref nChaek, ref nRow, strOldData, strSname, strDeptCode);
                        strOldData = strNewData;
                        nMirAmt1 = (long)(VB.Val(dt.Rows[i]["YAmt"].ToString().Trim()));
                        nMirAmt2 = (long)(VB.Val(dt.Rows[i]["JAmt"].ToString().Trim()));
                        strSname = dt.Rows[i]["Sname"].ToString().Trim();
                        strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                    }

                }
                dt.Dispose();
                dt = null;


                Display_Rtn(ref nMirAmt1, ref nMirAmt2, ref nMirAmt3, ref nChaek, ref nRow, strOldData, strSname, strDeptCode);
                Total_Rtn(ref nRow, nMirTot1, nMirTot2, nMirTot3);

                ssView_Sheet1.RowCount = nRow;
                btnView.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Display_Rtn(ref long nMirAmt1, ref long nMirAmt2, ref long nMirAmt3, ref long nChaek, ref int nRow, string strOldData, string strSname, string strDeptCode) //: '1건의 내용을 Display
        {
            nMirAmt3 = nMirAmt2 - nMirAmt1;
            nChaek = nChaek + nMirAmt3;
            if (rdoJob1.Checked == true && (nMirAmt3 > -10000 && nMirAmt3 < 10000))
            {
                return;
            }
            if (rdoJob2.Checked == true && nMirAmt1 != 0 && nMirAmt2 != 0)
            {
                return;
            }

            nRow = nRow + 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow + 10;
            }
            ssView_Sheet1.Cells[nRow - 1, 0].Text = VB.Left(strOldData, 2);
            ssView_Sheet1.Cells[nRow - 1, 1].Text = VB.Mid(strOldData, 3, 8);
            ssView_Sheet1.Cells[nRow - 1, 2].Text = strSname;
            ssView_Sheet1.Cells[nRow - 1, 3].Text = strDeptCode;
            ssView_Sheet1.Cells[nRow - 1, 4].Text = VB.Right(strOldData, 10);
            ssView_Sheet1.Cells[nRow - 1, 5].Text = nMirAmt1.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[nRow - 1, 6].Text = nMirAmt2.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[nRow - 1, 7].Text = nMirAmt3.ToString("###,###,###,##0 ");
        }

        private void Total_Rtn(ref int nRow, long nMirTot1, long nMirTot2, long nMirTot3)
        {
            nRow = nRow + 1;

            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }
            ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 합계 **";
            ssView_Sheet1.Cells[nRow - 1, 5].Text = nMirTot1.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[nRow - 1, 6].Text = nMirTot2.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[nRow - 1, 7].Text = nMirTot1.ToString("###,###,###,##0 ");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

            if (strPrintName != "")
            {
                strTitle = cboyyyy.Text + "청구예상액과 실청구액 점검";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                strHeader += CS.setSpdPrint_String("출력시간 : " + strDTP, new Font("굴림체", 11), clsSpread.enmSpdHAlign.Left, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
            }
        }
    }
}
