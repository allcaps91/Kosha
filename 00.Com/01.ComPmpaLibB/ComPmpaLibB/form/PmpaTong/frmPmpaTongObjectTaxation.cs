using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    /// <summary>
    /// \OPD\olrepa\olrepa.vbp
    /// \OPD\olrepa\Frm세무용감액내역.frm
    /// </summary>
    public partial class frmPmpaTongObjectTaxation : Form
    {
        public frmPmpaTongObjectTaxation()
        {
            InitializeComponent();
        }

        private void frmPmpaTongObjectTaxation_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            txtPart.Text = clsType.User.Sabun;

            dtpDate1.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

            btnPrint.Enabled = false;
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            SS1PrintBuild();
        }

        void SS1PrintBuild()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strSabun = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " select A.pano,sum(amt) amt,B.JUMIN1,B.JUMIN3,B.SNAME,ADMIN.FC_BAS_GAMF_MESSAGE(A.pano) MESSAGE,ADMIN.FC_BAS_GAMF_SABUN(A.pano) SABUN, ";
                SQL += ComNum.VBLF + "       ADMIN.FC_BAS_BUCODE_DEPTNAMEK(SUBSTR(ADMIN.FC_BAS_GAMF_SABUN(A.pano),2,6)) DEPT ,gubun,decode(trim(sunext),'Y92I','감액[직원본인]','Y92J','직원[직원배우자]','Y92K','감액(직원직계,존비속]','Y92L','감액[배우자부모,형제자매]')  sunext  from (  ";
                SQL += ComNum.VBLF + "       select pano,amt ,'I' gubun,sunext from    ipd_new_cash where sunext  IN ('Y92I','Y92J','Y92K','Y92L') ";
                SQL += ComNum.VBLF + "       and actdate >=to_date('2015-01-01','yyyy-mm-dd') and  actdate <=to_date('2015-12-31','yyyy-mm-dd')  ";
                SQL += ComNum.VBLF + "       union all";
                SQL += ComNum.VBLF + "  select pano, (amt1+amt2) amt,'O' gubun,sunext from    opd_slip where sunext  IN ('Y92I','Y92J','Y92K','Y92L')";
                SQL += ComNum.VBLF + " and actdate >=to_date('2015-01-01','yyyy-mm-dd') and  actdate <=to_date('2015-12-31','yyyy-mm-dd' ) ) A ,BAS_PATIENT B WHERE A.PANO =B.PANO group by A.pano,B.JUMIN1,B.JUMIN3,B.SNAME,gubun,sunext order by gubun,pano";

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


                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ss1_Sheet1.RowCount = dt.Rows.Count;

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();

                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dt.Rows[i]["JUMIN3"].ToString().Trim());

                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AMT"].ToString().Trim();

                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["MESSAGE"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 4].Text = ss1_Sheet1.Cells[i, 4].Text.Trim().Length == 0 ? READ_Gamek_info(dt.Rows[i]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dt.Rows[i]["JUMIN3"].ToString().Trim())) : "";

                    strSabun = ss1_Sheet1.Cells[i, 4].Text.Trim();

                    ss1_Sheet1.Cells[i, 6].Text = READ_Gamek_buse(VB.Right(strSabun, 5));
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["gubun"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["sunext"].ToString().Trim();
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

        string READ_Gamek_info(string ArgJumin)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT b.GamMessage ";
                SQL += ComNum.VBLF + " FROM ADMIN.INSA_MSTB a, ADMIN.BAS_GAMF b ";
                SQL += ComNum.VBLF + "  WHERE a.JUMIN3 = b.GAMJUMIN3 ";
                SQL += ComNum.VBLF + "   AND a.JUMIN3 ='" + clsAES.AES(ArgJumin) + "' ";
                SQL += ComNum.VBLF + "   AND rownum=1 union all ";
                SQL += ComNum.VBLF + " SELECT '퇴사' GamMessage  ";
                SQL += ComNum.VBLF + "  FROM ADMIN.insa_mst ";
                SQL += ComNum.VBLF + " WHERE Jumin3  = '" + clsAES.AES(ArgJumin) + "'  ";//   '2013-02-20
                SQL += ComNum.VBLF + "   AND SABUN <'60000' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                rtnVal = dt.Rows[0]["GamMessage"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        string READ_Gamek_buse(string Argsabun)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT ADMIN.FC_BAS_BUCODE_DEPTNAMEK( '" + Argsabun +"') buname     from dual";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                rtnVal = dt.Rows[0]["buname"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ss1_Sheet1.Protect = false;
            ss1.SaveExcel(@"C:\CMC" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "_" + ComQuery.CurrentDateTime(clsDB.DbCon, "T") + ".xls", 
                    FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            ss1_Sheet1.Protect = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtPart_Enter(object sender, EventArgs e)
        {
            txtPart.SelectionStart = 0;
            txtPart.SelectionLength = txtPart.Text.Length;
        }

        private void txtPart_Leave(object sender, EventArgs e)
        {
            txtPart.Text = txtPart.Text.ToUpper();
        }
    }
}
