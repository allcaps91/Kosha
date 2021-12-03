using ComBase; //기본 클래스
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Description : 수가정보
    /// Author : 이상훈
    /// Create Date : 2017.10.11
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="Ocs\Frm오더수가정보.frm"/>
    public partial class FrmOrdSugaInfo : Form
    {
        string strSugaCode;
        string strOrdCode;

        double nPrice;  //현재수가
        double nBaseAmt;

        string SQL;
        DataTable dt = null;
        DataTable dt1 = null;
        string SqlErr = "";     //에러문 받는 변수

        public FrmOrdSugaInfo(string sSugaCode, string sOrdCode)
        {
            InitializeComponent();

            strSugaCode = sSugaCode;
            strOrdCode = sOrdCode;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void FrmOrdSugaInfo_Load(object sender, EventArgs e)
        {
            fn_Screen_Clear();

            panHelp.Visible = false;

            if (strSugaCode.Trim() != "" && strSugaCode != null && strOrdCode.Trim() != "" && strOrdCode != null)
            {
                txtSuCode.Text = strSugaCode.ToUpper().Trim();
                lblOrdCode.Text = strOrdCode.ToUpper().Trim();

                fn_Screen_Diplay();
            }
            else
            {
                MessageBox.Show("수가코드를 읽어오지 못했습니다. 재조회를 위해 화면을 종료합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
        }

        void fn_Screen_Clear()
        {
            lblSugaF.Text = "";
            rtxtInfo.Rtf = "";
            pnlSuga.Enabled = true;
            txtSuCode.Text = "";
            txtSuNamek.Text = "";
            txtSuNameE.Text = "";
        }

        void fn_Screen_Diplay()
        {
            try
            {
                SQL = "";
                SQL += " SELECT JONG, REMARK, ROWID                 \r";
                SQL += "   FROM ADMIN.BAS_SIMSAINFOR          \r";
                SQL += "  WHERE SUNEXT = '" + txtSuCode.Text + "'   \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                rtxtInfo.Text = "";
                if (dt.Rows.Count > 0)
                {
                    rtxtInfo.Rtf = dt.Rows[0]["REMARK"].ToString().Trim().Replace("`", "'");
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            try
            {
                SQL = "";
                SQL += " SELECT a.Bun, a.Nu, a.SugbA, a.SugbB, a.SugbC  \r";
                SQL += "      , a.SugbD, a.SugbE, a.SugbC, a.SugbF      \r";
                SQL += "      , b.SUNAMEK, b.SUNAMEE, a.BAMT            \r";
                SQL += "   FROM ADMIN.BAS_SUT a                   \r";
                SQL += "      , ADMIN.BAS_SUN b                   \r";
                SQL += "  WHERE a.SuCode = '" + txtSuCode.Text + "'     \r";
                SQL += "    AND a.SuNext = b.SuNext(+)                  \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon); 
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    fn_Read_DRUG_JEP_REQ_DETAIL(txtSuCode.Text.Trim());
                    return;
                }
                else
                {
                    switch (dt.Rows[0]["SUGBF"].ToString())
                    {
                        case "0":
                            lblSugaF.Text = "0.급여";
                            break;
                        case "1":
                            lblSugaF.Text = "1.비급여";
                            break;
                        case "2":
                            lblSugaF.Text = "2.비급여 (입력 수정 가능)";
                            break;
                        case "3":
                            lblSugaF.Text = "3.100/100";
                            break;
                        default:
                            lblSugaF.Text = "";
                            break;
                    }

                    txtSuNamek.Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                    txtSuNameE.Text = dt.Rows[0]["SUNAMEE"].ToString().Trim();

                    nPrice = double.Parse(dt.Rows[0]["BAMT"].ToString());

                    nBaseAmt = nPrice;
                    //가산금액
                    if (dt.Rows[0]["SUGBA"].ToString() == "1" && dt.Rows[0]["SUGBE"].ToString() == "1")
                    {
                        nPrice = nBaseAmt + (nPrice * 0.25);
                    }

                    if (dt.Rows[0]["SUGBF"].ToString() == "0")
                    {
                        //lblSugaF.Text = "<급여수가> 총액:" + string.Format("{0:###,###,###,##0}", nPrice) + "\r\n" + "  건강보험 50%:" + string.Format("{0:###,###,###,##0}", nPrice * 0.5) + "  건강보험 10%:" + string.Format("{0:###,###,###,##0}", nPrice * 0.1) + " 입원 20%:" + string.Format("{0:###,###,###,##0}", nPrice * 0.2);
                        //2020-10-15 안정수, 전산의뢰 <2020-2276> 심사팀 요청으로 수정 
                        lblSugaF.Text = "<급여수가> 총액:" + string.Format("{0:###,###,###,##0}", nPrice) + "\r\n" + "  건강보험 외래 50%:" + string.Format("{0:###,###,###,##0}", nPrice * 0.5) + " 입원 20%:" + string.Format("{0:###,###,###,##0}", nPrice * 0.2) + "  산정특례 10%:" + string.Format("{0:###,###,###,##0}", nPrice * 0.1);
                    }
                    else if (dt.Rows[0]["SUGBF"].ToString() == "1")
                    {
                        lblSugaF.Text = "<비급여수가> 총액:" + string.Format("{0:###,###,###,##0}", nPrice);
                    }
                    else if (dt.Rows[0]["SUGBF"].ToString() == "2")
                    {
                        lblSugaF.Text = "<보험총액(100/100)수가> 총액:" + string.Format("{0:###,###,###,##0}", nPrice);
                    }

                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void fn_Read_DRUG_JEP_REQ_DETAIL(string strSuNext)
        {
            try
            {
                SQL = "";
                SQL += " SELECT CERT, Contents, Bun, LTDNAME, BCode, JEPCODE, JEPNAME, UNIT1, UNIT2, UNIT3, PART_J  \r";
                SQL += "      , PART_F, PART_O, SDATE, BIGO, PRICE, JEPENAME, SUNGBUN, DOSCODE, JEHYUNG             \r";
                SQL += "   FROM ADMIN.DRUG_JEP_REQ_DETAIL                                                      \r";
                SQL += "  WHERE JEPCODE = '" + strSuNext + "'                                                       \r";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                
                if (dt1.Rows.Count > 0)
                {
                    txtSuNamek.Text = dt1.Rows[0]["JEPNAME"].ToString().Trim();
                    txtSuNameE.Text = dt1.Rows[0]["JEPENAME"].ToString().Trim();

                    nPrice = double.Parse(dt1.Rows[0]["PRICE"].ToString().Trim());
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void txtSuCode_Leave(object sender, EventArgs e)
        {
            txtSuCode.Text = txtSuCode.Text.Trim().ToUpper();

            if (txtSuCode.Text.Trim() == "") return;

            fn_Screen_Diplay();
        }

        private void txtSuCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                txtSuNameE.Focus();
            }
        }

        void btnHelp_Click(object sender, EventArgs e)
        {
            panHelp.Visible = true;
        }

        private void btnPanExit_Click(object sender, EventArgs e)
        {
            panHelp.Visible = false;
        }
    }
}
