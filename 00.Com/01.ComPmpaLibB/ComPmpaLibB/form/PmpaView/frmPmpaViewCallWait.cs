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
    /// Class Name      : SupEndsComPmpaLibB
    /// File Name       : frmPmpaViewCallWait.cs
    /// Description     : 순번 대기 명단
    /// Author          : 안정수
    /// Create Date     : 2017-09-20
    /// Update History  : 2017-11-06
    /// 실제 테스트 필요
    /// <history>       
    /// d:\psmh\OPD\oumsad\FrmCallWait.frm(FrmCallWait.frm) => frmSupEndsSTS03frmPmpaViewCallWait.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\FrmCallWait.frm(FrmCallWait.frm)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewCallWait : Form
    {
        bool FnCall = false;
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        public frmPmpaViewCallWait()
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
            CS.Spread_All_Clear(ssList);
            ssList_Sheet1.Rows.Count = 0;

            clsCall.GstrRetTKNo = "";
            clsCall.GstrJumin1_WaitCall = "";
            clsCall.GstrJumin2_WaitCall = "";
            clsCall.GstrPano_Call = "";

            FnCall = false;

            eGetData();
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
        }

        void eGetData()
        {
            int i = 0;
            string strJumin = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            CS.Spread_All_Clear(ssList);
            ssList_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                            ";
            SQL += ComNum.VBLF + "  TKNO,CUSTID,CUSTNM,JUMIN_NO                     ";
            SQL += ComNum.VBLF + "FROM SMC.TBTICKETHST                              ";
            SQL += ComNum.VBLF + "WHERE 1=1                                         ";
            SQL += ComNum.VBLF + "      AND TKTM >= TRUNC(SYSDATE)                  ";
            SQL += ComNum.VBLF + "      AND ZONEID ='" + clsCall.GstrZoneID + "'    ";

            if (FnCall == false)
            {
                SQL += ComNum.VBLF + "  AND (STANBYTM IS NULL OR STANBYTM = '')     ";
                SQL += ComNum.VBLF + "ORDER By TKTM                                 ";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND STANBYTM IS NOT NULL                    ";
                SQL += ComNum.VBLF + "ORDER By TKTM DESC                            ";
            }

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                        strJumin = dt.Rows[i]["JUMIN_NO"].ToString().Trim();

                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["TKNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CUSTNM"].ToString().Trim();

                        if (strJumin != "")
                        {
                            ssList_Sheet1.Cells[i, 2].Text = VB.Left(strJumin, 6) + "-" + clsAES.DeAES(VB.Mid(strJumin, 7, (strJumin.Length) - 6));
                        }
                        else
                        {
                            ssList_Sheet1.Cells[i, 2].Text = "";
                        }

                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["CUSTID"].ToString().Trim();
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

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (clsCall.GnCallCnt > 0)
            {
                return;
            }

            if (e.Column < 0 || e.Row < 0)
            {
                return;
            }

            clsCall.GstrRetTKNo = ssList_Sheet1.Cells[e.Row, 0].Text;
            clsCall.GstrJumin1_WaitCall = VB.Pstr(ssList_Sheet1.Cells[e.Row, 2].Text, "-", 1);
            clsCall.GstrJumin2_WaitCall = VB.Pstr(ssList_Sheet1.Cells[e.Row, 2].Text, "-", 2);
            clsCall.GstrPano_Call = ssList_Sheet1.Cells[e.Row, 3].Text;

            this.Close();
        }

        private void chkCall_Click(object sender, EventArgs e)
        {
            if (chkCall.Checked == true)
                FnCall = true;
            else
                FnCall = false;

            eGetData();

        }

    }
}
