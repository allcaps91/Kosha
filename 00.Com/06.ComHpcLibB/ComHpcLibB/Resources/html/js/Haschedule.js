var calendar ;
 document.addEventListener('DOMContentLoaded', function() {
    var calendarEl = document.getElementById('calendar');

     calendar = new FullCalendar.Calendar(calendarEl, {
      plugins: [ 'interaction', 'dayGrid' ,'googleCalendar', 'timeGrid'],
	  customButtons: {
		nextButton: {
		  icon:'fc-icon-chevron-right',
		  click: function() {
			 calendar.next();//moment(info.event.start).format('YYYY-MM-DD')
			 console.log(calendar.view.type);
			(async function()
				{
					await CefSharp.BindObjectAsync("cefsharpBoundAsync");
					var nextDate = moment(calendar.getDate()).format('YYYY-MM-DD');
					cefsharpBoundAsync.next(nextDate, calendar.view.type);	
			})();
		  }
		},
		prevButton: {
		  icon:'fc-icon-chevron-left',
		  click: function() {
			calendar.prev();
		  }
		},
		nextYearButton: {
		  icon:'fc-icon-chevrons-right',
		  click: function() {
			console.log("next");
			calendar.next();
		  }
		},
		prevYearButton: {
		  icon:'fc-icon-chevrons-left',
		  click: function() {
			calendar.prev();
		  }
		}
	  },
      header: {
        left: 'prevYearButton,prevButton,nextButton,nextYearButton today ',
        center: 'title',
        right: 'dayGridMonth,timeGridWeek,timeGridDay'
      },
       locale: 'ko',
      defaultDate: new Date(),
      navLinks: true, // can click day/week names to navigate views
      editable: true,
      eventLimit: true, // allow "more" link when too many events
	  selectable: true,
      selectMirror: true,
	  displayEventTime:false,
	  googleCalendarApiKey: 'AIzaSyCxx9rlTfIPF0F2xvqvPTp3GjfRSnqaX3Q',
	   eventSources: [
        {
		  id:"google",
          googleCalendarId: 'ko.south_korea#holiday@group.v.calendar.google.com',
		  color: 'red',
		  editable: false
        },
		
      ],
	   /*events: [
        {
          title: 'Business Lunch',
          start: '2019-08-03T13:00:00',
		  end: '2019-08-03T17:00:00',
          constraint: 'businessHours'
        }
		]*/
      
    });

	//calendar.on('dateClick', function(info) {
	//  console.log('clicked on ', info);
	//});
 
	calendar.on('select', function(info) {
		console.log('select ',info);
		console.log('select ',info.view.type);
		console.log('select ',info.startStr);
		(async function()
		{
			await CefSharp.BindObjectAsync("cefsharpBoundAsync");
			cefsharpBoundAsync.newEvent(info.startStr, info.view.type);	// 메소드는 소문자로시작해야함
		})();

	});

	calendar.on('eventClick', function(info) {
		info.jsEvent.preventDefault();
		console.log('eventClick ',info.event);
		console.log('eventClick ',info.event.title);
		console.log('eventClick ',info.event.id);
		console.log('startDate ',moment(info.event.start).format('YYYY-MM-DD'));
		var startStr = moment(info.event.start).format('YYYY-MM-DD');
		(async function()
		{
			await CefSharp.BindObjectAsync("cefsharpBoundAsync");
			cefsharpBoundAsync.updateEvent(info.event.id, startStr, info.view.type);	// 메소드는 소문자로시작해야함
		})();
	});

	calendar.on('eventDrop', function(info) {
		console.log('eventDrop ',info.event);
		console.log('eventDrop ',info.event.dateStr);
		//https://fullcalendar.io/docs/eventDrop
		console.log('eventDrop ',info.event.start);
		console.log('eventDrop ',info.event.id);	  
	    console.log('eventDrop ',moment(info.event.start).format('YYYY-MM-DD') );

		var startStr = moment(info.event.start).format('YYYY-MM-DD HH:mm');
		(async function()
		{
			await CefSharp.BindObjectAsync("cefsharpBoundAsync");
			cefsharpBoundAsync.dragDropEvent(info.event.id, startStr, info.view.type);	// 메소드는 소문자로시작해야함
		})();
	}); 
	
	calendar.render();
	(async function()
		{
			await CefSharp.BindObjectAsync("cefsharpBoundAsync");
			cefsharpBoundAsync.ready(moment(calendar.getDate()).format('YYYY-MM-DD'));	// 메소드는 소문자로시작해야함
	})();
	
});
  
function test(){
var source = new Array();
 var event = new Object();       
            event.title ="vfgf";
            event.start ="2019-08-02";
			source.push(event);

var dog = [{title: "hhhhhh", start: "2019-08-02"}]; // 자바스크립트 객체

 

var data = JSON.stringify(dog);    
console.log(JSON.parse(data));                // 자바스크립트 객체를 문자열로 변환함.
calendar.addEventSource( JSON.parse(data) );
  console.log(calendar.getDate());
}

function gettt(){
 var events = calendar.getEventSources();
 for(var i=0; i<events.length; i++){
	events[i].remove();
	}
}
  

function CallC(){

/*
	var source =  [
        {
          title: '하하호호hhhhhhh Day Event',
          start: '2019-08-01',
        },
    ];

    calendar.addEventSource( source );
	*/

}

//json 한건추가(중복제거) 테스트 안해봄..
function AddEvent(json){
	var event = JSON.parse(json);
	var e = calendar.getEventById(event.id);
	if(e!=null){
		e.remove();
	}
	calendar.addevent(event);
}

//json문자열로 리소스 추가(중복제거)
function AddEventRemoveAndJson(json){
	 var events = JSON.parse(json);
	  console.log( events);
	 for(var i=0; i<events.length; i++){
		var event = calendar.getEventById( events[i].id );
		if(event != null){
			event.remove();
		}	
	 }
	 calendar.addEventSource(events);
}
function AddEventJson(json){
	 var events = JSON.parse(json);
	 console.log( events);
	 var sources = calendar.getEventSources();
	 for(var i=0; i<sources.length; i++){
		 
		 if(sources[i].id!="google"){
			 sources[i].remove();
		 }
	 }
	 calendar.addEventSource(events);
}
function CallJs(a){
	console.log(a);
	var data = JSON.stringify(a);    
	console.log(JSON.parse(a));                // 자바스크립트 객체를 문자열로 변환함.
	
	calendar.addEventSource(JSON.parse(a));

}