var boardId = $('#boardId').val();
var rank = 11;
pathArray = window.location.pathname.split( '/' );
host = pathArray[0];

var topTen = [];
var scores = [];

function LeaderBoardScore(scoreID, playerID, leaderboardID, score, rank, userName, leaderboard)
{
    var scoreID = scoreID;
    var playerID = playerID;
    var leaderboardID = leaderboardID;
    var score = score;
    var rank = rank;
    var userName = userName;
	var leaderboard = leaderboard;
}

$.get(host + "/Leaderboard/GetTopTen/" + boardId, function(data) {
	var count = 1;

	for (i in data) {
		topTen.push(data[i]);       
		count++;
		$('#topTen').append('<tr><td class="topTen">' + 
			data[i].rank +
			'</td><td class="topTen" id="userName">' +
			data[i].userName +
			'</td><td class="topTen">' +
			data[i].score +
			'</td><td class="topTen"><img src="' + host + '/Content/images/icons/Gate.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/key.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/quarter.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/Gate.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/key.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/quarter.png" width="25px" height="25px"></td></tr>');
	}

	if (count < 11) {
		$('#moreScores').slideUp();
		rank = count;
	}
});

function getScores() {
	$.get(host + "/Leaderboard/GetScores/" + boardId + "?rank=" + rank, function(data) {
		var temp = rank;
		if (rank >= 11) {
			for (i in data) {
				scores.push(data[i]);
				$('#scoreboard').append('<tr><td class="score">' + 
					data[i].rank +
					'</td><td class="score" id="userName">' +
					data[i].userName +
					'</td><td class="score">' +
					data[i].score +
					'</td><td class="topTen">'
					+ '<img src="' + host + '/Content/images/icons/Gate.png" width="25px" height="25px">'
					+ '<img src="' + host + '/Content/images/icons/key.png" width="25px" height="25px">'
					+ '<img src="' + host + '/Content/images/icons/quarter.png" width="25px" height="25px">'
					+ '<img src="' + host + '/Content/images/icons/Gate.png" width="25px" height="25px">'
					+ '<img src="' + host + '/Content/images/icons/key.png" width="25px" height="25px">'
					+ '<img src="' + host + '/Content/images/icons/quarter.png" width="25px" height="25px"></td></tr>');
				rank++;
			}

			if ((rank - temp) < 25) $('#moreScores').slideUp();
		}
	});
}

getScores();

$('#search').click(function() {
	if ($('#searchTerm').val() != '') {
		$('#userName.topTen').each(function() {
			if ($('#searchTerm').val() === $(this).text()) {
				$(document).scrollTop($(this).position().top-100);
				$(this).parent().css({ opacity: 1.0 });
			}
			else $(this).parent().css({ opacity: 0.3 });
		});

		$('#userName.score').each(function() {
			if ($('#searchTerm').val() === $(this).text()) {
				$(document).scrollTop($(this).position().top-100);
				$(this).parent().css({ opacity: 1.0 });
			}
			else $(this).parent().css({ opacity: 0.3 });
		});
	}
});

$('#clearSearch').click(function() {

	$('#userName.topTen').each(function() {
			$(this).parent().css({ opacity: 1.0 });
	});

	$('#userName.score').each(function() {
		$(this).parent().css({ opacity: 1.0 });
	});

	$('#searchTerm').val("");
});

$('#moreScores').click(function() {
	getScores();
});