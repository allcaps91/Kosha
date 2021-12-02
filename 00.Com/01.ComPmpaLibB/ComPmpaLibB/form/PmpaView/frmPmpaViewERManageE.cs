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
    /// File Name       : frmPmpaViewERManageE.cs
    /// Description     : 응급관리료(AC101A)
    /// Author          : 안정수
    /// Create Date     : 2017-09-21
    /// Update History  : 2017-11-29
    /// 조회 시 쿼리부분 수정
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm응급관리료.frm(Frm응급관리료) => frmPmpaViewERManageE.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm응급관리료.frm(Frm응급관리료)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewERManageE : Form
    {
        public frmPmpaViewERManageE()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
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
            optSort2.Checked = true;

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            dtpFDate.Text = Convert.ToDateTime(CurrentDate).AddDays(-1).ToShortDateString();
            dtpFTime.Text = "00:01";
            dtpTDate.Text = Convert.ToDateTime(CurrentDate).AddDays(-1).ToShortDateString();
            dtpTTime.Text = "23:59";
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            //ssList.ActiveSheet.Cells[0, 9].Text = "zzz";
            //ssList.ActiveSheet.Columns[9].Visible = false;


            #endregion

            strTitle = "응급관리료 감액자 명단";
            strSubTitle = "조회일자 : " + dtpFDate.Text + " " + dtpFTime.Text + " ~ " + dtpTDate.Text + " " + dtpTTime.Text;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 100, 10, 95, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;

            string strPano = "";
            string strBDate = "";
            int nAge = 0;

            string strSex = "";
            string strSname = "";
            string strDeptCode = "";
            string strJTime = "";
            string strFDate = "";
            string strFTime = "";
            string strTDate = "";
            string strTTime = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            strFDate = dtpFDate.Text;
            strFTime = dtpFTime.Text;
            strTDate = dtpTDate.Text;
            strTTime = dtpTTime.Text;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                ";
            SQL += ComNum.VBLF + "  PANO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,                                            ";
            SQL += ComNum.VBLF + "  AGE, SEX, SNAME, DEPTCODE,                                                          ";
            SQL += ComNum.VBLF + "  TO_CHAR(JTIME, 'YYYY-MM-DD HH24:MI') JTIME                                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
            SQL += ComNum.VBLF + "      AND JTIME >= TO_DATE('" + strFDate + " " + strFTime + "','YYYY-MM-DD HH24:MI')  ";
            SQL += ComNum.VBLF + "      AND JTIME <= TO_DATE('" + strTDate + " " + strTTime + "','YYYY-MM-DD HH24:MI')  ";
            SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                         ";
            SQL += ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                         ";
            SQL += ComNum.VBLF + "      AND DEPTCODE  = 'ER'                                                            ";
            if (optSort0.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY PANO                                                                     ";
            }
            else if (optSort1.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY SNAME                                                                    ";
            }
            else
            {
                SQL += ComNum.VBLF + "ORDER BY JTIME                                                                    ";
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
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    ssList_Sheet1.Rows.Count = 0;

                    for (i = 0; i < nRead; i++)
                    {
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strBDate = dt.Rows[i]["BDATE"].ToString().Trim();
                        strSex = dt.Rows[i]["SEX"].ToString().Trim();
                        nAge = Convert.ToInt32(dt.Rows[i]["AGE"].ToString().Trim());
                        strSname = dt.Rows[i]["SNAME"].ToString().Trim();
                        strDeptCode = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        strJTime = dt.Rows[i]["JTIME"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                        ";
                        SQL += ComNum.VBLF + "  SUM(NAL) NAL                                                ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP                           ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                        SQL += ComNum.VBLF + "      AND PANO  = '" + strPano + "'                           ";
                        SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD')    ";
                        SQL += ComNum.VBLF + "      AND SUCODE IN ('AC101A','V1200A')                       ";
                        SQL += ComNum.VBLF + "HAVING SUM(NAL) > 0                                           ";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dt1.Rows[0]["NAL"].ToString().Trim()) > 0)
                            {
                                ssList_Sheet1.Rows.Count += 1;

                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = strPano;
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = strSname;
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = nAge.ToString();
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = strSex;
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = strDeptCode;
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = strJTime;
                                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 6].Text = " ";

                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                    }
                }

                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
            //ComFunc.MsgBox("조회가 끝났습니다.");

        }

    }
}
