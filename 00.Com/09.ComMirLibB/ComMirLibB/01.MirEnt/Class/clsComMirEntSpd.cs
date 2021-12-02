using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using ComBase; //기본 클래스
using System.Windows.Forms;

namespace ComMirLibB.MirEnt
{
    /// <summary>
    /// Class Name      : 
    /// File Name       : clsComMirLibB.MirEnt.cs
    /// Description     : 
    /// Author          : 
    /// Create Date     : 
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history> 
    public class clsComMirEntSpd
    {

        clsSpread methodSpd = new clsSpread();
        

        public enum enmSpdType
        {
            /// <summary>데이트형</summary>
            Date,
            /// <summary>시간</summary>
            Time,
            /// <summary>일자 시간</summary>
            Date_Time,
            /// <summary>버튼형태셀 설정</summary>
            Button,
            /// <summary>체크버튼형태셀 설정</summary>
            CheckBox,
            /// <summary>콤버버튼형태셀 설정</summary>
            ComboBox,
            /// <summary>쓸수있는 형태셀 설정</summary>
            Text,
            /// <summary>레이블형</summary>
            Label,
            /// <summary>넘버형</summary>
            number,
            /// <summary>보이지 않는셀 설정</summary>
            Hide,
            /// <summary>보이는 셀 설정</summary>
            View,
            /// <summary>보이는 셀 설정</summary>
            IMAGE
        }

        public enum enmSpdCellHorizontalAlignment
        {
            Left = 1,
            Center = 2,
            Right = 3,
        }

        public enum enmSpdCellVerticalAlignment
        {
            Top = 1,
            Center = 2, 
            Bottom = 3,
        }

        #region //청구 Main Dtl List   enum, 배열변수 - frmMirEntMain.cs
        /// <summary> 청구 Main List enm </summary>        
        public enum enmMirEntMainMirInsDtl
        {
            A0, SeqNo1, SeqNo2, ItemGubun, Bun, GbGisul, GbChild, UpCheck, Check, Sunext, Sunamek,                     //1
            CBODRBUNHO, Price, Qty, Nal, Div, GbNgt, GbSelf, KTASLEVL, A19, A20,                                             //2
            GbChild_OLD, A22, A23, GBSUGBS, A25, A26, A27, XrayRead, Amt, DrugAmt, // Memos, S, GbChild2, B, V,       //3
            WonSayu, Remark, Memos, FrDate, ToDate, A35, A36, EdiSeq, EdiHang, EdiMok,HU, EdiCode, EdiPrice, EdiQty,                               //4
            EdiNal, EdiAmt, GBPOWDER, GBASADD, GBGSADD, EdiDRUGAmt, OLDPrice, OLDQty, OLDNal, OLDAMT, WRTNOS, Rowid, DIVQTY,                      //5
            EDIDIVQTY, EDIDIV, SCODESAYU, SCODEREMARK, ILLCODE, OLDGbSelf, Samt, DRAUTO, OLDGBSUGBS, GBSAKDTL,     //6
            SugbAA, KTASLEVL_OLD, ER_Base, SugbB, SUGBAC, GBSPC, AMT2, DRBUNHO,GBDRG, DRGSANJUNG, DRGCHECK          //11
        }
        //청구 Main List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntMainMirInsDtl =
            { "","SeqNo1","SeqNo2","ItemGubun","Bun","GbGisul","GbChild","UpCheck","선","Code","분류(품목명)",                  //1 
                                                       "의사", "단가","수량","날","DIV","야공","S","ER","S","T",                                                         //2
                                                       "GbChildOLD","V","W","선별","Y","Z","AA","실판독", "금액","약제상한",                                                      //3
                                                       "원외","참고사항", "확인코드","시작일자","종료일자","코드","변경사유","줄","H","M","HU","표준코드","EDI단가","EDIQty",               //4
                                                       "EDI날수","EDI금액","P","ASA","외가가산", "EDI약상한","OLD단가","OLD수량","OLD날수","OLD금액","WRTNOS","ROWID","1회투여",              //5
                                                       "EDI 1회투여", "EDI횟수","동일성분","동일성분기타", "상병","OLD S항", "상한가","DRAUTO","OLDGbSugbS","삭감조정",//6
                                                       "SugbAA","KTASLVLOLD","ER_기본단","SugbB","SugbAC", "선택진료여부", "선택진료비" , "의사","DRG비급여","DRG산정내역","DRG계산"};                                                   //7 

        /// <summary> 청구 Main List 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntMainMirInsDtl = {30, 30, 30, 30, 30, 30, 30, 30, 20, 60,300,   //1
                                                    60, 60, 40, 30, 30, 30, 30, 30, 30, 30,   //2
                                                    30, 40, 40, 30, 30, 30, 30, 30, 60, 60,   //3
                                                    30, 100, 100, 90, 90, 30, 60, 30, 30, 30, 30, 90, 60, 60, //4
                                                    60, 60, 20, 40, 90, 90, 60, 60, 60, 60, 60, 90, 90,   //5
                                                    90, 90, 90, 90, 90, 90, 90, 90, 90, 90,   //6
                                                    90, 90, 90, 90, 60, 90, 90, 90, 80, 30, 10};          //7


        /// <summary> 청구 Main List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntMainMirInsDtl(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt) 
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntMainMirInsDtl)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.ColumnHeader.Rows[0].Height = 15;
            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 20; 
            //spd.ActiveSheet.Columns[-1].Height= 100;
            spd.ActiveSheet.Rows[-1].Height = 9;


            spd.ActiveSheet.RowHeader.Visible = false;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            setColStyle_Label(spd, -1, -1, true, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.Check, clsSpread.enmSpdType.CheckBox);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.GBPOWDER, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)clsMirEntSpd.enmMirEntMainMirInsDtl.GbNgt, clsSpread.enmSpdType.Text);
            setColStyle_Text(spd, -1, (int)enmMirEntMainMirInsDtl.Sunext, 8, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);

            //spd.ActiveSheet.Rows[(int)clsMirEntSpd.enmMirEntMainMirInsDtl.Sunamek].Font = new Font("굴림", 9, FontStyle.u.upper);

            setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.Price, 0, 99999999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            //setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.Qty, 2, 999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.Qty, 2, 999, -999, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.Nal, 0, 999, -999, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.Div, 0, 9, 0, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);
            setColStyle_Text(spd, -1, (int)enmMirEntMainMirInsDtl.GbNgt, 1, false, CellHorizontalAlignment.Center, CellVerticalAlignment.Center, CharacterCasing.Normal);
            
            //setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.GbNgt, 0, 6, 0, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);

            setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.GbSelf, 0, 2, 0, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);

            setColStyle_Text(spd, -1, (int)enmMirEntMainMirInsDtl.GBSUGBS, 1, false, CellHorizontalAlignment.Center, CellVerticalAlignment.Center, CharacterCasing.Normal);

            //setColStyle_Label(spd, -1, (int)clsMirEntSpd.enmMirEntMainMirInsDtl.Sunamek, true, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);
            setColStyle_Text(spd, -1, (int)enmMirEntMainMirInsDtl.Sunamek, 40, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);
            setColStyle_Text(spd, -1, (int)enmMirEntMainMirInsDtl.Remark, 50, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Normal);
            setColStyle_Text(spd, -1, (int)enmMirEntMainMirInsDtl.Memos, 200, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Normal);

            setColStyle_Text(spd, -1, (int)enmMirEntMainMirInsDtl.WonSayu, 2, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Normal);

            setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.Amt, 0, 99999999, -99999999, CellHorizontalAlignment.Right, CellVerticalAlignment.Center, true);


            setColStyle_Text(spd, -1, (int)enmMirEntMainMirInsDtl.KTASLEVL, 1, false, CellHorizontalAlignment.Center, CellVerticalAlignment.Center, CharacterCasing.Normal);
            //setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.KTASLEVL, 0, 6, 0, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);

            setColStyle_Date(spd, -1, (int)enmMirEntMainMirInsDtl.FrDate, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);
            setColStyle_Date(spd, -1, (int)enmMirEntMainMirInsDtl.ToDate, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);

            setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.EdiQty, 2, 99999999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center, true);


            setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.OLDPrice, 0, 99999999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.OLDQty, 2, 999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.OLDNal, 0, 999, 0, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);
            setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.OLDAMT, 0, 99999999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center, true);

            setColStyle_Text(spd, -1, (int)enmMirEntMainMirInsDtl.GBGSADD, 1, false, CellHorizontalAlignment.Center, CellVerticalAlignment.Center, CharacterCasing.Normal);
            

            setColStyle_Text(spd, -1, (int)enmMirEntMainMirInsDtl.DRBUNHO, 40, false, CellHorizontalAlignment.Center, CellVerticalAlignment.Center, CharacterCasing.Normal);

            //setColStyle_Text(spd, -1, (int)clsMirEntSpd.enmMirEntMainMirInsDtl.CBODRBUNHO, 40, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);

            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A0, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.SeqNo1, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.SeqNo2, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.ItemGubun, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.Bun, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.GbGisul, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.GbChild, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.UpCheck, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.Samt, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.DrugAmt, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A19, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A20, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.GbChild_OLD, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A22, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A23, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A25, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A26, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A27, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.OLDPrice, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.OLDQty, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.OLDNal, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.OLDAMT, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.OLDQty, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.WRTNOS, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.Rowid, clsSpread.enmSpdType.Hide);
            if(!(clsType.User.Sabun == "15273" || clsType.User.Sabun == "45316"))
            {
                methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.Hide);  //실판독
            }
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A35, clsSpread.enmSpdType.Hide);       //변경코드
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A36, clsSpread.enmSpdType.Hide);       //변경사유
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.HU, clsSpread.enmSpdType.Hide); //호스피스 숨기기

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 Main 환자명단 List   enum, 배열변수 - frmMirEntMain.cs
        /// <summary> 청구 Main 환자명단 List enm </summary>        
        public enum enmMirEntMainMirPanoList
        {
            A0, Pano, SName, DEPTCODE, WRTNO
        }
        //청구 Main 환자명단 List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntMainMirPanoList =
            { "","등록번호","성명", "진료과", "WRTNO" };                                           //7 

        /// <summary> 청구 Main 환자명단 List 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntMainMirPanoList = { 30, 80, 80, 80, 30 };                 //7


        /// <summary> 청구 Main 환자명단 List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntMainMirPanoList(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntMainMirPanoList)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            spd.ActiveSheet.RowHeader.Visible = false;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            setColStyle_Label(spd, -1, -1, true, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.Check, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);
            ////methodSpd.setColStyle(spd, -1, (int)clsMirEntSpd.enmMirEntMainMirInsDtl.GbNgt, clsSpread.enmSpdType.Text);
            //setColStyle_Text(spd, -1, (int)clsMirEntSpd.enmMirEntMainMirInsDtl.Sunext, 8, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);

            ////spd.ActiveSheet.Rows[(int)clsMirEntSpd.enmMirEntMainMirInsDtl.Sunamek].Font = new Font("굴림", 9, FontStyle.u.upper);

            ////A0,SeqNo1, SeqNo2, ItemGubun, Bun, GbGisul, GbChild, UpCheck, Check, Sunext, Sunamek,                        //1
            ////    Samt, Price, Qty, Nal, Div, GbNgt, GbSelf, KTASLEVL, A19, A20,                                           //2
            ////    GbChild_OLD, A22, A23, GBSUGBS, A25, A26, A27, XrayRead, Amt, DrugAmt, // Memos, S, GbChild2, B, V,               //3
            ////    WonSayu, Remark, Memos, FrDate, A35, A36,EdiSeq, EdiCode, EdiPrice, EdiQty,                               //4
            ////    EdiNal, EdiAmt, EdiDRUGAmt, OLDPrice, OLDQty, OLDNal, OLDAMT, WRTNOS, Rowid, DIVQTY,                      //5
            ////    EDIDIVQTY, EDIDIV, SCODESAYU, SCODEREMARK, ILLCODE, OLDGbSelf, DRBUNHO, DRAUTO, OLDGBSUGBS, GBSAKDTL,     //6
            ////    SugbAA, KTASLEVL_OLD, ER_Base, SugbB, SUGBAC, GBGSADD                                                                                   //7

            //setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.Price, 0, 99999999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            ////setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.Qty, 2, 999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            //setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.Qty, 2, 999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            //setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.Nal, 0, 999, 0, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);
            //setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.Div, 0, 1, 0, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);
            //setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.GbNgt, 0, 3, 0, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);
            //setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.GbSelf, 0, 1, 0, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);

            ////setColStyle_Label(spd, -1, (int)clsMirEntSpd.enmMirEntMainMirInsDtl.Sunamek, true, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);
            //setColStyle_Text(spd, -1, (int)clsMirEntSpd.enmMirEntMainMirInsDtl.Sunamek, 40, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);
            //setColStyle_Text(spd, -1, (int)clsMirEntSpd.enmMirEntMainMirInsDtl.Remark, 50, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Normal);
            //setColStyle_Text(spd, -1, (int)clsMirEntSpd.enmMirEntMainMirInsDtl.Memos, 200, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Normal);
            //setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.Amt, 0, 99999999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center, true);

            //setColStyle_Date(spd, -1, (int)clsMirEntSpd.enmMirEntMainMirInsDtl.FrDate, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);

            //setColStyle_number(spd, -1, (int)enmMirEntMainMirInsDtl.EdiQty, 2, 99999999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center, true);

            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirPanoList.A0, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirPanoList.WRTNO, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.SeqNo2, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.ItemGubun, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.Bun, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.GbGisul, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.GbChild, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.UpCheck, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.Samt, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A19, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A20, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.GbChild_OLD, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A23, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A25, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A26, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.A27, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.OLDPrice, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.OLDQty, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.OLDNal, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.OLDAMT, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.OLDQty, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.WRTNOS, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.Rowid, clsSpread.enmSpdType.Hide);


            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 Main OutDrug List   enum, 배열변수 - frmMirEntMain.cs
        /// <summary> 청구 원외처방 List enm </summary>        
        public enum enmMirEntMainMirOutDrug
        {
            A0, A1, SuCode, Qty, DivQty, Div, Nal, SuNamek, EdiCode,                                                   //1
            PName, DaiCode, ClassName, GbSelf,GbSugbs, Multi, MultiRemark, ScodeSayu, ScodeRemark, Rowid, WrtnoS, SlipDate,    //2
            GBV252, GBV352, V100, GbSakDtl                                                                             //3
        }

        //청구 Main List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntMainMirOutDrug =
            { "", "원외처방번호","수가코드","수량","1회투여","#","날수","수가명칭","표준코드",                                                            //1
          "표준코드명","약품분류","약품분류명", "S" ,"선별","저함량사유코드", "저함량사유(참고사항)", "동일성분코드", "동일성분(참고)","ROWID","WRTNOS","처방일자번호",  //2
          "GBV252","GBV352","V100", "원외삭감내역(6개월전)"  };                                                                                      //3

        /// <summary> 청구 Main List 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntMainMirOutDrug = {   30, 100,  60, 30,  60,  30, 30,  200, 60,        //1
                                                    200,  30, 100, 30, 30, 100, 200, 80, 200,  30, 100, 90, //2
                                                     60, 60, 60, 200                                    //3
                                               }; 


        /// <summary> 청구 Main List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntMainMirOutDrug(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntMainMirOutDrug)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            spd.ActiveSheet.RowHeader.Visible = false;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            setColStyle_Label(spd, -1, (int)enmMirEntMainMirOutDrug.SuNamek, true, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);
            setColStyle_Label(spd, -1, (int)enmMirEntMainMirOutDrug.PName, true, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);
            setColStyle_Label(spd, -1, (int)enmMirEntMainMirOutDrug.ClassName, true, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            setColStyle_Text(spd, -1, (int)enmMirEntMainMirOutDrug.Multi, 1, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);
            setColStyle_Text(spd, -1, (int)enmMirEntMainMirOutDrug.ScodeSayu, 1, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper); 

            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.Multi, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);


            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.GbSak, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.A0, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.Rowid, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.WrtnoS, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 Main Amt List   enum, 배열변수 - frmMirEntMain.cs
        /// <summary> 청구 Amt List enm </summary>        
        public enum enmMirEntMainMirAmt
        {
            A01, A02, A03, A04, A05
        }

        //청구 Main Amt 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntMainMirAmt =
            { "Check","구분","Qty","재료대","행위료" };                                                                                                        //3

        /// <summary> 청구 Main Amt 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntMainMirAmt = { 10, 100, 30, 90, 90 };


        /// <summary> 청구 Main List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntMainMirAmt(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntMainMirAmt)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 20;
            //spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;

            spd.ActiveSheet.RowHeader.Visible = false;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirAmt.A03, clsSpread.enmSpdType.number);

            setColStyle_Label(spd, -1, (int)enmMirEntMainMirAmt.A04, true, false, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            setColStyle_Label(spd, -1, (int)enmMirEntMainMirAmt.A05, true, false, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);


            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.Check, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);


            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirAmt.A01, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.Rowid, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.WrtnoS, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 Main DRG Amt List   enum, 배열변수 - frmMirEntMain.cs
        /// <summary> 청구 Amt List enm </summary>        
        public enum enmMirEntMainMirDRGAmt
        {
            A01, A02, A03, A04, A05
        }

        //청구 Main Amt 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntMainMirDRGAmt =
            { "Check","구분","Qty","재료대","행위료" };                                                                                                        //3

        /// <summary> 청구 Main Amt 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntMainMirDRGAmt = { 30, 100, 30, 90, 90 };


        /// <summary> 청구 Main List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntMainMirDRGAmt(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntMainMirDRGAmt)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 20;


            spd.ActiveSheet.RowHeader.Visible = false;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            setColStyle_Label(spd, -1, (int)enmMirEntMainMirDRGAmt.A04, true, false, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            setColStyle_Label(spd, -1, (int)enmMirEntMainMirDRGAmt.A05, true, false, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.Check, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);


            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirDRGAmt.A01, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.Rowid, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.WrtnoS, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 Main ILLS List   enum, 배열변수 - frmMirEntMain.cs
        /// <summary> 청구 원외처방 List enm </summary>        
        public enum enmMirEntMainMirILLS
        {
            A0, A1, A2, A3, A4, A5, A6, A7
        }

        //청구 Main List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntMainMirILLS =
            { "", "O","진료과목","No","특진상해","개시일","일수","진료결과" };                                                                                                        //3

        /// <summary> 청구 Main List 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntMainMirILLS = { 20, 20, 70, 30, 60, 60, 30, 60 };


        /// <summary> 청구 Main List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntMainMirILLS(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntMainMirILLS)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 20;

            spd.ActiveSheet.RowHeader.Visible = false;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLS.A1, clsSpread.enmSpdType.CheckBox);
            setColStyle_Text(spd, -1, (int)enmMirEntMainMirILLS.A2, 40, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);


            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLS.A0, clsSpread.enmSpdType.Hide);
            setColStyle_Text(spd, -1, (int)enmMirEntMainMirILLS.A2, 40, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);
            setColStyle_Text(spd, -1, (int)enmMirEntMainMirILLS.A4, 4, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);
            setColStyle_Text(spd, -1, (int)enmMirEntMainMirILLS.A5, 8, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);

            setColStyle_number(spd, -1, (int)enmMirEntMainMirILLS.A6, 0, 999, -1, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            //setColStyle_number(spd, -1, (int)enmMirEntMainMirILLS.Qty, 2, 999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.Rowid, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.WrtnoS, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 Main ILLSD List   enum, 배열변수 - frmMirEntMain.cs
        /// <summary> 청구 원외처방 List enm </summary>        
        public enum enmMirEntMainMirILLSD
        {
            A0, A1, A2
        }

        //청구 Main List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntMainMirILLSD =
            { "", "상병", "상병명" };                                                                                                        //3

        /// <summary> 청구 Main List 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntMainMirILLSD = { 30, 60, 300 };


        /// <summary> 청구 Main List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntMainMirILLSD(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntMainMirILLSD)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 20;

            spd.ActiveSheet.RowHeader.Visible = false;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLS.A1, clsSpread.enmSpdType.CheckBox);
            //setColStyle_Text(spd, -1, (int)clsMirEntSpd.enmMirEntMainMirILLS.A2, 40, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);


            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_L, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);    


            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLSD.A0, clsSpread.enmSpdType.Hide);
            //setColStyle_Text(spd, -1, (int)enmMirEntMainMirILLS.A2, 40, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);
            //setColStyle_Text(spd, -1, (int)enmMirEntMainMirILLS.A4, 4, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);
            //setColStyle_Text(spd, -1, (int)enmMirEntMainMirILLS.A5, 8, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);

            //setColStyle_number(spd, -1, (int)enmMirEntMainMirILLS.A6, 0, 999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            //setColStyle_number(spd, -1, (int)enmMirEntMainMirILLS.Qty, 2, 999, 0, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.Rowid, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.WrtnoS, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 PaceChoice ss List   enum, 배열변수 - frmMirEntPanoChoice.cs
        /// <summary> 청구 환자선택 List enm </summary>        
        public enum enmMirEntPanoChoice
        {
            A0, EdiMirno, UpCnt1, BohoJong, Seqno, Scode, StopFlag, Wrtno, Dtno, Deptcode1, JinDate1, RateBon, Kiho, GKiho, blank, Wrtno2, Tamt, Jamt, Bamt
        }

        //청구 환자선택 List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntPanoChoice =
            { " ", "EDI여부","보류","청구구분","일련번호","상해외인","변경","청구번호" , "청구분야", "진료과", "진료개시일", "본인부담", "조합코드", "증번호", " ", "청구번호", "총진료비", "조합부담금", "본인부담금" };

        /// <summary> 청구 환자선택 List 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntPanoChoice = { 10, 30, 30, 40, 70, 40, 30, 80, 80, 80, 80, 50, 90, 90, 5, 80, 80, 80, 80 };


        /// <summary> 청구 환자선택 List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntPanoChoice(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntPanoChoice)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 40;

            spd.ActiveSheet.RowHeader.Visible = false;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;


            //0.OperationMode
            //spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            //spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            //methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            setColStyle_Label(spd, -1, -1, true, false, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLS.A1, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);


            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            //methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntPanoChoice.A0, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.Rowid, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.WrtnoS, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 GbnEntrye ssList   enum, 배열변수 - frmMirEntGbnEntry.cs
        /// <summary> 청구 구분등록 재청구 ,추가청구, 중간청구  List enm </summary>        
        public enum enmMirEntGbnEntry
        {
            A0, YYMM, BI, DEPT, IODATE, SEQNO, JEPNO, CHASU, MUKNO, JEPDATE
        }

        //청구 청구구분등록 List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntGbnEntry =
            { " ", "년월","종류","과","진료기간","일련번호","접수번호","접수차수" , "묶음번호", "접수일자"};

        /// <summary> 청구 청구구분등록 List 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntGbnEntry = { 10, 50, 50, 50, 190, 80, 120, 80, 80, 80 };


        /// <summary> 청구 청구구분등록 List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntGbnEntry(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntGbnEntry)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 40;

            spd.ActiveSheet.RowHeader.Visible = false;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;


            //0.OperationMode
            //spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLS.A1, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);


            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntPanoChoice.A0, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.Rowid, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.WrtnoS, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 환자목록 ssList   enum, 배열변수 - frmMirEntPanoList.cs
        /// <summary> 청구 환자목록  List enm </summary>        
        public enum enmMirEntPanoList  
        {                                //OUTDATE
            A0, GBNEDI, GBN, GBNJIN, PANO, SNAME, JINDATE1, STOPFLAG, SEQNO, EDITAMT, ILLCODE1,ILLCODE2, VCODE, DRCODE, DEPTCODE1, JINILSU, JUMIN1, JUMIN2, BOHOJONG, UPCNT1, MIROK, WEEK, JEPNO, AMT, JOBSABUN, JOBDATE, WRTNO, SNO, DRGCODE,GBHU,GBCP,GBBI,GBGS,BUILDTIME
        }

        //청구 환자목록 List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntPanoList =
            //                                       "퇴원완료일"
            { " ", "청구구분","산재구분","산재진료구분","등록번호", "수진자명","진료개시일","변경", "Seqno","총진료비","상병1","상병2","VCODE","의사","진료과","내원일수",  "생년월일","성" , "구분", "보류", "청구완성", "차수", "접수번호","일당진료비", "심사자", "최종심사일시", "청구번호",  "시설번호",   "DRG", "HU","CP","자격","차상위", "빌드시간"};

        /// <summary> 청구 환자목록 List 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntPanoList = { 10, 60, 60, 60, 60, 60, 70, 40, 60, 80, 60, 60, 50, 50, 50, 40, 60, 10, 40, 40, 40, 60, 60, 80, 50, 60, 60, 60, 60, 30,30,30,30, 180 };


        /// <summary> 청구 환자목록 List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntPanoList(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)  
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntGbnEntry)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 40;

            spd.ActiveSheet.RowHeader.Visible = true;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;


            //0.OperationMode
            //spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;
            spd.ActiveSheet.Rows.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            setColStyle_number(spd, -1, (int)enmMirEntPanoList.EDITAMT, 0, 99999999, -99999999, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);
            setColStyle_number(spd, -1, (int)enmMirEntPanoList.AMT, 0, 99999999, -99999999, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);

            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLS.A1, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);


            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntPanoList.A0, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntPanoList.DRGCODE, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.Rowid, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.WrtnoS, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(spd, (int)enmMirEntPanoList.WEEK, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(spd, (int)enmMirEntPanoList.GBBI, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(spd, (int)enmMirEntPanoList.GBGS, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(spd, (int)enmMirEntPanoList.GBCP, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        } 

        #endregion

        #region //청구 환경설정 ss1   enum, 배열변수 - frmMirEntFlagSet.cs
        /// <summary> 청구 환경설정 수가  List enm </summary>  
        /// 

        public enum enmMirEntFlagSet1
        {
            A0, Del, SuNext, BackColor, Memo, ROWID, RGB
        }

        //청구 환경설정 수가 List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntFlagSet1 =
            //                                       
            { " ","삭제", "수가","배경색","참고","ROWID","RGB" };

        /// <summary> 청구 환경설정 수가List 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntFlagSet1 = { 10, 40, 80, 100, 300, 40, 40 };


        /// <summary> 청구 환경설정 수가 List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntFlagSet1(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntGbnEntry)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 40;

            spd.ActiveSheet.RowHeader.Visible = false;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;


            //0.OperationMode
            //spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            // spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet1.Del, clsSpread.enmSpdType.CheckBox);
            setColStyle_Text(spd, -1, (int)enmMirEntFlagSet1.SuNext, 8, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Upper);
            setColStyle_Text(spd, -1, (int)enmMirEntFlagSet1.Memo, 200, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Normal);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLS.A1, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);


            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet1.A0, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet1.ROWID, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet1.RGB, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 

            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, 10, false);
            //unary.Operator = FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo;
            //unary.Value = 10;
            //unary.BackColor = Color.Red;
            //unary.FontStyle = new FarPoint.Win.Spread.SpreadFontStyle(RegularBoldItalicFontStyle.Bold);
            //fpSpread1.ActiveSheet.SetConditionalFormatting(1, 1, unary);

            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, true, false);
            ////unary.BackColor = Color.LightGreen;
            //unary.Value = true;
            //unary.BackColor = Color.Red;
            //unary.ForeColor = Color.Red;
            ////spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmMirEntFlagSet1.Del, unary); //체크
            //spd.ActiveSheet.SetConditionalFormatting(0, 0, spd.ActiveSheet.RowCount, spd.ActiveSheet.ColumnCount ,unary); //체크

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 환경설정 ss2   enum, 배열변수 - frmMirEntFlagSet.cs
        /// <summary> 청구 환경설정 확인코드  List enm </summary>  
        /// 

        public enum enmMirEntFlagSet2
        {
            A0, Del, Memo, ROWID
        }

        //청구 환경설정 확인코드 List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntFlagSet2 =
            //                                       
            { " ","삭제", "확인코드","ROWID" };

        /// <summary> 청구 환경설정 확인코드 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntFlagSet2 = { 10, 40, 300, 60 };


        /// <summary> 청구 환경설정 확인코드 List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntFlagSet2(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntGbnEntry)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 40;

            spd.ActiveSheet.RowHeader.Visible = false;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;


            //0.OperationMode
            //spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            //spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet2.Del, clsSpread.enmSpdType.CheckBox);
            setColStyle_Text(spd, -1, (int)enmMirEntFlagSet2.Memo, 200, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Normal);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLS.A1, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);


            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet2.A0, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet2.ROWID, clsSpread.enmSpdType.Hide);

            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.Rowid, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.WrtnoS, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 환경설정 ss3  enum, 배열변수 - frmMirEntFlagSet.cs
        /// <summary> 청구 환경설정 참고사항 줄단위  List enm </summary>  
        /// 

        public enum enmMirEntFlagSet3
        {
            A0, Del, Memo, No, ROWID
        }

        //청구 환경설정 참고사항 줄단위 List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntFlagSet3 =
            //                                       
            { " ","삭제", "참고사항(줄단위)","순서", "ROWID" };

        /// <summary> 청구 환경설정 참고사항 줄단위 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntFlagSet3 = { 10, 40, 400, 20, 60 };


        /// <summary> 청구 환경설정 참고사항 줄단위 List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntFlagSet3(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntGbnEntry)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 40;

            spd.ActiveSheet.RowHeader.Visible = false;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;


            //0.OperationMode
            //spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            //spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet3.Del, clsSpread.enmSpdType.CheckBox);
            setColStyle_Text(spd, -1, (int)enmMirEntFlagSet3.Memo, 200, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Normal);

            //setColStyle_number(spd, -1, (int)enmMirEntFlagSet3.No, 0, 3, 0, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLS.A1, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);


            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet3.A0, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet3.ROWID, clsSpread.enmSpdType.Hide);

            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.Rowid, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.WrtnoS, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 환경설정 ss4  enum, 배열변수 - frmMirEntFlagSet.cs
        /// <summary> 청구 환경설정 참고사항 명세서단위  List enm </summary>  
        /// 

        public enum enmMirEntFlagSet4
        {
            A0, Del, Memo, ROWID
        }

        //청구 환경설정  참고사항 명세서단위 List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntFlagSet4 =
            //                                       
            { " ","삭제", "참고사항(명세서단위)","ROWID" };

        /// <summary> 청구 환경설정 참고사항 명세서단위 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntFlagSet4 = { 10, 40, 500, 60 };


        /// <summary> 청구 환경설정 참고사항 줄단위 List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntFlagSet4(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntGbnEntry)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 40;

            spd.ActiveSheet.RowHeader.Visible = false;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;


            //0.OperationMode
            //spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            // spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet4.Del, clsSpread.enmSpdType.CheckBox);
            setColStyle_Text(spd, -1, (int)enmMirEntFlagSet4.Memo, 200, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Normal);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLS.A1, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);


            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet4.A0, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet4.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 수술 개인재료 등록/삭제 ssList  enum, 배열변수 - frmMirEntOpGuip.cs
        /// <summary> 청구 수술 개인재료 등록/삭제 List enm </summary>  
        /// 

        public enum enmMirEntOpGuip
        {
            A0, GDate, Bcode, Qty, Price, Amt, Name, PName, Spec
        }

        //청구 수술 개인재료 등록/삭제 List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntOpGuip =
            //                                       
            { " ","구입일자", "표준코드", "수량", "단가", "금액", "구입처", "표준코드 명칭" ,"명칭"};

        /// <summary> 청구 수술 개인재료 등록/삭제 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntOpGuip = { 10, 60, 60, 40, 60, 80, 80, 300, 20 };


        /// <summary> 청구 수술 개인재료 등록/삭제 List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntOpGuip(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntOpGuip)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 40;
            spd.ActiveSheet.RowHeader.Visible = false;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;
            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //0.OperationMode
            //spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            // spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet4.Del, clsSpread.enmSpdType.CheckBox);
            //setColStyle_Text(spd, -1, (int)clsMirEntSpd.enmMirEntFlagSet4.Memo, 200, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Normal);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLS.A1, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntOpGuip.A0, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.Rowid, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.WrtnoS, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 EDI 청구 내역 ssList  enum, 배열변수 - frmMirEntEdiView.cs
        /// <summary> 청구 EDI 청구 내역 List enm </summary>  
        /// 

        public enum enmMirEntEdiView
        {
            A0, SuNext, Qty, Nal, Price, EdiPrice, Amt, EdiAmt, EdiCode, EdiQty, EdiNal, SuNameK, Pname
        }

        //청구 EDI 청구 내역 List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntEdiView =
            //                                       
            { " ","수가코드", "Qty", "Nal", "청구단가", "표준단가", "청구금액", "표준금액" ,"표준코드","EdiQty", "EdiNal", "수가명칭","표준명칭"};

        /// <summary> 청구 EDI 청구 내역 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntEdiView = { 10, 60, 30, 30, 60, 60, 60, 60, 60, 30, 30, 100, 100 };


        /// <summary> 청구 EDI 청구 내역 List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntEdiView(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntEdiView)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 40;
            spd.ActiveSheet.RowHeader.Visible = false;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;
            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //0.OperationMode
            //spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            // spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet4.Del, clsSpread.enmSpdType.CheckBox);
            //setColStyle_Text(spd, -1, (int)clsMirEntSpd.enmMirEntFlagSet4.Memo, 200, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Normal);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLS.A1, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntEdiView.A0, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.Rowid, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.WrtnoS, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        #region //청구 진료과에 전달사항 등록 ssList  enum, 배열변수 - frmMirEntDoctMsgEntry.cs
        /// <summary> 청구 진료과에 전달사항 등록 List enm </summary>  
        /// 

        public enum enmMirEntDoctMsgEntry
        {
            A0, YYMM, VREMARK, ENTDATE, VROWID
        }

        //청구 진료과에 전달사항 등록 List 컬럼헤드 배열 </summary>
        public string[] sSpdenmMirEntDoctMsgEntry =
        {
            " ","진료년월", "내용", "등록일", "Rowid"
        };

        /// <summary> 청구 진료과에 전달사항 등록 컬럼사이즈 배열 </summary>
        public int[] nSpdenmMirEntDoctMsgEntry = { 10, 60, 200, 90, 30 };


        /// <summary> 청구 진료과에 전달사항 등록 List 스프레드 표시   </summary>  
        public void sSpd_enmMirEntDoctMsgEntry(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirEntDoctMsgEntry)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 40;
            spd.ActiveSheet.RowHeader.Visible = false;

            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;
            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //0.OperationMode
            //spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            // spd.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntFlagSet4.Del, clsSpread.enmSpdType.CheckBox);
            //setColStyle_Text(spd, -1, (int)clsMirEntSpd.enmMirEntFlagSet4.Memo, 200, false, CellHorizontalAlignment.Left, CellVerticalAlignment.Center, CharacterCasing.Normal);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirILLS.A1, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirInsDtl.XrayRead, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsRCP01A.Infect, clsSpread.enmSpdType.IMAGE);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsRCP01A.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmMirEntDoctMsgEntry.A0, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmMirEntDoctMsgEntry.VROWID, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.WrtnoS, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMirEntMainMirOutDrug.SlipNo, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.RDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsRCP01A.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.STS01, unary); //후불

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.sName2, unary); //동명

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "기관지", false);
            //unary.BackColor = sup.ENDO_1;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "위", false);
            //unary.BackColor = sup.ENDO_2;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대장", false);
            //unary.BackColor = sup.ENDO_3;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "ERCP", false);
            //unary.BackColor = sup.ENDO_4;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Gubun, unary); //

        }

        #endregion

        public void setColStyle(FpSpread o, int nRow, int nCol, enmSpdType type, string[] strCombo = null, string strBtn = null
          , string strBtnDown = null, string chkCaption = null, bool isMulti = true)
        {
            o.ActiveSheet.Columns.Get(nCol).Locked = false;

            if (type == enmSpdType.Button)
            {
                #region Button

                ButtonCellType spdObj = new ButtonCellType();
                spdObj.Text = strBtn;
                spdObj.TextDown = strBtnDown;

                if (nRow == -1)
                {
                    o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
                }
                else
                {
                    o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
                }

                #endregion

            }
            else if (type == enmSpdType.CheckBox)
            {
                #region CheckBox

                CheckBoxCellType spdObj = new CheckBoxCellType();
                spdObj.Caption = chkCaption;

                if (nRow == -1)
                {
                    o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;

                    o.ActiveSheet.Columns[nCol].HorizontalAlignment = CellHorizontalAlignment.Center;
                    o.ActiveSheet.Columns[nCol].VerticalAlignment = CellVerticalAlignment.Center;
                }
                else
                {
                    o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;

                    o.ActiveSheet.Cells[nRow, nCol].HorizontalAlignment = CellHorizontalAlignment.Center;
                    o.ActiveSheet.Cells[nRow, nCol].VerticalAlignment = CellVerticalAlignment.Center;

                }

                #endregion

            }
            else if (type == enmSpdType.ComboBox)
            {
                #region ComboBox

                ComboBoxCellType spdObj = new ComboBoxCellType();
                spdObj.ListWidth = Convert.ToInt16(o.ActiveSheet.Columns.Get(nCol).Width);
                spdObj.Editable = true;
                if (strCombo == null || strCombo.ToString().Trim().Length == 0)
                {
                    string[] s = { "Null.없음" };
                    spdObj.Items = s;
                }
                else
                {
                    spdObj.Items = strCombo;
                }

                if (nRow == -1)
                {
                    o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
                }
                else
                {
                    o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
                }

                if (o.ActiveSheet.NonEmptyRowCount > 0 && strCombo != null)
                {
                    for (int i = 0; i < o.ActiveSheet.Rows.Count; i++)
                    {
                        if (o.ActiveSheet.Cells[i, nCol].Text.Trim().Length == 0)
                        {
                            o.ActiveSheet.Cells[i, nCol].Text = strCombo[0];
                        }
                    }
                }
                spdObj.Editable = false;
                #endregion

            }
            else if (type == enmSpdType.Text)
            {
                #region Text

                TextCellType spdObj = new TextCellType();
                spdObj.Multiline = isMulti;
                spdObj.MaxLength = 4000;

                if (nRow == -1)
                {
                    o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
                }
                else
                {
                    o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
                }

                #endregion

            }

            else if (type == enmSpdType.Date)
            {
                #region Date
                DateTimeCellType spdObj = new DateTimeCellType();
                spdObj.DateTimeFormat = DateTimeFormat.ShortDate;

                if (nRow == -1)
                {
                    o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
                }
                else
                {
                    o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
                }
                #endregion
            }

            else if (type == enmSpdType.Time)
            {
                #region Time
                DateTimeCellType spdObj = new DateTimeCellType();
                spdObj.DateTimeFormat = DateTimeFormat.TimeOnly;

                if (nRow == -1)
                {
                    o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
                }
                else
                {
                    o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
                }
                #endregion
            }
            else if (type == enmSpdType.Date_Time)
            {
                #region Time
                DateTimeCellType spdObj = new DateTimeCellType();
                spdObj.DateTimeFormat = DateTimeFormat.UserDefined;
                spdObj.UserDefinedFormat = "yyyy-MM-dd HH:mm";

                if (nRow == -1)
                {
                    o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
                }
                else
                {
                    o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
                }
                #endregion
            }

            else if (type == enmSpdType.Label)
            {
                #region Label

                o.ActiveSheet.Columns.Get(nCol).Locked = true;
                TextCellType spdObj = new TextCellType();
                spdObj.Multiline = isMulti;
                spdObj.WordWrap = true;

                if (nRow == -1)
                {
                    o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
                }
                else
                {
                    o.ActiveSheet.Cells[nRow, nCol].Locked = true;
                }

                #endregion

            }
            else if (type == enmSpdType.number)
            {
                #region number

                NumberCellType spdObj = new NumberCellType();
                //spdObj.DecimalPlaces = 0;


                if (nRow == -1)
                {

                    o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
                    o.ActiveSheet.Columns[nCol].HorizontalAlignment = CellHorizontalAlignment.Right;
                    o.ActiveSheet.Columns[nCol].VerticalAlignment = CellVerticalAlignment.Center;
                }
                else
                {
                    o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
                }

                #endregion

            }
            else if (type == enmSpdType.Hide)
            {
                #region Hide

                o.ActiveSheet.Columns.Get(nCol).Visible = false;
                return;

                #endregion
            }
            else if (type == enmSpdType.View)
            {
                #region View

                o.ActiveSheet.Columns.Get(nCol).Visible = true;
                return;

                #endregion
            }
            else if (type == enmSpdType.IMAGE)
            {
                #region IMAGE

                ImageCellType spdObj = new ImageCellType();
                spdObj.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                if (nRow == -1)
                {
                    o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
                }
                else
                {
                    o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
                }




                return;

                #endregion
            }


        }

        public void setColStyle_number(FpSpread o, int nRow, int nCol, int DecimalPlaces, double MaximumValue, double MinimumValue, CellHorizontalAlignment HA, CellVerticalAlignment VA, bool Locked = false)
        {
            o.ActiveSheet.Columns.Get(nCol).Locked = Locked;
            NumberCellType spdObj = new NumberCellType();

            //spdObj.WordWrap = true;
            if (DecimalPlaces < 0)
            {
                spdObj.DecimalPlaces = 0;
            }
            else
            {
                spdObj.DecimalPlaces = DecimalPlaces;
            }

            //spdObj.DecimalPlaces = 3;
            spdObj.DecimalSeparator = ".";

            //spdObj.FixedPoint = true;
            //spdObj.LeadingZero = FarPoint.Win.Spread.CellType.LeadingZero.UseRegional;
            spdObj.MaximumValue = MaximumValue;
            spdObj.MinimumValue = MinimumValue;
            //spdObj.NegativeFormat = FarPoint.Win.Spread.CellType.NegativeFormat.Parentheses;
            //spdObj.NegativeRed = true;
            spdObj.Separator = ",";
            spdObj.ShowSeparator = true;

            //spdObj.SpinButton = true;
            //spdObj.SpinDecimalIncrement = 10;
            //spdObj.SpinIntegerIncrement = 5;
            // spdObj.SpinWrap = true;


            //spdObj.CharacterCasing = CharacterCasing.Upper;
            o.ActiveSheet.Columns[nCol].HorizontalAlignment = HA;// CellHorizontalAlignment.Left;
            o.ActiveSheet.Columns[nCol].VerticalAlignment = VA;

            if (nRow == -1)
            {
                o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
            }
            else
            {
                o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
            }


        }

        public void setColStyle_Date(FpSpread o, int nRow, int nCol, CellHorizontalAlignment HA, CellVerticalAlignment VA)
        {
            o.ActiveSheet.Columns.Get(nCol).Locked = false;
            DateTimeCellType spdObj = new DateTimeCellType();
            spdObj.DateTimeFormat = DateTimeFormat.ShortDate;

            o.ActiveSheet.Columns[nCol].HorizontalAlignment = HA;// CellHorizontalAlignment.Left;
            o.ActiveSheet.Columns[nCol].VerticalAlignment = VA;

            if (nRow == -1)
            {
                o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
            }
            else
            {
                o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
            }


        }

        public void setColStyle_Label(FpSpread o, int nRow, int nCol, bool WordWrap, bool MultiLine, CellHorizontalAlignment HA, CellVerticalAlignment VA)
        {
            o.ActiveSheet.Columns.Get(nCol).Locked = true;
            TextCellType spdObj = new TextCellType();
            spdObj.Multiline = MultiLine;
            spdObj.WordWrap = true;
            //spdObj.CharacterCasing = CharacterCasing.Upper;
            o.ActiveSheet.Columns[nCol].HorizontalAlignment = HA;// CellHorizontalAlignment.Left;
            o.ActiveSheet.Columns[nCol].VerticalAlignment = VA;

            if (nRow == -1)
            {
                o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
            }
            else
            {
                o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
            }


        }
        public void setColStyle_Text(FpSpread o, int nRow, int nCol, int MaxLength, bool MultiLine, CellHorizontalAlignment HA, CellVerticalAlignment VA, CharacterCasing CharacterCasing)
        {
            o.ActiveSheet.Columns.Get(nCol).Locked = false;
            TextCellType spdObj = new TextCellType();
            spdObj.Multiline = MultiLine;
            spdObj.MaxLength = MaxLength;
            spdObj.CharacterCasing = CharacterCasing;



            o.ActiveSheet.Columns[nCol].HorizontalAlignment = HA;// CellHorizontalAlignment.Left;
            o.ActiveSheet.Columns[nCol].VerticalAlignment = VA;

            if (nRow == -1)
            {
                o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
            }
            else
            {
                o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
            }


        }

        public int Spd_DataRowCnt(FarPoint.Win.Spread.FpSpread SpdNm)
        {
            int nRow = 0;

            for (int i = 0; i < SpdNm.ActiveSheet.RowCount; i++)
            {
                for (int j = 0; j < SpdNm.ActiveSheet.ColumnCount; j++)
                {
                    if (SpdNm.ActiveSheet.Cells[i,j].Text.Trim() != "")
                    {
                        nRow += 1;
                        break;
                    }
                }
            }
            return nRow;
        }
    }
}
