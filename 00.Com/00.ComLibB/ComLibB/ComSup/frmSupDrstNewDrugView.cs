using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstNewDrugView.cs
    /// Description     : 신약/사용 중단 약품안내
    /// Author          : 이정현
    /// Create Date     : 2017-12-15
    /// <history> 
    /// 신약/사용 중단 약품안내
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\FrmNewDrugView.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstNewDrugView : Form
    {
        public frmSupDrstNewDrugView()
        {
            InitializeComponent();
        }

        private void frmSupDrstNewDrugView_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
            
            GetTotal();
            GetData();
        }

        private void btnTotal_Click(object sender, EventArgs e)
        {
            GetTotal();
        }

        private void GetTotal()
        {
            lblName1.ForeColor = Color.FromArgb(0, 0, 0);
            lblName1.BackColor = Color.FromArgb(255, 255, 192);
            lblName2.ForeColor = Color.FromArgb(0, 0, 0);
            lblName2.BackColor = Color.FromArgb(255, 255, 192);
            lblName3.ForeColor = Color.FromArgb(0, 0, 0);
            lblName3.BackColor = Color.FromArgb(255, 255, 192);
            lblName4.ForeColor = Color.FromArgb(0, 0, 0);
            lblName4.BackColor = Color.FromArgb(255, 255, 192);

            panView1.Dock = DockStyle.Top;
            panView2.Dock = DockStyle.Top;
            panView3.Dock = DockStyle.Top;
            panView4.Dock = DockStyle.Top;

            panView1.Visible = true;
            panView2.Visible = true;
            panView3.Visible = true;
            panView4.Visible = true;
        }

        private void lblName_DoubleClick(object sender, EventArgs e)
        {
            string strGubun = VB.Right(((Label)sender).Name, 1);

            lblName1.ForeColor = Color.FromArgb(0, 0, 0);
            lblName1.BackColor = Color.FromArgb(255, 255, 192);
            lblName2.ForeColor = Color.FromArgb(0, 0, 0);
            lblName2.BackColor = Color.FromArgb(255, 255, 192);
            lblName3.ForeColor = Color.FromArgb(0, 0, 0);
            lblName3.BackColor = Color.FromArgb(255, 255, 192);
            lblName4.ForeColor = Color.FromArgb(0, 0, 0);
            lblName4.BackColor = Color.FromArgb(255, 255, 192);

            panView1.Dock = DockStyle.None;
            panView2.Dock = DockStyle.None;
            panView3.Dock = DockStyle.None;
            panView4.Dock = DockStyle.None;

            panView1.Visible = false;
            panView2.Visible = false;
            panView3.Visible = false;
            panView4.Visible = false;

            switch (strGubun)
            {
                case "1":
                    lblName1.ForeColor = Color.FromArgb(255, 255, 0);
                    lblName1.BackColor = Color.FromArgb(0, 0, 192);
                    panView1.Dock = DockStyle.Fill;
                    panView1.Visible = true;
                    break;
                case "2":
                    lblName2.ForeColor = Color.FromArgb(255, 255, 0);
                    lblName2.BackColor = Color.FromArgb(0, 0, 192);
                    panView2.Dock = DockStyle.Fill;
                    panView2.Visible = true;
                    break;
                case "3":
                    lblName3.ForeColor = Color.FromArgb(255, 255, 0);
                    lblName3.BackColor = Color.FromArgb(0, 0, 192);
                    panView3.Dock = DockStyle.Fill;
                    panView3.Visible = true;
                    break;
                case "4":
                    lblName4.ForeColor = Color.FromArgb(255, 255, 0);
                    lblName4.BackColor = Color.FromArgb(0, 0, 192);
                    panView4.Dock = DockStyle.Fill;
                    panView4.Visible = true;
                    break;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            
            READ_SS1();
            READ_SS2();
            READ_SS3();
            READ_SS4();
        }

        private void READ_SS1()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView1_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JEPCODE, REMARK1, REMARK2, REMARK2_CHECK, REMARK3, REMARK3_ETC, BIGO, ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_INFO01";
                SQL = SQL + ComNum.VBLF + "     WHERE (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL)";
                SQL = SQL + ComNum.VBLF + "ORDER BY WRITEDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT * 4);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i, 0].Text = READ_JEP_INFO(dt.Rows[i]["JEPCODE"].ToString().Trim());

                        switch (dt.Rows[i]["REMARK1"].ToString().Trim())
                        {
                            case "1": ssView1_Sheet1.Cells[i, 1].Text = "원내외혼용"; break;
                            case "2": ssView1_Sheet1.Cells[i, 1].Text = "원내전용"; break;
                            case "3": ssView1_Sheet1.Cells[i, 1].Text = "원외전용"; break;
                        }

                        if (dt.Rows[i]["REMARK2_CHECK"].ToString().Trim() == "1")
                        {
                            ssView1_Sheet1.Cells[i, 2].Text = "추후 알림";
                        }
                        else
                        {
                            ssView1_Sheet1.Cells[i, 2].Text = Convert.ToDateTime(dt.Rows[i]["REMARK2"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }

                        switch (dt.Rows[i]["REMARK3"].ToString().Trim())
                        {
                            case "1":
                                ssView1_Sheet1.Cells[i, 3].Text = "유사/동종약 없음";
                                break;
                            case "2":
                                ssView1_Sheet1.Cells[i, 3].Text = "제형/품목 추가"
                                    + ComNum.VBLF + "  => 유사/동종약 : " + dt.Rows[i]["REMARK3_ETC"].ToString().Trim();
                                break;
                            case "3":
                                ssView1_Sheet1.Cells[i, 3].Text = "기존약 대체"
                                    + ComNum.VBLF + "  => 기존약 : " + dt.Rows[i]["REMARK3_ETC"].ToString().Trim();
                                break;
                        }

                        ssView1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_SS2()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView2_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JEPCODE, REMARK1, REMARK1_CHECK, REMARK2, REMARK2_ETC, REMARK3, REMARK3_ETC, REMARK4, BIGO, ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_INFO02";
                SQL = SQL + ComNum.VBLF + "     WHERE (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL)";
                SQL = SQL + ComNum.VBLF + "ORDER BY WRITEDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.RowCount = dt.Rows.Count;
                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT * 4);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView2_Sheet1.Cells[i, 0].Text = READ_JEP_INFO(dt.Rows[i]["JEPCODE"].ToString().Trim());

                        switch (dt.Rows[i]["REMARK4"].ToString().Trim())
                        {
                            case "1": ssView2_Sheet1.Cells[i, 1].Text = "원내외혼용"; break;
                            case "2": ssView2_Sheet1.Cells[i, 1].Text = "원내전용"; break;
                            case "3": ssView2_Sheet1.Cells[i, 1].Text = "원외전용"; break;
                        }

                        if (dt.Rows[i]["REMARK1_CHECK"].ToString().Trim() == "1")
                        {
                            ssView2_Sheet1.Cells[i, 2].Text = "추후 알림";
                        }
                        else
                        {
                            ssView2_Sheet1.Cells[i, 2].Text = Convert.ToDateTime(dt.Rows[i]["REMARK1"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }

                        switch (dt.Rows[i]["REMARK2"].ToString().Trim())
                        {
                            case "1": ssView2_Sheet1.Cells[i, 3].Text = "약사위원회 결과"; break;
                            case "2": ssView2_Sheet1.Cells[i, 3].Text = "생산중단"; break;
                            case "3": ssView2_Sheet1.Cells[i, 3].Text = "일시품절"; break;
                            case "4": ssView2_Sheet1.Cells[i, 3].Text = "보험코드 삭제"; break;
                            case "5": ssView2_Sheet1.Cells[i, 3].Text = "기타 : " + dt.Rows[i]["REMARK2_ETC"].ToString().Trim(); break;
                        }

                        switch (dt.Rows[i]["REMARK3"].ToString().Trim())
                        {
                            case "1": ssView2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["REMARK3_ETC"].ToString().Trim(); break;
                            case "2": ssView2_Sheet1.Cells[i, 4].Text = "없음"; break;
                            case "3": ssView2_Sheet1.Cells[i, 4].Text = "추후 알림"; break;
                        }

                        ssView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_SS3()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView3_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JEPCODE, REMARK1, REMARK1_CHECK, REMARK2, REMARK3, BIGO, ROWID, WRITEDATE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_INFO04";
                SQL = SQL + ComNum.VBLF + "     WHERE (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL)";
                SQL = SQL + ComNum.VBLF + "ORDER BY WRITEDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView3_Sheet1.RowCount = dt.Rows.Count;
                    ssView3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT * 4);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView3_Sheet1.Cells[i, 0].Text = READ_JEP_INFO(dt.Rows[i]["JEPCODE"].ToString().Trim());

                        if (dt.Rows[i]["REMARK1_CHECK"].ToString().Trim() == "1")
                        {
                            ssView3_Sheet1.Cells[i, 1].Text = "추후 알림";
                        }
                        else
                        {
                            ssView3_Sheet1.Cells[i, 1].Text = Convert.ToDateTime(dt.Rows[i]["REMARK1"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }

                        switch (dt.Rows[i]["REMARK2"].ToString().Trim())
                        {
                            case "1": ssView3_Sheet1.Cells[i, 2].Text = "원내외혼용"; break;
                            case "2": ssView3_Sheet1.Cells[i, 2].Text = "원내전용"; break;
                            case "3": ssView3_Sheet1.Cells[i, 2].Text = "원외전용"; break;
                        }

                        switch (dt.Rows[i]["REMARK3"].ToString().Trim())
                        {
                            case "1": ssView3_Sheet1.Cells[i, 3].Text = "원내외혼용"; break;
                            case "2": ssView3_Sheet1.Cells[i, 3].Text = "원내전용"; break;
                            case "3": ssView3_Sheet1.Cells[i, 3].Text = "원외전용"; break;
                        }

                        ssView3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                        ssView3_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        //ssView3_Sheet1.Cells[i, 6].Text = dt.Rows[i]["WRITEDATE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_SS4()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView4_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JEPCODE, REMARK1, REMARK2, REMARK2_CHECK, REMARK3, REMARK3_ETC, BIGO, ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_INFO03";
                SQL = SQL + ComNum.VBLF + "     WHERE (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL)";
                SQL = SQL + ComNum.VBLF + "ORDER BY WRITEDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView4_Sheet1.RowCount = dt.Rows.Count;
                    ssView4_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT * 4);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView4_Sheet1.Cells[i, 0].Text = READ_JEP_INFO(dt.Rows[i]["JEPCODE"].ToString().Trim());
                        ssView4_Sheet1.Cells[i, 1].Text = dt.Rows[i]["REMARK1"].ToString().Trim();
                        
                        if (dt.Rows[i]["REMARK2_CHECK"].ToString().Trim() == "1")
                        {
                            ssView4_Sheet1.Cells[i, 2].Text = "추후 알림";
                        }
                        else
                        {
                            ssView4_Sheet1.Cells[i, 2].Text = Convert.ToDateTime(dt.Rows[i]["REMARK2"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }

                        switch (dt.Rows[i]["REMARK3"].ToString().Trim())
                        {
                            case "1": ssView4_Sheet1.Cells[i, 3].Text = "표준코드 변경"; break;
                            case "2": ssView4_Sheet1.Cells[i, 3].Text = "함량 변경"; break;
                            case "3": ssView4_Sheet1.Cells[i, 3].Text = "기타 : " + dt.Rows[i]["REMARK3_ETC"].ToString().Trim(); break;
                        }

                        ssView4_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                        ssView4_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_JEP_INFO(string strJEPCODE)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            rtnVal = "◈약품코드:" + strJEPCODE;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUNEXT, HNAME, JEHENG, SNAME, UNIT, REMARK011, REMARK012, REMARK013";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW";
                SQL = SQL + ComNum.VBLF + "     WHERE SUNEXT = '" + strJEPCODE + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = rtnVal + ComNum.VBLF + "◈약품명:" + dt.Rows[0]["HNAME"].ToString().Trim()
                                    + ComNum.VBLF + "◈함량:" + dt.Rows[0]["SNAME"].ToString().Trim() + " " + dt.Rows[0]["UNIT"].ToString().Trim()
                                    + ComNum.VBLF + "◈제형:" + dt.Rows[0]["JEHENG"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
