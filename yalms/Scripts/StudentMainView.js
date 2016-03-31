//
$(document).ready(function () {
    $("#AssignmentsTree").jstree({
        "plugins": ["themes", "html_data"]
    })
    .on('select_node.jstree', function (e, node) {
        if (node.node.children.length == 0) {
            $("#assignmentLabel").html(node.node.text);
            $("#assignmentId").val(node.node.id);
            $('#assignmentUploadBtn').prop('disabled', false);
        }
    })
});

