using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmSlipView4.cs
    /// Description     : 수가코드별 처방내역 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-15
    /// Update History  : 
    /// <history>       
    /// D:\타병원\PSMHH\mir\miretc\miretc24.frm(FrmSilpView) => frmSlipView4.cs 으로 변경함
    /// 실데이터로 테스트 필요
    /// GstrHelpCode를 받아오는 생성자 추가
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\mir\miretc\miretc24.frm(FrmSilpView)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\mir\miretc\miretc.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmSlipView4 : Form
    {
        ComFunc CF = new ComFunc();
        string mstrHelpCode = "";
        public frmSlipView4()
        {
            InitializeComponent();
            setParam();
        }
        private void setParam()
        {
            this.ssSuga.Change += new ChangeEventHandler(Spread_Change);
        }
        private void Spread_Change(object sender, ChangeEventArgs e)
        {
            int Col = 0;
            int Row = 0;

            if (sender == this.ssSuga)
            {
                string strCode = "";
                string SQL = "";
                string SqlErr = ""; //에러문 받는 변수
                DataTable dt = null;

                if (e.Column != 1 )
                {
                    return;
                }
                strCode = ssSuga_Sheet1.Cells[e.Row, e.Column].Text;

                if (strCode == "")
                {
                    return;
                }

                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  SuNameK";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN";
                SQL += ComNum.VBLF + "  WHERE SuNext = '" + strCode.ToUpper() + "' ";
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

                ssSuga_Sheet1.Cells[e.Row, 2].Text = dt.Rows[0]["SuNameK"].ToString().Trim();
                dt.Dispose();
                dt = null;
            }
        }


        public frmSlipView4(string strHelpCode)
        {
            InitializeComponent();
            mstrHelpCode = strHelpCode;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmSlipView4_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            lblDaiCode.Text = "";

            chkQty.Checked = true;

            optIpd.Checked = true;
            optAct.Checked = true;
            optBun1.Checked = true;
            optF1.Checked = true;
            optBi1.Checked = true;

            SetCombo();
        }

        string READ_DaicodeName(string ArgCode)
        {
            string ArgReturn = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            if(Convert.ToInt16(ArgCode) == 0)
            {
                return ArgReturn;
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  ClassName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLASS";
            SQL += ComNum.VBLF + "WHERE ClassCode=" + Convert.ToInt16(ArgCode) + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }

            if (dt.Rows.Count > 0)
            {
                ArgReturn = dt.Rows[0]["ClassName"].ToString().Trim();
            }
            else
            {
                ArgReturn = "** ERROR **";
            }

            dt.Dispose();
            dt = null;

            return ArgReturn;
        }

        void SCREEN_CLEAR()
        {
            for(int i = 0; i < ssAct_Sheet1.RowCount; i++)
            {
                for(int j = 0; j < ssAct_Sheet1.ColumnCount; j++)
                {
                    ssAct_Sheet1.Cells[i, j].Text = "";
                }
            }
        }

        void SetCombo()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            //TxtFdate.Text = DATE_ADD(GstrSysDate, -5)
            //TxtTdate.Text = DATE_ADD(GstrSysDate, -1)

            btnNext.Enabled = false;
            btnPrint.Enabled = false;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  DEPTCODE,DEPTNAMEK";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
            SQL += ComNum.VBLF + "ORDER BY PRINTRANKING";
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

            cboDept.Items.Clear();
            cboDept.Items.Add("**.전체과");
            for(i = 0; i < dt.Rows.Count; i++)
            {
                cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            cboDept.SelectedIndex = 0;

            cboDrCode.Items.Clear();
            cboDrCode.Items.Add("****.전체의사");
            cboDrCode.SelectedIndex = 0;

            txtPano.Text = "";
            txtSname.Text = "";
        }

        void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            string strDept = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            strDept = VB.Left(cboDept.SelectedItem.ToString(), 2);

            cboDrCode.Items.Clear();

            if(strDept == "")
            {
                return;
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  DRCODE, DRNAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
            SQL += ComNum.VBLF + "WHERE TOUR = 'N'";
            if(strDept != "**")
            {
                SQL += ComNum.VBLF + "AND DRDEPT1  = '" + strDept + "' ";
            }
            SQL += ComNum.VBLF + "ORDER BY PRINTRANKING";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                cboDrCode.Items.Add("****.전체");
            }
            else
            {
                ComFunc.MsgBox("진료과장이 없습니다. ");
            }

            for(i = 0; i < dt.Rows.Count; i++)
            {
                cboDrCode.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
            }
            dt.Dispose();
            dt = null;

            cboDrCode.SelectedIndex = 0;

        }

        void btnNext_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();

            btnNext.Enabled = false;
            btnPrint.Enabled = false;
            btnView.Enabled = true;
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strHead = "";
            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";

            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs0";
            strFont2 = "/fn\"바탕체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs0";

            strHead = "수가코드별 처방 내역";

            strHead1 = "/f1" + VB.Space(25) + strHead;
            strHead2 = "/l/f2" + "작업기간 : " + dtpFDate.Text + " ==> " + dtpTDate.Text + "/n";
            strHead2 = strHead2 + "/l/f2" + "인쇄일자 : " + DateTime.Now.ToString("yyyy-MM-dd");

            ssAct_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;

            ssAct_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;

            ssAct_Sheet1.PrintInfo.Margin.Top = 50;
            ssAct_Sheet1.PrintInfo.Margin.Bottom = 2000;
            ssAct_Sheet1.PrintInfo.Margin.Left = 0;
            ssAct_Sheet1.PrintInfo.Margin.Right = 0;

            ssAct_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssAct_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;

            ssAct_Sheet1.PrintInfo.ShowBorder = true;
            ssAct_Sheet1.PrintInfo.ShowColor = false;
            ssAct_Sheet1.PrintInfo.ShowGrid = true;
            ssAct_Sheet1.PrintInfo.ShowShadows = false;
            ssAct_Sheet1.PrintInfo.UseMax = false;
            ssAct_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssAct_Sheet1.PrintInfo.Preview = true;
            ssAct.PrintSheet(0);
        }

        void optAct_Click(object sender, EventArgs e)
        {
            if (optAct.Checked ==  true)
            {
                ssAct_Sheet1.ColumnHeader.Cells[0, 1].Value = "발생일자";
                ssAct_Sheet1.ColumnHeader.Cells[0, 2].Value = "등록번호";
                ssAct_Sheet1.ColumnHeader.Cells[0, 3].Value = "성  명";
            }
            else
            {
                ssAct_Sheet1.ColumnHeader.Cells[0, 1].Value = "등록번호";
                ssAct_Sheet1.ColumnHeader.Cells[0, 2].Value = "성  명";
                ssAct_Sheet1.ColumnHeader.Cells[0, 3].Value = "발생일자";
            }
        }

        void ssSuga_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strCode = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if(e.Column != 2)
            {
                return;
            }
            strCode = ssSuga_Sheet1.Cells[e.Row, e.Column].Text;

            if (strCode == "")
            {
                return;
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  SuNameK";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN";
            SQL += ComNum.VBLF + "  WHERE SuNext = '" + strCode + "' ";
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

            ssSuga_Sheet1.Cells[e.Row, 2].Text = dt.Rows[0]["SuNameK"].ToString().Trim();
            dt.Dispose();
            dt = null;
        }

        void txtDaiCode_DoubleClick(object sender, EventArgs e)
        {
            mstrHelpCode = "";

            frmYAKHelp frm = new frmYAKHelp();
            frm.Show();

            if(mstrHelpCode != "")
            {
                txtDaiCode.Text = mstrHelpCode;
                lblDaiCode.Text = READ_DaicodeName(txtDaiCode.Text);
                mstrHelpCode = "";
                SendKeys.Send("{TAB}");
            }
        }

        void txtDaiCode_Leave(object sender, EventArgs e)
        {
            lblDaiCode.Text = READ_DaicodeName(txtDaiCode.Text);
        }

        void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSname.Text = CF.Read_Patient(clsDB.DbCon, ComFunc.SetAutoZero(txtPano.Text, 8), "2");

                if (txtSname.Text == "")
                {
                    ComFunc.MsgBox("해당 등록번호는 없는 번호입니다. ");
                    return;
                }
            }
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            int i, j, k;
            int nREAD;
            int nRow;

            string strDel = "";
            string strCode = "";
            string strNewData = "";
            string strOldData = "";
            string strSuCode = "";
            string strTemp = "";
            string SQL = "";
            string SqlErr = "";

            DataTable dt = null;

            ssAct_Sheet1.RowCount = 0;

            if (String.Compare(dtpFDate.Text, dtpTDate.Text) > 0)
            {
                ComFunc.MsgBox("시작일자가 종료일자보다 큼");
                return;
            }

            if(Convert.ToInt32(ComFunc.TimeDiffMin(dtpFDate.Text, dtpTDate.Text)) > 525600)
            {
                ComFunc.MsgBox("작업기간은 최대 365일간 입니다.");

                if (MessageBox.Show("오전업무 사용량이 많은 경우 오후에 작업을 해주십시오. 그래도 계속 작업 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            strSuCode = "";

            for (i = 0; i < ssSuga_Sheet1.RowCount; i++)
            {
                strDel = ssSuga_Sheet1.Cells[i, 0].Text;
                strCode = ssSuga_Sheet1.Cells[i, 1].Text;

                if (strDel != "True" && strCode != "")
                {
                    strSuCode = strSuCode + "'" + strCode + "',";
                }
            }

            if (txtDaiCode.Text != "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  SUNEXT";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN";
                SQL += ComNum.VBLF + "WHERE DAICODE = '" + txtDaiCode.Text.ToUpper() + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strSuCode = strSuCode + "'" + dt.Rows[i]["SUNEXT"].ToString().Trim() + "',";
                }
                SqlErr = "";
                dt.Dispose();
                dt = null;
            }

            if (strSuCode == "")
            {
                ComFunc.MsgBox("조회할 수가코드가 공란입니다.");
            }

            //마지막의 ","를 제거함
            strSuCode = VB.Left(strSuCode, VB.Len(strSuCode) - 1);

            btnView.Enabled = false;

            
            strTemp = "";

            //심사계수녀님 요청으로 누적별로 변경

            if (optIpd.Checked == true || optOut.Checked == true)
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  /*+index(a INDEX_IPDNEWSL5)*/ SuNext,Pano,Bi,WardCode,DeptCode,DrCode, ";
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  SuNext,Pano,Bi,WardCode,DeptCode,DrCode, ";
            }
            SQL += ComNum.VBLF + "  TO_CHAR(Bdate,'yyyy-mm-dd') BIlja,SUM(Qty *Nal) QTY,GbSelf,GbGisul,";
            SQL += ComNum.VBLF + "  round(SUM(BaseAmt)/ COUNT(*)) BASEAMT ,SUM(Amt1+Amt2) Amt  ";

            if (optIpd.Checked == true || optOut.Checked == true)
            {
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a";
            }
            else
            {
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP";
            }

            SQL += ComNum.VBLF + " WHERE ActDate >= TO_DATE('" + dtpFDate.Text + "','yyyy-mm-dd')";
            SQL += ComNum.VBLF + "      AND SuNext IN (" + strSuCode.ToUpper() + ") ";

            if (optBun2.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND BUN IN ('11','12')  ";
            }
            if (optBun3.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND BUN IN ('20')  ";
            }

            SQL += ComNum.VBLF + "   AND BDate >= TO_DATE('" + dtpFDate.Text + "','yyyy-mm-dd')";
            SQL += ComNum.VBLF + "   AND BDate <= TO_DATE('" + dtpTDate.Text + "','yyyy-mm-dd')";

            if(optBi2.Checked == true)
            {
                SQL += ComNum.VBLF + "   AND BI IN ('11','12','13') ";
            }
            else if(optBi3.Checked == true)
            {
                SQL += ComNum.VBLF + "   AND BI IN ('21','22') ";
            }
            else if(optBi4.Checked == true)
            {
                SQL += ComNum.VBLF + "   AND BI IN ('31') ";
            }
            else if(optBi5.Checked == true)
            {
                SQL += ComNum.VBLF + "   AND BI IN ('52','55') ";
            }
            else if(optBi6.Checked == true)
            {
                SQL += ComNum.VBLF + "   AND BI IN ('51') ";
            }
            else if (optBi7.Checked == true)
            {
                SQL += ComNum.VBLF + "   AND BI IN ('11','12','13','21','22') ";
            }

            if(optF2.Checked == true)
            {
                SQL += ComNum.VBLF + "   AND GbSelf ='0'";
            }
            else if(optF3.Checked == true)
            {
                SQL += ComNum.VBLF + "   AND GbSelf ='1' ";
            }
            else if(optF4.Checked == true)
            {
                SQL += ComNum.VBLF + "   AND GbSelf ='2' ";
            }
            else if(optF5.Checked == true)
            {
                SQL += ComNum.VBLF + "   AND GbSelf IN ('1','2') ";
            }

            if(VB.Left(cboDept.SelectedItem.ToString(), 2) != "**")
            {
                SQL += ComNum.VBLF + "   AND DEPTCODE = '" + VB.Left(cboDept.SelectedItem.ToString(), 2) + "' ";
            }

            if(VB.Left(cboDrCode.SelectedItem.ToString(), 4) != "****")
            {
                SQL += ComNum.VBLF + "   AND DRCODE = '" + VB.Left(cboDrCode.SelectedItem.ToString(), 4) + "' ";
            }

            if(txtPano.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "   AND  PANO = '"+ txtPano.Text + "' ";
            }

            SQL += ComNum.VBLF + "   GROUP BY SuNext , Pano, Bi, WardCode, DeptCode, DrCode, ";
            SQL += ComNum.VBLF + "      BDATE, GbSelf,GbGisul ";

            if(chkQty.Checked == true)
            {
                SQL += ComNum.VBLF + "   HAVING SUM(Qty *Nal) > 0 ";
            }

            if(optAll.Checked == true)
            {
                string strSql = "";
                strSql = VB.Replace(SQL, "OPD_SLIP", "IPD_NEW_SLIP");
                SQL += ComNum.VBLF + "   UNION ALL  " + strSql;
            }
            else
            {
                if(optAct.Checked == true)
                {
                    SQL += ComNum.VBLF + "  ORDER BY SuNext,Bdate,Pano  ";
                }
                else
                {
                    SQL += ComNum.VBLF + "  ORDER BY SuNext,Pano,Bdate  ";
                }
            }

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            nRow = 0;

            nREAD = dt.Rows.Count;
            ssAct_Sheet1.RowCount = dt.Rows.Count;

            for(i = 0; i < nREAD; i++)
            {
                if(optAct.Checked == true)
                {
                    strNewData = dt.Rows[i]["SuNext"].ToString().Trim();
                    strNewData += dt.Rows[i]["BIlja"].ToString().Trim();
                }
                else
                {
                    strNewData = dt.Rows[i]["SuNext"].ToString().Trim();
                    strNewData += dt.Rows[i]["Pano"].ToString().Trim();
                }

                nRow = nRow + 1;

                if(nRow > ssAct_Sheet1.RowCount)
                {
                    ssAct_Sheet1.RowCount = nRow;
                }

                if(optAll.Checked == true)
                {
                    ssAct_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ssAct_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Bilja"].ToString().Trim();
                }

                if(strNewData != strOldData)
                {
                    if(optAct.Checked == true)
                    {
                        ssAct_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssAct_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Bilja"].ToString().Trim();
                        ssAct_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        Sname_Display(dt.Rows[i]["Pano"].ToString().Trim(), i);
                    }
                    else
                    {
                        ssAct_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssAct_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        Sname_Display(dt.Rows[i]["Pano"].ToString().Trim(), i);
                        ssAct_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BIlja"].ToString().Trim();
                    }
                    strOldData = strNewData;
                }
                else
                {
                    if(optAct.Checked == true)
                    {
                        ssAct_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Bilja"].ToString().Trim();
                        ssAct_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        Sname_Display(dt.Rows[i]["Pano"].ToString().Trim(), i);
                    }
                    else
                    {
                        ssAct_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BIlja"].ToString().Trim();
                    }
                }

                ssAct_Sheet1.Cells[i, 4].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                ssAct_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Bi"].ToString().Trim();
                ssAct_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                DrName_Display(dt.Rows[i]["DrCode"].ToString().Trim(), i);
                ssAct_Sheet1.Cells[i, 8].Text = String.Format("{0:#,##0.0}", dt.Rows[i]["Qty"]);
                ssAct_Sheet1.Cells[i, 10].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                ssAct_Sheet1.Cells[i, 11].Text = String.Format("{0:#,##0}", dt.Rows[i]["BaseAmt"]);
                ssAct_Sheet1.Cells[i, 12].Text = String.Format("{0:#,##0}", dt.Rows[i]["Amt"]);
            }

            if(nRow == 0)
            {
                ComFunc.MsgBox("자료가 1건도 없습니다. ");
            }
            else
            {
                ssAct_Sheet1.RowCount = nRow;
            }

            btnNext.Enabled = true;
            btnPrint.Enabled = true;
            
        }

        void Sname_Display(string Pano, int nRow)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT    ";
            SQL += ComNum.VBLF + "  Sname";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
            SQL += ComNum.VBLF + "WHERE Pano = '" + Pano + "'   ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (optAll.Checked == true)
            {
                ssAct_Sheet1.Cells[nRow, 3].Text = dt.Rows[0]["Sname"].ToString().Trim();
            }
            else
            {
                ssAct_Sheet1.Cells[nRow, 2].Text = dt.Rows[0]["Sname"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            return;

        }

        void DrName_Display(string DrCode, int nRow)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT    ";
            SQL += ComNum.VBLF + "  DrName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
            SQL += ComNum.VBLF + "WHERE DrCode = '" + DrCode + "'   ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }


            ssAct_Sheet1.Cells[nRow, 7].Text = dt.Rows[0]["DrName"].ToString().Trim();
            
            dt.Dispose();
            dt = null;

            return;
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
