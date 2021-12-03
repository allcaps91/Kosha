using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using ComBase;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\misu\misuta.vbp\FrmIpgumPrint (FrmIpgumPrint.frm)>> frmPmPaVIEWIpgumPrint.cs 폼이름 재정의" />
    public partial class frmPmPaVIEWIpgumPrint : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread ();
        ComFunc CF = new ComFunc ();
        clsPmpaMisu CPM = new clsPmpaMisu ();
        clsPmpaFunc CPF = new clsPmpaFunc ();

        string strDTP = ComFunc.FormatStrToDate (ComQuery.CurrentDateTime (clsDB.DbCon, "D") , "D");

        string [] GstrGels = new string [51];

        public frmPmPaVIEWIpgumPrint ()
        {
            InitializeComponent ();
        }

        private void frmPmPaVIEWIpgumPrint_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            cboFDate.Text = VB.Left (strDTP , 8) + "01";
            cboTDate.Text = strDTP;


            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT MiaCode GelCode, MiaName                   ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA                                    ";
                SQL = SQL + ComNum.VBLF + "  WHERE (MiaCode LIKE 'JK%' OR MiaCode LIKE 'KB%') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY MiaCode                                 ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                cboGel.Items.Clear ();
                cboGel.Items.Add ("****.전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboGel.Items.Add (dt.Rows [i] ["gelcode"].ToString ().Trim () + "." + dt.Rows [i] ["mianame"].ToString ().Trim ());
                    GstrGels [i] = dt.Rows [i] ["GelCode"].ToString ().Trim ();
                }
                dt.Dispose ();
                dt = null;

                cboGel.SelectedIndex = 0;

                ssView_Sheet1.RowCount = 0;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnView_Click (object sender , EventArgs e)
        {
            if (ComQuery.IsJobAuth (this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            int nRow = 0;
            int nCount = 0;
            int nWRTNO = 0;
            double nJanAmt = 0;
            double nIpgumAmt = 0;
            double nTotMirAmt = 0;
            double nTotSakAmt = 0;
            double nTotIpgumAmt = 0;
            double nTotChungAmt = 0;
            double nAllTotMirAmt = 0;
            double nAllTotSakAmt = 0;
            double nAllTotIpgumAmt = 0;
            double nAllTotChungAmt = 0;
            double nTotMrAmt = 0;
            string strGelCode = "";
            string strGelCode_NEW = "";
            DataTable dt = null;
            DataTable dtFn = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            if (string.Compare (cboTDate.Text , cboFDate.Text) < 0)
            {
                ComFunc.MsgBox ("종료일자가 시작일자 보다 작음" , "확인");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //' 해당기간의 입금내역을 Read
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT GELCODE SGELCODE , WRTNO,TO_CHAR(BDate,'YY-MM-DD') vBDate,                         ";
                SQL = SQL + ComNum.VBLF + "        MisuID vMisuID,SUM(Amt) vIAmt                                                      ";
                SQL = SQL + ComNum.VBLF + "   FROM MISU_SLIP                                                                          ";
                SQL = SQL + ComNum.VBLF + "  WHERE Bdate >= TO_DATE('" + cboFDate.Text + "','YYYY-MM-DD')                                   ";
                SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + cboTDate.Text + "','YYYY-MM-DD')                                   ";
                SQL = SQL + ComNum.VBLF + "    AND Class  = '07'                                                                      ";
                SQL = SQL + ComNum.VBLF + "    AND Gubun  >= '21'                                                                     ";
                SQL = SQL + ComNum.VBLF + "    AND Gubun  <= '29'                                                                     ";

                if (VB.Left (cboGel.Text , 4) != "****")
                {
                    SQL = SQL + ComNum.VBLF + "  AND GelCode = '" + VB.Left (cboGel.Text , 4) + "'  ";
                }
                SQL = SQL + ComNum.VBLF + "  GROUP BY GELCODE,WRTNO,BDate,MisuID                                                      ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY GELCODE,BDate,MisuID                                                            ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("입금된 내역이 1건도 없습니다." , "확인");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nRow = 0;
                nTotMrAmt = 0;
                nTotSakAmt = 0;
                nTotIpgumAmt = 0;
                nTotChungAmt = 0;

                if (dt.Rows.Count > 19)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count + 1;
                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight (-1 , ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nWRTNO = (int)VB.Val (dt.Rows [i] ["WRTNO"].ToString ().Trim ());
                    strGelCode_NEW = dt.Rows [i] ["SGELCODE"].ToString ().Trim ();

                    if (strGelCode != strGelCode_NEW)
                    {
                        if (i != 0)
                        {
                            nRow = nRow + 1;

                            if (nRow > ssView_Sheet1.RowCount)
                            {
                                ssView_Sheet1.RowCount = nRow + 1;
                            }

                            ssView_Sheet1.Cells [nRow - 1 , 4].Text = CPM.READ_BAS_MIA (strGelCode);
                            ssView_Sheet1.Cells [nRow - 1 , 5].Text = nTotChungAmt.ToString ("###,###,###,##0");
                            ssView_Sheet1.Cells [nRow - 1 , 6].Text = nTotIpgumAmt.ToString ("###,###,###,##0");
                            ssView_Sheet1.Cells [nRow - 1 , 7].Text = nTotSakAmt.ToString ("###,###,###,##0");

                            if (nTotMirAmt != 0 && nTotSakAmt != 0)
                            {
                                ssView_Sheet1.Cells [nRow - 1 , 8].Text = (nTotSakAmt / nTotChungAmt * 100).ToString ("##0.00") + "%";
                            }

                            nCount = 0;
                            nTotMirAmt = 0;
                            nTotSakAmt = 0;
                            nTotIpgumAmt = 0;
                            nTotChungAmt = 0;
                        }
                        strGelCode = strGelCode_NEW;
                    }

                    nRow = nRow + 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow + 1;
                    }

                    nCount = nCount + 1;
                    //미수 Master를 READ

                    CPM.READ_MISU_IDMST (nWRTNO);
                    nJanAmt = clsPmpaType.TMM.Amt [2];
                    //청구금액
                    nIpgumAmt = VB.Val (dt.Rows [i] ["vIAmt"].ToString ().Trim ());

                    // 전월의 미수잔액을 Read

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT SUM(Amt) IpgumAmt                       ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_SLIP                       ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "    AND WRTNO = '" + dt.Rows [i] ["WRTNO"].ToString ().Trim () + "'  ";
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + cboTDate.Text + "','YYYY-MM-DD')   ";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun >= '21'                                          ";

                    SqlErr = clsDB.GetDataTable (ref dtFn , SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dtFn.Rows.Count == 1)
                    {
                        nJanAmt = nJanAmt - VB.Val (dtFn.Rows [0] ["IpgumAmt"].ToString ().Trim ());
                    }

                    ssView_Sheet1.Cells [nRow - 1 , 0].Text = nCount.ToString ();
                    ssView_Sheet1.Cells [nRow - 1 , 1].Text = dt.Rows [i] ["vBdate"].ToString ().Trim ();
                    ssView_Sheet1.Cells [nRow - 1 , 2].Text = dt.Rows [i] ["vMisuID"].ToString ().Trim ();
                    ssView_Sheet1.Cells [nRow - 1 , 3].Text = CPF.Get_BasPatient (clsDB.DbCon, clsPmpaType.TMM.MisuID).Rows [0] ["SNAME"].ToString ().Trim ();
                    ssView_Sheet1.Cells [nRow - 1 , 4].Text = VB.Right (clsPmpaType.TMM.FromDate , 10) + "~" + VB.Right (clsPmpaType.TMM.ToDate , 10);
                    ssView_Sheet1.Cells [nRow - 1 , 5].Text = clsPmpaType.TMM.Amt [2].ToString ("###,###,###,##0");
                    ssView_Sheet1.Cells [nRow - 1 , 6].Text = nIpgumAmt.ToString ("###,###,###,##0");
                    Application.DoEvents ();
                    if (nJanAmt < 1)
                    {
                        ssView_Sheet1.Cells [nRow - 1 , 7].Text = clsPmpaType.TMM.Amt [4].ToString ("###,###,###,##0"); //'삭감액

                        if (clsPmpaType.TMM.Amt [2] != 0 && clsPmpaType.TMM.Amt [4] != 0)
                        {
                            ssView_Sheet1.Cells [nRow - 1 , 8].Text = (clsPmpaType.TMM.Amt [4] / clsPmpaType.TMM.Amt [2] * 100).ToString ("##0.00") + "%";
                            nTotMirAmt = nTotMirAmt + clsPmpaType.TMM.Amt [2];
                            nTotSakAmt = nTotSakAmt + clsPmpaType.TMM.Amt [4];
                            nAllTotMirAmt = nAllTotMirAmt + clsPmpaType.TMM.Amt [2];
                            nAllTotSakAmt = nAllTotSakAmt + clsPmpaType.TMM.Amt [4];
                        }
                        else
                        {
                            ssView_Sheet1.Cells [nRow - 1 , 8].Text = "0.00%";
                        }
                    }
                    dtFn.Dispose ();
                    dtFn = null;

                    nTotIpgumAmt = nTotIpgumAmt + nIpgumAmt;
                    nAllTotIpgumAmt = nAllTotIpgumAmt + nIpgumAmt;
                    nTotChungAmt = nTotChungAmt + clsPmpaType.TMM.Amt [2];
                    nAllTotChungAmt = nAllTotChungAmt + clsPmpaType.TMM.Amt [2];
                    Application.DoEvents ();
                }


                nRow = nRow + 1;
                if (nRow > ssView_Sheet1.RowCount)
                {
                    ssView_Sheet1.RowCount = nRow + 1;
                }
                ssView_Sheet1.Cells [nRow - 1 , 4].Text = CPM.READ_BAS_MIA (dt.Rows [dt.Rows.Count - 1] ["SGELCODE"].ToString ().Trim ());
                ssView_Sheet1.Cells [nRow - 1 , 5].Text = nTotChungAmt.ToString ("###,###,###,##0");
                ssView_Sheet1.Cells [nRow - 1 , 6].Text = nTotIpgumAmt.ToString ("###,###,###,##0");
                ssView_Sheet1.Cells [nRow - 1 , 7].Text = nTotSakAmt.ToString ("###,###,###,##0");

                if (nTotMirAmt != 0 && nTotSakAmt != 0)
                {
                    ssView_Sheet1.Cells [nRow - 1 , 8].Text = (nTotSakAmt / nTotChungAmt * 100).ToString ("##0.00") + "%";
                }


                nRow = nRow + 1;
                if (nRow > ssView_Sheet1.RowCount)
                {
                    ssView_Sheet1.RowCount = nRow + 1;
                }
                ssView_Sheet1.Cells [nRow - 1 , 4].Text = " ** 전체합계 **";
                ssView_Sheet1.Cells [nRow - 1 , 5].Text = nAllTotChungAmt.ToString ("###,###,###,##0");
                ssView_Sheet1.Cells [nRow - 1 , 6].Text = nAllTotIpgumAmt.ToString ("###,###,###,##0");
                ssView_Sheet1.Cells [nRow - 1 , 7].Text = nAllTotSakAmt.ToString ("###,###,###,##0");

                if (nAllTotMirAmt != 0 && nAllTotSakAmt != 0)
                {
                    ssView_Sheet1.Cells [nRow - 1 , 8].Text = (nAllTotSakAmt / nAllTotChungAmt * 100).ToString ("##0.00") + "%";
                }

                if (nRow < 18)
                {
                    nRow = nRow + 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells [nRow - 1 , 5].Text = "< 이   하  ";
                    ssView_Sheet1.Cells [nRow - 1 , 6].Text = "공   란 >  ";
                }

                dt.Dispose ();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }

        private void btnPrint_Click (object sender , EventArgs e)
        {
            if (ComQuery.IsJobAuth (this , "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth (this , "P", clsDB.DbCon) == false) { return; }     //권한확인

            strTitle = "교 통 환 자  진 료 비  입 금 표";

            strHeader = CS.setSpdPrint_String (strTitle , new Font ("굴림체" , 20 , FontStyle.Bold) , clsSpread.enmSpdHAlign.Center , false , true);
            strHeader += CS.setSpdPrint_String ("입금기간 : " + cboFDate.Text + "=>" + cboTDate.Text , new Font ("굴림체" , 10) , clsSpread.enmSpdHAlign.Left , false , true);
            strHeader += CS.setSpdPrint_String ("거 래 처 : " + cboGel.Text , new Font ("굴림체" , 10) , clsSpread.enmSpdHAlign.Left , false , true);

            strFooter = CS.setSpdPrint_String (null , null , clsSpread.enmSpdHAlign.Center , true , true);
            strFooter += CS.setSpdPrint_String ("입금확인: 재무회계팀 (인)" + clsType.User.JobName , new Font ("굴림체" , 9) , clsSpread.enmSpdHAlign.Right , false , true);

            setMargin = new clsSpread.SpdPrint_Margin (10 , 10 , 50 , 10 , 50 , 10);
            setOption = new clsSpread.SpdPrint_Option (PrintOrientation.Portrait , PrintType.All , 0 , 0 , true , false , true , true , false , false , true);

            CS.setSpdPrint (ssView , PrePrint , setMargin , setOption , strHeader , strFooter);
        }
    }
}
