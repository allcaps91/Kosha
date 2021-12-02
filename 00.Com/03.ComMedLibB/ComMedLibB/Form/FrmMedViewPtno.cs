using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// File Name       : FrmMedViewPtno.cs
    /// Description     : 환자조회
    /// Author          : 이상훈
    /// Create Date     : 2017-11
    /// <history>       
    /// </history>
    /// <seealso>
    /// frmViewPtno.frm
    /// </seealso>
    /// </summary>
    public partial class FrmMedViewPtno : Form
    {
        string SQL;
        DataTable dt = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수
        
        string FstrSelectPtno;
        string FstrSName;

        clsSpread SP = new clsSpread();

        public FrmMedViewPtno()
        {
            InitializeComponent();
        }

        public FrmMedViewPtno(string strSName)
        {
            InitializeComponent();
            FstrSName = strSName;
        }

        private void FrmMedViewPtno_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등    
            
            if (FstrSName != null && FstrSName.Trim() != "")
            {
                txtSName.Text = FstrSName.Trim();
                txtSName_KeyDown(txtSName, new KeyEventArgs(Keys.Enter));
            }
        }

        void fn_Read_Id()
        {
            string strPtno = "";
            string strSName = "";
            string strJumin = "";
            string strSex = "";
            string strLDate = "";
            string strTel = "";

            try
            {
                SQL = "";
                SQL += " SELECT Pano, Sname, Jumin1, Jumin2,Jumin3, Sex             \r";
                SQL += "      , Tel, TO_CHAR(LastDate,'YYYY-MM-DD') Ldate           \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_PATIENT                             \r";
                SQL += "  WHERE Sname  =  '" + txtSName.Text.Trim() + "'            \r";
                SQL += "    AND LastDate > SysDate - 200                            \r"; //내원일이 210일 안에
                SQL += "  ORDER BY Sname, Jumin1, Jumin2                            \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssList.ActiveSheet.RowCount = dt.Rows.Count;
                    ssList.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strPtno = dt.Rows[i]["PANO"].ToString();
                        strSName = dt.Rows[i]["SNAME"].ToString();
                        if (dt.Rows[i]["JUMIN3"].ToString() != "")
                        {
                            strJumin = dt.Rows[i]["JUMIN1"].ToString() + "-" + clsAES.DeAES(dt.Rows[i]["JUMIN3"].ToString());
                        }
                        else
                        {
                            strJumin = dt.Rows[i]["JUMIN1"].ToString() + "-" + dt.Rows[i]["JUMIN2"].ToString();
                        }
                        strSex = dt.Rows[i]["SEX"].ToString();
                        strTel = dt.Rows[i]["TEL"].ToString();
                        strLDate = dt.Rows[i]["LDATE"].ToString();

                        ssList.ActiveSheet.Cells[i, 0].Text = strPtno;
                        ssList.ActiveSheet.Cells[i, 1].Text = strSName;
                        ssList.ActiveSheet.Cells[i, 2].Text = strJumin;
                        ssList.ActiveSheet.Cells[i, 3].Text = strSex;
                        ssList.ActiveSheet.Cells[i, 4].Text = strLDate;
                        ssList.ActiveSheet.Cells[i, 5].Text = strTel;
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
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList.ActiveSheet.NonEmptyRowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                SP.setSpdSort(ssList, e.Column, true);
                return;
            }
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList.ActiveSheet.NonEmptyRowCount == 0) return;

            if (e == null)
            {
                clsOrdFunction.GstrPatNo = ssList.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, 0].Text;
            }
            else
            {
                clsOrdFunction.GstrPatNo = ssList.ActiveSheet.Cells[e.Row, 0].Text;
            }

            this.Close();
        }
       
        private void ssList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                ssList_CellDoubleClick(sender, null);
            }
        }

        private void txtSName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                fn_Read_Id();
            }
        }
    }
}
