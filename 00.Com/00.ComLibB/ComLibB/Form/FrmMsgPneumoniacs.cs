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
    public partial class FrmMsgPneumoniacs : Form
    {
        int rowcounter;
        string SQL;
        DataTable dt = null;
        DataTable dt1 = null;
        string SqlErr = ""; //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        public FrmMsgPneumoniacs()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNot_Click(object sender, EventArgs e)
        {
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {   
                SQL = "";
                SQL += " UPDATE ADMIN.IPD_NEW_MASTER SET                  \r";
                SQL += "        PNEUMONIA = 'N'                                 \r";
                SQL += "  WHERE PANO = '" + clsOrdFunction.Pat.PtNo + "'        \r";
                SQL += "    AND IPDNO = " + clsOrdFunction.Pat.IPDNO + "        \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                this.Close();
                
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
