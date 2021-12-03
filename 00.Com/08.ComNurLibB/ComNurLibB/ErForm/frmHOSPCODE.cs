using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedErNr
    /// File Name       : frmHOSPCODE.cs
    /// Description     : 병원코드조회
    /// Author          : 이현종
    /// Create Date     : 2018-04-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrer\frmHOSPCODE.frm(frmHOSPCODE.frm) >> frmHOSPCODE.cs 폼이름 재정의" />
    public partial class frmHOSPCODE : Form
    {        
        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        //Messgae Send
        public delegate void SendMsg(string strHospCode, string strHospName, string strHospGubn);
        public event SendMsg rSendMsg;

        public frmHOSPCODE()
        {
            InitializeComponent();
        }

        private void frmHOSPCODE_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            this.StartPosition = FormStartPosition.CenterParent;

            ss1_Sheet1.RowCount = 1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //GetSearchData();
        }

        void GetSearchData(bool isName)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";

            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT HOSPNUMB, HOSPNAME, HOSPGUBN, HOSPADDR ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_HOSPITAL ";
                if(isName == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE HOSPNAME LIKE '%" + txtHOSPNAME.Text.Trim() + "%' ";
                    SQL = SQL + ComNum.VBLF + "  AND trim(HOSPCODE) not in (select trim(HOSPCODE) from bas_hospital_delete) ";     //'삭제된 테이블은 따로 관리를 해야 추가될때 용이함으로 테이블 만듬 bas_hospital_delete
                    SQL = SQL + ComNum.VBLF + " AND HOSPNUMB IS NOT NULL";
                    SQL = SQL + ComNum.VBLF + " order by decode(trim(hospzips),'370019',1,'370012',1,'370701',1,'370702',1,'370100',1,2) ,hospdate desc,HOSPNAME ASC    ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE HOSPNUMB LIKE '%" + txtHOSPCODE.Text.Trim() + "%' ";
                    SQL = SQL + ComNum.VBLF + "  AND trim(HOSPCODE) not in (select trim(HOSPCODE) from bas_hospital_delete) ";// '삭제된 테이블은 따로 관리를 해야 추가될때 용이함으로 테이블 만듬 bas_hospital_delete
                    SQL = SQL + ComNum.VBLF + " order by decode(trim(hospzips),'370019',1,'370012',1,'370701',1,'370702',1,'370100',1,2) ,hospdate desc,HOSPNAME ASC    ";
                }

                SQL = SQL + ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
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

                ss1_Sheet1.RowCount = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["HOSPNUMB"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["HOSPNAME"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["HOSPGUBN"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["HOSPADDR"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtHOSPNAME_Leave(object sender, EventArgs e)
        {
            if (txtHOSPNAME.Text.Trim() == "") return;

            GetSearchData(true);
        }

        private void txtHOSPCODE_Leave(object sender, EventArgs e)
        {
            if (txtHOSPCODE.Text.Trim() == "") return;

            GetSearchData(false);
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0) return;

            string strHospCode = ss1_Sheet1.Cells[e.Row, 0].Text;
            string strHospName = ss1_Sheet1.Cells[e.Row, 1].Text;
            string strHospGubn = ss1_Sheet1.Cells[e.Row, 2].Text;

            if (strHospCode == "") return;
            if (strHospName == "") return;
            if (strHospGubn == "") return;
            
            rSendMsg(strHospCode, strHospName, strHospGubn);
            rEventClosed();
        }

        private void txtHOSPNAME_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetSearchData(true);
            }
        }

        private void txtHOSPCODE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetSearchData(false);
            }
        }
    }
}
