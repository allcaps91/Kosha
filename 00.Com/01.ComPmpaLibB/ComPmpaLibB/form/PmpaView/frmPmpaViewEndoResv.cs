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
    /// File Name       : frmPmpaViewEndoResv.cs
    /// Description     : 의료급여 내시경 예약 환자
    /// Author          : 안정수
    /// Create Date     : 2017-08-25
    /// Update History  : 2017-11-02
    /// <history>       
    /// d:\psmh\OPD\oumsad\Frm내시경예약환자.frm(Frm내시경예약환자.frm) => frmPmpaViewEndoResv.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\Frm내시경예약환자.frm(Frm내시경예약환자.frm)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewEndoResv : Form
    {
        ComFunc CF = new ComFunc();
        public frmPmpaViewEndoResv()
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

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
        }

        void eGetData()
        {
            int i = 0;
            int nREAD = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  PTNO, TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,                        ";
            SQL += ComNum.VBLF + "  DEPTCODE, DRCODE                                                ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "ENDO_JUPMST                             ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
            SQL += ComNum.VBLF + "      AND RDATE = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')    ";
            SQL += ComNum.VBLF + "      AND ORDERCODE IN ('GI1','GI1A','GI2','GI2A','GI3','GI3A')   ";
            SQL += ComNum.VBLF + "      AND RESULTDATE IS NULL                                      ";
            SQL += ComNum.VBLF + "ORDER BY PTNO                                                     ";

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
                    nREAD = dt.Rows.Count;
                    ssList_Sheet1.Rows.Count = nREAD;

                    for (i = 0; i < nREAD; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = CF.Read_Patient(clsDB.DbCon, dt.Rows[i]["PTNO"].ToString().Trim(), "2");
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
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
