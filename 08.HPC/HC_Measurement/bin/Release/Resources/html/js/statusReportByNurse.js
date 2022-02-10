$(document).ready(function () {
   
    (async function()
    {
        await CefSharp.BindObjectAsync("cefsharpBoundAsync");
        cefsharpBoundAsync.readyStatusReportNurse();
        
    })();
   
    //SetSite("");
  // SetsiteStatusDtoJson("");
  // SetPERFORMCONTENT("");
});

//기관보관용
function setOSHA() {
    //$('#osha1').text("계 장");
    $('#osha1').text("팀 장");
    $('#osha2').text("부 장");
}
function unSetOSHA() {
    $('#osha1').text("");
    $('#osha2').text("");
}

function SetSite(json){
   // var json ='{"ID":30,"SITE_ID":2600,"ESTIMATE_ID":0,"VISITDATE":"20200212","VISITRESERVEDATE":"20200211","SiteStatusDto":{"CURRENTWORKERCOUNT":100,"NEWWORKERCOUNT":2,"RETIREWORKERCOUNT":30,"CHANGEWORKERCOUNT":66,"ACCIDENTWORKERCOUNT":100,"DEADWORKERCOUNT":3,"INJURYWORKERCOUNT":13,"BIZDISEASEWORKERCOUNT":88,"GENERALHEALTHCHECKDATE":"2020-02-05","SPECIALHEALTHCHECKDATE":"2020-02-14","GENERALD2COUNT":11,"GENERALC2COUNT":123,"SPECIALD1COUNT":142,"SPECIALC1COUNT":123,"SPECIALD2COUNT":44,"SPECIALC2COUNT":55,"SPECIALDNCOUNT":66,"SPECIALCNCOUNT":77,"WEMDATE":null,"WEMEXPORSURE":"N","WEMEXPORSUREREMARK":"3333333","WEMHARMFULFACTORS":"drrrrrrrrrrrrrrrr","RowStatus":0},"SITEMANAGERNAME":"1","SITEMANAGERGRADE":"1","NURSENAME":"drtdrtdrt","ISDELETED":"N","MODIFIED":"2020-02-15T12:09:08.174202","MODIFIEDUSER":"41783","CREATED":"2020-02-12T20:58:16.439903","CREATEDUSER":"41783","RowStatus":0}'       
    var j = JSON.parse(json);
    console.log(j);
    console.log(j.SiteStatusDto);
    var month = j.VISITDATE.substr(4, 2);
    console.log("month:" + month);
    if (month.length > 1) {
        if (month.substr(0, 1) === "0") {
            month = month.substr(1, 1);
        }
    }
    console.log("month:" + month);
    $('#month_text').text(month);
   
    $('#VISITDATE').text(j.VISITDATE.substring(0,4) +"년 " + j.VISITDATE.substring(4,6) +"월 "+ j.VISITDATE.substring(6,8) +"일");
    $('#SITEMANAGERGRADE').text(j.SITEMANAGERGRADE);
    $('#SITEMANAGERNAME').text(j.SITEMANAGERNAME);
    if (j.VISITRESERVEDATE.length > 0) {
        $('#VISITRESERVEDATE').text(j.VISITRESERVEDATE.substring(0, 4) + "년 " + j.VISITRESERVEDATE.substring(4, 6) + "월 " + j.VISITRESERVEDATE.substring(6, 8) + "일");
    }
    
    $('#NURSENAME').text(j.NURSENAME);
}

//업무수행내역
function SetPERFORMCONTENT(json){
   /* var json ='{"IsOshaPlan":"Y","IsOshaGeneral":"Y","IsMsds":"Y","IsMsdsInstall":"Y","IsMsdsSpeacial":"Y","IsHcPlan":"Y","IsHcManage":"Y","IsHcExplain"'
    +':"Y","IsDesease":"Y","IsDesease2":"Y","IsDeseaseCheck":"Y","IsDeseaseCheck2":"Y","IsEmergency":"Y","IsEmergencyManage":"Y","IsJobManage":"Y","IsJobManage2"'
    +':"Y","IsCommite":"Y","IsProtection1":"Y","IsProtection2":"Y","IsEdu":"Y","IsEduType":"Y","IsEduKind1":"Y","IsEduKind2":"Y","IsEduKind3":"Y","IsEduKind4":"Y"'
    +',"IsEduKind5":"Y","IsEduKind6":"Y","IsEduCount":"11","IsEduTitle":"1132123123","IsHealth1":"Y","IsHealth2":"Y","IsDe1":"Y","IsDe2":"Y","IsDe3":"Y",'
    +'"IsHe1":"Y","IsHe2":"Y","IsHe3":"Y","SangdamCount":1,"IsSangdam1":"Y","IsSangdam2":"Y","IsSangdam3":"Y","IsSangdam4":"Y","IsSangdam5":"Y","IsSangdam6":"Y",'
    +'"CheckCount1":12,"CheckCount2":2,"CheckCount3":33,"CheckCount4":23,"OshaData":"123123","RowStatus":0}';
    */

    console.log(json);
    var x = JSON.parse(json);
    console.log(getCheck(x.IsOshaPlan))
    $('#IsOshaPlan').text(getCheck(x.IsOshaPlan));
    $('#IsOshaGeneral').text(getCheck(x.IsOshaGeneral));
    $('#IsMsds').text(getCheck(x.IsMsds));
    $('#IsMsdsInstall').text(getCheck(x.IsMsdsInstall));
    $('#IsMsdsSpeacial').text(getCheck(x.IsMsdsSpeacial));
    $('#IsHcPlan').text(getCheck(x.IsHcPlan));
    $('#IsHcManage').text(getCheck(x.IsHcManage));
    $('#IsHcExplain').text(getCheck(x.IsHcExplain));
    $('#IsDesease').text(getCheck(x.IsDesease));
    $('#IsDesease2').text(getCheck(x.IsDesease2));
    $('#IsDeseaseCheck').text(getCheck(x.IsDeseaseCheck));
    $('#IsDeseaseCheck2').text(getCheck(x.IsDeseaseCheck2));
    $('#IsEmergency').text(getCheck(x.IsEmergency));
    $('#IsEmergencyManage').text(getCheck(x.IsEmergencyManage));
    $('#IsJobManage').text(getCheck(x.IsJobManage));
    $('#IsJobManage2').text(getCheck(x.IsJobManage2));
    $('#IsCommite').text(getCheck(x.IsCommite));

    $('#IsProtection1').text(getCheck(x.IsProtection1));
    $('#IsProtection2').text(getCheck(x.IsProtection2));

    $('#IsEdu').text(getCheck(x.IsEdu));

    console.log("x.IsEduType:"+x.IsEduType);
    if (x.IsEduType =="Y"){
        console.log("정기 체크");
        $('#IsEdutypey').prop('checked', true); //보건교육실시 정기 = Y 기타 = N
    }

    if (x.IsEduType == "N") {
        console.log("기타 체크");
        $('#IsEdutypen').attr('checked', true);  //보건교육실시 정기 = Y 기타 = N
    }
    
    //$('#IsEduType').text(getCheck(x.IsEduType));
    if(x.IsEduKind1=="Y"){
        $('#IsEduKind1').prop('checked', true); 
    }
    if(x.IsEduKind2=="Y"){
        $('#IsEduKind2').prop('checked', true); 
    }
    if(x.IsEduKind3=="Y"){
        $('#IsEduKind3').prop('checked', true); 
    }
    if(x.IsEduKind4=="Y"){
        $('#IsEduKind4').prop('checked', true); 
    }
    if(x.IsEduKind5=="Y"){
        $('#IsEduKind5').prop('checked', true); 
    }
    if(x.IsEduKind6=="Y"){
        $('#IsEduKind6').prop('checked', true); 
    }
    
    $('#IsEduCount').text("참석자: " +x.IsEduCount +" 명");
    $('#IsEduTitle').text("주제: "+ x.IsEduTitle);

    $('#IsHealth1').text(getCheck(x.IsHealth1));
    $('#IsHealth2').text(getCheck(x.IsHealth2));
    $('#IsDe1').text(getCheck(x.IsDe1));
    $('#IsDe2').text(getCheck(x.IsDe2));
    $('#IsDe3').text(getCheck(x.IsDe3));
    $('#IsHe1').text(getCheck(x.IsHe1));
    $('#IsHe2').text(getCheck(x.IsHe2));
    $('#IsHe3').text(getCheck(x.IsHe3));

    $('#SangdamCount').text("•총 상담인원: "+ x.SangdamCount +" 명");
    console.log(x.IsSangdam1)
    if(x.IsSangdam1=="Y"){
        $('#IsSangdam1').prop('checked', true); 
    }
    if(x.IsSangdam2=="Y"){
        $('#IsSangdam2').prop('checked', true); 
    }
    if(x.IsSangdam3=="Y"){
        $('#IsSangdam3').prop('checked', true); 
    }
    if(x.IsSangdam4=="Y"){
        $('#IsSangdam4').prop('checked', true); 
    }
    if(x.IsSangdam5=="Y"){
        $('#IsSangdam5').prop('checked', true); 
    }
    if(x.IsSangdam6=="Y"){
        $('#IsSangdam6').prop('checked', true); 
    }
  
    console.log(x.CheckCount1)
    $('#CheckCount1').text("•혈압: " + x.CheckCount1 +" 건");
    $('#CheckCount2').text("•혈당: " + x.CheckCount2+" 건");
    $('#CheckCount3').text("•소변: " + x.CheckCount3+" 건");
    $('#CheckCount4').text("•체지방: " + x.CheckCount4+" 건");

    $('#OshaData').text(x.OshaData);

    $("input[type=radio]").each(function () {

        if ($(this).is(":checked") == true) {
            $(this).replaceWith("<span style='font-weight:bold'>√<span>");
        }
    });
    $("input[type=checkbox]").each(function () {
        if ($(this).is(":checked") == true) {
            $(this).replaceWith("<span style='font-weight:bold'>√<span>");
        }
    });

}
function SetsiteStatusDtoJson(json){
   /* var json ='{"CURRENTWORKERCOUNT":100,"NEWWORKERCOUNT":2,"RETIREWORKERCOUNT":30,"CHANGEWORKERCOUNT":66,"ACCIDENTWORKERCOUNT":100,"DEADWORKERCOUNT":3,"INJURYWORKERCOUNT":13,"BIZDISEASEWORKERCOUNT":88,'
    +'"GENERALHEALTHCHECKDATE":"2020-02-05","SPECIALHEALTHCHECKDATE":"2020-02-14","GENERALD2COUNT":11,"GENERALC2COUNT":123,"SPECIALD1COUNT":142,"SPECIALC1COUNT":123,"SPECIALD2COUNT":44,"SPECIALC2COUNT":55,'
    +'"SPECIALDNCOUNT":66,"SPECIALCNCOUNT":77,"WEMDATE":"aa","WEMEXPORSURE":"Y","WEMEXPORSUREREMARK":"3333333","WEMHARMFULFACTORS":"asd","RowStatus":0}';
    
    */
    console.log(json);
    
    var x = JSON.parse(json);
    $('#SITENAME').text(x.SITENAME);
    $('#DEPTNAME').text(x.DEPTNAME);
    $('#footer').text(x.SITENAME);
    $('#EXTDATA').text(x.EXTDATA);
    $('#CURRENTWORKERCOUNT').text(x.CURRENTWORKERCOUNT);
    $('#NEWWORKERCOUNT').text(x.NEWWORKERCOUNT);
    $('#RETIREWORKERCOUNT').text(x.RETIREWORKERCOUNT);
    $('#CHANGEWORKERCOUNT').text(x.CHANGEWORKERCOUNT);
    $('#ACCIDENTWORKERCOUNT').text(x.ACCIDENTWORKERCOUNT);
    $('#DEADWORKERCOUNT').text(x.DEADWORKERCOUNT);
    $('#INJURYWORKERCOUNT').text(x.INJURYWORKERCOUNT);
    $('#BIZDISEASEWORKERCOUNT').text(x.BIZDISEASEWORKERCOUNT);
    $('#GENERALHEALTHCHECKDATE').text(x.GENERALHEALTHCHECKDATE);
    $('#SPECIALHEALTHCHECKDATE').text(x.SPECIALHEALTHCHECKDATE);
    $('#GENERALD2COUNT').text(x.GENERALD2COUNT);
    $('#GENERALC2COUNT').text(x.GENERALC2COUNT);

    $('#GENERALTOTALCOUNT').text(x.GENERALTOTALCOUNT);
    $('#SPECIALTOTALCOUNT').text(x.SPECIALTOTALCOUNT);




    $('#SPECIALD1COUNT').text("D1:" + x.SPECIALD1COUNT);
    $('#SPECIALC1COUNT').text("C1:" + x.SPECIALC1COUNT);
    $('#SPECIALD2COUNT').text("D2:" + x.SPECIALD2COUNT);
    $('#SPECIALC2COUNT').text("C2:" + x.SPECIALC2COUNT);
    $('#SPECIALDNCOUNT').text("DN:" + x.SPECIALDNCOUNT);
    $('#SPECIALCNCOUNT').text("CN:" + x.SPECIALCNCOUNT);


    $('#WEMDATE').text(x.WEMDATE);
  
    $('#WEMEXPORSUREREMARK').text(x.WEMEXPORSUREREMARK);
    $('#WEMHARMFULFACTORS').text(x.WEMHARMFULFACTORS);

    
    
    if(x.WEMEXPORSURE=="Y"){
        $('#WEMEXPORSURE').prop("checked", true);

    }else{
        $('#WEMEXPORSURE').prop("checked", false);
    }
    if (x.WEMEXPORSURE2 == "Y") {
        $('#WEMEXPORSURE2').prop("checked", true);

    } else {
        $('#WEMEXPORSURE2').prop("checked", false);
    }
    
    //$('#WORKCONTENT').text("주요내용:"+ j.WORKCONTENT);

    //$('#OSHADATE').text("실시,예정일:"+j.OSHADATE+   "주요내용:"+j.OSHACONTENT); // 산업안전보건위원회
    $("input[type=radio]").each(function () {

        if ($(this).is(":checked") == true) {
            $(this).replaceWith("<span style='font-weight:bold'>√<span>");
        }
    });
    $("input[type=checkbox]").each(function () {
        if ($(this).is(":checked") == true) {
            $(this).replaceWith("<span style='font-weight:bold'>√<span>");
        }
    });
}

function SetOpinion(html){
    $('#opinionSection').append(html);
}
function getCheck(val){
    if(val == "Y"){
        return '√';
    }else {
        return '-';
    }
}


function setSiteSign(base64Image) {
    console.log(base64Image);
  //  base64Image="data:image/png;base64,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";
    $('#siteManagerSign').attr('src', base64Image);
    $('#siteManagerSign').show();

    
}
function setUserSign(base64Image) {
    console.log(base64Image);
  //  base64Image="data:image/png;base64,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";
    $('#userSign').attr('src', base64Image);
    $('#userSign').show();
    
}