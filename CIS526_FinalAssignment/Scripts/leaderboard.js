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
	for (i in data) {
		topTen.push(data[i]);
		$('#topTen').append('<tr><td class="topTen">' + 
			data[i].rank +
			'</td><td class="topTen">' +
			data[i].userName +
			'</td><td class="topTen">' +
			data[i].score +
			'</td><td class="topTen"><img src="' + host + '/Content/images/icons/Gate.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/key.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/quarter.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/Gate.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/key.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/quarter.png" width="25px" height="25px"></td></tr>');
	}
});

function getScores() {
	$.get(host + "/Leaderboard/GetScores/" + boardId + "?rank=" + rank, function(data) {
		for (i in data) {
			scores.push(data[i]);
			$('#scoreboard').append('<tr><td class="score">' + 
				data[i].rank +
				'</td><td class="score">' +
				data[i].userName +
				'</td><td class="score">' +
				data[i].score +
				'</td><td class="topTen"><img src="' + host + '/Content/images/icons/Gate.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/key.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/quarter.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/Gate.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/key.png" width="25px" height="25px"><img src="' + host + '/Content/images/icons/quarter.png" width="25px" height="25px"></td></tr>');
			rank++;
		}
	});
}

getScores();

$('#moreScores').click(function() {
	getScores();
});