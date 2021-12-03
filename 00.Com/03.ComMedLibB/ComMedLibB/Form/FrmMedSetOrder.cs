using ComBase;
using ComEmrBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Description : 약속처방
    /// Author : 이상훈
    /// Create Date : 2017.11.16
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="FrmViewPRM.frm"/>

    public partial class FrmMedSetOrder : Form
    {
        string SQL;
        DataTable dt = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        int nIndexPRM;
        string strDeptDrPRM;

        //string FstrDrCode1; //현재의사
        //string FstrDept1;

        private clsSpread SP = new clsSpread();
        private clsOrdDisp OdDsp = new clsOrdDisp();
        private clsOrdFunction OF = new clsOrdFunction();

        //public delegate void SetOrderAllInput_Click(string SetName, string DeptDr);
        //public static event SetOrderAllInput_Click SetOrderAllInput;

        public delegate void SetIpdOrderInput_Click(string GbPrm, string SetName, string DeptDr, string OrderCode, string RowId);
        public static event SetIpdOrderInput_Click SetIpdOrderInput;

        public delegate void SetOpdOrderInput_Click(string GbPrm, string SetName, string DeptDr, string OrderCode, string RowId);
        public static event SetOpdOrderInput_Click SetOpdOrderInput;

        public delegate void SetErOrderInput_Click(string GbPrm, string SetName, string DeptDr, string OrderCode, string RowId);
        public static event SetErOrderInput_Click SetErOrderInput;

        public delegate void SetOrderRowcount_Read(string GbOrd);
        public static event SetOrderRowcount_Read OrderCount;

        public delegate void Order_Clear(string GbOrd);
        public static event Order_Clear Ord_Clear;

        public delegate void SetDiagAllInput_Click(string illCode);
        public static event SetDiagAllInput_Click SetDiagAllInput;

        public delegate void SetDiagInput_Click(string illCode);
        public static event SetDiagInput_Click SetDiagInput;

        public delegate void EventClosed();
        public static event EventClosed rEventClosed;

        //private System.Windows.Forms.ImageList dummyImageList;

        public FrmMedSetOrder()
        {
            InitializeComponent();

            ////listview는 row높이 설정을 하지 못하므로 꼼수부림(rowheight 늘려주기위함)
            //dummyImageList = new ImageList();
            //dummyImageList.ImageSize = new Size(1, 20);

            //LstSetName.view = View.Details;
            //LstSetName.SmallImageList = dummyImageList;
            ////===========================================================
        }

        private void FrmMedSetOrder_Load(object sender, EventArgs e)
        {
            this.Location = new Point(190, 98);

            clsOrdFunction.GstrViewSlipCallPosition = "";

            if (clsOrdFunction.GstrGbJob == "ER")
            {
                btnEM.Visible = true;
                btnEM.Enabled = true;
            }
            else
            {
                btnEM.Visible = false;
                btnEM.Enabled = false;
            }

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            ssSetOrder_Sheet1.Columns.Get(7).Visible = false;

            if (clsOrdFunction.Gstr약속처방DrCode != "")
            {
                lblDrIdNumber.Text = clsOrdFunction.GstrDrName + "[" + clsOrdFunction.Gstr약속처방DrCode + "]";
            }
            else
            {
                lblDrIdNumber.Text = clsOrdFunction.GstrDrName + "[" + clsType.User.Sabun + "]";
            }

            string FstrDrCode1 = clsType.User.Sabun;
            //FstrDept1 = clsPublic.GstrDeptCode;
            clsOrdFunction.GstrGbPrm = "PROM";

            if (clsPublic.GstrJobMan == "간호사" && clsOrdFunction.GstrVerbalDrCode != "")
            {
                btnSetPersonal.Enabled = false;
                btnSetDept.Enabled = false;
                btnSetExam.Enabled = false;

                btnOrderCopy.Enabled = false;
                btnSelectDelete.Enabled = false;
                btnSetOrdDelete.Enabled = false;

                rdoGbOrder1.Enabled = false;
                rdoGbOrder2.Enabled = false;
                rdoGbOrder3.Enabled = false;
                rdoGbOrder4.Enabled = false;

                clsPublic.GstrWardCode = "";
                clsPublic.GstrICUWard = "";
                clsPublic.GstrWardCode = VB.Left(VB.GetSetting("TWIN", "NURVIEW", "WARDCODE"), 2);  //registry 값 읽어오기
                clsPublic.GstrICUWard = ComFunc.MidH(VB.GetSetting("TWIN", "NURVIEW", "WARDCODE"), 4, 4);  //registry 값 읽어오기
                
                fn_Read_Set(4, clsPublic.GstrWardCode);
            }
            else if (clsPublic.GstrDeptCode.Trim() == "AN" || clsPublic.GstrDeptCode.Trim() == "XR")
            {
                nIndexPRM = 2;
                strDeptDrPRM = clsPublic.GstrDeptCode;      //마취과/방사선과
                fn_Read_Set(2, clsPublic.GstrDeptCode);     //과 약속처방  Read
                btnSelectDelete.Enabled = false;
                btnSetOrdDelete.Enabled = false;
            }
            else
            {
                nIndexPRM = 1;
                if (clsOrdFunction.Gstr약속처방DrCode != "")
                {
                    strDeptDrPRM = clsOrdFunction.Gstr약속처방DrCode;
                }
                else
                {
                    strDeptDrPRM = clsType.User.Sabun;
                }
                if (clsOrdFunction.Gstr약속처방DrCode.Trim() == null)
                {
                    clsOrdFunction.Gstr약속처방DrCode = "";
                }
                
                if (clsOrdFunction.GstrGbJob == "ER" && clsOrdFunction.Gstr약속처방DrCode.Trim() != "")
                {
                    FstrDrCode1 = clsOrdFunction.Gstr약속처방DrCode.Trim();
                    lblDrIdNumber.Text = clsVbfunc.GetOCSDoctorName(clsDB.DbCon, clsOrdFunction.Gstr약속처방DrCode.Trim()) + "[" + clsOrdFunction.Gstr약속처방DrCode.Trim() + "]";
                }
                fn_Read_Set(1, FstrDrCode1);
                btnSelectDelete.Enabled = true;
                btnSetOrdDelete.Enabled = true;
            }
        }

        private void BackColor_Clear()
        {
            txtSetName.Text = "";

            btnSetPersonal.BackColor = Color.White;
            btnSetDept.BackColor = Color.White;
            btnSetExam.BackColor = Color.White;
            btnSlips.BackColor = Color.White;
            btnEM.BackColor = Color.White;

            btnSetPersonal.ForeColor = Color.Black;
            btnSetDept.ForeColor = Color.Black;
            btnSetExam.ForeColor = Color.Black;
            btnSlips.ForeColor = Color.Black;
            btnEM.ForeColor = Color.Black;
        }

        void Select_Color_Set(Button Btn)
        {
            Btn.BackColor = Color.RoyalBlue;
            Btn.ForeColor = Color.White;
        }

        private void btnSetPersonal_Click(object sender, EventArgs e)
        {
            BackColor_Clear();
            Select_Color_Set(btnSetPersonal);

            btnSend.Enabled = false;
            btnSelectDelete.Enabled = true;
            btnSetOrdDelete.Enabled = true;
            btnSetillDelete.Enabled = true;

            LstSetName.Items.Clear();
            SP.Spread_All_Clear(ssSetOrder);

            btnOrderCopy.Text = "과 약속처방 등록";
            btnDiagnosisCopy.Text = "과 약속상병 등록";

            clsOrdFunction.GstrGbPrm = "PROM";

            rdoGbOrder1.Enabled = true;
            rdoGbOrder2.Enabled = true;
            rdoGbOrder3.Enabled = true;
            rdoGbOrder4.Enabled = true;

            nIndexPRM = 1;
            if (clsOrdFunction.Gstr약속처방DrCode != "")
            {
                strDeptDrPRM = clsOrdFunction.Gstr약속처방DrCode;
            }
            else
            {
                strDeptDrPRM = clsType.User.Sabun;
            }

            fn_Read_Set(1, strDeptDrPRM);

            Application.DoEvents();
        }

        void fn_Read_Set(int sGbn, string sDeptDr)
        {
            string strDeptDr = "";

            LstSetName.Items.Clear();
            SP.Spread_All_Clear(ssSetOrder);

            strDeptDr = sDeptDr;

            if (VB.Left(sDeptDr, 2) == "IU")
            {
                strDeptDr = VB.Left(strDeptDr, 5);
            }

            //switch (sGbn)
            //{
            //    case 1:
            //        btnSetPersonal.ForeColor = Color.RoyalBlue;
            //        btnSetDept.ForeColor = Color.White;
            //        btnSetExam.ForeColor = Color.White;
            //        break;
            //    case 2:
            //        btnSetPersonal.ForeColor = Color.White;
            //        btnSetDept.ForeColor = Color.RoyalBlue;
            //        btnSetExam.ForeColor = Color.White;
            //        break;
            //    case 3:
            //        btnSetPersonal.ForeColor = Color.White;
            //        btnSetDept.ForeColor = Color.White;
            //        btnSetExam.ForeColor = Color.FromArgb(255, 0, 0);
            //        break;
            //    default:
            //        break;
            //}

            try
            {
                SQL = "";
                SQL += " SELECT distinct PRMname                                            \r";
                SQL += "   FROM ADMIN.OCS_OPRM                                         \r";
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    SQL += "  WHERE DeptDr = '" + strDeptDr.Trim() + "'                     \r";
                }
                else
                {
                    if (strDeptDr.Trim() == "MG" || strDeptDr.Trim() == "MC" || strDeptDr.Trim() == "MP" ||
                    strDeptDr.Trim() == "ME" || strDeptDr.Trim() == "MN" || strDeptDr.Trim() == "MR" || strDeptDr.Trim() == "MI")
                    {
                        SQL += "  WHERE DeptDr IN('" + strDeptDr.Trim() + "', 'MD')             \r";
                    }
                    else
                    {
                        SQL += "  WHERE DeptDr = '" + strDeptDr.Trim() + "'                     \r";
                    }
                }
                
                if (sGbn == 4)
                {
                    SQL += "    AND GBORDER = 'N'                                           \r";    //간호처방
                }
                else if (sGbn == 3)
                {
                    SQL += "    AND GBORDER = 'P'                                           \r";    //검사처방
                }
                else if (rdoGbOrder1.Checked == true)
                {
                    SQL += "    AND GBORDER = ' '                                           \r";
                }
                else if (rdoGbOrder2.Checked == true)
                {
                    SQL += "    AND GBORDER = 'M'                                           \r";
                }
                else if (rdoGbOrder3.Checked == true)
                {
                    SQL += "    AND GBORDER = 'F'                                           \r";
                }
                else if (rdoGbOrder4.Checked == true)
                {
                    SQL += "    AND GBORDER = 'T'                                           \r";
                }
                else if (rdoGbOrder5.Checked == true)
                {
                    SQL += "    AND GBORDER = 'E'                                           \r";
                }
                if (txtSet.Text != "")
                {
                    SQL += "   AND (PRMNAME LIKE '%" + txtSet.Text.Trim().ToLower() + "%'   \r";
                    SQL += "    OR  PRMNAME LIKE '%" + txtSet.Text.Trim().ToUpper() + "%')  \r";
                }
                SQL += "  GROUP BY PRMNAME                                                  \r";
                SQL += "  ORDER BY PRMNAME                                                  \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        LstSetName.Items.Add(dt.Rows[i]["PRMNAME"].ToString());
                    }
                }

                if (LstSetName.Items.Count > 0)
                {
                    LstSetName.SelectedIndex = 0;
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

        private void btnSetDept_Click(object sender, EventArgs e)
        {
            BackColor_Clear();
            Select_Color_Set(btnSetDept);

            btnSelectDelete.Enabled = false;
            btnSetOrdDelete.Enabled = false;
            btnSetillDelete.Enabled = false;



            LstSetName.Items.Clear();
            SP.Spread_All_Clear(ssSetOrder);

            clsOrdFunction.GstrGbPrm = "DPROM";

            btnOrderCopy.Text = "개인 약속처방 등록";
            btnDiagnosisCopy.Text = "개인 약속상병 등록";

            rdoGbOrder1.Enabled = true;
            rdoGbOrder2.Enabled = true;
            rdoGbOrder3.Enabled = true;
            rdoGbOrder4.Enabled = true;

            nIndexPRM = 2;
            if (clsOrdFunction.GstrDept.Trim() == "AN" || clsOrdFunction.GstrDept.Trim() == "XR")
            {   
                strDeptDrPRM = clsOrdFunction.GstrDept;
            }
            else
            {
                strDeptDrPRM = clsPublic.GstrDeptCode;
            }
            fn_Read_Set(2, strDeptDrPRM);
        }

        private void btnSetExam_Click(object sender, EventArgs e)
        {
            BackColor_Clear();
            Select_Color_Set(btnSetExam);

            btnSelectDelete.Enabled = false;
            btnSetOrdDelete.Enabled = false;
            btnSetillDelete.Enabled = false;

            btnSend.Enabled = false;
            btnSetOrdDelete.Enabled = false;
            btnSelectDelete.Enabled = false;

            rdoGbOrder1.Enabled = false;
            rdoGbOrder2.Enabled = false;
            rdoGbOrder3.Enabled = false;
            rdoGbOrder4.Enabled = false;

            clsOrdFunction.GstrGbPrm = "EXAM";

            nIndexPRM = 3;
            strDeptDrPRM = clsType.User.Sabun;

            fn_Read_Set(3, strDeptDrPRM);
        }

        private void btnSlips_Click(object sender, EventArgs e)
        {
            clsOrdFunction.GstrViewSlipCallPosition = "SET";

            if (rEventClosed != null)
            {
                rEventClosed();
            }
            else
            {
                this.Dispose();
                this.Close();
            }
        }

        private void btnEM_Click(object sender, EventArgs e)
        {
            BackColor_Clear();
            Select_Color_Set(btnEM);

            btnSelectDelete.Enabled = false;
            btnSetOrdDelete.Enabled = false;
            btnSetillDelete.Enabled = false;
            
            clsOrdFunction.GstrGbPrm = "ER";

            btnOrderCopy.Text = "개인약속  등록";
            btnDiagnosisCopy.Text = "개인약속상병  등록";

            rdoGbOrder1.Enabled = true;
            rdoGbOrder2.Enabled = true;
            rdoGbOrder3.Enabled = true;
            rdoGbOrder4.Enabled = true;

            clsOrdFunction.GstrGbPrm = "EM";

            strDeptDrPRM = "EM";

            fn_Read_Set(5, strDeptDrPRM);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //string strMsg = "";

            //if (LstSetName.SelectedIndex == -1) return;

            //clsOrdFunction.GnOrderCount = 0;

            //OrderCount("OPD");

            //if (clsOrdFunction.GnOrderCount > 0)
            //{
            //    if (clsOrdFunction.GstrGbJob == "ER")
            //    {
            //        strMsg = "";
            //        strMsg += "현재 Order 화면에 처방 내역이 존재합니다" + "\r\n\r\n";
            //        strMsg += "[Add : 예] [Replace : 아니오] [취소 : 취소] ";
            //        DialogResult result = MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //        if (result == DialogResult.Cancel)
            //        {
            //            return;
            //        }
            //        else if (result == DialogResult.No)
            //        {
            //            Ord_Clear("OPD");
            //        }
            //    }
            //}

            //if (clsType.User.IsNurse == "OK")
            //{
            //    strDeptDrPRM = clsPublic.GstrWardCode;
            //}
            //else if (btnSetDept.BackColor == Color.RoyalBlue)
            //{
            //    if (clsOrdFunction.GstrDept == "AN" || clsOrdFunction.GstrDept == "XR")
            //    {
            //        strDeptDrPRM = clsOrdFunction.GstrDept;
            //    }
            //    else
            //    {
            //        strDeptDrPRM = clsPublic.GstrDeptCode;
            //    }
            //}
            //else if (btnSetPersonal.BackColor == Color.RoyalBlue)
            //{
            //    if (clsOrdFunction.GstrGbJob == "ER")
            //    {
            //        strDeptDrPRM = FstrDrCode1;
            //    }
            //    else
            //    {
            //        strDeptDrPRM = clsType.User.Sabun;
            //    }
            //}
            //else if (btnSetExam.BackColor == Color.RoyalBlue)
            //{
            //    strDeptDrPRM = clsOrdFunction.GstrDrCode;
            //    if (clsOrdFunction.GstrGbJob == "ER")
            //    {
            //        strDeptDrPRM = FstrDrCode1;
            //    }
            //}
            //else if (btnEM.BackColor == Color.RoyalBlue)
            //{
            //    strDeptDrPRM = "EM";
            //}

            //Cursor.Current = Cursors.WaitCursor;

            //for (int i = 0; i < LstSetName.Items.Count; i++)
            //{
            //    if (LstSetName.SelectedItems[i].ToString() != "")
            //    //if (LstSetName.SelectedIndices[i].ToString() != "")
            //    {
            //        clsOrdFunction.GstrSELECTPxName = LstSetName.Items[i].ToString();
            //        strSetName = LstSetName.Items[i].ToString();
            //        SetOrderAllInput(strSetName, strDeptDrPRM);
            //    }
            //}
            //Cursor.Current = Cursors.Default;
        }

        private void ssSetOrder_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                btnOrderSelCancel_Click(btnOrderSelCancel, e);
            }
            else
            {
                if (ssSetOrder.ActiveSheet.Cells[e.Row, 0, e.Row, ssSetOrder.ActiveSheet.ColumnCount - 1].BackColor != Color.FromArgb(255, 255, 150))
                {
                    ssSetOrder.ActiveSheet.Cells[e.Row, 0, e.Row, ssSetOrder.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 150);
                }
                else
                {
                    ssSetOrder.ActiveSheet.Cells[e.Row, 0, e.Row, ssSetOrder.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
                }
            }
        }

        private void btnillSelCancel_Click(object sender, EventArgs e)
        {
            if (ssSetDiagnosis.ActiveSheet.NonEmptyRowCount == 0) return;
            ssSetDiagnosis.ActiveSheet.Cells[0, 0, (int)ssSetDiagnosis.ActiveSheet.RowCount - 1, (int)ssSetDiagnosis.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
        }

        private void btnOrderSelCancel_Click(object sender, EventArgs e)
        {
            if (ssSetOrder.ActiveSheet.NonEmptyRowCount == 0) return;
            ssSetOrder.ActiveSheet.Cells[0, 0, (int)ssSetOrder.ActiveSheet.RowCount - 1, (int)ssSetOrder.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
        }

        private void btnReName_Click(object sender, EventArgs e)
        {
            if (LstSetName.SelectedIndex == -1) return;

            if (LstSetName.Items[LstSetName.SelectedIndex].ToString().Trim() == "") return;

            if (txtSetName.Text.Trim() == "") return;

            if (LstSetName.Items[LstSetName.SelectedIndex].ToString().Trim() == txtSetName.Text.Trim())
            {
                MessageBox.Show("변경할 약속처방 명칭과 기존 약속처방 명칭이 동일합니다!!!", "약속처방 명칭확인");
                txtSetName.Focus();
                return;
            }

            if (MessageBox.Show("약속처방 명칭이 변경 됩니다. 계속 하시겠습니까 ?", "약속처방명칭 변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            Update_Promise();

            txtSetName.Text = "";

            if (btnSetPersonal.BackColor == Color.RoyalBlue)
            {
                btnSetPersonal_Click(btnSetPersonal, new EventArgs());
            }
            else if (btnSetDept.BackColor == Color.RoyalBlue)
            {
                btnSetDept_Click(btnSetDept, new EventArgs());
            }
            else if (btnSetExam.BackColor == Color.RoyalBlue)
            {
                btnSetExam_Click(btnSetExam, new EventArgs());
            }
            else if (btnEM.BackColor == Color.RoyalBlue)
            {
                btnEM_Click(btnSetExam, new EventArgs());
            }
        }

        private void Update_Promise()
        {
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += " UPDATE ADMIN.OCS_OPRM SET                                                             \r";
                SQL += "        PRMNAME = '" + txtSetName.Text.Trim() + "'                                          \r";
                if (clsOrdFunction.GstrGbPrm == "PROM")
                {
                    SQL += "  WHERE (DEPTDR = '" + clsOrdFunction.GstrDrCode + "'                                   \r";
                    SQL += "     or  DEPTDR = '" + clsType.User.Sabun.Trim() + "')                                  \r";
                }
                else if (clsOrdFunction.GstrGbPrm == "DPROM")
                {
                    SQL += "  WHERE DEPTDR = '" + clsPublic.GstrDeptCode + "'                                       \r";
                }
                else if (clsOrdFunction.GstrGbPrm == "ALL")
                {
                    SQL += "  WHERE DEPTDR = 'ALL'                                                                  \r";
                }
                SQL += "     AND PRMNAME = '" + LstSetName.Items[LstSetName.SelectedIndex].ToString().Trim() + "'   \r";      
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
                Cursor.Current = Cursors.Default;
                MessageBox.Show("변경 완료 되었습니다!!!");
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSelectDelete_Click(object sender, EventArgs e)
        {
            int nCnt = 0;

            if (ssSetOrder.ActiveSheet.NonEmptyRowCount == 0)
            {
                MessageBox.Show("삭제할 약속처방이 존재하지 않습니다!", "약속처방삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ssSetOrder.ActiveSheet.NonEmptyRowCount == 1)
            {
                MessageBox.Show("약속처방 삭제 버튼을 이용해서 삭제하세요!", "약속처방삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < ssSetOrder.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssSetOrder.ActiveSheet.Cells[i, 0].BackColor == Color.FromArgb(255, 255, 150))
                {
                    nCnt++;
                    break;
                }
            }

            if (nCnt == 0)
            {
                MessageBox.Show("삭제할 약속처방이 선택되지 않았습니다!, ", "약속처방삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("선택된 약속 처방을 삭제 하시겠습니까?  ", "약속처방삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    for (int i = 0; i < ssSetOrder.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        if (ssSetOrder.ActiveSheet.Cells[i, 0].BackColor == Color.FromArgb(255, 255, 150))
                        {

                            SQL = "";
                            SQL += " DELETE FROM ADMIN.OCS_OPRM                                            \r";
                            SQL += "  WHERE ROWID = '" + ssSetOrder.ActiveSheet.Cells[i, 25].Text.Trim() + "'   \r";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(ex.Message);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            btnSetPersonal_Click(btnSetPersonal, new EventArgs());
        }

        private void btnOrderCopy_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            //string strOK = "";

            if (clsType.User.IsNurse == "OK") return;

            if (btnOrderCopy.Text == "과 약속처방 등록")
            {
                strMsg = "선택된 My Prescription Name : ";
                strMsg += LstSetName.Items[LstSetName.SelectedIndex].ToString().Trim() + "\r\n\r\n";
                strMsg += "과 약속처방에 등록하시겠습니까??";
            }
            else
            {
                strMsg = "선택된 진료과 Prescription Name : ";
                strMsg += LstSetName.Items[LstSetName.SelectedIndex].ToString().Trim() + "\r\n\r\n";
                strMsg += "개인 약속처방에 등록하시겠습니까??";
            }

            if (MessageBox.Show(strMsg, "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    SQL = "";
                    SQL += " SELECT DeptDr                                          \r";
                    SQL += "   FROM ADMIN.OCS_OPRM                             \r";
                    if (btnOrderCopy.Text == "과 약속처방 등록")
                    {
                        SQL += "  WHERE DeptDr = '" + clsPublic.GstrDeptCode + "'   \r";
                    }
                    else
                    {
                        SQL += "  WHERE DeptDr = '" + clsOrdFunction.GstrDrCode + "'\r";
                    }
                    SQL += "    AND PRMname = '" + LstSetName.Items[LstSetName.SelectedIndex].ToString().Trim() + "' \r";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("이미 기존의 약속처방이 존재합니다", "등록불가", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dt.Dispose();
                        dt = null;
                        return;
                    }
                    else
                    {
                        fn_SetOrd_Insert();
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
        }

        void fn_SetOrd_Insert()
        {
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += " INSERT INTO ADMIN.OCS_OPRM                                                \r";
                SQL += "        (DEPTDR, PRMNAME, SEQNO, ORDERCODE, SUCODE, BUN, SLIPNO                 \r";
                SQL += "      , REALQTY, QTY, NAL, GBDIV, DOSCODE, GBBOTH, GBINFO, GBER, GBSELF         \r";
                SQL += "      , GBSPC, REMARK, ILLCODES, BOOWI1, BOOWI2, BOOWI3, BOOWI4, ENTDATE        \r";
                SQL += "      , GBORDER, GBPRN, GBTFLAG, GBPORT, GBGROUP, CONTENTS                      \r";
                SQL += "      , BCONTENTS , CPCODE, CPDAY, ILLCODES_KCD6 )                              \r";
                if (btnOrderCopy.Text == "과 약속처방 등록")
                {
                    SQL += " (SELECT '" + clsPublic.GstrDeptCode.Trim() + "'                            \r";
                }
                else
                {
                    SQL += " (SELECT '" + clsOrdFunction.GstrDrCode.Trim() + "'                         \r";
                }
                SQL += "      , '" + LstSetName.Items[LstSetName.SelectedIndex].ToString().Trim() + "'  \r";
                SQL += "      , Seqno, OrderCode, SuCode, Bun, Slipno                                   \r";
                SQL += "      , RealQty, Qty, Nal, GbDiv, DosCode, GbBoth                               \r";
                SQL += "      , GbInfo, GbER, GbSelf, GbSpc, Remark, IllCodes                           \r";
                SQL += "      , Boowi1, Boowi2, Boowi3, Boowi4, SYSDATE                                 \r";
                SQL += "      , GbOrder, GbPrn, GbTflag, GbPort, GbGroup                                \r";
                SQL += "      , Contents, BContents, CPCODE, CPDAY, ILLCODES_KCD6                       \r";
                SQL += "   FROM ADMIN.OCS_OPRM                                                     \r";
                if (btnOrderCopy.Text == "과 약속처방 등록")
                {
                    SQL += "  WHERE DeptDr  = '" + clsOrdFunction.GstrDrCode + "'                       \r";
                }
                else
                {
                    SQL += "  WHERE DeptDr  = '" + clsPublic.GstrDeptCode + "'                          \r";
                }
                SQL += "     AND PRMname = '" + LstSetName.Items[LstSetName.SelectedIndex].ToString().Trim() + "')\r";
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

        private void LstSetName_Click(object sender, EventArgs e)
        {
            string strDeptDr = "";
            string strSetName = "";

            SP.Spread_All_Clear(ssSetOrder);
            SP.Spread_All_Clear(ssSetDiagnosis);

            if (LstSetName.SelectedIndex == -1) return;

            if (LstSetName.SelectedIndex > -1)
            {
                strSetName = LstSetName.Items[LstSetName.SelectedIndex].ToString().Trim();
            }
            else if (LstSetName.SelectedIndex == 0)
            {
                strSetName = LstSetName.Items[0].ToString().Trim();
            }
            else
            {
                strSetName = "";
            }

            txtSetName.Text = strSetName.Trim();

            if (btnSetDept.Enabled == false && btnSetPersonal.Enabled == false && btnSetExam.Enabled == false)
            {
                strDeptDr = clsPublic.GstrWardCode;
            }
            else if (btnSetDept.BackColor == Color.RoyalBlue)
            {
                if (clsOrdFunction.GstrDept == "AN" || clsOrdFunction.GstrDept == "XR")
                {
                    strDeptDr = clsOrdFunction.GstrDept;
                }
                else
                {
                    strDeptDr = clsPublic.GstrDeptCode;
                }
            }
            else if (btnSetPersonal.BackColor == Color.RoyalBlue)
            {
                strDeptDr = clsType.User.Sabun;
                if (clsOrdFunction.GstrGbJob == "ER")
                {
                    if (clsOrdFunction.Gstr약속처방DrCode != "")
                    {
                        strDeptDr = clsOrdFunction.Gstr약속처방DrCode;
                    }
                    else
                    {
                        strDeptDr = clsType.User.Sabun;
                    }
                }
            }
            else if (btnSetExam.BackColor == Color.RoyalBlue)
            {
                //strDeptDr = clsPublic.GstrDeptCode;
                //if (clsOrdFunction.GstrGbJob == "ER")
                //{
                //    strDeptDr = FstrDrCode1;
                //}
                strDeptDr = clsType.User.Sabun;
            }
            else if (btnEM.BackColor == Color.RoyalBlue)
            {
                strDeptDr = "EM";
            }

            if (VB.Left(strDeptDr, 2) == "IU")
            {
                strDeptDr = VB.Left(strDeptDr, 5);
            }

            strSetName = txtSetName.Text;

            ssSetOrder_Sheet1.RowCount = 300;
            ssSetDiagnosis_Sheet1.RowCount = 30;

            OdDsp.fn_Read_SetOrder(ssSetOrder, ssSetDiagnosis, strSetName, strDeptDr, nIndexPRM);
            ssSetOrder.ActiveSheet.Cells[0, 0, (int)ssSetOrder.ActiveSheet.RowCount - 1, (int)ssSetOrder.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 150);
        }
        
        private void btnOrderInput_Click(object sender, EventArgs e)
        {
            string sSetName = "";
            string sDeptDr = "";
            string sOrderCode = "";
            string sRowId = "";
            string sGroup = "";

            if (rdoGbOrder2.Checked == true)
            {
                clsOrdFunction.GstrSetOrderGubun = "Adm";
                clsOrdFunction.GstrSetGbAct = "M";
            }
            else if (rdoGbOrder3.Checked == true)
            {
                clsOrdFunction.GstrSetOrderGubun = "Pre";
                clsOrdFunction.GstrSetGbAct = "F";
            }
            else if (rdoGbOrder4.Checked == true)
            {
                clsOrdFunction.GstrSetOrderGubun = "Post";
                clsOrdFunction.GstrSetGbAct = "T";
            }
            else
            {
                clsOrdFunction.GstrSetOrderGubun = "";
                clsOrdFunction.GstrSetGbAct = " ";
            }

            for (int i = 0; i < ssSetOrder.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssSetOrder.ActiveSheet.Cells[i, 2].BackColor == Color.FromArgb(255, 255, 150))
                {
                    sSetName = LstSetName.Items[LstSetName.SelectedIndex].ToString().Trim();
                    sDeptDr = strDeptDrPRM;
                    sOrderCode = ssSetOrder.ActiveSheet.Cells[i, 0].Text.Trim();
                    sRowId = ssSetOrder.ActiveSheet.Cells[i, 25].Text.Trim();
                    sGroup = ssSetOrder.ActiveSheet.Cells[i, 10].Text.Trim();

                    if (sGroup != "" && sGroup != null)
                    {
                        if (rdoGbOrder2.Checked == true)
                        {
                            //clsOrdFunction.GstrSetSort = 91 + int.Parse(sGroup);
                        }
                        else if (rdoGbOrder3.Checked == true)
                        {
                            //clsOrdFunction.GstrSetSort = 92 + int.Parse(sGroup);
                        }
                        else if (rdoGbOrder4.Checked == true)
                        {
                            //clsOrdFunction.GstrSetSort = 93 + int.Parse(sGroup);
                        }
                    }

                    if (clsOrdFunction.GstrGbJob == "OPD")
                    {
                        SetOpdOrderInput(clsOrdFunction.GstrGbPrm, sSetName, sDeptDr, sOrderCode, sRowId);
                    }
                    else if (clsOrdFunction.GstrGbJob == "IPD")
                    {
                        SetIpdOrderInput(clsOrdFunction.GstrGbPrm, sSetName, sDeptDr, sOrderCode, sRowId);
                    }
                    else if (clsOrdFunction.GstrGbJob == "ER")
                    {
                        SetErOrderInput(clsOrdFunction.GstrGbPrm, sSetName, sDeptDr, sOrderCode, sRowId);
                    }
                }
            }

            clsOrdFunction.GstrSetOrderGubun = "";
            clsOrdFunction.GstrSetGbAct = "";
            clsOrdFunction.GstrSetSort = 0;
        }

        private void btnSetOrdDelete_Click(object sender, EventArgs e)
        {
            if (ssSetOrder.ActiveSheet.NonEmptyRowCount == 0 && ssSetDiagnosis.ActiveSheet.NonEmptyRowCount == 0)
            {
                MessageBox.Show("삭제할 약속처방이 존재하지 않습니다", "약속처방삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ssSetOrder.ActiveSheet.NonEmptyRowCount > 0)
            {
                if (MessageBox.Show("선택된 약속 처방을 삭제 하시겠습니까?  ", "약속처방삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    try
                    {
                        SQL = "";
                        SQL += " DELETE ADMIN.OCS_OPRM                             \r";
                        SQL += "  WHERE PRMNAME = '" + txtSetName.Text.Trim() + "'      \r";
                        SQL += "    AND DEPTDR = '" + strDeptDrPRM + "'                 \r";
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
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("삭제 완료 되었습니다!!!");
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

            if (btnSetPersonal.BackColor == Color.RoyalBlue)
            {
                btnSetPersonal_Click(btnSetPersonal, new EventArgs());
            }
            else if (btnSetDept.BackColor == Color.RoyalBlue)
            {
                btnSetDept_Click(btnSetDept, new EventArgs());
            }
            else if (btnSetExam.BackColor == Color.RoyalBlue)
            {
                btnSetExam_Click(btnSetExam, new EventArgs());
            }
            else if (btnEM.BackColor == Color.RoyalBlue)
            {
                btnEM_Click(btnSetExam, new EventArgs());
            }
        }

        private void btnillsSelectDelete_Click(object sender, EventArgs e)
        {
            int nCnt = 0;

            if (ssSetDiagnosis.ActiveSheet.NonEmptyRowCount == 0)
            {
                MessageBox.Show("삭제할 약속 상병이 존재하지 않습니다!", "약속상병삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ssSetDiagnosis.ActiveSheet.NonEmptyRowCount == 1)
            {
                MessageBox.Show("약속 상병 삭제 버튼을 이용해서 삭제하세요!", "약속상병삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < ssSetDiagnosis.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssSetDiagnosis.ActiveSheet.Cells[i, 0].BackColor == Color.FromArgb(255, 255, 150))
                {
                    nCnt++;
                    break;
                }
            }

            if (nCnt == 0)
            {
                MessageBox.Show("삭제할 약속 상병이 선택되지 않았습니다!, ", "약속상병삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("선택된 약속 상병을 삭제 하시겠습니까?  ", "약속상병삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                clsDB.setBeginTran(clsDB.DbCon);


                for (int i = 0; i < ssSetDiagnosis.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssSetDiagnosis.ActiveSheet.Cells[i, 0].BackColor == Color.FromArgb(255, 255, 150))
                    {
                        try
                        {
                            SQL = "";
                            SQL = "";
                            SQL += " UPDATE FROM ADMIN.OCS_OPRM                                                \r";
                            SQL += "    SET ILLCODES      = ''                                                      \r";
                            SQL += "      , BOWI1         = ''                                                      \r";
                            SQL += "      , BOWI2         = ''                                                      \r";
                            SQL += "      , BOWI3         = ''                                                      \r";
                            SQL += "      , BOWI4         = ''                                                      \r";
                            SQL += "      , ILLCODES_KCD6 = ''                                                      \r";
                            SQL += "  WHERE ROWID = '" + ssSetDiagnosis.ActiveSheet.Cells[i, 6].Text.Trim() + "'    \r";
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
            }            
            btnSetPersonal_Click(btnSetPersonal, e);
        }

        private void btnSetillDelete_Click(object sender, EventArgs e)
        {
            if (ssSetDiagnosis.Sheets[0].NonEmptyRowCount == 0)
            {
                MessageBox.Show("삭제할 약속상병이 존재하지 않습니다", "약속상병삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("선택된 약속 상병을 삭제 하시겠습니까?  ", "약속상병삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL += " UPDATE ADMIN.OCS_OPRM                 \r";
                    SQL += "    SET ILLCODES      = ''                  \r";
                    SQL += "      , BOOWI1        = ''                  \r";
                    SQL += "      , BOOWI2        = ''                  \r";
                    SQL += "      , BOOWI3        = ''                  \r";
                    SQL += "      , BOOWI4        = ''                  \r";
                    SQL += "      , ILLCODES_KCD6 = ''                  \r";
                    SQL += "  WHERE PRMNAME  = '" + txtSetName.Text + "'     \r";
                    SQL += "    AND DEPTDR = '" + strDeptDrPRM + "'     \r";
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
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }
                btnSetPersonal_Click(btnSetPersonal, e);
            }
        }

        private void btnillInput_Click(object sender, EventArgs e)
        {
            string sGBPrm = clsOrdFunction.GstrGbPrm;

            for (int i = 0; i < ssSetDiagnosis.ActiveSheet.NonEmptyRowCount; i++)
            {
                //if (ssSetDiagnosis.ActiveSheet.Cells[i, 0].BackColor == Color.FromArgb(255, 255, 150))
                //{
                SetDiagInput(ssSetDiagnosis.ActiveSheet.Cells[i, 0].Text.Trim());
                //}
            }
        }

        private void btnAllInput_Click(object sender, EventArgs e)
        {
            btnOrderInput_Click(btnOrderInput, e);
            btnillInput_Click(btnillInput, e);
        }

        private void btnDiagnosisCopy_Click(object sender, EventArgs e)
        {

        }

        private void ssSetOrder_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string sSetName = "";
            string sDeptDr = "";
            string sOrderCode = "";
            string sRowId = "";

            if (rdoGbOrder2.Checked == true)
            {
                clsOrdFunction.GstrSetOrderGubun = "Adm";
                clsOrdFunction.GstrSetGbAct = "M";
            }
            else if (rdoGbOrder3.Checked == true)
            {
                clsOrdFunction.GstrSetOrderGubun = "Pre";
                clsOrdFunction.GstrSetGbAct = "F";
            }
            else if (rdoGbOrder4.Checked == true)
            {
                clsOrdFunction.GstrSetOrderGubun = "Post";
                clsOrdFunction.GstrSetGbAct = "T";
            }
            else
            {
                clsOrdFunction.GstrSetOrderGubun = "";
                clsOrdFunction.GstrSetGbAct = " ";
            }

            sSetName = LstSetName.Items[LstSetName.SelectedIndex].ToString().Trim();
            sDeptDr = strDeptDrPRM;
            sOrderCode = ssSetOrder.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            sRowId = ssSetOrder.ActiveSheet.Cells[e.Row, 25].Text.Trim();

            if (clsOrdFunction.GstrGbJob == "OPD")
            {
                SetOpdOrderInput(clsOrdFunction.GstrGbPrm, sSetName, sDeptDr, sOrderCode, sRowId);
            }
            else if (clsOrdFunction.GstrGbJob == "IPD")
            {
                SetIpdOrderInput(clsOrdFunction.GstrGbPrm, sSetName, sDeptDr, sOrderCode, sRowId);
            }
            else if (clsOrdFunction.GstrGbJob == "ER")
            {
                SetErOrderInput(clsOrdFunction.GstrGbPrm, sSetName, sDeptDr, sOrderCode, sRowId);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            clsOrdFunction.GstrViewSlipCallPosition = "";
            if (rEventClosed != null)
            {
                rEventClosed();
            }
            else
            {
                this.Close();
            }
        }

            private void LstSetName_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                LstSetName.ClearSelected();
            }
        }

        private void rdoGbOrder1_Click(object sender, EventArgs e)
        {
            fn_View_Click(e);
        }

        private void rdoGbOrder2_Click(object sender, EventArgs e)
        {
            fn_View_Click(e);
        }

        private void rdoGbOrder3_Click(object sender, EventArgs e)
        {
            fn_View_Click(e);
        }

        private void rdoGbOrder4_Click(object sender, EventArgs e)
        {
            fn_View_Click(e);
        }

        private void rdoGbOrder5_Click(object sender, EventArgs e)
        {
            fn_View_Click(e);
        }

        void fn_View_Click(EventArgs e)
        {
            if (btnSetPersonal.BackColor == Color.RoyalBlue)
            {
                btnSetPersonal_Click(btnSetPersonal, e);
            }
            else if (btnSetDept.BackColor == Color.RoyalBlue)
            {
                btnSetDept_Click(btnSetDept, e);
            }
            else if (btnSetExam.BackColor == Color.RoyalBlue)
            {
                btnSetExam_Click(btnSetExam, e);
            }
            else if (btnEM.BackColor == Color.RoyalBlue)
            {
                btnEM_Click(btnEM, e);
            }
        }

        private void txtSet_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strDeptDr = "";
            int nGbn = 0;

            if (e.KeyChar == (char)13)
            {
                if (clsOrdFunction.GstrGbPrm == "PROM")
                {
                    strDeptDr = clsType.User.Sabun;
                    nGbn = 1;
                }
                else if (clsOrdFunction.GstrGbPrm == "DPROM")
                {
                    strDeptDr = clsPublic.GstrDeptCode.Trim();
                    nGbn = 2;
                }
                else if (clsOrdFunction.GstrGbPrm == "EXAM")
                {
                    strDeptDr = clsType.User.Sabun;
                    nGbn = 3;
                }
                else if (clsOrdFunction.GstrGbPrm == "EM")
                {
                    strDeptDr = "EM";
                    nGbn = 5;
                }

                fn_Read_Set(nGbn, strDeptDr);
            }
        }

        private void btnAllOrderSelect_Click(object sender, EventArgs e)
        {
            if (ssSetOrder.ActiveSheet.NonEmptyRowCount == 0) return;
            ssSetOrder.ActiveSheet.Cells[0, 0, (int)ssSetOrder.ActiveSheet.RowCount - 1, (int)ssSetOrder.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 150);
        }

        private void txtSet_KeyUp(object sender, KeyEventArgs e)
        {
            string strDeptDr = "";
            int nGbn = 0;

            if (clsOrdFunction.GstrGbPrm == "PROM")
            {
                strDeptDr = clsType.User.Sabun;
                nGbn = 1;
            }
            else if (clsOrdFunction.GstrGbPrm == "DPROM")
            {
                strDeptDr = clsPublic.GstrDeptCode.Trim();
                nGbn = 2;
            }
            else if (clsOrdFunction.GstrGbPrm == "EXAM")
            {
                strDeptDr = clsType.User.Sabun;
                nGbn = 3;
            }
            else if (clsOrdFunction.GstrGbPrm == "EM")
            {
                strDeptDr = "EM";
                nGbn = 5;
            }

            fn_Read_Set(nGbn, strDeptDr);
        }

        private void LstSetName_DoubleClick(object sender, EventArgs e)
        {
            LstSetName_Click(sender, e);
            //btnOrderInput_Click(sender, e);
            btnAllInput_Click(sender, e);

            //【약속처방후 창닫기 】
            string strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "PRMCLOSE");
            if (VB.Val(strEmrOption) == 1)
            {
                clsOrdFunction.GstrViewSlipCallPosition = "";
                if (rEventClosed != null)
                {
                    rEventClosed();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void ssSetDiagnosis_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SetDiagInput(ssSetDiagnosis.ActiveSheet.Cells[e.Row, 0].Text.Trim());
        }

        private void btnSetDeleteAll_Click(object sender, EventArgs e)
        {
            if (DeleteSetAll() == true)
            {
                txtSetName.Text = "";

                if (btnSetPersonal.BackColor == Color.RoyalBlue)
                {
                    btnSetPersonal_Click(btnSetPersonal, new EventArgs());
                }
                else if (btnSetDept.BackColor == Color.RoyalBlue)
                {
                    btnSetDept_Click(btnSetDept, new EventArgs());
                }
                else if (btnSetExam.BackColor == Color.RoyalBlue)
                {
                    btnSetExam_Click(btnSetExam, new EventArgs());
                }
                else if (btnEM.BackColor == Color.RoyalBlue)
                {
                    btnEM_Click(btnSetExam, new EventArgs());
                }
            }
        }

        private bool DeleteSetAll()
        {
            //if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            //{
            //    return false; //권한 확인
            //}

            if (MessageBox.Show("선택된 약속 처방을 삭제 하시겠습니까?  ", "약속처방삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return false; //권한 확인
            }
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            string strDeptDr = "";

            strDeptDr = strDeptDrPRM;

            if (VB.Left(strDeptDrPRM, 2) == "IU")
            {
                strDeptDr = VB.Left(strDeptDrPRM, 5);
            }

            try
            {
                SQL = "";
                SQL += " DELETE ADMIN.OCS_OPRM                             \r";
                if (strDeptDr.Trim() == "MG" || strDeptDr.Trim() == "MC" || strDeptDr.Trim() == "MP" ||
                    strDeptDr.Trim() == "ME" || strDeptDr.Trim() == "MN" || strDeptDr.Trim() == "MR" || strDeptDr.Trim() == "MI")
                {
                    SQL += "  WHERE DeptDr IN('" + strDeptDr.Trim() + "', 'MD')             \r";
                }
                else
                {
                    SQL += "  WHERE DeptDr = '" + strDeptDr.Trim() + "'                     \r";
                }
                SQL += "    AND PRMNAME = '" + txtSetName.Text.Trim() + "'      \r";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("삭제중 오류가 발생하였습니다.");
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }




    }
}
