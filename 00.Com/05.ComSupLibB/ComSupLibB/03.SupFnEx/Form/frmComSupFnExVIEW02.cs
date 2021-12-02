using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using ComSupLibB.SupXray;

namespace ComSupLibB.SupFnEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupFnEx
    /// File Name       : frmComSupFnExVIEW02.cs
    /// Description     : 기능검사 의사명단에서 의사 선택
    /// Author          : 윤조연
    /// Create Date     : 2017-06-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// frmComSupFnExViewDr.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\Ocs\ekg\ekg06.frm(FrmViewDr) >> frmComSupFnExVIEW02.cs 폼이름 재정의" />
    public partial class frmComSupFnExVIEW02 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSup sup = new clsComSup();
        clsComSupFnExSpd fnexSql = new clsComSupFnExSpd();
        clsComSupXraySQL cSQL = new clsComSupXraySQL();
        
        public delegate void SendMsg(string[] str);
        public event SendMsg rSendMsg;
        string[] msgX = null;

        #endregion

        public frmComSupFnExVIEW02()
        {
            InitializeComponent();

            setEvent();
        }

        //기본값 세팅
        void setCtrlData(PsmhDb pDbCon)
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            setCombo(pDbCon);

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            //this.btnSearch2.Click += BtnSearch1_Click;
            //this.btnExit.Click += new EventHandler(eBtnEvent);

            this.ssList.CellDoubleClick += ssList_CellDoubleClick;


            this.cboDept.SelectedIndexChanged += new EventHandler(eCboEvent);

        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            msgX = new string[Enum.GetValues(typeof(clsComSupFnExSpd.enmSpdDr)).Length];
            msgX[(int)clsComSupFnExSpd.enmSpdDr.DeptCode] = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmSpdDr.DeptCode].Text;
            msgX[(int)clsComSupFnExSpd.enmSpdDr.DrCode] = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmSpdDr.DrCode].Text;
            msgX[(int)clsComSupFnExSpd.enmSpdDr.DrName] = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmSpdDr.DrName].Text;

            rSendMsg(msgX);

            this.Hide();
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

                setCtrlData(clsDB.DbCon);

                screen_clear();

                //
                //clsSupFnExSpd.AUTO_SPREAD_SET_SupFnExMain(ssList_Sheet1, 10, 0);
            }
        }
        
        void eBtnEvent(object sender, EventArgs e)
        {
            //if (sender == this.btnExit)
            //{
            //    this.Close();
            //}

        }

        void eCboEvent(object sender, EventArgs e)
        {
            DataTable dt = sup.sel_Bas_Doctor_ComBo(clsDB.DbCon, VB.Left(this.cboDept.SelectedItem.ToString(), 2),"",true,false,true);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                ssList.ActiveSheet.DataSource = dt;
                //헤더 및 사이즈
                methodSpd.setHeader(ssList, fnexSql.sSpdDr, fnexSql.nSpdDr);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }
        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            //txtSearch.Text = "";   
            //dtpFDate.Text =cpublic.strSysDate;  

        }

        void setCombo(PsmhDb pDbCon)
        {
            cboDept.Items.Clear();

            //XRAY_방사선종류
            DataTable dt = comSql.sel_BAS_CLINICDEPT_COMBO(pDbCon);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboDept, dt, clsParam.enmComParamComboType.ALL);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }

        }

      
    }
}
