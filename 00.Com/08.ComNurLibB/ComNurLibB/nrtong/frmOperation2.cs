using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB;
using ComBase;
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmOperation2.cs
    /// Description     : 수술실 대장
    /// Author          : 안정수
    /// Create Date     : 2018-02-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 nrtong36.frm(FrmOperation2) 폼 frmOperation2.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrtong\nrtong36.frm(FrmOperation2) >> frmOperation2.cs 폼이름 재정의" />
    public partial class frmOperation2 : Form, MainFormMessage
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

        public frmOperation2(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmOperation2()
        {
            InitializeComponent();
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

        void Set_Init()
        {
            int i = 0;

            dtpFDate.Text = clsPublic.GstrSysDate;
            dtpTDate.Text = clsPublic.GstrSysDate;

            cboGbn.Items.Clear();
            cboGbn.Items.Add(" ");
            cboGbn.Items.Add("1.정규수술대장");
            cboGbn.Items.Add("2.응급수술대장");
            cboGbn.Items.Add("3.통원수술대장");
            cboGbn.Items.Add("4.수술실처치대장");
            cboGbn.Items.Add("5.수술실운동대장");
            cboGbn.Items.Add("6.수술실교정대장");
            cboGbn.Items.Add("7.조직검사시술대장");
            cboGbn.Items.Add("8.수술취소대장");
            cboGbn.SelectedIndex = 0;

            cboGbn2.Items.Clear();
            cboGbn2.Items.Add(" ");
            cboGbn2.Items.Add("1.전신마취대장");
            cboGbn2.Items.Add("2.부위마취대장(전체)");
            cboGbn2.Items.Add("3.국소마취대장");
            cboGbn2.Items.Add("4.부위마취대장(척추마취)");
            cboGbn2.Items.Add("5.부위마취대장(경막외마취)");
            cboGbn2.Items.Add("6.부위마취대장(상박신경총마취)");
            cboGbn2.Items.Add("7.부위마취대장(기타)");
            cboGbn2.SelectedIndex = 0;

            //진료과 Combo SET
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  DeptCode";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ClinicDept";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND DeptCode NOT IN ('II','HR','TO','R6','HD','CS','PT','AN')";
            SQL += ComNum.VBLF + "ORDER BY PrintRanking";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                cboDept.Items.Clear();
                cboDept.Items.Add("전체");

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                }
            }

            dt.Dispose();
            dt = null;

            cboDept.SelectedIndex = 0;

            clsSpread.gSpreadHeaderLineBoder(ssList, 0, 0, 0, ssList_Sheet1.ColumnCount - 1, Color.Black, 1, false, false, false, true);
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
            bool PrePrint = false;          

            strTitle = "수 술 실 대 장";
            strSubTitle = "조회기간 : " + dtpFDate.Text + "~" + dtpTDate.Text;
            strSubTitle += "\r\n" + "인쇄일자 : " + clsPublic.GstrSysDate;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);            

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false, 0.85f);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
        }

        void eGetData()
        {
            int i = 0;
            int nREAD = 0;
            int nRow = 0;

            string strOK = "";
            string strOpSize = "";
            string strOpSize2 = "";
            string strFDate = "";
            string strTDate = "";
            string strOpGbn = "";
            string strOpGbn2 = "";
            string strOpGbn3 = "";
            string strOpGbnX = "";
            string strAngbn = "";

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            if(cboGbn.Text != "")
            {
                switch(VB.Left(cboGbn.SelectedItem.ToString().Trim(), 1))
                {
                    case "1":
                        strOpGbn = "1";         //정규수술
                        break;

                    case "2":
                        strOpGbn = "2','3";     //응급수술
                        break;

                    case "3":
                        strOpGbn = "4";         //통원수술
                        break;

                    case "4":
                        strOpGbn = "4";         //처치
                        break;

                    case "5":
                        strOpGbn = "5";         //교정
                        break;

                    case "6":
                        strOpGbn = "6";         //운동
                        break;

                    case "7":
                        strOpGbn = "E/B";         //조직검사시술
                        break;

                    case "8":
                        strOpGbn = "X";         //취소건
                        break;
                }
            }

            if(cboGbn2.Text != "")
            {
                switch(VB.Left(cboGbn2.SelectedItem.ToString().Trim(), 1))
                {
                    case "1":
                        strAngbn = "G";                         //전신마취
                        break;

                    case "2":
                        strAngbn = "S','E','A";                 //부위마취전체
                        break;

                    case "3":
                        strAngbn = "L";                         //국소마취
                        break;

                    case "4":
                        strAngbn = "S";                         //부위마취-척추마취
                        break;

                    case "5":
                        strAngbn = "E";                         //부위마취-경막외마취
                        break;

                    case "6":
                        strAngbn = "A";                         //부위마취-상박신경총마취
                        break;

                    case "7":
                        strAngbn = "M','LV-A','LV-B','LV-C";    //기타마취-mask,정맥-a,b,c
                        break;
                }
            }

            CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT * ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ORAN_MASTER";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND OpDate >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND OpDate <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";

            if(cboDept.SelectedItem.ToString().Trim() != "전체")
            {
                SQL += ComNum.VBLF + "  AND DeptCode = '" + cboDept.SelectedItem.ToString().Trim() + "' ";  //과
            }

            if(strOpGbn != "")
            {
                SQL += ComNum.VBLF + "  AND OpBun IN ('" + strOpGbn + "')";     //수술구분
            }

            if(strAngbn != "")
            {
                SQL += ComNum.VBLF + "  AND AnGbn IN ('" + strAngbn + "')";     //마취구분
            }

            if(strOpGbn3 != "")
            {
                SQL += ComNum.VBLF + "  AND OpTitle IN ('" + strOpGbn3 + "')";  //마취구분
            }

            if(strOpGbnX == "X")
            {
                SQL += ComNum.VBLF + "  AND OpCancel IS NOT NULL";
            }
            
            SQL += ComNum.VBLF + "      AND (GbAngio IS NULL OR GbAngio <> 'Y')";


            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;
                ssList.ActiveSheet.Rows.Count = nREAD;

                for(i = 0; i < nREAD; i++)
                {
                    strOK = "OK";

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  OpSize";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ORAN_OPBUN";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND Dept='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";
                    SQL += ComNum.VBLF + "      AND Bun='" + dt.Rows[i]["OpBun"].ToString().Trim() + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if(dt1.Rows.Count > 0)
                    {
                        switch (dt1.Rows[0]["OpSize"].ToString().Trim())
                        {
                            case "1":
                                strOpSize = "대";
                                break;

                            case "2":
                                strOpSize = "중";
                                break;

                            case "3":
                                strOpSize = "소";
                                break;

                            case "4":
                                strOpSize2 = "처치";
                                break;

                            case "5":
                                strOpSize2 = "교정";
                                break;

                            case "6":
                                strOpSize2 = "운동";
                                break;

                            default:
                                strOpSize = "";
                                strOpSize2 = "";
                                break;
                        }
                    }

                    if(strOpGbn2 != "")
                    {
                        strOK = "";

                        switch (strOpGbn2)
                        {
                            case "4":
                                if(strOpSize2 == "처치")
                                {
                                    strOK = "OK";
                                }
                                break;

                            case "5":
                                if (strOpSize2 == "교정")
                                {
                                    strOK = "OK";
                                }
                                break;

                            case "6":
                                if (strOpSize2 == "운동")
                                {
                                    strOK = "OK";
                                }
                                break;
                        }
                    }

                    if(strOK == "OK")
                    {
                        nRow += 1;

                        ssList.ActiveSheet.Cells[nRow - 1, 0].Text = VB.Left(dt.Rows[i]["OpDate"].ToString().Trim(), 10);
                        ssList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["OpRoom"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 3].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 5].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 7].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 8].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        ssList.ActiveSheet.Cells[nRow - 1, 9].Text = dt.Rows[i]["DIAGNOSIS"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 10].Text = dt.Rows[i]["OpTitle"].ToString().Trim();

                        switch (dt.Rows[i]["OpBun"].ToString().Trim())
                        {
                            case "1":
                                ssList.ActiveSheet.Cells[nRow - 1, 11].Text = "정규수술";
                                break;

                            case "2":
                                ssList.ActiveSheet.Cells[nRow - 1, 11].Text = "응급수술";
                                break;

                            case "3":
                                ssList.ActiveSheet.Cells[nRow - 1, 11].Text = "ER응급수술";
                                break;

                            case "4":
                                ssList.ActiveSheet.Cells[nRow - 1, 11].Text = "통원수술";
                                break;

                            case "A":
                                ssList.ActiveSheet.Cells[nRow - 1, 11].Text = "환자사유";
                                break;

                            case "B":
                                ssList.ActiveSheet.Cells[nRow - 1, 11].Text = "환자거부";
                                break;

                            case "C":
                                ssList.ActiveSheet.Cells[nRow - 1, 11].Text = "입원안함";
                                break;

                            case "X":
                                ssList.ActiveSheet.Cells[nRow - 1, 11].Text = "취소전체";
                                break;

                            case "0":
                                ssList.ActiveSheet.Cells[nRow - 1, 11].Text = "병원사유";
                                break;
                        }

                        ssList.ActiveSheet.Cells[nRow - 1, 12].Text = strOpSize2;
                        ssList.ActiveSheet.Cells[nRow - 1, 13].Text = dt.Rows[i]["AnDoct1"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 14].Text = dt.Rows[i]["AnGbn"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 15].Text = strOpSize;

                        dt1.Dispose();
                        dt1 = null;

                        ssList.ActiveSheet.Cells[nRow - 1, 16].Text = "무";

                        if(dt.Rows[i]["OPCANCEL"].ToString().Trim() != "")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 16].Text = "유";
                        }

                        ssList_Sheet1.SetRowHeight(nRow - 1, (int)ssList_Sheet1.GetPreferredRowHeight(nRow - 1));

                    }
                }

            }

            dt.Dispose();
            dt = null;

            ssList.ActiveSheet.Rows.Count = nRow;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssList_Sheet1.RowCount = 0;
        }
    }
}


