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
		$('#topTen').append('<tr><td>' + 
			data[i].rank +
			'</td><td>' +
			data[i].userName +
			'</td><td>' +
			data[i].score +
			'</td></tr>');
	}
});

$.get(host + "/Leaderboard/GetScores/" + boardId + "?rank=" + rank, function(data) {
	for (i in data) {
		scores.push(data[i]);
		$('#scoreboard').append('<tr><td>' + 
			data[i].rank +
			'</td><td>' +
			data[i].userName +
			'</td><td>' +
			data[i].score +
			'</td></tr>');
		rank++;
	}
});