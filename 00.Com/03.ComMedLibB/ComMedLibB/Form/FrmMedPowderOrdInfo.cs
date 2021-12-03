using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Description : 산제(Powder)불가약품 오더 알림정보
    /// Author : 이상훈
    /// Create Date : 2017.12.06
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="Frm파우더오더정보.frm"/>
    public partial class FrmMedPowderOrdInfo : Form
    {
        string FstrGubun = "";
        string FstrSuCode = "";
        string FstrSts = "";

        int FnRow = 0;
        int FnCol = 0;

        string SQL;
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        ComFunc CF = new ComFunc();
        clsOrdDisp OD = new clsOrdDisp();

        public delegate void PowderNotReason_Input(int nRow, string sReason);
        public static event PowderNotReason_Input PowderNotReason;

        public delegate void PowderOrder_Input(string sSlipNo, string sOrderCode, int nRow, double nQty);
        public static event PowderOrder_Input PowderOrderDisp;

        public FrmMedPowderOrdInfo()
        {
            InitializeComponent();
        }

        public FrmMedPowderOrdInfo(string sGubun, string sSuCode, string sSts, int nRow, int nCol)
        {
            InitializeComponent();

            FstrGubun = sGubun;
            FstrSuCode = sSuCode;
            FstrSts = sSts;
            FnRow = nRow;
            FnCol = nCol;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmMedPowderOrdInfo_Load(object sender, EventArgs e)
        {
            int nRow = 0;
            double nQty = 0;
            double nDiv = 0;

            string strPW = "";
            string strGubun = "";
            string strSucode = "";
            string strChk = "";

            string strUnit = "";
            int nDispSpace = 0;

            string strNameE = "";
            string strNameG = "";
            int j = 0;

            string strNeverPowder = "";

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            for (int i = 3; i < 21; i++)
            {
                ssList_Sheet1.Columns.Get(i).Visible = false;
            }

            this.Size = new Size(620, 249);

            btnReason.Visible = true;
            label2.Visible = true;
            txtReason.Visible = false;

            strChk = "";
            //txtReason.Text = "";

            
            cboReason.Items.Clear();
            cboReason.Items.Add("");
            cboReason.Items.Add("L-tube feeding");
            cboReason.Items.Add("close observation 예정");
            cboReason.SelectedIndex = 0;

            clsPublic.Gstr파우더STS = "";

            strGubun = FstrGubun;
            strSucode = FstrSuCode;

            lblInfo.Text = "";
            lblInfo.Text = "[" + strSucode + "] 산제불가약";

            try
            {
                SQL = "";
                SQL += " SELECT NOT_POWDER_SUB, NOT_POWDER_ETC, NOT_POWDER_NEVER  \r";
                SQL += "   FROM KOSMOS_ADM.DRUG_MASTER4                           \r";
                SQL += "  WHERE JEPCODE = '" + strSucode + "'                     \r"; 
                SQL += "    AND NOT_POWDER = '1'                                  \r";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblInfo.Text = "[" + strSucode + "] 산제불가사유 : " + CF.Read_Bcode_Name(clsDB.DbCon, "DRUG_산재불가사유", dt.Rows[0]["Not_Powder_Sub"].ToString().Trim());

                    strNeverPowder = dt.Rows[0]["NOT_POWDER_NEVER"].ToString().Trim();
                }

                lblGuide.Text = "산제(Powder)불가약 입니다." + "\r\n\r\n" + "그래도 사용 해야 할 경우 사유를 입력하십시오!!" + "\r\n" + "사유를 입력 안하시면 알약으로 처방됩니다.";
                //txtReason.Focus();

                cboReason.SelectedIndex = 0;
                cboReason.Focus();

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
                //SQL = "";
                //SQL += " SELECT R_JepCode                           \r";
                //SQL += "   FROM KOSMOS_ADM.DRUG_MASTER4_REPLACE     \r";
                //SQL += "  WHERE JepCode = '" + strSucode + "'       \r";
                //SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                //if (SqlErr != "")
                //{
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    return;
                //}

                //if (dt1.Rows.Count > 0)
                //{
                //    strChk = "OK";

                //    for (int i = 0; i < dt1.Rows.Count; i++)
                //    {

                //    }
                //}

                SQL = "";
                SQL += " SELECT A.R_JepCode                                         \r";
                SQL += "      , B.OrderCode, B.OrderName, B.OrderNameS, B.GbInput   \r";
                SQL += "      , B.DispSpace, B.GbInfo,    B.GbBoth,     B.Bun       \r";
                SQL += "      , B.NextCode,  B.SuCode,    B.GbDosage,   B.SpecCode  \r";
                SQL += "      , B.Slipno,    B.GbImiv,    B.DispHeader, T.SugbJ     \r";
                SQL += "      , decode(b.GBDOSAGE, '1', KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(B.SPECCODE), '') DOSNAME    \r";
                SQL += "      , decode(b.GBDOSAGE, '1', KOSMOS_OCS.FC_OCS_ODOSAGE_DIV(B.SPECCODE), '') CGBDIV    \r";
                SQL += "   FROM KOSMOS_ADM.DRUG_MASTER4_REPLACE A                   \r";
                SQL += "      , KOSMOS_OCS.OCS_ORDERCODE        B                   \r";
                SQL += "      , KOSMOS_PMPA.BAS_SUT             T                   \r";
                SQL += "  WHERE JepCode = '" + strSucode + "'                       \r";
                SQL += "    AND B.Slipno      IN ('0003','0004','0005')             \r";
                SQL += "    AND B.Seqno      <> 0                                   \r";
                SQL += "    AND B.SendDept   <> 'N'                                 \r";
                SQL += "    AND B.OrderCode = A.R_JEPCODE                           \r";
                SQL += "    AND B.SuCode = T.SuCode(+)                              \r";
                if (clsPublic.GstrWardCode != "ER")
                {
                    SQL += "    AND (T.SugbJ <> '1' OR T.SugbJ IS NULL)             \r";
                }
                SQL += "  ORDER BY B.Seqno                                          \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssList.ActiveSheet.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    strChk = "OK";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["R_JepCode"].ToString().Trim();
                        strUnit = dt.Rows[i]["OrderName"].ToString().Trim();
                        nDispSpace = Convert.ToInt32(dt.Rows[i]["DispSpace"].ToString().Trim());
                        j = 1 + (2 * nDispSpace);   //Space Size 계산

                        strNameE = VB.Space(j) + strUnit + dt.Rows[i]["DispHeader"].ToString().Trim();
                        strNameG = VB.Space(j) + strUnit + dt.Rows[i]["OrderNameS"].ToString().Trim();

                        ssList.ActiveSheet.Cells[i, 2].Text = strNameG;
                        ssList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["OrderCode"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["GbInput"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["GbInfo"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["GbBoth"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["Bun"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["NextCode"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                        //ssList.ActiveSheet.Cells[i, 10].Text = "";
                        ssList.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["GbDosage"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["SpecCode"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 13].Text = dt.Rows[i]["Slipno"].ToString().Trim();
                        //ssList.ActiveSheet.Cells[i, 14].Text = "";
                        ssList.ActiveSheet.Cells[i, 15].Text = dt.Rows[i]["GbImiv"].ToString().Trim();
                        //ssList.ActiveSheet.Cells[i, 16].Text = dt.Rows[i]["DispHeader"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 17].Text = dt.Rows[i]["DispHeader"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 18].Text = dt.Rows[i]["OrderNameS"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 19].Text = VB.Space(j) + strUnit;
                        //ssList.ActiveSheet.Cells[i, 20].Text = "";
                        if (dt.Rows[i]["DOSNAME"].ToString().Trim() != "")
                        {
                            ssList.ActiveSheet.Cells[i, 21].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();
                        }
                        else
                        {
                            ssList.ActiveSheet.Cells[i, 21].Text = dt.Rows[i]["SpecCode"].ToString().Trim();
                        }
                        ssList.ActiveSheet.Cells[i, 22].Text = dt.Rows[i]["CGBDIV"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 23].Text = dt.Rows[i]["CGBDIV"].ToString().Trim();
                    }
                }
                
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            //대체약 있을경우
            if (strChk == "OK")
            {
                this.Size = new Size(620, 464);
                
                //label2.Visible = false;
                //txtReason.Visible = false;
                //btnReason.Visible = false;

                lblGuide.Text = "대체약이 있는 산제(Powder) 불가약품입니다.." + "\r\n\r\n" + "대체약을 선택후 전송하십시오!!" + "\r\n\r\n" + "그래도 사용 해야 할 경우 사유를 입력하십시오!!" + "\r\n" + "사유를 입력 안하시면 알약으로 처방됩니다.";
                
            }

            //2019-08-13 산제절대불가
            if (strNeverPowder == "1")
            {
                cboReason.Enabled = false;
                btnReason.Enabled = false;
                lblGuide.Text = "산제(POWDER) 절대 불가약품 입니다." + "\r\n\r\n" + "대체약을 선택후 전송하십시오!!" + "\r\n" + "대체약을 선택하지 않을경우 알약으로 처방됩니다.";
            }
        }

        private void btnOrdSend_Click(object sender, EventArgs e)
        {
            int nCnt = 0;
            string strOK = "";
            int[] nSelectRow = new int[500];
            string strNextOK = "";
            string strSlipNo = "";
            double nQty = 0;
            double nDiv = 0;
            double nQty2 = 0;

            //for (int i = 0; i < 500; i++)
            //{
            //    nSelectRow[i] = 0;
            //}

            for (int i = 0; i < ssList.ActiveSheet.NonEmptyRowCount; i++)
            {
                strOK = "OK";
                if (ssList.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    strOK = "NO";
                }
                else
                {
                    nCnt += 1;
                }

                if (ssList.ActiveSheet.Cells[i, 2].Text == "")
                {
                    strOK = "NO";
                }

                if (strOK == "OK")
                {
                    strNextOK = "OK";
                }
            }

            if (nCnt > 1)
            {
                MessageBox.Show("대체약은 한개만 선택하여 전송하십시오!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < ssList.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssList.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strSlipNo = ssList.ActiveSheet.Cells[nSelectRow[i], 13].Text;
                    nQty = int.Parse(ssList.ActiveSheet.Cells[nSelectRow[i], 22].Text);
                    nDiv = int.Parse(ssList.ActiveSheet.Cells[nSelectRow[i], 23].Text);

                    nQty2 = 0;

                    if (nQty > 0 && nDiv > 0)
                    {
                        nQty2 = double.Parse(string.Format("{0:###0.##}", nQty / nDiv));

                        if (nQty2 >= 1 || nQty2 == 0.5 || nQty2 == 0.33 || nQty2 == 0.66 || nQty2 == 0.25 ||
                            nQty2 == 0.75 || nQty2 == 0.2 || nQty2 == 0.4 || nQty2 == 0.6 || nQty2 == 0.8) 
                        {
                        }
                        else
                        {
                            nQty2 = 1;
                        }
                    }

                    clsPublic.Gstr파우더_SuCode = ssList.ActiveSheet.Cells[i, 3].Text.Trim();
                    clsPublic.Gstr파우더Gubun = "대체오더전송";

                    PowderOrderDisp(strSlipNo, ssList.ActiveSheet.Cells[i, 3].Text.Trim(), FnRow, nQty);
                }

                clsPublic.Gstr파우더STS = "OK";

                this.Close();
            }
        }

        private void btnReason_Click(object sender, EventArgs e)
        {
            //if (txtReason.Text.Trim() == "")
            //{
            //    MessageBox.Show("사유를 입력 하십시오!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    txtReason.Focus();
            //    return;
            //}

            //PowderNotReason(FnRow, txtReason.Text.Trim());

            if (cboReason.Text.Trim() == "")
            {
                MessageBox.Show("사유를 입력 하십시오!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboReason.Focus();
                return;
            }

            PowderNotReason(FnRow, cboReason.Text.Trim());

            this.Close();
        }
    }
}
