using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Class Name : frmContractorDecreaseCriterion
/// File Name : frmContractorDecreaseCriterion.cs
/// Title or Description : 계약처 감액기준 등록 페이지
/// Author : 박성완
/// Create Date : 2017-06-09
/// <history> 
/// </history>
/// </summary>
namespace ComLibB
{
    public partial class frmContractorDecreaseCriterion : Form
    {
        string fstrCode = "";
        string fstrROWID = "";

        public frmContractorDecreaseCriterion()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 화면정리 함수
        /// </summary>
        private void Screen_Clear()
        {
            lblLTD.Text = "";
            fstrROWID = "";

            dtpDate.Text = "";
            txtBi51.Text = "";
            txtBi33.Text = "";
            txtSono.Text = "";
            txtMRI.Text = "";
            txtFood.Text = "";
            txtRoom.Text = "";
            txtDT1.Text = "";
            txtDT2.Text = "";
            txtDT3.Text = "";
            txtBonin.Text = "";

            grbDate.Enabled = true;
            btnNew.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
        }

        /// <summary>
        /// 리스트박스 및 출력하기 위한 스프레드에 데이터 셋팅
        /// </summary>
        private void List_Set()
        {
            string strList = "";
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            SQL = "SELECT MiaCode,MiaName,TO_CHAR(DelDate,'YYYY-MM-DD') DelDate ";
            SQL = SQL + ComNum.VBLF + "  FROM BAS_MIA ";
            SQL = SQL + ComNum.VBLF + " WHERE MiaClass='90' ";
            SQL = SQL + ComNum.VBLF + "   AND MiaCode LIKE 'H%' ";
            if (chkDelLtd.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " AND DelDate IS NULL ";
            }
            if (optSort0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY MiaName,MiaCode ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY MiaCode ";
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

            ssPrint_Sheet1.Rows.Count = dt.Rows.Count;
            lst1.Items.Clear();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strList = dt.Rows[i]["MiaCode"].ToString().Trim() + " ";
                strList = strList + dt.Rows[i]["MiaName"].ToString().Trim() + " ";
                strList = strList + dt.Rows[i]["DelDate"].ToString().Trim();

                lst1.Items.Add(strList);

                ssPrint_Sheet1.Cells[i, 0].Text = dt.Rows[i]["MiaCode"].ToString().Trim();
                ssPrint_Sheet1.Cells[i, 1].Text = dt.Rows[i]["MiaName"].ToString().Trim();
                ssPrint_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DelDate"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 기존 등록된 감액 기준을 읽는다.
        /// </summary>
        private void Combo_Date_Set()
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "SELECT TO_CHAR(SDate,'YYYY-MM-DD') SDate ";
            SQL = SQL + ComNum.VBLF + " FROM BAS_GAMLTD ";
            SQL = SQL + ComNum.VBLF + "WHERE LtdCode='" + fstrCode + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY SDate DESC ";
            SQL = SQL + ComNum.VBLF + "";

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
            cboDate.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboDate.Items.Add(dt.Rows[i]["SDate"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                grbDate.Enabled = true;
                cboDate.Enabled = true;
                btnNew.Enabled = true;
                return;
            }
            else
            {
                dt.Dispose();
                dt = null;
                cboDate.Text = "";
                cboDate.Enabled = false;
            }
        }

        private void Screen_Display()
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "SELECT TO_CHAR(SDate,'YYYY-MM-DD') SDate ";
            SQL = SQL + ComNum.VBLF + " FROM BAS_GAMLTD ";
            SQL = SQL + ComNum.VBLF + "WHERE LtdCode='" + fstrCode + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY SDate DESC ";
            SQL = SQL + ComNum.VBLF + "";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            //if (dt.Rows.Count == 0)
            //{
            //    dt.Dispose();
            //    dt = null;
            //    ComFunc.MsgBox("해당 DATA가 없습니다.");
            //    return;
            //}
            cboDate.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboDate.Items.Add(dt.Rows[i]["SDate"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                grbDate.Enabled = true;
                cboDate.Enabled = true;
                btnNew.Enabled = true;
                cboDate.SelectedIndex = 0;
                return;
            }
            else
            {
                dt.Dispose();
                dt = null;
                cboDate.Text = "";
                cboDate.Enabled = false;
                //btnNew.PerformClick();
                funcNew();
            }
        }

        private bool DeleteData()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "DELETE BAS_GAMLTD WHERE ROWID='" + fstrROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private bool SaveData()
        {
            double nBi_33 = VB.Val(txtBi33.Text);
            double nBi_51 = VB.Val(txtBi51.Text);
            double nSONO = VB.Val(txtSono.Text);
            double nMRI = VB.Val(txtMRI.Text);
            double nFood = VB.Val(txtFood.Text);
            double nRoom = VB.Val(txtRoom.Text);
            double nDT1 = VB.Val(txtDT1.Text);
            double nDT2 = VB.Val(txtDT2.Text);
            double nDT3 = VB.Val(txtDT3.Text);
            double nBonin = VB.Val(txtBonin.Text);

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            


            try
            {
                if (lblLTD.Text.Substring(0, 4) != "H20A" && lblLTD.Text.Substring(0, 4) != "H20B" && nBonin > 0)
                {
                    MessageBox.Show("포항남구소방서 H20A, H20B만 본인부담율을 저장할수있습니다.", "확인");
                    return false;
                }

                if (fstrROWID == "")
                {
                    SQL = "INSERT INTO BAS_GAMLTD (LtdCode,SDate,Bi_51,Bi_33,DT1,DT2,DT3,";
                    SQL = SQL + ComNum.VBLF + " Sono,MRI,Food,Room,Bohum,EntSabun,EntTime) VALUES ('";
                    SQL = SQL + ComNum.VBLF + fstrCode + "',TO_DATE('" + dtpDate.Text + "','YYYY - MM - DD'),";
                    SQL = SQL + ComNum.VBLF + nBi_51 + "," + nBi_33 + "," + nDT1 + "," + nDT2 + ",";
                    SQL = SQL + ComNum.VBLF + nDT3 + "," + nSONO + "," + nMRI + "," + nFood + "," + nRoom + "," + nBonin + ",";
                    SQL = SQL + ComNum.VBLF + clsPublic.GnJobSabun + ",SYSDATE) ";
                }
                else
                {
                    SQL = "UPDATE BAS_GAMLTD SET ";
                    SQL = SQL + ComNum.VBLF + " SDate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " BI_51=" + nBi_51 + ",";
                    SQL = SQL + ComNum.VBLF + " BI_33=" + nBi_33 + ",";
                    SQL = SQL + ComNum.VBLF + " DT1=" + nDT1 + ",";
                    SQL = SQL + ComNum.VBLF + " DT2=" + nDT2 + ",";
                    SQL = SQL + ComNum.VBLF + " DT3=" + nDT3 + ",";
                    SQL = SQL + ComNum.VBLF + " SONO=" + nSONO + ",";
                    SQL = SQL + ComNum.VBLF + " MRI=" + nMRI + ",";
                    SQL = SQL + ComNum.VBLF + " FOOD=" + nFood + ",";
                    SQL = SQL + ComNum.VBLF + " ROOM=" + nRoom + ",";
                    SQL = SQL + ComNum.VBLF + " Bohum=" + nBonin + ",";
                    SQL = SQL + ComNum.VBLF + " EntSabun=" + clsPublic.GnJobSabun + ",";
                    SQL = SQL + ComNum.VBLF + " EntTime=SYSDATE ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID='" + fstrROWID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Screen_Clear();
                Combo_Date_Set();
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void frmContractorDecreaseCriterion_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            Screen_Clear();
            ssPrint_Sheet1.ClearRange(0, 0, ssPrint_Sheet1.Rows.Count - 1, ssPrint_Sheet1.Columns.Count - 1, true);

            btnNew.Enabled = false;
            grbDate.Enabled = false;
            lblLTD.Text = "";

            if (clsPublic.GnJobSabun == 4349 || clsPublic.GnJobSabun == 19684 || clsPublic.GnJobSabun == 18266) //각각 전산실,박시철,전정훈
            {
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);
            List_Set();
        }

        private void chkDelLtd_Click(object sender, EventArgs e)
        {
            List_Set();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Display();
            if ( cboDate.Enabled == true)
            {
                cboDate.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("정말로 삭제를 하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            if (DeleteData() == false) return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == false) return;
        }

        private void cboDate_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (cboDate.Text.Trim() == "")
            {
                MessageBox.Show("적용일자가 공란입니다.", "오류");
                return;
            }

            //감액정보를 읽는 SQL
            SQL = "SELECT Bi_51,Bi_33,DT1,DT2,DT3,Sono,Mri,Food,Room,Bohum,ROWID ";
            SQL = SQL + ComNum.VBLF + " FROM BAS_GAMLTD ";
            SQL = SQL + ComNum.VBLF + "WHERE LtdCode='" + fstrCode + "' ";
            SQL = SQL + ComNum.VBLF + "  AND SDate=TO_DATE('" + cboDate.Text + "','YYYY-MM-DD') ";

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
                MessageBox.Show(cboDate.Text + "일 감액기준 정보가 없습니다", "오류");
                return;
            }

            fstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();

            dtpDate.Text = cboDate.Text.Trim();
            txtBi51.Text = dt.Rows[0]["Bi_51"].ToString();
            txtBi33.Text = dt.Rows[0]["Bi_33"].ToString();
            txtSono.Text = dt.Rows[0]["Sono"].ToString();
            txtMRI.Text = dt.Rows[0]["MRI"].ToString();
            txtFood.Text = dt.Rows[0]["Food"].ToString();
            txtRoom.Text = dt.Rows[0]["Room"].ToString();
            txtDT1.Text = dt.Rows[0]["DT1"].ToString();
            txtDT2.Text = dt.Rows[0]["DT2"].ToString();
            txtDT3.Text = dt.Rows[0]["DT3"].ToString();
            txtBonin.Text = dt.Rows[0]["Bohum"].ToString();

            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void lst1_DoubleClick(object sender, EventArgs e)
        {
            fstrCode = lst1.Text.Substring(0, 4);
            lblLTD.Text = lst1.Text.Trim();
            Screen_Display();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "계 약 처 리스트";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + DateTime.Now.ToString(), new Font("굴림체", 15), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

            SPR.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //fstrROWID = "";
            //cboDate.Text = "";
            //dtpDate.Text = clsPublic.GstrSysDate;
            //btnSave.Enabled = true;
            //btnCancel.Enabled = true;
            //btnDelete.Enabled = false;
            //dtpDate.Focus();
            funcNew();
        }

        void funcNew()
        {
            fstrROWID = "";
            cboDate.Text = "";
            dtpDate.Text = clsPublic.GstrSysDate;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = false;
            dtpDate.Focus();
        }

        private void optSort0_CheckedChanged(object sender, EventArgs e)
        {
            List_Set();
        }

        /// <summary>
        /// 텍스트 박스에 이벤트 참조 11개
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBi51_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( e.KeyChar == 13)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void cboDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (cboDate.Text.Trim() == "")
            {
                MessageBox.Show("적용일자가 공란입니다.", "오류");
                return;
            }

            //감액정보를 읽는 SQL
            SQL = "SELECT Bi_51,Bi_33,DT1,DT2,DT3,Sono,Mri,Food,Room,Bohum,ROWID ";
            SQL = SQL + ComNum.VBLF + " FROM BAS_GAMLTD ";
            SQL = SQL + ComNum.VBLF + "WHERE LtdCode='" + fstrCode + "' ";
            SQL = SQL + ComNum.VBLF + "  AND SDate=TO_DATE('" + cboDate.Text + "','YYYY-MM-DD') ";

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
                MessageBox.Show(cboDate.Text + "일 감액기준 정보가 없습니다", "오류");
                return;
            }

            fstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();

            dtpDate.Text = cboDate.Text.Trim();
            txtBi51.Text = dt.Rows[0]["Bi_51"].ToString();
            txtBi33.Text = dt.Rows[0]["Bi_33"].ToString();
            txtSono.Text = dt.Rows[0]["Sono"].ToString();
            txtMRI.Text = dt.Rows[0]["MRI"].ToString();
            txtFood.Text = dt.Rows[0]["Food"].ToString();
            txtRoom.Text = dt.Rows[0]["Room"].ToString();
            txtDT1.Text = dt.Rows[0]["DT1"].ToString();
            txtDT2.Text = dt.Rows[0]["DT2"].ToString();
            txtDT3.Text = dt.Rows[0]["DT3"].ToString();
            txtBonin.Text = dt.Rows[0]["Bohum"].ToString();

            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = true;
        }
    }
}
