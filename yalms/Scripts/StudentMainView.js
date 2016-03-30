//

$(function () {
    $("#AssignmentsTree").jstree({
        "plugins": ["themes", "html_data"]
    });
})
.bind("select_node.jstree", function (e, data) {
    var CurrObj = data.rslt.obj;
    //Toggle on the click of that Node's name
    $("#demo1").jstree("toggle_node", data.rslt.obj);
})
.bind("hover_node.jstree", function (e, data) {
    //on hover
    var nodeId = data.rslt.obj[0].id;
})
.bind("loaded.jstree", function (event, data) {
    $(this).jstree("open_all");
    alert("tree loaded");
});
