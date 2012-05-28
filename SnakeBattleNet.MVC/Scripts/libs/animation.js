function startReplay(battleReplay, imageObjects) {
    var canvas = document.getElementById("canva");
    canvas.width = battleReplay.field.X * 10;
    canvas.height = battleReplay.field.Y * 10;

    animate(canvas, battleReplay, imageObjects);
};

function PreloadResources(battleLink, texturesLink) {
    var imageObjects = new Array();
    loadTextures(texturesLink, imageObjects);
    loadBattle(battleLink, imageObjects);
}

function loadTextures(texturesLink, imageObjects) {
    $.ajax({
        url: texturesLink,
        type: 'POST',
        dataType: 'json',
        data: '{id: "ID-javascript" }',
        contentType: 'application/json',
        success: function (data) { preloadImages(imageObjects, data); },
        async: false
    });
}

function loadBattle(battleLink, imageObjects) {
    var battle;
    $.ajax({
        url: battleLink,
        type: 'POST',
        dataType: 'json',
        data: '{id: "ID-javascript" }',
        contentType: 'application/json',
        success: function (data) { battle = data; },
        complete: function () { startReplay(battle, imageObjects); },
        async: false
    });
}

function preloadImages(o, images) {
    //Preload field textures
    o[0] = new Image();
    o[0].src = images.field;

    //Preload snakes textures
    for (var i = 0; i < images.snakes.length; i++) {
        o[i + 1] = new Image();
        o[i + 1].src = images.snakes[i];
    }
}

function animate(canvas, battlefield, imageObject) {
    var context = canvas.getContext("2d");

    // clear
    context.clearRect(0, 0, canvas.width, canvas.height);
    // move
    moveSnakes(battlefield);
    // draw
    drawBattlefield(context, battlefield, imageObject);

    // request new frame
    requestAnimFrame(function () { animate(canvas, battlefield, imageObject); });
}

function moveSnakes(battlefield) {
    battlefield.segments.X += 1;
}

function drawBattlefield(context, battlefield, imageObject) {
    var elem = 10;

    for (var x = 0; x < battlefield.field.X; x++) {
        for (var y = 0; y < battlefield.field.Y; y++) {
            context.drawImage(imageObject[0], 10, 0, elem, elem, x * 10, y * 10, elem, elem);
        }
    }

    var tail = battlefield.segments;

    context.drawImage(imageObject[2], 0, 0, elem, elem, tail.X, tail.Y, elem, elem);
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