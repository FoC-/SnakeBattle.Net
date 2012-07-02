var numberOfModules = 0;

function AddModule(snakeId) {
    var position = $("#Modules div.span4").index($("#AddNew"));
    var newModuleId = guidGenerator();
    if (SetModuleSuccess(snakeId, newModuleId, position)) {
    var html = '<div class="span4" id="' + newModuleId + '">' +
            '<div class="span2"></div>' +
            '<div class="span1">' +
            '<div class="btn btn-mini" onclick="DeleteModule(' + snakeId + ', ' + newModuleId + ');"><span>Delete</span></div>' +
            '<div class="btn btn-mini" onclick="InsertModule(' + snakeId + ', ' + newModuleId + ');"><span>Insert</span></div>' +
            '</div>' +
            '<script type="text/javascript">DrawModule(' + snakeId + ', ' + newModuleId + ');</script>' +
        '</div>';
    //add html before AddNew
    };
}

function InsertModule(snakeId, moduleId) {
    var position = $("#Modules div.span4").index($("#" + moduleId));
    var newModuleId = guidGenerator();
    if (SetModuleSuccess(snakeId, newModuleId, position)) {
        //add html before moduleid
    };
}

function SetModuleSuccess(snakeId, moduleId, position) {
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
        return true;
    }
    return false;
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
        $("#" + moduleId).remove();
    }
}


function DrawModule(snakeId, moduleId) {
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
        numberOfModules++;
        var o = '<canvas height="70" width="70" class="table-bordered" onclick="CanvaClick(this)"></canvas>';
        $("#" + moduleId + " .span2").prepend(o);
        //    $("#" + moduleId + " .span2").html(o);

    }
}

function CanvaClick(element) {

}



function guidGenerator() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}