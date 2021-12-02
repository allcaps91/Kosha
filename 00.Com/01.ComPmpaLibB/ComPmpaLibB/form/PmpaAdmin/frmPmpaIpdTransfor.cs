using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComPmpaLibB
{
    public partial class frmPmpaIpdTransfor : Form
    {
        string FstrSPC = "", FstrSPC2 = "";
        
        public frmPmpaIpdTransfor()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            this.cboSetWard.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.cboRoom.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.cboDept.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.cboDrCode.KeyPress     += new KeyPressEventHandler(eKeyPress);
            this.dtpTrsDate.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.txtPano.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.txtViewPano.KeyPress   += new KeyPressEventHandler(eKeyPress);

            this.cboDept.SelectedIndexChanged   += new EventHandler(eCboChange);
            this.cboDrCode.SelectedIndexChanged += new EventHandler(cboDrCode_SelectedIndexChanged);
        }

        void eCboChange(object sender, EventArgs e)
        {
            if (sender == cboDept)
            {
                Setting_Dept(clsDB.DbCon, cboDept, txtDeptName2);
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == cboSetWard)
                {
                    cboRoom.Focus();
                }
                else if (sender == cboRoom)
                {
                    cboDept.Focus();
                }
                else if (sender == cboDept)
                {
                    cboDrCode.Focus();
                }
                else if (sender == cboDrCode)
                {
                    dtpTrsDate.Focus();
                }
                else if (sender == dtpTrsDate)
                {
                    btnSave.Focus();
                }
                else if (sender == txtPano)
                {
                    txtPano.Text = VB.Format(VB.Val(txtPano.Text), "00000000");
                    btnSearch.Focus();
                }
                else if (sender == txtViewPano)
                {
                    txtViewPano.Text = VB.Format(VB.Val(txtViewPano.Text), "00000000");
                    txtViewSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtViewPano.Text);
                    Screen_Display_Sel(txtViewPano.Text);
                    btnSearchSel.Focus();
                }
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmPmpaIpdTransfor_Load(object sender, EventArgs e)
        {
            cboWard.Items.Clear();
            cboWard.Items.Add(" ");
            clsVbfunc.SetWardCodeCombo(clsDB.DbCon,  cboWard, "2", false, 2);

            cboSetWard.Items.Clear();
            cboSetWard.Items.Add("전체");
            clsVbfunc.SetWardCodeCombo(clsDB.DbCon, cboSetWard, "2", true, 2);

            clsVbfunc.SetComboDept(clsDB.DbCon,cboDept, "2", 2);

            cboSel.Items.Clear();
            cboSel.Items.Add("0");
            cboSel.Items.Add("1");
            //cboSel.SelectedIndex = 0;
            Clear_Screen();

            clsLockCheck.GstrLockPtno = "";
        }

        void cboSetWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (cboSetWard.Text.Trim() == "전체") { return; }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT RoomCode ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ROOM ";
                SQL += ComNum.VBLF + "  WHERE WardCode='" + cboSetWard.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND TBed > 0 ";
                SQL += ComNum.VBLF + "  ORDER BY RoomCode ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                cboRoom.Items.Clear();
                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    cboRoom.Items.Add(Dt.Rows[i]["ROOMCODE"].ToString().Trim());
                }
                cboRoom.SelectedIndex = -1;

                Dt.Dispose();
                Dt = null;

                for (i = 0; i < cboRoom.Items.Count; i++)
                {
                    if (Convert.ToInt16(cboRoom.Items[i]) == Convert.ToInt16(clsPmpaType.IMST.RoomCode))
                    {
                        cboDept.SelectedIndex = i;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return;
            }
        }

        void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (cboDept.Text.Trim() == "전체") { return; }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT DRCODE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL += ComNum.VBLF + "  WHERE (DrDept1='" + cboDept.Text.Trim() + "'  ";
                SQL += ComNum.VBLF + "     OR  DrDept2='" + cboDept.Text.Trim() + "') ";
                SQL += ComNum.VBLF + "    AND (Tour='N' OR Tour IS NULL) ";
                SQL += ComNum.VBLF + "  ORDER BY DrCode ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                cboDrCode.Items.Clear();
                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    cboDrCode.Items.Add(Dt.Rows[i]["DRCODE"].ToString().Trim());
                }
                cboDrCode.SelectedIndex = 0;

                Dt.Dispose();
                Dt = null;

                for (i = 0; i < cboDrCode.Items.Count; i++)
                {
                    if ((string)cboDrCode.Items[i] == clsPmpaType.IMST.DrgCode)
                    {
                        cboDrCode.SelectedIndex = i;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return;
            }
        }

        void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            Display_Patient_List(cboWard.Text, txtPano.Text);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            Display_Patient_List(cboWard.Text, txtPano.Text);
        }

        void Display_Patient_List(string strWard, string strPano)
        {
            int i = 0;
            int nREAD = 0;
            int nRow = 0;

            DataTable Dt = new DataTable();
            clsPmpaFunc clsPF = new clsPmpaFunc();
            
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SSList.ActiveSheet.ClearRange(0, 0, SSList_Sheet1.Rows.Count, SSList_Sheet1.ColumnCount, false);
            SSList_Sheet1.Rows.Count = 0;

            try
            {
                Dt = clsPF.Get_Ipd_New_Master(clsDB.DbCon, strPano, strWard, 0);
                nREAD = Dt.Rows.Count;

                if (Dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                for (i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (nRow > SSList_Sheet1.Rows.Count)
                    {
                        SSList_Sheet1.Rows.Count = nRow;
                    }
                    SSList_Sheet1.Cells[i, 0].Text = VB.Format(Convert.ToInt32(Dt.Rows[i]["PANO"].ToString()), "00000000");
                    SSList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SName"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["RoomCode"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["InDate"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["IPDNO"].ToString().Trim();

                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return;
            }
        }
        
        void SSList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            clsIument clsIU = new clsIument();

            //string strLockFlag = "";
            long nIPDNO = 0;

            if (e.Row < 0 || e.Column < 0) { return; }

            nIPDNO = Convert.ToInt32(SSList_Sheet1.Cells[e.Row, 5].Text);

            clsIU.Read_Ipd_Master(clsDB.DbCon, "", nIPDNO);
            clsIU.Read_Ipd_Mst_Trans(clsDB.DbCon, clsPmpaType.IMST.Pano, clsPmpaType.IMST.LastTrs, "");
            Screen_Display();
            txtViewPano.Text = VB.Format(Convert.ToInt32(clsPmpaType.TIT.Pano), "00000000");
            txtViewSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon,  txtViewPano.Text);
            Screen_Display_Sel(clsPmpaType.IMST.Pano);

            if (clsPmpaType.IMST.GbSTS != "0")
            {
                ComFunc.MsgBox("재원환자가 아닙니다.");
                Clear_Screen();
                //GstrLockTransPano = ""
                txtPano.Focus();
                return;
            }

            if (clsDB.DbCon.strDbOption != "Develop")
            { 
                clsLockCheck.GstrLockPtno = SSList_Sheet1.Cells[e.Row, 0].Text;
                clsLockCheck.GstrLockRemark = clsType.User.IdNumber + " " + clsType.User.UserName + "님이 전실전과 작업 중입니다.";
                string strLockFlag = clsLockCheck.IpdOcs_Lock_Insert_NEW();
                if (strLockFlag == "NO")
                {
                    Clear_Screen();
                    clsLockCheck.GstrLockPtno = "";
                    txtPano.Focus();
                    return;
                }
            }

            cboSetWard.Focus();

        }

        void btnSearchSel_Click(object sender, EventArgs e)
        {
            Screen_Display_Sel(txtViewPano.Text);
        }

        void Screen_Display()
        {
            int Inx;

            if (clsLockCheck.GstrLockPtno != "")
            {
                clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                clsLockCheck.GstrLockPtno = "";
            }
         
            clsPmpaSel clsPS = new clsPmpaSel();

            txtWard.Text = clsPmpaType.IMST.WardCode;
            txtRoomCode.Text = clsPmpaType.IMST.RoomCode.ToString();
            txtDept.Text = clsPmpaType.IMST.DeptCode;
            txtDrCode.Text = clsPmpaType.IMST.DrCode;
            txtDeptName.Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, clsPmpaType.IMST.DeptCode);
            txtDrName.Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, clsPmpaType.IMST.DrCode);
            txtSel.Text = clsPmpaType.IMST.GbSpc;

            if (txtSel.Text == "1") { txtSel.BackColor = Color.Wheat; }
            txtSelDate.Text = ComFunc.READ_PANO_SELECT_MST_BDATE(clsDB.DbCon,clsPmpaType.IMST.Pano, "I", clsPmpaType.IMST.DrCode, clsPublic.GstrSysDate);

            Inx = cboSetWard.FindString(clsPmpaType.IMST.WardCode);
            cboSetWard.SelectedIndex = Inx;

            Inx = cboRoom.FindString(clsPmpaType.IMST.RoomCode.ToString());
            cboRoom.SelectedIndex = Inx;

            Inx = cboDept.FindString(clsPmpaType.IMST.DeptCode);
            cboDept.SelectedIndex = Inx;

            Inx = cboDrCode.FindString(clsPmpaType.IMST.DrCode);
            cboDrCode.SelectedIndex = Inx;
            
            dtpTrsDate.Text = clsPublic.GstrSysDate;

            Screen_Display_IPD_TransFor();

        }

        void Screen_Display_Sel(string strPano)
        {
            int i = 0;
            int nREAD = 0;
            int nRow = 0;
            string strNew = "";
            string strOLD = "";
            //string strIO = "";
            string strUse = "";

            DataTable Dt = new DataTable();
            clsPmpaFunc clsPF = new clsPmpaFunc();
            clsSpread cSpd = new clsSpread();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SS_SEL.ActiveSheet.ClearRange(0, 0, SS_SEL_Sheet1.Rows.Count, SS_SEL_Sheet1.ColumnCount, false);
            SS_SEL_Sheet1.Rows.Count = 0;

            try
            {
                Dt = clsPF.Get_Bas_Select_Mst(clsDB.DbCon, strPano, "");
                nREAD = Dt.Rows.Count;

                if (Dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (nREAD == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                for (i = 0; i < nREAD; i++)
                {
                    strNew = Dt.Rows[i]["DrCode"].ToString().Trim();

                    nRow += 1;
                    if (nRow > SS_SEL_Sheet1.Rows.Count)
                    {
                        SS_SEL_Sheet1.Rows.Count = nRow;
                    }

                    if (strNew != strOLD)
                    {
                        SS_SEL_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["GUBUN"].ToString().Trim();
                        SS_SEL_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        SS_SEL_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DRCODE"].ToString().Trim();
                        SS_SEL_Sheet1.Cells[i, 3].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon,Dt.Rows[i]["DRCODE"].ToString().Trim());
                    }

                    strUse = "Y";
                    if (Dt.Rows[i]["DelDate"].ToString().Trim() != "")
                    {
                        strUse = "";
                    }
                    SS_SEL_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["SDate"].ToString().Trim();
                    SS_SEL_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["EDate"].ToString().Trim();
                    SS_SEL_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["DelDate"].ToString().Trim();
                    SS_SEL_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["EntDate"].ToString().Trim();
                    SS_SEL_Sheet1.Cells[i, 8].Text = clsVbfunc.GetInSaName(clsDB.DbCon, Dt.Rows[i]["EntSabun"].ToString().Trim());

                    strOLD = Dt.Rows[i]["DrCode"].ToString().Trim();
                    
                    if (strUse != "Y")
                    {
                        cSpd.setSpdCellColor(SS_SEL, i, 5, i, 7, Color.FromArgb(255, 155, 155));
                    }
                    else
                    {
                        cSpd.setSpdCellColor(SS_SEL, i, 5, i, 7, Color.FromArgb(255, 255, 255));
                    }

                }

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return;
            }

        }

        void Screen_Display_IPD_TransFor()
        {
            int i = 0;
            int nREAD = 0;
            int nRow = 0;

            DataTable Dt = new DataTable();
            clsPmpaFunc clsPF = new clsPmpaFunc();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수


            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
            SS1_Sheet1.Rows.Count = 0;

            try
            {
                Dt = clsPF.Get_Ipd_Transfor(clsDB.DbCon, clsPmpaType.IMST.Pano, clsPmpaType.IMST.InDate);
                nREAD = Dt.Rows.Count;

                if (Dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                for (i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (nRow > SS1_Sheet1.Rows.Count)
                    {
                        SS1_Sheet1.Rows.Count = nRow;
                    }
                    SS1_Sheet1.Cells[i, 0].Text = VB.Left(Dt.Rows[i]["TrsDate"].ToString(), 10);
                    SS1_Sheet1.Cells[i, 1].Text = VB.Right(Dt.Rows[i]["TrsDate"].ToString(), 5);
                    SS1_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["FrWard"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["FrRoom"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["FrDept"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 5].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon,Dt.Rows[i]["FrDoctor"].ToString().Trim());
                    SS1_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["FrSPC"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 7].Text = "☞";

                    SS1_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["ToWard"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["ToRoom"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["ToDept"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 11].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon,Dt.Rows[i]["ToDoctor"].ToString().Trim());
                    SS1_Sheet1.Cells[i, 12].Text = Dt.Rows[i]["ToSPC"].ToString().Trim();

                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return;
            }

        }

        void Clear_Screen()
        {
            SSList.ActiveSheet.ClearRange(0, 0, SSList_Sheet1.Rows.Count, SSList_Sheet1.ColumnCount, false);
            SSList_Sheet1.Rows.Count = 0;

            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
            SS1_Sheet1.Rows.Count = 0;

            SS_SEL.ActiveSheet.ClearRange(0, 0, SS_SEL_Sheet1.Rows.Count, SS_SEL_Sheet1.ColumnCount, false);
            SS_SEL_Sheet1.Rows.Count = 0;

            txtPano.Text = "";
            txtRoomCode.Text = "";
            txtDept.Text = ""; txtDeptName.Text = "";
            txtDrCode.Text = ""; txtDrName.Text = "";
            txtSel.Text = ""; txtSelDate.Text = "";
            txtSel.BackColor = Color.White;

            cboSetWard.SelectedIndex = -1;
            cboRoom.SelectedIndex = -1;
            cboDept.SelectedIndex = 0; txtDeptName2.Text = "";
            cboDrCode.SelectedIndex = -1; txtDrName2.Text = "";
            //cboSel.SelectedIndex = -1;
            txtSelDate2.Text = "";
            dtpTrsDate.Text = clsPublic.GstrSysDate;

            chkTrans.Checked = true;

            txtViewPano.Text = "";

            txtPano.Focus();
        }
        
        void btnCancel_Click(object sender, EventArgs e)
        {
            if (clsLockCheck.GstrLockPtno != "")
            {
                clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                clsLockCheck.GstrLockPtno = "";
            }
            Clear_Screen();
            txtPano.Focus();
        }

        void frmPmpaIpdTransfor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (clsLockCheck.GstrLockPtno != "")
            {
                clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                clsLockCheck.GstrLockPtno = "";
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            #region 변수생성
            int nJewon = 0, nTrCnt = 0, nTrIlsu = 0, nIlsu = 0;
            int nTBed1 = 0, nTBed2 = 0, nHBed = 0;
            string strDept1 = "", strDoct1 = "", strWard1 = "", strRoom1 = "";
            string strDept2 = "", strDoct2 = "", strWard2 = "", strRoom2 = "";
            string strFWard = "", strInDate = "", strBi = "", strAge = "", strSex = "";
            string strKekli1 = "", strTWard = "", strRemark = "", strKekli2 = "";
            #endregion

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            clsIuSentChk clsIuSchk = new clsIuSentChk();

            ComFunc.ReadSysDate(clsDB.DbCon);

            if (clsPmpaType.IMST.IPDNO == 0) { return; }

            if (cboDept.Text == "MD")
            {
                ComFunc.MsgBox("세부내과를 선택하십시오.");
                return;
            }

            strDept1 = clsPmpaType.IMST.DeptCode;
            strDoct1 = clsPmpaType.IMST.DrCode;
            strWard1 = clsPmpaType.IMST.WardCode;
            strRoom1 = clsPmpaType.IMST.RoomCode.ToString();
            strFWard = strWard1;

            strKekli1 = clsPmpaType.IMST.GbKekli;
            strInDate = VB.Left(clsPmpaType.IMST.InDate, 10);
            strBi = clsPmpaType.IMST.Bi;
            strAge = clsPmpaType.IMST.Age.ToString();
            strSex = clsPmpaType.IMST.Sex;
            nJewon = clsPmpaType.IMST.Ilsu;

            if (clsPmpaType.IMST.Dept1 != "") { nTrCnt = 1; }
            if (clsPmpaType.IMST.Dept2 != "") { nTrCnt = 2; }
            if (clsPmpaType.IMST.Dept2 != "") { nTrCnt = 3; }

            nTrIlsu += clsPmpaType.IMST.Ilsu1;
            nTrIlsu += clsPmpaType.IMST.Ilsu2;
            nTrIlsu += clsPmpaType.IMST.Ilsu3;

            nIlsu = nJewon - nTrIlsu;

            //변경한 내역을 변수에 저장
            strDept2 = cboDept.Text.Trim();
            strDoct2 = VB.Left(cboDrCode.Text.Trim(), 4);
            strWard2 = cboSetWard.Text.Trim();
            strRoom2 = cboRoom.Text.Trim();

            if (strDept2 == "") { ComFunc.MsgBox("진료과가 공란입니다."); return; }
            if (strDoct2 == "") { ComFunc.MsgBox("의사코드가 공란입니다."); return; }
            if (strWard2 == "") { ComFunc.MsgBox("병동코드가 공란입니다."); return; }
            if (strRoom2 == "") { ComFunc.MsgBox("병실코드가 공란입니다."); return; }

            if (clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, strDept2) == "")
            {
                ComFunc.MsgBox("진료과 오류"); return;
            }
            if (clsVbfunc.GetBASDoctorName(clsDB.DbCon,strDoct2) == "")
            {
                ComFunc.MsgBox("의사코드 오류"); return;
            }
            if (clsIuSchk.Chk_WardCode(clsDB.DbCon, strWard2) == false)
            {
                ComFunc.MsgBox("병동코드 오류"); return;
            }
            if (clsIuSchk.Chk_RoomCode(clsDB.DbCon, strWard2, strRoom2) == "")
            {
                ComFunc.MsgBox("병실코드 오류"); return;
            }

            strTWard = strWard2;

            strRemark = strRoom1 + "(" + strDept1 + ")=>" + strRoom2 + "(" + strDept2 + ")";
            strKekli2 = clsPmpaType.IMST.GbKekli;

            //선택진료관련 체크
            FstrSPC = "";
            FstrSPC2 = "";

            if (clsPmpaPb.GstrSelUse == "OK")
            {
                FstrSPC = txtSel.Text.Trim();
                FstrSPC2 = cboSel.Text.Trim();

                //2011-08-19 소아과 입원 선택없음
                if (strDept2 == "PD" && FstrSPC2 == "1")
                {
                    FstrSPC2 = "0";
                }

                if (txtSelDate.Text.Trim() == "선택진료 N" && cboSel.Text.Trim() == "1")
                {
                    ComFunc.MsgBox("선택진료로 변경할 수 없습니다.");
                    return;
                }

                if (txtSelDate2.Text.Trim() == "" && cboSel.Text.Trim() == "1" && strDept2 != "PD")
                {
                    if (ComFunc.MsgBoxQ("선택신청서가 없습니다. 이대로 작업하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }
            }

            if (strWard1 == strWard2 && strRoom1 == strRoom2 && strDept1 == strDept2 && strKekli1 == strKekli2)
            {
                if (strDoct1 != strDoct2)
                {
                    if (ComFunc.MsgBoxQ("의사만 변경되는 작업입니다.. 작업하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        SEL_Doctor_Change_Process();

                        if (clsLockCheck.GstrLockPtno != "")
                        {
                            clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                            clsLockCheck.GstrLockPtno = "";
                        }

                        ComFunc.MsgBox("의사변경 작업 완료!!");

                        Clear_Screen();
                        txtPano.Focus();
                    }
                }
                else
                {
                    ComFunc.MsgBox("전실전과 변경한 내역이 현재 입원마스타와 동일함.");
                }
                return;
            }

            //if (clsPmpaPb.GstrSelUse == "OK")
            //{
            //    if (cboSel.Text == "") { ComFunc.MsgBox("변경할 선택진료 구분이 공란입니다."); return; }
            //    if (txtSel.Text == "") { ComFunc.MsgBox("변경전 선택진료구분 공란입니다."); return; }
            //    if (cboSel.Text != "0" && cboSel.Text != "1") { ComFunc.MsgBox("변경할 선택진료 구분값 오류입니다.(0 or 1)"); return; }
            //    if (txtSel.Text != "0" && txtSel.Text != "1") { ComFunc.MsgBox("변경전 선택진료 구분값 오류입니다.(0 or 1)"); return; }
            //}

            #region 실제 Data UpDate

             
            clsDB.setBeginTran(clsDB.DbCon);

            #region UpDate_Ipd_NEW_Master 재원 Master
            SQL = "";
            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL += ComNum.VBLF + " SET WardCode = '" + strWard2 + "', ";
            SQL += ComNum.VBLF + "     RoomCode = '" + strRoom2 + "', ";
            SQL += ComNum.VBLF + "     DeptCode = '" + strDept2 + "', ";
            SQL += ComNum.VBLF + "     DrCode   = '" + strDoct2 + "', ";
            SQL += ComNum.VBLF + "     GbKekli  = '" + strKekli2 + "', ";
            SQL += ComNum.VBLF + "     GBSPC ='" + cboSel.Text.Trim() + "', ";
            if (strWard2 == "IU")
            {
                SQL += ComNum.VBLF + "  FROMTRANS = '" + strRoom2 + "' ,";
            }
            else
            {
                SQL += ComNum.VBLF + "  FROMTRANS = '' ,";
            }
            SQL += ComNum.VBLF + "     TrsDate = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')  ";
            if (chkTrans.Checked == true)
            {
                if (strDept1 != strDept2)
                {
                    switch (nTrCnt)
                    {
                        case 1:
                            SQL += ComNum.VBLF + ", Dept1   = '" + strDept2 + "',  ";
                            SQL += ComNum.VBLF + "  Doctor1 = '" + strDoct2 + "',  ";
                            SQL += ComNum.VBLF + "  Ilsu1   =  " + nIlsu + "";
                            break;
                        case 2:
                            SQL += ComNum.VBLF + ", Dept3   = '" + strDept2 + "',  ";
                            SQL += ComNum.VBLF + "  Doctor3 = '" + strDoct2 + "',  ";
                            SQL += ComNum.VBLF + "  Ilsu3   =  " + nIlsu + "";
                            break;
                        case 4:
                            SQL += ComNum.VBLF + ", Ilsu3 = " + nIlsu + "";
                            break;
                    }
                }
            }
            SQL += ComNum.VBLF + " WHERE IPDNO = " + clsPmpaType.IMST.IPDNO + " ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;

            }
            #endregion

            #region UpDate_Ipd_TRANS 입원 Trans
            SQL = "";
            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS ";
            SQL += ComNum.VBLF + "    SET DeptCode = '" + strDept2 + "', ";
            SQL += ComNum.VBLF + "        GBSPC    = '" + cboSel.Text.Trim() + "', ";
            SQL += ComNum.VBLF + "        DrCode   = '" + strDoct2 + "'  ";
            SQL += ComNum.VBLF + " WHERE TRSNO = " + clsPmpaType.IMST.LastTrs + " ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;

            }
            #endregion

            #region UpDate_Bas_Patient 환자 Master
            SQL = "";
            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
            SQL += ComNum.VBLF + "    SET DrCode   = '" + strDoct2 + "', ";
            SQL += ComNum.VBLF + "        DeptCode = '" + strDept2 + "' ";
            SQL += ComNum.VBLF + "  WHERE Pano     = '" + clsPmpaType.IMST.Pano + "' ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;

            }
            #endregion

            #region Update_Bas_Room 호실 Master
            nTBed1 = 0;
            nTBed2 = 0;
            if (strRoom1 != strRoom2)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TBed, HBed  FROM " + ComNum.DB_PMPA +  "BAS_ROOM ";
                SQL += ComNum.VBLF + "  WHERE RoomCode = '" + strRoom1 + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                nTBed1 = Convert.ToInt16(Dt.Rows[0]["TBed"].ToString().Trim());
                nHBed = Convert.ToInt16(Dt.Rows[0]["HBed"].ToString().Trim()) - 1;

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_ROOM ";
                SQL += ComNum.VBLF + "    SET Hbed     = " + nHBed + " ";
                SQL += ComNum.VBLF + "  WHERE RoomCode = '" + strRoom1 + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;

                }

                //변경 후
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TBed, HBed  FROM " + ComNum.DB_PMPA + "BAS_ROOM ";
                SQL += ComNum.VBLF + "  WHERE RoomCode = '" + strRoom2 + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                nTBed2 = Convert.ToInt16(Dt.Rows[0]["TBed"].ToString().Trim());
                nHBed = Convert.ToInt16(Dt.Rows[0]["HBed"].ToString().Trim()) - 1;

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_ROOM ";
                SQL += ComNum.VBLF + "    SET Hbed     = " + nHBed + " ";
                SQL += ComNum.VBLF + "  WHERE RoomCode = '" + strRoom2 + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;                    
                }
                
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TBed FROM " + ComNum.DB_PMPA + "BAS_ROOM ";
                SQL += ComNum.VBLF + "  WHERE RoomCode = '" + strRoom1 + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                nTBed1 = Convert.ToInt16(Dt.Rows[0]["TBed"].ToString().Trim());
                nTBed2 = Convert.ToInt16(Dt.Rows[0]["TBed"].ToString().Trim());

                Dt.Dispose();
                Dt = null;
                
            }
            #endregion

            #region 전실전과 저장않함 관련 Update_Ipd_Transfor
            if (chkTrans.Checked == false)
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "IPD_TRANSFOR (";
                SQL += ComNum.VBLF + "    Pano,     TrsDate,  Sname, ";
                SQL += ComNum.VBLF + "    FrWard,   FrRoom,   FrRoomBedCount,   FrDept,   FrDoctor, FrKekli,  ";
                SQL += ComNum.VBLF + "    ToWard,   ToRoom,   ToRoomBedCount,   ToDept,   ToDoctor, ToKekli,  ";
                SQL += ComNum.VBLF + "    Password, Part,     IPDNO,            FrSPC,    ToSPC ) ";
                SQL += ComNum.VBLF + " VALUES ('" + clsPmpaType.IMST.Pano + "',SYSDATE,'" + clsPmpaType.IMST.Sname + "',";
                SQL += ComNum.VBLF + " '" + strWard1 + "','" + strRoom1 + "' ," + nTBed1 + ",";
                SQL += ComNum.VBLF + " '" + strDept1 + "','" + strDoct1 + "' ,'" + strKekli1 + "','" + strWard2 + "',";
                SQL += ComNum.VBLF + " '" + strRoom2 + "', " + nTBed2 + "    ,'" + strDept2 + "', ";
                SQL += ComNum.VBLF + " '" + strDoct2 + "','" + strKekli2 + "','" + clsPublic.GstrJobSabun + "','@',";
                SQL += ComNum.VBLF + clsPmpaType.IMST.IPDNO + ",'" + FstrSPC + "','" + FstrSPC2 + "' ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }
                
            }
            #endregion

            #region Update_Diet_Order 영양실 Order
            SQL = "";
            SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "DIET_ORDER ";
            SQL += ComNum.VBLF + "   SET WardCode = '" + strWard2 + "',";
            SQL += ComNum.VBLF + "       RoomCode = '" + strRoom2 + "' ";
            SQL += ComNum.VBLF + " WHERE Pano = '" + clsPmpaType.IMST.Pano + "' ";
            SQL += ComNum.VBLF + "   AND OrderDate = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;
            }
            #endregion

            #region UpDate_NUR_Master 간호부 환자 마스타
            if (chkTrans.Checked == false)
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "NUR_MASTER ";
                SQL += ComNum.VBLF + "    SET ReqTransforTime='',";
                SQL += ComNum.VBLF + "        ReqTransfor ='' ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.IMST.Pano + "' ";
                SQL += ComNum.VBLF + "    AND InDate=TO_DATE('" + VB.Left(clsPmpaType.IMST.InDate, 10) + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND ReqTransforTime IS NOT NULL ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "NUR_MASTER ";
                SQL += ComNum.VBLF + "    SET ReqTransforTime='',";
                SQL += ComNum.VBLF + "        INWARD = '" + cboSetWard.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "        INROOM = '" + cboRoom.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "        INDEPT = '" + cboDept.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "        INDRCODE = '" + VB.Left(cboDrCode.Text.Trim(), 4) + "', ";
                SQL += ComNum.VBLF + "        OUTILSU = 0 ,                             ";
                SQL += ComNum.VBLF + "        ReqTransfor ='' ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.IMST.Pano + "' ";
                SQL += ComNum.VBLF + "    AND InDate=TO_DATE('" + VB.Left(clsPmpaType.IMST.InDate, 10) + "','YYYY-MM-DD') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                //2020-12-12 현재 재원 중 입원병동 변경건 계속 발생하여 원인 분석위한 히스토리 추가(김현욱)
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "NUR_MASTER_TESTPAT_0807(  ";
                SQL += ComNum.VBLF + " PANO, INDATE, INWARD, INROOM, ";
                SQL += ComNum.VBLF + " INDEPT, INDRCODE, ENTDATE, ENTSABUN ) VALUES ( ";
                SQL += ComNum.VBLF + " '" + clsPmpaType.IMST.Pano + "', TO_DATE('" + VB.Left(clsPmpaType.IMST.InDate, 10) + "','YYYY-MM-DD'), '" + cboSetWard.Text.Trim() + "', '" + cboRoom.Text.Trim() + "', ";
                SQL += ComNum.VBLF + " '" + cboDept.Text.Trim() + "', '" + VB.Left(cboDrCode.Text.Trim(), 4) + "', SYSDATE, '" + clsType.User.Sabun + "') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

            }
            
            #endregion

            #region UpDate_NUR_ReqTransfor 전실전과 신청 마스타
            SQL = "";
            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "NUR_ReqTransfor ";
            SQL += ComNum.VBLF + "    SET TrsTime = SYSDATE,";
            SQL += ComNum.VBLF + "        Status  = '2' ";
            SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.IMST.Pano + "' ";
            SQL += ComNum.VBLF + "    AND ReqTime>=TO_DATE('" + VB.Left(clsPmpaType.IMST.InDate, 10) + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND Status = '1' ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;
            }
            #endregion
            //clsConAcpInfo clsEmr = new clsConAcpInfo();

            //TODO : EMR 프로젝트 포함시키기
            NEW_TextEMR_TRANSFOR(clsPmpaType.IMST.Pano, clsPmpaType.IMST.InDate, strDept1, strDept2, strDoct1, strDoct2);

            clsDB.setCommitTran(clsDB.DbCon);

            #endregion

            if (clsLockCheck.GstrLockPtno != "")
            {
                clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                clsLockCheck.GstrLockPtno = "";
            }

            if (FstrSPC == "1" || FstrSPC2 == "1")
            {
                ComFunc.MsgBox("전실전과 저장 완료!!" + ComNum.VBLF + "선택의사관련 변경건은 반드시 자격을 잘라주시고 개인별 신청서변경을 해주십시오.");
            }

            Clear_Screen();
            txtPano.Focus();

        }
         bool NEW_TextEMR_TRANSFOR(string strPatid, string strBDate, string strDeptCode, string strDeptCode2, string strDrCode, string strDrCode2)
        {

            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strJumin = "";   //주민암호화
            string strOK = "";
            string strDrCode2Sabun = "";
            string strDept = "";
           
            strBDate = VB.Format(Convert.ToDateTime(strBDate) , "yyyyMMdd");

            try
            {
                SQL = "SELECT P.PANO, P.SNAME, P.SEX, P.JUMIN1 ,P.JUMIN2,  P.JUMIN3, E.PATID , E.ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT  P , " + ComNum.DB_EMR + "EMR_PATIENTT E";
                SQL = SQL + ComNum.VBLF + "WHERE E.PATID (+)=P.PANO";
                SQL = SQL + ComNum.VBLF + "AND P.PANO ='" + strPatid.ToString().Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (dt.Rows[0]["PATID"].ToString().Trim() == "")    //EMR_PATIENTT 테이블에 환자가 없다.
                {
                    strJumin = dt.Rows[i]["Jumin1"].ToString().Trim() + VB.Left(dt.Rows[i]["Jumin2"].ToString().Trim(), 1) + "******";

                    SQL = "INSERT INTO " + ComNum.DB_EMR + "EMR_PATIENTT(PATID, JUMINNO, NAME, SEX) ";
                    SQL = SQL + ComNum.VBLF + "VALUES('" + dt.Rows[0]["Pano"].ToString().Trim() + ", ";
                    SQL = SQL + ComNum.VBLF + "'" + strJumin + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + dt.Rows[0]["sName"].ToString().Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + dt.Rows[0]["Sex"].ToString().Trim() + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                else
                {
                    strJumin = dt.Rows[i]["Jumin1"].ToString().Trim() + VB.Left(dt.Rows[i]["Jumin2"].ToString().Trim(), 1) + "******";

                    SQL = "UPDATE " + ComNum.DB_EMR + "EMR_PATIENTT";
                    SQL = SQL + ComNum.VBLF + "SET NAME ='" + dt.Rows[i]["sName"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + ", SEX  ='" + dt.Rows[i]["Sex"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + ", JUMINNO ='" + strJumin + "'";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                dt.Dispose();
                dt = null;

                // 입원
                SQL = "SELECT  S.PANO, TO_CHAR(S.INDATE, 'YYYYMMDD') INDATE,  TO_CHAR(S.OUTDATE, 'YYYYMMDD') OUTDATE, S.DeptCode, S.ROWID,D.SABUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ipd_new_master S , kosmos_ocs.ocs_doctor d";
                SQL = SQL + ComNum.VBLF + "WHERE S.DrCode = d.drcode";
                SQL = SQL + ComNum.VBLF + "AND S.PANO = '" + strPatid + "'";
                SQL = SQL + ComNum.VBLF + "AND TRUNC(S.InDate) = TO_DATE('" + strBDate + "', 'YYYYMMDD')";
                SQL = SQL + ComNum.VBLF + "AND S.EMR ='1'"; //처리된것만 전실전과처리

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "SELECT TREATNO, ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_TREATT";
                SQL = SQL + ComNum.VBLF + "WHERE PATID = '" + strPatid + "'";
                SQL = SQL + ComNum.VBLF + "AND INDATE  ='" + strBDate + "'";
                SQL = SQL + ComNum.VBLF + "AND CLINCODE = '" + strDeptCode + "'";
                SQL = SQL + ComNum.VBLF + "AND CLASS = 'I'";

                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    // 의사코드를 사번 읽기
                    SQL = "SELECT SABUN";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                    SQL = SQL + ComNum.VBLF + "WHERE DRCODE ='" + strDrCode2 + "'"; //바뀔의사코드를 사번으로 읽음

                    SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    strDrCode2 = "";
                    if (dt.Rows.Count > 0)
                    {
                        strDrCode2Sabun = dt3.Rows[0]["SABUN"].ToString().Trim();

                        if (strDeptCode == "MD" && dt3.Rows[0]["SABUN"].ToString().Trim() == "19094" || dt.Rows[0]["SABUN"].ToString().Trim() == "30322")
                        {
                            strDept = "RA";
                        }
                        else
                        {
                            strDept = strDeptCode2;
                        }
                    }
                    else
                    {
                        dt3.Dispose();
                        dt3 = null;
                    }
                    if (strDrCode2Sabun != "")
                    {
                        SQL = "UPDATE " + ComNum.DB_EMR + "EMR_TREATT SET";
                        SQL = SQL + ComNum.VBLF + "DOCCODE = '" + VB.Val(strDrCode2Sabun) + "', ";
                        SQL = SQL + ComNum.VBLF + "ClinCode = '" + strDept + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt2.Rows[0]["ROWID"].ToString().Trim() + "'";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }

                dt2.Dispose();
                dt2 = null;

                dt.Dispose();
                dt = null;

                return true;
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
                Cursor.Current = Cursors.Default;
                return false;
            }
        }
        void SEL_Doctor_Change_Process()
        {
            string strDrCode = "";
            string strMsg = "";

            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            strDrCode = VB.Left(cboDrCode.Text, 4).Trim();

            if (clsPmpaType.IMST.GbSTS != "0" && clsPmpaType.IMST.GbSTS != "2")
            {
                ComFunc.MsgBox("재원환자가 아닙니다.");
                return;
            }
            if (clsPmpaType.IMST.MstCNT > 0)
            {
                if (clsPmpaType.IMST.MstCNT > 1)
                {
                    ComFunc.MsgBox("입원마스터가 2건 이상입니다.");
                    return;
                }

                 
                clsDB.setBeginTran(clsDB.DbCon);

                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL += ComNum.VBLF + "    SET DRCODE      = '" + strDrCode + "', ";
                SQL += ComNum.VBLF + "        GBSPC       = '" + cboSel.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "  WHERE PANO        = '" + txtPano.Text.Trim() + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + "    SET DRCODE      = '" + strDrCode + "', ";
                SQL += ComNum.VBLF + "        GBSPC       = '" + cboSel.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "  WHERE PANO        = '" + txtPano.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND GBSTS  IN ('0','2') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                //작업 History에 Insert
                strMsg = txtPano.Text + " ";
                strMsg += txtViewSName.Text + " ";
                strMsg += "(" +txtDrCode.Text + "=>" + cboDrCode.Text + ")";

                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_JOBHISTORY (";
                SQL += ComNum.VBLF + "        JOBTIME,JOBPROG,ENTNAME,REMARK,IPDNO ) VALUES ( ";
                SQL += ComNum.VBLF + "     SYSDATE,'수동변경',  '" + clsPublic.GstrJobName + "','" + strMsg + "'," + clsPmpaType.TIT.Ipdno.ToString() + ") ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            
        }

        void Setting_Dept(PsmhDb pDbCon, ComboBox cboDept, TextBox txtName)
        {
            int i = 0;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (cboDept.Text.Trim() == "")
                return;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DeptNameK FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT  ";
            SQL += ComNum.VBLF + " WHERE DeptCode = '" + cboDept.Text + "' ";
            //SQL += ComNum.VBLF + "   AND GBJUPSU = '1' ";       //접수가능한 진료과만
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (Dt.Rows.Count > 0)
            {
                txtName.Text = Dt.Rows[0]["DeptNameK"].ToString().Trim();
            }
            else
            {
                txtName.Text = "진료과코드 Err";
            }

            Dt.Dispose();
            Dt = null;

        }

        void cboDrCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();

            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT DrCode, DrName                                ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR              ";
                SQL += ComNum.VBLF + "  WHERE DrCode = '" + VB.Left(cboDrCode.Text, 4) + "' ";
                SQL += ComNum.VBLF + "    AND TOUR != 'Y'                                   ";
                SQL += ComNum.VBLF + "  ORDER BY DrCode                                     ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    txtDrName2.Text = Dt.Rows[0]["DrName"].ToString().Trim();
                }
                else
                {
                    txtDrName2.Text = "";
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return;
            }
        }

    }
}
