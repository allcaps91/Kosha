using System;
using System.Drawing;


namespace AUTO_SPREAD
{
    //public class clsSpreadSet
    //{
    //    //외래간호 스테이션 메인
    //    public enum MedOpdNrMain
    //    {
    //        check01 = 0,
    //        DeptCode,
    //        DrName,
    //        Pano,
    //        SName,
    //        STS00,//성별/나이
    //        Jin,
    //        JepTime,
    //        ArryTime,
    //        SinGu,
    //        STS01, //원거리
    //        STS02, //EMR
    //        STS03, //진료수납
    //        STS04, //감염정보1
    //        STS05, //감염정보2
    //        STS06, //장애정보
    //        STS07, //문자동의
    //        Remark, //참고사항
    //        DrCode,
    //        check02,//회송
    //        frROWID,
    //        DrRead, //처방의판독
    //        ExamResult,
    //        Vital,
    //        STS08, //어르신먼저
    //        STS09, //미시행
    //        SPC, //선택신청
    //        SName2,
    //        STS10, //후불대상
    //        STS11, //VIP
    //        STS12, //공단건진
    //        STS13 //해피콜


    //    }

    //    public static void AUOT_SPREAD_SET_MedOpdNrMain(FarPoint.Win.Spread.SheetView Spd, int RowCnt, int ColCnt)
    //    {
    //        FarPoint.Win.Spread.CellType.CheckBoxCellType TypeCheckBox = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
    //        FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
    //        FarPoint.Win.Spread.CellType.TextCellType TypeMultiText = new FarPoint.Win.Spread.CellType.TextCellType();
    //        TypeText.MaxLength = 1000;


    //        Spd.RowCount = RowCnt;
    //        Spd.ColumnCount = ColCnt;
    //        if (ColCnt == 0) Spd.ColumnCount = Enum.GetValues(typeof(MedOpdNrMain)).Length;


    //        Spd.ColumnHeader.Rows[0].Height = 55;

    //        //0
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.check01].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.check01].Text = "선택";
    //        Spd.Columns[(int)MedOpdNrMain.check01].CellType = TypeCheckBox;
    //        Spd.Columns[(int)MedOpdNrMain.check01].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.check01].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.check01].Width = 18;
    //        Spd.Columns[(int)MedOpdNrMain.check01].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.check01].Visible = true;


    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.DeptCode].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.DeptCode].Text = "과";
    //        Spd.Columns[(int)MedOpdNrMain.DeptCode].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.DeptCode].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.DeptCode].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.DeptCode].Width = 30;
    //        Spd.Columns[(int)MedOpdNrMain.DeptCode].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.DeptCode].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.DrName].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.DrName].Text = "의사명";
    //        Spd.Columns[(int)MedOpdNrMain.DrName].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.DrName].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.DrName].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.DrName].Width = 50;
    //        Spd.Columns[(int)MedOpdNrMain.DrName].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.DrName].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.Pano].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.Pano].Text = "등록번호";
    //        Spd.Columns[(int)MedOpdNrMain.Pano].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.Pano].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.Pano].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.Pano].Width = 80;
    //        Spd.Columns[(int)MedOpdNrMain.Pano].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.Pano].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.SName].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.SName].Text = "성명";
    //        Spd.Columns[(int)MedOpdNrMain.SName].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.SName].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.SName].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.SName].Width = 70;
    //        Spd.Columns[(int)MedOpdNrMain.SName].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.SName].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS00].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS00].Text = "성별/나이";
    //        Spd.Columns[(int)MedOpdNrMain.STS00].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS00].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS00].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS00].Width = 40;
    //        Spd.Columns[(int)MedOpdNrMain.STS00].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS00].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.Jin].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.Jin].Text = "접수구분";
    //        Spd.Columns[(int)MedOpdNrMain.Jin].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.Jin].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.Jin].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.Jin].Width = 35;
    //        Spd.Columns[(int)MedOpdNrMain.Jin].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.Jin].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.JepTime].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.JepTime].Text = "접수시간";
    //        Spd.Columns[(int)MedOpdNrMain.JepTime].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.JepTime].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.JepTime].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.JepTime].Width = 40;
    //        Spd.Columns[(int)MedOpdNrMain.JepTime].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.JepTime].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.ArryTime].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.ArryTime].Text = "도착시간";
    //        Spd.Columns[(int)MedOpdNrMain.ArryTime].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.ArryTime].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.ArryTime].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.ArryTime].Width = 40;
    //        Spd.Columns[(int)MedOpdNrMain.ArryTime].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.ArryTime].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.SinGu].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.SinGu].Text = "신환";
    //        Spd.Columns[(int)MedOpdNrMain.SinGu].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.SinGu].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.SinGu].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.SinGu].Width = 20;
    //        Spd.Columns[(int)MedOpdNrMain.SinGu].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.SinGu].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS01].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS01].Text = "원거리";
    //        Spd.Columns[(int)MedOpdNrMain.STS01].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS01].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS01].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS01].Width = 20;
    //        Spd.Columns[(int)MedOpdNrMain.STS01].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS01].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS02].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS02].Text = "EMR";
    //        Spd.Columns[(int)MedOpdNrMain.STS02].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS02].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS02].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS02].Width = 25;
    //        Spd.Columns[(int)MedOpdNrMain.STS02].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS02].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS03].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS03].Text = "진료수납";
    //        Spd.Columns[(int)MedOpdNrMain.STS03].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS03].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS03].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS03].Width = 30;
    //        Spd.Columns[(int)MedOpdNrMain.STS03].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS03].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS04].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS04].Text = "격리주의";
    //        Spd.Columns[(int)MedOpdNrMain.STS04].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS04].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS04].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS04].Width = 80;
    //        Spd.Columns[(int)MedOpdNrMain.STS04].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS04].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS05].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS05].Text = "임시";
    //        Spd.Columns[(int)MedOpdNrMain.STS05].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS05].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS05].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS05].Width = 20;
    //        Spd.Columns[(int)MedOpdNrMain.STS05].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS05].Visible = false;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS06].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS06].Text = "장애";
    //        Spd.Columns[(int)MedOpdNrMain.STS06].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS06].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS06].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS06].Width = 20;
    //        Spd.Columns[(int)MedOpdNrMain.STS06].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS06].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS07].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS07].Text = "문자동의";
    //        Spd.Columns[(int)MedOpdNrMain.STS07].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS07].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS07].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS07].Width = 30;
    //        Spd.Columns[(int)MedOpdNrMain.STS07].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS07].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.Remark].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.Remark].Text = "참고사항";
    //        Spd.Columns[(int)MedOpdNrMain.Remark].CellType = TypeMultiText;
    //        TypeMultiText.WordWrap = true;
    //        Spd.Columns[(int)MedOpdNrMain.Remark].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.Remark].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.Remark].Width = 200;
    //        Spd.Columns[(int)MedOpdNrMain.Remark].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.Remark].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.DrCode].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.DrCode].Text = "의사코드";
    //        Spd.Columns[(int)MedOpdNrMain.DrCode].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.DrCode].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.DrCode].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.DrCode].Width = 30;
    //        Spd.Columns[(int)MedOpdNrMain.DrCode].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.DrCode].Visible = false;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.check02].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.check02].Text = "회송";
    //        Spd.Columns[(int)MedOpdNrMain.check02].CellType = TypeCheckBox;
    //        Spd.Columns[(int)MedOpdNrMain.check02].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.check02].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.check02].Width = 18;
    //        Spd.Columns[(int)MedOpdNrMain.check02].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.check02].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.frROWID].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.frROWID].Text = "ROWID";
    //        Spd.Columns[(int)MedOpdNrMain.frROWID].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.frROWID].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.frROWID].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.frROWID].Width = 100;
    //        Spd.Columns[(int)MedOpdNrMain.frROWID].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.frROWID].Visible = false;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.DrRead].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.DrRead].Text = "처방의판독";
    //        Spd.Columns[(int)MedOpdNrMain.DrRead].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.DrRead].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.DrRead].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.DrRead].Width = 70;
    //        Spd.Columns[(int)MedOpdNrMain.DrRead].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.DrRead].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.ExamResult].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.ExamResult].Text = "검사결과";
    //        Spd.Columns[(int)MedOpdNrMain.ExamResult].CellType = TypeCheckBox;
    //        Spd.Columns[(int)MedOpdNrMain.ExamResult].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.ExamResult].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.ExamResult].Width = 18;
    //        Spd.Columns[(int)MedOpdNrMain.ExamResult].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.ExamResult].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.Vital].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.Vital].Text = "바이탈";
    //        Spd.Columns[(int)MedOpdNrMain.Vital].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.Vital].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.Vital].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.Vital].Width = 20;
    //        Spd.Columns[(int)MedOpdNrMain.Vital].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.Vital].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS08].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS08].Text = "어르신";
    //        Spd.Columns[(int)MedOpdNrMain.STS08].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS08].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS08].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS08].Width = 20;
    //        Spd.Columns[(int)MedOpdNrMain.STS08].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS08].Visible = false;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS09].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS09].Text = "미시행";
    //        Spd.Columns[(int)MedOpdNrMain.STS09].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS09].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS09].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS09].Width = 20;
    //        Spd.Columns[(int)MedOpdNrMain.STS09].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS09].Visible = false;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.SPC].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.SPC].Text = "선택신청";
    //        Spd.Columns[(int)MedOpdNrMain.SPC].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.SPC].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.SPC].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.SPC].Width = 20;
    //        Spd.Columns[(int)MedOpdNrMain.SPC].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.SPC].Visible = false;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.SName2].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.SName2].Text = "성명";
    //        Spd.Columns[(int)MedOpdNrMain.SName2].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.SName2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.SName2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.SName2].Width = 80;
    //        Spd.Columns[(int)MedOpdNrMain.SName2].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.SName2].Visible = false;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS10].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS10].Text = "후불대상";
    //        Spd.Columns[(int)MedOpdNrMain.STS10].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS10].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS10].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS10].Width = 30;
    //        Spd.Columns[(int)MedOpdNrMain.STS10].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS10].Visible = false;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS11].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS11].Text = "VIP";
    //        Spd.Columns[(int)MedOpdNrMain.STS11].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS11].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS11].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS11].Width = 30;
    //        Spd.Columns[(int)MedOpdNrMain.STS11].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS11].Visible = false;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS12].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS12].Text = "공단건진";
    //        Spd.Columns[(int)MedOpdNrMain.STS12].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS12].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS12].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS12].Width = 30;
    //        Spd.Columns[(int)MedOpdNrMain.STS12].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS12].Visible = true;

    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS13].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
    //        Spd.ColumnHeader.Cells[0, (int)MedOpdNrMain.STS13].Text = "해피콜";
    //        Spd.Columns[(int)MedOpdNrMain.STS13].CellType = TypeText;
    //        Spd.Columns[(int)MedOpdNrMain.STS13].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS13].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns[(int)MedOpdNrMain.STS13].Width = 20;
    //        Spd.Columns[(int)MedOpdNrMain.STS13].Locked = false;
    //        Spd.Columns[(int)MedOpdNrMain.STS13].Visible = true;

    //    }


    //    public static void AUOT_SPREAD_SET_MedOpdNrMain_Wait(FarPoint.Win.Spread.SheetView Spd, int RowCnt, int ColCnt)
    //    {
    //        FarPoint.Win.Spread.CellType.CheckBoxCellType TypeCheckBox = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
    //        FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
    //        TypeText.MaxLength = 1000;


    //        Spd.RowCount = RowCnt;
    //        Spd.ColumnCount = ColCnt;
    //        if (ColCnt == 0) Spd.ColumnCount = Enum.GetValues(typeof(MedOpdNrMain)).Length;


    //        Spd.ColumnHeader.Rows[0].Height = 54;

    //        Spd.ColumnHeader.Rows.Get(0).Height = 22F;

    //        //'타이틀 설정'            
    //        Spd.Cells.Get(0, 0).Value = "의사";
    //        Spd.Columns.Get(0).Width = 29F;
    //        Spd.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns.Get(0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));

    //        Spd.Cells.Get(1, 0).Value = "NO";
    //        Spd.Columns.Get(1).Width = 2F;
    //        Spd.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //        Spd.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //        Spd.Columns.Get(0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));

    //        for (int i = 0; i < 16; i++)
    //        {
    //            Spd.Cells.Get(1, (i * 6) + 2 + 0).Value = "순서";
    //            Spd.Columns.Get((i * 6) + 2 + 0).Width = 40F;
    //            Spd.Columns.Get((i * 6) + 2 + 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //            Spd.Columns.Get((i * 6) + 2 + 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //            Spd.Cells.Get(1, (i * 6) + 2 + 1).Value = "성명";
    //            Spd.Columns.Get((i * 6) + 2 + 1).Width = 72F;
    //            Spd.Columns.Get((i * 6) + 2 + 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //            Spd.Columns.Get((i * 6) + 2 + 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //            Spd.Columns.Get((i * 6) + 2 + 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(252)))), ((int)(((byte)(221)))));
    //            Spd.Cells.Get(1, (i * 6) + 2 + 2).Value = "예약";
    //            Spd.Columns.Get((i * 6) + 2 + 2).Width = 42F;
    //            Spd.Columns.Get((i * 6) + 2 + 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //            Spd.Columns.Get((i * 6) + 2 + 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //            Spd.Cells.Get(1, (i * 6) + 2 + 3).Value = "도착";
    //            Spd.Columns.Get((i * 6) + 2 + 3).Width = 42F;
    //            Spd.Columns.Get((i * 6) + 2 + 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //            Spd.Columns.Get((i * 6) + 2 + 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //            Spd.Cells.Get(1, (i * 6) + 2 + 4).Value = "구분";
    //            Spd.Columns.Get((i * 6) + 2 + 4).Width = 42F;
    //            Spd.Columns.Get((i * 6) + 2 + 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
    //            Spd.Columns.Get((i * 6) + 2 + 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
    //            Spd.Cells.Get(1, (i * 6) + 2 + 5).Value = "";
    //            Spd.Columns.Get((i * 6) + 2 + 5).Width = 2F;

    //            Spd.Rows.Get(1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));

    //        }



    //    }


    //}
}
