using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaGaJepsuView.cs  ==== > 사용무 (2020.10.11) 호출하는곳 없음.(HEA_RESERVED 테이블 존재하지 않음)
/// Description     : 예약 접수자 명단조회
/// Author          : 이상훈
/// Create Date     : 2019-10-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain17.frm(FrmGaJepsuView)" />

namespace HC_Main
{
    public partial class frmHaGaJepsuView : Form
    {
        //사용무 : 호출하는곳 없음(2019-10-02) 확인
        HicLtdService hicLtdService = null;
        HeaResvSetService heaResvSetService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hc = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public delegate void Spread_DoubleClick(string strRemark);
        public event Spread_DoubleClick SpreadDoubleClick;

        public frmHaGaJepsuView()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicLtdService = new HicLtdService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnDelete)
            {
                string Response = "";
                string strChk = "";
                string strJEPDATE  = "";
                long nPano = 0;

                if (MessageBox.Show("체크하신 가접수자료를 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                        {
                            SS1.ActiveSheet.Cells[i, 0].Text = "";
                            strJEPDATE = "";
                            nPano = 0;
                            strJEPDATE = SS1.ActiveSheet.Cells[i, 3].Text.Trim();
                            nPano = long.Parse(SS1.ActiveSheet.Cells[i, 5].Text.Trim());

                            //예약접수 자료 삭제
                            //int result = 
                        }
                    }
                }
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "접  수  자    명  단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("조회기간:" + dtpFrDate.Text + " ~ " + dtpToDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += sp.setSpdPrint_String("검진회사:" + VB.Pstr(txtLtdCode.Text, ".", 2), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += sp.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnSearch)
            {
                string strFDate = "";
                string strTDate = "";
                string strLtdCode = "";
                string strJumin = "";
                long nPano = 0;

                sp.Spread_All_Clear(SS1);

                strFDate = dtpFrDate.Text;
                strTDate = DateTime.Parse(dtpToDate.Text).AddDays(1).ToShortDateString();
                strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);

                List<HEA_RESV_SET> list = heaResvSetService.GetItembyYDateLtdCode(strFDate, strTDate, strLtdCode);

                //if (list.Count > 0)
                //{
                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        nPano = list[i].PANO;


                //    }
                //}
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                //TODO : 이상훈 - 호출하는곳 확인 후 Delegate 연결 필요 = >화면 사용무

                string strData = "";

                strData = string.Format("{0:0000000000}", SS1.ActiveSheet.Cells[e.Row, 5].Text);    //건진번호
                strData += string.Format("{0:00}", SS1.ActiveSheet.Cells[e.Row, 9].Text);           //건진종류
                strData += SS1.ActiveSheet.Cells[e.Row, 18].Text;
                clsPublic.GstrRetValue = strData;
                this.Hide();
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void fn_Screen_Clear()
        {
            txtLtdCode.Text = "";
        }
        
        void Read_Ltd_Info()
        {
            //사업장코드의 지사,사업장기호를 Display
            HIC_LTD list = hicLtdService.GetCountLtdCode(long.Parse(txtLtdCode.Text));
            if (list == null)
            {
                txtLtdCode.Text = "";
                MessageBox.Show("사업장이 등록안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtLtdCode.Text += "." + list.NAME.Trim();
        }
    }
}
