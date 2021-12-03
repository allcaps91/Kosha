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
    /// File Name       : frmPmpaViewSangdamPano.cs
    /// Description     : 상담자 명단찾기
    /// Author          : 안정수
    /// Create Date     : 2017-08-08
    /// Update History  : 2017-10-23   
    /// <history>       
    /// d:\psmh\Etc\csinfo\CsInfo60.frm(FrmSangdamPano) => frmPmpaViewSangdamPano.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\Etc\csinfo\CsInfo60.frm(FrmSangdamPano)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewSangdamPano : Form
    {
        ComFunc CF = new ComFunc();
        public frmPmpaViewSangdamPano()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);

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

            ssList_Sheet1.Columns[7].Visible = false; //ROWID

            Set_Combo();

            //본 소스에서도 인쇄버튼만 있고 구현이 안되어있으므로 false 처리
            btnPrint.Enabled = false;
            btnPrint.Visible = false;
        }

        void Set_Combo()
        {
            int i = 0;
            string strCODE = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            cboJob.Items.Clear();
            cboJob.Items.Add("1.재원환자");
            cboJob.Items.Add("2.입원환자");
            cboJob.Items.Add("3.퇴원환자");
            cboJob.Items.Add("4.외래진료");
            cboJob.Items.Add("5.종검환자");
            cboJob.Items.Add("6.가정간호");
            cboJob.Items.Add("9.면담자");
            cboJob.SelectedIndex = 0;

            cboDept.Items.Clear();
            cboDept.Items.Add("**.전체진료과");

            //진료과
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                        ";
            SQL += ComNum.VBLF + "  DeptCode,DeptNameK                                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ClinicDept                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "      ANd DeptCode NOT IN ('II','R6')                         ";
            SQL += ComNum.VBLF + "ORDER BY PrintRanking,DeptCode                                ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strCODE = dt.Rows[i]["DeptCode"].ToString().Trim() + ".";
                        strCODE += dt.Rows[i]["DeptNameK"].ToString().Trim();
                        cboDept.Items.Add(strCODE);
                    }

                }

                cboDept.SelectedIndex = 0;
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                //                
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                //                
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void ePrint()
        {
            // 기존소스에서 프린트 기능 구현 안되있음
            btnPrint.Enabled = false;
        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;
            int nRow = 0;

            string strJob = "";
            string strDept = "";
            string strFDate = "";
            string strTDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int nAge = 0;
            string strSex = "";

            ssList_Sheet1.Rows.Count = 0;
            ssList_Sheet1.Rows.Count = 20;
            Cursor.Current = Cursors.WaitCursor;

            strJob = VB.Left(cboJob.SelectedItem.ToString(), 1);
            strDept = VB.Left(cboDept.SelectedItem.ToString(), 2);
            strFDate = dtpFdate.Text;
            strTDate = dtpTdate.Text;

            if (strJob == "")
            {
                ComFunc.MsgBox("작업구분이 공란입니다.");
                return;
            }
            if (strDept == "")
            {
                ComFunc.MsgBox("진료과가 공란입니다.");
                return;
            }

            switch (strJob)
            {
                // 재원환자, 입원환자
                case "1":
                case "2":
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                                    ";
                    SQL += ComNum.VBLF + "  a.Pano,b.Sname,a.Age,a.Sex,b.Tel,b.PName,a.WardCode,                                    ";
                    SQL += ComNum.VBLF + "  a.RoomCode,a.DeptCode                                                                   ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, " + ComNum.DB_PMPA + "BAS_PATIENT b          ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                    SQL += ComNum.VBLF + "      AND a.Pano < '90000000'                                                             ";
                    SQL += ComNum.VBLF + "      AND a.OUTDATE IS NULL                                                               ";
                    SQL += ComNum.VBLF + "      AND a.GBSTS  = '0' ";
                    if (strDept != "**")
                    {
                        SQL += ComNum.VBLF + "      AND a.DeptCode='" + strDept + "'                                                ";
                    }
                    // 입원환자
                    if (strJob == "2")
                    {
                        SQL += ComNum.VBLF + "      AND a.InDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')                          ";
                        SQL += ComNum.VBLF + "      AND a.InDate<=TO_DATE('" + strTDate + " 23:59','YYYY-MM-DD HH24:MI')            ";
                    }
                    SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                                ";
                    SQL += ComNum.VBLF + "ORDER BY b.SName,b.Jumin1                                                                 ";
                    break;

                //퇴원환자
                case "3":
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                                    ";
                    SQL += ComNum.VBLF + "  a.Pano,b.Sname,a.Age,a.Sex,b.Tel,b.PName,a.WardCode,                                    ";
                    SQL += ComNum.VBLF + "  a.RoomCode,a.DeptCode                                                                   ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, " + ComNum.DB_PMPA + "BAS_PATIENT b          ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                    SQL += ComNum.VBLF + "      AND a.ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')                             ";
                    SQL += ComNum.VBLF + "      AND a.ActDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD')                             ";
                    SQL += ComNum.VBLF + "      AND a.Pano < '90000000'                                                             ";
                    SQL += ComNum.VBLF + "      AND a.GBSTS = '7'                                                                   ";
                    SQL += ComNum.VBLF + "      AND a.OUTDATE IS NOT NULL                                                           ";
                    if (strDept != "**")
                    {
                        SQL += ComNum.VBLF + "      AND a.DeptCode='" + strDept + "'                                                ";
                    }
                    SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                                ";
                    SQL += ComNum.VBLF + "ORDER BY b.SName,b.Jumin1                                                                 ";
                    break;

                //외래진료
                case "4":
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                                    ";
                    SQL += ComNum.VBLF + "  a.Pano,b.Sname,a.Age,a.Sex,b.Tel,b.PName,'' WardCode,                                   ";
                    SQL += ComNum.VBLF + "  '' RoomCode,a.DeptCode                                                                  ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER a, " + ComNum.DB_PMPA + "BAS_PATIENT b              ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                    SQL += ComNum.VBLF + "      AND a.ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')                             ";
                    SQL += ComNum.VBLF + "      AND a.ActDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD')                             ";
                    SQL += ComNum.VBLF + "      AND a.Pano < '90000000'                                                             ";
                    if (strDept != "**")
                    {
                        SQL += ComNum.VBLF + "      AND a.DeptCode='" + strDept + "'                                                ";
                    }
                    SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                                ";
                    SQL += ComNum.VBLF + "ORDER BY b.SName,b.Jumin1                                                                 ";
                    break;

                //종검환자
                case "5":
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                                    ";
                    SQL += ComNum.VBLF + "  a.Pano,b.Sname,'999' Age,b.Sex,b.Tel,b.PName,'' WardCode,                               ";
                    SQL += ComNum.VBLF + "  '' RoomCode                                                                ";                    
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MED_PATIENT a, " + ComNum.DB_PMPA + "BAS_PATIENT b             ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                    SQL += ComNum.VBLF + "      AND a.SDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')                               ";
                    SQL += ComNum.VBLF + "      AND a.SDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD')                               ";
                    SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)";
                    SQL += ComNum.VBLF + "ORDER BY b.SName,b.Jumin1";
                    break;

                //가정간호
                case "6":
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                                    ";
                    SQL += ComNum.VBLF + "  a.Pano,b.Sname,'999' Age,b.Sex,b.Tel,b.PName,'' WardCode,                               ";
                    SQL += ComNum.VBLF + "  '' RoomCode,a.DeptCode                                                                  ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_HOMEVISIT a, " + ComNum.DB_PMPA + "BAS_PATIENT b           ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                    SQL += ComNum.VBLF + "      AND a.VDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')                               ";
                    SQL += ComNum.VBLF + "      AND a.VDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD')                               ";
                    SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)";
                    SQL += ComNum.VBLF + "ORDER BY b.SName,b.Jumin1";
                    break;

                //상담환자
                case "9":
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                                    ";
                    SQL += ComNum.VBLF + "  a.Pano,b.Sname,'999' Age,b.Sex,b.Tel,b.PName,'' WardCode,                               ";
                    SQL += ComNum.VBLF + "  '' RoomCode,a.DeptCode                                                                  ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_SANGDAM a, " + ComNum.DB_PMPA + "BAS_PATIENT b      ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                    SQL += ComNum.VBLF + "      AND a.SDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')                               ";
                    SQL += ComNum.VBLF + "      AND a.SDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD')                               ";
                    SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)";
                    SQL += ComNum.VBLF + "ORDER BY b.SName,b.Jumin1";
                    break;
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();

                        strSex = dt.Rows[i]["Sex"].ToString().Trim();

                        //재원자
                        //if (strJob == "1" || strJob == "2")
                        //{
                            nAge = Convert.ToInt32(dt.Rows[i]["Age"].ToString().Trim());
                        //}
                        //else
                        //{
                        //    nAge = ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[i]["Jumin1"].ToString().Trim() + dt.Rows[i]["Jumin2"].ToString().Trim());
                        //}

                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + strSex;
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Tel"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PName"].ToString().Trim();
                        //재원자
                        if (strJob == "1" || strJob == "2")
                        {
                            ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        }

                        if (strJob != "9")
                        {
                            ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        }
                    }
                }

                else if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("해당 DATA가 존재하지 않습니다.");
                }
            }

            catch (System.Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
            }

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;
        }

        void cboJob_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            switch (VB.Left(cboJob.SelectedItem.ToString(), 1))
            {
                // 재원환자
                case "1":
                    groupBox3.Enabled = false;
                    groupBox3.Text = "";
                    dtpFdate.Text = "";
                    dtpTdate.Text = "";
                    break;

                // 입원환자
                case "2":
                    groupBox3.Enabled = true;
                    groupBox3.Text = "입원일자";
                    dtpFdate.Text = Convert.ToDateTime(CurrentDate).AddDays(-2).ToShortDateString();
                    dtpTdate.Text = CurrentDate;
                    cboDept.SelectedIndex = 0;
                    break;

                // 퇴원환자
                case "3":
                    groupBox3.Enabled = true;
                    groupBox3.Text = "퇴원일자";
                    dtpFdate.Text = Convert.ToDateTime(CurrentDate).AddDays(-3).ToShortDateString();
                    dtpTdate.Text = Convert.ToDateTime(CurrentDate).AddDays(-1).ToShortDateString();
                    cboDept.SelectedIndex = 0;
                    break;

                // 외래진료
                case "4":
                    groupBox3.Enabled = true;
                    groupBox3.Text = "외래진료일";
                    dtpFdate.Text = Convert.ToDateTime(CurrentDate).AddDays(-2).ToShortDateString();
                    dtpTdate.Text = CurrentDate;
                    cboDept.SelectedIndex = 0;
                    break;

                // 종합검진
                case "5":
                    groupBox3.Enabled = true;
                    groupBox3.Text = "종합검진일";
                    dtpFdate.Text = Convert.ToDateTime(CurrentDate).AddDays(-2).ToShortDateString();
                    dtpTdate.Text = CurrentDate;
                    cboDept.SelectedIndex = 0;
                    break;

                // 가정간호
                case "6":
                    groupBox3.Enabled = true;
                    groupBox3.Text = "방문일자";
                    dtpFdate.Text = Convert.ToDateTime(CurrentDate).AddDays(-2).ToShortDateString();
                    dtpTdate.Text = CurrentDate;
                    cboDept.SelectedIndex = 0;
                    break;

                // 면담자
                case "9":
                    groupBox3.Enabled = true;
                    groupBox3.Text = "면담일자";
                    dtpFdate.Text = Convert.ToDateTime(CurrentDate).AddDays(-2).ToShortDateString();
                    dtpTdate.Text = CurrentDate;
                    cboDept.SelectedIndex = 0;
                    break;
            }
        }
    }
}
