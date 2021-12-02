using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupEndoMayakList.cs
    /// Description     : 내시경실비상마약류관리리스트
    /// Author          : 이정현
    /// Create Date     : 2017-11-27
    /// Update History  : 2017-11-24 새 폼 생성된거 추가 작업
    /// <history> 
    /// 내시경실비상마약류관리리스트
    /// </history>
    /// <seealso>
    /// PSMH\drug\drmayak\Frm내시경실비상마약류관리리스트.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drmayak\drmayak.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupEndoMayakList : Form
    {
        public frmComSupEndoMayakList()
        {
            InitializeComponent();
        }

        private void frmComSupEndoMayakList_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssView_Sheet1.RowCount = 0;
            ssPrint_Sheet1.RowCount = 8;

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            cboJepCode.Items.Clear();
            cboJepCode.Items.Add("전체");
            //cboJepCode.Items.Add("A-POL12A");
            //cboJepCode.Items.Add("A-PO12GA");
            cboJepCode.Items.Add("A-ANE12");
            cboJepCode.Items.Add("A-ANE12G");
            cboJepCode.Items.Add("N-PTD25");
            //cboJepCode.Items.Add("A-BASCA");
            //cboJepCode.Items.Add("A-POL12");
            cboJepCode.Items.Add("A-BASCAM");
            cboJepCode.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strPTNO = "";
            string strSuCode = "";
            string strGubun = "";

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PTNO, BDATE, DEPTCODE, SUCODE, 'O' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER MST";
                SQL = SQL + ComNum.VBLF + "     WHERE EXISTS";
                SQL = SQL + ComNum.VBLF + "             (SELECT * FROM " + ComNum.DB_MED + "ENDO_JUPMST SUB";
                SQL = SQL + ComNum.VBLF + "                 WHERE RDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                     AND RDATE < TO_DATE('" + dtpDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                     AND MST.BDATE = SUB.BDATE";
                SQL = SQL + ComNum.VBLF + "                     AND MST.PTNO = SUB.PTNO";
                SQL = SQL + ComNum.VBLF + "                     AND MST.DEPTCODE = SUB.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "                     AND SUB.BUSE = '056104')";

                if (chkSaveData.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "         AND NOT EXISTS";
                    SQL = SQL + ComNum.VBLF + "                     (SELECT * FROM " + ComNum.DB_MED + "ENDO_LIST B";
                    SQL = SQL + ComNum.VBLF + "                         WHERE BDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "                             AND BDATE < TO_DATE('" + dtpDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "                             AND MST.PTNO = B.PTNO";
                    SQL = SQL + ComNum.VBLF + "                             AND MST.SUCODE = B.SUCODE)";
                }

                SQL = SQL + ComNum.VBLF + "         AND SUCODE IN";
                SQL = SQL + ComNum.VBLF + "                 (SELECT";
                SQL = SQL + ComNum.VBLF + "                     JEPCODE";
                SQL = SQL + ComNum.VBLF + "                 From " + ComNum.DB_ERP + "DRUG_JEP";
                SQL = SQL + ComNum.VBLF + "                     WHERE CHENGGU = '09'";

                //if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-POL12")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE = '" + cboJepCode.Text + "'";
                //}

                //if (cboJepCode.Text == "A-POL12")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE IN ('A-POL12', 'A-POL12G')";
                //}

                // 2019-07-25 코드변경으로 수정
                //if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-PO12GA")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT = '" + cboJepCode.Text + "'";
                //}

                //if (cboJepCode.Text == "A-PO12GA")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT IN ('A-POL12A', 'A-PO12GA', 'A-POL12G')";
                //}

                // 2020-10-05 코드변경으로 수정
                if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-ANE12" && cboJepCode.Text != "A-ANE12G")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE = '" + cboJepCode.Text + "'";
                }

                if (cboJepCode.Text == "A-ANE12")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE IN ('A-POL12A', 'A-ANE12')";
                }
                else if (cboJepCode.Text == "A-ANE12G")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE IN ('A-POL12A', 'A-PO12GA', 'A-POL12G', 'A-ANE12G')";
                }

                SQL = SQL + ComNum.VBLF + "                 Union All";
                SQL = SQL + ComNum.VBLF + "                 SELECT";
                SQL = SQL + ComNum.VBLF + "                     SUNEXT";
                SQL = SQL + ComNum.VBLF + "                 FROM " + ComNum.DB_ERP + "DRUG_JEP A, " + ComNum.DB_ERP + "DRUG_CONVRATE B";
                SQL = SQL + ComNum.VBLF + "                     WHERE A.SUB = '16'";
                SQL = SQL + ComNum.VBLF + "                         AND A.BUN = '2'";

                //if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-POL12")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT = '" + cboJepCode.Text + "'";
                //}

                //if (cboJepCode.Text == "A-POL12")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT IN ('A-POL12', 'A-POL12G')";
                //}

                //2019-07-25 코드변경으로 수정

                //if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-PO12GA")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT = '" + cboJepCode.Text + "'";
                //}

                //if (cboJepCode.Text == "A-PO12GA")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT IN ('A-POL12A', 'A-PO12GA', 'A-POL12G')";
                //}

                // 2020-10-05 코드변경으로 수정
                if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-ANE12" && cboJepCode.Text != "A-ANE12G")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT = '" + cboJepCode.Text + "'";
                }

                if (cboJepCode.Text == "A-ANE12")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT IN ('A-POL12A', 'A-ANE12')";
                }
                else if (cboJepCode.Text == "A-ANE12G")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT IN ('A-POL12A', 'A-PO12GA', 'A-POL12G', 'A-ANE12G')";
                }

                SQL = SQL + ComNum.VBLF + "                         AND A.JEPCODE = B.JEPCODE)";

                if (chkTO.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "         AND DEPTCODE != 'TO'";
                }

                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     PTNO, BDATE, DEPTCODE, SUCODE, 'I' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER MST";
                SQL = SQL + ComNum.VBLF + "     WHERE EXISTS";
                SQL = SQL + ComNum.VBLF + "             (SELECT * FROM " + ComNum.DB_MED + "ENDO_JUPMST SUB";
                SQL = SQL + ComNum.VBLF + "                 WHERE RDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                     AND RDATE < TO_DATE('" + dtpDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                     AND MST.BDATE = SUB.BDATE";
                SQL = SQL + ComNum.VBLF + "                     AND MST.PTNO = SUB.PTNO";
                SQL = SQL + ComNum.VBLF + "                     AND MST.DEPTCODE = SUB.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "                     AND SUB.BUSE = '056104')";

                if (chkSaveData.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "         AND NOT EXISTS";
                    SQL = SQL + ComNum.VBLF + "                 (SELECT * FROM " + ComNum.DB_MED + "ENDO_LIST B";
                    SQL = SQL + ComNum.VBLF + "                     WHERE BDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "                         AND BDATE < TO_DATE('" + dtpDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "                         AND MST.PTNO = B.PTNO";
                    SQL = SQL + ComNum.VBLF + "                         AND MST.SUCODE = B.SUCODE)";
                }

                SQL = SQL + ComNum.VBLF + "         AND SUCODE IN";
                SQL = SQL + ComNum.VBLF + "                 (SELECT";
                SQL = SQL + ComNum.VBLF + "                     JEPCODE";
                SQL = SQL + ComNum.VBLF + "                 From " + ComNum.DB_ERP + "DRUG_JEP";
                SQL = SQL + ComNum.VBLF + "                     WHERE CHENGGU = '09'";

                //if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-POL12")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE = '" + cboJepCode.Text + "'";
                //}

                //if (cboJepCode.Text == "A-POL12")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE IN ('A-POL12', 'A-POL12G')";
                //}

                //2019-07-25 코드변경으로 수정
                //if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-PO12GA")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE = '" + cboJepCode.Text + "'";
                //}

                //if (cboJepCode.Text == "A-PO12GA")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE IN ('A-POL12A', 'A-PO12GA', 'A-POL12G')";
                //}

                // 2020-10-05 코드변경으로 수정
                if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-ANE12" && cboJepCode.Text != "A-ANE12G")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE = '" + cboJepCode.Text + "'";
                }

                if (cboJepCode.Text == "A-ANE12")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE IN ('A-POL12A', 'A-ANE12')";
                }
                else if (cboJepCode.Text == "A-ANE12G")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE IN ('A-POL12A', 'A-PO12GA', 'A-POL12G', 'A-ANE12G')";
                }

                SQL = SQL + ComNum.VBLF + "                 Union All";
                SQL = SQL + ComNum.VBLF + "                 SELECT";
                SQL = SQL + ComNum.VBLF + "                     SUNEXT";
                SQL = SQL + ComNum.VBLF + "                 FROM " + ComNum.DB_ERP + "DRUG_JEP A, " + ComNum.DB_ERP + "DRUG_CONVRATE B";
                SQL = SQL + ComNum.VBLF + "                     WHERE A.SUB = '16'";
                SQL = SQL + ComNum.VBLF + "                         AND A.BUN = '2'";

                //if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-POL12")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT = '" + cboJepCode.Text + "'";
                //}

                //if (cboJepCode.Text == "A-POL12")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT IN ('A-POL12', 'A-POL12G')";
                //}

                //2019-07-25 코드변경으로 수정
                //if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-PO12GA")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT = '" + cboJepCode.Text + "'";
                //}

                //if (cboJepCode.Text == "A-PO12GA")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT IN ('A-POL12A', 'A-PO12GA', 'A-POL12G')";
                //}

                // 2020-10-05 코드변경으로 수정
                if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-ANE12" && cboJepCode.Text != "A-ANE12G")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT = '" + cboJepCode.Text + "'";
                }

                if (cboJepCode.Text == "A-ANE12")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT IN ('A-POL12A', 'A-ANE12')";
                }
                else if (cboJepCode.Text == "A-ANE12G")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT IN ('A-POL12A', 'A-PO12GA', 'A-POL12G', 'A-ANE12G')";
                }

                SQL = SQL + ComNum.VBLF + "                         AND A.JEPCODE = B.JEPCODE)";

                if (chkTO.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "         AND DEPTCODE != 'TO'";
                }

                #region 2021-11-11 종검 예약환자 관련 로직 추가
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     PTNO, BDATE, DEPTCODE, SUCODE, 'O' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "HIC_HYANG_APPROVE MST";
                SQL = SQL + ComNum.VBLF + "     WHERE EXISTS";
                SQL = SQL + ComNum.VBLF + "             (SELECT * FROM " + ComNum.DB_MED + "ENDO_JUPMST SUB";
                SQL = SQL + ComNum.VBLF + "                 WHERE RDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                     AND RDATE < TO_DATE('" + dtpDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                     AND MST.BDATE = SUB.BDATE";
                SQL = SQL + ComNum.VBLF + "                     AND MST.PTNO = SUB.PTNO";
                SQL = SQL + ComNum.VBLF + "                     AND MST.DEPTCODE = SUB.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "                     AND SUB.BUSE = '056104')";
                SQL = SQL + ComNum.VBLF + "       AND BDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (chkSaveData.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "         AND NOT EXISTS";
                    SQL = SQL + ComNum.VBLF + "                 (SELECT * FROM " + ComNum.DB_MED + "ENDO_LIST B";
                    SQL = SQL + ComNum.VBLF + "                     WHERE BDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "                         AND BDATE < TO_DATE('" + dtpDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "                         AND MST.PTNO = B.PTNO";
                    SQL = SQL + ComNum.VBLF + "                         AND MST.SUCODE = B.SUCODE)";
                }


                SQL = SQL + ComNum.VBLF + "         AND NOT EXISTS";
                SQL = SQL + ComNum.VBLF + "                     (SELECT 1 FROM " + ComNum.DB_MED + "OCS_OORDER B";
                SQL = SQL + ComNum.VBLF + "                         WHERE BDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                             AND MST.PTNO   = B.PTNO";
                SQL = SQL + ComNum.VBLF + "                             AND MST.SUCODE = B.SUCODE";
                SQL = SQL + ComNum.VBLF + "                      UNION ALL";
                SQL = SQL + ComNum.VBLF + "                     SELECT 1 FROM " + ComNum.DB_MED + "OCS_IORDER B";
                SQL = SQL + ComNum.VBLF + "                         WHERE BDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                             AND MST.PTNO   = B.PTNO";
                SQL = SQL + ComNum.VBLF + "                             AND MST.SUCODE = B.SUCODE)";

                SQL = SQL + ComNum.VBLF + "         AND SUCODE IN";
                SQL = SQL + ComNum.VBLF + "                 (SELECT";
                SQL = SQL + ComNum.VBLF + "                     JEPCODE";
                SQL = SQL + ComNum.VBLF + "                 From " + ComNum.DB_ERP + "DRUG_JEP";
                SQL = SQL + ComNum.VBLF + "                     WHERE CHENGGU = '09'";

                //if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-POL12")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE = '" + cboJepCode.Text + "'";
                //}

                //if (cboJepCode.Text == "A-POL12")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE IN ('A-POL12', 'A-POL12G')";
                //}

                //2019-07-25 코드변경으로 수정
                //if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-PO12GA")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE = '" + cboJepCode.Text + "'";
                //}

                //if (cboJepCode.Text == "A-PO12GA")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE IN ('A-POL12A', 'A-PO12GA', 'A-POL12G')";
                //}

                // 2020-10-05 코드변경으로 수정
                if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-ANE12" && cboJepCode.Text != "A-ANE12G")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE = '" + cboJepCode.Text + "'";
                }

                if (cboJepCode.Text == "A-ANE12")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE IN ('A-POL12A', 'A-ANE12')";
                }
                else if (cboJepCode.Text == "A-ANE12G")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND JEPCODE IN ('A-POL12A', 'A-PO12GA', 'A-POL12G', 'A-ANE12G')";
                }

                SQL = SQL + ComNum.VBLF + "                 Union All";
                SQL = SQL + ComNum.VBLF + "                 SELECT";
                SQL = SQL + ComNum.VBLF + "                     SUNEXT";
                SQL = SQL + ComNum.VBLF + "                 FROM " + ComNum.DB_ERP + "DRUG_JEP A, " + ComNum.DB_ERP + "DRUG_CONVRATE B";
                SQL = SQL + ComNum.VBLF + "                     WHERE A.SUB = '16'";
                SQL = SQL + ComNum.VBLF + "                         AND A.BUN = '2'";

                //if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-POL12")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT = '" + cboJepCode.Text + "'";
                //}

                //if (cboJepCode.Text == "A-POL12")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT IN ('A-POL12', 'A-POL12G')";
                //}

                //2019-07-25 코드변경으로 수정
                //if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-PO12GA")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT = '" + cboJepCode.Text + "'";
                //}

                //if (cboJepCode.Text == "A-PO12GA")
                //{
                //    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT IN ('A-POL12A', 'A-PO12GA', 'A-POL12G')";
                //}

                // 2020-10-05 코드변경으로 수정
                if (cboJepCode.Text != "전체" && cboJepCode.Text != "A-ANE12" && cboJepCode.Text != "A-ANE12G")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT = '" + cboJepCode.Text + "'";
                }

                if (cboJepCode.Text == "A-ANE12")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT IN ('A-POL12A', 'A-ANE12')";
                }
                else if (cboJepCode.Text == "A-ANE12G")
                {
                    SQL = SQL + ComNum.VBLF + "                         AND SUNEXT IN ('A-POL12A', 'A-PO12GA', 'A-POL12G', 'A-ANE12G')";
                }

                SQL = SQL + ComNum.VBLF + "                         AND A.JEPCODE = B.JEPCODE)";

                if (chkTO.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "         AND DEPTCODE != 'TO'";
                }
                #endregion

                SQL = SQL + ComNum.VBLF + "ORDER BY SUCODE, PTNO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strPTNO = dt.Rows[i]["PTNO"].ToString().Trim();
                        strSuCode = dt.Rows[i]["SUCODE"].ToString().Trim();

                        if (strSuCode == "N-PTD25")
                        {
                            strGubun = "마약";
                        }
                        else
                        {
                            strGubun = "향정";
                        }

                        ssView_Sheet1.Cells[i, 1].Text = (i + 1).ToString();
                        ssView_Sheet1.Cells[i, 2].Text = strGubun;
                        ssView_Sheet1.Cells[i, 3].Text = strSuCode;
                        ssView_Sheet1.Cells[i, 4].Text = dtpDate.Value.ToString("yyyy-MM-dd");
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        
                        if (dt.Rows[i]["GUBUN"].ToString().Trim() == "O")
                        {
                            ssView_Sheet1.Cells[i, 6].Text = "외래";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 6].Text = "입원";
                        }

                        ssView_Sheet1.Cells[i, 7].Text = clsVbfunc.GetPatientName(clsDB.DbCon, strPTNO);
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = "1";
                        ssView_Sheet1.Cells[i, 10].Text = READ_MAGAM(clsDB.DbCon, strPTNO, dtpDate.Value.ToString("yyyy-MM-dd"), strSuCode);

                        if (ssView_Sheet1.Cells[i, 10].Text.Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 2].BackColor = Color.Yellow;
                        }
                    }
                }

                if (chkSaveOnly.Checked == true)
                {
                    for (i = 0; i < ssView_Sheet1.RowCount; i++)
                    {
                        if (ssView_Sheet1.Cells[i, 10].Text.Trim() == "")
                        {
                            ssView_Sheet1.Rows[i].Remove();
                            i--;
                        }
                    }
                }

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = "합계";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = dt.Rows.Count.ToString("#,##0");

                dt.Dispose();
                dt = null;
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
            }
        }

        private string READ_MAGAM(PsmhDb pDbCon, string strPano, string strBdate, string strSUCODE)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "ENDO_LIST";
                SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND BDATE = '" + strBdate + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SUCODE = '" + strSUCODE + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == true)
            {
                GetData();
            }
        }

        private bool SaveData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strSuCode = "";
            string strBDate = "";
            string strSname = "";
            string strPTNO = "";

            if (ssView_Sheet1.RowCount == 0) { return rtnVal; }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strSuCode = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    strBDate = ssView_Sheet1.Cells[i, 4].Text.Trim();
                    strSname = ssView_Sheet1.Cells[i, 7].Text.Trim();
                    strPTNO = ssView_Sheet1.Cells[i, 8].Text.Trim();

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true && strSuCode != "")
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "ENDO_LIST";
                        SQL = SQL + ComNum.VBLF + "     (PTNO, BDATE, SUCODE, SNAME) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strBDate + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strSuCode + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strSname + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (DelData() == true)
            {
                GetData();
            }
        }

        private bool DelData()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            if (ssView_Sheet1.RowCount == 0) { return rtnVal; }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true && ssView_Sheet1.Cells[i, 10].Text.Trim() != "")
                    {
                        SQL = "";
                        SQL = "DELETE FROM" + ComNum.DB_MED + "ENDO_LIST";
                        SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + ssView_Sheet1.Cells[i, 10].Text.Trim() + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strHead1 = "";
            int i = 0;
            int intSum = 0;
            int intCount = 1;

            FarPoint.Win.ComplexBorder complexBorder1 =
                new FarPoint.Win.ComplexBorder(
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder2 =
                new FarPoint.Win.ComplexBorder(
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            ssPrint_Sheet1.RowCount = 8;

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true && ssView_Sheet1.Cells[i, 3].Text.Trim() != "합계")
                {
                    ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                    ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = intCount.ToString();
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = ssView_Sheet1.Cells[i, 2].Text;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].Text = ssView_Sheet1.Cells[i, 3].Text;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].Text = ssView_Sheet1.Cells[i, 4].Text;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].Text = ssView_Sheet1.Cells[i, 5].Text;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 6].Text = ssView_Sheet1.Cells[i, 6].Text;

                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].ColumnSpan = 2;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].Text = ssView_Sheet1.Cells[i, 7].Text;

                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 9].Text = ssView_Sheet1.Cells[i, 8].Text;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 10].Text = ssView_Sheet1.Cells[i, 9].Text;

                    intSum = intSum + Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 9].Text.Replace(",", "")));
                    intCount++;

                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0, ssPrint_Sheet1.RowCount - 1, 9].Border = complexBorder1;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 10].Border = complexBorder2;
                }
            }

            if (ssPrint_Sheet1.RowCount == 8) { return; }

            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
            ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 1, ComNum.SPDROWHT);

            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].ColumnSpan = 2;

            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = "합계";
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 10].Text = intSum.ToString();

            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0, ssPrint_Sheet1.RowCount - 1, 9].Border = complexBorder1;
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 10].Border = complexBorder2;

            strFont1 = "/fn\"맑은 고딕\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";

            strHead1 = "/c/f1" + "내시경실 비상 마약류 관리 리스트" + "/f1/n";

            ssPrint_Sheet1.Cells[0, 1].Text = dtpDate.Value.ToString("yyyy년 MM월 dd일") + " 시간 :";

            ssPrint_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssPrint_Sheet1.PrintInfo.Header = strFont1 + strHead1;
            ssPrint_Sheet1.PrintInfo.Margin.Top = 20;
            ssPrint_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssPrint_Sheet1.PrintInfo.Margin.Header = 10;
            ssPrint_Sheet1.PrintInfo.ShowColor = false;
            ssPrint_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPrint_Sheet1.PrintInfo.ShowBorder = false;
            ssPrint_Sheet1.PrintInfo.ShowGrid = false;
            ssPrint_Sheet1.PrintInfo.ShowShadows = false;
            ssPrint_Sheet1.PrintInfo.UseMax = true;
            ssPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPrint_Sheet1.PrintInfo.UseSmartPrint = false;
            ssPrint_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssPrint_Sheet1.PrintInfo.Preview = false;
            ssPrint.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true && e.Column == 0)
            {
                if (Convert.ToBoolean(ssView_Sheet1.ColumnHeader.Cells[0, 0].Value) == true)
                {
                    ssView_Sheet1.ColumnHeader.Cells[0, 0].Value = false;

                    ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 2, 0].Value = false;
                }
                else
                {
                    ssView_Sheet1.ColumnHeader.Cells[0, 0].Value = true;

                    ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 2, 0].Value = true;
                }
            }
            else if (e.Column != 1 && e.Column != 8 && e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
            }
        }

        private void chkSaveOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSaveOnly.Checked == true)
            {
                chkSaveData.Checked = true;
            }
        }
    }
}
