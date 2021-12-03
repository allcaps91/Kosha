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

namespace ComNurLibB
{
    public partial class frmISS : Form
    {
        private frmAISCode frmAISCodeX = null;

        private string mstrPano = "";
        private string mstrInDate = "";
        private string mstrROWID = "";

        private string mstrAISSCODE = "";
        private string mstrAISSCOMM = "";
        private string mstrAISSSCOR = "";

        public frmISS()
        {
            InitializeComponent();
        }

        public frmISS(string strPano, string strInDate)
        {
            InitializeComponent();
            mstrPano = strPano;
            mstrInDate = strInDate;
        }

        private void frmISS_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            CLEAR_SCREEN();

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT JUMIN1, SEX, SNAME ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + mstrPano + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtPANO.Text = mstrPano;
                    txtYEAR.Text = dt.Rows[0]["SEX"].ToString().Trim() + " / " + clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, mstrPano);
                    txtNAME.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            READ_DATA();
        }

        private void READ_DATA()
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT PART1_CODE, PART1_NAME, PART1_SCORE, ";
                SQL = SQL + ComNum.VBLF + " PART2_CODE, PART2_NAME, PART2_SCORE, ";
                SQL = SQL + ComNum.VBLF + " PART3_CODE, PART3_NAME, PART3_SCORE, ";
                SQL = SQL + ComNum.VBLF + " PART4_CODE, PART4_NAME, PART4_SCORE, ";
                SQL = SQL + ComNum.VBLF + " PART5_CODE, PART5_NAME, PART5_SCORE, ";
                SQL = SQL + ComNum.VBLF + " PART6_CODE, PART6_NAME, PART6_SCORE, ISS_SCORE, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_ER_PATIENT_ISS";
                SQL = SQL + ComNum.VBLF + " WHERE BDATE = TO_DATE('" + mstrInDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND PANO = '" + mstrPano + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtAIC_1.Text = dt.Rows[0]["PART1_CODE"].ToString().Trim();
                    txtAICn_1.Text = dt.Rows[0]["PART1_NAME"].ToString().Trim();
                    txtAIS_1.Text = dt.Rows[0]["PART1_SCORE"].ToString().Trim();
                    txtAIC_2.Text = dt.Rows[0]["PART2_CODE"].ToString().Trim();
                    txtAICn_2.Text = dt.Rows[0]["PART2_NAME"].ToString().Trim();
                    txtAIS_2.Text = dt.Rows[0]["PART2_SCORE"].ToString().Trim();
                    txtAIC_3.Text = dt.Rows[0]["PART3_CODE"].ToString().Trim();
                    txtAICn_3.Text = dt.Rows[0]["PART3_NAME"].ToString().Trim();
                    txtAIS_3.Text = dt.Rows[0]["PART3_SCORE"].ToString().Trim();
                    txtAIC_4.Text = dt.Rows[0]["PART4_CODE"].ToString().Trim();
                    txtAICn_4.Text = dt.Rows[0]["PART4_NAME"].ToString().Trim();
                    txtAIS_4.Text = dt.Rows[0]["PART4_SCORE"].ToString().Trim();
                    txtAIC_5.Text = dt.Rows[0]["PART5_CODE"].ToString().Trim();
                    txtAICn_5.Text = dt.Rows[0]["PART5_NAME"].ToString().Trim();
                    txtAIS_5.Text = dt.Rows[0]["PART5_SCORE"].ToString().Trim();
                    txtAIC_6.Text = dt.Rows[0]["PART6_CODE"].ToString().Trim();
                    txtAICn_6.Text = dt.Rows[0]["PART6_NAME"].ToString().Trim();
                    txtAIS_6.Text = dt.Rows[0]["PART6_SCORE"].ToString().Trim();

                    txtCISS.Text = dt.Rows[0]["ISS_SCORE"].ToString().Trim();
                    mstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void CLEAR_SCREEN()
        {
            txtYEAR.Text = "";

            txtAIC_1.Text = "";             //'두경부 손상내용
            txtAICn_1.Text = "";
            txtAIS_1.Text = "";             //'점수

            txtAIC_2.Text = "";             //'안면 손상내용
            txtAICn_2.Text = "";
            txtAIS_2.Text = "";             //'점수

            txtAIC_3.Text = "";             //'흉부 손상내용
            txtAICn_3.Text = "";
            txtAIS_3.Text = "";             //'점수

            txtAIC_4.Text = "";             //'복부 손상내용
            txtAICn_4.Text = "";
            txtAIS_4.Text = "";             //'점수

            txtAIC_5.Text = "";             //'사지 손상내용
            txtAICn_5.Text = "";
            txtAIS_5.Text = "";             //'점수

            txtAIC_6.Text = "";             //'외부 손상내용
            txtAICn_6.Text = "";
            txtAIS_6.Text = "";             //'점수

            txtCISS.Text = "";              //'ISS
        }

        private void btnAIC_1_Click(object sender, EventArgs e)
        {
            if (frmAISCodeX != null)
            {
                frmAISCodeX.Dispose();
                frmAISCodeX = null;
            }
            frmAISCodeX = new frmAISCode("HEAD|NECK");
            frmAISCodeX.rSendMsg += FrmSugaSerchX_rSendMsg;
            frmAISCodeX.rEventClosed += FrmSugaSerchX_rEventClosed;
            frmAISCodeX.ShowDialog();

            txtAICn_1.Text = mstrAISSCODE;
            txtAICn_1.Text = mstrAISSCOMM;
            txtAIS_1.Text = mstrAISSSCOR;
        }

        private void btnAIC_2_Click(object sender, EventArgs e)
        {
            if (frmAISCodeX != null)
            {
                frmAISCodeX.Dispose();
                frmAISCodeX = null;
            }
            frmAISCodeX = new frmAISCode("FACE");
            frmAISCodeX.rSendMsg += FrmSugaSerchX_rSendMsg;
            frmAISCodeX.rEventClosed += FrmSugaSerchX_rEventClosed;
            frmAISCodeX.ShowDialog();

            txtAIC_2.Text = mstrAISSCODE;
            txtAICn_2.Text = mstrAISSCOMM;
            txtAIS_2.Text = mstrAISSSCOR;
        }

        private void btnAIC_3_Click(object sender, EventArgs e)
        {
            if (frmAISCodeX != null)
            {
                frmAISCodeX.Dispose();
                frmAISCodeX = null;
            }
            frmAISCodeX = new frmAISCode("THORAX");
            frmAISCodeX.rSendMsg += FrmSugaSerchX_rSendMsg;
            frmAISCodeX.rEventClosed += FrmSugaSerchX_rEventClosed;
            frmAISCodeX.ShowDialog();

            txtAIC_3.Text = mstrAISSCODE;
            txtAICn_3.Text = mstrAISSCOMM;
            txtAIS_3.Text = mstrAISSSCOR;
        }

        private void btnAIC_4_Click(object sender, EventArgs e)
        {
            if (frmAISCodeX != null)
            {
                frmAISCodeX.Dispose();
                frmAISCodeX = null;
            }
            frmAISCodeX = new frmAISCode("ABDOMEN");
            frmAISCodeX.rSendMsg += FrmSugaSerchX_rSendMsg;
            frmAISCodeX.rEventClosed += FrmSugaSerchX_rEventClosed;
            frmAISCodeX.ShowDialog();

            txtAIC_4.Text = mstrAISSCODE;
            txtAICn_4.Text = mstrAISSCOMM;
            txtAIS_4.Text = mstrAISSSCOR;
        }

        private void btnAIC_5_Click(object sender, EventArgs e)
        {
            if (frmAISCodeX != null)
            {
                frmAISCodeX.Dispose();
                frmAISCodeX = null;
            }
            frmAISCodeX = new frmAISCode("LOWER EXTREMITY|UPPER EXTREMITY");
            frmAISCodeX.rSendMsg += FrmSugaSerchX_rSendMsg;
            frmAISCodeX.rEventClosed += FrmSugaSerchX_rEventClosed;
            frmAISCodeX.ShowDialog();

            txtAIC_5.Text = mstrAISSCODE;
            txtAICn_5.Text = mstrAISSCOMM;
            txtAIS_5.Text = mstrAISSSCOR;
        }

        private void btnAIC_6_Click(object sender, EventArgs e)
        {
            if (frmAISCodeX != null)
            {
                frmAISCodeX.Dispose();
                frmAISCodeX = null;
            }
            frmAISCodeX = new frmAISCode("EXTERNAL");
            frmAISCodeX.rSendMsg += FrmSugaSerchX_rSendMsg;
            frmAISCodeX.rEventClosed += FrmSugaSerchX_rEventClosed;
            frmAISCodeX.ShowDialog();

            txtAIC_6.Text = mstrAISSCODE;
            txtAICn_6.Text = mstrAISSCOMM;
            txtAIS_6.Text = mstrAISSSCOR;
        }

        private void FrmSugaSerchX_rEventClosed()
        {
            if (frmAISCodeX != null)
            {
                frmAISCodeX.Dispose();
                frmAISCodeX = null;
            }
        }

        private void FrmSugaSerchX_rSendMsg(string strAISSCODE, string strAISSCOMM, string strAISSSCOR)
        {
            mstrAISSCODE = strAISSCODE;
            mstrAISSCOMM = strAISSCOMM;
            mstrAISSSCOR = strAISSSCOR;
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            int nTempA = 0;
            int[] nTemp = new int[7];

            txtCISS.Text = "";
            nTemp[0] = (int)VB.Val(txtAIS_1.Text);
            nTemp[1] = (int)VB.Val(txtAIS_2.Text);
            nTemp[2] = (int)VB.Val(txtAIS_3.Text);
            nTemp[3] = (int)VB.Val(txtAIS_4.Text);
            nTemp[4] = (int)VB.Val(txtAIS_5.Text);
            nTemp[5] = (int)VB.Val(txtAIS_6.Text);

            for (i = 0; i <= 5; i++)
            {
                if (nTemp[i] == 6)
                {
                    txtCISS.Text = "75";
                    break;
                }
            }

            if (txtCISS.Text != "75")
            {
                for (i = 0; i <= 5; i++)
                {
                    for (j = 0; j <= 5; j++)
                    {
                        if (nTemp[i] < nTemp[j])
                        {
                            nTempA = nTemp[i];
                            nTemp[i] = nTemp[j];
                            nTemp[j] = nTempA;
                        }
                    }
                }
                txtCISS.Text = (nTemp[0] * nTemp[0]) + (nTemp[1] * nTemp[1]) + (nTemp[2] * nTemp[2]).ToString();          //'퇴실시 ISS
            }

            btnSave.PerformClick();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {            
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;


            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            
            if (VB.Trim(txtCISS.Text) == "")
            {
                ComFunc.MsgBox("ISS 점수가 없습니다.", "확인");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " UPDATE KOSMOS_PMPA.NUR_ER_PATIENT SET ";
                SQL = SQL + ComNum.VBLF + "    ISS = '" + VB.Trim(txtCISS.Text) + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE JDATE = TO_DATE('" + mstrInDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "     AND PANO = '" + mstrPano + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }


                if (mstrROWID == "")
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_PATIENT_ISS(";
                    SQL = SQL + ComNum.VBLF + " PART1_CODE, PART1_NAME, PART1_SCORE, ";
                    SQL = SQL + ComNum.VBLF + " PART2_CODE, PART2_NAME, PART2_SCORE, ";
                    SQL = SQL + ComNum.VBLF + " PART3_CODE, PART3_NAME, PART3_SCORE, ";
                    SQL = SQL + ComNum.VBLF + " PART4_CODE, PART4_NAME, PART4_SCORE, ";
                    SQL = SQL + ComNum.VBLF + " PART5_CODE, PART5_NAME, PART5_SCORE, ";
                    SQL = SQL + ComNum.VBLF + " PART6_CODE, PART6_NAME, PART6_SCORE, ";
                    SQL = SQL + ComNum.VBLF + " ISS_SCORE, WRITEDATE, WRITESABUN, BDATE, PANO) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Trim(txtAIC_1.Text) + "','" + VB.Trim(txtAICn_1.Text) + "','" + VB.Trim(txtAIS_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Trim(txtAIC_2.Text) + "','" + VB.Trim(txtAICn_2.Text) + "','" + VB.Trim(txtAIS_2.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Trim(txtAIC_3.Text) + "','" + VB.Trim(txtAICn_3.Text) + "','" + VB.Trim(txtAIS_3.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Trim(txtAIC_4.Text) + "','" + VB.Trim(txtAICn_4.Text) + "','" + VB.Trim(txtAIS_4.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Trim(txtAIC_5.Text) + "','" + VB.Trim(txtAICn_5.Text) + "','" + VB.Trim(txtAIS_5.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Trim(txtAIC_6.Text) + "','" + VB.Trim(txtAICn_6.Text) + "','" + VB.Trim(txtAIS_6.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Trim(txtCISS.Text) + "', SYSDATE, " + clsType.User.Sabun + ", TO_DATE('" + mstrInDate + "','YYYY-MM-DD'), '" + mstrPano + "') ";
                }
                else
                {
                    SQL = " UPDATE KOSMOS_PMPA.NUR_ER_PATIENT_ISS SET ";
                    SQL = SQL + ComNum.VBLF + " PART1_CODE = '" + VB.Trim(txtAIC_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART1_NAME = '" + VB.Trim(txtAICn_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART1_SCORE = '" + VB.Trim(txtAIS_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART2_CODE = '" + VB.Trim(txtAIC_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART2_NAME = '" + VB.Trim(txtAICn_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART2_SCORE = '" + VB.Trim(txtAIS_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART3_CODE = '" + VB.Trim(txtAIC_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART3_NAME = '" + VB.Trim(txtAICn_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART3_SCORE = '" + VB.Trim(txtAIS_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART4_CODE = '" + VB.Trim(txtAIC_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART4_NAME = '" + VB.Trim(txtAICn_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART4_SCORE = '" + VB.Trim(txtAIS_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART5_CODE = '" + VB.Trim(txtAIC_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART5_NAME = '" + VB.Trim(txtAICn_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART5_SCORE = '" + VB.Trim(txtAIS_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART6_CODE = '" + VB.Trim(txtAIC_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART6_NAME = '" + VB.Trim(txtAICn_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " PART6_SCORE = '" + VB.Trim(txtAIS_1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " ISS_SCORE    = '" + VB.Trim(txtCISS.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " WRITEDATE = SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + " WRITESABUN = " + clsType.User.Sabun;
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + mstrROWID + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtAIS_1_Leave(object sender, EventArgs e)
        {
            if (VB.Val(((TextBox)sender).Text) > 6)
            {
                ComFunc.MsgBox("AIS 점수 범위는 0~6입니다.");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
            }
        }
    }
}
