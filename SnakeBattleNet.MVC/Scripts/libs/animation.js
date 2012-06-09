//Global variables
var imageObjects = new Array();
var canvas;
var context;
var battleReplay;

function PreloadResources(replayLink, texturesLink) {
    canvas = document.getElementById("canva");
    context = canvas.getContext("2d");

    loadTextures(texturesLink);
    loadBattleReplay(replayLink);
}

function loadTextures(texturesLink) {
    $.ajax({
        url: texturesLink,
        type: 'POST',
        dataType: 'json',
        data: '{id: "ID-javascript" }',
        contentType: 'application/json',
        success: function (data) { preloadImages(data); },
        async: false
    });
}

function loadBattleReplay(replayLink) {
    $.ajax({
        url: replayLink,
        type: 'POST',
        dataType: 'json',
        data: '{id: "ID-javascript" }',
        contentType: 'application/json',
        success: function (data) { battleReplay = data; },
        complete: function () { animate(); },
        async: false
    });
}

function preloadImages(images) {
    //Preload field textures
    imageObjects[0] = new Image();
    imageObjects[0].src = images.field;

    //Preload snakes textures
    for (var i = 0; i < images.snakes.length; i++) {
        imageObjects[i + 1] = new Image();
        imageObjects[i + 1].src = images.snakes[i];
    }
}

function animate() {
    // clear
    context.clearRect(0, 0, canvas.width, canvas.height);
    // move
    // draw
    drawBattlefield();

    // request new frame
    requestAnimFrame(function () { animate(); });
}

function drawBattlefield() {
    canvas.width = battleReplay.fieldSize.X * 10;
    canvas.height = battleReplay.fieldSize.Y * 10;

    for (var y = 0; y < battleReplay.fieldSize.Y; y++) {
        for (var x = 0; x < battleReplay.fieldSize.X; x++) {
            var textureNum = determElement(battleReplay.field[y * battleReplay.fieldSize.X + x]);
            drawElement(0, textureNum, x, y);
        }
    }
}

function drawElement(textures, textureNumber, x, y) {
    var elem = 10;
    context.drawImage(imageObjects[textures], textureNumber, 0, elem, elem, x * elem, y * elem, elem, elem);
}

function determElement(name) {
    var e = 0;
    switch (name) {
        case "W":
            e = 0;
            break;
        case "E":
            e = 10;
            break;
        case "G":
            e = 20;
            break;
        case "H":
            e = 0;
            break;
        case "B":
            e = 10;
            break;
        case "T":
            e = 20;
            break;
    }
    return e;
}

window.requestAnimFrame = (function (callback) {
    return window.requestAnimationFrame ||
                window.webkitRequestAnimationFrame ||
                window.mozRequestAnimationFrame ||
                window.oRequestAnimationFrame ||
                window.msRequestAnimationFrame ||
                function (callback) {
                    window.setTimeout(callback, 1000 / 60);
                };
})();