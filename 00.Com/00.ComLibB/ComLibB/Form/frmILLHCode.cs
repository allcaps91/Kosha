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

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB.dll
    /// File Name       : FrmIllHCode.cs
    /// Description     : 희귀난치성 질환자 산정특례 등록 기준
    /// Author          : 이정현
    /// Create Date     : 2017-06-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// frmILLHCode.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\buppat\FrmIllHCode.frm
    /// </seealso>
    /// <vbp>
    /// default : VB\basic\buppat\buppat.vbp
    /// </vbp>
    public partial class frmILLHCode : Form
    {
        private string GstrROWID = "";
        private string FstrIlls = "";
        public frmILLHCode()
        {
            InitializeComponent();
        }

        public frmILLHCode(string argIlls)
        {
            InitializeComponent();
            FstrIlls = argIlls;
        }


        private void frmILLHCode_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            FormClear();

            //if (clsPublic.GstrSysDate.CompareTo( "2021-01-01") >= 0 )
            //{
                rdo2019.Checked = false;
                rdo2021.Checked = true;
            //}
            //else
            //{
                rdo2018.Visible = false;
            //    rdo2019.Visible = false;
            //    rdo2021.Visible = false;
            //    grpGubun.Visible = false;
            //   rdo2021.Checked = false;
            //    rdo2019.Checked = false;
            //    rdo2018.Checked = true;
            //
            if (FstrIlls != "")
            {
                txtIllCode_S.Text = FstrIlls;
                btnView_Click(null, null);
                FstrIlls = "";
            }
            else
            {
                GetData(0, "A");
            }

            //TODO : EXEName
            //if (VB.UCase(App.EXEName) == "BUSUGA")
            //{
            //    btnSave.Visible = true;
            //}
            //else
            //{
            //    btnSave.Visible = false;
            //}
            
        }

        private void FormClear()
        {
            txtIllCode_S.Text = "";

            txtILLCode.Text = "";
            txtVCode.Text = "";
            txtIllNameK.Text = "";
            txtIllNameE.Text = "";
            txtGiJun.Text = "";
            txtGiJun2.Text = "";

            txtHak1.Text = "";
            txtHak2.Text = "";
            txtHak3.Text = "";
            txtHak4.Text = "";

            lbPart.Text = "";

            ssView_Sheet1.RowCount = 0;

            ssGB_Sheet1.Cells[0, 1].Text = "";
            ssGB_Sheet1.Cells[1, 1].Text = "";
            ssGB_Sheet1.Cells[2, 1].Text = "";
            ssGB_Sheet1.Cells[3, 1].Text = "";
            ssGB_Sheet1.Cells[4, 1].Text = "";
            ssGB_Sheet1.Cells[5, 1].Text = "";

            ssGB2_Sheet1.Cells[0, 1].Text = "";
            ssGB2_Sheet1.Cells[1, 1].Text = "";
            ssGB2_Sheet1.Cells[2, 1].Text = "";
            ssGB2_Sheet1.Cells[3, 1].Text = "";
            ssGB2_Sheet1.Cells[4, 1].Text = "";
            ssGB2_Sheet1.Cells[5, 1].Text = "";
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (txtIllCode_S.Text.Trim() == "")
            {
                return;
            }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT ILLCODE, ILLNAMEK, ROWID  ";
                if (rdo2018.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ILLS_H ";
                }
                else if(rdo2019.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ILLS_H2 ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ILLS_H3 ";
                }

                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "   AND ILLCODE = '" + txtIllCode_S.Text + "' ";
                
                if (rdoGubun0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND Gubun ='3'     ";     // 2014변경
                }
                else if (rdoGubun1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND Gubun ='4'     ";     // 2014변경
                }
                if (rdo2019.Checked == true)
                {

                    if (rdoPart1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND PART = '1'";
                    }
                    else if (rdoPart2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND PART = '2'";
                    }
                    else if (rdoPart3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND PART = '3'";
                    }
                    else if (rdoPart4.Checked == true)      //결핵 임의 추가
                    {
                        SQL = SQL + ComNum.VBLF + " AND PART = '0'";
                    }
                    else if (rdoPart5.Checked == true)      //잠복결핵 임의 추가
                    {
                        SQL = SQL + ComNum.VBLF + " AND PART = '4'";
                    }
                    else     //전체
                    {
                        SQL = SQL + ComNum.VBLF + " AND PART IN ('1','2','3','0','4')";
                    }
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

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

        private void rdoGubun_CheckedChanged(object sender, EventArgs e)
        {
            GetData(0, "A");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int intIndex = Convert.ToInt32(VB.Val(VB.Right(((Button)sender).Name, 2)));
            string strText = ((Button)sender).Text;

            GetData(intIndex, strText);
        }

        private void GetData(int intIndex, string strText)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strIllCode = strText + "%";

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT ILLCODE, ILLNAMEK, ROWID   ";
                if (rdo2018.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ILLS_H ";
                }
                else if (rdo2019.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ILLS_H2 ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ILLS_H3 ";
                }
                if (intIndex != 26)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE ILLCODE LIKE  ('" + strIllCode + "' ) ";

                    if (rdoGubun0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND Gubun ='3'     ";
                    }
                    else if (rdoGubun1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND Gubun ='4'     ";
                    }
                }
                else
                {
                    if (rdoGubun0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE Gubun ='3'     ";
                    }
                    else if (rdoGubun1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE Gubun ='4'     ";
                    }
                }
                //if (rdo2019.Checked == true)
                //{

                if (rdoPart1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND PART = '1'";
                }
                else if (rdoPart2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND PART = '2'";
                }
                else if (rdoPart3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND PART = '3'";
                }
                else if (rdoPart4.Checked == true)      //결핵 임의 추가
                {
                    SQL = SQL + ComNum.VBLF + " AND PART = '0'";
                }
                else if (rdoPart5.Checked == true)      //잠복결핵 임의 추가
                {
                    SQL = SQL + ComNum.VBLF + " AND PART = '4'";
                }


                else     //전체
                {
                    SQL = SQL + ComNum.VBLF + " AND PART IN ('1','2','3','0','4')";
                }


                SQL = SQL + ComNum.VBLF + " ORDER BY ILLCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            if (e.ColumnHeader == true || e.RowHeader == true)
            {
                return;
            }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            GstrROWID = ssView_Sheet1.Cells[e.Row, 2].Text;

            try
            {
                SQL = "";
                SQL = "SELECT ILLCODE, ILLNAMEK, ILLNAMEE,VCode, ";
                SQL = SQL + ComNum.VBLF + " GIJUN, GBN1, GBN2, GBN3, GBN4, GBN5, GBN6,GBN7, HAK1, HAK2, HAK3, HAK4, ";
                SQL = SQL + ComNum.VBLF + " GIJUN2, GBN21, GBN22, GBN23, GBN24, GBN25, GBN26, GBN27, ";
                
                if (rdo2018.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " '' PART, '' BIGO, '' BIGO2 ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ILLS_H ";
                }
                else if (rdo2019.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " PART, BIGO, '' BIGO2 ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ILLS_H2 ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " PART, BIGO, BIGO2 ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ILLS_H3 ";
                }
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + GstrROWID + "' ";

                if (rdoGubun0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND Gubun ='3'     ";
                }
                else if (rdoGubun1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND Gubun ='4'     ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                txtILLCode.Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                txtIllNameK.Text = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
                txtIllNameE.Text = dt.Rows[0]["ILLNAMEE"].ToString().Trim();
                txtGiJun.Text = dt.Rows[0]["GIJUN"].ToString().Trim();
                txtBigo.Text = dt.Rows[0]["BIGO"].ToString().Trim();
                txtBigo2.Text = dt.Rows[0]["BIGO2"].ToString().Trim();

                lbPart.Text = "";
                switch (dt.Rows[0]["PART"].ToString().Trim())
                {
                    case "0":
                        lbPart.Text = "결핵";
                        break;
                    case "4":
                        lbPart.Text = "잠복결핵";
                        break;
                    case "1":
                        lbPart.Text = "희귀";
                        break;
                    case "2":
                        lbPart.Text = "중증난치";
                        break;
                    case "3":
                        lbPart.Text = "중증치매";
                        break;

                }

                //if (rdo2019.Checked == true && lbPart.Text.Trim() == "결핵")
                if (lbPart.Text.Trim() == "결핵")
                    {
                    label8.Text = "검사기준";
                    label9.Text = "";
                    ssGB.ActiveSheet.Cells[0, 0].Text = "1.영상검사";
                    ssGB.ActiveSheet.Cells[1, 0].Text = "2-1.도말검사";
                    ssGB.ActiveSheet.Cells[2, 0].Text = "2-2.배양검사";
                    ssGB.ActiveSheet.Cells[3, 0].Text = "3.조직학검사";
                    ssGB.ActiveSheet.Cells[4, 0].Text = "4.임상진단";
                    ssGB.ActiveSheet.Cells[5, 0].Text = "5.기타";
                    ssGB.ActiveSheet.Cells[6, 0].Text = "필수검사조합";

                }
                else if (lbPart.Text.Trim() == "잠복결핵")
                {
                    label8.Text = "검사기준";
                    label9.Text = ""; 

                    ssGB.ActiveSheet.Cells[0, 0].Text = "1.TST";
                    ssGB.ActiveSheet.Cells[1, 0].Text = "2.IGRA";
                    ssGB.ActiveSheet.Cells[2, 0].Text = "3.영상검사상 활동성결핵 아님";
                    ssGB.ActiveSheet.Cells[3, 0].Text = "4.도말/배양검사상 활동성결핵 아님";
                    ssGB.ActiveSheet.Cells[4, 0].Text = "5.조직학적 검사상 활동성결핵 아님";
                    ssGB.ActiveSheet.Cells[5, 0].Text = "6.기타";
                    ssGB.ActiveSheet.Cells[6, 0].Text = "필수검사조합";

                }
                else
                {
                    label8.Text = "신규 등록기준(대한의학회안)";
                    label9.Text = "재등록기준(대한의학회안)";
                    ssGB.ActiveSheet.Cells[0, 0].Text = "1.영상검사";
                    ssGB.ActiveSheet.Cells[1, 0].Text = "2.특수 생화학/면역학검사";
                    ssGB.ActiveSheet.Cells[2, 0].Text = "3.유전학검사";
                    ssGB.ActiveSheet.Cells[3, 0].Text = "4.조직학검사";
                    ssGB.ActiveSheet.Cells[4, 0].Text = "5.임상진단";
                    ssGB.ActiveSheet.Cells[5, 0].Text = "6.기타";
                    ssGB.ActiveSheet.Cells[6, 0].Text = "필수검사조합";
                }
                ssGB_Sheet1.Cells[0, 1].Text = dt.Rows[0]["GBN1"].ToString().Trim();
                ssGB_Sheet1.Cells[1, 1].Text = dt.Rows[0]["GBN2"].ToString().Trim();
                ssGB_Sheet1.Cells[2, 1].Text = dt.Rows[0]["GBN3"].ToString().Trim();
                ssGB_Sheet1.Cells[3, 1].Text = dt.Rows[0]["GBN4"].ToString().Trim();
                ssGB_Sheet1.Cells[4, 1].Text = dt.Rows[0]["GBN5"].ToString().Trim();
                ssGB_Sheet1.Cells[5, 1].Text = dt.Rows[0]["GBN6"].ToString().Trim();
                ssGB_Sheet1.Cells[6, 1].Text = dt.Rows[0]["GBN7"].ToString().Trim();


                //2014-11-06 add
                txtGiJun2.Text = dt.Rows[0]["GIJUN2"].ToString().Trim();

                ssGB2_Sheet1.Cells[0, 1].Text = dt.Rows[0]["GBN21"].ToString().Trim();
                ssGB2_Sheet1.Cells[1, 1].Text = dt.Rows[0]["GBN22"].ToString().Trim();
                ssGB2_Sheet1.Cells[2, 1].Text = dt.Rows[0]["GBN23"].ToString().Trim();
                ssGB2_Sheet1.Cells[3, 1].Text = dt.Rows[0]["GBN24"].ToString().Trim();
                ssGB2_Sheet1.Cells[4, 1].Text = dt.Rows[0]["GBN25"].ToString().Trim();
                ssGB2_Sheet1.Cells[5, 1].Text = dt.Rows[0]["GBN26"].ToString().Trim();
                ssGB2_Sheet1.Cells[6, 1].Text = dt.Rows[0]["GBN27"].ToString().Trim();

                txtHak1.Text = dt.Rows[0]["HAK1"].ToString().Trim();
                txtHak2.Text = dt.Rows[0]["HAK2"].ToString().Trim();
                txtHak3.Text = dt.Rows[0]["HAK3"].ToString().Trim();
                txtHak4.Text = dt.Rows[0]["HAK4"].ToString().Trim();

                txtVCode.Text = dt.Rows[0]["VCODE"].ToString().Trim();

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (GstrROWID != "")
            {
                string SQL = "";
                string SqlErr = "";
                int intRowAffected = 0;

                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(clsDB.DbCon);
                

                try
                {
                    SQL = "";
                    if (rdo2018.Checked == true)
                    {
                        SQL = "UPDATE  ADMIN.BAS_ILLS_H";
                    }
                    else if (rdo2019.Checked == true)
                    {
                        SQL = "UPDATE  ADMIN.BAS_ILLS_H2";
                    }
                    else
                    {
                        SQL = "UPDATE  ADMIN.BAS_ILLS_H3";
                    }
                    
                    SQL = SQL + ComNum.VBLF + "     SET";
                    if (rdo2021.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         BIGO2 = '" + txtBigo2.Text + "',   ";
                    }
                    SQL = SQL + ComNum.VBLF + "         GIJUN = '" + txtGiJun.Text + "',";
                    SQL = SQL + ComNum.VBLF + "         GIJUN2 = '" + txtGiJun2.Text + "'   ";
                    SQL = SQL + ComNum.VBLF + "WHERE  ROWID = '" + GstrROWID + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장하였습니다.");
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void rdo2019_CheckedChanged(object sender, EventArgs e)
        {
            //if (rdo2019.Checked == true)
            //{
            //    grpGubun.Visible = true;
            //}
            //else
            //{
            //    grpGubun.Visible = false;
            //}
        }

        private void rdo2018_CheckedChanged(object sender, EventArgs e)
        {
            //if (rdo2018.Checked == true)
            //{
            //    grpGubun.Visible = false;
            //}
            //else
            //{
            //    grpGubun.Visible = true;
            //}
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }
    }
}
