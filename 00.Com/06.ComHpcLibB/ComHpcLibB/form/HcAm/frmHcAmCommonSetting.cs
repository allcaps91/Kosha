using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ComBase.Controls;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcAmCommonSetting.cs
/// Description     : 암 판정 권고사항
/// Author          : 이상훈
/// Create Date     : 2020-04-09
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm권고사항.frm(Frm상용세팅)" />
namespace ComHpcLibB
{
    public partial class frmHcAmCommonSetting : Form
    {
        HicResultwardService hicResultwardService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public delegate void SetGstrValue(string GstrValue);
        public event SetGstrValue rSetGstrValue;

        string FstrGubun;
        string FstrExam;
        string FstrContents;
        string FstrContents1;

        public frmHcAmCommonSetting(string strGubun, string strExam)
        {
            InitializeComponent();

            FstrGubun = strGubun;
            FstrExam = strExam;

            SetEvent();
        }

        void SetEvent()
        {
            hicResultwardService = new HicResultwardService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnMenuSelect.Click += new EventHandler(eBtnClick);
            this.btnMenuCancel.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Screen_Clear();
            fn_ComboPan_Set();
            fn_Screen_Display();
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
                fn_Screen_Display();
            }
            else if (sender == btnMenuSelect)
            {
                FstrContents = "";

                if (FstrGubun == "61")
                {
                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                        {
                            FstrContents += SS1.ActiveSheet.Cells[i, 1].Text.Trim() + "{}";
                            FstrContents += SS1.ActiveSheet.Cells[i, 3].Text.Trim() + "\r\n";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                        {
                            FstrContents += SS1.ActiveSheet.Cells[i, 1].Text.Trim() + "\r\n";
                        }
                    }
                }

                HIC_RESULTWARD item = SS1.GetCurrentRowData() as HIC_RESULTWARD;
                rSetGstrValue(FstrContents);
                this.Close();
            }
            else if (sender == btnMenuCancel)
            {
                fn_Screen_Clear();
                this.Close();
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                SS1.ActiveSheet.Cells[e.Row, 0].Text = "True";
                eBtnClick(btnMenuSelect, new EventArgs());
            }
        }

        void fn_Screen_Clear()
        {
            sp.Spread_All_Clear(SS1);
            clsPublic.GstrRetValue = "";
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            string strGubun = "";

            sp.Spread_All_Clear(SS1);

            strGubun = VB.Left(cboPan.Text.Trim(), 1);

            //기존의 자료를 읽음
            List<HIC_RESULTWARD> list = hicResultwardService.GetItembyGubunExamWardGubun2(FstrGubun, FstrExam, strGubun);

            nREAD = list.Count;
            SS1.ActiveSheet.RowCount = nREAD;

            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].WARDNAME;
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].GUBUN2;
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].SEQNO.To<string>("");
                }
            }
            else
            {
                fn_Screen_Clear();
            }

            //RowHeight 자동 조정
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (SS1.ActiveSheet.Cells[i, 1].Text.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Rows[i].Height = 20;
                }
                else
                {
                    Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 1);
                    SS1.ActiveSheet.Rows[i].Height = size.Height;
                }
            }
        }

        void fn_ComboPan_Set()
        {
            switch (FstrGubun)
            {
                case "11":
                    cboPan.Items.Clear();
                    cboPan.Items.Add("*.전체");
                    cboPan.Items.Add("1.이상소견없음 또는 위염");
                    cboPan.Items.Add("2.양성질환");
                    cboPan.Items.Add("3.위암 의심");
                    cboPan.Items.Add("4.위암");
                    cboPan.Items.Add("5.기타");
                    cboPan.Items.Add("6.기존 위암환자");
                    cboPan.SelectedIndex = 0;
                    break;
                case "21":
                    cboPan.Items.Clear();
                    cboPan.Items.Add("*.전체");
                    cboPan.Items.Add("1.이상소견없음");
                    cboPan.Items.Add("2.양성질환(용종)");
                    cboPan.Items.Add("3.대장암 의심");
                    cboPan.Items.Add("4.대장암");
                    cboPan.Items.Add("5.기타");
                    cboPan.Items.Add("6.기존 대장암환자");
                    cboPan.SelectedIndex = 0;
                    break;
                case "31":
                    cboPan.Items.Clear();
                    cboPan.Items.Add("*.전체");
                    cboPan.Items.Add("1.간암의심소견없음");
                    cboPan.Items.Add("2.특정검사요망(3개월 이내");
                    cboPan.Items.Add("3.간암 의심(정밀 검사 요망)");
                    cboPan.Items.Add("4.기타");
                    cboPan.Items.Add("5.기존 간암 환자");
                    cboPan.SelectedIndex = 0;
                    break;
                case "41":
                    cboPan.Items.Clear();
                    cboPan.Items.Add("*.전체");
                    cboPan.Items.Add("1.이상소견없음");
                    cboPan.Items.Add("2.양성질환");
                    cboPan.Items.Add("3.유방암 의심");
                    cboPan.Items.Add("4.판정유보");
                    cboPan.Items.Add("5.기존 유방암환자");
                    cboPan.SelectedIndex = 0;
                    break;
                case "51":
                    cboPan.Items.Clear();
                    cboPan.Items.Add("*.전체");
                    cboPan.Items.Add("1.이상소견없음");
                    cboPan.Items.Add("2.반응성 소견 및 감염성질환");
                    cboPan.Items.Add("3.비정형 상피세포 이상");
                    cboPan.Items.Add("4.자궁경부암 전구단계 의심");
                    cboPan.Items.Add("5.자궁경부암 의심");
                    cboPan.Items.Add("6.기타");
                    cboPan.Items.Add("7.기존자궁경부암 환자");
                    cboPan.SelectedIndex = 0;
                    break;
                case "61":
                    cboPan.Items.Clear();
                    cboPan.Items.Add("*.전체");
                    cboPan.Items.Add("1.이상소견없음");
                    cboPan.Items.Add("2.양성결절");
                    cboPan.Items.Add("3.경계선결절");
                    cboPan.Items.Add("4.폐암의심");
                    cboPan.Items.Add("4.폐암의심(4A)");
                    cboPan.Items.Add("4.폐암의심(4B)");
                    cboPan.Items.Add("4.폐암의심(4X)");
                    cboPan.Items.Add("5.기타:폐결절외 의미있는 소견(S)");
                    cboPan.SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }
    }
}
