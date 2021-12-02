using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmyoyang01
    /// File Name : frmyoyang01.cs
    /// Title or Description : 요양환자 명부
    /// Author : 박창욱
    /// Create Date : 2017-06-02
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    ///     06-15 박창욱 : VB.DateDiff 공통함수 적용. 임시 구현된 DATE_ILSU함수 삭제.
    /// </summary>
    /// <history>  
    /// VB\busanid6.frm(Frmyoyang01) -> frmyoyang01.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\busanid\busanid6.frm(Frmyoyang01)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\busanid\\busanid.vbp
    /// </vbp>
    public partial class frmyoyang01 : Form
    {
        private string gstrJobMan = "";

        public frmyoyang01()
        {
            InitializeComponent();
        }

        public frmyoyang01(string strJobMan)
        {
            InitializeComponent();
            gstrJobMan = strJobMan;
        }

        private void frmyoyang01_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            String strFont1 = "";
            String strFont2 = "";
            String strHead1 = "";
            String strHead2 = "";
            String printDate = "";
            String jobMan = "";

            printDate = clsPublic.GstrSysDate;
            jobMan = gstrJobMan;

            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"바탕체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/f1" + "                                           요  양  환  자  명  부" + "/n";


            strHead2 = "/l/f2" + "작업일자 : " + printDate + VB.Space(13) + "                               <의료기관명: 포 항 성 모 병 원>                " + "                           PAGE : " + " /P"; ;

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 5;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            int j = 0;
            int nRow = 0;
            int nCol = 0;
            int nIcount = 0;
            int nOcount = 0;
            int intMaxRow = 0;
            int nGiGan = 0;
            int nGiGan2 = 0;
            string strSDate = "";
            string strDate = "";
            string strPano = "";
            string strindate = "";
            string strChk = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;
            DataTable dt2 = null;

            ssView_Sheet1.Cells[2, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            strSDate = clsPublic.GstrSysDate;

            try
            {
                SQL = "";
                SQL = "    SELECT PANO,COPRNAME,COPRNO,SNAME,JUMIN1,JUMIN2,TO_CHAR(DATE1,'YYYY-MM-DD') DATE1,TO_CHAR(DATE2,'YYYY-MM-DD') DATE2";
                SQL = SQL + ComNum.VBLF + " FROM BAS_SANID";
                SQL = SQL + ComNum.VBLF + " WHERE BI ='31'";
                SQL = SQL + ComNum.VBLF + " AND (DATE3 IS NULL OR DATE3 > TO_DATE('" + strSDate + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + " AND DATE1 IS NOT NULL";
                SQL = SQL + ComNum.VBLF + " AND DATE2 IS NOT NULL";
                SQL = SQL + ComNum.VBLF + " ORDER BY COPRNAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                intMaxRow = dt.Rows.Count + 3;
                ssView_Sheet1.RowCount = dt.Rows.Count * 10;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                nRow = 2;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strChk = "Y";
                    strPano = dt.Rows[i]["PANO"].ToString().Trim();


                    #region //Sungin_display
                    Sungin_display(strPano, strSDate, ref intMaxRow,
                            ref strChk, ref nGiGan, ref nGiGan2, ref strDate,
                            ref nCol, ref nIcount, ref nOcount);
                    #endregion //Sungin_display


                    if (strChk != "N")
                    {
                        ssView_Sheet1.Cells[nRow, nCol].Text = strDate;
                        ssView_Sheet1.Cells[nRow, 0].Text = ComFunc.MidH(dt.Rows[i]["COPRNAME"].ToString().Trim(), 5, (int)(ComFunc.LenH(dt.Rows[i]["COPRNAME"].ToString()) - 4));
                        ssView_Sheet1.Cells[nRow, 1].Text = dt.Rows[i]["COPRNO"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow, 3].Text = dt.Rows[i]["JUMIN1"].ToString().Trim() + "-" + dt.Rows[i]["JUMIN2"].ToString().Trim(); ;
                        ssView_Sheet1.Cells[nRow, 4].Text = dt.Rows[i]["DATE1"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow, 5].Text = dt.Rows[i]["DATE2"].ToString().Trim();

                        //-----상병명-------------------
                        //dt3, SQL3---------------------
                        #region Illcode_display
                        SQL = "";
                        SQL = "  SELECT to_char(INDATE) INDATE FROM MIR_ILLS  ";
                        SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + " AND Bi = '31'  ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY INDATE DESC ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt2.Rows.Count == 0)
                        {
                            dt2.Dispose();
                            dt2 = null;
                            //return;
                        }
                        else
                        {
                            strindate = dt2.Rows[0]["INDATE"].ToString().Trim();

                            dt2.Dispose();
                            dt2 = null;
                        }

                        SQL = "";
                        SQL = " SELECT IllName FROM MIR_ILLS  ";
                        SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + " AND Bi = '31' ";
                        SQL = SQL + ComNum.VBLF + " AND InDate = to_date('" + strindate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + " AND ROWNUM < '5'";
                        SQL = SQL + ComNum.VBLF + " ORDER BY Rank,IllCode ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt2.Rows.Count == 0)
                        {
                            dt2.Dispose();
                            dt2 = null;
                        }
                        else
                        {
                            intMaxRow = intMaxRow + dt2.Rows.Count - 1;

                            for (j = 0; j < dt2.Rows.Count; j++)
                            {
                                //ssView_Sheet1.Rows.Count = nRow + j;
                                ssView_Sheet1.Cells[nRow + j, 8].Text = dt2.Rows[j]["ILLName"].ToString().Trim();
                            }
                            nRow = nRow + dt2.Rows.Count;

                            dt2.Dispose();
                            dt2 = null;
                        }
                        #endregion ////------------------------------


                        nRow = nRow + 1;
                    }
                }
                ssView_Sheet1.RowCount = nRow + 3;
                ssView_Sheet1.Cells[nRow + 1, 6].Text = "입원:" + nIcount + "명";
                ssView_Sheet1.Cells[nRow + 1, 7].Text = "외래:" + nOcount + "명";

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string Sungin_display(string strPano, string strSDate, ref int intMaxRow,
                            ref string strChk, ref int nGiGan, ref int nGiGan2, ref string strDate,
                            ref int nCol, ref int nIcount, ref int nOcount)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL = "   SELECT  DATEAPPROVAL,IPDTODATE, OPDTODATE FROM BAS_SANDTL";
            SQL = SQL + ComNum.VBLF + " WHERE PANO= '" + strPano + "'";
            SQL = SQL + ComNum.VBLF + " ORDER BY DATEAPPROVAL DESC";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return strChk;
            }
            if (dt.Rows.Count == 0)
            {
                strChk = "N";
                dt.Dispose();
                dt = null;
                return strChk;
            }

            intMaxRow = intMaxRow + dt.Rows.Count;

            if (VB.Val(dt.Rows[0]["IPDTODATE"].ToString().Trim()) != 0)
            {
                nGiGan = (int)VB.DateDiff("d", (VB.Left(dt.Rows[0]["IPDTODATE"].ToString().Trim(), 4) + "-" +
                                    VB.Mid(dt.Rows[0]["IPDTODATE"].ToString().Trim(), 5, 2) + "-" +
                                    VB.Right(dt.Rows[0]["IPDTODATE"].ToString().Trim(), 2)), strSDate);

                if (VB.Val(dt.Rows[0]["OPDTODATE"].ToString().Trim()) != 0)
                {
                   nGiGan2 = (int)VB.DateDiff("d", VB.Left(dt.Rows[0]["OPDTODATE"].ToString().Trim(), 4) + "-" +
                                    VB.Mid(dt.Rows[0]["OPDTODATE"].ToString().Trim(), 5, 2) + "-" +
                                    VB.Right(dt.Rows[0]["OPDTODATE"].ToString().Trim(), 2), strSDate);

                    if (nGiGan - nGiGan2 > 0)
                    {
                        if (nGiGan > 0)
                        {
                            nCol = 6;
                            strDate = dt.Rows[0]["IPDTODATE"].ToString().Trim();
                            nIcount = nIcount + 1;
                        }
                        else
                        {
                            strChk = "N";
                            return strChk;
                        }
                    }
                    else
                    {
                        if (nGiGan2 > 0)
                        {
                            nCol = 7;
                            strDate = dt.Rows[0]["OPDTODATE"].ToString().Trim();
                            nOcount = nOcount + 1;
                        }
                    }
                }
                else
                {
                    if (nGiGan > 0)
                    {
                        nCol = 6;
                        strDate = dt.Rows[0]["IPDTODATE"].ToString().Trim();
                        nIcount = nIcount + 1;
                    }
                    else
                    {
                        strChk = "N";
                    }
                }
            }
            else
            {
                if (VB.Val(dt.Rows[0]["OPDTODATE"].ToString().Trim()) != 0)
                {
                    nGiGan2 = (int)VB.DateDiff("d", VB.Left(dt.Rows[0]["OPDTODATE"].ToString().Trim(), 4) + "-" +
                                    VB.Mid(dt.Rows[0]["OPDTODATE"].ToString().Trim(), 5, 2) + "-" +
                                    VB.Right(dt.Rows[0]["OPDTODATE"].ToString().Trim(), 2), strSDate);
                    
                    if (nGiGan2 > 0)
                    {
                        nCol = 7;
                        strDate = dt.Rows[0]["OPDTODATE"].ToString().Trim();
                        nOcount = nOcount + 1;
                    }
                    else
                    {
                        strChk = "N";
                    }
                }
                else
                {
                    strChk = "N";
                }
            }
            dt.Dispose();
            dt = null;

            return strChk;
        }
    }
}
