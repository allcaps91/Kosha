using System;
using System.Drawing;
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;

namespace ComSupLibB.SupDrst
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupDrst 
    /// File Name       : clsComSupDrstSpd.cs
    /// Description     : 진료지원 공통 약제 스프레드 표시관련 class
    /// Author          : 윤조연
    /// Create Date     : 2018-11-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>    
    public class clsComSupDrstSpd
    {

        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();

        #region //조제DUR 전송 enum, 배열변수
        

        #region //조제DUR 전송 - 대상자 명단
        public enum enmDurSend01
        {
            chk01, AtcDate, AtcTime, Gubun,TuyakNo
            ,Pano,SName,Bi,DeptCode,DrCode
            ,DrName,BDate,Send,STS ,ROWID
        }

        
        public string[] sSpdDurSend01 = { "선택", "조제일자", "조제시간","구분","투약번호"
                                             ,"등록번호","환자성명","자격","과","의사코드"
                                             ,"의사명","처방일자","SEND","STS","ROWID"
                                             };

        
        public int[] nSpdDurSend01 = { 30,90,60,50,60
                                      ,80,60,30,30,50
                                      ,80,80,30,30,80
                                      };

        
        public void sSpd_DurSend01(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };


            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmDurSend01)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 12;
            spd.HorizontalScrollBarHeight = 12;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmDurSend01.chk01, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmDurSend01.ChkName, clsSpread.enmSpdType.ComboBox);
            //methodSpd.setColStyle(spd, -1, (int)enmDurSend01.ChkName, clsSpread.enmSpdType.ComboBox,s);

            spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0).CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            spd.ActiveSheet.ColumnHeader.Cells[0, 0].HorizontalAlignment = CellHorizontalAlignment.Center;
            spd.ActiveSheet.ColumnHeader.Cells[0, 0].VerticalAlignment = CellVerticalAlignment.Center;

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C); ;
            //methodSpd.setColAlign(spd, (int)enmDurSend01.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmDurSend01.result, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든
            methodSpd.setColStyle(spd, -1, (int)enmDurSend01.DrCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmDurSend01.ROWID, clsSpread.enmSpdType.Hide);


            // 5. sort, filter
            //methodSpd.setSpdFilter(spd, (int)enmDurSend01.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdSort(spd, (int)enmDurSend01.DrName, true);
            methodSpd.setSpdFilter(spd, (int)enmDurSend01.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "종검당일상담", false);
            //unary.BackColor = Color.BlueViolet;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayViewList.STS00, unary);




        }

        #endregion

        #region //조제DUR 전송 - 약상세내역
        public enum enmDurSend02
        {
            Bun, SuCode, SuName, sCode, Qty
            , Div, Nal, Bi, DosCode, SuCode2
            ,  ROWID
        }


        public string[] sSpdDurSend02 = { "분류유형", "약품코드","약품명칭", "성분코드","Qty"
                                             ,"Div","Nal","자격","용법","약코드"
                                             ,"ROWID"
                                             };


        public int[] nSpdDurSend02 = { 30,50,100,50,30                                      
                                      ,30,30,30,100,60
                                      ,80
                                      };


        public void sSpd_DurSend02(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };


            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmDurSend02)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 12;
            spd.HorizontalScrollBarHeight = 12;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmDurSend02.chk01, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmDurSend02.ChkName, clsSpread.enmSpdType.ComboBox);
            //methodSpd.setColStyle(spd, -1, (int)enmDurSend02.ChkName, clsSpread.enmSpdType.ComboBox,s);

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C); ;
            //methodSpd.setColAlign(spd, (int)enmDurSend02.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmDurSend02.result, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmDurSend02.DrCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmDurSend02.ROWID, clsSpread.enmSpdType.Hide);


            // 5. sort, filter
            //methodSpd.setSpdFilter(spd, (int)enmDurSend01.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdSort(spd, (int)enmDurSend01.DrName, true);
            //methodSpd.setSpdFilter(spd, (int)enmDurSend02.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "종검당일상담", false);
            //unary.BackColor = Color.BlueViolet;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayViewList.STS00, unary);




        }

        #endregion


        #endregion



    }
}
