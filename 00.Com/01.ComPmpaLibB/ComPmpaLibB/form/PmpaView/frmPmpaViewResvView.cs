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
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;


namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpViewResvView.cs
    /// Description     : 예약 환자 조회
    /// Author          : 안정수
    /// Create Date     : 2017-08-24
    /// Update History  : 2017-11-02
    /// <history>       
    /// 실제 테스트 필요
    /// d:\psmh\OPD\oumsad\OUMSAD22.FRM(FrmResvView) => frmPmpViewResvView.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\OUMSAD22.FRM(FrmResvView)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewResvView : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        clsPmpaFunc CPF = new clsPmpaFunc();
        string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");        

        string mstrBarPano = "";
        string mstrBarDept = "";
        public frmPmpaViewResvView()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewResvView(string GstrBarPano, string GstrBarDept)
        {
            InitializeComponent();
            setEvent();

            mstrBarPano = GstrBarPano;
            mstrBarDept = GstrBarDept;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);

            this.cboDept.LostFocus += new EventHandler(eCtl_LostFocus);
            this.cboDoctor.LostFocus += new EventHandler(eCtl_LostFocus);
            this.cboDoctor.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboDept.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.ssList.CellClick += new CellClickEventHandler(Spread_Click);

        }
        void Spread_Click(object sender, CellClickEventArgs e)
        {
            clsSpread CS = new clsSpread();
            ComFunc CF = new ComFunc();
            clsPmpaFunc CPF = new clsPmpaFunc();

            if (sender == this.ssList)
            {
                if (e.ColumnHeader == true)
                {
                    CS.setSpdSort(ssList, e.Column, true);
                    return;
                }


            }
        }
        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.cboDept)
            {
                Load_DRCode();
                
            }
            else if (sender == this.cboDoctor)
            {
                Load_DRName();
            }
            else if (sender == this.txtPano)
            {
                btnView.Focus();
            }
        }

        private void eCtl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.cboDept)
            {
                Load_DRCode();
            }
            else if (sender == this.cboDoctor)
            {
                Load_DRName();
            }
            
        }

            void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  
            Load_DeptCode();

            Set_Combo();


            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddYears(+1);

            txtPano.Focus();

        }

        void Load_DeptCode()
        {
            int i = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            for (i = 0; i < 50; i++)
            {
                clsPmpaPb.GstrSetDeptCodes[i] = "";
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                ";
            SQL += ComNum.VBLF + " * FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT          ";
            SQL += ComNum.VBLF + "WHERE GBJUPSU   = '1' ";
            SQL += ComNum.VBLF + "Order By Printranking                                 ";

            try
            {
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

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        clsPmpaPb.GstrSetDeptCodes[i] = dt.Rows[i]["DeptCode"].ToString().Trim();
                        clsPmpaPb.GstrSetDepts[i] = dt.Rows[i]["DeptNameK"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }


        void Load_DRCode()
        {
         

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";



            SQL = "";
            SQL += ComNum.VBLF + " SELECT DrCode ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND (Tour IS NULL OR Tour != 'Y') ";
            SQL += ComNum.VBLF + "    AND (DrDept1 = '" + cboDept.Text + "' ";
            SQL += ComNum.VBLF + "         OR DrDept2 = '" + cboDept.Text + "') ";
            SQL += ComNum.VBLF + "    AND DRCODE NOT IN ('0580') ";

            if (cboDept.Text == "HR")
                SQL += ComNum.VBLF + "   OR DRCODE ='7101'";
            else if (cboDept.Text == "TO")
                SQL += ComNum.VBLF + " AND DRCODE IN ('7102') "; // 종검-최덕호,김중구

            SQL += ComNum.VBLF + " Order By DrCode ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                cboDoctor.Items.Clear();


                if (SqlErr != "")
                {
                    ComFunc.MsgBox("의사스케줄 조회오류7");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }



                if (dt.Rows.Count > 0)
                {
                    cboDoctor.Items.Clear();
                    cboDoctor.Items.Add("**");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                            cboDoctor.Items.Add(dt.Rows[i]["DrCode"].ToString().Trim());
                    }
                    cboDoctor.SelectedIndex = 0;

                }
            }
            
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        void Load_DRName()
        {


            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";



            SQL = "";
            SQL += ComNum.VBLF + " SELECT DrName ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND DRCODE = '" + cboDoctor.Text + "' ";
        
        
          
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                txtDrNM.Text = "";


                if (SqlErr != "")
                {
                    ComFunc.MsgBox("의사스케줄 조회오류7");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }



                if (dt.Rows.Count > 0)
                {
                    txtDrNM.Text = dt.Rows[0]["DrName"].ToString().Trim();


                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }
        void Set_Combo()
        {
            int i = 0;

            for (i = 0; i < 50; i++)
            {
                if (clsPmpaPb.GstrSetDeptCodes[i] == "")
                {
                    break;
                }
                cboDept.Items.Add(clsPmpaPb.GstrSetDeptCodes[i]);
            }

            cboDept.Items.Add("****");
            cboDept.SelectedIndex = i;
            txtDept.Text = "전  체";

           


            CS.Spread_All_Clear(ssList);
            ssList_Sheet1.Rows.Count = 0;

            txtPano.Text = "";
            txtSName.Text = "";
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                //                
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }
            else if (sender == this.btnPrint)
            {


                Set_Print();
            }


        }

        void eGetData()
        {
            int i = 0;
            int nRow = 0;

            CS.Spread_All_Clear(ssList);
            ssList_Sheet1.Rows.Count = 0;
            nRow = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                        ";
            SQL += ComNum.VBLF + "  PANO, SNAME, TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') DATE3, DEPTCODE, DRCODE, '' as Tel ,'' ETCJIN                     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW                                                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
            SQL += ComNum.VBLF + "      AND Pano  like '" + txtPano.Text + "%'                                                  ";
           // SQL += ComNum.VBLF + "      AND DATE3 >= TO_DATE('" + CurrentDate + "','YYYY-MM-DD')                                ";
            SQL += ComNum.VBLF + "      AND RETDATE IS NULL                                                                     ";
            if (cboDept.SelectedItem.ToString() != "****")
            {
                SQL += ComNum.VBLF + "  AND DEPTCODE ='" + cboDept.SelectedItem.ToString() + "'                                 ";
            }
            if (cboDoctor.Text != "**" && cboDoctor.Text != "")
            {
                SQL += ComNum.VBLF + "  AND DRCODE ='" + cboDoctor.Text + "'                                 ";
            }
            if (chkNow.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND DATE3 >= TO_DATE('" + CurrentDate + "','YYYY-MM-DD')                                ";
                SQL += ComNum.VBLF + "      AND DATE3 <= TO_DATE('" + CurrentDate + "','YYYY-MM-DD') + 1                        ";
            }
            else
            {

                SQL = SQL + ComNum.VBLF + "         AND DATE3 >=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND DATE3 <=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') + 1 ";
            }


            SQL += ComNum.VBLF + "UNION ALL                                                                                     ";

            SQL += ComNum.VBLF + "SELECT                                                                                        ";
            SQL += ComNum.VBLF + "  PANO, SNAME, TO_CHAR(RDATE,'YYYY-MM-DD') || ' ' || RTime AS DATE3, DEPTCODE, DRCODE, 'Y' AS Tel ,ETCJIN         ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_TELRESV                                                        ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
            SQL += ComNum.VBLF + "      AND Pano  like '" + txtPano.Text + "%'                                                 ";
           // SQL += ComNum.VBLF + "      AND RDATE >= TO_DATE('" + CurrentDate + "','YYYY-MM-DD')                                ";
            if (cboDept.SelectedItem.ToString() != "****")
            {
                SQL += ComNum.VBLF + "  AND DEPTCODE ='" + cboDept.SelectedItem.ToString() + "'                                 ";
            }
            if (cboDoctor.Text != "**" && cboDoctor.Text != "")
            {
                SQL += ComNum.VBLF + "  AND DRCODE ='" + cboDoctor.Text + "'                                 ";
            }
            if (chkNow.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND RDATE = TO_DATE('" + CurrentDate + "','YYYY-MM-DD')                                ";
            }
            else
            {

                SQL = SQL + ComNum.VBLF + "         AND RDATE >=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND RDATE <=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            }
            SQL += ComNum.VBLF + "     order by 3                             ";


            try
            {
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

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow += 1;
                        if (ssList_Sheet1.Rows.Count < nRow)
                        {
                            ssList_Sheet1.Rows.Count = nRow;
                        }
                        ssList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SNAME"].ToString();
                        ssList_Sheet1.Cells[nRow - 1, 2].Text = VB.Pstr(dt.Rows[i]["DATE3"].ToString().Trim(), " ", 1);
                        ssList_Sheet1.Cells[nRow - 1, 3].Text = VB.Pstr(dt.Rows[i]["DATE3"].ToString().Trim(), " ", 2);
                        ssList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 5].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                        ssList_Sheet1.Cells[nRow - 1, 6].Text = ( dt.Rows[i]["TEL"].ToString().Trim() == "Y" ? "전화" : "" ) + " " + ( dt.Rows[i]["ETCJIN"].ToString().Trim() == "1" ? "코로나검사" : "") ;
                                             

                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

      
        void Set_Print()
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string strTitle = "예 약 현 황";
            string strHeader = "";

           
            
           // strTitle = "진료과별 외래환자(전체)";
            

            clsSpread CS = new clsSpread();

            strHeader = CS.setSpdPrint_String(strTitle, new Font("맑은 고딕", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
           // strHeader += CS.setSpdPrint_String("작업일자: " + dtpYear.Value.ToString("yyyy년 MM월"), new Font("맑은 고딕", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, false);
            strHeader += CS.setSpdPrint_String("출력시간: " + Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("yyyy-MM-dd HH:mm") + " 출력자 : " + clsType.User.UserName, new Font("맑은 고딕", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, false);
            CS = null;


            ssList_Sheet1.PrintInfo.Header = strHeader;
            ssList_Sheet1.PrintInfo.Margin.Left = 90;
            ssList_Sheet1.PrintInfo.Margin.Right = 10;
            ssList_Sheet1.PrintInfo.Margin.Top = 50;
            ssList_Sheet1.PrintInfo.Margin.Bottom = 200;
          //  ssList_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Show;
          //  ssList_Sheet1.PrintInfo.ShowColumnHeader = PrintHeader.Show;
            ssList_Sheet1.PrintInfo.ShowBorder = true;
            ssList_Sheet1.PrintInfo.ShowColor = false;
            ssList_Sheet1.PrintInfo.ShowGrid = true;
            ssList_Sheet1.PrintInfo.ShowShadows = false;
            ssList_Sheet1.PrintInfo.UseMax = false;
          //  ssList_Sheet1.PrintInfo.Centering = Centering.Horizontal;
          //  ssList_Sheet1.PrintInfo.Orientation = PrintOrientation.Landscape;
            ssList_Sheet1.PrintInfo.Preview = false;
            ssList_Sheet1.PrintInfo.ZoomFactor = 0.80f;
            ssList.PrintSheet(0);

        }
        void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dt = null;

            if (e.KeyChar == 13)
            {
                btnView.Focus();
            }
            if (e.KeyChar == 13)
            {
                string ret_str = "";
                string strPano = "";

                if (VB.IsNull(txtPano.Text) || txtPano.Text.Trim() == "")
                {
                    return;
                }

                if (CF.READ_BARCODE(txtPano.Text.Trim()) == true)
                {
                    txtPano.Text = mstrBarPano;
                    cboDept.Text = mstrBarDept;
                }
                else
                {
                    strPano = ComFunc.SetAutoZero(txtPano.Text, 8);
                }

                ret_str = CPF.Pano_Check_Digit(clsDB.DbCon, strPano);

                if (!VB.IsNumeric(txtPano.Text))
                {
                    ComFunc.MsgBox("병록번호 ReEnter !!");
                    txtPano.Focus();
                }
                else if (ret_str == "NO")
                {
                    ComFunc.MsgBox("병록번호 ReEnter !!");
                    txtPano.Text = "";
                    txtPano.Focus();
                }

                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

                dt = CPF.Get_BasPatient(clsDB.DbCon, txtPano.Text);

                if (dt.Rows.Count < 0)
                {
                    ComFunc.MsgBox("병록번호 ReEnter !!");
                    txtPano.Text = "";
                    txtPano.Focus();
                }
            }
        }

        void txtPano_Enter(object sender, EventArgs e)
        {
            TextBox tP = sender as TextBox;
            tP.SelectionStart = 0;
            tP.SelectionLength = tP.Text.Length;
        }

        void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                {
                    txtPano.SelectAll();
                }
            }
        }

        void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;

            cboDept.Text = cboDept.SelectedItem.ToString().ToUpper();

            foreach (string s in cboDept.Items)
            {
                if (cboDept.SelectedItem.ToString() == s)
                {
                     txtDept.Text = clsPmpaPb.GstrSetDepts[cboDept.SelectedIndex];
                    Load_DRCode();
                }
            }

            if (cboDept.SelectedItem.ToString() == "****")
            {
                txtDept.Text = "전  체";
            }
        }

        private void cboDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_DRName();
        }

        private void frmPmpaViewResvView_Activated(object sender, EventArgs e)
        {
            txtPano.Focus();
        }
    }
}

