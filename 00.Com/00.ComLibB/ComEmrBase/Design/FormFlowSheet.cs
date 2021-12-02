namespace ComEmrBase
{
    //=CellType
    //TextCellType
    //CheckBoxCellType
    //ComboBoxCellType

    //=HorizontalAlignment
    //Left
    //Center
    //Right

    //=VerticalAlignment
    //Top
    //Center
    //Bottom

    //=MultiLine
    //False
    //True

    //=CheckTextAlignment
    //Right
    //Left
    //Top
    //Bottom

    /// <summary>
    /// 프로우시트 속성
    /// </summary>
    public class FormFlowSheet
    {
        public int ItemNumber = 0;
        public string ItemCode = string.Empty;
        public string ItemName = string.Empty;
        public string CellType = string.Empty;
        public string HorizontalAlignment = string.Empty;
        public string VerticalAlignment = string.Empty;
        public int Width = 0;
        public int Height = 0;
        public string MultiLine = string.Empty;
        public string CheckTextAlignment = string.Empty;
        public string UserMcro = string.Empty;
        public string UserFunc = string.Empty;
        public string UserFuncNm = string.Empty;
        public string INITVALUE = string.Empty;
        public string MIBI = string.Empty;
    }
}
