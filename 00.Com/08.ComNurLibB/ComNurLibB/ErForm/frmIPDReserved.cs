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
using ComLibB;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmIPDReserved.cs
    /// Description     : 입원안내문
    /// Author          : 유진호
    /// Create Date     : 2018-05-04
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrer\Frm입원안내문.frm(Frm입원안내문.frm) >> frmIPDReserved.cs 폼이름 재정의" />	
    public partial class frmIPDReserved : Form
    {
        private string strPano = "";
        private string FstrROWID = "";

        public frmIPDReserved()
        {
            InitializeComponent();
        }

        public frmIPDReserved(string strPano)
        {
            InitializeComponent();
            this.strPano = strPano;
        }

        private void frmIPDReserved_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);
            SET_ComboDept();
            txtJDate.Text = clsPublic.GstrSysDate;
            SELECT_DATA();
        }

        private void SET_ComboDept()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            cboDept.Items.Clear();

            try
            {
                SQL = "SELECT PrintRanking,DeptCode ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ( 'MD','HD','OC','II','R6','TO','HR','PT','AN','HC','OM','LM' ) ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
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

        private void SELECT_DATA()
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ComFunc CF = new ComFunc();

            cboDept.Items.Clear();

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.ROWID,B.SNAME,JUMIN1,JUMIN2,DECODE(B.SEX,'M','남','여') ";
                SQL = SQL + ComNum.VBLF + "        SEX,A.DEPTCODE,A.DRCODE,A.REMARK,DIAGNOSIS,GBER_WARD,GB_INFECT ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_RESERVED A, KOSMOS_PMPA.BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + " WHERE REDATE(+) = TO_DATE('" + txtJDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND GBER(+) = 'Y' ";
                SQL = SQL + ComNum.VBLF + "    AND B.PANO = '" + strPano + "'  ";
                SQL = SQL + ComNum.VBLF + "    AND A.PANO(+) = B.PANO";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    txtPano.Text = strPano;
                    txtSName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    txtJumin1.Text = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    txtJumin2.Text = dt.Rows[0]["JUMIN2"].ToString().Trim();
                    txtAge.Text = ComFunc.AgeCalcEx(txtJumin1.Text + txtJumin2.Text, txtJDate.Text).ToString();
                    txtSex.Text = dt.Rows[0]["SEX"].ToString().Trim();


                    if (dt.Rows[0]["ROWID"].ToString().Trim() != "")
                    {
                        ComFunc.MsgBox("등록된 내용이 있어 수정 모드로 자료를 불러옵니다..");
                        FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                        cboDept.Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                        cboDoct.Text = dt.Rows[0]["DRCODE"].ToString().Trim() + "." + CF.READ_DrName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());

                        if (dt.Rows[0]["GBER_WARD"].ToString().Trim() == "0")
                        {
                            optWard_0.Checked = true;
                        }
                        else if (dt.Rows[0]["GBER_WARD"].ToString().Trim() == "1")
                        {
                            optWard_1.Checked = true;
                        }

                        if (dt.Rows[0]["GB_Infect"].ToString().Trim() == "1")
                        {
                            chkPrt_impt.Checked = true;
                        }
                        else
                        {
                            chkPrt_impt.Checked = false;
                        }

                        txtDiss.Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
                    }
                    else
                    {
                        cboDept.Text = "";
                        cboDoct.Text = "";
                        optWard_1.Checked = true;
                        chkPrt_impt.Checked = false;
                        txtDiss.Text = "";
                        FstrROWID = "";
                    }
                }
                dt.Dispose();
                dt = null;
                CF = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strDoct = "";

            cboDoct.Items.Clear();

            try
            {                
                SQL = "SELECT DrName,DrCode,Sabun FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode = '" + cboDept.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GbOut ='N' ";
                SQL = SQL + ComNum.VBLF + "  AND SUBSTR(DRCODE,3,2) <> '99' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY DRCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strDoct = dt.Rows[i]["DrCode"].ToString().Trim() + "." + dt.Rows[i]["DrName"].ToString().Trim();
                    cboDoct.Items.Add(strDoct);
                    cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                }

                cboDoct.SelectedIndex = 0;

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return;  //권한 확인

            if (btnSaveClick(FstrROWID) == true)
            {
                prtBarCode();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool btnSaveClick(string strROWID)
        {
            bool rtnVal = false;
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strWard = "";
            //string strWSABUN = "";
            //string strWBUSE  = "";
            //string StrRDate = "";
            //string strTARGET1  = "";
            //string strTARGET2 = "";
            string strINFECT = "";


            if (optWard_0.Checked == true)
            {
                strWard = "0";
            }
            else
            {
                strWard = "1";
            }

            if (chkPrt_impt.Checked == true)
            {
                strINFECT = "1";
            }
            else
            {
                strINFECT = "0";
            }
            

            if( cboDept.Text == "" || cboDoct.Text == "" )
            {
                ComFunc.MsgBox("진료과 및 주치의 정보가 누락되었습니다.");
                return rtnVal;
            }


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                if (FstrROWID != "")
                {
                    SQL = " UPDATE KOSMOS_PMPA.IPD_RESERVED ";
                    SQL = SQL + ComNum.VBLF + " SET  DEPTCODE ='" + cboDept.Text + "',";
                    SQL = SQL + ComNum.VBLF + " DRCODE ='" + cboDoct.Text.Split('.')[0] + "',";
                    SQL = SQL + ComNum.VBLF + " GBER_WARD ='" + strWard + "',";
                    SQL = SQL + ComNum.VBLF + " GB_Infect ='" + strINFECT + "',";
                    SQL = SQL + ComNum.VBLF + " SDATE =SYSDATE , ";
                    SQL = SQL + ComNum.VBLF + " DIAGNOSIS ='" + txtDiss.Text + "'";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";                    
                }
                else
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.IPD_RESERVED(";
                    SQL = SQL + ComNum.VBLF + " PANO, SNAME, REDATE, DEPTCODE, ";
                    SQL = SQL + ComNum.VBLF + "  DRCODE, SDATE , ";
                    SQL = SQL + ComNum.VBLF + "  GBER, DIAGNOSIS, GB_Infect ,GBER_WARD ) VALUES (";
                    SQL = SQL + ComNum.VBLF + "'" + txtPano.Text + "','" + txtSName.Text + "',TO_DATE('" + txtJDate.Text + "','YYYY-MM-DD '),";
                    SQL = SQL + ComNum.VBLF + "'" + cboDept.Text + "','" + cboDoct.Text.Split('.')[0] + "',SYSDATE ,'Y' ,";
                    SQL = SQL + ComNum.VBLF + "'" + txtDiss.Text + "','" + strINFECT + "','" + strWard + "') ";
                }
                
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        //수혈바코드인쇄
        private void prtBarCode()
        {
            //TODO
        }
    }
}
