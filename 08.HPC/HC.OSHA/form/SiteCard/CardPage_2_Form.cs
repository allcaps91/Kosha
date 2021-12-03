using ComBase.Controls;
using ComBase.Mvc.Utils;
using HC.Core.Common.Interface;
using HC_Core;
using HC.Core.Common.Util;
using HC.Core.Model;
using HC.OSHA.Dto;
using HC.OSHA.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase.Mvc.Enums;
using System.Drawing.Printing;
using ComHpcLibB.Model;

namespace HC_OSHA
{
    public partial class CardPage_2_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        private HcOshaCard4_1Service hcOshaCard4_1Service;
        HcOshaCard4_3Service hcOshaCard4_3Service;
      
        public CardPage_2_Form()
        {
            InitializeComponent();
            hcOshaCard4_1Service = new HcOshaCard4_1Service();
            hcOshaCard4_3Service = new HcOshaCard4_3Service();
        }

        private void CardPage_2_Form_Load(object sender, EventArgs e)
        {
            TxtTASKDIAGRAM.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD4_1.TASKDIAGRAM)});


            SSList2.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList2.AddColumnText("성명", nameof(HC_OSHA_CARD4_3.NAME), 80, IsReadOnly.N, new SpreadCellTypeOption { IsMulti = true, IsSort = false });
            SSList2.AddColumnText("선임일자", nameof(HC_OSHA_CARD4_3.DECLAREDATE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsMulti = true, IsSort = false });
            SSList2.AddColumnText("직책", nameof(HC_OSHA_CARD4_3.GRADE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsMulti = true, IsSort = false });
            SSList2.AddColumnText("공정(업무)명)", nameof(HC_OSHA_CARD4_3.TASKNAME), 100, IsReadOnly.N, new SpreadCellTypeOption { IsMulti = true, IsSort = false });
            SSList2.AddColumnText("교육과정명", nameof(HC_OSHA_CARD4_3.EDUNAME), 100, IsReadOnly.N, new SpreadCellTypeOption { IsMulti = true, IsSort = false });
            SSList2.AddColumnText("교육기간", nameof(HC_OSHA_CARD4_3.EDUDATE), 150, IsReadOnly.N, new SpreadCellTypeOption { IsMulti = true, IsSort = false });
            SSList2.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += (s, ev) => { SSList2.DeleteRow(ev.Row); };
        }
   
        public void Clear()
        {
         //   SSCard.ActiveSheet.Cells[0, 0].Value = "4. 업무(작업) 개요(2)";
        //    SSCard.ActiveSheet.Cells[2, 0].Value = "";

            TxtPrint.Text = "";
        }

        public void Search()
        {
            if (base.SelectedSite == null)
            {
                return;
            }

            Clear();

            //HC_OSHA_CARD4_1 dto = hcOshaCard4_1Service.hcOshaCard41Repository.FindByEstimate(base.SelectedEstimate.ID, base.GetCurrentYear());
            HC_OSHA_CARD4_1 dto = hcOshaCard4_1Service.hcOshaCard41Repository.FindByEstimate(base.SelectedEstimate.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));

            if (dto == null)
            {
                panDiagram.SetData(new HC_OSHA_CARD4_1());
              //  SSCard.ActiveSheet.Cells[0, 0].Value = "4. 업무(작업) 개요(2) - 해당없음";
            }
            else
            {
                panDiagram.SetData(dto);
                if (dto.TASKDIAGRAM.IsNullOrEmpty())
                {
                    TxtPrint.Text = "";
                   // SSCard.ActiveSheet.Cells[0, 0].Value = "4. 업무(작업) 개요(2) - 해당없음";
                }
                else
                {
                    //  SSCard.ActiveSheet.Cells[2, 0].Value = dto.TASKDIAGRAM;
                       TxtPrint.Text = dto.TASKDIAGRAM;
                    //  TxtPrint.SetValue(dto.TASKDIAGRAM);
                   // TxtPrint.Text = "awdaw";

                }
            }
        }

        private void Search2()
        {
            List<HC_OSHA_CARD4_3> list = hcOshaCard4_3Service.FindAll(base.SelectedEstimate.ID);
            SSList2.SetDataSource(list);

            //string text = "";
            //foreach(HC_OSHA_CARD4_3 dto in list)
            //{
            //    text += "성명:" + dto.NAME +" 선임일자:" + dto.DECLAREDATE + " 직책:" + dto.GRADE + " 공정(업무명):" + dto.TASKNAME + " 교육과정명:" + dto.EDUNAME +" 교육기간:" + dto.EDUDATE + "\r\n";
            //}

            //int selectionIndex = TxtTASKDIAGRAM.TextLength;
            //TxtTASKDIAGRAM.Text = TxtTASKDIAGRAM.Text.Insert(selectionIndex, text);
        }

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;

            panDiagram.SetData(new HC_OSHA_CARD4_1());
        }
        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;

            Search();
            Search2();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if(base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (panDiagram.Validate<HC_OSHA_CARD4_1>())
                {
                    HC_OSHA_CARD4_1 dto = panDiagram.GetData<HC_OSHA_CARD4_1>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    //HC_OSHA_CARD4_1 saved = hcOshaCard4_1Service.Save(dto, base.GetCurrentYear());
                    HC_OSHA_CARD4_1 saved = hcOshaCard4_1Service.Save(dto, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));

                    panDiagram.SetData(saved);
                    
                    Search();

                    MessageUtil.Info("저장하였습니다.");
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD4_1 dto = panDiagram.GetData<HC_OSHA_CARD4_1>();

            if (dto.ID > 0)
            {
                if(MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard4_1Service.Delete(dto);
                    panDiagram.Initialize();

                    Search();
                }
            }
        }

        private int linesPrinted;
        private string[] lines;
        public void Print()
        {
            //SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            //print.Execute();
            //Task.WaitAny(print.Execute());

            Search();

            string result = txt_split(TxtPrint.Text, 70);

            char[] param = { '\n' };

            lines = result.Split(param);

            int i = 0;
            char[] trimParam = { '\r' };
            foreach (string s in lines)
            {
                lines[i++] = s.TrimEnd(trimParam);
            }

            PrintDocument document = new PrintDocument();
            document.PrintPage += Document_PrintPage;
            
            document.Print();
        }

        public bool NewPrint()
        {
            Search();

            string result = txt_split(TxtPrint.Text, 70);

            char[] param = { '\n' };

            lines = result.Split(param);

            int i = 0;
            char[] trimParam = { '\r' };
            foreach (string s in lines)
            {
                lines[i++] = s.TrimEnd(trimParam);
            }

            PrintDocument document = new PrintDocument();
            document.PrintPage += Document_PrintPage;

            document.Print();
            return true;
        }


        private void Document_PrintPage(object sender, PrintPageEventArgs e)
        {
            // e.Graphics.DrawString(TxtPrint.Text, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, 20, 20);
            Brush brush = new SolidBrush(TxtPrint.ForeColor);
            

            Rectangle printAbleRect = Rectangle.Round(e.PageSettings.PrintableArea);
            Pen cPen = new Pen(Color.Black);
            cPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            cPen.Width = 0.5f;

            Font titleFont = new Font("돋음", 11f, FontStyle.Bold);
            //{Name = "맑은 고딕" Size=9.75}
            Font contentFont = new Font("맑은 고딕", 8.5f, FontStyle.Regular);

            float startX = 50 + printAbleRect.X;
            float startY = 20 + printAbleRect.Y;
            float plusY = 20 + printAbleRect.Y;
            float endLineX = printAbleRect.Width - (startX - 10);

            //e.Graphics.DrawString("4. 업무(작업) 개요(2)", titleFont, Brushes.Black, startX ,  plusY);
            e.Graphics.DrawString("4. 업무(작업) 개요(2)", titleFont, Brushes.Black, 60, plusY);
            e.Graphics.DrawRectangle(cPen, new Rectangle(60, (int)plusY + 25, 700, 1050));

            float lineY = plusY + 23;
            plusY = lineY;
            //e.Graphics.DrawLine(cPen, startX - 10,  lineY, endLineX,  lineY);

            while (linesPrinted < lines.Length)
            {
                if(linesPrinted == 0)
                {
                    plusY += 30;
                }
                else
                {
                    plusY += 15;
                }
                
                e.Graphics.DrawString(lines[linesPrinted++], contentFont, brush, startX, plusY);
             
                if (plusY >= e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            //  테두리 인쇄
            //e.Graphics.DrawRectangle(cPen, new Rectangle(20, 16, 738, 1100));

            linesPrinted = 0;
            e.HasMorePages = false;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void SSCard_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null || base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                SSList2.AddRows();
            }
        }
        public string txt_split(string s_str, int str_cnt)
        {
            string r_str = "";

            string[] s_split = s_str.Split('\n');

            for (int i = 0; i < s_split.Length; i++)
            {
                if (s_split[i].Length > str_cnt)
                {
                    if (i <= 0)
                    {
                        r_str += txt_split(s_str.Insert(str_cnt, "\n"), str_cnt);

                    }
                    else
                    {
                        r_str += txt_split(s_split[i].Insert(str_cnt, "\n"), str_cnt);
                    }

                }
                else
                {
                    r_str += s_split[i] + "\n";
                }

            }
            return r_str;
        }
        private void BtnSave2_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (SSList2.Validate())
                {
                    IList<HC_OSHA_CARD4_3> list = SSList2.GetEditbleData<HC_OSHA_CARD4_3>();
                    if (list.Count > 0)
                    {
                        if (hcOshaCard4_3Service.Save(list, base.SelectedEstimate.ID, base.SelectedSite.ID))
                        {
                            //    MessageUtil.Info("저장하였습니다");
                            Search2();
                        }
                        else
                        {
                            MessageUtil.Alert("오류가 발생하였습니다. ");

                        }
                    }
                }
            }
        }

        private void BtnGetData_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                return;
            }
            HcOshaContractService service = new HcOshaContractService();
            HC_OSHA_CONTRACT dto = service.FindByEstimateId(base.SelectedEstimate.ID);
            if (dto != null)
            {
                string text1 = "";
                if (dto.ISWEM == "Y")
                {
                    text1 = "작업환경측정 대상 " + dto.ISWEMDATA + " 에서 측정" + "\r\n";
                }
                if (dto.ISCOMMITTEE == "Y")
                {
                    text1 += "산업안전보건위원회 설치 " + "\r\n";
                }
               

                if (dto.ISSKELETON == "Y")
                {
                    text1 += "근골격계 유해요인조사 대상 실시일 " + dto.ISSKELETONDATE + "\r\n";
                }

                if (dto.ISSPACEPROGRAM == "Y")
                {
                    text1 += "밀폐공간보건프로그램 대상 실시일 " + dto.ISSPACEPROGRAMDATE + "\r\n";
                }
                if (dto.ISEARPROGRAM == "Y")
                {
                    text1 += "청력보존프로그램 대상 실시일 " + dto.ISEARPROGRAMDATE + "\r\n";
                }
                if (dto.ISSTRESS == "Y")
                {
                    text1 += "직무스트레스평가 대상 실시일 " + dto.ISSTRESSDATE + "\r\n";
                }
                if (dto.ISBRAINTEST == "Y")
                {
                    text1 += "직무스트레스평가 대상 실시일 " + dto.ISBRAINTESTDATE + "\r\n";
                }
                if (dto.ISSPECIAL == "Y")
                {
                    text1 += "특별관리물질 취급 " + dto.ISSPECIALDATA + "\r\n";
                }
               
                int selectionIndex = TxtTASKDIAGRAM.SelectionStart;
                TxtTASKDIAGRAM.Text = TxtTASKDIAGRAM.Text.Insert(selectionIndex, text1);
            }
        }

        private void BtnGet_Click(object sender, EventArgs e)
        {
            List<HC_OSHA_CARD4_3> list = hcOshaCard4_3Service.FindAll(base.SelectedEstimate.ID);
            if (list.Count > 0)
            {
                string text = "\n 관리감독자";
                foreach (HC_OSHA_CARD4_3 model in list)
                {
                    text += "\n 성명:" + model.NAME + " 선임일자:" + model.DECLAREDATE + " 직책:" + model.GRADE + " 공정(업무명):" + model.TASKNAME + " 교육과정명:" + model.EDUNAME + " 교육기간:" + model.EDUDATE + "\r\n";
                }

                //   SSCard.ActiveSheet.Cells[2, 0].Value = SSCard.ActiveSheet.Cells[2, 0].Value + text;
                //    SSCard.ActiveSheet.Rows[2].Height = SSCard.ActiveSheet.Rows[2].GetPreferredHeight();

                int selectionIndex = TxtTASKDIAGRAM.SelectionStart;
                TxtTASKDIAGRAM.Text = TxtTASKDIAGRAM.Text.Insert(selectionIndex, text);
            }
        }
    }
}
