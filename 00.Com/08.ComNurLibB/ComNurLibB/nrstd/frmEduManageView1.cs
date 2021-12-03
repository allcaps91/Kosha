using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComNurLibB
{
    public partial class frmEduManageView1 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();


        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;
        DataTable dt1 = null;

        #endregion


        #region MainFormMessage InterFace

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {

        }

        public void MsgUnloadForm(Form frm)
        {

        }

        public void MsgFormClear()
        {

        }

        public void MsgSendPara(string strPara)
        {

        }

        #endregion

        public frmEduManageView1()
        {
            InitializeComponent();
            setEvent();
        }
        public frmEduManageView1(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);

            this.btnPrint.Click += new EventHandler(eBtnPrint);

            this.dtpFDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtSabun.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
        }
        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                ComFunc.ReadSysDate(clsDB.DbCon);

                Set_Init();
            }
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eBtnClick(object sender, EventArgs e)
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
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtSabun)
            {
                txtSabun.Text = ComFunc.SetAutoZero(txtSabun.Text, 5);

                if (e.KeyChar == 13)
                {
                    txtName.Text = CF.Read_SabunName(clsDB.DbCon, txtSabun.Text.Trim());
                }
            }

            else if (sender == this.dtpFDate || sender == this.dtpTDate)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void Set_Init()
        {
            int i = 0;

            txtSabun.Text = "";
            txtName.Text = "";

            dtpFDate.Text = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-20).ToShortDateString();

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  A.MATCH_CODE BUSE, B.NAME BUNAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_CODE A, " + ComNum.DB_PMPA + "BAS_BUSE B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND A.MATCH_CODE = B.BUCODE ";
            SQL += ComNum.VBLF + "      AND A.SUBUSE = '1'";
            SQL += ComNum.VBLF + "ORDER BY SUBRANKING ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                cboBuse.Items.Clear();
                cboBuse.Items.Add(" ");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboBuse.Items.Add(dt.Rows[i]["BuName"].ToString().Trim() + "." + dt.Rows[i]["Buse"].ToString().Trim());
                }

                cboBuse.Items.Add("정형외과(일반).100251");
            }

            dt.Dispose();
            dt = null;

            cboJong.Items.Clear();
            cboJong.Items.Add(" ");
            cboJong.Items.Add("01.병동교육");
            cboJong.Items.Add("02.감염교육");
            cboJong.Items.Add("03.QI교육");
            cboJong.Items.Add("04.CS교육");
            cboJong.Items.Add("05.CPR교육");
            cboJong.Items.Add("06.학술강좌");
            cboJong.Items.Add("07.간호부주최 직무교육");
            cboJong.Items.Add("08.전직원교육");
            cboJong.Items.Add("09.특강(간협)");
            cboJong.Items.Add("10.연수교육");
            cboJong.Items.Add("11.10대질환");
            cboJong.Items.Add("12.보수교육");
            cboJong.Items.Add("13.기타Report");
            cboJong.Items.Add("14.강사활동(교육)");
            cboJong.Items.Add("15.프리셉터교육");
            cboJong.Items.Add("16.Cyber 교육");
            cboJong.Items.Add("17.승진자교육");
            cboJong.Items.Add("18.기타");
            cboJong.SelectedIndex = 0;

            cboManJong.Items.Clear();
            cboManJong.Items.Add(" ");
            cboManJong.Items.Add("01.의사");
            cboManJong.Items.Add("02.전담간호사");
            cboManJong.Items.Add("03.병동간호사");
            cboManJong.Items.Add("04.타부서의뢰");
            cboManJong.Items.Add("05.기타");
            cboManJong.Items.Add("06.간호사");
            cboJong.SelectedIndex = 0;

            cboJumsu.Items.Clear();
            cboJumsu.Items.Add(" ");
            cboJumsu.Items.Add("01.2점");
            cboJumsu.Items.Add("02.3점");
            cboJumsu.Items.Add("03.5점");
            cboJumsu.Items.Add("04.5점이상");
            cboJumsu.SelectedIndex = 0;

            eGetData();
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            string strBuse = "";
            bool PrePrint = false;

            if (cboBuse.SelectedIndex == -1)
            {
                strBuse = "전체부서";
            }

            else
            {
                strBuse = VB.Pstr(cboBuse.SelectedItem.ToString().Trim(), ".", 1);
            }

            strTitle = strBuse + " 개인별 교육내역";

            strSubTitle = "    조회기간 : " + dtpFDate.Text + " ~ " + dtpTDate.Text;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String(strSubTitle, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("    출력일자 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void eGetData()
        {
            int i = 0;
            double nSum = 0;

            int nREAD = 0;

            CS.Spread_All_Clear(ssList1);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  A.WRTNO,A.SABUN,A.SNAME,A.BUCODE,A.IPSADATE,";
            SQL += ComNum.VBLF + "  TO_CHAR(A.FrDate,'YYYY-MM-DD') FrDate,TO_CHAR(A.ToDate,'YYYY-MM-DD') ToDate,";
            SQL += ComNum.VBLF + "  A.JIKJONG,A.EDUJONG,A.EDUNAME,";
            SQL += ComNum.VBLF + "  A.OptTime,A.EDUTIME,A.MAN, A.OptPlace,A.PLACE,A.JUMSU,A.GUBUN,A.ENTDATE,A.ROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_MST A, " + ComNum.DB_ERP + "INSA_MST B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ((A.GUBUN = '2' AND A.SIGN = '1') OR A.Gubun ='1')";
            SQL += ComNum.VBLF + "      AND TRUNC(A.SABUN) = B.SABUN ";

            if (txtSabun.Text != "")
            {
                SQL += ComNum.VBLF + "  AND TRUNC(SABUN) =  '" + txtSabun.Text + "'";
            }

            SQL += ComNum.VBLF + "      AND FRDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND FRDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";

            if (cboBuse.Text != "" && cboBuse.SelectedIndex != 0)
            {
                SQL += ComNum.VBLF + "  AND B.BUSE ='" + VB.Pstr(cboBuse.SelectedItem.ToString(), ".", 2) + "'";
            }

            if (cboJong.Text != "" && cboJong.SelectedIndex != 0)
            {
                SQL += ComNum.VBLF + "  AND EduJong ='" + VB.Left(cboJong.SelectedItem.ToString().Trim(), 2) + "'";
            }

            if (cboManJong.Text != "" && cboManJong.SelectedIndex != 0)
            {
                SQL += ComNum.VBLF + "  AND ManJong ='" + VB.Left(cboManJong.SelectedItem.ToString().Trim(), 2) + "'";
            }

            if (cboJumsu.Text != "" && cboJumsu.SelectedIndex != 0)
            {
                switch (VB.Left(cboJumsu.SelectedItem.ToString().Trim(), 2))
                {
                    case "04":
                        SQL += ComNum.VBLF + "AND Jumsu >='" + VB.Pstr(VB.Pstr(cboJumsu.SelectedItem.ToString().Trim(), ".", 2), "점", 1) + "' ";
                        break;

                    default:
                        SQL += ComNum.VBLF + "AND Jumsu ='" + VB.Pstr(VB.Pstr(cboJumsu.SelectedItem.ToString().Trim(), ".", 2), "점", 1) + "' ";
                        break;
                }
            }

            SQL += ComNum.VBLF + "Order By EntDate";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ssList1.ActiveSheet.Rows.Count = dt.Rows.Count;
                nREAD = dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    ssList1.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                    //개인정보읽기
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  c.Code, a.Sabun,TO_CHAR(a.IpsaDay,'YYYY-MM-DD') IpsaDay,a.Jik,a.KorName,a.Buse,b.Name BuseName,c.Name JikName";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST a, " + ComNum.DB_PMPA + "BAS_BUSE b, " + ComNum.DB_ERP + "INSA_CODE c";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND a.IpsaDay<=TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND (a.ToiDay IS NULL OR a.ToiDay>=TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'))";
                    SQL += ComNum.VBLF + "      AND a.Sabun  ='" + dt.Rows[i]["Sabun"].ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "      AND a.Buse=b.BuCode(+)";
                    SQL += ComNum.VBLF + "      AND a.Jik=c.Code(+)";
                    SQL += ComNum.VBLF + "      AND c.Gubun='2'";
                    SQL += ComNum.VBLF + "ORDER BY c.Code, a.Sabun, a.KorName";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssList1.ActiveSheet.Cells[i, 2].Text = dt1.Rows[0]["BuseName"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    switch (Convert.ToInt32(VB.Val(dt.Rows[i]["EduJong"].ToString().Trim())))
                    {
                        case 1:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "병동";
                            break;

                        case 2:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "감염";
                            break;

                        case 3:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "QI";
                            break;

                        case 4:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "CS";
                            break;

                        case 5:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "CPR";
                            break;

                        case 6:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "학술";
                            break;

                        case 7:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "직무";
                            break;

                        case 8:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "전직원";
                            break;

                        case 9:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "특강";
                            break;

                        case 10:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "연수";
                            break;

                        case 11:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "10대";
                            break;

                        case 12:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "보수";
                            break;

                        case 13:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "Report";
                            break;

                        case 14:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "강사";
                            break;

                        case 15:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "프리셉터";
                            break;

                        case 16:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "Cyber";
                            break;

                        case 17:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "승진자";
                            break;

                        default:
                            ssList1.ActiveSheet.Cells[i, 3].Text = "기타";
                            break;
                    }

                    ssList1.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["EduName"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["FrDate"].ToString().Trim();

                    if (dt.Rows[i]["ToDate"].ToString().Trim() != "")
                    {
                        ssList1.ActiveSheet.Cells[i, 5].Text += "~" + dt.Rows[i]["ToDate"].ToString().Trim();
                    }

                    switch (dt.Rows[i]["OptTIme"].ToString().Trim())
                    {
                        case "0":
                            ssList1.ActiveSheet.Cells[i, 6].Text = "10분";
                            break;

                        case "1":
                            ssList1.ActiveSheet.Cells[i, 6].Text = "30분내";
                            break;

                        case "2":
                            ssList1.ActiveSheet.Cells[i, 6].Text = "1시간내";
                            break;

                        case "3":
                            ssList1.ActiveSheet.Cells[i, 6].Text = "90분";
                            break;

                        case "4":
                            ssList1.ActiveSheet.Cells[i, 6].Text = "2시간";
                            break;
                    }

                    if (dt.Rows[i]["EDUTime"].ToString().Trim() != "")
                    {
                        ssList1.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["EDUTime"].ToString().Trim();
                    }

                    switch (dt.Rows[i]["OptPlace"].ToString().Trim())
                    {
                        case "0":
                            ssList1.ActiveSheet.Cells[i, 7].Text = "마리아홀";
                            break;

                        case "1":
                            ssList1.ActiveSheet.Cells[i, 7].Text = "466호실";
                            break;
                    }

                    if (dt.Rows[i]["Place"].ToString().Trim() != "")
                    {
                        ssList1.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["Place"].ToString().Trim();
                    }

                    ssList1.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["Jumsu"].ToString().Trim();
                    nSum = nSum + VB.Val(dt.Rows[i]["Jumsu"].ToString().Trim());
                    ssList1.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["Man"].ToString().Trim();
                }

                ssList1.ActiveSheet.Rows.Count += 1;
                ssList1.ActiveSheet.Cells[ssList1.ActiveSheet.Rows.Count - 1, 8].Text = nSum.ToString();
            }

            dt.Dispose();
            dt = null;
        }
    }
}
