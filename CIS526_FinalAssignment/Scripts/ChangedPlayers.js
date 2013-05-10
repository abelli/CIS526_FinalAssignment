var playerId = $('#playerId').val();
pathArray = window.location.pathname.split('/');
host = pathArray[0];
var tasks = [];

$.get(host + "/Player/GetChangedPlayers/", function (data) {
    var count = 1;

    for (i in data) {
        tasks.push(data[i]);
        count++;
        $('#playerChanged').append('<tr><td class="playerChanged">' +
			'<a href="' + host + '/Player/Details/' + data[i].ID + '">' + data[i].username + '</a>' +
			'</td><td class="playerChanged" id="pointsEarned">' +
			data[i].totalScore +
			'</td><td class="playerChanged">' +
			data[i].isFrozen +
			'</td>');
    }

});