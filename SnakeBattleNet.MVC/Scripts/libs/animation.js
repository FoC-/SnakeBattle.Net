﻿//Global variables
var imageObjects = new Array();
var scale = 10;
var canvas;
var context;
var battleReplay;

function LoadBattleReplay(replayLink, texturesLink) {
    $.ajax({
        url: replayLink,
        type: 'POST',
        dataType: 'json',
        data: '{id: "ID-javascript" }',
        contentType: 'application/json',
        success: function (data) { battleReplay = data; },
        complete: function () { InitResources(texturesLink); },
        async: false
    });
}

function InitResources(texturesLink) {
    canvas = document.getElementById("canva");
    canvas.width = battleReplay.fieldSize.X * scale;
    canvas.height = battleReplay.fieldSize.Y * scale;
    context = canvas.getContext("2d");

    $.ajax({
        url: texturesLink,
        type: 'POST',
        dataType: 'json',
        data: '{id: "ID-javascript" }',
        contentType: 'application/json',
        success: function (data) { preloadImages(data); },
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
    for (var y = 0; y < battleReplay.fieldSize.Y; y++) {
        for (var x = 0; x < battleReplay.fieldSize.X; x++) {
            var textureNum = determElement(battleReplay.field[y * battleReplay.fieldSize.X + x]);
            drawElement(0, textureNum, x, y);
        }
    }
}

function drawElement(textures, textureNumber, x, y) {
    context.drawImage(imageObjects[textures], textureNumber, 0, 10, 10, x * scale, y * scale, scale, scale);
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