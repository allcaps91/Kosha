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
    /// File Name       : frmPmpaViewName.cs
    /// Description     : 당일 이름별 수진자 조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-05
    /// Update History  : 2017-11-03
    /// <history>       
    /// d:\psmh\OPD\oviewa\OVIEWA08.FRM(FrmNameView) => frmPmpaViewName.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA08.FRM(FrmNameView)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewName : Form
    {
        string[] strDeptCodes = new string[31];
        string[] GstrSETChojaes = new string[9];
        string[] strBis = new string[57];
        string[] strReserved = new string[3];
        string[] GstrSETSin = new string[3];
        string[] GstrSETGameks = new string[100];
        string[] GstrSETJins = new string[7];
        string[] GstrSETSpcs = new string[3];


        private void Bas_Setting()
        {
            GstrSETSin[0] = "구환"; GstrSETSin[1] = "신환";
            strReserved[0] = "당일접수"; strReserved[1] = "예약접수";

            strBis[11] = "공단"; strBis[21] = "보호1"; strBis[31] = "산재";
            strBis[12] = "직장"; strBis[22] = "보호2"; strBis[32] = "공상";
            strBis[13] = "지역"; strBis[23] = "보호3"; strBis[33] = "산재공상";
            strBis[14] = ""; strBis[24] = "행려"; strBis[34] = "";
            strBis[15] = ""; strBis[25] = ""; strBis[35] = "";

            strBis[41] = "공단180"; strBis[51] = "일반";
            strBis[42] = "직장180"; strBis[52] = "TA보험";
            strBis[43] = "지역180"; strBis[53] = "계약";
            strBis[44] = "가족계획"; strBis[54] = "미확인";
            strBis[45] = "보험계약"; strBis[55] = "TA일반";

            GstrSETChojaes[1] = "초진"; GstrSETChojaes[3] = "재진";
            GstrSETChojaes[2] = "초진심야"; GstrSETChojaes[4] = "재진심야";

            GstrSETChojaes[5] = "초진휴일";
            GstrSETChojaes[6] = "재진휴일";


            GstrSETGameks[1] = "재단성직자"; GstrSETGameks[10] = "직원배우자";
            GstrSETGameks[2] = "타재단성직자"; GstrSETGameks[11] = "직원지계,존비속";
            GstrSETGameks[3] = "성직자부모"; GstrSETGameks[12] = "직원 장인,장모";
            GstrSETGameks[4] = "성직자자친형제"; GstrSETGameks[13] = "시용자";
            GstrSETGameks[5] = "성직자친척"; GstrSETGameks[14] = "실습교육생";
            GstrSETGameks[6] = "재단산하기관";
            GstrSETGameks[7] = "직원의친척";
            GstrSETGameks[8] = "승 려";
            GstrSETGameks[9] = "직원본인";

            GstrSETJins[0] = "수납"; GstrSETJins[2] = "접수 II";
            GstrSETJins[1] = "후불"; GstrSETJins[3] = "진단서발급";
            GstrSETJins[4] = "신생아,신검"; GstrSETJins[5] = "전환예약";

            GstrSETSpcs[0] = "비특진";
            GstrSETSpcs[1] = "특진환자";
        }

        void Clear_SS()
        {
            ssList1_Sheet1.Cells[0, 1, ssList1_Sheet1.Rows.Count - 1, 2].Text = "";
        }
        public frmPmpaViewName()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnOK.Click += new EventHandler(eBtnEvent);

            this.txtPart.GotFocus += new EventHandler(eControl_GotFocus);
            this.txtSname.GotFocus += new EventHandler(eControl_GotFocus);


            this.txtPart.LostFocus += new EventHandler(eControl_LostFocus);

            this.txtPart.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtSname.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
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

            txtSname.Select();

            Bas_Setting();
            txtSname.ImeMode = ImeMode.Hangul;
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPart)
            {
                if (e.KeyChar == 13)
                {
                    txtPart.Text = txtPart.Text.ToUpper();
                    SendKeys.Send("{TAB}");
                }
            }

            else if (sender == this.txtSname)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");

                    btnOK_Click();
                }
            }
        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPart)
            {
                TextBox tP = (TextBox)sender;
                tP.SelectionStart = 0;
                tP.SelectionLength = tP.Text.Length;
            }

            else if (sender == this.txtSname)
            {
                TextBox tP = (TextBox)sender;

                tP.SelectionStart = 0;
                tP.SelectionLength = tP.Text.Length;
            }
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            txtPart.Text = txtPart.Text.ToUpper();
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnOK)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                btnOK_Click();
            }
        }

        void btnOK_Click()
        {
            int i = 0;
            int j = 0;

            string strSname = "";
            string strPano = "";
            string strDept = "";
            string strChojae = "";
            string strDrname = "";
            string strAmt7 = "";
            string strAmt77 = "";
            string strField = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            Clear_SS();

            if (txtSname.Text == "" || txtSname.Text == null)
            {
                txtSname.Focus();
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  Sname,Pano,DeptCode,Chojae,DrName,Amt7                                                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER M, " + ComNum.DB_PMPA + "BAS_DOCTOR D               ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND M.DrCode = D.DrCode(+)                                                          ";
            SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')   ";            
            SQL += ComNum.VBLF + "      AND Sname LIKE '" + txtSname.Text + "%'                                             ";
            if (txtPart.Text != "" && txtPart.Text != null)
            {
                SQL += ComNum.VBLF + "  AND Part = '" + txtPart.Text + "'                                                   ";
            }
            SQL += ComNum.VBLF + "ORDER BY Sname                                                                            ";

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
                    txtSname.Focus();
                }

                if (dt.Rows.Count > 0)
                {
                    ssList2_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSname = dt.Rows[i]["Sname"].ToString().Trim();
                        strPano = dt.Rows[i]["Pano"].ToString().Trim();
                        strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                        strChojae = GstrSETChojaes[Convert.ToInt32(dt.Rows[i]["Chojae"].ToString().Trim())];
                        strDrname = VB.Mid(dt.Rows[i]["DrName"].ToString().Trim(), 1, 8);
                        strAmt7 = String.Format("{0:#,###,###,###0}", VB.Val(dt.Rows[i]["Amt7"].ToString().Trim()));

                        //strAmt77 = VB.Space(5 - VB.Len(strAmt7)) + strAmt7 + " ";
                        //strField = strSname + strPano + strDept + strChojae + strDrname + strAmt77;

                        ssList2_Sheet1.Cells[i, 0].Text = strSname;
                        ssList2_Sheet1.Cells[i, 1].Text = strPano;
                        ssList2_Sheet1.Cells[i, 2].Text = strDept;
                        ssList2_Sheet1.Cells[i, 3].Text = strChojae;
                        ssList2_Sheet1.Cells[i, 4].Text = strDrname;
                        ssList2_Sheet1.Cells[i, 5].Text = strAmt7;
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

        void ssList2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPano = "";
            string strDr = "";
            string strDept = "";

            //if(txtSname.Text == "")
            //{
            //    return;
            //}

            strPano = ssList2_Sheet1.Cells[e.Row, 1].Text;
            strDept = ssList2_Sheet1.Cells[e.Row, 2].Text;
            strDr = ssList2_Sheet1.Cells[e.Row, 4].Text;

            Clear_SS();
            Read_OM(strPano, strDept, strDr);
        }

        void Read_OM(string strPano, string strDept, string strDr)
        {
            string strRep = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                            ";
            SQL += ComNum.VBLF + "  Sname,M.DeptCode,DeptNameK,Sex,Age,M.DrCode,Bi, Chojae,                                                         ";
            SQL += ComNum.VBLF + "  M.JiCode,JiName,Reserved,GbGamek,GbSpc,Jin,Singu,Rep,                                                           ";
            SQL += ComNum.VBLF + "  TO_CHAR(Jtime,'yy-mm-dd hh24:mi') Jtime,                                                                        ";
            SQL += ComNum.VBLF + "  TO_CHAR(Stime,'yy-mm-dd hh24:mi') Stime, Amt7                                                                   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER M, " + ComNum.DB_PMPA + "BAS_AREA A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                         ";
            SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')                           ";            
            SQL += ComNum.VBLF + "      AND Pano = '" + strPano + "'                                                                                ";
            SQL += ComNum.VBLF + "      AND M.DeptCode = '" + strDept + "'                                                                          ";
            SQL += ComNum.VBLF + "      AND M.DeptCode = C.DeptCode(+)                                                                              ";
            SQL += ComNum.VBLF + "      AND M.JiCode = A.JiCode(+)                                                                                  ";

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
                    ssList1_Sheet1.Cells[0, 1].Text = dt.Rows[0]["Sname"].ToString().Trim();
                    ssList1_Sheet1.Cells[1, 1].Text = strPano;
                    ssList1_Sheet1.Cells[2, 1].Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                    ssList1_Sheet1.Cells[3, 1].Text = dt.Rows[0]["DrCode"].ToString().Trim();
                    ssList1_Sheet1.Cells[4, 1].Text = dt.Rows[0]["Bi"].ToString().Trim();
                    ssList1_Sheet1.Cells[5, 1].Text = dt.Rows[0]["JiCode"].ToString().Trim();
                    ssList1_Sheet1.Cells[6, 1].Text = dt.Rows[0]["Reserved"].ToString().Trim();
                    ssList1_Sheet1.Cells[7, 1].Text = dt.Rows[0]["Chojae"].ToString().Trim();
                    ssList1_Sheet1.Cells[8, 1].Text = dt.Rows[0]["GbGamek"].ToString().Trim();
                    ssList1_Sheet1.Cells[9, 1].Text = dt.Rows[0]["GbSpc"].ToString().Trim();
                    ssList1_Sheet1.Cells[10, 1].Text = dt.Rows[0]["Jin"].ToString().Trim();
                    ssList1_Sheet1.Cells[11, 1].Text = dt.Rows[0]["Singu"].ToString().Trim();
                    ssList1_Sheet1.Cells[12, 1].Text = dt.Rows[0]["Rep"].ToString().Trim();
                    strRep = ssList1_Sheet1.Cells[12, 1].Text;
                    ssList1_Sheet1.Cells[13, 1].Text = VB.Mid(dt.Rows[0]["JTime"].ToString().Trim(), 1, 8);
                    ssList1_Sheet1.Cells[14, 1].Text = VB.Mid(dt.Rows[0]["STime"].ToString().Trim(), 1, 8);
                    ssList1_Sheet1.Cells[15, 1].Text = String.Format("{0:#,###}", VB.Val(dt.Rows[0]["Amt7"].ToString().Trim()));

                    ssList1_Sheet1.Cells[0, 2].Text = dt.Rows[0]["Sex"].ToString().Trim() + " / " + dt.Rows[0]["Age"].ToString().Trim();
                    ssList1_Sheet1.Cells[2, 2].Text = dt.Rows[0]["DeptNamek"].ToString().Trim();
                    ssList1_Sheet1.Cells[3, 2].Text = strDr;
                    ssList1_Sheet1.Cells[4, 2].Text = strBis[Convert.ToInt32(VB.Val(dt.Rows[0]["Bi"].ToString().Trim()))];
                    ssList1_Sheet1.Cells[5, 2].Text = dt.Rows[0]["JiName"].ToString().Trim();
                    ssList1_Sheet1.Cells[6, 2].Text = strReserved[Convert.ToInt32(VB.Val(dt.Rows[0]["Reserved"].ToString().Trim()))];

                    ssList1_Sheet1.Cells[7, 2].Text = GstrSETChojaes[Convert.ToInt32(VB.Val(dt.Rows[0]["Chojae"].ToString().Trim()))];
                    ssList1_Sheet1.Cells[8, 2].Text = GstrSETGameks[Convert.ToInt32(VB.Val(dt.Rows[0]["GbGamek"].ToString().Trim()))];
                    ssList1_Sheet1.Cells[9, 2].Text = GstrSETSpcs[Convert.ToInt32(VB.Val(dt.Rows[0]["GbSpc"].ToString().Trim()))];
                    ssList1_Sheet1.Cells[10, 2].Text = GstrSETJins[Convert.ToInt32(VB.Val(dt.Rows[0]["Jin"].ToString().Trim()))];
                    ssList1_Sheet1.Cells[11, 2].Text = GstrSETSin[Convert.ToInt32(VB.Val(dt.Rows[0]["Singu"].ToString().Trim()))];
                    ssList1_Sheet1.Cells[13, 2].Text = VB.Mid(dt.Rows[0]["JTime"].ToString().Trim(), 10, 14);
                    ssList1_Sheet1.Cells[14, 2].Text = VB.Mid(dt.Rows[0]["STime"].ToString().Trim(), 10, 14);

                    if (strRep == "+")
                    {
                        ssList1_Sheet1.Cells[12, 2].Text = "발행";
                    }
                    else if (strRep == "-")
                    {
                        ssList1_Sheet1.Cells[12, 2].Text = "환불";
                    }
                    else
                    {
                        ssList1_Sheet1.Cells[12, 2].Text = "미발행";
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

        void btnOK_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnOK.Focus();
            }
        }

        void txtPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSname.Focus();
            }
        }

        void txtSname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnOK.Focus();
            }
        }
    }
}
