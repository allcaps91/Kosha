using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    /// <summary>
    /// \OPD\jepres\Frm의료급여승인취소관리.frm
    /// </summary>
    public partial class frmPmpaMedicalCareAdmin : Form
    {
        public frmPmpaMedicalCareAdmin()
        {
            InitializeComponent();
        }

        private void frmPmpaMedicalCareAdmin_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetSearchData();
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, PANO,                                                         ";
                SQL += ComNum.VBLF + " SNAME, DEPTCODE, BI, MCODE, MSEQNO                                                                ";
                SQL += ComNum.VBLF + "  FROM OPD_MASTER                                                                                  ";
                SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + dtpBDate.Text + "','YYYY-MM-DD')                                       ";
                SQL += ComNum.VBLF + "   AND PANO NOT IN (SELECT PANO FROM IPD_NEW_MASTER                                                ";
                SQL += ComNum.VBLF + "                     WHERE BI IN ('22','21')                                                       ";
                SQL += ComNum.VBLF + "                       AND INDATE >= TO_DATE('" + dtpBDate.Text + "','YYYY-MM-DD')                 ";
                SQL += ComNum.VBLF + "                       AND INDATE <= TO_DATE('" + dtpBDate.Value.AddDays(1).ToShortDateString() + "','YYYY-MM-DD'))   ";
                if (rdoJob1.Checked)
                {
                    SQL += ComNum.VBLF + "  AND (MSEQNO IS NULL OR MSEQNO ='')                                ";
                }
                SQL += ComNum.VBLF + "   AND BI IN ('21','22')                                                                           ";
                SQL += ComNum.VBLF + "   AND REP <> '#' ";
                SQL += ComNum.VBLF + " ORDER BY PANO                                                                                     ";

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
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssList_Sheet1.RowCount = dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BI"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["MCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["MSEQNO"].ToString().Trim();

                    if(rdoJob1.Checked)
                    {
                        SQL = " SELECT SuCode";
                        SQL += ComNum.VBLF + " FROM ADMIN.OCS_OORDER";
                        SQL += ComNum.VBLF + "  WHERE PTNO ='" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "   AND DEPTCODE ='" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "   AND BDATE = TO_DATE('" + dtpBDate.Text + "','YYYY-MM-DD')  ";
                        SQL += ComNum.VBLF + "   AND GBSUNAP ='0' ";
                        SQL += ComNum.VBLF + "   AND SUBSTR(SUCODE ,1,2) ='$$' ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if(dt2.Rows.Count > 0)
                        {
                            for(int j = 0; j < dt2.Rows.Count; j++)
                            {
                                ssList_Sheet1.Cells[i, 5].Text += dt2.Rows[j]["SuCode"].ToString().Trim() + ",";
                            }
                        }

                        dt2.Dispose();
                        dt2 = null;
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            Set_Print();
        }

        void Set_Print()
        {
            string strTitle = "의료급여 승인내역";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            btnPrint.Enabled = false;


            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업 일자 : " + dtpBDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("작성자    : " + clsType.User.UserName , new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력 시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 30, 200, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            CS.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
            CS = null;

            btnPrint.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ssList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //TODO
            //Dim strPano     As String
            //Dim strDeptCode As String
            //Dim strBDATE    As String



            //If Row > 0 And Col = 5 Then

            //    Call READ_SYSDATE

            //    strBDATE = GstrSysDate


            //    SS1.Row = Row
            //    SS1.Col = 1: strPano = Trim(SS1.Text)
            //    SS1.Col = 3: strDeptCode = Trim(SS1.Text)


            //    If strPano <> "" And strDeptCode <> "" Then


            //        BOHO.JOB = "잔액확인"
            //        BOHO.Pano = strPano
            //        BOHO.DeptCode = strDeptCode
            //        BOHO.BDate = strBDATE



            //        Frm의료급여승인_SUB.Show 1


            //        Call CmdView_Click


            //    End If
            //End If
        }
    }
}
