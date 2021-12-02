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
    /// File Name       : frmPmpaViewDrugNoView.cs
    /// Description     : 투약번호 조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-04
    /// Update History  : 2017-11-03
    /// <history>       
    /// d:\psmh\OPD\oumsad\oumsad32.frm(FrmDrugNoView) => frmPmpaViewDrugNoView.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\oumsad32.frm(FrmDrugNoView)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewDrugNoView : Form
    {
        public frmPmpaViewDrugNoView()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);

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

            txtDrugPano.Focus();            
            ssList_Sheet1.Rows.Count = 0;

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void txtDrugPano_Enter(object sender, EventArgs e)
        {
            TextBox tP = sender as TextBox;
            tP.SelectionStart = 0;
            tP.SelectionLength = tP.Text.Length;
        }

        void txtDrugPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                {
                    txtDrugPano.SelectAll();
                }
            }
        }

        void txtDrugPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
            {
                return;
            }

            else if (e.KeyChar == 13)
            {
                string strPano = "";
                string strSeqNo = "";
                string strSname = "";
                string strGwa = "";

                string SQL = "";
                string SqlErr = "";
                DataTable dt = null;

                int i = 0;
                int nSeqNo = 0;

                if (txtDrugPano.Text == "" || txtDrugPano.Text == null)
                {
                    return;
                }

                strPano = ComFunc.SetAutoZero(txtDrugPano.Text, 8);
                txtDrugPano.Text = strPano;

                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                    ";
                SQL += ComNum.VBLF + "  TUNO,  Sname, DeptCode, SLIPGBN                                         ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_TUYAK                                      ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
                SQL += ComNum.VBLF + "      AND Tudate = TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD')  ";
                SQL += ComNum.VBLF + "      AND Pano = '" + strPano + "'                                        ";

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
                        ssList_Sheet1.Rows.Count = dt.Rows.Count;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            nSeqNo = Convert.ToInt32(String.Format("{0:####}", VB.Val(dt.Rows[i]["TuNO"].ToString().Trim())));
                            strSname = dt.Rows[i]["Sname"].ToString().Trim();
                            strGwa = dt.Rows[i]["DeptCode"].ToString().Trim();

                            if (dt.Rows[i]["SlipGbn"].ToString().Trim() == "3")
                            {
                                strGwa += "   (원외)";
                            }
                            else
                            {
                                strGwa += "   (원내)";
                            }

                            ssList_Sheet1.Cells[i, 0].Text = nSeqNo.ToString();
                            ssList_Sheet1.Cells[i, 1].Text = strSname;
                            ssList_Sheet1.Cells[i, 2].Text = strGwa;

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
        }
    }
}
