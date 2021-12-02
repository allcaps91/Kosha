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
    /// File Name       : frmPmpaViewInfoList.cs
    /// Description     : 고객정보 구분별 명단
    /// Author          : 안정수
    /// Create Date     : 2017-08-07
    /// Update History  : 2017-11-07
    /// <history>           
    /// d:\psmh\Etc\csinfo\csinfo02.frm(FrmInfoList) => frmPmpaViewInfoList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\Etc\csinfo\csinfo02.frm(FrmInfoList)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewInfoList : Form, MainFormMessage
    {
        ComFunc CF = new ComFunc();
        string SendPano = "";

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

        public frmPmpaViewInfoList(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmPmpaViewInfoList()
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

            ssList_Sheet1.Columns[7].Visible = false; //ROWID

            Set_Combo();

            optDel0.Checked = true;
        }

        void Set_Combo()
        {
            int i = 0;
            string strCODE = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //병동코드 Combobox SET
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                         ";
            SQL += ComNum.VBLF + "  WardCode                                                                     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD                                            ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                      ";
            SQL += ComNum.VBLF + "  AND WardCode NOT IN ('2W')                                                   ";
            SQL += ComNum.VBLF + "ORDER BY WardCode                                                              ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                cboWard.Items.Clear();
                cboWard.Items.Add("전체");
                cboWard.SelectedIndex = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strCODE = dt.Rows[i]["WardCode"].ToString().Trim();
                    cboWard.Items.Add(strCODE);
                }

                dtpFdate.Text = Convert.ToDateTime(dtpTdate.Text).AddDays(-31).ToShortDateString();

                cboGubun.Items.Clear();
                cboGubun.Items.Add("***.전체");

                CF.ComboGubun_ADDITEM(clsDB.DbCon, cboGubun);
                cboGubun.SelectedIndex = 0;
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
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
            string strSubTitle = "";

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            strTitle = "고객정보 구분별 명단";
            strSubTitle = "정보구분: " + cboGubun.SelectedItem.ToString().Trim();

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 60, 50);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, true, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eGetData()
        {
            int i = 0;
            string strGubun = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strGubun = VB.Left(cboGubun.SelectedItem.ToString(), 3);
            ssList_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                            ";
            SQL += ComNum.VBLF + "  a.Pano,b.SName,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,                                             ";
            SQL += ComNum.VBLF + "  a.Gubun,a.Code,a.Remark,a.BuseName,a.ROWID                                                      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_DATA a, " + ComNum.DB_PMPA + "BAS_PATIENT b                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                         ";
            SQL += ComNum.VBLF + "  AND a.BDate>=TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')                                      ";
            SQL += ComNum.VBLF + "  AND a.BDate<=TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD')                                      ";
            if (strGubun != "***")
            {
                SQL += ComNum.VBLF + "  AND a.Gubun='" + strGubun + "'                                                              ";
            }
            if (optDel0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND a.DelDate IS NULL                                                                       ";
            }
            if (cboWard.SelectedItem.ToString() != "전체")
            {
                SQL += ComNum.VBLF + "  AND a.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                        ";
                SQL += ComNum.VBLF + "                          WHERE WardCode='" + cboWard.SelectedItem.ToString() + "'            ";
                SQL += ComNum.VBLF + "                            AND GBSTS IN ('0','2') AND OUTDATE IS NULL )                      ";
            }
            SQL += ComNum.VBLF + "  AND a.Pano=b.Pano(+)                                                                            ";
            SQL += ComNum.VBLF + "ORDER BY a.BDate,a.Pano                                                                           ";

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
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BuseName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Remark"].ToString().Trim();
                        //deldate가 테이블에 없다고 함
                        //ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DelDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }
            }

            catch (System.Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
            btnPrint.Enabled = true;
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SendPano = ssList_Sheet1.Cells[e.Row, 1].Text;

            frmViewCsinfo f = new frmViewCsinfo(SendPano);        
            f.Show();
        }
    }
}
