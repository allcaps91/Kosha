using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrLogin : Form
    {
        //환자정보 전달
        public delegate void SendUserInfo(string strName, string strBuse);
        public event SendUserInfo rSendUserInfo;

        private Dictionary<string, string> pstrBlood = new Dictionary<string, string>();

        /// <summary>
        /// 부서 이름
        /// </summary>
        private string BuseName = string.Empty;

        private string mstrPtNo = string.Empty;
        private string mstrBloodNo = string.Empty;

        private string mstrCOMPONENT = string.Empty;

        public frmEmrLogin(string strPtNo, string strBloodNo, string COMPONENT)
        {
            mstrPtNo = strPtNo;
            mstrBloodNo = strBloodNo;
            mstrCOMPONENT = COMPONENT;
            InitializeComponent();
        }

        private void btnAppUserChk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BuseName))
            {
                if (chk1.Checked && chk2.Checked && chk3.Checked && chk4.Checked && chk5.Checked)
                {
                    if (rSendUserInfo != null)
                    {
                        rSendUserInfo(txtConfirmID2.Text.Trim(), BuseName);
                        Close();
                    }
                    else
                    {
                        Close();
                    }
                }
            }
            else
            {
                ComFunc.MsgBoxEx(this, "모든 항목을 확인하여야 혈액확인이 가능합니다.");
            }
        }

        private void txtConfirmID2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtConfirmPw2.Focus();
            }
        }

        private void txtConfirmPw2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                if (clsType.User.IdNumber.Equals(txtConfirmID2.Text.Trim()))
                {
                    ComFunc.MsgBoxEx(this, "동일한 작성자를 입력하실 수 없습니다.");
                    return;
                }

                #region 쿼리
                OracleDataReader reader = null;
                string SQL = " SELECT USERNAME, C.NAME  ";
                SQL = SQL += ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_USER A";
                SQL = SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_ADM.INSA_MST B";
                SQL = SQL += ComNum.VBLF + "     ON A.SABUN = B.SABUN";
                SQL = SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_BUSE C";
                SQL = SQL += ComNum.VBLF + "     ON B.BUSE = C.BUCODE";
                SQL = SQL += ComNum.VBLF + "WHERE IDNUMBER =  '" + txtConfirmID2.Text.Trim() + "'";
                SQL = SQL += ComNum.VBLF + "  AND PASSHASH256 =  '" + clsSHA.SHA256(txtConfirmPw2.Text.Trim()) + "'";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    txtConfirmID2.Text = reader.GetValue(0).ToString().Trim();
                    BuseName = reader.GetValue(1).ToString().Trim();
                    btnAppUserChk.Enabled = true;
                }

                reader.Dispose();
                #endregion
            }
        }

        private void frmEmrLogin_Load(object sender, EventArgs e)
        {
            Blood_Code_SET();
            READ_BLOOD_INFO(mstrPtNo, mstrBloodNo, mstrCOMPONENT);
        }

        void Blood_Code_SET()
        {
            pstrBlood.Clear();
            pstrBlood.Add("BT021", "P/C(농축적혈구)");
            pstrBlood.Add("BT041", "FFP(신선동결혈장)");
            pstrBlood.Add("BT023", "PLT/C(농축혈소판)");
            pstrBlood.Add("BT011", "W/B(전혈)");
            pstrBlood.Add("BT051", "Cyro(동결침전제제)");
            pstrBlood.Add("BT071", "W/RBC(세척적혈구)");
            pstrBlood.Add("BT101", "WBC/C(농축백혈구)");
            pstrBlood.Add("BT31", "PRP(혈소판풍부혈장)");
            pstrBlood.Add("BT27", "ph-P");
            pstrBlood.Add("BT24", "ph-PLT");
            pstrBlood.Add("BT25", "ph-WBC");
            pstrBlood.Add("BT26", "ph-CB");
            pstrBlood.Add("BT081", "F/RBC(백혈구여과제거 적혈구)");
        }

        void READ_BLOOD_INFO(string argPTNO, string argBLOODNO, string argCOMPONENT)
        {
            chk1.Checked = false;
            chk2.Checked = false;
            chk3.Checked = false;
            chk4.Checked = false;
            chk5.Checked = false;

            txtPano.Clear();
            txtBlood.Clear();
            txtBloodGBN.Clear();
            txtBloodNo.Clear();
            txtBloodDate.Clear();
            ssBlood_Sheet1.RowCount = 0;

            DataTable dt = null;

            #region 진짜 쿼리
            string SQL = " SELECT TO_CHAR(B.OUTDATE, 'YYYY-MM-DD HH24:MI:SS') OUTDATE , A.PANO, (C.PTABO || C.PTRH) AS PTAR, B.COMPONENT, B.BLOODNO, TO_CHAR(B.EXPIRE, 'YYYY-MM-DD') EXPIRE, C.GUMSAJA";
            SQL += ComNum.VBLF + " FROM KOSMOS_OCS.EXAM_BLOODTRANS A, KOSMOS_OCS.EXAM_BLOOD_IO B, KOSMOS_OCS.EXAM_BLOODCROSSM C";
            if (argBLOODNO.Length == 10)
            {
                SQL += ComNum.VBLF + "WHERE B.BLOODNO   = '" + argBLOODNO + "'";
                SQL += ComNum.VBLF + "  AND B.COMPONENT = '" + argCOMPONENT + "'";
            }
            else
            {
                SQL += ComNum.VBLF + "WHERE B.BLOODNO IN(" + argBLOODNO + ")";
                SQL += ComNum.VBLF + "  AND B.COMPONENT = 'BT023'";
            }
            SQL += ComNum.VBLF + "  AND A.PANO    = '" + argPTNO + "'";
            SQL += ComNum.VBLF + "  AND A.GBJOB IN ('3','4')";
            SQL += ComNum.VBLF + "  AND A.PANO    = B.PANO";
            SQL += ComNum.VBLF + "  AND A.PANO    = C.PANO";
            SQL += ComNum.VBLF + "  AND A.BLOODNO = B.BLOODNO";
            SQL += ComNum.VBLF + "  AND A.BLOODNO = C.BLOODNO";
            SQL += ComNum.VBLF + "  AND C.GBSTATUS <> '3'";
            SQL += ComNum.VBLF + "GROUP BY B.OUTDATE, A.PANO, C.PTABO || C.PTRH, B.COMPONENT, B.BLOODNO, B.EXPIRE, C.GUMSAJA";

            try
            {
                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtPano.Text = argPTNO;
                    txtBlood.Text = dt.Rows[0]["PTAR"].ToString().Trim();
                    string strVal = string.Empty;
                    pstrBlood.TryGetValue(dt.Rows[0]["COMPONENT"].ToString().Trim(), out strVal);

                    txtBloodGBN.Text = strVal;
                    txtBloodNo.Text = argBLOODNO;
                    txtBloodDate.Text = dt.Rows[0]["EXPIRE"].ToString().Trim();

                    if (argBLOODNO.Length > 10)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //if (dt.Rows[i]["COMPONENT"].ToString().Equals("BT023"))
                            //{
                                txtBloodNo.Visible = false;
                                txtBloodDate.Visible = false;
                                Width = 497;
                                panPLTC.Visible = true;
                                ssBlood_Sheet1.RowCount += 1;
                                ssBlood_Sheet1.Cells[ssBlood_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BLOODNO"].ToString();
                                ssBlood_Sheet1.Cells[ssBlood_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["EXPIRE"].ToString();
                            //}
                        }
                    }
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            #endregion

        }

        private void btnExitUserChk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
