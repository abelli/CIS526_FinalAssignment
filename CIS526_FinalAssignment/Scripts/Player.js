var playerId = $('#playerId').val();
pathArray = window.location.pathname.split( '/' );
host = pathArray[0];
var tasks = [];

$.get(host + "/Player/GetTasks/" + playerId, function (data) {
    var count = 1;

    for (i in data) {
        tasks.push(data[i]);
        count++;
        $('#playerTask').append('<tr><td class="playerTask">' +
			'<a href="'+ host + '/Task/Details/'+ data[i].taskID + '">' + data[i].taskName + '</a>'+
			'</td><td class="playerTask" id="pointsEarned">' +
			data[i].pointsEarned +
			'</td><td class="playerTask">' +
			data[i].completionTime +
			'</td>');
    }

});