using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// Class Name      : ComLibB.dll
    /// File Name       : frmyoyang02.cs
    /// Description     : 1년 이상 장기요양환자 현황
    /// Author          : 김효성
    /// Create Date     : 2017-06-19
    /// Update History  : 함수 GoTO  #region //Sungindisplay
    /// Update History  : 함수 GoTO  #region //Illcodedisplay
    /// </summary>
    /// <history>  
    /// VB\basic\busanid\busanid7.frm => frmyoyang02.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\busanid\busanid7.frm(frmyoyang02)
    /// </seealso>
    /// <vbp>
    /// default : VB\basic\busanid\busanid.vbp
    /// </vbp>
    public partial class frmyoyang02 : Form
    {
        public frmyoyang02 ()
        {
            InitializeComponent ();
        }

        private void frmyoyang02_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회 EX
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등
        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }

        private void btnPrint_Click (object sender , EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string PrintDate = "";
            string GstrSysDate = "";

            PrintDate = GstrSysDate;

            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"바탕체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/c/f1" + "1년 이상 장기요양환자 현황" + "/n"  + "/n";
            strHead2 = "/l/f2" + "작업일자 : " + PrintDate + VB.Space (13) + "                            <의료기관명: 포 항 성 모 병 원>                " + "                           PAGE : " + " /P";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet (0);

        }

        private void GetSearch ()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            int m = 0;
            int strMaxRow = 0;
            int ArgYY = 0;
            int ArgMM = 0;
            int ArgDD = 0;
            int nCount = 0;
            int nGiGan = 0;
            int nGiGan2 = 0;
            string SqlErr = ""; //에러문 받는 변수
            string strSDate = "";
            string strPano = "";
            string strindate = "";
            string strChk = "";
            string strlastday = "";
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;

            string GstrSysDate = Convert.ToDateTime (ComFunc.FormatStrToDateEx (ComQuery.CurrentDateTime (clsDB.DbCon, "D") , "D" , "-")).ToString ("yyyy-MM-dd");
            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return; //권한 확인

            try
            {
                //strSDate = clsPublic.GstrSysDate; //EX
                strSDate = GstrSysDate;

                ArgYY = Convert.ToInt32 (VB.Val (VB.Left (strSDate , 4)));
                ArgMM = Convert.ToInt32 (VB.Val (VB.Mid (strSDate , 6 , 2)));
                ArgDD = Convert.ToInt32 (VB.Val (VB.Right (strSDate , 2)));

                for (i = 0; i < 365; i++)
                {
                    ArgDD = ArgDD - 1;
                    if (ArgDD == 0)
                    {
                        ArgMM = ArgMM - 1;
                        if (ArgMM == 0)
                        {
                            ArgYY = ArgYY - 1;
                            ArgMM = 12;
                        }
                        strlastday = clsVbfunc.LastDay (ArgYY , ArgMM);
                        ArgDD = Convert.ToInt32 (VB.Val (ComFunc.RightH (strlastday , 2)));
                    }// if end
                }//for end

                SQL = "   SELECT PANO,COPRNAME,COPRNO,SNAME,JUMIN1,JUMIN2,TO_CHAR(DATE1,'YYYY-MM-DD') DATE1,TO_CHAR(DATE2,'YYYY-MM-DD') DATE2,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DATE3,'YYYY-MM-DD') DATE3";
                SQL = SQL + ComNum.VBLF + " FROM BAS_SANID";
                SQL = SQL + ComNum.VBLF + " WHERE BI ='31'";
                SQL = SQL + ComNum.VBLF + " AND (DATE3 IS NULL OR DATE3 > TO_DATE('" + strSDate + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + " AND DATE1 < TO_DATE('" + strlastday + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " AND DATE1 IS NOT NULL";
                SQL = SQL + ComNum.VBLF + " AND DATE2 IS NOT NULL";
                SQL = SQL + ComNum.VBLF + " ORDER BY COPRNAME";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count * 10;

                nCount = 1;
                k = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells [k , 0].Text = nCount.ToString ().Trim ();
                    ssView_Sheet1.Cells [k , 1].Text = dt.Rows [i] ["SNAME"].ToString ().Trim ();
                    ssView_Sheet1.Cells [k , 2].Text = dt.Rows [i] ["JUMIN1"].ToString ().Trim () + "-" + dt.Rows [i] ["JUMIN2"].ToString ().Trim ();
                    ssView_Sheet1.Cells [k , 3].Text = VB.Mid (dt.Rows [i] ["COPRNAME"].ToString ().Trim () , 5 , dt.Rows [i] ["COPRNAME"].ToString ().Trim ().Length);
                    ssView_Sheet1.Cells [k , 4].Text = dt.Rows [i] ["DATE1"].ToString ().Trim ();

                    strPano = dt.Rows [i] ["PANO"].ToString ().Trim ();

                    strChk = "Y";

                    #region //Sungindisplay
                    SQL = "   SELECT IPDFRDATE,OPDFRDATE,IPDTODATE, OPDTODATE FROM BAS_SANDTL";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO= '" + strPano + "'";
                    SQL = SQL + ComNum.VBLF + " ORDER BY DATEAPPROVAL DESC";

                    SqlErr = clsDB.GetDataTable (ref dt1 , SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count == 0)
                    {
                        strChk = "N";
                        dt1.Dispose ();
                        dt1 = null;
                        continue;
                    }

                    if (VB.Val (dt1.Rows [0] ["IPDTODATE"].ToString ().Trim ()) != 0)
                    {
                        nGiGan = Convert.ToInt32 (VB.DateDiff ("d" , Convert.ToDateTime (ComFunc.FormatStrToDate (dt1.Rows [0] ["IPDTODATE"].ToString ().Trim () , "D"))
                            , Convert.ToDateTime (strSDate)));

                        if (VB.Val (dt1.Rows [0] ["OPDTODATE"].ToString ().Trim ()) != 0)
                        {
                            nGiGan2 = Convert.ToInt32 (VB.DateDiff ("d" , Convert.ToDateTime (ComFunc.FormatStrToDate (dt1.Rows [0] ["OPDTODATE"].ToString ().Trim () , "D"))
                            , Convert.ToDateTime (strSDate)));

                            if (nGiGan - nGiGan2 > 0)
                            {
                                if (nGiGan > 0) ssView_Sheet1.Cells [k , 5].Text = "입원";
                                else strChk = "N";

                            }
                            else
                            {
                                if (nGiGan2 > 0) ssView_Sheet1.Cells [k , 5].Text = "외래";
                            }
                        }
                        else
                        {
                            if (nGiGan2 > 0) ssView_Sheet1.Cells [k , 5].Text = "외래";
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32 (VB.Val (dt1.Rows [0] ["OPDTODATE"].ToString ().Trim ())) != 0)
                        {
                            nGiGan2 = Convert.ToInt32 (VB.DateDiff ("d" , Convert.ToDateTime (ComFunc.FormatStrToDate (dt1.Rows [0] ["OPDTODATE"].ToString ().Trim () , "D"))
                        , Convert.ToDateTime (strSDate)));

                            if (nGiGan2 > 0) ssView_Sheet1.Cells [k , 5].Text = "외래";
                            else strChk = "N";

                        }
                        else strChk = "N";
                    }
                    #endregion

                    if (strChk != "N")
                    {

                        #region //Illcodedisplay

                        SQL = "  SELECT INDATE FROM MIR_ILLS  ";
                        SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + " AND Bi = '31'  ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY INDATE DESC ";

                        SqlErr = clsDB.GetDataTable (ref dt2 , SQL, clsDB.DbCon);
                        //strindate = dt2.Rows [0] ["INDATE"].ToString ().Trim ();

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            strindate = dt2.Rows [0] ["INDATE"].ToString ().Trim ();
                        }

                        dt2.Dispose ();
                        dt2 = null;

                        SQL = "  SELECT IllName FROM MIR_ILLS  ";
                        SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + " AND Bi = '31' ";
                        SQL = SQL + ComNum.VBLF + " AND InDate = '" + strindate + "'";
                        SQL = SQL + ComNum.VBLF + " ORDER BY Rank,IllCode ";

                        SqlErr = clsDB.GetDataTable (ref dt2 , SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        
                        strMaxRow = strMaxRow + dt2.Rows.Count;

                        l = 0;

                        for (j = 0; j < dt2.Rows.Count; j++)
                        {
                            ssView_Sheet1.Cells [k + l , 6].Text = dt2.Rows [j] ["ILLName"].ToString ().Trim ();
                            l = l + 1;
                        }

                        dt2.Dispose ();
                        dt2 = null;

                        #endregion

                        if (l > m)
                        {
                            k = k + l;
                            strMaxRow = strMaxRow + l;
                        }
                        else
                        {
                            k = k + m;
                            strMaxRow = strMaxRow + m;
                        }
                        k = k + 1;
                    }
                    else
                    {
                        nCount = nCount - 1;
                    }
                    nCount = nCount + 1;
                }

                dt.Dispose ();
                dt = null;
                ssView_Sheet1.RowCount = strMaxRow;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void btnSearch_Click (object sender , EventArgs e)
        {
            GetSearch ();
        }
    }
}
