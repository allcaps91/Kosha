using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\nurse\nrtong\nrmain.vbp\FrmEmIljiPrint.frm >> frmEmIljiPrint.cs 폼이름 재정의" />

    public partial class frmEmIljiPrint : Form
    {
        //int nRow = 0;
        //int nCol = 0;

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        public frmEmIljiPrint()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int J = 0;
            string strNowDate = "";
            int nSSRow = 0;
            int[] nTotal = new int[6];
            int[] nTotal2 = new int[6];
            int i = 0;
            //int k = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Clear();

            dtpDate.Visible = false;
            btnSearch.Visible = false;

            strNowDate = dtpDate.Value.ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT DEPT, INWON11, INWON12, INWON13, INWON14, INWON15, INWON21, INWON22,";
                SQL = SQL + ComNum.VBLF + " INWON23, INWON24, INWON25, INWON31, INWON32, INWON33, INWON34, INWON35,";
                SQL = SQL + ComNum.VBLF + " INWON41, INWON42, INWON43, INWON44, INWON45, INWON51, INWON52, INWON53,";
                SQL = SQL + ComNum.VBLF + " INWON54, INWON55, OP, DAMA, TS, DEATH, DOA, PT, Rowid";
                SQL = SQL + ComNum.VBLF + " FROM NUR_EMINWON";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE =TO_DATE('" + strNowDate + "','YYYY-MM-DD')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i]["dept"].ToString().Trim())
                    {
                        case "MD":
                            nSSRow = 3;
                            break;
                        case "GS":
                            nSSRow = 4;
                            break;
                        case "OG":
                            nSSRow = 5;
                            break;
                        case "PD":
                            nSSRow = 6;
                            break;
                        case "OS":
                            nSSRow = 7;
                            break;
                        case "NS":
                            nSSRow = 8;
                            break;
                        case "NE":
                            nSSRow = 9;
                            break;
                        case "CS":
                            nSSRow = 10;
                            break;
                        case "NP":
                            nSSRow = 11;
                            break;
                        case "UR":
                            nSSRow = 12;
                            break;
                        case "EN":
                            nSSRow = 13;
                            break;
                        case "OT":
                            nSSRow = 14;
                            break;
                        case "DM":
                            nSSRow = 15;
                            break;
                        case "DT":
                            nSSRow = 16;
                            break;
                        case "PC":
                            nSSRow = 17;
                            break;
                        case "RM":
                            nSSRow = 18;
                            break;
                        case "ER":
                            nSSRow = 19;
                            break;
                    }

                    //입원
                    SS1_Sheet1.Cells[nSSRow - 1, 1].Text = dt.Rows[i]["Inwon11"].ToString().Replace(",", "");
                    nTotal[1] = nTotal[1] + Convert.ToInt32(dt.Rows[i]["Inwon11"].ToString().Replace(",", ""));
                    SS1_Sheet1.Cells[nSSRow - 1, 2].Text = dt.Rows[i]["Inwon12"].ToString().Replace(",", "");
                    nTotal[2] = nTotal[2] + Convert.ToInt32(dt.Rows[i]["Inwon11"].ToString().Replace(",", ""));
                    SS1_Sheet1.Cells[nSSRow - 1, 3].Text = dt.Rows[i]["Inwon13"].ToString().Replace(",", "");
                    nTotal[3] = nTotal[3] + Convert.ToInt32(dt.Rows[i]["Inwon11"].ToString().Replace(",", ""));
                    SS1_Sheet1.Cells[nSSRow - 1, 4].Text = dt.Rows[i]["Inwon14"].ToString().Replace(",", "");
                    nTotal[4] = nTotal[4] + Convert.ToInt32(dt.Rows[i]["Inwon11"].ToString().Replace(",", ""));
                    SS1_Sheet1.Cells[nSSRow - 1, 5].Text = dt.Rows[i]["Inwon15"].ToString().Replace(",", "");
                    nTotal[5] = nTotal[5] + Convert.ToInt32(dt.Rows[i]["Inwon11"].ToString().Replace(",", ""));

                    //퇴원
                    SS1_Sheet1.Cells[nSSRow - 1, 7].Text = dt.Rows[i]["Inwon21"].ToString().Replace(",", "");
                    nTotal[1] = nTotal[1] + Convert.ToInt32(dt.Rows[i]["Inwon21"].ToString().Replace(",", ""));
                    SS1_Sheet1.Cells[nSSRow - 1, 8].Text = dt.Rows[i]["Inwon22"].ToString().Replace(",", "");
                    nTotal[2] = nTotal[2] + Convert.ToInt32(dt.Rows[i]["Inwon22"].ToString().Replace(",", ""));
                    SS1_Sheet1.Cells[nSSRow - 1, 9].Text = dt.Rows[i]["Inwon23"].ToString().Replace(",", "");
                    nTotal[3] = nTotal[3] + Convert.ToInt32(dt.Rows[i]["Inwon23"].ToString().Replace(",", ""));
                    SS1_Sheet1.Cells[nSSRow - 1, 10].Text = dt.Rows[i]["Inwon24"].ToString().Replace(",", "");
                    nTotal[4] = nTotal[4] + Convert.ToInt32(dt.Rows[i]["Inwon24"].ToString().Replace(",", ""));
                    SS1_Sheet1.Cells[nSSRow - 1, 11].Text = dt.Rows[i]["Inwon25"].ToString().Replace(",", "");
                    nTotal[5] = nTotal[5] + Convert.ToInt32(dt.Rows[i]["Inwon25"].ToString().Replace(",", ""));

                    //Keep
                    SS1_Sheet1.Cells[nSSRow - 1, 13].Text = dt.Rows[i]["Inwon51"].ToString().Replace(",", "");
                    nTotal[1] = nTotal[1] + Convert.ToInt32(dt.Rows[i]["Inwon51"].ToString().Replace(",", ""));
                    SS1_Sheet1.Cells[nSSRow - 1, 14].Text = dt.Rows[i]["Inwon52"].ToString().Replace(",", "");
                    nTotal[2] = nTotal[2] + Convert.ToInt32(dt.Rows[i]["Inwon52"].ToString().Replace(",", ""));
                    SS1_Sheet1.Cells[nSSRow - 1, 15].Text = dt.Rows[i]["Inwon53"].ToString().Replace(",", "");
                    nTotal[3] = nTotal[3] + Convert.ToInt32(dt.Rows[i]["Inwon53"].ToString().Replace(",", ""));
                    SS1_Sheet1.Cells[nSSRow - 1, 16].Text = dt.Rows[i]["Inwon54"].ToString().Replace(",", "");
                    nTotal[4] = nTotal[4] + Convert.ToInt32(dt.Rows[i]["Inwon54"].ToString().Replace(",", ""));
                    SS1_Sheet1.Cells[nSSRow - 1, 17].Text = dt.Rows[i]["Inwon55"].ToString().Replace(",", "");
                    nTotal[5] = nTotal[5] + Convert.ToInt32(dt.Rows[i]["Inwon55"].ToString().Replace(",", ""));

                    //total
                    SS1_Sheet1.Cells[nSSRow - 1, 19].Text = nTotal[1].ToString();
                    SS1_Sheet1.Cells[nSSRow - 1, 20].Text = nTotal[2].ToString();
                    SS1_Sheet1.Cells[nSSRow - 1, 21].Text = nTotal[3].ToString();
                    SS1_Sheet1.Cells[nSSRow - 1, 22].Text = nTotal[4].ToString();
                    SS1_Sheet1.Cells[nSSRow - 1, 23].Text = nTotal[5].ToString();

                    //사망
                    SS1_Sheet1.Cells[nSSRow - 1, 25].Text = dt.Rows[i]["Inwon31"].ToString().Replace(",", "");
                    SS1_Sheet1.Cells[nSSRow - 1, 26].Text = dt.Rows[i]["Inwon32"].ToString().Replace(",", "");
                    SS1_Sheet1.Cells[nSSRow - 1, 27].Text = dt.Rows[i]["Inwon33"].ToString().Replace(",", "");
                    SS1_Sheet1.Cells[nSSRow - 1, 28].Text = dt.Rows[i]["Inwon34"].ToString().Replace(",", "");
                    SS1_Sheet1.Cells[nSSRow - 1, 29].Text = dt.Rows[i]["Inwon35"].ToString().Replace(",", "");

                    //전원
                    SS1_Sheet1.Cells[nSSRow - 1, 31].Text = dt.Rows[i]["Inwon41"].ToString().Replace(",", "");
                    SS1_Sheet1.Cells[nSSRow - 1, 32].Text = dt.Rows[i]["Inwon42"].ToString().Replace(",", "");
                    SS1_Sheet1.Cells[nSSRow - 1, 33].Text = dt.Rows[i]["Inwon43"].ToString().Replace(",", "");
                    SS1_Sheet1.Cells[nSSRow - 1, 34].Text = dt.Rows[i]["Inwon44"].ToString().Replace(",", "");
                    SS1_Sheet1.Cells[nSSRow - 1, 35].Text = dt.Rows[i]["Inwon45"].ToString().Replace(",", "");

                    //수술건수
                    SS1_Sheet1.Cells[nSSRow - 1, 37].Text = dt.Rows[i]["op"].ToString().Replace(",", "");

                    //DAMA 건수
                    SS1_Sheet1.Cells[nSSRow - 1, 38].Text = dt.Rows[i]["Dama"].ToString().Replace(",", "");

                    //T/S 건수
                    SS1_Sheet1.Cells[nSSRow - 1, 39].Text = dt.Rows[i]["Ts"].ToString().Replace(",", "");

                    //사망건수
                    SS1_Sheet1.Cells[nSSRow - 1, 40].Text = dt.Rows[i]["Death"].ToString().Replace(",", "");

                    //D.O.A 건수
                    SS1_Sheet1.Cells[nSSRow - 1, 41].Text = dt.Rows[i]["Doa"].ToString().Replace(",", "");

                    //해여Pt 건수
                    SS1_Sheet1.Cells[nSSRow - 1, 42].Text = dt.Rows[i]["Pt"].ToString().Replace(",", "");

                    for (J = 1; J <= 5; J++)
                    {
                        nTotal[J] = 0;
                    }
                }


                //합계
                for (J = 2; J <= 43; J++)
                {
                    for (i = 2; i < SS1_Sheet1.RowCount - 1; i++)
                    {
                        nTotal2[1] = nTotal2[1] + (int)VB.Val(SS1_Sheet1.Cells[i - 1, J - 1].Text);
                    }
                    SS1_Sheet1.Cells[19, J - 1].Text = nTotal2[1].ToString();
                    nTotal2[1] = 0;
                }

                dt.Dispose();
                dt = null;

                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                btnCancel.Enabled = true;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Clear()
        {
            SS1_Sheet1.Cells[2, 1, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Text = "";
        }

        private void frmEmIljiPrint_Load(object sender, EventArgs e)
        {
            dtpDate.Value = Convert.ToDateTime(strDTP);
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = true;
            dtpDate.Enabled = true;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }
    }
}
