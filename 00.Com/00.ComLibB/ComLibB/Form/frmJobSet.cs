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

namespace ComLibB
{
    /// <summary> 작업환경 설정 </summary>
    public partial class frmJobSet : Form
    {
        int GnPrtOnOff = 0; //global 1.인쇄함 0.인쇄않함
        int GnCancelFlag = 0; //global 1.표시함 0.표시안함
        int GnSaveFlag = 1; //global 1.일자별 2.등록번호별 3.이동않함
        int GnJobSabun = 0; //global


        /// <summary> 작업환경 설정 </summary>
        public frmJobSet()
        {
            InitializeComponent();
        }

        void frmJobSet_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            if (GnPrtOnOff == 1)
            {
                optPrt0.Checked = true;
            }

            if (GnPrtOnOff == 0)
            {
                optPrt1.Checked = true;
            }

            if (GnCancelFlag == 1)
            {
                optCancel0.Checked = true;
            }

            if (GnCancelFlag == 0)
            {
                optCancel1.Checked = true;
            }

            if (GnSaveFlag == 1)
            {
                optSave0.Checked = true;
            }

            if (GnSaveFlag == 2)
            {
                optSave1.Checked = true;
            }

            if (GnSaveFlag == 3)
            {
                optSave2.Checked = true;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strROWID = string.Empty;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (optPrt0.Checked == true)
                {
                    GnPrtOnOff = 1;
                }

                if (optPrt1.Checked == true)
                {
                    GnPrtOnOff = 0;
                }

                if (optCancel0.Checked == true)
                {
                    GnCancelFlag = 1;
                }

                if (optCancel1.Checked == true)
                {
                    GnCancelFlag = 0;
                }

                if (optSave0.Checked == true)
                {
                    GnSaveFlag = 1;
                }

                if (optSave1.Checked == true)
                {
                    GnSaveFlag = 2;
                }

                if (optSave2.Checked == true)
                {
                    GnSaveFlag = 3;
                }

                SQL = "";
                SQL = "SELECT ROWID FROM " +ComNum.DB_PMPA + "ETC_PCSET ";
                SQL = SQL + ComNum.VBLF + "WHERE SABUN=" + GnJobSabun + " ";

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

                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                dt.Dispose();
                dt = null;

                if (strROWID != "")
                {
                    if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                    {
                        return; //권한 확인
                    }

                    SQL = "";
                    SQL = "UPDATE ETC_PCSET SET READ_FLAG1='" + GnPrtOnOff + "',";
                    SQL = SQL + ComNum.VBLF + " READ_FLAG2='" + GnCancelFlag + "',";
                    SQL = SQL + ComNum.VBLF + " READ_FLAG3='" + GnSaveFlag + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + strROWID + "' ";
                }
                else
                {
                    if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                    {
                        return; //권한 확인
                    }

                    SQL = "";
                    SQL = "INSERT INTO ETC_PCSET (Sabun,READ_Flag1,READ_Flag2,READ_Flag3) ";
                    SQL = SQL + ComNum.VBLF + "VALUES (" + GnJobSabun + ",'" + GnPrtOnOff + "','";
                    SQL = SQL + ComNum.VBLF + GnCancelFlag + "','" + GnSaveFlag + "') ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                this.Close();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
