//
$(document).ready(function () {
    $("#AssignmentsTree").jstree({
        "plugins": ["themes", "html_data"]
    })
    .on("select_node.jstree", function (e, data) {
        var href = data.node.a_attr.href
        document.location.href = href;
    })
});

