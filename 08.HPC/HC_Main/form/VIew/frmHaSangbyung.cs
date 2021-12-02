using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using System;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaSangbyung.cs
/// Description     : 종합검진 상병별 대상자 조회
/// Author          : 이상훈
/// Create Date     : 2019-10-07
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain71.frm(FrmSangByung)" />

namespace HC_Main
{
    public partial class frmHaSangbyung : Form
    {
        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        public frmHaSangbyung()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);            
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            cboSB.Text = "";
            txtLtdCode.Text = "";
            cboSB.Items.Clear();
            cboSB.Items.Add(" ");
            cboSB.Items.Add("**.전 체");
            cboSB.Items.Add("01.고혈압");
            cboSB.Items.Add("02.고지혈");
            cboSB.Items.Add("03.당뇨질환");
            cboSB.Items.Add("04.간장질환");
            cboSB.Items.Add("05.신장질환");
            cboSB.Items.Add("06.흉부질환");
            cboSB.Items.Add("07.기타흉부질환");
            cboSB.Items.Add("08.빈혈증");
            cboSB.Items.Add("09.부인과질환");
            cboSB.Items.Add("10.기타질환");
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                string strFDate = "";
                string strTDate = "";
                string strSB = "";   //상병명
                string strLtdCode = "";

                strFDate = dtpFrDate.Text;
                strTDate = dtpToDate.Text;
                strSB = VB.Left(cboSB.Text.Trim(), 2);
                strLtdCode = txtLtdCode.Text.Trim();

                ///TODO : 이상훈 (2019.10.07) 쿼리만 있고 Display 가 없음.
                //?????????????????????????????
                //SQL = " SELECT A.SNAME,A.AGE,A.SEX,A.PANREMARK,B.RESULT "
                //SQL = SQL & " FROM HEA_JEPSU A, HEA_RESULT B "
                //SQL = SQL & " WHERE A.SDATE >= TO_DATE('" & Trim(strFDate) & "','YYYY-MM-DD') "
                //SQL = SQL & "   AND A.SDATE <= TO_DATE('" & Trim(strTDate) & "','YYYY-MM-DD') "
                //SQL = SQL & "   AND A.WRTNO = B.WRTNO(+) "
                //SQL = SQL & "   AND A.LTDCODE=" & Val(strLtdCode) & " "
                //?????????????????????????????
            }
            else if (sender == btnLtdCode)
            {
                frmHcLtdHelp frm = new frmHcLtdHelp();
                frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                frm.ShowDialog();
                frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length > 4)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
        }
    }
}
