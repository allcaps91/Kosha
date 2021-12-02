using System;
using System.Drawing;
using ComBase;

namespace ComEmrBase
{
    /// <summary>
    /// 디자인시 사용 : 스프래드
    /// </summary>
    public class DesignFunc
    {
        /// <summary>
        /// 기본높이(너비) 세팅 함수
        /// </summary>
        /// <param name="ItemCd"></param>
        public static void DefaultHeight(FarPoint.Win.Spread.SheetView spd, string pFLOWGB, int Val)
        {
            if (pFLOWGB.Equals("ROW"))
            {
                if (spd.RowCount == 0)
                    return;
            }
            else
            {
                if (spd.ColumnCount == 0)
                    return;
            }

            if (pFLOWGB.Equals("ROW"))
            {
                spd.Columns[0, spd.ColumnCount - 1].Width = Val;
            }
            else
            {
                spd.Rows[0, spd.RowCount - 1].Height = Val;
            }
        }

        /// <summary>
        /// 시간에 따른 Duty 자동 세팅
        /// </summary>
        /// <param name="ItemCd"></param>
        public static void DefaultDuty(FarPoint.Win.Spread.FpSpread spd, int Col = 1, int Row = 1)
        {
            if (spd.ActiveSheet.RowCount == 0)
                return;

            DateTime SysTime = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

            string rtnVal = string.Empty;

            //Day
            if (DateTime.Compare(SysTime, Convert.ToDateTime("07:00")) >= 0 && DateTime.Compare(SysTime, Convert.ToDateTime("14:59")) <= 0)
            {
                rtnVal = "Day";
            }
            //Evening
            else if (DateTime.Compare(SysTime, Convert.ToDateTime("15:00")) >= 0 && DateTime.Compare(SysTime, Convert.ToDateTime("22:59")) <= 0)
            {
                rtnVal = "Evening";
            }
            //Night
            else if (DateTime.Compare(SysTime, Convert.ToDateTime("23:00")) >= 0 || DateTime.Compare(SysTime, Convert.ToDateTime("06:59")) <= 0)
            {
                rtnVal = "Night";
            }

            spd.ActiveSheet.Cells[Row, Col].Text = rtnVal;
        }

        /// <summary>
        /// 기본값 세팅 함수
        /// </summary>
        /// <param name="ItemCd"></param>
        public static void DefaultValue(FarPoint.Win.Spread.SheetView spd, string pFLOWGB, FormFlowSheet[] pFormFlowSheet, string ItemCd, string DefaultValue)
        {
            if (pFLOWGB.Equals("ROW"))
            {
                if (spd.RowCount == 0)
                    return;
            }
            else
            {
                if (spd.ColumnCount == 0)
                    return;
            }

            for (int i = 0; i < pFormFlowSheet.Length; i++)
            {
                if(pFormFlowSheet[i].ItemCode.Equals(ItemCd))
                {
                    if (pFLOWGB.Equals("ROW"))
                    {
                        spd.Cells[i, 0].Text = DefaultValue;
                    }
                    else
                    {
                        spd.Cells[0, i].Text = DefaultValue;

                    }
                }
            }
        }

        /// <summary>
        /// 문자열 => CellType
        /// </summary>
        /// <param name="strCellType"></param>
        /// <param name="strMultiLine"></param>
        /// <param name="strCheckTextAlignment"></param>
        /// <param name="strUserMcro"></param>
        /// <param name="pOption"></param>
        /// <returns></returns>
        public static FarPoint.Win.Spread.CellType.ICellType CellType(string strCellType, string strMultiLine, string strCheckTextAlignment, string strUserMcro, string pOption = "W")
        {
            FarPoint.Win.Spread.CellType.TextCellType rtnTextCellType = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType rtnCheckBoxCellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType rtnComboBoxCellType = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            switch (strCellType)
            {
                case "TextCellType":
                    
                    if (strMultiLine == "True")
                    {
                        rtnTextCellType.Multiline = true;
                        rtnTextCellType.MaxLength = 32767;
                        rtnTextCellType.WordWrap = true;
                    }
                    else
                    {
                        rtnTextCellType.Multiline = false;
                    }
                    return rtnTextCellType;
                case "CheckBoxCellType":
                    return rtnCheckBoxCellType;
                case "ComboBoxCellType":
                    if (pOption == "V")
                    {
                        rtnTextCellType.Multiline = false;
                        return rtnTextCellType;
                    }
                    else
                    {
                        
                        return rtnComboBoxCellType;
                    }
                default:
                    
                    if (strMultiLine == "True")
                    {
                        rtnTextCellType.Multiline = true;
                    }
                    else
                    {
                        rtnTextCellType.Multiline = false;
                    }
                    return rtnTextCellType;
            }
        }

        /// <summary>
        /// 문자열 => HorizontalAlignment
        /// </summary>
        /// <param name="strAlignment"></param>
        /// <returns></returns>
        public static FarPoint.Win.Spread.CellHorizontalAlignment HorizontalAlignment(string strAlignment)
        {
            switch (strAlignment)
            {
                case "Left":
                    return FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                case "Center":
                    return FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                case "Right":
                    return FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                default:
                    return FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            }
        }

        /// <summary>
        /// 문자열 => VerticalAlignment
        /// </summary>
        /// <param name="strAlignment"></param>
        /// <returns></returns>
        public static FarPoint.Win.Spread.CellVerticalAlignment VerticalAlignment(string strAlignment)
        {
            switch (strAlignment)
            {
                case "Top":
                    return FarPoint.Win.Spread.CellVerticalAlignment.Top;
                case "Center":
                    return FarPoint.Win.Spread.CellVerticalAlignment.Center;
                case "Bottom":
                    return FarPoint.Win.Spread.CellVerticalAlignment.Bottom;
                default:
                    return FarPoint.Win.Spread.CellVerticalAlignment.Center;
            }
        }

        /// <summary>
        /// 스프래드 콤보 값 세팅
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="sFind"></param>
        public static void PropComboFindEx(FarPoint.Win.Spread.FpSpread spd, int Row, int Col, string sFind)
        {
            int i = 0;

            if (sFind == null) return;
            if (sFind == "") return;
            try
            {
                FarPoint.Win.Spread.CellType.ComboBoxCellType cmbocell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                cmbocell = (FarPoint.Win.Spread.CellType.ComboBoxCellType)spd.ActiveSheet.GetCellType(Row, Col);
                for (i = 0; i <= cmbocell.ListControl.Items.Count - 1; i++)
                {
                    spd.ActiveSheet.Cells[Row, Col].Value = cmbocell.ListControl.Items[i];
                    if (VB.UCase(spd.ActiveSheet.Cells[Row, Col].Text.Trim()) == VB.UCase(sFind.Trim()))
                        return;
                }
                spd.ActiveSheet.Cells[Row, Col].Value = -1;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 스프래드 세팅
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="pFLOWGB"></param>
        /// <param name="pFLOWITEMCNT"></param>
        /// <param name="pFLOWHEADCNT"></param>
        /// <param name="pFormFlowSheet"></param>
        /// <param name="pOption"></param>
        public static void SetInitSpd(FarPoint.Win.Spread.FpSpread spd, string pFLOWGB, int pFLOWITEMCNT, int pFLOWHEADCNT, FormFlowSheet[] pFormFlowSheet, string pOption = "W")
        {
            int i = 0;
            int intHeight = 24;
            int intWidth = 24;

            int intStart = 0;
            int intItem = 0;

            FarPoint.Win.Spread.CellType.TextCellType rtnTextCellType = new FarPoint.Win.Spread.CellType.TextCellType();

            if (pFLOWGB == "ROW") //세로방식(아래로 작성)
            {
                #region //ROW
                for (i = 0; i < pFormFlowSheet.Length; i++)
                {
                    if (intWidth < pFormFlowSheet[i].Width)
                    {
                        intWidth = pFormFlowSheet[i].Width;
                    }
                }

                spd.ActiveSheet.Columns[0].Width = intWidth;

                if (pOption == "V")
                {
                    spd.ActiveSheet.Rows[0].Label = "작성일자";
                    spd.ActiveSheet.Rows[0].CellType = rtnTextCellType;
                    spd.ActiveSheet.Rows[0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ActiveSheet.Rows[0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ActiveSheet.Rows[0].Height = 24;

                    spd.ActiveSheet.Rows[1].Label = "시간";
                    spd.ActiveSheet.Rows[1].CellType = rtnTextCellType;
                    spd.ActiveSheet.Rows[1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ActiveSheet.Rows[1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ActiveSheet.Rows[1].Height = 24;

                    intStart = clsEmrNum.FLOWVIWADD_LEFT;
                }

                for (i = intStart; i < pFormFlowSheet.Length + intStart; i++)
                {
                    spd.ActiveSheet.Rows[i].Label = " ";
                    spd.ActiveSheet.Rows[i].CellType = DesignFunc.CellType(pFormFlowSheet[intItem].CellType, pFormFlowSheet[intItem].MultiLine, pFormFlowSheet[intItem].CheckTextAlignment, pFormFlowSheet[intItem].UserMcro, pOption);
                    spd.ActiveSheet.Rows[i].HorizontalAlignment = DesignFunc.HorizontalAlignment(pFormFlowSheet[intItem].HorizontalAlignment);
                    spd.ActiveSheet.Rows[i].VerticalAlignment = DesignFunc.VerticalAlignment(pFormFlowSheet[intItem].VerticalAlignment);
                    spd.ActiveSheet.Rows[i].Height = pFormFlowSheet[intItem].Height;

                    if (pOption == "W")
                    {
                        spd.ActiveSheet.Rows[i].Locked = false;
                        if (pFormFlowSheet[intItem].CellType == "ComboBoxCellType")
                        {
                            if (pFormFlowSheet[intItem].UserMcro.Trim() != "")
                            {
                                string[] arryUserMcro = VB.Split(pFormFlowSheet[intItem].UserMcro.Trim(), "^");
                                clsSpread.gSpreadComboDataSetEx1(spd, i, 0, i, 0, arryUserMcro, true);
                                if (arryUserMcro.Length > 0 )
                                {
                                    spd.ActiveSheet.Cells[i, 0].Text = arryUserMcro[0];
                                }
                            }
                        }
                    }

                    intItem = intItem + 1;
                }

                if (pOption == "V")
                {
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 4].Label = "작성자";
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 4].CellType = rtnTextCellType;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 4].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 4].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 4].Height = 80;

                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 3].Label = "사본발급";
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 3].CellType = rtnTextCellType;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 3].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 3].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 3].Height = 24;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 3].ForeColor = Color.Red;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 3].Font = new Font("굴림체", 10, FontStyle.Bold);

                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 2].Label = "EMRNO";
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 2].CellType = rtnTextCellType;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 2].Height = 24;

                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 1].Label = "USEID";
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 1].CellType = rtnTextCellType;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ActiveSheet.Rows[spd.ActiveSheet.RowCount - 1].Height = 24;

                }
                #endregion //ROW
            }
            else //가로방식(옆으로 작성)
            {
                #region //COL
                for (i = 0; i < pFormFlowSheet.Length; i++)
                {
                    if (intHeight < pFormFlowSheet[i].Height)
                    {
                        intHeight = pFormFlowSheet[i].Height;
                    }
                }

                spd.ActiveSheet.Rows[0].Height = intHeight;

                if (pOption == "V")
                {
                    spd.ActiveSheet.Columns[0].Label = "작성일자";
                    spd.ActiveSheet.Columns[0].CellType = rtnTextCellType;
                    spd.ActiveSheet.Columns[0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ActiveSheet.Columns[0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ActiveSheet.Columns[0].Width = 30;

                    spd.ActiveSheet.Columns[1].Label = "시간";
                    spd.ActiveSheet.Columns[1].CellType = rtnTextCellType;
                    spd.ActiveSheet.Columns[1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ActiveSheet.Columns[1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ActiveSheet.Columns[1].Width = 16;

                    spd.ActiveSheet.Columns[2].Label = "작성자";
                    spd.ActiveSheet.Columns[2].CellType = rtnTextCellType;
                    spd.ActiveSheet.Columns[2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ActiveSheet.Columns[2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ActiveSheet.Columns[2].Width = 26;

                    intStart = clsEmrNum.FLOWVIWADD_LEFT;
                }
                

                for (i = intStart; i < pFormFlowSheet.Length + intStart; i++)
                {
                    spd.ActiveSheet.Columns[i].Label = " ";
                    spd.ActiveSheet.Columns[i].CellType = DesignFunc.CellType(pFormFlowSheet[intItem].CellType, pFormFlowSheet[intItem].MultiLine, pFormFlowSheet[intItem].CheckTextAlignment, pFormFlowSheet[intItem].UserMcro, pOption);
                    spd.ActiveSheet.Columns[i].HorizontalAlignment = DesignFunc.HorizontalAlignment(pFormFlowSheet[intItem].HorizontalAlignment);
                    spd.ActiveSheet.Columns[i].VerticalAlignment = DesignFunc.VerticalAlignment(pFormFlowSheet[intItem].VerticalAlignment);
                    spd.ActiveSheet.Columns[i].Width = pFormFlowSheet[intItem].Width;
                    
                    if (pOption == "W")
                    {
                        spd.ActiveSheet.Columns[i].Locked = false;
                        if (pFormFlowSheet[intItem].CellType == "ComboBoxCellType")
                        {
                            if (pFormFlowSheet[intItem].UserMcro.Trim() != "")
                            {
                                string[] arryUserMcro = VB.Split(pFormFlowSheet[intItem].UserMcro.Trim(), "^");
                                clsSpread.gSpreadComboDataSetEx1(spd, 0, i, 0, i, arryUserMcro, true);
                                if (arryUserMcro.Length > 0)
                                {
                                    spd.ActiveSheet.Cells[0, i].Text = arryUserMcro[0];
                                }
                            }
                        }
                    }

                    intItem = intItem + 1;
                }

                if (pOption == "V")
                {
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 3].Label = "사본발급";
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 3].CellType = rtnTextCellType;
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 3].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 3].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 3].Width = 60;
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 3].ForeColor = Color.Red;
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 3].Font = new Font("굴림체", 10, FontStyle.Bold);

                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 2].Label = "EMRNO";
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 2].CellType = rtnTextCellType;
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 2].Width = 32;

                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 1].Label = "USEID";
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 1].CellType = rtnTextCellType;
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ActiveSheet.Columns[spd.ActiveSheet.ColumnCount - 1].Width = 32;
                    
                }
                #endregion //COL
            }
        }

        /// <summary>
        /// 스프래드 해드 세팅
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="pFLOWGB"></param>
        /// <param name="pFLOWITEMCNT"></param>
        /// <param name="pFLOWHEADCNT"></param>
        /// <param name="mFormFlowSheetHead"></param>
        /// <param name="pOption"></param>
        public static void SetHead(FarPoint.Win.Spread.SheetView spd, string pFLOWGB, int pFLOWITEMCNT, int pFLOWHEADCNT, FormFlowSheetHead[,] mFormFlowSheetHead, string pOption = "W")
        {
            spd.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            spd.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(220)))), ((int)(((byte)(227)))));
            spd.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            spd.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;

            spd.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            spd.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(220)))), ((int)(((byte)(227)))));
            spd.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            spd.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;

            FarPoint.Win.Spread.CellType.TextCellType rtnTextCellType = new FarPoint.Win.Spread.CellType.TextCellType();
            rtnTextCellType.Multiline = true;
            rtnTextCellType.WordWrap = true;

            int intItem = 0;
            int intStart = 0;

            if (pFLOWGB == "ROW") //세로방식(아래로 작성)
            {
                #region //ROW
                if (pOption == "V")
                {
                    spd.Rows[0].Height = 24;
                    spd.RowHeader.Columns[0].Width = 32;
                    spd.RowHeader.Cells[0, 0].Font = new Font("굴림체", 9);
                    spd.RowHeader.Cells[0, 0].CellType = rtnTextCellType;
                    spd.RowHeader.Cells[0, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.RowHeader.Cells[0, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.RowHeader.Cells[0, 0].Text = "작성 일자";
                    spd.RowHeader.Cells[0, 0].RowSpan = pFLOWHEADCNT;
                    spd.RowHeader.Cells[0, 0].ColumnSpan = 1;
                    spd.Rows[0].Locked = true;

                    spd.Rows[1].Height = 24;
                    spd.RowHeader.Cells[1, 0].Font = new Font("굴림체", 9);
                    spd.RowHeader.Cells[1, 0].CellType = rtnTextCellType;
                    spd.RowHeader.Cells[1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.RowHeader.Cells[1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.RowHeader.Cells[1, 0].Text = "시간";
                    spd.RowHeader.Cells[1, 0].RowSpan = pFLOWHEADCNT;
                    spd.RowHeader.Cells[1, 0].ColumnSpan = 1;
                    spd.Rows[1].Locked = true;
                                       
                    intStart = clsEmrNum.FLOWVIWADD_LEFT;
                }


                for (int i = intStart; i < pFLOWITEMCNT + intStart; i++)
                {
                    for (int j = 0; j < pFLOWHEADCNT; j++)
                    {
                        spd.Rows[i].Height = mFormFlowSheetHead[intItem, j].Height;
                        spd.RowHeader.Columns[j].Width = mFormFlowSheetHead[intItem, j].Width;
                        spd.RowHeader.Cells[i, j].Font = new Font("굴림체", 9);
                        spd.RowHeader.Cells[i, j].CellType = DesignFunc.CellType("TextCellType", mFormFlowSheetHead[intItem, j].MultiLine, "", "");
                        spd.RowHeader.Cells[i, j].HorizontalAlignment = DesignFunc.HorizontalAlignment(mFormFlowSheetHead[intItem, j].HorizontalAlignment);
                        spd.RowHeader.Cells[i, j].VerticalAlignment = DesignFunc.VerticalAlignment(mFormFlowSheetHead[intItem, j].VerticalAlignment);
                        spd.RowHeader.Cells[i, j].Text = mFormFlowSheetHead[intItem, j].Text;
                        spd.RowHeader.Cells[i, j].RowSpan = mFormFlowSheetHead[intItem, j].SpanRow;
                        spd.RowHeader.Cells[i, j].ColumnSpan = mFormFlowSheetHead[intItem, j].SpanCol;
                    }

                    intItem = intItem + 1;
                }

                if (pOption == "V")
                {
                    spd.Rows[spd.RowCount - 4].Height = 24;
                    spd.RowHeader.Cells[spd.RowCount - 4, 0].Font = new Font("굴림체", 9);
                    spd.RowHeader.Cells[spd.RowCount - 4, 0].CellType = rtnTextCellType;
                    spd.RowHeader.Cells[spd.RowCount - 4, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.RowHeader.Cells[spd.RowCount - 4, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.RowHeader.Cells[spd.RowCount - 4, 0].Text = "작성자";
                    spd.RowHeader.Cells[spd.RowCount - 4, 0].RowSpan = pFLOWHEADCNT;
                    spd.RowHeader.Cells[spd.RowCount - 4, 0].ColumnSpan = 1;
                    spd.Rows[spd.RowCount - 4].Locked = true;

                    if(spd.RowHeader.Columns.Count > spd.RowCount -3 )
                    {
                        spd.RowHeader.Columns[spd.RowCount - 3].Width = 60;
                    }
                    spd.RowHeader.Cells[spd.RowCount - 3, 0].Font = new Font("굴림체", 9);
                    spd.RowHeader.Cells[spd.RowCount - 3, 0].CellType = rtnTextCellType;
                    spd.RowHeader.Cells[spd.RowCount - 3, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.RowHeader.Cells[spd.RowCount - 3, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.RowHeader.Cells[spd.RowCount - 3, 0].Text = "사본발급";
                    spd.RowHeader.Cells[spd.RowCount - 3, 0].RowSpan = pFLOWHEADCNT;
                    spd.RowHeader.Cells[spd.RowCount - 3, 0].ColumnSpan = 1;
                    spd.Rows[spd.RowCount - 3].ForeColor = Color.Red;
                    spd.Rows[spd.RowCount - 3].Font = new Font("굴림체", 10, FontStyle.Bold);
                    spd.Rows[spd.RowCount - 3].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                    spd.Rows[spd.RowCount - 3].Locked = true;
                    spd.Rows[spd.RowCount - 3].Visible = true;

                    spd.Rows[spd.RowCount - 2].Height = 24;
                    if (spd.RowHeader.Columns.Count > spd.RowCount - 2)
                    {
                        spd.RowHeader.Columns[spd.RowCount - 2].Width = 32;
                    }
                    spd.RowHeader.Cells[spd.RowCount - 2, 0].Font = new Font("굴림체", 9);
                    spd.RowHeader.Cells[spd.RowCount - 2, 0].CellType = rtnTextCellType;
                    spd.RowHeader.Cells[spd.RowCount - 2, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.RowHeader.Cells[spd.RowCount - 2, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.RowHeader.Cells[spd.RowCount - 2, 0].Text = "EMRNO";
                    spd.RowHeader.Cells[spd.RowCount - 2, 0].RowSpan = pFLOWHEADCNT;
                    spd.RowHeader.Cells[spd.RowCount - 2, 0].ColumnSpan = 1;
                    spd.Rows[spd.RowCount - 2].Locked = true;
                    spd.Rows[spd.RowCount - 2].Visible = false;

                    spd.Rows[spd.RowCount - 1].Height = 24;
                    if (spd.RowHeader.Columns.Count > spd.RowCount - 1)
                    {
                        spd.RowHeader.Columns[spd.RowCount - 1].Width = 32;
                    }
                    spd.RowHeader.Cells[spd.RowCount - 1, 0].Font = new Font("굴림체", 9);
                    spd.RowHeader.Cells[spd.RowCount - 1, 0].CellType = rtnTextCellType;
                    spd.RowHeader.Cells[spd.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.RowHeader.Cells[spd.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.RowHeader.Cells[spd.RowCount - 1, 0].Text = "USEID";
                    spd.RowHeader.Cells[spd.RowCount - 1, 0].RowSpan = pFLOWHEADCNT;
                    spd.RowHeader.Cells[spd.RowCount - 1, 0].ColumnSpan = 1;
                    spd.Rows[spd.RowCount - 1].Locked = true;
                    spd.Rows[spd.RowCount - 1].Visible = false;
                }
                #endregion //ROW
            }
            else //가로방식(옆으로 작성)
            {
                #region //COL
                if (pOption == "V")
                {
                    spd.Columns[0].Width = 70;
                    spd.ColumnHeader.Cells[0, 0].Font = new Font("굴림체", 9);
                    spd.ColumnHeader.Cells[0, 0].CellType = rtnTextCellType;
                    spd.ColumnHeader.Cells[0, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ColumnHeader.Cells[0, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ColumnHeader.Cells[0, 0].Text = "작성일자";
                    spd.ColumnHeader.Cells[0, 0].RowSpan = pFLOWHEADCNT;
                    spd.ColumnHeader.Cells[0, 0].ColumnSpan = 1;
                    spd.Columns[0].Locked = true;

                    spd.Columns[1].Width = 38;
                    spd.ColumnHeader.Cells[0, 1].Font = new Font("굴림체", 9);
                    spd.ColumnHeader.Cells[0, 1].CellType = rtnTextCellType;
                    spd.ColumnHeader.Cells[0, 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ColumnHeader.Cells[0, 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ColumnHeader.Cells[0, 1].Text = "시간";
                    spd.ColumnHeader.Cells[0, 1].RowSpan = pFLOWHEADCNT;
                    spd.ColumnHeader.Cells[0, 1].ColumnSpan = 1;
                    spd.Columns[1].Locked = true;
                    
                    intStart = clsEmrNum.FLOWVIWADD_LEFT;
                }

                for (int i = intStart; i < pFLOWITEMCNT + intStart; i++)
                {
                    for (int j = 0; j < pFLOWHEADCNT; j++)
                    {
                        spd.Columns[i].Width = mFormFlowSheetHead[intItem, j].Width;
                        spd.ColumnHeader.Rows[j].Height = mFormFlowSheetHead[intItem, j].Height;
                        spd.ColumnHeader.Cells[j, i].Font = new Font("굴림체", 9);
                        spd.ColumnHeader.Cells[j, i].CellType = DesignFunc.CellType("TextCellType", mFormFlowSheetHead[intItem, j].MultiLine, "", "");
                        spd.ColumnHeader.Cells[j, i].HorizontalAlignment = DesignFunc.HorizontalAlignment(mFormFlowSheetHead[intItem, j].HorizontalAlignment);
                        spd.ColumnHeader.Cells[j, i].VerticalAlignment = DesignFunc.VerticalAlignment(mFormFlowSheetHead[intItem, j].VerticalAlignment);
                        spd.ColumnHeader.Cells[j, i].Text = mFormFlowSheetHead[intItem, j].Text;
                        spd.ColumnHeader.Cells[j, i].RowSpan = mFormFlowSheetHead[intItem, j].SpanRow;
                        spd.ColumnHeader.Cells[j, i].ColumnSpan = mFormFlowSheetHead[intItem, j].SpanCol;
                    }

                    intItem = intItem + 1;
                }

                if (pOption == "V")
                {
                    spd.Columns[spd.ColumnCount - 4].Width = 48;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 4].Font = new Font("굴림체", 9);
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 4].CellType = rtnTextCellType;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 4].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 4].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 4].Text = "작성자";
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 4].RowSpan = pFLOWHEADCNT;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 4].ColumnSpan = 1;
                    spd.Columns[spd.ColumnCount - 4].Locked = true;

                    spd.Columns[spd.ColumnCount - 3].Width = 60;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 3].Font = new Font("굴림체", 9);
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 3].CellType = rtnTextCellType;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 3].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 3].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 3].Text = "사본발급";
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 3].RowSpan = pFLOWHEADCNT;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 3].ColumnSpan = 1;
                    spd.Columns[spd.ColumnCount - 3].Locked = true;

                    spd.Columns[spd.ColumnCount - 2].Width = 32;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 2].Font = new Font("굴림체", 9);
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 2].CellType = rtnTextCellType;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 2].Text = "EMRNO";
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 2].RowSpan = pFLOWHEADCNT;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 2].ColumnSpan = 1;
                    spd.Columns[spd.ColumnCount - 2].Locked = true;
                    spd.Columns[spd.ColumnCount - 2].Visible = false;

                    spd.Columns[spd.ColumnCount - 1].Width = 32;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 1].Font = new Font("굴림체", 9);
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 1].CellType = rtnTextCellType;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 1].Text = "USEID";
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 1].RowSpan = pFLOWHEADCNT;
                    spd.ColumnHeader.Cells[0, spd.ColumnCount - 1].ColumnSpan = 1;
                    spd.Columns[spd.ColumnCount - 1].Locked = true;
                    spd.Columns[spd.ColumnCount - 1].Visible = false;
                }
                #endregion //COL
            }
        }
    }
}
