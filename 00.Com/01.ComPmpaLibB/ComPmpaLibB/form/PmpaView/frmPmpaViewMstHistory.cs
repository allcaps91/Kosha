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
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMstHistory.cs
    /// Description     : 고객정보 마스타 변경 내역
    /// Author          : 안정수
    /// Create Date     : 2017-08-07
    /// Update History  : 2017-10-23
    /// 출력, 조회 부분 수정
    /// <history>       
    /// d:\psmh\Etc\csinfo\csinfo57.frm(FrmMstHistory) => frmPmpaViewMstHistory.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\Etc\csinfo\csinfo57.frm(FrmMstHistory)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewMstHistory : Form, MainFormMessage
    {
        public delegate void SendPano(string sPano);
        public event SendPano rSendPano;

        ComFunc CF = new ComFunc();

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

        public frmPmpaViewMstHistory(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmPmpaViewMstHistory()
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

            dtpFdate.Text = Convert.ToDateTime(dtpTdate.Text).AddDays(-31).ToShortDateString();
            dtpTdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
                //                
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
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

            ssList.ActiveSheet.Columns[12].Visible = false;
            ssList.ActiveSheet.Columns[13].Visible = false;
            ssList.ActiveSheet.Columns[14].Visible = false;


            #endregion

            strTitle = "고객정보 마스타 변경 내역";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 80, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, true, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            ssList.ActiveSheet.Columns[12].Visible = true;
            ssList.ActiveSheet.Columns[13].Visible = true;
            ssList.ActiveSheet.Columns[14].Visible = true;
            #endregion
        }

        void eGetData()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                            ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.JobDate,'YYYYMMDD HH24:MI') JobDate,a.JobSabun,a.GbJob,                               ";
            SQL += ComNum.VBLF + "  a.Pano,b.SName,a.Tel,a.HPhone,TO_CHAR(a.Birth,'YYYY-MM-DD') Birth,                              ";
            SQL += ComNum.VBLF + "  a.GbBirht,a.EMail,a.Jikup,a.Religion,a.GbInfor,a.Ltdname,a.Remark                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_MSTHISTORY a, " + ComNum.DB_PMPA + "BAS_PATIENT b           ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                         ";
            SQL += ComNum.VBLF + "      AND a.JobDate>=TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')                                ";
            SQL += ComNum.VBLF + "      AND a.JobDate<=TO_DATE('" + dtpTdate.Text + " 23:59','YYYY-MM-DD HH24:MI')                  ";
            SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                                        ";
            SQL += ComNum.VBLF + "ORDER BY a.JobDate,a.GbJob,a.Pano                                                                 ";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["JobDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = CF.READ_PassName(clsDB.DbCon, dt.Rows[i]["JobSabun"].ToString().Trim());

                        switch (dt.Rows[i]["GbJob"].ToString().Trim())
                        {
                            case "1":
                                ssList_Sheet1.Cells[i, 2].Text = "신규";
                                break;
                            case "2":
                                ssList_Sheet1.Cells[i, 2].Text = "수정전";
                                break;
                            case "3":
                                ssList_Sheet1.Cells[i, 2].Text = "수정후";
                                break;
                            case "4":
                                ssList_Sheet1.Cells[i, 2].Text = "삭제";
                                break;
                        }

                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Tel"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["HPhone"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Birth"].ToString().Trim();

                        switch (dt.Rows[i]["GbBirht"].ToString().Trim())
                        {
                            case "+":
                                ssList_Sheet1.Cells[i, 8].Text = "양력";
                                break;
                            case "-":
                                ssList_Sheet1.Cells[i, 8].Text = "음력";
                                break;
                            default:
                                ssList_Sheet1.Cells[i, 8].Text = "";
                                break;
                        }

                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["EMail"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 10].Text = CF.READ_Jikup_Name(clsDB.DbCon, dt.Rows[i]["Jikup"].ToString().Trim());

                        // 종교구분
                        switch (dt.Rows[i]["Religion"].ToString().Trim())
                        {
                            case "1":
                                ssList_Sheet1.Cells[i, 11].Text = "카톨릭";
                                break;
                            case "2":
                                ssList_Sheet1.Cells[i, 11].Text = "기독교";
                                break;
                            case "3":
                                ssList_Sheet1.Cells[i, 11].Text = "불교";
                                break;
                            case "4":
                                ssList_Sheet1.Cells[i, 11].Text = "천도교";
                                break;
                            case "5":
                                ssList_Sheet1.Cells[i, 11].Text = "유교";
                                break;
                            case "9":
                                ssList_Sheet1.Cells[i, 11].Text = "기타";
                                break;
                            default:
                                ssList_Sheet1.Cells[i, 11].Text = "";
                                break;
                        }

                        ssList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["GbInfor"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 13].Text = dt.Rows[i]["LtdName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 14].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    }
                }
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

            btnPrint.Enabled = true;
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            rSendPano(ssList_Sheet1.Cells[e.Row, 1].Text);
        }
    }
}
