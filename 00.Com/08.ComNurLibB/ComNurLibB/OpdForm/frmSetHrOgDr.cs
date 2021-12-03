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
using ComLibB;


namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmSetHrOgDr.cs
    /// Description     : 검진OG의사설정
    /// Author          : 유진호
    /// Create Date     : 2018-01-12
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\Frm검진OG의사설정
    /// </history>
    /// </summary>
    public partial class frmSetHrOgDr : Form
    {
        PsmhVB psmhVB = new PsmhVB();
        private string FstrDrIndex = "";

        public frmSetHrOgDr()
        {
            InitializeComponent();
        }

        private void frmSetHrOgDr_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.Columns[4].Visible = false;
            ssView_Sheet1.Columns[5].Visible = false;
            ssView_Sheet1.Columns[6].Visible = false;
            ssView_Sheet1.Columns[7].Visible = false;

            ComFunc.ReadSysDate(clsDB.DbCon);
                        
            dtpDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            //의사불러오기
            setCombo();

        }

        private void setCombo()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strDoct = "";

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Code,Name FROM KOSMOS_ABC.ABCIF_BCODE  ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='검진_OG의사' ";
                SQL = SQL + ComNum.VBLF + "   AND DelDate IS NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY Code   ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    cboDrSabun.Items.Clear();
                    cboDrSabun.Items.Add("");
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDoct = dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim();
                        cboDrSabun.Items.Add(strDoct);
                        FstrDrIndex = FstrDrIndex + dt.Rows[i]["Code"].ToString().Trim() + "," + (i + 1) + ";";
                    }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            btnSearchClick();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            btnSaveClick();
            btnSearchClick();
            ssView2_Sheet1.RowCount = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearchClick()
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strExcode = "";

            try
            {
                lblWrtno.Text = "";
                lblExCode.Text = "";
                lblinfo.Text = "";
                cboDrSabun.Text = "";

                //'검사코드읽어오기
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Code,Name FROM KOSMOS_ABC.ABCIF_BCODE  ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='검진_OG방문검사' ";
                SQL = SQL + ComNum.VBLF + "   AND DelDate IS NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY Code   ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strExcode = strExcode + dt.Rows[i]["Code"].ToString().Trim() + ",";
                    }

                    if (VB.L(strExcode, ",") > 1) strExcode = VB.Mid(strExcode, 1, VB.Len(strExcode) - 1);
                }

                ssView_Sheet1.RowCount = 0;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.SNAME,a.WRTNO,a.AGE,a.PTNO,a.Pano,b.ExCode,c.Oper_Dept,c.Oper_Dct,c.ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_JEPSU a, " + ComNum.DB_PMPA + "HIC_RESULT b, KOSMOS_ABC.ABCIF_OG_DCT c ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.WRTNO=b.WRTNO(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=c.WRTNO(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.JEPDATE = " + ComFunc.ConvOraToDate(dtpDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND a.DelDate IS NULL  ";
                SQL = SQL + ComNum.VBLF + "   AND a.GbSts NOT IN ('D') ";
                SQL = SQL + ComNum.VBLF + "   AND b.EXCODE IN ( '" + strExcode + "' ) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Oper_Dct"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ExCode"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                ssView2_Sheet1.RowCount = 0;

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private bool btnSaveClick()
        {
            bool rtVal = false;
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strExcode = "";
            int nWRTNO = 0;
            int nHPano = 0;
            string strPANO = "";
            string strROWID = "";


            strExcode = lblExCode.Text;
            nWRTNO = Convert.ToInt32(VB.Val(lblWrtno.Text));

            if (nWRTNO == 0)
            {
                ComFunc.MsgBox("해당환자를 선택해 주세요.");
                return rtVal;
            }

            if (strExcode == "")
            {
                ComFunc.MsgBox("해당환자를 선택해 주세요.");
                return rtVal;
            }

            strROWID = ssView2_Sheet1.Cells[0, 5].Text;
            strPANO = ssView2_Sheet1.Cells[0, 0].Text;
            nHPano = Convert.ToInt32(VB.Val(ssView2_Sheet1.Cells[0, 2].Text));

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strROWID != "")
                {
                    SQL = " UPDATE KOSMOS_ABC.ABCIF_OG_DCT SET ";
                    if (VB.Trim(cboDrSabun.Text) == "")
                    {
                        SQL = SQL + "  Oper_Dept ='', ";
                    }
                    else
                    {
                        SQL = SQL + "  Oper_Dept ='011103', ";      //'산부인과
                    }
                    SQL = SQL + "  HPano = " + nHPano + ", ";
                    SQL = SQL + "  Pano = '" + strPANO + "', ";
                    SQL = SQL + "  Oper_Dct ='" + VB.Trim(psmhVB.Pstr(cboDrSabun.Text, ".", 1)) + "' ";
                    SQL = SQL + " WHERE WRTNO =" + nWRTNO + " ";
                    SQL = SQL + "  AND ExCode ='" + strExcode + "' ";
                }
                else
                {
                    SQL = " INSERT INTO KOSMOS_ABC.ABCIF_OG_DCT (JepDate,PANO,WRTNO,HPANO,GUBUN,EXCODE,OPER_DEPT,OPER_DCT ) VALUES ( ";
                    SQL = SQL + ComFunc.ConvOraToDate(dtpDate.Value,"D") + ", ";
                    SQL = SQL + " '" + strPANO + "', " + nWRTNO + ", " + nHPano + ",'1', ";
                    SQL = SQL + " '" + strExcode + "', ";
                    if (VB.Trim(cboDrSabun.Text) == "")
                    {
                        SQL = SQL + " '', ";
                    }
                    else
                    {
                        SQL = SQL + " '011103', ";      //'산부인과
                    }
                    SQL = SQL + " '" + VB.Trim(psmhVB.Pstr(cboDrSabun.Text, ".", 1)) + "') ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;

            lblWrtno.Text = "";
            lblExCode.Text = "";
            lblinfo.Text = "";
            cboDrSabun.Text = "";

            ssView2_Sheet1.RowCount = 1;
            lblinfo.Text = "【등록번호】" + ssView_Sheet1.Cells[e.Row, 1].Text;
            ssView2_Sheet1.Cells[0,0].Text = ssView_Sheet1.Cells[e.Row, 1].Text;
            lblinfo.Text = lblinfo.Text + " " + ssView_Sheet1.Cells[e.Row, 0].Text;
            lblinfo.Text = lblinfo.Text + " 나이:" + ssView_Sheet1.Cells[e.Row, 2].Text;

            if(ssView_Sheet1.Cells[e.Row, 3].Text != "")
            {
                cboDrSabun.SelectedIndex = Convert.ToInt32(VB.Val(psmhVB.Pstr(psmhVB.Pstr(psmhVB.Pstr(FstrDrIndex, VB.Trim(ssView_Sheet1.Cells[e.Row, 3].Text), 2), ";", 1), ",", 2)));
            }

            lblWrtno.Text = ssView_Sheet1.Cells[e.Row, 4].Text;
            ssView2_Sheet1.Cells[0, 1].Text = ssView_Sheet1.Cells[e.Row, 4].Text;
            lblExCode.Text = ssView_Sheet1.Cells[e.Row, 5].Text;
            ssView2_Sheet1.Cells[0, 3].Text = ssView_Sheet1.Cells[e.Row, 5].Text;
            ssView2_Sheet1.Cells[0, 2].Text = ssView_Sheet1.Cells[e.Row, 6].Text;
            ssView2_Sheet1.Cells[0, 5].Text = ssView_Sheet1.Cells[e.Row, 7].Text;            
        }
    }
}
