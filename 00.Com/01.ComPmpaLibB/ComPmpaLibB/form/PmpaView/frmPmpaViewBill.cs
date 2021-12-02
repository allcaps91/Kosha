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
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>frmpmpaviewbill
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewBill.cs
    /// Description     : 계산서 조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-29
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\wontax\Frm계산서조회.frm(Frm계산서조회) => frmPmpaViewBill.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\wontax\Frm계산서조회.frm(Frm계산서조회)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewBill : Form, MainFormMessage
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

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

        public frmPmpaViewBill(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmPmpaViewBill()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);

            this.txtTaxNo.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtTaxNo)
            {
                if (e.KeyChar == 13)
                {
                    btnView.Focus();
                }
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등       

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            opt2.Checked = true;
            txtTaxNo.Text = "";

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            dtpFDate.Text = Convert.ToDateTime(CurrentDate).AddDays(-20).ToShortDateString();
            dtpTDate.Text = CurrentDate;
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

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }

            else if (sender == this.btnCancel)
            {
                ssList_Sheet1.Rows.Count = 0;
                ssList_Sheet1.Rows.Count = 20;
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            strTitle = "원무팀 계산서 내역";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 130, 50);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, true, true, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;


            #endregion
        }

        void eGetData()
        {
            int i = 0;
            int nREAD = 0;

            string strGbn = "";            
            string strOldName = "";
            
            string strTaxNo = "";

            double nGAmt = 0;
            double nVat = 0;
            double nToGamt = 0;
            double nToVat = 0;            

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            //세금계산서 매출/매입(매입 1, 매출 2)
            strGbn = "2";
            strTaxNo = txtTaxNo.Text.Trim();

            ssList_Sheet1.Rows.Count = 0;
            nToGamt = 0;
            nToVat = 0;
            nGAmt = 0;
            nVat = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = "";
            SQL += ComNum.VBLF + "CREATE OR REPLACE VIEW KOSMOS_ADM.VIEW_TAXCASH_VIEW2                                      ";
            SQL += ComNum.VBLF + "(BuName,Ltd, Name, UPTae, JongMok,GbTRB, GAmt, Gbn, BDate) AS                             ";
            //계산서
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  '원무과',A.TAXNO, A.Name, A.UPTAE, A.JONGMOK,A.GbTRB, A.TAXAMT, '2', A.Bdate            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_WONTAX A                                                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND A.BDate >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD')                       ";
            SQL += ComNum.VBLF + "      AND A.BDate <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD')                       ";
            SQL += ComNum.VBLF + "      AND (A.DELDATE IS NULL OR A.DELDATE  = '')                                          ";
            if (strTaxNo != "")
            {
                SQL += ComNum.VBLF + "  AND A.TAXNO = '" + strTaxNo + "'                                                    ";
            }
            SQL += ComNum.VBLF + "UNION ALL                                                                                 ";
            //계산서 국세청전송(+)
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  '원무과',A.TAXNO, A.Name, A.UPTAE, A.JONGMOK,A.GbTRB, A.TAXAMT, '2', A.Bdate            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_WONTAX A                                                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND A.BDate >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD')                       ";
            SQL += ComNum.VBLF + "      AND A.BDate <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD')                       ";
            SQL += ComNum.VBLF + "      AND A.DELDATE IS NOT NULL                                                           ";
            //SQL += ComNum.VBLF + "      AND A.BDATE <> A.DELDATE                                                            ";
            //2021-11-02 2021년 기준으로 조회 기준 변경
            //기존은 삭제일 등록일 같은건 안나오도록. => 2021년 이후 삭제일 있으면 모두 표시
            SQL += ComNum.VBLF + "      AND (                                                               ";
            SQL += ComNum.VBLF + "                   A.BDATE >= TO_DATE('2021-01-01','YYYY-MM-DD')          ";
            SQL += ComNum.VBLF + "             OR   (A.BDATE < TO_DATE('2021-01-01','YYYY-MM-DD') AND A.BDATE <> A.DELDATE ) ";
            SQL += ComNum.VBLF + "          )                                                               ";

            if (strTaxNo != "")
            {
                SQL += ComNum.VBLF + "  AND A.TAXNO = '" + strTaxNo + "'                                                    ";
            }
            SQL += ComNum.VBLF + "UNION ALL                                                                                 ";

            //계산서 국세청전송(-)
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  '원무과',A.TAXNO, A.Name, A.UPTAE, A.JONGMOK,A.GbTRB,(A.TAXAMT * -1), '2', A.Bdate      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_WONTAX A                                                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
            SQL += ComNum.VBLF + "      AND A.Bdate >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD')                       ";
            SQL += ComNum.VBLF + "      AND A.Bdate <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD')                       ";
            SQL += ComNum.VBLF + "      AND A.DELDATE IS NOT NULL                                                           ";
            //SQL += ComNum.VBLF + "      AND A.BDATE <> A.DELDATE                                                            ";
            //2021-11-02 2021년 기준으로 조회 기준 변경
            //기존은 삭제일 등록일 같은건 안나오도록. => 2021년 이후 삭제일 있으면 모두 표시
            SQL += ComNum.VBLF + "      AND (                                                               ";
            SQL += ComNum.VBLF + "                   A.BDATE >= TO_DATE('2021-01-01','YYYY-MM-DD')          ";
            SQL += ComNum.VBLF + "             OR   (A.BDATE < TO_DATE('2021-01-01','YYYY-MM-DD') AND A.BDATE <> A.DELDATE ) ";
            SQL += ComNum.VBLF + "          )                                                               ";


            if (strTaxNo != "")
            {
                SQL += ComNum.VBLF + "  AND A.TAXNO = '" + strTaxNo + "'                                                    ";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  Ltd, Name, UpTae, JongMok,GbTRB, GAmt,TO_CHAR(BDate,'YYYY-MM-DD') BDATE     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "VIEW_TAXCASH_VIEW2                                  ";
            if (opt0.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY NAME, LTD                                                        ";
            }
            else if (opt1.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY LTD,NAME,BDATE                                                   ";
            }
            else
            {
                SQL += ComNum.VBLF + "ORDER BY BDATE,NAME                                                       ";
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
                    nREAD = dt.Rows.Count;
                    strOldName = "";
                    ssList_Sheet1.Rows.Count = nREAD + 1;

                    for (i = 0; i < nREAD; i++)
                    {
                        nGAmt = VB.Val(dt.Rows[i]["Gamt"].ToString().Trim());

                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Ltd"].ToString().Trim();       //사업자등록번호
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Name"].ToString().Trim();      //상호
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["UpTae"].ToString().Trim();     //업태
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["JongMok"].ToString().Trim();   //종목
                        ssList_Sheet1.Cells[i, 5].Text = nGAmt.ToString();                          //공급가액
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GbTRB"].ToString().Trim();     //전자계산서
                        nToGamt += nGAmt;
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

            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 4].Text = "합  계";
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 5].Text = nToGamt.ToString();

            SQL = "";
            SQL = "DROP VIEW KOSMOS_ADM.VIEW_TAXCASH_VIEW2";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            clsDB.setCommitTran(clsDB.DbCon);

        }


    }
}
