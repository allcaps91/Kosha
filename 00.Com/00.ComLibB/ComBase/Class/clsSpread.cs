using ComBase.Controls;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComBase
{
    public class clsSpread : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public static object GstrObjName; //Object Name
        //public static object GstrSSName; //Spread Sheet Name

        public static FarPoint.Win.Spread.SheetView GstrSSName;

        public const CellVerticalAlignment VAlign_B = CellVerticalAlignment.Bottom;
        public const CellVerticalAlignment VAlign_C = CellVerticalAlignment.Center;
        public const CellVerticalAlignment VAlign_D = CellVerticalAlignment.Distributed;
        public const CellVerticalAlignment VAlign_G = CellVerticalAlignment.General;
        public const CellVerticalAlignment VAlign_J = CellVerticalAlignment.Justify;
        public const CellVerticalAlignment VAlign_T = CellVerticalAlignment.Top;

        public const CellHorizontalAlignment HAlign_C = CellHorizontalAlignment.Center;
        public const CellHorizontalAlignment HAlign_D = CellHorizontalAlignment.Distributed;
        public const CellHorizontalAlignment HAlign_G = CellHorizontalAlignment.General;
        public const CellHorizontalAlignment HAlign_J = CellHorizontalAlignment.Justify;
        public const CellHorizontalAlignment HAlign_L = CellHorizontalAlignment.Left;
        public const CellHorizontalAlignment HAlign_R = CellHorizontalAlignment.Right;

        /// <summary>
        /// Description : OperationMode property settings
        /// Author : 박병규
        /// Create Date : 2017.08.30
        /// </summary>
        public const OperationMode NORMAL = OperationMode.Normal;
        public const OperationMode READONLY = OperationMode.ReadOnly;
        public const OperationMode ROWMODE = OperationMode.ReadOnly;
        public const OperationMode SINGLE = OperationMode.SingleSelect;
        public const OperationMode MULTI = OperationMode.MultiSelect;
        public const OperationMode EXT = OperationMode.ExtendedSelect;

        private bool Printing = false; //프린트가 하나 종료되고 다음 프린트를 하기 위해 사용

        /// <summary>
        /// 콤보스타일 셀에서 원하는 콤보 위치로 세팅 ; 디자이너에서 아이템 채웠을 경우
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="CodeLen"></param>
        /// <param name="sFind"></param>
        public static void gSpreadComboFind(FarPoint.Win.Spread.FpSpread spd, int Row, int Col, int CodeLen, string sFind)
        {
            int i = 0;

            if (sFind == "")
            {
                return;
            }
            try
            {
                FarPoint.Win.Spread.CellType.ComboBoxCellType cmbocell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                cmbocell = (FarPoint.Win.Spread.CellType.ComboBoxCellType)spd.ActiveSheet.GetCellType(Row, Col);
                for (i = 0; i <= cmbocell.Items.Length - 1; i++)
                {
                    spd.ActiveSheet.Cells[Row, Col].Value = cmbocell.Items[i];
                    if (VB.UCase(VB.Trim(VB.Right(spd.ActiveSheet.Cells[Row, Col].Text, CodeLen))) == VB.UCase(VB.Trim(sFind)))
                        return;
                }
                spd.ActiveSheet.Cells[Row, Col].Value = -1;
            }
            catch
            {
            }

        }

        /// <summary>
        /// 콤보스타일 셀에서 원하는 콤보 위치로 세팅 ; 디자이너에서 아이템 채웠을 경우
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="sFind"></param>
        public static void gSpreadComboFindEx(FarPoint.Win.Spread.FpSpread spd, int Row, int Col, string sFind)
        {
            int i = 0;

            if (sFind == "")
            {
                return;
            }
            try
            {
                FarPoint.Win.Spread.CellType.ComboBoxCellType cmbocell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                cmbocell = (FarPoint.Win.Spread.CellType.ComboBoxCellType)spd.ActiveSheet.GetCellType(Row, Col);
                for (i = 0; i <= cmbocell.Items.Length - 1; i++)
                {
                    spd.ActiveSheet.Cells[Row, Col].Value = cmbocell.Items[i];
                    if (VB.UCase(VB.Trim(spd.ActiveSheet.Cells[Row, Col].Text)) == VB.UCase(VB.Trim(sFind)))
                        return;
                }
                spd.ActiveSheet.Cells[Row, Col].Value = -1;
            }
            catch
            {
            }

        }


        /// <summary>
        /// 콤보스타일 셀에서 원하는 콤보 인덱스 받아온다 ; 디자이너에서 아이템 채웠을 경우
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="sFind"></param>
        /// <returns></returns>
        public static int gSpreadComboindex(FarPoint.Win.Spread.FpSpread spd, int Row, int Col, string sFind)
        {
            int i = -1;

            if (sFind == "")
            {
                return -1;
            }
            try
            {
                FarPoint.Win.Spread.CellType.ComboBoxCellType cmbocell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                cmbocell = (FarPoint.Win.Spread.CellType.ComboBoxCellType)spd.ActiveSheet.GetCellType(Row, Col);
                for (i = 0; i <= cmbocell.Items.Length - 1; i++)
                {
                    spd.ActiveSheet.Cells[Row, Col].Value = cmbocell.Items[i];
                    if (VB.UCase(VB.Trim(spd.ActiveSheet.Cells[Row, Col].Text)) == VB.UCase(VB.Trim(sFind)))
                    {
                        return i;
                    }
                }

                return -1;
            }
            catch
            {
                return -1;
            }

        }

        /// <summary>
        /// 스프레드 컬럼 속성을 정의하고 값을 세팅을 한다
        /// </summary>
        /// <param name="Spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="CellType"></param>
        /// <param name="bLocked"></param>
        /// <param name="VAlign"></param>
        /// <param name="HAlign"></param>
        /// <param name="strVal"></param>
        /// <param name="Multiline"></param>
        public static void SetTypeAndValue(FarPoint.Win.Spread.SheetView Spd, int Row, int Col, string CellType,
                                        bool bLocked, FarPoint.Win.Spread.CellVerticalAlignment VAlign, FarPoint.Win.Spread.CellHorizontalAlignment HAlign, string strVal, bool Multiline)
        {
            FarPoint.Win.Spread.CellType.CheckBoxCellType TypeCheckBox = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();

            if (CellType.Equals("CheckBoxCellType"))
            {
                Spd.Cells[Row, Col].CellType = TypeCheckBox;
                Spd.Cells[Row, Col].VerticalAlignment = VAlign;
                Spd.Cells[Row, Col].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                Spd.Cells[Row, Col].Locked = bLocked;
                Spd.Cells[Row, Col].Value = strVal.Equals("1"); // ( ? true : false);
            }
            else
            {
                if (Multiline == true)
                {
                    TypeText.Multiline = true;
                    TypeText.WordWrap = true;
                    TypeText.MaxLength = 10000;
                }
                else
                {
                    TypeText.Multiline = false;
                }
                Spd.Cells[Row, Col].CellType = TypeText;
                Spd.Cells[Row, Col].VerticalAlignment = VAlign;
                Spd.Cells[Row, Col].HorizontalAlignment = HAlign;
                Spd.Cells[Row, Col].Locked = bLocked;
                Spd.Cells[Row, Col].Text = strVal;
            }

        }
        /// <summary>
        /// 스프래드에 Data를 표시한다.
        /// </summary>
        /// <param name="Spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="strVal"></param>
        public static void SetSpdValue(FarPoint.Win.Spread.SheetView Spd, int Row, int Col, string strVal)
        {
            FarPoint.Win.Spread.CellType.CheckBoxCellType TypeCheckBox = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();

            if (Spd.Cells[Row, Col].CellType == TypeCheckBox)
            {
                Spd.Cells[Row, Col].Value = strVal.Equals("1");
            }
            else
            {
                Spd.Cells[Row, Col].Text = strVal;
            }

        }

        /// <summary>
        /// 콤보스타일 셀에서 원하는 콤보 위치로 세팅 : 코딩으로 세팅을 한경우
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="CodeLen"></param>
        /// <param name="sFind"></param>
        public static void gSpreadComboListFind(FarPoint.Win.Spread.FpSpread spd, int Row, int Col, int CodeLen, string sFind)
        {
            int i = 0;

            if (sFind == "")
            {
                return;
            }
            try
            {
                FarPoint.Win.Spread.CellType.ComboBoxCellType cmbocell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                cmbocell = (FarPoint.Win.Spread.CellType.ComboBoxCellType)spd.ActiveSheet.GetCellType(Row, Col);
                for (i = 0; i <= cmbocell.ListControl.Items.Count - 1; i++)
                {
                    spd.ActiveSheet.Cells[Row, Col].Value = cmbocell.ListControl.Items[i];
                    if (VB.UCase(VB.Trim(VB.Right(spd.ActiveSheet.Cells[Row, Col].Text, CodeLen))) == VB.UCase(VB.Trim(sFind)))
                        return;
                }
                spd.ActiveSheet.Cells[Row, Col].Value = -1;
            }
            catch
            {
            }

        }

        /// <summary>
        /// 콤보스타일 셀에서 원하는 콤보 위치로 세팅 : 코딩으로 세팅을 한경우
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="sFind"></param>
        public static void gSpreadComboListFindEx(FarPoint.Win.Spread.FpSpread spd, int Row, int Col, string sFind)
        {
            int i = 0;

            if (sFind == "")
            {
                return;
            }
            try
            {
                FarPoint.Win.Spread.CellType.ComboBoxCellType cmbocell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                cmbocell = (FarPoint.Win.Spread.CellType.ComboBoxCellType)spd.ActiveSheet.GetCellType(Row, Col);
                for (i = 0; i <= cmbocell.ListControl.Items.Count - 1; i++)
                {
                    spd.ActiveSheet.Cells[Row, Col].Value = cmbocell.ListControl.Items[i];
                    if (VB.UCase(VB.Trim(spd.ActiveSheet.Cells[Row, Col].Text)) == VB.UCase(VB.Trim(sFind)))
                        return;
                }
                spd.ActiveSheet.Cells[Row, Col].Value = -1;
            }
            catch
            {
            }

        }

        /// <summary>
        /// 콤보스타일 셀에서 Data를 세팅한다
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="Row2"></param>
        /// <param name="Col2"></param>
        /// <param name="intSpace"></param>
        /// <param name="strCode"></param>
        /// <param name="strCodeName"></param>
        public static void gSpreadComboDataSet(FarPoint.Win.Spread.FpSpread spd, int Row, int Col, int Row2, int Col2, int intSpace, string[] strCode, string[] strCodeName)
        {
            int i = 0;

            try
            {
                if (strCode.Length <= 0)
                {
                    return;
                }
                //FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
                //spd.ActiveSheet.Cells[Row, Col, Row2, Col2].CellType = TypeText;

                FarPoint.Win.Spread.CellType.ComboBoxCellType Type1 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                Type1.Clear();

                //spd.ActiveSheet.Cells[Row, Col, Row2, Col2].CellType = Type1;
                spd.ActiveSheet.Cells[Row, Col, Row2, Col2].CellType = Type1;
                ListBox list1 = new ListBox();
                for (i = 0; i < strCode.Length; i++)
                {
                    list1.Items.Add(strCodeName[i] + VB.Space(intSpace) + strCode[i]);
                }
                Type1.ListControl = list1;
                Type1.Editable = false;
                //spd.ActiveSheet.Cells[Row, Col, Row2, Col2].CellType = Type1;
                spd.ActiveSheet.Cells[Row, Col, Row2, Col2].CellType = Type1;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 스프래드 콤보 세팅 명칭만.
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="Row2"></param>
        /// <param name="Col2"></param>
        /// <param name="strCode"></param>
        public static void gSpreadComboDataSetEx(FarPoint.Win.Spread.FpSpread spd, int Row, int Col, int Row2, int Col2, string[] strCode)
        {
            int i = 0;

            try
            {
                if (strCode.Length <= 0)
                {
                    return;
                }
                FarPoint.Win.Spread.CellType.ComboBoxCellType Type1 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                Type1.Clear();
                spd.ActiveSheet.Cells[Row, Col, Row2, Col2].CellType = Type1;
                ListBox list1 = new ListBox();
                for (i = 0; i < strCode.Length; i++)
                {
                    list1.Items.Add(strCode[i]);
                }
                Type1.ListControl = list1;
                Type1.Editable = true;
                spd.ActiveSheet.Cells[Row, Col, Row2, Col2].CellType = Type1;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 스프래드 콤보 세팅 명칭만 + EDIT 여부
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="Row2"></param>
        /// <param name="Col2"></param>
        /// <param name="strCode"></param>
        /// <param name="blnEdit"></param>
        public static void gSpreadComboDataSetEx1(FarPoint.Win.Spread.FpSpread spd, int Row, int Col, int Row2, int Col2, string[] strCode, bool blnEdit)
        {
            int i = 0;

            try
            {
                if (strCode.Length <= 0)
                {
                    return;
                }
                FarPoint.Win.Spread.CellType.ComboBoxCellType Type1 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                Type1.Clear();
                spd.ActiveSheet.Cells[Row, Col, Row2, Col2].CellType = Type1;
                ListBox list1 = new ListBox();
                for (i = 0; i < strCode.Length; i++)
                {
                    list1.Items.Add(strCode[i]);
                }
                Type1.ListControl = list1;
                Type1.Editable = blnEdit;
                spd.ActiveSheet.Cells[Row, Col, Row2, Col2].CellType = Type1;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 스프레드 엔터치면 다음 로우로(Key_Down 이벤트)
        /// </summary>
        /// <param name="spd"></param>
        public static void gSpreadEnter(FarPoint.Win.Spread.FpSpread spd)
        {
            FarPoint.Win.Spread.InputMap inputmap;
            inputmap = spd.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused);
            inputmap.Put(new FarPoint.Win.Spread.Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextRow);
        }

        /// <summary>
        /// 스프레드 엔터치면 다음 칼럼으로(Key_Down 이벤트)
        /// </summary>
        /// <param name="spd"></param>
        public static void gSpreadEnter_NextColumn(FarPoint.Win.Spread.FpSpread spd)
        {
            FarPoint.Win.Spread.InputMap inputmap;
            inputmap = spd.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused);
            inputmap.Put(new FarPoint.Win.Spread.Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
        }


        public static void gSpreadEnter_NextCol(FarPoint.Win.Spread.FpSpread spd)
        {
            FarPoint.Win.Spread.InputMap inputmap;
            inputmap = spd.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused);
            inputmap.Put(new FarPoint.Win.Spread.Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
        }

        public static void gSpreadEnter_NextRow(FarPoint.Win.Spread.FpSpread spd)
        {
            FarPoint.Win.Spread.InputMap inputmap;
            inputmap = spd.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused);
            inputmap.Put(new FarPoint.Win.Spread.Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextRow);
        }

        public static void gSpreadEnter_NextRowFirstColumn(FarPoint.Win.Spread.FpSpread spd)
        {
            FarPoint.Win.Spread.InputMap inputmap;
            inputmap = spd.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused);
            inputmap.Put(new FarPoint.Win.Spread.Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextRowFirstColumn);
        }


        /// <summary>
        /// 콤보스타일 셀에서 원하는 Item을 반환
        /// </summary>
        /// <param name="spread"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="CodeLen"></param>
        /// <param name="strItemdata"></param>
        /// <returns></returns>
        public static string gGetSpreadComboItem(FarPoint.Win.Spread.FpSpread spread, int row, int column, string strItemdata)
        {
            string functionReturnValue = null;
            FarPoint.Win.Spread.CellType.ComboBoxCellType cmbocell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            int i = 0;
            functionReturnValue = "";
            var _with1 = spread.Sheets[0];

            cmbocell = (FarPoint.Win.Spread.CellType.ComboBoxCellType)spread.ActiveSheet.GetCellType(row, column);
            for (i = 0; i <= cmbocell.Items.Length - 1; i++)
            {
                if (cmbocell.ItemData[i] == strItemdata)
                {
                    functionReturnValue = cmbocell.Items[i];
                    break;
                }
            }
            return functionReturnValue;
        }

        /// <summary>
        /// 콤보스타일 셀에서 원하는 ItemData을 반환
        /// </summary>
        /// <param name="spread"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="CodeLen"></param>
        /// <param name="strItem"></param>
        /// <returns></returns>
        public static string gGetSpreadComboItemData(FarPoint.Win.Spread.FpSpread spread, int row, int column, string strItem)
        {
            string functionReturnValue = null;
            FarPoint.Win.Spread.CellType.ComboBoxCellType cmbocell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            int i = 0;
            functionReturnValue = "";
            var _with1 = spread.Sheets[0];

            cmbocell = (FarPoint.Win.Spread.CellType.ComboBoxCellType)spread.ActiveSheet.GetCellType(row, column);
            for (i = 0; i <= cmbocell.Items.Length - 1; i++)
            {
                if (cmbocell.Items[i] == strItem)
                {
                    functionReturnValue = cmbocell.ItemData[i];
                    break;
                }
            }
            return functionReturnValue;
        }

        /// <summary>
        /// Spread Sort 
        /// </summary>
        /// <param name="spread"></param>
        /// <param name="Col"></param>
        public static void gSpdSortRow(FarPoint.Win.Spread.FpSpread spread, int Col)
        {
            if (spread.ActiveSheet.ColumnHeader.Cells[0, Col].Column.SortIndicator == FarPoint.Win.Spread.Model.SortIndicator.Descending)
            {
                spread.ActiveSheet.SortRows(Col, true, true);
            }
            else
            {
                spread.ActiveSheet.SortRows(Col, false, true);
            }
            spread.ActiveSheet.ActiveRowIndex = 0;
            spread.ShowRow(0, 0, FarPoint.Win.Spread.VerticalPosition.Top);
        }

        /// <summary>
        /// Spread Sort 
        /// </summary>
        /// <param name="spread"></param>
        /// <param name="Col"></param>
        public static void gSpdSortRow(FarPoint.Win.Spread.FpSpread spread, int Col, ref bool bolSort, bool bol)
        {
            if (bol == true)
            {
                if (bolSort == false)
                {
                    spread.ActiveSheet.SortRows(Col, true, false);
                    bolSort = true;
                }
                else
                {
                    spread.ActiveSheet.SortRows(Col, false, false);
                    bolSort = false;
                }

                spread.ActiveSheet.ActiveRowIndex = 0;
                spread.ShowRow(0, 0, FarPoint.Win.Spread.VerticalPosition.Top);

                spread.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
            }
            spread.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
        }

        /// <summary> 스프레드의 좌측 정렬
        /// 2017.05.24 박병규
        /// </summary>
        public enum enmSpdHAlign { General, Left, Center, Right };

        /// <summary>
        /// <param name="spdHeader"></param>
        /// <param name="f"></param>
        /// <param name="align"></param>
        /// <param name="PageNumber"></param>
        /// <param name="Return"></param>
        /// 2017.05.24 박병규
        /// </summary>
        public string setSpdPrint_String(string spdHeader, Font f, enmSpdHAlign align, bool PageNumber, bool Return)
        {
            string strReturn = null;
            char ch = '"';

            if (align == enmSpdHAlign.Left || align == enmSpdHAlign.General)
            {
                strReturn += "/l";
            }
            else if (align == enmSpdHAlign.Center)
            {
                strReturn += "/c";
            }
            else if (align == enmSpdHAlign.Right)
            {
                strReturn += "/r";
            }

            if (f != null)
            {
                strReturn += "/fn" + ch + f.Name + ch;
                strReturn += "/fz" + ch + f.Size + ch;
                strReturn += f.Bold == true ? "/fb1" : "/fb0";
                strReturn += f.Italic == true ? "/fi1" : "/fi0";
                strReturn += f.Underline == true ? "/fu1" : "/fu0";
                strReturn += f.Strikeout == true ? "/fk1" : "/fk0";
            }

            if (PageNumber == true)
            {
                strReturn += "/p";
            }
            else
            {
                strReturn += spdHeader.ToString();
            }

            if (Return == true)
            {
                strReturn += "/n";
            }
            return strReturn;
        }

        /// <summary>스프레드출력여백설정
        /// <param name="header">머릿말 여백</param>
        /// <param name="foot">아래말 여백</param>
        /// <param name="top">위 여백</param>
        /// <param name="bottom">아래 여백</param>
        /// <param name="left">좌 여백</param>
        /// <param name="right">우 여백</param>
        /// 2017.05.24 박병규
        /// </summary>
        public struct SpdPrint_Margin
        {
            public PrintMargin Margin;

            public SpdPrint_Margin(Int32 header, Int32 foot, Int32 top, Int32 bottom, Int32 left, Int32 right)
            {
                Margin = new PrintMargin();
                Margin.Header = header;
                Margin.Footer = foot;
                Margin.Top = top;
                Margin.Bottom = bottom;
                Margin.Left = left;
                Margin.Right = right;
            }
        }

        /// <summary>스프레드출력옵션설정
        /// <param name="orientation">가로,세로 페이지 설정</param>
        /// <param name="pageRange">폐이지 프린트 옵션(전체,현재,선택부분</param>
        /// <param name="PageStart">지정된 페이지일경우 시작 페이지</param>
        /// <param name="pageEnd">지정된 페이지일경우 끝나는 페이지</param>
        /// <param name="showColHead">컬럼 해더를 보일경우</param>
        /// <param name="showRowHead">컬럼 로우를 보일경우</param>
        /// <param name="showGrid">그리드 라인을 표기할 경우</param>
        /// <param name="showBord">페이지 보드설정</param>
        /// <param name="showShoadows">그림자 설정</param>
        /// <param name="showColor">컬러 설정</param>
        /// <param name="smartPrint">자동 설정 부분</param>
        /// <param name="ZoomFactor">페이지 줌</param>
        /// 2017.05.24 박병규
        /// </summary>
        public struct SpdPrint_Option
        {
            public PrintType pageRange;
            public PrintOrientation orientation;
            public Int32 PageStart, pageEnd;
            public bool showColHead, showRowHead, showGrid, showBord, showShoadows, showColor, smartPrint;
            public float ZoomFactor;

            public SpdPrint_Option(PrintOrientation orientation, PrintType pageRange, Int32 PageStart, Int32 pageEnd,
                                        bool showColHead, bool showRowHead, bool showGrid, bool showBord, bool showShoadows,
                                        bool showColor, bool smartPrint, float ZoomFactor = 1f)
            {
                this.orientation = orientation;
                this.pageRange = pageRange;
                this.PageStart = PageStart;
                this.pageEnd = pageEnd;

                this.showColHead = showColHead;
                this.showRowHead = showRowHead;
                this.showGrid = showGrid;
                this.showBord = showBord;
                this.showShoadows = showShoadows;
                this.showColor = showColor;
                this.smartPrint = smartPrint;
                this.ZoomFactor = ZoomFactor;


            }
        }

        /// <summary>스프레드 출력
        /// <param name="o">대상 스프레드</param>
        /// <param name="prePrint">미리보기 여부</param>
        /// <param name="setMargin">스프레드 여백설정부분</param>
        /// <param name="setOption">스프레드 출력 옵션 설정</param>
        /// <param name="Header">머릿말</param>
        /// <param name="Foot">꼬릿말</param>
        /// <param name="isCentering">중앙정렬 옵션</param>
        /// 2017.05.24 박병규
        /// </summary>
        public void setSpdPrint(FpSpread o, bool prePrint, SpdPrint_Margin setMargin, SpdPrint_Option setOption, string Header, string Foot)
        {
            PrintInfo info = new PrintInfo();
            SmartPrintRulesCollection prules = new SmartPrintRulesCollection();

            info.ColStart = o.ActiveSheet.Models.Selection.AnchorColumn;
            info.ColEnd = o.ActiveSheet.Models.Selection.LeadColumn;
            info.RowStart = o.ActiveSheet.Models.Selection.AnchorRow;
            info.RowEnd = o.ActiveSheet.Models.Selection.LeadRow;

            info.Orientation = setOption.orientation;
            if (info.ColStart > 0 || info.ColEnd > 0 || info.RowStart > 0 || info.RowEnd > 0)
            {
                if (info.ColStart != info.ColEnd || info.RowStart != info.RowEnd)
                {
                    info.PrintType = PrintType.CellRange;
                }
                else
                {
                    info.PrintType = setOption.pageRange;
                }
            }
            else
            {
                info.PrintType = setOption.pageRange;
            }

            info.PageStart = setOption.PageStart;
            info.PageEnd = setOption.pageEnd;


            info.ShowColumnHeaders = setOption.showColHead;
            info.ShowRowHeaders = setOption.showRowHead;
            info.ShowGrid = setOption.showGrid;
            info.ShowBorder = setOption.showBord;
            info.ShowShadows = setOption.showShoadows;
            info.ShowColor = setOption.showColor;
            info.ZoomFactor = setOption.ZoomFactor;
            info.Centering = Centering.Horizontal;
            
            if (setOption.smartPrint == true)
            {
                info.UseMax = true;
                info.BestFitCols = false;
                info.BestFitRows = false;

                info.SmartPrintPagesTall = 99;
                info.SmartPrintPagesWide = 1;

                //prules.Add(new BestFitColumnRule(ResetOption.None));
                prules.Add(new LandscapeRule(ResetOption.None));
                prules.Add(new ScaleRule(ResetOption.None, 1, (float)0.1, (float)0.1));

                info.SmartPrintRules = prules;
                info.UseSmartPrint = true;
            }

            info.Margin = setMargin.Margin;
            info.Header = Header;
            info.Footer = Foot;
            info.Preview = prePrint;

            int oldRowCount = o.ActiveSheet.RowCount;
            //int oldColCount = o.ActiveSheet.ColumnCount-1;

            //if (o.ActiveSheet.RowCount < 4)
            //{
            //    for (int i = o.ActiveSheet.RowCount; i < 4; i++)
            //    {
            //        o.ActiveSheet.RowCount += 1;
            //        o.ActiveSheet.Cells[i, 0].Value = "";
            //        o.ActiveSheet.Cells[i, oldColCount].Value = "";
            //    }
            //}

            o.ActiveSheet.PrintInfo = info;
            o.PrintSheet(o.ActiveSheetIndex);

            //o.ActiveSheet.RowCount = oldRowCount;
        }

        /// <summary>스프레드 출력
        /// <param name="o">대상 스프레드</param>
        /// <param name="prePrint">미리보기 여부</param>
        /// <param name="setMargin">스프레드 여백설정부분</param>
        /// <param name="setOption">스프레드 출력 옵션 설정</param>
        /// <param name="Header">머릿말</param>
        /// <param name="Foot">꼬릿말</param>
        /// <param name="isCentering">중앙정렬 옵션</param>
        /// 2017.05.24 박병규
        /// </summary>
        public void setSpdPrint(FpSpread o, bool prePrint, SpdPrint_Margin setMargin, SpdPrint_Option setOption, string Header, string Foot, bool isCentering = true, int isFirstPageNumber = 1)
        {
            PrintInfo info = new PrintInfo();
            SmartPrintRulesCollection prules = new SmartPrintRulesCollection();

            info.ColStart = o.ActiveSheet.Models.Selection.AnchorColumn;
            info.ColEnd = o.ActiveSheet.Models.Selection.LeadColumn;
            info.RowStart = o.ActiveSheet.Models.Selection.AnchorRow;
            info.RowEnd = o.ActiveSheet.Models.Selection.LeadRow;

            info.Orientation = setOption.orientation;
            if (info.ColStart > 0 || info.ColEnd > 0 || info.RowStart > 0 || info.RowEnd > 0)
            {
                if (info.ColStart != info.ColEnd || info.RowStart != info.RowEnd)
                {
                    info.PrintType = PrintType.CellRange;
                }
                else
                {
                    info.PrintType = setOption.pageRange;
                }
            }
            else
            {
                info.PrintType = setOption.pageRange;
            }

            info.PageStart = setOption.PageStart;
            info.PageEnd = setOption.pageEnd;


            info.ShowColumnHeaders = setOption.showColHead;
            info.ShowRowHeaders = setOption.showRowHead;
            info.ShowGrid = setOption.showGrid;
            info.ShowBorder = setOption.showBord;
            info.ShowShadows = setOption.showShoadows;
            info.ShowColor = setOption.showColor;
            info.ZoomFactor = setOption.ZoomFactor;

            if (isCentering)
            {
                info.Centering = Centering.Horizontal;
            }

            if (isFirstPageNumber > 1)
            {
                info.FirstPageNumber = isFirstPageNumber; //페이지 번호를 특정번호부터 부여한다.
            }

            if (setOption.smartPrint == true)
            {
                info.UseMax = true;
                info.BestFitCols = true;
                info.BestFitRows = true;

                info.SmartPrintPagesTall = 1;
                info.SmartPrintPagesWide = 1;

                prules.Add(new BestFitColumnRule(ResetOption.None));
                prules.Add(new LandscapeRule(ResetOption.None));
                prules.Add(new ScaleRule(ResetOption.None, 1, (float)0.1, (float)0.1));

                info.SmartPrintRules = prules;
                info.UseSmartPrint = true;
            }

            info.Margin = setMargin.Margin;
            info.Header = Header;
            info.Footer = Foot;
            info.Preview = prePrint;

            int oldRowCount = o.ActiveSheet.RowCount;
            //int oldColCount = o.ActiveSheet.ColumnCount-1;

            //if (o.ActiveSheet.RowCount < 4)
            //{
            //    for (int i = o.ActiveSheet.RowCount; i < 4; i++)
            //    {
            //        o.ActiveSheet.RowCount += 1;
            //        o.ActiveSheet.Cells[i, 0].Value = "";
            //        o.ActiveSheet.Cells[i, oldColCount].Value = "";
            //    }
            //}

            o.ActiveSheet.PrintInfo = info;
            o.PrintSheet(o.ActiveSheetIndex);

            //o.ActiveSheet.RowCount = oldRowCount;
        }

        int FistPageRowCount = 0;

        public void setSpdPrint(FpSpread o, bool prePrint, SpdPrint_Margin setMargin, SpdPrint_Option setOption, string Header, string Foot, Centering center, Image[] image = null)
        {
            PrintInfo info = new PrintInfo();
            SmartPrintRulesCollection prules = new SmartPrintRulesCollection();

            info.ColStart = o.ActiveSheet.Models.Selection.AnchorColumn;
            info.ColEnd = o.ActiveSheet.Models.Selection.LeadColumn;
            info.RowStart = o.ActiveSheet.Models.Selection.AnchorRow;
            info.RowEnd = o.ActiveSheet.Models.Selection.LeadRow;

            info.Orientation = setOption.orientation;
            if (info.ColStart > 0 || info.ColEnd > 0 || info.RowStart > 0 || info.RowEnd > 0)
            {
                if (info.ColStart != info.ColEnd || info.RowStart != info.RowEnd)
                {
                    info.PrintType = PrintType.CellRange;
                }
                else
                {
                    info.PrintType = setOption.pageRange;
                }
            }
            else
            {
                info.PrintType = setOption.pageRange;
            }

            info.PageStart = setOption.PageStart;
            info.PageEnd = setOption.pageEnd;

            info.ShowColumnHeaders = setOption.showColHead;
            info.ShowRowHeaders = setOption.showRowHead;
            info.ShowGrid = setOption.showGrid;
            info.ShowBorder = setOption.showBord;
            info.ShowShadows = setOption.showShoadows;
            info.ShowColor = setOption.showColor;
            info.ZoomFactor = setOption.ZoomFactor;
            info.Centering = center;

            if (setOption.smartPrint == true)
            {
                info.UseMax = true;
                info.BestFitCols = true;
                info.BestFitRows = true;

                info.SmartPrintPagesTall = 1;
                info.SmartPrintPagesWide = 1;

                prules.Add(new BestFitColumnRule(ResetOption.None));
                prules.Add(new LandscapeRule(ResetOption.None));
                prules.Add(new ScaleRule(ResetOption.None, 1, (float)0.1, (float)0.1));

                info.SmartPrintRules = prules;
                info.UseSmartPrint = true;
            }

            info.Margin = setMargin.Margin;

            if (image != null)
            {
                info.Images = image;

                if (Header.IndexOf("/g\"0\"") == -1)
                {
                    Header = Header + "/n/r/g\"0\"";
                }
            }

            info.Header = Header;
            info.Footer = Foot;
            info.Preview = prePrint;

            int oldRowCount = o.ActiveSheet.RowCount;

            if (image == null)
            {
                o.ActiveSheet.PrintInfo = info;
            }
            else
            {
                o.PrintMessageBox += O_PrintMessageBox; //스프레드 메세지 이용하는 이벤트 이지만 프린트 시작과 끝을 알 수 있어서 사용

                //결재 라인이 있으면 첫장 출력후 나머지 장을 출력한다.
                info.PrintType = PrintType.PageRange;
                info.PageStart = 0;
                info.PageEnd = 1;

                o.ActiveSheet.PrintInfo = info;

                o.PrintHeaderFooterArea += O_PrintHeaderFooterArea; //첫장에 출력되는 로우수 가져오기 위해 사용

                o.PrintSheet(o.ActiveSheetIndex); // 첫장 출력

                Printing = true; //프린트가 하나 종료되고 다음 프린트를 하기 위해 사용

                while (Printing == true)
                {
                    Application.DoEvents();
                }

                o.PrintHeaderFooterArea -= O_PrintHeaderFooterArea;

                o.PrintMessageBox -= O_PrintMessageBox;

                info.Header = null;

                //2번째장
                info.ColStart = 0;
                info.ColEnd = o.ActiveSheet.ColumnCount - 1;
                info.RowStart = FistPageRowCount + o.ActiveSheet.FrozenRowCount; //첫장에 출력된 로우수에 고정로우 수를 더 해준다.
                info.RowEnd = o.ActiveSheet.RowCount - 1;

                if (info.RowStart == o.ActiveSheet.RowCount) // 첫장에 모두 출력 했으면 더 이상 출력 하지 않는다.
                    return;

                info.PrintType = PrintType.CellRange;

                info.FirstPageNumber = 2; //페이지 번호를 2번부터 넣어준다.
                o.ActiveSheet.PrintInfo = info;
            }

            o.PrintSheet(o.ActiveSheetIndex);

            //o.ActiveSheet.RowCount = oldRowCount;
        }

        private void O_PrintMessageBox(object sender, PrintMessageBoxEventArgs e)
        {
            Printing = e.BeginPrinting; // false면 프린트 종료
        }

        private void O_PrintHeaderFooterArea(object sender, PrintHeaderFooterAreaEventArgs e)
        {
            if (e.PageNumber == 1)
            {
                FistPageRowCount = e.Cells.RowCount;
            }
        }





        /// <summary>스프레드 출력
        /// <author>김홍록</author>
        /// <date>2017.11.28</date>
        /// <param name="o">대상 스프레드</param>
        /// <param name="prePrint">미리보기 여부</param>
        /// 
        /// <param name="setMargin">스프레드 여백설정부분</param>
        /// <param name="setOption">스프레드 출력 옵션 설정</param>
        /// <param name="Header">머릿말</param>
        /// <param name="Foot">꼬릿말</param>
        /// </summary>
        public void setSpdPrint(FpSpread o, bool prePrint, SpdPrint_Margin setMargin, SpdPrint_Option setOption, string Header, string Foot, Centering center)
        {
            PrintInfo info = new PrintInfo();
            SmartPrintRulesCollection prules = new SmartPrintRulesCollection();

            info.ColStart = o.ActiveSheet.Models.Selection.AnchorColumn;
            info.ColEnd = o.ActiveSheet.Models.Selection.LeadColumn;
            info.RowStart = o.ActiveSheet.Models.Selection.AnchorRow;
            info.RowEnd = o.ActiveSheet.Models.Selection.LeadRow;

            info.Orientation = setOption.orientation;
            if (info.ColStart > 0 || info.ColEnd > 0 || info.RowStart > 0 || info.RowEnd > 0)
            {
                if (info.ColStart != info.ColEnd || info.RowStart != info.RowEnd)
                {
                    info.PrintType = PrintType.CellRange;
                }
                else
                {
                    info.PrintType = setOption.pageRange;
                }
            }
            else
            {
                info.PrintType = setOption.pageRange;
            }

            info.PageStart = setOption.PageStart;
            info.PageEnd = setOption.pageEnd;

            info.ShowColumnHeaders = setOption.showColHead;
            info.ShowRowHeaders = setOption.showRowHead;
            info.ShowGrid = setOption.showGrid;
            info.ShowBorder = setOption.showBord;
            info.ShowShadows = setOption.showShoadows;
            info.ShowColor = setOption.showColor;
            info.ZoomFactor = setOption.ZoomFactor;

            if (setOption.smartPrint == true)
            {
                info.UseMax = true;
                info.BestFitCols = true;
                info.BestFitRows = true;

                info.SmartPrintPagesTall = 1;
                info.SmartPrintPagesWide = 1;

                prules.Add(new BestFitColumnRule(ResetOption.None));
                prules.Add(new LandscapeRule(ResetOption.None));
                prules.Add(new ScaleRule(ResetOption.None, 1, (float)0.1, (float)0.1));

                info.SmartPrintRules = prules;
                info.UseSmartPrint = true;
            }

            #region //2018-08-04 안정수, 병리과 이미지서명 출력관련하여 추가함
            if (o.Name == "ss_S")
            {
                info.Colors = new Color[] { Color.Red, Color.Blue };
                info.Images = new Image[] { Properties.Resources._53784_2_ };
            }
            else if (o.Name == "ss_S_1")
            {
                info.Colors = new Color[] { Color.Red, Color.Blue };
                info.Images = new Image[] { Properties.Resources._47787_2_ };
            }
            #endregion

            info.Margin = setMargin.Margin;
            info.Header = Header;
            info.Footer = Foot;



            info.Preview = prePrint;

            info.Centering = center;



            int oldRowCount = o.ActiveSheet.RowCount;
            //int oldColCount = o.ActiveSheet.ColumnCount-1;

            //if (o.ActiveSheet.RowCount < 4)
            //{
            //    for (int i = o.ActiveSheet.RowCount; i < 4; i++)
            //    {
            //        o.ActiveSheet.RowCount += 1;
            //        o.ActiveSheet.Cells[i, 0].Value = "";
            //        o.ActiveSheet.Cells[i, oldColCount].Value = "";
            //    }
            //}


            o.ActiveSheet.PrintInfo = info;


            o.PrintSheet(o.ActiveSheetIndex);


            //o.ActiveSheet.RowCount = oldRowCount;
        }

        /// <summary>스프레드 출력
        /// <author>유진호</author>
        /// <date>2018.04.12</date>
        /// </summary>
        /// <param name="o">대상 스프레드</param>
        /// <param name="prePrint">미리보기 여부</param>
        /// <param name="setMargin">스프레드 여백설정부분</param>
        /// <param name="setOption">스프레드 출력 옵션 설정</param>
        /// <param name="Header">머릿말</param>
        /// <param name="Foot">꼬릿말</param>
        /// <param name="PrinterName">프린터명</param>
        public void setSpdPrint(FpSpread o, bool prePrint, SpdPrint_Margin setMargin, SpdPrint_Option setOption, string Header, string Foot, string PrinterName)
        {
            PrintInfo info = new PrintInfo();
            SmartPrintRulesCollection prules = new SmartPrintRulesCollection();

            info.ColStart = o.ActiveSheet.Models.Selection.AnchorColumn;
            info.ColEnd = o.ActiveSheet.Models.Selection.LeadColumn;
            info.RowStart = o.ActiveSheet.Models.Selection.AnchorRow;
            info.RowEnd = o.ActiveSheet.Models.Selection.LeadRow;

            info.Orientation = setOption.orientation;
            if (info.ColStart > 0 || info.ColEnd > 0 || info.RowStart > 0 || info.RowEnd > 0)
            {
                if (info.ColStart != info.ColEnd || info.RowStart != info.RowEnd)
                {
                    info.PrintType = PrintType.CellRange;
                }
                else
                {
                    info.PrintType = setOption.pageRange;
                }
            }
            else
            {
                info.PrintType = setOption.pageRange;
            }

            info.PageStart = setOption.PageStart;
            info.PageEnd = setOption.pageEnd;

            info.ShowColumnHeaders = setOption.showColHead;
            info.ShowRowHeaders = setOption.showRowHead;
            info.ShowGrid = setOption.showGrid;
            info.ShowBorder = setOption.showBord;
            info.ShowShadows = setOption.showShoadows;
            info.ShowColor = setOption.showColor;
            info.ZoomFactor = setOption.ZoomFactor;

            if (setOption.smartPrint == true)
            {
                info.UseMax = true;
                info.BestFitCols = true;
                info.BestFitRows = true;

                info.SmartPrintPagesTall = 1;
                info.SmartPrintPagesWide = 1;

                prules.Add(new BestFitColumnRule(ResetOption.None));
                prules.Add(new LandscapeRule(ResetOption.None));
                prules.Add(new ScaleRule(ResetOption.None, 1, (float)0.1, (float)0.1));

                info.SmartPrintRules = prules;
                info.UseSmartPrint = true;
            }

            info.Printer = PrinterName;
            info.Margin = setMargin.Margin;
            info.Header = Header;
            info.Footer = Foot;
            info.Preview = prePrint;

            int oldRowCount = o.ActiveSheet.RowCount;

            o.ActiveSheet.PrintInfo = info;
            o.PrintSheet(o.ActiveSheetIndex);
        }


        public void setSpdPrintToPdf(FpSpread o, bool prePrint, SpdPrint_Margin setMargin, SpdPrint_Option setOption, string Header, string Foot, Centering center, string PathAndPdfFileName, string PdfPassword)
        {
            PrintInfo info = new PrintInfo();
            SmartPrintRulesCollection prules = new SmartPrintRulesCollection();

            info.ColStart = o.ActiveSheet.Models.Selection.AnchorColumn;
            info.ColEnd = o.ActiveSheet.Models.Selection.LeadColumn;
            info.RowStart = o.ActiveSheet.Models.Selection.AnchorRow;
            info.RowEnd = o.ActiveSheet.Models.Selection.LeadRow;

            info.Orientation = setOption.orientation;
            if (info.ColStart > 0 || info.ColEnd > 0 || info.RowStart > 0 || info.RowEnd > 0)
            {
                if (info.ColStart != info.ColEnd || info.RowStart != info.RowEnd)
                {
                    info.PrintType = PrintType.CellRange;
                }
                else
                {
                    info.PrintType = setOption.pageRange;
                }
            }
            else
            {
                info.PrintType = setOption.pageRange;
            }

            info.PageStart = setOption.PageStart;
            info.PageEnd = setOption.pageEnd;

            info.ShowColumnHeaders = setOption.showColHead;
            info.ShowRowHeaders = setOption.showRowHead;
            info.ShowGrid = setOption.showGrid;
            info.ShowBorder = setOption.showBord;
            info.ShowShadows = setOption.showShoadows;
            info.ShowColor = setOption.showColor;
            info.ZoomFactor = setOption.ZoomFactor;

            if (setOption.smartPrint == true)
            {
                info.UseMax = true;
                info.BestFitCols = true;
                info.BestFitRows = true;

                info.SmartPrintPagesTall = 1;
                info.SmartPrintPagesWide = 1;

                prules.Add(new BestFitColumnRule(ResetOption.None));
                prules.Add(new LandscapeRule(ResetOption.None));
                prules.Add(new ScaleRule(ResetOption.None, 1, (float)0.1, (float)0.1));

                info.SmartPrintRules = prules;
                info.UseSmartPrint = true;
            }

            info.Margin = setMargin.Margin;
            info.Header = Header;
            info.Footer = Foot;
            info.Preview = prePrint;
            info.Centering = center;
            info.PrintToPdf = true;
            info.PdfFileName = PathAndPdfFileName;
            FarPoint.PDF.PdfSecurity security = new FarPoint.PDF.PdfSecurity("admin", PdfPassword);
            info.PdfSecurity = security;

            o.ActiveSheet.PrintInfo = info;
            o.PrintSheet(o.ActiveSheetIndex);
        }


        /// <summary>2017.05.25.김홍록 : 스프레드 정렬</summary>
        /// <param name="o"></param>
        /// <param name="nCol"></param>
        public void setSpdSort(FpSpread o, int nCol, bool isEnable)
        {
            if (nCol == -1)
            {
                for (int i = 0; i < o.ActiveSheet.Columns.Count; i++)
                {
                    o.ActiveSheet.Columns.Get(i).AllowAutoSort = isEnable;
                }
            }
            else
            {
                o.ActiveSheet.Columns.Get(nCol).AllowAutoSort = isEnable;
            }

        }

        /// <summary>2017.05.25.김홍록: 헤더 사이즈 설정</summary>
        /// <param name="o"></param>
        /// <param name="size"></param>
        public void setHeardSize(FpSpread o, int size)
        {
            o.ActiveSheet.ColumnHeader.Rows[0].Height = size;

        }

        /// <summary> 스프레드 헤더를 보일것인가 말것인가
        /// </summary>
        /// <param name="o">대상스프레드am>
        /// <param name="IsRow">로우 보일것인가?</param>
        /// <param name="IsCol">컬럼 보일것인가?</param>
        public void setIsHeaderView(FpSpread o, bool IsRow, bool IsCol)
        {
            try
            {
                o.ActiveSheet.Models.ColumnHeaderRowAxis.SetVisible(0, IsCol);
                o.ActiveSheet.Models.RowHeaderColumnAxis.SetVisible(0, IsRow);
            }
            catch { }
        }

        /// <summary>2017.05.25.김홍록 : 헤더 설정</summary>
        /// <param name="o">대상스프테드</param>
        /// <param name="strTitly">타이틀모임</param>
        /// <param name="size">각타이틀의 사이즈</param>
        /// <param name="isCol">컬럼 헤더일 경우 True</param>
        public void setHeader(FpSpread o, string[] strTitly, int[] size, bool isCol = true, string[] strDataField = null)
        {
            try
            {
                if (isCol == true)
                {
                    o.ActiveSheet.ColumnCount = strTitly.Length;

                    for (int i = 0; i < o.ActiveSheet.ColumnCount; i++)
                    {
                        o.ActiveSheet.Columns.Get(i).Label = strTitly[i];
                        o.ActiveSheet.Columns.Get(i).Width = size[i];
                        o.ActiveSheet.Columns.Get(i).Font = new Font("굴림체", 9);
                        if (strDataField != null)
                        {
                            o.ActiveSheet.Columns[i].DataField = strDataField[i];
                        }
                    }
                }
                else
                {
                    o.ActiveSheet.RowCount = strTitly.Length;

                    for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                    {
                        o.ActiveSheet.Rows.Get(i).Label = strTitly[i];
                        o.ActiveSheet.Rows.Get(i).Height = size[i];
                        o.ActiveSheet.Rows.Get(i).Font = new Font("굴림체", 9);
                    }

                }
            }
            catch { }
        }

        /// <summary>2017.05.25.김홍록 : 헤더 설정</summary>
        /// <param name="o">대상스프테드</param>
        /// <param name="strTitly">타이틀모임</param>
        /// <param name="size">각타이틀의 사이즈</param>
        /// <param name="isCol">컬럼 헤더일 경우 True</param>
        public void setHeader(FpSpread o, string[] strTitly, int[] size, int nSize, bool isCol = true)
        {
            try
            {
                if (isCol == true)
                {
                    o.ActiveSheet.ColumnCount = strTitly.Length;

                    for (int i = 0; i < o.ActiveSheet.ColumnCount; i++)
                    {
                        o.ActiveSheet.Columns.Get(i).Label = strTitly[i];
                        o.ActiveSheet.Columns.Get(i).Width = size[i];
                        o.ActiveSheet.Columns.Get(i).Font = new Font("맑은 고딕", nSize);
                    }
                }
                else
                {
                    o.ActiveSheet.RowCount = strTitly.Length;

                    for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                    {
                        o.ActiveSheet.Rows.Get(i).Label = strTitly[i];
                        o.ActiveSheet.Rows.Get(i).Height = size[i];
                        o.ActiveSheet.Rows.Get(i).Font = new Font("맑은 고딕", nSize);
                    }

                }
            }
            catch { }
        }

        /// <summary>2017.05.25.김홍록: 스프레드 종류</summary>
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

        /// <summary>2017.05.25.김홍록: 스프레드 종류</summary>
        /// <param name="o">대상스프레드</param>
        /// <param name="nRow">변경할 로우값</param>
        /// <param name="nCol">변경할 컬럼값</param>
        /// <param name="type">변경 타입</param>
        /// <param name="strCombo">콤보박스초기 표기값</param>
        /// <param name="strBtn">버튼의 보이는 값</param>
        /// <param name="strBtnDown">버튼 클릭시 보이는 값</param>
        /// <param name="chkCaption">체크박스일경우 보이는 값</param>
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
                //spdObj.DecimalPlaces = deciMalplace;

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

        /// <summary>
        /// 2021. 10. 29 김민철
        /// setColStyle 함수 SpreadCellTypeOption 사용버전
        /// </summary>
        /// <param name="o"></param>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <param name="type"></param>
        /// <param name="option"></param>
        public void setColStyle(FpSpread o, int nRow, int nCol, enmSpdType type, SpreadCellTypeOption option)
        {
            o.ActiveSheet.Columns.Get(nCol).Locked = false;

            if (type == enmSpdType.Button)
            {
                #region Button

                ButtonCellType spdObj = new ButtonCellType();
                spdObj.Text = option.ButtonText;
                spdObj.TextDown = option.strBtnDown;

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
                spdObj.Caption = option.chkCaption;

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
                if (option.strCombo == null || option.strCombo.ToString().Trim().Length == 0)
                {
                    string[] s = { "Null.없음" };
                    spdObj.Items = s;
                }
                else
                {
                    spdObj.Items = option.strCombo;
                }

                if (nRow == -1)
                {
                    o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
                }
                else
                {
                    o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
                }

                if (o.ActiveSheet.NonEmptyRowCount > 0 && option.strCombo != null)
                {
                    for (int i = 0; i < o.ActiveSheet.Rows.Count; i++)
                    {
                        if (o.ActiveSheet.Cells[i, nCol].Text.Trim().Length == 0)
                        {
                            o.ActiveSheet.Cells[i, nCol].Text = option.strCombo[0];
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
                spdObj.Multiline = option.IsMulti;
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
                spdObj.Multiline = option.IsMulti;
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
                spdObj.DecimalPlaces = option.DecimalPlaces;

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

        /// <summary> 스프레드 정렬방법 컬럼기준
        /// </summary>
        /// <param name="o">대상스프레드</param>
        /// <param name="nCol">컬럼위치</param>
        /// <param name="HAlign">좌정렬 값</param>
        /// <param name="VAlign">우정렬 값</param>
        public void setColAlign(FpSpread o, int nCol, CellHorizontalAlignment HAlign, CellVerticalAlignment VAlign)
        {

            o.ActiveSheet.Columns[nCol].HorizontalAlignment = HAlign;
            o.ActiveSheet.Columns[nCol].VerticalAlignment = VAlign;
        }

        /// <summary>컬럼 머지</summary>
        /// <param name="o">대상스프레드</param>
        /// <param name="nCol">해당 컬럼</param>
        public void setColMerge(FpSpread o, int nCol)
        {
            o.ActiveSheet.SetColumnMerge(nCol, FarPoint.Win.Spread.Model.MergePolicy.Always);
        }

        /// <summary>로우 머지</summary>
        /// <param name="o">대상 스프레드</param>
        /// <param name="nRow">해당 로우</param>
        public void setRowMerge(FpSpread o, int nRow)
        {
            o.ActiveSheet.SetRowMerge(nRow, FarPoint.Win.Spread.Model.MergePolicy.Always);
        }

        /// <summary>Rowcount를 지우지 않고 spread text clear
        /// <param name="o">대상 스프레드</param>
        /// <param name="nRowCnt"></param>
        /// <param name="nColCnt"></param>
        /// 2017.06.01 박병규
        /// </summary>
        public void Spread_Clear(FpSpread o, int nRowCnt, int nColCnt)
        {
            o.ActiveSheet.ClearRange(0, 0, nRowCnt, nColCnt, true);
        }

        /// <summary>특정범위를 지우지 않고 spread text clear
        /// <param name="o">대상 스프레드</param>
        /// <param name="nRow">시작 Row</param>
        /// <param name="nCol">시작 Col</param>
        /// <param name="nRowCnt">Row Count</param>
        /// <param name="nColCnt">Col Count</param>
        /// 2017.06.23 박병규
        /// </summary>
        public void Spread_Clear_Range(FpSpread o, int nRow, int nCol, int nRowCnt, int nColCnt)
        {
            o.ActiveSheet.ClearRange(nRow, nCol, nRowCnt, nColCnt, true);
        }

        /// <summary>
        /// 스프레드 셀 병합
        /// </summary>
        /// <param name="o"></param>
        /// <param name="Row1">시작 Row</param>
        /// <param name="Col1">시작 Col</param>
        /// <param name="RowCnt">Row Count</param>
        /// <param name="ColCnt">Col Count</param>
        public void CellSpan(FpSpread o, int Row1, int Col1, int RowCnt, int ColCnt)
        {
            o.ActiveSheet.AddSpanCell(Row1, Col1, RowCnt, ColCnt);
        }

        /// <summary>
        /// 스프레드 Clear
        /// 2017.06.13 이상훈
        /// </summary>
        /// <param name="argSpdName"></param>
        public void Spread_All_Clear(FpSpread SpdName)
        {
            if (SpdName.ActiveSheet.RowCount != 0)
            {
                SpdName.ActiveSheet.ClearRange(0, 0, (int)SpdName.ActiveSheet.RowCount, (int)SpdName.ActiveSheet.ColumnCount, true);
                SpdName.ActiveSheet.RowCount = 0;
                SpdName.ActiveSheet.ClearControls();
                SpdName.DataSource = null;
            }
        }

        /// <summary>
        /// 단순 Data 값만 Clear, Defualt Row Count 지정
        /// </summary>
        /// <param name="SpdName"></param>
        /// <param name="DefaultRowCnt"></param>
        public void Spread_Clear_Simple(FpSpread SpdName, int DefaultRowCnt = 0)
        {
            SpdName.ActiveSheet.ClearRange(0, 0, SpdName.ActiveSheet.Rows.Count, SpdName.ActiveSheet.ColumnCount, true);
            SpdName.ActiveSheet.Rows.Count = DefaultRowCnt;
        }

        /// <summary>
        /// 스프레드 행높이 지정
        /// </summary>
        /// 2017.06.13 이상훈
        /// <param name="Fps"></param>
        /// <param name="argHeight"></param>
        public void SetfpsRowHeight(FpSpread SpdName, float argHeight)
        {
            SpdName.ActiveSheet.Rows[-1].Height = argHeight;
            //for (int i = 0; i < SpdName.ActiveSheet.RowCount; i++)
            //{
            //    SpdName.ActiveSheet.Rows[i].Height = argHeight;
            //}
        }

        /// <summary>
        /// 셀 가운데,위,아래,왼쪽,오른쪽 맞춤  2017-06-01 KMC
        /// </summary>
        /// <param name="o"></param>
        /// <param name="Row1">시작 Row</param>
        /// <param name="Col1">시작 Col</param>
        /// <param name="Row2">마지막 Row</param>
        /// <param name="Col2">마지막 Col</param>
        /// <param name="HAlign">[수평]General=0, Left=1, Center=2, Right=3,  Justify=4, Distributed = 5</param>
        /// <param name="VAlign">[수직]General=0, Top=1,  Center=2, Bottom=3, Justify=4, Distributed = 5</param>
        public void CellAlignMent(FpSpread o, int Row1, int Col1, int Row2, int Col2, CellHorizontalAlignment HAlign, CellVerticalAlignment VAlign)
        {
            FarPoint.Win.Spread.Cell cellrange;
            cellrange = o.ActiveSheet.Cells[Row1, Col1, Row2, Col2];

            cellrange.HorizontalAlignment = HAlign;
            cellrange.VerticalAlignment = VAlign;
        }

        /// <summary>
        /// 스프레드 라인그리기   2017-06-01 KMC
        /// </summary>
        /// <param name="spd">대상스프레드</param>
        /// <param name="Row1">시작 Row</param>
        /// <param name="Col1">시작 Col</param>
        /// <param name="Row2">종료 Row</param>
        /// <param name="Col2">종료 Col</param>
        /// <param name="Color"></param>
        /// <param name="Linethick">라인두께</param> 
        /// <param name="Left"></param>
        /// <param name="Top"></param>
        /// <param name="Right"></param>
        /// <param name="Bottom"></param>
        public static void gSpreadLineBoder(FpSpread spd, int Row1, int Col1, int Row2, int Col2, Color Color, int Linethick, bool Left, bool Top, bool Right, bool Bottom)
        {
            try
            {
                FarPoint.Win.LineBorder LineBorder = new FarPoint.Win.LineBorder(Color, Linethick, Left, Top, Right, Bottom);
                //spd.ActiveSheet.Cells[Row, Col].Border = LineBorder;
                spd.ActiveSheet.Cells[Row1, Col1, Row2, Col2].Border = LineBorder;
            }
            catch
            {

            }
        }

        /// <summary>
        /// 스프레드 라인그리기   2017-06-01 KMC
        /// </summary>
        /// <param name="spd">대상스프레드</param>
        /// <param name="Row1">시작 Row</param>
        /// <param name="Col1">시작 Col</param>
        /// <param name="Row2">종료 Row</param>
        /// <param name="Col2">종료 Col</param>
        /// <param name="Color"></param>
        /// <param name="Linethick">라인두께</param> 
        /// <param name="Left"></param>
        /// <param name="Top"></param>
        /// <param name="Right"></param>
        /// <param name="Bottom"></param>
        public static void gSpreadHeaderLineBoder(FpSpread spd, int Row1, int Col1, int Row2, int Col2, Color Color, int Linethick, bool Left, bool Top, bool Right, bool Bottom)
        {
            try
            {
                FarPoint.Win.LineBorder LineBorder = new FarPoint.Win.LineBorder(Color, Linethick, Left, Top, Right, Bottom);
                //spd.ActiveSheet.Cells[Row, Col].Border = LineBorder;
                spd.ActiveSheet.ColumnHeader.Cells[Row1, Col1, Row2, Col2].Border = LineBorder;
            }
            catch
            {

            }
        }

        /// <summary>스프레드 필터링</summary>
        /// <author>김홍록</author>
        /// <date>2017.06.08</date>
        /// <param name="o">대상스프레드</param>
        /// <param name="col">대상컬럼</param>
        /// <param name="mode">필터방법</param>
        public void setSpdFilter(FpSpread o, int col, AutoFilterMode mode, bool isEnable)
        {
            if (col == -1)
            {
                for (int i = 0; i < o.ActiveSheet.Columns.Count; i++)
                {
                    o.ActiveSheet.Columns[i].AllowAutoFilter = isEnable;
                }

            }
            else
            {
                o.ActiveSheet.Columns[col].AllowAutoFilter = isEnable;
            }
            o.ActiveSheet.AutoFilterMode = mode;
        }

        /// <summary>스프레드 조건부 서식</summary>
        /// <author>김홍록</author>
        /// <date>2017.06.08</date>
        /// <param name="o">대상스프레드</param>
        /// <param name="col">해당컬럼</param>
        public void setSpdCondiFomatting(FpSpread o, int col)
        {
            UnaryComparisonConditionalFormattingRule unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.Operator = UnaryComparisonOperator.EqualTo;
            //unary.Value = "Y";
            unary.BackColor = Color.Azure;
            o.ActiveSheet.SetConditionalFormatting(-1, -1, unary);
        }

        /// <summary>스프레드 셀색 설정</summary>
        /// <author>김홍록</author>
        /// <date>2017.06.08</date>
        /// <param name="o">대상스프레드</param>
        /// <param name="row">시작로우</param>
        /// <param name="col">시작컬럼</param>
        /// <param name="row2">종료로우</param>
        /// <param name="col2">종료컬럼</param>
        /// <param name="color">설정색</param>
        public void setSpdCellColor(FpSpread o, int row, int col, int row2, int col2, Color color)
        {
            o.ActiveSheet.Cells[row, col, row2, col2].BackColor = color;
        }

        /// <summary>스프레드 폰트색 설정</summary>
        /// <author>박병규</author>
        /// <date>2017.06.16</date>
        /// <param name="o">대상스프레드</param>
        /// <param name="row">시작로우</param>
        /// <param name="col">시작컬럼</param>
        /// <param name="row2">종료로우</param>
        /// <param name="col2">종료컬럼</param>
        /// <param name="color">설정색</param>
        public void setSpdForeColor(FpSpread o, int row, int col, int row2, int col2, Color color)
        {
            o.ActiveSheet.Cells[row, col, row2, col2].ForeColor = color;
        }

        /// <summary>
        /// 스프레드 각 셀의 값을 초기화 한다.
        /// <param name="Spd"></param>
        /// 2017-06-05 안정수
        /// </summary>
        //public void SS_Clear(FarPoint.Win.Spread.SheetView Spd)
        //{
        //    int i, j;

        //    for (i = 0; i < Spd.RowCount; i++)
        //    {
        //        for (j = 0; j < Spd.ColumnCount; j++)
        //        {
        //            Spd.Cells[i, j].Text = "";
        //        }
        //    }
        //}

        /// <summary>스프레드 너비 자동조절
        /// <param name="o">대상 스프레드</param>
        /// 2017.06.27 박병규
        /// </summary>
        public void SetPreferredWidth(FpSpread o)
        {
            for (int i = 0; i < o.ActiveSheet.ColumnCount; i++)
            {
                o.ActiveSheet.Columns[i].Width = o.ActiveSheet.GetPreferredColumnWidth(i) + 8;
            }
        }

        /// <summary>스프레드 높이 자동조절
        /// <param name="o">대상 스프레드</param>
        /// 2017.06.27 박병규
        /// </summary>
        public void SetPreferredHeight(FpSpread o)
        {
            for (int i = 0; i < o.ActiveSheet.RowCount; i++)
            {
                o.ActiveSheet.Rows[i].Height = o.ActiveSheet.Rows[i].GetPreferredHeight() + 4;
            }
        }

        public enum enmSpdEnterKey
        {
            /// <summary>엔터키를 받으면 셀의 우측로 포커스 이동</summary>
            Right,
            /// <summary>엔터키를 받으면 셀의 아래로 포커스 이동</summary>
            Down
        };

        /// <summary>스프레드 셀 모두 체크</summary>
        /// <author>김홍록</Author>
        /// <date>2017.06.30</date>
        /// <param name="o">대상스프레드</param>
        /// <param name="nCol">대상컬럼</param>
        /// <param name="isChk">체크유무</param>
        public void setSpdCellChk_All(FpSpread o, int nCol, bool isChk)
        {
            bool isFilter = false;
            for (int i = 0; i < o.ActiveSheet.RowCount; i++)
            {

                try
                {
                    isFilter = o.ActiveSheet.RowFilter.IsRowFilteredOut(i);

                    if (isFilter == true)
                    {
                        break;
                    }
                }
                catch
                {

                    isFilter = false;
                    break;
                }
            }

            if (isChk == true)
            {
                for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                {

                    o.ActiveSheet.Cells[i, nCol].Text = "";

                    if (isFilter == true)
                    {
                        if (o.ActiveSheet.RowFilter.IsRowFilteredOut(i) == false)
                        {
                            o.ActiveSheet.Cells[i, nCol].Text = "True";
                        }
                    }
                    else
                    {
                        o.ActiveSheet.Cells[i, nCol].Text = "True";
                    }

                }
            }
            else
            {
                for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                {
                    if (isFilter == true)
                    {
                        if (o.ActiveSheet.RowFilter.IsRowFilteredOut(i) == false)
                        {
                            o.ActiveSheet.Cells[i, nCol].Text = "";
                        }

                    }
                    else
                    {
                        o.ActiveSheet.Cells[i, nCol].Text = "";
                    }
                }
            }
        }

        /// <summary> 스프레드로우 삽입 또는 삭제</summary>
        /// <author>김홍록</author>
        /// <date>2017.07.11</date>
        /// <param name="o">대상스프레드</param>
        /// <param name="IsInsert">셀로우 삽입시 설정</param>
        public void setDel_Ins(FpSpread o, bool IsInsert)
        {
            if (IsInsert == true)
            {
                if (o.ActiveSheet.Rows.Count == 0)
                {
                    o.ActiveSheet.Rows.Count = 1;
                }
                else if (o.ActiveSheet.Rows.Count - 1 == o.ActiveSheet.ActiveCell.Row.Index)
                {
                    o.ActiveSheet.Rows.Count = o.ActiveSheet.Rows.Count + 1;
                }
                else
                {
                    o.ActiveSheet.AddRows(o.ActiveSheet.ActiveCell.Row.Index, 1);
                }
            }
            else
            {
                if (o.ActiveSheet.Rows.Count > 0)
                {
                    o.ActiveSheet.RemoveRows(o.ActiveSheet.ActiveCell.Row.Index, 1);
                }
            }
        }

        /// <summary>스프레드 엔터키를 통한 이동</summary>
        /// <author>김홍록</author>
        /// <date>2017.07.11</date>
        /// <param name="o"></param>
        /// <param name="keyType"></param>
        public void setEnterKey(FpSpread o, enmSpdEnterKey keyType)
        {
            InputMap im;
            im = o.GetInputMap(InputMapMode.WhenAncestorOfFocused);


            if (keyType == enmSpdEnterKey.Right)
            {
                if (o.ActiveSheet.ActiveColumn.Index < o.ActiveSheet.ColumnCount - 1)
                {
                    im.Put(new Keystroke(Keys.Enter, Keys.None), SpreadActions.MoveToNextColumn);
                }
                else
                {
                    if (o.ActiveSheet.ActiveCell.Row.Index == o.ActiveSheet.Rows.Count - 1)
                    {
                        setDel_Ins(o, true);
                    }


                    o.ActiveSheet.SetActiveCell(o.ActiveSheet.ActiveCell.Row.Index + 1, 0);
                }
            }
            else if (keyType == enmSpdEnterKey.Down)
            {
                if (o.ActiveSheet.ActiveCell.Row.Index < o.ActiveSheet.Rows.Count)
                {
                    im.Put(new Keystroke((char)Keys.Enter), SpreadActions.MoveToNextRow);
                }
            }
        }

        /// <summary> 스프레드로우 위치를 위로 이동</summary>
        /// <author>김홍록</author>
        /// <date>2017.08.07</date>
        /// <param name="o">대상스프레드</param>
        public void setRowUp(FpSpread o)
        {
            try
            {
                if (o.ActiveSheet.ActiveRow != null)
                {
                    int Row = o.ActiveSheet.ActiveRow.Index;

                    if (Row != 0)
                    {
                        o.ActiveSheet.MoveRow(Row, Row - 1, true);
                        o.ActiveSheet.ActiveRowIndex = Row - 1;
                    }
                }

            }
            catch (Exception)
            {


            }
        }

        /// <summary> 스프레드로우 위치를 아래로 이동</summary>
        /// <author>김홍록</author>
        /// <date>2017.08.07</date>
        /// <param name="o">대상스프레드</param>
        public void setRowDown(FpSpread o)
        {
            try
            {
                if (o.ActiveSheet.ActiveRow != null)
                {
                    int Row = o.ActiveSheet.ActiveRow.Index;
                    int RowCount = o.ActiveSheet.RowCount - 1;

                    if (Row != RowCount)
                    {
                        o.ActiveSheet.MoveRow(Row, Row + 1, true);
                        o.ActiveSheet.ActiveRowIndex = Row + 1;
                    }
                }
            }
            catch (Exception)
            {


            }
        }

        /// <summary>스프레드 특정 값 위치 찾기</summary>
        /// <author>김홍록</author>
        /// <date>2017.10.12</date>
        /// <param name="o"></param>
        /// <param name="strFind1"></param>
        /// <param name="nFindCol1"></param>
        /// <returns></returns>
        public int findValueRow(FpSpread o, string strFind1, string strFind2, int nFromCol, int nToCol, int nFindCol1, int nFindCol2)
        {
            int nReturn = -1;
            string spdData1 = string.Empty;
            string spdData2 = string.Empty;

            for (int i = nFromCol; i < nToCol; i++)
            {
                spdData1 = o.ActiveSheet.Cells[i, nFindCol1].Text.Trim();
                spdData2 = o.ActiveSheet.Cells[i, nFindCol2].Text.Trim();

                if (spdData1 == strFind1 && spdData2 != strFind2)
                {
                    return i;
                }
            }


            return nReturn;
        }

        /// <summary>스프레드 체크 박스 체크 여부</summary>
        /// <author>김홍록</author>
        /// <date>2017.10.23</date>
        /// <param name="o">대상스프레드</param>
        /// <param name="nCol">체크박스가 있는 컬럼</param>
        /// <returns></returns>
        public bool isChk(FpSpread o, int nCol)
        {
            bool b = false;

            for (int i = 0; i < o.ActiveSheet.Rows.Count; i++)
            {
                if (o.ActiveSheet.Cells[i, nCol].Text.Equals("True"))
                {
                    return true;
                }
            }

            return b;
        }

        /// <summary>
        /// 콤보스타일 셀에서 원하는 콤보 위치로 세팅 ; 콤보아이템 첫글자가 같을 경우  "1.테스트"  == "1"
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="CodeLen"></param>
        /// <param name="sFind"></param>
        public static void gSdCboItemFindLeft(FarPoint.Win.Spread.FpSpread spd, int Row, int Col, int CodeLen, string sFind)
        {
            int i = 0;

            if (sFind == "")
            {
                return;
            }
            try
            {
                FarPoint.Win.Spread.CellType.ComboBoxCellType cmbocell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                cmbocell = (FarPoint.Win.Spread.CellType.ComboBoxCellType)spd.ActiveSheet.GetCellType(Row, Col);
                for (i = 0; i <= cmbocell.Items.Length - 1; i++)
                {
                    spd.ActiveSheet.Cells[Row, Col].Value = cmbocell.Items[i];
                    if (VB.UCase(VB.Trim(VB.Left(spd.ActiveSheet.Cells[Row, Col].Text, CodeLen))) == VB.UCase(VB.Trim(sFind)))
                        return;
                }
                spd.ActiveSheet.Cells[Row, Col].Value = -1;
            }
            catch
            {
            }

        }

        /// <summary>2017.12.05. 김민철: 셀길이 </summary>        
        /// <param name="o">대상스프레드</param>
        /// <param name="nRow">변경할 Row값</param>
        /// <param name="nCol">변경할 Col값</param>
        /// <param name="nLen">세팅할 MaxLength</param>
        public void setColLength(FpSpread o, int nRow, int nCol, int nMaxLen)
        {
            o.ActiveSheet.Columns.Get(nCol).Locked = false;

            TextCellType spdObj = new TextCellType();
            spdObj.MaxLength = nMaxLen;

            if (nRow == -1)
            {
                o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
            }
            else
            {
                o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
            }
        }

        /// <summary>2017.12.07. 김민철 셀 숫자형 및 소수점 세팅</summary>        
        /// <param name="o">대상스프레드</param>
        /// <param name="nRow">변경할 Row값</param>
        /// <param name="nCol">변경할 Col값</param>
        /// <param name="nLen">세팅할 Decimal 자릿수</param>
        public void setColNumberDec(FpSpread o, int nRow, int nCol, int nDec)
        {
            NumberCellType spdObj = new NumberCellType();
            spdObj.DecimalPlaces = nDec;
            spdObj.ShowSeparator = true;
            spdObj.MaximumValue = 999999999;
            spdObj.MinimumValue = -999999999;

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
        }

        /// <summary>
        /// RowData 위로 이동
        /// </summary>
        /// <param name="fprSpr"></param>
        /// <param name="intActRow"></param>
        /// <param name="nMinRow"></param>
        public void sprRowUp(ref FarPoint.Win.Spread.FpSpread SpdNm, int intActRow, int nMinRow)
        {
            SetfpsNull(SpdNm);
            int intRowcnt = SpdNm.ActiveSheet.NonEmptyRowCount;
            SetDeleteNull(SpdNm, intRowcnt);
            if (intActRow != 0)
            {
                if (intActRow <= nMinRow) return;
                try
                {
                    SpdNm.ActiveSheet.MoveRow(intActRow, intActRow - 1, 1, true);
                }
                catch { }
                SpdNm.ActiveSheet.ActiveRowIndex = SpdNm.ActiveSheet.ActiveRowIndex - 1;
            }
        }

        public void sprRowUp_First(ref FarPoint.Win.Spread.FpSpread SpdNm, int intActRow, int nMinRow)
        {
            SetfpsNull(SpdNm);
            int intRowcnt = SpdNm.ActiveSheet.NonEmptyRowCount;
            SetDeleteNull(SpdNm, intRowcnt);
            if (intActRow != 0)
            {
                if (intActRow <= nMinRow) return;
                SpdNm.ActiveSheet.MoveRow(intActRow, nMinRow, true);
                SpdNm.ActiveSheet.ActiveRowIndex = nMinRow;
            }
        }

        public void sprRowDown_Last(ref FarPoint.Win.Spread.FpSpread SpdNm, int intActRow, int nMaxRow)
        {
            if (clsOrdFunction.GstrGbJob == "OPD")
            {
                if (clsOrdFunction.GEnvSet_Item21 != null && clsOrdFunction.GEnvSet_Item21 == "2")
                {
                    if (SpdNm.ActiveSheet.Cells[SpdNm.ActiveSheet.ActiveRowIndex + 1, 3].Text == "") return;
                }
                else
                {
                    if (SpdNm.ActiveSheet.Cells[SpdNm.ActiveSheet.ActiveRowIndex + 1, 1].Text == "") return;
                }
            }
            else
            {
                if (SpdNm.ActiveSheet.Cells[SpdNm.ActiveSheet.ActiveRowIndex + 1, 2].Text == "") return;
            }

            if (intActRow != (int)SpdNm.ActiveSheet.RowCount - 1)
            {
                SpdNm.ActiveSheet.MoveRow(intActRow, nMaxRow - 1, true);
                SpdNm.ActiveSheet.ActiveRowIndex = nMaxRow - 1;
            }
        }

        /// <summary>
        /// RowData 아래로 이동
        /// </summary>
        /// <param name="fprSpr"></param>
        /// <param name="intActRow"></param>
        /// <param name="nMinRow"></param>
        public void sprRowDown(ref FarPoint.Win.Spread.FpSpread SpdNm, int intActRow, int nMinRow)
        {
            if (clsOrdFunction.GstrGbJob == "OPD")
            {
                if (SpdNm.ActiveSheet.Cells[SpdNm.ActiveSheet.ActiveRowIndex + 1, 1].Text == "") return;
            }
            else
            {
                if (SpdNm.ActiveSheet.Cells[SpdNm.ActiveSheet.ActiveRowIndex + 1, 2].Text == "") return;
            }

            if (intActRow != (int)SpdNm.ActiveSheet.RowCount - 1)
            {
                SpdNm.ActiveSheet.MoveRow(intActRow, intActRow + 1, true);
                SpdNm.ActiveSheet.ActiveRowIndex = SpdNm.ActiveSheet.ActiveRowIndex + 1;
            }
        }

        public void SetfpsNull(FpSpread SpdNm)
        {
            for (int i = 0; i < SpdNm.ActiveSheet.NonEmptyRowCount; i++)
            {
                for (int j = 0; j < SpdNm.ActiveSheet.ColumnCount; j++)
                {
                    if (fpSprChkVal(SpdNm, i, j) == "")
                    {
                        SpdNm.ActiveSheet.Cells[i, j].Value = null;
                    }
                }
            }
        }

        public void SetDeleteNull(FpSpread SpdNm, int argRowcnt) 
        {
            for (int i = argRowcnt; i >= 0; i--)
            {
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    if (clsType.User.IdNumber == "53775")
                    {
                        if (fpSprChkVal(SpdNm, i, 2) == "")
                        {
                            SpdNm.ActiveSheet.Rows[i].Remove();
                        }
                    }
                    else
                    {
                        if (fpSprChkVal(SpdNm, i, 1) == "")
                        {
                            SpdNm.ActiveSheet.Rows[i].Remove();
                        }
                    }
                }
                else
                {
                    if (fpSprChkVal(SpdNm, i, 2) == "")
                    {
                        SpdNm.ActiveSheet.Rows[i].Remove();
                    }
                }

            }
        }

        public string fpSprChkVal(FpSpread SpdNm, int sprrow, int sprcol)
        {
            if (SpdNm.ActiveSheet.Cells[sprrow, sprcol].Value == null)
            {
                return "";
            }
            else
            {
                return SpdNm.ActiveSheet.Cells[sprrow, sprcol].Value.ToString();
            }
        }

        public int Spread_Row_Count(FarPoint.Win.Spread.FpSpread SpdNm, int nStartRowCount, int nMaxRowCount)
        {
            int j = 0;

            for (int i = nStartRowCount; i < nMaxRowCount; i++)
            {
                if (SpdNm.ActiveSheet.Cells[i, 2].Text.Trim() != "")
                {
                    j += 1;
                }
                else
                {
                    break;
                }
            }
            return j;
        }

        /// <summary>엑셀파일생성</summary>
        /// <author>유진호</author>
        /// <date>2018.03.09</date>
        /// <param name="spd">대상스프레드</param>                
        public void ExportToXLS(FpSpread spd)
        {
            using (SaveFileDialog mtsDlg = new SaveFileDialog())
            {
                mtsDlg.InitialDirectory = Application.StartupPath;
                mtsDlg.Filter = "Excel files (*.xls)|*.xls|All file(*.*)|*.*";
                mtsDlg.FilterIndex = 1;

                if (mtsDlg.ShowDialog() == DialogResult.OK)
                {
                    spd.ActiveSheet.Protect = false;
                    spd.SaveExcel(mtsDlg.FileName, FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
                    ComFunc.MsgBox("저장이 완료 되었습니다.");
                }
            }
        }

        /// <summary>
        /// Spread 지정된 열의 체크박스 전체 선택, 해제
        /// </summary>
        /// <param name="sdp"></param>
        /// <param name="Col"></param>
        /// <param name="ArgVal">Y: 체크, N: 체크해제</param>
        public void Spd_All_Check_Col(FpSpread spd, int nCol, string ArgVal)
        {
            CheckBoxCellType chk = new CheckBoxCellType();

            for (int i = 0; i < spd.ActiveSheet.RowCount - 1; i++)
            {
                if (ArgVal == "Y")
                {
                    spd.ActiveSheet.Cells[-1, nCol].Text = "True";
                }
                else
                {
                    spd.ActiveSheet.Cells[-1, nCol].Text = "False";
                }
            }

        }

        /// <summary>
        /// ComboBox Cell 화살표로 이동 작업중
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        public void setSpdComboArrExit(FpSpread spd, int nRow, int nCol)
        {
            ComboBoxCellType cmbocell = new ComboBoxCellType();
            cmbocell.Items = (new String[] { "", "1", "2", "3", "4", "5" });
            cmbocell.AcceptsArrowKeys = FarPoint.Win.SuperEdit.AcceptsArrowKeys.AllArrows;
            cmbocell.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
            cmbocell.Editable = true;

            spd.ActiveSheet.Cells[nRow, nCol].CellType = cmbocell;
        }

        /// <summary> 해당 스프레드에서 필터해서 나온 ROW건수 체크 </summary>
        /// <author>윤조연</author>
        /// <date>2018.05.16</date>
        /// <param name="o">해당 스프레드</param>
        /// <returns></returns>
        public int SpdFilter_DataRowCount(FpSpread o)
        {
            int nRowCnt = o.ActiveSheet.RowCount;
            int nCnt = 0;

            for (int i = 0; i <= nRowCnt - 1; i++)
            {
                if (o.ActiveSheet.RowFilter.IsRowFilteredOut(i) == true)
                {
                    nCnt++;
                }
            }

            if (nCnt > 0)
            {
                return nRowCnt - nCnt;
            }
            else
            {
                return nRowCnt;
            }
        }

        /// <summary>
        /// 스프레드 Tab치면 다음 로우로(Key_Down 이벤트)
        /// </summary>
        /// <param name="spd"></param>
        public static void gSpreadTabNextRow(FarPoint.Win.Spread.FpSpread spd)
        {
            FarPoint.Win.Spread.InputMap inputmap;
            inputmap = spd.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused);
            inputmap.Put(new FarPoint.Win.Spread.Keystroke(Keys.Tab, Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextRow);
        }

        /// <summary>
        /// 스프레드 Tab치면 다음 칼럼으로(Key_Down 이벤트)
        /// </summary>
        /// <param name="spd"></param>
        public static void gSpreadTabNextCol(FarPoint.Win.Spread.FpSpread spd)
        {
            FarPoint.Win.Spread.InputMap inputmap;
            inputmap = spd.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused);
            inputmap.Put(new FarPoint.Win.Spread.Keystroke(Keys.Tab, Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
        }

        /// <summary>
        /// Spread Loop Display
        /// </summary>
        /// <param name="DT"></param>
        /// <param name="argSpreadName"></param>
        /// <param name="RowCount"></param>
        public void Spread_Loop_Disp(DataTable DT, FarPoint.Win.Spread.FpSpread argSpreadName, long RowCount)
        {
            if (RowCount > 0)
            {
                argSpreadName.ActiveSheet.RowCount = (int)RowCount;

                for (int i = 0; i < (int)argSpreadName.ActiveSheet.RowCount; i++)
                {
                    for (int j = 0; j < (int)argSpreadName.ActiveSheet.ColumnCount; j++)
                    {
                        argSpreadName.ActiveSheet.Cells[i, j].Text = DT.Rows[i][j].ToString().To<string>("");
                    }
                }
            }
        }
    }
}
