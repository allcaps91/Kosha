using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmAnUserSearch : Form
    {
        public string SearchGbn = string.Empty;
        public bool IsSave;
        
        public frmAnUserSearch()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAnUserSearch_Load(object sender, EventArgs e)
        {
            //  의사찾기
            if(SearchGbn.Equals("Surgeon"))
            {
                ssMain.ActiveSheet.Rows.Count = 0;
                cboDeptCode.Visible = true;
                label1.Visible = true;

                Init();
            }
            else if (SearchGbn.Equals("Assist"))
            {
                ssMain.ActiveSheet.Rows.Count = 0;
                cboDeptCode.Visible = true;
                label1.Visible = true;

                Init();
            }
            //  마취의 Assist
            else if (SearchGbn.Equals("AN.DR"))
            {
                SetAnDr();
            }
            //  간호사 찾기
            else
            {
                SetAnNr();
            }
        }

        /// <summary>
        /// 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            IsSave = true;
            this.Close();
        }


        private void SetSpreadDisplay(DataTable dt)
        {
            ssMain.ActiveSheet.Rows.Count = 0;
            ssMain.ActiveSheet.Rows.Count = dt.Rows.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssMain.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                ssMain.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
            }
        }

        /// <summary>
        /// 마취의사 조회
        /// </summary>
        private void SetAnDr()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT SABUN, DRNAME AS NAME FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE  DEPTCODE IN ( 'RT','PC')  ";  // '~1;
                SQL = SQL + ComNum.VBLF + "   AND  GBOUT = 'N'   ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY DRCODE  ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                SetSpreadDisplay(dt);

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 간호사 조회
        /// </summary>
        private void SetAnNr()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = SQL + ComNum.VBLF + "SELECT SABUN, KORNAME AS NAME";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_MST A";
                SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_BUSE B";
                SQL = SQL + ComNum.VBLF + "          ON A.BUSE = B.BUCODE";
                SQL = SQL + ComNum.VBLF + " WHERE A.BUSE = '" + SearchGbn + "'";
                SQL = SQL + ComNum.VBLF + "   AND TOIDAY IS NULL";
                if (SearchGbn == "033102")
                {
                    SQL = SQL + ComNum.VBLF + "  AND JIK IN ('32','33','34') ";
                    SQL = SQL + ComNum.VBLF + "  AND JIKJONG = '41' ";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY SABUN ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                SetSpreadDisplay(dt);

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 콤보박스 세팅
        /// </summary>
        private void Init()
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT DEPTCODE, DEPTNAMEK FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE  DEPTCODE  NOT IN ('II','R6','HR','PT', 'TO' ) ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY  PRINTRANKING ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                cboDeptCode.Items.Clear();
                foreach(DataRow row in dt.Rows)
                {
                    cboDeptCode.Items.Add(row["DEPTCODE"].ToString() + "." + row["DEPTNAMEK"].ToString());
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboDeptCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if(cboDeptCode.SelectedIndex < 0)
                {
                    ssMain.ActiveSheet.Rows.Count = 0;
                    return;
                }

                string[] split = cboDeptCode.Text.Split(new string[] { "." }, StringSplitOptions.None);
                string medDept = split[0];

                SQL = "";
                SQL = " SELECT DRCODE AS SABUN, DRNAME AS NAME ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_PMPA.BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE  DRDEPT1 = '" + medDept + "' ";
                SQL = SQL + ComNum.VBLF + " AND    TOUR <> 'Y' ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY PRINTRANKING, DRCODE ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                SetSpreadDisplay(dt);
                
                dt.Dispose();
                dt = null;


                if (SearchGbn.Equals("Assist"))
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT SABUN, KORNAME AS NAME";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_MST A";
                    SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_BUSE B";
                    SQL = SQL + ComNum.VBLF + "          ON A.BUSE = B.BUCODE";
                    SQL = SQL + ComNum.VBLF + " WHERE A.BUSE = '033103'";
                    SQL = SQL + ComNum.VBLF + "   AND TOIDAY IS NULL";
                   
                    SQL = SQL + ComNum.VBLF + " ORDER BY SABUN ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        return;
                    }

                    int j = ssMain.ActiveSheet.Rows.Count;
                    ssMain.ActiveSheet.Rows.Count += dt.Rows.Count;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssMain.ActiveSheet.Cells[i + j, 1].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        ssMain.ActiveSheet.Cells[i + j, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                  
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }
    }
}
