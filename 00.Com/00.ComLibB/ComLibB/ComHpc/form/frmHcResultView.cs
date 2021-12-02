using ComBase;
using ComBase.Controls;
using ComLibB;
using ComLibB.Dto;
using ComLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcResultView.cs
/// Description     : 접수번호별 검사결과 조회
/// Author          : 이상훈
/// Create Date     : 2020-06-11
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain86.frm(frmHcResultView)" />

namespace ComLibB
{
    public partial class frmHcResultView : Form
    {
        //HicJepsuService hicJepsuService = null;
        //HicResultExCodeService hicResultExCodeService = null;
        //HeaExjongService heaExjongService = null;

        ComHpcService comHpcService = null;

        clsSpread sp = new clsSpread();
        //clsHcMain hm = new clsHcMain();
        //clsHaBase hb = new clsHaBase();
        //clsHcFunc hf = new clsHcFunc();
        clsComHpc ch = new clsComHpc();
        ComFunc cf = new ComFunc();

        string fstrPtno;
        string FstrGubun;   //종검/일검 구분(종검 : HEA , 일검 : HIC)
        long FnWrtNo;

        public frmHcResultView(string strGubun, long nWrtNo, string strPtno = "")
        {
            InitializeComponent();
            FstrGubun = strGubun;
            FnWrtNo = nWrtNo;
            fstrPtno = strPtno;
            SetEvent();
        }

        void SetEvent()
        {
            //hicJepsuService = new HicJepsuService();
            //hicResultExCodeService = new HicResultExCodeService();
            //heaExjongService = new HeaExjongService();

            comHpcService = new ComHpcService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            ssList.ActiveSheet.RowCount = 0;

            if (!fstrPtno.IsNullOrEmpty())
            {
                List<COMHPC> list = comHpcService.GetHicJepsuitembyPtno(fstrPtno);
                if(list.Count> 0)
                {
                    ssList.ActiveSheet.RowCount = list.Count;
                    for (int i = 0; i < list.Count ; i++)
                    {
                        ssList.ActiveSheet.Cells[i, 0].Text = list[i].JEPDATE;
                        ssList.ActiveSheet.Cells[i, 1].Text = list[i].GJJONG;
                        ssList.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.ToString();
                    }
                }
            }

            txtWrtNo.Text = "";
            if (FnWrtNo > 0)
            {
                txtWrtNo.Text = FnWrtNo.ToString();
                fn_Screen_Display();
            }
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
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            string strJepDate = "";
            string strExCode = "";
            string strSex = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";

            if (!txtWrtNo.Text.IsNullOrEmpty())
            {
                FnWrtNo = txtWrtNo.Text.Trim().To<long>();
            }

            //Screen_Injek_display  //인적사항을 Display
            //HIC_JEPSU list = hicJepsuService.GetItemHicHeabyWrtNo(FstrGubun, FnWrtNo);
            COMHPC list = comHpcService.GetItemHicHeabyWrtNo(FstrGubun, FnWrtNo);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            strJepDate = list.JEPDATE;
            strSex = list.SEX;

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PTNO;
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.SEX + "/" + list.AGE.ToString();
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = ch.READ_Ltd_Name(list.LTDCODE.ToString()); //회사명
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = strJepDate;
            if (FstrGubun == "HIC")
            {
                ssPatInfo.ActiveSheet.Cells[0, 5].Text = ch.READ_GjJong_Name(list.GJJONG);  //건진유형
            }
            else if (FstrGubun == "HEA")
            {
                //ssPatInfo.ActiveSheet.Cells[0, 5].Text = heaExjongService.Read_ExJong_Name(list.GJJONG); //종검유형
                ssPatInfo.ActiveSheet.Cells[0, 5].Text = comHpcService.Read_ExJong_Name(list.GJJONG); //종검유형
            }

            //Screen_Exam_Items_display  '검사항목을 Display
            //List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoOrderbyPanjengPartExCode(FnWrtNo);
            //List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItemHicHeabyWrtNoOrderbyPanjengPartExCode(FstrGubun, FnWrtNo);
            List<COMHPC> list2 = comHpcService.GetItemHicHeabyWrtNoOrderbyPanjengPartExCode(FstrGubun, FnWrtNo);

            nREAD = list2.Count;
            SS2.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list2[i].EXCODE;
                strResult = list2[i].RESULT;
                strResCode = list2[i].RESCODE;
                strResultType = list2[i].RESULTTYPE;
                strGbCodeUse = list2[i].GBCODEUSE;

                SS2.ActiveSheet.Cells[i, 0].Text = list2[i].EXCODE;
                SS2.ActiveSheet.Cells[i, 1].Text = list2[i].HNAME;
                SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                
                if (strGbCodeUse == "Y")
                {
                    if (strResult != "")
                    {
                        SS2.ActiveSheet.Cells[i, 3].Text = ch.READ_ResultName(strResCode, strResult);
                    }
                }

                if (list2[i].PANJENG == "2")
                {
                    SS2.ActiveSheet.Cells[i, 4].Text = "*";
                }

                //참고치를 Dispaly
                strNomal = ch.EXAM_NomalValue_SET(strExCode, strJepDate, strSex, list2[i].MIN_M, list2[i].MAX_M, list2[i].MIN_F, list2[i].MAX_F);
                SS2.ActiveSheet.Cells[i, 5].Text = strNomal;
            }

            //검사결과의 판정값이 R,B인것을 위에 표시
            //FarPoint.Win.Spread.SortInfo[] si = new FarPoint.Win.Spread.SortInfo[] {
            //   new FarPoint.Win.Spread.SortInfo(2,true)
            //,  new FarPoint.Win.Spread.SortInfo(3,true)};

            //SS2_Sheet1.SortRows(0, SS2_Sheet1.RowCount, si);
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssList)
            {
                txtWrtNo.Text = ssList.ActiveSheet.Cells[e.Row, 2].Text;
                fn_Screen_Display();
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtWrtNo.Text != "")
                {
                    fn_Screen_Display();
                }
            }
        }
    }
}
