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
    /// File Name       : frmPmpaViewPersonJub.cs
    /// Description     : 개인별 접수 내역 조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-26
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\oviewa\OVIEWA03.FRM(FrmPersonJub) => frmPmpaViewPersonJub.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA03.FRM(FrmPersonJub)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewPersonJub : Form
    {
        ComFunc CF = new ComFunc();

        string strDate = "";
        string strDate1 = "";
        string strPanoNumber = "";
        string strDeptCode = "";

        string mstrPANO = "";
        string mstrJobPart = "";

        int nSelect = 0;
        int nRowindi = 0;

        public frmPmpaViewPersonJub()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewPersonJub(string GstrPANO, string GstrJobPart)
        {
            InitializeComponent();
            mstrPANO = GstrPANO;
            mstrJobPart = GstrJobPart;
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnNext.Click += new EventHandler(eBtnEvent);
            this.btnBack.Click += new EventHandler(eBtnEvent);
            this.btnClose.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);

            this.btnView.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtPart.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtNamePano.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.optSelect1.CheckedChanged += new EventHandler(eOpt_ClickEvent);
            this.optSelect2.CheckedChanged += new EventHandler(eOpt_ClickEvent);
            this.optSelect3.CheckedChanged += new EventHandler(eOpt_ClickEvent);

            this.dtpDate.GotFocus += new EventHandler(eControl_GotFocus);
            this.txtPart.GotFocus += new EventHandler(eControl_GotFocus);
            this.txtNamePano.GotFocus += new EventHandler(eControl_GotFocus);


            this.txtPart.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtNamePano.LostFocus += new EventHandler(eControl_LostFocus);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등     

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;


            optSelect1.Checked = true;

            ssList1.Dock = DockStyle.Fill;
            PanSS.Visible = false;
            txtNamePano.Enabled = true;
            txtPart.Enabled = false;
            dtpDate.Enabled = false;
            ssList1.Enabled = false;
            btnView.Enabled = false;
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPart)
            {
                txtPart.Text = txtPart.Text.ToUpper();
            }

            else if (sender == this.txtNamePano)
            {
                if (optSelect1.Checked == true)
                {
                    txtNamePano.Text = ComFunc.SetAutoZero(txtNamePano.Text, 8);
                }
                else
                {
                    txtNamePano.ImeMode = ImeMode.Hangul;
                }
            }
        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            if (sender == dtpDate)
            {
                btnView.Enabled = true;
                strDate1 = "";
            }

            else if (sender == txtPart)
            {
                txtPart.SelectionStart = 0;
                txtPart.SelectionLength = (txtPart.Text).Length;
                dtpDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }

            else if (sender == txtNamePano)
            {
                if (optSelect2.Checked == true)
                {
                    txtNamePano.ImeMode = ImeMode.Hangul;
                }

                txtNamePano.SelectionStart = 0;
                txtNamePano.SelectionLength = (txtNamePano.Text).Length;
                btnView.Enabled = true;
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == btnView)
            {
                if (e.KeyChar == 13)
                {
                    if (optSelect1.Checked == true)
                    {
                        if (txtNamePano.Text == "" || VB.IsNull(txtNamePano.Text))
                        {
                            ComFunc.MsgBox("병록번호가 비어잇습니다.");
                        }
                    }

                    else if (optSelect2.Checked == true)
                    {
                        if (txtNamePano.Text == "" || VB.IsNull(txtNamePano.Text))
                        {
                            ComFunc.MsgBox("수진자명이 비어잇습니다.");
                        }
                    }

                    else if (optSelect3.Checked == true)
                    {
                        if (txtPart.Text == "" || VB.IsNull(txtPart.Text))
                        {
                            ComFunc.MsgBox("조가 비어잇습니다.");
                        }

                        strDate = dtpDate.Text;
                    }


                    //등록번호 자동입력을 위한 전역 변수 2012-04-03 이주형
                    mstrPANO = txtNamePano.Text;

                    ListPersonJubBuild();

                    if (optSelect3.Checked == true)
                    {
                        dtpDate.Focus();
                    }

                    else if (optSelect1.Checked == true || optSelect2.Checked == true)
                    {
                        txtNamePano.Focus();
                    }
                }
            }

            else if (sender == this.txtPart)
            {
                if (e.KeyChar == 13)
                {
                    txtPart.Text = txtPart.Text.ToUpper();
                    dtpDate.Focus();

                    eGetData();
                }
            }

            else if (sender == this.txtNamePano)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                    txtNamePano.Text = ComFunc.SetAutoZero(txtNamePano.Text, 8);
                    eGetData();
                }
            }

        }

        void eOpt_ClickEvent(object sender, EventArgs e)
        {
            if (sender == this.optSelect1)
            {
                ssList1_Sheet1.Rows.Count = 0;

                //등록번호 자동입력 2012-04-03 안정수
                txtNamePano.Text = mstrPANO;
                txtPart.Text = "";
                btnView.Enabled = true;
                txtNamePano.Enabled = true;
                dtpDate.Enabled = false;
                txtPart.Enabled = false;
                lblNamePano.Text = "병록번호";
                txtNamePano.Focus();
            }

            else if (sender == this.optSelect2)
            {
                ssList1_Sheet1.Rows.Count = 0;

                txtNamePano.Text = "";
                txtPart.Text = "";
                btnView.Enabled = true;
                txtNamePano.Enabled = true;
                dtpDate.Enabled = false;
                txtPart.Enabled = false;
                lblNamePano.Text = "수진자명";
                txtNamePano.Focus();
            }

            else if (sender == this.optSelect3)
            {
                ssList1_Sheet1.Rows.Count = 0;
                strDate1 = "";
                txtNamePano.Text = "";
                txtPart.Text = "";
                btnView.Enabled = true;
                txtNamePano.Enabled = false;
                dtpDate.Enabled = true;
                txtPart.Enabled = true;
                txtPart.Text = mstrJobPart;
                lblNamePano.Text = "";
                txtPart.Focus();
            }

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnNext)
            {
                btnNext_Click();
            }

            else if (sender == this.btnBack)
            {
                btnBack_Click();
            }

            else if (sender == this.btnClose)
            {
                btnClose_Click();
            }

            else if (sender == this.btnCancel)
            {
                ComFunc.SetAllControlClear(panel1);
            }
        }

        void btnBack_Click()
        {
            int a = ssList1_Sheet1.ActiveRowIndex;

            if (a < 0 || a != 0)
            {
                strDate = ssList1_Sheet1.Cells[a - 1, 5].Text;
                strDate1 = ssList1_Sheet1.Cells[a - 1, 7].Text;
                strPanoNumber = ssList1_Sheet1.Cells[a - 1, 0].Text;
                strDeptCode = ssList1_Sheet1.Cells[a - 1, 2].Text;
            }
            else if (a == 0)
            {
                strDate = ssList1_Sheet1.Cells[a, 5].Text;
                strDate1 = ssList1_Sheet1.Cells[a, 7].Text;
                strPanoNumber = ssList1_Sheet1.Cells[a, 0].Text;
                strDeptCode = ssList1_Sheet1.Cells[a, 2].Text;
            }

            PersonJubBuild();
        }

        void btnClose_Click()
        {
            PanSS.Visible = false;
            ssList1.Enabled = true;

            if (optSelect1.Checked == true || optSelect2.Checked == true)
            {
                txtNamePano.Enabled = true;
            }

            else
            {
                txtPart.Enabled = true;
                dtpDate.Enabled = true;
            }

       
            btnView.Enabled = true;

            //List로 구현된것을 -> 스프레드로 구현했으므로 주석처리
            //ListPersonJub.ListIndex = nSelect
            //ListPersonJub.SetFocus
        }

        void btnNext_Click()
        {
            int a = ssList1_Sheet1.ActiveRowIndex;

            strDate = ssList1_Sheet1.Cells[a + 1, 5].Text;
            strDate1 = ssList1_Sheet1.Cells[a + 1, 7].Text;
            strPanoNumber = ssList1_Sheet1.Cells[a + 1, 0].Text;
            strDeptCode = ssList1_Sheet1.Cells[a + 1, 2].Text;

            PersonJubBuild();
        }

        void eGetData()
        {
            if (optSelect1.Checked == true)
            {
                if (txtNamePano.Text == "" || VB.IsNull(txtNamePano.Text))
                {
                    ComFunc.MsgBox("병록번호가 비어있습니다.");
                }
            }

            else if (optSelect2.Checked == true)
            {
                if (txtNamePano.Text == "" || VB.IsNull(txtNamePano.Text))
                {
                    ComFunc.MsgBox("수진자명이 비어있습니다.");
                }
            }

            else if (optSelect3.Checked == true)
            {
                if (txtPart.Text == "" || VB.IsNull(txtPart.Text))
                {
                    ComFunc.MsgBox("조가 비어있습니다.");
                }

                strDate = dtpDate.Text;
            }


            //등록번호 자동입력을 위한 전역 변수 2012-04-03 이주형
            mstrPANO = txtNamePano.Text;

            ListPersonJubBuild();

            if (optSelect3.Checked == true)
            {
                dtpDate.Focus();
            }

            else if (optSelect1.Checked == true || optSelect2.Checked == true)
            {
                txtNamePano.Focus();
            }
        }

        void ssList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            strDate = ssList1_Sheet1.Cells[e.Row, 5].Text;
            strDate1 = ssList1_Sheet1.Cells[e.Row, 7].Text;
            strPanoNumber = ssList1_Sheet1.Cells[e.Row, 0].Text;
            strDeptCode = ssList1_Sheet1.Cells[e.Row, 2].Text;

            PersonJubBuild();

            PanSS.Dock = DockStyle.Fill;
            PanSS.Visible = true;
        }

        void PersonJubBuild()
        {
            int i = 0;
            int j = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  TO_CHAR(ActDate,'YYYY-MM-DD') AcDate, Pano, O.DeptCode, Bi,     ";
            SQL += ComNum.VBLF + "  Sname, Sex, Age, JiCode, D.DrCode, Reserved, ChoJae, GbGameK,   ";
            SQL += ComNum.VBLF + "  GbSpc, o.Jin, SinGu, Bohun, Rep, Part, DeptNameK, DrName,       ";
            SQL += ComNum.VBLF + "  TO_CHAR(JTime,'HH24:MI:SS') JinDate,                            ";
            SQL += ComNum.VBLF + "  TO_CHAR(Stime,'HH24:MI:SS') SuDate,                             ";
            SQL += ComNum.VBLF + "  Amt3, Amt4, Amt5, Amt6, Amt7                                    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER      O,                     ";
            SQL += ComNum.VBLF + ComNum.DB_PMPA + "Bas_CliniCdept  C,                     ";
            SQL += ComNum.VBLF + ComNum.DB_PMPA + "Bas_Doctor      D                      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
            SQL += ComNum.VBLF + "      AND TO_CHAR(JTime,'YYYY-MM-DD') = '" + strDate + "'         ";
            SQL += ComNum.VBLF + "      AND TO_CHAR(BDATE,'YYYY-MM-DD') = '" + strDate1 + "'        ";
            SQL += ComNum.VBLF + "      AND Pano = '" + strPanoNumber + "'                          ";
            SQL += ComNum.VBLF + "      AND O.DeptCode = '" + strDeptCode.Trim() + "'               ";
            SQL += ComNum.VBLF + "      AND O.DEPTCODE = C.DEPTCODE                                 ";
            SQL += ComNum.VBLF + "      AND O.DRCODE   = D.DRCODE                                   ";

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
                    PersonJubClear();

                    i = 0;

                    txt0.Text = dt.Rows[0]["AcDate"].ToString().Trim();
                    txt1.Text = dt.Rows[0]["Pano"].ToString().Trim();
                    txt2.Text = dt.Rows[0]["DeptNameK"].ToString().Trim();

                    switch (dt.Rows[0]["Bi"].ToString().Trim())
                    {
                        case "11":
                            txt3.Text = "공   단";
                            break;
                        case "12":
                            txt3.Text = "직   장";
                            break;
                        case "13":
                            txt3.Text = "지   역";
                            break;
                        case "21":
                            txt3.Text = "보호1종";
                            break;
                        case "22":
                            txt3.Text = "보호2종";
                            break;
                        case "23":
                            txt3.Text = "보호3종";
                            break;
                        case "24":
                            txt3.Text = "행   려";
                            break;
                        case "31":
                            txt3.Text = "산   재";
                            break;
                        case "32":
                            txt3.Text = "공   상";
                            break;
                        case "33":
                            txt3.Text = "산재공상";
                            break;
                        case "41":
                            txt3.Text = "공단180";
                            break;
                        case "42":
                            txt3.Text = "직장180";
                            break;
                        case "43":
                            txt3.Text = "지역180";
                            break;
                        case "44":
                            txt3.Text = "가계부 ";
                            break;
                        case "45":
                            txt3.Text = "보험계약";
                            break;
                        case "51":
                            txt3.Text = "일   반";
                            break;
                        case "52":
                            txt3.Text = "TA 보험";
                            break;
                        case "53":
                            txt3.Text = "계약처 ";
                            break;
                        case "54":
                            txt3.Text = "미확인 ";
                            break;
                        case "55":
                            txt3.Text = "TA 일반";
                            break;
                    }

                    txt4.Text = dt.Rows[0]["Sname"].ToString().Trim();

                    if (dt.Rows[0]["Sex"].ToString().Trim() == "F")
                    {
                        txt5.Text = "여자";
                    }
                    else if (dt.Rows[0]["Sex"].ToString().Trim() == "M")
                    {
                        txt5.Text = "남자";
                    }

                    txt6.Text = dt.Rows[0]["Age"].ToString().Trim();
                    txt7.Text = dt.Rows[0]["JiCode"].ToString().Trim();

                    txt8.Text = dt.Rows[0]["DrName"].ToString().Trim();

                    if (dt.Rows[0]["Reserved"].ToString().Trim() == "1")
                    {
                        txt9.Text = "예약접수";
                    }
                    else
                    {
                        txt9.Text = "당일접수";
                    }

                    switch (dt.Rows[0]["ChoJae"].ToString().Trim())
                    {
                        case "1":
                            txt10.Text = "초진";
                            break;
                        case "2":
                            txt10.Text = "초진야간";
                            break;
                        case "3":
                            txt10.Text = "재진";
                            break;
                        case "4":
                            txt10.Text = "재진야간";
                            break;
                    }

                    txt11.Text = dt.Rows[0]["GbGameK"].ToString().Trim();

                    if (dt.Rows[0]["GbSpc"].ToString().Trim() == "1")
                    {
                        txt12.Text = "특진";
                    }
                    else if ((dt.Rows[0]["GbSpc"].ToString().Trim() == "0"))
                    {
                        txt12.Text = "비특진";
                    }

                    txt13.Text = dt.Rows[0]["Jin"].ToString().Trim();

                    if (dt.Rows[0]["SinGu"].ToString().Trim() == "1")
                    {
                        txt14.Text = "신환";
                    }
                    else if (dt.Rows[0]["SinGu"].ToString().Trim() == "0")
                    {
                        txt14.Text = "구환";
                    }

                    txt15.Text = dt.Rows[0]["Bohun"].ToString().Trim();
                    txt16.Text = dt.Rows[0]["Rep"].ToString().Trim();
                    txt17.Text = dt.Rows[0]["Part"].ToString().Trim();
                    txt18.Text = dt.Rows[0]["JinDate"].ToString().Trim();
                    txt19.Text = dt.Rows[0]["SuDate"].ToString().Trim();

                    txt20.Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[0]["Amt3"].ToString().Trim()));
                    txt21.Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[0]["Amt4"].ToString().Trim()));
                    txt22.Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[0]["Amt5"].ToString().Trim()));
                    txt23.Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[0]["Amt6"].ToString().Trim()));
                    txt24.Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[0]["Amt7"].ToString().Trim()));
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            ssList1.Enabled = false;
            txtNamePano.Enabled = false;
            txtPart.Enabled = false;
            dtpDate.Enabled = false;
            optSelect1.Enabled = false;
            optSelect2.Enabled = false;
            optSelect3.Enabled = false;
            btnView.Enabled = false;
        }

        void PersonJubClear()
        {
            ComFunc.SetAllControlClear(PanSS);
        }

        void ListPersonJubBuild()
        {
            int i = 0;
            int j = 0;
            string strPano = "";
            string strSname = "";
            string strDept = "";
            string strSex = "";
            string strAge = "";
            string strDate2 = "";
            string strTime = "";
            string strDate3 = "";
            string strPart = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList1.Enabled = true;
            ssList1_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  Pano, SName, DeptCode, Sex, Age, TO_CHAR(Jtime,'YYYY-MM-DD') JinDate,       ";
            SQL += ComNum.VBLF + "  TO_CHAR(Jtime,'HH24:MI') JinTime, Part, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            if (optSelect1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND Pano = '" + txtNamePano.Text + "'                                   ";
            }
            else if (optSelect2.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND Sname = '" + txtNamePano.Text + "'                                  ";
            }
            else if (optSelect3.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND ActDate = TO_DATE('" + strDate + "','YYYY-MM-DD')                   ";
                if (txtPart.Text.Trim() != "" && !VB.IsNull(txtPart.Text.Trim()))
                {
                    SQL += ComNum.VBLF + "AND Part = '" + txtPart.Text + "'                                     ";
                }
            }
            if (optSelect3.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY Deptcode, JTime Desc, Pano                                       ";
            }
            else
            {
                SQL += ComNum.VBLF + "ORDER BY JTime Desc, Pano                                                 ";
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
                    ssList1_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strPano = dt.Rows[i]["Pano"].ToString().Trim();
                        strSname = dt.Rows[i]["Sname"].ToString().Trim();
                        strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                        strSex = dt.Rows[i]["Sex"].ToString().Trim();
                        strAge = dt.Rows[i]["Age"].ToString().Trim();
                        strDate2 = dt.Rows[i]["JinDate"].ToString().Trim();
                        strDate3 = dt.Rows[i]["BDate"].ToString().Trim();
                        strTime = dt.Rows[i]["JinTime"].ToString().Trim();
                        strPart = dt.Rows[i]["Part"].ToString().Trim();

                        ssList1_Sheet1.Cells[i, 0].Text = strPano;
                        ssList1_Sheet1.Cells[i, 1].Text = strSname;
                        ssList1_Sheet1.Cells[i, 2].Text = strDept;
                        ssList1_Sheet1.Cells[i, 3].Text = strSex;
                        ssList1_Sheet1.Cells[i, 4].Text = strAge;
                        ssList1_Sheet1.Cells[i, 5].Text = strDate2;
                        ssList1_Sheet1.Cells[i, 6].Text = strTime;
                        ssList1_Sheet1.Cells[i, 7].Text = strDate3;
                        ssList1_Sheet1.Cells[i, 8].Text = strPart;
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
    }
}
