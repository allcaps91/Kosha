using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmJusoSearch : Form
    {
        public frmJusoSearch()
        {
            InitializeComponent();
        }

        private void frmJusoSearch_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //GetData();
            GetDataDt();
        }

        private void GetData()
        {
            DataSet ds = null;
            string strSql = "";
            string SqlErr = "";
            int RowCount = 0;

            ssView_Sheet1.RowCount = 0;

            if (txtJusoSearch.Text.Trim() == "")
            {
                ComFunc.MsgBox("검색란이 공란입니다.", "확인");
                return;
            }
            string strJusoSearch = txtJusoSearch.Text.Trim();

            try
            {
                strSql = "";
                strSql = strSql + ComNum.VBLF + "SELECT ";
                strSql = strSql + ComNum.VBLF + "   ZIPCODE, ";
                if(optSort0.Checked == true && chkZipcode.Checked == false)
                {
                    strSql = strSql + ComNum.VBLF + "   SI_DO || ' ' || SI_GUN_GU || ' ' || EUP_MYEON || ' ' ||  GIL AS ADDRS";
                }
                else if(optSort1.Checked == true && chkZipcode.Checked == false)
                {
                    strSql = strSql + ComNum.VBLF + "   SI_DO || ' ' || SI_GUN_GU  || ' ' || DONG  || ' ' ||  EUP_MYEON  || ' ' || RI AS ADDRS";
                }
                else if(optSort0.Checked == true && chkZipcode.Checked == true)
                {
                    strSql = strSql + ComNum.VBLF + "   SI_DO  || ' ' || SI_GUN_GU  || ' ' || EUP_MYEON  || ' ' || GIL AS ADDRS";
                }
                else if(optSort1.Checked == true && chkZipcode.Checked == true)
                {
                    strSql = strSql + ComNum.VBLF + "   SI_DO || ' ' || SI_GUN_GU  || ' ' || DONG  || ' ' ||  EUP_MYEON  || ' ' || RI AS ADDRS";
                }

                strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ZIPSNEW_2";

                if(optSort0.Checked == true && chkZipcode.Checked == false)
                {
                    strSql = strSql + ComNum.VBLF + "  WHERE GIL LIKE '%" + strJusoSearch + "%'";
                }
                else if(optSort1.Checked == true && chkZipcode.Checked == false)
                {
                    strSql = strSql + ComNum.VBLF + "  WHERE EUP_MYEON LIKE '%" + strJusoSearch + "%' OR RI LIKE '%" + strJusoSearch + "%' OR DONG LIKE '%" + strJusoSearch + "%'";
                }
                else if(chkZipcode.Checked == true)
                {
                    strSql = strSql + ComNum.VBLF + "  WHERE ZIPCODE LIKE '%" + strJusoSearch + "%'";
                }
                strSql = strSql + ComNum.VBLF + "ORDER BY SI_DO, SI_GUN_GU, ZIPCODE";

                SqlErr = clsDB.GetDataSet(ref ds, strSql, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, strSql, clsDB.DbCon); //에러로그 저장
                    return;
                }
                RowCount = ds.Tables[0].Rows.Count;

                if(RowCount == 0)
                {
                    ds.Dispose();
                    ds = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                clsDB.DataSetToSpd(ds, ssView); //DataSet Spread에 DataSource로 매핑
                                                // clsDB.DataSetToSpdRow(ds, ssView, RowCount, 0); //DataSet Spread에 DataSource로 매핑
                ds.Dispose();
                ds = null;
            }
            catch(Exception ex)
            {
                if(ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, strSql, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetDataDt()
        {
            DataTable dt = null;
            string strSql = "";
            string SqlErr = "";
            int RowCount = 0;

            ssView_Sheet1.RowCount = 0;

            if (txtJusoSearch.Text.Trim() == "")
            {
                ComFunc.MsgBox("검색란이 공란입니다.", "확인");
                return;
            }
            string strJusoSearch = txtJusoSearch.Text.Trim();

            try
            {
                strSql = "";
                strSql = strSql + ComNum.VBLF + "SELECT ";
                strSql = strSql + ComNum.VBLF + "   ZIPCODE, ";
                if(optSort0.Checked == true && chkZipcode.Checked == false)
                {
                    strSql = strSql + ComNum.VBLF + "   SI_DO || ' ' || SI_GUN_GU || ' ' || EUP_MYEON || ' ' ||  GIL AS ADDRS";
                }
                else if(optSort1.Checked == true && chkZipcode.Checked == false)
                {
                    strSql = strSql + ComNum.VBLF + "   SI_DO || ' ' || SI_GUN_GU  || ' ' || DONG  || ' ' ||  EUP_MYEON  || ' ' || RI AS ADDRS";
                }
                else if(optSort0.Checked == true && chkZipcode.Checked == true)
                {
                    strSql = strSql + ComNum.VBLF + "   SI_DO  || ' ' || SI_GUN_GU  || ' ' || EUP_MYEON  || ' ' || GIL AS ADDRS";
                }
                else if(optSort1.Checked == true && chkZipcode.Checked == true)
                {
                    strSql = strSql + ComNum.VBLF + "   SI_DO || ' ' || SI_GUN_GU  || ' ' || DONG  || ' ' ||  EUP_MYEON  || ' ' || RI AS ADDRS";
                }

                strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ZIPSNEW_2";

                if(optSort0.Checked == true && chkZipcode.Checked == false)
                {
                    strSql = strSql + ComNum.VBLF + "  WHERE GIL LIKE '%" + strJusoSearch + "%'";
                }
                else if(optSort1.Checked == true && chkZipcode.Checked == false)
                {
                    strSql = strSql + ComNum.VBLF + "  WHERE EUP_MYEON LIKE '%" + strJusoSearch + "%' OR RI LIKE '%" + strJusoSearch + "%' OR DONG LIKE '%" + strJusoSearch + "%'";
                }
                else if(chkZipcode.Checked == true)
                {
                    strSql = strSql + ComNum.VBLF + "  WHERE ZIPCODE LIKE '%" + strJusoSearch + "%'";
                }
                strSql = strSql + ComNum.VBLF + "ORDER BY SI_DO, SI_GUN_GU, ZIPCODE";

                SqlErr = clsDB.GetDataTable(ref dt, strSql, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, strSql, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                //DataTable의 data를 Spread에 DataSource로 매핑
                ssView_Sheet1.Rows.Count = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ZIPCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ADDRS"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, strSql, clsDB.DbCon); //에러로그 저장
            }
        }

        private void txtJusoSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;

            GetData();
        }

        private void frmJusoSearch_Activated(object sender, EventArgs e)
        {
            txtJusoSearch.Focus();
        }
    }
}
