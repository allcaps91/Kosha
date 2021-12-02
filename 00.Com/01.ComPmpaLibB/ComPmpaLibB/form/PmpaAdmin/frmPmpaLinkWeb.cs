using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Security.Cryptography;

/// <summary>
/// Description : 환자정보를 앱으로 연동
/// Author : 박병규
/// Create Date : 2017.06.20
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaLinkWeb : Form
    {
        DataTable Dt = new DataTable();
        DataTable DtPat = new DataTable();
        string SQL = "";
        string SqlErr = "";
        int intRowCnt = 0;

        clsPmpaFunc CPF = null;

        String strPtno = string.Empty;
        string strSname = string.Empty;
        string strSex = string.Empty;
        string strJumin1 = string.Empty;
        string strHphone = string.Empty;
        
        public frmPmpaLinkWeb()
        {
            InitializeComponent();
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eForm_Load);

            this.txtPtno.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);

            this.btnSave.Click += new EventHandler(eCtl_Click);
            this.btnExit.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
            else if (sender == this.btnExit)
                this.Close();
        }

        private void Save_Process(PsmhDb pDbCon)
        {
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, SNAME, JUMIN1, ";
            SQL += ComNum.VBLF + "        SEX,  HPHONE , ROWID ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_PATIENT ";
            SQL += ComNum.VBLF + "  WHERE PANO = '" + txtPtno.Text + "'";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count == 0)
                return;

            //TODO : Call MyadoConnect("psmh", "psmh", "psmh2")

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string strOK = "OK";

                string strPtno = Dt.Rows[0]["PANO"].ToString().Trim();
                string strSname = Dt.Rows[0]["sName"].ToString().Trim();
                string strJumin = Dt.Rows[0]["Jumin1"].ToString().Trim();
                string strSex = Dt.Rows[0]["Sex"].ToString().Trim();
                string strHTel = clsSHA.SHA256(VB.Replace(Dt.Rows[0]["Hphone"].ToString().Trim(), "-", ""));

                //SQL = "SELECT M_PTNO  FROM tb_patbav WHERE M_PTNO = '" & strPANO & "' "
                //Result = MyAdoOpenSet(rs2, SQL)

                //If Myrowindicator = 0 Then
                //    SQL = " INSERT INTO tb_patbav(m_ptno, m_ptnm, m_birth, m_sexcl, m_telno, m_remark  ) VALUES( "
                //    SQL = SQL & vbCr & "'" & strPANO & "' , '" & strSname & "" & "' , '" & strJumin1 & "', '" & strSex & "', '" & strHTel & "' , '" & strRemark & "' ) "
                //    Result = MyAdoExecute(SQL)
                //    If Result <> "00" Then strOK = "NO"
                //Else
                //    SQL = " UPDATE tb_patbav SET "
                //    SQL = SQL & "  m_passwrod = '" & strJumin1 & "', "
                //    SQL = SQL & "  m_ptnm = '" & strSname & "' ,"
                //    SQL = SQL & "  m_birth = '" & strJumin1 & "' ,"
                //    SQL = SQL & "  m_sexcl = '" & strSex & "', "
                //    SQL = SQL & "  m_telno = '" & strHTel & "' , "
                //    SQL = SQL & "   m_remark = '" & strRemark & "' "
                //    SQL = SQL & "  WHERE m_ptno = '" & strPANO & "'  "
                //    Result = MyAdoExecute(SQL)
                //    If Result <> "00" Then strOK = "NO"
                //End If

                //If strOK = "OK" Then
                //   SQL = " UPDATE KOSMOS_PMPA.BAS_PATIENT SET WEBSEND ='*' , WEBSENDDATE = SYSDATE "
                //   SQL = SQL & " WHERE ROWID = '" & Trim(RsBas!ROWID & "") & "' "
                //   Result = AdoExecute(SQL)
                //End If
            }

            //TODO : Call MyAdoDisConnect

            Dt.Dispose();
            Dt = null;
        }


        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno && e.KeyChar == (char)13)
            {
                txtPtno.Text = string.Format("{0:D8}", Convert.ToInt64(txtPtno.Text));

                DtPat = CPF.Get_BasPatient(clsDB.DbCon, txtPtno.Text);

                strPtno = DtPat.Rows[0]["PANO"].ToString().Trim();
                strSname = DtPat.Rows[0]["SNAME"].ToString().Trim();
                strSex = DtPat.Rows[0]["SEX"].ToString().Trim();
                strJumin1 = DtPat.Rows[0]["JUMIN1"].ToString().Trim();

                //TODO
                strHphone = DtPat.Rows[0]["HPHONE"].ToString().Trim();

                DtPat.Dispose();
                DtPat = null;
            }
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            clsPmpaFunc CPF = new clsPmpaFunc();

            ComFunc.SetAllControlClear(pnlBody);
        }
    }
}
