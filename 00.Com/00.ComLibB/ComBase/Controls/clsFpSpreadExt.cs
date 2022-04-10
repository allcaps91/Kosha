using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Exceptions;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Validation;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ComBase.Controls
{
    public static class clsFpSpreadExt
    {
        /// <summary>
        /// 스프레드 초기화
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <param name="spreadOption"></param>
        public static void Initialize(this FpSpread fpSpread, SpreadOption spreadOption = null)
        {
            if (spreadOption != null)
            {
                fpSpread.ActiveSheet.ColumnHeader.Rows[0].Height = spreadOption.ColumnHeaderHeight;

            }
            else
            {
                spreadOption = new SpreadOption();
            }

            fpSpread.Tag = spreadOption;

            if (spreadOption.RowHeight > 0)
            {
                fpSpread.ActiveSheet.SetRowHeight(-1, spreadOption.RowHeight);

            }
            fpSpread.AutoClipboard = true;
            fpSpread.ActiveSheet.RowHeaderVisible = spreadOption.RowHeaderVisible;
            fpSpread.ActiveSheet.ColumnHeaderVisible = spreadOption.ColumnHeaderVisible;
            fpSpread.AllowColumnMove = spreadOption.IsColumnMove;
            fpSpread.HorizontalScrollBarPolicy = ScrollBarPolicy.AsNeeded;
            fpSpread.VerticalScrollBarPolicy = ScrollBarPolicy.AsNeeded;
            fpSpread.ShowCellErrors = true;
            fpSpread.ShowRowErrors = true;
            fpSpread.ActiveSheet.Rows.Clear();
            fpSpread.ActiveSheet.Columns.Clear();
            fpSpread.ActiveSheet.AutoGenerateColumns = false;
            fpSpread.ActiveSheet.DataAutoSizeColumns = false;
            fpSpread.ActiveSheet.DataAutoCellTypes = false;
            fpSpread.ActiveSheet.AutoSortEnhancedMode = SortingMode.RangeSorting;
            fpSpread.ActiveSheet.AutoFilterMode = AutoFilterMode.EnhancedContextMenu;

            if (spreadOption.IsRowSelectColor)
            {
                fpSpread.ActiveSheet.SelectionUnit = SelectionUnit.Row;
                fpSpread.ActiveSheet.SelectionBackColor = Color.AliceBlue;
                fpSpread.ActiveSheet.OperationMode = OperationMode.SingleSelect;
                fpSpread.ActiveSheet.ShowRowSelector = true;
                fpSpread.ActiveSheet.SelectionStyle = SelectionStyles.Both;
            }

            InputMap inputMap = new InputMap();

            inputMap.Put(new Keystroke(Keys.Back, Keys.None), SpreadActions.StartEditing);
            inputMap.Put(new Keystroke(Keys.Return, Keys.None), SpreadActions.MoveToNextCellThenControl);
            inputMap.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.C, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCopy);
            inputMap.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.V, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardPaste);

            fpSpread.SetInputMap(InputMapMode.WhenFocused, OperationMode.Normal, inputMap);

            //fpSpread.EditModeStarting += (s, evt) =>
            //{
            //    FpSpread fp = s as FpSpread;
            //    Column col = fp.ActiveSheet.Columns[fp.ActiveSheet.ActiveColumnIndex];
            //    if(col.CellType is NumberCellType)
            //    {
            //        NumberCellType cell = col.CellType as NumberCellType;
            //        cell.FocusPosition = EditorFocusCursorPosition.End;
            //    }
            //};

            //fpSpread.EditorFocused += (s, evt) =>
            //{

            //};

            fpSpread.EditChange += (sender, e) =>
            {
                if (fpSpread.DataSource != null)
                {
                    object value = fpSpread.GetActiveCell().Value;
                    string field = fpSpread.GetActiveCell().Column.DataField;
                    object obj = fpSpread.GetRowData(fpSpread.GetActiveRowIndex());

                    if (fpSpread.Column(e.Column).CellType.GetType() == typeof(DateTimeCellType))
                    {
                        SpreadCellTypeOption opt = fpSpread.Column(e.Column).Tag as SpreadCellTypeOption;
                        if (opt != null)
                        {
                            string orgDataField = opt.OrgFieldName;
                            string copyDataField = opt.CopyFieldName;
                            DateTime dtm;

                            if (value == null)
                            {
                                obj.SetPropertieValue(orgDataField, null);
                            }
                            else
                            {
                                if (DateTime.TryParse(value.ToString(), out dtm))
                                {
                                    obj.SetPropertieValue(orgDataField, dtm.ToString(opt.dbDateTimeType.GetEnumDescription()));
                                }
                            }
                        }
                    }

                    IList list = ((IList)fpSpread.DataSource);
                    if (list != null)
                    {
                        RowStatus status = (RowStatus)fpSpread.GetRowData(e.Row).GetPropertieValue("RowStatus");

                        //object d = fpSpread.GetRowData(e.Row);
                        ////  모델의 지정된 행 인덱스에 대응하는 시트의 행을 가져옵니다
                        //int dd =  fpSpread.ActiveSheet.GetViewRowFromModelRow(e.Row);

                        ////시트의 지정된 행 인덱스에 대응하는 데이터 모델의 행을 가져옵니다
                        //int aa = fpSpread.ActiveSheet.GetModelRowFromViewRow(e.Row);

                        if (status == RowStatus.None)
                        {

                            ((IList)fpSpread.DataSource)[e.Row].SetPropertieValue("RowStatus", RowStatus.Update);
                        }
                    }
                }
            };
            fpSpread.EditModeOff += (sender, e) =>
            {
                Cell cell = fpSpread.GetActiveCell();
                if (cell != null)
                {
                    //  발리데이션 체크 문제로 인한 추가
                    //  SpreadOption.IsValidation 기본 true
                    //  2019-10-12 길광호
                    if (spreadOption.IsValidation)
                    {
                        int column = fpSpread.GetActiveCell().Column.Index;
                        int row = fpSpread.GetActiveCell().Row.Index;
                        object value = fpSpread.GetActiveCell().Value;
                        string filedName = fpSpread.ActiveSheet.Cells[row, column].Column.DataField;
                        bool isValid = true;
                        try
                        {
                            BaseDto dto = fpSpread.GetRowData(row) as BaseDto;
                            if (dto != null)
                            {
                                dto.Validate(filedName, value);
                            }
                        }
                        catch (MTSValidationException ex)
                        {
                            List<MTSValidationResult> list = ex.ValidationResult;
                            foreach (MTSValidationResult result in list)
                            {
                                if (filedName.Equals(result.PropertyName))
                                {
                                    fpSpread.ActiveSheet.SetCellErrorText(row, column, result.Message);
                                    isValid = false;
                                }
                            }
                        }
                        if (isValid)
                        {
                            fpSpread.ActiveSheet.SetCellErrorText(row, column, string.Empty);
                        }
                    }
                    else
                    {
                        IList list = fpSpread.DataSource as IList;
                        if (list != null)
                        {
                            //  수정 체크
                            //2020-03-19 스프레드 세팅시 datafield에 값이 없는경우 RowStatus 값 Return 시 null exception Error 발생
                            RowStatus status = (RowStatus)((IList)fpSpread.DataSource)[fpSpread.GetActiveRowIndex()].GetPropertieValue("RowStatus");
                            if (status == RowStatus.None)
                            {
                                ((IList)fpSpread.DataSource)[fpSpread.GetActiveRowIndex()].SetPropertieValue("RowStatus", RowStatus.Update);
                            }
                        }
                    }
                }
            };

            fpSpread.ButtonClicked += (sender, e) =>
            {
                if (fpSpread.DataSource != null)
                {
                    IList list = fpSpread.DataSource as IList;
                    if (list != null)
                    {
                        if (fpSpread.RowCount() > e.Row)
                        {
                            RowStatus status = (RowStatus)((IList)fpSpread.DataSource)[e.Row].GetPropertieValue("RowStatus");

                            if (status == RowStatus.None)
                            {
                                ((IList)fpSpread.DataSource)[e.Row].SetPropertieValue("RowStatus", RowStatus.Update);
                            }
                        }
                    }
                }

                if ((sender is FpSpread) == false)
                    return;

                Column col = (sender as FpSpread).ActiveSheet.Columns[e.Column];

                SpreadCellTypeOption option = col.Tag as SpreadCellTypeOption;

                if (option == null)
                {
                    return;
                }

                //Trigger 사용 여부 체크 "True"는 넣어 줘야 함.
                if (option.TriggerData.Empty())
                    return;

                //  같은거만 됨
                if (option.TriggerType != TriggerType.Equals)
                {
                    return;
                }

                string dataField = col.DataField;

                if (option.TriggerField.NotEmpty())
                {
                    dataField = option.TriggerField;
                }

                if (dataField.IndexOf("zTemp") > -1)
                {
                    dataField = option.OrgFieldName;
                }

                if (((IList)fpSpread.DataSource) == null)
                    return;

                if (fpSpread.DataSource.ToString().IndexOf("System.ComponentModel.BindingList") < 0)
                {
                    return;
                }

                var row = ((IList)fpSpread.DataSource)[e.Row];

                if (row.GetPropertieValue(dataField) == null)
                {
                    return;
                }

                if (option.TriggerColorType == TriggerColorType.Background)
                {
                    fpSpread.Row(e.Row).BackColor = Color.Empty;
                }
                else
                {
                    fpSpread.Row(e.Row).ForeColor = Color.Empty;
                }

                if (row.GetPropertieValue(dataField).Equals(option.TriggerData) == false)
                {
                    return;
                }

                if (option.TriggerTarget == TriggerTarget.Row)
                {
                    if (option.TriggerColorType == TriggerColorType.Background)
                    {
                        fpSpread.Row(e.Row).BackColor = option.TriggerColor;
                    }
                    else
                    {
                        fpSpread.Row(e.Row).ForeColor = option.TriggerColor;
                    }
                }
                else
                {
                    if (option.TriggerColorType == TriggerColorType.Background)
                    {
                        fpSpread.Cell(e.Row, e.Column).BackColor = option.TriggerColor;
                    }
                    else
                    {
                        fpSpread.Cell(e.Row, e.Column).ForeColor = option.TriggerColor;
                    }
                }

            };


            fpSpread.CellClick += (sender, e) =>
            {
                //if (e.Button == MouseButtons.Right)
                //{
                //    if (System.Diagnostics.Debugger.IsAttached == true)
                //    {
                //    }
                //}
            };

            fpSpread.CellDoubleClick += (sender, e) =>
            {
                if (fpSpread.ActiveSheet.Columns[e.Column].Tag != null)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (fpSpread.ActiveSheet.Columns[e.Column].Tag.ToString() == "DATE")
                        {


                        }
                    }
                }
            };

            fpSpread.ActiveSheet.PropertyChanged += (sender, args) =>
            {
                SheetView sheet = sender as SheetView;
                FpSpread spread = sheet.FpSpread;
                object dataSource = sheet.DataSource;

                if (args.PropertyName == "DataSource" || args.PropertyName == "DataModel")
                {
                    if (dataSource is IBindingList)
                    {
                        //  바인딩 데이터 수정시 스프레드 적용
                        (dataSource as IBindingList).ListChanged += (s, e) =>
                        {
                            IBindingList list = (dataSource as IBindingList);

                            if (list.Count == 0)
                            {
                                return;
                            }

                            if (e.ListChangedType == ListChangedType.ItemAdded)
                            {
                                list[e.NewIndex].SetPropertieValue("RowStatus", RowStatus.Insert);
                                foreach (Column column in fpSpread.Columns())
                                {
                                    if (column.CellType.GetType() == typeof(DateTimeCellType))
                                    {
                                        SpreadCellTypeOption opt = column.Tag as SpreadCellTypeOption;
                                        string orgField = opt.OrgFieldName;
                                        string copyField = opt.CopyFieldName;
                                        string value = list[e.NewIndex].GetPropertieValue(orgField).To(string.Empty);

                                        if (value.NotEmpty())
                                        {
                                            DateTime dtm = DateTime.ParseExact(value, opt.dbDateTimeType.GetEnumDescription(), null);
                                            list[e.NewIndex].SetPropertieValue(copyField, dtm.ToString(opt.dateTimeType.GetEnumDescription()));
                                        }
                                    }
                                    else if (column.CellType.GetType() == typeof(CheckBoxCellType))
                                    {
                                        SpreadCellTypeOption opt = column.Tag as SpreadCellTypeOption;
                                        string orgField = opt.OrgFieldName;
                                        string copyField = opt.CopyFieldName;
                                        string value = list[e.NewIndex].GetPropertieValue(orgField).To(string.Empty);

                                        list[e.NewIndex].SetPropertieValue(orgField, value.Equals(opt.TextTrue) ? opt.TextTrue : opt.TextFalse);
                                        list[e.NewIndex].SetPropertieValue(copyField, value.Equals(opt.TextTrue) ? "True" : "False");
                                    }
                                }
                            }
                            else if (e.ListChangedType == ListChangedType.ItemDeleted)
                            {
                                try
                                {
                                    if ((RowStatus)list[e.NewIndex].GetPropertieValue("RowStatus") == RowStatus.None)
                                    {
                                        list[fpSpread.GetActiveRowIndex()].SetPropertieValue("RowStatus", RowStatus.Delete);
                                    }
                                }
                                catch (Exception ex)
                                { }
                            }
                            else
                            {
                                //list[fpSpread.GetActiveRowIndex()].SetPropertieValue("RowStatus", RowStatus.Update);

                                if ((RowStatus)list[e.NewIndex].GetPropertieValue("RowStatus") == RowStatus.None)
                                {
                                    list[fpSpread.GetActiveRowIndex()].SetPropertieValue("RowStatus", RowStatus.Update);
                                }
                                foreach (Column column in fpSpread.Columns())
                                {
                                    if (column.CellType.GetType() == typeof(DateTimeCellType))
                                    {
                                        SpreadCellTypeOption opt = column.Tag as SpreadCellTypeOption;
                                        string orgField = opt.OrgFieldName;
                                        string copyField = opt.CopyFieldName;
                                        string value = list[e.NewIndex].GetPropertieValue(orgField).To(string.Empty);

                                        if (value.NotEmpty())
                                        {
                                            DateTime dtm = DateTime.ParseExact(value, opt.dbDateTimeType.GetEnumDescription(), null);
                                            list[e.NewIndex].SetPropertieValue(copyField, dtm.ToString(opt.dateTimeType.GetEnumDescription()));
                                        }
                                    }
                                    else if (column.CellType.GetType() == typeof(CheckBoxCellType))
                                    {
                                        SpreadCellTypeOption opt = column.Tag as SpreadCellTypeOption;
                                        string orgField = opt.OrgFieldName;
                                        string copyField = opt.CopyFieldName;
                                        string value = list[e.NewIndex].GetPropertieValue(orgField).To(string.Empty);

                                        list[e.NewIndex].SetPropertieValue(orgField, value.Equals(opt.TextTrue) ? opt.TextTrue : opt.TextFalse);
                                        list[e.NewIndex].SetPropertieValue(copyField, value.Equals(opt.TextTrue) ? "True" : "False");
                                    }
                                }
                            }
                            ColumnOptionCheck(fpSpread.ActiveSheet);
                        };
                    }
                    ColumnOptionCheck(fpSpread.ActiveSheet);
                }
            };
        }

        public static void ColumnOptionCheck(SheetView sheet)
        {
            FpSpread spread = sheet.FpSpread;
            object dataSource = sheet.DataSource;

            int i = 1;
            foreach (Column col in sheet.Columns)
            {
                SpreadCellTypeOption option = col.Tag as SpreadCellTypeOption;
                if (option == null)
                {
                    continue;
                }
                if (col.CellType.GetType() == typeof(DateTimeCellType))
                {
                    col.DataField = string.Concat("zTemp", i.ToString());
                    option.SetCopyFieldName(col.DataField);
                    var list = ((IList)dataSource).Cast<object>();

                    list.ToList().ForEach(r =>
                    {
                        if (r.GetPropertieValue(option.OrgFieldName).NotEmpty())
                        {
                            DateTime dtm = DateTime.ParseExact(r.GetPropertieValue(option.OrgFieldName).ToString(), option.dbDateTimeType.GetEnumDescription(), null);
                            r.SetPropertieValue(col.DataField, dtm.ToString(option.dateTimeType.GetEnumDescription()));
                        }
                    });

                    i++;
                }
                if (col.CellType.GetType() == typeof(CheckBoxCellType))
                {
                    col.DataField = string.Concat("zTemp", i.ToString());
                    option.SetCopyFieldName(col.DataField);
                    if (dataSource != null)
                    {
                        var list = ((IList)dataSource).Cast<object>();

                        list.ToList().ForEach(r =>
                        {
                            if (r.GetPropertieValue(option.OrgFieldName).NotEmpty())
                            {
                                if (option.TextTrue.Equals(r.GetPropertieValue(option.OrgFieldName)))
                                {
                                    r.SetPropertieValue(col.DataField, "True");
                                }
                                else
                                {
                                    r.SetPropertieValue(col.DataField, "False");
                                }
                            }
                            else
                            {
                                r.SetPropertieValue(col.DataField, "False");
                            }
                        });
                    }
                    i++;
                }

                TriggerCheck(option, col, spread);
            }
        }

        public static void TriggerCheck(SpreadCellTypeOption option, Column col, FpSpread fpSpread)
        {
            if (option == null)
            {
                return;
            }
            if ((option.TriggerType != TriggerType.NotEmpty && option.TriggerData.Empty()) || col.CellType.GetType() == typeof(CheckBoxCellType))
            {
                return;
            }

            for (int i = 0; i < fpSpread.RowCount(); i++)
            {
                //fpSpread.Row(i).BackColor = Color.White;
            }

            string dataField = col.DataField;
            if (option.TriggerField.NotEmpty())
            {
                dataField = option.TriggerField;
            }
            object triggerData = option.TriggerData;
            //col.BackColor = Color.White;
            var list = ((IList)fpSpread.DataSource).Cast<object>();
            dynamic result = null;

            if (list.Count() == 0)
            {
                return;
            }
            //  같은거
            if (option.TriggerType == TriggerType.Equals)
            {
                var temp = list.Select((r, idx) => new { idx, item = r.GetPropertieValue(dataField).Equals(option.TriggerData) });
                result = temp.Where(r => r.item.Equals(true));
            }
            //  아닌거
            if (option.TriggerType == TriggerType.NotEquals)
            {
                var temp = list.Select((r, idx) => new { idx, item = r.GetPropertieValue(dataField) == null || !r.GetPropertieValue(dataField).ToString().Equals(option.TriggerData.ToString()) });
                result = temp.Where(r => r.item.Equals(true));
            }
            //  빈값이 아닌거
            else if (option.TriggerType == TriggerType.NotEmpty)
            {
                var temp = list.Select((r, idx) => new { idx, item = r.GetPropertieValue(dataField).NotEmpty() });
                result = temp.Where(r => r.item.Equals(true));
            }
            //  포함된거
            else if (option.TriggerType == TriggerType.In)
            {
                if (triggerData is List<TriggerInfo>)
                {
                    foreach (TriggerInfo info in (triggerData as List<TriggerInfo>))
                    {
                        var temp = list.Select((r, idx) => new
                        {
                            idx,
                            item = r.GetPropertieValue(dataField).NotEmpty()
                                            && r.GetPropertieValue(dataField).ToString().IndexOf(info.data.ToString()) > -1
                        });
                        result = temp.Where(r => r.item.Equals(true));

                        foreach (var item in result)
                        {
                            if (info.triggerTarget == TriggerTarget.Row)
                            {
                                if (info.colorType == TriggerColorType.Background)
                                {
                                    fpSpread.Row((int)item.idx).BackColor = info.color;
                                }
                                else
                                {
                                    fpSpread.Row((int)item.idx).ForeColor = info.color;
                                }
                            }
                            else
                            {
                                if (info.colorType == TriggerColorType.Background)
                                {
                                    fpSpread.Cell((int)item.idx, fpSpread.GetColumnIndex(col.DataField)).BackColor = info.color;
                                }
                                else
                                {
                                    fpSpread.Cell((int)item.idx, fpSpread.GetColumnIndex(col.DataField)).ForeColor = info.color;
                                }
                            }
                        }
                    }

                    return;
                }
                else if (triggerData is string[])
                {
                    foreach (string data in (triggerData as string[]))
                    {
                        var temp = list.Select((r, idx) => new
                        {
                            idx,
                            item = r.GetPropertieValue(dataField).NotEmpty()
                                            && r.GetPropertieValue(dataField).ToString().IndexOf(data.ToString()) > -1
                        });
                        result = temp.Where(r => r.item.Equals(true));

                        foreach (var item in result)
                        {
                            if (option.TriggerTarget == TriggerTarget.Row)
                            {
                                fpSpread.Row((int)item.idx).BackColor = option.TriggerColor;
                            }
                            else
                            {
                                fpSpread.Cell((int)item.idx, fpSpread.GetColumnIndex(dataField)).BackColor = option.TriggerColor;
                            }
                        }
                    }

                    return;
                }
                else
                {
                    var temp = list.Select((r, idx) => new
                    {
                        idx,
                        item = r.GetPropertieValue(dataField).NotEmpty()
                    && r.GetPropertieValue(dataField).ToString().IndexOf(triggerData.ToString()) > -1
                    });
                    result = temp.Where(r => r.item.Equals(true));
                }
            }
            else if (option.TriggerType == TriggerType.Up)
            {
                var temp = list.Select((r, idx) => new
                {
                    idx,
                    item = r.GetPropertieValue(dataField).NotEmpty()
                    && r.GetPropertieValue(dataField).To<decimal>() > triggerData.To<decimal>()
                });
                result = temp.Where(r => r.item.Equals(true));
            }
            else if (option.TriggerType == TriggerType.Down)
            {
                var temp = list.Select((r, idx) => new
                {
                    idx,
                    item = r.GetPropertieValue(dataField).NotEmpty()
                    && r.GetPropertieValue(dataField).To<decimal>() < triggerData.To<decimal>()
                });
                result = temp.Where(r => r.item.Equals(true));
            }

            foreach (var item in result)
            {
                if (option.TriggerTarget == TriggerTarget.Row)
                {
                    if (option.TriggerColorType == TriggerColorType.Background)
                    {
                        fpSpread.Row((int)item.idx).BackColor = option.TriggerColor;
                    }
                    else
                    {
                        fpSpread.Row((int)item.idx).ForeColor = option.TriggerColor;
                    }
                }
                else
                {
                    if (option.TriggerColorType == TriggerColorType.Background)
                    {
                        fpSpread.Cell((int)item.idx, fpSpread.GetColumnIndex(col.DataField)).BackColor = option.TriggerColor;
                    }
                    else
                    {
                        fpSpread.Cell((int)item.idx, fpSpread.GetColumnIndex(col.DataField)).ForeColor = option.TriggerColor;
                    }
                }
            }
        }

        #region CellType별 옵션 지정

        public static Column AddColumn(this FpSpread fpSpread, string caption, string dataField, int width, SpreadCellTypeOption option)
        {
            return AddColumn(fpSpread, caption, dataField, width, FpSpreadCellType.TextCellType, option);
        }

        /// <summary>
        /// 컬럼추가
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <param name="caption"></param>
        /// <param name="dataField"></param>
        /// <param name="width"></param>
        /// <param name="isVisible"></param>
        /// <param name="isEditble"></param>
        /// <returns></returns>
        /// <history>[2019.07.08 김민철]컬럼추가시 정렬여부 추가함</history>
        public static Column AddColumn(this FpSpread fpSpread, string caption, string dataField, int width, FpSpreadCellType cellType = FpSpreadCellType.TextCellType, SpreadCellTypeOption option = null)
        {
            fpSpread.ActiveSheet.Columns.Add(fpSpread.ActiveSheet.Columns.Count, 1);
            Column column = fpSpread.ActiveSheet.Columns[fpSpread.ActiveSheet.Columns.Count - 1];

            if (option == null)
            {
                option = new SpreadCellTypeOption();
            }

            column.DataField = dataField;
            column.Label = caption;
            column.Width = width;
            column.Visible = option.IsVisivle;
            column.Locked = !option.IsEditble;
            column.VerticalAlignment = option.VAligen;
            column.HorizontalAlignment = option.Aligen;
            column.AllowAutoFilter = option.isFilter;
            column.AllowAutoSort = option.IsSort;
            column.BackColor = option.BackColor;
            column.ForeColor = option.ForceColor;
            column.Tag = option;
            column.AllowAutoSort = option.IsSort;
            column.AllowAutoFilter = option.isFilter;
            //column.SortIndicator = SortIndicator.Ascending;
            column.MergePolicy = option.mergePolicy;
            
            string fontName = fpSpread.Font.FontFamily.Name;
            if (option.FontName.NotEmpty())
            {
                fontName = option.FontName;
            }

            float fontSize = fpSpread.Font.Size;
            if (option.FontSize > 0)
            {
                fontSize = option.FontSize;
            }

            Font newFont = new Font(fontName, fontSize, option.FontStyle);
            column.Font = newFont;

            if (option.isFilter)
            {
                fpSpread.ActiveSheet.AutoFilterMode = AutoFilterMode.EnhancedContextMenu;
            }

            switch (cellType)
            {
                case FpSpreadCellType.TextCellType:
                    column.CellType = AddTextColumn(option);
                    break;
                case FpSpreadCellType.DateTimeCellType:
                    column.CellType = AddDateTimeColumn(option);
                    option.SetOrgFieldName(dataField);
                    break;
                case FpSpreadCellType.TimeCellType:
                    column.CellType = AddTimeColumn(option);
                    break;
                case FpSpreadCellType.IDateTimeCellType:
                    column.CellType = AddColumnDateTimeColumn(fpSpread, option);
                    break;
                case FpSpreadCellType.ComboBoxCellType:
                    column.CellType = AddComboBoxColumn(option);
                    break;
                case FpSpreadCellType.CheckBoxCellType:
                    column.CellType = AddCheckBoxColumn(option);
                    option.SetOrgFieldName(dataField);

                    //  체크박스 변경시 원본 데이터 수정
                    (column.CellType as CheckBoxCellType).EditorValueChanged += (s, e) =>
                    {
                        CheckBoxCellType checkBoxCellType = s as CheckBoxCellType;
                        int row = fpSpread.GetActiveRowIndex();
                        int col = fpSpread.GetActiveColIndex();
                        bool isCheck = fpSpread.Cell(row, col).Value.To<bool>();

                        SpreadCellTypeOption opt = fpSpread.Column(col).Tag as SpreadCellTypeOption;
                        string orgDataField = opt.OrgFieldName;
                        string copyDataField = opt.CopyFieldName;

                        if (isCheck)
                        {
                            ((IList)fpSpread.DataSource)[row].SetPropertieValue(orgDataField, option.TextTrue);
                        }
                        else
                        {
                            ((IList)fpSpread.DataSource)[row].SetPropertieValue(orgDataField, option.TextFalse);
                            //fpSpread.Row(row).BackColor = Color.Empty;
                        }
                    };

                    break;
                case FpSpreadCellType.ImageCellType:
                    column.CellType = AddImageColumn(option);
                    break;
                case FpSpreadCellType.ButtonCellType:
                    column.CellType = AddButtonColumn(option);
                    break;
                case FpSpreadCellType.ProgressCellType:
                    column.CellType = AddProgressColumn(option);
                    break;
                case FpSpreadCellType.ColorPickerCellType:
                    column.CellType = AddColorPickerColumn(option);
                    break;
                case FpSpreadCellType.CurrencyCellType:
                    column.CellType = AddCurrencyColumn(option);
                    break;
                case FpSpreadCellType.HyperLinkCellType:
                    column.CellType = AddHyperLinkColumn(option);
                    break;
                case FpSpreadCellType.ListBoxCellType:
                    column.CellType = AddListBoxColumn(option);
                    break;
                case FpSpreadCellType.MaskCellType:
                    column.CellType = AddMaskColumn(option);
                    break;
                case FpSpreadCellType.MultiOptionCellType:
                    column.CellType = AddMultiOptionColumn(option);
                    break;
                case FpSpreadCellType.MultiColumnComboBoxCellType:
                    column.CellType = AddMultiColumnComboBoxColumn(option);
                    break;
                case FpSpreadCellType.NumberCellType:
                    column.CellType = AddNumberColumn(option);
                    break;
                case FpSpreadCellType.PercentCellType:

                    column.CellType = AddPercentColumn(option);
                    break;
                case FpSpreadCellType.RichTextCellType:
                    column.CellType = AddRichTextColumn(option);
                    break;
                case FpSpreadCellType.SliderCellType:
                    column.CellType = AddSliderColumn(option);
                    break;
                case FpSpreadCellType.RegularExpressionCellType:
                    column.CellType = AddRegularExpressionColumn(option);
                    break;
            }

            return column;
        }

        /// <summary>
        /// 칼럼 변경
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <param name="columnIndex"></param>
        /// <param name="caption"></param>
        /// <param name="dataField"></param>
        /// <param name="width"></param>
        /// <param name="cellType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static Column ChangeColumn(this FpSpread fpSpread, int columnIndex, string caption, string dataField, int width, FpSpreadCellType cellType = FpSpreadCellType.TextCellType, SpreadCellTypeOption option = null)
        {
            Column column = fpSpread.ActiveSheet.Columns[columnIndex];

            if (option == null)
            {
                option = new SpreadCellTypeOption();
            }

            column.DataField = dataField;
            column.Label = caption;
            column.Width = width;
            column.Visible = option.IsVisivle;
            column.Locked = !option.IsEditble;
            column.VerticalAlignment = option.VAligen;
            column.HorizontalAlignment = option.Aligen;
            column.AllowAutoFilter = option.isFilter;
            column.AllowAutoSort = option.IsSort;
            column.BackColor = option.BackColor;
            column.ForeColor = option.ForceColor;
            column.Tag = option.Tag;
            column.MergePolicy = option.mergePolicy;

            switch (cellType)
            {
                case FpSpreadCellType.TextCellType:
                    column.CellType = AddTextColumn(option);
                    break;
                case FpSpreadCellType.DateTimeCellType:
                    column.CellType = AddDateTimeColumn(option);
                    break;
                case FpSpreadCellType.TimeCellType:
                    column.CellType = AddTimeColumn(option);
                    break;
                case FpSpreadCellType.IDateTimeCellType:
                    column.CellType = AddColumnDateTimeColumn(fpSpread, option);
                    break;
                case FpSpreadCellType.ComboBoxCellType:
                    column.CellType = AddComboBoxColumn(option);
                    break;
                case FpSpreadCellType.CheckBoxCellType:
                    column.CellType = AddCheckBoxColumn(option);
                    break;
                case FpSpreadCellType.ImageCellType:
                    column.CellType = AddImageColumn(option);
                    break;
                case FpSpreadCellType.ButtonCellType:
                    column.CellType = AddButtonColumn(option);
                    break;
                case FpSpreadCellType.ProgressCellType:
                    column.CellType = AddProgressColumn(option);
                    break;
                case FpSpreadCellType.ColorPickerCellType:
                    column.CellType = AddColorPickerColumn(option);
                    break;
                case FpSpreadCellType.CurrencyCellType:
                    column.CellType = AddCurrencyColumn(option);
                    break;
                case FpSpreadCellType.HyperLinkCellType:
                    column.CellType = AddHyperLinkColumn(option);
                    break;
                case FpSpreadCellType.ListBoxCellType:
                    column.CellType = AddListBoxColumn(option);
                    break;
                case FpSpreadCellType.MaskCellType:
                    column.CellType = AddMaskColumn(option);
                    break;
                case FpSpreadCellType.MultiOptionCellType:
                    column.CellType = AddMultiOptionColumn(option);
                    break;
                case FpSpreadCellType.MultiColumnComboBoxCellType:
                    column.CellType = AddMultiColumnComboBoxColumn(option);
                    break;
                case FpSpreadCellType.NumberCellType:
                    column.CellType = AddNumberColumn(option);
                    break;
                case FpSpreadCellType.PercentCellType:
                    column.CellType = AddPercentColumn(option);
                    break;
                case FpSpreadCellType.RichTextCellType:
                    column.CellType = AddRichTextColumn(option);
                    break;
                case FpSpreadCellType.SliderCellType:
                    column.CellType = AddSliderColumn(option);
                    break;
                case FpSpreadCellType.RegularExpressionCellType:
                    column.CellType = AddRegularExpressionColumn(option);
                    break;
            }

            return column;
        }

        private static TextCellType AddTextColumn(SpreadCellTypeOption option)
        {
            TextCellType textCellType = new TextCellType();
            textCellType.Multiline = option.IsMulti;
            textCellType.WordWrap = option.WordWrap;
            textCellType.MaxLength = option.TextMaxLength;
            
            return textCellType;
        }

        private static DateTimeCellType AddDateTimeColumn(SpreadCellTypeOption option)
        {
            DateTimeCellType dateTimeCellType = new DateTimeCellType();

            dateTimeCellType.DateTimeFormat = DateTimeFormat.UserDefined;
            dateTimeCellType.UserDefinedFormat = option.dateTimeType.GetEnumDescription();// "yyyy-MM";
            dateTimeCellType.EditorValue = option.dateTimeEditorValue;
            dateTimeCellType.DropDownButton = option.IsShowCalendarButton;
            dateTimeCellType.NullDisplay = "";
            dateTimeCellType.EditorValueChanged += (s, e) =>
            {

            };
            return dateTimeCellType;
        }

        private static DateTimeCellType AddTimeColumn(SpreadCellTypeOption option)
        {
            DateTimeCellType dateTimeCellType = new DateTimeCellType();

            dateTimeCellType.DateTimeFormat = DateTimeFormat.UserDefined;
            dateTimeCellType.UserDefinedFormat = "HH:mm";
            dateTimeCellType.DropDownButton = false;
            return dateTimeCellType;
        }

        private static DateTimeCellType AddColumnDateTimeColumn(FpSpread fpSpread, SpreadCellTypeOption option)
        {
            ColumnDateTimeCellType dateTimeCellType;
            if (option.ICustomCellType != null)
            {
                dateTimeCellType = (ColumnDateTimeCellType)option.ICustomCellType;
                (dateTimeCellType as ICustomCellType).fpspread = fpSpread;
            }
            else
            {
                dateTimeCellType = new ColumnDateTimeCellType();
            }
            //날짜데이타형식
            dateTimeCellType.DateTimeFormat = DateTimeFormat.UserDefined;
            dateTimeCellType.UserDefinedFormat = option.dateTimeType.GetEnumDescription();
            dateTimeCellType.EditorValue = option.dateTimeEditorValue;
            dateTimeCellType.DropDownButton = option.IsShowCalendarButton;

            return dateTimeCellType;
        }

        private static ComboBoxCellType AddComboBoxColumn(SpreadCellTypeOption option)
        {
            ComboBoxCellType comboBoxCellType = new ComboBoxCellType();
            if (option.Items != null)
            {
                List<string> items = new List<string>();
                List<string> itemData = new List<string>();
                foreach (var item in option.Items)
                {
                    string data = item.GetPropertieValue(option.ValueMember).ToString();
                    string display = item.GetPropertieValue(option.DisplayMember).ToString();

                    itemData.Add(data);
                    if (option.IsValueMember)
                    {
                        items.Add(string.Concat(data, ".", display));
                    }
                    else
                    {
                        items.Add(display);
                    }
                }

                comboBoxCellType.Items = items.ToArray();
                comboBoxCellType.ItemData = itemData.ToArray();
            }

            comboBoxCellType.EditorValue = EditorValue.ItemData;
            return comboBoxCellType;
        }

        private static CheckBoxCellType AddCheckBoxColumn(SpreadCellTypeOption option)
        {
            CheckBoxCellType checkBoxCellType = new CheckBoxCellType();
            return checkBoxCellType;
        }

        private static ImageCellType AddImageColumn(SpreadCellTypeOption option)
        {
            ImageCellType imageCellType = new ImageCellType();
            return imageCellType;
        }

        private static ButtonCellType AddButtonColumn(SpreadCellTypeOption option)
        {
            ButtonCellType buttonCellType = new ButtonCellType() { Text = option.ButtonText };
            return buttonCellType;
        }

        private static ProgressCellType AddProgressColumn(SpreadCellTypeOption option)
        {
            ProgressCellType progressCellType = new ProgressCellType();
            return progressCellType;
        }
        private static ColorPickerCellType AddColorPickerColumn(SpreadCellTypeOption option)
        {
            ColorPickerCellType colorPickerCellType = new ColorPickerCellType();
            return colorPickerCellType;
        }
        private static CurrencyCellType AddCurrencyColumn(SpreadCellTypeOption option)
        {
            CurrencyCellType currencyCellType = new CurrencyCellType();
            return currencyCellType;
        }
        private static HyperLinkCellType AddHyperLinkColumn(SpreadCellTypeOption option)
        {
            HyperLinkCellType hyperLinkCellType = new HyperLinkCellType();
            return hyperLinkCellType;
        }
        private static ListBoxCellType AddListBoxColumn(SpreadCellTypeOption option)
        {
            ListBoxCellType listBoxCellType = new ListBoxCellType();
            return listBoxCellType;
        }
        private static MaskCellType AddMaskColumn(SpreadCellTypeOption option)
        {
            MaskCellType maskCellType = new MaskCellType();
            maskCellType.Mask = option.Mask;
            return maskCellType;
        }
        private static MultiOptionCellType AddMultiOptionColumn(SpreadCellTypeOption option)
        {
            MultiOptionCellType multiOptionCellType = new MultiOptionCellType();
            return multiOptionCellType;
        }
        private static MultiColumnComboBoxCellType AddMultiColumnComboBoxColumn(SpreadCellTypeOption option)
        {
            MultiColumnComboBoxCellType multiColumnComboBoxCellType = new MultiColumnComboBoxCellType();
            return multiColumnComboBoxCellType;
        }
        private static NumberCellType AddNumberColumn(SpreadCellTypeOption option)
        {
            NumberCellType numberCellType = new NumberCellType();
            numberCellType.Separator = ",";
            numberCellType.ShowSeparator = true;
            numberCellType.DecimalPlaces = option.DecimalPlaces;
            numberCellType.MaximumValue = 9999999999999;
            numberCellType.MinimumValue = -9999999999999;
            numberCellType.FocusPosition = EditorFocusCursorPosition.End;
            numberCellType.NegativeRed = option.NegativeRed;
            return numberCellType;
        }
        private static PercentCellType AddPercentColumn(SpreadCellTypeOption option)
        {
            PercentCellType percentCellType = new PercentCellType();
            percentCellType.DecimalPlaces = option.DecimalPlaces;
            percentCellType.MaximumValue = 100000000000000D;
            percentCellType.MinimumValue = -100000000000000D;
            percentCellType.PercentSign = "%";
            //percentCellType.ShowSeparator = true;
            //percentCellType.Separator = ",";

            return percentCellType;
        }
        private static RichTextCellType AddRichTextColumn(SpreadCellTypeOption option)
        {
            RichTextCellType richTextCellType = new RichTextCellType();
            return richTextCellType;
        }
        private static SliderCellType AddSliderColumn(SpreadCellTypeOption option)
        {
            SliderCellType sliderCellType = new SliderCellType();
            return sliderCellType;
        }
        private static RegularExpressionCellType AddRegularExpressionColumn(SpreadCellTypeOption option)
        {
            RegularExpressionCellType regularExpressionCellType = new RegularExpressionCellType();
            return regularExpressionCellType;
        }

        #endregion

        /// <summary>
        /// 헤더에 체크박스 설정
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <param name="fieldName"></param>
        /// <param name="cellType"></param>
        /// <returns></returns>
        public static Column SetColumnHead(this FpSpread fpSpread, string fieldName, FpSpreadCellType cellType)
        {
            int colIdx = -1;
            for (int i = 0; i < fpSpread.ActiveSheet.Columns.Count; i++)
            {
                string dataField = fpSpread.ActiveSheet.Columns[i].DataField;
                if (fieldName.Equals(dataField))
                {
                    colIdx = i;
                    break;
                }
            }

            if (colIdx < 0)
            {
                return null;
            }

            Column col = fpSpread.ActiveSheet.ColumnHeader.Columns[colIdx];
            col.Locked = false;
            col.CellType = new CheckBoxCellType();
            col.HorizontalAlignment = CellHorizontalAlignment.Center;
            col.VerticalAlignment = CellVerticalAlignment.Center;
            return col;
        }

        public static Cells HeaderCells(this FpSpread fpSpread)
        {
            return fpSpread.ActiveSheet.ColumnHeader.Cells;
        }

        public static Cell HeaderCell(this FpSpread fpSpread, int row, int col)
        {
            return fpSpread.ActiveSheet.ColumnHeader.Cells[row, col];
        }

        public static void ShowStatusBar(this FpSpread fpSpread)
        {
            fpSpread.StatusBarVisible = true;
        }

        public static void HideStatusBar(this FpSpread fpSpread)
        {
            fpSpread.StatusBarVisible = false;
        }

        public static void SetHeaderColor(this FpSpread fpSpread, Color columnHeaderColor)
        {
            for (int i = 0; i < fpSpread.ActiveSheet.ColumnHeader.Columns.Count; i++)
            {
                fpSpread.ActiveSheet.ColumnHeader.Columns[i].BackColor = columnHeaderColor;
            }
        }

        public static ColumnNumber AddColumnNumber(this FpSpread fpSpread, string caption, string dataField, int width, SpreadCellTypeOption option = null)
        {
            if (option == null)
            {
                option = new SpreadCellTypeOption();
            }
            ColumnNumber column = new ColumnNumber(fpSpread, caption, dataField, width, option);
            return column;
        }


        public static ColumnText AddColumnText(this FpSpread fpSpread, string caption, string dataField, int width, IsReadOnly isReadOnly = IsReadOnly.Y, SpreadCellTypeOption option = null)
        {
            if (option == null)
            {
                option = new SpreadCellTypeOption();
            }
            ColumnText column = new ColumnText(fpSpread, caption, dataField, width, isReadOnly, option);
            return column;
        }
        public static ColumnButton AddColumnButton(this FpSpread fpSpread, string caption, int width, SpreadCellTypeOption option = null)
        {
            if (option == null)
            {
                option = new SpreadCellTypeOption();
            }
            ColumnButton column = new ColumnButton(fpSpread, caption, "", width, option);
            return column;
        }
        public static ColumnCheckBox AddColumnCheckBox(this FpSpread fpSpread, string caption, string dataField, int width, ICheckBoxCellType checkBoxCellType, SpreadCellTypeOption option = null)
        {
            if (option == null)
            {
                option = new SpreadCellTypeOption();
            }
            ColumnCheckBox column = new ColumnCheckBox(fpSpread, caption, dataField, width, checkBoxCellType, option);
            return column;
        }
        public static ColumnDateTime AddColumnDateTime(this FpSpread fpSpread, string caption, string dataField, int width, IsReadOnly isReadOnly, DateTimeType dateTimeType = DateTimeType.YYYY_MM_DD, SpreadCellTypeOption option = null)
        {
            if (option == null)
            {
                option = new SpreadCellTypeOption();
            }
            ColumnDateTime column = new ColumnDateTime(fpSpread, caption, dataField, width, isReadOnly, dateTimeType, option);
            return column;
        }
        public static ColumnComboBox AddColumnComboBox(this FpSpread fpSpread, string caption, string dataField, int width, IsReadOnly isReadOnly = IsReadOnly.N, SpreadComboBoxData comboBoxData = null, SpreadCellTypeOption option = null)
        {
            if (option == null)
            {
                option = new SpreadCellTypeOption();
            }
            ColumnComboBox column = new ColumnComboBox(fpSpread, caption, dataField, width, isReadOnly, comboBoxData, option);
            return column;
        }
        public static ColumnListBox AddColumnListBox(this FpSpread fpSpread, string caption, string dataField, int width, ImageList imageList, SpreadCellTypeOption option = null)
        {
            if (option == null)
            {
                option = new SpreadCellTypeOption();
            }
            ColumnListBox column = new ColumnListBox(fpSpread, caption, dataField, width, imageList, option);
            return column;
        }
        public static ColumnImage AddColumnImage(this FpSpread fpSpread, string caption, string dataField, int width, SpreadCellTypeOption option = null)
        {
            if (option == null)
            {
                option = new SpreadCellTypeOption();
            }
            ColumnImage column = new ColumnImage(fpSpread, caption, dataField, width, option);
            return column;
        }

        ///// <summary>
        ///// 유효성검사 확장 메서드
        ///// </summary>
        ///// <param name="col"></param>
        ///// <param name="mtsValidation"></param>
        //public static void Validation(this AbstractColumn column, MTSValidation mtsValidation)
        //{
        //    column.MTSValidation = mtsValidation;
        //    column.SetCellTypeValidation(mtsValidation);
        //}
        /// <summary>
        /// 편집가능한 스프레드의 에러 텍스트 여부를 확인합니다
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <returns></returns>
        public static bool Validate(this FpSpread fpSpread)
        {
            //성능에 문제가 된다면 수정된 데이타만 유효성검사하는 방법 강구해야함. 수정된 dto의 col, row 인덱스를 저장 해놓고
            //fpSpread.ActiveSheet.SetCellErrorText(e.Row, e.Column, result.Message); 에러를 표시할수 있음
            //IList<T> list = fpSpread.GetEditbleData<T>();
            //for(int j=0; j< list.Count; j++)
            //{

            //        BaseDto dto = list[j] as BaseDto;
            //        dto.Validate();


            //}
            bool isValid = true;
            for (int i = 0; i < fpSpread.ActiveSheet.RowCount; i++)
            {
                BaseDto dto = fpSpread.GetRowData(i) as BaseDto;
                if (dto == null)
                {

                    throw new MTSException("바인딩 되지 않은 로우가 있습니다, datasource 초기화를 확인하세요");
                }
                for (int j = 0; j < fpSpread.ActiveSheet.ColumnCount; j++)
                {
                    string fieldName = fpSpread.ActiveSheet.Columns[j].DataField;
                    try
                    {
                        dto.Validate(fieldName, fpSpread.ActiveSheet.Cells[i, j].Value);
                    }
                    catch (MTSValidationException ex)
                    {
                        foreach (MTSValidationResult result in ex.ValidationResult)
                        {
                            if (fieldName.Equals(result.PropertyName))
                            {
                                fpSpread.ActiveSheet.SetCellErrorText(i, j, result.Message);
                                isValid = false;
                            }
                        }

                    }

                }
            }
            return isValid;
        }



        /// <summary>
        /// 로우개수
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <returns></returns>
        public static int RowCount(this FpSpread fpSpread)
        {
            return fpSpread.ActiveSheet.RowCount;
        }

        /// <summary>
        /// 컬럼 개수
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <returns></returns>
        public static int ColumnCount(this FpSpread fpSpread)
        {
            return fpSpread.ActiveSheet.ColumnCount;
        }

        /// <summary>
        /// 로우추가
        /// 기본값 세팅하기 -> DTO 생성자에서 기본값 설정
        /// </summary>
        /// <param name="fpSpread"></param>
        public static int AddRows(this FpSpread fpSpread, int addRowCount = 1)
        {
            int rowCount = fpSpread.RowCount();

            fpSpread.ActiveSheet.AddRows(fpSpread.RowCount(), addRowCount);
            if (fpSpread.DataSource != null)
            {
                IList list = ((IList)fpSpread.DataSource);
                if (list != null)
                {
                    for (int i = rowCount; i < fpSpread.RowCount(); i++)
                    {
                        ((IList)fpSpread.DataSource)[i].SetPropertieValue("RowStatus", RowStatus.Insert);
                        fpSpread.ActiveSheet.ActiveRowIndex = i;
                    }
                }
                else
                {
                    for (int i = rowCount; i < fpSpread.RowCount(); i++)
                    {
                        ((IBindingList)fpSpread.DataSource)[i].SetPropertieValue("RowStatus", RowStatus.Insert);
                        fpSpread.ActiveSheet.ActiveRowIndex = i;
                    }
                }
            }
            fpSpread.SetViewportTopRow(0, fpSpread.RowCount());
            return fpSpread.RowCount() - 1;
        }

        /// <summary>
        /// 로우 삽입
        /// </summary>
        /// <param name="fpSpread"></param>
        public static void InsertRows(this FpSpread fpSpread, int rowIndex, int addRowCount = 1)
        {
            int maxIndex = rowIndex + addRowCount;

            fpSpread.ActiveSheet.ActiveRowIndex = rowIndex;

            fpSpread.ActiveSheet.AddRows(rowIndex, addRowCount);
            //fpSpread.SetViewportTopRow(rowIndex, fpSpread.RowCount());
            fpSpread.ResumeLayout();
            fpSpread.Refresh();

            if (fpSpread.DataSource != null)
            {
                IList list = ((IList)fpSpread.DataSource);
                if (list != null)
                {
                    for (int i = rowIndex; i < maxIndex; i++)
                    {
                        object data = fpSpread.GetRowData((fpSpread.RowCount() - 1) + i);
                        data.SetPropertieValue("RowStatus", RowStatus.Insert);
                        //((IList)fpSpread.DataSource)[i].SetPropertieValue("RowStatus", RowStatus.Insert);
                    }

                    //((IList)fpSpread.DataSource)[fpSpread.RowCount() - 1].SetPropertieValue("RowStatus", RowStatus.Insert);
                }
                else
                {
                    for (int i = rowIndex; i < maxIndex; i++)
                    {
                        object data = fpSpread.GetRowData((fpSpread.RowCount() - 1) + i);
                        data.SetPropertieValue("RowStatus", RowStatus.Insert);

                        //((IBindingList)fpSpread.DataSource)[i].SetPropertieValue("RowStatus", RowStatus.Insert);
                    }

                    //((IBindingList)fpSpread.DataSource)[fpSpread.RowCount() - 1].SetPropertieValue("RowStatus", RowStatus.Insert);
                }

                fpSpread.ResumeLayout();
            }
        }

        /// <summary>
        /// 로우삭제
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <param name="row"></param>
        public static void DeleteRow(this FpSpread fpSpread, int row = -1)
        {
            int rowIndex = row == -1 ? fpSpread.ActiveSheet.ActiveRowIndex : row;

            if (fpSpread.DataSource != null)
            {
                IList list = ((IList)fpSpread.DataSource);
                if (list != null)
                {
                    RowStatus status = (RowStatus)((IList)fpSpread.DataSource)[rowIndex].GetPropertieValue("RowStatus");

                    if (status == RowStatus.Insert)
                    {
                        fpSpread.ActiveSheet.RemoveRows(rowIndex, 1);
                        return;
                    }
                    else if (status == RowStatus.Delete)
                    {
                        ((IList)fpSpread.DataSource)[rowIndex].SetPropertieValue("RowStatus", RowStatus.None);
                        fpSpread.ActiveSheet.Rows[rowIndex].ResetFont();
                    }
                    else
                    {
                        ((IList)fpSpread.DataSource)[rowIndex].SetPropertieValue("RowStatus", RowStatus.Delete);
                        Font font = new Font("굴림", 9, FontStyle.Regular);
                        fpSpread.ActiveSheet.Rows[rowIndex].Font = new Font(font, FontStyle.Strikeout);
                    }
                }
                else
                {
                    RowStatus status = (RowStatus)((IBindingList)fpSpread.DataSource)[rowIndex].GetPropertieValue("RowStatus");

                    if (status == RowStatus.Insert)
                    {
                        fpSpread.ActiveSheet.RemoveRows(rowIndex, 1);
                        return;
                    }
                    else if (status == RowStatus.Delete)
                    {
                        ((IBindingList)fpSpread.DataSource)[rowIndex].SetPropertieValue("RowStatus", RowStatus.None);
                        fpSpread.ActiveSheet.Rows[rowIndex].ResetFont();
                    }
                    else
                    {
                        ((IBindingList)fpSpread.DataSource)[rowIndex].SetPropertieValue("RowStatus", RowStatus.Delete);
                        Font font = new Font("굴림", 9, FontStyle.Regular);
                        fpSpread.ActiveSheet.Rows[rowIndex].Font = new Font(font, FontStyle.Strikeout);
                    }
                }
            }
            else
            {
                fpSpread.ActiveSheet.RemoveRows(rowIndex, 1);
            }
        }

        /// <summary>
        /// 로우데이터
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static object GetRowData(this FpSpread fpSpread, int row)
        {
            if (fpSpread.DataSource != null)
            {
                //  모델의 지정된 행 인덱스에 대응하는 시트의 행을 가져옵니다
                //  fpSpread.ActiveSheet.GetViewRowFromModelRow(row);

                //시트의 지정된 행 인덱스에 대응하는 데이터 모델의 행을 가져옵니다
                //  fpSpread.ActiveSheet.GetModelRowFromViewRow(row);

                int index = fpSpread.ActiveSheet.GetModelRowFromViewRow(row);
                if (index == -1)
                {
                    return null;
                }

                if (fpSpread.DataSource is IList)
                {
                    IList collection = (IList)fpSpread.DataSource;
                    if (collection == null)
                    {
                        IBindingList bindingList = (IBindingList)fpSpread.DataSource;
                        return bindingList[index];
                    }

                    return collection[index];
                }
                else
                {
                    DataTable dt = fpSpread.DataSource as DataTable;
                    if (dt != null)
                    {
                        return dt.Rows[index];
                    }
                }
            }

            return null;
        }

        public static void SetRowData(this FpSpread fpSpread, int row, BaseDto dto)
        {
            for (int i = 0; i < fpSpread.Columns().Count; i++)
            {
                Column col = fpSpread.Column(i);
                if (col.CellType is DateTimeCellType)
                {
                    if (col.Tag is SpreadCellTypeOption)
                    {
                        SpreadCellTypeOption option = col.Tag as SpreadCellTypeOption;

                        if (dto.GetPropertieValue(option.OrgFieldName).NotEmpty())
                        {
                            DateTime dtm = DateTime.ParseExact(dto.GetPropertieValue(option.OrgFieldName).ToString(), option.dbDateTimeType.GetEnumDescription(), null);
                            dto.SetPropertieValue(col.DataField, dtm.ToString(option.dateTimeType.GetEnumDescription()));
                        }
                    }
                }
            }

            ((IList)fpSpread.DataSource)[row] = dto;
            fpSpread.ResumeLayout();
        }

        /// <summary>
        /// 데이타소스 바인딩
        /// SpreadOption.RowHeightAuto = true 이며 Row 높이를 자동으로 조절한다.
        /// SpreadOption.RowHeight를 명시적으로 지정하여 사용할경우는 RowHeightAuto는 적용되지 않늗다
        /// TODO : dhkim 2019-10-15 건수가 많을경우 Rows[i].GetPreferredHeight() 느린 현상이 있어 주석처리함 다른 방법 모색중.
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <param name="collection"></param>
        public static void SetDataSource(this FpSpread fpSpread, IList collection)
        {
            fpSpread.ActiveSheet.RowCount = 0;
            fpSpread.DataSource = collection;


            SpreadOption option = fpSpread.Tag as SpreadOption;
            if (option != null)
            {

                if (option.RowHeightAuto && option.RowHeight == 0)
                {
                    for (int i = 0; i < fpSpread.RowCount(); i++)
                    {
                        fpSpread.ActiveSheet.Rows[i].Height = fpSpread.ActiveSheet.Rows[i].GetPreferredHeight();
                    }
                }
                if (option.ColumnHeaderColor != Color.Empty)
                {
                    for (int i = 0; i < fpSpread.ActiveSheet.ColumnHeader.Columns.Count; i++)
                    {
                        fpSpread.ActiveSheet.ColumnHeader.Columns[i].BackColor = option.ColumnHeaderColor;
                    }
                }

                for (int rowIndex = 0; rowIndex < fpSpread.RowCount(); rowIndex++)
                {
                    RowStatus status = RowStatus.None;

                    if (((IList)fpSpread.DataSource)[rowIndex].GetPropertieValue("RowStatus") != null)
                    {
                        status = (RowStatus)((IList)fpSpread.DataSource)[rowIndex].GetPropertieValue("RowStatus");
                        if (status == RowStatus.Insert)
                        {
                            //fpSpread.ActiveSheet.RemoveRows(rowIndex, 1);
                            //return;
                        }
                        else if (status == RowStatus.Delete)
                        {
                            //((IList)fpSpread.DataSource)[rowIndex].SetPropertieValue("RowStatus", RowStatus.Delete);

                            if (fpSpread.ActiveSheet.Columns[0].CellType is CheckBoxFlagEnumCellType<IsDeleted>)
                            {
                                Font font = new Font("굴림", 9, FontStyle.Regular);
                                fpSpread.ActiveSheet.Rows[rowIndex].Font = new Font(font, FontStyle.Strikeout);
                                fpSpread.ActiveSheet.Cells[rowIndex, 0].Value = IsDeleted.Y;
                            }

                        }
                        else
                        {
                            ((IList)fpSpread.DataSource)[rowIndex].SetPropertieValue("RowStatus", RowStatus.None);
                            fpSpread.ActiveSheet.Rows[rowIndex].ResetFont();
                        }
                    }



                    //    fpSpread.ActiveSheet.Rows[i].Height = fpSpread.ActiveSheet.Rows[i].GetPreferredHeight();
                }

            }
            //fpSpread.ResumeLayout();
        }

        /// <summary>
        /// 수정된 데이터 찾기
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fpSpread"></param>
        /// <returns></returns>
        public static List<T> GetEditbleData<T>(this FpSpread fpSpread)
        {
            if (fpSpread.DataSource != null)
            {
                return (fpSpread.DataSource as IList<T>).ToList().FindAll(r => (r as BaseDto).RowStatus != RowStatus.None);
            }

            return null;
        }

        public static BindingList<T> GetEditbleBindingData<T>(this FpSpread fpSpread) where T : new()
        {
            if (fpSpread.DataSource != null)
            {
                //if(fpSpread.DataSource is IList)
                {
                    return new BindingList<T>((fpSpread.DataSource as BindingList<T>).ToList()
                        .FindAll(r => (r as BaseDto).RowStatus != RowStatus.None));
                }
                //else
                //{
                //    var rows = (fpSpread.DataSource as DataTable).AsEnumerable().Where(r=> !r["RowStatus"].Equals("None"));
                //    BindingList<T> list = new BindingList<T>();
                //    foreach(DataRow row in rows)
                //    {
                //        T t = new T();
                //        PropertyInfo[] infos = t.GetType().GetProperties();
                //        foreach(PropertyInfo info in infos)
                //        {
                //            if (row.ItemArray.Contains(info.Name))
                //            {
                //                t.SetPropertieValue(info.Name, row[info.Name]);
                //            }
                //        }

                //    }
                //    return null;
                //}
            }

            return null;
        }

        /// <summary>
        /// 현재로우 데이터
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <returns></returns>
        public static object GetCurrentRowData(this FpSpread fpSpread)
        {
            if (fpSpread.ActiveSheet.ActiveRowIndex < 0)
            {
                return null;
            }

            if (fpSpread.DataSource != null)
            {
                //  모델의 지정된 행 인덱스에 대응하는 시트의 행을 가져옵니다
                //  fpSpread.ActiveSheet.GetViewRowFromModelRow(row);

                //시트의 지정된 행 인덱스에 대응하는 데이터 모델의 행을 가져옵니다
                //  fpSpread.ActiveSheet.GetModelRowFromViewRow(row);

                int index = fpSpread.ActiveSheet.GetModelRowFromViewRow(fpSpread.ActiveSheet.ActiveRowIndex);

                IList collection = (IList)fpSpread.DataSource;
                if (collection == null)
                {
                    IBindingList bindingList = (IBindingList)fpSpread.DataSource;

                    return bindingList[index];
                }
                return collection[index];
            }

            return null;
        }

        /// <summary>
        /// 스프레드 Cell Value
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static object GetCellValue(this FpSpread fpSpread, int row, int col)
        {
            return fpSpread.ActiveSheet.Cells[row, col].Value;
        }

        public static Cell GetCell2(this FpSpread fpSpread, int row, int col)
        {
            return fpSpread.ActiveSheet.Cells[row - 1, col - 1];
        }

        /// <summary>
        /// 스프레드 Cell Text
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string GetCellText(this FpSpread fpSpread, int row, int col)
        {
            return fpSpread.ActiveSheet.Cells[row, col].Text;
        }

        /// <summary>
        /// 컬럼 인덱스
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <returns></returns>
        public static int GetActiveColIndex(this FpSpread fpSpread)
        {
            return fpSpread.ActiveSheet.ActiveColumnIndex;
        }

        public static void SetActiveColIndex(this FpSpread fpSpread, int col)
        {
            fpSpread.ActiveSheet.ActiveColumnIndex = col;
        }


        /// <summary>
        /// 로우 인덱스
        /// </summary>
        /// <param name="fpSpread"></param>
        /// <returns></returns>
        public static int GetActiveRowIndex(this FpSpread fpSpread)
        {
            return fpSpread.ActiveSheet.ActiveRowIndex;
        }

        public static void SetActiveRowIndex(this FpSpread fpSpread, int row)
        {
            fpSpread.ActiveSheet.ActiveRowIndex = row;
        }


        public static Cell GetActiveCell(this FpSpread fpSpread)
        {
            return fpSpread.ActiveSheet.ActiveCell;
        }

        public static Row GetActiveRow(this FpSpread fpSpread)
        {
            return fpSpread.ActiveSheet.ActiveRow;
        }

        public static Row Row(this FpSpread fpSpread, int row)
        {
            return fpSpread.ActiveSheet.Rows[row];
        }

        public static void SetCellText(this FpSpread fpSpread, int row, int col, string txt)
        {
            fpSpread.ActiveSheet.Cells[row, col].Text = txt;
        }

        public static void SetCellValue(this FpSpread fpSpread, int row, int col, object data)
        {
            fpSpread.ActiveSheet.Cells[row, col].Value = data;
        }

        public static void SetCellValue2(this FpSpread fpSpread, int row, int col, object data)
        {
            fpSpread.ActiveSheet.Cells[row - 1, col - 1].Value = data;
        }

        public static void ExportExcel(this FpSpread fpSpread)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel파일|*.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName.Length != 0)
            {
                //  엑셀시트 보호 해제
                fpSpread.ActiveSheet.Protect = false;

                fpSpread.SaveExcel(saveFileDialog.FileName,
                    FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat |
                    FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders |
                    FarPoint.Excel.ExcelSaveFlags.AutoRowHeight);

                Process.Start(saveFileDialog.FileName);

                //  엑셀시트 보호
                fpSpread.ActiveSheet.Protect = true;
            }
        }

        public static void RowsClear(this FpSpread fpSpread)
        {
            fpSpread.ActiveSheet.Rows.Clear();
        }

        public static void ColumnsClear(this FpSpread fpSpread)
        {
            fpSpread.ActiveSheet.Columns.Clear();
        }

        public static Cells Cells(this FpSpread fpSpread)
        {
            return fpSpread.ActiveSheet.Cells;
        }

        public static Cell Cell(this FpSpread fpSpread, int row, int col)
        {
            return fpSpread.ActiveSheet.Cells[row, col];
        }

        public static void TagDataClear(this FpSpread fpSpread)
        {
            for (int i = 0; i < fpSpread.RowCount(); i++)
            {
                for (int j = 0; j < fpSpread.ColumnCount(); j++)
                {
                    if (fpSpread.Cells()[i, j].Tag.NotEmpty())
                    {
                        fpSpread.Cells()[i, j].Value = string.Empty;
                    }
                }
            }
        }

        public static T GetStaticDataSource<T>(this FpSpread fpSpread)
        {
            if (fpSpread.Tag == null)
            {
                return default(T);
            }

            T t = (T)fpSpread.Tag;

            for (int i = 0; i < fpSpread.RowCount(); i++)
            {
                for (int j = 0; j < fpSpread.ColumnCount(); j++)
                {
                    if (fpSpread.Cells()[i, j].Tag.NotEmpty())
                    {
                        try
                        {
                            if (t.GetPropertieValue(fpSpread.Cells()[i, j].Tag.ToString()).GetType() == typeof(long))
                            {
                                t.SetPropertieValue(fpSpread.Cells()[i, j].Tag.ToString(), fpSpread.Cells()[i, j].Value.To<Int64>());
                            }
                            else
                            {
                                t.SetPropertieValue(fpSpread.Cells()[i, j].Tag.ToString(), fpSpread.Cells()[i, j].Value);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return t;
        }

        public static void SetStaticDataSource(this FpSpread fpSpread, object dto)
        {
            fpSpread.Tag = dto;

            Dictionary<string, object> dic = dto.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .ToDictionary(prop => prop.Name, prop => prop.GetValue(dto, null));

            for (int i = 0; i < fpSpread.RowCount(); i++)
            {
                for (int j = 0; j < fpSpread.ColumnCount(); j++)
                {
                    if (fpSpread.Cells()[i, j].Tag.NotEmpty())
                    {
                        if (dic.ContainsKey(fpSpread.Cells()[i, j].Tag.ToString()))
                        {
                            fpSpread.Cells()[i, j].Value = dic[fpSpread.Cells()[i, j].Tag.ToString()];
                        }
                    }
                }
            }
        }

        public static Columns Columns(this FpSpread fpSpread)
        {
            return fpSpread.ActiveSheet.Columns;
        }

        public static Column Column(this FpSpread fpSpread, int col)
        {
            return fpSpread.ActiveSheet.Columns[col];
        }

        public static int GetColumnIndex(this FpSpread fpSpread, string fieldName)
        {
            for (int i = 0; i < fpSpread.ColumnCount(); i++)
            {
                if (fieldName.Equals(fpSpread.Column(i).DataField))
                {
                    int col = fpSpread.ActiveSheet.GetModelColumnFromViewColumn(i);
                    return col;
                }
            }
            return -1;
        }
    }

    public class SpreadOption
    {
        public SelectionUnit SpreadSelectionUnit = SelectionUnit.Cell;

        /// <summary>
        /// 로우 높이
        /// </summary>
        public int RowHeight = 0;
        public bool RowHeightAuto = false;
        /// <summary>
        /// 헤더높이
        /// </summary>
        public int ColumnHeaderHeight = 55;
        public Color ColumnHeaderColor = Color.AliceBlue;

        public Keys MoveKey = Keys.None;
        public FpSpreadNextMoveType NextMoveType = FpSpreadNextMoveType.None;
        /// <summary>
        /// Row 선택 표시여부
        /// </summary>
        public bool IsRowSelectColor = false;
        /// <summary>
        /// Row 헤더 표시여부
        /// </summary>
        public bool RowHeaderVisible = false;
        public bool ColumnHeaderVisible = true;

        public bool IsValidation = false;
        public bool IsColumnMove = false;
        //InputMap inputMap = new InputMap();
        //inputMap.Put(new Keystroke(Keys.Return, Keys.None), SpreadActions.MoveToNextCellThenControl);

        //fpSpread.SetInputMap(InputMapMode.WhenFocused, OperationMode.Normal, inputMap);

        //fpSpread.ActiveSheet.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;

        public List<Column> DateTimeColumns = null;
    }

    /// <summary>
    /// 스프레드 옵션
    /// </summary>
    public class SpreadCellTypeOption
    {
        /// <summary>
        /// [표시여부] 보임 : true, 안보임 : false
        /// </summary>
        public bool IsVisivle = true;
        /// <summary>
        /// [수정여부] 가능 : true, 불가능 : false
        /// </summary>
        public bool IsEditble = true;
        /// <summary>
        /// Text MultiLine 여부
        /// </summary>
        public bool IsMulti = false;
        /// <summary>
        /// Sort 여부
        /// </summary>
        public bool IsSort = false;

        /// <summary>
        /// sort 방향
        /// </summary>
        public SortIndicator sortIndicator = SortIndicator.None;
        /// <summary>
        /// Filter 여부
        /// </summary>
        public bool isFilter = false;
        /// <summary>
        /// 정렬
        /// </summary>
        public CellHorizontalAlignment Aligen = CellHorizontalAlignment.Center;
        // <summary>
        /// 정렬
        /// </summary>
        public CellVerticalAlignment VAligen = CellVerticalAlignment.Center;
        /// <summary>
        /// Cell 배경색
        /// </summary>
        public Color BackColor = Color.White;
        /// <summary>
        /// 글자색
        /// </summary>
        public Color ForceColor = Color.Black;

        /// <summary>
        /// 텍스트 최대 문자
        /// </summary>
        public int TextMaxLength = int.MaxValue;
        /// <summary>
        /// 텍스트 최소문자
        /// </summary>
        public int TextMinLength = int.MinValue;

        /// <summary>
        /// 날짜 포멧(보여줄 형식)        
        /// </summary>
        public DateTimeType dateTimeType = DateTimeType.YYYY_MM_DD;
        public DateTimeType dbDateTimeType = DateTimeType.YYYYMMDD;
        /// <summary>
        /// 날짜 데이타 형식(값을 가져올때 날짜형식
        /// </summary>
        public DateTimeEditorValue dateTimeEditorValue = DateTimeEditorValue.DateTime;
        /// <summary>
        /// 날짜 달력 버튼 표시여부
        /// </summary>
        public bool IsShowCalendarButton = true;
        /// <summary>
        /// Number 소수점 자리수
        /// </summary>
        public int DecimalPlaces = 0;
        /// <summary>
        /// 음수이경우 빨강색으로 적용 넘버셀
        /// </summary>
        public bool NegativeRed = false;

        /// <summary>
        /// Combo 아이템
        /// </summary>
        public object[] Items;
        /// <summary>
        /// Combo 표시 프로퍼티
        /// </summary>
        public string DisplayMember;
        /// <summary>
        /// Combo 값 프로퍼티
        /// </summary>
        public string ValueMember;
        /// <summary>
        /// Combo Value값 표시여부
        /// </summary>
        public bool IsValueMember;
        /// <summary>
        /// 체크박스 False Text
        /// </summary>
        public string TextFalse = "False";
        /// <summary>
        /// 체크박스 True Text
        /// </summary>
        public string TextTrue = "True";

        /// <summary>
        /// Button Text
        /// </summary>
        public string ButtonText;

        /// <summary>
        /// 프로그래스 Max
        /// </summary>
        public int ProgressMax;
        /// <summary>
        /// 프로그래스 Min
        /// </summary>
        public int ProgressMin;

        /// <summary>
        /// ListBox Items
        /// </summary>
        public string[] ListBoxItems;
        /// <summary>
        /// ListBox ItemData
        /// </summary>
        public string[] ListBoxData;
        /// <summary>
        /// 커스텀 CellType
        /// </summary>
        public ICustomCellType ICustomCellType;

        /// <summary>
        /// 셀 Merge 방식 
        /// Alyways (같은값은 항상 머지)
        /// </summary>
        public MergePolicy mergePolicy = MergePolicy.None;

        /// <summary>
        /// TextCellType에서 widht에 따른 자동 줄바꿈 여부
        /// </summary>
        public bool WordWrap = true;

        public string Mask;

        public string Tag;

        public object TriggerData = string.Empty;
        public TriggerType TriggerType = TriggerType.Equals;
        public TriggerTarget TriggerTarget = TriggerTarget.Row;
        public string TriggerField = string.Empty;
        public Color TriggerColor = Color.Yellow;
        public TriggerColorType TriggerColorType = TriggerColorType.Background;

        public string FontName = string.Empty;
        public float FontSize = 0;
        public FontStyle FontStyle = FontStyle.Regular;

        public string strBtn;
        public string strBtnDown;
        public string chkCaption;
        public string[] strCombo;

        public string OrgFieldName { get; private set; }

        public void SetOrgFieldName(string name)
        {
            this.OrgFieldName = name;
        }

        public string CopyFieldName { get; private set; }
        public void SetCopyFieldName(string name)
        {
            this.CopyFieldName = name;
        }
    }

    public enum TriggerType
    {
        Equals, In, Up, Down, NotEmpty, NotEquals
    }
    public enum TriggerTarget
    {
        Row, Cell
    }

    public enum TriggerColorType
    {
        Force, Background
    }

    public enum FpSpreadNextMoveType
    {
        None, Row, Column
    }
    public enum FpSpreadCellType
    {
        TextCellType,
        DateTimeCellType,
        TimeCellType,
        IDateTimeCellType,
        ComboBoxCellType,
        CheckBoxCellType,
        ImageCellType,
        ButtonCellType,
        ProgressCellType,
        ColorPickerCellType,
        CurrencyCellType,
        HyperLinkCellType,
        ListBoxCellType,
        MaskCellType,
        MultiOptionCellType,
        MultiColumnComboBoxCellType,
        NumberCellType,
        PercentCellType,
        RichTextCellType,
        SliderCellType,
        RegularExpressionCellType
    }
    public enum DateTimeType
    {
        /// <summary>
        /// DB 컬럼 DateTime or TimeStamp
        /// </summary>
        [Description("")]
        None,
        /// <summary>
        /// 
        /// </summary>
        [Description("yyyyMMdd")]
        YYYYMMDD,
        /// <summary>
        /// 
        /// </summary>
        [Description("yyyyMMdd HH:mm")]
        YYYYMMDDHHMM,
        /// <summary>
        /// 
        /// </summary>
        [Description("yyyyMMdd HH:mm:ss")]
        YYYYMMDDHHMMSS,
        /// <summary>
        /// 
        /// </summary>
        [Description("yyyyMM")]
        YYYYMM,
        /// <summary>
        /// 
        /// </summary>
        [Description("yyyy")]
        YYYY,
        /// <summary>
        /// 
        /// </summary>
        [Description("yyyy-MM-dd")]
        YYYY_MM_DD,
        /// <summary>
        /// 
        /// </summary>
        [Description("yyyy-MM-dd HH:mm")]
        YYYY_MM_DD_HH_MM,
        /// <summary>
        /// 
        /// </summary>
        [Description("yyyy-MM-dd HH:mm:ss")]
        YYYY_MM_DD_HH_MM_SS,
        /// <summary>
        /// 
        /// </summary>
        [Description("yyyy-MM")]
        YYYY_MM,
        /// <summary>
        /// 
        /// </summary>
        [Description("HH:mm:ss")]
        HH_MM_SS,
        /// <summary>
        /// 
        /// </summary>
        [Description("HH:mm")]
        HH_MM,
        /// <summary>
        /// 
        /// </summary>
        [Description("HHmm")]
        HHMM,
    }

    public class TriggerInfo
    {
        public string data = string.Empty;
        public Color color = Color.Yellow;
        public TriggerTarget triggerTarget = TriggerTarget.Row;
        public TriggerColorType colorType = TriggerColorType.Background;
    }
}
