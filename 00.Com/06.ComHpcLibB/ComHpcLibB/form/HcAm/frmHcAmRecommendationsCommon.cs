using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcAmRecommendationsCommon.cs
/// Description     : 암판정 권고사항 등록
/// Author          : 이상훈
/// Create Date     : 2019-12-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm권고사항상용구.frm(Frm권고사항상용구)" />

namespace ComHpcLibB
{
    public partial class frmHcAmRecommendationsCommon : Form
    {
        BasBcodeService basBcodeService = null;
        HicResultwardService hicResultwardService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        string FstrGubun;   //검사구분

        //FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
        //FarPoint.Win.Spread.CellType.ComboBoxCellType combo1 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

        public frmHcAmRecommendationsCommon()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            basBcodeService = new BasBcodeService();
            hicResultwardService = new HicResultwardService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSeq.Click += new EventHandler(eBtnClick);
            this.btnStmch.Click += new EventHandler(eBtnClick);
            this.btnColon.Click += new EventHandler(eBtnClick);
            this.btnSogen.Click += new EventHandler(eBtnClick);
            this.btnBSogen.Click += new EventHandler(eBtnClick);
            this.btnWSogen.Click += new EventHandler(eBtnClick);
            this.btnLSogen1.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.SS1.LeaveCell += new LeaveCellEventHandler(eSpLeaveCell);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            sp.Spread_All_Clear(SS1);
            SS1_Sheet1.Columns.Get(5).Visible = false;
            fn_Screen_Clear();
            SS1.ActiveSheet.RowCount = 200;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnStmch)
            {
                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 200;

                FstrGubun = "11";

                //1.정상" + Chr$(9) + "2.양성질환" + Chr$(9) + "3.위암의심" + Chr$(9) + "4.위암" + Chr$(9) + "5.기타
                //1.위장조영검사" + Chr$(9) + "2.위내시경검사

                fn_CelltypeCombo(5, 2, 3, "HC_암판정구분_위암", "HC_암검사구분_위암");

                fn_Screen_Display();
            }
            else if (sender == btnColon)
            {
                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 200;

                FstrGubun = "21";

                //1.음성" + Chr$(9) + "2.정상" + Chr$(9) + "3.양성질환" + Chr$(9) + "4.대장암의심" + Chr$(9) + "5.대장암" + Chr$(9) + "6.기존대장암 환자" + Chr$(9) + "7.기타
                //1.분변잠혈반응검사" + Chr$(9) + "2.대장이중조영검사" + Chr$(9) + "3.대장내시경검사

                fn_CelltypeCombo(5, 2, 3, "HC_암판정구분_대장암", "HC_암검사구분_대장암");

                fn_Screen_Display();
            }
            else if (sender == btnSogen)
            {
                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 200;

                FstrGubun = "31";

                //1.이상없음" + Chr$(9) + "2.양성질환" + Chr$(9) + "3.간암의심" + Chr$(9) + "4.기타" + Chr$(9) + "5.기존간암 환자" + Chr$(9) + "6.이상있음" + Chr$(9) + "7.간암고위험간장질환
                //1.복부초음파" + Chr$(9) + "2.혈청태아단백검사" + Chr$(9) + "3.간장질환검사

                fn_CelltypeCombo(5, 2, 3, "HC_암판정구분_간암", "HC_암검사구분_간암");

                fn_Screen_Display();
            }
            else if (sender == btnBSogen)
            {
                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 200;

                FstrGubun = "41";

                //1.정상" + Chr$(9) + "2.양성질환" + Chr$(9) + "3.유방암의심" + Chr$(9) + "4.판정유보" + Chr$(9) + "5.기존유방암 환자
                //1.유방단순촬영

                fn_CelltypeCombo(5, 2, 3, "HC_암판정구분_유방암", "HC_암검사구분_유방암");

                fn_Screen_Display();
            }
            else if (sender == btnWSogen)
            {
                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 200;

                FstrGubun = "51";

                //1.정상" + Chr$(9) + "2.검체부적절" + Chr$(9) + "3.염증성 또는 감염성질환" + Chr$(9) + "4.상피세포 이상" + Chr$(9) + "5.자궁경부암 의심" + Chr$(9) + "6.기타" + Chr$(9) + "7.기존자궁경부암 환자
                //1.자궁세포검사

                fn_CelltypeCombo(5, 2, 3, "HC_암판정구분_자궁암", "HC_암검사구분_자궁암");

                fn_Screen_Display();
            }
            else if (sender == btnLSogen1)
            {
                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 200;

                FstrGubun = "61";

                //1.이상없음" + Chr$(9) + "2.양성결절" + Chr$(9) + "3.경계선결절" + Chr$(9) + "4.폐암의심" + Chr$(9) + "5.기타
                //1.CT

                fn_CelltypeCombo(5, 2, 3, "HC_암판정구분_폐암", "HC_암검사구분_폐암");

                fn_Screen_Display();
            }
        }

        void fn_CelltypeCombo(int k, int nCol1, int nCol2, string sGubun1, string sGubun2)
        {
            string[] sComboList = new string[k];

            FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType combo1 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            List<BAS_BCODE> list = basBcodeService.GetCodeNamebyBCode(sGubun1, "");

            if (list.Count > 0)
            {
                Array.Resize(ref sComboList, list.Count);

                for (int K = 0; K < list.Count; K++)
                {
                    sComboList[K] = "";
                }

                for (int i = 0; i < list.Count; i++)
                {
                    sComboList[i] = list[i].CODE + "." + list[i].NAME;
                }

                combo.Items = sComboList;
                combo.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                combo.MaxDrop = 20;
                combo.MaxLength = 100;
                combo.ListWidth = 200;
                combo.Editable = false;

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    SS1.ActiveSheet.Cells[i, nCol1].CellType = combo;
                }

                List<BAS_BCODE> list2 = basBcodeService.GetCodeNamebyBCode(sGubun2, "");

                Array.Resize(ref sComboList, list2.Count);

                for (int K = 0; K < list2.Count; K++)
                {
                    sComboList[K] = "";
                }

                for (int i = 0; i < list2.Count; i++)
                {
                    sComboList[i] = list2[i].CODE + "." + list2[i].NAME;
                }

                combo1.Items = sComboList;
                combo1.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                combo1.MaxDrop = 20;
                combo1.MaxLength = 100;
                combo1.ListWidth = 200;
                combo1.Editable = false;

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    SS1.ActiveSheet.Cells[i, nCol2].CellType = combo1;
                }
            }
        }

        void fn_Screen_Clear()
        {
            sp.Spread_All_Clear(SS1);
            SS1.ActiveSheet.RowCount = 200;

            //RowHeight 자동 조정
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (SS1.ActiveSheet.Cells[i, 4].Text.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Rows[i].Height = 20;
                }
                else
                {
                    Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 4);
                    SS1.ActiveSheet.Rows[i].Height = size.Height;
                }
            }
        }

        void fn_Screen_Display()
        {
            int nRow = 0;
            long nRead = 0;

            SS1_Sheet1.Columns.Get(5).Visible = false;

            //기존의 자료를 읽음
            List<HIC_RESULTWARD> list = hicResultwardService.GetItembyGubun(FstrGubun);

            nRead = list.Count;
            SS1_Sheet1.ColumnHeader.Rows.Get(-1).Height = 25F;
            for (int i = 0; i < nRead; i++)
            {
                SS1.ActiveSheet.Cells[i, 1].Text = string.Format("{0:00000}", (int)list[i].SEQNO);
                if (!list[i].GUBUN2.IsNullOrEmpty())
                {
                    //스프레드 콤보박스 Set
                    clsSpread.gSdCboItemFindLeft(SS1, i, 2, 1, list[i].GUBUN2);
                }
                if (!list[i].EXAMWARD.IsNullOrEmpty())
                {
                    clsSpread.gSdCboItemFindLeft(SS1, i, 3, 1, list[i].EXAMWARD);
                }
                SS1.ActiveSheet.Cells[i, 4].Text = list[i].WARDNAME;
                SS1.ActiveSheet.Cells[i, 5].Text = list[i].ROWID;
            }

            //RowHeight 자동 조정
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 4);
                SS1.ActiveSheet.Rows[i].Height = size.Height;
            }
        }

        void eSpLeaveCell(object sender, LeaveCellEventArgs e)
        {
            if (e.Column != 3) return;

            if (SS1.ActiveSheet.Cells[e.Row, e.Column].Text.Trim().Length > 600)
            {
                MessageBox.Show("권고사항이 600자를 초과하였습니다.", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
