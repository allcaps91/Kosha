
$(document).ready(function () {
    console.log("레디");

    (async function()
    {
        await CefSharp.BindObjectAsync("cefsharpBoundAsync");
        cefsharpBoundAsync.ready();
    })()
                
    var minWidth = 7;
	var maxWidth = 10;

	var canvas = document.getElementById("canvas-signature");
	signaturePad = new SignaturePad(canvas	, {
		dotSize:0.0,
		minDistance:0,
	    minWidth: minWidth,
	    maxWidth: maxWidth,
	    penColor: "rgba(0,0,0, 1)",
	    backgroundColor: 'rgb(255, 255, 255,0)',
	});	
	//서명은 크기 고정
	$('#canvas-signature').attr("width", "630px");
	$('#canvas-signature').attr("height", "200px");
	
	$('#sign').click(function(){
        var signature = signaturePad.toDataURL();
        console.log(signature);
        if(signature.length<8000){
            alert("서명을 식별할 수 없습니다. 보다 정확하게 서명해주세요");
            return ;
        }

        (async function()
        {
            await CefSharp.BindObjectAsync("cefsharpBoundAsync");
            cefsharpBoundAsync.saveSiteUser( signature );
        })()
    });
    
	$('#signSave').click(function(){
        var signature = signaturePad.toDataURL();
        console.log(signature);
        if(signature.length<8000){
            alert("서명을 식별할 수 없습니다. 보다 정확하게 서명해주세요");
            return ;
        }

        (async function()
        {
            await CefSharp.BindObjectAsync("cefsharpBoundAsync");
            cefsharpBoundAsync.save( signature );
        })()
    });
/*	$('#signCancel').click(function(){
		
		console.log("취소");
		
    });
    */
	$('#signDel').click(function(){
		
        signaturePad.clear();
		
	});
});




function showSaveButton(){
    $('#signSave').show();
    $('#sign').hide();
}