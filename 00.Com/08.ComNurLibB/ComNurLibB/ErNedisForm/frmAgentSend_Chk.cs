using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB;
using ComBase;

namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmAgentSend_Chk.cs
    /// Description     : NEDIS응급자료전송 체크
    /// Author          : 유진호
    /// Create Date     : 2018-05-04
    /// <history>       
    /// D:\참고소스\포항성모병원 VB Source(2018.04.01)\nurse\nrer\agentSend_CHECK
    /// </history>
    /// </summary>
    public partial class frmAgentSend_Chk : Form
    {
        public frmAgentSend_Chk()
        {
            InitializeComponent();
        }

        private void frmAgentSend_Chk_Load(object sender, EventArgs e)
        {            
            ComFunc.ReadSysDate(clsDB.DbCon);            
            timer1.Enabled = true;
        }

        private void Data_Display()
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                //'DB서버에서 50분동안 데이터 갱신작업 유무확인
                SQL = " select * from  nur_er_EMIHPTMI";
                SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT <= to_char(sysdate,'yyyymmdd')";     //'2007-09-14 全 FULL SCAN 않하도록 처리"
                SQL = SQL + ComNum.VBLF + "  AND PTMIINDT >= to_char(sysdate-1,'yyyymmdd') ";   //'2007-09-14 全 FULL SCAN 않하도록 처리
                SQL = SQL + ComNum.VBLF + "   and gbsend ='Y'";
                SQL = SQL + ComNum.VBLF + "   minus ";
                SQL = SQL + ComNum.VBLF + " SELECT* from  nur_er_EMIHPTMI as of timestamp ( systimestamp - interval '50' minute) ";
                //'50분 전 데이터 조회
                SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT <= to_char(sysdate,'yyyymmdd') ";    //'2007-09-14 全 FULL SCAN 않하도록 처리
                SQL = SQL + ComNum.VBLF + "  AND PTMIINDT >= to_char(sysdate-1,'yyyymmdd') ";   //'2007-09-14 全 FULL SCAN 않하도록 처리
                SQL = SQL + ComNum.VBLF + "   and gbsend ='Y'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    INSERT_SMS(0);
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
            }


            try
            {
                //'네디스DB서버에서 PTMISTAT in ('U','C'); 데이터 갱신 국립중앙응급서버로 갱신값을 받지 않는 갓들 확인
                //'----------------------------------------------------
                SQL = " SELECT * FROM EMIHPTMI@EDISAGENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT>=to_char(sysdate-1,'yyyymmdd') and PTMISTAT in ('U','C') ";
                SQL = SQL + ComNum.VBLF + " AND PTMIINDT <= to_char(sysdate,'yyyymmdd') AND PTMIEMCD='C24C0083' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 50)
                {
                    //'미 전송된 자료가 50 건 이상일때(전송값을 리턴받을때 다운타임 조건때문)
                    INSERT_SMS(1);
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            ComFunc.ReadSysDate(clsDB.DbCon);
            lblDateTime.Text = VB.Left(clsPublic.GstrSysTime, 2) + ":" + VB.Right(clsPublic.GstrSysTime, 2);

            if (string.Compare(VB.Left(clsPublic.GstrSysTime, 2), "09") > 0 && VB.Right(clsPublic.GstrSysTime, 2) == "00")
            {
                Data_Display();   //'매시정각마다            
            }

            timer1.Enabled = true;
        }

        private bool INSERT_SMS(int iValue)
        {
            bool rtnVal = false;
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strTxtTel = "";
            string strTxtRetTel = "";
            string strTxtMsg = "";
            //string strRTime = "";
            string strYYMM = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            if (string.Compare(VB.Left(clsPublic.GstrSysTime, 2), "09") < 0) return rtnVal;

            strYYMM = Convert.ToDateTime(clsPublic.GstrSysDate).ToString("yyyyMM");

            strTxtTel = "01085669765";
            if (iValue == 0)
            {
                strTxtMsg = "네디스 센드 프로그램 작동 확인 ";
            }
            else
            {
                strTxtMsg = "네디스 에이전트 프로그램 작동 확인 ";
            }

            //'==================================================================================================================↓2011-06-20 추가
            strTxtRetTel = "0542608336";
            //'==================================================================================================================↑2011-06-20 추가



            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " INSERT INTO ETC_SMS(JobDate, Hphone,Gubun,";
                SQL = SQL + ComNum.VBLF + " Rettel,SendMsg,EntSabun,EntDate)";
                SQL = SQL + ComNum.VBLF + " VALUES ( sysdate,'" + strTxtTel + "',";
                SQL = SQL + ComNum.VBLF + "'A','" + strTxtRetTel + "','" + strTxtMsg + "','4349',SYSDATE) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);

                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);

                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAgentSend_Chk_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
