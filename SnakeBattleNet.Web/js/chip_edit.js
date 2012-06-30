var chip;
var numberOfModules = 0;

function AddModule(snakeId) {
    var position = $("#Modules div.span4").index($("#AddNew"));
    SetModule(snakeId, guidGenerator(), position);
}

function InsertModule(snakeId, moduleId) {
    var position = $("#Modules div.span4").index($("#" + moduleId));
    SetModule(snakeId, guidGenerator(), position);
}

function SetModule(snakeId, moduleId, position) {
    var response;

    $.ajax({
        url: 'Edit/InsertModule',
        type: 'POST',
        dataType: 'json',
        data: '{snakeId:"' + snakeId +
           '", moduleId:"' + moduleId +
           '", position:' + position + '}',
        contentType: 'application/json',
        success: function (data) { response = data; },
        async: false
    });

    if (response.Status === "OK") {

        numberOfModules++;
    }
}

function DeleteModule(snakeId, moduleId) {
    var response;
    $.ajax({
        url: 'Edit/DeleteModule',
        type: 'POST',
        dataType: 'json',
        data: '{snakeId: "' + snakeId + '", moduleId:"' + moduleId + '"}',
        contentType: 'application/json',
        success: function (data) { response = data; },
        async: false
    });

    if (response.Status === "OK") {
        numberOfModules--;
    }
}


function DrawModule(snakeId, moduleId) {
    var o = '<canvas height="70" width="70" class="table-bordered" onclick="CanvaClick(this)"></canvas>';
    $("#" + moduleId + " .span2").prepend(o);
//    $("#" + moduleId + " .span2").html(o);

    var response;
    $.ajax({
        url: 'Edit/GetModule',
        type: 'POST',
        dataType: 'json',
        data: '{snakeId: "' + snakeId + '", moduleId:"' + moduleId + '"}',
        contentType: 'application/json',
        success: function (data) { response = data; },
        async: false
    });

    if (response.Status === "OK") {
        numberOfModules--;
    }



    numberOfModules++;
}

function CanvaClick(element) {

}



function guidGenerator() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}