using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
//using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHcActRemarkMgmt.cs
/// Description     : 참고사항 관리
/// Author          : 이상훈
/// Create Date     : 2020-10-12
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "신규" />

namespace HC_Act
{
    public partial class frmHcActRemarkMgmt : Form
    {
        HicBcodeService hicBcodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public delegate void SetGstrValue(string GstrValue);
        public event SetGstrValue rSetGstrValue;

        string FstrGubun = "HIC_참고사항문구관리";
        string FstrRmkArea = "";

        public frmHcActRemarkMgmt()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcActRemarkMgmt(string strRmkArea)
        {
            InitializeComponent();
            FstrRmkArea = strRmkArea;
            SetEvent();
        }

        void SetEvent()
        {
            hicBcodeService = new HicBcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnInput.Click += new EventHandler(eBtnClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            string strName = "";

            if (FstrRmkArea == "INPUT")
            {
                if (ssList.ActiveSheet.NonEmptyColumnCount > 0)
                {   
                    if (ssList.ActiveSheet.Cells[e.Row, 2].Text.Trim() != "")
                    {
                        strName = ssList.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                    }

                    rSetGstrValue(strName);
                }
            }
            this.Close();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;

            ComFunc.ReadSysDate(clsDB.DbCon);

            sp.Spread_All_Clear(ssList);
            ssList_Sheet1.SetRowHeight(-1, 22);

            if (FstrRmkArea == "INPUT")
            {
                btnSearch.Visible = false;
                btnSave.Visible = false;
                btnInput.Visible = true;
            }
            else
            {
                btnSearch.Visible = true;
                btnSave.Visible = true;
                btnInput.Visible = false;
            }

            eBtnClick(btnSearch, new EventArgs());
        }

        void eBtnClick(object sender, EventArgs e)
        {
            string strCode = "";
            string strRowId = "";

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                sp.Spread_All_Clear(ssList);

                List<HIC_BCODE> list = hicBcodeService.GetItembyGubun(FstrGubun);

                if (list.Count > 0)
                {
                    ssList.ActiveSheet.RowCount = list.Count + 20;
                    for (int i = 0; i < list.Count; i++)
                    {
                        ssList.ActiveSheet.Cells[i, 1].Text = list[i].CODE;
                        ssList.ActiveSheet.Cells[i, 2].Text = list[i].NAME;
                        ssList.ActiveSheet.Cells[i, 3].Text = list[i].SORT.To<string>();
                        ssList.ActiveSheet.Cells[i, 4].Text = list[i].RID;
                    }
                }
                else
                {
                    ssList.ActiveSheet.RowCount = 20;
                }
            }
            else if (sender == btnSave)
            {
                clsDB.setBeginTran(clsDB.DbCon);

                if (ssList.ActiveSheet.NonEmptyRowCount > 0)
                {
                    if (fn_Delete(FstrGubun) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("참고사항 항목 입력 중 오류발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    for (int i = 0; i < ssList.ActiveSheet.NonEmptyRowCount; i++)
                    {   
                        HIC_BCODE item = new HIC_BCODE();

                        if (ssList.ActiveSheet.Cells[i, 0].Text == "")
                        {
                            item.GUBUN = FstrGubun;
                            item.CODE = ssList.ActiveSheet.Cells[i, 1].Text;
                            item.NAME = ssList.ActiveSheet.Cells[i, 2].Text;
                            item.SORT = ssList.ActiveSheet.Cells[i, 3].Text.To<long>();

                            if (fn_Insert(item) == false)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("참고사항 항목 입력 중 오류발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                MessageBox.Show("저장되었습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                eBtnClick(btnExit, new EventArgs());
            }
            else if (sender == btnInput)
            {
                string strName = "";

                if (FstrRmkArea == "INPUT")
                {
                    if (ssList.ActiveSheet.NonEmptyColumnCount > 0)
                    {   
                        for (int i = 0; i < ssList.ActiveSheet.NonEmptyColumnCount; i++)
                        {
                            if (ssList.ActiveSheet.Cells[i, 0].Text == "True")
                            {   
                                if (ssList.ActiveSheet.Cells[i, 2].Text.Trim() != "")
                                {
                                    strName += ssList.ActiveSheet.Cells[i, 2].Text.Trim() + ",";                                    
                                }
                            }
                        }
                        strName = VB.Mid(strName, 1, strName.Length - 1);
                        rSetGstrValue(strName);
                    }
                }
                this.Close();
            }
        }

        bool fn_Insert(HIC_BCODE item)
        {
            int result = 0;
            bool rtnVal = true;

            result = hicBcodeService.SaveBcode(item);

            if (result < 0)
            {   
                rtnVal = false;
            }
            return rtnVal;
        }

        bool fn_Delete(string argGubun)
        {
            int result = 0;
            bool rtnVal = true;

            result = hicBcodeService.DeletebyGubun(argGubun);

            if (result < 0)
            {
                rtnVal = false;
            }
            return rtnVal;
        }
    }
}
