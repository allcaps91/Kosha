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
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmCodehelp.cs
    /// Description     : 코드 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-09
    /// Update History  : 
    /// <history>       
    /// optJong 컨트롤 존재안함, 코드는 구현해놓음, 실제 데이터 테스트 필요, GstrHelpCode를 받아오는 생성자 추가, GstrHelpName을 전달하는 델리게이트 추가
    /// D:\타병원\PSMHH\OPD\oumsad\oumsad26.frm(FrmCodeHelp) => FrmCodehelp.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\OPD\oumsad\oumsad26.frm(FrmCodeHelp)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\OPD\oumsad\oumsad.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class FrmCodehelp : Form
    {
        //이벤트를 전달할 경우
        public delegate void SetHelpName(string strHelpName); //Old : GstrHelpName
        public event SetHelpName rSetHelpName;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        string strNewTable = "";
        string strOldTable = "";

        string mstrHelpCode = "";

        public FrmCodehelp()
        {
            InitializeComponent();
        }

        public FrmCodehelp(string strHelpCode)
        {
            InitializeComponent();
            mstrHelpCode = strHelpCode;
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void FrmCodehelp_Activated(object sender, EventArgs e)
        {
            strNewTable = mstrHelpCode;
            mstrHelpCode = "";
            txtData.Text = "";
            lblMsg.Text = "";

            if (strOldTable == strNewTable)
            {
                txtData.Focus();
                return;
            }

            switch (strNewTable)
            {
                case "MIA":
                    this.Text = "조합코드 찾기";
                    grbJong.Visible = true;
                    grbBun.Visible = false;
                    optGbn0.Text = "조합명칭";
                    optGbn1.Text = "조합코드";
                    optGbn2.Visible = false;
                    break;

                case "ILLS":
                    this.Text = "상병코드 찾기";
                    grbBun.Visible = false;
                    grbJong.Visible = false;
                    optGbn0.Text = "한글명칭";
                    optGbn1.Text = "영문명칭";
                    optGbn2.Visible = true;
                    optGbn2.Text = "상병코드";
                    break;

                case "DOSAGE":
                    this.Text = "용법코드 조회";
                    grbJong.Visible = false;
                    grbBun.Visible = false;
                    optGbn0.Text = "DIV#";
                    optGbn1.Text = "용법코드";
                    optGbn2.Visible = false;
                    break;

                case "MAIL":
                    this.Text = "우편번호,지역코드 찾기";
                    grbBun.Visible = false;
                    grbJong.Visible = false; 
                    optGbn0.Text = "동명칭";
                    optGbn1.Text = "주소";
                    optGbn2.Visible = false;
                    break;

                case "SUGA":
                    this.Text = "수가코드 조회";
                    grbBun.Visible = true;
                    grbJong.Visible = false;
                    optGbn0.Text = "행위료";
                    optGbn1.Text = "재료대";
                    optGbn2.Visible = true;
                    optGbn2.Text = "전체";
                    break;

                default:
                    this.Text = "각종 코드 찾기";
                    break;
            }

            strOldTable = strNewTable;

            txtData.Focus();
        }

        void FrmCodehelp_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            optBun5.Checked = true;
            optGbn0.Checked = true;
            optJong5.Checked = true;
        }

        void HelpData_List_SET()
        {
            int i = 0;

            string strData = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssSuga_Sheet1.RowCount = 0;

            mstrHelpCode = "";

            switch (strNewTable)
            {
                case "MIA":
                    // GoSub BAS_MIA_HelpSelect
                    // 조합기호를 READ

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "MiaCode xViewCode1,MiaClass xViewCode2,MiaName xViewName ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MIA";
                    if (optBun0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "WHERE MiaName LIKE '%" + txtData.Text.Trim() + "%'";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "WHERE MiaCode LIKE '%" + txtData.Text.Trim() + "%'";
                    }

                    //환자종류별
                    if(optJong0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND MiaClass = '11' ";
                    }
                    else if(optJong1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND MiaClass = '13' ";
                    }
                    else if(optJong2.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND MiaClass = '12' ";
                    }
                    else if(optJong3.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND MiaClass = '20' ";
                    }
                    else if(optJong4.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND MiaClass >= '90' ";
                    }

                    SQL += ComNum.VBLF + "  AND DelDate IS NULL ";
                    SQL += ComNum.VBLF + "ORDER BY MiaCode ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    HelpSelect_Display(SqlErr, SQL, dt);

                    dt.Dispose();
                    dt = null;

                    break;

                case "ILLS":
                    // BAS_ILLS_HelpSelect
                    // 상병코드를 SELECT
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "IllCode xViewCode1,IllClass xViewCode2,";
                    if (optGbn1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "IllNameE xViewName ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "IllNameK xViewName ";
                    }
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                    if (optGbn0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "WHERE IllNameK LIKE '%" + txtData.Text.Trim() + "%'";
                    }
                    else if (optGbn1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "WHERE IllNameE LIKE '%" + txtData.Text.Trim() + "%'";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "WHERE IllCode LIKE '%" + txtData.Text.Trim() + "%'";
                    }
                    SQL += ComNum.VBLF + "ORDER BY ILLCode ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    HelpSelect_Display(SqlErr, SQL, dt);

                    dt.Dispose();
                    dt = null;

                    break;

                case "MAIL":
                    // BAS_MAIL_HelpSelect
                    // 우편번호,지역코드 Select
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "MailCode xViewCode1,MailJiyek xViewCode2,MailJuso xViewName ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MAILNEW";
                    if (optGbn0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "WHERE MailDong LIKE '%" + txtData.Text.Trim() + "%'";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "WHERE MailJuso LIKE '%" + txtData.Text.Trim() + "%'";
                    }
                    SQL += ComNum.VBLF + "ORDER BY MailCode ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    HelpSelect_Display(SqlErr, SQL, dt);

                    dt.Dispose();
                    dt = null;

                    break;

                case "DOSAGE":
                    // BAS_Dosage_HelpSelect
                    // 용법코드

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "DosageCode xViewCode1,DosageDiv xViewCode2, ";
                    SQL += ComNum.VBLF + "DosageName1 xViewName ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOSAGE1";
                    SQL += ComNum.VBLF + "WHERE DosageDiv > 0";
                    if (optGbn0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "AND DosageDiv = " + txtData.Text.Trim() + " ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "AND DosageCode LIKE '" + txtData.Text.Trim() + "%' ";
                    }
                    SQL += ComNum.VBLF + "ORDER BY DosageDiv,DosageCode ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    HelpSelect_Display(SqlErr, SQL, dt);

                    dt.Dispose();
                    dt = null;

                    break;

                case "SUGA":
                    // BAS_SUGA_HelpSelect
                    // 수가코드를 SELECT

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "SuCode xViewCode1,SuNext xViewCode2, ";
                    SQL += ComNum.VBLF + "Bun xViewBun,SuNameK xViewName ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "VIEW_SUGA_CODE";
                    // 분류별
                    if (optBun0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "WHERE BUN <= '10'";
                    }

                    else if (optBun1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "WHERE BUN >= '11' AND BUN <= '21' ";
                    }

                    else if (optBun2.Checked == true)
                    {
                        SQL += ComNum.VBLF + "WHERE BUN >= '22' AND BUN <= '40' ";
                    }

                    else if (optBun3.Checked == true)
                    {
                        SQL += ComNum.VBLF + "WHERE Bun >= '41' AND Bun <= '63' ";
                    }

                    else if (optBun4.Checked == true)
                    {
                        SQL += ComNum.VBLF + "WHERE Bun >= '65' AND Bun <= '73' ";
                    }

                    else
                    {
                        SQL += ComNum.VBLF + "WHERE Bun >= '74' ";
                    }

                    // 행위, 재료구분
                    if (optGbn0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND SugbE = '1'  ";
                    }
                    else if (optGbn1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND SugbE = '0'  ";
                    }

                    // 명칭으로 찾기
                    if (txtData.Text != "")
                    {
                        SQL += ComNum.VBLF + "  AND SuNameK LIKE '%" + txtData.Text + "%'  ";
                    }
                    SQL += ComNum.VBLF + "  AND DelDate IS NULL  ";
                    SQL += ComNum.VBLF + "ORDER BY SuCode,SuNext ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    ssSuga_Sheet1.RowCount = dt.Rows.Count;
                    for (i = 0; i < ssSuga_Sheet1.RowCount; i++)
                    {
                        ssSuga_Sheet1.Cells[i, 0].Text = dt.Rows[i]["xViewCode1"].ToString().Trim();
                        ssSuga_Sheet1.Cells[i, 1].Text = dt.Rows[i]["xViewCode2"].ToString().Trim();
                        ssSuga_Sheet1.Cells[i, 2].Text = dt.Rows[i]["xViewBun"].ToString().Trim();
                        ssSuga_Sheet1.Cells[i, 3].Text = dt.Rows[i]["xViewName"].ToString().Trim();

                    }
                    dt.Dispose();
                    dt = null;

                    break;
            }

            if (ssSuga_Sheet1.RowCount > 0)
            {
                lblMsg.Text = "원하시는 코드를 더블클릭 하세요.";
            }

        }

        void HelpSelect_Display(string SqlErr, string SQL, DataTable dt)
        {
            int i = 0;

            ssSuga_Sheet1.RowCount = dt.Rows.Count;

            for (i = 0; i < ssSuga_Sheet1.RowCount; i++)
            {
                ssSuga_Sheet1.Cells[i, 0].Text = dt.Rows[i]["xViewCode1"].ToString().Trim();
                ssSuga_Sheet1.Cells[i, 1].Text = dt.Rows[i]["xViewCode2"].ToString().Trim();
                ssSuga_Sheet1.Cells[i, 2].Text = dt.Rows[i]["xViewName"].ToString().Trim();
            }

            if (strNewTable == "MAIL" && dt.Rows.Count == 1)
            {
                mstrHelpCode = VB.Left(dt.Rows[0]["xViewCode1"].ToString().Trim() + VB.Space(6), 6) + dt.Rows[0]["xViewCode2"].ToString().Trim();
                rSetHelpName(dt.Rows[0]["xViewName"].ToString().Trim());
                this.Hide();
                return;
            }

            dt.Dispose();
            dt = null;
        }

        void ssSuga_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            // TODO 
            // VB에서는 리스트로 구현되어있으며, 어떤값이 들어가는지 확인하고 구현하려고 했지만 실행위치를 찾을수가 없었으며
            // 실제 테스트가 필요함
            // 본소스는 아래에 첨부
            switch (strNewTable)
            {
                case "MIA":
                    if (e.Column == 0 || e.Column == 1)
                    {
                        mstrHelpCode = ssSuga_Sheet1.Cells[e.Row, e.Column].Text;
                    }
                    if (e.Column == 2)
                    {
                        rSetHelpName(ssSuga_Sheet1.Cells[e.Row, e.Column].Text);
                    }
                    else
                    {
                        return;
                    }
                    break;

                case "MAIL":
                    if (e.Column == 0 || e.Column == 1)
                    {
                        mstrHelpCode = ssSuga_Sheet1.Cells[e.Row, e.Column].Text;
                    }
                    if (e.Column == 2)
                    {
                        rSetHelpName(ssSuga_Sheet1.Cells[e.Row, e.Column].Text);
                    }
                    else
                    {
                        return;
                    }
                    break;
            }
            this.Hide();
        }

        void ssSuga_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
            {
                return;
            }

            this.Hide();
        }

        void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
            {
                return;
            }

            txtData.Text = txtData.Text.Trim();

            if (strNewTable == "MIA")
            {
                if (optGbn0.Checked == true && VB.Len(txtData.Text) < 2)
                {
                    ComFunc.MsgBox("명칭으로 조회하실 자료의 길이가 짧음");
                    txtData.Focus();
                    return;
                }
            }

            HelpData_List_SET();
        }

        private void btnView_Click(object sender, EventArgs e)
        {

        }
    }
}
