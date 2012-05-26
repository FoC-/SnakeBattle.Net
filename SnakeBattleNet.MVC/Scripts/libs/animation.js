function init(battlefield) {
    var canvas = document.getElementById("canva");

    var imageObject = new Image();
    imageObject.onload = function () {
        animate(canvas, battlefield, imageObject);
    };
    imageObject.src = 'Content/snake.bmp';
};

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
    var tail = battlefield.segments[0];
    
    var elem = 10;
    context.drawImage(imageObject, 0, 0, elem, elem, tail.x, tail.y, elem, elem);
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