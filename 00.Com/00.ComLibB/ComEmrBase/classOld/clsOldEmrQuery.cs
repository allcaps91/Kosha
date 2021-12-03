using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComEmrBase
{
    public class clsOldEmrQuery
    {
        public static bool START_TUYAK()
        {
            //' SQL1 = "  SELECT CODE"
            //' SQL1 = SQL1 & vbCr & "   FROM ADMIN.BAS_BCODE"
            //' SQL1 = SQL1 & vbCr & " WHERE GUBUN = 'EMR_투약기록지분리시행'"
            //' SQL1 = SQL1 & vbCr & "      AND CODE = 'START'"
            //' SQL1 = SQL1 & vbCr & "      AND NAME = 'Y'"
            //' Call AdoOpenSet(RSt, SQL1, , , False)
            //' If RowIndicator > 0 Then START_TUYAK = True
            //' Call AdoCloseSet(RSt)
            //' If GnJobSabun = 23515 Then
            //'     START_TUYAK = True
            //' End If
            // '조건없이. 2018-02-09
            return true;
        }

    }
}
