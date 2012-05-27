function init(battlefield, images) {
    var canvas = document.getElementById("canva");
    canvas.width = battlefield.field.x * 10;
    canvas.height = battlefield.field.y * 10;
    

    var imageObjects = new Array();
    preloadImages(imageObjects, images);

    imageObjects[imageObjects.length - 1].onload = function () {
        animate(canvas, battlefield, imageObjects);
    };
};

function preloadImages(o, images) {
    //Preload field textures
    o[0] = new Image();
    o[0].src = images.field;
    
    //Preload snakes textures
    for (var i = 0; i < images.snakes.length; i++) {
        o[i+1] = new Image();
        o[i+1].src = images.snakes[i];
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
    battlefield.segments[0].x += 10;
}

function drawBattlefield(context, battlefield, imageObject) {
    var elem = 10;

    for (var x = 0; x < battlefield.field.x; x++) {
        for (var y = 0; y < battlefield.field.y; y++) {
            context.drawImage(imageObject[0], 10, 0, elem, elem, x*10, y*10, elem, elem);
        }
    }

    var tail = battlefield.segments[0];
    
    context.drawImage(imageObject[2], 0, 0, elem, elem, tail.x, tail.y, elem, elem);
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