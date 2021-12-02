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
    public partial class frmJemsuSuga2 : Form
    {

        /// Class Name      : frmJemsuSuga2
        /// File Name       : frmJemsuSuga2.cs
        /// Description     : 분기별 신고 약가 수가 일괄변경
        /// Author          : 최익준
        /// Create Date     : 2017-09-08
        /// Update History  : 
        /// </summary>
        /// <vbp>           : PSMH\basic\busuga\BuSuga46.frm
        /// default
        /// </vbp>
        /// 
        public frmJemsuSuga2()
        {
            InitializeComponent();
        }
        string strMsgList = "";
        public frmJemsuSuga2(string strMsgList)
        {
            string GstrMsgList = strMsgList;
            InitializeComponent();
        }

        int GnSS2Row = 0;


        private void frmJemsuSuga2_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "SELECT BUNGI";
                SQL = SQL + ComNum.VBLF + "FROM EDI_YAKGUIP";
                SQL = SQL + ComNum.VBLF + "WHERE GBN = ' '";
                SQL = SQL + ComNum.VBLF + "AND SDATE >=TO_DATE('2011-01-01','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "GROUP BY BUNGI";
                SQL = SQL + ComNum.VBLF + "ORDER BY BUNGI DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                cboBun.SelectedIndex = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboBun.Items.Add(dt.Rows[i]["BUNGI"] + "");
                }

                dt.Dispose();
                dt = null;

                cboBun.SelectedIndex = 0;

                ssSuga_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                lblJob.Text = "";

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;
            int nRow = 0;
            int nREAD = 0;
            int nJobCNT = 0;
            string strBCode = "";
            string strBunGi = "";
            string strYear = "";
            string strJDate = "";
            string strGDate = "";       //구입일자
            string strOK = "";
            double dblPrice = 0;        //구입신고 평균단가
            double dblBPrice = 0;       //표준코드 단가
            double dblSAmt = 0;         //약제상한단가 (표준코드 환산 계산단가)

            lblJob.Text = "";

            strBunGi = cboBun.Text;
            strYear = VB.Left(cboBun.Text, 4);

            //clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);
            Cursor.Current = Cursors.WaitCursor;

            switch (Convert.ToInt32(VB.Mid(strBunGi, 5, 2)))
            {
                case 14:
                    strJDate = strYear + "-05-01";
                    break;

                case 24:
                    strJDate = strYear + "-08-01";
                    break;

                case 34:
                    strJDate = strYear + "-11-01";
                    break;

                case 44:
                    strJDate = strYear + "-02-01";
                    break;

            }

            strMsgList = strBunGi + "분기별 수가 적용일자가 정말로 " + strJDate + "일이" + ComNum.VBLF;
            strMsgList = strMsgList + "맞습니까? 만일 오류날짜를 지정하면" + ComNum.VBLF;
            strMsgList = strMsgList + "다시 복구가 불가능 합니다.";
            if (ComFunc.MsgBoxQ(strMsgList, "날짜확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            btnStart.Enabled = false;
            btnCancle.Enabled = false;

            strOK = "OK";
            ssSuga_Sheet1.RowCount = 50;

            //점수가 0인것은 처리 안함
            //적용일자1이 수가적용일과 동리한것만 작업을 함.

            SQL = " SELECT TO_DATE(GDATE ,'YYYY-MM-DD') GDATE, TO_DATE(SDATE ,'YYYY-MM-DD') SDATE,  BCODE, AVGAMT  ";
            SQL = SQL + ComNum.VBLF + "   FROM ";
            SQL = SQL + ComNum.VBLF + "   EDI_YAKGUIP";
            SQL = SQL + ComNum.VBLF + "          WHERE GBN = ' '";
            SQL = SQL + ComNum.VBLF + "          AND BUNGI = '" + strBunGi + "' ";
            //SQL = SQL + "  GROUP BY BUNGI "
            //SQL = SQL + "     AND BCODE ='641100770'"
            SQL = SQL + "ORDER BY BCode ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            nREAD = dt.Rows.Count;

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");

                return;
            }
            if (nREAD == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("처리할 분기신고 자료가 없습니다.", "확인");

                return;
            }

            nRow = 0;

            for (i = 0; i < nREAD; i++)
            {
                strBCode = dt.Rows[i]["BCdoe"].ToString().Trim();
                dblPrice = VB.Val(dt.Rows[i]["AVGAMT"].ToString().Trim());

                //If nPrice = 0 And nJemsu <> 0 Then
                //nPrice = Fix(((nJemsu * nJemsuPrice) / 10) + 0.5)
                //nPrice = nPrice * 10
                //End If
                nJobCNT = 0;
                if (dblPrice > 0)
                {
                    //TODO : BCode_SUGA_UPDATE_WORK 정의 필요
                }
                else
                {
                    nJobCNT = -1;
                }

                lblJob.Text = i + ":" + strBCode + " " + dblPrice;

                if (nJobCNT != -2 && nJobCNT != 0)
                {
                    nRow = nRow + 1;
                    if (ssSuga_Sheet1.RowCount < nRow)
                    {
                        ssSuga_Sheet1.RowCount = nRow;
                    }
                    ssSuga_Sheet1.Cells[nRow, 1].Text = strBCode;
                    //'SS1.Col = 2: SS1.Text = Format(nJemsu, "###,###,###,##0.00 ")
                    ssSuga_Sheet1.Cells[nRow, 3].Text = VB.Format(dblPrice, "###,###,###,##0");
                    if (nJobCNT == -1)
                    {
                        ssSuga_Sheet1.Cells[nRow, 4].Text = "**ERROR**";
                    }
                    else
                    {
                        ssSuga_Sheet1.Cells[nRow, 4].Text = VB.Format(nJobCNT, "###,###,##0");
                    }
                }
            }

            ssSuga_Sheet1.RowCount = nRow;

            if (strOK == "OK")
            {
                btnStart.Enabled = true;
                btnCancle.Enabled = true;

                ComFunc.MsgBox("정상적으로 처리됨", "확인");
            }
            else
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return;
            }
                
        }
    }
}
