function startReplay(battleReplay, imageObjects) {
    var canvas = document.getElementById("canva");

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
    // draw
    drawBattlefield(canvas, battlefield.fieldSize, battlefield.field, imageObject);

    // request new frame
    requestAnimFrame(function () { animate(canvas, battlefield, imageObject); });
}

function drawBattlefield(canvas, size, field, imageObject) {
    canvas.width = size.X * 10;
    canvas.height = size.Y * 10;

    var context = canvas.getContext("2d");
    var elem = 10;

    for (var y = 0; y < size.Y; y++) {
        for (var x = 0; x < size.X; x++) {
            var currentElement = determElement(field[y * size.X + x]);
            context.drawImage(imageObject[0], currentElement, 0, elem, elem, x * elem, y * elem, elem, elem);
        }
    }
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