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

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpViewResvView.cs
    /// Description     : 예약 환자 조회
    /// Author          : 안정수
    /// Create Date     : 2017-08-24
    /// Update History  : 
    /// <history>       
    /// 실제 테스트 필요
    /// d:\psmh\OPD\oumsad\OUMSAD22.FRM(FrmResvView) => frmPmpViewResvView.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\OUMSAD22.FRM(FrmResvView)
    /// </seealso>
    /// </summary>
    public partial class frmPmpViewResvView : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        clsPmpaFunc CPF = new clsPmpaFunc();
        clsOumsad CO = new clsOumsad();
        string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

        string mstrBarPano = "";
        string mstrBarDept = "";
        public frmPmpViewResvView()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpViewResvView(string GstrBarPano, string GstrBarDept)
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

        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            Load_DeptCode();

            Set_Combo();

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
            SQL += ComNum.VBLF + "  TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') DATE3, DEPTCODE, DRCODE, '' as Tel                      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW                                                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
            SQL += ComNum.VBLF + "      AND Pano ='" + txtPano.Text + "'                                                        ";
            SQL += ComNum.VBLF + "      AND DATE3 >= TO_DATE('" + CurrentDate + "','YYYY-MM-DD')                                ";
            SQL += ComNum.VBLF + "      AND RETDATE IS NULL                                                                     ";
            if (cboDept.SelectedItem.ToString() != "****")
            {
                SQL += ComNum.VBLF + "  AND DEPTCODE ='" + cboDept.SelectedItem.ToString() + "'                                 ";
            }

            SQL += ComNum.VBLF + "UNION ALL                                                                                     ";

            SQL += ComNum.VBLF + "SELECT                                                                                        ";
            SQL += ComNum.VBLF + "  TO_CHAR(RDATE,'YYYY-MM-DD') || ' ' || RTime AS DATE3, DEPTCODE, DRCODE, 'Y' AS Tel          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_TELRESV                                                        ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
            SQL += ComNum.VBLF + "      AND Pano ='" + txtPano.Text + "'                                                        ";
            SQL += ComNum.VBLF + "      AND RDATE >= TO_DATE('" + CurrentDate + "','YYYY-MM-DD')                                ";
            if (cboDept.SelectedItem.ToString() != "****")
            {
                SQL += ComNum.VBLF + "  AND DEPTCODE ='" + cboDept.SelectedItem.ToString() + "'                                 ";
            }

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

                        ssList_Sheet1.Cells[nRow - 1, 0].Text = VB.Pstr(dt.Rows[i]["DATE3"].ToString().Trim(), " ", 1);
                        ssList_Sheet1.Cells[nRow - 1, 1].Text = VB.Pstr(dt.Rows[i]["DATE3"].ToString().Trim(), " ", 2);
                        ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 3].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                        ssList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["TEL"].ToString().Trim() == "Y" ? "전화" : "";
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

        void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
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

                //
                if (CO.READ_BAS_PATIENT(clsDB.DbCon, txtPano.Text) == "NO")
                {
                    ComFunc.MsgBox("병록번호 ReEnter !!");
                    txtPano.Text = "";
                    txtPano.Focus();
                }
                txtSName.Text = CF.Read_Patient(clsDB.DbCon, txtPano.Text, "2");

                btnView.Focus();
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
                }
            }

            if (cboDept.SelectedItem.ToString() == "****")
            {
                txtDept.Text = "전  체";
            }
        }
    }
}

