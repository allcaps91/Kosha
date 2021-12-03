using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// mhemrviewer
    /// frmPatientInfoSearch
    /// </summary>
    public partial class frmEmrPatientInfoSearch : Form
    {
        //환자정보 전달
        public delegate void SendPatInfo(string strPtNo, string strPtName);
        public event SendPatInfo rSendPatInfo;

        /// <summary>
        /// 환자 이름
        /// </summary>
        string mstrPtName = string.Empty;

        public frmEmrPatientInfoSearch(string strPtNm)
        {
            InitializeComponent();
            mstrPtName = strPtNm;
        }

        private void FrmEmrPatientInfoSearch_Load(object sender, EventArgs e)
        {
            if(mstrPtName.Length > 0)
            {
                txtInquire.Text = mstrPtName;
                GetPatList();
            }
        }

        public void GetPatList()
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string strInqInfo = string.Empty;
            string[] strSsno = new string[2];
            bool bolInpt = false;

            if ( rdoInquire2.Checked)
            {

                strSsno[0] = VB.Left(txtInquire.Text.Trim(), 6);
                strSsno[1] = VB.Right(txtInquire.Text.Trim(), 7);
                if(bolInpt == false && !VB.IsNumeric(strSsno[0]) ||
                    !VB.IsNumeric(strSsno[1]) || 
                    txtInquire.Text.Trim().Replace("-", "").Length != 13)
                {
                    ComFunc.MsgBoxEx(this, "주민번호를 정확하게 입력해 주세요");
                    return;
                }

            }

            else if(rdoInquire0.Checked)
            {
                strInqInfo = txtInquire.Text.Trim();
                if(bolInpt == false && strInqInfo.Length < 2)
                {
                    ComFunc.MsgBoxEx(this, "2자 이상 검색하세요");
                    return;
                }
            }

            else if(rdoInquire1.Checked)
            {
                strInqInfo = VB.Format(VB.Val(txtInquire.Text.Trim()), "00000000");
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.PTNO , A.PTNAME, A.BIRTHDATE, A.SSNO1, A.SSNO2, A.ADDR, TO_CHAR(A.LSTINCMDATE, 'YYYY-MM-DD') LSTINCMDATE, A.DEPTCODE";
                SQL += ComNum.VBLF + " FROM ADMIN.VIEWBPT2 A";


                if(bolInpt && strInqInfo == "")
                {

                }
                else
                {

                    if( rdoInquire1.Checked)
                    {
                        SQL += ComNum.VBLF + "                WHERE A.PTNO = '" + (strInqInfo).Trim() + "' ";
                    }
                    else if(rdoInquire0.Checked)
                    {
                        SQL += ComNum.VBLF + "                    WHERE A.PTNAME LIKE '" + (strInqInfo).Trim() + "%' ";
                    }
                    else if(rdoInquire2.Checked)
                    {
                        SQL += ComNum.VBLF + "              WHERE A.SSNO1 = '" + strSsno[0] + "'";
                        SQL += ComNum.VBLF + "                AND A.SSNO2 = '" + strSsno[1] + "'";
                    }
               }


                if (rdoInquire1.Checked)
                {
                    SQL += ComNum.VBLF + "  ORDER BY A.PTNO, A.PTNAME, A.SSNO1, A.SSNO2";
                }
                else if (rdoInquire0.Checked)
                {
                    SQL += ComNum.VBLF + "  ORDER BY A.PTNAME, A.SSNO1, A.SSNO2";
                }
                else if (rdoInquire2.Checked)
                {
                    SQL += ComNum.VBLF + "  ORDER BY SSNO1, SSNO2";
                }


                string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    ComFunc.MsgBoxEx(this, "환자가 존재하지 않습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssPatientInfoList_Sheet1.RowCount = dt.Rows.Count;
                ssPatientInfoList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssPatientInfoList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PTNO"].ToString().Trim();        //병록번호
                    ssPatientInfoList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNAME"].ToString().Trim();      //환자이름
                    ssPatientInfoList_Sheet1.Cells[i, 2].Text = string.Format("{0}-{1}",
                        dt.Rows[i]["SSNO1"].ToString().Trim(), dt.Rows[i]["SSNO2"].ToString().Trim());       //주민등록번호
                    if(dt.Rows[i]["SSNO1"].ToString().Trim().Length > 0 && dt.Rows[i]["SSNO2"].ToString().Trim().Length > 0)
                    {
                        ssPatientInfoList_Sheet1.Cells[i, 3].Text = ComFunc.SexCheck(dt.Rows[i]["SSNO1"].ToString().Trim() + dt.Rows[i]["SSNO2"].ToString().Trim(), "1");        //성별
                    }
                    ssPatientInfoList_Sheet1.Cells[i, 4].Text = ComFunc.AgeCalcX1(dt.Rows[i]["SSNO1"].ToString().Trim() + dt.Rows[i]["SSNO2"].ToString().Trim(), strCurDate);        //나이
                    ssPatientInfoList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ADDR"].ToString().Trim();        //주소
                    ssPatientInfoList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["LSTINCMDATE"].ToString().Trim(); //최종내원일
                    ssPatientInfoList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();    //진료과
                }

                dt.Dispose();
            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }


        private void BtnSearch_Click(object sender, EventArgs e)
        {
            GetPatList();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SsPatientInfoList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssPatientInfoList_Sheet1.RowCount == 0)
                return;

            rSendPatInfo(ssPatientInfoList_Sheet1.Cells[e.Row, 0].Text.Trim(), ssPatientInfoList_Sheet1.Cells[e.Row, 1].Text.Trim());
            Close();
        }

        private void TxtInquire_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                GetPatList();
            }
        }
    }
}
