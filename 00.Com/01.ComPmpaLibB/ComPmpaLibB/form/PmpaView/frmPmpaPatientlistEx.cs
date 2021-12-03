using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using ComBase; //기본 클래스
namespace ComPmpaLibB
{
    public partial class frmPmpaPatientlistEx : Form

      
    {
        clsSpread CS = null;
        ComQuery CQ = null;

        public frmPmpaPatientlistEx()
        {
            InitializeComponent();
            setParam();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

      
        private void setParam()
        {
            this.ssView.Change += new ChangeEventHandler(Spread_Change);
            this.ssView.KeyDown += new KeyEventHandler(Spread_KeyDown);
        }

        private void Spread_Change(object sender, ChangeEventArgs e)
        {

            string strCode = string.Empty;
            string strPano = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            
            try
            {
                if (e.Column != 0)
                {
                    return;
                }

                
                strCode = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim();
                strPano = string.Format("{0:D8}", Convert.ToInt32(strCode));
                ssView_Sheet1.Cells[e.Row, e.Column].Text = strPano;
                if (strPano == "")
                {
                    return;
                }

                SQL = "";
                SQL += ComNum.VBLF + " SELECT SName, Sex, Jumin1, ";
                SQL += ComNum.VBLF + "        Jumin2, Jumin3, ZipCode1, ";
                SQL += ComNum.VBLF + "        ZipCode2, Juso, Tel, ";
                SQL += ComNum.VBLF + "        TO_CHAR(StartDate,'YYYY-MM-DD') StartDate, ";
                SQL += ComNum.VBLF + "        TO_CHAR(LastDate,'YYYY-MM-DD')  LastDate, ";
                SQL += ComNum.VBLF + "        TO_CHAR(Birth,'YYYY-MM-DD')  Birth, ";
                SQL += ComNum.VBLF + "        GbBirth, DeptCode, HPhone, ";
                SQL += ComNum.VBLF + "        EMail, Jikup, GbJuger, ";
                SQL += ComNum.VBLF + "        Religion, GbInfor, GbSMS, ";
                SQL += ComNum.VBLF + "        Gb_VIP, Gb_VIP_Remark, ZipCode3, ";
                SQL += ComNum.VBLF + "        BuildNo, RoadDetail ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND Pano = '" + strPano + "' ";

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
                string strJumin = dt.Rows[0]["Jumin1"].ToString().Trim() + "-" ;
                if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                    strJumin += clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                else
                    strJumin += dt.Rows[0]["Jumin2"].ToString().Trim();
                int nAge = ComFunc.AgeCalc(clsDB.DbCon, VB.Replace(strJumin,"-",""));
                // 전화번호, 휴대폰번호, 주소)
                ssView_Sheet1.Cells[e.Row, 1].Text = dt.Rows[0]["sname"].ToString().Trim(); ;
                ssView_Sheet1.Cells[e.Row, 2].Text = strJumin;
                ssView_Sheet1.Cells[e.Row, 3].Text = dt.Rows[0]["Sex"].ToString().Trim() + "/" + nAge;  //성별/나이
                ssView_Sheet1.Cells[e.Row, 4].Text = dt.Rows[0]["Tel"].ToString().Trim();
                ssView_Sheet1.Cells[e.Row, 5].Text = dt.Rows[0]["HPhone"].ToString().Trim();

                string strPostCode = dt.Rows[0]["ZipCode1"].ToString().Trim() + dt.Rows[0]["ZipCode2"].ToString().Trim();

                if (dt.Rows[0]["BuildNo"].ToString().Trim() != "")
                {
                    ssView_Sheet1.Cells[e.Row, 6].Text = Read_RoadJuso(clsDB.DbCon, dt.Rows[0]["BuildNo"].ToString().Trim()) + " " + dt.Rows[0]["RoadDetail"].ToString().Trim();
                }
                else
                {
                    ssView_Sheet1.Cells[e.Row, 6].Text = Read_Juso(clsDB.DbCon, strPostCode) + " " + dt.Rows[0]["Juso"].ToString().Trim();

                   
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        private void Spread_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.ssView)
            {
                int selRow = ssView_Sheet1.ActiveRowIndex;
                int selCol = ssView_Sheet1.ActiveColumnIndex;

                if (e.KeyCode == Keys.Enter)
                {
                    if (selCol == 0)
                    {
                        string strCode = string.Empty;
                        string strPano = string.Empty;

                        string SQL = string.Empty;
                        string SqlErr = string.Empty;
                        DataTable dt = null;

                        try
                        {
                           

                            strCode = ssView_Sheet1.Cells[selRow, selCol].Text.Trim();
                            strPano = string.Format("{0:D8}", Convert.ToInt32(strCode));
                            ssView_Sheet1.Cells[selRow, selCol].Text = strPano;
                            if (strPano == "")
                            {
                                return;
                            }

                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT SName, Sex, Jumin1, ";
                            SQL += ComNum.VBLF + "        Jumin2, Jumin3, ZipCode1, ";
                            SQL += ComNum.VBLF + "        ZipCode2, Juso, Tel, ";
                            SQL += ComNum.VBLF + "        TO_CHAR(StartDate,'YYYY-MM-DD') StartDate, ";
                            SQL += ComNum.VBLF + "        TO_CHAR(LastDate,'YYYY-MM-DD')  LastDate, ";
                            SQL += ComNum.VBLF + "        TO_CHAR(Birth,'YYYY-MM-DD')  Birth, ";
                            SQL += ComNum.VBLF + "        GbBirth, DeptCode, HPhone, ";
                            SQL += ComNum.VBLF + "        EMail, Jikup, GbJuger, ";
                            SQL += ComNum.VBLF + "        Religion, GbInfor, GbSMS, ";
                            SQL += ComNum.VBLF + "        Gb_VIP, Gb_VIP_Remark, ZipCode3, ";
                            SQL += ComNum.VBLF + "        BuildNo, RoadDetail ";
                            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                            SQL += ComNum.VBLF + "    AND Pano = '" + strPano + "' ";

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
                            string strJumin = dt.Rows[0]["Jumin1"].ToString().Trim() + "-";
                            if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                                strJumin += clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                            else
                                strJumin += dt.Rows[0]["Jumin2"].ToString().Trim();
                            int nAge = ComFunc.AgeCalc(clsDB.DbCon, VB.Replace(strJumin, "-", ""));
                            // 전화번호, 휴대폰번호, 주소)
                            ssView_Sheet1.Cells[selRow, 1].Text = dt.Rows[0]["sname"].ToString().Trim(); ;
                            ssView_Sheet1.Cells[selRow, 2].Text = strJumin;
                            ssView_Sheet1.Cells[selRow, 3].Text = dt.Rows[0]["Sex"].ToString().Trim() + "/" + nAge;  //성별/나이
                            ssView_Sheet1.Cells[selRow, 4].Text = dt.Rows[0]["Tel"].ToString().Trim();
                            ssView_Sheet1.Cells[selRow, 5].Text = dt.Rows[0]["HPhone"].ToString().Trim();

                            string strPostCode = dt.Rows[0]["ZipCode1"].ToString().Trim() + dt.Rows[0]["ZipCode2"].ToString().Trim();

                            if (dt.Rows[0]["BuildNo"].ToString().Trim() != "")
                            {
                                ssView_Sheet1.Cells[selRow, 6].Text = Read_RoadJuso(clsDB.DbCon, dt.Rows[0]["BuildNo"].ToString().Trim()) + " " + dt.Rows[0]["RoadDetail"].ToString().Trim();
                            }
                            else
                            {
                                ssView_Sheet1.Cells[selRow, 6].Text = Read_Juso(clsDB.DbCon, strPostCode) + " " + dt.Rows[0]["Juso"].ToString().Trim();


                            }

                            dt.Dispose();
                            dt = null;
                        }
                        catch (Exception ex)
                        {
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(ex.Message);
                            return;
                        }
                    }

                }
            }
        }



        String Read_Juso(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtJuso = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT MAILJUSO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MAILNEW ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN = '2' ";
            SQL += ComNum.VBLF + "    AND MAILCODE = '" + ArgCode + "' ";
            SqlErr = clsDB.GetDataTableREx(ref DtJuso, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                DtJuso.Dispose();
                DtJuso = null;

                return rtnVal;
            }

            if (DtJuso.Rows.Count > 0)
            {
                rtnVal = DtJuso.Rows[0]["MAILJUSO"].ToString().Trim() + " ";
            }

            DtJuso.Dispose();
            DtJuso = null;

            return rtnVal;
        }
        String Read_RoadJuso(PsmhDb pDbCon, string ArgBuildNo, string ArgZipCode = "")
        {
            DataTable DtJuso = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ZIPNAME1 || ' ' || ZIPNAME2 || ' ' || ZIPNAME3 AS HEADJUSO,";
            SQL += ComNum.VBLF + "        ROADNAME, BUILDNAME, BUN1, BUN2 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ZIPS_ROAD ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND BUILDNO = '" + ArgBuildNo + "' ";

            if (ArgZipCode != "")
                SQL += ComNum.VBLF + "    AND ZIPCODE = '" + ArgZipCode + "' ";

            SqlErr = clsDB.GetDataTableREx(ref DtJuso, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                DtJuso.Dispose();
                DtJuso = null;

                return rtnVal;
            }

            if (DtJuso.Rows.Count > 0)
            {
                rtnVal = DtJuso.Rows[0]["HEADJUSO"].ToString().Trim() + " ";
                rtnVal += DtJuso.Rows[0]["ROADNAME"].ToString().Trim() + " ";
                rtnVal += DtJuso.Rows[0]["BUN1"].ToString().Trim() + " ";

                if (VB.Val(DtJuso.Rows[0]["BUN2"].ToString().Trim()) > 0)
                {
                    rtnVal += "-" + DtJuso.Rows[0]["BUN2"].ToString().Trim() + " ";
                }

                if (DtJuso.Rows[0]["BUILDNAME"].ToString().Trim() != "")
                {
                    rtnVal += DtJuso.Rows[0]["BUILDNAME"].ToString().Trim();
                }
            }

            DtJuso.Dispose();
            DtJuso = null;

            return rtnVal;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            bool x = false;

            if (ComFunc.MsgBoxQ("파일로 만드시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                return;

            x = ssView.SaveExcel("C:\\환자정보.xlsx", FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);
            {
                if (x == true)
                    ComFunc.MsgBox("엑셀파일이 생성이 되었습니다.", "확인");
                else
                    ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
            }
        }
    }
}
