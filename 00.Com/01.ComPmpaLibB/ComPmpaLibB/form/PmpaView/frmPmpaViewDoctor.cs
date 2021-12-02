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
    /// File Name       : frmPmpaViewDoctor.cs
    /// Description     : 의사정보 조회
    /// Author          : 안정수
    /// Create Date     : 2017-08-14
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\oumsad\FrmDoctor.frm(FrmDoctor) => frmPmpaViewDoctor.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\FrmDoctor.frm(FrmDoctor)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewDoctor : Form
    {
        ComFunc CF = new ComFunc();
        public frmPmpaViewDoctor()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
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

            ssList_Sheet1.Rows.Count = 0;

            txtDrName.Text = "";
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                eGetData();
            }
        }

        void eGetData()
        {
            int i = 0;
            int nRow = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (txtDrName.Text == "")
            {
                return;
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                     ";
            SQL += ComNum.VBLF + "   a.DrCode, a.DrDept1, a.DrName, a.GbChoice               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR a                    ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                  ";
            SQL += ComNum.VBLF + "       AND a.DrName like '%" + txtDrName.Text + "%'        ";
            SQL += ComNum.VBLF + "       AND a.TOUR <> 'Y'                                   ";
            SQL += ComNum.VBLF + "ORDER By a.DrName                                          ";

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
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DrDept1"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = CF.READ_OCS_Doctor3_DrBunho(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GbChoice"].ToString().Trim();
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
        }

        void txtDrName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                eGetData();
            }
        }
    }
}
