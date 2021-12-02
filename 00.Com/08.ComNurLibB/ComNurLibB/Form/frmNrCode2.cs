using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;

namespace ComNurLibB
{
    public partial class frmNrCode2 : Form
    {
        frmNrCode NC = new frmNrCode();

        public frmNrCode2()
        {
            InitializeComponent();
        }

        private void frmNrCode2_Load(object sender, EventArgs e)
        {
            for (int i = 6; i < 11; i++)
            {
                Spd.Columns[i].Visible = false;
            }

            cboJong.Items.Clear();
            cboJong.Items.Add("A.주간당직");
            cboJong.Items.Add("A.저간당직");

            cboJong.SelectedIndex = 0;

            btnSearch.Enabled = true;
            btnRegist.Enabled = false;
            btnCancel.Enabled = false;
            ssList.Enabled = false;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            NC.SCREEN_CLEAR();
            btnSearch.Enabled = true;
            btnRegist.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;

            //Spd -> ssNrCode2, Spd2 -> ssList
            for (int i = 0; i < Spd2.Rows.Count; i++)
            {
                for (int j = 0; j < Spd2.Columns.Count; j++)
                {
                    Spd2.Cells[i, j].Text = "";
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        void Search()
        {
            int i = 0;


            string strSabun = "";
            string strName = "";

            NC.SCREEN_CLEAR();
            btnSearch.Enabled = false;
            btnRegist.Enabled = true;
            btnCancel.Enabled = true;
            btnExit.Enabled = false;

            DataTable dt = null;
            DataTable dt2 = null;
            string strSql = string.Empty;

            Spd.Rows.Count = 0;
            Spd2.Rows.Count = 0;

            strSql = "";
            strSql = strSql + ComNum.VBLF + "SELECT ";
            strSql = strSql + ComNum.VBLF + "    Sabun, KorName, Jik ";
            strSql = strSql + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST ";
            strSql = strSql + ComNum.VBLF + " WHERE JIK IN ('04','31','32','33') ";
            strSql = strSql + ComNum.VBLF + " AND Buse IN ( ";
            strSql = strSql + ComNum.VBLF + "SELECT ";
            strSql = strSql + ComNum.VBLF + " MATCH_CODE ";
            strSql = strSql + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE ";
            strSql = strSql + ComNum.VBLF + " WHERE GUBUN = '2') ";
            strSql = strSql + ComNum.VBLF + " AND ToiDay is NULL ";
            strSql = strSql + ComNum.VBLF + " ORDER BY KorName ";
            dt = clsDB.GetDataTable(strSql);

            if (dt == null)
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }


            for (i = 0; i < dt.Rows.Count; i++)
            {
                Spd2.Rows.Count += 1;
                //// 사번
                //strData1 = VB.Left(dt.Rows[0]["Sabun"].ToString().Trim() + VB.Space(8), 8);
                //// 사번 + 성명
                //strData2 = strData1 + VB.Left(dt.Rows[0]["KorName"].ToString().Trim() + "" + VB.Space(10), 10);
                //// 사번 + 성명 + 직종
                //strData3 = strData2 + VB.Left(dt.Rows[0]["Jik"].ToString().Trim() + VB.Space(2), 2);      


                // 사번
                strSabun = dt.Rows[i]["Sabun"].ToString().Trim();
                // 사번 + 성명
                strName = dt.Rows[i]["KorName"].ToString().Trim();

                Spd2.Cells[i, 0].Text = strSabun;
                Spd2.Cells[i, 1].Text = strName;
            }

            dt.Dispose();
            dt = null;


            strSql = "";
            strSql = strSql + ComNum.VBLF + "SELECT ";
            strSql = strSql + ComNum.VBLF + " Code, Name, Jik, PrintRanking, ROWID ";
            strSql = strSql + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE ";
            strSql = strSql + ComNum.VBLF + " WHERE GUBUN = '" + VB.Left(cboJong.SelectedItem.ToString(), 1) + "' ";
            strSql = strSql + ComNum.VBLF + " ORDER BY PrintRanking, Jik, Name ";
            dt2 = clsDB.GetDataTable(strSql);

            if (dt2 == null)
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            for (i = 0; i < dt2.Rows.Count; i++)
            {
                Spd.Rows.Count += 1;
                Spd.Cells[i, 0].Text = "";
                Spd.Cells[i, 1].Text = dt2.Rows[i]["PrintRanking"].ToString().Trim();
                Spd.Cells[i, 2].Text = dt2.Rows[i]["Code"].ToString().Trim();
                Spd.Cells[i, 3].Text = dt2.Rows[i]["Name"].ToString().Trim();
                Spd.Cells[i, 4].Text = dt2.Rows[i]["Jik"].ToString().Trim();
                Spd.Cells[i, 5].Text = READ_JikName(dt2.Rows[i]["Jik"].ToString().Trim());
                Spd.Cells[i, 6].Text = dt2.Rows[i]["Rowid"].ToString().Trim();
                Spd.Cells[i, 7].Text = dt2.Rows[i]["PrintRanking"].ToString().Trim();
                Spd.Cells[i, 8].Text = dt2.Rows[i]["Code"].ToString().Trim();
                Spd.Cells[i, 9].Text = dt2.Rows[i]["Name"].ToString().Trim();
                Spd.Cells[i, 10].Text = dt2.Rows[i]["Jik"].ToString().Trim();

            }

            dt2.Dispose();
            dt2 = null;

            ssList.Enabled = true;
            ssNrCode2.Enabled = true;

        }

        /// <summary>
        /// 직무코드 -> 직무명으로
        /// </summary>
        /// <param name="Jik"></param>
        /// <returns></returns>
        string READ_JikName(string Jik)
        {
            int i = 0;
            string strSql = string.Empty;
            DataTable dt = null;

            string rntVal = "";

            strSql = "";
            strSql = strSql + ComNum.VBLF + "SELECT ";
            strSql = strSql + ComNum.VBLF + " Name cJikName ";
            strSql = strSql + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE ";
            strSql = strSql + ComNum.VBLF + " WHERE GUBUN = '1' ";
            strSql = strSql + ComNum.VBLF + " AND Code = '" + Jik + "' ";
            dt = clsDB.GetDataTable(strSql);


            if (dt == null)
            {
                rntVal = "";
            }
            if (dt.Rows.Count > 0)
            {
                rntVal = dt.Rows[0]["cJikName"].ToString().Trim();
            }

            return rntVal;
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            SaveData(Spd2);
        }

        void SaveData(FarPoint.Win.Spread.SheetView Spd2)
        {
            int i, j;
            string strOK = "";

            string strCode = "", strName = "";
            string strRowid = "";
            string strOldCode = "", strOldName = "";
            string strDel = "";
            int nSeq = 0, nOldSeq = 0;
            string strJik = "", strOldJik = "";

            DataTable dt = null;

            string strSql = string.Empty;

            btnRegist.Enabled = false;
            btnCancel.Enabled = false;

            for (i = 0; i < Spd2.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data + 1); i++)
            {
                strDel = Spd2.Cells[i, 0].Text == "True" ? "1" : "0";
                if (Spd2.Cells[i, 1].Text != "")
                {
                    nSeq = Convert.ToInt16(Spd2.Cells[i, 1].Text);
                }
                strCode = Spd2.Cells[i, 2].Text;
                strName = Spd2.Cells[i, 3].Text;
                strJik = Spd2.Cells[i, 4].Text;
                strRowid = Spd2.Cells[i, 6].Text;
                if (Spd2.Cells[i, 7].Text != "")
                {
                    nOldSeq = Convert.ToInt16(Spd2.Cells[i, 7].Text);
                }

                strOldCode = Spd2.Cells[i, 8].Text.Trim();
                strOldName = Spd2.Cells[i, 9].Text.Trim();
                strOldJik = Spd2.Cells[i, 10].Text.Trim();


                if (strCode.Trim() == "99")
                {
                    i = i;
                }

                // 기존데이터 삭제
                if (strDel == "1" && strRowid != "")
                {
                    clsDB.setBeginTran();
                    try
                    {
                        strSql = "";
                        strSql = strSql + ComNum.VBLF + " DELETE FROM ";
                        strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CODE ";
                        strSql = strSql + ComNum.VBLF + "WHERE ROWID = '" + strRowid + "' ";
                        clsDB.ExecuteNonQuery(strSql);

                        clsDB.setCommitTran();
                        ComFunc.MsgBox("삭제하였습니다.");
                    }
                    catch (Exception e)
                    {
                        clsDB.setRollbackTran();
                        ComFunc.MsgBox("");
                    }

                }

                // 기존데이터 수정 or 신규등록
                else if (strDel != "1" && strCode != "")
                {
                    strOK = "NO";
                    if (strCode != strOldCode) strOK = "OK";
                    if (strName != strOldName) strOK = "OK";
                    if (nSeq != nOldSeq) strOK = "OK";
                    if (strJik != strOldJik) strOK = "OK";
                    if (strOK == "OK")
                    {
                        //신규등록
                        if (strRowid == "")
                        {
                            clsDB.setBeginTran();
                            try
                            {
                                strSql = "";
                                strSql = strSql + ComNum.VBLF + "INSERT INTO ";
                                strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CODE ";
                                strSql = strSql + ComNum.VBLF + " (Gubun, Code, Name, Jik, PrintRanking) ";
                                strSql = strSql + ComNum.VBLF + " VALUES ('" + VB.Left(cboJong.Text, 1) + "','" + strCode + "','" + strName + "','" + strJik + "' ," + nSeq + " )";
                                clsDB.ExecuteNonQuery(strSql);

                                clsDB.setCommitTran();
                                ComFunc.MsgBox("등록하였습니다.");
                            }
                            catch (Exception e)
                            {
                                clsDB.setRollbackTran();
                                ComFunc.MsgBox("");
                            }
                        }
                        //기존데이터 수정
                        else if (strRowid != "")
                        {
                            clsDB.setBeginTran();
                            try
                            {
                                strSql = "";
                                strSql = strSql + ComNum.VBLF + "UPDATE ";
                                strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CODE ";
                                strSql = strSql + ComNum.VBLF + " SET Code = '" + strCode + "', ";
                                strSql = strSql + ComNum.VBLF + " Name = '" + strName + "', Jik = " + strJik + " ,PrintRanking = " + nSeq + " ";
                                strSql = strSql + ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";
                                clsDB.ExecuteNonQuery(strSql);

                                clsDB.setCommitTran();
                                ComFunc.MsgBox("수정하였습니다.");
                            }
                            catch (Exception e)
                            {
                                clsDB.setRollbackTran();
                                ComFunc.MsgBox("");
                            }
                        }
                    }
                }

                NC.SCREEN_CLEAR();

                btnSearch.Enabled = true;
                btnRegist.Enabled = false;
                btnCancel.Enabled = false;
                btnExit.Enabled = true;
            }
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i;

            string strOK = "";
            string strDel = "";
            string strSabun1 = "";
            string strSabun2 = "";
            string strName = "";
            string strJik = "";

            strSabun1 = Spd2.Cells[e.Row, 0].Text;
            strName = Spd2.Cells[e.Row, 1].Text;
            strJik = Spd.Cells[e.Row, 4].Text;

            strOK = "OK";

            for(i = 0; i < Spd.Rows.Count; i++)
            {
                strDel = Spd2.Cells[i, 0].Text == "True" ? "1" : "0";
                //strSabun2 = 
            }

        }
    }
}
