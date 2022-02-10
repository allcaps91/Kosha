

(function() {
	var cmd = {
		canUndo: false, // The undo snapshot will be handled by 'insertElement'.
		exec: function( editor ) {
			var date = new Date();		
			var id = 'signature_'+date.getTime();
			var element = CKEDITOR.dom.element.createFromHtml( '<eas class="sign-button"data-cke-pa-onclick="easSignOpen(\''+id+'\')">'
					//+'<img  class="img-fluid eas-sign-signature signature_'+date.getTime()+'" src="" />'
					
					+'<input type="text"  id="'+id+'" class="eas eas-sign eas-sign-patient eas-sign-inputtext single-sign patient-name" readonly>'
					+'<span class="single-sign-text eas-sign-inputtext">(서명)</span>'
					//+'<button class="btn  btn-warning btn-sm btn-rounded waves-effect btn-sign" type="button" onclick="easSignOpen(\''+id+'\')">서명</button>'
					+'</eas>' );
		   // editor.insertElement(element);
			var startRange = editor.getSelection();
			var parent = startRange.getStartElement();
			
			if(parent.$.tagName == "P" || parent.$.tagName == "DIV"  || parent.$.tagName == "TD"){
				editor.insertElement( element );	
			}else{
				console.log(" 라벨 안으로 들어가지 않도록")
				element.insertAfter(parent);
			}
		    
		},
	};

	var pluginName = 'opinion';
	CKEDITOR.plugins.add( pluginName, {
		// jscs:disable maximumLineLength
		//lang: 'en,ko', // %REMOVE_LINE_CORE%
		// jscs:enable maximumLineLength
		icons: 'opinion', // %REMOVE_LINE_CORE%
		hidpi: true, // %REMOVE_LINE_CORE%
		init: function( editor ) {
			if ( editor.blockless )
				return;
			editor.addContentsCss('' );
			editor.ui.addToolbarGroup("psmh", 1);
			editor.addCommand( pluginName, cmd );
			editor.ui.addButton && editor.ui.addButton( 'opinion', {
				label: '현실태 및 문제점 - 개선의견',
				command: 'opinion',
				toolbar: 'psmh,0'
			} );
		}
	} );
} )();
