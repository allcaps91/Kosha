using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewResv.cs
    /// Description     : 예약현황 조회
    /// Author          : 안정수
    /// Create Date     : 2017-08-24
    /// Update History  : 
    /// <history>    
    /// TODO : FrmPatientSearch 폼 구현필요, 실제 테스트 필요
    /// d:\psmh\OPD\oiguide\oiguide04.frm(FrmResv) => frmPmpaViewResv.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oiguide\oiguide04.frm(FrmResv)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewResv : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        string mstrHelpCode = "";

        public frmPmpaViewResv()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewResv(string GstrHelpCode)
        {
            InitializeComponent();
            setEvent();
            mstrHelpCode = GstrHelpCode;
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnSearch.Click += new EventHandler(eBtnEvent);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  
            this.StartPosition = FormStartPosition.CenterScreen;

            txtPano.Text = "";
            txtSName.Text = "";

            CS.Spread_All_Clear(ssList);

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
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

        void eGetData()
        {
            int i = 0;
            txtSName.Text = "";
            txtPano.Text = txtPano.Text.Trim();

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (txtPano.Text == "")
            {
                return;
            }

            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

            ssList_Sheet1.Rows.Count = 0;
            CS.Spread_All_Clear(ssList);

            //환자마스터를 READ
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                    ";
            SQL += ComNum.VBLF + "  SName                                   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT    ";
            SQL += ComNum.VBLF + "WHERE Pano='" + txtPano.Text + "'         ";

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
                    txtSName.Text = dt.Rows[0]["SName"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            //예약내역을 READ
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                 ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.Date3,'YYYY-MM-DD HH24:MI') RDate, A.BI,                                   ";
            SQL += ComNum.VBLF + "  a.DeptCode,b.DrName,a.RMemo                                                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW a, " + ComNum.DB_PMPA + "BAS_DOCTOR b      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                              ";
            SQL += ComNum.VBLF + "      AND a.Pano='" + txtPano.Text + "'                                                ";
            SQL += ComNum.VBLF + "      AND (a.TRANSDATE IS NULL OR TRUNC(a.TRANSDATE)=TRUNC(SYSDATE) )                  ";
            SQL += ComNum.VBLF + "      AND a.RETDATE IS NULL                                                            ";
            SQL += ComNum.VBLF + "      AND a.DrCode=b.DrCode(+)                                                         ";
            SQL += ComNum.VBLF + "ORDER BY a.Date3                                                                       ";

            try
            {

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Rows.Count += 1;

                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "예약";
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10);
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = VB.Right(dt.Rows[i]["RDate"].ToString().Trim(), 5);
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = CF.Read_Bi_Name(clsDB.DbCon, dt.Rows[i]["BI"].ToString().Trim(), "1");
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) == DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 6].Text = "당일예약";
                        }
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 7].Text = dt.Rows[i]["RMemo"].ToString().Trim();
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

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                 ";
            SQL += ComNum.VBLF + "  TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE, A.RTIME,                                        ";
            SQL += ComNum.VBLF + "  a.DeptCode,b.DrName                                                                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_TELRESV a, " + ComNum.DB_PMPA + "BAS_DOCTOR b           ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                              ";
            SQL += ComNum.VBLF + "      AND a.Pano='" + txtPano.Text + "'                                                ";
            SQL += ComNum.VBLF + "      AND RDATE >= TRUNC(SYSDATE)                                                      ";
            SQL += ComNum.VBLF + "      AND a.DrCode=b.DrCode(+)                                                         ";
            SQL += ComNum.VBLF + "ORDER BY a.RDATE                                                                       ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Rows.Count += 1;

                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "전화";
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10);
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = dt.Rows[i]["RTIME"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = dt.Rows[i]["DrName"].ToString().Trim();

                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 7].Text = "";
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

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                 ";
            SQL += ComNum.VBLF + "  Pano,SName,DeptCode,DrCode,                                                          ";
            SQL += ComNum.VBLF + "  TO_CHAR(BDATE,'YYYY-MM-DD') RDATE,                                                   ";
            SQL += ComNum.VBLF + "  TO_CHAR(JTIME,'HH24:MI') Rtime,PART                                                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                              ";
            SQL += ComNum.VBLF + "      AND Pano='" + txtPano.Text + "'                                                  ";
            SQL += ComNum.VBLF + "      AND BDATE = TRUNC(SYSDATE)                                                       ";
            SQL += ComNum.VBLF + "      AND PART = '333'                                                                 ";
            SQL += ComNum.VBLF + "      AND JIN = '5'                                                                    ";
            SQL += ComNum.VBLF + "ORDER BY BDATE                                                                         ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Rows.Count += 1;

                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "당일대리";
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 1].Text = VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10);
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 2].Text = dt.Rows[i]["RTIME"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());

                        ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 7].Text = "";
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

            if (ssList_Sheet1.Rows.Count == 0)
            {
                ComFunc.MsgBox("예약내역이 없습니다.");
            }
        }

        void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnView.Focus();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            eGetData2();
        }

        void eGetData2()
        {
            mstrHelpCode = "";

            txtPano.Text = "";
            clsPublic.GstrChoicePano = "";

            frmPatientSearch frmPatientSearchX = new frmPatientSearch();
            frmPatientSearchX.StartPosition = FormStartPosition.CenterParent;
            frmPatientSearchX.ShowDialog();
            frmPatientSearchX.Dispose();
            frmPatientSearchX = null;

            if(clsPublic.GstrChoicePano.Length > 0)
            {
                txtPano.Text = clsPublic.GstrChoicePano;
                eGetData();
            }
        }
    }
}
