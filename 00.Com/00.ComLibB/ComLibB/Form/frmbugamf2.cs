using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// Class Name      : ComLibB.dll
    /// File Name       : frmbugamf2.cs
    /// Description     : 1년 이상 장기요양환자 현황
    /// Author          : 김효성
    /// Create Date     : 2017-06-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// VB\basic\bugamf\frmBugamf2.frm => frmbugamf2.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\bugamf\frmBugamf2.frm(FrmGamf2)
    /// </seealso>
    /// <vbp>
    /// default : VB\basic\bugamf\bugamf.vbp
    /// </vbp>

    public partial class frmbugamf2 : Form
    {
        public frmbugamf2 ()
        {
            InitializeComponent ();
        }

        private void frmbugamf2_Load (object sender , EventArgs e)
        {
            string strDate = "";

            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);

            ssView_Sheet1.RowCount = 0;
            btnSearch.Enabled = true;
            btnPrint.Enabled = false;
            txtSearch.Text = "";
            strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime (clsDB.DbCon, "D") , "D" , "-");
            dtpStart.Value = Convert.ToDateTime (strDate);
            dtpEnd.Value = Convert.ToDateTime (strDate);
        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }

        private void btnPrint_Click (object sender , EventArgs e)
        {
            if (ComQuery.IsJobAuth(this , "P", clsDB.DbCon) == false) return;//권한 확인

            string strFont1 = "";
            string strFont2="";
            string strHead1="";
            string strHead2="";          

            if (ssView_Sheet1.RowCount < 1) return;

            strFont1 = "/fn\"굴림체\" /fz\"20\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = strHead1 + "/c" + "감액 대상자 등록내역 ";
            strHead2 = "/n/l/f2" + "인쇄일자 : " + clsPublic.GstrSysDate;

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet (0);
        }

        private void btnSearch_Click (object sender , EventArgs e)
        {

            string dd = clsAES.DeAES ("IQFedZnJs7N88Mt6zNFfvZCR+Oc3TGRomEp8n0XPGmk=");


            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string SQL = "";
            string SQL1 = "";
            string strGubun = "";
            string strJumin = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            strGubun = txtSearch.Text.Trim ();
            try
            {
                ssView_Sheet1.RowCount = 0;

                if (optJumin.Checked == true)
                {
                    SQL = "    SELECT GAMJUMIN,GAMSABUN,GAMGUBUN,GAMMESSAGE,GAMNAME,";
                    SQL = SQL + ComNum.VBLF + " GAMSOSOK,GAMCODE,GAMJUMIN3,PANO,ENTSABUN, ";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(GAMENTER,'YYYY-MM-DD') GamEnter, ";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(GAMOUT,'YYYY-MM-DD') GamOut, ";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(GAMEND,'YYYY-MM-DD') GamEnd, ";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') EntDate ";
                    SQL = SQL + ComNum.VBLF + " FROM BAS_GAMF_HISTORY ";
                    SQL = SQL + ComNum.VBLF + " Where ENTDATE >=TO_DATE('" + dtpStart.Value.ToString ("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ENTDATE < TO_DATE('" + dtpEnd.Value.AddDays (1).ToString ("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND GAMJUMIN3 = '" + clsAES.AES (txtSearch.Text).Trim () + "' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY EntDate ";
                }
                if(optin.Checked == true)
                {
                    SQL = " SELECT Jumin1, Jumin3 From Bas_Patient Where Pano ='" + txtSearch.Text.Trim () + "' ";

                    SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                    if (dt.Rows.Count > 0)
                    {
                        strJumin = dt.Rows [0] ["Jumin1"].ToString ().Trim () + clsAES.DeAES (dt.Rows [0] ["Jumin3"].ToString ().Trim ());
                    }
                    dt.Dispose ();
                    dt = null;

                    if (strJumin != "")
                    {
                        SQL = "    SELECT GAMJUMIN,GAMSABUN,GAMGUBUN,GAMMESSAGE,GAMNAME,";
                        SQL = SQL + ComNum.VBLF + " GAMSOSOK,GAMCODE,GAMJUMIN3,PANO,ENTSABUN, ";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(GAMENTER,'YYYY-MM-DD') GamEnter, ";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(GAMOUT,'YYYY-MM-DD') GamOut, ";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(GAMEND,'YYYY-MM-DD') GamEnd, ";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') EntDate ";
                        SQL = SQL + ComNum.VBLF + " FROM BAS_GAMF_HISTORY ";
                        SQL = SQL + ComNum.VBLF + " Where ENTDATE >=TO_DATE('" + dtpStart.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND ENTDATE < TO_DATE('" + dtpEnd.Value.AddDays (1).ToString ("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND GAMJUMIN3 = '" + clsAES.AES (strJumin).Trim () + "' ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY EntDate ";
                    }
                }

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
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight (-1 , ComNum.SPDROWHT);

                for (i=0; i<dt.Rows.Count; i++)
                {
                    strJumin = clsAES.DeAES (dt.Rows [i] ["GAMJUMIN3"].ToString ().Trim ());
                    strJumin1 = VB.Left (strJumin , 6);
                    strJumin2 = VB.Right (strJumin , 7);

                    ssView_Sheet1.Cells[i , 0].Text = strJumin1 + "-" + strJumin2.ToString().Trim();
                    ssView_Sheet1.Cells [i , 0].Text = clsVbfunc.GetBCODENameCode (clsDB.DbCon, "2", "BAS_감액코드명", dt.Rows[i]["GAMGUBUN"].ToString().Trim());
                    ssView_Sheet1.Cells[i , 2].Text = dt.Rows [i]["Gamname"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["GAMMESSAGE"].ToString ().Trim ();

                    SQL1 = " SELECT PANO FROM BAS_PATIENT ";
                    SQL1 = SQL1 + ComNum.VBLF + " WHERE JUMIN1 = '" + strJumin1 + "' ";
                    SQL1 = SQL1 + ComNum.VBLF + "   AND JUMIN3 = '" + clsAES.AES(strJumin2) + "' ";

                    SqlErr = clsDB.GetDataTable (ref dt1 , SQL, clsDB.DbCon);

                    ssView_Sheet1.Cells [i , 4].Text = dt.Rows [0] ["PANO"].ToString ().Trim ();

                    dt1.Dispose ();
                    dt1 = null;

                    ssView_Sheet1.Cells [i , 5].Text = dt.Rows [i] ["EntDate"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 6].Text = dt.Rows [i] ["ENTSABUN"].ToString ().Trim ();
                }

                btnPrint.Enabled = true;

                dt.Dispose ();
                dt = null;
                ssView_Sheet1.SetRowHeight (i , Convert.ToInt32 (ssView_Sheet1.GetPreferredRowHeight (i)) + 13);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
        }
        
        private void txtSearch_EnabledChanged (object sender , EventArgs e)
        {
            txtSearch.ImeMode = ImeMode;
            btnSearch.Enabled = true;
        }

        private void txtSearch_KeyDown (object sender , KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                SendKeys.Send ("{Tab}");
            }
        }

        private void txtSearch_Leave (object sender , EventArgs e)
        {
            if(optJumin.Checked == true)
            {
                txtSearch.Text = txtSearch.Text.Replace("-","");
            }
            else if(optin.Checked == true)
            {
                txtSearch.Text = ComFunc.LPAD (txtSearch.Text , 8 , "0");
            }
        }
    }
}
