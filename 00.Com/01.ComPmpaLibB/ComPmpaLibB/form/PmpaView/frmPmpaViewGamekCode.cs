using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewGamekCode.cs
    /// Description     : 일자별 - 직원진료 대상자 조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-26
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm재원감액코드조회.frm(Frm재원감액코드조회) => frmPmpaViewGamekCode.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm재원감액코드조회.frm(Frm재원감액코드조회)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewGamekCode : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        public frmPmpaViewGamekCode()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
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
            optIO0.Checked = true;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnView)
            {
                //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                //{
                //    return; //권한 확인
                //}
                eGetData();
            }
        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            CS.Spread_All_Clear(ssList);

            if (optIO0.Checked == true)
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  M.Pano,M.SName,M.Bi,M.PName,M.GbGamek,";
                SQL += ComNum.VBLF + "  TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,M.Ilsu,M.IpdNo,M.GbSts,";
                SQL += ComNum.VBLF + "  TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate,";
                SQL += ComNum.VBLF + "  M.DeptCode,M.DrCode";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER m";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND (M.OutDate IS NULL OR M.OutDate>=TO_DATE('" + Convert.ToDateTime(dtpDate.Text).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "      AND M.IpwonTime < TO_DATE('" + Convert.ToDateTime(dtpDate.Text).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND M.Pano < '90000000'";
                SQL += ComNum.VBLF + "      AND M.Pano <> '81000004'";
                SQL += ComNum.VBLF + "      AND M.GbSTS <> '9' ";
                SQL += ComNum.VBLF + "      AND M.GbGamek ='21'";
            }

            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  M.Pano,M.SName,M.Bi,M.GbGamek,";
                SQL += ComNum.VBLF + "  TO_CHAR(M.ActDate,'YYYY-MM-DD') ActDate,";
                SQL += ComNum.VBLF + "  TO_CHAR(M.BDate,'YYYY-MM-DD') InDate,";
                SQL += ComNum.VBLF + "  M.DeptCode,M.DrCode";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER M";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND M.ACTDATE= TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND M.Pano < '90000000'";
                SQL += ComNum.VBLF + "      AND M.Pano <> '81000004'";
                SQL += ComNum.VBLF + "      AND M.GbGamek ='21'";
            }

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
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;

                    ssList_Sheet1.Rows.Count = nRead;

                    for (i = 0; i < nRead; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GbGamek"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = Bas_Gamek_Name2(dt.Rows[i]["GbGamek"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        public string Bas_Gamek_Name2(string ArgCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                ";
            SQL += ComNum.VBLF + "  NAME                                ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE  ";
            SQL += ComNum.VBLF + "WHERE 1=1                             ";
            SQL += ComNum.VBLF + "      AND GUBUN = 'BAS_감액코드명'    ";
            SQL += ComNum.VBLF + "      AND CODE  = '" + ArgCode + "'   ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                    
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    rtnVal = "감액무";
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

    }
}
