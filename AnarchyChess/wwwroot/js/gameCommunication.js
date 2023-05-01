var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
var accteptableClasses = isWhite ? ".white" : ".black";

connection.on("PieceMoved", function (fromPosition, toPosition) {
    var fromSquare = $(`.chess-square[data-position='${fromPosition}']`);
    var toSquare = $(`.chess-square[data-position='${toPosition}']`);
    toSquare.html(fromSquare.html());
    fromSquare.html('');
});

connection.on("GameState", function (fromPosition, toPosition) {
    var fromSquare = $(`.chess-square[data-position='${fromPosition}']`);
    var toSquare = $(`.chess-square[data-position='${toPosition}']`);
    toSquare.html(fromSquare.html());
    fromSquare.html('');
});

$(".chess-board").on("mousedown", accteptableClasses, function () {
    $(this).draggable({
        revert: "invalid",
        start: function (event, ui) {
            ui.helper.data("fromPosition", $(this).parent().data("position"));
        }
    });
});

$(".chess-square").droppable({
    accept: accteptableClasses,
    drop: function (event, ui) {
        var fromPosition = ui.helper.data("fromPosition");
        var toPosition = $(this).data("position");

        connection.invoke("MovePiece", fromPosition, toPosition, gameId).catch(function (err) {
            return console.error(err.toString());
        });

        $(this).html(`<div class=\"piece ${accteptableClasses.replace(".", "")}\">${ui.helper.html()}</div>`);
        ui.helper.remove();
    }
});


function addChessPiece(square, piece, color) {
    $(`.chess-square[data-position='${square}']`).html(`<div class="piece ${color}">${piece}</div>`);
}

function selectFromPieces(pieceName) {
    var pieces = {
        "white": {
            "pawn": "&#9817;",
            "rook": "&#9814;",
            "knight": "&#9816;",
            "bishop": "&#9815;",
            "queen": "&#9813;",
            "king": "&#9812;"
        },
        "black": {
            "pawn": "&#9823;",
            "rook": "&#9820;",
            "knight": "&#9822;",
            "bishop": "&#9821;",
            "queen": "&#9819;",
            "king": "&#9818;"
        }
    };

    var color = pieceName.split('_')[0].toLowerCase();
    var name = pieceName.split('_')[1].toLowerCase();

    return pieces[color][name];
}

function addChessPieces() {

    

    gameState.forEach(piece => {
        var color = piece.pieceType.split('_')[0].toLowerCase();
        addChessPiece(piece.position, selectFromPieces(piece.pieceType), color);
        console.log(piece);
    });

    /*
    // Pawns
    for (var i = 0; i < 8; i++) {
        addChessPiece(`${String.fromCharCode(97 + i)}2`, pieces.white.pawn);
        addChessPiece(`${String.fromCharCode(97 + i)}7`, pieces.black.pawn);
    }

    // Rooks
    addChessPiece('a1', pieces.white.rook);
    addChessPiece('h1', pieces.white.rook);
    addChessPiece('a8', pieces.black.rook);
    addChessPiece('h8', pieces.black.rook);

    // Knights
    addChessPiece('b1', pieces.white.knight);
    addChessPiece('g1', pieces.white.knight);
    addChessPiece('b8', pieces.black.knight);
    addChessPiece('g8', pieces.black.knight);

    // Bishops
    addChessPiece('c1', pieces.white.bishop);
    addChessPiece('f1', pieces.white.bishop);
    addChessPiece('c8', pieces.black.bishop);
    addChessPiece('f8', pieces.black.bishop);

    // Queens
    addChessPiece('d1', pieces.white.queen);
    addChessPiece('d8', pieces.black.queen);

    // Kings
    addChessPiece('e1', pieces.white.king);
    addChessPiece('e8', pieces.black.king);
    */
}

function joinGroup() {
    connection.invoke("JoinGroup", gameId).catch(function (err) {
        return console.error(err.toString());
    });
}


$(document).ready(function () {
    connection.start().then(function () {
        console.log("connected");
        connection.invoke("JoinGroup", gameId).catch(function (err) {
            return console.error(err.toString());
        });
    })
        .catch(function (err) {
            return console.error(err.toString());
        });
    addChessPieces();
});