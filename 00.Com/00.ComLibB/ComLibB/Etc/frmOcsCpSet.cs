using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    public partial class frmOcsCpSet : Form
    {
        private clsOrderEtc OE = null;
        private clsOrderEtc.OCS_CP_RECORD OCR = new clsOrderEtc.OCS_CP_RECORD();
        private double GdblCPNO = 0;

        public frmOcsCpSet(double dblCPNO = 0)
        {
            InitializeComponent();

            GdblCPNO = dblCPNO;
        }

        private void frmOcsCpSet_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            OE = new clsOrderEtc();
            OE.Clear_OCS_CP_RECORD(ref OCR);
            
            SCREEN_CLEAR();

            OCR.PtNo = clsOrdFunction.Pat.PtNo;
            OCR.OPD_ROWID = clsOrdFunction.Pat.Mst_ROWID;
            OCR.CPNO = GdblCPNO;

            SetCbo();

            Read_Pat_Info();
        }

        private void SCREEN_CLEAR()
        {
            btnCPWarm.Enabled = false;
            btnCPActive.Enabled = false;
            btnCPDeActive.Enabled = false;
            btnCPAct.Enabled = false;

            lblWarm.Text = "";
            lblActive.Text = "";
            lblDeActive.Text = "";
            lblAct.Text = "";

            cboCpCode.Enabled = false;

            if (OCR.CP_SELECT == true)
            {
                cboCpCode.Enabled = true;

                if (OCR.CP_NEW == false)
                {
                    cboCpCode.Enabled = false;
                }
            }
            else
            {
                if (OCR.CP_CNT == 0)
                {
                    cboCpCode.Enabled = true;
                }
                else
                {
                    btnCPNew.Visible = true;
                }
            }
        }

        private void SetCbo()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboCpCode.Text = "";
            cboCpCode.Items.Clear();
            cboCpCode.Items.Add("");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     BasCd, BasName ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD ";
                SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "         AND GRPCDB = 'CP관리' ";
                SQL = SQL + ComNum.VBLF + "         AND GRPCD = 'CP코드관리' ";
                SQL = SQL + ComNum.VBLF + "         AND BasName1 = 'ER' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboCpCode.Items.Add(dt.Rows[i]["BASCD"].ToString().Trim() + "." + dt.Rows[i]["BASNAME"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboCpCode.SelectedIndex = 0;

                cboCpSayu.Text = "";
                cboCpSayu.Items.Clear();
                cboCpSayu.Items.Add("001.test");

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Read_Pat_Info()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strTemp = "";

            lblPatInfo.Text = "";

            OE.Read_ERPat_Info(ref OCR);

            lblPatInfo.Text = OCR.CP_STS;

            if (OCR.ER_PATIENT_InDate == "") { return; }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //CP 등록 체크
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CPNO, Ptno, GbIO, DeptCode, Bi ";
                SQL = SQL + ComNum.VBLF + "     , CpCode, ROWID ";
                SQL = SQL + ComNum.VBLF + "     , TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE ";
                SQL = SQL + ComNum.VBLF + "     , TO_CHAR(InTIME,'YYYY-MM-DD HH24:MI') AS InTIME ";
                SQL = SQL + ComNum.VBLF + "     , WarmDate, WarmTime, WarmSabun ";           //예비CP
                SQL = SQL + ComNum.VBLF + "     , StartDate, StartTime, StartSabun ";        //CP등록
                SQL = SQL + ComNum.VBLF + "     , ActDate, ActTime, ActSabun ";              //시술
                SQL = SQL + ComNum.VBLF + "     , DropDate, DropTime, DropSabun ";           //CP제외
                SQL = SQL + ComNum.VBLF + "     , CancerDate, CancerTime, CancerSabun ";     //CP중단
                SQL = SQL + ComNum.VBLF + "     , CallDate, CallTime, CallSabun ";           //의사콜
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_RECORD ";
                SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "         AND PTNO = '" + OCR.PtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND BDate = TO_DATE('" + OCR.BDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND GbIO = 'E' ";
                SQL = SQL + ComNum.VBLF + "         AND InTime = TO_DATE('" + OCR.ER_PATIENT_InDate + " " + OCR.ER_PATIENT_InTime + "','YYYY-MM-DD HH24:MI')  ";
                
                if (OCR.CP_NEW == false)
                {
                    if (OCR.CPNO > 0)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND CPNO = '" + OCR.CPNO + "' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         AND CPNO = '1' ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         AND CPNO = '1' ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    lblPatInfo.Text += " 미등록 )";

                    btnCPActive.Enabled = true;
                    btnCPWarm.Enabled = true;
                }
                else
                {
                    cboCpCode.Enabled = false;
                    btnCPActive.Enabled = false;
                    btnCPDeActive.Enabled = true;
                    btnCPWarm.Enabled = true;
                    btnCPAct.Enabled = true;
                    btnCPNew.Visible = true;

                    OCR.CPNO = VB.Val(dt.Rows[0]["CPNO"].ToString().Trim());
                    OCR.CP_CODE = dt.Rows[0]["CPCODE"].ToString().Trim();

                    if (OCR.CP_CODE != "")
                    {
                        OCR.CP_Name = OE.READ_CP_NAME(OCR.CP_CODE, "ER");
                    }

                    OCR.OPD_BDate = dt.Rows[0]["BDATE"].ToString().Trim();
                    OCR.BDate = dt.Rows[0]["BDATE"].ToString().Trim();
                    OCR.CP_ROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                    if (OCR.CP_CODE != "")
                    {
                        cboCpCode.Text = OCR.CP_CODE + "." + OCR.CP_Name;
                    }

                    if (dt.Rows[0]["STARTDATE"].ToString().Trim() != "")
                    {
                        strTemp = "activation +";
                        lblActive.Text = Convert.ToDateTime(ComFunc.FormatStrToDateEx(dt.Rows[0]["STARTDATE"].ToString().Trim(), "D", "-") + " " + ComFunc.FormatStrToDateEx(dt.Rows[0]["STARTTIME"].ToString().Trim(), "T", ":")).ToString("yyyy/MM/dd HH:mm:ss") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["STARTSABUN"].ToString().Trim()) + ")";
                    }

                    if (dt.Rows[0]["DROPDATE"].ToString().Trim() != "")
                    {
                        strTemp = "deactivation +";
                        lblDeActive.Text = Convert.ToDateTime(ComFunc.FormatStrToDateEx(dt.Rows[0]["DROPDATE"].ToString().Trim(), "D", "-") + " " + ComFunc.FormatStrToDateEx(dt.Rows[0]["DROPTIME"].ToString().Trim(), "T", ":")).ToString("yyyy/MM/dd HH:mm:ss") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DROPSABUN"].ToString().Trim()) + ")";
                        btnCPDeActive.Enabled = false;
                    }

                    if (dt.Rows[0]["WARMDATE"].ToString().Trim() != "")
                    {
                        strTemp += "예비 CP +";
                        lblWarm.Text = Convert.ToDateTime(ComFunc.FormatStrToDateEx(dt.Rows[0]["WARMDATE"].ToString().Trim(), "D", "-") + " " + ComFunc.FormatStrToDateEx(dt.Rows[0]["WARMTIME"].ToString().Trim(), "T", ":")).ToString("yyyy/MM/dd HH:mm:ss") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["WARMSABUN"].ToString().Trim()) + ")";
                        btnCPWarm.Enabled = false;

                        if (dt.Rows[0]["STARTDATE"].ToString().Trim() == "")
                        {
                            btnCPActive.Enabled = true;
                        }
                    }

                    if (dt.Rows[0]["ACTDATE"].ToString().Trim() != "")
                    {
                        strTemp += "시술 +";
                        lblAct.Text = Convert.ToDateTime(ComFunc.FormatStrToDateEx(dt.Rows[0]["ACTDATE"].ToString().Trim(), "D", "-") + " " + ComFunc.FormatStrToDateEx(dt.Rows[0]["ACTTIME"].ToString().Trim(), "T", ":")).ToString("yyyy/MM/dd HH:mm:ss") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["ACTSABUN"].ToString().Trim()) + ")";
                        btnCPAct.Enabled = false;
                    }

                    if (dt.Rows[0]["CALLDATE"].ToString().Trim() != "")
                    {
                        strTemp += "의사콜 +";
                    }

                    if (strTemp != "")
                    {
                        if (VB.Right(strTemp, 1) == "+")
                        {
                            strTemp = VB.Mid(strTemp, 1, strTemp.Length - 1);
                        }

                        lblPatInfo.Text += strTemp + ")";
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnCPNew_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            
            OCR.CP_ROWID = "";

            OCR.CP_NEW = true;

            cboCpCode.Enabled = true;
            cboCpCode.Text = "";

            btnCPWarm.Enabled = true;
            btnCPActive.Enabled = true;

            Read_Pat_Info();
        }

        private void btnCPWarm_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQ("[예비 CP] 작업을 하시겠습니까??", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                if (OE.CP_ER_Save(ref OCR, "예비 CP", VB.Pstr(cboCpCode.Text, ".", 1).Trim()) == true)
                {
                    this.Close();
                }
            }
        }

        private void btnCPActive_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQ("[activation] 작업을 하시겠습니까??", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                if (OE.CP_ER_Save(ref OCR, "CP activation", VB.Pstr(cboCpCode.Text, ".", 1).Trim()) == true)
                {
                    this.Close();
                }
            }
        }

        private void btnCPDeActive_Click(object sender, EventArgs e)
        {
            string strMSG = "";

            if (ComFunc.MsgBoxQ("[deactivation] 작업을 하시겠습니까??", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                if (OE.CP_ER_Save(ref OCR, "CP deactivation", VB.Pstr(cboCpCode.Text, ".", 1).Trim()) == true)
                {
                    if (ComFunc.MsgBoxQ("기존 발송된 문자의 취소문자 작업을 하시겠습니까??", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        strMSG = OCR.PtNo + " " + OCR.sName + " " + OCR.Sex + "/" + OCR.Age + " " + OCR.CP_Name + " CP대상 해제합니다.";

                        if (OE.SEND_DeActivation_SMS(OCR.PtNo, OCR.CPNO, strMSG) == true)
                        {
                            if (OE.CP_ER_Save(ref OCR, "SMS CP deactivation", "") == true)
                            {
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
        }

        private void btnCPAct_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQ("[시술 CP] 작업을 하시겠습니까??", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                if (OE.CP_ER_Save(ref OCR, "시술", VB.Pstr(cboCpCode.Text, ".", 1).Trim()) == true)
                {
                    this.Close();
                }
            }
        }
    }
}
